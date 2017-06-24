var menuPool = null;
var menuConfirmCancelFare = null;

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case 'SC_SHOW_CANCEL_FARE_CONFIRM_MENU':
            menuPool = API.getMenuPool();
            menuConfirmCancelFare = API.createMenu("Confirmação", "Deseja mesmo cancelar a corrida?", 0, 0, 6);
            menuConfirmCancelFare.AddItem(API.createMenuItem("Sim", "Desejo cancelar a corrida e não receber o pagamento"));
            menuConfirmCancelFare.AddItem(API.createMenuItem("Não", "Não desejo cancelar a corrida"));

            menuPool.Add(menuConfirmCancelFare);
            menuConfirmCancelFare.Visible = true;

            menuConfirmCancelFare.OnItemSelect.connect(function (sender, item, index) {
                API.triggerServerEvent("CS_CANCEL_FARE_CONFIRMATION", index);
            });

            break;

        case 'SC_CLOSE_CANCEL_FARE_CONFIRM_MENU':
            menuConfirmCancelFare.Visible = false;
            break;

        case 'SC_REQUEST_TAXI':
            var player = API.getLocalPlayer();
            var pos = API.getEntityPosition(player);

            var streetName = API.getStreetName(pos);
            var zoneName = API.getZoneName(pos);

            API.triggerServerEvent("CS_REQUEST_TAXI", streetName, zoneName);
            break;
    }
});

API.onUpdate.connect(function () {
    if (menuPool != null) {
        menuPool.ProcessMenus();
    }
});