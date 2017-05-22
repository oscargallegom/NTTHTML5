Imports NTTHTML5.SubprojectNameData

Public Class Subproject
    Inherits System.Web.UI.Page

    Private lblMessage As Label
    Private imgIcon As Image
    Public Overridable Property DeleteText As String
    'Classes definition
    Private _startInfo As New StartInfo
    Private _fieldsInfo1 As New List(Of FieldsData)
    Private _subprojectName As New List(Of SubprojectNameData)
    'retrieve the currentfieldnumber
    Private currentFieldNumber As Short = 0
    Private currentScenarioNumber As Short = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("userGuide") = "" Then
            Response.Redirect("~/Default.aspx", False)
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

        ArrangeInfo("Open")
        If Not Page.Request.Params("__EVENTTARGET") Is Nothing Then
            Select Case True
                Case Page.Request.Params("__EVENTTARGET").Contains("btnDeleteSubproject")
                    btnDeleteSubproject_Click()
            End Select
        End If

        If Not IsPostBack Then
            currentFieldNumber = Session("currentFieldNumber")
            ddlFields.SelectedIndex = currentFieldNumber
            LoadFields(ddlFields, _fieldsInfo1, currentFieldNumber)
            LoadScenarios(ddlScenario, _fieldsInfo1(currentFieldNumber)._scenariosInfo, currentScenarioNumber)
            LoadSubprojectNames(0)
        End If

    End Sub

    Private Sub LoadSubprojectNames(index As UShort)
        ddlSubproject.DataSource = _subprojectName
        ddlSubproject.DataTextField = "Name"
        ddlSubproject.DataValueField = "Name"
        ddlSubproject.DataBind()
        If _subprojectName.Count > 0 Then
            ddlSubproject.SelectedIndex = index
            LoadSubprojects()
        End If
    End Sub

    Public Sub LoadSubprojects()
        If ddlSubproject.SelectedIndex >= 0 Then
            gvSubproject.DataSource = _subprojectName(ddlSubproject.SelectedIndex)._subproject
            gvSubproject.DataBind()
        End If
    End Sub

    Protected Sub btnSaveContinue_Click()
        Response.Redirect("Simulation.aspx", False)
    End Sub

    Private Sub ChangeLanguageContent()
        btnAddSubproject.Text = cntDoc.Descendants("AddNewSubproject").Value
        btnDeleteSubproject.Text = cntDoc.Descendants("DeleteSubproject").Value
        btnIncludeScenario.Text = cntDoc.Descendants("SelectSubproject").Value
        btnSaveContinue.Text = cntDoc.Descendants("Continue").Value
        lblScenario.Text = cntDoc.Descendants("Scenario").Value
        lblField.Text = cntDoc.Descendants("Field").Value
        lblSubproject.Text = cntDoc.Descendants("Subproject").Value
        lblSubprojectName.Text = cntDoc.Descendants("SubprojectName").Value
        fsetSubproject.InnerHtml = cntDoc.Descendants("AddDeleteSelectSubproject").Value
        fsetSelect.InnerHtml = cntDoc.Descendants("SelectFieldScenario").Value
        fsetGridView.InnerHtml = cntDoc.Descendants("ListFieldScenario").Value
        'grid view
        gvSubproject.Columns(0).HeaderText = cntDoc.Descendants("Delete").Value
        gvSubproject.Columns(0).HeaderText = cntDoc.Descendants("Field").Value
        gvSubproject.Columns(0).HeaderText = cntDoc.Descendants("Scenario").Value
    End Sub

    Private Sub btnAddNewSub_Click(sender As Object, e As System.EventArgs) Handles btnAddSubproject.Click
        Try
            'check that the scenario name is in place
            If txtName.Text = "" Or txtName.Text = String.Empty Then Throw New Global.System.Exception(msgDoc.Descendants("SubprojectName").Value & vbCrLf)
            'check that scneario does not exist.
            For i = 0 To _subprojectName.Count - 1
                If txtName.Text.Trim = _subprojectName(i).Name Then
                    showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("SubprojectExist").Value)
                    Exit Sub
                End If
            Next
            Dim subproject As SubprojectNameData
            subproject = New SubprojectNameData
            subproject.Name = txtName.Text
            txtName.Text = ""
            _subprojectName.Add(subproject)
            ddlSubproject.DataSource = _subprojectName
            ddlSubproject.DataBind()
            showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", "New Scenario " & msgDoc.Descendants("RecordAdded").Value)
            currentScenarioNumber = _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count - 1
            _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Lm = True
            btnAddSubproject.Text = cntDoc.Descendants("AddNewSubproject").Value
            LoadSubprojectNames(ddlSubproject.Items.Count - 1)

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", ex.Message)
        End Try
    End Sub

    Protected Sub ddlFields_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFields.SelectedIndexChanged
        currentFieldNumber = sender.SelectedIndex
        Session("currentFieldNumber") = currentFieldNumber
        If _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count > 0 Then
            currentScenarioNumber = 0
        Else
            currentScenarioNumber = -1
        End If
        LoadScenarios(ddlScenario, _fieldsInfo1(currentFieldNumber)._scenariosInfo, currentScenarioNumber)
    End Sub

    Protected Sub gvSubproject_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Try
            _subprojectName(ddlSubproject.SelectedIndex)._subproject.RemoveAt(e.RowIndex)
            _subprojectName(ddlSubproject.SelectedIndex).TotalArea = _subprojectName(ddlSubproject.SelectedIndex)._subproject.Sum(Function(x) x.Area)
            gvSubproject.DataBind()

            showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("RecordDeleted").Value)
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("DeleteScenarioError").Value & msgDoc.Descendants("Errors").Value)
        End Try
    End Sub

    Protected Sub btnIncludeScenario_Click(sender As Object, e As EventArgs) Handles btnIncludeScenario.Click
        Try
            'validate that atleast one subproject has been created
            If ddlSubproject.Items.Count = 0 Then
                Throw New Global.System.Exception(msgDoc.Descendants("SubprojectError").Value)
            End If

            'validate that the field to inlcude in the subproject has not been included yet
            For Each record In _subprojectName(ddlSubproject.SelectedIndex)._subproject
                If record.Field = ddlFields.SelectedItem.Text Then
                    Throw New Global.System.Exception(msgDoc.Descendants("FieldError").Value)
                    Exit Sub
                End If
            Next
            Dim scenario As New SubprojectNameData.SubProjectData

            scenario.Field = ddlFields.SelectedItem.Text
            scenario.Scenario = ddlScenario.SelectedItem.Text
            scenario.Area = _fieldsInfo1(ddlFields.SelectedIndex).Area
            _subprojectName(ddlSubproject.SelectedIndex)._subproject.Add(scenario)

            _subprojectName(ddlSubproject.SelectedIndex).TotalArea = _subprojectName(ddlSubproject.SelectedIndex)._subproject.Sum(Function(x) x.Area)
            LoadSubprojects()

        Catch ex As System.Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("IncludeScenarioError").Value & ex.Message)
        End Try
    End Sub

    Private Sub gvSubproject_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSubproject.RowCreated
        Select e.Row.RowType
            Case DataControlRowType.DataRow
                e.Row.Cells(0).Controls(0).ID = "btnDelete"
                Dim btnDelete As New Object
                btnDelete = e.Row.Cells(0).FindControl("btnDelete")
                btnDelete.Text = cntDoc.Descendants("Delete").Value
            Case DataControlRowType.Header
                e.Row.Cells(0).Text = cntDoc.Descendants("Delete").Value
                e.Row.Cells(1).Text = cntDoc.Descendants("Field").Value
                e.Row.Cells(2).Text = cntDoc.Descendants("Scenario").Value
        End Select

    End Sub

    Private Sub ddlSubproject_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlSubproject.SelectedIndexChanged
        LoadSubprojects()
    End Sub

    Private Sub btnDeleteSubproject_Click()
        Try
            If ddlSubproject.SelectedIndex < 0 Then
                Throw New Global.System.Exception(msgDoc.Descendants("NoSubproject").Value & vbCrLf)
            End If

            _subprojectName.RemoveAt(ddlSubproject.SelectedIndex)
            LoadSubprojectNames(0)
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", ex.Message)
        End Try
    End Sub
    Private Sub ArrangeInfo(openSave As String)
        Select Case openSave
            'this is done after the open subrutine
            Case "Open"
                _startInfo = Session("projects")._StartInfo
                _fieldsInfo1 = Session("projects")._fieldsInfo1
                _subprojectName = Session("projects")._subprojectName
                'this is done before the save subroutine
            Case "Save"
                Session("projects")._StartInfo = _startInfo
                Session("projects")._fieldsInfo1 = _fieldsInfo1
                Session("projects")._subprojectName = _subprojectName
        End Select
    End Sub
End Class