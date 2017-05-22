Imports System.IO

Public Class Project
    Inherits System.Web.UI.Page
    Private lblMessage As Label
    Private imgIcon As Image
    'Classes definition
    Private _projects As New ProjectsData
    Private _startInfo As New StartInfo
    Private _sitesInfo As New List(Of SiteData)

    Private Sub Project_Error(sender As Object, e As System.EventArgs) Handles Me.Error
        Dim ac As Integer = 0
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        folder = My.Computer.FileSystem.GetParentPath(Server.MapPath(""))
        If Session("userGuide") = "" Then
            Response.Redirect("~/Default.aspx", False)
            Exit Sub
        End If
        'load whatever is in xml file but just for StartInfo
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

        ArrangeInfo("Open")
        If Not Page.IsPostBack Then
            If _startInfo.projectName <> "" And _startInfo.projectName <> String.Empty Then
                TurnOffOnButtons("opened")
            Else
                TurnOffOnButtons("closed")
            End If
        End If

        If Not Page.Request.Params("__EVENTTARGET") Is Nothing Then
            Select Case True
                Case Page.Request.Params("__EVENTTARGET").Contains("ddlExample")
                    ddlExamples_SelectedIndexChanged()
                Case Page.Request.Params("__EVENTTARGET").Contains("btnUpload")
                    btnOpenProject_Click()
                Case Page.Request.Params("__EVENTTARGET").Contains("btnSave")
                    btnSave_Click()
                Case Page.Request.Params("__EVENTTARGET").Contains("btnContinue")
                    btnContinue_click()
                Case Page.Request.Params("__EVENTTARGET").Contains("btnCloseProject")
                    btnCloseProject_Click()
            End Select
        End If
        AddProjectValues()
        openXMLLanguagesFile()
        ChangeLanguageContent()
        PopulateExamples()
    End Sub

    Private Sub PopulateExamples()
        ddlExamples.Items.Clear()
        ddlExamples.Items.Add(cntDoc.Descendants("SelectOne").Value)
        ddlExamples.Items.Add(cntDoc.Descendants("MDExample").Value)
        ddlExamples.Items.Add(cntDoc.Descendants("OHExample").Value)
    End Sub
    Private Sub ChangeLanguageContent()
        btnCreateProject.Text = cntDoc.Descendants("CreateNewProject").Value
        btnOpenExample.Text = cntDoc.Descendants("OpenExampleProject").Value
        btnOpenProject.Text = cntDoc.Descendants("OpenSavedProject").Value
        btnCloseProject.Text = cntDoc.Descendants("CloseActiveProject").Value
        btnSave.Text = cntDoc.Descendants("Save").Value
        btnContinue.Text = cntDoc.Descendants("Continue").Value
        btnSaveProject.Text = cntDoc.Descendants("SaveProject").Value
        'btnUpload.Text = cntDoc.Descendants("Upload").Value
        lblExample.Text = cntDoc.Descendants("SelectExample").Value
        lblProjectName.Text = cntDoc.Descendants("ProjectName").Value
        lblProjectDate.Text = cntDoc.Descendants("DateCreated").Value
        btnCloseProject.ToolTip = cntDoc.Descendants("ClearProject").Value
        lblProjectDescription.InnerText = cntDoc.Descendants("Description").Value
        lblProjectDescription.InnerHtml = cntDoc.Descendants("Description").Value
        fsetControls.InnerHtml = cntDoc.Descendants("SelectOption").Value
        fsetNew.InnerHtml = cntDoc.Descendants("EnterProjectInformation").Value
        fsetOpen.InnerHtml = cntDoc.Descendants("UploadingProject").Value
        fsetExample.InnerHtml = cntDoc.Descendants("SelectExample").Value
        'Uploader.Attributes.Add("desc", cntDoc.Descendants("UploadLabel").Value)
        btnUploader.Value = cntDoc.Descendants("UploadLabel").Value
        'tool tips
        btnCreateProject.ToolTip = msgDoc.Descendants("ttNewProject").Value
        btnOpenExample.ToolTip = msgDoc.Descendants("ttOpenExampleProject").Value
        btnOpenProject.ToolTip = msgDoc.Descendants("ttOpenSavedProject").Value
        btnCloseProject.ToolTip = msgDoc.Descendants("ttCloseProject").Value
        btnSave.ToolTip = msgDoc.Descendants("ttSaveAndContinue").Value
        btnSaveProject.ToolTip = msgDoc.Descendants("ttSaveproject").Value
        'btnUpload.ToolTip = msgDoc.Descendants("ttChoseFile").Value
        txtProjectName.ToolTip = msgDoc.Descendants("ttPorjectName").Value
        txtProjectDescription.ToolTip = msgDoc.Descendants("ttPorjectDescription").Value
    End Sub

    Private Sub TurnOffOnButtons(type As String)
        Try
            btnCreateProject.Style.Item("display") = ""
            btnOpenProject.Style.Item("display") = ""
            btnSaveProject.Style.Item("display") = ""
            btnOpenExample.Style.Item("display") = ""
            btnCloseProject.Style.Item("display") = ""
            sctNew.Style.Item("display") = "none"
            sctExample.Style.Item("display") = "none"
            sctOpen.Style.Item("display") = "none"

            Select Case type.ToLower
                Case "opened"
                    btnCreateProject.Style.Item("display") = "none"
                    btnOpenProject.Style.Item("display") = "none"
                    btnSaveProject.Style.Item("display") = ""
                    btnOpenExample.Style.Item("display") = "none"
                    btnCloseProject.Style.Item("display") = ""
                    sctNew.Style.Item("display") = ""
                Case "closed"
                    btnCreateProject.Style.Item("display") = ""
                    btnOpenProject.Style.Item("display") = ""
                    btnSaveProject.Style.Item("display") = "none"
                    btnOpenExample.Style.Item("display") = ""
                    btnCloseProject.Style.Item("display") = "none"
                    sctNew.Style.Item("display") = "none"
            End Select
        Catch e1 As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & e1.Message)
        End Try
    End Sub

    Private Sub AddProjectValues()
        If _startInfo Is Nothing Then Exit Sub
        txtProjectName.Text = _startInfo.projectName
        txtProjectDescription.Text = _startInfo.description
        If _startInfo.dates Is Nothing Then _startInfo.dates = Now()
        txtDate1.Value = _startInfo.dates
        If _startInfo.projectName <> "" Then sctNew.Style.Item("display") = ""
    End Sub

    Protected Sub btnSave_Click()
        Try
            _startInfo.projectName = txtProjectName.Text
            _startInfo.description = txtProjectDescription.Text
            _startInfo.dates = txtDate1.Value
            If _sitesInfo.Count = 0 Then
                Dim newSite As New SiteData
                _sitesInfo.Add(newSite)
            End If
            _sitesInfo(0).Latitude = 0
            _sitesInfo(0).Longitude = 0
            ArrangeInfo("Save")

            showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("InformationSaved").Value)

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Error").Value & ex.Message)
        End Try
    End Sub

    Protected Sub btnContinue_click()
        Response.Redirect("Location.aspx", False)
    End Sub

    Protected Sub btnCloseProject_Click()
        'InitializeLists()
        Session("projects") = Nothing
        Session("scenariosToRun") = Nothing
        Session("currentFieldNumber") = -1
        _startInfo = New StartInfo
        _sitesInfo = New List(Of SiteData)
        TurnOffOnButtons("closed")
        If File.Exists(folder + "\" + NTTxmlFolder + "\" + Session("userGuide") + ".xml") Then File.Delete(folder + "\" + NTTxmlFolder + "\" + Session("userGuide") + ".xml")
        Session("userGuide") = System.Guid.NewGuid.ToString
        showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("ProjectClosed").Value)
    End Sub

    Protected Sub btnSaveProject_Click(sender As Object, e As EventArgs) Handles btnSaveProject.Click
        Dim sFile As String = String.Empty
        Dim swFile As IO.StreamWriter = Nothing
        Dim output As StringBuilder = Nothing

        Try
            SaveProject(Session("userGuide"), Session("projects"))
            sFile = folder + "\" + NTTxmlFolder + "\" + Session("userGuide") + ".xml"
            If IO.File.Exists(sFile) Then
                Response.Clear()
                Response.ContentType = "text/xml"
                Response.AddHeader("Content-Disposition", "attachment;filename=NTTProjectFile.prj")
                Response.WriteFile(sFile)
                'Response.TransmitFile(sFile)
                Response.End()
            End If

            showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("ProjectSaved").Value)

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value)
        End Try
    End Sub

    Protected Sub ddlExamples_SelectedIndexChanged()
        Dim errors As String = "OK"
        Try
            Session("userGuide") = System.Guid.NewGuid.ToString
            Dim file As String = String.Empty
            Select Case ddlExamples.SelectedIndex
                Case 1
                    file = folder & "\App_Data\" & "MD_SingleField.xml"
                Case 2
                    file = folder & "\App_Data\" & "OH_MultipleFields.xml"
            End Select
            errors = btnOpen_File(file, _projects)
            Session("projects") = _projects
            ArrangeInfo("Open")
            If errors = "OK" Then
                showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("ProjectOpened").Value)
                AddProjectValues()
                TurnOffOnButtons("opened")
                ArrangeInfo("Save")
                Session("scenariosToRun") = Nothing
                If _projects._fieldsInfo1.count > 0 Then Session("currentFieldNumber") = 0
            Else
                If errors = "Cancelled" Then
                    showMessage(lblMessage, imgIcon, "Orange", "WarningIcon.jpg", msgDoc.Descendants("OptionCanceled").Value)
                Else
                    showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value)
                End If
                Exit Sub
            End If

        Catch e1 As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & e1.Message)
        End Try
    End Sub

    Protected Sub btnOpenProject_Click()
        Dim errors As String = "OK"
        Dim contentLen As Integer = 0

        Try
            Session("userGuide") = System.Guid.NewGuid.ToString
            contentLen = Uploader.PostedFile.ContentLength
            Me.Uploader.PostedFile.SaveAs(folder & "\App_xml\" & Session("userGuide") & ".xml")

            errors = btnOpen_File(folder & "\App_xml\" & Session("userGuide") & ".xml", _projects)
            Session("projects") = _projects
            ArrangeInfo("Open")
            If errors = "OK" Then
                showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("ProjectOpened").Value)
                AddProjectValues()
                TurnOffOnButtons("opened")
                ArrangeInfo("Save")
                Session("scenariosToRun") = Nothing
                If _projects._fieldsInfo1.Count > 0 Then Session("currentFieldNumber") = 0
                'SaveProject(Session("userGuide"), _projects)
            Else
                If errors = "Cancelled" Then
                    showMessage(lblMessage, imgIcon, "Orange", "WarningIcon.jpg", msgDoc.Descendants("OptionCanceled").Value)
                Else
                    showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value)
                End If
                Exit Sub
            End If

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Protected Sub btnCreateProject_Click(sender As Object, e As System.EventArgs) Handles btnCreateProject.Click
        Session("userGuide") = System.Guid.NewGuid.ToString
        'InitializeLists()
        Session("projects") = Nothing
        Session("scenariosToRun") = Nothing
        AddProjectValues()
        TurnOffOnButtons("opened")
        showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("ProjectCreated").Value)
    End Sub

    Private Sub ArrangeInfo(openSave As String)
        Select Case openSave
            'this is done after the open subrutine
            Case "Open"
                If Session("projects") Is Nothing Then Exit Sub
                _startInfo = Session("projects")._StartInfo
                _sitesInfo = Session("projects")._sitesInfo
                '_sitesInfo = _projects._sitesInfo
                'this is done before the save subroutine
            Case "Save"
                If Session("projects") Is Nothing Then Session("projects") = _projects
                Session("projects")._StartInfo = _startInfo
                '_projects._sitesInfo = _sitesInfo
                Session("projects")._sitesInfo = _sitesInfo
        End Select
    End Sub

End Class