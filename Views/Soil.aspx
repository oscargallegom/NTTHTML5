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
        <asp:LinkButton ID="btnSave" runat="server" Text="Save" ></asp:LinkButton>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="btnContinue" runat="server" Text="Continue"  ></asp:LinkButton>
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
                    <select id="ddlSoils" name="ddlSoils" runat="server"></select>
                </td>
                <td style="text-align:left">
                    <input type="button" runat="server" id="btnAddSoil" value="Add Soil Selected" onclick="__doPostBack('btnAddSoil','btnAddSoil')"/>
                    <input type="button" runat="server" id="btnAddEmptySoil" value="Add Empty Soil" onclick="__doPostBack('btnAddEmptySoil','btnAddEmptySoil')"/>
                </td>
            </tr>
            <tr id="trField">
                <td >
                    <input class="label" runat="server" id="lblFields" name="lblFields" type="text" value="Select Field" />
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlFields" AutoPostBack="true" ></asp:DropDownList>
                    <input type="hidden" id="hdnField" runat="server" />
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
        &nbsp;<legend id="lblSoilsInfo" runat="server">Soils Information</legend>
        <asp:GridView ID="gvSoils" runat="server" PageSize="50" AutoGenerateColumns="false" style="width:100%">
            <FooterStyle CssClass="gvFooterStyle" />
            <RowStyle CssClass="gvRowStyle"/>
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
                <asp:TemplateField HeaderText="Key" >
                    <ItemTemplate>
                        <input ID="txtKey" runat="server" type="text" value='<%#Eval("key")%>' class="gvTextBoxMedium" /> 
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Symbol" >
                    <ItemTemplate>
                        <input ID="txtSymbol" class="gvStringBoxSmall" type="text" runat="server" value='<%#Eval("Symbol")%>'  />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Group" >
                    <ItemTemplate>
                        <input ID="txtGroup" class="gvStringBoxSmall" type="text" runat="server" value='<%#Eval("Group")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Name" >
                    <ItemTemplate>
                        <input ID="txtName" type="text" class="gvStringBox" runat="server" value='<%#Eval("Name")%>'  />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Albedo" >
                    <ItemTemplate>
                        <input id="txtAlbedo" type="text" runat="server" value='<%#Eval("albedo")%>' class="gvTextBoxMedium"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Slope" >
                    <ItemTemplate>
                        <input id="txtSlope" type="text" runat="server" value='<%#Eval("slope")%>' class="gvTextBoxMedium"/> 
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Percentage" >
                    <ItemTemplate>
                        <input id="txtPercentage" type="text" value='<%#Eval("percentage")%>' runat="server" class="gvTextBoxMedium"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Drainage Class" >
                    <ItemTemplate>
                        <select id="ddlDrainageCl" value='<%#Eval("wtmx")%>' runat="server">
                            <option value=0>Well Drained</option>
                            <option value=5>Poorly Drained</option> 
                            <option value=6>Somewhat poorly Drained</option>
                        </select>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            
        </asp:GridView>
    </fieldset>
    
    <section id="LayersModal" class="modalDialog1" >
        <div>         
            <h2><asp:Label ID="lblLayersTitle" runat="server" Text="Layers" CssClass="treeView1" ></asp:Label></h2>
            <a href="#close" title="Close" class="close">X</a>
            <table>
                <tr>
                    <td>
<%--                        <asp:Label ID="lblSoil" CssClass="label" runat="server" Text="Soil" ></asp:Label>--%>
                        <asp:Label ID="lblSoilName" CssClass="label" runat="server" Text="Soil Name" BackColor="#FFFFCC"></asp:Label>
                        <%--<asp:Button ID="btnAddEmptyLayer" runat="server" Text="Add Empty Layer" onclick="__doPostBack('btnAddLayer','btnAddLayer')"></asp:Button>--%>
                        <input type="button" runat="server" id="btnAddEmptyLayer" value="Add Layer" onclick="__doPostBack('btnAddLayer','btnAddLayer')"/>
                    </td>
                    <td style="text-align:right">
                        <asp:LinkButton ID="btnSaveLayer" runat="server" Text="Save & Continue"></asp:LinkButton> 
                    </td>
                </tr>
            </table>
            <section id="sctLayers" class="section">
               <asp:GridView ID="gvLayers" runat="server" AutoGenerateColumns="false">
                    <FooterStyle CssClass="gvFooterStyle" />
                    <RowStyle CssClass="gvRowStyle" />
                    <SelectedRowStyle CssClass="gvSelectedRowStyle" />            
                    <HeaderStyle CssClass="gvHeaderStyle" />
                    <Columns>
                        <asp:CommandField HeaderText="Delete" ShowDeleteButton="true" ShowHeader="true" DeleteText="Delete Layer"/>
                        <asp:BoundField DataField="LayerNumber" HeaderText="Layer" ReadOnly="true" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                        <asp:TemplateField HeaderText="Depth(in)" >
                            <ItemTemplate>
                                <input ID="txtDepth" class="gvTextBox" runat="server" value='<%#Eval("depth")%>'/> 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Soil P" >
                            <ItemTemplate>
                                <input ID="txtSoilP" class="gvTextBox" runat="server" value='<%#Eval("soilp")%>'/> 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bulk Density(Mg/m3)" >
                            <ItemTemplate>
                                <input ID="txtBD" class="gvTextBox" runat="server" value='<%#Eval("bd")%>' /> 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sand(%)" >
                            <ItemTemplate>
                                <input ID="txtSand" class="gvTextBox" runat="server" value='<%#Eval("sand")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Silt(%)" >
                            <ItemTemplate>
                                <input ID="txtSilt" class="gvTextBox" runat="server" value='<%#Eval("silt")%>' /> 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="OM(%)" >
                            <ItemTemplate>
                                <input ID="txtOM" class="gvTextBox" runat="server" value='<%#Eval("om")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PH" >
                            <ItemTemplate>
                                <input ID="txtPH" class="gvTextBox" runat="server" value='<%#Eval("ph")%>' /> 
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
             </section>
        </div>
    </section>
</asp:Content>
