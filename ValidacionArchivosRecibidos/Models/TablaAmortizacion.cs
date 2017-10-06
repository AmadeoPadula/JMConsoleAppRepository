using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidacionArchivosRecibidos.Models
{
    public class TablaAmortizacion
    {
        public int TablaAmortizacionId { get; set; }
        public int NumeroCredito { get; set; }
        public int NumeroPago { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Capital { get; set; }
        public decimal PagoCapital { get; set; }
        public decimal PagoInteresesMoratorios { get; set; }
        public decimal PagoInteresesOrdinarios { get; set; }
        public decimal PagoIvaIntereses { get; set; }
        public decimal PagoMensualTotal { get; set; }
        public decimal PagoFijoMensual { get; set; }
        public DateTime? FechaAlta { get; set; } = DateTime.Now;

    } // public class TablaAmortizacion
} // public class TablaAmortizacion
