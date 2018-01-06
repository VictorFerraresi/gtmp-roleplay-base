using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetoRP.Business.Player;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using ProjetoRP.Entities;
using ProjetoRP.Types;
using System.Timers;

namespace ProjetoRP.Business.Career
{
    public class TaxiCareerBLL
    {
        PlayerBLL PlayerBLL = new PlayerBLL();

        public bool IsValidTaxi(Entities.Vehicle.Vehicle veh)
        {
            return veh.Name.Equals("Taxi");
        }

        public void SendMessageToOnDuty(string msg)
        {
            List<Client> players = API.shared.getAllPlayers();
            foreach (Client c in players)
            {
                if (c.hasData("TAXI_DUTY"))
                {
                    API.shared.sendChatMessageToPlayer(c, msg);
                }
            }
        }

        public int GetOnDutyCount()
        {
            int count = 0;

            List<Client> players = API.shared.getAllPlayers();
            foreach (Client c in players)
            {
                if (c.hasData("TAXI_DUTY"))
                {
                    count++;
                }
            }

            return count;
        }

        public void StartFare(Character driver, Character customer)
        {
            Timer FareTimer;
            FareTimer = new Timer(3000);
            FareTimer.AutoReset = true;
            FareTimer.Elapsed += (sender, e) => { ProcessFare(driver, customer); };
            FareTimer.Start();

            Client customerClient = ActivePlayer.Get(customer).Client;
            Client driverClient = ActivePlayer.Get(driver).Client;

            API.shared.sendChatMessageToPlayer(customerClient, "A viagem foi iniciada. Caso você ou o motorista saiam do veículo, ela será finalizada.");
            API.shared.sendChatMessageToPlayer(driverClient, "A viagem foi iniciada. Caso você ou o passageiro saiam do veículo, ela será finalizada.");

            customerClient.setData("TAXI_FARE", 0);
            customerClient.setData("TAXI_TIMER", FareTimer);

            driverClient.setData("TAXI_POSITION", driverClient.vehicle.position);
        }

        public void ProcessFare(Character driver, Character customer)
        {            
            Client customerClient = ActivePlayer.Get(customer).Client;
            Client driverClient = ActivePlayer.Get(driver).Client;

            Vector3 actualPos = driverClient.vehicle.position;
            Vector3 oldPos = driverClient.getData("TAXI_POSITION");

            float distFloat = actualPos.DistanceTo(oldPos);
            int dist = (int)Math.Floor(distFloat);

            int fare = customerClient.getData("TAXI_FARE");
            fare += 1; //$1 each 3 seconds

            fare += (dist/5);

            driverClient.setData("TAXI_POSITION", driverClient.vehicle.position);
            customerClient.setData("TAXI_FARE", fare);

            API.shared.sendChatMessageToPlayer(customerClient, "FARE: " + fare);
            API.shared.sendChatMessageToPlayer(driverClient, "FARE: " + fare);
        }

        public void FinishFare(Character driver, Character customer)
        {
            Client customerClient = ActivePlayer.Get(customer).Client;
            Client driverClient = ActivePlayer.Get(driver).Client;

            Timer FareTimer = customerClient.getData("TAXI_TIMER");
            FareTimer.Stop();
            FareTimer.Dispose();

            int fare = customerClient.getData("TAXI_FARE");

            string fareMsg = string.Format("A viagem foi concluída. Você pagou ~g~${0} ~w~ao motorista.", fare);
            API.shared.sendChatMessageToPlayer(customerClient, fareMsg);
            fareMsg = string.Format("A viagem foi concluída. Você recebeu ~g~${0} ~w~do passageiro.", fare);
            API.shared.sendChatMessageToPlayer(driverClient, fareMsg);

            PlayerBLL.Player_TakeMoney(customer, fare);

            driver.Payment += fare;

            string salaryMsg = string.Format("~g~+${0} ~w~adicionados ao pagamento", fare);
            API.shared.sendNotificationToPlayer(driverClient, salaryMsg);

            customerClient.resetData("TAXI_FARE");
            customerClient.resetData("TAXI_TIMER");
            customerClient.resetData("TAXI_DRIVER");

            driverClient.resetData("TAXI_POSITION");
            driverClient.resetData("TAXI_CUSTOMER");
        }

        public void CancelFare(Character driver, Character customer)
        {
            Client customerClient = ActivePlayer.Get(customer).Client;
            Client driverClient = ActivePlayer.Get(driver).Client;

            Timer FareTimer = customerClient.getData("TAXI_TIMER");
            FareTimer.Stop();
            FareTimer.Dispose();
            
            customerClient.resetData("TAXI_FARE");
            customerClient.resetData("TAXI_TIMER");
            customerClient.resetData("TAXI_DRIVER");

            driverClient.resetData("TAXI_POSITION");
            driverClient.resetData("TAXI_CUSTOMER");
        }
    }
}