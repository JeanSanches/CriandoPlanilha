using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CriadorPlanilhas
{
    public partial class Form1 : Form
    {
        private int tempoPausa = 20;
        public Form1()
        {
            InitializeComponent();

            dtpReferencia.Format = DateTimePickerFormat.Custom;

            dtpReferencia.CustomFormat = "MMM yyyy";
            txtHoraExtra.Text = "0";
            txtMinutoExtra.Text = "0";

            txtHoraEspera.Text = "0";
            txtMinutoEspera.Text = "0";
        }

        private void btnGerar_Click(object sender, EventArgs e)
        {
            dgvDados.Rows.Clear();

            var index = 0;

            foreach (var dia in GetDias(dtpReferencia.Value.Year, dtpReferencia.Value.Month))
            {
                DateTime dateValue = new DateTime(dtpReferencia.Value.Year, dtpReferencia.Value.Month, dia);
                var diaSemana = dateValue.ToString("dddd", new CultureInfo("pt-BR"));
                index = dgvDados.Rows.Add();
                dgvDados.Rows[index].Cells["Dia"].Value = dia + "  |  " + diaSemana;

                if (diaSemana != "domingo")
                {
                    dgvDados.Rows[index].Cells["Entrada"].Value = GerarHoraInicial(index);
                    dgvDados.Rows[index].Cells["IntervaloInicial"].Value = GerarIntervaloInicial(index);
                    dgvDados.Rows[index].Cells["IntervaloFinal"].Value = GerarIntervaloFinal(index);
                    dgvDados.Rows[index].Cells["HoraExtra"].Value = "0";
                    dgvDados.Rows[index].Cells["HoraExtraMinutos"].Value = "0";
                    dgvDados.Rows[index].Cells["TempoEspera"].Value = "0";
                    dgvDados.Rows[index].Cells["TempoEsperaMinutos"].Value = "0";
                }
                else
                {
                    dgvDados.Rows[index].Cells["Entrada"].Value = "-";
                    dgvDados.Rows[index].Cells["IntervaloInicial"].Value = "-";
                    dgvDados.Rows[index].Cells["IntervaloFinal"].Value = "-";
                    dgvDados.Rows[index].Cells["HoraExtra"].Value = "-";
                    dgvDados.Rows[index].Cells["HoraExtraMinutos"].Value = "-";
                    dgvDados.Rows[index].Cells["TempoEspera"].Value = "-";
                    dgvDados.Rows[index].Cells["TempoEsperaMinutos"].Value = "-";
                }


                Thread.Sleep(tempoPausa);
            }

            CalcularValoresAleatorios(Convert.ToInt32(txtHoraExtra.Text) * 60 + Convert.ToInt32(txtMinutoExtra.Text), "HoraExtra", "HoraExtraMinutos");

            CalcularValoresAleatorios(Convert.ToInt32(txtHoraEspera.Text) * 60 + Convert.ToInt32(txtMinutoEspera.Text), "TempoEspera", "TempoEsperaMinutos");

            for (index = 0; index < dgvDados.Rows.Count; index++)
            {
                if (!dgvDados.Rows[index].Cells["Entrada"].Value.Equals("-"))
                {
                    dgvDados.Rows[index].Cells["Saida"].Value = GerarHoraFinal(index);

                    Thread.Sleep(tempoPausa);
                }
                else
                {
                    dgvDados.Rows[index].Cells["Saida"].Value = "-";

                }
            }

            CorrecaoInterJornada();

            GerarTotatais();

            dgvDados.Enabled = true;
        }

        private void GerarTotatais()
        {
            txtTotalHoraExtra.Text = CalcularTotal("HoraExtraMinutos");
            txtTotalHoraEspera.Text = CalcularTotal("TempoEsperaMinutos");

            for (var index = 0; index < dgvDados.Rows.Count; index++)
            {
                CalcularParcialTrabalhado(index);
                CalcularTotalTrabalhado(index);
            }
        }

        private string CalcularTotal(string chave)
        {
            var total = 0;

            for (var index = 0; index < dgvDados.Rows.Count; index++)
            {
                if (!dgvDados.Rows[index].Cells[chave].Value.Equals("-"))
                    total += Convert.ToInt32(dgvDados.Rows[index].Cells[chave].Value);
            }

            return TraduzirMinutosTotal(total);
        }

        private string TraduzirMinutos(int index, string chaveCalculo)
        {
            if (!dgvDados.Rows[index].Cells[chaveCalculo].Value.Equals("-"))
            {
                var minutos = Convert.ToInt32(dgvDados.Rows[index].Cells[chaveCalculo].Value);

                if (minutos == 0)
                    return "-";

                var horaTraduzida = minutos / 60;
                var minutoTraduzido = minutos % 60;


                return horaTraduzida + "h " + minutoTraduzido + "m";
            }

            return "-";
        }

        private string TraduzirMinutosTotal(int valor)
        {
            if (valor == 0)
                return "-";

            var horaTraduzida = valor / 60;
            var minutoTraduzido = valor % 60;


            return horaTraduzida + "h " + minutoTraduzido + "m";
        }

        private IEnumerable<int> GetDias(int ano, int mes)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(ano, mes));
        }

        private DateTime GerarHoraInicial(int index)
        {
            var minutes = new Random().Next(120);

            var data = new DateTime(dtpReferencia.Value.Year, dtpReferencia.Value.Month, index + 1, 7, 0, 0);

            return data.AddMinutes(minutes);
        }

        private void CorrecaoInterJornada()
        {
            var horaInicial = 0;
            var minutos = 0;

            for (var index = 0; index < dgvDados.Rows.Count; index++)
            {
                if (!dgvDados.Rows[index].Cells["Entrada"].Value.Equals("-"))
                {
                    var verificaInterjornada = index > 0;
                    var valorAleatorioFinal = 120;

                    if (verificaInterjornada)
                        if (!dgvDados.Rows[index - 1].Cells["Saida"].Value.Equals("-"))
                        {
                            var dataSaidaDiaAnterior = Convert.ToDateTime(dgvDados.Rows[index - 1].Cells["Saida"].Value);

                            if (dataSaidaDiaAnterior.Hour >= 20)
                            {
                                var dataAnterior = dataSaidaDiaAnterior.AddHours(11);
                                var valueOld = Convert.ToDateTime(dgvDados.Rows[index].Cells["Entrada"].Value);

                                if (valueOld < dataAnterior)
                                {
                                    horaInicial = dataAnterior.Hour;
                                    minutos = dataAnterior.Minute;
                                    valorAleatorioFinal = 10;

                                    var minutes = new Random().Next(1, valorAleatorioFinal);

                                    var data = new DateTime(dtpReferencia.Value.Year, dtpReferencia.Value.Month, index + 1, horaInicial, minutos, 0);
                                    var dataFinal = data.AddMinutes(minutes);

                                    dgvDados.Rows[index].Cells["Entrada"].Value = dataFinal;

                                    var diferenca = (dataFinal - valueOld).TotalMinutes;

                                    dgvDados.Rows[index].Cells["IntervaloInicial"].Value = Convert.ToDateTime(dgvDados.Rows[index].Cells["IntervaloInicial"].Value).AddMinutes(diferenca);
                                    dgvDados.Rows[index].Cells["IntervaloFinal"].Value = Convert.ToDateTime(dgvDados.Rows[index].Cells["IntervaloFinal"].Value).AddMinutes(diferenca);
                                    dgvDados.Rows[index].Cells["Saida"].Value = Convert.ToDateTime(dgvDados.Rows[index].Cells["Saida"].Value).AddMinutes(diferenca);
                                }
                            }

                            var value = Convert.ToInt32((Convert.ToDateTime(dgvDados.Rows[index].Cells["Entrada"].Value) - dataSaidaDiaAnterior).TotalMinutes);

                            dgvDados.Rows[index].Cells["IntervaloInterJornada"].Value = TraduzirMinutosTotal(value);
                        }
                        else     
                            dgvDados.Rows[index].Cells["IntervaloInterJornada"].Value = "-";
                        
                }
                else             
                    dgvDados.Rows[index].Cells["IntervaloInterJornada"].Value = "-";

                Thread.Sleep(tempoPausa);

            }
        }

        private DateTime GerarIntervaloInicial(int index)
        {
            var entrada = Convert.ToDateTime(dgvDados.Rows[index].Cells["Entrada"].Value);

            var data = new DateTime(entrada.Year, entrada.Month, entrada.Day, entrada.Hour, entrada.Minute, entrada.Second);

            return data.AddMinutes(new Random().Next(240, 360));
        }

        private DateTime GerarIntervaloFinal(int index)
        {
            var intervaloInicial = Convert.ToDateTime(dgvDados.Rows[index].Cells["IntervaloInicial"].Value);

            var data = new DateTime(intervaloInicial.Year, intervaloInicial.Month, intervaloInicial.Day, intervaloInicial.Hour, intervaloInicial.Minute, intervaloInicial.Second);

            return data.AddMinutes(new Random().Next(60, 120));
        }

        private DateTime GerarHoraFinal(int index)
        {
            var entrada = Convert.ToDateTime(dgvDados.Rows[index].Cells["Entrada"].Value);
            var intervaloInicial = Convert.ToDateTime(dgvDados.Rows[index].Cells["IntervaloInicial"].Value);
            var intervaloFinal = Convert.ToDateTime(dgvDados.Rows[index].Cells["IntervaloFinal"].Value);

            var horaTrabalhadaAteIntervalo = (intervaloInicial - entrada).TotalMinutes;

            var horaFinal = intervaloFinal.AddMinutes((8 * 60) - horaTrabalhadaAteIntervalo);

            return horaFinal.AddMinutes(Convert.ToInt32(dgvDados.Rows[index].Cells["HoraExtraMinutos"].Value))
             .AddMinutes(Convert.ToInt32(dgvDados.Rows[index].Cells["TempoEsperaMinutos"].Value));
        }

        private string CalcularValorAleatorio(int index, ref int valor, string chave, int max)
        {
            if (!dgvDados.Rows[index].Cells[chave].Value.Equals("-"))
            {
                var possuiHorasExtras = new Random().Next(3);

                if (possuiHorasExtras == 1 || possuiHorasExtras == 2)
                {
                    var horas = new Random().Next(1, max);

                    var valorFinal = Convert.ToInt32(dgvDados.Rows[index].Cells[chave].Value) + horas;

                    if (valorFinal <= 120)
                    {
                        valor -= horas;

                        if (valor < 0)
                        {
                            valorFinal += valor;

                            valor = 0;
                        }

                        return (valorFinal).ToString();
                    }

                    return dgvDados.Rows[index].Cells[chave].Value.ToString();
                }

                return dgvDados.Rows[index].Cells[chave].Value.ToString();
            }

            return "-";
        }

        private void CalcularValoresAleatorios(int totalHoras, string chaveVisivel, string chaveCalculo)
        {
            var index = 0;
            var rodadas = 0;
            var max = 120;

            while (totalHoras > 0)
            {
                if (index == dgvDados.Rows.Count)
                {
                    index = 0;
                    rodadas++;

                    if (rodadas == 15)
                    {
                        max = Convert.ToInt32(Math.Ceiling((decimal)totalHoras / 10));
                        rodadas = 0;
                    }
                }

                dgvDados.Rows[index].Cells[chaveCalculo].Value = CalcularValorAleatorio(index, ref totalHoras, chaveCalculo, max);
                dgvDados.Rows[index].Cells[chaveVisivel].Value = TraduzirMinutos(index, chaveCalculo);

                index++;

                Thread.Sleep(tempoPausa);
            }
        }

        private void CalcularParcialTrabalhado(int index)
        {
            if (!dgvDados.Rows[index].Cells["Entrada"].Value.Equals("-"))
            {
                var entrada = Convert.ToDateTime(dgvDados.Rows[index].Cells["Entrada"].Value);
                var intervaloInicial = Convert.ToDateTime(dgvDados.Rows[index].Cells["IntervaloInicial"].Value);

                var minutos = Convert.ToInt32((intervaloInicial - entrada).TotalMinutes);

                dgvDados.Rows[index].Cells["ParcialTrabalhado"].Value = TraduzirMinutosTotal(minutos);
                dgvDados.Rows[index].Cells["ParcialTrabalhadoMinutos"].Value = minutos;
            }
            else
            {
                dgvDados.Rows[index].Cells["ParcialTrabalhado"].Value = "-";
                dgvDados.Rows[index].Cells["ParcialTrabalhadoMinutos"].Value = "-";
            }
        }

        private void CalcularTotalTrabalhado(int index)
        {
            if (!dgvDados.Rows[index].Cells["Entrada"].Value.Equals("-"))
            {
                var retorno = Convert.ToDateTime(dgvDados.Rows[index].Cells["IntervaloFinal"].Value);
                var saida = Convert.ToDateTime(dgvDados.Rows[index].Cells["Saida"].Value);

                var minutos = Convert.ToInt32(dgvDados.Rows[index].Cells["ParcialTrabalhadoMinutos"].Value) + Convert.ToInt32((saida - retorno).TotalMinutes);

                dgvDados.Rows[index].Cells["TotalTrabalhado"].Value = TraduzirMinutosTotal(minutos);
                dgvDados.Rows[index].Cells["TotalTrabalhadoMinutos"].Value = minutos;
            }
            else
            {
                dgvDados.Rows[index].Cells["TotalTrabalhado"].Value = "-";
                dgvDados.Rows[index].Cells["TotalTrabalhadoMinutos"].Value = "-";
            }
        }
    }
}
