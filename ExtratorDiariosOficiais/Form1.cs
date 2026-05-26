using CefSharp;
using CefSharp.Handler;
using CefSharp.OffScreen;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace ExtratorDiariosOficiais
{
    public partial class Form1 : Form
    {
        private const string InGovBaseUrl = "https://www.in.gov.br";

        private const string DiarioOficialUrl =
            $"{InGovBaseUrl}/leiturajornal?org=Minist%C3%A9rio%20da%20Fazenda&ato=Pauta";

        private readonly CookieContainer _cookieContainer = new();
        private readonly HttpClient _client;
        private ChromiumWebBrowser _browser = null!;
        private List<JsonObject.JsonArray> _entradas = new();
        private List<Extracao> _extracoes = new();
        private bool _extracaoSolicitada;
        private bool _extracaoEmAndamento;

        public Form1()
        {
            InitializeComponent();

            _client = CreateHttpClient(_cookieContainer);
            InitializeChromium();
        }

        private void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            settings.CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "CefSharp\\Cache");
            Cef.Initialize(settings);
            _browser = new ChromiumWebBrowser(DiarioOficialUrl);
            _browser.RequestHandler = new DiarioOficialRequestHandler(OnRequestIntercepted, OnResponseIntercepted);
            _browser.FrameLoadEnd += async (sender, args) => await OnBrowserFrameLoadEndAsync(args);
        }

        private static HttpClient CreateHttpClient(CookieContainer cookieContainer)
        {
            var client = new HttpClient(new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = cookieContainer,
                AutomaticDecompression = DecompressionMethods.All
            });

            client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/147.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8));
            client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7");
            client.DefaultRequestHeaders.Referrer = new Uri("https://www.in.gov.br/");

            return client;
        }

        private async Task OnBrowserFrameLoadEndAsync(FrameLoadEndEventArgs args)
        {
            if (!args.Frame.IsMain)
            {
                return;
            }

            if (!_extracaoSolicitada || _extracaoEmAndamento || _browser.Address != DiarioOficialUrl)
            {
                return;
            }

            _extracaoEmAndamento = true;

            try
            {
                _entradas.Clear();
                _extracoes.Clear();

                var parametros = await GetParametrosPaginaAsync();
                if (parametros is null)
                {
                    return;
                }

                foreach (var entrada in parametros.jsonArray)
                {
                    if (entrada.urlTitle.TrimStart('/').StartsWith("pauta-de-julgamento-"))
                    {
                        _entradas.Add(entrada);
                    }
                }

                PrepararProgressBar(_entradas.Count);

                await SyncChromiumCookiesToHttpClientAsync();

                for (var index = 0; index < _entradas.Count; index++)
                {
                    await GetDiarioOficialPageAsync(_entradas[index].urlTitle);
                    AtualizarProgressBar(index + 1, _entradas.Count);
                }

                GenerateExcelFile();
                MostrarMensagem("Extração finalizada");
            }
            catch (Exception ex)
            {
                MostrarMensagem($"Erro durante a extração: {ex.Message}");
            }
            finally
            {
                _extracaoSolicitada = false;
                _extracaoEmAndamento = false;
                AtualizarEstadoExtracao(false);
            }
        }

        private async void btn_Extrair_Click(object sender, EventArgs e)
        {
            _extracaoSolicitada = true;
            AtualizarEstadoExtracao(true);
            await _browser.LoadUrlAsync(DiarioOficialUrl);

            // foreach (var entrada in _entradas)
            // {
            //     var result = await GetDiarioOficialPageAsync(entrada.urlTitle);
            // }
        }

        private async Task SyncChromiumCookiesToHttpClientAsync()
        {
            var uri = new Uri(DiarioOficialUrl);
            var cookies = await Cef.GetGlobalCookieManager()
                .VisitUrlCookiesAsync(DiarioOficialUrl, includeHttpOnly: true);

            foreach (var cookie in cookies)
            {
                AddCookieToContainer(uri, cookie);
            }
        }

        private void AddCookieToContainer(Uri uri, CefSharp.Cookie chromiumCookie)
        {
            var httpCookie = new System.Net.Cookie(chromiumCookie.Name, chromiumCookie.Value)
            {
                Path = string.IsNullOrWhiteSpace(chromiumCookie.Path) ? "/" : chromiumCookie.Path,
                Secure = chromiumCookie.Secure,
                HttpOnly = chromiumCookie.HttpOnly
            };

            if (chromiumCookie.Expires.HasValue)
            {
                httpCookie.Expires = chromiumCookie.Expires.Value;
            }

            _cookieContainer.Add(uri, httpCookie);
        }

        private async Task<string> GetDiarioOficialPageAsync(string urlTitle)
        {
            var requestUrl = new Uri(new Uri(InGovBaseUrl), ("web/dou/-/" + urlTitle));
            using var response = await _client.GetAsync(requestUrl);
            var responseResult = await response.Content.ReadAsStringAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(responseResult);
            var classIdentifica = doc.DocumentNode.SelectSingleNode("//p[@class='identifica']");
            var elementosDepois = new List<HtmlNode>();

            var atual = classIdentifica.NextSibling;
            while (atual != null)
            {
                if (atual.NodeType == HtmlNodeType.Element)
                    elementosDepois.Add(atual);

                atual = atual.NextSibling;
            }

            var extracao = new Extracao();
            var relator = string.Empty;
            foreach (var item in elementosDepois)
            {
                var x = Regex.Match(item.InnerText,
                    @"DIA(\s+|t\+)(?<Dia>\d{1,})(\s+|\t+)de(?<Mes>.+)(\s+|\t+)de(\s+|\t+)(?<Ano>\d{4})",
                    RegexOptions.IgnoreCase);
                if (x.Success)
                {
                    if (TemDadosParaExportar(extracao))
                    {
                        _extracoes.Add(extracao);
                    }

                    extracao = new Extracao();
                    extracao.Dia = int.Parse(x.Groups["Dia"].Value.Trim());
                    extracao.Mes = x.Groups["Mes"].Value.Trim();
                    extracao.Ano = int.Parse(x.Groups["Ano"].Value.Trim());
                    continue;
                }

                x = Regex.Match(item.InnerText, @"Relator\(a\)\:(\s+|\t+)(?<Relator>.+)",
                    RegexOptions.IgnoreCase);
                if (x.Success)
                {
                    relator = x.Groups["Relator"].Value.Trim();
                    continue;
                }

                x = Regex.Match(item.InnerText,
                    @"(\s+|\t+)?\d+(\s+|\t+)?\-(\s+|\t+)?Processo(\s+|\t+)?n.+?\:(\s+|\t+)?(?<NProcesso>.+)(\s+|\t+)?\-(\s+|\t+)?Recorrente:(?<Recorrente>.+)(\s+|\t+)?e(\s+|\t+)?Interessado\:(?<Interessado>.+)",
                    RegexOptions.IgnoreCase);
                if (x.Success)
                {
                    extracao.Processos.Add(new Processo
                    {
                        Relator = relator,
                        NumeroProcesso = x.Groups["NProcesso"].Value.Trim(),
                        Recorrente = x.Groups["Recorrente"].Value.Trim(),
                        Interessado = x.Groups["Interessado"].Value.Trim()
                    });
                    continue;
                }
            }

            if (TemDadosParaExportar(extracao))
            {
                _extracoes.Add(extracao);
            }
            
            return responseResult;
        }

        private void PrepararProgressBar(int totalPaginas)
        {
            ExecutarNaUiThread(() =>
            {
                progressBarExtracao.Minimum = 0;
                progressBarExtracao.Maximum = Math.Max(totalPaginas, 1);
                progressBarExtracao.Value = 0;
                progressBarExtracao.Visible = true;

                lblProgressoExtracao.Text = $"0 de {totalPaginas} páginas processadas";
                lblProgressoExtracao.Visible = true;
            });
        }

        private void AtualizarProgressBar(int paginasProcessadas, int totalPaginas)
        {
            ExecutarNaUiThread(() =>
            {
                progressBarExtracao.Value = Math.Min(paginasProcessadas, progressBarExtracao.Maximum);
                lblProgressoExtracao.Text = $"{paginasProcessadas} de {totalPaginas} páginas processadas";
            });
        }

        private void AtualizarEstadoExtracao(bool emAndamento)
        {
            ExecutarNaUiThread(() => btn_Extrair.Enabled = !emAndamento);
        }

        private void MostrarMensagem(string mensagem)
        {
            ExecutarNaUiThread(() => MessageBox.Show(this, mensagem));
        }

        private void ExecutarNaUiThread(Action action)
        {
            if (!IsHandleCreated || IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                Invoke(action);
                return;
            }

            action();
        }

        private static bool TemDadosParaExportar(Extracao extracao)
        {
            return extracao.Dia > 0
                   || extracao.Ano > 0
                   || !string.IsNullOrWhiteSpace(extracao.Mes)
                   || extracao.Processos.Count > 0;
        }

        private void GenerateExcelFile()
        {
            var filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                $"extracoes-diario-oficial-{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}.xlsx");

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Extracoes");

            worksheet.Cell(1, 1).Value = "Dia";
            worksheet.Cell(1, 2).Value = "Mes";
            worksheet.Cell(1, 3).Value = "Ano";
            worksheet.Cell(1, 4).Value = "Relator";
            worksheet.Cell(1, 5).Value = "NumeroProcesso";
            worksheet.Cell(1, 6).Value = "Recorrente";
            worksheet.Cell(1, 7).Value = "Interessado";

            var row = 2;
            foreach (var extracao in _extracoes)
            {
                if (extracao.Processos.Count == 0)
                {
                    worksheet.Cell(row, 1).Value = extracao.Dia;
                    worksheet.Cell(row, 2).Value = extracao.Mes;
                    worksheet.Cell(row, 3).Value = extracao.Ano;
                    row++;
                    continue;
                }

                foreach (var processo in extracao.Processos)
                {
                    worksheet.Cell(row, 1).Value = extracao.Dia;
                    worksheet.Cell(row, 2).Value = extracao.Mes;
                    worksheet.Cell(row, 3).Value = extracao.Ano;
                    worksheet.Cell(row, 4).Value = processo.Relator;
                    worksheet.Cell(row, 5).Value = processo.NumeroProcesso;
                    worksheet.Cell(row, 6).Value = processo.Recorrente;
                    worksheet.Cell(row, 7).Value = processo.Interessado;
                    row++;
                }
            }

            var headerRange = worksheet.Range(1, 1, 1, 7);
            headerRange.Style.Font.Bold = true;

            if (row > 2)
            {
                worksheet.Range(1, 1, row - 1, 7).CreateTable();
            }

            worksheet.Columns().AdjustToContents();

            workbook.SaveAs(filePath);
        }

        private async Task<JsonObject.Root?> GetParametrosPaginaAsync()
        {
            var scriptResponse = await _browser.EvaluateScriptAsync("""
                                                                    (() => {
                                                                        const params = document.getElementById("params");
                                                                        return params ? params.textContent : null;
                                                                    })()
                                                                    """);

            if (!scriptResponse.Success || scriptResponse.Result is null)
            {
                return null;
            }

            var json = scriptResponse.Result.ToString();
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            return JsonSerializer.Deserialize<JsonObject.Root>(json);
        }

        private void OnRequestIntercepted(InterceptedRequest request)
        {
            Debug.WriteLine($"[CEF REQUEST] {request.Identifier} {request.Method} {request.Url}");

            foreach (var header in request.Headers)
            {
                Debug.WriteLine($"[CEF REQUEST HEADER] {request.Identifier} {header.Key}: {header.Value}");
            }
        }

        private void OnResponseIntercepted(InterceptedResponse response)
        {
            Debug.WriteLine(
                $"[CEF RESPONSE] {response.Identifier} {response.StatusCode} {response.MimeType} {response.Url}");
        }

        private sealed class DiarioOficialRequestHandler : RequestHandler
        {
            private readonly Action<InterceptedRequest> _onRequest;
            private readonly Action<InterceptedResponse> _onResponse;

            public DiarioOficialRequestHandler(
                Action<InterceptedRequest> onRequest,
                Action<InterceptedResponse> onResponse)
            {
                _onRequest = onRequest;
                _onResponse = onResponse;
            }

            protected override IResourceRequestHandler GetResourceRequestHandler(
                IWebBrowser chromiumWebBrowser,
                IBrowser browser,
                IFrame frame,
                IRequest request,
                bool isNavigation,
                bool isDownload,
                string requestInitiator,
                ref bool disableDefaultHandling)
            {
                return new DiarioOficialResourceRequestHandler(_onRequest, _onResponse);
            }
        }

        private sealed class DiarioOficialResourceRequestHandler : ResourceRequestHandler
        {
            private readonly Action<InterceptedRequest> _onRequest;
            private readonly Action<InterceptedResponse> _onResponse;

            public DiarioOficialResourceRequestHandler(
                Action<InterceptedRequest> onRequest,
                Action<InterceptedResponse> onResponse)
            {
                _onRequest = onRequest;
                _onResponse = onResponse;
            }

            protected override CefReturnValue OnBeforeResourceLoad(
                IWebBrowser chromiumWebBrowser,
                IBrowser browser,
                IFrame frame,
                IRequest request,
                IRequestCallback callback)
            {
                _onRequest(new InterceptedRequest(
                    request.Identifier,
                    request.Method,
                    request.Url,
                    request.Headers.AllKeys
                        .Where(key => key is not null)
                        .ToDictionary(key => key!, key => request.Headers[key!] ?? string.Empty)));

                return CefReturnValue.Continue;
            }

            protected override bool OnResourceResponse(
                IWebBrowser chromiumWebBrowser,
                IBrowser browser,
                IFrame frame,
                IRequest request,
                IResponse response)
            {
                _onResponse(new InterceptedResponse(
                    request.Identifier,
                    request.Url,
                    response.StatusCode,
                    response.MimeType));

                return false;
            }
        }

        private sealed record InterceptedRequest(
            ulong Identifier,
            string Method,
            string Url,
            IReadOnlyDictionary<string, string> Headers);

        private sealed record InterceptedResponse(
            ulong Identifier,
            string Url,
            int StatusCode,
            string MimeType);
    }
}
