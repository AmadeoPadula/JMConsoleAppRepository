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


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Cliente>().ToTable("Clientes")
                .HasKey(d => d.ClienteId);
        }
    }
}
