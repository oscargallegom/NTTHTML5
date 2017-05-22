function rbtnStation_onclick(way) {
    document.getElementById("MainContent_fsetStations").style.display = "none";
    document.getElementById("MainContent_fsetUploadWeatherFile").style.display = "none";
    document.getElementById("MainContent_fsetCoordinates").style.display = "none";
    document.getElementById("MainContent_tdWeather").style.verticalAlign = "middle";
    var indexOf = document.getElementById("MainContent_lblStation").innerHTML.indexOf("PRISM");
    switch (way) {
        case "Station":
            if (indexOf != -1) 
                //{document.getElementById("MainContent_fsetStations").style.display = "";}
            break;
        case "Prism":
            break;
        case "Own":
            document.getElementById("MainContent_fsetUploadWeatherFile").style.display = "";
            break;
        case "Coordinates":
            document.getElementById("MainContent_fsetCoordinates").style.display = "";
            break;
    }
}
