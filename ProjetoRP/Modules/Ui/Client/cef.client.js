var mainBrowser = null;
var cursorStatus = 0;

API.onResourceStart.connect(function() {
	var resolution = API.getScreenResolution();
	mainBrowser = API.createCefBrowser(resolution.Width, resolution.Height, true);
	API.waitUntilCefBrowserInit(mainBrowser);
	API.setCefBrowserPosition(mainBrowser, 0, 0);
	API.loadPageCefBrowser(mainBrowser, "Ui/page.html");

	API.triggerServerEvent("CS_UI_PRELOAD_READY");
});

API.onResourceStop.connect(function(e, ev) {
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

API.onKeyDown.connect(function(sender, args) {
	if (args.KeyCode == Keys.F1 && cursorStatus == 0) {
		API.showCursor(!API.isCursorShown());
		API.setCanOpenChat(!API.getCanOpenChat());
	}
});

function triggerEvent(name, data = "") {
	API.triggerServerEvent(name, data);
}

API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "SC_UI_EVAL") {
        mainBrowser.call("eval", args[0]);
    }

    if (eventName == "SC_UI_CURSOR_FIXED_ON") {
    	cursorStatus = 1;
    	API.showCursor(true);
    	API.setCanOpenChat(false);
    }

    if (eventName == "SC_UI_CURSOR_FIXED_OFF") {
    	cursorStatus = 1;
    	API.showCursor(false);
    	API.setCanOpenChat(true);
    }

    if (eventName == "SC_UI_CURSOR_FREE") {
    	cursorStatus = 0;
    	API.showCursor(false);
    	API.setCanOpenChat(true);
    }
});

function debugOutput(text) {
	API.sendChatMessage(text);
}