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
                new Entities.Player { Name = "victor", Email = "victor@projetorp.work", Password = "$2a$04$1A9nNtvnE/0p7WK2GQ1rluEe7iW7hisYpY1C1I9S8HBYFx1f6CETO" },
                new Entities.Player { Name = "rene", Email = "rene@projetorp.work", Password = "$2a$04$1A9nNtvnE/0p7WK2GQ1rluEe7iW7hisYpY1C1I9S8HBYFx1f6CETO" },
                new Entities.Player { Name = "jimmy", Email = "jimmy@projetorp.work", Password = "$2a$04$1A9nNtvnE/0p7WK2GQ1rluEe7iW7hisYpY1C1I9S8HBYFx1f6CETO" }
            );   
            context.SaveChanges();

            context.Characters.AddOrUpdate(
                p => p.Name,
                new Entities.Character { Name = "Pete Sahut", Player = context.Players.Where(p => p.Name == "lgm").Single(), Cash = 500, Bank = 250000, Savings = 0, Skin = "Michael", X = 442.4198, Y = -982.5987, Z = 30.6896, Dimension = 1, Xp = 0, Level = 1},
                new Entities.Character { Name = "Dominic Fisherman", Player = context.Players.Where(p => p.Name == "victor").Single(), Cash = 500, Bank = 250000, Savings = 0, Skin = "Michael", X = 442.4198, Y = -982.5987, Z = 30.6896, Dimension = 1, Xp = 0, Level = 1 },
                new Entities.Character { Name = "Rene Kasper", Player = context.Players.Where(p => p.Name == "rene").Single(), Cash = 500, Bank = 250000, Savings = 0, Skin = "Michael", X = 442.4198, Y = -982.5987, Z = 30.6896, Dimension = 1, Xp = 0, Level = 1 },
                new Entities.Character { Name = "Jimmy Braescher", Player = context.Players.Where(p => p.Name == "jimmy").Single(), Cash = 500, Bank = 250000, Savings = 0, Skin = "Michael", X = 442.4198, Y = -982.5987, Z = 30.6896, Dimension = 1, Xp = 0, Level = 1 }
            );
            context.SaveChanges();

            context.Factions.AddOrUpdate(
                p => p.Name,
                new Entities.Faction.Faction { Name = "Paleto Bay Sheriffs Department", Acro = "PBSD", Type = (Entities.Faction.FactionType)2, Bank = 250000 },
                new Entities.Faction.Faction { Name = "Paleto Bay Fire Department", Acro = "PBFD", Type = (Entities.Faction.FactionType)3, Bank = 150000 }
            );
            context.SaveChanges();

            context.Ranks.AddOrUpdate(
                p => p.Name,
                new Entities.Faction.Rank { Name = "Líder", Level = 5, Faction = context.Factions.Where(f => f.Acro == "PBSD").Single(),Leader = true },
                new Entities.Faction.Rank { Name = "Sub-Líder", Level = 4, Faction = context.Factions.Where(f => f.Acro == "PBSD").Single(), Leader = false },
                new Entities.Faction.Rank { Name = "Comandante", Level = 3, Faction = context.Factions.Where(f => f.Acro == "PBSD").Single(), Leader = false },
                new Entities.Faction.Rank { Name = "Sargento", Level = 2, Faction = context.Factions.Where(f => f.Acro == "PBSD").Single(), Leader = false },
                new Entities.Faction.Rank { Name = "Recruta", Level = 1, Faction = context.Factions.Where(f => f.Acro == "PBSD").Single(), Leader = false },

                new Entities.Faction.Rank { Name = "Líder", Level = 5, Faction = context.Factions.Where(f => f.Acro == "PBFD").Single(), Leader = true },
                new Entities.Faction.Rank { Name = "Sub-Líder", Level = 4, Faction = context.Factions.Where(f => f.Acro == "PBFD").Single(), Leader = false },
                new Entities.Faction.Rank { Name = "Comandante", Level = 3, Faction = context.Factions.Where(f => f.Acro == "PBFD").Single(), Leader = false },
                new Entities.Faction.Rank { Name = "Doutor", Level = 2, Faction = context.Factions.Where(f => f.Acro == "PBFD").Single(), Leader = false },
                new Entities.Faction.Rank { Name = "Paramédico", Level = 1, Faction = context.Factions.Where(f => f.Acro == "PBFD").Single(), Leader = false }
            );
            context.SaveChanges();

            context.Careers.AddOrUpdate(
                p => p.Name,
                new Entities.Career.Career { Name = "Caminhoneiro", Type = Entities.Career.CareerType.Trucker, X = -275.2385, Y = 6047.524, Z = 31.5914, Dimension = 1, Public = true }
            );
            context.SaveChanges();

            context.Vehicles.AddOrUpdate( //PBSD VEHICLES
                v => v.Name,
                new Entities.Vehicle.Vehicle
                {
                    Name = "Sheriff",
                    Owner_Id = context.Factions.Where(f => f.Acro == "PBSD").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION,
                    X = -435.1524,
                    Y = 6031.542,
                    Z = 30.9454,
                    rX = 0.5395,
                    rY = -0.0059,
                    rZ = 28.4567,
                    Dimension = 1,
                    Health = 1000,
                    Color1 = 111,
                    Color2 = 111,
                    LicensePlate = "PBSD"
                },
                new Entities.Vehicle.Vehicle
                {
                    Name = "Sheriff",
                    Owner_Id = context.Factions.Where(f => f.Acro == "PBSD").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION,
                    X = -438.6821,
                    Y = 6029.076,
                    Z = 30.9455,
                    rX = 0.5423,
                    rY = -0.0353,
                    rZ = 32.3514,
                    Dimension = 1,
                    Health = 1000,
                    Color1 = 111,
                    Color2 = 111,
                    LicensePlate = "PBSD"
                },
                new Entities.Vehicle.Vehicle
                {
                    Name = "Sheriff2",
                    Owner_Id = context.Factions.Where(f => f.Acro == "PBSD").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION,
                    X = -452.3529,
                    Y = 6049.364,
                    Z = 30.9627,
                    rX = 0.0418,
                    rY = -0.0056,
                    rZ = -141.7564,
                    Dimension = 1,
                    Health = 1000,
                    Color1 = 111,
                    Color2 = 0,
                    LicensePlate = "PBSD"
                },
                new Entities.Vehicle.Vehicle
                {
                    Name = "Policeb",
                    Owner_Id = context.Factions.Where(f => f.Acro == "PBSD").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION,
                    X = -449.3483,
                    Y = 6053.436,
                    Z = 30.8194,
                    rX = 0.5522,
                    rY = -3.6968,
                    rZ = -149.8488,
                    Dimension = 1,
                    Health = 1000,
                    Color1 = 111,
                    Color2 = 111,
                    LicensePlate = "PBSD"
                },
                new Entities.Vehicle.Vehicle
                {
                    Name = "Policeb",
                    Owner_Id = context.Factions.Where(f => f.Acro == "PBSD").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION,
                    X = -445.3249,
                    Y = 6055.415,
                    Z = 30.8191,
                    rX = 0.6801,
                    rY = -5.3623,
                    rZ = -152.5508,
                    Dimension = 1,
                    Health = 1000,
                    Color1 = 111,
                    Color2 = 111,
                    LicensePlate = "PBSD"
                },
                new Entities.Vehicle.Vehicle
                {
                    Name = "Polmav",
                    Owner_Id = context.Factions.Where(f => f.Acro == "PBSD").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION,
                    X = -475.4922,
                    Y = 5988.176,
                    Z = 31.7245,
                    rX = 0.1113,
                    rY = -0.0096,
                    rZ = -41.9114,
                    Dimension = 1,
                    Health = 1000,
                    Color1 = 111,
                    Color2 = 111,
                    LicensePlate = "PBSD"
                }
            );
            context.SaveChanges();

            context.Vehicles.AddOrUpdate( //TRUCKER JOB VEHICLES
                v => v.Name,
                new Entities.Vehicle.Vehicle
                {
                    Name = "Boxville2",
                    Owner_Id = context.Careers.Where(c => c.Name == "Caminhoneiro").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_CAREER,
                    X = -271.5052,
                    Y = 6069.387,
                    Z = 31.3660,
                    rX = 0.0646,
                    rY = 0.0065,
                    rZ = 124.1328,
                    Dimension = 1,
                    Health = 1000,
                    Color1 = 111,
                    Color2 = 111,
                    LicensePlate = "A111BAC"
                },
                new Entities.Vehicle.Vehicle
                {
                    Name = "Boxville3",
                    Owner_Id = context.Careers.Where(c => c.Name == "Caminhoneiro").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_CAREER,
                    X = -269.0002,
                    Y = 6066.663,
                    Z = 31.3658,
                    rX = 0.0310,
                    rY = 0.0540,
                    rZ = 123.1728,
                    Dimension = 1,
                    Health = 1000,
                    Color1 = 111,
                    Color2 = 111,
                    LicensePlate = "A112BAC"
                },
                new Entities.Vehicle.Vehicle
                {
                    Name = "Boxville4",
                    Owner_Id = context.Careers.Where(c => c.Name == "Caminhoneiro").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_CAREER,
                    X = -265.6275,
                    Y = 6064.297,
                    Z = 31.3657,
                    rX = 0.1065,
                    rY = 0.0487,
                    rZ = 123.0673,
                    Dimension = 1,
                    Health = 1000,
                    Color1 = 111,
                    Color2 = 0,
                    LicensePlate = "A113BAC"
                },
                new Entities.Vehicle.Vehicle
                {
                    Name = "Boxville3",
                    Owner_Id = context.Careers.Where(c => c.Name == "Caminhoneiro").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_CAREER,
                    X = -262.9933,
                    Y = 6061.311,
                    Z = 31.4579,
                    rX = -0.3647,
                    rY = 2.4708,
                    rZ = 123.0673,
                    Dimension = 1,
                    Health = 1000,
                    Color1 = 111,
                    Color2 = 111,
                    LicensePlate = "A114BAC"
                },
                new Entities.Vehicle.Vehicle
                {
                    Name = "Boxville4",
                    Owner_Id = context.Careers.Where(c => c.Name == "Caminhoneiro").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_CAREER,
                    X = -260.0002,
                    Y = 6058.795,
                    Z = 31.6143,
                    rX = -0.0048,
                    rY = 2.4708,
                    rZ = 123.2669,
                    Dimension = 1,
                    Health = 1000,
                    Color1 = 111,
                    Color2 = 111,
                    LicensePlate = "A115BAC"
                },
                new Entities.Vehicle.Vehicle
                {
                    Name = "Boxville1",
                    Owner_Id = context.Careers.Where(c => c.Name == "Caminhoneiro").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_CAREER,
                    X = -257.22,
                    Y = 6056.072,
                    Z = 31.8606,
                    rX = 0.2252,
                    rY = 3.5552,
                    rZ = 124.3577,
                    Dimension = 1,
                    Health = 1000,
                    Color1 = 111,
                    Color2 = 111,
                    LicensePlate = "A116BAC"
                }
            );
            context.SaveChanges();

            context.Lockers.AddOrUpdate(
                p => p.Name,
                new Entities.Faction.Locker { Name = "Vestiário", Faction = context.Factions.Where(f => f.Acro == "PBSD").Single(), X = -452.0233, Y = 6006.116, Z = 31.8409, Dimension = 1 }             
            );
            context.SaveChanges();          

        }
    }
}
