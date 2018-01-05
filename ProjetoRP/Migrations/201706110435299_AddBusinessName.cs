namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBusinessName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Businesses", "Name", c => c.String(nullable: false, maxLength: 40, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Businesses", "Name");
        }
    }
}
