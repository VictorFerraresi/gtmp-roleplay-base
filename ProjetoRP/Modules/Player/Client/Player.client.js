var idleCamera = null;
var idleCameraPos = new Vector3(-183.087, 4803.357, 329.569);
var idleCameraPlayerPos = new Vector3(-183.087, 4803.357, 329.569 - 15.0);
var idleCameraLookAt = new Vector3(4.586, 4585.728, 264.119);

API.onResourceStart.connect(function () {
    API.callNative("_TRANSITION_TO_BLURRED", 2.0);
    API.setHudVisible(false);

    API.setEntityPositionFrozen(API.getLocalPlayer(), true);
    API.setEntityPosition(API.getLocalPlayer(), idleCameraPlayerPos);

    idleCamera = API.createCamera(idleCameraPos, new Vector3(0, 0, 0));
    API.pointCameraAtPosition(idleCamera, idleCameraLookAt);
    API.setActiveCamera(idleCamera);

    API.triggerServerEvent("CS_PLAYER_PRELOAD_READY");
});

var charselCamera = null;
var charselPlayerPos = new Vector3(-1150.293, -1513.864, 9.633);
var charselCameraLookAt = new Vector3(-1150.293, -1513.864, 10.633);
var charselCameraPos = new Vector3(-1146.890, -1512.382, 10.633);
var charselPlayerRot = new Vector3(0, 0, 299.432);

API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "SC_DO_CHARSEL") {
        if (idleCamera != null) {
            // Is it enough?
            idleCamera = null;
        }
        API.callNative("_TRANSITION_FROM_BLURRED", 2.0);

        var player = API.getLocalPlayer();
        var charselCamera = API.createCamera(charselCameraPos, new Vector3(0, 0, 0));
        API.pointCameraAtPosition(charselCamera, charselCameraLookAt);

        // API.callNative("SET_PLAYER_CONTROL", API.getLocalPlayer(), false, 0);

        // API.setPlayerSkin(-257153498);
        API.setEntityPosition(API.getLocalPlayer(), charselPlayerPos);
        API.setEntityRotation(API.getLocalPlayer(), charselPlayerRot);

        API.setActiveCamera(charselCamera);
    } else if (eventName == "SC_DO_SPAWN") {
        API.setActiveCamera(null);
        API.setHudVisible(true);
    }
});