namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBusinessType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Businesses", "BusinessType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Businesses", "BusinessType");
        }
    }
}
