API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "SC_PRINT_AREA_NAME") {
        var player = API.getLocalPlayer();
        var pos = API.getEntityPosition(player);
        API.sendChatMessage(API.getStreetName(pos) + ", " + API.getZoneName(pos));        
    }
});