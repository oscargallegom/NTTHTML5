<%@ Page Title="Location" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Location.aspx.vb" Inherits="NTTHTML5.Location" %>

<%@ Register TagPrefix="cc3" Namespace="BunnyBear" Assembly="msgBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" >
     <script type="text/javascript" src="<%= ResolveUrl ("~/Scripts/location1.js") %>"></script>  
    <div class="save">
        <asp:LinkButton ID="btnSaveContinue" runat="server" Text="Save & Continue"></asp:LinkButton>
    </div>
    <table>
        <tr>
            <td >
                <asp:Button ID="btnMapSystem" runat="server" Text="Mapping System" onClick="btnMapSystem_Click"/>
                <asp:Button ID="btnStateCounty" runat="server" Text="State / County Select" OnClick="btnStateCounty_Click"/>
                <asp:Button ID="btnUserInput" runat="server" Text="User Input" OnClick="btnUserInput_Click"/>
            </td>
        </tr>
    </table>

    <section id="sctMap" runat="server" style="display:none" >
        <table><tr><td id="tdGoogleMap" runat="server"></td></tr></table>
    </section>
    <fieldset id="sctStateCounty" runat="server" style="display:none">
        <legend id="lgdStateCounty" runat="server" >Select State and County</legend>
        <table >
            <tr>
                <td>
                     <asp:Label ID="lblState" Text="Select State: " runat="server"></asp:Label>                    
                     <select id="ddlStates" name="ddlStates" class="textfeld" onchange="selectCounties();return false" onfocus="" runat="server"></select>
                     <asp:Label ID="lblCounties" Text="Select a County: " runat="server"></asp:Label>
                     <select id="ddlCounties" name="ddlCounties" class="textfeld" runat="server" onchange="return countySelected(this);"></select>             
                     <select id="ddlAllCounties" name="ddlAllCounties" style="display:none" runat="server"></select>
                     <input type="hidden" runat="server" id="txtCounty" />
               </td>
            </tr>
        </table>
    </fieldset>
    <fieldset id="sctInput" runat="server" style="display:none">
        <legend id="lgdUserInput" runat="server" >Enter descriptive location</legend>
        <table >
            <tr>
                <td>
                    <input type="text" id="lblGeneralLocation" value="General Location" runat="server" class="label" readonly="true">
                    <input type="text" id="lblSpecificLocation" value="Specific Location" runat="server" class="label" readonly="true">
                </td>
            </tr>
            <tr>
                <td>
                    <input type="text" name="txtGeneralLocation" runat="server">
                    <input type="text" name="txtspecificLocation" runat="server">
               </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
