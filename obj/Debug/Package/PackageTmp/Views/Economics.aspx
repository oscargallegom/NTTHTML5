<%@ Page Title="Economics" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Economics.aspx.vb" Inherits="NTTHTML5.Economics1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server" ></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" width="100%" >
    <%--     <script type="text/javascript" src="<%= ResolveUrl ("~/Scripts/economics.js") %>"></script>--%>
    <script type="text/javascript">
        var edit = "";

        function turnOnOff() {
            var turn = document.getElementById('chkSelecteds').checked;
            var ddlIndex = document.getElementById('MainContent_ddlkindOfInfo').selectedIndex;
            var display = ""
            if (turn == true) { display = "none"; }
            var gv; 
            switch (ddlIndex) { 
                case 1:
                    gv = document.getElementById('MainContent_gvFeeds');
                    break;
                case 2:
                    gv = document.getElementById('MainContent_gvEquipment1');
                    break;
                case 3:
                    gv = document.getElementById('MainContent_gvStructure');
                    break;
                case 4:
                    gv = document.getElementById('MainContent_gvOtherInputs');
                    break;
            }
            for (i = 1; i < gv.rows.length; i++) {
                if (gv.rows[i].cells[0].children[0].checked == false) { gv.rows[i].style.display = display; }
            }
        }

        function SaveChecked(chkBox, row) {
            if (chkBox.checked == true) { row.cells[0].children[0].value = "True"; }
            else { row.cells[0].children[0].value = "False"; }
        }

        function SetSaveOn() {
            document.getElementById("MainContent_hdnSave").value = document.getElementById("MainContent_ddlkindOfInfo").selectedIndex; 
        }

        function onChange(rowIndex) {
            if (document.getElementById("MainContent_hdnEdit").value == "Edit") {return false; }
        }

        function onEdit(rowIndex, activeRow) {
            if (activeRow.textContent.substring(0, 4) == "Edit") { document.getElementById("MainContent_hdnEdit").value = "Edit"; }
            if (activeRow.textContent.substring(0, 6) == "Update") { document.getElementById("MainContent_hdnEdit").value = "Update"; }
        }

    </script>

    <div class="save" >
        <asp:LinkButton ID="btnSave" runat="server" Text="Save" ></asp:LinkButton>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="btnContinue" runat="server" Text="Continue"  ></asp:LinkButton>
        <input type="hidden" id="hdnSave" runat="server" />
        <input type="hidden" id="hdnEdit" runat="server" />
     </div>
    <section id="sctEconomicInfo" class="section">
      <table id="tblSKI" runat="server">
        <tr id="trSKI">
            <td>
                <input runat="server" id="lblSKI" type="text" value="Select kind of information" class="label"/>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlkindOfInfo" AutoPostBack="true" onchange="javascript:onChange();" >
                <%--<asp:DropDownList runat="server" ID="ddlkindOfInfo" AutoPostBack="true">--%>
                    <asp:ListItem  Text="Select One" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Feeds" ></asp:ListItem>
                    <asp:ListItem Text="Equipment" ></asp:ListItem>
                    <asp:ListItem Text="Structure" ></asp:ListItem>
                    <asp:ListItem Text="Other Input" ></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <input runat="server" id="lblSOS" type="text" value="Show only selected from the list" class="label" />
             </td>
             <td>
                <input type="checkbox" id="chkSelecteds" onclick="turnOnOff()" />
             </td>
        </tr>
      </table>
      </section>    
    <table style="vertical-align:top">
        <tr>
            <td>
                <asp:GridView ID="gvFeeds" runat="server" AutoGenerateColumns="false" AutoGenerateEditButton="true">
                    <FooterStyle CssClass="gvFooterStyle" />
                    <RowStyle CssClass="gvRowStyle"/>
                    <SelectedRowStyle CssClass="gvSelectedRowStyle" />
                    <PagerStyle CssClass="gvPagerSyle" HorizontalAlign="Center" />
                    <HeaderStyle CssClass="gvHeaderStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select" >                    
                             <ItemTemplate>
                                <input type="checkbox" id="chkSelected" runat="server" checked='<%#Eval("selected")%>' />
                             </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="true" ></asp:BoundField>
                        <asp:BoundField DataField="sellingPrice" HeaderText="Selling Price"></asp:BoundField>
                        <asp:BoundField DataField="purchasePrice" HeaderText="Purchase Price"></asp:BoundField>
                        <asp:BoundField DataField="Concentrate" HeaderText="Concentrate"></asp:BoundField>
                        <asp:BoundField DataField="Forage" HeaderText="Forage"></asp:BoundField>
                        <asp:BoundField DataField="Grain" HeaderText="Grain"></asp:BoundField>
                        <asp:BoundField DataField="Hay" HeaderText="Hay"></asp:BoundField>
                        <asp:BoundField DataField="Pasture" HeaderText="Pasture"></asp:BoundField>
                        <asp:BoundField DataField="Silage" HeaderText="Silage"></asp:BoundField>
                        <asp:BoundField DataField="Supplement" HeaderText="Supplement"></asp:BoundField>
                    </Columns>
                </asp:GridView>

                <asp:GridView ID="gvEquipment1" runat="server" AutoGenerateColumns="false" AutoGenerateEditButton="true">
                    <FooterStyle CssClass="gvFooterStyle" />
                    <RowStyle CssClass="gvRowStyle"/>
                    <SelectedRowStyle CssClass="gvSelectedRowStyle" />
                    <PagerStyle CssClass="gvPagerSyle" HorizontalAlign="Center" />
                    <HeaderStyle CssClass="gvHeaderStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select" >                    
                             <ItemTemplate>
                                <input type="checkbox" id="chkSelected" runat="server" checked='<%#Eval("selected")%>' />
                             </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="true" ></asp:BoundField>
                        <asp:BoundField DataField="LeaseRate" HeaderText="Lease Rate"  ></asp:BoundField>
                        <asp:BoundField DataField="NewPrice" HeaderText="New Price" ></asp:BoundField>
                        <asp:BoundField DataField="NewHours" HeaderText="New Hours"  ></asp:BoundField>
                        <asp:BoundField DataField="CurrentPrice" HeaderText="Current Price"  ></asp:BoundField>
                        <asp:BoundField DataField="HoursRemaining" HeaderText="Hours Remaining"  ></asp:BoundField>
                        <asp:BoundField DataField="Width" HeaderText="Width"  ></asp:BoundField>
                        <asp:BoundField DataField="Speed" HeaderText="Speed"  ></asp:BoundField>
                        <asp:BoundField DataField="FieldEfficiency" HeaderText="Field Efficiency"  ></asp:BoundField>
                        <asp:BoundField DataField="HorsePower" HeaderText="Horse Power" ></asp:BoundField>
                        <asp:BoundField DataField="Rf1" HeaderText="Rf1" ></asp:BoundField>
                        <asp:BoundField DataField="Rf2" HeaderText="Rf2"  ></asp:BoundField>
                        <asp:BoundField DataField="IrLoan" HeaderText="Ir Loan" ></asp:BoundField>
                        <asp:BoundField DataField="LLoan" HeaderText="L Loan" ></asp:BoundField>
                        <asp:BoundField DataField="IrEquity" HeaderText="Ir Equity" ></asp:BoundField>
                        <asp:BoundField DataField="PDebt" HeaderText="P Debt" ></asp:BoundField>
                        <asp:BoundField DataField="Year" HeaderText="Year" ></asp:BoundField>
                        <asp:BoundField DataField="Rv1" HeaderText="Rv1"  ></asp:BoundField>
                        <asp:BoundField DataField="Rv2" HeaderText="Rv2" ></asp:BoundField>
                    </Columns>
                </asp:GridView>

                <asp:GridView ID="gvStructure" runat="server" AutoGenerateColumns="false" AutoGenerateEditButton="true">
                    <FooterStyle CssClass="gvFooterStyle" />
                    <RowStyle CssClass="gvRowStyle"/>
                    <SelectedRowStyle CssClass="gvSelectedRowStyle" />
                    <PagerStyle CssClass="gvPagerSyle" HorizontalAlign="Center" />
                    <HeaderStyle CssClass="gvHeaderStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select" >                    
                             <ItemTemplate>
                                <input type="checkbox" id="chkSelected" runat="server" checked='<%#Eval("selected")%>'/>
                             </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="true" ></asp:BoundField>
                        <asp:BoundField DataField="LeaseRate" HeaderText="Lease Rate" ></asp:BoundField>
                        <asp:BoundField DataField="NewPrice" HeaderText="New Price" ></asp:BoundField>
                        <asp:BoundField DataField="NewLife" HeaderText="New Life" ></asp:BoundField>
                        <asp:BoundField DataField="CurrentPrice" HeaderText="Current Price" ></asp:BoundField>
                        <asp:BoundField DataField="LifeRemaining" HeaderText="Life Remaining" ></asp:BoundField>
                        <asp:BoundField DataField="MaintenanceCoefficient" HeaderText="Maintenance Coefficient" ></asp:BoundField>
                        <asp:BoundField DataField="LoanInterestRate" HeaderText="Loan Interest Rate" ></asp:BoundField>
                        <asp:BoundField DataField="LengthLoan" HeaderText="Length Loan" ></asp:BoundField>
                        <asp:BoundField DataField="InterestRateEquity" HeaderText="Interest Rate Equity" ></asp:BoundField>
                        <asp:BoundField DataField="ProportionDebt" HeaderText="Proportion Debt" ></asp:BoundField>
                        <asp:BoundField DataField="Year" HeaderText="Year" ></asp:BoundField>
                    </Columns>
                </asp:GridView>

                <asp:GridView ID="gvOtherInputs" runat="server" AutoGenerateColumns="false" AutoGenerateEditButton="true">
                    <FooterStyle CssClass="gvFooterStyle" />
                    <RowStyle CssClass="gvRowStyle"/>
                    <SelectedRowStyle CssClass="gvSelectedRowStyle" />
                    <PagerStyle CssClass="gvPagerSyle" HorizontalAlign="Center" />
                    <HeaderStyle CssClass="gvHeaderStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select" >                    
                             <ItemTemplate>
                                <input type="checkbox" id="chkSelected" runat="server" checked='<%#Eval("selected")%>'/>
                             </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="true" ></asp:BoundField>
                        <asp:BoundField DataField="Values" HeaderText="Values"  ></asp:BoundField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
