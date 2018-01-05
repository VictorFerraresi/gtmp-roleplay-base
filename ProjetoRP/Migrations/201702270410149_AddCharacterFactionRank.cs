namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCharacterFactionRank : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "Rank_Id", c => c.Int());
            CreateIndex("dbo.Characters", "Rank_Id");
            AddForeignKey("dbo.Characters", "Rank_Id", "dbo.Ranks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Characters", "Rank_Id", "dbo.Ranks");
            DropIndex("dbo.Characters", new[] { "Rank_Id" });
            DropColumn("dbo.Characters", "Rank_Id");
        }
    }
}
