﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GTANetworkServer;
using GTANetworkShared;

namespace ProjetoRP.Business
{
    public class FactionBLL
    {
        public void LoadFactions()
        {
            Business.GlobalVariables.Instance.ServerFactions = SQL_FetchFactions();
        }

        public void SaveFactions()
        {
            using (var context = new DatabaseContext())
            {
                foreach (var faction in Business.GlobalVariables.Instance.ServerFactions)
                {
                    Faction_Save(faction);
                }
            }
        }

        public void Faction_Save(Entities.Faction.Faction faction)
        {
            using (var context = new DatabaseContext())
            {
                context.Factions.Attach(faction);
                context.Entry(faction).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Faction_Create(string name, string acro, Entities.Faction.FactionType type, int bank = 20000)
        {
            Entities.Faction.Faction faction = new Entities.Faction.Faction();            

            faction.Name = name;
            faction.Acro = acro;
            faction.Type = type;
            faction.Bank = bank;            

            using (var context = new DatabaseContext())
            {
                context.Factions.Attach(faction);
                context.Factions.Add(faction);
                context.SaveChanges();                
            }

            Faction_AddLeaderRank(faction, "Líder", 1);

            Business.GlobalVariables.Instance.ServerFactions.Add(faction);            
        }        

        public void Faction_Delete(Entities.Faction.Faction faction)
        {            
            using (var context = new DatabaseContext())
            {
                context.Factions.Attach(faction);
                context.Factions.Remove(faction);
                context.SaveChanges();
            }

            Business.GlobalVariables.Instance.ServerFactions.Remove(faction);
        }

        public void Faction_AddLeaderRank(Entities.Faction.Faction faction, string name, int level)
        {
            Entities.Faction.Rank leaderRank = new Entities.Faction.Rank();

            leaderRank.Leader = true;
            leaderRank.Level = level;
            leaderRank.Name = name;
            leaderRank.Faction = faction;
            leaderRank.Faction_Id = faction.Id;

            faction.Ranks.Add(leaderRank);

            using (var context = new DatabaseContext())
            {
                context.Ranks.Attach(leaderRank);
                context.Ranks.Add(leaderRank);
                context.SaveChanges();
            }

            Faction_Save(faction);
        }

        public Entities.Faction.Faction FindFactionById(int id) //Should we be using C#'s predicate List find?
        {
            Entities.Faction.Faction found = null;

            foreach (var faction in Business.GlobalVariables.Instance.ServerFactions)
            {
                if (faction.Id == id)
                {
                    found = faction;
                    break;
                }
            }

            return found;
        }

        public Entities.Faction.Rank Faction_GetLeaderRank(Entities.Faction.Faction faction)
        {
            Entities.Faction.Rank rank = null;
            foreach (var r in faction.Ranks)
            {
                if (r.Leader)
                {
                    rank = r;
                }
            }

            return rank;
        }

        public bool Faction_IsLeader(Entities.Character character, Entities.Faction.Faction faction)
        {
            if (Faction_GetLeader(faction).Id == character.Id)
            {
                return true;
            }
            return false;
        }

        public bool Faction_Validate(string name, string acro, int type, string bank, out string msg)
        {
            if (Business.GlobalVariables.Instance.ServerFactions.Find(x => x.Name == name) != null)
            {
                msg = "Já existe uma facção com este nome!";
                return false;
            }
            if (Business.GlobalVariables.Instance.ServerFactions.Find(x => x.Acro == acro) != null)
            {
                msg = "Já existe uma facção com este acrônimo!";
                return false;
            }
            if (!Enum.IsDefined(typeof(Entities.Faction.FactionType), type))
            {
                msg = "Este tipo de facção é inválido!";
                return false;
            }

            int bankVal = 0;

            if(!int.TryParse(bank, out bankVal)){
                msg = "Digite apenas números no campo do banco!";
                return false;
            }
            if (bankVal < 1)
            {
                msg = "Escolha um valor para o cofre maior do que 0!";
                return false;
            }
            msg = "Você criou esta facção com sucesso!";
            return true;
        }

        // SQL Functions
        public Entities.Faction.Faction SQL_FetchFactionData(int faction_id)
        {
            Entities.Faction.Faction faction = null;

            using (var context = new DatabaseContext())
            {
                faction = (from f in context.Factions where f.Id == faction_id select f).AsNoTracking().Single();
            }

            return faction;
        }

        public List<Entities.Faction.Faction> SQL_FetchFactions()
        {
            List<Entities.Faction.Faction> factions = new List<Entities.Faction.Faction>();

            using (var context = new DatabaseContext())
            {
                factions = (from f in context.Factions select f).Include(f => f.Ranks).AsNoTracking().ToList();
            }
            return factions;
        }        

        public Entities.Character Faction_GetLeader(Entities.Faction.Faction faction)
        {
            Entities.Character character = null;            

            Entities.Faction.Rank rank = faction.Ranks.FirstOrDefault(r => r.Leader == true);

            using (var context = new DatabaseContext())
            {                
                character = (from c in context.Characters where c.Rank_Id == rank.Id select c).AsNoTracking().Single();
            }            

            return character;
        }
    }
}