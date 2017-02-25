var menuPool = null;
var menuConfirmPropertyBuy = null;

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case 'SC_SHOW_BUY_PROP_CONFIRM_MENU':
            menuPool = API.getMenuPool();
            menuConfirmPropertyBuy = API.createMenu("Confirmação", "Deseja comprar esta propriedade?", 0, 0, 6);
            menuConfirmPropertyBuy.AddItem(API.createMenuItem("Sim", "Quero comprar esta propriedade por $" + args[1]));
            menuConfirmPropertyBuy.AddItem(API.createMenuItem("Não", "Não quero comprar esta propriedade"));

            menuPool.Add(menuConfirmPropertyBuy);
            menuConfirmPropertyBuy.Visible = true;

            menuConfirmPropertyBuy.OnItemSelect.connect(function (sender, item, index) {
                API.triggerServerEvent("CS_BUY_PROP_CONFIRMATION", index, args[0]);
            });

            break;

        case 'SC_CLOSE_BUY_PROP_CONFIRM_MENU':
            menuConfirmPropertyBuy.Visible = false;
            break;
    }
});

API.onUpdate.connect(function () {
    if (menuPool != null) {
        menuPool.ProcessMenus();
    }
});