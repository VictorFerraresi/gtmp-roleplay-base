namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCharacterCareerInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "CareerRank", c => c.Int());
            AddColumn("dbo.Characters", "CareerExperience", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Characters", "CareerExperience");
            DropColumn("dbo.Characters", "CareerRank");
        }
    }
}
