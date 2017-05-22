Imports System.Net.Mail
Imports System.Net
Imports System.Web.UI
'Imports EASendMail

Public Class Site
    Inherits System.Web.UI.MasterPage

    Dim menuOptions(12) As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cs As ClientScriptManager = Page.ClientScript
        If Request.QueryString.Count > 0 Then
            If Request.QueryString("signOff") = "Off" Then
                Session("email") = ""
            End If
            If Request.QueryString("E") = "1" Then
                Session("EPage") = True
            Else
                Session("EPage") = False
            End If
            If Request.QueryString("M") = "1" Then
                Session("MPage") = True
            Else
                Session("MPage") = False
            End If
            If Request.QueryString("W") = "1" Then
                Session("WPage") = True
            Else
                Session("WPage") = False
            End If
            If Request.QueryString("D") = "1" Then
                Session("DnDc") = True
            Else
                Session("DnDc") = False
            End If
        End If
        LoadMasterContent()
        cs.GetPostBackEventReference(Me, "")
        openXMLLanguagesFile()
        ChangeLanguageContent()
        NavigationMenu.Items(0).Enabled = False
        ActivateOptions(NavigationMenu)
        lblStatus.Text = createSummaryText(MainContent.Page.Title)
        NavigationMenu_MenuItemClick()
        If Session("Language") Is Nothing Then
            Session("Language") = english
            Session("ManagementLanguage") = english
        End If
        If Page.Request.Params("__EVENTARGUMENT") Is Nothing Then Exit Sub
        If (Page.Request.Params("__EVENTARGUMENT").Contains(spanish) Or Page.Request.Params("__EVENTARGUMENT").Contains(english)) And Page.Request.Params("__EVENTTARGET") = "Language" Then
            Session("Language") = Page.Request.Params("__EVENTARGUMENT")
        End If

    End Sub

    Private Sub turnOnOff(count As UShort, ByRef navigationMenu As Menu)
        Dim all As UShort = 11  'first turn off all of the menu option from 2 to 11

        For i = 0 To all
            navigationMenu.Items(i).Text = "<div style='color:Gray'>" + menuOptions(i) + "</div>"
            navigationMenu.Items(i).Enabled = False
        Next
        navigationMenu.Items(12).Text = "<div style='color:Purple; float: right;'>" + menuOptions(12) + "</div>"
        For i = 0 To count
            If navigationMenu.Items(i).Text.Contains(aplDoc.Descendants("Economics").Value) Then
                If Session("EPage") = True Then
                    navigationMenu.Items(i).Text = "<div style='color:Black'>" + menuOptions(i) + "</div>"
                    navigationMenu.Items(i).Enabled = True
                End If
            ElseIf navigationMenu.Items(i).Text.Contains(aplDoc.Descendants("Modifications").Value) Then
                If Session("MPage") = True Or Page.Server.MachineName = "T-NN" Then
                    navigationMenu.Items(i).Text = "<div style='color:Black'>" + menuOptions(i) + "</div>"
                    navigationMenu.Items(i).Enabled = True
                End If
            Else
                navigationMenu.Items(i).Text = "<div style='color:Black'>" + menuOptions(i) + "</div>"
                navigationMenu.Items(i).Enabled = True
            End If
            If navigationMenu.Items(i).Text.Contains(aplDoc.Descendants("Modifications").Value) And Page.Server.MachineName <> "T-NN" Then
                If Session("MPage") = False Then
                    navigationMenu.Items.RemoveAt(i)
                End If
            End If
        Next
    End Sub

    Public Sub ActivateOptions(ByRef navigationMenu As Menu)
        trNavigationMenu.Visible = False
        trLoginMenu.Visible = False
        If Session("email") Is DBNull.Value Then
            trLoginMenu.Visible = True
            Exit Sub
        Else
            If Session("email") = String.Empty Then
                trLoginMenu.Visible = True
                Exit Sub
            Else
                trNavigationMenu.Visible = True
            End If
        End If

        Dim count As UShort = 11
        If Session("projects") Is Nothing Then
            count = 1
        Else
            If Session("projects")._StartInfo.projectName <> "" Then
                count = 2
                If Session("projects")._StartInfo.StateName <> "" Then
                    count = 3
                    'If _StartInfo.stationWay <> "" Then
                    'count = 4
                    If Session("projects")._fieldsInfo1.Count > 0 Then
                        count = 5
                        If Session("currentFieldNumber") >= 0 AndAlso Session("projects")._fieldsInfo1(Session("currentFieldNumber"))._soilsInfo.Count > 0 Then
                            count = 6
                            If Session("projects")._fieldsInfo1(Session("currentFieldNumber"))._scenariosInfo.Count > 0 Then
                                count = 11
                            End If
                        End If
                    End If
                End If
            End If
        End If
        turnOnOff(count, navigationMenu)
    End Sub

    Private Sub ChangeLanguageContent()
        menuOptions(0) = aplDoc.Descendants("Welcome").Value
        menuOptions(1) = aplDoc.Descendants("Project").Value
        menuOptions(2) = aplDoc.Descendants("Location").Value
        menuOptions(3) = aplDoc.Descendants("Weather").Value
        menuOptions(4) = aplDoc.Descendants("Fields").Value
        menuOptions(5) = aplDoc.Descendants("Soils").Value
        menuOptions(6) = aplDoc.Descendants("Management").Value
        menuOptions(7) = aplDoc.Descendants("Subproject").Value
        menuOptions(8) = aplDoc.Descendants("Economics").Value
        menuOptions(9) = aplDoc.Descendants("Simulation").Value
        menuOptions(10) = aplDoc.Descendants("Results").Value
        menuOptions(11) = aplDoc.Descendants("Modifications").Value
        menuOptions(12) = aplDoc.Descendants("Signoff").Value

        NavigationMenu.Items(0).Text = aplDoc.Descendants("Welcome").Value
        NavigationMenu.Items(1).Text = aplDoc.Descendants("Project").Value
        NavigationMenu.Items(2).Text = aplDoc.Descendants("Location").Value
        NavigationMenu.Items(3).Text = aplDoc.Descendants("Weather").Value
        NavigationMenu.Items(4).Text = aplDoc.Descendants("Fields").Value
        NavigationMenu.Items(5).Text = aplDoc.Descendants("Soils").Value
        NavigationMenu.Items(6).Text = aplDoc.Descendants("Management").Value
        NavigationMenu.Items(7).Text = aplDoc.Descendants("Subproject").Value
        NavigationMenu.Items(8).Text = aplDoc.Descendants("Economics").Value
        NavigationMenu.Items(9).Text = aplDoc.Descendants("Simulation").Value
        NavigationMenu.Items(10).Text = aplDoc.Descendants("Results").Value
        NavigationMenu.Items(11).Text = aplDoc.Descendants("Modifications").Value
        NavigationMenu.Items(12).Text = aplDoc.Descendants("Signoff").Value
        LoginMenu.Items(0).Text = aplDoc.Descendants("LogInPageTitle").Value
        LoginMenu.Items(1).Text = aplDoc.Descendants("RegisterPageTitle").Value
        itmInstructions.InnerText = cntDoc.Descendants("Instructions").Value
        itmContactUs.InnerText = cntDoc.Descendants("ContactUs").Value
        itmAboutNTT.InnerText = cntDoc.Descendants("About").Value
        itmNTTHelp.InnerText = cntDoc.Descendants("NTTHelp").Value
        itmLanguage1.InnerText = cntDoc.Descendants("Language").Value
    End Sub

    Private Sub LoadMasterContent()
        lblContactTitle.InnerText = cntDoc.Document.Descendants("ContactTitle").Value
        lblContactName.InnerText = cntDoc.Document.Descendants("ContactName").Value
        btnContactEnviar.Text = cntDoc.Document.Descendants("ContactSubmit").Value
        lblContactEmail.InnerText = cntDoc.Document.Descendants("ContactEmail").Value
        lblContactSubject.InnerText = cntDoc.Document.Descendants("ContactSubject").Value
        lblContactMessage.InnerText = cntDoc.Document.Descendants("ContactMessage").Value
        itmInstructions.InnerText = cntDoc.Descendants("Instructions").Value
        itmContactUs.InnerText = cntDoc.Descendants("ContactUs").Value
        itmAboutNTT.InnerText = cntDoc.Descendants("About").Value
        itmNTTHelp.InnerText = cntDoc.Descendants("NTTHelp").Value
        itmLanguage1.InnerText = cntDoc.Descendants("Language").Value
        itmEnglish.InnerText = "English"
        itmSpanish.InnerText = "Español"
        itmAboutNTT.Target = "_blank"
        itmNTTHelp.Target = "_blank"
        Select Case Session("Language")
            Case english
                If System.Net.Dns.GetHostName = "T-NN" Then
                    itmAboutNTT.HRef = "http://nn.tarleton.edu/NTTHelpEnglish/1)%20About%20NTT%20done.htm"
                    itmNTTHelp.HRef = "http://nn.tarleton.edu/NTTHelpEnglish/0) Table of Contents.htm"
                Else
                    itmAboutNTT.HRef = "http://45.40.132.224/NTTHelpEnglish/1)%20About%20NTT%20done.htm"
                    itmNTTHelp.HRef = "http://45.40.132.224/NTTHelpEnglish/0) Table of Contents.htm"
                End If
            Case spanish
                If System.Net.Dns.GetHostName = "T-NN" Then
                    itmAboutNTT.HRef = "http://nn.tarleton.edu/NTTHelpSpanish/1)%20About%20NTT%20done.htm"
                    itmNTTHelp.HRef = "http://nn.tarleton.edu/NTTHelpSpanish/0) Table of Contents.htm"
                Else
                    itmAboutNTT.HRef = "http://45.40.132.224/NTTHelpSpanish/1)%20About%20NTT%20done.htm"
                    itmNTTHelp.HRef = "http://45.40.132.224/NTTHelpSpanish/0) Table of Contents.htm"
                End If
            Case portuguese
        End Select

    End Sub

    Protected Sub btnContactEnviar_Click(sender As Object, e As EventArgs) Handles btnContactEnviar.Click
        Dim smtpAddress As String = "smtp.gmail.com"   'hotmail smtp server = smtp.live.com
        Dim portNumber As Int16 = 587
        Dim enableSSL As Boolean = True
        Dim emailFrom As String = "NutrientTrackingTool@gmail.com"
        Dim emailCC As String = txtContactEmail.Text
        Dim password As String = "20150712_NTT"
        'Dim emailTo As String = "ogallego@tiaer.tarleton.edu; oscargallegom@hotmail.com; " & txtContactEmail.Text
        Dim subject As String = txtContactSubject.Text
        Dim body As String = txtContactMessage.Text

        Try
            Using mailMessage As New MailMessage()
                With mailMessage
                    .To.Add("ogallego@tiaer.tarleton.edu")
                    .To.Add("oscargallegom@hotmail.com")
                    .To.Add(txtContactEmail.Text)
                    .From = New MailAddress(emailFrom)
                    .Subject = subject
                    .Body = body
                    .IsBodyHtml = True
                End With

                Dim client As New SmtpClient(smtpAddress, portNumber)
                client.UseDefaultCredentials = False
                client.Credentials = New NetworkCredential(emailFrom, password)
                client.EnableSsl = enableSSL
                client.Send(mailMessage)

            End Using

            showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("EmailMessage").Value)

        Catch ex As System.Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Protected Sub NavigationMenu_MenuItemClick()
        Select Case True
            Case MainContent.Page.Title = "Home Page"
                lbltitle.Text = aplDoc.Document.Descendants("WelcomePageTitle").Value
            Case MainContent.Page.Title = "Project"
                lbltitle.Text = aplDoc.Document.Descendants("ProjectPageTitle").Value
                lblInstructions.InnerText = aplDoc.Document.Descendants("InstructionsProject").Value
            Case MainContent.Page.Title = "Location"
                lbltitle.Text = aplDoc.Document.Descendants("LocationPageTitle").Value
                lblInstructions.InnerText = aplDoc.Document.Descendants("InstructionsLocation").Value
            Case MainContent.Page.Title = "Weather"
                lbltitle.Text = aplDoc.Document.Descendants("WeatherPageTitle").Value
                lblInstructions.InnerText = aplDoc.Document.Descendants("InstructionsWeather").Value
            Case MainContent.Page.Title = "Field"
                lbltitle.Text = aplDoc.Document.Descendants("FieldPageTitle").Value
                lblInstructions.InnerText = aplDoc.Document.Descendants("InstructionsFields").Value
            Case MainContent.Page.Title = "Soil"
                lbltitle.Text = aplDoc.Document.Descendants("SoilPageTitle").Value
                lblInstructions.InnerText = aplDoc.Document.Descendants("InstructionsSoils").Value
            Case MainContent.Page.Title = "Management"
                lbltitle.Text = aplDoc.Document.Descendants("ManagementPageTitle").Value
                lblInstructions.InnerText = aplDoc.Document.Descendants("InstructionsManagement").Value
            Case MainContent.Page.Title = "Economics"
                lbltitle.Text = aplDoc.Document.Descendants("EconomicsPageTitle").Value
                lblInstructions.InnerText = aplDoc.Document.Descendants("InstructionsEconomics").Value
            Case MainContent.Page.Title = "Simulation"
                lbltitle.Text = aplDoc.Document.Descendants("SimulationPageTitle").Value
                lblInstructions.InnerText = aplDoc.Document.Descendants("InstructionsSimulation").Value
            Case MainContent.Page.Title = "Results"
                lbltitle.Text = aplDoc.Document.Descendants("ResultPageTitle").Value
                lblInstructions.InnerText = aplDoc.Document.Descendants("InstructionsResults").Value
            Case MainContent.Page.Title = "Subproject"
                lbltitle.Text = aplDoc.Document.Descendants("SubprojectPageTitle").Value
                lblInstructions.InnerText = aplDoc.Document.Descendants("InstructionsSubproject").Value
            Case MainContent.Page.Title = "Modifications"
                lbltitle.Text = aplDoc.Document.Descendants("ModifyPageTitle").Value
                lblInstructions.InnerText = aplDoc.Document.Descendants("InstructionsModifications").Value
            Case MainContent.Page.Title = "LogIn"
                lbltitle.Text = aplDoc.Document.Descendants("LogInPageTitle").Value
                lblInstructions.InnerText = aplDoc.Document.Descendants("InstructionsLogIn").Value
            Case MainContent.Page.Title = "Register"
                lbltitle.Text = aplDoc.Document.Descendants("RegisterPageTitle").Value
                lblInstructions.InnerText = aplDoc.Document.Descendants("InstructionsRegister").Value
        End Select

    End Sub
End Class