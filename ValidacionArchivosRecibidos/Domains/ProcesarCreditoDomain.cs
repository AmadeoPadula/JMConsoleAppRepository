using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Spire.Xls;
using ValidacionArchivosRecibidos.Clases;
using ValidacionArchivosRecibidos.Models;

namespace ValidacionArchivosRecibidos.Domains
{
    public class ProcesarCreditoDomain
    {
        public ProcesarCreditoDomain()
        {
            DbContext = new ValidacionContext();
        }

        private ValidacionContext DbContext;
        private const string CadenaDeConexionExcel = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;';";


        public void ProcesarArchivosCredito(List<DirectorioCredito> informacionCredito)
        {

            //Si no existe tabla de amortizacion no se puede procesar
            var existeTablaAmortizacion = informacionCredito.Any(ic => ic.Archivo == "tab.xls");
            if (!existeTablaAmortizacion) return;

            //Leer archivo de Tabla de pagos
            var tablaAmortizacion = informacionCredito.First(ic => ic.Archivo == "tab.xls");

            if (tablaAmortizacion != null)
            {
                if (!tablaAmortizacion.Excepcion)
                    ExtraerTablaAmortizacion(tablaAmortizacion);
            } // if (tablaPagos != null)

            var historicoPagos = informacionCredito.First(ic => ic.Archivo == "hist.xls");
            var tablaMovimientos = informacionCredito.First(ic => ic.Archivo == "mov.xls");

            if (historicoPagos != null && tablaMovimientos != null)
            {
                if (!tablaMovimientos.Excepcion)
                    ExtraerTablaMovimientos(tablaMovimientos);

                if (!historicoPagos.Excepcion)
                    ExtraerHistoricoPagos(historicoPagos);
            } // if (historicoPagos != null && tablaMovimientos != null)

        } // public void ProcesarArchivosCredito(List<DirectorioCredito> informacionCredito)

