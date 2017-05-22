<%@ Page Title="Project" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Project.aspx.vb" Inherits="NTTHTML5.Project" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <script type="text/javascript" src="<%= ResolveUrl ("~/Scripts/project.js") %>"></script>  
<script type="text/javascript" >
</script>
     <div class="save">
        <asp:LinkButton ID="btnSaveContinue" runat="server" Text="Save & Continue" OnClick="btnSaveContinue_Click"></asp:LinkButton>
     </div>
     <fieldset class="section">
     <legend id="fsetControls" runat="server">Select an Option</legend>
     <table>
        <tr>
            <td>
                <asp:Button ID="btnCreateProject" runat="server" Text="Create New Project" OnClick="btnCreateProject_Click" />
                <asp:Button ID="btnOpenProject" runat="server" Text="Open Saved Project" OnClientClick="TurnOnSection('MainContent_sctOpen'); return false" />
                <asp:Button ID="btnSaveProject" runat="server" Text="Save Active Project . . ." OnClick="btnSaveProject_Click"/>
                <asp:Button ID="btnOpenExample" runat="server" Text="Open an Example Project" OnClientClick="TurnOnSection('MainContent_sctExample'); return false"/>
                <asp:Button ID="btnCloseProject" runat="server" Text="Close Active Project" OnClick="btnCloseProject_Click" OnClientClick="return btnCloseProject_ClientClick(); "/>
            </td>
        </tr>
    </table>
    </fieldset> 
    <fieldset id="sctOpen" class="section" runat="server" style="display:none">
        <legend id="fsetOpen" runat="server">Select your porject file for uploading</legend>
        <table >
            <tr>
                <td>
                    <asp:FileUpload ID="Uploader" runat="server" />
                    <asp:Label ID="lblOpen" Text="Click Browse to select the file and then Upload" runat="server"></asp:Label>
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" Width="56px" OnClientClick="__doPostBack('btnUpload', 'Load Project')" />                
               </td>
            </tr>
        </table>
    </fieldset>
    <fieldset id="sctExample" class="section" runat="server" style="display:none">
        <legend id="fsetExample" runat="server">Select the Example you want to open</legend>
        <table >
            <tr>
                <td>
                    <asp:Label ID="lblExample" Text="Select Example: " runat="server"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlExamples" AutoPostBack="true" OnSelectedIndexChanged="ddlExamples_SelectedIndexChanged">
                        <asp:ListItem Text="Select One" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="California Single field" ></asp:ListItem>
                        <asp:ListItem Text="Ohio Multiple Fields" ></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset id="sctNew" class="section" runat="server" style="display:none">
        <legend id="fsetNew" runat="server">Enter project information</legend>
        <table >
            <tr>
                <td>
                    <asp:Label ID="lblProjectName" Text="Project Name:" runat="server"></asp:Label>
                    <asp:TextBox ID="txtProjectName" runat="server"></asp:TextBox>
                    <asp:Label ID="lblProjectDate" Text="Date Created:" runat="server"></asp:Label>
                    <input type="text" runat="server" readonly="true" id="txtDate1" />
                </td>
            </tr>
            <tr >
                <td>
                    <label id="lblProjectDescription" style="text-align:center" runat="server" >Description</label>                    
                    <asp:TextBox ID="txtProjectDescription" Rows="5" runat="server" TextMode="MultiLine" Wrap="true" Width="353px"></asp:TextBox>
                </td>
            </tr>
        </table>        
    </fieldset>
</asp:Content>
