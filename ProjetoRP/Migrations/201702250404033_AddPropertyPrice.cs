namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertyPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Properties", "Price", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Properties", "Price");
        }
    }
}
