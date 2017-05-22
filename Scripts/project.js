function TurnOnSection(sct) {
    document.getElementById(sct).style.display = "";
    switch (sct) {
//        case "MainContent_sctNew":
//            document.getElementById("MainContent_sctExample").style.display = "none";
//            document.getElementById("MainContent_sctOpen").style.display = "none";
//            //document.getElementById("MainContent_txtDate").innerHTML = new Date();
            //document.getElementById("MainContent_txtDate").innerText = new Date();
//            break; 
        case "MainContent_sctExample":
            document.getElementById("MainContent_sctNew").style.display = "none";
            document.getElementById("MainContent_sctOpen").style.display = "none";
            break;
        case "MainContent_sctOpen":
            document.getElementById("MainContent_sctNew").style.display = "none";
            document.getElementById("MainContent_sctExample").style.display = "none";
            break;
    }
}
function btnCloseProject_ClientClick() {
    var answer = confirm(document.getElementById("MainContent_btnCloseProject").title);
    if (answer) { return true; }
    return false;
}

function showModal() {
    window.location = "#openModalIns";
}


