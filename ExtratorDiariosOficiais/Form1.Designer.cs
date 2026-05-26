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
            btn_Extrair = new Button();
            progressBarExtracao = new ProgressBar();
            lblProgressoExtracao = new Label();
            SuspendLayout();
            // 
            // btn_Extrair
            // 
            btn_Extrair.Location = new System.Drawing.Point(12, 12);
            btn_Extrair.Name = "btn_Extrair";
            btn_Extrair.Size = new System.Drawing.Size(92, 28);
            btn_Extrair.TabIndex = 0;
            btn_Extrair.Text = "Extrair";
            btn_Extrair.UseVisualStyleBackColor = true;
            btn_Extrair.Click += btn_Extrair_Click;
            // 
            // progressBarExtracao
            // 
            progressBarExtracao.Location = new System.Drawing.Point(12, 51);
            progressBarExtracao.Name = "progressBarExtracao";
            progressBarExtracao.Size = new System.Drawing.Size(473, 23);
            progressBarExtracao.TabIndex = 1;
            progressBarExtracao.Visible = false;
            // 
            // lblProgressoExtracao
            // 
            lblProgressoExtracao.AutoSize = true;
            lblProgressoExtracao.Location = new System.Drawing.Point(12, 80);
            lblProgressoExtracao.Name = "lblProgressoExtracao";
            lblProgressoExtracao.Size = new System.Drawing.Size(0, 15);
            lblProgressoExtracao.TabIndex = 2;
            lblProgressoExtracao.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(497, 310);
            Controls.Add(lblProgressoExtracao);
            Controls.Add(progressBarExtracao);
            Controls.Add(btn_Extrair);
            Text = "Extração - Diário Oficial da União";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_Extrair;
        private ProgressBar progressBarExtracao;
        private Label lblProgressoExtracao;
    }
}
