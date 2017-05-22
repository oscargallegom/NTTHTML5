Imports System.IO

Public Class Location
    Inherits System.Web.UI.Page
    'Private dc As New NTTDBDataContext
    Private intKey As Integer
    Private key As New Random()
    'Classes definition
    Private _project As New ProjectsData
    Private _startInfo As New StartInfo
    Private _controlValues As New List(Of ParmsData)
    Private _parmValues As New List(Of ParmsData)
    Private folder As String
    'Controls used in sitemaster page
    Private lblMessage As Label
    Private imgIcon As Image
    'state and county lists
    Private _states As Object
    Private _counties As New List(Of CodeAndName)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        folder = My.Computer.FileSystem.GetParentPath(Server.MapPath(""))
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
        If Request.QueryString.Count > 0 Then
            btnSave_Click()
            btnContinue_Click()
        End If

        If Not Page.Request.Params("__EVENTTARGET") Is Nothing Then
            Select Case True
                Case Page.Request.Params("__EVENTTARGET").Contains("btnSave")
                    btnSave_Click()
                Case Page.Request.Params("__EVENTTARGET").Contains("btnContinue")
                    btnContinue_Click()
            End Select
        End If
        AddProjectValues()

        If Not IsPostBack Then
            LoadStates()
            LoadCounties("")
            sctMap.Style.Item("display") = "none"
            sctStateCounty.Style.Item("display") = "none"
            sctInput.Style.Item("display") = "none"

            Select Case _startInfo.Status
                Case googleMaps
                    sctMap.Style.Item("display") = ""
                    btnMapSystem_Click()
                    Session.Item("MapStatus") = "Started"
                Case (stateCounty)
                    sctStateCounty.Style.Item("display") = ""
                Case userInput
                    sctInput.Style.Item("display") = ""
            End Select
        End If
    End Sub

    Public Sub LoadStates()
        Try
            _states = GetStates()
            ddlStates.DataTextField = "Name"
            ddlStates.DataValueField = "StateAbrev"
            ddlStates.DataSource = _states
            ddlStates.DataBind()

            If _startInfo.StateAbrev <> "" AndAlso Not (_startInfo.StateAbrev Is Nothing) Then
                ddlStates.Value = _startInfo.StateAbrev
            Else
                ddlStates.SelectedIndex = 0
            End If

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", "Error Getting States")
        End Try
    End Sub

    Private Sub LoadCounties(type As String)
        Dim newCounty As CodeAndName
        Dim item As ListItem

        Try
            Dim countyQuery = GetCounties()
            Dim found As Boolean = False
            _counties.Clear()
            ddlCounties.Items.Clear()
            For Each c In countyQuery
                newCounty = New CodeAndName
                item = New ListItem
                newCounty.Name = c.item("name")
                item.Text = c.item("name")
                newCounty.Code = c.item("StateAbrev").Trim & "-" & c.item("name")
                item.Value = newCounty.Code
                newCounty.CountyCode = c.item("Code")
                newCounty.Lat = c.item("Lat")
                newCounty.Lon = c.item("Long")
                newCounty.Abbreviation = c.item("StateAbrev")
                _counties.Add(newCounty)
                If c.item("StateAbrev").Trim = ddlStates.Items(ddlStates.SelectedIndex).Value.Trim Then
                    ddlCounties.Items.Add(item)
                    If Not (_startInfo.countyCode Is Nothing) AndAlso _startInfo.countyCode.Trim <> "" AndAlso _startInfo.countyCode.Trim = c.item("Code").Trim Then
                        ddlCounties.Value = newCounty.Code
                        found = True
                    End If
                End If
            Next

            If type = "ddlNo" Then Exit Sub
            txtCounty.Value = ddlCounties.Items(ddlCounties.SelectedIndex).Text

            If found = False Then ddlCounties.SelectedIndex = 0

            ddlAllCounties.DataTextField = "Name"
            ddlAllCounties.DataValueField = "Code"
            ddlAllCounties.DataSource = _counties
            ddlAllCounties.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ChangeLanguageContent()
        btnMapSystem.Text = cntDoc.Descendants("MappingSystem").Value
        btnStateCounty.Text = cntDoc.Descendants("StateCounty").Value
        btnUserInput.Text = cntDoc.Descendants("UserInput").Value
        lblGeneralLocation.Value = cntDoc.Descendants("GeneralLocation").Value
        lblSpecificLocation.Value = cntDoc.Descendants("SpecificLocation").Value
        lblState.Text = cntDoc.Descendants("States").Value
        lblCounties.Text = cntDoc.Descendants("Counties").Value
        lgdStateCounty.InnerHtml = cntDoc.Descendants("StateCounty").Value
        lgdUserInput.InnerHtml = cntDoc.Descendants("LocationInfoSubtitle").Value
        lgdStateCounty.InnerText = cntDoc.Descendants("StateCounty").Value
        lgdUserInput.InnerText = cntDoc.Descendants("LocationInfoSubtitle").Value
        btnSave.Text = cntDoc.Descendants("Save").Value
        btnContinue.Text = cntDoc.Descendants("Continue").Value
        'tool tips
        btnMapSystem.ToolTip = msgDoc.Descendants("ttMapingSystem").Value
        btnStateCounty.ToolTip = msgDoc.Descendants("ttStateCounty").Value
        btnUserInput.ToolTip = msgDoc.Descendants("ttUserInput").Value
        btnSave.ToolTip = msgDoc.Descendants("ttSaveAndContinue").Value
    End Sub

    Private Sub btnSave_Click()
        Dim message As String = "OK"
        Try
            If sctMap.Style.Item("display") = "" Then
                If Session.Item("MapStatus") = "Started" Then
                    MsgBox(msgDoc.Descendants("NoSubmit").Value, MsgBoxStyle.OkOnly, "Submit")
                    Exit Sub
                End If
            End If
            If _startInfo.StateName Is Nothing Then
                _startInfo.StateName = ""
            End If
            If _startInfo.StateAbrev Is Nothing Then
                _startInfo.currentWeatherPath = ""
                _startInfo.StateAbrev = ""
                _startInfo.countyName = ""
            End If
            Select Case True
                Case sctStateCounty.Style.Item("display") = ""
                    If _startInfo.StateAbrev.Trim <> ddlStates.Items(ddlStates.SelectedIndex).Value.Trim And _startInfo.countyName.Trim <> ddlCounties.Items(ddlCounties.SelectedIndex).Text.Trim Then
                        _startInfo.currentWeatherPath = ""
                    End If
                    _startInfo.StateAbrev = ddlStates.Items(ddlStates.SelectedIndex).Value
                    _startInfo.StateName = ddlStates.Items(ddlStates.SelectedIndex).Text
                    _startInfo.countyCode = ddlCounties.Items(ddlCounties.SelectedIndex).Value
                    If _counties.Count = 0 Then LoadCounties("ddlNo")
                    For Each County In _counties
                        If txtCounty.Value.Trim = County.Name.Trim And _startInfo.StateAbrev.Trim = County.Abbreviation.Trim Then
                            _startInfo.countyCode = County.CountyCode
                            _startInfo.countyName = County.Name
                            _startInfo.weatherLat = County.Lat
                            _startInfo.weatherLon = County.Lon
                            Exit For
                        End If
                    Next
                    _startInfo.Status = stateCounty
                    If _startInfo.stationWay = "" Then _startInfo.stationWay = station
                Case sctInput.Style.Item("display") = ""
                    'Validate  inputs
                    If txtGeneralLocation.Value = "" Then
                        txtGeneralLocation.Style.Item("backgroundColor") = "Red"
                        Throw New Exception("Location can not be blank. Please enter Location")
                        Exit Sub
                    End If
                    If txtspecificLocation.Value = "" Then
                        txtspecificLocation.Style.Item("backgroundColor") = "Red"
                        Throw New Exception("Location can not be blank. Please enter Location")
                        Exit Sub
                    End If
                    If txtLatitude.Value = "" Then
                        txtspecificLocation.Style.Item("backgroundColor") = "Red"
                        Throw New Exception("Latitude can not be blank. Please enter Location")
                        Exit Sub
                    End If
                    If txtLongitude.Value = "" Then
                        txtspecificLocation.Style.Item("backgroundColor") = "Red"
                        Throw New Exception("Longitude can not be blank. Please enter Location")
                        Exit Sub
                    End If
                    _startInfo.Status = userInput
                    If _startInfo.stationWay = "" Then _startInfo.stationWay = own
                    _startInfo.StateName = txtGeneralLocation.Value
                    _startInfo.countyName = txtspecificLocation.Value
                    _startInfo.weatherLat = txtLatitude.Value
                    _startInfo.weatherLon = txtLongitude.Value
                    _startInfo.WindCode = "999"
                    _startInfo.WindName = "CHINAG"
                    _startInfo.Wp1Code = "999"
                    _startInfo.Wp1Name = "CHINAG"
                Case sctMap.Style.Item("display") = ""
                    _startInfo.Status = googleMaps
                    If _startInfo.stationWay = "" Then _startInfo.stationWay = prism
            End Select
            If (ddlStates.Items.Count > 0 AndAlso _startInfo.StateName.Trim <> ddlStates.Items(ddlStates.SelectedIndex).Text.Trim) Or _controlValues.Count <= 0 Then
                LoadControlFile(_controlValues, _startInfo.StateAbrev)      'load control values (control.dat)
                LoadParmFile(_parmValues, _startInfo.StateAbrev)      'load parm values (parm.dat)
            End If

            ArrangeInfo("Save")
            showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("InformationSaved").Value)

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Error").Value & ex.Message)
        End Try
    End Sub

    Protected Sub btnContinue_Click()
        Response.Redirect("Weather.aspx", False)
    End Sub

    Protected Sub btnStateCounty_Click(sender As Object, e As System.EventArgs) Handles btnStateCounty.Click
        btnSave.Style.Item("display") = ""
        btnContinue.Style.Item("display") = ""
        _startInfo.stationWay = station
        _startInfo.Status = stateCounty
        sctMap.Style.Item("display") = "none"
        sctStateCounty.Style.Item("display") = ""
        sctInput.Style.Item("display") = "none"
    End Sub

    Protected Sub btnMapSystem_Click() Handles btnMapSystem.Click
        'save project to be able to restore after map is submitted.
        SaveProject(Session("userGuide"), _project)
        'hide save and county because is no needed. 
        btnSave.Style.Item("display") = "none"
        btnContinue.Style.Item("display") = "none"
        'change text for save and continue because there is nothing to save. googlemap program save all of the information
        _startInfo.stationWay = prism
        _startInfo.Status = googleMaps
        Dim params As String = NTTFilesFolder & Session("userGuide") & "&key=" & intKey
        intKey = key.Next
        'tdGoogleMap.InnerHtml = " src='" & "GoogleMap.aspx?" & params & "'></iframe>"
        'tdGoogleMap.InnerHtml = "<iframe name='embededframeBas' runat='server' id='embededframeBas' height='500' marginwidth='0' marginheight='0' width='1050' scrolling='yes' frameborder='no' src='" & "GoogleMap.aspx?" & params & "'></iframe>"
        embededframeBas.Attributes.Item("src") = "GoogleMap.aspx?" & params
        'Dim lc As LiteralControl = New LiteralControl
        'lc.Text = "<iframe name='embededframeBas' runat='server' id='embededframeBas' height='500' marginwidth='0' marginheight='0' width='1050' scrolling='yes' frameborder='no' src='" & "GoogleMap.aspx?" & params & "'></iframe>"
        'tdGoogleMap.Controls.Add(lc)
        sctMap.Style.Item("display") = ""
        sctStateCounty.Style.Item("display") = "none"
        sctInput.Style.Item("display") = "none"
    End Sub

    Protected Sub btnUserInput_Click(sender As Object, e As System.EventArgs) Handles btnUserInput.Click
        btnSave.Style.Item("display") = ""
        btnContinue.Style.Item("display") = ""
        _startInfo.stationWay = own
        _startInfo.Status = userInput
        sctMap.Style.Item("display") = "none"
        sctStateCounty.Style.Item("display") = "none"
        sctInput.Style.Item("display") = ""
    End Sub

    Public Sub LoadControlFile(_controlValues As List(Of ParmsData), state As String)
        Dim control As ParmsData
        Try
            'If (ddlStates.Items.Count > 0 AndAlso _startInfo.StateName.Trim <> ddlStates.Items(ddlStates.SelectedIndex).Text.Trim) Or _controlValues.Count <= 0 Then
            _controlValues.Clear()
            Dim controls = GetControlsDesc(state)
            'If Not controls.HasRows Then
            '    controls = GetControlsDesc("**")
            'End If
            For Each c In controls
                control = New ParmsData
                With control
                    .Name = c.item("Name")
                    .Code = c.item("Code")
                    .Value1 = c.item("Value")
                    .Range1 = c.item("Range1")
                    .Range2 = c.item("Range2")
                End With
                If Not _controlValues.Contains(control) Then
                    _controlValues.Add(control)
                End If
            Next
            'End If

        Catch ex As System.Exception
        End Try
    End Sub

    Public Sub LoadParmFile(_parmValues As List(Of ParmsData), state As String)
        Dim parm As ParmsData
        Try
            'If (ddlStates.Items.Count > 0 AndAlso _startInfo.StateName.Trim <> ddlStates.Items(ddlStates.SelectedIndex).Text.Trim) Or _parmValues.Count <= 0 Then
            _parmValues.Clear()
            Dim parms = GetParmDesc(state, 0)
            'If Not parms.HasRows Then
            '    parms = GetParmDesc("**")
            'End If
            For Each c In parms
                parm = New ParmsData
                With parm
                    .Name = c.item("Name")
                    .Code = c.item("Code")
                    .Value1 = c.item("Value")
                    .Range1 = c.item("Range1")
                    .Range2 = c.item("Range2")
                End With
                If Not _parmValues.Contains(parm) Then
                    _parmValues.Add(parm)
                End If
            Next
            'End If

        Catch ex As System.Exception
        End Try
    End Sub

    Private Sub AddProjectValues()
        Select Case _startInfo.Status
            Case stateCounty
                If _startInfo.StateAbrev <> "" AndAlso Not (_startInfo.StateAbrev Is Nothing) Then
                    ddlStates.Value = _startInfo.StateAbrev
                Else
                    ddlStates.SelectedIndex = 0
                End If
            Case userInput
                txtGeneralLocation.Value = _startInfo.StateName
                txtspecificLocation.Value = _startInfo.countyName
                txtLatitude.Value = _startInfo.weatherLat
                txtLongitude.Value = _startInfo.weatherLon
        End Select

    End Sub
    Private Sub ArrangeInfo(openSave As String)
        Select Case openSave
            'this is done after the open subrutine
            Case "Open"
                _project = Session("projects")
                _startInfo = Session("projects")._StartInfo
                _parmValues = Session("projects")._parmValues
                _controlValues = Session("projects")._controlValues
            Case "Save"
                Session("projects")._StartInfo = _startInfo
                Session("projects")._parmValues = _parmValues
                Session("projects")._controlValues = _controlValues
        End Select
    End Sub

End Class