using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracionCarteraJM.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public int NumeroCliente { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
    }
}