        private void ExtraerHistoricoPagos(DirectorioCredito tablaHistoricoPagos)
        {
            var pathHistoricoPagos = tablaHistoricoPagos.Ruta;
            var directorioCreditoId = tablaHistoricoPagos.DirectorioCreditoId;
            var creditoId = tablaHistoricoPagos.CreditoId;

            //Validar si el archivo ya fue previamente procesado
            var procesado = DbContext.DirectoriosCreditos.FirstOrDefault(dc => dc.DirectorioCreditoId == directorioCreditoId).Procesado;

            if (!procesado)
            {
                //Editar archivo:
                var workbook = new Workbook();
                workbook.LoadFromFile(pathHistoricoPagos);
                var sheet = workbook.Worksheets[0];


                //Eliminar filas Encabezado
                UtileriasClass.DeleteRows(sheet, 1);
                workbook.SaveToFile(pathHistoricoPagos);
            }

            var stringConexionExcel = string.Format(CadenaDeConexionExcel, pathHistoricoPagos); //Valor Yes or No depende de si archivo Excel tiene header o no

            OleDbConnection connExcel = new OleDbConnection(stringConexionExcel);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dataTable = new DataTable();
            cmdExcel.Connection = connExcel;

            try
            {
                //Obten la primera página del archivo Excel
                connExcel.Open();
                DataTable dtExcelSchema;

                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                string SheetName = dtExcelSchema.Rows[1]["TABLE_NAME"].ToString();

                //Leer la información de la primera página
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dataTable);


                //Validar que existan las columnas en el archivo origen
                var columnas = dataTable.Columns;
                var logFaltaColumna = new Log
                {
                    DirectorioCreditoId = directorioCreditoId
                };

                var columnasBuscadas = new[]
                {
                    "Cuota",
                    "Fecha",
                    "Total",
                    "Cargos",
                    "Principal",
                    "Int# Vigente",
                    "Int# Vencido",
                    "Int# Orden",
                    "CPA",
                    "Moratorios",
                    "IVA"
                };


                foreach (var columnaActual in columnasBuscadas)
                {
                    if (columnas.Contains(columnaActual)) continue;

                    logFaltaColumna.Descripcion = columnaActual;
                    InsertarRegistroBitacora(logFaltaColumna);
                    return;
                } // foreach (var columnaActual in columnasBuscadas)

                var historicoPagos = new List<HistoricoPago>();

                var numeroLinea = 1;

                foreach (DataRow renglonDataRow in dataTable.Rows)
                {
                    //Si aparece la palabra "Cuota" se brinca la fila
                    var cuota = renglonDataRow[columnasBuscadas[0]].ToString();

                    if (cuota.Contains("Cuota")) continue;
                    if (cuota.Contains("Totales:")) break;

                    if (string.IsNullOrEmpty(cuota))
                    {
                        numeroLinea++;
                        continue;
                    }


                    var historicoPago = new HistoricoPago();

                    //NumeroCredito
                    historicoPago.NumeroCredito = creditoId;


                    //Cuota
                    if (!int.TryParse(renglonDataRow[columnasBuscadas[0]].ToString(), out int cuotaNumero))
                    {
                        InsertarRegistroBitacora(new Log
                        {
                            DirectorioCreditoId = directorioCreditoId,
                            Descripcion = string.Format(Log.ErrorCast, "Cuota"),
                            NumeroLinea = numeroLinea
                        });

                        numeroLinea++;
                        continue;
                    }

                    historicoPago.Cuota = cuotaNumero;

                    //Fecha
                    var fecha = UtileriasClass.ConvertirCadenaAFecha(renglonDataRow[columnasBuscadas[1]].ToString());
                    if (fecha == null)
                    {
                        InsertarRegistroBitacora(new Log
                        {
                            DirectorioCreditoId = directorioCreditoId,
                            Descripcion = string.Format(Log.ErrorCast, "Fecha"),
                            NumeroLinea = numeroLinea

                        });

                        numeroLinea++;
                        continue;
                    }

                    historicoPago.Fecha = (DateTime)fecha;

                    //Total
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[2]].ToString(), out decimal total))
                    {
                        InsertarRegistroBitacora(new Log
                        {
                            DirectorioCreditoId = directorioCreditoId,
                            Descripcion = string.Format(Log.ErrorCast, "Total"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.Total = total;

                    //Cargos
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[3]].ToString(), out decimal cargos))
                    {
                        InsertarRegistroBitacora(new Log
                        {
                            DirectorioCreditoId = directorioCreditoId,
                            Descripcion = string.Format(Log.ErrorCast, "Cargos"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.Cargos = cargos;

                    //Principal
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[4]].ToString(), out decimal principal))
                    {
                        InsertarRegistroBitacora(new Log
                        {
                            DirectorioCreditoId = directorioCreditoId,
                            Descripcion = string.Format(Log.ErrorCast, "Principal"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.Principal = principal;


                    //InteresVigente
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[5]].ToString(), out decimal interesVigente))
                    {
                        InsertarRegistroBitacora(new Log
                        {
                            DirectorioCreditoId = directorioCreditoId,
                            Descripcion = string.Format(Log.ErrorCast, "InteresVigente"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.InteresVigente = interesVigente;


                    //InteresVencido
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[6]].ToString(), out decimal interesVencido))
                    {
                        InsertarRegistroBitacora(new Log
                        {
                            DirectorioCreditoId = directorioCreditoId,
                            Descripcion = string.Format(Log.ErrorCast, "InteresVencido"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.InteresVencido = interesVencido;

                    //InteresOrdinario
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[7]].ToString(), out decimal interesOrdinario))
                    {
                        InsertarRegistroBitacora(new Log
                        {
                            DirectorioCreditoId = directorioCreditoId,
                            Descripcion = string.Format(Log.ErrorCast, "InteresOrdinario"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.InteresOrdinario = interesOrdinario;

                    //Cpa
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[8]].ToString(), out decimal cpa))
                    {
                        InsertarRegistroBitacora(new Log
                        {
                            DirectorioCreditoId = directorioCreditoId,
                            Descripcion = string.Format(Log.ErrorCast, "Cpa"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.Cpa = cpa;

                    //Moratorios
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[9]].ToString(), out decimal moratorios))
                    {
                        InsertarRegistroBitacora(new Log
                        {
                            DirectorioCreditoId = directorioCreditoId,
                            Descripcion = string.Format(Log.ErrorCast, "Moratorios"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.Moratorios = moratorios;

                    //Iva
                    if (!decimal.TryParse(renglonDataRow[columnasBuscadas[10]].ToString(), out decimal iva))
                    {
                        InsertarRegistroBitacora(new Log
                        {
                            DirectorioCreditoId = directorioCreditoId,
                            Descripcion = string.Format(Log.ErrorCast, "Iva"),
                            NumeroLinea = numeroLinea
                        });
                    }

                    historicoPago.Iva = iva;


                    //Agregar historico pagos
                    historicoPagos.Add(historicoPago);

                    numeroLinea += 1;
                }

                //Insertar en Base de Datos
                DbContext.HistoricoPagos.AddRange(historicoPagos);
                DbContext.SaveChanges();

                if (!procesado)
                {
                    var directorioCreditoBaseDatos = DbContext.DirectoriosCreditos.FirstOrDefault(dc => dc.DirectorioCreditoId == directorioCreditoId);
                    directorioCreditoBaseDatos.Procesado = true;
                    directorioCreditoBaseDatos.FechaProcesado = DateTime.Now;

                    DbContext.SaveChanges();
                }

                Console.WriteLine($@"Fin de importacion Historico de Movimientos, Crédito {creditoId}");
            } // try
            catch (Exception eCargar)
            {
                throw eCargar;
            } // catch (Exception eCargar)
            finally
            {
                // Cierra la conexión
                connExcel.Close();
            } // finally
        } // private void ExtraerHistoricoPagos(DirectorioCredito tablaHistoricoPagos, int directorioCreditoId, int creditoId)

        private void ExtraerTablaAmortizacion(DirectorioCredito tablaAmortizacion)
        {
            var pathTablaPagos = tablaAmortizacion.Ruta;
            var directorioCreditoId = tablaAmortizacion.DirectorioCreditoId;
            var creditoId = tablaAmortizacion.CreditoId;

            TipoArchivo.TipoArchivoEnum tipoArchivo;

            //Determinar el tipo de formato del archivo:
            var workbook = new Workbook();
            workbook.LoadFromFile(pathTablaPagos);
            var sheet = workbook.Worksheets[0];
            var infomacionCredito = sheet.Range["B11"].Value;

            //Validar si el archivo ya fue previamente procesado
            var procesado = DbContext.DirectoriosCreditos.FirstOrDefault(dc => dc.DirectorioCreditoId == directorioCreditoId).Procesado;

            tipoArchivo = infomacionCredito.Contains("Crédito Folio:") ? TipoArchivo.TipoArchivoEnum.ConFormato : TipoArchivo.TipoArchivoEnum.SinFormato;

            if (tipoArchivo == TipoArchivo.TipoArchivoEnum.ConFormato)
            {
                if (!int.TryParse(infomacionCredito.Replace("Crédito Folio:", ""), out int numeroCreditoArchivo))
                {
                    throw new Exception("El numero de credito contenido en el archivo no coincide con el archivo procesado: " + pathTablaPagos);
                } // if (!int.TryParse(infomacionCredito.Replace("Crédito Folio:",""), out int numeroCreditoArchivo))
            }

            if (tipoArchivo == TipoArchivo.TipoArchivoEnum.ConFormato)
            {
                if (!procesado)
                {
                    //Descombinar celdas
                    UtileriasClass.UnMergeWorksheet(sheet);

                    //Eliminar filas Encabezado
                    UtileriasClass.DeleteRows(sheet, 1, 22);

                    //Eliminar intermedio entre encabezado y tabla con valor
                    UtileriasClass.DeleteRows(sheet, 2);
                    workbook.SaveToFile(pathTablaPagos);
                }
            } // if (tipoArchivo == TipoArchivo.TipoArchivoEnum.ConFormato)

            var stringConexionExcel = string.Format(CadenaDeConexionExcel, pathTablaPagos); //Valor Yes or No depende de si archivo Excel tiene header o no

            OleDbConnection connExcel = new OleDbConnection(stringConexionExcel);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dataTable = new DataTable();
            cmdExcel.Connection = connExcel;

            try
            {
                //Obten la primera página del archivo Excel
                connExcel.Open();
                DataTable dtExcelSchema;

                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                var SheetName = dtExcelSchema.Rows[
                    tipoArchivo == TipoArchivo.TipoArchivoEnum.SinFormato
                        ? 0
                        : 1
                ]["TABLE_NAME"].ToString();

                //Leer la información de la primera página
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dataTable);


                //Validar que existan las columnas en el archivo origen
                var columnas = dataTable.Columns;
                var logFaltaColumna = new Log
                {
                    DirectorioCreditoId = directorioCreditoId
                };

                var columnasBuscadas = tipoArchivo == TipoArchivo.TipoArchivoEnum.SinFormato
                    ? new[]
                    {
                        "Numero de Pago",
                        "Fecha de Pago    ",
                        "Capital",
                        "Pago Capital",
                        "Pago Intereses Moratorios",
                        "Pago Intereses Ordinarios",
                        "Pago IVA Intereses",
                        "Pago Mensual Total",
                        "Pago Fijo Mensual"
                    }
                    : new[]
                    {
                        "Numero de Pago",
                        "Fecha de Pago",
                        "Capital",
                        "Pago Capital",
                        "Pago Intereses Moratorios",
                        "Pago Intereses ",
                        "Pago IVA Intereses",
                        "Pago Mensual Total",
                        "Pago Fijo Mensual"
                    };



                foreach (var columnaActual in columnasBuscadas)
                {
                    if (columnas.Contains(columnaActual)) continue;

                    logFaltaColumna.Descripcion = columnaActual;
                    InsertarRegistroBitacora(logFaltaColumna);
                    return;
                } // foreach (var columnaActual in columnasBuscadas)

                var tablaAmortizacionLista = new List<TablaAmortizacion>();

                var numeroLinea = 1;
                foreach (DataRow renglonDataRow in dataTable.Rows)
                {

                    //Es menor al ultimo reglon?
                    if (numeroLinea < dataTable.Rows.Count)
                    {
                        var tablaAmorizacion = new TablaAmortizacion();

                        //NumeroCredito
                        tablaAmorizacion.NumeroCredito = creditoId;


                        //NumeroPago
                        if (!int.TryParse(renglonDataRow[columnasBuscadas[0]].ToString(), out int numeroPago))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "NumeroCredito"),
                                NumeroLinea = numeroLinea
                            });

                            numeroLinea++;
                            continue;
                        }

                        tablaAmorizacion.NumeroPago = numeroPago;

                        //FechaPago
                        var fechaPago = UtileriasClass.ConvertirCadenaAFecha(renglonDataRow[columnasBuscadas[1]].ToString());
                        if (fechaPago == null)
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "FechaPago"),
                                NumeroLinea = numeroLinea

                            });

                            numeroLinea++;
                            continue;
                        }

                        tablaAmorizacion.FechaPago = (DateTime)fechaPago;

                        //Capital
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[2]].ToString(), out decimal capital))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Capital"),
                                NumeroLinea = numeroLinea

                            });

                            numeroLinea++;
                            continue;
                        }

                        tablaAmorizacion.Capital = capital;

                        //PagoCapital
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[3]].ToString(), out decimal pagoCapital))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "PagoCapital"),
                                NumeroLinea = numeroLinea

                            });
                        }

                        tablaAmorizacion.PagoCapital = pagoCapital;

                        //PagoInteresesMoratorios
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[4]].ToString(), out decimal pagoInteresesMoratorios))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "PagoInteresesMoratorios"),
                                NumeroLinea = numeroLinea

                            });
                        }

                        tablaAmorizacion.PagoInteresesMoratorios = pagoInteresesMoratorios;


                        //PagoInteresesOrdinarios
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[5]].ToString(), out decimal pagoInteresesOrdinarios))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "PagoInteresesOrdinarios"),
                                NumeroLinea = numeroLinea

                            });
                        }

                        tablaAmorizacion.PagoInteresesOrdinarios = pagoInteresesOrdinarios;

                        //PagoIvaIntereses
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[6]].ToString(), out decimal pagoIvaIntereses))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "PagoIvaIntereses"),
                                NumeroLinea = numeroLinea

                            });
                        }

                        tablaAmorizacion.PagoIvaIntereses = pagoIvaIntereses;

                        //PagoMensualTotal
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[7]].ToString(), out decimal pagoMensualTotal))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "PagoMensualTotal"),
                                NumeroLinea = numeroLinea

                            });
                        }

                        tablaAmorizacion.PagoMensualTotal = pagoMensualTotal;

                        //PagoFijoMensual
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[8]].ToString(), out decimal pagoFijoMensual))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "PagoFijoMensual"),
                                NumeroLinea = numeroLinea

                            });
                        }

                        tablaAmorizacion.PagoFijoMensual = pagoFijoMensual;


                        //Agregar tabla de amortización
                        tablaAmortizacionLista.Add(tablaAmorizacion);

                        numeroLinea += 1;

                    }
                    else
                    {
                        //TODO: Se puede hacer algo con el ultimo renglon ???
                    }

                }

                //Insertar en Base de Datos
                DbContext.TablasAmortizacion.AddRange(tablaAmortizacionLista);
                DbContext.SaveChanges();

                if (!procesado)
                {
                    var directorioCreditoBaseDatos = DbContext.DirectoriosCreditos.FirstOrDefault(dc => dc.DirectorioCreditoId == directorioCreditoId);
                    directorioCreditoBaseDatos.Procesado = true;
                    directorioCreditoBaseDatos.FechaProcesado = DateTime.Now;

                    DbContext.SaveChanges();
                }

                Console.WriteLine($@"Fin de importacion Tabla de Amortización, Crédito {creditoId}");

            } // try

            catch (Exception eCargar)
            {
                throw eCargar;

            } // catch (Exception eCargar)

            finally
            {
                // Cierra la conexión
                connExcel.Close();
            } // finally
        } // private void ExtraerTablaAmortizacion(DirectorioCredito tablaMovimientos, int directorioCreditoId, int creditoId)

        private void ExtraerTablaMovimientos(DirectorioCredito tablaMovimientos)
        {
            var pathTablaMovimientos = tablaMovimientos.Ruta;
            var directorioCreditoId = tablaMovimientos.DirectorioCreditoId;
            var creditoId = tablaMovimientos.CreditoId;

            //Validar si el archivo ya fue previamente procesado
            var procesado = DbContext.DirectoriosCreditos.FirstOrDefault(dc => dc.DirectorioCreditoId == directorioCreditoId).Procesado;

            var stringConexionExcel = string.Format(CadenaDeConexionExcel, pathTablaMovimientos); //Valor Yes or No depende de si archivo Excel tiene header o no

            OleDbConnection connExcel = new OleDbConnection(stringConexionExcel);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dataTable = new DataTable();
            cmdExcel.Connection = connExcel;

            try
            {
                //Obten la primera página del archivo Excel
                connExcel.Open();
                DataTable dtExcelSchema;

                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

                //Leer la información de la primera página
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dataTable);


                //Validar que existan las columnas en el archivo origen
                var columnas = dataTable.Columns;
                var logFaltaColumna = new Log
                {
                    DirectorioCreditoId = directorioCreditoId
                };

                var columnasBuscadas = new[]
                {
                    "Fecha",
                    "Descripcion",
                    "Capital",
                    "Cargos",
                    "Interes",
                    "Moratorios",
                    "Iva",
                    "Otros",
                    "Total"
                };


                foreach (var columnaActual in columnasBuscadas)
                {
                    if (columnas.Contains(columnaActual)) continue;

                    logFaltaColumna.Descripcion = columnaActual;
                    InsertarRegistroBitacora(logFaltaColumna);
                    return;
                } // foreach (var columnaActual in columnasBuscadas)

                var movimientos = new List<Movimiento>();

                var numeroLinea = 1;
                foreach (DataRow renglonDataRow in dataTable.Rows)
                {

                    //El archivo esta vacio???
                    if (dataTable.Rows.Count == 2 && numeroLinea == 1)
                    {
                        if (string.IsNullOrEmpty(renglonDataRow[columnasBuscadas[0]].ToString()))
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = Log.ErrorArchivoVacio,
                                NumeroLinea = null
                            });
                        break;
                    }

                    //Es menor al ultimo reglon?
                    if (numeroLinea < dataTable.Rows.Count)
                    {
                        var movimiento = new Movimiento();

                        //NumeroCredito
                        movimiento.NumeroCredito = creditoId;


                        //Fecha
                        var fecha = UtileriasClass.ConvertirCadenaAFecha(renglonDataRow[columnasBuscadas[0]].ToString());
                        if (fecha == null)
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Fecha"),
                                NumeroLinea = numeroLinea
                            });
                            numeroLinea++;
                            continue;
                        }

                        movimiento.Fecha = (DateTime) fecha;

                        //Descripcion
                        var descripcion = renglonDataRow[columnasBuscadas[1]].ToString();
                        movimiento.Descripcion = string.IsNullOrEmpty(descripcion) ? null : descripcion;

                        //Capital
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[2]].ToString(), out decimal capital))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Capital"),
                                NumeroLinea = numeroLinea
                            });
                            numeroLinea++;
                            continue;
                        }

                        movimiento.Capital = capital;

                        //Cargos
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[3]].ToString(), out decimal cargos))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Cargos"),
                                NumeroLinea = numeroLinea
                            });
                        }

                        movimiento.Cargos = cargos;

                        //Interes
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[4]].ToString(), out decimal interes))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Interes"),
                                NumeroLinea = numeroLinea
                            });
                        }

                        movimiento.Interes = interes;


                        //Moratorios
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[5]].ToString(), out decimal moratorios))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Moratorios"),
                                NumeroLinea = numeroLinea
                            });
                        }

                        movimiento.Moratorios = moratorios;

                        //Iva
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[6]].ToString(), out decimal iva))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Iva"),
                                NumeroLinea = numeroLinea
                            });
                        }

                        movimiento.Iva = iva;

                        //Otros
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[7]].ToString(), out decimal otros))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Otros"),
                                NumeroLinea = numeroLinea
                            });
                        }

                        movimiento.Otros = otros;

                        //Total
                        if (!decimal.TryParse(renglonDataRow[columnasBuscadas[8]].ToString(), out decimal total))
                        {
                            InsertarRegistroBitacora(new Log
                            {
                                DirectorioCreditoId = directorioCreditoId,
                                Descripcion = string.Format(Log.ErrorCast, "Total"),
                                NumeroLinea = numeroLinea
                            });
                        }

                        movimiento.Total = total;


                        //Agregar tabla de amortización
                        movimientos.Add(movimiento);

                        numeroLinea += 1;
                    }
                    else
                    {
                        //TODO: Se puede hacer algo con el ultimo renglon ???
                    }
                }

                //Insertar en Base de Datos
                DbContext.Movimentos.AddRange(movimientos);
                DbContext.SaveChanges();

                if (!procesado)
                {
                    var directorioCreditoBaseDatos = DbContext.DirectoriosCreditos.FirstOrDefault(dc => dc.DirectorioCreditoId == directorioCreditoId);
                    directorioCreditoBaseDatos.Procesado = true;
                    directorioCreditoBaseDatos.FechaProcesado = DateTime.Now;

                    DbContext.SaveChanges();
                }


                Console.WriteLine($@"Fin de importacion Tabla de Movimientos, Crédito {creditoId}");

            } // try

            catch (Exception eCargar)
            {
                throw eCargar;
            } // catch (Exception eCargar)

            finally
            {
                // Cierra la conexión
                connExcel.Close();
            } // finally
        } // private void ExtraerTablaMovimientos(DirectorioCredito tablaMovimientos, int directorioCreditoId, int creditoId)

        public void InsertarRegistroBitacora(Log log)
        {
            DbContext.Logs.Add(log);
            DbContext.SaveChanges();
        } // public void InsertarRegistroBitacora(Log log)

    } // public class ProcesarCreditoDomain
} // namespace ValidacionArchivosRecibidos.Domains
