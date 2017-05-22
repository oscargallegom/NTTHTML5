<%@ Page Title="Results" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Results.aspx.vb" Inherits="NTTHTML5.Results" %>
<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <script type="text/javascript" src="<%= ResolveUrl ("~/Scripts/results.js") %>"></script>--%>
    <script type="text/javascript">
//        function turnOnOfOptions() {
//            var ddlType = document.getElementById("MainContent_ddlType");
//            var divField = document.getElementById("MainContent_sctFields")
//            var divSubproject = document.getElementById("MainContent_sctSubprojects")

//            if (ddlType.selectedIndex == 0) { divField.style.display = ""; divSubproject.style.display = "none"; }
//            else { divField.style.display = "none"; divSubproject.style.display = ""; }
//        }


        function turnOn(row) {
            var rowIndex = row.parentElement.parentElement.rowIndex;
            var gridView = document.getElementById("MainContent_gvResults");
            var rowType = gridView.rows[rowIndex].cells[1].textContent;
            var display = "none";
            var j = 1;
            if (rowIndex + 1 < gridView.rows.length) { if (gridView.rows[rowIndex + 1].style.display == "none") { display = ""; } }
            for (i = rowIndex + 1; i < gridView.rows.length; i++) {
                if (gridView.rows[i].cells[7].textContent == "Total") { break; }
                else {
                    gridView.rows[rowIndex + j].style.display = display;
                    gridView.rows[rowIndex + j].cells[0].children[0].style.display = "none";
                    j++;
                }
            }
        }
    </script>
    <section class="section">
        <table>
            <tr>
                <td>
                    <label id="lblType" runat="server" class="label" ></label>
                    <select id="ddlType" runat="server" onchange="__doPostBack();">
                        <option value="Field/Scenario" selected="selected"></option>
                        <option value="Subproject" ></option>
                    </select>
                </td>
                <td>
                    <section id="sctFields" runat="server">
                        <label id="lblField" runat="server" class="label"></label>
                        <select id="ddlField" runat="server" onchange="__doPostBack();" ></select>
                        <label id="lblScenario1" runat="server" class="label"></label>
                        <select id="ddlScenarios1" runat="server" onchange="__doPostBack();" ></select>
                        <label id="lblScenario2" runat="server" class="label"></label>
                        <select id="ddlScenarios2" runat="server" onchange="__doPostBack();" ></select>
                    </section>
                    <section id="sctSubprojects" runat="server">
                        <label id="lblSubproject1" runat="server" class="label"></label>
                        <asp:DropDownList id="ddlSubproject1" runat="server"></asp:DropDownList>
                        <label id="lblSubproject2" runat="server" class="label"></label>
                        <asp:DropDownList id="ddlSubproject2" runat="server"></asp:DropDownList>
                    </section>
                </td>
            </tr>
            <tr class="align-right" style="width:100%" >
                <td class="align-right" colspan="2">
                    <asp:Button ID="btnSummary" runat="server" autopostback="true" Text="Summary" style="margin:0,15,0,15" />
                    <asp:Button ID="btnBySoil" runat="server" Text="By Soil" style="margin:0,15,0,15"/>
                    <asp:Button ID="btnGraphs" runat="server" Text="Graphs" style="margin:0,15,0,15"/>
                    <asp:Button ID="btnEconomics" runat="server" Text="Economics" style="margin:0,15,0,15"/>
                    <asp:Button ID="btnCreatePDF" runat="server" Text="Create PDF" style="margin:0,15,0,15"/>
                    <asp:Button ID="DownloadDnDc" runat="server" Text="Download DnDc Files" style="margin:0,15,0,15"/>
                </td>
            </tr>
        </table>
    </section>
    <fieldset id="fsetSoils" runat="server">
        <legend id="lblSoils" runat="server" class="label" >Select Soil</legend>
        <asp:DropDownList id="ddlSoils" runat="server" AutoPostBack="True"></asp:DropDownList>
    </fieldset>
    <section id="sctSummary" runat="server" class="section">
        <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="false" >
            <FooterStyle CssClass="gvFooterStyle" />
            <RowStyle CssClass="gvRowStyle" />
            <HeaderStyle CssClass="gvHeaderStyle" />
            <Columns>
                <asp:TemplateField HeaderText="Detail" >
                    <ItemTemplate>
                        <input type="checkbox" id="chkDetail" name="chkDetail" onclick="turnOn(this);" runat="server"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField  DataField="Description" HeaderText="Description" ReadOnly="true" ></asp:BoundField>
                <asp:BoundField  DataField="Scenario1" ReadOnly="true" ItemStyle-HorizontalAlign="Right" ></asp:BoundField>
                <asp:BoundField  DataField="Scenario2" ReadOnly="true" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                <asp:BoundField  DataField="Difference" HeaderText="Difference" ReadOnly="true" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                <asp:BoundField  DataField="Reduction" HeaderText="Reduction (%)" ReadOnly="true" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                <asp:BoundField  DataField="TotalArea" HeaderText="Total Area" ReadOnly="true" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                <asp:BoundField  DataField="Type" HeaderText="" ReadOnly="true" ></asp:BoundField>
            </Columns>
        </asp:GridView>
    </section>

    <section id="sctGraphs" runat="server" class="section" style="display:none">
        <fieldset id="fsetGraphs">
            <legend id="lblGraphs" runat="server">Graphs</legend>
            <select id="ddlAverage" runat="server" onchange="return __doPostBack('ddlInfo', 'TEST');">
                <option value="Monthly Average (The entire period)" />
                <option value="Annual (Last 12 years)" />
            </select>
            <select id="ddlInfo" runat="server" onchange="return __doPostBack('ddlInfo', 'TEST');" ></select>
            <select id="ddlCrop" runat="server" style="display:none" onchange="return __doPostBack('ddlInfo', 'TEST');"></select>
        </fieldset>

        <fieldset id="fsetChart" runat="server">
            <legend id="lblChart" runat="server">Chart</legend>

            <asp:Chart ID="Chart1" runat="server" Width="771px">
                <Series>
                    <asp:Series Name="Series1" Legend="Legend1" XValueType="String" YValueType="Single">
                        <Points>
                            <asp:DataPoint XValue="1" YValues="10" />
                            <asp:DataPoint XValue="2" YValues="20" />
                            <asp:DataPoint XValue="3" YValues="30" />
                        </Points>
                    </asp:Series>
                    <asp:Series ChartArea="ChartArea1" Name="Series2" Legend="Legend1" XValueType="String" YValueType="Single">
                        <Points>
                            <asp:DataPoint XValue="1" YValues="50" />
                            <asp:DataPoint XValue="2" YValues="60" />
                            <asp:DataPoint XValue="3" YValues="70" />
                        </Points>
                    </asp:Series>
                </Series>
                <ChartAreas><asp:ChartArea Name="ChartArea1"></asp:ChartArea></ChartAreas>
                <Titles><asp:Title Name="Title1" Text="Flow"></asp:Title></Titles>
                <Legends><asp:Legend></asp:Legend></Legends>
            </asp:Chart>
        </fieldSet>
    </section>

</asp:Content>
