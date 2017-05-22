<%@ Page Title="Simulation" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Simulation.aspx.vb" Inherits="NTTHTML5.Simulation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <script type="text/javascript" src="<%= ResolveUrl ("~/Scripts/simulation.js") %>"></script>  
     <script type="text/javascript">
         function DeleteRow(element) {
             var r = confirm("Do you want to delete this Field?");
             if (r == true) {
                 return true;
             } else {
                 return false;
             }
         }

         function WaitSimulation() {
             document.getElementById("MainContent_fsetSimulation").style.display = "";
//             document.getElementById("MainContent_sctListOfSimulation").style.display = "none";
         }
         
         function turnOnOffControls() {
             var ddlType = document.getElementById("MainContent_ddlType");
             var divField = document.getElementById("divScenario")
             var divSubproject = document.getElementById("divSubproject")

             if (ddlType.selectedIndex == 0) { divField.style.display = ""; divSubproject.style.display = "none"; document.getElementById("fsetAll").style.display="" }
             else { divField.style.display = "none"; divSubproject.style.display = ""; document.getElementById("fsetAll").style.display = "none" }
         }</script>
     <div class="save" >
        <asp:LinkButton ID="btnSaveContinue" runat="server" Text="Save & Continue" OnClick="btnSaveContinue_Click"></asp:LinkButton>
     </div>
     <table>
        <tr>
            <td>
                <fieldset class="section">
                    <legend id="lblTypes" runat="server">Select Type to Simulate (Field/Scenario or Subproject)</legend>
                    <label id="lblType" class="label" runat="server">Select Type</label>
                    <select id="ddlType" runat="server" onchange="turnOnOffControls();"></select>
                </fieldset>
            </td>
            <td>
                <fieldset class="section">
                    <legend id="lblScenarios" runat="server">Select Specific Scenario or Subproject</legend>
                    <div id="divScenario">
                        <label id="lblField" class="label" runat="server">Select Field</label>
                        <asp:DropDownList id="ddlField" runat="server" AutoPostBack="true"></asp:DropDownList>
                        <label id="lblScenario" class="label" runat="server">Select Scenario</label>
                        <asp:DropDownList id="ddlScenario" runat="server"></asp:DropDownList>
                        <asp:Button id="btnAddSimulations" runat="server" text="Add Field/Scenario to Run"/>
                    </div>
                    <div id="divSubproject" style="display:none">
                        <label id="lblSubproject" class="label" runat="server">Select Subproject</label>
                        <asp:DropDownList id="ddlSubproject" runat="server"></asp:DropDownList>
                        <asp:Button id="btnAddSubproject" runat="server" text="Add Subproject to Run"/>
                    </div>
                </fieldset>
            </td>
            <td>
                <fieldset class="section" id="fsetAll">
                    <legend id="lblAll" runat="server">Add or Remove All</legend>
                    <input id="btnAddAll" runat="server" value="Add All" type="button" />
                    <input id="btnRemoveAll" runat="server" value="Remove All" type="button" />                    
                </fieldset>
            </td>
         </tr>
     </table>

    <fieldset id="fsetSimulation" runat="server" class="section" style="display:none;">
        <legend id="lblSimuation" runat="server">Simulation</legend>
        <input type="text" runat="server" id="txtSimulation" value="Plase Wait... Simulation is in process" readonly="true" />
    </fieldset>

    <fieldset class="section" >
            <legend id="lblRunSimulation" runat="server">Select type of simulation</legend>
            <input id="btnRunEnvironment" runat="server" value="Run Environment" type="button" onclick="WaitSimulation();" />
            <input id="btnRunEconomic" runat="server" value="Run Economics" type="button" />                    

        <asp:GridView ID="gvSimulations" runat="server" AutoGenerateColumns="false" OnRowDeleting="gvSimulation_RowDeleting" >
            <FooterStyle CssClass="gvFooterStyle" />
            <RowStyle CssClass="gvRowStyle" />
            <HeaderStyle CssClass="gvHeaderStyle" />
            <Columns>
                <asp:CommandField HeaderText="Delete" ShowDeleteButton="true" ShowHeader="true"  />
                <asp:BoundField  DataField="Field" HeaderText="Field" ReadOnly="true" ></asp:BoundField>
                <asp:BoundField  DataField="Scenario" HeaderText="Scenario" ReadOnly="true" ></asp:BoundField>
                <asp:TemplateField HeaderText="Project" >
                    <ItemTemplate>
                        <asp:Image ID="imgProject" ImageUrl='<%#Eval("project")%>' runat="server" > </asp:Image>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Location" >
                    <ItemTemplate>
                        <asp:Image ID="imgLocation" ImageUrl='<%#Eval("location")%>' runat="server" > </asp:Image>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Weather" >
                    <ItemTemplate>
                        <asp:Image ID="imgWeather" ImageUrl='<%#Eval("weather")%>' runat="server" > </asp:Image>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fields" >
                    <ItemTemplate>
                        <asp:Image ID="imgFields" ImageUrl='<%#Eval("fields")%>' runat="server" > </asp:Image>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Soils" >
                    <ItemTemplate>
                        <asp:Image ID="imgSoils" ImageUrl='<%#Eval("soils")%>' runat="server" > </asp:Image>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Management" >
                    <ItemTemplate>
                        <asp:Image ID="imgManagement" ImageUrl='<%#Eval("management")%>' runat="server" > </asp:Image>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField  DataField="Date1" HeaderText="Last Simulation" ReadOnly="true" ></asp:BoundField>
                <asp:BoundField  DataField="Message" HeaderText="Comments" ReadOnly="true" ></asp:BoundField>
            </Columns>
        </asp:GridView>
    </fieldset>
</asp:Content>
