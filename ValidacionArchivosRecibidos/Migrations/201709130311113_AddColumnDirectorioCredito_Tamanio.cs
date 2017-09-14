namespace ValidacionArchivosRecibidos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnDirectorioCredito_Tamanio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DirectoriosCreditos", "Tamanio", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DirectoriosCreditos", "Tamanio");
        }
    }
}
