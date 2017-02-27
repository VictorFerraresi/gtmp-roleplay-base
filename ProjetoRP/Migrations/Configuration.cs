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
        }
    }
}
