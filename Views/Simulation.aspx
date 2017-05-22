<%@ Page Title="Simulation" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Simulation.aspx.vb" Inherits="NTTHTML5.Simulation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <script type="text/javascript" src="<%= ResolveUrl ("~/Scripts/simulation.js") %>"></script>  
     <script type="text/javascript">
         function SelectAllScenarios() {
             var checked = true;
             var on = "on"
             if (!document.getElementById("chkSelectAll").checked) { checked = false; on = "off" }
             var oprs = document.getElementById("MainContent_gvSimulations");
             for (i = 1; i < oprs.rows.length; i++) {
                 oprs.rows[i].cells[0].children[0].checked = checked;
                 oprs.rows[i].cells[0].children[0].value = checked;
             }
         }

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
             document.getElementById("MainContent_fsetListSimulations").style.display = "none";
             __doPostBack("btnRunEnvironment", "RunEnvironment")
         }
         
         function turnOnOffControls() {
             var ddlType = document.getElementById("MainContent_ddlType");
             var divField = document.getElementById("MainContent_divScenario")
             var divSubproject = document.getElementById("MainContent_divSubproject")

             if (ddlType.selectedIndex == 0) { divField.style.display = ""; divSubproject.style.display = "none"; document.getElementById("fsetAll").style.display="" }
             else { divField.style.display = "none"; divSubproject.style.display = ""; document.getElementById("fsetAll").style.display = "none" }
             __doPostBack("ddlType", "ChangeSimulationType");

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
                    <select id="ddlType" runat="server" onchange="turnOnOffControls();" ></select>
                </fieldset>
            </td>
            <td>
                <fieldset class="section">
                    <legend id="lblScenarios" runat="server" style="visibility:hidden">Select Specific Scenario or Subproject</legend>
                    <div id="divScenario" runat="server">
                        <label id="lblField" class="label" runat="server">Select Field</label>
                        <asp:DropDownList id="ddlField" runat="server" AutoPostBack="true"></asp:DropDownList>
                        <label id="lblScenario" class="label" runat="server" style="visibility:hidden">Select Scenario</label>
                        <asp:DropDownList id="ddlScenario" runat="server"  style="visibility:hidden"></asp:DropDownList>
                        <input type="button" id="btnAddSimulations" runat="server" value="Add Field/Scenario to Run" style="visibility:hidden"/>
                    </div>
                    <div id="divSubproject" style="display:none" runat="server">
                        <label id="lblSubproject" class="label" runat="server">Select Subproject</label>
                        <asp:DropDownList id="ddlSubproject" runat="server"></asp:DropDownList>
                        <asp:Button id="btnAddSubproject" runat="server" text="Add Subproject to Run"/>
                    </div>
                </fieldset>
            </td>
            <td>
                <fieldset class="section" id="fsetAll" style="visibility:hidden">
                    <legend id="lblAll" runat="server">Add or Remove All</legend>
                    <input id="btnAddAll" runat="server" value="Add All" type="button" />
                    <input id="btnRemoveAll" runat="server" value="Remove All" type="button" />                    
                </fieldset>
            </td>
         </tr>
     </table>

    <fieldset id="fsetSimulation" runat="server" class="section" style="display:none;">
        <legend id="lblSimuation" runat="server">Simulation</legend>
        <input type="text" runat="server" id="txtSimulation" value="Plase Wait... Simulation is in process" readonly="true" style="width:100%; border:0; color:#009900; background-color: #FFFF00" />
    </fieldset>

    <fieldset class="section" id="fsetListSimulations" runat="server">
        <legend id="lblRunSimulation" runat="server">List of Scenarios to Simulate</legend>
        <input id="btnRunEnvironment" runat="server" value="Run Environment" type="button" onclick="WaitSimulation();" />
        <input id="btnRunEconomic" runat="server" value="Run Economics" type="button" />                    
        <%--<input id="btnRunEconomic" runat="server" value="Run Economics" type="button" disabled="disabled" />--%>                    

        <asp:GridView ID="gvSimulations" runat="server" AutoGenerateColumns="false">
            <FooterStyle CssClass="gvFooterStyle" />
            <RowStyle CssClass="gvRowStyle" />
            <HeaderStyle CssClass="gvHeaderStyle" />
            <Columns>
                <%--<asp:CommandField HeaderText="Delete" ShowDeleteButton="true" ShowHeader="true"  />--%>
                <asp:TemplateField HeaderText="Select" ItemStyle-Width="30px"> 
                    <HeaderTemplate>Select&#10;
                        <input id="chkSelectAll" type="checkbox" value="Select" onclick="SelectAllScenarios()" />
                    </HeaderTemplate>                   
                    <ItemTemplate >                                                   
                        <input id="chkSelected" type="checkbox" runat="server"/>
                    </ItemTemplate>
                </asp:TemplateField>
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
                <asp:BoundField  DataField="Fieldindex" ReadOnly="true"  ></asp:BoundField>
                <asp:BoundField  DataField="ScenarioIndex" ReadOnly="true" ></asp:BoundField>
            </Columns>
        </asp:GridView>
    </fieldset>
</asp:Content>
