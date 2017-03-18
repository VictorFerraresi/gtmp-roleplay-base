namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSlotToContainer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Placements_ContainerItems", "Slot", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Placements_ContainerItems", "Slot");
        }
    }
}
