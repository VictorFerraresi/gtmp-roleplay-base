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

                                var skins = ["CopCutscene", "Prisguard01SMM", "Sheriff01SMY", "Sheriff01SFY", "Ranger01SMY",
                                "Ranger01SFY", "Armoured01", "Armoured01SMM", "Armoured02SMM", "Casey", "CiaSec01SMM",
                                "FibArchitect", "FibSec01", "PrologueSec02", "Snowcop01SMM", "TrafficWarden", "Hwaycop01SMY", "Marine03SMY",
                                "PrologueSec01Cutscene"];

                                menuPool = API.getMenuPool();
                                menuLockerUniform = API.createMenu("Uniforme", "Selecione um uniforme", 0, 0, 6);
                                menuLockerUniform.AddItem(API.createMenuItem("Officer", "Skin name: CopCutscene"));
                                menuLockerUniform.AddItem(API.createMenuItem("Officer 2", "Skin name: PrisGuard01SMM"));
                                menuLockerUniform.AddItem(API.createMenuItem("Officer 3", "Skin name: Sheriff01SMY"));
                                menuLockerUniform.AddItem(API.createMenuItem("Female Officer", "Skin name: Sheriff01SFY"));
                                menuLockerUniform.AddItem(API.createMenuItem("Ranger", "Skin name: Ranger01SMY"));
                                menuLockerUniform.AddItem(API.createMenuItem("Female Ranger", "Skin name: Ranger01SFY"));
                                menuLockerUniform.AddItem(API.createMenuItem("Carcereiro 1", "Skin name: Armoured01"));
                                menuLockerUniform.AddItem(API.createMenuItem("Carcereiro 2", "Skin name: Armoured01SMM"));
                                menuLockerUniform.AddItem(API.createMenuItem("Carcereiro 3", "Skin name: Armoured02SMM"));
                                menuLockerUniform.AddItem(API.createMenuItem("Carcereiro 4", "Skin name: Casey"));
                                menuLockerUniform.AddItem(API.createMenuItem("Detetive IIA", "Skin name: CIASec01SMM"));
                                menuLockerUniform.AddItem(API.createMenuItem("Detetive FIB", "Skin name: FIBArchitect"));
                                menuLockerUniform.AddItem(API.createMenuItem("Detetive FIB Colete", "Skin name: FIBSec01"));
                                menuLockerUniform.AddItem(API.createMenuItem("Officer with Jacket", "Skin name: PrologueSec02"));
                                menuLockerUniform.AddItem(API.createMenuItem("Snow Officer", "Skin name: SnowCop01SMM"));
                                menuLockerUniform.AddItem(API.createMenuItem("Traffic Officer", "Skin name: TrafficWarden"));
                                menuLockerUniform.AddItem(API.createMenuItem("Highway Patrol Officer", "Skin name: HWayCop01SMY"));
                                menuLockerUniform.AddItem(API.createMenuItem("Special Operations Officer", "Skin name: Marine03SMY"));                                                                                                                              
                                menuLockerUniform.AddItem(API.createMenuItem("High Command Officer 1", "Skin name: PrologueSec01Cutscene"));                                                                                                                      
                                menuPool.Add(menuLockerUniform);
                                menuLockerUniform.Visible = true;

                                menuLockerUniform.OnItemSelect.connect(function (sender, item, index) {
                                    API.triggerServerEvent("CS_SET_LOCKER_SKIN", skins[index]);                                         
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

        case 'SC_SHOW_LOCKER_MENU_EMS':
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

                                var skins = ["Paramedic01SMM", "Fireman01SMY", "Baywatch01SMY", "Baywatch01SFY", "Scientist01SMM",
                                    "ChemWork01GMM", "BoatStaff01M"];

                                menuPool = API.getMenuPool();
                                menuLockerUniform = API.createMenu("Uniforme", "Selecione um uniforme", 0, 0, 6);
                                menuLockerUniform.AddItem(API.createMenuItem("Paramédico", "Skin name: Paramedic01SMM"));
                                //menuLockerUniform.AddItem(API.createMenuItem("Paramédica", "Skin name: ExecutivePAFemale02"));                                                                
                                menuLockerUniform.AddItem(API.createMenuItem("Bombeiro", "Skin name: Fireman01SMY"));
                                menuLockerUniform.AddItem(API.createMenuItem("Salva Vidas", "Skin name: BayWatch01SMY"));
                                menuLockerUniform.AddItem(API.createMenuItem("Salva Vidas Mulher", "Skin name: BayWatch01SFY"));
                                menuLockerUniform.AddItem(API.createMenuItem("Médico", "Skin name: Scientist01SMM"));                                
                                menuLockerUniform.AddItem(API.createMenuItem("HAZMAT", "Skin name: ChemWork01GMM"));
                                menuLockerUniform.AddItem(API.createMenuItem("High Command Officer", "Skin name: BoatStaff01M"));                                
                                menuPool.Add(menuLockerUniform);
                                menuLockerUniform.Visible = true;

                                menuLockerUniform.OnItemSelect.connect(function (sender, item, index) {
                                    API.triggerServerEvent("CS_SET_LOCKER_SKIN", skins[index]);
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
                                menuLockerEquipment.AddItem(API.createMenuItem("Faca", "Faca Baioneta"));
                                menuLockerEquipment.AddItem(API.createMenuItem("Martelo", "Sledge"));
                                menuLockerEquipment.AddItem(API.createMenuItem("Pé de Cabra", "Pé de Cabra"));
                                menuLockerEquipment.AddItem(API.createMenuItem("Machado", "Machado de Fibra de Vidro"));
                                menuLockerEquipment.AddItem(API.createMenuItem("Extintor de Incêndio", "Apagador de pequenos focos de incêndio"));

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
                                            API.triggerServerEvent("CS_SET_LOCKER_EQUIPMENT", "Knife", 1);
                                            break;

                                        case 3:
                                            API.triggerServerEvent("CS_SET_LOCKER_EQUIPMENT", "Hammer", 1);
                                            break;

                                        case 4:
                                            API.triggerServerEvent("CS_SET_LOCKER_EQUIPMENT", "Crowbar", 1);
                                            break;

                                        case 5:
                                            API.triggerServerEvent("CS_SET_LOCKER_EQUIPMENT", "Battleaxe", 1);
                                            break;

                                        case 6:
                                            API.triggerServerEvent("CS_SET_LOCKER_EQUIPMENT", "FireExtinguisher", 1000);
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