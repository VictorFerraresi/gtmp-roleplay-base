namespace ProjetoRP.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ProjetoRP.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProjetoRP.DatabaseContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //                   

            context.Players.AddOrUpdate(
                p => p.Name,
                new Entities.Player { Name = "lgm", Email = "lgm@projetorp.work", Password = "$2a$04$1A9nNtvnE/0p7WK2GQ1rluEe7iW7hisYpY1C1I9S8HBYFx1f6CETO" },
                new Entities.Player { Name = "victor", Email = "victor@projetorp.work", Password = "$2a$04$1A9nNtvnE/0p7WK2GQ1rluEe7iW7hisYpY1C1I9S8HBYFx1f6CETO" }
            );   
            context.SaveChanges();

            context.Characters.AddOrUpdate(
                p => p.Name,
                new Entities.Character { Name = "Pete Sahut", Player = context.Players.Where(p => p.Name == "lgm").Single(), Cash = 500, Bank = 250000, Savings = 0, Skin = "Michael", X = 442.4198, Y = -982.5987, Z = 30.6896, Dimension = 1, Xp = 0, Level = 1},
                new Entities.Character { Name = "Dominic Fisherman", Player = context.Players.Where(p => p.Name == "victor").Single(), Cash = 500, Bank = 250000, Savings = 0, Skin = "Michael", X = 442.4198, Y = -982.5987, Z = 30.6896, Dimension = 1, Xp = 0, Level = 1 }
            );
            context.SaveChanges();

            context.Factions.AddOrUpdate(
                p => p.Name,
                new Entities.Faction.Faction { Name = "Los Santos Police Department", Acro = "LSPD", Type = (Entities.Faction.FactionType)2, Bank = 250000 },
                new Entities.Faction.Faction { Name = "Los Santos Fire Department", Acro = "LSFD", Type = (Entities.Faction.FactionType)3, Bank = 150000 }
            );
            context.SaveChanges();

            context.Ranks.AddOrUpdate(
                p => p.Name,
                new Entities.Faction.Rank { Name = "Líder", Level = 5, Faction = context.Factions.Where(f => f.Acro == "LSPD").Single(),Leader = true },
                new Entities.Faction.Rank { Name = "Sub-Líder", Level = 4, Faction = context.Factions.Where(f => f.Acro == "LSPD").Single(), Leader = false },
                new Entities.Faction.Rank { Name = "Comandante", Level = 3, Faction = context.Factions.Where(f => f.Acro == "LSPD").Single(), Leader = false },
                new Entities.Faction.Rank { Name = "Sargento", Level = 2, Faction = context.Factions.Where(f => f.Acro == "LSPD").Single(), Leader = false },
                new Entities.Faction.Rank { Name = "Recruta", Level = 1, Faction = context.Factions.Where(f => f.Acro == "LSPD").Single(), Leader = false },

                new Entities.Faction.Rank { Name = "Líder", Level = 5, Faction = context.Factions.Where(f => f.Acro == "LSFD").Single(), Leader = true },
                new Entities.Faction.Rank { Name = "Sub-Líder", Level = 4, Faction = context.Factions.Where(f => f.Acro == "LSFD").Single(), Leader = false },
                new Entities.Faction.Rank { Name = "Comandante", Level = 3, Faction = context.Factions.Where(f => f.Acro == "LSFD").Single(), Leader = false },
                new Entities.Faction.Rank { Name = "Doutor", Level = 2, Faction = context.Factions.Where(f => f.Acro == "LSFD").Single(), Leader = false },
                new Entities.Faction.Rank { Name = "Paramédico", Level = 1, Faction = context.Factions.Where(f => f.Acro == "LSFD").Single(), Leader = false }
            );
            context.SaveChanges();

            context.Vehicles.AddOrUpdate(
                v => v.Name,
                new Entities.Vehicle.Vehicle
                {
                    Name = "Police",
                    Owner_Id = 1,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION,
                    X = 408.2356,
                    Y = -979.9663,
                    Z = 28.8742,
                    rX = 0.0501,
                    rY = 0.0615,
                    rZ = 50.6764,
                    Dimension = 0,
                    Health = 1000,
                    Color1 = 111,
                    Color2 = 0,
                    LicensePlate = "LSPD"
                },
                new Entities.Vehicle.Vehicle
                {
                    Name = "Police",
                    Owner_Id = 1,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION,
                    X = 408.2093,
                    Y = -984.52,
                    Z = 28.8726,
                    rX = 0.0182,
                    rY = -0.0337,
                    rZ = 52.9352,
                    Dimension = 0,
                    Health = 1000,
                    Color1 = 111,
                    Color2 = 0,
                    LicensePlate = "LSPD"
                },
                new Entities.Vehicle.Vehicle
                {
                    Name = "Police",
                    Owner_Id = 1,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION,
                    X = 408.0875,
                    Y = -988.9463,
                    Z = 28.8724,
                    rX = 0.0201,
                    rY = -0.0088,
                    rZ = 51.7958,
                    Dimension = 0,
                    Health = 1000,
                    Color1 = 111,
                    Color2 = 0,
                    LicensePlate = "LSPD"
                },
                new Entities.Vehicle.Vehicle
                {
                    Name = "Police",
                    Owner_Id = 1,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION,
                    X = 408.081,
                    Y = -993.4136,
                    Z = 28.8725,
                    rX = 0.0104,
                    rY = -0.0070,
                    rZ = 51.0044,
                    Dimension = 0,
                    Health = 1000,
                    Color1 = 111,
                    Color2 = 0,
                    LicensePlate = "LSPD"
                },
                new Entities.Vehicle.Vehicle
                {
                    Name = "Police",
                    Owner_Id = 1,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION,
                    X = 408.1247,
                    Y = -998.1014,
                    Z = 28.8727,
                    rX = 0.0452,
                    rY = -0.0078,
                    rZ = 51.9310,
                    Dimension = 0,
                    Health = 1000,
                    Color1 = 111,
                    Color2 = 0,
                    LicensePlate = "LSPD"
                }
            );
            context.SaveChanges();

            context.Lockers.AddOrUpdate(
                p => p.Name,
                new Entities.Faction.Locker { Name = "Vestiário", Faction = context.Factions.Where(f => f.Acro == "LSPD").Single(), X = 457.7964, Y = -990.9011, Z = 30.6896, Dimension = 1 }             
            );
            context.SaveChanges();

        }
    }
}
