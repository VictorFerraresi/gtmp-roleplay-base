using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

public class Ui : Script
{
    public event ExportedEvent onUiEventTrigger;
    public Ui()
    {
        API.onResourceStop += stopResourceHandler;
        API.onResourceStart += startResourceHandler;
    }

    private void startResourceHandler()
    {
       
    }

    private void stopResourceHandler()
    {

    }

    /* EXPORTS */
    public void evalUi(Client player, string code)
    {
        API.triggerClientEvent(player, "SC_UI_EVAL", code);
    }

    public void fixCursor(Client player, bool onoff)
    {
       API.triggerClientEvent(player, "SC_UI_CURSOR_FIXED_" + (onoff? "ON" : "OFF"));
    }

    public void freeCursor(Client player)
    {
        API.triggerClientEvent(player, "SC_UI_CURSOR_FREE");
    }
}