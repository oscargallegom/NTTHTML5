<%@ Page Title="Field" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Field.aspx.vb" Inherits="NTTHTML5.Field" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var chk = "No";
        function AssignValue() {
            chk = "Yes";
        }

        function onClick(element) {
            if (chk == "Yes") {
                var rowIndex = element;
                var activeRow = document.getElementById('MainContent_gvFields').rows[rowIndex + 1];
                activeRow.cells[7].children[0].value = activeRow.cells[7].children[1].checked;
                __doPostBack('chkForestry', element);
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

    </script>
     <div class="save" >
        <asp:LinkButton ID="btnSaveContinue" runat="server" Text="Save & Continue" ></asp:LinkButton>
     </div>
    <fieldset id="sctGeneralInfo" class="section">
    <legend id="lblFieldsInfo" runat="server">Fields Information</legend>
        <asp:Button ID="btnAddField" runat="server" Text="Add"/>
        <asp:GridView ID="gvFields" runat="server" AutoGenerateColumns="false" OnRowDeleting="gvField_RowDeleting" >
            <FooterStyle CssClass="gvFooterStyle" />
            <RowStyle CssClass="gvRowStyle" />
            <AlternatingRowStyle CssClass="gvAltRowStyle" />
            <HeaderStyle CssClass="gvHeaderStyle" />
            <Columns>
                <%--<asp:ButtonField HeaderText="Delete" ShowHeader="true" ButtonType="Link" Text="Delete" />--%>
                <asp:CommandField HeaderText="Delete" ShowDeleteButton="true" ShowHeader="true" DeleteText="Delete"/>
                <asp:BoundField  DataField="number" HeaderText="#" ReadOnly="true" ></asp:BoundField>
                <asp:TemplateField HeaderText="Name" >
                    <ItemTemplate>
                        <asp:TextBox ID="txtName" Text='<%#Eval("name")%>' runat="server" > </asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Area (ac.)">
                    <ItemTemplate>
                        <asp:TextBox ID="txtArea" CssClass="gvTextBox" Text='<%#Eval("area")%>' runat="server"  > </asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Channel Vegetation Cover Condition" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlRchc" runat="server" Text='<%#Eval("rchcVal")%>' ToolTip="Select Channel Vegetation Cover Condition" ></asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Channel Erodibility" HeaderStyle-Wrap="true" HeaderStyle-Width="160">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlRchk" runat="server" Text='<%#Eval("rchkVal")%>' ToolTip="Select Channel Erodibility" ></asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField  DataField="avgSlope" HeaderText="Average Slope" ReadOnly="true" ItemStyle-CssClass="gvTextBoxSmall"></asp:BoundField>
                <asp:TemplateField HeaderText="Forestry" >
                    <ItemTemplate>
                        <input type="hidden" id="txtForestry" name="txtForestry" runat="server" />
                        <input type="checkbox" runat="server" id="chkForestry" name="chkFor" checked='<%#Eval("forestry")%>' onchange="AssignValue();"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </fieldset>    
</asp:Content>