function allowDrop(ev) {
   ev.preventDefault();
}

function drag(ev) {
    ev.dataTransfer.setData("drag", ev.target.id);
    //alert("a");
}

function drop(ev) {
    ev.preventDefault();
    var target = ev.target.id;
    var source = ev.dataTransfer.getData("drag");

    if(source.split('-')[0] == "slot") {
    	if(target.split('-')[0] == "slot") {
    		inv_app.slotToSlot(source.split('-')[1], target.split('-')[1]);
    	}
    } else if(source == "split") {
    	inv_app.split(target.split('-')[1]);
    }
}
