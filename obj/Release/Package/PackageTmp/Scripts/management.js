/* Management Functions */
function ValidateInput(txt) {
    if (txt.value < txt.attributes["min"].value || txt.value > txt.attributes["max"].value) {txt.style.backgroundColor = "red"; }
}

function CheckForResArea(ddl, AIorAF) {
    if (AIorAF == "AI") {
        if (ddl[ddl.selectedIndex].value == "8") { document.getElementById("MainContent_txtAIResArea").style.display = ""; document.getElementById("MainContent_lblAIResArea").style.display = ""; }
        else { document.getElementById("MainContent_txtAIResArea").style.display = "none"; document.getElementById("MainContent_lblAIResArea").style.display = "none"; }
        if (ddl[ddl.selectedIndex].value == "7") { document.getElementById("MainContent_txtAISafetyFactor").style.display = ""; document.getElementById("MainContent_lblAISafetyFactor").style.display = ""; }
        else { document.getElementById("MainContent_txtAISafetyFactor").style.display = "none"; document.getElementById("MainContent_lblAISafetyFactor").style.display = "none"; }
    }
    else 
    {
//        if (ddl[ddl.selectedIndex].value == "8") { document.getElementById("MainContent_txtAFResArea").style.display = ""; document.getElementById("MainContent_lblAFResArea").style.display = ""; }
//        else { document.getElementById("MainContent_txtAFResArea").style.display = "none"; document.getElementById("MainContent_lblAFResArea").style.display = "none"; }
        if (ddl[ddl.selectedIndex].value == "7") { document.getElementById("MainContent_txtAFSafetyFactor").style.display = ""; document.getElementById("MainContent_lblAFSafetyFactor").style.display = ""; }
        else { document.getElementById("MainContent_txtAFSafetyFactor").style.display = "none"; document.getElementById("MainContent_lblAFSafetyFactor").style.display = "none"; }
    }
}

function ClearInputs(table) { 
    switch (table) {
        case "tblAI":
            document.getElementById("MainContent_ddlAIIrrigationType").selectedIndex = 0;
            document.getElementById("MainContent_txtAIWaterStress").value = 0.80;
            document.getElementById("MainContent_txtAIIrrigationEfficiancy").value = 0.00;
            document.getElementById("MainContent_txtAIFrequency").value = 0;
            document.getElementById("MainContent_txtAIMaxSingleAppl").value = 3.00;
            document.getElementById("MainContent_txtAIResArea").value = 0.00;
            document.getElementById("MainContent_txtAISafetyFactor").value = 0.00;
            break;
        case "tblAF":
            document.getElementById("MainContent_ddlAFIrrigationType").selectedIndex = 0;
            document.getElementById("MainContent_txtAFWaterStress").value = 0.80;
            document.getElementById("MainContent_txtAFIrrigationEfficiancy").value = 0.00;
            document.getElementById("MainContent_txtAFFrequency").value = 0;
            document.getElementById("MainContent_txtAFMaxSingleAppl").value = 3.00;
            document.getElementById("MainContent_txtAFNConc").value = 0.00;
            document.getElementById("MainContent_txtAFSafetyFactor").value = 0.00;
            break;
        case "tblTD":
            document.getElementById("MainContent_txtTDDepth").value = 0.00;
            break;
        case "tblPPND":
            document.getElementById("MainContent_txtPPNDWidth").value = 0.00;
            document.getElementById("MainContent_txtPPNDSides").value = 0.00;
            break;
        case "tblPPDS":
            document.getElementById("MainContent_txtPPDSWidth").value = 0.00;
            document.getElementById("MainContent_txtPPDSSides").value = 0.00;
            break;
        case "tblPPDE":
            document.getElementById("MainContent_txtPPDEWidth").value = 0.00;
            document.getElementById("MainContent_txtPPDESides").value = 0.00;
            document.getElementById("MainContent_txtPPDEResArea").value = 0.00;
            break;
        case "tblPPTW":
            document.getElementById("MainContent_txtPPTWWidth").value = 0.00;
            document.getElementById("MainContent_txtPPTWSides").value = 0.00;
            document.getElementById("MainContent_txtPPTWResArea").value = 0.00;
            break;
        case "tblWL":
            document.getElementById("MainContent_txtWLArea").value = 0.00;
            break;
        case "tblPND":
            document.getElementById("MainContent_txtPNDFraction").value = 0.00;
            break;
        case "tblSF":
            document.getElementById("MainContent_txtSFAnimals").value = 0.00;
            document.getElementById("MainContent_txtSFDays").value = 0.00;
            document.getElementById("MainContent_txtSFHours").value = 0.00;
            document.getElementById("MainContent_txtSFManure").value = 0.00;
            document.getElementById("MainContent_ddlSFType").selectedIndex = 0.00;
            document.getElementById("MainContent_txtSFNO3").value = 0.00;
            document.getElementById("MainContent_txtSFPO4").value = 0.00;
            document.getElementById("MainContent_txtSFOrgN").value = 0.00;
            document.getElementById("MainContent_txtSFOrgP").value = 0.00;
            break;
        case "tblSBS":
            document.getElementById("MainContent_chkSBS").checked = false;
            break;
        case "tblRF":
            document.getElementById("MainContent_txtRFArea").value = 0.00;
            document.getElementById("MainContent_txtRFWidth").value = 0.00;
            document.getElementById("MainContent_txtRFGrass").value = 0.00;
            document.getElementById("MainContent_txtRFRatio").value = 0.00;
            break;
        case "tblFS":
            document.getElementById("MainContent_txtFSArea").value = 0.00;
            document.getElementById("MainContent_txtFSWidth").value = 0.00;
            document.getElementById("MainContent_txtFSRatio").value = 0.00;
            document.getElementById("MainContent_ddlFSType").selectedIndex = 0.00;
            break;
        case "tblWW":
            document.getElementById("MainContent_txtWWWidth").value = 0.00;
            document.getElementById("MainContent_ddlWWType").selectedIndex = 0.00;
            break;
        case "tblCB":
            document.getElementById("MainContent_txtCBBufferWidth").value = 0.00;
            document.getElementById("MainContent_txtCBCropWidth").value = 0.00;
            document.getElementById("MainContent_ddlCBType").selectedIndex = 0.00;
            break;
        case "tblLL":
            document.getElementById("MainContent_txtLLReduction").value = 0.00;
            break;
        case "tblTS":
            document.getElementById("MainContent_chkTS").checked = false;
            break;
        case "tblLM":
            document.getElementById("MainContent_chkLM").checked = false;
            break;
        case "tblAoC":
            document.getElementById("MainContent_chkAoC").checked = false;
            break;
        case "tblGC":
            document.getElementById("MainContent_chkGC").checked = false;
            break;
        case "tblSA":
            document.getElementById("MainContent_chkSA").checked = false;
            break;
        case "tblSdg":
            document.getElementById("MainContent_txtSdgArea").value = 0.00;
            document.getElementById("MainContent_txtSdgWidth").value = 0.00;
            document.getElementById("MainContent_txtSdgSlopeRatio").value = 0.00;
            document.getElementById("MainContent_ddlSdgCrop").selectedIndex = 0.00;
            break;
    }
    return false;
}

