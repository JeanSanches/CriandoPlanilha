
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
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
            lbTempoExecucao.Text = "";
        }

        private void btnGerar_Click(object sender, EventArgs e)
        {
            btnGerar.Enabled = false;
            lbTempoExecucao.Text = "";
            dgvDados.Rows.Clear();

            timerGerarPlanilha.Start();
        }

        private void timerGerarPlanilha_Tick(object sender, EventArgs e)
        {
            timerGerarPlanilha.Stop();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var index = 0;
            var diaMaximoMes = DateTime.DaysInMonth(dtpReferencia.Value.Year, dtpReferencia.Value.Month);
            var diasTrabalhados = 0;
            var domingos = new List<int>();
            var sabados = new List<int>();

            foreach (var dia in GetDias(dtpReferencia.Value.Year, dtpReferencia.Value.Month))
            {
                DateTime dateValue = new DateTime(dtpReferencia.Value.Year, dtpReferencia.Value.Month, dia);
                var diaSemana = dateValue.ToString("dddd", new CultureInfo("pt-BR"));
                index = dgvDados.Rows.Add();
                dgvDados.Rows[index].Cells["Dia"].Value = dia + "  |  " + diaSemana;
                dgvDados.Rows[index].Cells["Entrada"].Value = "";

                if (diaSemana == "domingo")
                    domingos.Add(dia);

                if (diaSemana == "sábado")
                    sabados.Add(dia);

            }

            for (var x = domingos.Count; x > 0; x--)
            {
                dgvDados.Rows[domingos[x - 1] - 1].Cells["Entrada"].Value = "-";
            }

            var folgas = 6 - domingos.Count();

            for (; folgas > 0; folgas--)
            {
                var diaFolga = new Random().Next(1, sabados.Count());

                Thread.Sleep(tempoPausa);

                var sabadoSelecionado = sabados[diaFolga - 1];

                dgvDados.Rows[sabadoSelecionado - 1].Cells["Entrada"].Value = "-";

                sabados.Remove(sabadoSelecionado);
            }

            for (index = 0; index < dgvDados.Rows.Count; index++)
            {
                if (!dgvDados.Rows[index].Cells["Entrada"].Value.ToString().Equals("-"))
                {
                    diasTrabalhados++;
                    dgvDados.Rows[index].Cells["Entrada"].Value = GerarHoraInicial(index);
                    dgvDados.Rows[index].Cells["IntervaloInicial"].Value = GerarIntervaloInicial(index);
                    dgvDados.Rows[index].Cells["IntervaloFinal"].Value = GerarIntervaloFinal(index);
                    dgvDados.Rows[index].Cells["HoraExtra"].Value = "-";
                    dgvDados.Rows[index].Cells["HoraExtraMinutos"].Value = "0";
                    dgvDados.Rows[index].Cells["TempoEspera"].Value = "-";
                    dgvDados.Rows[index].Cells["TempoEsperaMinutos"].Value = "0";
                    dgvDados.Rows[index].Cells["Parada"].Value = "-";
                }
                else
                {
                    dgvDados.Rows[index].Cells["Entrada"].Value = "-";
                    dgvDados.Rows[index].Cells["IntervaloInicial"].Value = "-";
                    dgvDados.Rows[index].Cells["IntervaloFinal"].Value = "-";
                    dgvDados.Rows[index].Cells["HoraExtra"].Value = "-";
                    dgvDados.Rows[index].Cells["HoraExtraMinutos"].Value = "0";
                    dgvDados.Rows[index].Cells["TempoEspera"].Value = "-";
                    dgvDados.Rows[index].Cells["TempoEsperaMinutos"].Value = "-";
                    dgvDados.Rows[index].Cells["Parada"].Value = "-";
                }

                Thread.Sleep(tempoPausa);
            }
            var horasExtrasInformado = Convert.ToInt32(txtHoraExtra.Text) * 60 + Convert.ToInt32(txtMinutoExtra.Text);
            var horasExtrasmaxima = (diasTrabalhados * 2) * 60;

            if (horasExtrasInformado > horasExtrasmaxima)
            {
                dgvDados.Rows.Clear();

                MessageBox.Show("A quantidade de hora extra informada não comporta os dias trabalhados (" + diasTrabalhados + "), ou seja, algum dia iria ficar com mais de 2 horas extras. ");

                stopWatch.Stop();
                var ts2 = stopWatch.Elapsed;

                lbTempoExecucao.Text = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts2.Hours, ts2.Minutes, ts2.Seconds, ts2.Milliseconds / 10);

                btnGerar.Enabled = true;

                return;
            }

            if (ccbParada.Checked)
            {
                var posicaoParada = 0;
                do
                {
                    posicaoParada = new Random().Next(0, dgvDados.Rows.Count);

                } while (dgvDados.Rows[posicaoParada].Cells["Entrada"].Value.ToString().Equals("-"));

                dgvDados.Rows[posicaoParada].Cells["Parada"].Value = "1";
            }

            CalcularValoresAleatorios(horasExtrasInformado, "HoraExtra", "HoraExtraMinutos", 1, 120, 120, diasTrabalhados, diaMaximoMes);
            ValidarHorasExtras(horasExtrasInformado, diasTrabalhados, 120);
            CalcularValoresAleatorios(Convert.ToInt32(txtHoraEspera.Text) * 60 + Convert.ToInt32(txtMinutoEspera.Text), "TempoEspera", "TempoEsperaMinutos", 180, 300, 300, diasTrabalhados, diaMaximoMes);

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

            AplicarEntraMaisTarde(diaMaximoMes);

            CorrecaoInterJornada();

            GerarTotatais();

            dgvDados.Enabled = true;
            btnExportar.Enabled = true;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            lbTempoExecucao.Text = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            btnGerar.Enabled = true;
        }

        private void AplicarEntraMaisTarde(int diaMaximoMes)
        {
            var qtde = new Random().Next(2) + 1;
            var diasAplicados = new List<int>();

            while (qtde > 0)
            {
                var index = 0;
                do
                {
                    index = new Random().Next(diaMaximoMes);
                } while (dgvDados.Rows[index].Cells["Entrada"].Value.Equals("-") || diasAplicados.Exists(d => d == index));

                diasAplicados.Add(index);

                var entrada = new Random().Next(1, 60);

                var entradaDia = Convert.ToDateTime(dgvDados.Rows[index].Cells["Entrada"].Value);
                var novaentrada = new DateTime(entradaDia.Year, entradaDia.Month, entradaDia.Day, 0, 0, 0).AddHours(13).AddMinutes(entrada);

                var diferenca = (novaentrada - entradaDia).TotalMinutes;

                dgvDados.Rows[index].Cells["Entrada"].Value = Convert.ToDateTime(dgvDados.Rows[index].Cells["Entrada"].Value).AddMinutes(diferenca);
                dgvDados.Rows[index].Cells["IntervaloInicial"].Value = Convert.ToDateTime(dgvDados.Rows[index].Cells["IntervaloInicial"].Value).AddMinutes(diferenca);
                dgvDados.Rows[index].Cells["IntervaloFinal"].Value = Convert.ToDateTime(dgvDados.Rows[index].Cells["IntervaloFinal"].Value).AddMinutes(diferenca);
                dgvDados.Rows[index].Cells["Saida"].Value = Convert.ToDateTime(dgvDados.Rows[index].Cells["Saida"].Value).AddMinutes(diferenca);

                qtde--;
            }
        }

        private void ValidarHorasExtras(int totalHorasExtras, int diasTrabalhados, int valorMax)
        {
            if (totalHorasExtras > (diasTrabalhados * valorMax * 0.7))
            {
                var diasSemExtra = ((diasTrabalhados * 60 * 2) - totalHorasExtras) / 60 / 2;

                diasSemExtra = diasSemExtra / 2;

                if (diasSemExtra > 0)
                {
                    var listaDias = new List<int>();
                    var listaDiasAplicados = new List<int>();

                    for (var i = 0; i < dgvDados.Rows.Count; i++)
                    {
                        if (!dgvDados.Rows[i].Cells["Entrada"].Value.Equals("-"))
                        {
                            if (dgvDados.Rows[i].Cells["HoraExtra"].Value.Equals("-"))
                                listaDias.Add(i);
                            else
                                listaDiasAplicados.Add(i);
                        }
                    }

                    while (diasSemExtra > 0)
                    {
                        var diaTirarExtra = new Random().Next(1, listaDiasAplicados.Count) - 1;
                        Thread.Sleep(tempoPausa);
                        var diaColocar = new Random().Next(1, listaDias.Count) - 1;
                        Thread.Sleep(tempoPausa);

                        var valorExtra = Convert.ToInt32(dgvDados.Rows[listaDiasAplicados[diaTirarExtra]].Cells["HoraExtraMinutos"].Value);

                        var valorAplicar = new Random().Next(40, valorExtra - 20);

                        dgvDados.Rows[listaDiasAplicados[diaTirarExtra]].Cells["HoraExtraMinutos"].Value = valorExtra - valorAplicar;
                        dgvDados.Rows[listaDiasAplicados[diaTirarExtra]].Cells["HoraExtra"].Value = TraduzirMinutosTotal(valorExtra - valorAplicar);

                        dgvDados.Rows[listaDias[diaColocar]].Cells["HoraExtraMinutos"].Value = valorAplicar;
                        dgvDados.Rows[listaDias[diaColocar]].Cells["HoraExtra"].Value = TraduzirMinutosTotal(valorAplicar);


                        listaDiasAplicados.RemoveAt(diaTirarExtra);
                        listaDias.RemoveAt(diaColocar);

                        diasSemExtra--;
                    }
                }
            }
            else
            {
                var totalTentativas = 0;
                var diasTrabalho = new List<int>();
                var quantidadeExtrasNaoAplicadas = 0;
                for (var index = 0; index < dgvDados.Rows.Count; index++)
                {
                    if (!dgvDados.Rows[index].Cells["Entrada"].Value.Equals("-"))
                    {
                        diasTrabalho.Add(index + 1);

                        if (dgvDados.Rows[index].Cells["HoraExtra"].Value.Equals("-"))
                            quantidadeExtrasNaoAplicadas++;
                    }
                }

                while (quantidadeExtrasNaoAplicadas < 7)
                {
                    totalTentativas = 0;

                    var diaTirarExtra = new Random().Next(1, diasTrabalho.Count);

                    Thread.Sleep(tempoPausa);
                    var index = diasTrabalho[diaTirarExtra - 1] - 1;

                    var horaExtra = Convert.ToInt32(dgvDados.Rows[index].Cells["HoraExtraMinutos"].Value);

                    if (horaExtra > 0)
                    {
                        dgvDados.Rows[index].Cells["HoraExtraMinutos"].Value = "0";
                        dgvDados.Rows[index].Cells["HoraExtra"].Value = "-";

                        while (horaExtra > 0)
                        {
                            var diaColocarExtra = 0;
                            var indexExtraAdd = 0;
                            var horaExtraAtual = 0;
                            do
                            {
                                diaColocarExtra = new Random().Next(1, diasTrabalho.Count);

                                Thread.Sleep(tempoPausa);

                                indexExtraAdd = diasTrabalho[diaColocarExtra - 1] - 1;
                                horaExtraAtual = Convert.ToInt32(dgvDados.Rows[indexExtraAdd].Cells["HoraExtraMinutos"].Value);

                            } while (diaColocarExtra == diaTirarExtra || horaExtraAtual == 0);

                            var totalHoras = horaExtraAtual + horaExtra;
                            if (totalHoras > 120)
                            {
                                var sobra = totalHoras - 120;

                                horaExtraAtual = totalHoras - sobra;

                                dgvDados.Rows[indexExtraAdd].Cells["HoraExtraMinutos"].Value = horaExtraAtual;
                                dgvDados.Rows[indexExtraAdd].Cells["HoraExtra"].Value = TraduzirMinutosTotal(horaExtraAtual);

                                horaExtra = sobra;
                            }
                            else
                            {
                                horaExtra = 0;

                                dgvDados.Rows[indexExtraAdd].Cells["HoraExtraMinutos"].Value = totalHoras;
                                dgvDados.Rows[indexExtraAdd].Cells["HoraExtra"].Value = TraduzirMinutosTotal(totalHoras);
                            }

                            totalTentativas++;

                            if (totalTentativas >= 60)
                            {
                                dgvDados.Rows[index].Cells["HoraExtraMinutos"].Value = horaExtra;
                                dgvDados.Rows[index].Cells["HoraExtra"].Value = TraduzirMinutosTotal(horaExtra);

                                break;
                            }
                        }

                        quantidadeExtrasNaoAplicadas++;
                    }
                }
            }
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

            return data.AddMinutes(new Random().Next(240, 330));
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

            return horaFinal.AddMinutes(Convert.ToInt32(dgvDados.Rows[index].Cells["HoraExtraMinutos"].Value));
        }

        private string CalcularValorAleatorio(int index, ref int valor, string chave, int min, int max, int valorMax)
        {
            if (!dgvDados.Rows[index].Cells[chave].Value.Equals("-"))
            {
                var possuiHorasExtras = new Random().Next(3);

                if (possuiHorasExtras == 1 || possuiHorasExtras == 2)
                {
                    var horas = new Random().Next(min, max);

                    var valorFinal = Convert.ToInt32(dgvDados.Rows[index].Cells[chave].Value) + horas;

                    if (valorFinal <= valorMax)
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

        private void CalcularValoresAleatorios(int totalHoras, string chaveVisivel, string chaveCalculo, int min, int max, int valorMax, int diasTrabalhados, int diaMaximoMes)
        {
            if (totalHoras > (diasTrabalhados * valorMax * 0.7))
            {
                var maximoDias = totalHoras / valorMax;
                var resto = totalHoras - (maximoDias * valorMax);

                var diasSem = diasTrabalhados - maximoDias;

                if (resto > 0)
                    diasSem--;

                var listaDiasSem = new List<int>(diasSem);

                while (diasSem > 0)
                {
                    var diaEscolhido = 0;
                    do
                    {
                        diaEscolhido = new Random().Next(1, diaMaximoMes);

                        Thread.Sleep(tempoPausa);
                    } while (dgvDados.Rows[diaEscolhido - 1].Cells["Entrada"].Value.Equals("-")
                    && ((chaveVisivel.Equals("TempoEspera") && dgvDados.Rows[diaEscolhido - 1].Cells["Parada"].Value.Equals("1")) || chaveVisivel.Equals("HoraExtra")));

                    listaDiasSem.Add(diaEscolhido);

                    diasSem--;
                }

                var x = 0;
                for (; x < diaMaximoMes && maximoDias > 0; x++)
                {
                    if (!dgvDados.Rows[x].Cells["Entrada"].Value.Equals("-") && !listaDiasSem.Exists(s => s == (x + 1)))
                    {
                        dgvDados.Rows[x].Cells[chaveCalculo].Value = valorMax;
                        dgvDados.Rows[x].Cells[chaveVisivel].Value = TraduzirMinutosTotal(valorMax);

                        maximoDias--;
                    }
                }

                if (resto > 0)
                {
                    for (; x < diaMaximoMes; x++)
                    {
                        if (!dgvDados.Rows[x].Cells["Entrada"].Value.Equals("-"))
                        {
                            dgvDados.Rows[x].Cells[chaveCalculo].Value = resto;
                            dgvDados.Rows[x].Cells[chaveVisivel].Value = TraduzirMinutosTotal(resto);

                            break;
                        }
                    }
                }

            }
            else
            {
                var index = 0;
                var rodadas = 0;
                while (totalHoras > 0)
                {

                    if (index == dgvDados.Rows.Count)
                    {
                        index = 0;
                        rodadas++;

                        if (rodadas >= 3)
                        {
                            max = Convert.ToInt32(Math.Ceiling((decimal)totalHoras / 7));

                            if (max > 120)
                                max = 120;

                            rodadas = 0;
                        }
                    }

                    if (!dgvDados.Rows[index].Cells["Entrada"].Value.Equals("-") && Convert.ToInt32(dgvDados.Rows[index].Cells[chaveCalculo].Value) < valorMax
                         && ((chaveVisivel.Equals("TempoEspera") && dgvDados.Rows[index].Cells["Parada"].Value.Equals("-")) || chaveVisivel.Equals("HoraExtra")))
                    {
                        dgvDados.Rows[index].Cells[chaveCalculo].Value = CalcularValorAleatorio(index, ref totalHoras, chaveCalculo, min, max, valorMax);
                        dgvDados.Rows[index].Cells[chaveVisivel].Value = TraduzirMinutos(index, chaveCalculo);
                    }

                    index++;

                    Thread.Sleep(tempoPausa);
                }
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

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNomeArquivo.Text))
            {
                using (var folderBrowserDialog = new FolderBrowserDialog())
                {
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        Exportar(folderBrowserDialog.SelectedPath);
                    }

                }
            }
            else { MessageBox.Show("O nome do arquivo deve ser informado"); }
        }

        private void Exportar(string diretorio)
        {
            FileInfo newFile = new FileInfo(@"" + diretorio + "\\" + txtNomeArquivo.Text + ".xlsx");
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(@"" + diretorio + "\\" + txtNomeArquivo.Text + ".xlsx");

            }

            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Planilha");

                worksheet.Cells.Style.WrapText = true;
                worksheet.View.ShowGridLines = false;//Remove the grid lines of the sheet

                worksheet.Cells["A1:H1"].Merge = true;

                DateTime dateValue = new DateTime(dtpReferencia.Value.Year, dtpReferencia.Value.Month, dtpReferencia.Value.Day);

                worksheet.Cells[1, 1].Value = dateValue.ToString("MMMM", new CultureInfo("pt-BR")).ToUpper();

                worksheet.Cells["A2:b2"].Merge = true;

                worksheet.Cells[2, 3].Value = "Entrada";
                worksheet.Cells[2, 3].AutoFitColumns(30, 40);

                worksheet.Cells[2, 4].Value = "Intervalo / Parada Obrigatória";
                worksheet.Cells[2, 4].AutoFitColumns(50, 70);

                worksheet.Cells[2, 5].Value = "Saida";
                worksheet.Cells[2, 5].AutoFitColumns(30, 40);

                worksheet.Cells[2, 6].Value = "Tempo de Espera";
                worksheet.Cells[2, 6].AutoFitColumns(30, 40);

                worksheet.Cells[2, 7].Value = "Hora Extra";
                worksheet.Cells[2, 7].AutoFitColumns(30, 40);

                worksheet.Cells[2, 8].Value = "Parada";
                worksheet.Cells[2, 8].AutoFitColumns(30, 40);

                var index = 0;

                for (var coluna = 1; coluna <= 8; coluna++)
                {
                    worksheet.Cells[1, coluna].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.FromArgb(191, 191, 191));
                    worksheet.Cells[2, coluna].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.FromArgb(191, 191, 191));
                }

                for (; index < dgvDados.Rows.Count; index++)
                {
                    var data = new DateTime(dateValue.Year, dateValue.Month, index + 1);
                    worksheet.Cells[index + 3, 1].Value = (index + 1) + "/" + data.ToString("MM", new CultureInfo("pt-BR"));
                    worksheet.Cells[index + 3, 2].Value = data.ToString("dddd", new CultureInfo("pt-BR"));
                    worksheet.Cells[index + 3, 2].AutoFitColumns(30, 40);

                    if (!dgvDados.Rows[index].Cells["Entrada"].Value.Equals("-"))
                    {
                        worksheet.Cells[index + 3, 3].Value = Convert.ToDateTime(dgvDados.Rows[index].Cells["Entrada"].Value).ToString("HH:mm");
                        worksheet.Cells[index + 3, 4].Value = Convert.ToDateTime(dgvDados.Rows[index].Cells["IntervaloInicial"].Value).ToString("HH:mm") + "/" + Convert.ToDateTime(dgvDados.Rows[index].Cells["IntervaloFinal"].Value).ToString("HH:mm");
                        worksheet.Cells[index + 3, 5].Value = Convert.ToDateTime(dgvDados.Rows[index].Cells["Saida"].Value).ToString("HH:mm");

                        worksheet.Cells[index + 3, 6].Value = dgvDados.Rows[index].Cells["TempoEspera"].Value;
                        if (!dgvDados.Rows[index].Cells["TempoEspera"].Value.Equals("-"))
                        {
                            worksheet.Cells[index + 3, 6].Value = dgvDados.Rows[index].Cells["TempoEspera"].Value;
                        }

                        worksheet.Cells[index + 3, 7].Value = dgvDados.Rows[index].Cells["HoraExtra"].Value;
                        if (!dgvDados.Rows[index].Cells["HoraExtra"].Value.Equals("-"))
                        {
                            worksheet.Cells[index + 3, 7].Value = dgvDados.Rows[index].Cells["HoraExtra"].Value;
                        }

                        worksheet.Cells[index + 3, 8].Value = "falso";
                        if (dgvDados.Rows[index].Cells["Parada"].Value.Equals("1"))
                        {
                            worksheet.Cells[index + 3, 8].Value = "verdadeiro";
                        }
                    }
                    else
                    {
                        using (ExcelRange range = worksheet.Cells[index + 3, 1, index + 3, 8])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.Font.Color.SetColor(Color.White);
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(128, 128, 128));
                        }
                    }

                    for (var coluna = 1; coluna <= 8; coluna++)
                        worksheet.Cells[index + 3, coluna].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.FromArgb(191, 191, 191));
                }

                worksheet.Cells[index + 3, 6].Value = txtTotalHoraEspera.Text;
                worksheet.Cells[index + 3, 7].Value = txtTotalHoraExtra.Text;

                using (ExcelRange range = worksheet.Cells[1, 1, index + 3, 8])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                package.Save();
            }

            MessageBox.Show("Exportação finalizada");
        }
    }
}
