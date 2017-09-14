namespace ValidacionArchivosRecibidos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnDirectorioCredito_Ruta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DirectoriosCreditos", "Ruta", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DirectoriosCreditos", "Ruta");
        }
    }
}
