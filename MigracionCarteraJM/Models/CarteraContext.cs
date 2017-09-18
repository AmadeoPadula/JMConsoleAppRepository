using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracionCarteraJM.Models
{
    public class CarteraContext:DbContext
    {

        public CarteraContext()
        {
            Database.Connection.ConnectionString = ConfigurationManager.ConnectionStrings["LocalCnn"].ConnectionString;
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Maestro> Maestros { get; set; }
        public DbSet<BaseCredito> BaseCreditos { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Cliente>().ToTable("Clientes")
                .HasKey(d => d.ClienteId);

            modelBuilder.Entity<Maestro>().ToTable("Maestros")
                .HasKey(d => d.MaestroId);

            modelBuilder.Entity<BaseCredito>().ToTable("BaseCreditos")
                .HasKey(d => d.BaseCreditoId);
        } // protected override void OnModelCreating(DbModelBuilder modelBuilder)
    } // public class CarteraContext:DbContext

} // namespace MigracionCarteraJM.Models
