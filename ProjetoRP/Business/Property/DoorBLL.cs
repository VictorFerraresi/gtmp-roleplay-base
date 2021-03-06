﻿using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using ProjetoRP.Business.Player;
using GrandTheftMultiplayer.Server.Extensions;

namespace ProjetoRP.Business.Property
{
    public class DoorBLL
    {
        public void LoadDoors()
        {
            Business.GlobalVariables.Instance.ServerDoors = SQL_FetchDoors();
        }

        public void SaveDoors()
        {
            using (var context = new DatabaseContext())
            {
                foreach (var door in Business.GlobalVariables.Instance.ServerDoors)
                {
                    Door_Save(door);
                }
            }            
        }

        public void Door_Save(Entities.Property.Door door)
        {
            using (var context = new DatabaseContext())
            {
                context.Doors.Attach(door);
                context.Entry(door).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Door_Create(Entities.Property.Property prop, long model, bool locked, Vector3 exterior, int exteriorDimension, Vector3 interior, int interiorDimension)
        {
            Entities.Property.Door door = new Entities.Property.Door();

            door.Property = prop;
            door.Model = model;
            door.Locked = locked;
            door.ExteriorX = exterior.X;
            door.ExteriorY = exterior.Y;
            door.ExteriorZ = exterior.Z;
            door.ExteriorDimension = exteriorDimension;
            door.InteriorX = interior.X;
            door.InteriorY = interior.Y;
            door.InteriorZ = interior.Z;
            door.InteriorDimension = interiorDimension;


            using (var context = new DatabaseContext())
            {
                context.Properties.Attach(prop);
                context.Doors.Add(door);
                context.SaveChanges();
            }

            Business.GlobalVariables.Instance.ServerDoors.Add(door);

            if (model != 0)
            {
                //Create door model
            }
        }

        public void Door_Delete(Entities.Property.Door door)
        {
            using (var context = new DatabaseContext())
            {
                context.Doors.Attach(door);
                context.Doors.Remove(door);
                context.SaveChanges();
            }            

            Business.GlobalVariables.Instance.ServerDoors.Remove(door);
        }

        public void Door_DeleteFromProperty(Entities.Property.Property prop)
        {
            List<Entities.Property.Door> toDelete = Business.GlobalVariables.Instance.ServerDoors.Copy();

            foreach (var door in toDelete)
            {
                if(door.Property == prop)
                {                    
                    Door_Delete(door);
                }
            }            
        }

        public Entities.Property.Door Door_GetNearestInRange(Client player, double range, bool outside)
        {
            Vector3 playerPos = API.shared.getEntityPosition(player);

            Entities.Property.Door nearestDoor = null;

            double nearestDistance = range;

            foreach (var door in Business.GlobalVariables.Instance.ServerDoors)
            {
                Vector3 doorPos;               
                int dimension;

                if (outside)
                {
                    dimension = door.ExteriorDimension;
                    doorPos = new Vector3(door.ExteriorX, door.ExteriorY, door.ExteriorZ);
                }
                else
                {
                    dimension = door.InteriorDimension;
                    doorPos = new Vector3(door.InteriorX, door.InteriorY, door.InteriorZ);
                }

                float distance = playerPos.DistanceTo(doorPos);                

                if (dimension == player.dimension && distance <= range && distance <= nearestDistance)
                {
                    nearestDistance = distance;
                    nearestDoor = door;
                }
            }
            return nearestDoor;
        }

        public Entities.Property.Door Door_GetNearestInRangeBothSides(Client player, double range)
        {
            Vector3 playerPos = API.shared.getEntityPosition(player);

            Entities.Property.Door nearestDoor = null;

            double nearestDistance = range;

            foreach (var door in Business.GlobalVariables.Instance.ServerDoors)
            {
                Vector3 doorPosExt = new Vector3(door.ExteriorX, door.ExteriorY, door.ExteriorZ);
                Vector3 doorPosInt = new Vector3(door.InteriorX, door.InteriorY, door.InteriorZ);

                float distanceExt = playerPos.DistanceTo(doorPosExt);
                float distanceInt = playerPos.DistanceTo(doorPosInt);

                if (door.ExteriorDimension == player.dimension  && distanceExt <= range && distanceExt <= nearestDistance)                     
                {
                    nearestDistance = distanceExt;
                    nearestDoor = door;
                }
                else if (door.InteriorDimension == player.dimension && distanceInt <= range && distanceInt <= nearestDistance)
                {
                    nearestDistance = distanceInt;
                    nearestDoor = door;
                }
            }            
            return nearestDoor;
        }

        public bool Door_IsLocked(Entities.Property.Door door)
        {
            return door.Locked;
        }


        // SQL Functions
        public Entities.Property.Door SQL_FetchDoorData(int door_id)
        {
            Entities.Property.Door door = null;

            using (var context = new DatabaseContext())
            {
                door = (from d in context.Doors where d.Id == door_id select d).AsNoTracking().Single();                               
            }

            return door;
        }

        public List<Entities.Property.Door> SQL_FetchDoors()
        {
            List<Entities.Property.Door> doors = new List<Entities.Property.Door>();

            using (var context = new DatabaseContext())
            {
                doors = (from d in context.Doors select d).Include(v => v.Property).AsNoTracking().ToList();                                       
            }
            return doors;
        }

        public Entities.Property.Door FindDoorById(int id) //Should we be using C#'s predicate List find?
        {
            Entities.Property.Door found = null;

            foreach (var door in Business.GlobalVariables.Instance.ServerDoors)
            {
                if (door.Id == id)
                {
                    found = door;
                    break;
                }
            }

            return found;
        }

        public void Door_LockCommand(Client player, Entities.Property.Door door) //To change with the item-key system
        {
            var ac = ActivePlayer.GetSpawned(player);
            if (ac == null) return;

            Entities.Character c = ac.Character;

            if (door.Property is Entities.Property.House)
            {
                Entities.Property.House h = (Entities.Property.House)door.Property;
                if (h.Owner_Id == c.Id)
                {
                    if (Door_IsLocked(door))
                    {
                        door.Locked = false;
                        API.shared.sendNotificationToPlayer(player, "Porta destrancada");
                    }
                    else
                    {
                        door.Locked = true;
                        API.shared.sendNotificationToPlayer(player, "Porta trancada");
                    }
                }
                else
                {
                    API.shared.sendChatMessageToPlayer(player, "Você não possui as chaves desta porta!");
                }
                
            }
            else if (door.Property is Entities.Property.Business)
            {
                Entities.Property.Business b = (Entities.Property.Business)door.Property;
                if (b.Owner_Id == c.Id)
                {
                    if (Door_IsLocked(door))
                    {
                        door.Locked = false;
                        API.shared.sendNotificationToPlayer(player, "Porta destrancada");
                    }
                    else
                    {
                        door.Locked = true;
                        API.shared.sendNotificationToPlayer(player, "Porta trancada");
                    }
                }
                else
                {
                    API.shared.sendChatMessageToPlayer(player, "Você não possui as chaves desta porta!");
                }
            }            
        }
    }
}