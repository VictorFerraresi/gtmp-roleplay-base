namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDoorToForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Doors", "Property_Id", "Properties");
            DropIndex("Doors", new[] { "Property_Id" });
            AlterColumn("dbo.Doors", "Property_Id", c => c.Int());
            CreateIndex("dbo.Doors", "Property_Id");
            AddForeignKey("dbo.Doors", "Property_Id", "dbo.Properties", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Doors", "Property_Id", "Properties");
            DropIndex("Doors", new[] { "Property_Id" });
            AlterColumn("dbo.Doors", "Property_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Doors", "Property_Id");
            AddForeignKey("dbo.Doors", "Property_Id", "dbo.Properties", "Id", cascadeDelete: true);
        }
    }
}
