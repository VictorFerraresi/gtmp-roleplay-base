namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Variation = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Placements",
                c => new
                    {
                        Item_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Item_Id)
                .ForeignKey("dbo.Items", t => t.Item_Id)
                .Index(t => t.Item_Id);
            
            CreateTable(
                "dbo.Placements_CharacterInventoryItems",
                c => new
                    {
                        Item_Id = c.Int(nullable: false),
                        Character_Id = c.Int(nullable: false),
                        Slot = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Item_Id)
                .ForeignKey("dbo.Placements", t => t.Item_Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id, cascadeDelete: true)
                .Index(t => t.Item_Id)
                .Index(t => t.Character_Id);
            
            CreateTable(
                "dbo.Placements_ContainerItems",
                c => new
                    {
                        Item_Id = c.Int(nullable: false),
                        ParentItem_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Item_Id)
                .ForeignKey("dbo.Placements", t => t.Item_Id)
                .ForeignKey("dbo.Items", t => t.ParentItem_Id, cascadeDelete: true)
                .Index(t => t.Item_Id)
                .Index(t => t.ParentItem_Id);
            
            CreateTable(
                "dbo.Placements_DropItems",
                c => new
                    {
                        Item_Id = c.Int(nullable: false),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        Dimension = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Item_Id)
                .ForeignKey("dbo.Placements", t => t.Item_Id)
                .Index(t => t.Item_Id);
            
            CreateTable(
                "dbo.Placements_TrunkItems",
                c => new
                    {
                        Item_Id = c.Int(nullable: false),
                        ParentVehicle_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Item_Id)
                .ForeignKey("dbo.Placements", t => t.Item_Id)
                .ForeignKey("dbo.Vehicles", t => t.ParentVehicle_Id, cascadeDelete: true)
                .Index(t => t.Item_Id)
                .Index(t => t.ParentVehicle_Id);
            
            CreateTable(
                "dbo.Items_CarKeys",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Vehicle_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.Vehicle_Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.Vehicle_Id);
            
            CreateTable(
                "dbo.Items_DoorKeys",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Door_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Id)
                .ForeignKey("dbo.Doors", t => t.Door_Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.Door_Id);
            
            CreateTable(
                "dbo.Items_Identifications",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Serial = c.String(maxLength: 64, storeType: "nvarchar"),
                        FirstName = c.String(maxLength: 64, storeType: "nvarchar"),
                        LastName = c.String(maxLength: 64, storeType: "nvarchar"),
                        Address = c.String(maxLength: 128, storeType: "nvarchar"),
                        DOB = c.String(maxLength: 12, storeType: "nvarchar"),
                        EXP = c.String(maxLength: 12, storeType: "nvarchar"),
                        Donor = c.Boolean(nullable: false),
                        Gender = c.Int(nullable: false),
                        Hair = c.Int(nullable: false),
                        Eyes = c.Int(nullable: false),
                        Height = c.Single(nullable: false),
                        Weight = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Items_Medkits",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Items_Pistols",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Bullets = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Items_PistolMagazines",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Bullets = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items_PistolMagazines", "Id", "dbo.Items");
            DropForeignKey("dbo.Items_Pistols", "Id", "dbo.Items");
            DropForeignKey("dbo.Items_Medkits", "Id", "dbo.Items");
            DropForeignKey("dbo.Items_Identifications", "Id", "dbo.Items");
            DropForeignKey("dbo.Items_DoorKeys", "Door_Id", "dbo.Doors");
            DropForeignKey("dbo.Items_DoorKeys", "Id", "dbo.Items");
            DropForeignKey("dbo.Items_CarKeys", "Vehicle_Id", "dbo.Vehicles");
            DropForeignKey("dbo.Items_CarKeys", "Id", "dbo.Items");
            DropForeignKey("dbo.Placements_TrunkItems", "ParentVehicle_Id", "dbo.Vehicles");
            DropForeignKey("dbo.Placements_TrunkItems", "Item_Id", "dbo.Placements");
            DropForeignKey("dbo.Placements_DropItems", "Item_Id", "dbo.Placements");
            DropForeignKey("dbo.Placements_ContainerItems", "ParentItem_Id", "dbo.Items");
            DropForeignKey("dbo.Placements_ContainerItems", "Item_Id", "dbo.Placements");
            DropForeignKey("dbo.Placements_CharacterInventoryItems", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.Placements_CharacterInventoryItems", "Item_Id", "dbo.Placements");
            DropForeignKey("dbo.Placements", "Item_Id", "dbo.Items");
            DropIndex("dbo.Items_PistolMagazines", new[] { "Id" });
            DropIndex("dbo.Items_Pistols", new[] { "Id" });
            DropIndex("dbo.Items_Medkits", new[] { "Id" });
            DropIndex("dbo.Items_Identifications", new[] { "Id" });
            DropIndex("dbo.Items_DoorKeys", new[] { "Door_Id" });
            DropIndex("dbo.Items_DoorKeys", new[] { "Id" });
            DropIndex("dbo.Items_CarKeys", new[] { "Vehicle_Id" });
            DropIndex("dbo.Items_CarKeys", new[] { "Id" });
            DropIndex("dbo.Placements_TrunkItems", new[] { "ParentVehicle_Id" });
            DropIndex("dbo.Placements_TrunkItems", new[] { "Item_Id" });
            DropIndex("dbo.Placements_DropItems", new[] { "Item_Id" });
            DropIndex("dbo.Placements_ContainerItems", new[] { "ParentItem_Id" });
            DropIndex("dbo.Placements_ContainerItems", new[] { "Item_Id" });
            DropIndex("dbo.Placements_CharacterInventoryItems", new[] { "Character_Id" });
            DropIndex("dbo.Placements_CharacterInventoryItems", new[] { "Item_Id" });
            DropIndex("dbo.Placements", new[] { "Item_Id" });
            DropTable("dbo.Items_PistolMagazines");
            DropTable("dbo.Items_Pistols");
            DropTable("dbo.Items_Medkits");
            DropTable("dbo.Items_Identifications");
            DropTable("dbo.Items_DoorKeys");
            DropTable("dbo.Items_CarKeys");
            DropTable("dbo.Placements_TrunkItems");
            DropTable("dbo.Placements_DropItems");
            DropTable("dbo.Placements_ContainerItems");
            DropTable("dbo.Placements_CharacterInventoryItems");
            DropTable("dbo.Placements");
            DropTable("dbo.Items");
        }
    }
}
