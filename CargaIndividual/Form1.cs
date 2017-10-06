using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CargaIndividual.Clases;
using CargaIndividual.Domains;

namespace CargaIndividual
{
    public partial class CargaIndividualForm : Form
    {

        public enum TiposArchivo
        {
            TablaAmortizacion = 1,
            Movimientos = 2,
            HistoricoPagos = 3
        } // public enum TiposArchivo

        public CargaIndividualForm()
        {
            InitializeComponent();
        }

        private void ExaminarButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.SeleccionarArchivoOpenFileDialog.ShowDialog() == DialogResult.OK)
                {

                    if (SeleccionarArchivoOpenFileDialog.SafeFileName == null) return;

                    this.NombreArchivoTextBox.Text = SeleccionarArchivoOpenFileDialog.FileName;

                    var nombreArchivo = SeleccionarArchivoOpenFileDialog.SafeFileName.ToLower();

                    var tipoArchivo = nombreArchivo == "tab.xls" ? TiposArchivo.TablaAmortizacion : (nombreArchivo == "mov.xls") ? TiposArchivo.Movimientos : (nombreArchivo == "hist.xls") ? TiposArchivo.HistoricoPagos : 0;

                    TablaAmortizacionCheckBox.Checked = tipoArchivo == TiposArchivo.TablaAmortizacion;
                    MovimientosCheckBox.Checked = tipoArchivo == TiposArchivo.Movimientos;
                    HistoricoPagosCheckBox.Checked = tipoArchivo == TiposArchivo.HistoricoPagos;

                    var rutaArchivo = Path.GetDirectoryName(SeleccionarArchivoOpenFileDialog.FileName).Split('\\');

                    var numeroCreditoTmp = rutaArchivo[rutaArchivo.Length - 1];

                    var numeroCreditoCorrecto = int.TryParse(numeroCreditoTmp, out int numeroCredito);

                    if (numeroCreditoCorrecto && tipoArchivo != 0)
                    {

                        if (tipoArchivo == TiposArchivo.HistoricoPagos | tipoArchivo == TiposArchivo.Movimientos)
                        {
                            bool existeTablaAmortizacion;

                            using (var db = new JMValidacionesDBContext())
                            {

                                existeTablaAmortizacion = db.TablasAmortizacion.Any(ta => ta.NumeroCredito == numeroCredito);
                            } // using (var db = new CarteraContext())

                            if (!existeTablaAmortizacion) DeshabilitarControles($"El crédito {numeroCredito} no tiene tabla de amortización");

                        }

                        this.NumeroCreditoTextBox.Text = numeroCredito.ToString();
                        this.ImportarButton.Enabled = true;
                    }
                    else
                    {
                        DeshabilitarControles();
                    }
                }
                else
                {
                    DeshabilitarControles();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        } // private void ExaminarButton_Click(object sender, EventArgs e)

        private void DeshabilitarControles(string mensaje = "")
        {
            this.NombreArchivoTextBox.Text = string.Empty;
            this.NumeroCreditoTextBox.Text = string.Empty;
            TablaAmortizacionCheckBox.Checked = false;
            MovimientosCheckBox.Checked = false;
            HistoricoPagosCheckBox.Checked = false;

            ImportarButton.Enabled = false;

            if (!string.IsNullOrEmpty(mensaje))
            {
                MessageBox.Show(mensaje, "Importar archivo");
            }

        }

        private void ImportarButton_Click(object sender, EventArgs e)
        {
            try
            {
                if(int.TryParse(NumeroCreditoTextBox.Text, out int numeroCredito))
                {
                    var tipoArchivo = TablaAmortizacionCheckBox.Checked ? TiposArchivo.TablaAmortizacion : HistoricoPagosCheckBox.Checked ? TiposArchivo.HistoricoPagos : MovimientosCheckBox.Checked ? TiposArchivo.Movimientos : 0;
                    var ruta = this.NombreArchivoTextBox.Text.Trim();
                    var primerProcesamiento = PrimerProcesamientoRadioButton.Checked;

                    if (tipoArchivo != 0)
                    {

                        var procesarCreditoDomain = new ProcesarCreditoDomain();
                        var archivoObservaciones = new List<Log>();

                        //Tabla de amortización
                        if (tipoArchivo == TiposArchivo.TablaAmortizacion)
                        {
                            archivoObservaciones = procesarCreditoDomain.ExtraerTablaAmortizacion(numeroCredito, ruta, primerProcesamiento);
                        }

                        //Tabla de movimientos
                        if (tipoArchivo == TiposArchivo.Movimientos)
                        {
                            archivoObservaciones = procesarCreditoDomain.ExtraerTablaMovimientos(numeroCredito, ruta, primerProcesamiento);
                        } // if (tipoArchivo == TiposArchivo.Movimientos)


                        //Historico de Pagos
                        if (tipoArchivo == TiposArchivo.HistoricoPagos)
                        {
                            archivoObservaciones = procesarCreditoDomain.ExtraerHistoricoPagos(numeroCredito, ruta, primerProcesamiento);
                        } // if (tipoArchivo == TiposArchivo.HistoricoPagos)

                        DeshabilitarControles();

                        if (archivoObservaciones.Any())
                        {
                            var logDetalle = new LogDetalle();
                            logDetalle.ListaMensajes = archivoObservaciones;

                            logDetalle.ShowDialog();
                        }
                        else
                        {

                            MessageBox.Show("El archivo ha sido exportado de manera exitosa", "Importar archivo");
                        }

                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }

        } // private void ImportarButton_Click(object sender, EventArgs e)
       
    }
}
