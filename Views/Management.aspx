<%@ Page Title="Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Management.aspx.vb" Inherits="NTTHTML5.Management" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <script type="text/javascript" src="<%= ResolveUrl ("~/Scripts/management.js") %>"></script>  
     <script type="text/javascript">
         function showModal1(show) {
             if (show == false) { window.location = "#ddlModalFerts"; }
             else {
                 var ddlType1 = document.getElementById("ddlType1");
                 var ddl = document.getElementById("ddlFerts");
                 ddl.length = 0;                 
                 var ddlTypes = document.getElementById("MainContent_ddlTypes");
                 var type = parseInt(ddlType1.value.split(",")[0]);  //this is 39 for commercial, 56 for solid manure and 57 for liquid manure. 39 is just an example. the real one depend on the selection
                 var type2 = 0
                 if (type == 56) { type2 = 40; }
                // add select one option in the list.
                 var option = document.createElement("option");
                 option.text = "Select One";
                 option.value = 0;
                 ddl.add(option);
                 for (i = 1; i < ddlTypes.length; i++) {
                     var values = ddlTypes[i].value.split(",");
                     if (values[1].split("|")[0] == "NUTC" && parseInt(values[0]) <= type && parseInt(values[0]) > type2) {
                         var option = document.createElement("option");
                         option.text = ddlTypes[i].text;
                         option.value = ddlTypes[i].value;
                         ddl.add(option);
                     }
                 }
             }
             window.location = "#ddlModalFerts";
         }


         function triggerFileUpload() {
             document.getElementById("MainContent_flUpload").click();
         }

         function calculatePP(per) {
             if (per.value > 0) {
                 var index = per.parentNode.parentNode.rowIndex;
                 var gv = document.getElementById("MainContent_gvCrops");
                 gv.rows[index].cells[4].children[0].value = per.value * gv.rows[index].cells[5].children[0].value / 100;
            }
         }

         function showModal(show) 
         {
             if (show == true) { window.location = "#MixCropsModal"; }
             else {
                 var gv = document.getElementById("MainContent_gvOperations");
                 var gc = document.getElementById("MainContent_gvCrops");
                 for (i = 1; i < gv.rows.length; i++) {
                     if (gv.rows[i].cells[0].children[0].checked == true) {
                         gv.rows[i].cells[0].children[0].checked = false;
                         gv.rows[i].cells[0].children[1].value = "False"
                         if (gv.rows[i].cells[1].children[0].attributes["cropvalue"].value == "367") {
                             var mixedCrops = gv.rows[i].attributes["mixedCrops"].value.split(",");
                             var mixedCropsInfo;
                             for (j = 0; j < mixedCrops.length; j++) {
                                 mixedCropsInfo = mixedCrops[j].split("|");
                                 for (k = 1; k < gc.rows.length; k++) {
                                     if (gc.rows[k].cells[1].children[0].value == mixedCropsInfo[0]) {
                                         gc.rows[k].cells[0].children[0].checked = true;
                                         gc.rows[k].cells[3].children[0].value = mixedCropsInfo[1];
                                         gc.rows[k].cells[4].children[0].value = mixedCropsInfo[2];
                                         break;
                                     }
                                 }
                             }
                             window.location = "#MixCropsModal"; 
                          }
                     }
                 }
             }                    
         }

         function SelectAllOperations() {
             var checked = true;
             var on = "on"
             if (!document.getElementById("chkSelectAll").checked) { checked = false; on = "off"}
             var oprs = document.getElementById("MainContent_gvOperations");
             for (i = 2; i < oprs.rows.length; i++) {
                 oprs.rows[i].cells[0].children[0].checked = checked;
                 oprs.rows[i].cells[0].children[0].value = checked;
             }
         }
         
         function ActivateUpload() {
             __doPostBack("uploadCropRotation", "uploadCropRotation");
         }

         function BlankText(id) {
             document.getElementById(id).value = '';
         }


         function DeleteOperations() {
             var answer = confirm("Are you sure you want to delete these operations?");
             if (answer == true) { __doPostBack('btnDeleteOperation', 'AddDeleteOperation'); }
             else { return false; }
         }

         function DeleteScenario() {
             var answer = confirm("Are you sure you want to delete this scenario?");
             if (answer == true) { __doPostBack("btnDeleteScenario", "DeleteScenario"); }
             else { return false; }
         }

         function TakeNutrients(ddlSFType, type) {
            //Take nutrients for animals in Stream Fencing BMP
             //var ddlSFType = document.getElementById("MainContent_ddlSFType");
             if (type == "SF") {
                 var nuts = ddlSFType[ddlSFType.selectedIndex].value.split("|");
                 document.getElementById("MainContent_txtSFNO3").value = nuts[2];
                 document.getElementById("MainContent_txtSFOrgN").value = nuts[3];
                 document.getElementById("MainContent_txtSFOrgP").value = nuts[4];
                 document.getElementById("MainContent_txtSFPO4").value = nuts[5];
                 document.getElementById("MainContent_txtSFManure").value = nuts[6];
                 document.getElementById("MainContent_txtSFNH3").value = nuts[7];
             }
             else if (type == "MC") 
                    {
                        var nuts = ddlSFType[ddlSFType.selectedIndex].value.split(",");
                        document.getElementById("MainContent_txtmcNO3N").value = nuts[0];
                        document.getElementById("MainContent_txtmcOrgN").value = nuts[1];
                        document.getElementById("MainContent_txtmcPO4P").value = nuts[2];
                        document.getElementById("MainContent_txtmcOrgP").value = nuts[3];
                        document.getElementById("MainContent_txtmcOM").value = nuts[4];
                        //todo add here the values for each manure control
                    }
             else {
                 var gv = document.getElementById("MainContent_gvOperations");
                 var operation = ddlSFType[ddlSFType.selectedIndex].value.split(",")[1].split("|");
                 gv.rows[lastRow].cells[9].children[0].value = operation[1];
                 gv.rows[lastRow].cells[10].children[0].value = operation[4];
                 gv.rows[lastRow].cells[11].children[0].value = operation[2];
                 gv.rows[lastRow].cells[12].children[0].value = operation[3];
                 gv.rows[lastRow].cells[13].children[0].value = operation[5];
                 gv.rows[lastRow].cells[14].children[0].value = operation[6];
             }
         }

         function changeCursor(lgd) {
            //change cursor 
            var lgd1 = lgd.replace("lbl", "tbl")
            document.getElementById(lgd).style.cursor = "pointer";
        }

        function hideBMP(tbl) {
             //hide a BMP when Hide is clicked
             var table = document.getElementById(tbl.replace("MainContent_l","t"));
             if (table.style.display == "none") { table.style.display = ""; }
             else { table.style.display = "none"; }
         }

         function selectCropping() {
            //Select cropping system
             var cropping = document.getElementById("MainContent_ddlCroppingSystemsAll");
             var croppingNew = document.getElementById("MainContent_ddlCroppingSystems");
             var tills = document.getElementById("ddlTillage");
             for (i = 0; i <= cropping.length - 1; i++) {
                 if (cropping[i].text == croppingNew[croppingNew.selectedIndex].text) {
                     cropping[i + tills.selectedIndex].selected = true;
                     break;
                 }
             }
             __doPostBack("btnUploadCroppingSystem", "UploadCroppingSystem")
         }

         function loadTills() {
            //Load tills for cropping systems
             var tills = document.getElementById("MainContent_ddlTillageAll");
             var tillNew = document.getElementById("ddlTillage");
             tillNew.length = 0;
             var selected = "";
             var croppings = document.getElementById("MainContent_ddlCroppingSystems");
             for (i = 0; i <= tills.length - 1; i++) {
                 if (tills[i].value == croppings[croppings.selectedIndex].text) {
                     var option = document.createElement("option");
                     option.text = tills[i].text;
                     option.value = tills[i].value;
                     tillNew.add(option);
                 }
             }
         }

         function ExpandOrCollapseNode(nodeText) {
             var node = document.getElementById(nodeText).style.display;
             var nodeParent = document.getElementById(nodeText.replace("Nodes", ""));
             switch (node) {
                 case "":
                 case "block":
                     node = "none";
                     break;
                 case "none":
                     node = "block";
                     break;
             }
             document.getElementById(nodeText).style.display = node;
         }

         function turnOnSelected(nodeText, id) {        
             document.getElementById("MainContent_fsetCroppingSystem").style.display = "none";
             document.getElementById("MainContent_fsetLoadCropping").style.display = "none";
             document.getElementById("MainContent_fsetCopyCropping").style.display = "none";
             switch (nodeText) {
                 case "Cropping System":
                 case "Sistema de Cultivo":
                     ExpandOrCollapseNode(id);  //"MainContent_tvMainn0Nodes"
                     break;
                 case "Best Management Practices (BMPs)":
                 case "Buenas Practicas Agrícolas (BPAs)":
                     ExpandOrCollapseNode(id);   //"MainContent_tvMainn6Nodes");
                     break;
                 case "Auto Irrigation and Fertigation":
                 case "Irrigación y Fertirrigación Automáticas":
                     ExpandOrCollapseNode(id);   //7
                     break;
                 case "Drainage Water Management System":
                 case "Manejo de Sistemas de Drenaje":
                     ExpandOrCollapseNode(id);  //10
                     break;
                 case "Wetlands and Ponds":
                 case "Humedales y Lagunas":
                     ExpandOrCollapseNode(id); //16
                     break;
                 case "Stream and Riparian Management":
                 case "Manejo de los rios y riberas":
                     ExpandOrCollapseNode(id);   //19
                     break;
                 case "Contour Buffer (Strip Farming)":
                 case "Buffer de Contorno (Cultivar en Franjas)":
                     ExpandOrCollapseNode(id);  //25
                     break;
                 case "Land Grading and Management":
                 case "Nivelación y Manejo de la Tierra":
                     ExpandOrCollapseNode(id);  //27
                     break;
                 case "Climate Change":
                 case "Cambio Climático":
                     ExpandOrCollapseNode(id);  //29
                     break;
                 case "Roads":
                 case "Carreteras":
                     ExpandOrCollapseNode(id);  //31
                     break;
                 case "Save Current Cropping System":
                 case "Salvar Sistema de Cultivo Activo":
                     SetSaveOn();
                     __doPostBack("btnSaveCropRotation", "SaveCropRotation");
                     break;
                 case "Select Cropping System":
                 case "Seleccionar Sistema de Cultivo": 
                     document.getElementById("MainContent_fsetCroppingSystem").style.display = "";
                     var croppings = document.getElementById("MainContent_ddlCroppingSystemsAll");
                     var croppingNew = document.getElementById("MainContent_ddlCroppingSystems");
                     croppingNew.length = 0;
                     var ant = "";
                     for (i = 0; i <= croppings.length - 1; i++) {
                         if (croppings[i].text != ant) {
                             var option = document.createElement("option");
                             option.text = croppings[i].text;
                             option.value = croppings[i].value;
                             croppingNew.add(option);
                         }
                         ant = croppings[i].text;
                     }
                     croppingNew.selecteIndex = 0;
                     loadTills();
                     break;
                 case "Copy Cropping System":
                 case "Copiar Sistema de Cultivo":
                     document.getElementById("MainContent_fsetCopyCropping").style.display = "";
                     break;
                 case "Upload Saved Cropping System":
                 case "Cargar Sistema de Cultivo Salvado":
                     document.getElementById("MainContent_fsetLoadCropping").style.display = "";
                     break;
                 case "Management Operations":
                 case "Manejo de las Operaciones":
                     document.getElementById("MainContent_fsetOperations").style.display = "";
                     break;
                 case "Auto Irrigation":
                 case "Irrigación Automática":
                     //validate inclusiveness with AF
                     if (document.getElementById("MainContent_fsetAF").style.display == "") {
                         var answer = confirm("Autofertigation is active. You can not have both at the same time. Click Ok. if you want to clear and close Autofertigation")
                         if (answer == false) { document.getElementById("MainContent_fsetAI").style.display = "none"; break; }
                         else { ClearInputs('tblAF'); document.getElementById("MainContent_fsetAF").style.display = "none"; }
                     }
                     document.getElementById("MainContent_fsetAI").style.display = "";
                     break;
                 case "Auto Fertigation":
                 case "Fertirrigación Automática":
                     //validate inclusiveness with AI
                     if (document.getElementById("MainContent_fsetAI").style.display == "") {
                         var answer = confirm("Autoirrigation is active. You can not have both at the same time. Click Ok. if you want to clear and close Autoirrigation")
                         if (answer == false) { document.getElementById("MainContent_fsetAF").style.display = "none"; break; }
                         else { ClearInputs('tblAI'); document.getElementById("MainContent_fsetAI").style.display = "none"; }
                     }
                     document.getElementById("MainContent_fsetAF").style.display = "";
                     break;
                 case "Tile Drain":
                 case "Sistema de Drenaje":
                     document.getElementById("MainContent_fsetTD").style.display = "";
                     break;
                 case "Pads and Pipes - No Ditch Improvement":
                 case "Almohadillas y Tuberías - Ningún Mejoramiento a la Zanja":
                     //validate inclusiveness with others PP
                     if (document.getElementById("MainContent_fsetPPDS").style.display == "" || document.getElementById("MainContent_fsetPPDE").style.display == "" || document.getElementById("MainContent_fsetPPTW").style.display == "") {
                         var answer = confirm("Another Pads & Pipes BMP is active. You can not have more than one Pads & Pipes BMP at the same time. Click Ok. if you want to clear it and close it")
                         if (answer == false) { document.getElementById("MainContent_fsetPPND").style.display = "none"; break; }
                         else {  //clear all of the PP 
                             ClearInputs('tblPPDS'); document.getElementById("MainContent_fsetPPDS").style.display = "none";
                             ClearInputs('tblPPDE'); document.getElementById("MainContent_fsetPPDE").style.display = "none";
                             ClearInputs('tblPPTW'); document.getElementById("MainContent_fsetPPTW").style.display = "none";
                         }
                     }
                     document.getElementById("MainContent_fsetPPND").style.display = "";
                     break;
                 case "Pads and Pipes - Two-Stage Ditch System":
                 case "Almohadillas y Tuberías - Sistema de Zanja de Dos Etapas":
                     //validate inclusiveness with others PP
                     if (document.getElementById("MainContent_fsetPPND").style.display == "" || document.getElementById("MainContent_fsetPPDE").style.display == "" || document.getElementById("MainContent_fsetPPTW").style.display == "") {
                         var answer = confirm("Another Pads & Pipes BMP is active. You can not have more than one Pads & Pipes BMP at the same time. Click Ok. if you want to clear it and close it")
                         if (answer == false) { document.getElementById("MainContent_fsetPPDS").style.display = "none"; break; }
                         else {  //clear all of the PP 
                             ClearInputs('tblPPND'); document.getElementById("MainContent_fsetPPND").style.display = "none";
                             ClearInputs('tblPPDE'); document.getElementById("MainContent_fsetPPDE").style.display = "none";
                             ClearInputs('tblPPTW'); document.getElementById("MainContent_fsetPPTW").style.display = "none";
                         }
                     }
                     document.getElementById("MainContent_fsetPPDS").style.display = "";
                     break;

                 case "Pads and Pipes - Ditch Enlargement and Reservoir System":
                 case "Almohadillas y Tuberías - Ampliación de la Zanja y Sistema de Represa":
                     //validate inclusiveness with others PP
                     if (document.getElementById("MainContent_fsetPPDS").style.display == "" || document.getElementById("MainContent_fsetPPND").style.display == "" || document.getElementById("MainContent_fsetPPTW").style.display == "") {
                         var answer = confirm("Another Pads & Pipes BMP is active. You can not have more than one Pads & Pipes BMP at the same time. Click Ok. if you want to clear it and close it")
                         if (answer == false) { document.getElementById("MainContent_fsetPPND").style.display = "none"; break; }
                         else {  //clear all of the PP 
                             ClearInputs('tblPPDS'); document.getElementById("MainContent_fsetPPDS").style.display = "none";
                             ClearInputs('tblPPND'); document.getElementById("MainContent_fsetPPND").style.display = "none";
                             ClearInputs('tblPPTW'); document.getElementById("MainContent_fsetPPTW").style.display = "none";
                         }
                     }
                     document.getElementById("MainContent_fsetPPDE").style.display = "";
                     break;
                 case "Pads and Pipes - Tailwater Irrigation":
                 case "Almohadillas y Tuberías - Irrigación desde Reserva":
                 //validate inclusiveness with others PP 
                     if (document.getElementById("MainContent_fsetPPDS").style.display == "" || document.getElementById("MainContent_fsetPPDE").style.display == "" || document.getElementById("MainContent_fsetPPND").style.display == "") {
                         var answer = confirm("Another Pads & Pipes BMP is active. You can not have more than one Pads & Pipes BMP at the same time. Click Ok. if you want to clear it and close it")
                         if (answer == false) { document.getElementById("MainContent_fsetPPTW").style.display = "none"; break; }
                         else {  //clear all of the PP 
                             ClearInputs('tblPPDS'); document.getElementById("MainContent_fsetPPDS").style.display = "none";
                             ClearInputs('tblPPDE'); document.getElementById("MainContent_fsetPPDE").style.display = "none";
                             ClearInputs('tblPPND'); document.getElementById("MainContent_fsetPPND").style.display = "none";
                         }
                     }
                     alert("Pads and Pipes - Tailwater Irrigation required Autoirrigations. Please add this BMP")
                     document.getElementById("MainContent_fsetPPTW").style.display = "";
                     document.getElementById("MainContent_fsetAI").style.display = "";
                     break;
                 case "Wetlands":
                 case "Humedales":
                     document.getElementById("MainContent_fsetWL").style.display = "";
                     break;
                 case "Ponds":
                 case "Lagunas":
                     document.getElementById("MainContent_fsetPND").style.display = "";
                     break;
                 case "Stream Fencing (Livestock Access Control)":
                 case "Cercado del rio (Control de Acceso del Ganado)":
                     document.getElementById("MainContent_fsetSF").style.display = "";
                     alert("A filter strip should be added with this BMP");
                     document.getElementById("MainContent_fsetFS").style.display = "";
                     break;
                 case "Streambank Stabilization":
                 case "Estabilización de la Orilla del rio":
                     document.getElementById("MainContent_fsetSBS").style.display = "";
                     break;
                 case "Riparian Forest Buffer":
                 case "Bosque Ribereño":
                     document.getElementById("MainContent_fsetRF").style.display = "";
                     break;
                 case "Filter Strip":
                 case "Zona de Contención Filtrante":
                     document.getElementById("MainContent_fsetFS").style.display = "";
                     break;
                 case "Waterway (Grassed Buffer)":
                 case "Canal de Agua (Zona de Pasto)":
                     document.getElementById("MainContent_fsetWW").style.display = "";
                     break;
                 case "Contour Buffer":
                 case "Buffer de Contorno":
                     document.getElementById("MainContent_fsetCB").style.display = "";
                     break;
                 case "Land Leveling":
                 case "Nivelación de la Tierra":
                     document.getElementById("MainContent_fsetLL").style.display = "";
                     break;
                 case "Terrace System":
                 case "Sistema de Terraza":
                     document.getElementById("MainContent_fsetTS").style.display = "";
                     break;
                 case "Manure Control":
                 case "Control de Estíercol":
                     var rows = document.getElementById("MainContent_gvOperations").rows.length - 1;
                     var mc = false;
                     for (i = 2; i <= rows; i++) {
                         if (document.getElementById("MainContent_gvOperations").rows[i].cells[6].children[0].value.indexOf("Solid") > -1) { mc = true; }
                     }
                     if (mc == true) {
                             document.getElementById("MainContent_fsetMC").style.display = "";
                             document.getElementById("MainContent_tblMC").style.display = "";
                             document.getElementById("MainContent_tblNoMC").style.display = "none";
                         }
                         else {
                             document.getElementById("MainContent_fsetMC").style.display = "";
                             document.getElementById("MainContent_tblMC").style.display = "none";
                             document.getElementById("MainContent_tblNoMC").style.display = "";
                         }
                     break;
                 case "Liming":
                 case "Encalado":
                     document.getElementById("MainContent_fsetLM").style.display = "";
                     break;
                 case "Temperature and Precipitation Changes":
                 case "Cambios de Temperatura y Precipitación":
                     document.getElementById("MainContent_fsetCC").style.display = "";
                     break;
                 case "Asfalt or Concrete":
                 case "Asfalto o Concreto":
                     document.getElementById("MainContent_fsetAoC").style.display = "";
                     break;
                 case "Grass Cover":
                 case "Cubierta de Pasto":
                     document.getElementById("MainContent_fsetGC").style.display = "";
                     break;
                 case "Slope Adjustment":
                 case "Ajuste de la Inclinación":
                     document.getElementById("MainContent_fsetSA").style.display = "";
                     break;
                 case "Shading":
                 case "Shading":
                     document.getElementById("MainContent_fsetSdg").style.display = "";
                     break;                     
             }
         }

         function loadScenarios(ddlFields) {
            //load scenarios linked to the field selected, using list of all scenarios in the project.
             var fieldName = ddlFields(ddlFields.selectedIndex).text;
             var ddlScenarios = document.getElementById("MainContent_ddlScenarios")
             var scenarioName = ddlScenarios(ddlScenarios.selectedIndex).text;
             var ddlAllScenarios = document.getElementById("MainContent_ddlAllScenario");
             var ddlScenarios = document.getElementById("MainContent_ddlFromScenario");
             ddlScenarios.length = 0;
             for (i = 0; i <= ddlAllScenarios.length - 1; i++) {
                 if (ddlAllScenarios[i].value == fieldName && ddlAllScenarios.text != scenarioName) {
                     var option = document.createElement("option");
                     option.text = ddlAllScenarios[i].text;
                     option.value = ddlAllScenarios[i].value;
                     ddlScenarios.appendChild(option);
                 }
             }
         }

         function AddDdls(txt, newRow, cell) {
             //Add a new ddl when a new row is created//
             var lastRow = document.getElementById("MainContent_gvOperations").rows.length - 2
             newRow.addEventListener('click', function () { onClick(lastRow); });
             var newCell = newRow.insertCell();  //add second cell
             var newText = document.createElement("input");
             newText.type="text";
             newText.id = "txt" + txt;
             newRow.setAttribute("RowNumber", lastRow)
             switch (cell) {
                 case 1:            /*crop */
                     newText.setAttribute("class", 'align-left');
                     newText.addEventListener('click', function () { loadDdl(this, 'MainContent_ddl' + txt + 's', cell); });
                     if (lastRow > -1) {
                         newText.value = document.getElementById("MainContent_gvOperations").rows(lastRow).cells(cell).children[0].value
                         newText.setAttribute("CropValue", document.getElementById("MainContent_gvOperations").rows(lastRow).cells(cell).children[0].attributes["CropValue"].value);
                     }
                     break;
                 case 2:            /*operation */
                     newText.setAttribute("class", 'align-left');
                     newText.addEventListener('click', function () { loadDdl(this, 'MainContent_ddl' + txt + 's', cell); });
                     newText.value = "Select One";
                     newText.setAttribute("OperValue", 0);
                     break;
                 case 3:            /*Year */
                     newText.setAttribute("class", 'gvTextBoxSmall')
                     newText.addEventListener('click', function () { loadDdl(this, 'MainContent_ddl' + txt + 's', cell); });
                     if (lastRow > -1) {
                         newText.value = document.getElementById("MainContent_gvOperations").rows(lastRow).cells(cell).children[0].value
                     }
                     break;
                 case 4:
                     newText.setAttribute("class", 'gvTextBoxSmall')
                     newText.addEventListener('click', function () { loadDdl(this, 'MainContent_ddl' + txt + 's', cell); });
                     if (lastRow > -1) { newText.value = document.getElementById("MainContent_gvOperations").rows(lastRow).cells(cell).children[0].value }
                     break;
                 case 5:
                     newText.setAttribute("class", 'gvTextBoxSmall')
                     newText.addEventListener('click', function () { loadDdl(this, 'MainContent_ddl' + txt + 's', cell); });
                     break;
                 case 6:                /* type */
                     newText.addEventListener('click', function () { loadDdl(this, 'MainContent_ddl' + txt + 's', cell); });
                     newText.value = "Select One";
                     newText.setAttribute("class", 'align-left');
                     newText.setAttribute("TypeValue", 0);
                     break;
                 case 7:
                     newText.setAttribute("class", 'gvTextBox')
                     break;
                 case 8:
                     newText.setAttribute("class", 'gvTextBox')
                     break;
                 case 9:
                     newText.setAttribute("class", 'gvTextBox')
                     break;
                 case 10:
                     newText.setAttribute("class", 'gvTextBox')
                     break;
                 case 11:
                     newText.setAttribute("class", 'gvTextBoxSmall')
                     break;
                 case 12:
                     newText.setAttribute("class", 'gvTextBoxSmall')
                     break;
                 case 13:
                     newText.setAttribute("class", 'gvTextBoxSmall')
                     break;
             }
             newCell.appendChild(newText);   //insert txt 
         }

         function AddRow() {
            //Add new row
             var table = document.getElementById("MainContent_gvOperations");
             var newRow = table.insertRow();
             var newCell = newRow.insertCell();  //add first cell
             var chkBox = document.createElement("input"); chkBox.type = 'checkbox';  //create checkbox
             newCell.appendChild(chkBox);       //add chkbox to cell
             AddDdls("Crop", newRow, 1);        //add Crop cell
             AddDdls("Oper", newRow, 2);        //add operation cell
             AddDdls("Year", newRow, 3);        //add Year cell
             AddDdls("Month", newRow, 4);        //add month cell
             AddDdls("Day", newRow, 5);        //add day cell
             AddDdls("Type", newRow, 6);        //add Type cell
             AddDdls("", newRow, 7);        //add Amount cell
             AddDdls("", newRow, 8);        //add depth cell
             AddDdls("", newRow, 9);        //add no3 cell
             AddDdls("", newRow, 10);        //add po4 cell
             AddDdls("", newRow, 11);        //add org N cell
             AddDdls("", newRow, 12);        //add org P cell
             AddDdls("", newRow, 13);        //add k cell
             AddDdls("", newRow, 14);        //add nh3 cell
         }

         function TakeMixedCrops(lastRow) {
             showModal(true);
             document.getElementById("MainContent_hdnOperationNumber").value = lastRow; 
         }

         // this function is called from every dropdownlist in the gvOperations.
         function takeCrop(ddl, cell, ddlId) {
             var gv = document.getElementById("MainContent_gvOperations");
             lastRow = ddl.parentElement.parentElement.rowIndex;
             gv.rows[lastRow].cells[cell].children[0].value = ddl[ddl.selectedIndex].text;
             switch (ddlId) {
                 case "MainContent_ddlCrops":
                     gv.rows[lastRow].cells[cell].children[0].attributes["CropValue"].value = ddl[ddl.selectedIndex].value;
                     if (gv.rows[lastRow].cells[2].children[0].value == "Planting" || ddl[ddl.selectedIndex].text == "Plantar") {
                         if (ddl[ddl.selectedIndex].value == "367") { TakeMixedCrops(lastRow); }
                         gv.rows[lastRow].cells[7].children[0].value = ddl[ddl.selectedIndex].attributes.pp;
                         gv.rows[lastRow].cells[1].children[0].attributes["CropPP"].value = ddl[ddl.selectedIndex].attributes.pp;
                     }
                     if (ddl[ddl.selectedIndex].value == "367" && (gv.rows[lastRow].cells[2].children[0].value == "Planting" || ddl[ddl.selectedIndex].text == "Plantar")) {
                        }
                     break;
                 case "MainContent_ddlOpers":
                     gv.rows[lastRow].cells[cell].children[0].attributes["OperValue"].value = ddl[ddl.selectedIndex].value;
                     if (ddl[ddl.selectedIndex].text == "Planting" || ddl[ddl.selectedIndex].text == "Plantar") {
                         if (gv.rows[lastRow].cells[1].children[0].attributes["CropValue"].value == "367") { TakeMixedCrops(lastRow); }
                        gv.rows[lastRow].cells[7].children[0].value = gv.rows[lastRow].cells[1].children[0].attributes["CropPP"].value; }
                     break;
                 case "MainContent_ddlTypes":
                     var operation = ddl[ddl.selectedIndex].value.split(",")[1].split("|");
                     gv.rows[lastRow].cells[cell].children[0].attributes["TypeValue"].value = ddl[ddl.selectedIndex].value;
                     if (operation[0] == "GRAZ") {
                         gv.rows[lastRow].cells[9].children[0].value = operation[1];
                         gv.rows[lastRow].cells[10].children[0].value = operation[4];
                         gv.rows[lastRow].cells[11].children[0].value = operation[2];
                         gv.rows[lastRow].cells[12].children[0].value = operation[3];
                         gv.rows[lastRow].cells[13].children[0].value = operation[5];
                     }
                     if (operation[0] == "NUTG") {
                         if (ddl[ddl.selectedIndex].value.split(",")[0] == "1") {
                             operation[1] = "1.00";
                             operation[2] = "0.00";
                             operation[3] = "0.00";
                             operation[4] = "0.00";
                             operation[5] = "0.00";
                             operation[6] = "0.00";
                         }
                         if (ddl[ddl.selectedIndex].text.indexOf("Liquid") == -1) {
                             showModal1(true);
                             break;
                         }
                         else {
                             gv.rows[lastRow].cells[9].children[0].value = operation[1];
                             gv.rows[lastRow].cells[10].children[0].value = operation[4];
                             gv.rows[lastRow].cells[11].children[0].value = operation[2];
                             gv.rows[lastRow].cells[12].children[0].value = operation[3];
                             gv.rows[lastRow].cells[13].children[0].value = operation[5];
                             gv.rows[lastRow].cells[13].children[0].value = operation[6];
                         }
                     }
             }
             gv.rows[lastRow].cells[cell].children[0].style.display = "";
             gv.rows[lastRow].cells[cell].removeChild(gv.rows[lastRow].cells[cell].children[1]);
         }

         function changeOperation(ddlOper, lastRow) {
            //When operation changes the rest of the columns will be reset.
             document.getElementById('MainContent_gvOperations').rows[lastRow + 2].cells[2].children[0].value=ddlOper[ddlOper.selectedIndex].text;
             document.getElementById('MainContent_gvOperations').rows[lastRow + 2].cells[6].children[0].value = "";
             if (ddlOper[ddlOper.selectedIndex].text!="Planting") document.getElementById('MainContent_gvOperations').rows[lastRow + 2].cells[7].children[0].value = "0";
             document.getElementById('MainContent_gvOperations').rows[lastRow + 2].cells[8].children[0].value = "0";
             document.getElementById('MainContent_gvOperations').rows[lastRow + 2].cells[9].children[0].value = "0";
             document.getElementById('MainContent_gvOperations').rows[lastRow + 2].cells[10].children[0].value = "0";
             document.getElementById('MainContent_gvOperations').rows[lastRow + 2].cells[11].children[0].value = "0";
             document.getElementById('MainContent_gvOperations').rows[lastRow + 2].cells[12].children[0].value = "0";
             document.getElementById('MainContent_gvOperations').rows[lastRow + 2].cells[13].children[0].value = "0";
             onClick(lastRow);
         }

         function loadDdl(me, ddlOrg, cell) {
             var lastRow = parseInt(me.parentElement.parentElement.attributes["RowNumber"].value) + 2
             var ddl = document.getElementById(ddlOrg);
             var ddlNew = document.createElement("select");
             var gv = document.getElementById("MainContent_gvOperations");
             //ddlNew.addEventListener('focusout', function () { takeCrop(this, cell, ddlOrg); });
             ddlNew.addEventListener('change', function () { takeCrop(this, cell, ddlOrg); });
             ddlNew.id = "ddlType1"
             if (cell == 2) {
                 ddlNew.addEventListener('change', function () { changeOperation(this, lastRow - 2); }); 
                }

             for (i = 0; i <= ddl.length - 1; i++) {
                 if (cell == 6) {
                     switch (gv.rows[lastRow].cells[2].children[0].value) {
                         case 'Planting':
                         case 'Plantar':
                             if (ddl[i].value.indexOf("PLNT") > -1) { AddOption(ddlNew, me, ddl[i].text, ddl[i].value, 0); }
                            if (ddl[i].value.indexOf("*") > -1) { AddOption(ddlNew, me, ddl[i].text, ddl[i].value, 0); }
                            break;
                         case 'Fertilizer':
                         case 'Fertilizante':
                             if (ddl[i].value.indexOf("NUTG") > -1) { AddOption(ddlNew, me, ddl[i].text, ddl[i].value, 0); }
                             if (ddl[i].value.indexOf("*") > -1) { AddOption(ddlNew, me, ddl[i].text, ddl[i].value, 0); }
                             break;
                         case 'Tillage':
                         case 'Arar':
                             if (ddl[i].value.indexOf("TILL") > -1) { AddOption(ddlNew, me, ddl[i].text, ddl[i].value, 0); }
                             if (ddl[i].value.indexOf("*") > -1) { AddOption(ddlNew, me, ddl[i].text, ddl[i].value, 0); }
                             break;
                         case 'Harvest':
                         case 'Cosechar':
                             if (ddl[i].value.indexOf("HARV") > -1) { AddOption(ddlNew, me, ddl[i].text, ddl[i].value, 0); }
                            break;
                         case 'Kill':
                         case 'Terminar':
                             if (ddl[i].value.indexOf("KILL") > -1) { AddOption(ddlNew, me, ddl[i].text, ddl[i].value, 0); }
                            break;
                         case 'Irrigation (Manual)':
                         case 'Irrigación (Manual)':
                             if (ddl[i].value.indexOf("IRRI") > -1) { AddOption(ddlNew, me, ddl[i].text, ddl[i].value, 0); }
                             if (ddl[i].value.indexOf("*") > -1) { AddOption(ddlNew, me, ddl[i].text, ddl[i].value, 0); }
                             break;
                         case 'Start Grazing':
                         case 'Iniciar Pasteo':
                             if (ddl[i].value.indexOf("GRAZ") > -1) { AddOption(ddlNew, me, ddl[i].text, ddl[i].value, 0); }
                             if (ddl[i].value.indexOf("*") > -1) { AddOption(ddlNew, me, ddl[i].text, ddl[i].value, 0); }
                             break;
                         case 'Stop Grazing':
                         case 'Terminar Pasteo':
                             if (ddl[i].value.indexOf("STOP") > -1) { AddOption(ddlNew, me, ddl[i].text, ddl[i].value, 0); }
                            break;
                         case 'Burn':
                         case 'Quema':
                             if (ddl[i].value.indexOf("BURN") > -1) { AddOption(ddlNew, me, ddl[i].text, ddl[i].value, 0); }
                            break;
                        case 'Liming':
                        case 'Encalado':
                            if (ddl[i].value.indexOf("LIME") > -1) { AddOption(ddlNew, me, ddl[i].text, ddl[i].value, 0); }
                            break;
                    }
                 }
                else {
                    if (cell == 1) { AddOption(ddlNew, me, ddl[i].text, ddl[i].value, ddl[i].attributes["PP"].value); }
                    else { AddOption(ddlNew, me, ddl[i].text, ddl[i].value, 0); }
                    }
             }

            gv.rows[lastRow].cells[6].children[0].disabled = false;
            if (cell == 6) {
                 switch (gv.rows[lastRow].cells[2].children[0].value) {
                     case 'Planting':
                     case 'Plantar':
                     case 'Fertilizer':
                     case 'Fertilizante':
                     case 'Tillage':
                     case 'Arar':
                     case 'Irrigation (Manual)':
                     case 'Irrigación (Manual)':
                     case 'Start Grazing':
                     case 'Iniciar Pasteo':
                         gv.rows[lastRow].cells[cell].appendChild(ddlNew);
                         gv.rows[lastRow].cells[cell].children[0].style.display = "none";
                         break;
                     case 'Kill':
                     case 'Terminar':
                         gv.rows[lastRow].cells[cell].children[0].disabled = true;
                         break;
                 }
             }
             else {
                 gv.rows[lastRow].cells[cell].appendChild(ddlNew);
                 gv.rows[lastRow].cells[cell].children[0].style.display = "none";
             }
         }

         function AddOption(ddlNew, me, text, value, attribute) {
            //Add an option to the ddl passed in ddlNew. It can be any ddl in the project.
             var option = document.createElement("option");
             option.text = text;
             option.value = value;
             option.attributes["pp"] = attribute;
             ddlNew.add(option);
             if (option.text.trim() == me.value.trim()) { ddlNew.selectedIndex = ddlNew.length-1; }
         }


         function Cropping() {
             var treeViewData = window["<%=tvMain.ClientID %>" + "_Data"];
             if (treeViewData.selectedNodeID.value != "") {
                 var selectedNode = document.getElementById(treeViewData.selectedNodeID.value);
                 var value = selectedNode.href.substring(selectedNode.href.indexOf(",") + 3, selectedNode.href.length - 2);
                 var nodeText = selectedNode.innerHTML;
                 selectedNode.href = "";
                 turnOnSelected(nodeText, selectedNode.id + 'Nodes');
                 nodeText=""
             }
             else { alert("No node selected."); }
             return false;
         }
      </script>
     <div class="save" >
        <asp:LinkButton ID="btnSave" runat="server" Text="Save" ></asp:LinkButton>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="btnContinue" runat="server" Text="Continue"  ></asp:LinkButton>
     </div>

    <section id="sctGeneralInfo" class="section">
        <table id="tblGeneralInfo" runat="server">
            <tr id="trGemeralInfo" >
                <td  >
                    <input class="label" runat="server" id="lblFields" name="lblFields" type="text" 
                        value="Select Field" readonly="readonly" tabindex="1" />
                    <asp:DropDownList runat="server" ID="ddlFields" AutoPostBack="true" TabIndex="2" ></asp:DropDownList>
                </td>
                <td>
                    <input type="hidden" id="hdnField" runat="server" />
                </td>
                <td >
                    <input class="label" runat="server" id="lblScenarios" name="lblScenarios" type="text" value="Select Scenario" readonly="readonly" tabindex="3" />
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlScenarios" AutoPostBack="true" TabIndex="4"></asp:DropDownList>
                    <input type="hidden" id="hdnScenario" runat="server" />
                </td>
                <td>
                    <asp:Button runat="server" ID="btnDeleteScenario" Text="Delete Scenario Selected" OnClientClick="DeleteScenario();" TabIndex="10"></asp:Button>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="txtScenarioName" text="Scenario Name" onclick="BlankText(this.id);" TabIndex="5"></asp:TextBox>&nbsp;
                    <asp:Button runat="server" ID="btnAddNewScenario" Text="Add New Scenario" OnClientClick="__doPostBack('btnAddNewScenario', 'AddNewScenario');" TabIndex="6"></asp:Button>
                </td>
                <td></td>
                <td><asp:Button runat="server" ID="btnRenameScenario" Text="Rename Scenario To" OnClientClick="__doPostBack('btnRenameScenario', 'AddRenameScenario');" TabIndex="7" ></asp:Button></td>
                <td><asp:TextBox runat="server" ID="txtScenarioRenamed" text="Rename Scenario to" onclick="BlankText(this.id);" TabIndex="5"></asp:TextBox>&nbsp;
