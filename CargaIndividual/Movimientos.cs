//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CargaIndividual
{
    using System;
    using System.Collections.Generic;
    
    public partial class Movimientos
    {
        public int MovimientoId { get; set; }
        public int NumeroCredito { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public decimal Capital { get; set; }
        public decimal Cargos { get; set; }
        public decimal Interes { get; set; }
        public decimal Moratorios { get; set; }
        public decimal Iva { get; set; }
        public decimal Otros { get; set; }
        public decimal Total { get; set; }
        public Nullable<System.DateTime> FechaAlta { get; set; }
    }
}
