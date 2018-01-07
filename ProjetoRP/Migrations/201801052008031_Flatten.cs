namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Flatten : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlayerAttributes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Attribute = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        ExpiresAt = c.DateTime(precision: 0),
                        Player_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Players", t => t.Player_Id, cascadeDelete: true)
                .Index(t => t.Player_Id);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        Password = c.String(nullable: false, maxLength: 60, storeType: "nvarchar"),
                        Email = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.Characters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        PlayerId = c.Int(nullable: false),
                        Cash = c.Int(nullable: false),
                        Bank = c.Int(nullable: false),
                        Savings = c.Int(nullable: false),
                        Payment = c.Int(nullable: false),
                        Skin = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        Dimension = c.Int(nullable: false),
                        Xp = c.Double(nullable: false),
                        Level = c.Int(nullable: false),
                        LogoutArea = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Faction_Id = c.Int(),
                        Rank_Id = c.Int(),
                        Career_Id = c.Int(),
                        CareerRank = c.Int(),
                        CareerExperience = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Careers", t => t.Career_Id)
                .ForeignKey("dbo.Factions", t => t.Faction_Id)
                .ForeignKey("dbo.Players", t => t.PlayerId, cascadeDelete: true)
                .ForeignKey("dbo.Ranks", t => t.Rank_Id)
                .Index(t => t.Name, unique: true)
                .Index(t => t.PlayerId)
                .Index(t => t.Faction_Id)
                .Index(t => t.Rank_Id)
                .Index(t => t.Career_Id);
            
            CreateTable(
                "dbo.Careers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        Dimension = c.Int(nullable: false),
                        Name = c.String(nullable: false, unicode: false),
                        Public = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Factions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 40, storeType: "nvarchar"),
                        Acro = c.String(nullable: false, maxLength: 7, storeType: "nvarchar"),
                        Type = c.Int(nullable: false),
                        Bank = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true)
                .Index(t => t.Acro, unique: true);
            
            CreateTable(
                "dbo.Lockers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 32, storeType: "nvarchar"),
                        Faction_Id = c.Int(nullable: false),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        Dimension = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Factions", t => t.Faction_Id, cascadeDelete: true)
                .Index(t => t.Faction_Id);
            
            CreateTable(
                "dbo.Ranks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        Level = c.Int(nullable: false),
                        Faction_Id = c.Int(nullable: false),
                        Leader = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Factions", t => t.Faction_Id, cascadeDelete: true)
                .Index(t => t.Faction_Id);
            
            CreateTable(
                "dbo.Doors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Property_Id = c.Int(),
                        Model = c.Long(nullable: false),
                        Locked = c.Boolean(nullable: false),
                        ExteriorX = c.Double(nullable: false),
                        ExteriorY = c.Double(nullable: false),
                        ExteriorZ = c.Double(nullable: false),
                        ExteriorDimension = c.Int(nullable: false),
                        InteriorX = c.Double(nullable: false),
                        InteriorY = c.Double(nullable: false),
                        InteriorZ = c.Double(nullable: false),
                        InteriorDimension = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Properties", t => t.Property_Id)
                .Index(t => t.Property_Id);
            
            CreateTable(
                "dbo.Properties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        Dimension = c.Int(nullable: false),
                        Address = c.String(unicode: false),
                        Price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Industries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        Dimension = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.LoadPoints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductType = c.Int(nullable: false),
                        Industry_Id = c.Int(),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        Dimension = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Industries", t => t.Industry_Id)
                .Index(t => t.Industry_Id);
            
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
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        Owner_Id = c.Int(),
                        Owner_Type = c.Int(nullable: false),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                        Z = c.Double(nullable: false),
                        rX = c.Double(nullable: false),
                        rY = c.Double(nullable: false),
                        rZ = c.Double(nullable: false),
                        Dimension = c.Int(nullable: false),
                        Health = c.Single(nullable: false),
                        Color1 = c.Int(nullable: false),
                        Color2 = c.Int(nullable: false),
                        LicensePlate = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Failed = c.Boolean(nullable: false),
                        Ip = c.String(nullable: false, maxLength: 45, storeType: "nvarchar"),
                        LoginAt = c.DateTime(nullable: false, precision: 0),
                        LogoutAt = c.DateTime(nullable: false, precision: 0),
                        Rgsc = c.String(maxLength: 64, storeType: "nvarchar"),
                        Character_Id = c.Int(),
                        ParentSession_Id = c.Int(),
                        Player_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .ForeignKey("dbo.Sessions", t => t.ParentSession_Id)
                .ForeignKey("dbo.Players", t => t.Player_Id, cascadeDelete: true)
                .Index(t => t.Character_Id)
                .Index(t => t.ParentSession_Id)
                .Index(t => t.Player_Id);
            
            CreateTable(
                "dbo.Businesses",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Owner_Id = c.Int(),
                        Name = c.String(nullable: false, maxLength: 40, storeType: "nvarchar"),
                        BusinessType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Properties", t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Owner_Id)
                .Index(t => t.Id)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Houses",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Owner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Properties", t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Owner_Id)
                .Index(t => t.Id)
                .Index(t => t.Owner_Id);
            
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
                        Slot = c.Int(nullable: false),
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
                "dbo.Cellphone",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Model = c.String(maxLength: 64, storeType: "nvarchar"),
                        Number = c.Int(nullable: false),
                        TurnedOn = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Items_Containers",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.Id)
                .Index(t => t.Id);
            
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
            DropForeignKey("dbo.Items_Containers", "Id", "dbo.Items");
            DropForeignKey("dbo.Cellphone", "Id", "dbo.Items");
            DropForeignKey("dbo.Items_CarKeys", "Vehicle_Id", "dbo.Vehicles");
            DropForeignKey("dbo.Items_CarKeys", "Id", "dbo.Items");
            DropForeignKey("dbo.Placements_TrunkItems", "ParentVehicle_Id", "dbo.Vehicles");
            DropForeignKey("dbo.Placements_TrunkItems", "Item_Id", "dbo.Placements");
            DropForeignKey("dbo.Placements_DropItems", "Item_Id", "dbo.Placements");
            DropForeignKey("dbo.Placements_ContainerItems", "ParentItem_Id", "dbo.Items");
            DropForeignKey("dbo.Placements_ContainerItems", "Item_Id", "dbo.Placements");
            DropForeignKey("dbo.Placements_CharacterInventoryItems", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.Placements_CharacterInventoryItems", "Item_Id", "dbo.Placements");
            DropForeignKey("dbo.Houses", "Owner_Id", "dbo.Characters");
            DropForeignKey("dbo.Houses", "Id", "dbo.Properties");
            DropForeignKey("dbo.Businesses", "Owner_Id", "dbo.Characters");
            DropForeignKey("dbo.Businesses", "Id", "dbo.Properties");
            DropForeignKey("dbo.Sessions", "Player_Id", "dbo.Players");
            DropForeignKey("dbo.Sessions", "ParentSession_Id", "dbo.Sessions");
            DropForeignKey("dbo.Sessions", "Character_Id", "dbo.Characters");
            DropForeignKey("dbo.Placements", "Item_Id", "dbo.Items");
            DropForeignKey("dbo.LoadPoints", "Industry_Id", "dbo.Industries");
            DropForeignKey("dbo.Doors", "Property_Id", "dbo.Properties");
            DropForeignKey("dbo.PlayerAttributes", "Player_Id", "dbo.Players");
            DropForeignKey("dbo.Characters", "Rank_Id", "dbo.Ranks");
            DropForeignKey("dbo.Characters", "PlayerId", "dbo.Players");
            DropForeignKey("dbo.Characters", "Faction_Id", "dbo.Factions");
            DropForeignKey("dbo.Ranks", "Faction_Id", "dbo.Factions");
            DropForeignKey("dbo.Lockers", "Faction_Id", "dbo.Factions");
            DropForeignKey("dbo.Characters", "Career_Id", "dbo.Careers");
            DropIndex("dbo.Items_PistolMagazines", new[] { "Id" });
            DropIndex("dbo.Items_Pistols", new[] { "Id" });
            DropIndex("dbo.Items_Medkits", new[] { "Id" });
            DropIndex("dbo.Items_Identifications", new[] { "Id" });
            DropIndex("dbo.Items_DoorKeys", new[] { "Door_Id" });
            DropIndex("dbo.Items_DoorKeys", new[] { "Id" });
            DropIndex("dbo.Items_Containers", new[] { "Id" });
            DropIndex("dbo.Cellphone", new[] { "Id" });
            DropIndex("dbo.Items_CarKeys", new[] { "Vehicle_Id" });
            DropIndex("dbo.Items_CarKeys", new[] { "Id" });
            DropIndex("dbo.Placements_TrunkItems", new[] { "ParentVehicle_Id" });
            DropIndex("dbo.Placements_TrunkItems", new[] { "Item_Id" });
            DropIndex("dbo.Placements_DropItems", new[] { "Item_Id" });
            DropIndex("dbo.Placements_ContainerItems", new[] { "ParentItem_Id" });
            DropIndex("dbo.Placements_ContainerItems", new[] { "Item_Id" });
            DropIndex("dbo.Placements_CharacterInventoryItems", new[] { "Character_Id" });
            DropIndex("dbo.Placements_CharacterInventoryItems", new[] { "Item_Id" });
            DropIndex("dbo.Houses", new[] { "Owner_Id" });
            DropIndex("dbo.Houses", new[] { "Id" });
            DropIndex("dbo.Businesses", new[] { "Owner_Id" });
            DropIndex("dbo.Businesses", new[] { "Id" });
            DropIndex("dbo.Sessions", new[] { "Player_Id" });
            DropIndex("dbo.Sessions", new[] { "ParentSession_Id" });
            DropIndex("dbo.Sessions", new[] { "Character_Id" });
            DropIndex("dbo.Placements", new[] { "Item_Id" });
            DropIndex("dbo.LoadPoints", new[] { "Industry_Id" });
            DropIndex("dbo.Industries", new[] { "Name" });
            DropIndex("dbo.Doors", new[] { "Property_Id" });
            DropIndex("dbo.Ranks", new[] { "Faction_Id" });
            DropIndex("dbo.Lockers", new[] { "Faction_Id" });
            DropIndex("dbo.Factions", new[] { "Acro" });
            DropIndex("dbo.Factions", new[] { "Name" });
            DropIndex("dbo.Characters", new[] { "Career_Id" });
            DropIndex("dbo.Characters", new[] { "Rank_Id" });
            DropIndex("dbo.Characters", new[] { "Faction_Id" });
            DropIndex("dbo.Characters", new[] { "PlayerId" });
            DropIndex("dbo.Characters", new[] { "Name" });
            DropIndex("dbo.Players", new[] { "Email" });
            DropIndex("dbo.Players", new[] { "Name" });
            DropIndex("dbo.PlayerAttributes", new[] { "Player_Id" });
            DropTable("dbo.Items_PistolMagazines");
            DropTable("dbo.Items_Pistols");
            DropTable("dbo.Items_Medkits");
            DropTable("dbo.Items_Identifications");
            DropTable("dbo.Items_DoorKeys");
            DropTable("dbo.Items_Containers");
            DropTable("dbo.Cellphone");
            DropTable("dbo.Items_CarKeys");
            DropTable("dbo.Placements_TrunkItems");
            DropTable("dbo.Placements_DropItems");
            DropTable("dbo.Placements_ContainerItems");
            DropTable("dbo.Placements_CharacterInventoryItems");
            DropTable("dbo.Houses");
            DropTable("dbo.Businesses");
            DropTable("dbo.Sessions");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Placements");
            DropTable("dbo.Items");
            DropTable("dbo.LoadPoints");
            DropTable("dbo.Industries");
            DropTable("dbo.Properties");
            DropTable("dbo.Doors");
            DropTable("dbo.Ranks");
            DropTable("dbo.Lockers");
            DropTable("dbo.Factions");
            DropTable("dbo.Careers");
            DropTable("dbo.Characters");
            DropTable("dbo.Players");
            DropTable("dbo.PlayerAttributes");
        }
    }
}
