namespace ExtratorDiariosOficiais
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            mainPanel = new Panel();
            subtitleLabel = new Label();
            titleLabel = new Label();
            actionPanel = new Panel();
            outputLabel = new Label();
            lblStatusExtracao = new Label();
            btn_Extrair = new Button();
            progressBarExtracao = new ProgressBar();
            lblProgressoExtracao = new Label();
            mainPanel.SuspendLayout();
            actionPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.BackColor = System.Drawing.Color.White;
            mainPanel.Controls.Add(subtitleLabel);
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(actionPanel);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new System.Drawing.Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Padding = new Padding(32, 28, 32, 32);
            mainPanel.Size = new System.Drawing.Size(640, 360);
            mainPanel.TabIndex = 0;
            // 
            // subtitleLabel
            // 
            subtitleLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            subtitleLabel.ForeColor = System.Drawing.Color.FromArgb(88, 95, 105);
            subtitleLabel.Location = new System.Drawing.Point(34, 77);
            subtitleLabel.Name = "subtitleLabel";
            subtitleLabel.Size = new System.Drawing.Size(552, 42);
            subtitleLabel.TabIndex = 1;
            subtitleLabel.Text = "Extrai pautas de julgamento do Diário Oficial da União e gera uma planilha em Documentos.";
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            titleLabel.ForeColor = System.Drawing.Color.FromArgb(28, 34, 43);
            titleLabel.Location = new System.Drawing.Point(32, 28);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new System.Drawing.Size(367, 41);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Extrator de Diários Oficiais";
            // 
            // actionPanel
            // 
            actionPanel.BackColor = System.Drawing.Color.FromArgb(246, 248, 251);
            actionPanel.Controls.Add(outputLabel);
            actionPanel.Controls.Add(lblStatusExtracao);
            actionPanel.Controls.Add(btn_Extrair);
            actionPanel.Controls.Add(progressBarExtracao);
            actionPanel.Controls.Add(lblProgressoExtracao);
            actionPanel.Location = new System.Drawing.Point(32, 137);
            actionPanel.Name = "actionPanel";
            actionPanel.Padding = new Padding(24);
            actionPanel.Size = new System.Drawing.Size(576, 183);
            actionPanel.TabIndex = 2;
            // 
            // outputLabel
            // 
            outputLabel.AutoSize = true;
            outputLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            outputLabel.ForeColor = System.Drawing.Color.FromArgb(97, 104, 115);
            outputLabel.Location = new System.Drawing.Point(27, 128);
            outputLabel.Name = "outputLabel";
            outputLabel.Size = new System.Drawing.Size(245, 15);
            outputLabel.TabIndex = 4;
            outputLabel.Text = "Arquivo de saída: pasta Documentos do usuário";
            // 
            // lblStatusExtracao
            // 
            lblStatusExtracao.AutoSize = true;
            lblStatusExtracao.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            lblStatusExtracao.ForeColor = System.Drawing.Color.FromArgb(28, 34, 43);
            lblStatusExtracao.Location = new System.Drawing.Point(27, 27);
            lblStatusExtracao.Name = "lblStatusExtracao";
            lblStatusExtracao.Size = new System.Drawing.Size(135, 20);
            lblStatusExtracao.TabIndex = 0;
            lblStatusExtracao.Text = "Pronto para extrair";
            // 
            // btn_Extrair
            // 
            btn_Extrair.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btn_Extrair.BackColor = System.Drawing.Color.FromArgb(35, 99, 235);
            btn_Extrair.Cursor = Cursors.Hand;
            btn_Extrair.FlatAppearance.BorderSize = 0;
            btn_Extrair.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(29, 78, 216);
            btn_Extrair.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(37, 99, 235);
            btn_Extrair.FlatStyle = FlatStyle.Flat;
            btn_Extrair.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btn_Extrair.ForeColor = System.Drawing.Color.White;
            btn_Extrair.Location = new System.Drawing.Point(426, 24);
            btn_Extrair.Name = "btn_Extrair";
            btn_Extrair.Size = new System.Drawing.Size(123, 40);
            btn_Extrair.TabIndex = 1;
            btn_Extrair.Text = "Extrair dados";
            btn_Extrair.UseVisualStyleBackColor = false;
            btn_Extrair.Click += btn_Extrair_Click;
            // 
            // progressBarExtracao
            // 
            progressBarExtracao.Location = new System.Drawing.Point(27, 80);
            progressBarExtracao.Name = "progressBarExtracao";
            progressBarExtracao.Size = new System.Drawing.Size(522, 16);
            progressBarExtracao.Style = ProgressBarStyle.Continuous;
            progressBarExtracao.TabIndex = 2;
            progressBarExtracao.Visible = false;
            // 
            // lblProgressoExtracao
            // 
            lblProgressoExtracao.AutoSize = true;
            lblProgressoExtracao.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            lblProgressoExtracao.ForeColor = System.Drawing.Color.FromArgb(73, 80, 92);
            lblProgressoExtracao.Location = new System.Drawing.Point(27, 103);
            lblProgressoExtracao.Name = "lblProgressoExtracao";
            lblProgressoExtracao.Size = new System.Drawing.Size(156, 17);
            lblProgressoExtracao.TabIndex = 3;
            lblProgressoExtracao.Text = "Aguardando solicitação";
            lblProgressoExtracao.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(640, 360);
            Controls.Add(mainPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(656, 399);
            MinimumSize = new System.Drawing.Size(656, 399);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Extração - Diário Oficial da União";
            mainPanel.ResumeLayout(false);
            mainPanel.PerformLayout();
            actionPanel.ResumeLayout(false);
            actionPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel mainPanel;
        private Label subtitleLabel;
        private Label titleLabel;
        private Panel actionPanel;
        private Label outputLabel;
        private Label lblStatusExtracao;
        private Button btn_Extrair;
        private ProgressBar progressBarExtracao;
        private Label lblProgressoExtracao;
    }
}
