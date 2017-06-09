namespace ProjetoRP.Business
{
    public class PlayerBLL
    {               
        public void Player_GiveMoney(Entities.Character c, int amount)
        {
            c.Cash += amount;
        }

        public void Player_TakeMoney(Entities.Character c, int amount)
        {
            c.Cash -= amount;
        }

        /*public int? Player_GetNextFreeId()
        {
            int? a = null;
            for(int i = 0; i < 1000; i++)
            {
                if(API.shared.getAllPlayers().Find(x => x.getData("playerId") == i) != null)
                {
                    continue;
                }
                a = i;
                break;        
            }
            return a;
        }*/
    }
}