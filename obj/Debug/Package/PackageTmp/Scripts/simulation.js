function turnOnOffControls() {
    var ddlType = document.getElementById("MainContent_ddlType");
    var divField = document.getElementById("MainContent_divField")
    var divSubproject = document.getElementById("MainContent_divSubproject")

    if (ddlType.selectedIndex == 0) { divField.style.display = ""; divSubproject.style.display = "none"; }
    else { divField.style.display = "none"; divSubproject.style.display = ""; }
}

