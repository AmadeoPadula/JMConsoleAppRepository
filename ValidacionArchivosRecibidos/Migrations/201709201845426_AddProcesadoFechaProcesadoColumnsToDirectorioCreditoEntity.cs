namespace ValidacionArchivosRecibidos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProcesadoFechaProcesadoColumnsToDirectorioCreditoEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DirectoriosCreditos", "Procesado", c => c.Boolean(nullable: false));
            AddColumn("dbo.DirectoriosCreditos", "FechaProcesado", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DirectoriosCreditos", "FechaProcesado");
            DropColumn("dbo.DirectoriosCreditos", "Procesado");
        }
    }
}
