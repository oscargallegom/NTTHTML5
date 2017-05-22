Imports System.Net.Mail
Imports System.Net
'Imports EASendMail

Public Class Login
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
                Case Page.Request.Params("__EVENTTARGET").Contains("btnForget")
                    btnForget_ServerClick()
                Case Page.Request.Params("__EVENTTARGET").Contains("btnChange")
                    btnChange_ServerClick()
            End Select
        End If
    End Sub

    Private Sub ChangeLanguageContent()
        lblEmail.InnerText = cntDoc.Descendants("Email").Value
        lblPassword.InnerText = cntDoc.Descendants("Password").Value
        lblNewPassword.InnerText = cntDoc.Descendants("NewPassword").Value
        lblConPasswowrd.InnerText = cntDoc.Descendants("ConPassword").Value
        btnLogin.Value = cntDoc.Descendants("Login").Value
        sbmtChange.Value = cntDoc.Descendants("ChangePassword").Value
        btnChange.Value = cntDoc.Descendants("ChangePassword").Value
        btnForget.Value = cntDoc.Descendants("ForgetPassword").Value
    End Sub

    Private Sub btnForget_ServerClick()
        Dim email As String = Me.Request.Form.Get("usermail")
        If email = String.Empty Or email = "" Then
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("NoEmail").Value)
            Exit Sub
        End If
        Dim name As String = CreatePassword(email)
        If name <> String.Empty Then
            SendPassword(email, name)
            showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("PasswordForget").Value)
        Else
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("LoginForgetFailure").Value)
        End If
    End Sub

    Private Sub btnLogin_ServerClick()
        Dim email As String = Me.Request.Form.Get("usermail")
        Dim password As String = Me.Request.Form.Get("password")
        Dim msgs As String = ""

        msgs = ValidateLogin(email, password, String.Empty, String.Empty, Now)
        If msgs = "OK" Then
            Session("email") = "Started"
            showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("LoginSuccesful").Value)
            Response.Redirect("~/Views/Project.aspx", False)
        Else
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("LoginFailure").Value & " ---- " & msgs & "---" & dbConnectString("No"))
        End If
    End Sub

    Private Sub btnChange_ServerClick()
        If Me.Request.Form.Get("newPassword") = Me.Request.Form.Get("conPassword") Then
            If ChangePassword(Me.Request.Form.Get("usermail"), Me.Request.Form.Get("password"), Me.Request.Form.Get("newPassword")) <> String.Empty Then
                showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("PasswordChanged").Value)
            Else
                showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("LoginChangeFailure").Value)
            End If
        Else
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("DifferentPassword").Value)
        End If
    End Sub

    'Protected Sub SendPassword(eMail As String, namePass As String)
    '    Dim _to As String = eMail
    '    Try
    '        Using mailMessage1 As New MailMessage()
    '            With mailMessage1
    '                .To.Add(_to)
    '                .From = New MailAddress("ntt@tiaer.tarleton.edu", "NTT Support Group")
    '                '.From = New MailAddress("", namePass.Split("|")(0))
    '                .Subject = "Account Request"
    '                .Body = namePass.Split("|")(0) & ", Your password has been changed to " & namePass.Split("|")(1) & "Please change it as soon as possible. Do not respond to this e-mail, it is generated Automatically. Thank you"
    '                .Sender = New MailAddress("ogallego@tiaer.tarleton.edu")
    '            End With

    '            Dim client As New SmtpClient()
    '            client.UseDefaultCredentials = True

    '            With client
    '                .Host = "tiaer1e.tarleton.edu"
    '                .Send(mailMessage1)
    '            End With

    '        End Using

    '        showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("EmailMessage").Value)

    '    Catch ex As System.Exception
    '        showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
    '    End Try
    'End Sub

    'Protected Sub SendPassword(eMail As String, namePass As String)
    '    Dim smtpAddress As String = "smtp.mail.yahoo.com"
    '    Dim portNumber As Int16 = 587
    '    Dim enableSSL As Boolean = True
    '    Dim emailFrom As String = "email@yahoo.com"
    '    Dim password As String = "abcdefg"
    '    Dim emailTo As String = "oscargallegom@hotmail.com"
    '    Dim subject As String = "Hello"
    '    Dim body As String = "Hello, I'm just writing this to say Hi!"

    '    Try
    '        Using mailMessage As New MailMessage()
    '            With mailMessage
    '                .To.Add(emailTo)
    '                .From = New MailAddress(emailFrom)
    '                '.From = New MailAddress("", namePass.Split("|")(0))
    '                .Subject = subject
    '                .Body = body
    '                .IsBodyHtml = True
    '            End With

    '            Dim client As New SmtpClient(smtpAddress, portNumber)
    '            client.Credentials = New NetworkCredential(emailFrom, password)
    '            client.EnableSsl = enableSSL
    '            client.Send(mailMessage)

    '        End Using

    '        showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("EmailMessage").Value)

    '    Catch ex As System.Exception
    '        showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
    '    End Try
    'End Sub

    Protected Sub SendPassword(eMail As String, namePass As String)
        Dim smtpAddress As String = "smtp.gmail.com"
        Dim portNumber As Int16 = 587
        Dim enableSSL As Boolean = True
        Dim emailFrom As String = "NutrientTrackingTool@gmail.com"
        Dim password As String = "20150712_NTT"
        Dim emailTo As String = eMail
        Dim subject As String = "NTT Password change Request"
        Dim body As String = namePass.Split("|")(0) & ", Your password has been changed to " & namePass.Split("|")(1) & ". Please change it as soon as possible. Do not respond to this e-mail, it is generated Automatically. Thank you"

        Try
            Using mailMessage As New MailMessage()
                With mailMessage
                    .To.Add(emailTo)
                    .From = New MailAddress(emailFrom)
                    '.From = New MailAddress("", namePass.Split("|")(0))
                    .Subject = subject
                    .Body = body
                    .IsBodyHtml = True
                End With

                Dim client As New SmtpClient(smtpAddress, portNumber)
                client.Credentials = New NetworkCredential(emailFrom, password)
                client.EnableSsl = enableSSL
                client.Send(mailMessage)

            End Using

            showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("EmailMessage").Value)

        Catch ex As System.Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    'Protected Sub SendPassword(eMail As String, namePass As String)
    '    'to use this method it is needed the reference. It is located in the resources folder (called EASendMail).
    '    'The reference is EASendmail20.dll.
    '    'The e-mail used at this moment is oscargallegom@gmail.com and the password is Gallego62689497. It should be changed to nttsupport1 and nttsupport102815.
    '    Dim oMail As New SmtpMail("TryIt")
    '    Dim oSmtp As New SmtpClient()

    '    Try
    '        ' Your gmail email address
    '        'oMail.From = "ogallego@tiaer.tarleton.edu"
    '        oMail.From = eMail

    '        oMail.Cc = eMail
    '        ' Set recipient email address, please change it to yours
    '        oMail.To = eMail

    '        ' Set email subject
    '        oMail.Subject = "NTT Password change Request"

    '        ' Set email body
    '        oMail.TextBody = namePass.Split("|")(0) & ", Your password has been changed to " & namePass.Split("|")(1) & "Please change it as soon as possible. Do not respond to this e-mail, it is generated Automatically. Thank you"

    '        'Gmail SMTP server address
    '        Dim oServer As New SmtpServer("smtp.gmail.com")

    '        ' set 25 port, if you want to use 587 port, please change 25 to 587
    '        oServer.Port = 25

    '        ' detect SSL/TLS automatically
    '        oServer.ConnectType = SmtpConnectType.ConnectSSLAuto

    '        ' Gmail user authentication should use your 
    '        ' Gmail email address as the user name. 
    '        ' For example: your email is "gmailid@gmail.com", then the user should be "gmailid@gmail.com"
    '        'oServer.User = "nttsupport1"
    '        'oServer.Password = "nttsupport102815"
    '        oServer.User = "oscargallegom"
    '        oServer.Password = "Gallego62689497"


    '        oSmtp.SendMail(oServer, oMail)
    '        showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("EmailMessage").Value)

    '    Catch ex As System.Exception
    '        showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
    '    End Try
    'End Sub

End Class