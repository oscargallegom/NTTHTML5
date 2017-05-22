<%@ Page Title="Register" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Register.aspx.vb" Inherits="NTTHTML5.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <table class="login">
            <tr><td><label class="login" id="lblEmail" runat="server" for="usermail" >Email</label></td></tr>
            <tr><td><input type="email" name="usermail" placeholder="yourname@email.com" required="true" class="loginInput"></td></tr>
            <tr><td><label id="lblPassword" runat="server" for="password" class="labelLogin">Password</label></td></tr>
            <tr><td><input type="password" id="password" name="password" placeholder="password" required="true" class="loginInput"></td></tr>
            <tr><td><label id="lblConPassword" runat="server" for="conPassword" class="labelLogin">Confirm Password</label></td></tr>
            <tr><td><input type="password" id="conPassword" name="conPassword" placeholder="Confirm password" required="true" class="loginInput"></td></tr>
            <tr><td><label id="lblName" runat="server" for="txtname" class="labelLogin">Your Name</label></td></tr>
            <tr><td><input type="text" name="txtName" placeholder="Your Name" required="true" class="loginInput"></td></tr>
            <tr><td><label id="lblInstitution" runat="server" for="txtInstitution" class="labelLogin">your Institution</label></td></tr>
            <tr><td><input type="text" name="txtInstitution" placeholder="Your Institution" required="true" class="loginInput"></td></tr>
            <tr><td><input type="submit" id="btnLogin" runat="server" value="Login" onclick="__doPostBack('btnLogin', 'Login')" class="btnSubmit"></td></tr>
        </table>
    </div>
</asp:Content>
