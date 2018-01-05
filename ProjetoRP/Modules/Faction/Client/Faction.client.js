var menuPool = null;
var menuLocker = null;
var menuLockerUniform = null;
var menuLockerEquipment = null;

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case 'SC_SHOW_LOCKER_MENU_POLICE':
            if (menuLocker == null || !menuLocker.Visible) {
                menuPool = API.getMenuPool();
                menuLocker = API.createMenu("Armário", "Selecione uma opção", 0, 0, 6);
                menuLocker.AddItem(API.createMenuItem("Uniforme", "Escolha um uniforme para o serviço"));            
                menuLocker.AddItem(API.createMenuItem("Equipamentos", "Retire equipamentos para o seu serviço"));
                menuLocker.AddItem(API.createMenuItem("Itens", "Retire itens para o seu serviço"));

                menuPool.Add(menuLocker);
                menuLocker.Visible = true;

                menuLocker.OnItemSelect.connect(function (sender, item, index) {
                    switch (index) {
                        case 0:
                            if (menuLockerUniform == null || !menuLockerUniform.Visible) {
                                menuLocker.Visible = false;

                                menuPool = API.getMenuPool();
                                menuLockerUniform = API.createMenu("Uniforme", "Selecione um uniforme", 0, 0, 6);
                                menuLockerUniform.AddItem(API.createMenuItem("Uniforme 1", "Skin name: Cop01SMY"));

                                menuPool.Add(menuLockerUniform);
                                menuLockerUniform.Visible = true;

                                menuLockerUniform.OnItemSelect.connect(function (sender, item, index) {
                                    switch (index) {
                                        case 0:
                                            API.triggerServerEvent("CS_SET_LOCKER_SKIN", "Cop01SMY");
                                            break;
                                    }
                                });
                            }
                            break;
                        case 1:
                            if (menuLockerEquipment == null || !menuLockerEquipment.Visible) {
                                menuLocker.Visible = false;

                                menuPool = API.getMenuPool();
                                menuLockerEquipment = API.createMenu("Equipamentos", "Selecione os equipamentos", 0, 0, 6);
                                menuLockerEquipment.AddItem(API.createMenuItem("Colete Balístico", "Proteção"));
                                menuLockerEquipment.AddItem(API.createMenuItem("Lanterna", "Lanterna Pelicano"));
                                menuLockerEquipment.AddItem(API.createMenuItem("Tonfa", "Arma branca"));
                                menuLockerEquipment.AddItem(API.createMenuItem("Pistola", "Beretta 92F, 9mm"));
                                menuLockerEquipment.AddItem(API.createMenuItem("Escopeta", "Benelli M4"));
                                menuLockerEquipment.AddItem(API.createMenuItem("Submetralhadora", " MP5"));
                                menuLockerEquipment.AddItem(API.createMenuItem("Rifle de Assalto", "HK416-14"));
                                menuLockerEquipment.AddItem(API.createMenuItem("Rifle de Precisão", "Remington 700"));

                                menuPool.Add(menuLockerEquipment);
                                menuLockerEquipment.Visible = true;

                                menuLockerEquipment.OnItemSelect.connect(function (sender, item, index) {
                                    switch (index) {
                                        case 0:
                                            API.triggerServerEvent("CS_SET_LOCKER_ARMOR");
                                            break;
                                        case 1:
                                            API.triggerServerEvent("CS_SET_LOCKER_EQUIPMENT", "Flashlight", 1);
                                            break;
                                        case 2:
                                            API.triggerServerEvent("CS_SET_LOCKER_EQUIPMENT", "Nightstick", 1);
                                            break;
                                        case 3:
                                            API.triggerServerEvent("CS_SET_LOCKER_EQUIPMENT", "Pistol", 50);
                                            break;
                                        case 4:
                                            API.triggerServerEvent("CS_SET_LOCKER_EQUIPMENT", "PumpShotgun", 15);
                                            break;
                                        case 5:
                                            API.triggerServerEvent("CS_SET_LOCKER_EQUIPMENT", "SMG", 150);
                                            break;
                                        case 6:
                                            API.triggerServerEvent("CS_SET_LOCKER_EQUIPMENT", "CarbineRifle", 150);
                                            break;
                                        case 7:
                                            API.triggerServerEvent("CS_SET_LOCKER_EQUIPMENT", "SniperRifle", 15);
                                            break;
                                    }
                                });
                            }
                            break;
                        case 2:
                            break;
                    }                
                });
            }
            break;            

        case 'SC_CLOSE_LOCKER_MENU':
            menuLocker.Visible = false;
            break;

        case 'SC_CLOSE_LOCKER_UNIFORM_MENU':
            menuLockerUniform.Visible = false;
            break;
    }
});

API.onKeyDown.connect(function (sender, key) {
    if (key.KeyCode == Keys.G) {
        API.triggerServerEvent("CS_SIREN_TOGGLE");        
    }
});

API.onEntityStreamIn.connect(function (ent, entType) {

    if (entType === 1) {
        if (API.hasEntitySyncedData(ent, 'SIREN_SOUND_STATUS')){
            var siren = API.getEntitySyncedData(ent, 'SIREN_SOUND_STATUS');

            API.callNative('0xD8050E0EB60CF274', ent, !siren);
        }        
    }
});

API.onUpdate.connect(function () {
    if (menuPool != null) {
        menuPool.ProcessMenus();
    }
});