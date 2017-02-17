using MySql.Data.Entity;
using ProjetoRP.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;

namespace ProjetoRP
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext() : base(nameOrConnectionString: "GameDb") {}

        public DbSet<Player> Players { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Session> Sessions { get; set; }
    }
}
