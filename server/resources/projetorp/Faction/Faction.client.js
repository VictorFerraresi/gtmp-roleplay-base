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