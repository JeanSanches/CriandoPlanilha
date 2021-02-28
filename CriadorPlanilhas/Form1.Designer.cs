namespace CriadorPlanilhas
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnGerar = new System.Windows.Forms.Button();
            this.dtpReferencia = new System.Windows.Forms.DateTimePicker();
            this.dgvDados = new System.Windows.Forms.DataGridView();
            this.btnExportar = new System.Windows.Forms.Button();
            this.txtHoraExtra = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMinutoExtra = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTotalHoraExtra = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMinutoEspera = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtHoraEspera = new System.Windows.Forms.TextBox();
            this.txtTotalHoraEspera = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.Dia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IntervaloInterJornada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Entrada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IntervaloInicial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParcialTrabalhado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IntervaloFinal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Saida = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalTrabalhado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TempoEspera = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TempoEsperaMinutos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HoraExtra = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HoraExtraMinutos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParcialTrabalhadoMinutos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalTrabalhadoMinutos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDados)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGerar
            // 
            this.btnGerar.Location = new System.Drawing.Point(662, 13);
            this.btnGerar.Name = "btnGerar";
            this.btnGerar.Size = new System.Drawing.Size(124, 34);
            this.btnGerar.TabIndex = 0;
            this.btnGerar.Text = "Gerar";
            this.btnGerar.UseVisualStyleBackColor = true;
            this.btnGerar.Click += new System.EventHandler(this.btnGerar_Click);
            // 
            // dtpReferencia
            // 
            this.dtpReferencia.Location = new System.Drawing.Point(502, 20);
            this.dtpReferencia.Name = "dtpReferencia";
            this.dtpReferencia.Size = new System.Drawing.Size(150, 20);
            this.dtpReferencia.TabIndex = 1;
            // 
            // dgvDados
            // 
            this.dgvDados.AllowUserToAddRows = false;
            this.dgvDados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDados.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Dia,
            this.IntervaloInterJornada,
            this.Entrada,
            this.IntervaloInicial,
            this.ParcialTrabalhado,
            this.IntervaloFinal,
            this.Saida,
            this.TotalTrabalhado,
            this.TempoEspera,
            this.TempoEsperaMinutos,
            this.HoraExtra,
            this.HoraExtraMinutos,
            this.ParcialTrabalhadoMinutos,
            this.TotalTrabalhadoMinutos});
            this.dgvDados.Enabled = false;
            this.dgvDados.Location = new System.Drawing.Point(12, 53);
            this.dgvDados.Name = "dgvDados";
            this.dgvDados.Size = new System.Drawing.Size(1027, 537);
            this.dgvDados.TabIndex = 2;
            // 
            // btnExportar
            // 
            this.btnExportar.Location = new System.Drawing.Point(880, 655);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(159, 32);
            this.btnExportar.TabIndex = 3;
            this.btnExportar.Text = "Exportar";
            this.btnExportar.UseVisualStyleBackColor = true;
            // 
            // txtHoraExtra
            // 
            this.txtHoraExtra.Location = new System.Drawing.Point(51, 20);
            this.txtHoraExtra.Name = "txtHoraExtra";
            this.txtHoraExtra.Size = new System.Drawing.Size(63, 20);
            this.txtHoraExtra.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Horas extras";
            // 
            // txtMinutoExtra
            // 
            this.txtMinutoExtra.Location = new System.Drawing.Point(174, 20);
            this.txtMinutoExtra.Name = "txtMinutoExtra";
            this.txtMinutoExtra.Size = new System.Drawing.Size(63, 20);
            this.txtMinutoExtra.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Horas";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(124, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Minutos";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(935, 602);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Total Horas Extras";
            // 
            // txtTotalHoraExtra
            // 
            this.txtTotalHoraExtra.Enabled = false;
            this.txtTotalHoraExtra.Location = new System.Drawing.Point(939, 619);
            this.txtTotalHoraExtra.Name = "txtTotalHoraExtra";
            this.txtTotalHoraExtra.Size = new System.Drawing.Size(100, 20);
            this.txtTotalHoraExtra.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(371, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Minutos";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(256, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Horas";
            // 
            // txtMinutoEspera
            // 
            this.txtMinutoEspera.Location = new System.Drawing.Point(421, 20);
            this.txtMinutoEspera.Name = "txtMinutoEspera";
            this.txtMinutoEspera.Size = new System.Drawing.Size(63, 20);
            this.txtMinutoEspera.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(256, 1);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Tempo de espera";
            // 
            // txtHoraEspera
            // 
            this.txtHoraEspera.Location = new System.Drawing.Point(298, 20);
            this.txtHoraEspera.Name = "txtHoraEspera";
            this.txtHoraEspera.Size = new System.Drawing.Size(63, 20);
            this.txtHoraEspera.TabIndex = 11;
            // 
            // txtTotalHoraEspera
            // 
            this.txtTotalHoraEspera.Enabled = false;
            this.txtTotalHoraEspera.Location = new System.Drawing.Point(823, 619);
            this.txtTotalHoraEspera.Name = "txtTotalHoraEspera";
            this.txtTotalHoraEspera.Size = new System.Drawing.Size(100, 20);
            this.txtTotalHoraEspera.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(819, 602);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Total Horas Espera";
            // 
            // Dia
            // 
            this.Dia.HeaderText = "";
            this.Dia.Name = "Dia";
            // 
            // IntervaloInterJornada
            // 
            this.IntervaloInterJornada.HeaderText = "Intervalo Inter Jornada";
            this.IntervaloInterJornada.Name = "IntervaloInterJornada";
            // 
            // Entrada
            // 
            dataGridViewCellStyle1.Format = "t";
            dataGridViewCellStyle1.NullValue = null;
            this.Entrada.DefaultCellStyle = dataGridViewCellStyle1;
            this.Entrada.HeaderText = "Entrada";
            this.Entrada.Name = "Entrada";
            // 
            // IntervaloInicial
            // 
            dataGridViewCellStyle2.Format = "t";
            dataGridViewCellStyle2.NullValue = null;
            this.IntervaloInicial.DefaultCellStyle = dataGridViewCellStyle2;
            this.IntervaloInicial.HeaderText = "Inicial Intervalo/ Parada Obrigatória";
            this.IntervaloInicial.Name = "IntervaloInicial";
            // 
            // ParcialTrabalhado
            // 
            this.ParcialTrabalhado.HeaderText = "Parcial Trabalhado";
            this.ParcialTrabalhado.Name = "ParcialTrabalhado";
            // 
            // IntervaloFinal
            // 
            dataGridViewCellStyle3.Format = "t";
            dataGridViewCellStyle3.NullValue = null;
            this.IntervaloFinal.DefaultCellStyle = dataGridViewCellStyle3;
            this.IntervaloFinal.HeaderText = "Final Intervalo/ Parada Obrigatória";
            this.IntervaloFinal.Name = "IntervaloFinal";
            // 
            // Saida
            // 
            dataGridViewCellStyle4.Format = "t";
            dataGridViewCellStyle4.NullValue = null;
            this.Saida.DefaultCellStyle = dataGridViewCellStyle4;
            this.Saida.HeaderText = "Saída";
            this.Saida.Name = "Saida";
            // 
            // TotalTrabalhado
            // 
            this.TotalTrabalhado.HeaderText = "Total Trabalhado";
            this.TotalTrabalhado.Name = "TotalTrabalhado";
            // 
            // TempoEspera
            // 
            this.TempoEspera.HeaderText = "Tempo de espera";
            this.TempoEspera.Name = "TempoEspera";
            // 
            // TempoEsperaMinutos
            // 
            this.TempoEsperaMinutos.HeaderText = "TempoEsperaMinutos";
            this.TempoEsperaMinutos.Name = "TempoEsperaMinutos";
            this.TempoEsperaMinutos.Visible = false;
            // 
            // HoraExtra
            // 
            this.HoraExtra.HeaderText = "Hora Extra";
            this.HoraExtra.Name = "HoraExtra";
            // 
            // HoraExtraMinutos
            // 
            this.HoraExtraMinutos.HeaderText = "HoraExtraMinutos";
            this.HoraExtraMinutos.Name = "HoraExtraMinutos";
            this.HoraExtraMinutos.Visible = false;
            // 
            // ParcialTrabalhadoMinutos
            // 
            this.ParcialTrabalhadoMinutos.HeaderText = "TotalTrabalhadoMinutos";
            this.ParcialTrabalhadoMinutos.Name = "ParcialTrabalhadoMinutos";
            this.ParcialTrabalhadoMinutos.Visible = false;
            // 
            // TotalTrabalhadoMinutos
            // 
            this.TotalTrabalhadoMinutos.HeaderText = "TotalTrabalhadoMinutos";
            this.TotalTrabalhadoMinutos.Name = "TotalTrabalhadoMinutos";
            this.TotalTrabalhadoMinutos.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 689);
            this.Controls.Add(this.txtTotalHoraEspera);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtMinutoEspera);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtHoraEspera);
            this.Controls.Add(this.txtTotalHoraExtra);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMinutoExtra);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtHoraExtra);
            this.Controls.Add(this.btnExportar);
            this.Controls.Add(this.dgvDados);
            this.Controls.Add(this.dtpReferencia);
            this.Controls.Add(this.btnGerar);
            this.Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDados)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGerar;
        private System.Windows.Forms.DateTimePicker dtpReferencia;
        private System.Windows.Forms.DataGridView dgvDados;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.TextBox txtHoraExtra;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMinutoExtra;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTotalHoraExtra;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMinutoEspera;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtHoraEspera;
        private System.Windows.Forms.TextBox txtTotalHoraEspera;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dia;
        private System.Windows.Forms.DataGridViewTextBoxColumn IntervaloInterJornada;
        private System.Windows.Forms.DataGridViewTextBoxColumn Entrada;
        private System.Windows.Forms.DataGridViewTextBoxColumn IntervaloInicial;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParcialTrabalhado;
        private System.Windows.Forms.DataGridViewTextBoxColumn IntervaloFinal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Saida;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalTrabalhado;
        private System.Windows.Forms.DataGridViewTextBoxColumn TempoEspera;
        private System.Windows.Forms.DataGridViewTextBoxColumn TempoEsperaMinutos;
        private System.Windows.Forms.DataGridViewTextBoxColumn HoraExtra;
        private System.Windows.Forms.DataGridViewTextBoxColumn HoraExtraMinutos;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParcialTrabalhadoMinutos;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalTrabalhadoMinutos;
    }
}

