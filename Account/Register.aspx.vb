Public Class Register
    Inherits System.Web.UI.Page
    Private lblMessage As Label
    Private imgIcon As Image

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        folder = My.Computer.FileSystem.GetParentPath(Server.MapPath(""))
        If Session("userGuide") = "" Then
            Response.Redirect("Default.aspx", False)
            Exit Sub
        End If
        lblMessage = New Label
        lblMessage = CType(Master.FindControl("lblMessage"), Label)
        imgIcon = New Image
        imgIcon = CType(Master.FindControl("imgIcon"), Image)
        lblMessage.Style.Item("display") = "none"
        imgIcon.Style.Item("display") = "none"
        lblMessage.Text = ""

        Select Case Page.Request.Params("__EVENTARGUMENT")
            Case english
                Session("Language") = english
            Case spanish
                Session("Language") = spanish
        End Select

        openXMLLanguagesFile()
        ChangeLanguageContent()

        If Not Page.Request.Params("__EVENTTARGET") Is Nothing Then
            Select Case True
                Case Page.Request.Params("__EVENTTARGET").Contains("btnLogin")
                    btnLogin_ServerClick()
            End Select
        End If
    End Sub

    Private Sub ChangeLanguageContent()
        lblEmail.InnerText = cntDoc.Descendants("Email").Value
        lblPassword.InnerText = cntDoc.Descendants("Password").Value
        lblName.InnerText = cntDoc.Descendants("Name").Value
        lblInstitution.InnerText = cntDoc.Descendants("YourInstitution").Value
        btnLogin.Value = cntDoc.Descendants("Login").Value
    End Sub

    Private Sub btnLogin_ServerClick()
        If Me.Request.Form.Get("password") = Me.Request.Form.Get("conPassword") Then
            If ValidateLogin(Me.Request.Form.Get("usermail"), Me.Request.Form.Get("password"), Me.Request.Form.Get("txtName"), Me.Request.Form.Get("txtInstitution"), Now) <> String.Empty Then
                Session("email") = "Started"
                showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("LoginSuccesful").Value)
                Response.Redirect("~/Views/Project.aspx", False)
            Else
                showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("LoginRegisterFailure").Value)
            End If
        Else
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("DifferentPassword").Value)
        End If
    End Sub
End Class