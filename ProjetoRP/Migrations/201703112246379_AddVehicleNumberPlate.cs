namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVehicleNumberPlate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "LicensePlate", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehicles", "LicensePlate");
        }
    }
}
