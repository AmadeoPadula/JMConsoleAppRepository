using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ValidacionArchivosRecibidos.Models
{
    public class ValidacionContext : DbContext
    {
        public ValidacionContext()
        {
            Database.Connection.ConnectionString = ConfigurationManager.ConnectionStrings["LocalCnnValidaciones"].ConnectionString;
        }

        public DbSet<DirectorioCredito> DirectoriosCreditos { get; set; }
        public DbSet<TablaAmortizacion> TablasAmortizacion { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<Movimiento> Movimentos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<DirectorioCredito>().ToTable("DirectoriosCreditos")
                .HasKey(d => d.DirectorioCreditoId);

            modelBuilder.Entity<TablaAmortizacion>().ToTable("TablasAmortizacion")
                .HasKey(d => d.TablaAmortizacionId);

            modelBuilder.Entity<Log>().ToTable("Logs")
                .HasKey(d => d.LogId);

            modelBuilder.Entity<Movimiento>().ToTable("Movimientos")
                .HasKey(d => d.MovimientoId);

        }
    }
}