</td>
            </tr>         
        </table>
    </section>

    <section>
        <table>
            <tr>
               <td style="vertical-align:top">
                  <a id="aTvMain" runat="server" href="#" title="Close" onclick="hideDivTvMain('MainContent_tvMain');" class="BMPclose" tabindex="8">Hide</a>
               </td>
            </tr>
            <tr>
                <td id="tdMain" style="border-style:solid; border-width:thin; background-color:Menu">                    
                    <asp:TreeView ID="tvMain" runat="server" ExpandDepth="3" onclick="return Cropping();" LeafNodeStyle-ForeColor="DarkCyan" ParentNodeStyle-ForeColor="DarkGreen" Font-Size="Medium" NodeStyle-VerticalPadding="5" HoverNodeStyle-Font-Underline="true" SelectedNodeStyle-Font-Bold="true" NodeStyle-ChildNodesPadding="1" LeafNodeStyle-VerticalPadding="4" ShowExpandCollapse="false">                       
                        <Nodes >
                            <asp:TreeNode Text="Cropping System" >
                                <asp:TreeNode Text="Select Cropping System"></asp:TreeNode>
                                <asp:TreeNode Text="Upload Saved Cropping System"></asp:TreeNode>
                                <asp:TreeNode Text="Copy Cropping System"></asp:TreeNode>
                                <asp:TreeNode Text="Save Current Cropping System" ></asp:TreeNode>
                                <asp:TreeNode Text="Management Operations" ></asp:TreeNode>
                            </asp:TreeNode>
                            <asp:TreeNode Text="Best Management Practices (BMPs)">
                                <asp:TreeNode Text="Auto Irrigation and Fertigation">
                                    <asp:TreeNode Text="Auto Irrigation"></asp:TreeNode>
                                    <asp:TreeNode Text="Auto Fertigation"></asp:TreeNode>
                                </asp:TreeNode>
                                <asp:TreeNode Text="Drainage Water Management System">
                                    <asp:TreeNode Text="Tile Drain"></asp:TreeNode>
                                    <asp:TreeNode Text="Pads and Pipes - No Ditch Improvement"></asp:TreeNode>
                                    <asp:TreeNode Text="Pads and Pipes - Two-Stage Ditch System"></asp:TreeNode>
                                    <asp:TreeNode Text="Pads and Pipes - Ditch Enlargement and Reservioir system"></asp:TreeNode>
                                    <asp:TreeNode Text="Pads and Pipes - Tailwater Irrigation"></asp:TreeNode>
                                </asp:TreeNode>
                                <asp:TreeNode Text="Wetlands and Ponds">
                                    <asp:TreeNode Text="Wetland"></asp:TreeNode>
                                    <asp:TreeNode Text="Pond"></asp:TreeNode>
                                </asp:TreeNode>
                                <asp:TreeNode Text="Streams and Riparian Management">
                                    <asp:TreeNode Text="Stream Fencing (Livestock Access Control)"></asp:TreeNode>
                                    <asp:TreeNode Text="Streambank Stabilization"></asp:TreeNode>
                                    <asp:TreeNode Text="Riparian Forest Buffer"></asp:TreeNode>
                                    <asp:TreeNode Text="Filter Strip"></asp:TreeNode>
                                    <asp:TreeNode Text="Water Way (Grassed Buffer)"></asp:TreeNode>
                                </asp:TreeNode>
                                <asp:TreeNode Text="Contour Buffer (Strip Farming)">
                                    <asp:TreeNode Text="Contour Buffer"></asp:TreeNode>
                                </asp:TreeNode>
                                <asp:TreeNode Text="Land Grading and Management">
                                    <asp:TreeNode Text="Land Leveling"></asp:TreeNode>
                                    <asp:TreeNode Text="Terrace System"></asp:TreeNode>
                                    <asp:TreeNode Text="Manure Control"></asp:TreeNode>
                                </asp:TreeNode>
