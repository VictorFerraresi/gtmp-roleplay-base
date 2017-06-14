namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCharacterSalary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "Payment", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Characters", "Payment");
        }
    }
}
