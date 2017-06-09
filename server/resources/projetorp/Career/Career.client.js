var menuPool = null;
var menuConfirmCareerLeave = null;

API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case 'SC_SHOW_LEAVE_CAREER_CONFIRM_MENU':
            menuPool = API.getMenuPool();
            menuConfirmCareerLeave = API.createMenu("Confirmação", "Deseja mesmo sair do emprego?", 0, 0, 6);
            menuConfirmCareerLeave.AddItem(API.createMenuItem("Sim", "Desejo sair do emprego e resetar o meu cargo"));
            menuConfirmCareerLeave.AddItem(API.createMenuItem("Não", "Não desejo sair do emprego"));

            menuPool.Add(menuConfirmCareerLeave);
            menuConfirmCareerLeave.Visible = true;

            menuConfirmCareerLeave.OnItemSelect.connect(function (sender, item, index) {
                API.triggerServerEvent("CS_LEAVE_CAREER_CONFIRMATION", index);
            });

            break;

        case 'SC_CLOSE_LEAVE_CAREER_CONFIRM_MENU':
            menuConfirmCareerLeave.Visible = false;
            break;
    }
});

API.onUpdate.connect(function () {
    if (menuPool != null) {
        menuPool.ProcessMenus();
    }
});