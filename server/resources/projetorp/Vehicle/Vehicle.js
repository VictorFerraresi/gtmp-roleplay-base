var menuPool = null;
var menuVehicleList = null;

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case 'SC_SHOW_VEHICLEMENU':
            menuPool = API.getMenuPool();
            menuVehicleList = API.createMenu("Meus Veiculos", 0, 0, 6);
            var vehicles = JSON.parse(args[0]);
            vehicles.forEach(function (vehicle) {
                var item = API.createMenuItem(vehicle.Name, "");
                menuVehicleList.AddItem(item);
            });

            menuPool.Add(menuVehicleList);
            menuVehicleList.Visible = true;

            menuVehicleList.OnItemSelect.connect(function (sender, item, index) {
                API.triggerServerEvent("CS_VEHICLE_SPAWN", JSON.stringify(vehicles[index]));                
            });

            break;

        case 'SC_CLOSE_VEHICLEMENU':
            menuVehicleList.Visible = false;
            break;
    }
});

API.onUpdate.connect(function () {
    if (menuPool != null) {
        menuPool.ProcessMenus();
    }
});