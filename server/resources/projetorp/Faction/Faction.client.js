var menuPool = null;
var menuCreateFaction = null;

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case 'SC_SHOW_CREATE_FAC_MENU':
            API.getUserInput("Nome da Facção", 40);
            //menuPool = API.getMenuPool();
            //menuCreateFaction = API.createMenu("Criar Facção", "Digite o nome da facção:", 0, 0, 6);            
            //menuCreateFaction.AddItem(API.createMenuItem("Sim", "Quero comprar esta propriedade por $" + args[1]));
            //menuCreateFaction.AddItem(API.createMenuItem("Não", "Não quero comprar esta propriedade"));

            //menuPool.Add(menuCreateFaction);
            //menuCreateFaction.Visible = true;

            //menuCreateFaction.OnItemSelect.connect(function (sender, item, index) {
            //    API.triggerServerEvent("CS_BUY_PROP_CONFIRMATION", index, args[0]);
            //});

            break;

        case 'SC_CLOSE_CREATE_FAC_MENU':
            menuCreateFaction.Visible = false;
            break;
    }
});

API.onUpdate.connect(function () {
    if (menuPool != null) {
        menuPool.ProcessMenus();
    }
});