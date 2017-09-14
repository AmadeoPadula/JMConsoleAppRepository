using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ValidacionArchivosRecibidos
{
    public class ValidacionContext : DbContext
    {
        public ValidacionContext()
        {
            Database.Connection.ConnectionString = ConfigurationManager.ConnectionStrings["LocalCnnValidaciones"].ConnectionString;
        }

        public DbSet<DirectorioCredito> DirectoriosCreditos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<DirectorioCredito>().ToTable("DirectoriosCreditos")
                .HasKey(d => d.DirectorioCreditoId);

        }
    }
}
