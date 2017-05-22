<%@ Page Title="Subproject" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Subproject.aspx.vb" Inherits="NTTHTML5.Subproject" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <script type="text/javascript" src="<%= ResolveUrl ("~/Scripts/subproject.js") %>"></script> 
     <script type="text/javascript">
         function DeleteSubproject() {
             var answer = confirm("Are you sure you want to delete this subproject?");
             if (answer == true) { __doPostBack("btnDeleteSubproject", "DeleteSubproject"); }
             else { return false; }
         } 
    </script>
     <div class="save" >
        <asp:LinkButton ID="btnSaveContinue" runat="server" Text="Save & Continue" OnClick="btnSaveContinue_Click"></asp:LinkButton>
     </div>
     <fieldset class="section" >
        <legend id="fsetSubproject" runat="server">Add, Delete, Select Subproject</legend>
        <asp:TextBox ID="txtName" CssClass="gvStringBoxLarge" runat="server" > </asp:TextBox>
        <asp:Label ID="lblSubprojectName" runat="server" Text="Subproject Name: " ></asp:Label>    
        <asp:Button ID="btnAddSubproject" runat="server" Text="Add New Subproject"/>
        <asp:Label ID="lblSubproject" runat="server" Text="Select Subproject: "></asp:Label>
        <asp:DropDownList ID="ddlSubproject" AutoPostBack="true" runat="server" OnSelectedIndexChange="dddlSubproject_SelectedIndexChanged(this, e)"></asp:DropDownList>
        <asp:Button ID="btnDeleteSubproject" runat="server" Text="Delete Subproject" OnClientClick="DeleteSubproject();" />
    </fieldset>
     <fieldset class="section">
        <legend id="fsetSelect" runat="server">Select Fields/Scenarios for this Subproject</legend>
        <asp:Label ID="lblField" runat="server" Text="Select Field: "></asp:Label>
        <asp:DropDownList ID="ddlFields" AutoPostBack="true" runat="server"></asp:DropDownList>
        <asp:Label ID="lblScenario" runat="server" Text="Select Scenario: "></asp:Label>
        <asp:DropDownList ID="ddlScenario" runat="server"></asp:DropDownList>
        <asp:Button ID="btnIncludeScenario" runat="server" Text="Include Field/Scenario"/>
    </fieldset>

    <fieldset id="sctSubproject" class="section">
        <legend id="fsetGridView" runat="server">List of Fields/Scenarios in the subproject selected</legend>
        <asp:GridView ID="gvSubproject" runat="server" AutoGenerateColumns="false" OnRowDeleting="gvSubproject_RowDeleting">
            <FooterStyle CssClass="gvFooterStyle" />
            <RowStyle CssClass="gvRowStyle"/>
            <SelectedRowStyle CssClass="gvSelectedRowStyle" />
            <PagerStyle CssClass="gvPagerSyle" HorizontalAlign="Center" />
            <HeaderStyle CssClass="gvHeaderStyle" />
            <Columns>
                <asp:CommandField HeaderText="Delete" ShowDeleteButton="true" ShowHeader="true" DeleteText="Delete" />
                <asp:BoundField  DataField="field" HeaderText="Field" ReadOnly="true" ></asp:BoundField>
                <asp:BoundField  DataField="scenario" HeaderText="Scenario" ReadOnly="true" ></asp:BoundField>
            </Columns>
        </asp:GridView>
    </fieldset>
</asp:Content>