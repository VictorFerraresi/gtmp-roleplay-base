namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVehicleLicensePlate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Vehicles", "Character_Id", "Characters");
            DropIndex("Vehicles", new[] { "Character_Id" });
            AddColumn("dbo.Vehicles", "Owner_Id", c => c.Int());
            AddColumn("dbo.Vehicles", "Owner_Type", c => c.Int(nullable: false));
            AddColumn("dbo.Vehicles", "LicensePlate", c => c.String(nullable: false, unicode: false));
            DropColumn("dbo.Vehicles", "Character_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vehicles", "Character_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Vehicles", "LicensePlate");
            DropColumn("dbo.Vehicles", "Owner_Type");
            DropColumn("dbo.Vehicles", "Owner_Id");
            CreateIndex("Vehicles", "Character_Id");
            AddForeignKey("Vehicles", "Character_Id", "Characters", "Id", cascadeDelete: true);
        }
    }
}
