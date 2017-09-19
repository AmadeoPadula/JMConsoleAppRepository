using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidacionArchivosRecibidos.Models
{
    public class Movimiento
    {
        public int MovimientoId { get; set; }
        public int NumeroCredito { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public decimal Capital { get; set; }
        public decimal Cargos { get; set; }
        public decimal Interes { get; set; }
        public decimal Moratorios { get; set; }
        public decimal Iva { get; set; }
        public decimal Otros { get; set; }
        public decimal Total { get; set; }
    }
}
