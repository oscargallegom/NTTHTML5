<%@ Page Title="Weather" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Weather.aspx.vb" Inherits="NTTHTML5.Weather" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <script type="text/javascript" src="<%= ResolveUrl ("~/Scripts/weather.js") %>"></script>  
     <script type="text/javascript">
//        function setHiddenValue() {
//            document.getElementById("MainContent_Hidden1").value = document.getElementById("MainContent_Uploader").value;
//        }

//        function triggerFileUpload() {
//            document.getElementById("MainContent_Uploader").click();
//        }
//    </script>
    <div id="tdSave" class="save" style="color: #99CCFF; font-weight: bold; width:50; text-align: right;" >
        <asp:LinkButton ID="btnSave" runat="server" Text="Save" ></asp:LinkButton>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="btnContinue" runat="server" Text="Continue"  ></asp:LinkButton>
    </div>
   
    <table >
        <tr>
            <td>
                <fieldset class="section">                        
                    <legend id="lblWeatherTitle" runat="server" >Select Source for Weather Information</legend>
                    <input type="radio" runat="server" id="rbtnStation" name="rbtnWeather" value="Station" onchange="rbtnStation_onclick('Station');" onselect="rbtnStation_onclick('Station');" />
                    <label id="lblStation" runat="server" for="rbtnStation">Weather Information Using PRISM Data</label><br />
                    <input type="radio" runat="server" id="rbtnOwn"  name="rbtnWeather" value="Own" onchange="return rbtnStation_onclick('Own');" onselect="return rbtnStation_onclick('Own');"/>
                    <label id="lblOwn" runat="server" for="rbtnOwn">Load your Own Weather File</label><br />
                    <input type="radio" runat="server" id="rbtnCoordinates"  name="rbtnWeather" value="Coordinates" onchange="return rbtnStation_onclick('Coordinates');" onselect="return rbtnStation_onclick('Coordinates');"/>
                    <label id="lblCoordinates" runat="server" for="rbtnStation">Load Using Specific Coordinates (USA only)</label><br />
                </fieldset>
            </td>
            <td id="tdWeather" runat="server" style="vertical-align: middle; text-align:left" >
                <fieldset style="display:none" id="fsetStations" runat="server" class="section">
                    <legend id="lblStations" runat="server">Select Station from the Drop Down List</legend>
                    <select id="ddlStations" name="ddlStations" runat="server" style="text-align:left;"  ></select>
                </fieldset>
                <fieldset id="fsetUploadWeatherFile" runat="server" style="display:none"  class="section">
                    <legend id="lblUploadWeatherFile" runat="server">Choose the file to Upload and then select Upload Weather File</legend>
                    <input id="flUpload" runat="server" type="file" onchange="__doPostBack('btnUpload', 'Load Project')"/>
                </fieldset>
                <fieldset id="fsetCoordinates" runat="server" class="section">
                    <legend id="lblCoordinates1" runat="server">Enter Coordinates and then save them</legend>
                    <asp:Label ID="lblLong" runat="server" Text="Longitude" ></asp:Label>
                    <asp:TextBox ID="txtLong" runat="server" Width="75px" CssClass="align-right"></asp:TextBox>
                    <asp:Label ID="lblLat" runat="server" Text="Latitude" ></asp:Label>
                    <asp:TextBox ID="txtLat" runat="server" Width="75px" CssClass="align-right"></asp:TextBox>
                    <asp:Button ID="btnSaveCoordinates" runat="server" Text="Save Coordinates" />
                </fieldset>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="font-weight: bold" >
                <asp:Label ID="lblYearsOfInfo" runat="server" Text="Years of Weather Information: "></asp:Label>
                <asp:Label ID="lblWeatherYears" runat="server" Text="1981 - 2000"></asp:Label>
            </td>
        </tr>
    </table>  
    <fieldset runat="server" class="section">
        <legend></legend>
        <asp:Label ID="lblSimulationNote" runat="server" Text="The period to simulate is the same as the period selected in the weather information. You can cange the period to be simulated but the model will add five years to the befinning to warm up. 
        The las twleve years are shown in the graphs in the Results page. If the period is less than 12 years, all of the years are shown on the Results page."></asp:Label>
        <br>
        <div id="divWeatherTitle" style="text-align:center; font-weight:bold">
            <asp:Label ID="lblPeriodToSimulate" runat="server" Text="Period to simulate:"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:TextBox ID="txtPeriodFromSimulate" runat="server" CssClass="align-right"></asp:TextBox> &nbsp;
            <asp:Label ID="lblTo" runat="server" Text="to"></asp:Label>&nbsp;&nbsp; 
            <asp:TextBox ID="txtPeriodToSimulate" runat="server" CssClass="align-right"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblSimulationPeriod" runat="server" Text="Simulation period from 0 to 0."></asp:Label>
        </div>
    </fieldset>          
  </asp:Content>
 