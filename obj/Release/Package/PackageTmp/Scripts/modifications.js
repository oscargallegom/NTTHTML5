function TurnTableOnOff(currentRow) {
    row = currentRow + 1;
    var gv = document.getElementById("MainContent_gvSubarea")
    var cell = 0
    for (i = 1; i <= gv.rows.length - 1; i++) {
        cell = i - 1
        var btn = gv.rows[i].getElementsByTagName("input")["MainContent_gvSubarea_btnShow_" + cell];
        if (i == row) {
            if (gv.rows[i].cells[4].style.display == "") { gv.rows[i].cells[4].style.display = "none"; btn.value = "Show"; }
            else { gv.rows[i].cells[4].style.display = ""; btn.value = "Hide"; }
        }
        else { gv.rows[i].cells[4].style.display = "none"; btn.value = "Show"; }
    }
}
