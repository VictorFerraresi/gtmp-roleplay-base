namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBusinessType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Businesses", "BizType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Businesses", "BizType");
        }
    }
}
