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
                new Entities.Character { Name = "Pete Sahut", Player = context.Players.Where(p => p.Name == "lgm").Single(), Cash = 500, Bank = 250000, Savings = 0, Skin = "Michael", X = 13.92, Y = 2.04, Z = 2.04, Dimension = 1, Xp = 0, Level = 1},
                new Entities.Character { Name = "Dominic Fisherman", Player = context.Players.Where(p => p.Name == "victor").Single(), Cash = 500, Bank = 250000, Savings = 0, Skin = "Michael", X = 13.92, Y = 2.04, Z = 2.04, Dimension = 1, Xp = 0, Level = 1 }
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
                new Entities.Faction.Rank { Name = "Sub-Líder", Level = 4, Faction = context.Factions.Where(f => f.Acro == "LSPD").Single(), Leader = true },
                new Entities.Faction.Rank { Name = "Comandante", Level = 3, Faction = context.Factions.Where(f => f.Acro == "LSPD").Single(), Leader = true },
                new Entities.Faction.Rank { Name = "Sargento", Level = 2, Faction = context.Factions.Where(f => f.Acro == "LSPD").Single(), Leader = true },
                new Entities.Faction.Rank { Name = "Recruta", Level = 1, Faction = context.Factions.Where(f => f.Acro == "LSPD").Single(), Leader = true },

                new Entities.Faction.Rank { Name = "Líder", Level = 5, Faction = context.Factions.Where(f => f.Acro == "LSFD").Single(), Leader = true },
                new Entities.Faction.Rank { Name = "Sub-Líder", Level = 4, Faction = context.Factions.Where(f => f.Acro == "LSFD").Single(), Leader = true },
                new Entities.Faction.Rank { Name = "Comandante", Level = 3, Faction = context.Factions.Where(f => f.Acro == "LSFD").Single(), Leader = true },
                new Entities.Faction.Rank { Name = "Doutor", Level = 2, Faction = context.Factions.Where(f => f.Acro == "LSFD").Single(), Leader = true },
                new Entities.Faction.Rank { Name = "Paramédico", Level = 1, Faction = context.Factions.Where(f => f.Acro == "LSFD").Single(), Leader = true }
            );
            context.SaveChanges();
        }
    }
}
