<%@ Page Title="LogIn" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Login.aspx.vb" Inherits="NTTHTML5.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <script type="text/javascript">
         function TurnChangeOn() {
             document.getElementById("newPassword").style.display="";
             document.getElementById("newPassword").value = "";
             document.getElementById("conPassword").style.display = "";
             document.getElementById("conPassword").value = "";
             document.getElementById("MainContent_sbmtChange").style.display = "";
             document.getElementById("MainContent_btnLogin").style.display = "none";
             document.getElementById("MainContent_lblNewPassword").style.display = "";             
         }
    </script>
    <div>
        <table class="login">
            <tr><td><label id="lblEmail" runat="server" for="usermail" >Email</label></td></tr>
            <tr><td><input type="email" name="usermail" placeholder="yourname@email.com" required="true" class="loginInput" ></td></tr>
            <tr><td><label id="lblPassword" runat="server" for="password">Password</label></td></tr>
            <tr>
                <td><input type="password" id="password" name="password" placeholder="password" required="true" class="loginInput"></td>
                <td><input type="button" id="btnForget" runat="server" name="btnForget" value="Forget Password" onclick="__doPostBack('btnForget', 'Forget')" class="btnLogin"/>
                </td>
                <td><input type="button" id="btnChange" runat="server" name="btnChange" value="Change Password" onclick="TurnChangeOn();" class="btnLogin"/>
                </td>
            </tr>
            <tr><td><label id="lblNewPassword" name="lblNewPassword" runat="server" for="newPassword" class="labelLogin" style="display:none">New Password</label></td></tr>
            <tr>
                <td><input type="password" id="newPassword" name="newPassword" placeholder="New Password" value="NewPassword" required="true" style="display:none" class="loginInput"></td>
            </tr>
            <tr><td><label id="lblConPasswowrd" name="lblConPassword" runat="server" for="conPassword" class="labelLogin" style="display:none">Confirm Password</label></td></tr>
            <tr>
                <td><input type="password" id="conPassword" name="conPassword" placeholder="Confirm Password" value="ConPassword" required="true" style="display:none" class="loginInput"></td>
            </tr>
            <tr><td><input type="submit" id="btnLogin" name="btnLogin" runat="server" value="Login" onclick="__doPostBack('btnLogin', 'Login')" class="btnSubmit"></td></tr>
            <tr><td><input type="submit" id="sbmtChange" name="sbmtChange" runat="server" value="Change Password" onclick="__doPostBack('btnChange', 'Change')" style="display:none; " class="btnSubmit"></td></tr>
        </table>
    </div>
</asp:Content>
