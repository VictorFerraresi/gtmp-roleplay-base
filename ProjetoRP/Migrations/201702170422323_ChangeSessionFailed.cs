namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeSessionFailed : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sessions", "Failed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sessions", "Failed", c => c.Int(nullable: false));
        }
    }
}
