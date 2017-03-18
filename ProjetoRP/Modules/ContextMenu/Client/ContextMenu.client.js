var mainBrowser = null;
var menuStatus = 0;

API.onResourceStart.connect(function () {
    var resolution = API.getScreenResolution();
    mainBrowser = API.createCefBrowser(resolution.Width / 2, resolution.Height / 2, true);
    API.waitUntilCefBrowserInit(mainBrowser);
    API.setCefBrowserPosition(mainBrowser, resolution.Width / 2, resolution.Height / 2);
    API.loadPageCefBrowser(mainBrowser, "ContextMenu/page.html");
});

API.onResourceStop.connect(function (e, ev) {
    if (mainBrowser != null) {
        API.destroyCefBrowser(mainBrowser);
    }
});

// API.onUpdate.connect(function(s,e) {
// 	if (API.isControlPressed(19)) {
// 		API.showCursor(true);
// 	} else {
// 		API.showCursor(false);
// 	}
// });

function isPressed() {
    return API.isControlPressed(179) && !API.isControlPressed(22);
}

API.onUpdate.connect(function () {
    var pressed = isPressed();
    if (pressed && menuStatus == 0) {
        menuStatus = 1;
        mainBrowser.call("show");
        API.showCursor(true);
    }

    if (!pressed && menuStatus == 1) {
        menuStatus = 0;
        mainBrowser.call("hide");
        API.showCursor(false);
    }
        /*var aimPos = API.getPlayerAimingPoint(API.getLocalPlayer());     //getting aiming position in 3d world
        var camPos = API.getGameplayCamPos();                            //getting camera position in 3d world

        aimPos = new Vector3(
                                ((aimPos.X - camPos.X) * 12) + camPos.X,
                                ((aimPos.Y - camPos.Y) * 12) + camPos.Y,
                                ((aimPos.Z - camPos.Z) * 12) + camPos.Z
                            );    //Set aimPos 12 times far away using camPos, because getPlayerAimingPoint does not always hit objects

        var rayCast = API.createRaycast(camPos, aimPos, 30, API.getLocalPlayer());

        if (rayCast.didHitEntity)//Is Raycast hits a (gta:n) entity
        {
            // var hitPlayer = rayCast.hitEntity; //extract the player object
            // API.sendChatMessage("You are aiming at " + API.getPlayerName(hitPlayer) + " right now!");
        }*/
});
