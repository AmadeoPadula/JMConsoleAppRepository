using System;

namespace MigracionCarteraJM.Models
{
    public class BaseCredito
    {
        public int BaseCreditoId { get; set; }
        public string Sucursal { get; set; }
        public string RangoDias { get; set; }
        public string TipoCredito { get; set; }
        public string NumeroCredito { get; set; }
        public int NumeroSocio { get; set; }
        public string Nombre { get; set; }
        public string Curp { get; set; }
        public string ClaveElector { get; set; }
        public decimal Capital { get; set; }
        public decimal MontoAmoritzacionesVencidas { get; set; }
        public int NumeroAmortizacionesVencidas { get; set; }
        public decimal InteresVigente { get; set; }
        public decimal InteresVencido { get; set; }
        public decimal InteresOrdinario { get; set; }
        public decimal SaldoMoratorio { get; set; }
        public decimal MontoDesembolsado { get; set; }
        public decimal TasaAnual { get; set; }
        public decimal TasaMensual { get; set; }
        public string ModalidadPago { get; set; }
        public int NumeroCuotas { get; set; }
        public DateTime FechaDesembolso { get; set; }
        public DateTime FechaVencimento { get; set; }
        public DateTime? FechaUltimoPagoCapital { get; set; }
        public decimal Reciprocidad { get; set; }
        public decimal SaldoCuentaAhorro { get; set; }
        public decimal SaldoInversiones { get; set; }
        public int VecesRecicle { get; set; }
        public string Producto { get; set; }
        public string NombreProducto { get; set; }
        public int Grupo { get; set; }
        public string NombreGrupo { get; set; }
        public string Direccion { get; set; }
        public int DiasAtraso { get; set; }
        public decimal CapitalExigible { get; set; }
        public decimal Saldo { get; set; }
        public DateTime? FechaPrimerCuotaNoPagada { get; set; }
        public string DestinoCredito { get; set; }
        public string Sexo { get; set; }
        public decimal Ingresos { get; set; }
        public decimal Egresos { get; set; }
        public int BandaMorosidad { get; set; }
        public string Municipio { get; set; }
        public int NumeroHabitantes { get; set; }
        public string ZonaMarginal { get; set; }
        public string Estado { get; set; }

    } // public class BaseCredito
} // namespace MigracionCarteraJM.Models
