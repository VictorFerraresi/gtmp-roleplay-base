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
                new Entities.Character { Name = "Pete Sahut", Player = context.Players.Where(p => p.Name == "lgm").Single(), Cash = 500, Bank = 250000, Savings = 0, Skin = "Michael", X = -438.0799, Y = 6021.344, Z = 31.4901, Dimension = 1, Xp = 0, Level = 1},
                new Entities.Character { Name = "Dominic Fisherman", Player = context.Players.Where(p => p.Name == "victor").Single(), Cash = 500, Bank = 250000, Savings = 0, Skin = "Michael", X = -438.0799, Y = 6021.344, Z = 31.4901, Dimension = 1, Xp = 0, Level = 1 },
                new Entities.Character { Name = "Rene Kasper", Player = context.Players.Where(p => p.Name == "rene").Single(), Cash = 500, Bank = 250000, Savings = 0, Skin = "Michael", X = -438.0799, Y = 6021.344, Z = 31.4901, Dimension = 1, Xp = 0, Level = 1 },
                new Entities.Character { Name = "Jimmy Braescher", Player = context.Players.Where(p => p.Name == "jimmy").Single(), Cash = 500, Bank = 250000, Savings = 0, Skin = "Michael", X = -438.0799, Y = 6021.344, Z = 31.4901, Dimension = 1, Xp = 0, Level = 1 }
            );
            context.SaveChanges();

            context.Attributes.AddOrUpdate(
                p => p.Attribute,
                new Entities.PlayerAttribute { Attribute = 0, CreatedAt = DateTime.Now, Player = context.Players.Where(p => p.Name == "lgm").Single() },
                new Entities.PlayerAttribute { Attribute = 0, CreatedAt = DateTime.Now, Player = context.Players.Where(p => p.Name == "victor").Single() },
                new Entities.PlayerAttribute { Attribute = 0, CreatedAt = DateTime.Now, Player = context.Players.Where(p => p.Name == "rene").Single() },
                new Entities.PlayerAttribute { Attribute = 0, CreatedAt = DateTime.Now, Player = context.Players.Where(p => p.Name == "jimmy").Single() }
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
                new Entities.Vehicle.Vehicle //ID 1
                {
                    Name = "Sheriff",
                    Owner_Id = context.Factions.Where(f => f.Acro == "PBSD").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION,
                    X = -435.1524, Y = 6031.542, Z = 30.9454, rX = 0.5395, rY = -0.0059, rZ = 28.4567, Dimension = 1, Health = 1000, Color1 = 111, Color2 = 111, LicensePlate = "PBSD"
                },
                new Entities.Vehicle.Vehicle //ID 2
                {
                    Name = "Sheriff",
                    Owner_Id = context.Factions.Where(f => f.Acro == "PBSD").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION,
                    X = -438.6821, Y = 6029.076, Z = 30.9455, rX = 0.5423, rY = -0.0353, rZ = 32.3514, Dimension = 1, Health = 1000, Color1 = 111, Color2 = 111, LicensePlate = "PBSD"
                },
                new Entities.Vehicle.Vehicle //ID 3
                {
                    Name = "Sheriff2",
                    Owner_Id = context.Factions.Where(f => f.Acro == "PBSD").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION,
                    X = -452.3529, Y = 6049.364, Z = 30.9627, rX = 0.0418, rY = -0.0056, rZ = -141.7564, Dimension = 1, Health = 1000, Color1 = 111, Color2 = 0, LicensePlate = "PBSD"
                },
                new Entities.Vehicle.Vehicle //ID 4
                {
                    Name = "Policeb",
                    Owner_Id = context.Factions.Where(f => f.Acro == "PBSD").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION,
                    X = -449.3483, Y = 6053.436, Z = 30.8194, rX = 0.5522, rY = -3.6968, rZ = -149.8488, Dimension = 1, Health = 1000, Color1 = 111, Color2 = 111, LicensePlate = "PBSD"
                },
                new Entities.Vehicle.Vehicle //ID 5
                {
                    Name = "Policeb",
                    Owner_Id = context.Factions.Where(f => f.Acro == "PBSD").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION,
                    X = -445.3249, Y = 6055.415, Z = 30.8191, rX = 0.6801, rY = -5.3623, rZ = -152.5508, Dimension = 1, Health = 1000, Color1 = 111, Color2 = 111, LicensePlate = "PBSD"
                },
                new Entities.Vehicle.Vehicle //ID 6
                {
                    Name = "Polmav",
                    Owner_Id = context.Factions.Where(f => f.Acro == "PBSD").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_FACTION,
                    X = -475.4922, Y = 5988.176, Z = 31.7245, rX = 0.1113, rY = -0.0096, rZ = -41.9114, Dimension = 1, Health = 1000, Color1 = 111, Color2 = 111, LicensePlate = "PBSD"
                }
            );
            context.SaveChanges();            

            context.Vehicles.AddOrUpdate( //TRUCKER JOB VEHICLES
                v => v.Name,
                new Entities.Vehicle.Vehicle //ID 7
                {
                    Name = "Boxville2",
                    Owner_Id = context.Careers.Where(c => c.Name == "Caminhoneiro").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_CAREER,
                    X = -271.5052, Y = 6069.387, Z = 31.3660, rX = 0.0646, rY = 0.0065, rZ = 124.1328, Dimension = 1, Health = 1000, Color1 = 111, Color2 = 111, LicensePlate = "A111BAC"
                },
                new Entities.Vehicle.Vehicle //ID 8
                {
                    Name = "Boxville3",
                    Owner_Id = context.Careers.Where(c => c.Name == "Caminhoneiro").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_CAREER,
                    X = -269.0002, Y = 6066.663, Z = 31.3658, rX = 0.0310, rY = 0.0540, rZ = 123.1728, Dimension = 1, Health = 1000, Color1 = 111, Color2 = 111, LicensePlate = "A112BAC"
                },
                new Entities.Vehicle.Vehicle //ID 9
                {
                    Name = "Boxville4",
                    Owner_Id = context.Careers.Where(c => c.Name == "Caminhoneiro").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_CAREER,
                    X = -265.6275, Y = 6064.297, Z = 31.3657, rX = 0.1065, rY = 0.0487, rZ = 123.0673, Dimension = 1, Health = 1000, Color1 = 111, Color2 = 0, LicensePlate = "A113BAC"
                },
                new Entities.Vehicle.Vehicle //ID 10
                {
                    Name = "Boxville3",
                    Owner_Id = context.Careers.Where(c => c.Name == "Caminhoneiro").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_CAREER,
                    X = -262.9933, Y = 6061.311, Z = 31.4579, rX = -0.3647, rY = 2.4708, rZ = 123.0673, Dimension = 1, Health = 1000, Color1 = 111, Color2 = 111, LicensePlate = "A114BAC"
                },
                new Entities.Vehicle.Vehicle //ID 11
                {
                    Name = "Boxville4",
                    Owner_Id = context.Careers.Where(c => c.Name == "Caminhoneiro").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_CAREER,
                    X = -260.0002, Y = 6058.795, Z = 31.6143, rX = -0.0048, rY = 2.4708, rZ = 123.2669, Dimension = 1, Health = 1000, Color1 = 111, Color2 = 111, LicensePlate = "A115BAC"
                },
                new Entities.Vehicle.Vehicle //ID 12
                {
                    Name = "Boxville1",
                    Owner_Id = context.Careers.Where(c => c.Name == "Caminhoneiro").Single().Id,
                    Owner_Type = Entities.Vehicle.OwnerType.OWNER_TYPE_CAREER,
                    X = -257.22, Y = 6056.072, Z = 31.8606, rX = 0.2252, rY = 3.5552, rZ = 124.3577, Dimension = 1, Health = 1000, Color1 = 111, Color2 = 111, LicensePlate = "A116BAC"
                }
            );
            context.SaveChanges();

            context.Lockers.AddOrUpdate(
                p => p.Name,
                new Entities.Faction.Locker { Name = "Vestiário", Faction = context.Factions.Where(f => f.Acro == "PBSD").Single(), X = -452.0233, Y = 6006.116, Z = 31.8409, Dimension = 1 },
                new Entities.Faction.Locker { Name = "Vestiário", Faction = context.Factions.Where(f => f.Acro == "PBFD").Single(), X = -366.1232, Y = 6103.068, Z = 35.4396, Dimension = 1 }
            );
            context.SaveChanges();

            context.Properties.AddOrUpdate( //Paleto Bay Businesses
                p => p.Address,
                new Entities.Property.Business { Address = "Paleto Boulevard, 2", BusinessType = (Entities.Property.BusinessType)4, Dimension = 1, Name = "Bar", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -389.20068359375, Y = 6050.419921875, Z = 31.5001277923584 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 4", BusinessType = (Entities.Property.BusinessType)4, Dimension = 1, Name = "Johnnys Liquor", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -406.4127197265625, Y = 6062.7197265625, Z = 31.500110626220703 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 5", BusinessType = (Entities.Property.BusinessType)15, Dimension = 1, Name = "Post OP", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -422.89117431640625, Y = 6135.8359375, Z = 31.87733268737793 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 8", BusinessType = (Entities.Property.BusinessType)5, Dimension = 1, Name = "Peckerwood", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -315.513916015625, Y = 6194.09130859375, Z = 31.560802459716797 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 11", BusinessType = (Entities.Property.BusinessType)1, Dimension = 1, Name = "General Store", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -325.9431457519531, Y = 6228.623046875, Z = 31.50290298461914 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 10", BusinessType = (Entities.Property.BusinessType)3, Dimension = 1, Name = "Golden Buns Bakery", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -302.4874572753906, Y = 6211.3369140625, Z = 31.42509651184082 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 12", BusinessType = (Entities.Property.BusinessType)6, Dimension = 1, Name = "Herr Kutz Barbearia", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -281.48211669921875, Y = 6232.5673828125, Z = 31.69072914123535 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 13", BusinessType = (Entities.Property.BusinessType)5, Dimension = 1, Name = "The Hen House", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -300.5987548828125, Y = 6256.7509765625, Z = 31.493371963500977 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 15", BusinessType = (Entities.Property.BusinessType)16, Dimension = 1, Name = "Paleto Pets", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -269.9703369140625, Y = 6283.7724609375, Z = 31.490217208862305 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 14", BusinessType = (Entities.Property.BusinessType)0, Dimension = 1, Name = "Aluga-se", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -276.1390075683594, Y = 6239.1767578125, Z = 31.489208221435547 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 17", BusinessType = (Entities.Property.BusinessType)4, Dimension = 1, Name = "The Bay Bar", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -262.7393493652344, Y = 6291.130859375, Z = 31.492786407470703 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 18", BusinessType = (Entities.Property.BusinessType)7, Dimension = 1, Name = "Red's Machines Supplies", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -195.5071258544922, Y = 6264.65185546875, Z = 31.489364624023438 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 20", BusinessType = (Entities.Property.BusinessType)4, Dimension = 1, Name = "Mojito Inn", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -122.93778991699219, Y = 6389.46435546875, Z = 32.177642822265625 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 23", BusinessType = (Entities.Property.BusinessType)4, Dimension = 1, Name = "Sally's", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -201.64356994628906, Y = 6354.40185546875, Z = 31.494558334350586 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 25", BusinessType = (Entities.Property.BusinessType)11, Dimension = 1, Name = "Bay Side Drugs", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -172.68341064453125, Y = 6381.45166015625, Z = 31.472795486450195 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 27", BusinessType = (Entities.Property.BusinessType)0, Dimension = 1, Name = "Escritório", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -163.60133361816406, Y = 6390.91796875, Z = 31.49041175842285 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 28", BusinessType = (Entities.Property.BusinessType)3, Dimension = 1, Name = "Xero Gas Station", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -92.761962890625, Y = 6409.6904296875, Z = 31.6403751373291 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 32", BusinessType = (Entities.Property.BusinessType)0, Dimension = 1, Name = "No Marks Cleaners", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -50.92090606689453, Y = 6459.68017578125, Z = 31.510101318359375 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 34", BusinessType = (Entities.Property.BusinessType)0, Dimension = 1, Name = "Belinda Mays Salon", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -44.41921615600586, Y = 6466.59619140625, Z = 31.503612518310547 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 36", BusinessType = (Entities.Property.BusinessType)0, Dimension = 1, Name = "Aluga-se", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -39.4416618347168, Y = 6471.4404296875, Z = 31.501224517822266 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 38", BusinessType = (Entities.Property.BusinessType)8, Dimension = 1, Name = "Ray's Electronics", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -30.513517379760742, Y = 6480.24609375, Z = 31.501001358032227 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 40", BusinessType = (Entities.Property.BusinessType)0, Dimension = 1, Name = "Aluga-se", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -20.629255294799805, Y = 6490.4873046875, Z = 31.500125885009766 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 42", BusinessType = (Entities.Property.BusinessType)9, Dimension = 1, Name = "Bay Hardware", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -11.053092002868652, Y = 6499.1630859375, Z = 31.505084991455078 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 44", BusinessType = (Entities.Property.BusinessType)10, Dimension = 1, Name = "Discount Store Clothing", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -1.1016453504562378, Y = 6516.9150390625, Z = 31.86890411376953 },
                new Entities.Property.Business { Address = "Paleto Boulevard, 31", BusinessType = (Entities.Property.BusinessType)1, Dimension = 1, Name = "Willie's Supermarket", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -59.4497184753418, Y = 6523.89208984375, Z = 31.490833282470703 },
                new Entities.Property.Business { Address = "Great Ocean Highway, 1000", BusinessType = (Entities.Property.BusinessType)1, Dimension = 1, Name = "Don's Country Store", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = 162.087158203125, Y = 6636.583984375, Z = 31.556400299072266 },
                new Entities.Property.Business { Address = "Great Ocean Highway, 1002", BusinessType = (Entities.Property.BusinessType)11, Dimension = 1, Name = "Pop's Pills", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = 150.8369903564453, Y = 6647.97509765625, Z = 31.59632110595703 },
                new Entities.Property.Business { Address = "Great Ocean Highway, 1004", BusinessType = (Entities.Property.BusinessType)7, Dimension = 1, Name = "Auto Services", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = 119.31420135498047, Y = 6626.69384765625, Z = 31.956052780151367 },
                new Entities.Property.Business { Address = "Pyrite Avenue, 1", BusinessType = (Entities.Property.BusinessType)3, Dimension = 1, Name = "Clucking Bell Farms", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -146.27020263671875, Y = 6303.560546875, Z = 31.558504104614258 },
                new Entities.Property.Business { Address = "Pyrite Avenue, 3", BusinessType = (Entities.Property.BusinessType)4, Dimension = 1, Name = "Del Vecchio Liquor", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -157.8534393310547, Y = 6328.716796875, Z = 31.580827713012695 },
                new Entities.Property.Business { Address = "Pyrite Avenue, 5", BusinessType = (Entities.Property.BusinessType)0, Dimension = 1, Name = "Aluga-se", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -167.6819305419922, Y = 6337.7578125, Z = 31.57286262512207 },
                new Entities.Property.Business { Address = "Duluoz Avenue, 2", BusinessType = (Entities.Property.BusinessType)13, Dimension = 1, Name = "Helmut's Autos", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -236.6693115234375, Y = 6202.33837890625, Z = 31.939151763916016 },
                new Entities.Property.Business { Address = "Duluoz Avenue, 1", BusinessType = (Entities.Property.BusinessType)0, Dimension = 1, Name = "Jetsam", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -250.34552001953125, Y = 6159.53076171875, Z = 31.46413230895996 },
                new Entities.Property.Business { Address = "Duluoz Avenue, 3", BusinessType = (Entities.Property.BusinessType)0, Dimension = 1, Name = "Paleto Bay Financial Services", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -272.87255859375, Y = 6182.1943359375, Z = 31.629892349243164 },
                new Entities.Property.Business { Address = "Duluoz Avenue, 9", BusinessType = (Entities.Property.BusinessType)14, Dimension = 1, Name = "Tattoo", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -289.3353576660156, Y = 6199.814453125, Z = 31.46491813659668 },
                new Entities.Property.Business { Address = "Great Ocean Highway, 2000", BusinessType = (Entities.Property.BusinessType)12, Dimension = 1, Name = "Ammunation", Owner = null, Owner_Id = null, Price = 25000, Type = (Entities.Property.PropertyType)2, X = -324.8990783691406, Y = 6075.9169921875, Z = 31.248964309692383 }
            );
            context.SaveChanges();

            context.Doors.AddOrUpdate( //Paleto Bay Businesses Doors
                p => p.Property,
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 2").Id, ExteriorDimension = 1, ExteriorX = -389.20068359375, ExteriorY = 6050.419921875, ExteriorZ = 31.5001277923584, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 2").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 4").Id, ExteriorDimension = 1, ExteriorX = -406.4127197265625, ExteriorY = 6062.7197265625, ExteriorZ = 31.500110626220703, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 4").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 5").Id, ExteriorDimension = 1, ExteriorX = -422.89117431640625, ExteriorY = 6135.8359375, ExteriorZ = 31.87733268737793, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 5").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 8").Id, ExteriorDimension = 1, ExteriorX = -315.513916015625, ExteriorY = 6194.09130859375, ExteriorZ = 31.560802459716797, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 8").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 11").Id, ExteriorDimension = 1, ExteriorX = -325.9431457519531, ExteriorY = 6228.623046875, ExteriorZ = 31.50290298461914, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 11").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 10").Id, ExteriorDimension = 1, ExteriorX = -302.4874572753906, ExteriorY = 6211.3369140625, ExteriorZ = 31.42509651184082, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 10").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 12").Id, ExteriorDimension = 1, ExteriorX = -281.48211669921875, ExteriorY = 6232.5673828125, ExteriorZ = 31.69072914123535, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 12").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 13").Id, ExteriorDimension = 1, ExteriorX = -300.5987548828125, ExteriorY = 6256.7509765625, ExteriorZ = 31.493371963500977, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 13").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 15").Id, ExteriorDimension = 1, ExteriorX = -269.9703369140625, ExteriorY = 6283.7724609375, ExteriorZ = 31.490217208862305, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 15").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 14").Id, ExteriorDimension = 1, ExteriorX = -276.1390075683594, ExteriorY = 6239.1767578125, ExteriorZ = 31.489208221435547, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 14").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 17").Id, ExteriorDimension = 1, ExteriorX = -262.7393493652344, ExteriorY = 6291.130859375, ExteriorZ = 31.492786407470703, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 17").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 18").Id, ExteriorDimension = 1, ExteriorX = -195.5071258544922, ExteriorY = 6264.65185546875, ExteriorZ = 31.489364624023438, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 18").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 20").Id, ExteriorDimension = 1, ExteriorX = -122.93778991699219, ExteriorY = 6389.46435546875, ExteriorZ = 32.177642822265625, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 20").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 23").Id, ExteriorDimension = 1, ExteriorX = -201.64356994628906, ExteriorY = 6354.40185546875, ExteriorZ = 31.494558334350586, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 23").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 25").Id, ExteriorDimension = 1, ExteriorX = -172.68341064453125, ExteriorY = 6381.45166015625, ExteriorZ = 31.472795486450195, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 25").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 27").Id, ExteriorDimension = 1, ExteriorX = -163.60133361816406, ExteriorY = 6390.91796875, ExteriorZ = 31.49041175842285, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 27").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 28").Id, ExteriorDimension = 1, ExteriorX = -92.761962890625, ExteriorY = 6409.6904296875, ExteriorZ = 31.6403751373291, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 28").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 32").Id, ExteriorDimension = 1, ExteriorX = -50.92090606689453, ExteriorY = 6459.68017578125, ExteriorZ = 31.510101318359375, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 32").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 34").Id, ExteriorDimension = 1, ExteriorX = -44.41921615600586, ExteriorY = 6466.59619140625, ExteriorZ = 31.503612518310547, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 34").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 36").Id, ExteriorDimension = 1, ExteriorX = -39.4416618347168, ExteriorY = 6471.4404296875, ExteriorZ = 31.501224517822266, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 36").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 38").Id, ExteriorDimension = 1, ExteriorX = -30.513517379760742, ExteriorY = 6480.24609375, ExteriorZ = 31.501001358032227, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 38").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 40").Id, ExteriorDimension = 1, ExteriorX = -20.629255294799805, ExteriorY = 6490.4873046875, ExteriorZ = 31.500125885009766, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 40").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 42").Id, ExteriorDimension = 1, ExteriorX = -11.053092002868652, ExteriorY = 6499.1630859375, ExteriorZ = 31.505084991455078, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 42").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 44").Id, ExteriorDimension = 1, ExteriorX = -1.1016453504562378, ExteriorY = 6516.9150390625, ExteriorZ = 31.86890411376953, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 44").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 31").Id, ExteriorDimension = 1, ExteriorX = -59.4497184753418, ExteriorY = 6523.89208984375, ExteriorZ = 31.490833282470703, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Paleto Boulevard, 31").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Great Ocean Highway, 1000").Id, ExteriorDimension = 1, ExteriorX = 162.087158203125, ExteriorY = 6636.583984375, ExteriorZ = 31.556400299072266, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Great Ocean Highway, 1000").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Great Ocean Highway, 1002").Id, ExteriorDimension = 1, ExteriorX = 150.8369903564453, ExteriorY = 6647.97509765625, ExteriorZ = 31.59632110595703, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Great Ocean Highway, 1002").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Great Ocean Highway, 1004").Id, ExteriorDimension = 1, ExteriorX = 119.31420135498047, ExteriorY = 6626.69384765625, ExteriorZ = 31.956052780151367, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Great Ocean Highway, 1004").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Pyrite Avenue, 1").Id, ExteriorDimension = 1, ExteriorX = -146.27020263671875, ExteriorY = 6303.560546875, ExteriorZ = 31.558504104614258, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Pyrite Avenue, 1").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Pyrite Avenue, 3").Id, ExteriorDimension = 1, ExteriorX = -157.8534393310547, ExteriorY = 6328.716796875, ExteriorZ = 31.580827713012695, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Pyrite Avenue, 3").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Pyrite Avenue, 5").Id, ExteriorDimension = 1, ExteriorX = -167.6819305419922, ExteriorY = 6337.7578125, ExteriorZ = 31.57286262512207, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Pyrite Avenue, 5").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Duluoz Avenue, 2").Id, ExteriorDimension = 1, ExteriorX = -236.6693115234375, ExteriorY = 6202.33837890625, ExteriorZ = 31.939151763916016, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Duluoz Avenue, 2").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Duluoz Avenue, 1").Id, ExteriorDimension = 1, ExteriorX = -250.34552001953125, ExteriorY = 6159.53076171875, ExteriorZ = 31.46413230895996, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Duluoz Avenue, 1").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Duluoz Avenue, 3").Id, ExteriorDimension = 1, ExteriorX = -272.87255859375, ExteriorY = 6182.1943359375, ExteriorZ = 31.629892349243164, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Duluoz Avenue, 3").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Duluoz Avenue, 9").Id, ExteriorDimension = 1, ExteriorX = -289.3353576660156, ExteriorY = 6199.814453125, ExteriorZ = 31.46491813659668, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Duluoz Avenue, 9").Id },
                new Entities.Property.Door { InteriorDimension = context.Properties.FirstOrDefault(p => p.Address == "Great Ocean Highway, 2000").Id, ExteriorDimension = 1, ExteriorX = -324.8990783691406, ExteriorY = 6075.9169921875, ExteriorZ = 31.248964309692383, InteriorX = -18.77586, InteriorY = -581.755, InteriorZ = 90.11491 , Locked = true, Property_Id = context.Properties.FirstOrDefault(p => p.Address == "Great Ocean Highway, 2000").Id }
            );
            context.SaveChanges();

            context.Industries.AddOrUpdate( 
                p => p.Name,
                new Entities.Industry.Industry { Name = "Clucking Bell Farms", X = -136.7201, Y = 6198.64, Z = 32.3832, Dimension = 1 },
                new Entities.Industry.Industry { Name = "Morris and Sons", X = -51.8700, Y = 6360.526, Z = 31.5989, Dimension = 1 },
                new Entities.Industry.Industry { Name = "Chumash Chew Farms", X = 413.013, Y = 6538.982, Z = 27.7354, Dimension = 1 }
            );
            context.SaveChanges();

            context.LoadPoints.AddOrUpdate(
                p => p.ProductType,
                new Entities.Industry.LoadPoint { ProductType = Types.ProductType.Meal, Industry = context.Industries.FirstOrDefault(i => i.Name == "Clucking Bell Farms"), X = 44.8546, Y = 6303.378, Z = 31.2182, Dimension = 1 },
                new Entities.Industry.LoadPoint { ProductType = Types.ProductType.Fruit, Industry = context.Industries.FirstOrDefault(i => i.Name == "Clucking Bell Farms"), X = 96.2339, Y = 6328.889, Z = 31.3758, Dimension = 1 },
                new Entities.Industry.LoadPoint { ProductType = Types.ProductType.Corn, Industry = context.Industries.FirstOrDefault(i => i.Name == "Clucking Bell Farms"), X = 93.4310, Y = 6335.079, Z = 31.3758, Dimension = 1 }
            );
            context.SaveChanges();

        }
    }
}
