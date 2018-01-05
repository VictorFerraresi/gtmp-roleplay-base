namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePropertyToForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Businesses", "Owner_Id", "Characters");
            DropForeignKey("Houses", "Owner_Id", "Characters");
            DropIndex("Businesses", new[] { "Owner_Id" });
            DropIndex("Houses", new[] { "Owner_Id" });
            AlterColumn("dbo.Businesses", "Owner_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Houses", "Owner_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Businesses", "Owner_Id");
            CreateIndex("dbo.Houses", "Owner_Id");
            AddForeignKey("dbo.Businesses", "Owner_Id", "dbo.Characters", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Houses", "Owner_Id", "dbo.Characters", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Houses", "Owner_Id", "Characters");
            DropForeignKey("Businesses", "Owner_Id", "Characters");
            DropIndex("Houses", new[] { "Owner_Id" });
            DropIndex("Businesses", new[] { "Owner_Id" });
            AlterColumn("dbo.Houses", "Owner_Id", c => c.Int());
            AlterColumn("dbo.Businesses", "Owner_Id", c => c.Int());
            CreateIndex("dbo.Houses", "Owner_Id");
            CreateIndex("dbo.Businesses", "Owner_Id");
            AddForeignKey("dbo.Houses", "Owner_Id", "dbo.Characters", "Id");
            AddForeignKey("dbo.Businesses", "Owner_Id", "dbo.Characters", "Id");
        }
    }
}