<%--                                <asp:TreeNode Text="Climate Change">
                                    <asp:TreeNode Text="Temperature and Precipitation Changes"></asp:TreeNode>
                                </asp:TreeNode>
--%>                            <asp:TreeNode Text="Roads">
                                    <asp:TreeNode Text="Asfalt or Concrete"></asp:TreeNode>
                                    <asp:TreeNode Text="Grass Cover"></asp:TreeNode>
                                    <asp:TreeNode Text="Slope Adjustmen"></asp:TreeNode>
                                    <asp:TreeNode Text="Shading"></asp:TreeNode>
                                </asp:TreeNode>
                            </asp:TreeNode>
                        </Nodes>
                    </asp:TreeView>
                </td>
                <td style="vertical-align:text-top;" runat="server" id="tdCurrentScenario" >
                    <fieldset id="fsetCroppingSystem"  runat="server" style="display:none">
                        <legend id="lblCroppingSystem" runat="server" >Existing Cropping System</legend>
                        <table id="tblCroppingSystem" >
                            <tr id="trCroppingSystem" >
                                <td >
                                    <input class="label" runat="server" id="lblSelectCroppingSystem" name="lblSelectCroppingSystem" type="text" value="Cropping System" readonly="true" />
                                    <select id="ddlCroppingSystems" runat="server" onchange="return loadTills();"></select>
                                    <select runat="server" id="ddlCroppingSystemsAll" hidden="hidden"></select>
                                    <input class="label" runat="server" id="lblTillage" name="lblTillage" 
                                        type="text" value="Select Tillage" readonly="true"
                                        maxlength="120" />
                                    <select id="ddlTillage"></select>
                                    <select id="ddlTillageAll" hidden="hidden" runat="server"></select>
                                    <asp:Button runat="server" ID="btnUpload" name="btnUpload" Text="Upload Cropping System" OnClientClick="return selectCropping();" ></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset id="fsetLoadCropping" runat="server" title="Upload Cropping System" style="display:none">
                        <legend ID="lblLoadCropping" runat="server" CssClass="treeView1">Upload Cropping System</legend>
                        <table id="tblLoadCropping" >
                            <tr id="trLoadCropping" >
                                <td>
                                    <input id="btnUploader" type="button" onclick="triggerFileUpload()" runat="server" />
                                    <%--<asp:Button ID="btnUploader" OnClientClick="triggerFileUpload()" Text="ASPNET Button" runat="server"/>--%>
                                    <input id="flUpload" runat="server" type="file" style="visibility:hidden" onchange="ActivateUpload()"/>
                                    <asp:Button ID="btnOpenCropping" runat="server" Text="Upload" Width="56px" style="display:none"/>                
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset id="fsetCopyCropping" runat="server" style="display:none">
                        <legend ID="lblCopyCropping" runat="server" CssClass="treeView1">Copy Cropping System</legend>
                        <table>
                            <tr>
                                <td ><input class="label" runat="server"  id="lblFromField" name="lblFields" type="text" value="From Field" /></td>
                                <td><select runat="server" id="ddlFromField" onchange="return loadScenarios(this);"></select></td>
                                <td ><input class="label" runat="server" id="lblFromScenario" name="lblScenarios" type="text" value="From Scenario" /></td>
                                <td>
                                    <select runat="server" id="ddlFromScenario"></select>
                                    <select runat="server" id="ddlAllScenario" style="display:none"></select>
                                </td>
                                <td><asp:LinkButton id="btnCopyCropping" runat="server" Text="Copy" BackColor="Silver" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" Width="100%" Font-Overline="False"></asp:LinkButton></td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset id="fsetOperations" runat="server" class="section" >
                        <legend ID="lblOperation" runat="server" CssClass="treeView1">Management Operation</legend>
                        <table id="tblOperations">
                            <tr>
                                <td >
                                    <input type="button" id="btnAddOperation" runat="server" value="Add New" onclick="__doPostBack('btnAddNewOperation', 'AddNewOperation');" />    <%--This add an operacion in VB  --%>
                                    <input type="button" id="btnDeleteOperation" runat="server" value="Delete Selected" onclick="DeleteOperations();" /> <%--this delete operation in javascript--%>
                                    <input type="button" id="btnMixePasture" runat="server" value="Show Grasses" style="visibility:hidden" onclick="return showModal(false);" /> <%--this show grasses for Mixed Grasse only in Javascript--%>
                                </td>
                            </tr>
                            <tr style="display:none">
                                <td>
                                    <select id="ddlCrops" runat="server" onblur="onBlur(1,1);"  ></select>
                                    <select id="ddlOpers" runat="server" ></select>
                                    <select id="ddlYears" runat="server" ></select>
                                    <select id="ddlMonths" runat="server" ></select>
                                    <select id="ddlDays" runat="server" ></select>
                                    <select id="ddlTypes" runat="server" ></select>
                                    <input type="hidden" id="ctlSave" value="No" runat="server"/>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvOperations" runat="server" AutoGenerateColumns="False" style="width:100%">
                                        <FooterStyle CssClass="gvFooterStyle" />
                                        <RowStyle CssClass="gvRowStyle" />
                                        <SelectedRowStyle CssClass="gvSelectedRowStyle" />
                                        <PagerStyle CssClass="gvPagerSyle" HorizontalAlign="Center" />
                                        <HeaderStyle CssClass="gvHeaderStyle" />  
                                        <AlternatingRowStyle CssClass="gvAltRowStyle" />                                      
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select" ItemStyle-Width="30px"> 
                                                <HeaderTemplate>Select&#10;
                                                    <input id="chkSelectAll" type="checkbox" value="Select" onclick="SelectAllOperations()" />
                                                </HeaderTemplate>                   
                                                <ItemTemplate >                                                   
                                                    <input id="chkSelected" type="checkbox" runat="server"/>
                                                    <%--<input id="txtSelected" type="hidden" class="align-left" runat="server" value='<%#Eval("selected")%>' onclick="return loadDdl(this, 'MainContent_ddlCrops',1);" />--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Crop" ItemStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <input id="txtCrop" type="text" class="gvStringBoxMedium" runat="server" value='<%#Eval("apexCropName")%>' title="Enter Value" onclick="return loadDdl(this, 'MainContent_ddlCrops',1);"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Operation" >
                                                <ItemTemplate>
                                                    <input id="txtOper" type="text" class="gvStringBoxMedium" runat="server" value='<%#Eval("apexOpName")%>' title="Enter Value" onclick="return loadDdl(this, 'MainContent_ddlOpers',2);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Year" >
                                                <ItemTemplate>
                                                    <input id="txtYear" type="text" class="gvTextBoxSmall" runat="server" value='<%#Eval("year")%>' title="Enter Value" onclick="return loadDdl(this, 'MainContent_ddlYears',3);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Month" >
                                                <ItemTemplate>
                                                    <input id="txtMonth" type="text" class="gvTextBoxSmall" runat="server" value='<%#Eval("month")%>' title="Enter Value" onclick="return loadDdl(this, 'MainContent_ddlMonths',4);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Day" >
                                                <ItemTemplate>
                                                    <input id="txtDay" type="text" class="gvTextBoxSmall" runat="server" value='<%#Eval("day")%>' title="Enter Value" onclick="return loadDdl(this, 'MainContent_ddlDays',5);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type" >
                                                <ItemTemplate>
                                                    <input id="txtType" type="text" class="gvStringBoxLarge" runat="server" value='<%#Eval("apexTillName")%>' title="Enter Value" onclick="return loadDdl(this, 'MainContent_ddlTypes',6);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount" >
                                                <ItemTemplate>
                                                    <input id="txtAmount" type="text" class="gvTextBoxSmall" runat="server" text-align="right" value='<%#Eval("apexOpv1")%>' title="Enter Value" />                                
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Depth" >
                                                <ItemTemplate>
                                                    <input id="txtDepth" type="text" class="gvTextBoxSmall" runat="server" value='<%#Eval("apexOpv2")%>' title="Enter Value" />                                
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NO3-N (dry)" >
                                                <ItemTemplate>
                                                    <input id="txtNO3" type="text" class="gvTextBoxSmall" runat="server" value='<%#Eval("no3")%>' title="Enter NO3 contribution (fraction)"  />                                
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PO4-P (dry)" >
                                                <ItemTemplate>
                                                    <input ID="txtPO4" type="text" class="gvTextBoxSmall" runat="server" value='<%#Eval("po4")%>' title="Enter PO4 contribution (fraction)" /> 
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Org N (dry)" >
                                                <ItemTemplate>
                                                    <input id="txtOrgN" type="text" class="gvTextBoxSmall" runat="server" value='<%#Eval("orgn")%>' title="Enter Org N contribution (fraction)" />                                
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Org P (dry)" >
                                                <ItemTemplate>
                                                    <input id="txtOrgP" type="text" class="gvTextBoxSmall" runat="server" value='<%#Eval("orgP")%>' title="Enter Org P contribution (fraction)" />                                
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="K" >
                                                <ItemTemplate>
                                                    <input id="txtK" type="text" class="gvTextBoxSmall" runat="server" value='<%#Eval("k")%>' title="Enter K contribution (fraction)" />                                
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="NH3" >
                                                <ItemTemplate>
                                                    <input id="txtNh3" type="text" class="gvTextBoxSmall" runat="server" value='<%#Eval("nh3")%>' title="Enter NH3 contribution (fraction)" />                                
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td style="vertical-align:top">
                                    <a id="aOpReadHide" runat="server" href="#close" title="Close" onclick="hideDiv('MainContent_fsetOperations');" class="BMPclose">Hide</a>
                                </td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetAI" runat="server" 
                        style="display:none; vertical-align:top;" class="section" width="Auto">
                        <legend id="lblAI" runat="server" class="label" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Auto Irrigation</legend>
                        <table id="tblAI">
                            <tr>
                                <td><asp:Label ID="lblAIIrrigationType" runat="server" Text="Irrigation Type" class="label"></asp:Label></td>
                                <td><asp:Label ID="lblAIWaterStress" runat="server" Text="Water Stress"></asp:Label></td>
                                <td><asp:Label ID="lblAIEfficiancy" runat="server" Text="Irrigation Efficiancy (0.00 - 1.00)"></asp:Label></td>
                                <td><asp:Label ID="lblAIFrequency" runat="server" Text="Irrigation Frequency (days)"></asp:Label></td>
                                <td><asp:Label ID="lblAIMaxSingleAppl" runat="server" Text="Maximum Single Application (in)"></asp:Label></td>
                                <td><asp:Label ID="lblAIResArea" runat="server" Text="Reservoir Area (ac.)"></asp:Label></td>
                                <td><asp:Label ID="lblAISafetyFactor" runat="server" Text="Safety Factor (0-1)" ></asp:Label></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td><asp:DropDownList ID="ddlAIIrrigationType" runat="server" onchange="return CheckForResArea(this, 'AI');"></asp:DropDownList></td>
                                <td><asp:TextBox ID="txtAIWaterStress" CssClass="gvTextBoxSmall" runat="server" onchange="ValidateInput(this);"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtAIIrrigationEfficiancy" CssClass="gvTextBoxSmall" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtAIFrequency" CssClass="gvTextBoxSmall" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtAIMaxSingleAppl" CssClass="gvTextBoxSmall" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtAIResArea" CssClass="gvTextBoxSmall" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtAISafetyFactor" CssClass="gvTextBoxSmall" runat="server" Text="0.5"></asp:TextBox></td>
                                <td><asp:Button ID="btnAIClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblAI')"></asp:Button></td>
                                <td style="vertical-align:top"><a id="aAIHide" runat="server" href="#close" title="Close" onclick="hideDiv('MainContent_fsetAI');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>
                            
                    <fieldset id="fsetAF" runat="server" style="display:none" class="section">
                        <legend runat="server" id="lblAF" class="label" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Auto Fertigation</legend>
                        <table id="tblAF">
                            <tr>
                                <td><asp:Label ID="lblAFIrrigationType" runat="server" Text="Irrigation Type"></asp:Label></td>
                                <td><asp:Label ID="lblAFWaterStress" runat="server" Text="Water Stress"></asp:Label></td>
                                <td><asp:Label ID="lblAFEfficiancy" runat="server" Text="Irrigation Efficiancy (%)"></asp:Label></td>
                                <td><asp:Label ID="lblAFFrequency" runat="server" Text="Irrigation Frequency (days)"></asp:Label></td>
                                <td><asp:Label ID="lblAFMaxSingleAppl" runat="server" Text="Maximum Single Application (in)"></asp:Label></td>
                                <td><asp:Label ID="lblAFNConc" runat="server" Text="N Concentration (ppm)" Visible="false"></asp:Label></td>
                                <td><asp:Label ID="lblAFSafetyFactor" runat="server" Text="Safety Factor (0-1)"></asp:Label></td>
                            </tr>
                            <tr>
                                <td><asp:DropDownList ID="ddlAFIrrigationType" runat="server" onchange="return CheckForResArea(this, 'AF');"></asp:DropDownList></td>
                                <td><asp:TextBox ID="txtAFWaterStress" CssClass="gvTextBoxSmall" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtAFIrrigationEfficiancy" CssClass="gvTextBoxSmall" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtAFFrequency" CssClass="gvTextBoxSmall" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtAFMaxSingleAppl" CssClass="gvTextBoxSmall" runat="server"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtAFNConc" CssClass="gvTextBoxSmall" runat="server" style="display:none" ></asp:TextBox></td>
                                <td><asp:TextBox ID="txtAFSafetyFactor" CssClass="gvTextBoxSmall" runat="server" Text="0.5"></asp:TextBox></td>
                                <td><asp:Button ID="btnAFClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblAF')"></asp:Button></td>
                                <td style="vertical-align:top"><a id="aAFHide" runat="server" href="#close" title="Close" onclick="hideDiv('MainContent_fsetAF');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetTD" runat="server" style="display:none" class="section" >
                    <legend runat="server" id="lblTD" class="label" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Tile Drain</legend>
                        <table id="tblTD">
                            <tr>
                                <td><asp:Label ID="lblTDDepth" runat="server" Text="Depth (ft)"></asp:Label>
                                    <asp:TextBox ID="txtTDDepth" runat="server" CssClass="gvTextBox"></asp:TextBox>
                                </td>
                                <td ><asp:Button ID="btnTDClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblTD')"/></td>
                                <td style="vertical-align:top"><a id="aTDHide" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetTD');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetPPND" runat="server" style="display:none" class="section">
                        <legend runat="server" id="lblPPND" class="label" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Pads and Pipes - No Ditch Improvement</legend> 
                        <table id="tblPPND">
                            <tr>
                                <td >
                                    <asp:Label ID="lblPPNDWidth" runat="server" Text="Width (ft)"></asp:Label>
                                    <asp:TextBox ID="txtPPNDWidth" runat="server" CssClass="gvTextBox"></asp:TextBox>
                                </td>
                                <td >
                                    <asp:Label ID="lblPPNDSides" runat="server" Text="Sides"></asp:Label>
                                    <asp:TextBox ID="txtPPNDSides" runat="server" CssClass="gvTextBox"></asp:TextBox>
                                </td>
                                <td ><asp:Button ID="btnPPNDClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblPPND')" /></td>
                                <td style="vertical-align:top"><a id="aPPNDHide" runat="server" href="#close" title="Close" onclick="hideDiv('MainContent_fsetPPND');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetPPDS" runat="server" style="display:none" class="section">
                        <legend runat="server" id="lblPPDS" class="label" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Pads and Pipes - Two-stage Ditch system</legend> 
                        <table id="tblPPDS">
                            <tr>
                                <td >
                                    <asp:Label ID="lblPPDSWidth" runat="server" Text="Width (ft)"></asp:Label>
                                    <asp:TextBox ID="txtPPDSWidth" runat="server" CssClass="gvTextBox"></asp:TextBox>
                                </td>
                                <td >
                                    <asp:Label ID="lblPPDSSides" runat="server" Text="Sides"></asp:Label>
                                    <asp:TextBox ID="txtPPDSSides" runat="server" CssClass="gvTextBox"></asp:TextBox>
                                </td>
                                <td ><asp:Button ID="btnPPDSClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblPPDS')" /></td>
                                <td style="vertical-align:top"><a id="aPPDSHide" runat="server" href="#close" title="Close" onclick="hideDiv('MainContent_fsetPPDS');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetPPDE" runat="server" style="display:none" class="section">
                        <legend runat="server" id="lblPPDE" class="label" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Pads and Pipes - Ditch Enlargement and Reservoir System</legend> 
                        <table id="tblPPDE">
                            <tr>
                                <td >
                                    <asp:Label ID="lblPPDEResArea" runat="server" Text="Reservoir area(ac)"></asp:Label>
                                    <asp:TextBox ID="txtPPDEResArea" runat="server" CssClass="gvTextBox"></asp:TextBox>
                                </td>
                                <td >
                                    <asp:Label ID="lblPPDEWidth" runat="server" Text="Pads and Pipes Width(ft)"></asp:Label>
                                    <asp:TextBox ID="txtPPDEWidth" runat="server"></asp:TextBox>
                                </td>
                                <td >
                                    <asp:Label ID="lblPPDESides" runat="server" Text="Sides"></asp:Label>
                                    <asp:TextBox ID="txtPPDESides" runat="server" CssClass="gvTextBox"></asp:TextBox>
                                </td>
                                <td ><asp:Button ID="btnPPDEClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblPPDE')" /></td>
                                <td style="vertical-align:top"><a id="aPPDEHide" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetPPDE');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetPPTW" runat="server" style="display:none" class="section">
                        <legend runat="server" id="lblPPTW" class="label" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Pads and Pipes - Tailwater Irrigation</legend> 
                        <table id="tblPPTW">
                            <tr>
                                <td >
                                    <asp:Label ID="lblPPTWResArea" runat="server" Text="Reservoir area(ac)"></asp:Label>
                                    <asp:TextBox ID="txtPPTWResArea" runat="server" CssClass="gvTextBox"></asp:TextBox>
                                </td>
                                <td >
                                    <asp:Label ID="lblPPTWWidth" runat="server" Text="Pads and Pipes Width(ft)"></asp:Label>
                                    <asp:TextBox ID="txtPPTWWidth" runat="server" CssClass="gvTextBox"></asp:TextBox>
                                </td>
                                <td >
                                    <asp:Label ID="lblPPTWSides" runat="server" Text="Sides"></asp:Label>
                                    <asp:TextBox ID="txtPPTWSides" runat="server" CssClass="gvTextBox"></asp:TextBox>
                                </td>
                                <td ><asp:Button ID="btnPPTWClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblPPTW')"/></td>
                                <td style="vertical-align:top"><a id="aPPTWHide" runat="server" href="#close" title="Close" onclick="hideDiv('MainContent_fsetPPTW');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetWL" runat="server" style="display:none" class="section">
                        <legend id="lblWL" class="label" runat="server" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Wetlands</legend>
                        <table id="tblWL">
                            <tr>
                                <td >
                                    <asp:Label ID="lblWLArea" runat="server" Text="Area (ac)"></asp:Label>
                                    <asp:TextBox ID="txtWLArea" runat="server" CssClass="gvTextBox"></asp:TextBox>
                                </td>
                                <td ><asp:Button ID="btnWLClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblWL')" /></td>
                                <td style="vertical-align:top"><a id="aWLHide" runat="server" href="#close" title="Close" onclick="hideDiv('MainContent_fsetWL');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetPND" runat="server" style="display:none" class="section">
                        <legend id="lblPND" class="label" runat="server" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Ponds</legend>
                        <table id="tblPND">
                            <tr>
                                <td >
                                    <asp:Label ID="lblPNDFraction" runat="server" Text="Fraction of Area Control by Pond(0.0 to 1.0)"></asp:Label>
                                    <asp:TextBox ID="txtPNDFraction" runat="server" CssClass="gvTextBox"></asp:TextBox>
                                    <asp:Button ID="btnPNDClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblPND')"/>
                                </td>
                                <td style="vertical-align:top"><a id="aPNDHide" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetPND');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>
                        
                    <fieldset id="fsetSF" runat="server" style="display:none" class="section">
                        <legend id="lblSF" class="label" runat="server" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Stream Fencing (Livestock Access Control)</legend>
                        <table id="tblSF">
                            <tr>
                                <td><asp:Label ID="lblSFAimals" runat="server" Text="Number of Animals"></asp:Label></td>
                                <td><asp:Label ID="lblSFDays" runat="server" Text="Days in Stream"></asp:Label></td>
                                <td><asp:Label ID="lblSFHours" runat="server" Text="Hours/day in Stream"></asp:Label></td>
                                <td><asp:Label ID="lblSFType" runat="server" Text="Select Animal"></asp:Label></td>
                                <td><asp:Label ID="lblSFManure" runat="server" Text="Dry Manure/Day (lbs)"></asp:Label></td>
                            </tr>
                            <tr>
                                <td><asp:TextBox ID="txtSFAnimals" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtSFDays" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtSFHours" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:DropDownList ID="ddlSFType" runat="server" onchange="TakeNutrients(this, 'SF')"></asp:DropDownList></td>
                                <td><asp:TextBox ID="txtSFManure" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td colspan="5"><asp:Label ID="lblSFNutrients" runat="server" Text="Nutrient Fractions (dry weight basis)"></asp:Label></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="lblSFNO3" runat="server" Text="NO3-N"></asp:Label></td>
                                <td><asp:Label ID="lblSFPO4" runat="server" Text="PO4-P"></asp:Label></td>
                                <td><asp:Label ID="lblSFOrgN" runat="server" Text="OrgN"></asp:Label></td>
                                <td><asp:Label ID="lblSFOrgP" runat="server" Text="OrgP"></asp:Label></td>
                                <td><asp:Label ID="lblSFMH3" runat="server" Text="NH3"></asp:Label></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td><asp:TextBox ID="txtSFNO3" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtSFPO4" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtSFOrgN" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtSFOrgP" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtSFNH3" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:Button ID="btnSFClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblSF')" /></td>
                                <td style="vertical-align:top"><a id="aSFHide" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetSF');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetSBS" runat="server" style="display:none" class="section">
                        <legend id="lblSBS" class="label" runat="server" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Streambank Stabilization</legend>
                        <table id="tblSBS">
                            <tr>
                                <td><asp:CheckBox ID="chkSBS" runat="server" Text="Select" /></td>
                                <td><asp:Button ID="btnSBSClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblSBS')"/></td>
                                <td style="vertical-align:top"><a id="aSBSHide" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetSBS');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetRF" runat="server" style="display:none" class="section">
                        <legend id="lblRF" class="label" runat="server" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Riparian Forest Buffer</legend>
                        <table id="tblRF">
                            <tr>
                                <td ><asp:Label ID="lblRFArea" runat="server" Text="Area (ac)"></asp:Label></td>
                                <td><asp:Label ID="lblRFWidth" runat="server" Text="Width (ft)"></asp:Label></td>
                                <td><asp:Label ID="lblRFGrass" runat="server" Text="Grass Field Portion (0.25-0.75)"></asp:Label></td>
                                <td><asp:Label ID="lblRFRatio" runat="server" Text="Buffer Slope Ratio to Upland (0.25-1.00)"></asp:Label></td>
                                <td></td>                            
                            </tr>
                            <tr>
                                <td><asp:TextBox ID="txtRFArea" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtRFWidth" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtRFGrass" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtRFRatio" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:Button ID="btnRFClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblRF')"/></td>    
                                <td style="vertical-align:top"><a id="aRFHide" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetRF');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetFS" runat="server" style="display:none" class="section">
                        <legend id="lblFS" class="label" runat="server" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Filter Strip</legend>
                        <table id="tblFS">
                            <tr>
                                <td><asp:Label ID="lblFSType" runat="server" Text="Vegetation"></asp:Label></td>
                                <td><asp:Label ID="lblFSArea" runat="server" Text="Area (ac)"></asp:Label></td>
                                <td><asp:Label ID="lblFSWidth" runat="server" Text="Width (ft)"></asp:Label></td>
                                <td><asp:Label ID="lblFSRatio" runat="server" Text="Buffer Slope Ratio to Upland (0.25-1.00)"></asp:Label></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td><asp:DropDownList ID="ddlFSType" runat="server"></asp:DropDownList></td>
                                <td><asp:TextBox ID="txtFSArea" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtFSWidth" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtFSRatio" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:Button ID="btnFSClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblFS')"/></td>
                                <td style="vertical-align:top"><a id="aFSHide" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetFS');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetWW" runat="server" style="display:none" class="section">
                        <legend id="lblWW" class="label" runat="server" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Water Ways</legend>
                        <table id="tblWW">
                            <tr>
                                <td><asp:Label ID="lblWWType" runat="server" Text="Vegetation"></asp:Label></td>
                                <td><asp:Label ID="lblWWWidth" runat="server" Text="Width (ft) *Icluding both sides of channel"></asp:Label></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td><asp:DropDownList ID="ddlWWType" runat="server"></asp:DropDownList></td>
                                <td><asp:TextBox ID="txtWWWidth" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:Button ID="btnWWClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblWW')"/></td>
                                <td style="vertical-align:top"><a id="aWWHide" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetWW');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetCB" runat="server" style="display:none" class="section">
                        <legend id="lblCB" class="label" runat="server" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Contour Buffer</legend>
                        <table id="tblCB">
                            <tr>
                                <td><asp:Label ID="lblCBType" runat="server" Text="Vegetation"></asp:Label></td>
                                <td><asp:Label ID="lblCBBufferWith" runat="server" Text="Buffer Width (ft)"></asp:Label></td>
                                <td><asp:Label ID="lblCBCropWidth" runat="server" Text="Crop Width (ft)"></asp:Label></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td><asp:DropDownList ID="ddlCBType" runat="server"></asp:DropDownList></td>
                                <td><asp:TextBox ID="txtCBBufferWidth" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtCBCropWidth" runat="server"  CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:Button ID="btnCBClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblCB')"/></td>
                                <td style="vertical-align:top"><a id="aCBHide" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetCB');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>
                        
                    <fieldset id="fsetLL" runat="server" style="display:none" class="section">
                        <legend id="lblLL" class="label" runat="server" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Land Leveling</legend>
                        <table id="tblLL">
                            <tr>
                                <td><asp:Label ID="lblReduction" runat="server" Text="Slope Reduction (%)"></asp:Label></td>
                                <td><asp:TextBox ID="txtLLReduction" runat="server" CssClass="gvTextBox"></asp:TextBox></td>                                   
                                <td><asp:Button ID="btnLLClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblLL')"/></td>
                                <td style="vertical-align:top"><a id="aLLHide" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetLL');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetTS" runat="server" style="display:none" class="section">
                        <legend id="lblTS" class="label" runat="server" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Terrace System</legend>
                        <table id="tblTS">
                            <tr>
                                <td><asp:CheckBox ID="chkTS" runat="server" Text="Select" /></td>
                                <td><asp:Button ID="btnTSClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblTS')"/></td>
                                <td style="vertical-align:top"><a id="aTSHide" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetTS');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetMC" runat="server" style="display:none" class="section">
                        <legend id="lblMC" class="label" runat="server" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Manure Control</legend>
                        <table id="tblMC" runat="server">
                            <tr>
                                <td><asp:Label ID="lblMCType" runat="server" Text="Manure Control Adjustment"></asp:Label></td>
                                <td><asp:DropDownList ID="ddlMCType" runat="server" onchange="TakeNutrients(this, 'MC')"></asp:DropDownList></td>
                            </tr>
                            <tr><td colspan=5><asp:Label ID="lblNutrientsAdj" runat="server"  Text="Nutrient Adjustment Factors" style="color:Blue"></asp:Label> </td></tr>
                            <tr>
                                <td colspan=5>
                                    <asp:Label ID="lblNO3NAdj" runat="server"  Text="NO3_N"></asp:Label>
                                    <asp:TextBox ID="txtmcNO3N" runat="server" CssClass="gvTextBoxSmall"></asp:TextBox>
                                    <asp:Label ID="lblmcOrgN" runat="server" Text="Org N"></asp:Label>
                                    <asp:TextBox ID="txtmcOrgN" runat="server" CssClass="gvTextBoxSmall"></asp:TextBox>
                                    <asp:Label ID="lblmcPO4P" runat="server" Text="PO4_P"></asp:Label>
                                    <asp:TextBox ID="txtmcPO4P" runat="server" CssClass="gvTextBoxSmall"></asp:TextBox>
                                    <asp:Label ID="lblmcOrgP" runat="server" Text="Org P"></asp:Label>
                                    <asp:TextBox ID="txtmcOrgP" runat="server" CssClass="gvTextBoxSmall"></asp:TextBox>
                                    <asp:Label ID="lblmcOM" runat="server" Text="Org Matter"></asp:Label>
                                    <asp:TextBox ID="txtmcOM" runat="server" CssClass="gvTextBoxSmall"></asp:TextBox>
                                </td>                                   
                                <td><asp:Button ID="btnMCClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblMC')"/></td>
                                <td style="vertical-align:top"><a id="aMCHide" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetMC');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                        <table id="tblNoMC" runat="server" style="display:none">
                            <tr>
                                <td><asp:Label ID="lblNoMC" runat="server" Text="There are not manure operations to applied Manure Control. Enter manure operation in you cropping system and then try again."></asp:Label></td>
                                <td style="vertical-align:top"><a id="a2" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetMC');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetLM" runat="server" style="display:none" class="section">                            
                        <legend id="lblLM" runat="server" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Liming</legend>                            
                        <table id="tblLM">
                            <tr>
                                <td >
                                    <asp:CheckBox ID="chkLM" runat="server" Text="Select" />
                                </td>
                                <td >
                                    <asp:Button ID="btnLMClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblLM')"/>
                                </td>
                                <td style="vertical-align:top"><a id="aLMHide" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetLM');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetCC" runat="server" style="display:none" class="section">
                        <legend id="lblCC" class="label" runat="server" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Temperature and Precipitation Changes</legend>
                        <table id="tblCC">
                            <tr>
                                <td >
                                    <asp:Label ID="lblCCMinTmp" runat="server" Text="Change Minimum Temperature(±C°)"></asp:Label>
                                    <asp:TextBox ID="txtCCMinTmp" runat="server" CssClass="gvTextBoxSmall"></asp:TextBox>
                                    <asp:Label ID="lblCCMaxTmp" runat="server" Text="Change Maximium Temperature(±C°)"></asp:Label>
                                    <asp:TextBox ID="txtCCMaxTmp" runat="server" CssClass="gvTextBoxSmall"></asp:TextBox>
                                    <asp:Label ID="lblCCPcp" runat="server" Text="Change Precipitation(±%)"></asp:Label>
                                    <asp:TextBox ID="txtCCPcp" runat="server" CssClass="gvTextBoxSmall"></asp:TextBox>
                                </td>
                                <td ><asp:Button ID="btnCCClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblCC')" /></td>
                                <td style="vertical-align:top"><a id="a1" runat="server" href="#close" title="Close" onclick="hideDiv('MainContent_fsetCC');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetAoC" runat="server" style="display:none" class="section">
                        <legend id="lblAoC" runat="server" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Asfalt or Concrete</legend>
                        <table id="tblAoC">
                            <tr>
                                <td ><asp:CheckBox ID="chkAoC" runat="server" Text="Select" /></td>
                                <td ><asp:Button ID="btnAoCClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblAoC')"/></td>
                                <td style="vertical-align:top"><a id="aAoCHide" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetAoC');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetGC" runat="server" style="display:none" class="section">
                        <legend id="lblGC" runat="server" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Grass Cover</legend>
                        <table id="tblGC">
                            <tr>
                                <td><asp:CheckBox ID="chkGC" runat="server" Text="Select" /></td>
                                <td><asp:Button ID="btnGCClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblGC')"/></td>
                                <td style="vertical-align:top"><a id="aGCHide" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetGC');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetSA" runat="server" style="display:none" class="section">
                        <legend id="lblSA" runat="server" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Slope Adjustment</legend>
                        <table id="tblSA">
                            <tr>
                                <td ><asp:CheckBox ID="chkSA" runat="server" Text="Select" /></td>
                                <td ><asp:Button ID="btnSAClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblSA')"/></td>
                                <td style="vertical-align:top"><a id="aSAHide" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetSA');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>

                    <fieldset id="fsetSdg" runat="server" style="display:none" class="section">
                        <legend id="lblSdg" class="label" runat="server" onmouseover="changeCursor(this.id);" onclick="hideBMP(this.id);">Shading</legend>
                        <table id="tblSdg">
                            <tr>
                                <td><asp:Label ID="lblSdgCrop" runat="server" Text="Vegetation"></asp:Label></td>
                                <td><asp:Label ID="lblSdgArea" runat="server" Text="Area (ac)"></asp:Label></td>
                                <td><asp:Label ID="lblSdgWidth" runat="server" Text="Width (ft)"></asp:Label></td>
                                <td><asp:Label ID="lblSdgSlopeRatio" runat="server" Text="Buffer Slope Ratio to Upland (0.25-1.00)"></asp:Label></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td><asp:DropDownList ID="ddlSdgCrop" runat="server"></asp:DropDownList></td>
                                <td><asp:TextBox ID="txtSdgArea" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtSdgWidth" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtSdgSlopeRatio" runat="server" CssClass="gvTextBox"></asp:TextBox></td>
                                <td><asp:Button ID="btnSdgClear" runat="server" Text="Clear" OnClientClick="return ClearInputs('tblSdg')"/></td>
                                <td style="vertical-align:top"><a id="aSdgHide" runat="server"  href="#close" title="Close" onclick="hideDiv('MainContent_fsetSdg');" class="BMPclose">Hide</a></td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </section>

    <section id="ddlModalFerts" class="modalDialog" style="width:15%">
        <div>
            <label id="lblFerts" runat="server" >Select Fertilizer</label>
            <a href="#close" title="Close" class="close">X</a>
            <table>
                <tr>
                    <td style="text-align:right">
                        <select id="ddlFerts" onchange="TakeNutrients(this)" ></select>
                    </td>
                </tr>
            </table>
        </div>
    </section>
            
    <section id="MixCropsModal" class="modalDialog" style="overflow:scroll; width:150%">
        <div>         
            <a href="#close" title="Close" class="close">X</a>
            <table>
                <tr>
                    <td style="text-align:right">
                        <asp:LinkButton ID="btnSaveMixedCrops" runat="server" Text="Save Crops Selected"></asp:LinkButton> 
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvCrops" runat="server" AutoGenerateColumns="false" >
                <FooterStyle CssClass="gvFooterStyle" />
                <RowStyle CssClass="gvRowStyle" />
                <SelectedRowStyle CssClass="gvSelectedRowStyle" />            
                <HeaderStyle CssClass="gvHeaderStyle" />
                <Columns>
                    <asp:TemplateField HeaderText="Select" >
                        <ItemTemplate>
                            <input type="checkbox" ID="chkSelect" value='<%#Eval("selected")%>' runat="server"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Code" >
                        <ItemTemplate>
                            <input ID="lblCode" readonly="redonly" style="border:none" runat="server" value='<%#Eval("code")%>'/> 
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name" >
                        <ItemTemplate>
                            <input ID="lblName" readonly="redonly" style="border:none" runat="server" value='<%#Eval("name")%>' /> 
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Percentage(%)" >
                        <ItemTemplate>
                            <input ID="txtPercentage" class="gvTextBox" runat="server" value='<%#Eval("percentage")%>' onchange="calculatePP(this);"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Plant Population(seeds/ac)" >
                        <ItemTemplate>
                            <input ID="txtPP" class="gvTextBox" runat="server" value='<%#Eval("pp")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <input ID="txtPPDefault" class="gvTextBox" type="hidden" runat="server" value='<%#Eval("ppDefault")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>         
            <input type="hidden" runat="server" id="hdnOperationNumber"/>
        </div>
    </section>

</asp:Content>
