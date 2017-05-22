<%@ Page Title="Economics" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Economics.aspx.vb" Inherits="NTTHTML5.Economics1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" width="100%">
    <%--     <script type="text/javascript" src="<%= ResolveUrl ("~/Scripts/economics.js") %>"></script>--%>
    <script type="text/javascript">
        function turnOnOff() {
            var turn = document.getElementById('chkSelecteds').checked;
            var display = ""
            if (turn == true) { display = "none"; }
            var gv = document.getElementById('MainContent_gvEconomics')
            for (i = 1; i < gv.rows.length; i++) {
                if (gv.rows[i].cells[0].children[0].checked == false) { gv.rows[i].style.display = display; }
            }
        }
    </script>

    <div class="save" >
        <asp:LinkButton ID="btnSaveContinue" runat="server" Text="Save & Continue"></asp:LinkButton>
     </div>
    <section id="sctEconomicInfo" class="section">
      <table id="tblSKI" runat="server">
        <tr id="trSKI">
            <td>
                <input runat="server" id="lblSKI" type="text" value="Select kind of information" class="label"/>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlkindOfInfo" AutoPostBack="true" >
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
                <%--<asp:CheckBox ID="chkSelecteds" runat="server"></asp:CheckBox>--%>
             </td>
        </tr>
      </table>
      </section>    
    <table style="vertical-align:top">
        <tr>
            <td>
                <asp:GridView ID="gvEconomics" runat="server" PageSize="50" AutoGenerateColumns="false"  >
                    <FooterStyle CssClass="gvFooterStyle" />
                    <RowStyle CssClass="gvRowStyle"/>
                    <SelectedRowStyle CssClass="gvSelectedRowStyle" />
                    <PagerStyle CssClass="gvPagerSyle" HorizontalAlign="Center" />
                    <HeaderStyle CssClass="gvHeaderStyle" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select" >                    
                             <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkSelected" Checked='<%#Eval("selected") %>' />
                             </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
            <%--<td>
                <div id="divFeed">
                    <table>
                        <tr>
                           <td>
                            <asp:Label ID="lblSellingPrice" Text="Selling Price: " runat="server"></asp:Label>
                            <asp:TextBox ID="txtSellingPrice" runat="server"></asp:TextBox>
                           </td>
                        </tr>
                        <tr>
                           <td>
                            <asp:Label ID="lblPurchasePrice" Text="Purchase Price: " runat="server"></asp:Label>
                            <asp:TextBox ID="txtPurchasePrice" runat="server"></asp:TextBox>
                           </td>
                        </tr>
                        <tr>
                           <td>
                            <asp:Label ID="lblConcentrate" Text="Concentrate: " runat="server"></asp:Label>
                            <asp:TextBox ID="txtConcentrate" runat="server"></asp:TextBox>
                           </td>
                        </tr>
                        <tr>
                           <td>
                            <asp:Label ID="lblForage" Text="Forage: " runat="server"></asp:Label>
                            <asp:TextBox ID="txtForage" runat="server"></asp:TextBox>
                           </td>
                        </tr>
                        <tr>
                           <td>
                            <asp:Label ID="lblGrain" Text="Grain: " runat="server"></asp:Label>
                            <asp:TextBox ID="txtGrain" runat="server"></asp:TextBox>
                           </td>
                        </tr>
                        <tr>
                           <td>
                            <asp:Label ID="lblHay" Text="Hay: " runat="server"></asp:Label>
                            <asp:TextBox ID="txtHay" runat="server"></asp:TextBox>
                           </td>
                        </tr>
                        <tr>
                           <td>
                            <asp:Label ID="lblPasture" Text="Pasture: " runat="server"></asp:Label>
                            <asp:TextBox ID="txtPasture" runat="server"></asp:TextBox>
                           </td>
                        </tr>
                        <tr>
                           <td>
                            <asp:Label ID="lblSilage" Text="Silage: " runat="server"></asp:Label>
                            <asp:TextBox ID="txtSilage" runat="server"></asp:TextBox>
                           </td>
                        </tr>
                        <tr>
                           <td>
                            <asp:Label ID="lblSupplement" Text="Supplement: " runat="server"></asp:Label>
                            <asp:TextBox ID="txtSupplement" runat="server"></asp:TextBox>
                           </td>
                        </tr>
                    </table>        
                </div>
        
                <div id="divEquipment">
                  <table>
                    <tr>
                      <td>
                        <asp:Label ID="lblLeaseRate" Text="Lease Rate: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtLeaseRate" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblNewPrice" Text="New Price: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtNewPrice" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblNewHours" Text="New Hours: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtNewHours" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblCurrentPrice" Text="Current Price: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtCurrentPrice" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblHoursRemaining" Text="Hours Remaining: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtHoursRemaining" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblWidth" Text="Width: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtWidth" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblSpeed" Text="Speed: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtSpeed" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblFieldEfficiency" Text="Field Efficiency: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtFieldEfficiency" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblHoursePower" Text="Hourse Power: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtHoursePower" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblRF1" Text="RF1: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtRF1" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblRF2" Text="RF2: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtRF2" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblIrloan" Text="Irloan: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtIrloan" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblLloan" Text="Lloan: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtLloan" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblIrequity" Text="Irequity: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtIrequity" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblPdebt" Text="Pdebt: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtPdebt" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblYear" Text="Year: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtYear" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblRV1" Text="RV1: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtRV1" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblRV2" Text="RV2: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtRV2" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                  </table>  
                </div>

                <div id="divStructure">
                    <table>
                     <tr>
                      <td>
                        <asp:Label ID="lblStrucLeaseRate" Text="Lease Rate: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtStrucLeaseRate" runat="server"></asp:TextBox>
                      </td>
                     </tr>
                     <tr>
                      <td>
                        <asp:Label ID="lblStrucNewPrice" Text="New Price: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtStrucNewPrice" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblNewLife" Text="New Life:" runat="server"></asp:Label>
                        <asp:TextBox ID="txtNewLife" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblStrucCurrentPrice" Text="Current Price: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtStrucCurrentPrice" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblLifeRemaining" Text="Life Remaining: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtLifeRemaining" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblMaintenanceCoefficient" Text="Maintenance Coefficient: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtMaintenanceCoefficient" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblLoanInterestRate" Text="Loan Interest Rate: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtLoanInterestRate" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblLengthLoan" Text="Length of Loan: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtLengthLoan" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblInterestRate" Text="Interest Rate on Equity: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtInterestRate" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblPrportionDebt" Text="Proportion of Debt: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtPrportionDebt" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td>
                        <asp:Label ID="lblStrucYear" Text="Year: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtStrucYear" runat="server"></asp:TextBox>
                      </td>
                    </tr>
                    </table>
                </div>

                <div id="divOtherInputs">
                    <table>
                     <tr>
                      <td>
                        <asp:Label ID="lblValue" Text="Value: " runat="server"></asp:Label>
                        <asp:TextBox ID="txtValue" runat="server"></asp:TextBox>
                      </td>
                     </tr>
                    </table>
                </div>
            </td>--%>
        </tr>
    </table>
</asp:Content>
