namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCharacterCareer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "Career_Id", c => c.Int());
            CreateIndex("dbo.Characters", "Career_Id");
            AddForeignKey("dbo.Characters", "Career_Id", "dbo.Careers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Characters", "Career_Id", "dbo.Careers");
            DropIndex("dbo.Characters", new[] { "Career_Id" });
            DropColumn("dbo.Characters", "Career_Id");
        }
    }
}