function hideDiv(div) {
    document.getElementById(div).style.display = "none";
}

function showModal() {
    window.location = "#OperationsModal";
}

function SaveChecked(chkBox, row) {
    if (chkBox.checked == true) { row.cells[0].children[1].value = "True"; }
    else { row.cells[0].children[1].value = "False"; }
}

function SetSaveOn() {
    document.getElementById("MainContent_ctlSave").value = "Yes";
}

function onClick(element) {
    SetSaveOn();
    var rowIndex = element;
    var activeRow = document.getElementById('MainContent_gvOperations').rows[rowIndex + 1];
    SaveChecked(activeRow.cells[0].children[0], activeRow);
    var operNumber = activeRow.cells[2].children[0].value;
    var gvTitles = document.getElementById('MainContent_gvOperations').getElementsByTagName('th');
    var crops = document.getElementsByTagName('ctl00$MainContent$gvOperations$ctl06$ddlCrop');
    var ddlcrop = "MainContent_gvOperations_ddlCrop_" + rowIndex
    crops = document.getElementById(ddlcrop);
    gvTitles[6].innerText = "";
    gvTitles[7].innerText = "";
    gvTitles[8].innerText = "";
    gvTitles[9].innerText = "";
    gvTitles[10].innerText = "";
    gvTitles[11].innerText = "";
    gvTitles[12].innerText = "";
    gvTitles[6].innerHTML = "";
    gvTitles[7].innerHTML = "";
    gvTitles[8].innerHTML = "";
    gvTitles[9].innerHTML = "";
    gvTitles[10].innerHTML = "";
    gvTitles[11].innerHTML = "";
    gvTitles[12].innerHTML = "";
    activeRow.cells[6].disabled = "disabled";
    activeRow.cells[7].children[0].readOnly = true;
    activeRow.cells[8].children[0].readOnly = true;
    activeRow.cells[9].children[0].readOnly = true;
    activeRow.cells[10].children[0].readOnly = true;
    activeRow.cells[11].children[0].readOnly = true;
    activeRow.cells[12].children[0].readOnly = true;
    switch (operNumber) {
        case "Planting":
            gvTitles[6].innerText = "Planting";
            gvTitles[7].innerText = "Plant Population";
            gvTitles[6].innerHTML = "Planting";
            gvTitles[7].innerHTML = "Plant Population";
            activeRow.cells[6].disabled = "";
            activeRow.cells[7].children[0].readOnly = false;
            break;
        case "Fertilizer":
            gvTitles[6].innerText = "Fertilizer";
            gvTitles[6].innerHTML = "Fertilizer";
            gvTitles[7].innerText = "Amount(lbs/ac)";
            gvTitles[8].innerText = "Depth(in)";
            gvTitles[9].innerText = "NO3";
            gvTitles[10].innerText = "PO4";
            gvTitles[11].innerText = "Org N";
            gvTitles[12].innerText = "Org P";
            gvTitles[7].innerHTML = "Amount(lbs/ac)";
            gvTitles[8].innerHTML = "Depth(in)";
            gvTitles[9].innerHTML = "NO3";
            gvTitles[10].innerHTML = "PO4";
            gvTitles[11].innerHTML = "Org N";
            gvTitles[12].innerHTML = "Org P";
            activeRow.cells[6].disabled = "";
            activeRow.cells[7].children[0].readOnly = false;
            activeRow.cells[8].children[0].readOnly = false;
            activeRow.cells[9].children[0].readOnly = false;
            activeRow.cells[10].children[0].readOnly = false;
            activeRow.cells[11].children[0].readOnly = false;
            activeRow.cells[12].children[0].readOnly = false;
            break;
        case "Tillage":
            gvTitles[6].innerText = "Tillage Method";
            gvTitles[6].innerHTML = "Tillage Method";
            activeRow.cells[6].disabled = "";
            break;
        case "Harvest":
            gvTitles[6].innerText = "Harvest";
            gvTitles[6].innerHTML = "Harvest";
            break;
        case "Kill":
            gvTitles[6].innerText = "Kill";
            gvTitles[6].innerHTML = "Kill";
            break;
        case "Irrigation (Manual)":
            gvTitles[6].innerText = "Irrigation Method";
            gvTitles[7].innerText = "Volume(in)";
            gvTitles[8].innerText = "Efficiency(0-1)";
            gvTitles[6].innerHTML = "Irrigation Method";
            gvTitles[7].innerHTML = "Volume(in)";
            gvTitles[8].innerHTML = "Efficiency(0-1)";
            activeRow.cells[6].disabled = "";
            activeRow.cells[7].children[0].readOnly = false;
            activeRow.cells[8].children[0].readOnly = false;
            break;
        case "Start Grazing":
            gvTitles[6].innerText = "Animal";
            gvTitles[7].innerText = "Amount";
            gvTitles[8].innerText = "Hours in field(0-24)";
            gvTitles[9].innerText = "NO3";
            gvTitles[10].innerText = "PO4";
            gvTitles[11].innerText = "Org N";
            gvTitles[12].innerText = "Org P";
            gvTitles[6].innerHTML = "Animal";
            gvTitles[7].innerHTML = "Amount";
            gvTitles[8].innerHTML = "Hours in field(0-24)";
            gvTitles[9].innerHTML = "NO3";
            gvTitles[10].innerHTML = "PO4";
            gvTitles[11].innerHTML = "Org N";
            gvTitles[12].innerHTML = "Org P";
            activeRow.cells[6].disabled = "";
            activeRow.cells[7].children[0].readOnly = false;
            activeRow.cells[8].children[0].readOnly = false;
            activeRow.cells[9].children[0].readOnly = false;
            activeRow.cells[10].children[0].readOnly = false;
            activeRow.cells[11].children[0].readOnly = false;
            activeRow.cells[12].children[0].readOnly = false;
            break;
        case "Stop Grazing":
            gvTitles[6].innerText = "Stop Grazing"
            gvTitles[7].innerText = "";
            gvTitles[8].innerText = "";
            gvTitles[6].innerHTML = "Stop Grazing"
            gvTitles[7].innerHTML = "";
            gvTitles[8].innerHTML = "";
            break;
        case "Burn":
            gvTitles[6].innerText = "Burn"
            gvTitles[7].innerText = "";
            gvTitles[8].innerText = "";
            gvTitles[6].innerHTML = "Burn"
            gvTitles[7].innerHTML = "";
            gvTitles[8].innerHTML = "";
            break;
    }
}

