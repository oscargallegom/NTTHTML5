<%@ Page Title="Soil" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Soil.aspx.vb" Inherits="NTTHTML5.Soil" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" width="100%">
     <script type="text/javascript" src="../Scripts/soil.js"></script> 
     <script type="text/javascript">
         function DeleteRow(element) {
             var r = confirm("Do you want to delete this Field?");
             if (r == true) { return true; }
             else { return false; }
         }

         function showModal() {
             window.location = "#LayersModal";
         }
     </script>
     <div class="save" >
        <asp:LinkButton ID="btnSaveContinue" runat="server" Text="Save & Continue" ></asp:LinkButton>
     </div>
    <fieldset class="section">
        <table id="tblSSA" runat="server">
            <tr id="trSSA">
                <td >
                    <input runat="server" id="lblSSA" name="lblSSA" type="text" value="Soil Survey Area" class="label" />                    
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlSSA" AutoPostBack="true"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td >
                    <input class="label" runat="server" id="lblSoils" name="lblSoils" type="text" value="Soils" />
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlSoils"></asp:DropDownList>
                </td>
                <td style="text-align:left">
                    <asp:Button runat="server" ID="btnAddSoil" Text="Add Soil Selected" />
                </td>
            </tr>
            <tr id="trField">
                <td >
                    <input class="label" runat="server" id="lblFields" name="lblFields" type="text" value="Select Field" />
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlFields" AutoPostBack="true"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="3" >
                    <asp:Label runat="server" ID="lblNote" Text="Note" ></asp:Label>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset id="sctSoils" class="section">
        <legend id="lblSoilsInfo" runat="server">Soils Information</legend>
        <asp:GridView ID="gvSoils" runat="server" PageSize="50" AutoGenerateColumns="false"               
            OnRowDeleting="gvSoils_RowDeleting" OnSelectedIndexChanging="gvSoils_SelectedIndexChanging">
            <FooterStyle CssClass="gvFooterStyle" />
            <RowStyle CssClass="gvRowStyle"/>
            <%--<SelectedRowStyle CssClass="gvSelectedRowStyle" />--%>
            <PagerStyle CssClass="gvPagerSyle" HorizontalAlign="Center" />
            <HeaderStyle CssClass="gvHeaderStyle" />
            <AlternatingRowStyle CssClass="gvAltRowStyle" />
            <Columns>
                <asp:CommandField HeaderText="Delete" ShowDeleteButton="true" ShowHeader="true" DeleteText="Delete"/>
                <asp:CommandField HeaderText="Layers" ShowSelectButton="true" ShowHeader="true" SelectText="Layers"/>              
                <asp:TemplateField HeaderText="Select" >                    
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkSelected" Checked='<%#Eval("selected") %>'/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField  DataField="Key" HeaderText="Key" ReadOnly="true" ></asp:BoundField>
                <asp:BoundField DataField="Symbol" HeaderText="Symbol" ReadOnly="true"></asp:BoundField>
                <asp:BoundField DataField="Group" HeaderText="Group" ReadOnly="true"></asp:BoundField>
                <asp:BoundField DataField="Name"  ReadOnly="true" HeaderText="Name" ></asp:BoundField>
                <asp:TemplateField HeaderText="Albedo" >
                    <ItemTemplate>
                        <asp:TextBox ID="txtAlbedo" CssClass="gvTextBox" runat="server" Text='<%#Eval("albedo")%>' ToolTip="Enter Soil Albedo" > </asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Slope" >
                    <ItemTemplate>
                        <asp:TextBox ID="txtSlope" CssClass="gvTextBox" runat="server" Text='<%#Eval("slope")%>' ToolTip="Enter Soil Slope(%)" > </asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Percentage" >
                    <ItemTemplate>
                        <asp:TextBox ID="txtPercentage" CssClass="gvTextBox" runat="server" Text='<%#Eval("percentage")%>' ToolTip="Enter Soil Percentage(%)" > </asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </fieldset>
    
    <section id="LayersModal" class="modalDialog" >
        <div>         
            <h2><asp:Label ID="lblLayersTitle" runat="server" Text="Layers" CssClass="treeView1" ></asp:Label></h2>
            <a href="#close" title="Close" class="close">X</a>
            <table>
                <tr>
                    <td>
<%--                        <asp:Label ID="lblSoil" CssClass="label" runat="server" Text="Soil" ></asp:Label>--%>
                        <asp:Label ID="lblSoilName" CssClass="label" runat="server" Text="Soil Name" BackColor="#FFFFCC"></asp:Label>
                        <asp:Button ID="btnAddEmptyLayer" runat="server" Text="Add Empty Layer"></asp:Button>
                    </td>
                    <td style="text-align:right">
                        <asp:LinkButton ID="btnSaveLayer" runat="server" Text="Save & Continue" OnClick="btnSaveLayers_Click"></asp:LinkButton> 
                    </td>
                </tr>
            </table>
            <section id="sctLayers" class="section">
               <asp:GridView ID="gvLayers" runat="server" AutoGenerateColumns="false"
                    OnRowDeleting="gvLayers_RowDeleting" >
                    <FooterStyle CssClass="gvFooterStyle" />
                    <RowStyle CssClass="gvRowStyle" />
                    <SelectedRowStyle CssClass="gvSelectedRowStyle" />            
                    <HeaderStyle CssClass="gvHeaderStyle" />
                    <Columns>
                        <asp:CommandField HeaderText="Delete" ShowDeleteButton="true" ShowHeader="true" />
                        <asp:BoundField DataField="LayerNumber" HeaderText="Layer" ReadOnly="true" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                        <asp:TemplateField HeaderText="Depth(in)" >
                            <ItemTemplate>
                                <asp:TextBox ID="txtDepth" CssClass="gvTextBox" runat="server" Text='<%#Eval("depth")%>' ToolTip="Enter Layer Depth in in."> </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Soil P" >
                            <ItemTemplate>
                                <asp:TextBox ID="txtSoilP" CssClass="gvTextBox" runat="server" Text='<%#Eval("soilp")%>' ToolTip="Enter Soil P(ppm)"> </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bulk Density(Mg/m3)" >
                            <ItemTemplate>
                                <asp:TextBox ID="txtBD" CssClass="gvTextBox" runat="server" Text='<%#Eval("bd")%>' ToolTip="Enter layer bulk density"> </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sand(%)" >
                            <ItemTemplate>
                                <asp:TextBox ID="txtSand" CssClass="gvTextBox" runat="server" Text='<%#Eval("sand")%>' ToolTip="Enter layer sand(%)"> </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Silt(%)" >
                            <ItemTemplate>
                                <asp:TextBox ID="txtSilt" CssClass="gvTextBox" runat="server" Text='<%#Eval("silt")%>' ToolTip="Enter layer silt (%)"> </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="OM(%)" >
                            <ItemTemplate>
                                <asp:TextBox ID="txtOM" CssClass="gvTextBox" runat="server" Text='<%#Eval("om")%>' ToolTip="Enter layer organic matter (%)"> </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PH" >
                            <ItemTemplate>
                                <asp:TextBox ID="txtPH" CssClass="gvTextBox" runat="server" Text='<%#Eval("ph")%>' ToolTip="Enter layer PH"> </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
             </section>
        </div>
    </section>
</asp:Content>
