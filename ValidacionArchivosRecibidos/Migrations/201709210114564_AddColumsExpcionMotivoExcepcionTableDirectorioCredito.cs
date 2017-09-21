namespace ValidacionArchivosRecibidos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumsExpcionMotivoExcepcionTableDirectorioCredito : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DirectoriosCreditos", "Excepcion", c => c.Boolean(nullable: false));
            AddColumn("dbo.DirectoriosCreditos", "MotivoExcepcion", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DirectoriosCreditos", "MotivoExcepcion");
            DropColumn("dbo.DirectoriosCreditos", "Excepcion");
        }
    }
}
