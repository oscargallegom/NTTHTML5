﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GoogleMap.aspx.vb" Inherits="NTTHTML5.GoogleMap" %>
<%@ Register TagPrefix="cc3" Namespace="BunnyBear" Assembly="msgBox" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"; style="width:95%">
    <head>
    <meta name="viewport" content="initial-scale=1.0, user-scalable=no"/>
    <title>AOI EDITING TOOL</title>
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="https://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css"/>
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="https://code.jquery.com/ui/1.11.1/jquery-ui.js"></script>
    <%--<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?libraries=geometry&sensor=false"></script>--%>
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?libraries=drawing,geometry&key=AIzaSyD4HGcSlTUPtyXsTqWKYDFWaLHBkoo9wl8"></script>
    <%--<script src="https://google-maps-utility-library-v3.googlecode.com/svn-history/r128/trunk/keydragzoom/src/keydragzoom.js" type="text/javascript"></script>
    <script src="http://google-maps-utility-library-v3.googlecode.com/svn/trunk/markerwithlabel.js" type="text/javascript"></script>
    <script src="http://google-maps-utility-library-v3.googlecode.com/svn/trunk/markerwithlabel/src/markerwithlabel.js" type="text/javascript"></script>--%>
    <script src="../Scripts/infoBox.js" type="text/javascript"></script>
    <style type="text/css">
        #map, html, body {
        padding: 0;
        margin: 0;
        height: 100%;
        }

        #dvPanel {
        font-family: Arial, sans-serif;
        font-size: 13px;
        float: right;
        margin: 0px;
        }

        #color-palette {
        clear: both;
        }

        .color-button {
        width: 14px;
        height: 14px;
        font-size: 0;
        margin: 2px;
        float: left;
        cursor: pointer;
        }
       
        #delete-button {
        margin-top: 10px;
        }
        
        #info-button {
        margin-top: 10px;
        }

        #savebutton {
        margin-top: 10px;
        }

        #TOPNAV {
        margin-top: 10px;
        font-family: Arial, sans-serif;
        font-size: 13px;
        float: right;
        }
        
        #dataPanel {
        margin-top: 10px;
        }
        p.margin
        {
        margin: 0; 
        }
        #textLong
        {
            width: 208px;
        }
        #lblFarm
        {
            width: 60px;
        }
        #lblField
        {
            width: 59px;
        }
        #bntDelete
        {
            width: 93px;
        }        
        .btnStyle
        {
            background-color:#0033CC; font-weight: bold; font-style: italic; font-size:medium; Color:White;
        }
        
        .trStyle
        {
            text-align:center;
            font-size: 16px; font-weight: bold; font-style: italic; font-variant: normal; color: #000000; background-color: #99CCFF;
        }
        .tableStyle
        {
            width: 100%;
            border-style: solid;
            border-width:thin;
        }
        .subTitleStyle
        {
            background-color:#FFFF99; 
            font-weight: bold;
        }
        .inputStyle
        {
            border-style: none; 
            background-color: #EFEFEF;
        }
    </style>
    

    <script type="text/javascript">

        function turnOffControls() {
                document.getElementById("dvForm").style.display = "none";
                document.getElementById("map").style.display = "none";
                document.getElementById("dvWait").style.display = "";
        }

    function pageLoad()
    {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
    }
        var drawingManager;
        var selectedShape;
        var colors = ['#1E90FF', '#FF1493'];
        var selectedColor;
        var colorButtons = {};
        var map = null;
        var geocoder = null;
        var inputStr;
        var strDrawnAOI = '';
        var x;
        var shapes = [];
        //        var newShape;
        var strFarmName;
        var strFarmXY;
        var arrayFieldsNames = [];
        var arrayFieldsArea = [];
        var arrayFieldsXY = [];
        var strStateCounty;
        var boundsPreDraw = null;
        var infoWindow;
        var infowindow = new google.maps.InfoWindow();
        var marker;

        function turnOnOffOption1(option1) {
            var opt1 = "trNavigation";
            var opt2 = "trShapeFile";
            var opt3 = "trTools";

            switch (option1) {
                case opt1:
                    document.getElementById(opt1).style.display = "";
                    document.getElementById(opt2).style.display = "none";
                    document.getElementById(opt3).style.display = "none";
                    break;
                case opt2:
                    document.getElementById(opt1).style.display = "none";
                    document.getElementById(opt2).style.display = "";
                    document.getElementById(opt3).style.display = "none";
                    break;
                case opt3:
                    document.getElementById(opt1).style.display = "none";
                    document.getElementById(opt2).style.display = "none";
                    document.getElementById(opt3).style.display = "";
                    break;
            }
        }
        
    $(function () {
        $('#btnContinue').click(function () {
            //Create a Dialog (modal)
            $("#dialog-modal").dialog({
                modal: false,
                hide : false
            });
        });
    });

    function clearSelection() {
            //alert('clearSelection()');
            if (document.getElementById("polyTypeFarm").checked == false && document.getElementById("polyTypeField").checked == false && drawingManager.drawingMode != null) {
                //alert(shapes.length);
                //alert(drawingManager.drawingMode);
                alert('Please specify the Area of Interest type: Farm or Field?');
                document.getElementById("polyTypeFarm").checked = true;
            }
            if (strFarmName != null && document.getElementById("polyTypeFarm").checked == true && drawingManager.drawingMode != null) {
                if (strFarmName != "") {
                    alert('Only one farm can be selected.');
                    document.getElementById("polyTypeField").checked = true;
                }
            }
            if (selectedShape) {
                selectedShape.setEditable(false);
                selectedShape = null;
            }
        }

        function setSelection(shape) {
            clearSelection();
            //alert('setSelection(shape)');
            selectedShape = shape;
            shape.setEditable(true);
            //05022014 Label the field***
            if (shape.content.indexOf('farm:') == -1) {
                showSelectedShapeInfo();
            }

            if (document.getElementById("polyTypeField").checked) {
                google.maps.event.addListener(shape.getPath(), 'insert_at', function () {

                    //alert(1);
                    var strTempDeletePolyInfo = shape.content;
                    var strTempInfo = strTempDeletePolyInfo.split(',');
                    var intIndex = strTempInfo[0].indexOf(":");
                    var strTempDeletePolyName = strTempInfo[0].substring(intIndex + 1).replace(/^\s+|\s+$/g, '');
                    for (var j = 0; j < arrayFieldsNames.length; j++) {
                        if (strTempDeletePolyName == arrayFieldsNames[j].replace(/^\s+|\s+$/g, '')) {
                            var areaPolyTemp = google.maps.geometry.spherical.computeArea(shape.getPath());
                            arrayFieldsArea[j] = areaPolyTemp;
                        }
                    }
                });
                google.maps.event.addListener(shape.getPath(), 'set_at', function () {

                    //alert(3);
                    var strTempDeletePolyInfo = shape.content;
                    var strTempInfo = strTempDeletePolyInfo.split(',');
                    var intIndex = strTempInfo[0].indexOf(":");
                    var strTempDeletePolyName = strTempInfo[0].substring(intIndex + 1).replace(/^\s+|\s+$/g, '');
                    for (var j = 0; j < arrayFieldsNames.length; j++) {
                        if (strTempDeletePolyName == arrayFieldsNames[j].replace(/^\s+|\s+$/g, '')) {
                            var areaPolyTemp = google.maps.geometry.spherical.computeArea(shape.getPath());
                            arrayFieldsArea[j] = areaPolyTemp;
                        }
                    }
                });
                google.maps.event.addListener(shape.getPath(), 'remove_at', function () {
                    //alert(2);
                    var strTempDeletePolyInfo = shape.content;
                    var strTempInfo = strTempDeletePolyInfo.split(',');
                    var intIndex = strTempInfo[0].indexOf(":");
                    var strTempDeletePolyName = strTempInfo[0].substring(intIndex + 1).replace(/^\s+|\s+$/g, '');
                    for (var j = 0; j < arrayFieldsNames.length; j++) {
                    	if (strTempDeletePolyName == arrayFieldsNames[j].replace(/^\s+|\s+$/g, '')) {
                            var areaPolyTemp = google.maps.geometry.spherical.computeArea(shape.getPath());
                            arrayFieldsArea[j] = areaPolyTemp;
                        }
                    }
                });

            }
            if (shape.content.indexOf('farm') != -1) {
                //alert('farm');
                var vertices = shape.getPath();
                //                var bounds = new google.maps.LatLngBounds();
                //                var polyCenter = bounds.getCenter();
                //                for (var i = 0; i < vertices.length; i++) {
                //                    var xy = vertices.getAt(i);
                //                    bounds.extend(xy);
                //                }
                //inputStr = vertices.b[0].mb + "," + vertices.b[0].lb;
                //inputStr = vertices.getAt(0);
                inputStr = vertices.getAt(0).lng() + "," + vertices.getAt(0).lat();
                codeLatLngStateLong(function (addr) {
                    //alert("The centroid of AOI is located in: " + addr);
                    document.getElementById("StateName").value = addr;
                });


                codeLatLngState(function (addr) {
                    //alert("The centroid of AOI is located in: " + addr);
                    document.getElementById("StateAbbr").value = addr;
                });

                codeLatLngCounty(function (addr) {
                    //alert("The centroid of AOI is located in: " + addr);
                    document.getElementById("CountyName").value = addr;
                });
                selectedShape.set('fillColor', '#1E90FF');
            } else {
                //alert('filed');
                selectedShape.set('fillColor', '#FF1493');                
            }
        }

        function SetLable(shape, lable) {
            var regionsOptions;
            var regionLabel;
            var bounds = new google.maps.LatLngBounds();
            var i;
            var coordinates =  shape.getPath().getArray();
            var totalLat = 0;
            var totalLong = 0; 
            
            for (i = 0; i < coordinates.length; i++) { 
                totalLat = totalLat + coordinates[i].lat();
                totalLong = totalLong + coordinates[i].lng();
            }
            totalLat = totalLat / coordinates.length ;
            totalLong = totalLong / coordinates.length ;           
            var latLng = new google.maps.LatLng(totalLat, totalLong);

            regionsOptions = {
                content: lable,
                boxStyle: { textAlign: "left", 
                    fontSize: "14px",
                    whiteSpace : "nowrap",
                    lineHeight: "16px",
                    fontWeight: "bold",
                    fontFamily: "Tahoma",
                    color: "white"},
                disableAutoPan: true,
                position: latLng,
                closeBoxURL: "",
                isHidden: false,
                pane: "floatPane",
                enableEventPropagation: true,
                boxClass: "regionLabel"
                };
                regionLabel = new InfoBox(regionsOptions);
                if (lable == "DeleteLabel") {
                    regionsOptions = { content: "" };
                    regionLabel.open(map)
                }   //regionLabel.setMap(null) }   //regionLabel.close(map);
                else { regionLabel.open(map); }
        }

        function deleteSelectedShape() {
            //alert('deleteSelectedShape()');
            if (selectedShape) {
                //SetLable(selectedShape, "DeleteLabel");
                selectedShape.setMap(null);
                //alert('selectedShape: ' + selectedShape.content);
                ////                alert('shapes: ' + shapes.content);
                // Find and remove item from an array
                var i = shapes.indexOf(selectedShape);
                if (i != -1) {
                    shapes.splice(i, 1);
                }

                // Delete polygon's content
                var strTempDeletePolyInfo = selectedShape.content;
                var strTempInfo = strTempDeletePolyInfo.split(',');
                var intIndex = strTempInfo[0].indexOf(":");
                var strTempDeletePolyName = strTempInfo[0].substring(intIndex + 1).replace(/^\s+|\s+$/g, '');
                if (strTempDeletePolyName == strFarmName.replace(/^\s+|\s+$/g, '')) {
                    strFarmName = "";
                    strFarmXY = "";
                    alert('You just deleted a farm');
                }
                else {
                    //alert(arrayFieldsNames.length);
                    for (var j = 0; j < arrayFieldsNames.length; j++) {
                        //                        alert(j);
                        //                        alert(":" + strTempDeletePolyName);
                        //                        alert(":"+arrayFieldsNames[j]);
                        if (strTempDeletePolyName == arrayFieldsNames[j].replace(/^\s+|\s+$/g, '')) {
                            arrayFieldsNames.splice(j, 1);
                            arrayFieldsArea.splice(j, 1);
                            arrayFieldsXY.splice(j, 1);
                            alert('You just deleted a field');
                        }
                    }

                }

                if (shapes.length == 0) {
                    document.getElementById("polyTypeFarm").checked = true;
                }

            }
        }

        function showSelectedShapeInfo() {
            if (selectedShape) {
                //                //selectedShape.setMap(null);
                //                //Since this Polygon only has one path, we can call getPath() to return the MVCArray of LatLngs
                //                var vertices = selectedShape.getPath();
                //                var bounds = new google.maps.LatLngBounds();
                ////                var contentString = "<b>User Defined Polygon</b><br />";
                ////                //Iterate over the vertices.
                ////                for (var i = 0; i < vertices.length; i++) {
                ////                    var xy = vertices.getAt(i);
                ////                    bounds.extend(xy);
                ////                    contentString += "<br />" + "Coordinate: " + i + "<br />" + xy.lat().toFixed(3) + "," + xy.lng().toFixed(3);
                ////                }
                //                var polyCenter = bounds.getCenter();
                //                inputStr = polyCenter.lat() + "," + polyCenter.lng();
                //                var myLatlng = new google.maps.LatLng(inputStr);
                //                var infowindow = new google.maps.InfoWindow({
                //                    content: selectedShape.content,
                //                });
                ////                var marker = new google.maps.Marker({
                ////                    position: polyCenter,
                ////                    map: map,
                ////                    title: 'Filed Information'
                ////                });
                //                google.maps.event.addListener(selectedShape, 'click', function () {
                //                    infowindow.open(map, selectedShape);
                //                });

                var strTempInfo = selectedShape.content.split(':');
                var intIndex = strTempInfo[1].indexOf(",");
                var strFiledName = strTempInfo[1].slice(1, intIndex);
                var bounds = new google.maps.LatLngBounds();
                var polyCenter = bounds.getCenter();
                inputStr = polyCenter.lat() + "," + polyCenter.lng();
                var myLatlng = new google.maps.LatLng(inputStr);

                var marker = new google.maps.Marker({
                    position: myLatlng,
                    labelContent: strFiledName,
                    labelAnchor: new google.maps.Point(95, 20),
                    labelClass: "labels",
                    labelStyle: { opacity: 0.75 },
                    zIndex: 999999,
                    map: map
                })

                google.maps.event.addListener(selectedShape, 'click', function (e) {
                    var content = "<div class='infowindow'>";
                    content += "Field's Name: " + strFiledName + "<br/>";
                    //                    content += "Location information:"+ "<br/>";
                    content += "Current location's latitude is: " + e.latLng.lat() + ", ";
                    content += "longitude is: " + e.latLng.lng() + "</div>";

                    HandleInfoWindow(e.latLng, content);
                });
            }
        }

        function saveSelectedShapeInfo() {
            //alert('saveSelectedShapeInfo()');
            if (selectedShape) {
                var vertices = selectedShape.getPath();
                var bounds = new google.maps.LatLngBounds();
                var polyCenter = bounds.getCenter();
                //document.getElementById("savedata").value = "";
                document.getElementById("savedata").value += selectedShape.content;
                /*              if(selectedShape.content.contains("farm")) {

                }else {
                document.getElementById("savedata").value += "Field(";
                } */
                for (var i = 0; i < vertices.length; i++) {
                    var xy = vertices.getAt(i);
                    bounds.extend(xy);
                    if (i == (vertices.length - 1)) {
                        document.getElementById("savedata").value +=
                xy.lng().toFixed(6) + "," + xy.lat().toFixed(6) + " ";
                        //                        strFarmXY = xy.lng().toFixed(6) + "," + xy.lat().toFixed(6);
                    }
                    else {
                        document.getElementById("savedata").value +=
                xy.lng().toFixed(6) + "," + xy.lat().toFixed(6) + " ";
                        //                        arrayFieldsXY.push(xy.lng().toFixed(6) + "," + xy.lat().toFixed(6));
                    }
                    //document.getElementById("savedata").value += "), ";
                    //document.getElementById("savedata").value += polyCenter.lat().toFixed(3)+"," + polyCenter.lng().toFixed(3) + ")\n";
                }
                document.getElementById("savedata").value += "\n";
            }
        }

        function codeLatLng(inputLatLng) {
            var input;
            if (document.getElementById('latlng').value != "") {
                input = document.getElementById('latlng').value
            }
            else {
                input = inputLatLng;
            }
            document.getElementById("Textlatlng").value = input;
            var latlngStr = input.split(',', 2);
            var lat = parseFloat(latlngStr[0]);
            var lng = parseFloat(latlngStr[1]);
            var latlng = new google.maps.LatLng(lat, lng);
            geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[1]) {
                        map.setCenter(results[1].geometry.location);
                        map.setZoom(20);
                        marker = new google.maps.Marker({
                            position: latlng,
                            map: map
                        });
                        infowindow.setContent(results[1].formatted_address);
                        infowindow.open(map, marker);
                    } else {
                        alert('No results found');
                    }
                } else {
                    alert('Geocoder failed due to: ' + status);
                }
            });
        }

        function codeAddress(inputAddress) {
            var address;
            if (document.getElementById('address').value != "") {
                address = document.getElementById('address').value
            }
            else {
                address = inputAddress;
            }
            document.getElementById("TextAddress").value = address;
            geocoder.geocode({ 'address': address }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    map.setCenter(results[0].geometry.location);
                    map.setZoom(20);
                    var marker = new google.maps.Marker({
                        map: map,
                        position: results[0].geometry.location
                    });
                } else {
                    alert('Geocode was not successful for the following reason: ' + status);
                }
            });
        }


        function codeLatLngState(callback) {
            var latlngStr = inputStr.split(',', 2);
            var lat = parseFloat(latlngStr[1]);
            var lng = parseFloat(latlngStr[0]);
            //            var latlng = inputStr;
            var latlng = new google.maps.LatLng(lat, lng);
            if (geocoder) {
                geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (results[0]) {
                            //break down the three dimensional array into simpler arrays
                            for (i = 0; i < results.length; ++i) {
                                var super_var1 = results[i].address_components;
                                for (j = 0; j < super_var1.length; ++j) {
                                    var super_var2 = super_var1[j].types;
                                    var strTempStateCounty = "";
                                    for (k = 0; k < super_var2.length; ++k) {
                                        //find State
                                        if (results[0].formatted_address.indexOf("Puerto Rico") != -1) {
                                            if (super_var2[k] == "country") {
                                                //put the state abbreviation in the form
                                                callback(super_var1[j].short_name);
                                            }                                        
                                        }
                                        else {
                                            if (super_var2[k] == "administrative_area_level_1") {
                                                //put the state abbreviation in the form
                                                callback(super_var1[j].short_name);
                                            }
                                        }
                                    }
                                }
                            }
                            //                            callback(markerAddress);
                        } else {
                            alert("No results found");
                        }
                    } else {
                        alert("Geocoder failed due to: " + status);
                    }
                });
            }
        }

        function codeLatLngStateLong(callback) {
            var latlngStr = inputStr.split(',', 2);
            var lat = parseFloat(latlngStr[1]);
            var lng = parseFloat(latlngStr[0]);
            //            var latlng = inputStr;
            var latlng = new google.maps.LatLng(lat, lng);
            if (geocoder) {
                geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (results[0]) {
                            //break down the three dimensional array into simpler arrays
                            for (i = 0; i < results.length; ++i) {
                                var super_var1 = results[i].address_components;
                                for (j = 0; j < super_var1.length; ++j) {
                                    var super_var2 = super_var1[j].types;
                                    var strTempStateCounty = "";
                                    for (k = 0; k < super_var2.length; ++k) {
                                        //find State
                                        if (results[0].formatted_address.indexOf("Puerto Rico") != -1) {
                                            if (super_var2[k] == "country") {
                                                //put the state abbreviation in the form
                                                callback(super_var1[j].long_name);
                                            }                                        
                                        }
                                        else {
                                            if (super_var2[k] == "administrative_area_level_1") {
                                                //put the state abbreviation in the form
                                                callback(super_var1[j].long_name);
                                            }
                                        }
                                    }
                                }
                            }
                            //                            callback(markerAddress);
                        } else {
                            alert("No results found");
                        }
                    } else {
                        alert("Geocoder failed due to: " + status);
                    }
                });
            }
        }

        function codeLatLngCounty(callback) {
            var latlngStr = inputStr.split(',', 2);
            var lat = parseFloat(latlngStr[1]);
            var lng = parseFloat(latlngStr[0]);
            var latlng = new google.maps.LatLng(lat, lng);
            //            var latlng = inputStr;
            if (geocoder) {
                geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (results[0]) {
                            //break down the three dimensional array into simpler arrays
                            for (i = 0; i < results.length; ++i) {
                                var super_var1 = results[i].address_components;
                                for (j = 0; j < super_var1.length; ++j) {
                                    var super_var2 = super_var1[j].types;
                                    var strTempStateCounty = "";
                                    for (k = 0; k < super_var2.length; ++k) {
                                        //find county
                                        if (results[0].formatted_address.indexOf("Puerto Rico") != -1) {
                                            if (super_var2[k] == "administrative_area_level_1") {
                                                //put the state abbreviation in the form
                                                callback(super_var1[j].long_name);
                                            }                                        
                                        }
                                        else {
                                            if (super_var2[k] == "administrative_area_level_2") {
                                                //put the county name in the form
                                                callback(super_var1[j].long_name);
                                            }
                                        }
                                    }
                                }
                            }
                            //                            callback(markerAddress);
                        } else {
                            alert("No results found");
                        }
                    } else {
                        alert("Geocoder failed due to: " + status);
                    }
                });
            }
        }

        //        function initContinued(addr) {
        //            //alert(addr);
        //            document.getElementById("StateCountyName").value = addr;
        //            alert(document.getElementById("StateCountyName").value);
        //        }

        /*       function selectColor(color) {
        //alert(couleur);
        selectedColor = color;
        for (var i = 0; i < colors.length; ++i) {
        var currColor = colors[i];
        colorButtons[currColor].style.border = currColor == color ? '2px solid #789' : '2px solid #fff';
        }
        // Retrieves the current options from the drawing manager and replaces the
        // stroke or fill color as appropriate.
        var polylineOptions = drawingManager.get('polylineOptions');
        polylineOptions.strokeColor = color;
        drawingManager.set('polylineOptions', polylineOptions);

        var rectangleOptions = drawingManager.get('rectangleOptions');
        rectangleOptions.fillColor = color;
        drawingManager.set('rectangleOptions', rectangleOptions);

        var circleOptions = drawingManager.get('circleOptions');
        circleOptions.fillColor = color;
        drawingManager.set('circleOptions', circleOptions);

        var polygonOptions = drawingManager.get('polygonOptions');
        polygonOptions.fillColor = color;
        drawingManager.set('polygonOptions', polygonOptions);
        }

        function setSelectedShapeColor(color) {
        if (selectedShape) {
        if (selectedShape.type == google.maps.drawing.OverlayType.POLYLINE) {
        selectedShape.set('strokeColor', color);
        } else {
        selectedShape.set('fillColor', color);
        }
        }
        } */

        /*       function makeColorButton(color) {
        var button = document.createElement('span');
        button.className = 'color-button';
        button.style.backgroundColor = color;
        google.maps.event.addDomListener(button, 'click', function() {
        selectColor(color);
        setSelectedShapeColor(color);
        });

        return button;
        }

        function buildColorPalette() {
        alert("buildColorPalette()");
        var colorPalette = document.getElementById('color-palette');
        for (var i = 0; i < colors.length; ++i) {
        var currColor = colors[i];
        var colorButton = makeColorButton(currColor);
        colorPalette.appendChild(colorButton);
        colorButtons[currColor] = colorButton;
        }
        selectColor(colors[0]);
        } */

        function findAddress(address) {
            var addressStr = document.getElementById("stateselect")[document.getElementById("stateselect").selectedIndex].text;
            var stateAbr = document.getElementById("stateselect")[document.getElementById("stateselect").selectedIndex].value;
            var countiesAll = document.getElementById("countyAll");
            var state;
            var countiSelect = document.getElementById("countyselect");
            var i = 0;
            countiSelect.length = 0;
            for (var n = 0; n < countiesAll.length - 1; n++) {
                state = countiesAll[n].value.substring(0, 2);
                if (stateAbr == state || n == 0) {
                    if (n == 0) { countiSelect.options[i] = new Option(document.getElementById("lblZoomCounty").value, ""); }
                    else { countiSelect.options[i] = new Option(countiesAll[n].text, countiesAll[n].value); }
                    i++;
                    //                    countyselect.add(countiesAll[n]);
                }
            }
            if (countiSelect.length == 1) { return; }
            if (!address && (addressStr != ''))
                address = "State of " + addressStr;
            else
                address = addressStr;
            if ((address != '') && geocoder) {
                geocoder.geocode({ 'address': address }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (status != google.maps.GeocoderStatus.ZERO_RESULTS) {
                            if (results && results[0]
	            && results[0].geometry && results[0].geometry.viewport)
                                map.fitBounds(results[0].geometry.viewport);
                        } else {
                            alert("No results found");
                        }
                    } else {
                        alert("Geocode was not successful for the following reason: " + status);
                    }
                });
            }
        }

        //in case to fine county
        function searchLocations() {
            var address = document.getElementById("addressInput").value;
            var geocoder = new google.maps.Geocoder();
            geocoder.geocode({ address: address }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    searchLocationsNear(results[0].geometry.location);
                } else {
                    alert(address + ' not found');
                }
            });
        }

        //
        function submitSelection(type) {
            turnOffControls();
            //            if (document.getElementById('hdf_Test').value != '') {
            //                drawPreSavedAOI(document.getElementById('hdf_Test').value);
            //            }
            //            else {

            arrayFieldsXY.length = 0;
            for (var n = 0; n < shapes.length; n++) {
                //                alert('shapes.length' + shapes.length);
                var shapesOne = shapes[n];
                if (shapesOne) {
                    var vertices = shapesOne.getPath();
                    var bounds = new google.maps.LatLngBounds();
                    var polyCenter = bounds.getCenter();
                    var strTempXY = "";
                    //document.getElementById("savedata").value = "";
                    //alert(shapesOne.content);
                    document.getElementById("savedata").value += shapesOne.content;
                    /*              if(selectedShape.content.contains("farm")) {

                    }else {
                    document.getElementById("savedata").value += "Field(";
                    } */
                    for (var i = 0; i < vertices.length; i++) {
                        var xy = vertices.getAt(i);
                        bounds.extend(xy);
                        if (i == (vertices.length - 1)) {
                            document.getElementById("savedata").value +=
                xy.lng().toFixed(6) + "," + xy.lat().toFixed(6) + " ";
                            strTempXY += xy.lng().toFixed(6) + "," + xy.lat().toFixed(6) + " ";
                        }
                        else {
                            document.getElementById("savedata").value +=
                xy.lng().toFixed(6) + "," + xy.lat().toFixed(6) + " ";
                            strTempXY += xy.lng().toFixed(6) + "," + xy.lat().toFixed(6) + " ";
                        }
                        //document.getElementById("savedata").value += "), ";
                        //document.getElementById("savedata").value += polyCenter.lat().toFixed(3)+"," + polyCenter.lng().toFixed(3) + ")\n";
                    }
                    //document.getElementById("savedata").value += ")\n";
                    //                    var areaPoly = google.maps.geometry.spherical.computeArea(vertices);
                    //                    document.getElementById("savedata").value += "area:" + areaPoly+"; ";
                    //                    alert('shapesOne.content' + shapesOne.content);
                    if (shapesOne.content.indexOf("farm:") != -1) {

                        strFarmXY = strTempXY;
                    }
                    else {
                        if (arrayFieldsXY.indexOf(strTempXY) == -1) {
                            arrayFieldsXY.push(strTempXY);
                            //alert('arrayFieldsXY.push(strTempXY);' + strTempXY);
                        }
                    }
                }
                document.getElementById("FarmName").value = strFarmName;
                document.getElementById("FarmXY").value = strFarmXY;
                document.getElementById("FieldsName").value = arrayFieldsNames;
                document.getElementById("FieldsXY").value = arrayFieldsXY;
                document.getElementById("FieldsArea").value = arrayFieldsArea;
            }
            if (strFarmName != '' && arrayFieldsNames.length > 0) {
                //document.getElementById("dialog-modal").style.display = "none";
                return true;
            }
            else {
                if (type == "CopyFarm") { return true; }
                if (strFarmXY != '' && arrayFieldsXY.length > 0) { 
                    alert('Please make sure the names of the farm and field(s) are already assigned.'); 
                }
                else { 
                    alert('Please select only one farm and at least one field!'); 
                }
                return false;
            }
        }

        function initialize() {
            //if (document.getElementById("hdnEnd").value == "Finished") { }
            //put lables in hidden input controls
            document.getElementById("bntDelete").value = document.getElementById("lblDelete").value;
            document.getElementById("lblZoomToState").label = document.getElementById("lblZoomState").value;
            //document.getElementById("lblZoomToCounty").label = document.getElementById("lblZoomCounty").value;
            //alert('initialize()');
            //            myWindow = window.open("", "", "width=200,height=100");
            //            myWindow.document.write("<p>This is 'myWindow'</p>");
            var tableId = '0IMZAFCwR-t7jZnVzaW9udGFibGVzOjIxMDIxNw';
            var locationColumn = 'State-County';

            geocoder = new google.maps.Geocoder();
            //
            map = new google.maps.Map(document.getElementById('map'), {
                zoom: 5,
                center: new google.maps.LatLng(39.10960, -96.5),
                mapTypeId: google.maps.MapTypeId.HYBRID,
                mapTypeControl: true,
                //mapTypeControlOptions: {style: google.maps.MapTypeControlStyle.DROPDOWN_MENU},
                navigationControl: true,
                //disableDefaultUI: true,
                scaleControl: true,
                overviewMapControl: true,
                zoomControl: true                
            });

            //            map = new google.maps.Map($('#map')[0], options);
            infoWindow = new google.maps.InfoWindow({
                maxWidth: 520,
                styles: [{ "featureType": "water",
                    "stylers": [ { "visibility": "on" }, {"color": "#000000"}, {"hue": "#000000" }  ]
                }]
            });
            //
            //            map.enableKeyDragZoom();
//            map.enableKeyDragZoom({
//                key: "shift",
//                boxStyle: {
//                    border: "3px dashed black",
//                    backgroundColor: "transparent",
//                    opacity: 1.0
//                },
//                veilStyle: {
//                    backgroundColor: "white",
//                    opacity: 0.15,
//                    cursor: "crosshair"
//                },
//                visualEnabled: true,
//                visualPosition: google.maps.ControlPosition.LEFT,
//                visualPositionMargin: new google.maps.Size(35, 0),
//                visualImages: {
//                    off: "dragzoom-off.png",
//                    on: "dragzoom-on.png",
//                    hot: "dragzoom-hot.png"
//                },
//                visualTips: {
//                    off: "Turn on",
//                    on: "Turn off"
//                }
//            });
            var polyOptions = {
                strokeWeight: 0,
                fillOpacity: 0.3,
                editable: true
            };
            // Creates a drawing manager attached to the map that allows the user to draw
            // markers, lines, and shapes. //google.maps.drawing.OverlayType.POLYGON
            drawingManager = new google.maps.drawing.DrawingManager({
                drawingMode: null,
                drawingControl: true,
                drawingControlOptions: {
                    position: google.maps.ControlPosition.TOP_CENTER,
                    drawingModes: [

                                google.maps.drawing.OverlayType.POLYGON]
                },
                markerOptions: {
                    draggable: true
                },
                polylineOptions: {
                    editable: true
                },
                rectangleOptions: polyOptions,
                circleOptions: polyOptions,
                polygonOptions: polyOptions,
                map: map
            });
            //drawingManager.setMap(map);
            //
            //Comment the line below because it cause two duplicate colore palette,Yang
            //buildColorPalette();
            //var polygons = [];

            strDrawnAOI = '<%= preDrawnAOI %>';
            document.getElementById("savedata").value = "";
            //document.getElementById("savedata").value = strDrawnAOI;
            if (strDrawnAOI.indexOf('farm') != -1) {
                //                alert(strDrawnAOI);
                drawPreSavedAOI(strDrawnAOI);
            }
            //            alert(strDrawnAOI);
            google.maps.event.addListener(drawingManager, 'overlaycomplete', function (e) {
                if (e.type != google.maps.drawing.OverlayType.MARKER) {
                    // Switch back to non-drawing mode after drawing a shape.
                    drawingManager.setDrawingMode(null);
                    // Add an event listener that selects the newly-drawn shape when the user
                    // mouses down on it.
                    var newShape = e.overlay;
                    shapes.push(newShape);
                    newShape.type = e.type;

                    if (document.getElementById("polyTypeFarm").checked) {
                        newShape.content = "farm: ";
                        var person = prompt("Please enter the farm name:", "farm");
                        if (person != null && person != "") {
                            newShape.content += person + ", ";
                            strFarmName = person;
                        }
                        else {
                            alert("Your did not specify farm name! A default value will be assigned.");
                            person = "farm";
                            newShape.content += person + ", ";
                            strFarmName = person;
                        }
                    } else {
                        newShape.content = "field: ";
                        person = prompt("Please enter the field name:", "field".concat(shapes.length - 1));
                        //person = prompt("Please enter the field name:", "field1");  change to have different field names. In case user do not change them. Oscar Gallego 01232015
                        if (person != null && person != "") {
                            newShape.content += person + ", ";
                            arrayFieldsNames.push(person);
                        }
                        else {
                            alert("You did not specify field name! A default value will be assigned.");
                            person = "field".concat(shapes.length - 1);
                            //person = "field1"; change to have different field names. In case user do not change them. Oscar Gallego 01232015
                            newShape.content += person + ", ";
                            arrayFieldsNames.push(person);
                        }

                        var areaPoly = google.maps.geometry.spherical.computeArea(newShape.getPath());
                        newShape.content += "area: " + areaPoly + ", ";
                        // a brand new polygon
                        arrayFieldsArea.push(areaPoly);
                        // if the user modifies polygon
                        google.maps.event.addListener(newShape.getPath(), 'insert_at', function () {

                            //add a new point;
                            var strTempDeletePolyInfo = newShape.content;
                            var strTempInfo = strTempDeletePolyInfo.split(',');
                            var intIndex = strTempInfo[0].indexOf(":");
                            var strTempDeletePolyName = strTempInfo[0].substring(intIndex + 1).replace(/^\s+|\s+$/g, '');
                            for (var j = 0; j < arrayFieldsNames.length; j++) {
                                if (strTempDeletePolyName == arrayFieldsNames[j].replace(/^\s+|\s+$/g, '')) {
                                    var areaPolyTemp = google.maps.geometry.spherical.computeArea(newShape.getPath());
                                    arrayFieldsArea[j] = areaPolyTemp;
                                }
                            }
                        });
                        google.maps.event.addListener(newShape.getPath(), 'set_at', function () {

                            //modify at point;
                            var strTempDeletePolyInfo = newShape.content;
                            var strTempInfo = strTempDeletePolyInfo.split(',');
                            var intIndex = strTempInfo[0].indexOf(":");
                            var strTempDeletePolyName = strTempInfo[0].substring(intIndex + 1).replace(/^\s+|\s+$/g, '');
                            for (var j = 0; j < arrayFieldsNames.length; j++) {
                                if (strTempDeletePolyName == arrayFieldsNames[j].replace(/^\s+|\s+$/g, '')) {
                                    var areaPolyTemp = google.maps.geometry.spherical.computeArea(newShape.getPath());
                                    arrayFieldsArea[j] = areaPolyTemp;
                                }
                            }
                        });
                        google.maps.event.addListener(newShape.getPath(), 'remove_at', function () {
                            //remove a point;
                            var strTempDeletePolyInfo = newShape.content;
                            var strTempInfo = strTempDeletePolyInfo.split(',');
                            var intIndex = strTempInfo[0].indexOf(":");
                            var strTempDeletePolyName = strTempInfo[0].substring(intIndex + 1).replace(/^\s+|\s+$/g, '');
                            for (var j = 0; j < arrayFieldsNames.length; j++) {
                                if (strTempDeletePolyName == arrayFieldsNames[j].replace(/^\s+|\s+$/g, '')) {
                                    var areaPolyTemp = google.maps.geometry.spherical.computeArea(newShape.getPath());
                                    arrayFieldsArea[j] = areaPolyTemp;
                                }
                            }
                        });
                        SetLable(newShape, person);
                    }
                    strDrawnAOI += newShape.content;

                    google.maps.event.addListener(newShape, 'click', function () {
                        //                        alert(newShape.content);
                        //                        google.maps.event.addListener(newShape.getPath(), 'set_at', function () {
                        //                            alert("set_at test");
                        //                        });
                        //                        google.maps.event.addListener(newShape.getPath(), 'insert_at', function () {
                        //                            alert("insert_at test");
                        //                        });
                        setSelection(newShape);
                    });
                    //                    google.maps.event.addListener(newShape, 'bounds_changed', function () {
                    //                        alert(0);
                    //                    });
                    //                    google.maps.event.addListener(newShape.getPath(), 'insert_at', function () {

                    //                        alert(1);
                    //                    });
                    //                    google.maps.event.addListener(newShape.getPath(), 'set_at', function () {

                    //                        alert(3);
                    //                    });
                    //                    google.maps.event.addListener(newShape.getPath(), 'remove_at', function () {
                    //                        alert(2);
                    //                    });
                    setSelection(newShape);
                }

            });
            // Clear the current selection when the drawing mode is changed, or when the
            // map is clicked.
            google.maps.event.addListener(drawingManager, 'drawingmode_changed', clearSelection);
            google.maps.event.addListener(map, 'click', clearSelection);
            google.maps.event.addDomListener(document.getElementById('bntDelete'), 'click', deleteSelectedShape);
            google.maps.event.addDomListener(document.getElementById('bntInfo'), 'click', showSelectedShapeInfo);
            //            google.maps.event.addDomListener(document.getElementById('savebutton'), 'click', saveSelectedShapeInfo);
            google.maps.event.addDomListener(window, 'load', initialize);

            //            var seletedState = document.getElementById("stateselect").value;
            //            alert(seletedState);        
            findAddress("United States");
            //
            var inputAddress = '<%= preAddress %>';
            if (inputAddress != "") {
                codeAddress(inputAddress);
            }
            //
            var inputLatLng = '<%= prelatlng %>';
            if (inputLatLng != "") {
                codeLatLng(inputLatLng);
            }

            if (strDrawnAOI != "") {
                document.getElementById("polyTypeFarm").checked = false;
                document.getElementById("polyTypeField").checked = true;
            }
            else {
                document.getElementById("polyTypeFarm").checked = true;
                document.getElementById("polyTypeField").checked = false;
            }
            //            google.maps.Map.enableKeyDragZoom();

            var btnFieldClick = document.getElementById('polyTypeField');
            btnFieldClick.onclick = handlerFieldClick;

            var btnFarmClick = document.getElementById('polyTypeFarm');
            btnFarmClick.onclick = handlerFarmClick;

            if (boundsPreDraw != null) {
                map.fitBounds(boundsPreDraw);
            }
            //add counties
            var layer = new google.maps.FusionTablesLayer({
                query: {
                    select: locationColumn,
                    from: tableId
                },
                styles: [{
                    polygonOptions: {
                        fillColor: '#FFFFFF',
                        fillOpacity: 0.01,
                        strokeColor: '#FF0000',
                        strokeWeight: 1
                    }
                }],
                map: map
            });

            google.maps.event.addDomListener(document.getElementById('countyselect'),
            'change', function () {
                updateMap(layer, tableId, locationColumn);
            });

        }

        function handlerFieldClick() {
            //alert('handler()');
            //alert('handlerFieldClick: ' + strDrawnAOI);
            //alert(strFarmName)

            if (strDrawnAOI == "") {
                if (strFarmName == null) {
                    alert('Please select a farm first, then select the fields.');
                    document.getElementById("polyTypeFarm").checked = true;
                }
            }
            else {
                if (strFarmName == "") {
                    alert('Please select a farm first, then select the fields.');
                    document.getElementById("polyTypeFarm").checked = true;
                }
            }



        }

        function handlerFarmClick() {
            //alert('handler()' + strDrawnAOI);
            //alert();
            //alert(strFarmName)

            if (strDrawnAOI != "") {
                if (strFarmName != "") {
                    alert('Only one farm is allowed, please continue to draw the fields.');
                    document.getElementById("polyTypeField").checked = true;
                }
            }
            //            else {
            //                if (strFarmName != "") {
            //                    alert('Only one farm is allowed, please continue draw the fields.');
            //                    document.getElementById("polyTypeField").checked = true;
            //                }
            //            }

        }

        function drawPreSavedAOI(strDrawnAOI) {
            // parse the information when ready creating a table of information
            x = strDrawnAOI.split('field:');
            // extract the coordinates and store them in the array countyCoordinates
            for (i = 0; i < x.length; i++) {
                //                alert(x[i]);
                //farm: farm, -113.928223,35.245619 -113.983154,33.605470 -110.731201,35.083956 -112.170410,36.288563 
                //cao f1, area:23020745067.074097, -113.840332,35.236646 -113.906250,33.742613 -110.808105,35.065973 
                //                x[i] = x[i].replace(/[(]/g, '');
                //                x[i] = x[i].replace(/ ,/g, ':');
                //               var a = x[i].split(' ');//not using the space to split the string any more since the farm or field name may contain the space
                var arrayIfXY = x[i].split(', ');
                //                alert(arrayIfXY.length);
                //                for (var xyz = 0; xyz < arrayIfXY.length - 1; xyz++) {
                //                    alert("arrayIfXY:" + arrayIfXY[xyz]);
                //                }
                var newShape;
                var countyCoordinates = [];
                var points = [];
                var a;
                var n;
                var strTempName = "";
                if (arrayIfXY[0].indexOf('farm:') != -1) {
                    //farm
                    strFarmName = arrayIfXY[0].slice(6, arrayIfXY[0].length);
                    //                    alert("strFarmName: " + strFarmName);
                    strFarmXY = arrayIfXY[1];
                    //                    alert("strFarmXY: " + strFarmXY);
                    a = arrayIfXY[1].split(' ');
                    for (var j = 0; j < a.length - 1; j++) {
                        if (a[j] != "") {
                            var strCoor = a[j].split(',');
                            var Latit = parseFloat(strCoor[1]);
                            var Longit = parseFloat(strCoor[0]);
                            var ll = new google.maps.LatLng(Latit, Longit);
                            points.push(ll);
                        }
                    }
                    newShape = new google.maps.Polygon({
                        paths: points,
                        strokeColor: '#FF0000',
                        strokeOpacity: 0.8,
                        strokeWeight: 1,
                        fillColor: '#1E90FF',
                        fillOpacity: 0.3
                        //strokeWeight: 0,
                    });
                    //                    newShape.content = "farm: ";
                    //                    n = arrayIfXY[0].indexOf(":");
                    //                    alert("arrayIfXY[0]:"+arrayIfXY[0]);
                    //                    alert(newShape.content);
                    newShape.content = arrayIfXY[0] + ", ";
                    boundsPreDraw = getBoundsForPoly(newShape);
                    var polyCenter = strFarmXY.split(' ');
                    inputStr = polyCenter[0];
                    codeLatLngState(function (addr) {
                        //alert("The centroid of AOI is located in: " + addr);
                        document.getElementById("StateAbbr").value = addr;
                    });

                    codeLatLngStateLong(function (addr) {
                        //alert("The centroid of AOI is located in: " + addr);
                        document.getElementById("StateName").value = addr;
                    });

                    codeLatLngCounty(function (addr) {
                        //alert("The centroid of AOI is located in: " + addr);
                        document.getElementById("CountyName").value = addr;
                    });
                }
                else {
                    //field
                    arrayFieldsNames.push(arrayIfXY[0].slice(1, arrayIfXY[0].length));

                    arrayFieldsXY.push(arrayIfXY[2]);
                    //                    alert("arrayFieldsXY.push(arrayIfXY[2]):" + arrayIfXY[2]);
                    a = arrayIfXY[2].split(' ');
                    for (var jfield = 0; jfield < a.length - 1; jfield++) {
                        if (a[jfield] != "") {
                            var strCoorfield = a[jfield].split(',');
                            var fieldLatit = parseFloat(strCoorfield[1]);
                            var fieldLongit = parseFloat(strCoorfield[0]);
                            var fieldll = new google.maps.LatLng(fieldLatit, fieldLongit);
                            points.push(fieldll);
                        }
                    }
                    newShape = new google.maps.Polygon({
                        paths: points,
                        strokeColor: '#FF0000',
                        strokeOpacity: 0.8,
                        strokeWeight: 1,
                        fillColor: '#FF1493',
                        fillOpacity: 0.3
                    });
                    var areaPolyTemp = google.maps.geometry.spherical.computeArea(newShape.getPath().getArray());
                    arrayFieldsArea.push(areaPolyTemp);
                    newShape.content = "field: ";
                    //                    n = arrayIfXY[0].indexOf(":");
                    newShape.content += arrayIfXY[0] + ", " + arrayIfXY[1] + ", ";
                    SetLable(newShape, arrayIfXY[0].slice(1, arrayIfXY[0].length));
                }

                //                n = arrayIfXY[0].indexOf(":");
                //                newShape.content += arrayIfXY[0].substring(n + 1) + ", ";
                //                //                alert("newShape.content" + newShape.content);
                //                newShape.content += arrayIfXY[1].substring(n + 1) + ", ";
                shapes.push(newShape);
                newShape.setMap(map);

                google.maps.event.addListener(newShape, 'click', function () {
                    this.setEditable(true);
                    setSelection(this);
                });
            }
        }

        function myFunction() {
            alert("Hello World!");
            var x = 5;
            return x;
        }

        function getBoundsForPoly(poly) {
            var bounds = new google.maps.LatLngBounds;
            poly.getPath().forEach(function (latLng) {
                bounds.extend(latLng);
            });
            return bounds;
        }

        function findCounty(address) {
            var addressStr = document.getElementById("countyselect").value;
            //            //Add Fusion County data here
            //            if (addressStr != '') {
            //                layer = new google.maps.FusionTablesLayer({
            //                    query: {
            //                        select: 'geometry',
            //                        from: '0IMZAFCwR-t7jZnVzaW9udGFibGVzOjIxMDIxNw',
            //                        //                        where: "'State-County' = 'IL-Cook'"
            //                        where: "'State-County' = '" + addressStr + "'"
            //                        //where: "delivery = '" + delivery + "'"

            //                    },
            //                    styles: [{
            //                        polygonOptions: {
            //                            fillColor: '#00FF00',
            //                            fillOpacity: 0.1
            //                        }
            //                    }]
            //                });
            //                layer.setMap(map);
            //            }
            //            //

            if (!address && (addressStr != '')) {
                var countyState = addressStr.split("-"); 
                //address = "County of " + addressStr;
                address =  countyState[1].trim() + " County, " + countyState[0];
                }
            else
                address = addressStr;

            if ((address != '') && geocoder) {
                geocoder.geocode({ 'address': address }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (status != google.maps.GeocoderStatus.ZERO_RESULTS) {
                            if (results && results[0] && results[0].geometry && results[0].geometry.viewport) {
                                map.fitBounds(results[0].geometry.viewport);
                                document.getElementById("trNavigation").style.display = "none";
                                document.getElementById("trTools").style.display = "";
                            }
                        } else { alert("No results found"); }
                    } else { alert("Geocode was not successful for the following reason: " + status); }
                });
            }
        }

        // Update the query sent to the Fusion Table Layer based on
        // the user selection in the select menu
        function updateMap(layer, tableId, locationColumn) {
            var delivery = document.getElementById('countyselect').value;
            if (delivery) {
                layer.setOptions({
                    query: {
                        select: locationColumn,
                        from: tableId,
                        where: "'State-County' = '" + delivery + "'"
                    }
                });
            } else {
                layer.setOptions({
                    query: {
                        select: locationColumn,
                        from: tableId
                    }
                });
            }
        }

        function HandleInfoWindow(latLng, content) {
            infoWindow.setContent(content);
            infoWindow.setPosition(latLng);
            infoWindow.open(map);
        }

        //        function loadCounty(address) {
        //            // fusion table test
        //            layer = new google.maps.FusionTablesLayer({
        //                query: {
        //                    select: '\'Geocodable address\'',
        //                    from: '1mZ53Z70NsChnBMm-qEYmSDOvLXgrreLTkQUvvg'
        //                }
        //            });
        //            layer.setMap(map);
        //        }

        //google.maps.event.addDomListener(window, 'load', initialize); <body onload="initialize()" >'<body onload="initialize()" >

    </script>
    </head>
    <body onload="initialize()" style="background-color:White; width: 95%;" >

    <div id="TOPNAV" style="padding-right: 0px; padding-left: 0px; margin-right: 0px; margin-left: 0px;" >&nbsp;</div>
    <div id="dvPanel" style="background-color:Menu; width: 26%; border-width:10" runat="server">
    <form id="Form1" action="" runat="server" style="width: 100%;">
        <div style="" id="dvForm" runat="server">
            <table class="tableStyle">
                <tr class="trStyle">
                    <td onclick="turnOnOffOption1('trShapeFile');"><label id="lblShapeFile" runat="server"></label></td>
                </tr>
                <tr id="trShapeFile" style="display:none; font-size: 14px; width">
                    <td >
                        <textarea ID="FarmName" rows="1" cols="1" runat="server" style="display:none"></textarea>
                        <textarea ID="FarmXY" rows="1" cols="1"  runat="server" style="display:none"></textarea>
                        <textarea ID="FieldsName" rows="1" cols="1" runat="server" style="display:none"></textarea>
                        <textarea ID="FieldsArea" rows="1" cols="1"  runat="server" style="display:none"></textarea>
                        <textarea ID="FieldsXY" rows="1" cols="1" runat="server" style="display:none"></textarea>
                        <textarea ID="StateName" rows="1" cols="1" runat="server" style="display:none"></textarea>
                        <textarea ID="StateAbbr" rows="1" cols="1" runat="server" style="display:none"></textarea>
                        <textarea ID="CountyName" rows="1" cols="1" runat="server" style="display:none"></textarea>
                        <textarea runat="server" id="savedata" rows="20" cols="38" style="display:none"></textarea>
                        <textarea ID="TextAddress" rows="1" cols="1" runat="server" style="display:none"></textarea>
                        <textarea ID="Textlatlng" rows="1" cols="1" runat="server" style="display:none"></textarea>
                        <table class="tableStyle" id="tblUploadAOI">
                            <tr>
                                <td>
                                    <asp:Label ID="lblFarmShp" runat="server" Text="Choose farm shapefile" ></asp:Label>
                                </td>
                                <td>
                                    <asp:FileUpload ID="fileUploadFarmShapefile" runat="server"/>        
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblFieldShp" runat="server" Text="Choose field(s) shapefile"></asp:Label>
                                </td>
                                <td>
                                    <asp:FileUpload ID="fileUploadFieldShapefile" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="bntUpload" runat="server" Text="Upload Shapfile"/>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <label id="_lblNote1" runat="server" contenteditable="false" rows="2" cols="50" style="font-size:small; background-color: #EFEFEF; border-style: none; overflow: hidden; height: 50px;"></label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <label id="lblNote2" runat="server" contenteditable="false" rows="6" cols="50" style="font-size:small; background-color: #EFEFEF; border-style: none; overflow: hidden; height: 50px;"></label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <table class="tableStyle">
                <tr class="trStyle">
                    <td onclick="turnOnOffOption1('trNavigation');"><label id="lblNavigation" runat="server"></label></td>
                </tr>
                <tr id="trNavigation" style="display:none" runat="server">
                    <td>
                        <table class="tableStyle">
                            <tr>
                                <td ><label id="lblNoteNavigation" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    <input id="txtAddress" type="text" runat="server" style="background-color: transparent; border-style: none"/> 
                                    <input type="text" name="textAddress" value="" id="address" /><br />
                                </td>
                                <td>     
                                    <asp:ImageButton runat="server" ID="imgAddress" ImageUrl="~/Resources/search.png"  Text="View" OnClientClick="return codeAddress()" />
                                </td>
                            </tr>
                            <tr>
                                <td class="style2">
                                    <input id="lblLatLon" type="text" runat="server" style="background-color: transparent; border-style: none"/> 
                                    <input type="text" name="textlatlng" value="" id="latlng" /> <br />
                                </td>
                                <td>     
                                    <asp:ImageButton runat="server" ID="imgLatlng" ImageUrl="~/Resources/search.png"  Text="View" OnClientClick="return codeLatLng()" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input id="lblZoomState" type="hidden" runat="server" />
                                    <input id="lblZoomCounty" type="hidden" runat="server" />
                                    <select id="stateselect" name="stateselect" class="textfeld" onchange="findAddress();return false" onfocus="" runat="server">
                                        <option value="" id="lblZoomToState"></option>
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <select id="countyselect" name="countryselect" class="textfeld" onchange="findCounty();return false" onfocus="" ></select>             
                                    <select id="countyAll" name="countyAll" style="display:none" runat="server"></select>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>   
            
            <table class="tableStyle">
                <tr class="trStyle">
                    <td onclick="turnOnOffOption1('trTools');"><label id="lblTools" runat="server"></label></td>
                </tr>
                <tr  id="trTools" runat="server">
                    <td>
                        <table class="tableStyle" id="tblSelectAOI">
                            <tr>
                                <td colspan="2"><label id="lblToolsNote1" runat="server" /></td>
                            </tr>
                            <tr>
                                <td colspan="2"><label id="lblToolsNote2" runat="server" /></td>
                            </tr>
                            <tr>
                                <td colspan="2"><label id="lblToolsNote4" runat="server" /></td>
                            </tr>
                            <tr>
                                <td colspan="2"><label id="lblToolsNote3" runat="server" /></td>
                            </tr>
                            <tr>
                                <td style="width:auto"><label id="lblEditingOptions" runat="server" class="btnStyle" style="width:100%"/></td>
                            </tr>
                            <tr>
                                <td >
                                    <input type="radio" name="polyType" id="polyTypeFarm" value="To Draw Farm"/>
                                    <label id="lblFarm" runat="server"></label>
                                    <input type="radio" name="polyType" id="polyTypeField" value="To Drow Fields"/>
                                    <label id="lblField" runat="server"></label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input id="bntDelete" type="button" value="Remove" class="btnStyle"/>
                                    <input id="lblDelete" type="hidden" runat="server" />
                                    <button type="button" id="btnNew" class="btnStyle" onclick="return newest();" style="visibility: hidden"></button>
                                </td>
                                <td style="right: auto; text-align: right">
                                    <input type="submit" id="savebutton" runat="server" value="Submit" style="display:none; font-size:larger"  />
                                    <button type="button" id="bntInfo" style="display:none; font-size:larger">Submit</button>
                                    <asp:Button runat="server" ID="submit" Text="Submit" CssClass="btnStyle" OnClientClick="return submitSelection('Submit')" ToolTip="Click after AOI selected for farm and fields" />
                                </td>
                            </tr>
                            <tr style="border-top:1; border-top-color:Black; border-top-style: solid; border-top-width: thin;">
                                <td colspan="2" style="border-style: solid none none none; border-width: thin"><label id="lblNote5" runat="server"/></td>
                            </tr>
                            <tr>
                                <td>
                                    <label id="lblFieldName" runat="server">Field Name</label>
                                    <input id="txtFieldName" type="text" runat="server" />
                                </td>
                                <td>
                                    <asp:Button ID="btnCopyFarm" runat="server" Text="Copy Farm as Field" OnClientClick="return submitSelection('CopyFarm')" CssClass="btnStyle" ToolTip="Click after AOI selected for farm and fields" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
    
        </div>
        <div id='dialog-modal' style="display:none">
            <table>
                <tr>
                    <td>
                        <label id="lblCopyFarm" style="font-size:small" runat="server"></label>
                    </td>
                </tr>
                <tr>
                    <td>
<%--                        <asp:Button runat="server" ID="btnContinue" Text="Continue" OnClientClick="return submitSelection()" CssClass="btnStyle" ToolTip="Click after AOI selected for farm and fields" />
--%>                    </td>
                </tr>
            </table>
        </div>

    </form>
    </div>
    <div id="map" style=" height:100%; width: 73%" runat="server">
     <cc3:msgBox id="MsgBox1" runat="server" postback="true"> </cc3:msgBox>
    </div>

    <div id="dvWait" runat="server" 
       style="font-size: large; font-style: italic; color: #009900; text-decoration: blink; display:none; background-color: #FFFF00;">            Please Wait........
    </div>

    <script src="http://www.google-analytics.com/urchin.js" type="text/javascript"></script>
    <script type="text/javascript">
        _uacct = "UA-162157-1";
        urchinTracker();
        function Text2_onclick() {}
    </script>
    </body>
</html> 