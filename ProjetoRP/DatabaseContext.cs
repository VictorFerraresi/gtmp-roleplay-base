using MySql.Data.Entity;
using ProjetoRP.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;
using ProjetoRP.Entities.ItemPlacement;

namespace ProjetoRP
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext() : base(nameOrConnectionString: "GameDb") {}

        public DbSet<Player> Players { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Session> Sessions { get; set; }

        public DbSet<Item> Items { get; set; }
        public DbSet<Placement> ItemsPlacement { get; set; }

        public DbSet<Entities.Vehicle.Vehicle> Vehicles { get; set; }        

        public DbSet<Entities.Property.Property> Properties { get; set; }
        public DbSet<Entities.Property.Door> Doors { get; set; }
        public DbSet<Entities.Faction.Faction> Factions { get; set; }
        public DbSet<Entities.Faction.Rank> Ranks { get; set; }
    }
}