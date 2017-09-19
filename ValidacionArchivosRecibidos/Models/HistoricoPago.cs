using System;
using System.Data;

namespace ValidacionArchivosRecibidos.Models
{
    public class HistoricoPago
    {
        public int HistoricoPagoId { get; set; }
        public int NumeroCredito { get; set; }
        public int Cuota { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public decimal Cargos { get; set; }
        public decimal Principal { get; set; }
        public decimal InteresVigente { get; set; }
        public decimal InteresVencido { get; set; }
        public decimal InteresOrdinario { get; set; }
        public decimal Cpa { get; set; }
        public decimal Moratorios { get; set; }
        public decimal Iva { get; set; }

    } // public class HistoricoPago
} // namespace ValidacionArchivosRecibidos.Models
