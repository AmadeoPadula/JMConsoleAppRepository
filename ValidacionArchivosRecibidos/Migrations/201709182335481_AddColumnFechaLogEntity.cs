namespace ValidacionArchivosRecibidos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnFechaLogEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "Fecha", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "Fecha");
        }
    }
}
