API.onKeyDown.connect(function (sender, key) {
    if (key.KeyCode == Keys.G) {
        API.triggerServerEvent("CS_SIREN_TOGGLE");        
    }
});