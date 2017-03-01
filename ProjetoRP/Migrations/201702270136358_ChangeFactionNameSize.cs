namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeFactionNameSize : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Factions", new[] { "Name" });
            AlterColumn("dbo.Factions", "Name", c => c.String(nullable: false, maxLength: 40, storeType: "nvarchar"));
            CreateIndex("dbo.Factions", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Factions", new[] { "Name" });
            AlterColumn("dbo.Factions", "Name", c => c.String(nullable: false, maxLength: 32, storeType: "nvarchar"));
            CreateIndex("dbo.Factions", "Name", unique: true);
        }
    }
}
