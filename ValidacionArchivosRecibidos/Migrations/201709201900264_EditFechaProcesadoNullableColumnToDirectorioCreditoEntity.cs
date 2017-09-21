namespace ValidacionArchivosRecibidos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditFechaProcesadoNullableColumnToDirectorioCreditoEntity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DirectoriosCreditos", "FechaProcesado", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DirectoriosCreditos", "FechaProcesado", c => c.DateTime(nullable: false));
        }
    }
}
