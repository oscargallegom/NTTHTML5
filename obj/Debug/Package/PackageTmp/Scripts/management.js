/* Management Functions */
function ValidateInput(txt) {
    if (txt.value < txt.attributes["min"].value || txt.value > txt.attributes["max"].value) {txt.style.backgroundColor = "red"; }
}

function CheckForResArea(ddl, AIorAF) {
    if (AIorAF == "AI") {
        if (ddl[ddl.selectedIndex].value == "8") { document.getElementById("MainContent_txtAIResArea").style.display = ""; document.getElementById("MainContent_lblAIResArea").style.display = ""; }
        else { document.getElementById("MainContent_txtAIResArea").style.display = "none"; document.getElementById("MainContent_lblAIResArea").style.display = "none"; }
        if (ddl[ddl.selectedIndex].value == "7") { document.getElementById("MainContent_txtAISafetyFactor").style.display = "none"; document.getElementById("MainContent_lblAISafetyFactor").style.display = "none"; }
        else { document.getElementById("MainContent_txtAISafetyFactor").style.display = "none"; document.getElementById("MainContent_lblAISafetyFactor").style.display = "none"; }
    }
    else 
    {
        if (ddl[ddl.selectedIndex].value == "7") { document.getElementById("MainContent_txtAFSafetyFactor").style.display = "none"; document.getElementById("MainContent_lblAFSafetyFactor").style.display = "none"; }
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
            document.getElementById("MainContent_txtAFNConc").value = 300.00;
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
        case "tblMC":
            document.getElementById("MainContent_txtmcNO3N").value = 0.00;   
            document.getElementById("MainContent_txtmcOrgN").value = 0.00;   
            document.getElementById("MainContent_txtmcPO4P").value = 0.00;   
            document.getElementById("MainContent_txtmcOrgP").value = 0.00;   
            document.getElementById("MainContent_txtmcOM").value = 0.00;   
            break;
        case "tblCC":
            document.getElementById("MainContent_txtCCMinTmp").value = 0.00;
            document.getElementById("MainContent_txtCCMaxTmp").value = 0.00;
            document.getElementById("MainContent_txtCCPcp").value = 0.00;
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

function hideDivTvMain(div) {
    var tvmainText = document.getElementById("MainContent_aTvMain").innerHTML;
    switch (tvmainText) {
        case "Hide":
            document.getElementById(div).style.display = "none";
            document.getElementById("MainContent_aTvMain").innerHTML = "Show"; 
            break;
        case "Ocultar":
            document.getElementById(div).style.display = "none";
            document.getElementById("MainContent_aTvMain").innerHTML = "Mostrar";
            break;
        case "Show":
            document.getElementById(div).style.display = "";
            document.getElementById("MainContent_aTvMain").innerHTML = "Hide";
            break;
        case "Mostrar":
            document.getElementById(div).style.display = "";
            document.getElementById("MainContent_aTvMain").innerHTML = "Ocultar";
            break;
    }
}

function SaveChecked(chkBox, row) {
    if (chkBox.checked == true) { row.cells[0].children[0].value = "True"; }
    else { row.cells[0].children[0].value = "False"; }
}

function SetSaveOn() {
    document.getElementById("MainContent_ctlSave").value = "Yes";
}

function onBlur(element, col) {
    for (j = 2; j <= document.getElementById('MainContent_gvOperations').rows.length-1; j++) {
        var activeRow = document.getElementById('MainContent_gvOperations').rows[j];
        for (i = 0; i <= 13; i++) {
            if (!(col == i && j == element+2)) {
                if (activeRow.cells[i].children.length > 1) {
                    activeRow.cells[i].children[0].style.display = ""; activeRow.cells[i].removeChild(activeRow.cells[i].children[1]); 
                }
            }
        }
    }
}

// when click on row.
function onClick(element) {
    SetSaveOn();
    var rowIndex = element;
    var activeRow = document.getElementById('MainContent_gvOperations').rows[rowIndex + 2];
    SaveChecked(activeRow.cells[0].children[0], activeRow);
    var operNumber = activeRow.cells[2].children[0].value;
    var typeNumber = activeRow.cells[6].children[0].value;
    var gvTitles = document.getElementById('MainContent_gvOperations').rows[1];  //get second line of titles
    //var gvTitles = document.getElementById('MainContent_gvOperations').getElementsByTagName('th');
    gvTitles.cells[6].innerText = "";
    gvTitles.cells[7].innerText = "";
    gvTitles.cells[8].innerText = "";
    gvTitles.cells[9].innerText = "";
    gvTitles.cells[10].innerText = "";
    gvTitles.cells[11].innerText = "";
    gvTitles.cells[12].innerText = "";
    gvTitles.cells[13].innerText = "";
    gvTitles.cells[6].innerHTML = "";
    gvTitles.cells[7].innerHTML = "";
    gvTitles.cells[8].innerHTML = "";
    gvTitles.cells[9].innerHTML = "";
    gvTitles.cells[10].innerHTML = "";
    gvTitles.cells[11].innerHTML = "";
    gvTitles.cells[12].innerHTML = "";
    gvTitles.cells[13].innerHTML = "";
    activeRow.cells[6].disabled = "disabled";
    activeRow.cells[7].children[0].readOnly = true;
    activeRow.cells[8].children[0].readOnly = true;
    activeRow.cells[9].children[0].readOnly = true;
    activeRow.cells[10].children[0].readOnly = true;
    activeRow.cells[11].children[0].readOnly = true;
    activeRow.cells[12].children[0].readOnly = true;
    activeRow.cells[13].children[0].readOnly = true;
    switch (operNumber) {
        case 'Planting':
            gvTitles.cells[6].innerText = "Planting";
            gvTitles.cells[7].innerText = "Plant Population&#10;(seeds/ac)";
            gvTitles.cells[6].innerHTML = "Planting";
            gvTitles.cells[7].innerHTML = "Plant Population&#10;(seeds/ac)";
            activeRow.cells[6].disabled = "";
            if (activeRow.cells[1].children[0].attributes["cropValue"].value != "367") { activeRow.cells[7].children[0].readOnly = false; }
            break;
        case 'Plantar':
            gvTitles.cells[6].innerText = "Plantar";
            gvTitles.cells[7].innerText = "Población de la Planta&#10;(semillas/ac)";
            gvTitles.cells[6].innerHTML = "Plantar";
            gvTitles.cells[7].innerHTML = "Población de la Planta&#10;(semillas/ac)";
            activeRow.cells[6].disabled = "";
            activeRow.cells[7].children[0].readOnly = false;
            break;
        case "Fertilizer":
            gvTitles.cells[6].innerText = "Fertilizer";
            gvTitles.cells[6].innerHTML = "Fertilizer";            
            gvTitles.cells[8].innerText = "Depth(in)";
            gvTitles.cells[9].innerText = "NO3";
            gvTitles.cells[10].innerText = "PO4";
            gvTitles.cells[11].innerText = "Org N";
            gvTitles.cells[12].innerText = "Org P";
            gvTitles.cells[13].innerText = "K";
            gvTitles.cells[14].innerText = "NH3";
            if (typeNumber == "Liquid Manure Fert.") { gvTitles.cells[7].innerHTML = "Volume&#10;(lbs/gl)"; gvTitles.cells[7].innerText = "Volume&#10;(lbs/gl)"; }
            else { gvTitles.cells[7].innerText = "Amount&#10;(lbs/ac)"; gvTitles.cells[7].innerHTML = "Amount&#10;(lbs/ac)"; }
            gvTitles.cells[8].innerHTML = "Depth(in)";
            gvTitles.cells[9].innerHTML = "NO3";
            gvTitles.cells[10].innerHTML = "PO4";
            gvTitles.cells[11].innerHTML = "Org N";
            gvTitles.cells[12].innerHTML = "Org P";
            gvTitles.cells[13].innerHTML = "K";
            gvTitles.cells[14].innerHTML = "NH3";
            activeRow.cells[6].disabled = "";
            activeRow.cells[7].children[0].readOnly = false;
            activeRow.cells[8].children[0].readOnly = false;
            activeRow.cells[9].children[0].readOnly = false;
            activeRow.cells[10].children[0].readOnly = false;
            activeRow.cells[11].children[0].readOnly = false;
            activeRow.cells[12].children[0].readOnly = false;
            activeRow.cells[13].children[0].readOnly = false;
            activeRow.cells[14].children[0].readOnly = false;
            break;
        case "Fertilizante":
            gvTitles.cells[6].innerText = "Fertilizante";
            gvTitles.cells[6].innerHTML = "Fertilizante";            
            gvTitles.cells[8].innerText = "Profundidad(in)";
            gvTitles.cells[9].innerText = "NO3";
            gvTitles.cells[10].innerText = "PO4";
            gvTitles.cells[11].innerText = "Org N";
            gvTitles.cells[12].innerText = "Org P";
            gvTitles.cells[13].innerText = "K";
            gvTitles.cells[14].innerText = "NH3";
            if (typeNumber == "Liquid Manure Fert.") { gvTitles.cells[7].innerHTML = "Volumen&#10;(lbs/gl)"; gvTitles.cells[7].innerText = "Volumen&#10;(lbs/gl)" }
            else { gvTitles.cells[7].innerHTML = "Cantidad&#10;(lbs/ac)"; gvTitles.cells[7].innerText = "Cantidad&#10;(lbs/ac)"; }
            gvTitles.cells[8].innerHTML = "Profunidad(in)";
            gvTitles.cells[9].innerHTML = "NO3";
            gvTitles.cells[10].innerHTML = "PO4";
            gvTitles.cells[11].innerHTML = "Org N";
            gvTitles.cells[12].innerHTML = "Org P";
            gvTitles.cells[13].innerHTML = "K";
            gvTitles.cells[14].innerHTML = "NH3";
            activeRow.cells[6].disabled = "";
            activeRow.cells[7].children[0].readOnly = false;
            activeRow.cells[8].children[0].readOnly = false;
            activeRow.cells[9].children[0].readOnly = false;
            activeRow.cells[10].children[0].readOnly = false;
            activeRow.cells[11].children[0].readOnly = false;
            activeRow.cells[12].children[0].readOnly = false;
            activeRow.cells[13].children[0].readOnly = false;
            activeRow.cells[14].children[0].readOnly = false;
            break;
        case "Tillage":
            gvTitles.cells[6].innerText = "Tillage Method";
            gvTitles.cells[6].innerHTML = "Tillage Method";
            activeRow.cells[6].disabled = "";
            break;
        case "Arar":
            gvTitles.cells[6].innerText = "Método de Arar";
            gvTitles.cells[6].innerHTML = "Método de Arar";
            activeRow.cells[6].disabled = "";
            break;
        case "Harvest":
            gvTitles.cells[6].innerText = "Harvest";
            gvTitles.cells[6].innerHTML = "Harvest";
            break;
        case "Cosechar":
            gvTitles.cells[6].innerText = "Cosechar";
            gvTitles.cells[6].innerHTML = "Cosechar";
            break;
        case "Kill":
            gvTitles.cells[6].innerText = "Kill";
            gvTitles.cells[6].innerHTML = "Kill";
            break;
        case "Terminar":
            gvTitles.cells[6].innerText = "Terminar";
            gvTitles.cells[6].innerHTML = "Terminar";
            break;
        case "Irrigation (Manual)":
            gvTitles.cells[6].innerText = "Irrigation Method";
            gvTitles.cells[7].innerText = "Volume(in)";
            gvTitles.cells[8].innerText = "Efficiency(0-1)";
            gvTitles.cells[6].innerHTML = "Irrigation Method";
            gvTitles.cells[7].innerHTML = "Volume(in)";
            gvTitles.cells[8].innerHTML = "Efficiency(0-1)";
            activeRow.cells[6].disabled = "";
            activeRow.cells[7].children[0].readOnly = false;
            activeRow.cells[8].children[0].readOnly = false;
        case "Irrigación (Manual)":
            gvTitles.cells[6].innerText = "Método de Irrigación";
            gvTitles.cells[7].innerText = "Volumen(in)";
            gvTitles.cells[8].innerText = "Eficiencia(0-1)";
            gvTitles.cells[6].innerHTML = "Método de Irrigación";
            gvTitles.cells[7].innerHTML = "Volumen(in)";
            gvTitles.cells[8].innerHTML = "Eficiencia(0-1)";
            activeRow.cells[6].disabled = "";
            activeRow.cells[7].children[0].readOnly = false;
            activeRow.cells[8].children[0].readOnly = false;
            break;
        case "Start Grazing":
            gvTitles.cells[6].innerText = "Animal";
            gvTitles.cells[7].innerText = "# of Animals";
            gvTitles.cells[8].innerText = "Hours in Field(0-24)";
            gvTitles.cells[9].innerText = "NO3";
            gvTitles.cells[10].innerText = "PO4";
            gvTitles.cells[11].innerText = "Org N";
            gvTitles.cells[12].innerText = "Org P";
            gvTitles.cells[13].innerText = "NH3";
            gvTitles.cells[6].innerHTML = "Animal";
            gvTitles.cells[7].innerHTML = "# of Animals";
            gvTitles.cells[8].innerHTML = "Hours in Field(0-24)";
            gvTitles.cells[9].innerHTML = "NO3";
            gvTitles.cells[10].innerHTML = "PO4";
            gvTitles.cells[11].innerHTML = "Org N";
            gvTitles.cells[12].innerHTML = "Org P";
            gvTitles.cells[13].innerHTML = "NH3";
            activeRow.cells[6].disabled = "";
            activeRow.cells[7].children[0].readOnly = false;
            activeRow.cells[8].children[0].readOnly = false;
            activeRow.cells[9].children[0].readOnly = false;
            activeRow.cells[10].children[0].readOnly = false;
            activeRow.cells[11].children[0].readOnly = false;
            activeRow.cells[12].children[0].readOnly = false;
            activeRow.cells[13].children[0].readOnly = false;
            break;
        case 'Iniciar Pasteo':
            gvTitles.cells[6].innerText = "Animal";
            gvTitles.cells[7].innerText = "# de Animales";
            gvTitles.cells[8].innerText = "Horas en Campo(0-24)";
            gvTitles.cells[9].innerText = "NO3";
            gvTitles.cells[10].innerText = "PO4";
            gvTitles.cells[11].innerText = "Org N";
            gvTitles.cells[12].innerText = "Org P";
            gvTitles.cells[13].innerText = "NH3";
            gvTitles.cells[6].innerHTML = "Animal";
            gvTitles.cells[7].innerHTML = "# de Animales";
            gvTitles.cells[8].innerHTML = "Horas en Campo(0-24)";
            gvTitles.cells[9].innerHTML = "NO3";
            gvTitles.cells[10].innerHTML = "PO4";
            gvTitles.cells[11].innerHTML = "Org N";
            gvTitles.cells[12].innerHTML = "Org P";
            gvTitles.cells[13].innerHTML = "NH3";
            activeRow.cells[6].disabled = "";
            activeRow.cells[7].children[0].readOnly = false;
            activeRow.cells[8].children[0].readOnly = false;
            activeRow.cells[9].children[0].readOnly = false;
            activeRow.cells[10].children[0].readOnly = false;
            activeRow.cells[11].children[0].readOnly = false;
            activeRow.cells[12].children[0].readOnly = false;
            activeRow.cells[13].children[0].readOnly = false;
            break;
        case "Stop Grazing":
            gvTitles.cells[6].innerText = "Stop Grazing"
            gvTitles.cells[6].innerHTML = "Stop Grazing"
            break;
        case 'Terminar Pasteo':
            gvTitles.cells[6].innerText = "Terminar Pasteo"
            gvTitles.cells[6].innerHTML = "Terminar Pasteo"
            break;
        case "Burn":
            gvTitles.cells[6].innerText = "Burn"
            gvTitles.cells[6].innerHTML = "Burn"
            break;
        case "Quema":
            gvTitles.cells[6].innerText = "Quema"
            gvTitles.cells[6].innerHTML = "Quema"
            break;
        case "Liming":
            gvTitles.cells[6].innerText = "Liming"
            gvTitles.cells[7].innerText = "Amount&#10;(t/ac)";
            gvTitles.cells[6].innerHTML = "Liming"
            gvTitles.cells[7].innerHTML = "Amount&#10;(t/ac)";
            activeRow.cells[7].children[0].readOnly = false;
            break;
        case "Encalado":
            gvTitles.cells[6].innerText = "Encalado"
            gvTitles.cells[7].innerText = "Cantidad&#10;(t/ac)";
            gvTitles.cells[6].innerHTML = "Encalado"
            gvTitles.cells[7].innerHTML = "Cantidad&#10;(t/ac)";
            activeRow.cells[7].children[0].readOnly = false;
            break;
    }
}

