API.onUpdate.connect(function () {

    if (API.isControlPressed(179) && !API.isControlPressed(22))// Basically middle mouse button
    {
        API.sendChatMessage("Test");
        var aimPos = API.getPlayerAimingPoint(API.getLocalPlayer());     //getting aiming position in 3d world
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
        }
    }
});