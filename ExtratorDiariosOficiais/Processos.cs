namespace ExtratorDiariosOficiais;

public class Extracao
{
    public int Dia { get; set; }
    public string Mes { get; set; }
    public int Ano { get; set; }
    public List<Processo> Processos { get; set; } = new List<Processo>();
}

public class Processo
{
    public string Relator { get; set; }
    public string NumeroProcesso { get; set; }
    public string Recorrente { get; set; }
    public string Interessado { get; set; }
}