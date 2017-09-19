namespace ValidacionArchivosRecibidos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLogColumnNumeroLinea : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "NumeroLinea", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "NumeroLinea");
        }
    }
}
