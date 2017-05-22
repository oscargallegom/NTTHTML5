function CheckEntry(txt) {
    if (txt.value == "") { lblMessage.innerText = "Location can not be blank. Please enter Location"; lblMessage.style.color = "Red"; lblMessage.style.display = "";txt.style.backgroundColor = "Red"; }
    else { txt.style.backgroundColor = ""; lblMessage.innerText = ""; lblMessage.style.display = "none"; lblMessage.style.color = ""; }
}

function countySelected(ddlCounties) {
    var txt = document.getElementById("MainContent_txtCounty")
    txt.value = ddlCounties[ddlCounties.selectedIndex].text;
}

function selectCounties() {
    var stateAbr = document.getElementById("MainContent_ddlStates")[document.getElementById("MainContent_ddlStates").selectedIndex].value;
    var countiesAll = document.getElementById("MainContent_ddlAllCounties");
    var counties = document.getElementById("MainContent_ddlCounties");
    var i = 0;
    counties.length = 0;

    for (var n = 0; n < countiesAll.length - 1; n++) {
        state = countiesAll[n].value.substring(0, 2);
        if (stateAbr == state) {
            //                if (stateAbr == state || n == 0) {
            //                    if (n == 0) { counties.options[i] = new Option(document.getElementById("lblZoomCounty").value, ""); }
            counties.options[i] = new Option(countiesAll[n].text, countiesAll[n].value);
            i++;
            //                    countyselect.add(countiesAll[n]);
        }
    }
    counties.selectedIndex = 0;
    countySelected(counties);
}
