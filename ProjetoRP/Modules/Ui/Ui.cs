using System;
using GTANetworkServer;
using GTANetworkShared;

public class Ui : Script
{
    public event ExportedEvent onUiEventTrigger;
    public Ui()
    {
        API.onResourceStop += stopResourceHandler;
        API.onResourceStart += startResourceHandler;
        API.onClientEventTrigger += OnClientEventTrigger;
    }

    private void startResourceHandler()
    {
       
    }

    private void stopResourceHandler()
    {

    }

    public void OnClientEventTrigger(Client player, string eventName, object[] args)
    { 
        onUiEventTrigger(player, eventName, args);
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