﻿Imports HUCal
Imports System.IO
Imports System.Security.AccessControl

Public Class Simulation
    Inherits System.Web.UI.Page
    'Private db As New NTTDBDataContext

    'Controls used in sitemaster page
    Private lblMessage As Label
    Private imgIcon As Image

    'general variables
    Private typeOfSimulation As UShort = 0
    Private simulationsCount As UShort  'keep trak of simualtions
    Private currentSubproject As UShort
    Private Const coma As Char = ", "
    Private sAPEXBasLoc As String
    Private sAPEXBasLoc1 As String = NTTFilesFolder & "APEX1"
    Private bProcessAPEXExited As Boolean = False
    'Fertilizer variables
    Private _CurrentNutrients As List(Of CurrentNutrients)
    Private newFertLine As New List(Of String)
    Private changeFertLine As New List(Of String)
    'operations variables
    Private depthAnt() As String = Nothing
    Private opers() As Integer = Nothing
    Private opcsFile = New List(Of String)
    Private riceCrop As Boolean = False 'control if rice crop is selected to add irrigation in subarae file.
    Private numOfDepths As UShort = 0
    Private fertCode As Short = 79
    Private cropsList As New List(Of String)
    Private changeTillDepth As New List(Of String)
    Private changeTillHE As New List(Of String)
    Private mixedCrops As Short = -1
    'soils variables
    Private lastSoil, lastSoilSub As UShort
    Private totalArea As Single
    Private soilList, soilInfo As New List(Of String)
    Private satCondOut As Single = 0
    'subareas variable
    Private lastSubarea, lastOwner As UShort
    Private subareaFile = New List(Of String)
    Public Class ResultsACY
        Public subArea As Short
        Public year As Short
        Public cropName As String
        Public cropYield As Single
    End Class    'Grazing variables
    Private lastHerd As UShort
    'Private herdList As New System.Collections.ObjectModel.ObservableCollection(Of String)
    Private herdList As New List(Of String)
    Private herdFile As String = String.Empty
    Private conversionUnit As Single
    Private GrazingB As Boolean = False
    Private animalUnits As List(Of AnimalUnitsData) = New List(Of AnimalUnitsData)
    Private animalB As String
    'site variables
    Private siteFile As String = String.Empty
    'results
    Private nAPEXYears As Short = 0
    Private _result As ScenariosData.APEXResults
    Private Structure CurrentNutrients
        Public code As UShort
        Public no3 As Single
        Public po4 As Single
        Public orgn As Single
        Public orgp As Single
    End Structure
    'FEM TAbles lists
    Private equipmentList As New List(Of String)
    Private feedList As New List(Of String)
    Private structureList As New List(Of String)
    Private otherInputList As New List(Of String)
    Private femList As New List(Of String)
    Private FEMResults As FEMData
    'Weather information
    Private _weather As String = "E:\weather"
    'Private windLocation As String = weather & "\wndFile"
    Private windLocation As String = windFiles
    'Private wp1Location As String = weather & "\wp1File"
    Private wp1Location As String = wp1Files
    Property Weather() As String
        Get
            Return _weather
        End Get
        Set(ByVal value As String)
            If System.Net.Dns.GetHostName = "T-NN" Then
                _weather = value
            Else
                _weather = "C:\weather"
            End If
        End Set
    End Property
    Private totalyears As UShort = 0
    'Classes definition
    'Private _projects As New ProjectsData
    Private _startInfo As New StartInfo
    Private _fieldsInfo1 As New List(Of FieldsData)
    Private _sitesInfo As New List(Of SiteData)
    Private _parmValues As New List(Of ParmsData)
    Private _controlValues As New List(Of ParmsData)
    Private _crops As New List(Of CropsData)
    Private _scenariosToRun As New List(Of ScenariosToRun)
    Public _bmpList As New List(Of String)
    Private _subprojectName As New List(Of SubprojectNameData)
    Public _equipmentTemp As List(Of EquipmentData)
    Public _feedTemp As List(Of FeedData)
    Public _structureTemp As List(Of StructureData)
    Public _otherTemp As List(Of OtherData)
    'retrieve the currentfieldnumber
    Private currentFieldNumber As Short = 0
    Private currentScenarioNumber As Short = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("EPage") = False Then btnRunEconomic.Style.Item("display") = "none"
        If Session("userGuide") = "" Then
            Response.Redirect("~/Default.aspx", False)
            'Response.Redirect(System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName).HostName & "/NTTG2
            'Response.Redirect("Default.aspx", False)
            Exit Sub
        End If
        lblMessage = New Label
        lblMessage = CType(Master.FindControl("lblMessage"), Label)
        imgIcon = New Image
        imgIcon = CType(Master.FindControl("imgIcon"), Image)
        lblMessage.Style.Item("display") = "none"
        imgIcon.Style.Item("display") = "none"
        lblMessage.Text = ""

        sAPEXBasLoc = NTTFilesFolder & "APEX1" & Session("userGuide").ToString

        Select Case Page.Request.Params("__EVENTARGUMENT")
            Case english
                Session("Language") = english
            Case spanish
                Session("Language") = spanish
        End Select

        openXMLLanguagesFile()
        ChangeLanguageContent()

        ArrangeInfo("Open")
        If ddlField.Items.Count > 0 Then currentFieldNumber = ddlField.SelectedIndex
        If ddlScenario.Items.Count > 0 Then currentScenarioNumber = ddlScenario.SelectedIndex

        Select Case True
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("btnRunEnvironment")
                APEXFolders()
                btnRunEnvironment_ServerClick()
        End Select

        CheckScenariosAdded()

        If Not IsPostBack Then
            currentFieldNumber = Session("currentFieldNumber")
            ddlField.SelectedIndex = currentFieldNumber
            currentScenarioNumber = Session("currentScenarioNumber")
            ddlScenario.SelectedIndex = currentScenarioNumber
            LoadTypes()
            LoadFields(ddlField, _fieldsInfo1, currentFieldNumber)
            LoadScenarios(ddlScenario, _fieldsInfo1(currentFieldNumber)._scenariosInfo, currentScenarioNumber)
            LoadSubprojectNames()
            For i = 0 To ddlScenario.Items.Count - 1
                AddScenarioToRun(ddlField.SelectedIndex, i, ddlField.SelectedItem.Text, ddlScenario.Items(i).Text)
                Session("scenariosToRun") = _scenariosToRun
            Next
        End If
        If _crops.Count <= 0 Then
            _crops = Session("crops")
        End If
    End Sub

    Private Sub TakeScenariosToRun()
        Dim _scenarioToAdd As ScenariosToRun
        For i = 0 To gvSimulations.Rows.Count - 1
            _scenarioToAdd = New ScenariosToRun
            _scenarioToAdd.Date1 = gvSimulations.Rows(i).Cells(9).Text
            _scenarioToAdd.Field = gvSimulations.Rows(i).Cells(1).Text
            _scenarioToAdd._fieldIndex = gvSimulations.Rows(i).Cells(11).Text
            _scenarioToAdd.Scenario = gvSimulations.Rows(i).Cells(2).Text
            _scenarioToAdd._ScenarioIndex = gvSimulations.Rows(i).Cells(12).Text
            _scenariosToRun.Add(_scenarioToAdd)
        Next
        Session("scenariosToRun") = _scenariosToRun
    End Sub

    Private Sub APEXFolders()
        Dim directoryFiles(), currentFiles() As String
        Dim textFile As String

        'Create APEX folder to run the current simulation
        If Directory.Exists(sAPEXBasLoc) = True Then
            Directory.Delete(sAPEXBasLoc, True)
        End If
        Directory.CreateDirectory(sAPEXBasLoc)
        directoryFiles = Directory.GetFiles(sAPEXBasLoc1)
        For Each textFile In directoryFiles
            currentFiles = Split(textFile, "\", 6)
            File.Copy(textFile, sAPEXBasLoc & "\" & currentFiles(3), True)
        Next
        If Directory.Exists(sAPEXBasLoc1 & "_" & _startInfo.StateAbrev.Trim) Then
            directoryFiles = Directory.GetFiles(sAPEXBasLoc1 & "_" & _startInfo.StateAbrev.Trim)
            For Each textFile In directoryFiles
                currentFiles = Split(textFile, "\", 6)
                File.Copy(textFile, sAPEXBasLoc & "\" & currentFiles(3), True)
            Next
        End If

        'Create FEM folder 
    End Sub

    Public Sub LoadSubprojectNames()
        Try
            If _subprojectName.Count = 0 Then Exit Sub
            ddlSubproject.DataSource = _subprojectName
            ddlSubproject.DataTextField = "Name"
            ddlSubproject.DataValueField = "Name"
            ddlSubproject.DataBind()
            ddlSubproject.SelectedIndex = 0
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Private Sub CheckScenariosAdded()
        For i = 0 To _scenariosToRun.Count - 1
            If _StartInfo.projectName <> String.Empty Then
                _scenariosToRun(i).Project = "~/Resources/GoIcon.jpg"
            Else
                _scenariosToRun(i).Project = "~/Resources/StopIcon.jpg"
            End If

            If _StartInfo.Status <> String.Empty Then
                _scenariosToRun(i).Location = "~/Resources/GoIcon.jpg"
            Else
                _scenariosToRun(i).Location = "~/Resources/StopIcon.jpg"
            End If

            If _StartInfo.stationYears > 0 Then
                _scenariosToRun(i).Weather = "~/Resources/GoIcon.jpg"
            Else
                _scenariosToRun(i).Weather = "~/Resources/StopIcon.jpg"
            End If

            If _scenariosToRun(i).Scenario <> "Subproject" Then
                If _fieldsInfo1(_scenariosToRun(i).FieldIndex).Area > 0 Then
                    _scenariosToRun(i).Fields = "~/Resources/GoIcon.jpg"
                Else
                    _scenariosToRun(i).Fields = "~/Resources/StopIcon.jpg"
                End If

                Dim j As Short = _scenariosToRun(i).FieldIndex
                If _fieldsInfo1(j)._soilsInfo.Count > 0 Then
                    For l = 0 To _fieldsInfo1(j)._soilsInfo.Where(Function(x) x.Selected).Count - 1
                        If _fieldsInfo1(j)._soilsInfo(l)._layersInfo.Count > 0 Then
                            For Each record In _fieldsInfo1(j)._soilsInfo(l)._layersInfo
                                If record.Depth <= 0 Or record.BD < BDMin Or record.BD > BDMax Or record.Sand < SandMin Or record.Sand > SandMax Or record.Silt < SiltMin Or record.Silt > SiltMax _
                                    Or record.SoilP < SoilPMin Or record.SoilP > SoilPMax Or record.OM < OMMin Or record.OM > OMMax Or record.PH < PHMin Or record.PH > PHMax Then
                                    _scenariosToRun(i).Soils = "~/Resources/StopIcon.jpg"
                                    Exit For
                                End If
                            Next
                            _scenariosToRun(i).Soils = "~/Resources/GoIcon.jpg"
                        Else
                            _scenariosToRun(i).Soils = "~/Resources/StopIcon.jpg"
                        End If
                    Next
                Else
                    _scenariosToRun(i).Soils = "~/Resources/StopIcon.jpg"
                End If
                If _fieldsInfo1(j)._scenariosInfo.Count > 0 Then
                    For l = 0 To _fieldsInfo1(j)._scenariosInfo.Count - 1
                        If _fieldsInfo1(j)._scenariosInfo(l)._operationsInfo.Count > 0 Then
                            _scenariosToRun(i).Management = "~/Resources/GoIcon.jpg"
                        Else
                            _scenariosToRun(i).Management = "~/Resources/StopIcon.jpg"
                        End If
                    Next
                Else
                    _scenariosToRun(i).Management = "~/Resources/StopIcon.jpg"
                End If
            Else
                'todo assume that fields and soils are ok. that is because the user can create subprojects. Check to see how to really validate
                _scenariosToRun(i).Fields = "~/Resources/GoIcon.jpg"
                _scenariosToRun(i).Soils = "~/Resources/GoIcon.jpg"
                _scenariosToRun(i).Management = "~/Resources/GoIcon.jpg"
            End If
            gvSimulations.DataSource = _scenariosToRun
            gvSimulations.DataBind()
        Next
    End Sub

    Private Sub LoadTypes()
        If ddlType.Items.Count <= 0 Then
            ddlType.Items.Add(cntDoc.Descendants("FieldScenario").Value)
            ddlType.Items.Add(cntDoc.Descendants("Subproject").Value)
        End If
        ddlType.SelectedIndex = 0
        ddlType_ServerChange(ddlType, New System.EventArgs)
    End Sub

    Private Sub ChangeLanguageContent()
        lblType.InnerText = cntDoc.Descendants("SelectTypeHeading").Value
        lblField.InnerText = cntDoc.Descendants("SelectFieldHeading").Value
        lblScenario.InnerText = cntDoc.Descendants("SelectScenarioHeading").Value
        btnSaveContinue.Text = cntDoc.Descendants("_Continue").Value
        btnAddSimulations.Value = cntDoc.Descendants("AddFieldScenarioToRun").Value
        btnAddAll.Value = cntDoc.Descendants("AddAll").Value
        btnRemoveAll.Value = cntDoc.Descendants("RemoveAll").Value
        btnRunEnvironment.Value = cntDoc.Descendants("RunSelected").Value
        btnRunEconomic.Value = cntDoc.Descendants("RunEconomics").Value
        lblTypes.InnerText = cntDoc.Descendants("SelectTypeHeading").Value
        lblScenarios.InnerHtml = cntDoc.Descendants("SelectSpecific").Value
        lblAll.InnerHtml = cntDoc.Descendants("AddRemoveAll").Value
        lblRunSimulation.InnerHtml = cntDoc.Descendants("ListScenariosSimulate").Value

        'gvSimulations.Columns(0).HeaderText = cntDoc.Descendants("Delete").Value
        gvSimulations.Columns(1).HeaderText = cntDoc.Descendants("Field").Value
        gvSimulations.Columns(2).HeaderText = cntDoc.Descendants("Scenario").Value
        gvSimulations.Columns(3).HeaderText = cntDoc.Descendants("Project").Value
        gvSimulations.Columns(4).HeaderText = cntDoc.Descendants("Location").Value
        gvSimulations.Columns(5).HeaderText = cntDoc.Descendants("Weather").Value
        gvSimulations.Columns(6).HeaderText = cntDoc.Descendants("Fields").Value
        gvSimulations.Columns(7).HeaderText = cntDoc.Descendants("Soil").Value
        gvSimulations.Columns(8).HeaderText = cntDoc.Descendants("Management").Value
        gvSimulations.Columns(9).HeaderText = cntDoc.Descendants("LastSimulation").Value
        gvSimulations.Columns(10).HeaderText = cntDoc.Descendants("Comments").Value
    End Sub

    Protected Sub btnSaveContinue_Click(sender As Object, e As EventArgs) Handles btnSaveContinue.Click
        Response.Redirect("Results.aspx", False)
    End Sub

    Protected Sub gvSimulation_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        _scenariosToRun.RemoveAt(e.RowIndex)
        Session("scenariosToRun") = _scenariosToRun
        gvSimulations.DataSource = _scenariosToRun
        gvSimulations.DataBind()
    End Sub

    Private Sub ddlField_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlField.SelectedIndexChanged
        currentFieldNumber = ddlField.SelectedIndex
        Session("currentFieldNumber") = currentFieldNumber
        If _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count > 0 Then
            currentScenarioNumber = 0
        Else
            currentScenarioNumber = -1
        End If
        LoadScenarios(ddlScenario, _fieldsInfo1(currentFieldNumber)._scenariosInfo, currentScenarioNumber)
        _scenariosToRun.Clear()
        For i = 0 To ddlScenario.Items.Count - 1
            AddScenarioToRun(ddlField.SelectedIndex, i, ddlField.SelectedItem.Text, ddlScenario.Items(i).Text)
            Session("scenariosToRun") = _scenariosToRun
        Next

    End Sub

    Protected Sub btnAddSimulations_Click(sender As Object, e As EventArgs) Handles btnAddSimulations.ServerClick
        If ddlField.SelectedIndex < 0 Then
            'If ddlField.SelectedIndex < 0 Or ddlScenario.SelectedIndex < 0 Then
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("AddFieldError").Value)
            Exit Sub
        End If
        AddScenarioToRun(ddlField.SelectedIndex, ddlScenario.SelectedIndex, ddlField.SelectedItem.Text, ddlScenario.SelectedItem.Text)
        Session("scenariosToRun") = _scenariosToRun
    End Sub

    Private Sub AddScenarioToRun(fieldIndex As UShort, scenarioIndex As UShort, fieldName As String, scenarioName As String)
        Try
            showMessage(lblMessage, imgIcon, "White", "", "")
            'If ddlField.Items.Count <= 0 Or ddlScenario.Items.Count <= 0 Then
            If ddlField.Items.Count <= 0 Then
                Throw New Global.System.Exception(msgDoc.Descendants("SimulationNoFieldNoScenario").Value)
                Exit Sub
            End If

            For i = 0 To _scenariosToRun.Count - 1
                If _scenariosToRun(i).Field = ddlField.SelectedItem.Text And _scenariosToRun(i)._fieldIndex = fieldIndex And _
                _scenariosToRun(i).Scenario = ddlScenario.SelectedItem.Text And _scenariosToRun(i)._ScenarioIndex = scenarioIndex Then
                    Throw New Global.System.Exception(msgDoc.Descendants("Field_ScenarioSelected").Value)
                    Exit Sub
                End If
            Next

            Dim scenario As New ScenariosToRun

            scenario._fieldIndex = fieldIndex
            scenario._ScenarioIndex = scenarioIndex
            scenario.Field = fieldName
            scenario.Scenario = scenarioName

            If _startInfo.projectName <> String.Empty Then
                scenario.Project = "~/Resources/GoIcon.jpg"
            Else
                scenario.Project = "~/Resources/StopIcon.jpg"
            End If

            If _startInfo.Status <> String.Empty Then
                scenario.Location = "~/Resources/GoIcon.jpg"
            Else
                scenario.Location = "~/Resources/StopIcon.jpg"
            End If

            If _startInfo.stationYears > 0 Then
                scenario.Weather = "~/Resources/GoIcon.jpg"
            Else
                scenario.Weather = "~/Resources/StopIcon.jpg"
            End If

            If _fieldsInfo1(scenario._fieldIndex).Area > 0 Then
                scenario.Fields = "~/Resources/GoIcon.jpg"
            Else
                scenario.Fields = "~/Resources/StopIcon.jpg"
            End If

            If _fieldsInfo1(scenario._fieldIndex)._soilsInfo.Count > 0 Then
                For i = 0 To _fieldsInfo1(scenario._fieldIndex)._soilsInfo.Where(Function(x) x.Selected).Count - 1  'use just the soils selected in soils page
                    If _fieldsInfo1(scenario._fieldIndex)._soilsInfo(i)._layersInfo.Count > 0 Then
                        For Each record In _fieldsInfo1(scenario._fieldIndex)._soilsInfo(i)._layersInfo  'validate layers
                            If record.Depth <= 0 Or record.BD < BDMin Or record.BD > BDMax Or record.Sand < SandMin Or record.Sand > SandMax Or record.Silt < SiltMin Or record.Silt > SiltMax _
                                Or record.SoilP < SoilPMin Or record.SoilP > SoilPMax Or record.OM < OMMin Or record.OM > OMMax Or record.PH < PHMin Or record.PH > PHMax Then
                                scenario.Soils = "~/Resources/StopIcon.jpg"
                                Exit For
                            End If
                        Next
                        scenario.Soils = "~/Resources/GoIcon.jpg"
                    Else
                        scenario.Soils = "~/Resources/StopIcon.jpg"
                    End If
                Next
            Else
                scenario.Soils = "~/Resources/StopIcon.jpg"
            End If

            If _fieldsInfo1(scenario._fieldIndex)._scenariosInfo.Count > 0 Then
                If _fieldsInfo1(scenario._fieldIndex)._scenariosInfo(scenario._ScenarioIndex)._operationsInfo.Count > 0 Then
                    scenario.Management = "~/Resources/GoIcon.jpg"
                Else
                    scenario.Management = "~/Resources/StopIcon.jpg"
                End If
            Else
                scenario.Management = "~/Resources/StopIcon.jpg"
            End If
            scenario.Date1 = _fieldsInfo1(scenario._fieldIndex)._scenariosInfo(scenario._ScenarioIndex)._results.lastSimulation
            _scenariosToRun.Add(scenario)

            gvSimulations.DataSource = _scenariosToRun
            gvSimulations.DataBind()

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("AddFieldError").Value & ex.Message)
        Finally
        End Try
    End Sub

    'Private Sub ddlType_ServerChange(sender As Object, e As System.EventArgs) Handles ddlType.ServerChange
    '    btnAddAll.Attributes.Item("display") = "none"
    '    btnRemoveAll.Attributes.Item("display") = "none"
    '    lblField.Attributes.Item("display") = "none"
    '    lblScenario.Attributes.Item("display") = "none"
    '    lblSubproject.Attributes.Item("display") = "none"
    '    btnAddSimulations.Attributes.Item("display") = "none"
    '    btnAddSubproject.Attributes.Item("display") = "none"
    '    ddlField.Attributes.Item("display") = "none"
    '    ddlScenario.Attributes.Item("display") = "none"
    '    ddlSubproject.Attributes.Item("display") = "none"
    '    If ddlType.SelectedIndex = 0 Then
    '        ddlField.Attributes.Item("display") = "none"
    '        ddlScenario.Attributes.Item("display") = "none"
    '        lblField.Attributes.Item("display") = "none"
    '        lblScenario.Attributes.Item("display") = "none"
    '        btnAddSimulations.Attributes.Item("display") = "none"
    '        btnAddAll.Attributes.Item("display") = "none"
    '        btnRemoveAll.Attributes.Item("display") = "none"
    '    Else
    '        lblSubproject.Attributes.Item("display") = "none"
    '        btnAddSubproject.Attributes.Item("display") = "none"
    '        ddlSubproject.Attributes.Item("display") = "none"
    '    End If
    'End Sub

    Private Sub btnAddAll_ServerClick(sender As Object, e As System.EventArgs) Handles btnAddAll.ServerClick
        _scenariosToRun.Clear()
        If ddlType.SelectedIndex = 0 Then
            For i = 0 To _fieldsInfo1.Count - 1
                For j = 0 To _fieldsInfo1(i)._scenariosInfo.Count - 1
                    AddScenarioToRun(i, j, _fieldsInfo1(i).Name, _fieldsInfo1(i)._scenariosInfo(j).Name)
                Next
            Next
        Else
            For i = 0 To ddlSubproject.Items.Count - 1
                ddlSubproject.SelectedIndex = i
                btnAddSubproject_Click()
                'AddScenarioToRun(i, 0, ddlSubproject.Items(i).Text, "Subproject")
            Next
        End If

        Session("scenariosToRun") = _scenariosToRun
        gvSimulations.DataBind()
    End Sub

    Private Sub btnRemoveAll_ServerClick(sender As Object, e As System.EventArgs) Handles btnRemoveAll.ServerClick
        _scenariosToRun.Clear()
        Session("scenariosToRun") = _scenariosToRun
        gvSimulations.DataBind()
    End Sub

    Protected Sub btnAddSubproject_Click()
        Try
            showMessage(lblMessage, imgIcon, "White", "", "")
            If ddlSubproject.Items.Count <= 0 Then
                Throw New System.Exception(msgDoc.Descendants("SimulationNoSubproject").Value)
                Exit Sub
            End If

            For i = 0 To _scenariosToRun.Count - 1
                If _scenariosToRun(i).Field = ddlSubproject.SelectedItem.Text And _scenariosToRun(i)._fieldIndex = ddlSubproject.SelectedIndex And _
                    _scenariosToRun(i).Scenario = "Subproject" And _scenariosToRun(i)._ScenarioIndex = 0 Then
                    Throw New System.Exception(msgDoc.Descendants("SubprojectSelected").Value)
                    Exit Sub
                End If
            Next

            Dim scenario As ScenariosToRun

            For i = 0 To ddlSubproject.Items.Count - 1
                scenario = New ScenariosToRun
                scenario.Field = ddlSubproject.Items(i).Text
                scenario._fieldIndex = i
                scenario.Scenario = "Subproject"
                scenario._ScenarioIndex = 0
                If _startInfo.projectName <> String.Empty Then
                    scenario.Project = "~/Resources/GoIcon.jpg"
                Else
                    scenario.Project = "~/Resources/StopIcon.jpg"
                End If

                If _startInfo.stationWay <> String.Empty Then
                    scenario.Location = "~/Resources/GoIcon.jpg"
                Else
                    scenario.Location = "/NTTSLVer102013;component/Images/StopIcon.jpg"
                End If

                If _startInfo.stationYears > 0 Then
                    scenario.Weather = "~/Resources/GoIcon.jpg"
                Else
                    scenario.Weather = "~/Resources/StopIcon.jpg"
                End If
                'todo assume that fields, soils, and management are ok. that is because the user can create subprojects means those are already there. Check to see how to really validate
                scenario.Fields = "~/Resources/GoIcon.jpg"
                scenario.Soils = "~/Resources/GoIcon.jpg"
                scenario.Management = "~/Resources/GoIcon.jpg"
                scenario.Date1 = _subprojectName(i)._results.lastSimulation
                _scenariosToRun.Add(scenario)
            Next

            gvSimulations.DataSource = _scenariosToRun
            gvSimulations.DataBind()

            Session("scenariosToRun") = _scenariosToRun

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Private Sub btnRunEnvironment_ServerClick()
        Try
            typeOfSimulation = 0
            If _scenariosToRun.Count <= 0 Then
                showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("NoScenarioSelected").Value)
                Exit Sub
            Else
                For i = 0 To _scenariosToRun.Count - 1
                    simulationsCount = i
                    Dim chkSelected As HtmlInputCheckBox
                    chkSelected = gvSimulations.Rows(i).FindControl("chkSelected")
                    If chkSelected.Checked = False Then
                        Continue For
                    End If
                    With _scenariosToRun(i)
                        If .Project.Contains("Stop") Or .Location.Contains("Stop") Or .Weather.Contains("Stop") Or .Fields.Contains("Stop") Or .Soils.Contains("Stop") Or .Management.Contains("Stop") Then
                            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("AddFieldError").Value)
                            Continue For
                        End If
                        createWeatherFile()
                        processSimulations()
                    End With
                Next
            End If

            If _result.message <> "OK" Then
                Throw New Global.System.Exception("Error running APEX - " & _result.message)
            End If

        Catch ex As System.Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Private Sub btnRunEconomic_ServerClick(sender As Object, e As System.EventArgs) Handles btnRunEconomic.ServerClick
        typeOfSimulation = 1
        If _scenariosToRun.Count <= 0 Then
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("NoScenarioSelected").Value)
            Exit Sub
        End If
        simulationsCount = 0
        processSimulations()
    End Sub

    Private Sub processSimulations()
        Dim fieldsToSimulate As UShort = 0
        Dim i As UShort = 0

        _CurrentNutrients = New List(Of CurrentNutrients)

        newFertLine.Clear()
        changeFertLine.Clear()
        changeTillDepth.Clear()
        'initilize APEX variables
        soilList.Clear()
        soilInfo.Clear()
        siteFile = String.Empty
        subareaFile.Clear()
        herdList.Clear()
        opcsFile.Clear()
        femList.Clear()
        lastSubarea = 0
        lastSoil = 0
        lastSoilSub = 0
        lastHerd = 0
        lastOwner = 0
        numOfDepths = 0
        depthAnt = Nothing
        opers = Nothing

        currentSubproject = 0
        showMessage(lblMessage, imgIcon, "Orange", "WarningIcon.jpg", msgDoc.Descendants("SimulationWaitMsg").Value.Replace("First", _scenariosToRun(simulationsCount).Field).Replace("Second", _scenariosToRun(simulationsCount).Scenario))

        If _scenariosToRun(simulationsCount).Scenario = "Subproject" Then
            For i = 0 To _subprojectName.Count - 1
                If _subprojectName(i).Name.Trim = _scenariosToRun(simulationsCount).Field.Trim Then
                    currentSubproject = i
                    For j = 0 To _subprojectName(i)._subproject.Count - 1
                        currentFieldNumber = FindFieldIndex(_subprojectName(i)._subproject(j).Field)
                        currentScenarioNumber = FindScenarioIndex(_subprojectName(i)._subproject(j).Scenario)
                        CreateSimulationFiles(j + 1, _scenariosToRun(simulationsCount).Field)
                    Next
                    Exit For
                End If
            Next
        Else

            currentFieldNumber = _scenariosToRun(simulationsCount)._fieldIndex
            currentScenarioNumber = _scenariosToRun(simulationsCount)._ScenarioIndex
            CreateSimulationFiles(1, _scenariosToRun(simulationsCount).Scenario)
        End If

        TransferControls()

        fsetSimulation.Style.Item("display") = "none"
        fsetListSimulations.Style.Item("display") = ""
    End Sub

    Public Sub TransferControls()
        Dim result As String = "OK"
        Try
            result = CreateParmFile()
            If result <> "OK" Then
                Throw New Global.System.Exception("Error creating parm file - " & result)
            End If

            result = CreateControlFile()
            If result <> "OK" Then
                Throw New Global.System.Exception("Error creating control file - " & result)
            End If

            'add new fertilizer operations to fert file
            File.Copy(sAPEXBasLoc & "\ferts.dat", sAPEXBasLoc & "\fert.dat", True)
            If newFertLine.Count > 0 Then
                Dim swFert As StreamWriter = New StreamWriter(sAPEXBasLoc & "\fert.dat", True)
                For i = 0 To newFertLine.Count - 1
                    swFert.WriteLine(newFertLine(i))
                Next
                swFert.Close()
                swFert.Dispose()
                swFert = Nothing
            End If

            'modify a fertilize code for grazing
            Dim temp, fertCode1 As String
            If changeFertLine.Count > 0 Then
                Dim srFile As StreamReader = New StreamReader(sAPEXBasLoc & "\fert.dat")
                Dim swFile1 As StreamWriter = New StreamWriter(sAPEXBasLoc & "\fertNew.dat", False)
                Dim found As Boolean = False

                Do While srFile.EndOfStream <> True
                    temp = srFile.ReadLine
                    If temp.Trim = "" Then Exit Do
                    fertCode1 = temp.Substring(0, 5).Trim
                    found = False
                    For i = 0 To changeFertLine.Count - 1
                        If fertCode1.Trim = changeFertLine(i).Substring(0, 5).Trim Then
                            swFile1.WriteLine(changeFertLine(i))
                            found = True
                            Exit For
                        End If
                    Next
                    If Not found Then
                        swFile1.WriteLine(temp)
                    End If
                Loop
                srFile.Close()
                swFile1.Close()
                swFile1 = Nothing
                srFile = Nothing
                File.Copy(sAPEXBasLoc & "\fertNew.dat", sAPEXBasLoc & "\fert.dat", True)
            End If

            'create herd file if there was grazing operations.
            If Not herdList Is Nothing AndAlso herdList.Count > 0 Then
                Dim swHerd As StreamWriter = New StreamWriter(sAPEXBasLoc & "\herd.dat")
                For i = 0 To herdList.Count - 1
                    swHerd.WriteLine(herdList(i))
                Next
                swHerd.WriteLine("")
                swHerd.Close()
                swHerd.Dispose()
                swHerd = Nothing
            End If

            result = APEXRun()

            createSiteFile()

        Catch ex As System.Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try

    End Sub

    Function APEXRun()
        Dim swFile1 As StreamWriter = Nothing

        Try
            swFile1 = New StreamWriter(File.Create(sAPEXBasLoc1 & Session("userGuide") & "\apexrun.dat"))
            swFile1.WriteLine("APEX001" & "1".PadLeft(4) & _StartInfo.WindCode.PadLeft(4) & _StartInfo.Wp1Code.PadLeft(4) & "1".PadLeft(4) & "0".PadLeft(4) & "0".PadLeft(4))
            swFile1.Close()
            swFile1.Dispose()
            swFile1 = Nothing
            Return "OK"

        Catch ex As System.Exception
            Return ex.Message
        Finally
            If Not swFile1 Is Nothing Then
                swFile1.Close()
                swFile1.Dispose()
                swFile1 = Nothing
            End If
        End Try

    End Function

    Private Function FindFieldIndex(fieldName As String) As Short
        Dim i As UShort = 0

        For i = 0 To _fieldsInfo1.Count - 1
            If _fieldsInfo1(i).Name.Trim = fieldName.Trim Then
                Return i
            End If
        Next
        Return -1
    End Function

    Private Function FindScenarioIndex(ScenarioName As String) As Short
        For i = 0 To _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count - 1
            If _fieldsInfo1(currentFieldNumber)._scenariosInfo(i).Name.Trim = ScenarioName.Trim Then
                Return i
            End If
        Next
        Return -1
    End Function

    Private Sub CreateSimulationFiles(fields As UShort, scenario As String)
        createSoils()
        CreateSubarea(fields)
    End Sub

    Private Sub createSoils()
        Dim APEXStrings1 As New System.Text.StringBuilder
        Dim ds1 As IEnumerable(Of LayersData)
        'Dim dr As IEnumerable(Of SoilsData)
        Dim soilSlope As Single
        Dim series As String = "" : Dim horizgen As String = "" : Dim horizdesc1 As String = "" : Dim horizdesc2 As String = ""
        Dim i As Integer
        '// Structure for soil layers
        'Dim maxDepth(10) As Single
        Dim Depth(10) As Single
        Dim BD(10) As Single
        Dim UW(10) As Single
        Dim FC(10) As Single
        Dim Sand(10) As Single
        Dim Silt(10) As Single
        Dim WN(10) As Single
        Dim PH(10) As Single
        Dim SMB(10) As Single
        Dim WOC(10) As Single
        Dim CAC(10) As Single
        Dim CEC(10) As Single
        Dim ROK(10) As Single
        Dim CNDS(10) As Single
        Dim SSF(10) As Single
        Dim RSD(10) As Single
        Dim BDD(10) As Single
        Dim PSP(10) As Single
        Dim SATC(10) As Single
        Dim HCL(10) As Single
        Dim WPO(10) As Single
        Dim CPRV(10) As Single
        Dim CPRH(10) As Single
        Dim RT(10) As Single
        Dim layerNumber, layers, initialLayer As Byte
        Dim ssaCode As String = String.Empty, seriesName As String = String.Empty
        Dim records As String = String.Empty
        Dim albedo As Single
        'added to control when information is not available
        Dim texture() As String = {"sandy clay loam", "silty clay loam", "loamy sand", "sandy loam", "sandy clay", "silt loam", "clay loam", "silty clay", "sand", "loam", "silt", "clay"}
        Dim sands() As Single = {53.2, 8.9, 80.2, 63.4, 52, 15, 29.1, 7.7, 84.6, 41.2, 4.9, 12.7}
        Dim silts() As Single = {20.6, 58.9, 14.6, 26.3, 6, 67, 39.3, 45.8, 11.1, 40.2, 85, 32.7}
        Dim satcs() As Single = {9.24, 11.4, 94.66, 48.01, 0.8, 15.55, 7.74, 5.29, 107.83, 19.98, 10.64, 2.1}
        Dim bds() As Single = {1.49, 1.2, 1.44, 1.46, 1.49, 1.31, 1.33, 1.21, 1.45, 1.4, 1.42, 1.24}
        Dim soilFileName As String = String.Empty
        Dim dtNow1 As Date = Date.Today
        Dim lastSoil1 As UShort = 0

        Try
            APEXStrings1.Length = 0
            'check to see if there are soils selected
            Dim selected As Boolean = False
            For l = 0 To _fieldsInfo1(currentFieldNumber)._soilsInfo.Count - 1
                If _fieldsInfo1(currentFieldNumber)._soilsInfo(l).Selected = True Then
                    selected = True
                    Exit For
                End If
            Next
            'if no soils selected the soils are sorted by area and then selects up to the three most dominant soils.
            If selected = False Then
                _fieldsInfo1(currentFieldNumber)._soilsInfo.Sort(New sortByArea())
                For i = 0 To _fieldsInfo1(currentFieldNumber)._soilsInfo.Count - 1
                    If i > 2 Then Exit For
                    _fieldsInfo1(currentFieldNumber)._soilsInfo(i).Selected = True
                Next
            End If

            Dim APEXScenarios As Short = 0
            i = 0
            For Each soils In _fieldsInfo1(currentFieldNumber)._soilsInfo.Where(Function(x) x.Selected = True)
                lastSoil1 = lastSoil + i + 1
                soilFileName = "APEX" & Format(lastSoil1, "000") & ".sol"
                totalArea += soils.Percentage
                totalArea += soils.Percentage
                APEXScenarios += 1
                records = " .sol file Soil:APEX" & Format(lastSoil1, "000") & ".sol" & "  Date: " & dtNow1 & "Soil Name:" & soils.Name
                soilInfo.Add(records)
                records = String.Empty
                layerNumber = 1
                ds1 = From r In soils._layersInfo Order By r.Depth

                Dim result As Single
                Dim hsg As Char
                For Each dr1 In ds1
                    '// try to find texture from texture description from database
                    'For j = 0 To texture.Length - 1
                    '    If dr1("texture").Contains(texture(j)) Then Exit For
                    'Next

                    If layerNumber = 1 Then
                        'validate if this layer is going to be used for Agriculture Lands
                        If Val(dr1.Depth <= 5) And Val(dr1.Sand) = 0 And Val(dr1.Silt) = 0 And Val(dr1.OM) > 25 And Val(dr1.BD) < 0.8 Then
                            Continue For
                        End If
                        If Not IsDBNull(soils.Albedo) Then
                            If Single.TryParse(soils.Albedo, result) Then
                                albedo = soils.Albedo
                            Else
                                albedo = 0.37
                            End If
                        Else
                            albedo = 0.37
                        End If
                    End If

                    If IsDBNull(soils.Group) Then
                        hsg = "B"
                    Else
                        hsg = soils.Group
                    End If

                    If IsDBNull(soils.Group) Then
                        Depth(layerNumber) = 0
                    Else
                        If Single.TryParse(dr1.Depth, result) Then
                            Depth(layerNumber) = dr1.Depth * in_to_cm
                        Else
                            Depth(layerNumber) = 0
                        End If
                    End If

                    'if current layer is deeper than maxDept then no more layers are needed.
                    'If Depth(layerNumber) > maxDepth(i) And maxDepth(i) > 0 Then Exit For
                    'These statements were added to control duplicated layers in the soil.
                    If layerNumber > 1 Then
                        If Depth(layerNumber) = Depth(layerNumber - 1) Then Continue For
                    End If

                    'UW(layerNumber) = 0
                    If dr1.Uw = 0 And layerNumber > 1 Then
                        UW(layerNumber) = UW(layerNumber - 1)
                    Else
                        UW(layerNumber) = dr1.Uw
                    End If
                    'FC(layerNumber) = 0
                    If dr1.Fc = 0 And layerNumber > 1 Then
                        FC(layerNumber) = FC(layerNumber - 1)
                    Else
                        FC(layerNumber) = dr1.Fc
                    End If
                    'These lines were changed to take Sand from xml file in case user had changed it
                    If IsDBNull(dr1.Sand) Or dr1.Sand = 0 Then
                        Sand(layerNumber) = Sand(layerNumber - 1)
                    Else
                        If Single.TryParse(dr1.Sand, result) Then
                            Sand(layerNumber) = dr1.Sand
                        Else
                            Sand(layerNumber) = Sand(layerNumber - 1)
                        End If
                    End If
                    If IsDBNull(dr1.Silt) Or dr1.Silt = 0 Then
                        Silt(layerNumber) = Silt(layerNumber - 1)
                    Else
                        If Single.TryParse(dr1.Silt, result) Then
                            Silt(layerNumber) = dr1.Silt
                        Else
                            Silt(layerNumber) = Silt(layerNumber - 1)
                        End If
                    End If

                    If dr1.Wn = 0 And layerNumber > 1 Then
                        WN(layerNumber) = WN(layerNumber - 1)
                    Else
                        WN(layerNumber) = dr1.Wn
                    End If
                    If IsDBNull(dr1.PH) Or dr1.PH = 0 Then
                        PH(layerNumber) = PH(layerNumber - 1)
                    Else
                        If Single.TryParse(dr1.PH, result) Then
                            PH(layerNumber) = dr1.PH
                        Else
                            PH(layerNumber) = PH(layerNumber - 1)
                        End If
                    End If
                    If PH(layerNumber) < PHMin Then PH(layerNumber) = PHMin
                    If PH(layerNumber) > PHMax Then PH(layerNumber) = PHMax

                    If IsDBNull(dr1.Cec) Or dr1.Cec < 0 Then
                        CEC(layerNumber) = CEC(layerNumber - 1)
                    Else
                        If Single.TryParse(dr1.Cec, result) Then
                            CEC(layerNumber) = dr1.Cec
                        Else
                            CEC(layerNumber) = CEC(layerNumber - 1)
                        End If
                    End If

                    SMB(layerNumber) = 0
                    If dr1.Smb = 0 And layerNumber > 1 Then
                        SMB(layerNumber) = SMB(layerNumber - 1)
                    Else
                        SMB(layerNumber) = dr1.Smb
                    End If
                    'These lines were changed to take Sand from xml file in case user had changed it
                    If IsDBNull(dr1.OM) Or dr1.OM = 0 Then
                        WOC(layerNumber) = WOC(layerNumber - 1)
                    Else
                        If Single.TryParse(dr1.OM, result) Then
                            WOC(layerNumber) = dr1.OM
                        Else
                            WOC(layerNumber) = WOC(layerNumber - 1)
                        End If
                    End If
                    WOC(layerNumber) = WOC(layerNumber) / om_to_oc
                    If WOC(layerNumber) < OCMin Then WOC(layerNumber) = OCMin
                    If WOC(layerNumber) > OCMax Then WOC(layerNumber) = OCMax

                    'CAC(layerNumber) = 0
                    If dr1.Cac = 0 And layerNumber > 1 Then
                        CAC(layerNumber) = CAC(layerNumber - 1)
                    Else
                        CAC(layerNumber) = dr1.Cac
                    End If
                    'ROK(layerNumber) = 0
                    If dr1.Rok = 0 And layerNumber > 1 Then
                        ROK(layerNumber) = ROK(layerNumber - 1)
                    Else
                        ROK(layerNumber) = dr1.Rok
                    End If
                    'CNDS(layerNumber) = 0
                    If dr1.Cnds = 0 And layerNumber > 1 Then
                        CNDS(layerNumber) = CNDS(layerNumber - 1)
                    Else
                        CNDS(layerNumber) = dr1.Cnds
                    End If
                    'soil P
                    If dr1.SoilP = 0 And layerNumber > 1 Then
                        'SSF(layerNumber) = SSF(layerNumber - 1)   change to put the default when the value for that layer is zero. According to Ali on 1/4/2017.
                        SSF(layerNumber) = SoilPDefault
                    Else
                        SSF(layerNumber) = dr1.SoilP
                    End If

                    'RSD(layerNumber) = 0
                    If dr1.Rsd = 0 And layerNumber > 1 Then
                        RSD(layerNumber) = RSD(layerNumber - 1)
                    Else
                        RSD(layerNumber) = dr1.Rsd
                    End If
                    'These lines were changed to take Silt from xml file in case user had changed it
                    If IsDBNull(dr1.BD) Or dr1.BD = 0 Then
                        BD(layerNumber) = BD(layerNumber - 1)
                    Else
                        If Single.TryParse(dr1.BD, result) Then
                            BD(layerNumber) = dr1.BD
                        Else
                            BD(layerNumber) = BD(layerNumber - 1)
                        End If
                    End If
                    If BD(layerNumber) < BDMin Then BD(layerNumber) = BDMin
                    If BD(layerNumber) > BDMax Then BD(layerNumber) = BDMax
                    BDD(layerNumber) = BD(layerNumber)
                    'psp
                    If dr1.Psp = 0 And layerNumber > 1 Then
                        PSP(layerNumber) = PSP(layerNumber - 1)
                    Else
                        PSP(layerNumber) = dr1.Psp
                    End If
                    Dim sand_silt As Decimal
                    sand_silt = Sand(layerNumber) + Silt(layerNumber)

                    If calSATC(Sand(layerNumber) / 100, (100 - sand_silt) / 100, WOC(layerNumber) * om_to_oc, 1, 0, 0) Then satCondOut = 0
                    SATC(layerNumber) = satCondOut
                    If SATC(layerNumber) = 0 Then
                        If layerNumber = 1 Then
                            SATC(layerNumber) = 0
                        Else
                            SATC(layerNumber) = SATC(layerNumber - 1)
                        End If
                    End If
                    HCL(layerNumber) = 0
                    WPO(layerNumber) = 0
                    CPRV(layerNumber) = 0
                    CPRH(layerNumber) = 0
                    RT(layerNumber) = 0

                    layerNumber = layerNumber + 1

                    If IsDBNull(soils.Slope) Then
                        soilSlope = 0.01
                    Else
                        soilSlope = (soils.Slope / 100)
                    End If
                    If hsg = "" Or hsg = String.Empty Then hsg = "B"
                Next
                'If first layer is less than 0.1 m a new layer is added as first one
                initialLayer = 1
                'If Math.Round(Depth(1) / 100, 3) > 0.1 Then
                '    Depth(0) = 10
                '    BD(0) = BD(1)
                '    UW(0) = UW(1)
                '    FC(0) = FC(1)
                '    Sand(0) = Sand(1)
                '    Silt(0) = Silt(1)
                '    WN(0) = WN(1)
                '    PH(0) = PH(1)
                '    SMB(0) = SMB(1)
                '    WOC(0) = WOC(1)
                '    CAC(0) = CAC(1)
                '    CEC(0) = CEC(1)
                '    ROK(0) = ROK(1)
                '    CNDS(0) = CNDS(1)
                '    SSF(0) = SSF(1)
                '    RSD(0) = RSD(1)
                '    BDD(0) = BDD(1)
                '    PSP(0) = PSP(1)
                '    SATC(0) = SATC(1)
                '    HCL(0) = HCL(1)
                '    WPO(0) = WPO(1)
                '    CPRV(0) = CPRV(0)
                '    CPRH(0) = CPRH(0)
                '    initialLayer = 0
                'End If

                ''If last layer is less than MaxDepth a new layer is added at the end with a depth of 2m
                'Select Case True
                '    Case Depth(layerNumber - 1) < 200
                '        Depth(layerNumber) = 200
                '        BD(layerNumber) = BD(layerNumber - 1)
                '        UW(layerNumber) = UW(layerNumber - 1)
                '        FC(layerNumber) = FC(layerNumber - 1)
                '        Sand(layerNumber) = Sand(layerNumber - 1)
                '        Silt(layerNumber) = Silt(layerNumber - 1)
                '        WN(layerNumber) = WN(layerNumber - 1)
                '        PH(layerNumber) = PH(layerNumber - 1)
                '        SMB(layerNumber) = SMB(layerNumber - 1)
                '        WOC(layerNumber) = WOC(layerNumber - 1)
                '        CAC(layerNumber) = CAC(layerNumber - 1)
                '        CEC(layerNumber) = CEC(layerNumber - 1)
                '        ROK(layerNumber) = ROK(layerNumber - 1)
                '        CNDS(layerNumber) = CNDS(layerNumber - 1)
                '        SSF(layerNumber) = SSF(layerNumber - 1)
                '        RSD(layerNumber) = RSD(layerNumber - 1)
                '        BDD(layerNumber) = BDD(layerNumber - 1)
                '        PSP(layerNumber) = PSP(layerNumber - 1)
                '        SATC(layerNumber) = SATC(layerNumber - 1)
                '        HCL(layerNumber) = HCL(layerNumber - 1)
                '        WPO(layerNumber) = WPO(layerNumber - 1)
                '        CPRV(layerNumber) = CPRV(layerNumber - 1)
                '        CPRH(layerNumber) = CPRH(layerNumber - 1)
                '        layerNumber += 1
                'End Select

                'Line 2 Column 1 and 2
                If albedo = 0 Then albedo = 0.2
                records = String.Empty
                records = Format(albedo, "####0.00").PadLeft(8)
                Select Case hsg
                    Case "A"
                        records &= "      1."
                    Case "B"
                        records &= "      2."
                    Case "C"
                        records &= "      3."
                    Case "D"
                        records &= "      4."
                End Select

                records &= soils.Ffc.ToString("F2").PadLeft(8)
                'controls if wtmx > 0 then check wtmn, wtbl, zqt, ztk to be sure they have the right value. If it is > 0 do nothing.
                If soils.Wtmx = 4 Then soils.Wtmx = 5 'before poor was 4 but now is 5. Dr. Saleh 11 07 2016. Oscar Gallego
                If soils.Wtmx > 0 And soils.Wtmn <= 0 Then
                    soils.Wtmn = 1
                End If
                If soils.Wtmx > 0 And soils.Wtbl <= 0 Then
                    soils.Wtbl = 2
                End If
                'if tile drain was set up wtmx, wtmn, and wtbl should be zero. Otherwise keep the numbers. 11 06 2016.
                'Goin back to the values for drainage type as before. Keep this here just in case Ali wants to have it latter 11 08 2016. Oscar Gallego
                If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.TileDrainDepth > 0 Then
                    records &= 0.ToString("F2").PadLeft(8)
                    records &= 0.ToString("F2").PadLeft(8)
                    records &= 0.ToString("F2").PadLeft(8)
                Else
                    records &= soils.Wtmn.ToString("F2").PadLeft(8)
                    records &= soils.Wtmx.ToString("F2").PadLeft(8)
                    records &= soils.Wtbl.ToString("F2").PadLeft(8)
                End If
                'records &= soils.Wtmn.ToString("F2").PadLeft(8)
                'records &= soils.Wtmx.ToString("F2").PadLeft(8)
                'records &= soils.Wtbl.ToString("F2").PadLeft(8)
                records &= soils.Gwst.ToString("F2").PadLeft(8)
                records &= soils.Gwmx.ToString("F2").PadLeft(8)
                records &= soils.Rftt.ToString("F2").PadLeft(8)
                records &= soils.Rfpk.ToString("F2").PadLeft(8)
                soilInfo.Add(records)
                'Line 3 Column 1 to 7
                records = String.Empty
                records &= soils.Tsla.ToString("F2").PadLeft(8)
                records &= soils.Xids.ToString("F2").PadLeft(8)
                records &= soils.Rtn1.ToString("F2").PadLeft(8)
                records &= soils.Xidk.ToString("F2").PadLeft(8)
                If soils.Wtmx > 0 And soils.Zqt <= 0 Then
                    records &= 2.ToString("F2").PadLeft(8)
                Else
                    records &= soils.Zqt.ToString("F2").PadLeft(8)
                End If
                records &= soils.Zf.ToString("F2").PadLeft(8)
                If soils.Wtmx > 0 And soils.Ztk <= 0 Then
                    records &= 1.ToString("F2").PadLeft(8)
                Else
                    records &= soils.Ztk.ToString("F2").PadLeft(8)
                End If
                'records = "    10.0     1.0"
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= (Depth(layers) / 100).ToString("F3").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    If BD(layers) = 0 Then BD(layers) = 1.3
                    records &= BD(layers).ToString("F3").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= UW(layers).ToString("F3").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= FC(layers).ToString("F3").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= Sand(layers).ToString("F2").PadLeft(8)
                    'records &= Format(Sand(layers), "    0.00").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= Silt(layers).ToString("F2").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= WN(layers).ToString("F2").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= PH(layers).ToString("F2").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= SMB(layers).ToString("F2").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= WOC(layers).ToString("F2").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= CAC(layers).ToString("F2").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= CEC(layers).ToString("F2").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= Format(ROK(layers), "    0.00").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= CNDS(layers).ToString("F2").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    'If Depth(layers) > SoilPMaxForSoilDepth Then SSF(layers) = SoilPDefault   this condition was commentarized according to Ali on 1/4/17 to allow to take the override value.
                    If SSF(layers) = 0 Then SSF(layers) = SoilPDefault
                    records &= SSF(layers).ToString("F2").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= RSD(layers).ToString("F2").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= BDD(layers).ToString("F2").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= PSP(layers).ToString("F2").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= SATC(layers).ToString("F2").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= HCL(layers).ToString("F2").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                For layers = initialLayer To layerNumber - 1
                    records &= WPO(layers).ToString("F2").PadLeft(8)
                Next
                soilInfo.Add(records)
                records = String.Empty
                'this lines are not added at this time to reduced the size of the information to be transmited to the server. The server will include them in blanks for now. 5/30/2014.
                ''INSERT LINES 25, 26, 27, 28
                'For layers = initialLayer To layerNumber - 1
                '    records &= 0.ToString("F2").PadLeft(8)
                'Next
                'soilInfo.Add(records)
                'soilInfo.Add(records)
                'soilInfo.Add(records)
                'soilInfo.Add(records)

                'records = String.Empty
                'For layers = initialLayer To layerNumber - 1
                '    records &= CPRV(layers).ToString("F2").PadLeft(8)
                'Next
                'soilInfo.Add(records)
                'records = String.Empty
                'For layers = initialLayer To layerNumber - 1
                '    records &= CPRH(layers).ToString("F2").PadLeft(8)
                'Next
                'soilInfo.Add(records)
                'records = String.Empty

                ''ADD LINES 31 TO 45
                'For layers = initialLayer To layerNumber - 1
                '    records &= 0.ToString("F2").PadLeft(8)
                'Next
                'soilInfo.Add(records)
                'soilInfo.Add(records)
                'soilInfo.Add(records)
                'soilInfo.Add(records)
                'soilInfo.Add(records)
                'soilInfo.Add(records)
                'soilInfo.Add(records)
                'soilInfo.Add(records)
                'soilInfo.Add(records)
                'soilInfo.Add(records)
                'soilInfo.Add(records)
                'soilInfo.Add(records)
                'soilInfo.Add(records)
                'soilInfo.Add(records)
                'soilInfo.Add(records)
                'soilInfo.Add(records)

                soilList.Add("  " & Format(lastSoil1, "000").PadLeft(8) & " APEX" & Format(lastSoil1, "000").PadLeft(3) & ".sol")
                i += 1
            Next

            lastSoil = lastSoil1
        Catch ex As System.Exception
            Dim ex1 As String = ex.Message
        Finally

        End Try
    End Sub

    Protected Function calSATC(ByVal sandIn As Decimal, ByVal clayIn As Decimal, ByVal orgMatterIn As Decimal, ByVal densityFactorIn As Decimal, ByVal gravelsIn As Decimal, ByVal salinityIn As Decimal) As Boolean
        'Outputs
        Dim wiltOut As Decimal
        Dim fieldCapOut As Decimal
        Dim saturationOut As Decimal
        Dim plantAvailOut As Decimal
        Dim matricDenOut As Decimal
        Dim airEntryOut As Decimal
        Dim gravelsOut As Decimal
        Dim X, Y, Z, i, j, k As Decimal
        Dim bulkDensityOut As Decimal

        Try

            wiltOut = -0.024 * sandIn + 0.487 * clayIn + 0.006 * orgMatterIn + 0.005 * sandIn * orgMatterIn - 0.013 * clayIn * orgMatterIn + 0.068 * sandIn * clayIn + 0.031 + _
            0.14 * (-0.024 * sandIn + 0.487 * clayIn + 0.006 * orgMatterIn + 0.005 * sandIn * orgMatterIn - 0.013 * clayIn * orgMatterIn + 0.068 * sandIn * clayIn + 0.031) - 0.02

            Y = -0.251 * sandIn + 0.195 * clayIn + 0.011 * orgMatterIn + 0.006 * sandIn * orgMatterIn - 0.027 * clayIn * orgMatterIn + 0.452 * sandIn * clayIn + 0.299

            X = 0.278 * sandIn + 0.034 * clayIn + 0.022 * orgMatterIn - 0.018 * sandIn * orgMatterIn - 0.027 * clayIn * orgMatterIn - 0.584 * sandIn * clayIn + 0.078 + _
            (0.636 * (0.278 * sandIn + 0.034 * clayIn + 0.022 * orgMatterIn - 0.018 * sandIn * orgMatterIn - 0.027 * clayIn * orgMatterIn - 0.584 * sandIn * clayIn + 0.078) - 0.107) + _
            Y + (1.283 * Y * Y - 0.374 * Y - 0.015) + -0.097 * sandIn + 0.043

            'I18=1.0
            matricDenOut = (1 - X) * 2.65 * 1.0
            fieldCapOut = Y + (1.283 * Y * Y - 0.374 * Y - 0.015) + 0.2 * ((1 - matricDenOut / 2.65) - (1 - ((1 - X) * 2.65) / 2.65))

            saturationOut = 1 - (matricDenOut / 2.65)

            plantAvailOut = (fieldCapOut - wiltOut) * (1 - ((matricDenOut / 2.65) * gravelsIn) / (1 - gravelsIn * (1 - matricDenOut / 2.65)))

            Z = -0.024 * sandIn + 0.487 * clayIn + 0.006 * orgMatterIn + 0.005 * sandIn * orgMatterIn - 0.013 * clayIn * orgMatterIn + 0.068 * sandIn * clayIn + 0.031 + _
            0.14 * (-0.024 * sandIn + 0.487 * clayIn + 0.006 * orgMatterIn + 0.005 * sandIn * orgMatterIn - 0.013 * clayIn * orgMatterIn + 0.068 * sandIn * clayIn + 0.031) - 0.02
            i = (Math.Log(Y + (1.283 * Y * Y - 0.374 * Y - 0.015) + 0.2 * ((1 - matricDenOut / 2.65) - (1 - ((1 - X) * 2.65) / 2.65))) / Math.Log(2.718281828) - Math.Log(Z) / Math.Log(2.718281828)) / (Math.Log(1500) / Math.Log(2.718281828) - Math.Log(33) / Math.Log(2.718281828))
            'J=0.0
            satCondOut = 1930 * ((1 - (matricDenOut / 2.65)) - (Y + (1.283 * Y * Y - 0.374 * Y - 0.015) + 0.2 * ((1 - matricDenOut / 2.65) - (1 - ((1 - X) * 2.65) / 2.65)))) ^ (3 - i) * (1 - gravelsIn) / (1 - gravelsIn * (1 - 1.5 * (matricDenOut / 2.65)))

            j = 1 - (matricDenOut / 2.65) - (Y + (1.283 * Y * Y - 0.374 * Y - 0.015) + 0.2 * ((1 - matricDenOut / 2.65) - (1 - ((1 - X) * 2.65) / 2.65)))
            k = -21.674 * sandIn - 27.932 * clayIn - 81.975 * j + 71.121 * sandIn * j + 8.294 * clayIn * j + 14.05 * sandIn * clayIn + 27.161

            airEntryOut = Y + (1.283 * Y * Y - 0.374 * Y - 0.015) + 0.2 * ((1 - matricDenOut / 2.65) - (1 - ((1 - X) * 2.65) / 2.65)) - (10 - 33) * ((1 - (matricDenOut / 2.65)) - (Y + (1.283 * Y * Y - 0.374 * Y - 0.015) + 0.2 * ((1 - matricDenOut / 2.65) - (1 - ((1 - X) * 2.65) / 2.65)))) / (33 - (k + (0.02 * k ^ 2 - 0.113 * k - 0.7)))

            gravelsOut = ((matricDenOut / 2.65) * gravelsIn) / (1 - gravelsIn * (1 - matricDenOut / 2.65))

            bulkDensityOut = gravelsOut * 2.65 + (1 - gravelsOut) * matricDenOut
            Return False
        Catch
            Return True
        End Try
    End Function

    Function CreateSubarea(operationNumber As UShort)
        Dim lastSoil1 As UShort = 0
        Dim lastOwner1 As UShort = 0
        Dim i As UShort = 0
        Dim nirr As UShort = 0

        Try
            For Each soil In _fieldsInfo1(currentFieldNumber)._soilsInfo.Where(Function(x) x.Selected = True)
                'create the operation file for this subarea.
                nirr = createOperations(soil)
                'create the subarea file
                If Not (_fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.CBCWidth > 0 And _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.CBBWidth > 0 And _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.CBCrop > 0) Then
                    addSubareaFile(soil._scenariosInfo(currentScenarioNumber)._subareasInfo, operationNumber, lastSoil1, lastOwner1, i, nirr, False)
                    i += 1
                End If
            Next
            If lastSoil1 > 0 Then
                lastSoilSub = lastSoil1 - 1
            Else
                lastSoilSub = 0
            End If
            lastSubarea = 0
            For Each buf In _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo
                If Not (buf.SbaType = "PPDE" Or buf.SbaType = "PPTW" Or buf.SbaType = "AITW" Or buf.SbaType = "CBMain") Then
                    'create the operation file for this subarea.
                    lastSubarea += 1
                    opcsFile.Add(buf.SubareaTitle)
                    opcsFile.Add(".OPC " & buf.SubareaTitle & " file Operation:1  Date: " & Now.ToString)
                    opcsFile.Add(buf._operationsInfo(0).LuNumber.ToString.PadLeft(4))
                    For Each oper In buf._operationsInfo
                        With oper
                            opcsFile.Add(.Year.ToString.PadLeft(3) & .Month.ToString.PadLeft(3) & .Day.ToString.PadLeft(3) & .ApexOp.ToString.PadLeft(5) & _
                                         0.ToString.PadLeft(5) & .ApexCrop.ToString.PadLeft(5) & .ApexOpType.ToString.PadLeft(5) & _
                                         (.OpVal1.ToString & ".0").PadLeft(8) & (.OpVal2.ToString & ".0").PadLeft(8))
                        End With
                    Next
                    opcsFile.Add("End " & buf.SubareaTitle)
                End If
                addSubareaFile(buf, operationNumber, lastSoil1, lastOwner1, 0, 0, True)
            Next

            lastSoilSub = lastSoil1
            lastOwner = lastOwner1
            'todo check this one.
            'lastSubarea += _fieldsInfo1(currentFieldNumber)._soilsInfo(i - 1)._scenariosInfo(currentScenarioNumber)._subareasInfo._line2(0).Iops

            Return "OK"

        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Private Function createOperations(soil As SoilsData) As UShort
        'This suroutine create operation files for Baseline and Alternative using information entered by user.
        'Dim tempSplit(13) As String
        Dim dtNow1 As Date = Now()
        'Dim opcs As String = String.Empty
        Dim irrigationType As Integer = 0
        'Dim hu As Single = 0
        Dim nirr As UShort = 0
        Dim j As UShort

        Try
            'verify if _crops are empty. If so get them.
            fertCode = 79
            GrazingB = False
            opcsFile.Add("Operation")

            Select Case _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AIType
                Case 0
                    irrigationType = 0
                Case 1
                    irrigationType = 500
                Case 2
                    irrigationType = 502
                Case 3
                    irrigationType = 530
                Case 4
                    irrigationType = 533
                Case 5
                    irrigationType = 534
                Case 6
                    irrigationType = 0
                Case 7
                    irrigationType = 502
                Case 8
                    irrigationType = 502
                Case Else
                    irrigationType = 0
            End Select
            'SORT operation records by date (year, month, day)
            Dim query As IEnumerable(Of OperationsData)
            query = From r In soil._scenariosInfo(currentScenarioNumber)._operationsInfo Order By r.Year, r.Month, r.Day, r.ApexOpName, r.EventId
            'check and fix the operation list                    
            query = FixOperationFile(query)
            'line 1
            opcsFile.Add(" .Opc file created directly by the user. Date: " & dtNow1)

            j = 0

            For Each operation In query
                If operation.ApexCrop = cropMixedGrass And (operation.ApexOpAbbreviation = planting Or operation.ApexOpAbbreviation = kill Or operation.ApexOpAbbreviation = tillage) Then
                    Dim mixedCrops = operation.MixedCropData.Split(",")
                    Dim mixedCropsInfo(2) As String
                    Dim newOper As OperationsData

                    For Each value In mixedCrops
                        newOper = New OperationsData
                        newOper = AddMixedOperations(operation)
                        mixedCropsInfo = value.Split("|")
                        newOper.ApexCrop = mixedCropsInfo(0)
                        newOper.OpVal5 = mixedCropsInfo(2) / ac_to_m2
                        newOper.setCN(newOper.ApexCrop, "", _fieldsInfo1.Count, soil.Group, currentFieldNumber, _startInfo, Session("UserGuide"))
                        AddOperation(newOper, irrigationType, nirr, soil.Percentage, j)
                    Next
                Else
                    AddOperation(operation, irrigationType, nirr, soil.Percentage, j)
                End If
            Next

            opcsFile.Add("End Operation")

            Return nirr

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("OperationFileError").Value & ex.Message)
            Return nirr
        End Try
    End Function

    Public Function AddMixedOperations(oper As OperationsData) As OperationsData
        Dim operation As New OperationsData

        operation.ApexCrop() = oper.ApexCrop
        operation.ApexCropName = oper.ApexCropName
        operation.ApexOp = oper.ApexOp
        operation.ApexOpAbbreviation = oper.ApexOpAbbreviation
        operation.ApexOpName = oper.ApexOpName
        operation.ApexOpType = oper.ApexOpType
        operation.ApexOpTypeName = oper.ApexOpTypeName
        operation.ApexOpv1 = oper.ApexOpv1
        operation.ApexOpv2 = oper.ApexOpv2
        operation.ApexTillCode = oper.ApexTillCode
        operation.ApexTillName = oper.ApexTillName
        operation.ConvertionUnit = oper.ConvertionUnit
        operation.Day = oper.Day
        operation.Month = oper.Month
        operation.Year = oper.Year
        operation.EventId = oper.EventId
        operation.Index = oper.Index
        operation.LuNumber = oper.LuNumber
        operation.NO3 = oper.NO3
        operation.PO4 = oper.PO4
        operation.OrgN = oper.OrgN
        operation.OrgP = oper.OrgP
        operation.NH3 = oper.NH3
        operation.OpVal1 = oper.OpVal1
        operation.OpVal2 = oper.OpVal2
        operation.OpVal3 = oper.OpVal3
        operation.OpVal4 = oper.OpVal4
        operation.OpVal5 = oper.OpVal5
        operation.OpVal6 = oper.OpVal6
        operation.OpVal7 = oper.OpVal7
        operation.Period = oper.Period
        operation.Scenario = oper.Scenario
        operation.Selected = oper.Selected
        operation.TractorId = oper.TractorId
        operation.var9 = oper.var9
        operation.MixedCropData = oper.MixedCropData
        Return operation
    End Function


    Private Sub AddOperation(operation As OperationsData, irrigationType As UShort, ByRef nirr As UShort, soilPercentage As Single, ByRef j As UShort)
        Dim APEXString As New System.Text.StringBuilder
        Dim items(8) As String
        Dim values(8) As Single
        Dim opv5 As String
        Dim luNumber As Short : Dim harvestCode As Short : Dim filterStrip As String = String.Empty
        Dim cropAnt As Short = 0
        Dim found As Boolean = False
        Dim operAnt As Integer = 799
        Dim animalCode As Short = 0
        Dim cn As Single = 0

        For i = 0 To 8 - 1
            items(i) = ""
            values(i) = 0
        Next
        items(7) = "LATITUDE"
        items(8) = "LONGITUDE"
        APEXString.Length = 0

        If cropAnt <> operation.ApexCrop Then
            For Each crop In _crops
                If crop.Number = operation.ApexCrop Then
                    luNumber = crop.LuNumber
                    harvestCode = crop.HarvestCode
                    filterStrip = crop.FilterStrip
                    Exit For
                End If
            Next
            cropAnt = operation.ApexCrop
        End If
        'if the process is starting the lines 1, 2, and 3 should be created
        If j = 0 Then
            If irrigationType > 0 Then
                opcsFile.Add(luNumber.ToString.Trim.PadLeft(4) & irrigationType.ToString.PadLeft(4))
            Else
                opcsFile.Add(luNumber.ToString.Trim.PadLeft(4))
            End If
            If irrigationType = 7 Then 'ADD BILD FURROW DIKE OPERATION for baseline
                opcsFile.Add(1.ToString.PadLeft(3) & 1.ToString.PadLeft(3) & 2.ToString.PadLeft(3) & 256.ToString.PadLeft(5) & 0.ToString.PadLeft(5) & operation.ApexCrop.ToString.PadLeft(5))
            End If
        End If

        opv5 = Space(5)
        APEXString.Append(operation.Year.ToString.PadLeft(3))           'Year
        APEXString.Append(operation.Month.ToString.PadLeft(3))          'Month
        If operation.Month = 12 And operation.Day = 31 Then
            APEXString.Append(30.ToString.PadLeft(3))           'Day
        Else
            APEXString.Append(operation.Day.ToString.PadLeft(3))                  'Day
        End If

        If operation.ApexOpAbbreviation.ToString.Trim = planting Or operation.ApexOpAbbreviation.ToString.Trim = tillage Or operation.ApexOpAbbreviation.ToString.Trim = harvest Or operation.ApexOpAbbreviation.ToString.Trim = irrigation Then
            Select Case operation.ApexOpAbbreviation.Trim
                Case harvest
                    If _fieldsInfo1(currentFieldNumber).Forestry Then
                        APEXString.Append(operation.ApexTillCode.ToString.PadLeft(5))    'Operation Code        'APEX0604
                    Else
                        APEXString.Append(harvestCode.ToString.PadLeft(5))    'Operation Code        'APEX0604
                    End If
                Case tillage
                    APEXString.Append(operation.ApexTillCode.ToString.PadLeft(5))    'Operation Code        'APEX0604
                Case irrigation
                    APEXString.Append(operation.ApexTillCode.ToString.PadLeft(5))    'Operation Code        'APEX0604
                Case planting
                    If operation.ApexOp <> 1 Then
                        APEXString.Append(operation.ApexOp.ToString.PadLeft(5))    'Operation Code        'APEX0604
                    Else
                        APEXString.Append(operation.ApexTillCode.ToString.PadLeft(5))    'Operation Code        'APEX0604
                    End If

                Case Else
                    APEXString.Append(operation.ApexTillCode.ToString.PadLeft(5))    'Operation Code        'APEX0604
            End Select
        Else
            If operation.ApexOpAbbreviation.ToString.Trim = fertilizer Then
                found = False
                If Not depthAnt Is Nothing Then
                    For n = 0 To depthAnt.Length - 1
                        If depthAnt(n) = operation.ApexOpv2 Then
                            operAnt = opers(n)
                            found = True
                        End If
                    Next
                    numOfDepths = depthAnt.Length
                Else
                    numOfDepths = 0
                End If

                If found = False Then
                    ReDim Preserve depthAnt(numOfDepths)
                    ReDim Preserve opers(numOfDepths)
                    operAnt = operAnt + 1
                    opers(opers.Length - 1) = operAnt
                    depthAnt(depthAnt.Length - 1) = operation.ApexOpv2
                    changeTillForDepth(operAnt, depthAnt(depthAnt.Length - 1))
                End If
                APEXString.Append(operAnt.ToString.PadLeft(5))    'Operation Code        'APEX0604
            Else
                APEXString.Append(operation.ApexTillCode.ToString.PadLeft(5))    'Operation Code        'APEX0604
            End If
        End If

        APEXString.Append("     ")                           'Tractor ID. Not Used  'APEX0604
        APEXString.Append(operation.ApexCrop.ToString.PadLeft(5))    'Crop Code             'APEX0604
        Select Case True
            Case operation.ApexOpAbbreviation.ToString.Trim = planting
                If operation.ApexCrop = "18" Then
                    riceCrop = True
                End If
                If luNumber = 28 Then
                    If operation.ApexCrop = 168 Or operation.ApexCrop = 199 Then 'add time from planting to harvest in year for plantain and bananas
                        APEXString.Append(1.ToString.Trim.PadLeft(5))
                    Else
                        APEXString.Append(filterStrip.ToString.Trim.PadLeft(5))
                    End If
                Else
                    APEXString.Append(0.ToString.PadLeft(5))    'TIME TO MATURITY       'APEX0604
                End If
                '***** opval1 heat units in planting operation is calcualted when management file is saved or when the project is uploaded. Simulation is not going to change it.
                'If operation.OpVal1 = 0 Then
                'Dim getHu As New GetHU  'commentarized because the ddl is nog going to be used, instead a web server will be in place
                'operation.OpVal1 = getHu.calcHU(operation.ApexCrop, wp1Files & "\" & _startInfo.Wp1Name & ".WP1", folder & "\App_Data\PHUCRP.DAT")

                'operation.OpVal1 = calcHU(operation.ApexCrop, _crops, _startInfo)
                'Dim hu As New NTTCalcHU.Service1SoapClient
                'Dim hu1 As New NTTCalcHU.ServiceSoapClient
                'operation.OpVal1 = Math.Round(hu.getHU(operation.ApexCrop, _startInfo.farmCentrois.Split(",")(1), _startInfo.farmCentrois.Split(",")(0)), 2)
                'operation.OpVal1 = Math.Round(hu.getHU(operation.ApexCrop, _startInfo.weatherLat, _startInfo.weatherLon), 2)
                'If System.Net.Dns.GetHostName = "T-NN" Then                
                'operation.OpVal1 = Math.Round(hu1.getHU(operation.ApexCrop, _startInfo.weatherLat, _startInfo.weatherLon, hu1.jdt(operation.Day, operation.Month, 0), sAPEXBasLoc1 & Session("userGuide")))
                'End If
                'If operation.OpVal1 = 0 Then
                'Changed back to take the HU from the database according to Ali 11/2/2016  - Oscar Gallego 11/2/2016
                'operation.OpVal1 = _crops.Where(Function(x) x.Number = operation.ApexCrop).Select(Function(x) x.heat_unit).SingleOrDefault
                'End If
                'End If
                APEXString.Append(operation.OpVal1.ToString("F2").PadLeft(8))  'change to take heatunits from program using wp1 file.
                items(0) = "Heat Units"
                values(0) = operation.OpVal1
                With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
                    If .PPNDWidth > 0 Or .PPDSWidth > 0 Or .PPDEWidth > 0 Or .PPTWWidth > 0 Then
                        APEXString.Append((Format(operation.OpVal2 * 0.9, "0.0")).PadLeft(8))   'curve number is reduce
                    Else
                        APEXString.Append((Format(operation.OpVal2, "0.0")).PadLeft(8))  'curve number
                    End If
                End With
                APEXString.Append("0.00".PadLeft(8))                      'Opv3. No entry needed.
                APEXString.Append("0.00".PadLeft(8))                      'Opv4. No entry needed.
                If operation.OpVal5 < 0.01 Then
                    APEXString.Append(Format(operation.OpVal5, "#.000000").PadLeft(8))                      'Opv 5 Plant Population.
                Else
                    APEXString.Append(Format(operation.OpVal5, "####0.00").PadLeft(8))                      'Opv 5 Plant Population.
                End If
                If operation.OpVal5 = 0 And operation.ApexOpv1 > 0 Then
                    operation.setOpval1(0, "")
                End If
            Case operation.ApexOpAbbreviation.ToString.Trim = irrigation
                APEXString.Append("0".PadLeft(5))    '
                items(0) = "Irrigation"
                values(0) = operation.OpVal2
                APEXString.Append(operation.OpVal1.ToString("F2").PadLeft(8))  'Volume applied for irrigation in mm
                nirr = 1
                APEXString.Append(Format(0, "####0.00").PadLeft(8))             'opval2
                APEXString.Append("0.00".PadLeft(8))                      'Opv3. No entry needed.
                APEXString.Append("0.00".PadLeft(8) & Format(operation.OpVal2, "####0.00").PadLeft(8))  'Opv4 Irrigation Efficiency
                APEXString.Append("0.00".PadLeft(8))                      'Opv5. No entry neede.
            Case operation.ApexOpAbbreviation.ToString.Trim = fertilizer            'fertilizer or fertilizer(folier)
                If operation.ApexTillName.ToString.ToLower.Contains("fert") Then
                    With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
                        If operation.ApexTillName.ToString.ToLower.Contains("solid") And .mcType > 0 Then
                            AddFert(operation.NO3 * .mcNO3_N, operation.PO4 * .mcPO4_P, operation.OrgN * .mcOrgN, operation.OrgP * .mcOrgP, operation.ApexTillName, operation.NH3, operation.K)
                        Else
                            AddFert(operation.NO3, operation.PO4, operation.OrgN, operation.OrgP, operation.ApexTillName, operation.NH3, operation.K)
                        End If
                    End With
                    APEXString.Append(fertCode.ToString.PadLeft(5))    'Fertilizer Code       'APEX0604
                    items(0) = fertCode
                Else
                    APEXString.Append(operation.ApexOpType.ToString.PadLeft(5))    'Fertilizer Code       'APEX0604
                    items(0) = operation.ApexOpType
                End If
                APEXString.Append(operation.OpVal1.ToString("F2").PadLeft(8))  'kg/ha of fertilizer applied
                values(0) = operation.OpVal1
                APEXString.Append(Format(Val(operation.OpVal2), "####0.00").PadLeft(8))
                items(1) = "Depth"
                values(1) = operation.OpVal2
                APEXString.Append("0.00".PadLeft(8))                      'Opv3. No entry needed.
                APEXString.Append("0.00".PadLeft(8))                      'Opv4. No entry needed.
                APEXString.Append("0.00".PadLeft(8))                      'Opv5. No entry neede.
            Case operation.ApexOpAbbreviation.ToString.Trim = grazing              'Grazing - kind and number of animals 
                APEXString.Append(0.ToString.PadLeft(5))    '
                'if number of animals were enter in modify page and it is the first grazing operation
                If GrazingB = False Then
                    items(3) = "DryMatterIntake"
                    values(3) = CreateHerdFile(operation.ApexOpv1, operation.ApexOpv2, operation.ApexTillName, soilPercentage)
                    animalB = operation.ApexTillCode
                    GrazingB = True
                    If operation.NO3 <> 0 Or operation.PO4 <> 0 Or operation.OrgN <> 0 Or operation.OrgP <> 0 Or operation.NH3 <> 0 Then
                        animalCode = getAnimalCode(operation.ApexTillCode)
                        ChangeFert(operation.NO3, operation.PO4, operation.OrgN, operation.OrgP, animalCode, operation.NH3, operation.K)
                    End If
                End If
                APEXString.Append(operation.OpVal1.ToString("F4").PadLeft(8))
                items(0) = "Kind"
                values(0) = operation.ApexTillCode
                items(1) = "Animals"
                values(1) = operation.ApexOpv1
                items(2) = "Hours"
                values(2) = operation.ApexOpv2
                APEXString.Append(Format(0, "####0.00").PadLeft(8))             'opval2
                APEXString.Append("0.00".PadLeft(8))                      'Opv3. No entry needed.
                APEXString.Append("0.00".PadLeft(8))                      'Opv4. No entry needed.
                APEXString.Append("0.00".PadLeft(8))                      'Opv5. No entry neede.
            Case operation.ApexOpAbbreviation.ToString.Trim = "pesticide"              'Grazing - kind and number of animals 
                APEXString.Append(operation.ApexTillCode.ToString.PadLeft(5))    '
                If operation.ApexOpv1 <= 0 Then operation.ApexOpv1 = 1
                APEXString.Append(Format((operation.ApexOpv1.ToString * lbs_to_kg / ac_to_ha), "####0.00").PadLeft(8))  'kg/ha of pesticide applied
                items(1) = "Pesticide"
                values(1) = operation.ApexOpv1.ToString * lbs_to_kg / ac_to_ha
                APEXString.Append(Format(0, "####0.00").PadLeft(8))             'opval2
                APEXString.Append("0.00".PadLeft(8))                      'Opv3. No entry needed.
                APEXString.Append("0.00".PadLeft(8))                      'Opv4. No entry needed.
                APEXString.Append("0.00".PadLeft(8))                      'Opv5. No entry neede.
            Case operation.ApexOpAbbreviation.ToString.Trim = tillage
                APEXString.Append(0.ToString.PadLeft(5))    '
                APEXString.Append(Format(0, "####0.00").PadLeft(8))
                items(0) = "Tillage"
                values(0) = 0
                APEXString.Append(Format(0, "####0.00").PadLeft(8))             'opval2
                APEXString.Append("0.00".PadLeft(8))                      'Opv3. No entry needed.
                APEXString.Append("0.00".PadLeft(8))                      'Opv4. No entry needed.
                APEXString.Append("0.00".PadLeft(8))                      'Opv5. No entry neede.
            Case operation.ApexOpAbbreviation.ToString.Trim = harvest
                APEXString.Append(0.ToString.PadLeft(5))    '
                APEXString.Append(Format(0, "####0.00").PadLeft(8))
                items(1) = "Curve Number"
                values(1) = cn
                If _fieldsInfo1(currentFieldNumber).Forestry Then
                    changeTillForHE(operation.ApexTillCode, operation.ApexOpv1)
                End If
                APEXString.Append(Format(0, "####0.00").PadLeft(8))             'opval2
                APEXString.Append("0.00".PadLeft(8))                      'Opv3. No entry needed.
                APEXString.Append("0.00".PadLeft(8))                      'Opv4. No entry needed.
                APEXString.Append("0.00".PadLeft(8))                      'Opv5. No entry neede.
            Case operation.ApexOpAbbreviation.ToString.Trim = kill
                APEXString.Append(0.ToString.PadLeft(5))    '
                APEXString.Append(Format(0, "####0.00").PadLeft(8))
                items(0) = "Curve Number"
                values(0) = cn
                items(1) = "Time of Operation"
                values(1) = cn
                APEXString.Append(Format(0, "####0.00").PadLeft(8))        'opval2
                APEXString.Append("0.00".PadLeft(8))                      'Opv3. No entry needed.
                APEXString.Append("0.00".PadLeft(8))                      'Opv4. No entry needed.
                APEXString.Append("0.00".PadLeft(8))                      'Opv5. No entry neede.
            Case operation.ApexOpAbbreviation.ToString.Trim = stopGrazing
                APEXString.Append(0.ToString.PadLeft(5))    '
                APEXString.Append(Format(0, "####0.00").PadLeft(8))
                items(0) = "Stop Grazing"
                values(0) = 0
                APEXString.Append(Format(0, "####0.00").PadLeft(8))       'opval2
                APEXString.Append("0.00".PadLeft(8))                      'Opv3. No entry needed.
                APEXString.Append("0.00".PadLeft(8))                      'Opv4. No entry needed.
                APEXString.Append("0.00".PadLeft(8))                      'Opv5. No entry neede.
            Case operation.ApexOpAbbreviation.ToString.Trim = liming
                APEXString.Append(0.ToString.PadLeft(5))    '
                APEXString.Append(operation.OpVal1.ToString("F2").PadLeft(8))  'kg/ha of fertilizer applied
            Case Else                                               'No entry needed.
                APEXString.Append(0.ToString.PadLeft(5))    '
                APEXString.Append(Format(0, "####0.00").PadLeft(8))       'opval1
                APEXString.Append(Format(0, "####0.00").PadLeft(8))       'opval2
                APEXString.Append("0.00".PadLeft(8))                      'Opv3. No entry needed.
                APEXString.Append("0.00".PadLeft(8))                      'Opv4. No entry needed.
                APEXString.Append("0.00".PadLeft(8))                      'Opv5. No entry neede.
        End Select

        APEXString.Append(opv5.PadLeft(5) & ".00")                    'Opv6
        APEXString.Append(operation.OpVal7.ToString("F2").PadLeft(8))                    'Opv7
        j += 1
        opcsFile.Add(APEXString.ToString)

        'add operations in list for fem.
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)
            femList.Add(.Name & coma & .Name & coma & _startInfo.StateName & coma & operation.Year & coma & operation.Month & coma & operation.Day & coma & operation.ApexOp & coma & operation.ApexOpName & coma & operation.ApexCrop & _
                    coma & operation.ApexCropName & coma & _startInfo.yearsRotation & coma & 0 & coma & 0 & coma & items(0) & coma & values(0) & coma & items(1) & coma & values(1) & coma & items(2) & coma & values(2) & coma & items(3) & coma & values(3) & coma & items(4) & coma & _
                    values(4) & coma & items(5) & coma & values(5) & coma & items(6) & coma & values(6) & coma & items(7) & coma & values(7) & coma & items(8) & coma & values(8))
        End With

    End Sub

    Function addSubareaFile(_subareaInfo As SubareasData, operationNumber As UShort, ByRef lastSoil1 As UShort, lastOwner1 As UShort, i As Short, nirr As UShort, buffer As Boolean)
        Dim sLine As String = String.Empty
        Dim j As UShort = i + 1
        Try
            With _subareaInfo
                '/line 1
                If buffer Then
                    subareaFile.Add((lastSoil1 + lastSubarea).ToString("D").PadLeft(8) & "0000000000000000   " & .SubareaTitle)
                Else
                    subareaFile.Add(j.ToString("D").PadLeft(8) & .SubareaTitle)
                End If
                '/line 2
                lastSoil1 = j + lastSoilSub
                sLine = lastSoil1.ToString("D").Trim.PadLeft(4)
                sLine += (lastSoil1 + lastSubarea).ToString("D").Trim.PadLeft(4)

                If ._line2(0).Iow = 0 Then ._line2(0).Iow = 1
                lastOwner1 = lastSoil1
                sLine += lastOwner1.ToString("D").Trim.PadLeft(4)    'owner id. Should change for each field
                sLine += ._line2(0).Ii.ToString("D").Trim.PadLeft(4)
                sLine += ._line2(0).Iapl.ToString("D").Trim.PadLeft(4)
                sLine += 0.ToString("D").Trim.PadLeft(4)       'column 6 line 1 is not used
                sLine += ._line2(0).Nvcn.ToString("D").Trim.PadLeft(4)
                sLine += ._line2(0).Iwth.ToString("D").Trim.PadLeft(4)
                sLine += ._line2(0).Ipts.ToString("D").Trim.PadLeft(4)
                sLine += ._line2(0).Isao.ToString("D").Trim.PadLeft(4)
                sLine += ._line2(0).Luns.ToString("D").Trim.PadLeft(4)
                sLine += ._line2(0).Imw.ToString("D").Trim.PadLeft(4)
                subareaFile.Add(sLine)
                sLine = String.Empty
                '/line 3
                With ._line3(0)
                    sLine = .Sno.ToString("F2").Trim.PadLeft(8)
                    sLine += .Stdo.ToString("F2").Trim.PadLeft(8)
                    sLine += .Yct.ToString("F2").Trim.PadLeft(8)
                    sLine += .Xct.ToString("F2").Trim.PadLeft(8)
                    sLine += .Azm.ToString("F2").Trim.PadLeft(8)
                    sLine += .Fl.ToString("F2").Trim.PadLeft(8)
                    sLine += .Fw.ToString("F2").Trim.PadLeft(8)
                    sLine += .Angl.ToString("F2").Trim.PadLeft(8)
                    subareaFile.Add(sLine)
                    sLine = String.Empty
                End With
                '/line 4
                With ._line4(0)
                    If .Wsa > 0 And i > 0 Then
                        'going back to add Ali 03/065/2017
                        'replace to make routing insted of adding because there is a problem in APEX0806 that make the simulation to crash in adding. The result are very similar.
                        If .Wsa < 0.01 Then
                            .Wsa = 0.01
                            'Else
                            'sLine = .Wsa.ToString("F2").Trim.PadLeft(8)
                        End If
                        sLine = (.Wsa * -1).ToString("F2").Trim.PadLeft(8)
                    Else
                        If .Wsa < 0.01 Then
                            .Wsa = 0.01
                            'Else
                        End If
                        sLine = .Wsa.ToString("F2").Trim.PadLeft(8)
                    End If
                    sLine += .chl.ToString("F4").Trim.PadLeft(8)
                    sLine += .Chd.ToString("F2").Trim.PadLeft(8)
                    sLine += .Chs.ToString("F2").Trim.PadLeft(8)
                    sLine += .Chn.ToString("F2").Trim.PadLeft(8)
                    sLine += .Slp.ToString("F4").Trim.PadLeft(8)
                    sLine += .Slpg.ToString("F2").Trim.PadLeft(8)
                    sLine += .Upn.ToString("F2").Trim.PadLeft(8)
                    sLine += .Ffpq.ToString("F2").Trim.PadLeft(8)
                    sLine += .Urbf.ToString("F2").Trim.PadLeft(8)
                    subareaFile.Add(sLine)
                    sLine = String.Empty
                End With
                '/line 5
                With ._line5(0)
                    If _subareaInfo._line4(0).chl <> .Rchl And i > 0 Then .Rchl = _subareaInfo._line4(0).chl
                    'CHANGED ON 03/06/2017 GOING BACK TO ADD BECAUSE OF ROTATIONAL GRAZING IN CBNTT
                    'If (operationNumber > 1 And i = 0) Or i > 0 Then
                    '    sLine = (.Rchl * 0.9).ToString("F4").Trim.PadLeft(8)
                    'Else
                    '    sLine = .Rchl.ToString("F4").Trim.PadLeft(8)
                    'End If
                    sLine = .Rchl.ToString("F4").Trim.PadLeft(8)
                    sLine += .Rchd.ToString("F2").Trim.PadLeft(8)
                    sLine += .Rcbw.ToString("F2").Trim.PadLeft(8)
                    sLine += .Rctw.ToString("F2").Trim.PadLeft(8)
                    sLine += .Rchs.ToString("F2").Trim.PadLeft(8)
                    sLine += .Rchn.ToString("F2").Trim.PadLeft(8)
                    sLine += .Rchc.ToString("F4").Trim.PadLeft(8)
                    sLine += .Rchk.ToString("F4").Trim.PadLeft(8)
                    sLine += .Rfpw.ToString("F0").Trim.PadLeft(8)
                    sLine += .Rfpl.ToString("F4").Trim.PadLeft(8)
                    subareaFile.Add(sLine)
                    sLine = String.Empty
                End With
                '/line 6
                With ._line6(0)
                    sLine = .Rsee.ToString("F2").Trim.PadLeft(8)
                    sLine += .Rsae.ToString("F2").Trim.PadLeft(8)
                    sLine += .Rsve.ToString("F2").Trim.PadLeft(8)
                    sLine += .Rsep.ToString("F2").Trim.PadLeft(8)
                    sLine += .Rsap.ToString("F2").Trim.PadLeft(8)
                    sLine += .Rsvp.ToString("F2").Trim.PadLeft(8)
                    sLine += .Rsv.ToString("F2").Trim.PadLeft(8)
                    sLine += .Rsrr.ToString("F2").Trim.PadLeft(8)
                    sLine += .Rsys.ToString("F2").Trim.PadLeft(8)
                    sLine += .Rsyn.ToString("F2").Trim.PadLeft(8)
                    subareaFile.Add(sLine)
                    sLine = String.Empty
                End With
                '/line 7
                With ._line7(0)
                    sLine = .Rshc.ToString("F3").Trim.PadLeft(8)
                    sLine += .Rsdp.ToString("F2").Trim.PadLeft(8)
                    sLine += .Rsbd.ToString("F2").Trim.PadLeft(8)
                    Dim pp_pond As Single = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.PndF
                    With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
                        If (.PPDSWidth > 0 Or .PPDEWidth > 0 Or .PPTWWidth > 0) Then
                            If Not buffer Then pp_pond = 0.5 Else pp_pond = 0
                        End If
                    End With
                    sLine += pp_pond.ToString("F2").Trim.PadLeft(8)
                    sLine += .Bcof.ToString("F2").Trim.PadLeft(8)
                    sLine += .Bffl.ToString("F2").Trim.PadLeft(8)
                    subareaFile.Add(sLine)
                    sLine = String.Empty
                End With
                '/line 8
                With ._line8(0)
                    If nirr > 0 Then
                        sLine = nirr.ToString("D2").Trim.PadLeft(4)
                    Else
                        sLine = .Nirr.ToString("D2").Trim.PadLeft(4)
                    End If
                    sLine += .Iri.ToString("D").Trim.PadLeft(4)
                    sLine += .Ifa.ToString("D").Trim.PadLeft(4)
                    sLine += .Lm.ToString("D").Trim.PadLeft(4)
                    sLine += .Ifd.ToString("D").Trim.PadLeft(4)
                    sLine += .Idr.ToString("D").Trim.PadLeft(4)
                    sLine += .Idf1.ToString("D").Trim.PadLeft(4)
                    sLine += .Idf2.ToString("D").Trim.PadLeft(4)
                    sLine += .Idf3.ToString("D").Trim.PadLeft(4)
                    sLine += .Idf4.ToString("D").Trim.PadLeft(4)
                    sLine += .Idf5.ToString("D").Trim.PadLeft(4)
                    subareaFile.Add(sLine)
                    sLine = String.Empty
                End With
                With ._line9(0)
                    '/line 9
                    If _subareaInfo._line8(0).Nirr > 0 Then sLine += .Bir.ToString("F2").Trim.PadLeft(8) Else sLine += 0.ToString("F2").Trim.PadLeft(8)
                    sLine += .Efi.ToString("F2").Trim.PadLeft(8)
                    If _subareaInfo._line8(0).Nirr > 0 Then sLine += .Vimx.ToString("F2").Trim.PadLeft(8) Else sLine += 0.ToString("F2").Trim.PadLeft(8)
                    sLine += .Armn.ToString("F2").Trim.PadLeft(8)
                    If _subareaInfo._line8(0).Nirr > 0 Then sLine += .Armx.ToString("F2").Trim.PadLeft(8) Else sLine += 0.ToString("F2").Trim.PadLeft(8)
                    If _subareaInfo._line8(0).Nirr > 0 Then sLine += .Bft.ToString("F2").Trim.PadLeft(8) Else sLine += 0.ToString("F2").Trim.PadLeft(8)
                    sLine += .Fnp4.ToString("F2").Trim.PadLeft(8)
                    sLine += .Fmx.ToString("F2").Trim.PadLeft(8)
                    sLine += .Drt.ToString("F2").Trim.PadLeft(8)
                    sLine += .Fdsf.ToString("F2").Trim.PadLeft(8)
                    subareaFile.Add(sLine)
                    sLine = String.Empty
                End With
                '/line 10
                With ._line10(0)
                    sLine = .Pec.ToString("F2").Trim.PadLeft(8)
                    sLine += .Dalg.ToString("F2").Trim.PadLeft(8)
                    sLine += .Vlgn.ToString("F2").Trim.PadLeft(8)
                    sLine += .Coww.ToString("F2").Trim.PadLeft(8)
                    sLine += .Ddlg.ToString("F2").Trim.PadLeft(8)
                    sLine += .Solq.ToString("F2").Trim.PadLeft(8)
                    sLine += .Sflg.ToString("F2").Trim.PadLeft(8)
                    sLine += .Fnp2.ToString("F2").Trim.PadLeft(8)
                    sLine += .Fnp5.ToString("F2").Trim.PadLeft(8)
                    sLine += .Firg.ToString("F2").Trim.PadLeft(8)
                    subareaFile.Add(sLine)
                    sLine = String.Empty
                End With
                '/line 11
                'If Grazing = True Then
                '    '/line 11
                '    subareaFile.Add(1.ToString.PadLeft(4))
                '    '/line 12
                '    subareaFile.Add(Format(biomass / ac_to_ha, "0.00").PadLeft(8))
                'Else
                '    '/line 11
                '    subareaFile.Add("   0")
                '    '/line 12
                '    subareaFile.Add("    0.00")
                '    Grazing = False
                'End If
                With ._line11(0)
                    sLine = .Ny1.ToString("D").Trim.PadLeft(4)
                    sLine += .Ny2.ToString("D").Trim.PadLeft(4)
                    sLine += .Ny3.ToString("D").Trim.PadLeft(4)
                    sLine += .Ny4.ToString("D").Trim.PadLeft(4)
                    subareaFile.Add(sLine)
                    sLine = String.Empty
                End With
                With ._line12(0)
                    sLine = .Xtp1.ToString("F2").Trim.PadLeft(8)
                    sLine += .Xtp2.ToString("F2").Trim.PadLeft(8)
                    sLine += .Xtp3.ToString("F2").Trim.PadLeft(8)
                    sLine += .Xtp4.ToString("F2").Trim.PadLeft(8)
                    subareaFile.Add(sLine)
                    sLine = String.Empty
                End With
                'floodPlain = 0.0
                'If SBSSelected = True Then
                '    If rchc > 0.01 Then
                '        rchc = 0.01
                '    End If
                '    If rchk > 0.01 Then
                '        rchk = 0.01
                '    End If
                '    rchn = 0.2
                '    chn = 0.1
                'End If
                ''/line 4
                'slopeLength = calcSlopeLength(soilSlope)
                'subareaFile.Add(currentAcreTmp.ToString("F2").PadLeft(8) & sCHL.ToString("F2").PadLeft(8) & "     .00   .0000" & chn.ToString("F2").PadLeft(8) & (soilSlope / 100).ToString("F3").PadLeft(8) & slopeLength.ToString("F2").PadLeft(8) & "   .0000" & "     .00    0.00")
                ''/line 5
                'If streamFencing = 1 Then
                '    subareaFile.Add(Format(rCHL.ToString("F2").PadLeft(8)) & "    0.00    0.00    0.00" & Format(Math.Round(0, 3), "0.000").ToString.PadLeft(8) & rchn.ToString("F2").PadLeft(8) & rchc.ToString("F2").PadLeft(8) & rchk.ToString("F2").PadLeft(8) & floodPlain.ToString("F2").PadLeft(8) & "     .00")
                'Else
                '    subareaFile.Add(Format(rCHL.ToString("F2").PadLeft(8)) & "    0.00    0.00    0.00" & Format(Math.Round(0, 3), "0.000").ToString.PadLeft(8) & rchn.ToString("F2").PadLeft(8) & rchc.ToString("F2").PadLeft(8) & rchk.ToString("F2").PadLeft(8) & floodPlain.ToString("F2").PadLeft(8) & "     .00")
                'End If
                ''/line 6
                'subareaFile.Add("    0.00    0.00    0.00    0.00    0.00    0.00    0.00   0.000    0.00   0.000")
                'With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
                '    If .PPDEWidth > 0 Or .PPDSWidth > 0 Or .PPTWWidth > 0 Then
                '        '/line 7
                '        subareaFile.Add("    0.00    0.00    0.00" & Format(0.5, "0.00").PadLeft(8) & "    0.00    0.00    0.00   0.000    0.00   0.000")
                '    Else
                '        '/line 7
                '        subareaFile.Add("    0.00    0.00    0.00" & Format(PNDFract, "0.00").PadLeft(8) & "    0.00    0.00    0.00   0.000    0.00   0.000")
                '    End If
                'End With

                'Dim FertInIrr As Integer = 0
                'FNP = FertInIrr
                'If Val(AFEff > 0) Then
                '    '/line 8
                '    subareaFile.Add(AFirrigationType.ToString.PadLeft(4) & AFIri.ToString.PadLeft(4) & "   0" & Format(lm, "0").PadLeft(4) & "   0" & drainDepthmm.ToString("D4") & "   1   0   0   1" & "   0")
                '    '/line 9
                '    subareaFile.Add(AFStress.ToString("F2").PadLeft(8) & Format(1 - AFEff, "0.00").PadLeft(8) & " 5000.00     .00" & AFMaxVol.ToString.Trim.PadLeft(8) & "     1.0" & "    0.00     0.0     2.0    0.0")
                '    ''/line 9
                'Else
                '    If irrigationType > 0 Then
                '        Select Case irrigationType
                '            Case 1 'Line 8 - IRR Code - Sprinkler
                '                subareaFile.Add("  01" & Format(IrrIri, "0").PadLeft(4) & "   0   0   0" & drainDepthmm.ToString("D1").PadLeft(4) & "   0   0   0   0   0")
                '            Case 2 'Line 8 - IRR Code - Furrow
                '                subareaFile.Add("  02" & Format(IrrIri, "0").PadLeft(4) & "   0   0   0" & drainDepthmm.ToString("D4") & "   0   0   0   0   0")
                '            Case 3 'Line 8 - IRR Code - Drip
                '                subareaFile.Add("  05" & Format(IrrIri, "0").PadLeft(4) & "   0   0   0" & drainDepthmm.ToString("D4") & "   0   0   0   0   0")
                '            Case 7 'Line 8 - IRR Code - Furrow Dike
                '                subareaFile.Add("  02" & Format(IrrIri, "0").PadLeft(4) & "   0   0   1" & drainDepthmm.ToString("D4") & "   0   0   0   0   0")
                '        End Select
                '        '/line 9
                '        If irrigationType = 7 Then
                '            subareaFile.Add(Format(Val(irrStress), "0.00").PadLeft(8) & Format(1 - IrrEff, "####0.00").PadLeft(8) & " 5000.00     .00" & MaxVol.ToString.Trim.PadLeft(8) & "     0.0" & FNP.ToString("F2").PadLeft(8) & "     0.0     2.0" & safty.ToString.PadLeft(8))
                '        Else
                '            subareaFile.Add(Format(Val(irrStress), "0.00").PadLeft(8) & Format(1 - IrrEff, "####0.00").PadLeft(8) & " 5000.00     .00" & MaxVol.ToString.Trim.PadLeft(8) & "     0.0" & FNP.ToString("F2").PadLeft(8) & "     0.0     2.0     0.0")
                '        End If
                '    Else
                '        '/line 8  '1 in first column (irrigation added to allow the application in manual irrigation. without this 1 no manual irrigation occur)
                '        subareaFile.Add("   1   0   0" & Format(lm, "0").PadLeft(4) & "   0" & drainDepthmm.ToString("D4") & "   0   0   0   0   0   0")
                '        '/line 9
                '        subareaFile.Add("     .00     .00    0.00     .00     .00     0.0     .00     0.0     2.0     0.0")
                '    End If
                'End If
                ''/line 10
                'If WWCrop > 0 Then pec = 1.0
                'If WWCrop > 0 And TSSelected = True Then pec = 0.4 * pec 'Calculate PEC for WW and PEC for TS multiplaying them
                'If CBSelected = 1 Or TSSelected = True Or WWCrop > 0 Then
                '    subareaFile.Add(Format(pec, "   0.000") & "     .00     .00     .00     .00     0.0     .00     0.0     .00     0.0")
                'Else
                '    subareaFile.Add("     1.0     .00     .00     .00     .00     0.0     .00     0.0     .00     0.0")
                'End If
                'soils -= 1
                'n += 1
            End With

            Return "OK"

        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Function getAnimalCode(ByRef animal As String) As Short
        Dim animalCode As Short

        Select Case True
            Case animal.Contains("Dairy")
                animalCode = 43
            Case animal.Contains("Beef")
                animalCode = 44
            Case animal = "Sheep"
                animalCode = 47
            Case animal = "Horse"
                animalCode = 49
            Case animal = "Llama" Or animal = "Alpaca" Or animal = "Buffalo" Or animal.Contains("Emu") Or animal = "Boiler"
                animalCode = 52
            Case animal = "Sheep"
            Case animal = "Swine"
                animalCode = 46
            Case Else
                animalCode = 56
        End Select
        Return animalCode
    End Function

    Private Function FixOperationFile(ByVal drIn As IEnumerable(Of OperationsData))
        Dim drOuts As New List(Of OperationsData)
        Dim drOut As OperationsData
        Dim totalRecords As Integer
        Dim firstDate, lastDate, lastYear As String
        Dim i, j As Integer
        Dim k As Integer = 0

        'drOut = drIn.Clone
        'Array.Clear(drOut, 0, drIn.GetUpperBound(0))
        totalRecords = drIn.Count - 1
        firstDate = Format(drIn(0).Month, "00") & Format(drIn(0).Day, "00")
        lastDate = Format(drIn(totalRecords).Month, "00") & Format(drIn(totalRecords).Day, "00")
        If firstDate > lastDate Then
            lastYear = drIn(totalRecords).Year
            'if this is the case the operation for the last year need to be put before the first record.
            For i = 0 To drIn.Count - 1
                If lastYear = drIn(i).Year Then
                    Exit For
                End If
            Next
            For j = i To drIn.Count - 1
                drOut = New OperationsData
                drOut = drIn(j)
                drOut.Year = "1"
                'drIn(j).year = "1"
                'drOuts(k) = drIn(j)
                drOuts.Add(drOut)
                'k += 1
            Next
            If i > 0 Then
                For j = 0 To i - 1
                    drOut = drIn(j)
                    drOuts.Add(drOut)
                    'drOuts(k) = drIn(j)
                    'k += 1
                Next
            End If
            Return drOuts
        Else
            Return drIn
        End If
    End Function

    Public Function CreateHerdFile(ByVal animals As Single, hours As Single, ByVal animalGroup As String, soilPerc As Single) As Single
        Dim manureProduced, bioConsumed, urineProduced As Single
        Dim manureId
        Dim animalField As Integer
        'Dim totalFields As Short = _fieldsInfo1(currentFieldNumber)._soilsInfo.Count

        'totalAnimals = 0
        'swHerd = New StreamWriter(tAPEXLoc & "\HERD0806.dat", False)
        'calculate number of animals.
        Select Case animalGroup
            Case "Dairy"    '1
                manureProduced = 3.9
                bioConsumed = 9.1
                urineProduced = 11.8
                manureId = 43
            Case "Dairy-dry cow"    '2
                manureProduced = 5.5
                bioConsumed = 9.1
                urineProduced = 11.8
                manureId = 43
            Case "Dairy-calf and heifer"     '3
                manureProduced = 5.5
                bioConsumed = 9.1
                urineProduced = 11.8
                manureId = 43
            Case "Dairy bull"     '4
                manureProduced = 3.9
                bioConsumed = 9.1
                urineProduced = 8.2
                manureId = 43
            Case "Beef"    '5
                manureProduced = 3.9
                bioConsumed = 9.1
                urineProduced = 8.2
                manureId = 44
            Case "Beef-bull"     '6
                manureProduced = 3.9
                bioConsumed = 9.1
                urineProduced = 8.2
                manureId = 44
            Case "Beef-feeder yearling"    '7
                manureProduced = 3.9
                bioConsumed = 9.1
                urineProduced = 8.2
                manureId = 44
            Case "Beef-calf"    '8
                manureProduced = 3.9
                bioConsumed = 9.1
                urineProduced = 8.2
                manureId = 44
            Case "Sheep"    '9
                manureProduced = 5
                bioConsumed = 9.0
                urineProduced = 6.8
                manureId = 47
            Case "Horse"   '10
                manureProduced = 6.8
                bioConsumed = 9.1
                urineProduced = 4.5
                manureId = 49
            Case "Llama"    '11
                manureProduced = 5
                bioConsumed = 9.1
                urineProduced = 6.8
                manureId = 52
            Case "Alpaca"   '12
                manureProduced = 5.1
                bioConsumed = 9.1
                urineProduced = 6.8
                manureId = 52
            Case "Buffalo"   '13
                manureProduced = 3.9
                bioConsumed = 9.1
                urineProduced = 8.2
                manureId = 52
            Case "Emu (breeding stock)"   '14
                manureProduced = 10
                bioConsumed = 9.1
                urineProduced = 6.8
                manureId = 52
            Case "Emu (young birds)"    '15
                manureProduced = 9.8
                bioConsumed = 9.0
                urineProduced = 6.8
                manureId = 52
            Case "Swine"    '16
                manureProduced = 5
                bioConsumed = 9.1
                urineProduced = 17.7
                manureId = 46
            Case "Broiler"    '17
                manureProduced = 10.4
                bioConsumed = 10
                urineProduced = 6.8
                manureId = 52
            Case Else
                manureProduced = 12
                bioConsumed = 9.08
                manureId = 56
        End Select

        If animals < 1 Then
            animals = 1
        End If
        If _animals.Count = 0 Then LoadAnimalUnits()
        For Each animal In _animals
            If animal.Number.Split("|")(0) = manureId Then
                conversionUnit = animal.ConversionUnit
            End If
        Next
        'Dim totalSoils = 1
        'If _fieldsInfo1(currentFieldNumber)._soilsInfo.Count = 1 Then totalSoils = 2
        'ReDim animalField(_fieldsInfo1(currentFieldNumber)._soilsInfo.Where(Function(x) x.Selected).Count)
        'Dim totalPercentage As Single = _fieldsInfo1(currentFieldNumber)._soilsInfo.Where(Function(y) y.Selected).Sum(Function(x) x.Percentage)
        lastHerd += 1

        animalField = animals * soilPerc / 100
        herdFile = (lastHerd).ToString.PadLeft(4) 'For different owners
        'comentarized because there is not field divided anymore
        'If _fieldsInfo1(currentFieldNumber)._soilsInfo.Count = 1 Then
        '    herdFile &= Format(CInt((animalField(0) / 2) * conversionUnit), "#####0.0").PadLeft(8)
        'Else
        '    herdFile &= Format(CInt(animalField(i) * conversionUnit), "#####0.0").PadLeft(8)
        'End If
        herdFile &= Format(animalField * conversionUnit, "#####0.0").PadLeft(8)
        herdFile &= Format(manureId, "#####0.0").PadLeft(8)
        herdFile &= Format((24 - hours) / 24, "####0.00").PadLeft(8)
        herdFile &= Format(bioConsumed, "####0.00").PadLeft(8)
        herdFile &= Format(manureProduced, "####0.00").PadLeft(8)
        herdFile &= urineProduced.ToString.PadLeft(8)
        herdList.Add(herdFile)
        'comentarized because there is not field divided anymore
        'duplicate in case it is just one soil because the area is divided in two equal fields.
        'If _fieldsInfo1(currentFieldNumber)._soilsInfo.Count = 1 Then
        '    herdList.Add(herdFile)
        'End If

        herdFile &= ""
        Return bioConsumed
    End Function

    Public Sub changeTillForDepthFile(oper As Integer, depthAnt As String, sAPEXLoc As String, firstTime As Boolean)
        Dim srFile As StreamReader = Nothing
        Dim swFile As StreamWriter = Nothing
        Dim found As Boolean = False

        Try
            Dim temp As String = String.Empty
            Dim code As String = String.Empty
            Dim newValues As String = "  " & oper
            If firstTime Then
                srFile = New StreamReader(File.OpenRead(sAPEXLoc & "\TILLOrg.DAT"))
                firstTime = False
            Else
                srFile = New StreamReader(File.OpenRead(sAPEXLoc & "\TILL.DAT"))
            End If
            swFile = New StreamWriter(File.OpenWrite(sAPEXLoc & "\TILLNew.dat"))
            newValues &= " C:FERT 5 CUST      5.      0.      0.      0.      0.     0.0    0.00   0.000   0.000   0.000   0.000   0.000   0.000   0.000   0.000   0.000   0.000"
            newValues &= Format(Val(depthAnt), "#.000").ToString.PadLeft(8)
            newValues &= "   0.000   0.000   0.000   0.000   9.000   0.000   0.000   0.000   0.000   5.000   5.363  FERTILIZER APP        " & oper
            swFile.WriteLine(srFile.ReadLine())
            swFile.WriteLine(srFile.ReadLine())
            Do While srFile.EndOfStream <> True
                temp = srFile.ReadLine
                code = temp.Substring(2, 3)

                If CShort(code) = oper Then
                    found = True
                    swFile.WriteLine(newValues)
                Else
                    If Not CShort(code) = oper Then
                        swFile.WriteLine(temp)
                    End If
                End If
            Loop
            If found = False Then swFile.WriteLine(newValues)

            srFile.Close()
            srFile.Dispose()
            srFile = Nothing
            swFile.Close()
            swFile.Dispose()
            swFile = Nothing

            File.Copy(sAPEXLoc & "\TILLNew.dat", sAPEXLoc & "\TILL.DAT", True)
        Catch ex As System.Exception
            If Not srFile Is Nothing Then
                srFile.Close()
                srFile.Dispose()
                srFile = Nothing
            End If
            If Not swFile Is Nothing Then
                swFile.Close()
                swFile.Dispose()
                swFile = Nothing
            End If
            File.Copy(sAPEXLoc & "\TILLNew.dat", sAPEXLoc & "\TILL.DAT", True)
        End Try
    End Sub
    Public Sub changeTillForDepth(oper As Integer, depthAnt As String)
        Dim code As String = String.Empty
        Dim newLine As String = "  " & oper
        newLine &= " C:FERT 5 CUST      5.      0.      0.      0.      0.     0.0    0.00   0.000   0.000   0.000   0.000   0.000   0.000   0.000   0.000   0.000   0.000"
        newLine &= Format((depthAnt) * 25.4, "####0.00").PadLeft(8)
        newLine &= "   0.000   0.000   0.000   0.000   9.000   0.000   0.000   0.000   0.000   5.000   5.363  FERTILIZER APP        " & oper
        changeTillDepth.Add(newLine)
    End Sub

    Public Sub ChangeFert(ByVal NO3N As Single, ByVal PO4P As Single, ByVal OrgN As Single, ByVal OrgP As Single, ByRef fert As Short, NH3 As Single, k As Single)
        Dim newLine As String
        'Dim srFile As StreamReader
        'Dim swFile As StreamWriter
        'Dim temp, fertCode1, newLine As String

        'srFile = New StreamReader(sAPEXLoc & "\fert0806.dat")
        'swFile = New StreamWriter(sAPEXLoc & "\fert.dat")

        'Do While srFile.EndOfStream <> True
        '    temp = srFile.ReadLine
        '    If temp.Trim = "" Then Exit Do
        '    fertCode1 = Left(temp, 5)
        '    If fertCode1 = fert Then
        newLine = fert.ToString("D").PadLeft(5)
        newLine = newLine & " " & "Manure  "
        newLine = newLine & " " & NO3N.ToString.PadLeft(7)
        newLine = newLine & " " & PO4P.ToString.PadLeft(7)
        newLine = newLine & " " & k.ToString.PadLeft(7)
        newLine = newLine & " " & OrgN.ToString.PadLeft(7)
        newLine = newLine & " " & OrgP.ToString.PadLeft(7)
        newLine = newLine & " " & NH3.ToString.PadLeft(7)
        newLine = newLine & "   0.350   0.000   0.000"
        changeFertLine.Add(newLine)
    End Sub

    Public Sub AddFert(ByVal no3n As Single, ByVal po4p As Single, ByVal orgN As Single, ByVal orgP As Single, ByVal type As String, NH3 As Single, k As Single)
        Dim newLine As String = String.Empty
        Dim exist As Boolean = False
        Dim count As UShort = 0

        For Each currentNutrient In _CurrentNutrients
            If currentNutrient.no3 = no3n And currentNutrient.po4 = po4p And currentNutrient.orgn = orgN And currentNutrient.orgp = orgP Then
                exist = True
                fertCode = currentNutrient.code
                Exit For
            End If
        Next


        If Not exist Then
            Dim newNutrient As New CurrentNutrients
            fertCode += 1
            newNutrient.code = fertCode
            newNutrient.no3 = no3n
            newNutrient.po4 = po4p
            newNutrient.orgn = orgN
            newNutrient.orgp = orgP
            _CurrentNutrients.Add(newNutrient)

            newLine = fertCode.ToString.PadLeft(5)
            newLine = newLine & " " & ("Fert " & fertCode.ToString).Trim.PadLeft(8)
            newLine = newLine & " " & Format(no3n, "0.0000").ToString.PadLeft(7)
            newLine = newLine & " " & Format(po4p, "0.0000").ToString.PadLeft(7)
            newLine = newLine & " " & Format(k, "0.0000").ToString.PadLeft(7)
            newLine = newLine & " " & Format(orgN, "0.0000").ToString.PadLeft(7)
            newLine = newLine & " " & Format(orgP, "0.0000").ToString.PadLeft(7)
            Dim OrgC As Single = 0
            Select Case True
                Case type.Contains("Commercial")
                    NH3 = 0
                Case type.Contains("Solid")
                    OrgC = 0.35
                Case type.Contains("Liquid")
                    OrgC = 0.15
            End Select
            newLine = newLine & " " & Format(NH3, "0.0000").ToString.PadLeft(7)
            newLine = newLine & " " & Format(OrgC, "0.0000").ToString.PadLeft(7)
            newLine = newLine & "   0.000   0.000"
            newFertLine.Add(newLine)
        End If
    End Sub

    Public Sub createSiteFile()
        Dim APEXStrings As New StringBuilder
        Dim swFile As StreamWriter = Nothing
        Dim dtNow1 As Date = Date.Now
        Dim sReturn As String = "OK"
        Dim nlat, nlong As Single

        Try
            Select Case _startInfo.Status.ToLower
                Case googleMaps.ToLower
                    nlat = _startInfo.weatherLat
                    nlong = _startInfo.weatherLon
                Case stateCounty.ToLower
                    nlat = _startInfo.weatherLat
                    nlong = _startInfo.weatherLon
                Case userInput.ToLower
                    nlat = 40.12
                    nlong = -103.17
            End Select

            APEXStrings.Length = 0
            swFile = New StreamWriter(File.OpenWrite(sAPEXBasLoc & "\APEX.sit"))
            'line 1
            APEXStrings.Append(" .sit file Subbasin:1  Date: " & dtNow1 + ControlChars.CrLf)
            'line 2
            APEXStrings.Append("" + ControlChars.CrLf)
            'line 3
            APEXStrings.Append("" + ControlChars.CrLf)
            'line 4
            APEXStrings.Append(Format(System.Math.Round(nlat, 2), "####0.00").PadLeft(8) & Format(System.Math.Round(nlong, 2), "####0.00").PadLeft(8) & _sitesInfo(0).Elevation.ToString("F2").PadLeft(8) & _sitesInfo(0).Apm.ToString("F2").PadLeft(8) & _sitesInfo(0).Co2x.ToString("F2").PadLeft(8) & _sitesInfo(0).Cqnx.ToString("F2").PadLeft(8) & _sitesInfo(0).Rfnx.ToString("F2").PadLeft(8) & _sitesInfo(0).Upr.ToString("F2").PadLeft(8) & _sitesInfo(0).Unr.ToString("F2").PadLeft(8) + ControlChars.CrLf)
            'line 5
            APEXStrings.Append("" + ControlChars.CrLf)
            'line 6
            APEXStrings.Append("" + ControlChars.CrLf)
            'line 7
            APEXStrings.Append("" + ControlChars.CrLf)
            'line 8
            APEXStrings.Append("" + ControlChars.CrLf)

            'Print lines 1-8 for both (Baseline and Alternative)
            swFile.WriteLine(APEXStrings)
            APEXStrings.Length = 0

            APEXStrings.Append("" + ControlChars.CrLf)
            'Print line 9 for Both - rowLink has nothing
            swFile.WriteLine(APEXStrings)
            APEXStrings.Length = 0
            'line 10
            APEXStrings.Append("" + ControlChars.CrLf)
            APEXStrings.Append("" + ControlChars.CrLf)
            APEXStrings.Append("" + ControlChars.CrLf)
            APEXStrings.Append("" + ControlChars.CrLf)
            'APEXStrings.Append("NLEAP.wth" + ControlChars.CrLf)  '// commentarized for APEX0604
            APEXStrings.Append("" + ControlChars.CrLf)
            APEXStrings.Append("" + ControlChars.CrLf)
            swFile.WriteLine(APEXStrings)
            swFile.Close()
            swFile = Nothing

            swFile = New StreamWriter(File.OpenWrite(sAPEXBasLoc & "\site.Dat"))
            swFile.WriteLine("    1 APEX.sit")
            swFile.WriteLine("")
            swFile.Close()
            swFile = Nothing

            createSoilFiles(soilList, soilInfo)

            CreateOperationFile(opcsFile)

            runScenario(subareaFile, _StartInfo.yearsRotation, _fieldsInfo1(currentFieldNumber)._soilsInfo.Select(Function(x) x.Selected = True).Count, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.CBCrop)

            If _result.message = "OK" Then
                RunScenarioCompleted()
            End If

        Catch ex As System.Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        Finally
            If Not swFile Is Nothing Then
                swFile.Close()
                swFile.Dispose()
                swFile = Nothing
            End If
        End Try

    End Sub

    'Public Function TransferSoils(Session("userGuide") As String, soilList As List(Of String), soilValues As List(Of String), firstTime As Boolean) As String
    '    Dim result As String = "OK"

    '    Try
    '        sAPEXBasLoc = sAPEXBasLoc
    '        result = createSoilFiles(Session("userGuide"), soilList, soilValues, firstTime)
    '        If result <> "OK" Then
    '            Throw New Global.System.Exception("Error creating the soil Files - " & result)
    '        End If

    '        Return result

    '    Catch ex As System.Exception
    '        result = ex.Message
    '        Return result
    '    End Try

    'End Function

    'Public Function TransferOperations(Session("userGuide") As String, operationsList As List(Of String)) As String
    '    Dim result As String
    '    Try
    '        result = CreateOperationFile(operationsList)
    '        If result <> "OK" Then
    '            Throw New Global.System.Exception("Error creating the management files - " & result)
    '        End If

    '        'results.Add(result)
    '        Return result

    '    Catch ex As System.Exception
    '        result = ex.Message
    '        'results.Add(result)
    '        Return result
    '    End Try

    'End Function

    Public Function TransferDNDC(initialDNDCFile As List(Of String), weatherDNDCFile As List(Of String), soilDNDCFile As List(Of String), cropDNDCFile As List(Of String), projectName As String) As String
        Dim result As String = "OK"
        Dim swFile1 As StreamWriter = Nothing
        Dim srFile As StreamReader = Nothing
        Dim tmpRow As String = String.Empty, tmpRow1 As String = String.Empty
        Try

            If File.Exists(sAPEXBasLoc & "\DnDc\" & projectName & ".dnd") Then File.Delete(sAPEXBasLoc & "\DnDc" & projectName & ".dnd")
            If Not File.Exists(sAPEXBasLoc & "\APEX.wth") Then
                Throw New Global.System.Exception("Error: Weather file dose not existe. Please run simulation first and the try again")
            End If
            srFile = New StreamReader(sAPEXBasLoc & "\APEX.wth")
            swFile1 = New StreamWriter(File.Create(sAPEXBasLoc & "\DnDc\" & projectName & ".dnd"))
            'write initial information
            For Each row In initialDNDCFile
                swFile1.WriteLine(row)
            Next
            'write weather information
            For Each row In weatherDNDCFile
                swFile1.WriteLine(row)
            Next
            'write soil information
            For Each row In soilDNDCFile
                swFile1.WriteLine(row)
            Next
            'write crop information
            For Each row In cropDNDCFile
                swFile1.WriteLine(row)
            Next
            'write all of the weather files
            Dim yearAnt As UInteger = 0
            Dim i As UShort = 0, day As UShort = 0
            Dim tmpRowYear As String = String.Empty
            Do While srFile.EndOfStream <> True
                tmpRow = srFile.ReadLine
                tmpRowYear = tmpRow.Substring(2, 4)
                If tmpRowYear <> yearAnt Then
                    day = 0
                    i += 1
                    yearAnt = tmpRowYear
                    swFile1.Flush()
                    swFile1.Close()
                    swFile1.Dispose()
                    swFile1 = Nothing
                    swFile1 = New StreamWriter(sAPEXBasLoc & "\DnDc" & "\climate" & i & ".txt")
                    swFile1.WriteLine(projectName)
                End If
                'transform APEX format to dNdC format
                day += 1
                tmpRow1 = day.ToString("D") & tmpRow.Substring(19, 13) & (CSng(tmpRow.Substring(32, 6)) / 10).ToString("F2").PadLeft(6)
                swFile1.WriteLine(tmpRow1)
            Loop

            If result <> "OK" Then
                Throw New Global.System.Exception("Error creating the DNDC files - " & result)
            End If

            Return result

        Catch ex As System.Exception
            result = ex.Message & " - " & tmpRow
            Return result
        Finally
            If swFile1 IsNot Nothing Then
                swFile1.Close()
                swFile1.Dispose()
                swFile1 = Nothing
            End If

            If srFile IsNot Nothing Then
                srFile.Close()
                srFile.Dispose()
                srFile = Nothing
            End If
        End Try
    End Function

    Public Sub runScenario(ByRef subareaList As List(Of String), ByRef yearsRotation As Short, ByRef numberOfSoils As Short, ByRef chkCBSelected As Single)
        Try
            _result.message = CreateSubareaFile(subareaList)
            If _result.message <> "OK" Then
                Throw New Global.System.Exception("Error creating the subarea file - " & _result.message)
            End If

            'Create bat file to run apex for Baseline
            Dim swfile As StreamWriter
            swfile = New StreamWriter(File.OpenWrite(sAPEXBasLoc & "\RunAPEX.bat"))
            swfile.WriteLine(Drive)
            swfile.WriteLine("cd\")
            swfile.WriteLine("Cd " & sAPEXBasLoc)
            swfile.WriteLine("APEX0806.exe")
            swfile.WriteLine(" ")
            swfile.Close()
            swfile.Dispose()
            swfile = Nothing
            'delete previous runs
            If File.Exists(sAPEXBasLoc & "\APEX001.ACY") Then File.Delete(sAPEXBasLoc & "\APEX001.ACY")
            If File.Exists(sAPEXBasLoc & "\APEX001.ASA") Then File.Delete(sAPEXBasLoc & "\APEX001.ASA")
            If File.Exists(sAPEXBasLoc & "\APEX001.AWS") Then File.Delete(sAPEXBasLoc & "\APEX001.AWS")
            If File.Exists(sAPEXBasLoc & "\APEX001.MSW") Then File.Delete(sAPEXBasLoc & "\APEX001.MSW")
            If File.Exists(sAPEXBasLoc & "\APEX001.NTT") Then File.Delete(sAPEXBasLoc & "\APEX001.NTT")

            'Run APEX0604 Baseline
            _result.message = doAPEXProcess(sAPEXBasLoc & "\RunAPEX.bat")
            If _result.message <> "OK" Then
                Throw New Global.System.Exception("Error running APEX program - " & _result.message)
            Else
                Dim swOutpPutFile As StreamReader = New StreamReader(File.OpenRead(sAPEXBasLoc & "\APEX001.OUT"))
                Dim outPutFile As String = swOutpPutFile.ReadToEnd
                If Not outPutFile.Contains("TOTAL RUN TIME") Then
                    _result.message = "Error running APEX program"
                    Throw New Global.System.Exception("Error running APEX program")
                End If
                swOutpPutFile.Close()
                swOutpPutFile.Dispose()
                swOutpPutFile = Nothing
            End If

            ReadAPEXResults(numberOfSoils, yearsRotation, chkCBSelected)

        Catch ex As System.Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", ex.Message)
        End Try
    End Sub

    Private Sub RunScenarioCompleted()
        _result.message = "OK"

        Try
            If typeOfSimulation = 1 Then
                showMessage(lblMessage, imgIcon, "Orange", "WarningIcon.jpg", msgDoc.Descendants("CalcuateEconomicsMsg").Value.Replace("First", _scenariosToRun(simulationsCount).Field))
                'createFEMTables
                createFEMTables()
                'run FEM
                _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._femInfo = runFEM(femList, _bmpList, _startInfo.countyCode, _startInfo.stationYears, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber).Name, equipmentList, feedList, structureList, otherInputList)
                _result.message = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._femInfo.Message
            End If
            'take today date 
            If _result.message = "OK" Then
                _scenariosToRun(simulationsCount).Date1 = DateTime.Now
                _scenariosToRun(simulationsCount).Message = msgDoc.Descendants("SimulationCompleteMsg").Value.Replace("First", "").Replace("Second", "").Replace("and", "/").Replace("y", "/")
            Else
                _scenariosToRun(simulationsCount).Date1 = New DateTime(1900, 1, 1)
                _scenariosToRun(simulationsCount).Message = msgDoc.Descendants("ProcessFailError").Value & _result.message
            End If
            'upadte last simulation date
            If _scenariosToRun(simulationsCount).Scenario = "Subproject" Then
                _subprojectName(_scenariosToRun(simulationsCount).FieldIndex)._results.lastSimulation = _scenariosToRun(simulationsCount).Date1
            Else
                _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._results.lastSimulation = _scenariosToRun(simulationsCount).Date1
            End If

            If _result.message <> "OK" Then
                showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", _result.message)
            Else
                showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("SimulationCompleteMsg").Value.Replace("First", _scenariosToRun(simulationsCount).Field).Replace("Second", _scenariosToRun(simulationsCount).Scenario))
            End If
            'gvSimulations.DataBind()

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", ex.Message)
        End Try
    End Sub

    Public Function runFEM(FEMList As List(Of String), BMPList As List(Of String), countyCode As String, rotation As UShort, scenario As String, equipmentList As List(Of String), feedList As List(Of String), structureList As List(Of String), otherInputList As List(Of String)) As FEMData
        runFEM = New FEMData
        runFEM.Message = "OK"
        Dim process As String = "createFEMOptionsFile"
        Try
            'Create xmlFile to update FEMTable
            UpdateFEMDataFile(equipmentList, feedList, structureList, otherInputList)
            'Create FEM option file
            runFEM.Message = createFEMOptionsFile(countyCode, rotation, scenario)
            If Not runFEM.Message.Contains("OK") Then
                Throw New Global.System.Exception("Error creating fem Options file - " & runFEM.Message)
            End If
            process = "PopulateFEMDatabase"
            runFEM.Message = PopulateFEMDatabase(FEMList, BMPList)
            If runFEM.Message <> "OK" Then
                Throw New Global.System.Exception("Error populating local database - " & runFEM.Message)
            End If

            'Create bat file to run FEM
            Dim swfile As StreamWriter
            swfile = New StreamWriter(File.OpenWrite(sAPEXBasLoc & "\RunFEM.bat"))
            swfile.WriteLine(Drive)
            swfile.WriteLine("cd\")
            swfile.WriteLine("Cd " & Drive & "\NTT_FEM")
            swfile.WriteLine("FEM.exe ntt " & sAPEXBasLoc & "\NTT_FEMOptions.txt")
            swfile.WriteLine(" ")
            swfile.Close()
            swfile.Dispose()
            swfile = Nothing
            'Run APEX0604 Baseline
            process = "doAPEXProcess2"
            _result.message = doAPEXProcess2(sAPEXBasLoc & "\RunFEM.bat")

            'runFEM.Message = doAPEXProcess1("E:\NTT_FEM\FEM.EXE", sAPEXBasLoc & "\NTT_FEMOptions.txt")
            If runFEM.Message <> "OK" Then
                Throw New Global.System.Exception("Error running FEM  - " & runFEM.Message)
            End If
            process = "GetFEMResults"
            runFEM = GetFEMResults()
            If runFEM.Message <> "OK" Then
                Throw New Global.System.Exception("Error getting FEM Results - " & runFEM.Message)
            End If
            process = "Return runFEM"
            Return runFEM
        Catch ex As Exception
            runFEM.Message = ex.Message & " - " & process
            Return runFEM
        End Try
    End Function

    Private Sub UpdateFEMDataFile(equipmentList As List(Of String), feedList As List(Of String), structureList As List(Of String), otherInputList As List(Of String))
        Dim dtBas As New Data.DataTable()               'Define datatable
        Dim myConnection As New OleDb.OleDbConnection   'Define connection
        Dim items() As String = Nothing                 'array to split the feeds list
        Dim sQuery As String = String.Empty             'Update Query string
        Dim ad As OleDb.OleDbDataAdapter = Nothing             'Define the adapter
        If File.Exists(sAPEXBasLoc & "\SelectedItems.txt") Then File.Delete(sAPEXBasLoc & "\SelectedItems.txt")
        Dim swFile As StreamWriter = New StreamWriter(sAPEXBasLoc & "\SelectedItems.txt")

        Try
            'myConnection.ConnectionString = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source= " & sAPEXBasLoc & "\FEMData1.mdb;"  'Connection String to access database
            myConnection.ConnectionString = ConnectionString(sAPEXBasLoc, "\FEMData1.mdb;")  'Connection String to access database
            myConnection.Open()     'Open the connection
            If feedList.Count > 0 Then swFile.WriteLine("Feeds")
            For i = 0 To feedList.Count - 1
                items = feedList(i).Split(",")
                sQuery = "UPDATE feedsAugmented SET" & _
                        "[Selling price]=" & items(1) & _
                        ",[Purchase price]=" & items(2) & _
                        ",[concentrate]=" & items(3) & _
                        ",[forage]=" & items(4) & _
                        ",[grain]=" & items(5) & _
                        ",[hay]=" & items(6) & _
                        ",[pasture]=" & items(7) & _
                        ",[silage]=" & items(8) & _
                        ",[supplement]=" & items(9) & _
                        " WHERE [Name]='" & items(0) & "'"
                ad = New OleDb.OleDbDataAdapter(sQuery, myConnection)  'add query and connection to the adapter
                ad.Fill(dtBas)          'run the query.
                swFile.WriteLine(items(0))
            Next

            If equipmentList.Count > 0 Then swFile.WriteLine("Equipment")
            For i = 0 To equipmentList.Count - 1
                items = equipmentList(i).Split(",")
                sQuery = "UPDATE machinAugmented SET" & _
                    "[Lease rate]=" & items(1) & _
                    ",[NEWPRICE]=" & items(2) & _
                    ",[NEWHOURS]=" & items(3) & _
                    ",[CURRENT PRICE]=" & items(4) & _
                    ",[HOURSREMAINING]=" & items(5) & _
                    ",[WIDTH]=" & items(6) & _
                    ",[SPEED]=" & items(7) & _
                    ",[FIELDEFFICIENCY]=" & items(8) & _
                    ",[HORSEPOWER]=" & items(9) & _
                    ",[RF1]=" & items(10) & _
                    ",[RF2]=" & items(11) & _
                    ",[IRLOAN]=" & items(12) & _
                    ",[LLOAN]=" & items(13) & _
                    ",[IREQUITY]=" & items(14) & _
                    ",[PDEBT]=" & items(15) & _
                    ",[YEAR]=" & items(16) & _
                    ",[RV1]=" & items(17) & _
                    ",[RV2]=" & items(18) & _
                    " WHERE [Name]='" & items(0) & "'"
                ad = New OleDb.OleDbDataAdapter(sQuery, myConnection)
                ad.Fill(dtBas)
                swFile.WriteLine(items(0))
            Next

            If structureList.Count > 0 Then swFile.WriteLine("Structure")
            For i = 0 To structureList.Count - 1
                items = structureList(i).Split(",")
                sQuery = "UPDATE facilityAugmented SET" & _
                    "[Lease Rate]=" & items(1) & _
                    ",[New Price]=" & items(2) & _
                    ",[New Life]=" & items(3) & _
                    ",[Current Price]=" & items(4) & _
                    ",[Life Remaining]=" & items(5) & _
                    ",[Maintenance Coefficient]=" & items(6) & _
                    ",[Loan Interest Rate]=" & items(7) & _
                    ",[Length of Loan]=" & items(8) & _
                    ",[Interest Rate on Equity]=" & items(9) & _
                    ",[Proportion of Debt]=" & items(10) & _
                    ",[Year]=" & items(11) & _
                    " WHERE [Name]='" & items(0) & "'"
                ad = New OleDb.OleDbDataAdapter(sQuery, myConnection)
                ad.Fill(dtBas)
                swFile.WriteLine(items(0))
            Next

            If otherInputList.Count > 0 Then swFile.WriteLine("OtherInputs")
            For i = 0 To otherInputList.Count - 1
                items = otherInputList(i).Split(",")
                sQuery = "UPDATE [Farm General] SET" & _
                    "[Value]=" & items(1) & _
                    " WHERE [Name]='" & items(0) & "'"
                ad = New OleDb.OleDbDataAdapter(sQuery, myConnection)
                ad.Fill(dtBas)
                swFile.WriteLine(items(0))
            Next

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", ex.Message)
        Finally
            dtBas.Dispose()
            dtBas = Nothing
            If Not ad Is Nothing Then
                ad.Dispose()
                ad = Nothing
            End If
            If Not myConnection Is Nothing Then
                myConnection.Close()
                myConnection.Dispose()
                myConnection = Nothing
            End If
            If Not swFile Is Nothing Then
                swFile.Close()
                swFile.Dispose()
                swFile = Nothing
            End If
        End Try
    End Sub

    Private Function createFEMOptionsFile(countyCode As String, timeHorizon As Short, scenario As String) As String
        Dim writer As New StreamWriter(sAPEXBasLoc & "\NTT_FEMOptions.txt")
        Dim writer1 As New StreamWriter(sAPEXBasLoc & "\fembat01.bat")
        Try
            Dim folder As String = sAPEXBasLoc

            writer.WriteLine("ManureRateCode,Manure")
            writer.WriteLine("LatLongFile," & Drive & "\NTT_FEM\Long_Lat.txt")
            writer.WriteLine("FileDir," & Drive & "\NTT_FEM")
            writer.WriteLine("FEMPath," & Drive & "\NTT_FEM")
            writer.WriteLine("FEMFilesPath," & folder & "\")
            writer.WriteLine("MMSDir," & Drive & "\NTT_FEM")
            writer.WriteLine("FEMProgPath," & Drive & "\NTT_FEM")
            writer.WriteLine("OperationsLibraryFile," & folder & "\Local.mdb")
            writer.WriteLine("FEMOutputFile," & folder & "\NTTFEMOut.mdb")
            writer.WriteLine("TimeHorizon," & timeHorizon)
            writer.WriteLine("NTTPath," & folder)
            writer.WriteLine("COUNTY," & countyCode)
            writer.WriteLine("Scenario," & scenario)
            Dim areaWithoutBMPs As Single = _fieldsInfo1(currentFieldNumber).Area
            With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
                areaWithoutBMPs = areaWithoutBMPs - .FSArea - .PPDEResArea - .PPTWResArea - .RFArea - .WLArea
            End With
            Dim cropUnit As String = "t/ac"
            For i = 0 To _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._results.SoilResults.Crops.cropName.Count - 1
                With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._results.SoilResults.Crops
                    For Each crop In _crops
                        If crop.Name.Trim = .cropName(i).Trim Then
                            cropUnit = crop.YieldUnit & "/ac"
                        End If
                    Next
                    writer.WriteLine("CROP," & .cropName(i) & "," & Math.Round(.cropYield(i), 2) & "," & cropUnit & "," & Math.Round(_fieldsInfo1(currentFieldNumber).Area, 2) & "," & Math.Round(areaWithoutBMPs, 2))
                End With
            Next
            writer.Close()
            writer.Dispose()
            writer = Nothing

            writer1.WriteLine(Left(folder, 2))
            writer1.WriteLine("cd " & folder)
            writer1.WriteLine(Drive & "\NTT_FEM\new2.exe")
            writer1.Close()
            writer1.Dispose()
            writer1 = Nothing

            Return "OK"

        Catch ex As System.Exception
            If Not writer1 Is Nothing Then
                writer1.Close()
                writer1 = Nothing
            End If

            If Not writer Is Nothing Then
                writer.Close()
                writer.Dispose()
                writer = Nothing
            End If
            Return ex.Message

        End Try
    End Function

    Private Function PopulateFEMDatabase(FEMList As List(Of String), BMPList As List(Of String)) As String
        Dim items() As String
        Dim dsBas As New DataSet
        Dim myConnection As New OleDb.OleDbConnection
        Dim sQuery As String = String.Empty
        'Dim dbConnectString As String = "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source= " & sAPEXBasLoc & "\Local.mdb;"
        Dim dbConnectString As String = ConnectionString(sAPEXBasLoc, "\Local.mdb;")
        Dim ad As OleDb.OleDbDataAdapter = Nothing

        Try
            myConnection.ConnectionString = dbConnectString
            If myConnection.State = ConnectionState.Closed Then
                myConnection.Open()
            End If

            'clear the FEM table
            sQuery = "DELETE * FROM FEM"
            ad = New OleDb.OleDbDataAdapter(sQuery, myConnection)
            ad.Fill(dsBas)
            'populate the FEM table with operations.
            For Each item In FEMList
                items = item.Split(",")
                sQuery = "INSERT INTO FEM " & _
                             "(Composite,[Applies To],[State],[Year],[Month],[Day],[APEX Operation Code],Operation,[APEX Crop Code]," & _
                             "Crop,[Year in rotation], [Rotation Length],Frequency, Item1, Value1, Item2, Value2, Item3, " & "Value3, Item4, Value4, Item5, Value5, Item6, Value6, Item7, Value7, Item8, Value8, Item9, Value9) " & _
                             "VALUES ('" & items(0) & "', '" & items(1) & "',  '" & items(2) & "'," & items(3) & ", " & items(4) & "," & items(5) & ",'" & items(6) & "', '" & items(7) & "'," & items(8) & _
                             ", '" & items(9) & "'," & items(10) & ", " & items(11) & ", '" & items(12) & "', '" & items(13) & "', '" & items(14) & "', '" & items(15) & "', '" & items(16) & "', '" & items(17) & "', '" & items(18) & "', '" & items(19) & "', '" & items(20) & "', '" & items(21) & "', '" & _
                             items(22) & "', '" & items(23) & "', '" & items(24) & "', '" & items(25) & "', '" & items(26) & "', '" & items(27) & "', '" & items(28) & "', '" & items(29) & "', '" & items(30) & "')"
                ad = New OleDb.OleDbDataAdapter(sQuery, myConnection)
                ad.Fill(dsBas)
            Next

            'populate the FEM table with bmps.
            For Each item In BMPList
                items = item.Split(",")
                sQuery = "INSERT INTO FEM " & _
                             "(Composite,[Applies To],[State],[Year],[Month],[Day],[APEX Operation Code],Operation,[APEX Crop Code]," & _
                             "Crop,[Year in rotation], [Rotation Length],Frequency, Item1, Value1, Item2, Value2, Item3, " & "Value3, Item4, Value4, Item5, Value5, Item6, Value6, Item7, Value7, Item8, Value8, Item9, Value9) " & _
                             "VALUES ('" & items(0) & "', '" & "BMP" & "',  '" & items(1) & "'," & 0 & ", " & 0 & "," & 0 & ",'" & 0 & "', '" & items(2) & "'," & 0 & _
                             ", '" & "None" & "'," & 0 & ", " & 0 & ", '" & 0 & "', '" & items(3) & "', '" & items(4) & "', '" & items(5) & "', '" & items(6) & "', '" & items(7) & "', '" & items(8) & "', '" & items(9) & "', '" & items(10) & "', '" & items(11) & "', '" & _
                             items(12) & "', '" & items(13) & "', '" & items(14) & "', '" & items(15) & "', '" & items(16) & "', '" & items(17) & "', '" & items(18) & "', '" & items(19) & "', '" & items(20) & "')"
                ad = New OleDb.OleDbDataAdapter(sQuery, myConnection)
                ad.Fill(dsBas)
            Next

            Return "OK"
            ad.Dispose()
            ad = Nothing
            myConnection.Close()
            myConnection.Dispose()
            myConnection = Nothing
            dsBas.Dispose()
            dsBas = Nothing
        Catch ex As System.Exception
            Return ex.Message
        End Try

    End Function

    Private Function GetFEMResults() As FEMData
        GetFEMResults = New FEMData
        Dim i As Integer = 0
        Dim tAcrage As Single
        Dim conBase As New OleDb.OleDbConnection
        Dim dr As OleDb.OleDbDataReader = Nothing

        Try
            If conBase.State = ConnectionState.Open Then
                'deley for the comunication to be open just in case it is still opened.
                For i = 0 To 100000000
                    If conBase.State = ConnectionState.Open Then
                        Exit For
                    End If
                Next i
            End If
            conBase.ConnectionString = ConnectionString(sAPEXBasLoc, "\NTTFEMOut.mdb;")
            'conBase.ConnectionString = "provider= Microsoft.Jet.OLEDB.4.0;Data Source=" & sAPEXBasLoc & "\NTTFEMOut.mdb"
            conBase.Open()
            'open and read baseline values
            Dim cmd As New OleDb.OleDbCommand("Select [Total Revenue], [Total Cost], [Net Returns], [Total Acreage], [Returns to Equity], [Principal and Interest] FROM [FEM Output Summary]", conBase)
            dr = cmd.ExecuteReader()
            While dr.Read
                'accumulate values
                GetFEMResults.TotalRevenue += dr.GetValue(0)
                GetFEMResults.TotalCost += dr.GetValue(1)
                GetFEMResults.NetReturn = dr.GetValue(2)
                tAcrage = dr.GetValue(3)
                GetFEMResults.NetCashFlow += dr.GetValue(4) + dr.GetValue(5)
                i = i + 1
            End While
            GetFEMResults.TotalRevenue = GetFEMResults.TotalRevenue / i / tAcrage
            GetFEMResults.TotalCost = GetFEMResults.TotalCost / i / tAcrage
            GetFEMResults.NetReturn = GetFEMResults.NetReturn / i / tAcrage
            GetFEMResults.NetCashFlow = GetFEMResults.NetCashFlow / i / tAcrage
            GetFEMResults.Message = "OK"
            dr.Close()
            dr = Nothing
            cmd.Dispose()
            cmd = Nothing
            conBase.Dispose()
            conBase = Nothing

            Return GetFEMResults
        Catch ex As System.Exception
            GetFEMResults.Message = ex.Message
            dr.Close()
            dr = Nothing
            conBase.Dispose()
            conBase = Nothing
            Return GetFEMResults
        End Try
    End Function

    Private Sub createFEMTables()
        feedList.Clear()
        For Each feed In _feedTemp.Where(Function(x) x.Selected)
            feedList.Add(feed.Name & coma & feed.SellingPrice & coma & feed.PurchasePrice & coma & feed.Concentrate & coma & feed.Forage & coma & feed.Grain & coma & feed.Hay &
                         coma & feed.Pasture & coma & feed.Silage & coma & feed.Supplement)
        Next
        equipmentList.Clear()
        For Each equip In _equipmentTemp.Where(Function(x) x.Selected)
            equipmentList.Add(equip.Name & coma & equip.LeaseRate & coma & equip.NewPrice & coma & equip.NewHours & coma & equip.CurrentPrice & coma & equip.HoursRemaining &
                              coma & equip.Width & coma & equip.Speed & coma & equip.FieldEfficiency & coma & equip.HorsePower & coma & equip.Rf1 & coma & equip.Rf2 & coma &
                              equip.IrLoan & coma & equip.LLoan & coma & equip.IrEquity & coma & equip.PDebt & coma & equip.Year & coma & equip.Rv1 & coma & equip.Rv2)
        Next
        structureList.Clear()
        For Each struct In _structureTemp.Where(Function(x) x.Selected)
            structureList.Add(struct.Name & coma & struct.LeaseRate & coma & struct.NewPrice & coma & struct.NewLife & coma & struct.CurrentPrice & coma & struct.LifeRemaining & coma &
                              struct.MaintenanceCoefficient & coma & struct.LoanInterestRate & coma & struct.LengthLoan & coma & struct.InterestRateEquity & coma &
                              struct.ProportionDebt & coma & struct.Year)
        Next
        otherInputList.Clear()
        For Each otherInput In _otherTemp.Where(Function(x) x.Selected)
            otherInputList.Add(otherInput.Name & coma & otherInput.Values)
        Next
    End Sub

    Function doAPEXProcess(ByVal sRunBat As String) As String
        Dim myProcess As System.Diagnostics.Process = New System.Diagnostics.Process
        Dim i As Integer
        Dim sReturn As String = ""

        Try
            ' set the file name and the command line args
            myProcess.StartInfo.FileName = "cmd.exe"
            myProcess.StartInfo.Arguments = "/C " & sRunBat & " " & Microsoft.VisualBasic.Chr(34) & " && exit"

            ' start the process in a hidden window
            'myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            myProcess.StartInfo.CreateNoWindow = True

            ' allow the process to raise events
            myProcess.EnableRaisingEvents = True
            ' add an Exited event handler
            AddHandler myProcess.Exited, AddressOf processAPEXExited

            myProcess.Start()
            For i = 0 To 100000000
                If myProcess.HasExited Then
                    Exit For
                End If
            Next i

            If myProcess.ExitCode = 0 Then
                sReturn = "OK"
            Else
                sReturn = "Calculation Process Failed, Check Your Inputs and Try Again"
            End If

            myProcess.Close()
            myProcess.Dispose()
            Return sReturn

        Catch ex As System.Exception
            Return ex.Message
        End Try
    End Function

    'Function doAPEXProcess1(ByVal femEXE As String, ByVal femOptions As String) As String
    '    Dim myProcess As Process = New Process
    '    Dim i As Integer
    '    Dim sReturn As String = ""
    '    Dim fil As String = ""

    '    Try
    '        ' set the file name and the command line args
    '        'myProcess.StartInfo.FileName = sRunBat
    '        myProcess.StartInfo.FileName = femEXE
    '        'myProcess.StartInfo.Arguments = "/C " & sRunBat & " " & Chr(34) & " && exit"
    '        myProcess.StartInfo.Arguments = "ntt " & femOptions
    '        ' start the process in a hidden window
    '        'myProcess.StartInfo.CreateNoWindow = True

    '        ' allow the process to raise events
    '        myProcess.EnableRaisingEvents = True
    '        ' add an Exited event handler
    '        AddHandler myProcess.Exited, AddressOf processAPEXExited

    '        fil = "fem->" & femEXE & " Agrumenst->" & myProcess.StartInfo.Arguments

    '        myProcess.Start()
    '        For i = 0 To 10000000
    '            If myProcess.HasExited Then
    '                Exit For
    '            End If
    '        Next i

    '        fil = "fem->" & femEXE & " Agrumenst1->" & myProcess.StartInfo.Arguments & " ExitCode->"

    '        If myProcess.ExitCode = 0 Then
    '            sReturn = "OK"
    '        Else
    '            sReturn = "Calculation Process Failed, Check Your Inputs and Try Again"
    '        End If

    '        fil = "fem->" & femEXE & " Agrumenst2->" & myProcess.StartInfo.Arguments
    '        myProcess.Close()
    '        myProcess.Dispose()
    '        Return sReturn

    '    Catch ex As System.Exception
    '        Return ex.Message & "File " & fil
    '    End Try
    'End Function

    Function doAPEXProcess2(ByVal sRunBat As String) As String
        Dim myProcess As System.Diagnostics.Process = New System.Diagnostics.Process
        Dim i As Integer
        Dim sReturn As String = ""

        Try
            ' set the file name and the command line args
            myProcess.StartInfo.FileName = "cmd.exe"
            myProcess.StartInfo.Arguments = "/C " & sRunBat & " " & Microsoft.VisualBasic.Chr(34) & " && exit"

            ' start the process in a hidden window
            'myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            myProcess.StartInfo.CreateNoWindow = True

            ' allow the process to raise events
            myProcess.EnableRaisingEvents = True
            ' add an Exited event handler
            AddHandler myProcess.Exited, AddressOf processAPEXExited

            myProcess.Start()
            For i = 0 To 100000000
                If myProcess.HasExited Then
                    Exit For
                End If
            Next i

            If myProcess.ExitCode = 0 Then
                sReturn = "OK"
            Else
                sReturn = "Calculation Process Failed, Check Your Inputs and Try Again"
            End If

            myProcess.Close()
            myProcess.Dispose()
            Return sReturn

        Catch ex As System.Exception
            Return ex.Message
        End Try
    End Function

    Private Sub processAPEXExited(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            bProcessAPEXExited = True
        Catch ex As System.Exception
        End Try
    End Sub

    Public Function CreateParmFile() As String
        CreateParmFile = "OK"
        Dim APEXStrings As New StringBuilder
        Dim swFile As StreamWriter = Nothing
        Dim i, j As Integer

        Try
            'create parm2110.DAT file  -  Used last one sent by Jimmy with APEXWA version (APEX0604)
            APEXStrings.Length = 0
            swFile = New StreamWriter(File.OpenWrite(sAPEXBasLoc & "\PARM.dat"))
            APEXStrings.Append("  90.050  99.950" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("   10.50  100.95" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("   50.10   95.95" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("    0.00    0.00" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("   25.05   75.90" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("    5.10  100.95" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("    5.25   50.95" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("    20.5   80.99" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("    1.10   10.99" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("   10.05  100.90" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("    5.01   20.90" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("    5.05  100.50" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("    1.80    3.99" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("    5.10   20.95" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("   10.10  100.95" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("    3.10   20.99" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("   20.10   50.95" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("    5.10   50.30" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("   10.01   25.95" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("  400.05  600.80" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("    10.5   100.9" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("  100.01  1000.9" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("    1.50    3.99" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("    1.25    5.95" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("   50.10   55.90" + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("                " + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("                " + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("                " + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("                " + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("   50.00   10.00" + Microsoft.VisualBasic.ControlChars.CrLf)
            'LoadParmFile(_parmValues)  'load thos parms that have been added after project was saved.
            For i = 0 To 8
                For j = 0 To 9
                    If (j + i * 10) > _parmValues.Count - 1 Or _parmValues(j + i * 10) Is Nothing Then Exit For
                    Select Case True
                        Case _parmValues(j + i * 10).Value1.ToString.Contains(".")
                            APEXStrings.Append(_parmValues(j + i * 10).Value1.ToString.Trim.PadLeft(8))
                        Case _parmValues(j + i * 10).Value1.ToString.Contains("E")
                            APEXStrings.Append(_parmValues(j + i * 10).Value1.ToString.Replace("E", ".E").Trim.PadLeft(8))
                        Case Else
                            APEXStrings.Append((_parmValues(j + i * 10).Value1.ToString.Trim & ".00").PadLeft(8))
                    End Select
                    If _parmValues(j + i * 10).Value1.ToString.Contains(".") Or _parmValues(j + i * 10).Value1.ToString.Contains("E") Then
                    Else
                    End If
                Next
                APEXStrings.Append(Microsoft.VisualBasic.ControlChars.CrLf)
            Next

            For j = 0 To 8
                If _parmValues(j + i * 10).Value1.ToString.Contains(".") Or _parmValues(j + i * 10).Value1.ToString.Contains("E") Then
                    APEXStrings.Append(_parmValues(j + i * 10).Value1.ToString.Trim.PadLeft(8))
                Else
                    APEXStrings.Append((_parmValues(j + i * 10).Value1.ToString.Trim & ".00").PadLeft(8))
                End If
            Next

            APEXStrings.Append(Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append("    .044     31.     .51     .57     10." + Microsoft.VisualBasic.ControlChars.CrLf)
            APEXStrings.Append(" " + Microsoft.VisualBasic.ControlChars.CrLf)
            swFile.WriteLine(APEXStrings)
            swFile.Close()

            APEXStrings = Nothing
            swFile = Nothing

            Return CreateParmFile

        Catch ex As System.Exception
            Return ex.Message
        Finally
            If Not swFile Is Nothing Then
                swFile.Close()
                swFile.Dispose()
                swFile = Nothing
            End If
        End Try
    End Function

    'Public Sub LoadParmFile(ByVal _parmValues)
    '    Try
    '        Dim parms = GetParmDesc(_startInfo.StateAbrev)
    '        If Not parms.HasRows Then
    '            parms = GetParmDesc("**")
    '        End If
    '        For Each c In parms
    '            _parmValues.Add(New ParmsData With {.Name = c.item("Name"), .Code = c.item("Code"), .Value1 = c.item("Value"), .Range1 = c.item("Range1"), .Range2 = c.item("Range2")})
    '        Next

    '    Catch ex As System.Exception
    '    End Try
    'End Sub

    Public Function CreateControlFile() As String
        CreateControlFile = "OK"
        Dim APEXStrings As New StringBuilder
        Dim swFile As StreamWriter = Nothing
        Dim NO3Irrppm As Single = 0
        Dim j As Integer

        Try
            APEXStrings.Length = 0
            swFile = New StreamWriter(File.OpenWrite(sAPEXBasLoc & "\Apexcont.dat"))
            'calculate years to simulate (finalYear - InitialYear + 5 years of warm up.
            APEXStrings.Append((_startInfo.stationFinalYear - _startInfo.stationInitialYear + 5 + 1).ToString("D").PadLeft(4))
            APEXStrings.Append((_startInfo.stationInitialYear - 5).ToString("D4"))
            For j = 2 To _controlValues.Count - 1
                Select Case j
                    Case Is < 19        'line 1
                        APEXStrings.Append(_controlValues(j).Value1.ToString.Trim.PadLeft(4))
                    Case 19
                        APEXStrings.Append(_controlValues(j).Value1.ToString.Trim.PadLeft(4))
                        APEXStrings.Append(Microsoft.VisualBasic.ControlChars.CrLf)
                    Case Is < 37        'line 2
                        APEXStrings.Append(_controlValues(j).Value1.ToString.Trim.PadLeft(4))
                    Case 37
                        APEXStrings.Append(_controlValues(j).Value1.ToString.Trim.PadLeft(4))
                        APEXStrings.Append(Microsoft.VisualBasic.ControlChars.CrLf)
                    Case Is < 39        'line 3
                        APEXStrings.Append(_controlValues(j).Value1.ToString("F2").Trim.PadLeft(8))
                    Case 40  'no included because is not afecting AF anymore. The AF changed the way it works using just values in subarea file.
                        'If NO3Irrppm > 0 Then
                        '    APEXStrings.Append(Format(NO3Irrppm, "0.00").PadLeft(8))
                        'Else
                        APEXStrings.Append(_controlValues(j).Value1.ToString("F2").Trim.PadLeft(8))
                        'End If
                    Case Is < 47        'line 4
                        APEXStrings.Append(_controlValues(j).Value1.ToString("F2").Trim.PadLeft(8))
                    Case 47
                        APEXStrings.Append(_controlValues(j).Value1.ToString("F2").Trim.PadLeft(8))
                        APEXStrings.Append(Microsoft.VisualBasic.ControlChars.CrLf)
                    Case Is < 57
                        APEXStrings.Append(_controlValues(j).Value1.ToString("F2").Trim.PadLeft(8))
                    Case 57
                        APEXStrings.Append(_controlValues(j).Value1.ToString("F2").Trim.PadLeft(8))
                        APEXStrings.Append(Microsoft.VisualBasic.ControlChars.CrLf)
                    Case Is < 67
                        APEXStrings.Append(_controlValues(j).Value1.ToString("F2").Trim.PadLeft(8))
                    Case 67
                        APEXStrings.Append(_controlValues(j).Value1.ToString("F2").Trim.PadLeft(8))
                        APEXStrings.Append(Microsoft.VisualBasic.ControlChars.CrLf)
                    Case Is < 75
                        APEXStrings.Append(_controlValues(j).Value1.ToString("F2").Trim.PadLeft(8))
                    Case 75
                        APEXStrings.Append(_controlValues(j).Value1.ToString("F2").Trim.PadLeft(8))
                        APEXStrings.Append(Microsoft.VisualBasic.ControlChars.CrLf)
                End Select
            Next
            swFile.WriteLine(APEXStrings)
            APEXStrings = Nothing

            Return CreateControlFile

        Catch ex As System.Exception
            Return ex.Message
        Finally
            If Not swFile Is Nothing Then
                swFile.Close()
                swFile.Dispose()
                swFile = Nothing
            End If
        End Try
    End Function

    Public Function createSoilFiles(listOfSoils As List(Of String), soilsInfo As List(Of String)) As String
        Dim swFile As StreamWriter = Nothing
        Dim swFile1 As StreamWriter = Nothing
        Dim j As Short = 1  'controls the number of soils in each project
        Dim soilName As String = Nothing

        Try

            'create list of soils file if first time 
            'If firstTime Then
            swFile = New StreamWriter(File.OpenWrite(sAPEXBasLoc & "\SOIL.dat"))
            For i = 0 To listOfSoils.Count - 1
                swFile.WriteLine(listOfSoils(i))
            Next
            swFile.Close()
            swFile.Dispose()
            swFile = Nothing
            'End If
            'create soil files
            For i = 0 To soilsInfo.Count - 1
                If soilsInfo(i).Contains("APEX") Then
                    If j > 1 Then
                        'add lines 25 to 44 in each soil file but the last one
                        For l = 24 To 45
                            swFile1.WriteLine(Space(10))
                        Next
                    End If
                    If Not swFile1 Is Nothing Then
                        swFile1.Close()
                        swFile1.Dispose()
                        swFile1 = Nothing
                    End If
                    soilName = soilsInfo(i).Substring(16, 11)
                    swFile1 = New StreamWriter(File.OpenWrite(sAPEXBasLoc & "\" & soilName))
                    j += 1
                End If
                swFile1.WriteLine(soilsInfo(i))
            Next

            'add lines 25 to 44 in the last soil file.
            For l = 24 To 45
                swFile1.WriteLine(Space(10))
            Next


            createSoilFiles = "OK"
            Return createSoilFiles
        Catch ex As System.Exception
            createSoilFiles = "Error: " & ex.Message & " Please fix the problem and try again"
            Return createSoilFiles
        Finally
            If Not swFile1 Is Nothing Then
                swFile1.Close()
                swFile1.Dispose()
                swFile1 = Nothing
            End If
            If Not swFile Is Nothing Then
                swFile.Close()
                swFile.Dispose()
                swFile = Nothing
            End If
        End Try
    End Function

    Private Function CreateOperationFile(ByRef operationsList As List(Of String)) As String
        Dim swFile As StreamWriter = Nothing
        Dim swFileList As StreamWriter = Nothing

        CreateOperationFile = "OK"
        Dim i As UShort = 0 : Dim opNumber As UShort = 0
        Dim depthAnt As String = "0"
        Dim oper As UShort = 799

        Try
            'If firstTransfer Then
            If File.Exists(sAPEXBasLoc & "\opcs.dat") Then File.Delete(sAPEXBasLoc & "\opcs.dat")
            swFileList = New StreamWriter(File.OpenWrite(sAPEXBasLoc & "\opcs.dat"))
            'Else
            'swFileList = New StreamWriter(sAPEXBasLoc & "\opcs.dat", True)
            'End If

            Dim firstTime As Boolean = True
            For i = 0 To operationsList.Count - 1
                If operationsList(i).Trim = "Operation" Then
                    opNumber += 1
                    swFile = New StreamWriter(File.Create(sAPEXBasLoc & "\APEX" & (opNumber).ToString("D3") & ".opc"))
                    swFileList.WriteLine((opNumber).ToString("D5") & " APEX" & (opNumber).ToString("D3") & ".opc")
                    i += 1
                    Do While operationsList(i).Trim <> "End Operation"
                        If operationsList(i).Length > 12 AndAlso (operationsList(i).Substring(11, 2) = "80" Or operationsList(i).Substring(11, 2) = "79") AndAlso (CSng(depthAnt) <> CSng(operationsList(i).Substring(38, 8)) Or firstTime) Then
                            oper = oper + 1
                            depthAnt = operationsList(i).Substring(37, 8)
                            changeTillForDepthFile(oper, depthAnt, sAPEXBasLoc, firstTime)
                            firstTime = False
                            operationsList(i).Substring(5, 3).Replace("580", CStr(oper))
                        End If
                        swFile.WriteLine(operationsList(i))
                        i += 1
                    Loop
                    swFile.Close()
                    swFile.Dispose()
                    swFile = Nothing
                    Continue For
                End If
                If i >= operationsList.Count Then Exit For
                If operationsList(i).Trim = "Filter Strip" Then
                    opNumber += 1
                    swFile = New StreamWriter(File.Create(sAPEXBasLoc & "\FS" & (opNumber).ToString("D3") & ".opc"))
                    swFileList.WriteLine((opNumber).ToString("D5") & " FS" & (opNumber).ToString("D3") & ".opc")
                    i += 1
                    Do While operationsList(i).Trim <> "End Filter Strip"
                        swFile.WriteLine(operationsList(i))
                        i += 1
                    Loop
                    swFile.Close()
                    swFile.Dispose()
                    swFile = Nothing
                    Continue For
                End If

                If i >= operationsList.Count Then Exit For
                If operationsList(i).Trim = "Riparian Forest" Then
                    opNumber += 1
                    swFile = New StreamWriter(File.Create(sAPEXBasLoc & "\RF" & (opNumber).ToString("D3") & ".opc"))
                    swFileList.WriteLine((opNumber).ToString("D5") & " RF" & (opNumber).ToString("D3") & ".opc")
                    i += 1
                    Do While operationsList(i).Trim <> "End Riparian Forest"
                        swFile.WriteLine(operationsList(i))
                        i += 1
                    Loop
                    swFile.Close()
                    swFile.Dispose()
                    swFile = Nothing
                    Continue For
                End If

                If i >= operationsList.Count Then Exit For
                If operationsList(i).Trim = "Contour Buffer" Then
                    opNumber += 1
                    swFile = New StreamWriter(File.Create(sAPEXBasLoc & "\CB" & (opNumber).ToString("D3") & ".opc"))
                    swFileList.WriteLine((opNumber).ToString("D5") & " CB" & (opNumber).ToString("D3") & ".opc")
                    i += 1
                    Do While operationsList(i).Trim <> "End Contour Buffer"
                        swFile.WriteLine(operationsList(i))
                        i += 1
                    Loop
                    swFile.Close()
                    swFile.Dispose()
                    swFile = Nothing
                    Continue For
                End If

                If i >= operationsList.Count Then Exit For
                If operationsList(i).Trim = "Wetland" Then
                    opNumber += 1
                    swFile = New StreamWriter(File.Create(sAPEXBasLoc & "\WL" & (opNumber).ToString("D3") & ".opc"))
                    swFileList.WriteLine((opNumber).ToString("D5") & " WL" & (opNumber).ToString("D3") & ".opc")
                    i += 1
                    Do While operationsList(i).Trim <> "End Wetland"
                        swFile.WriteLine(operationsList(i))
                        i += 1
                    Loop
                    swFile.Close()
                    swFile.Dispose()
                    swFile = Nothing
                    Continue For
                End If
            Next

            If Not swFile Is Nothing Then
                swFile.Close()
                swFile.Dispose()
                swFile = Nothing
            End If

            If Not swFileList Is Nothing Then
                swFileList.Close()
                swFileList.Dispose()
                swFileList = Nothing
            End If
            Return CreateOperationFile

        Catch ex As System.Exception
            If Not swFile Is Nothing Then
                swFile.Close()
                swFile.Dispose()
                swFile = Nothing
            End If
            If Not swFileList Is Nothing Then
                swFileList.Close()
                swFileList.Dispose()
                swFileList = Nothing
            End If
            Return ex.Message
        End Try
    End Function

    Private Function CreateSubareaFile(ByRef subareaList As List(Of String)) As String
        Dim swFile As StreamWriter = Nothing
        CreateSubareaFile = "OK"

        Try
            If File.Exists(sAPEXBasLoc & "\APEX.sub") Then File.Delete(sAPEXBasLoc & "\APEX.sub")
            swFile = New StreamWriter(File.OpenWrite(sAPEXBasLoc & "\APEX.sub"))

            For Each item In subareaList
                swFile.WriteLine(item)
            Next
            swFile.Close()
            swFile.Dispose()
            swFile = Nothing

            Return CreateSubareaFile

        Catch ex As System.Exception
            If Not swFile Is Nothing Then
                swFile.Close()
                swFile.Dispose()
                swFile = Nothing
            End If
            Return ex.Message
        End Try
    End Function

    Private Sub LoadResults(ByRef _resultsData As List(Of ResultsData), APEXStartYear As UShort)
        Dim srFile As StreamReader = Nothing
        Dim tempa As String = String.Empty
        Dim _oneResult As ResultsData
        Dim subs As UShort = 0
        Dim subAnt As UShort = 99
        Dim year As UShort = 0
        Dim irriSum As Single = 0
        Dim dprkSum As Single = 0
        Dim prknSum As Single = 0
        Dim prkpSum As Single = 0
        Dim totalSubs As UShort = 0
        Dim n2o_Annual As Single = 0
        Dim sub_total As UShort = 0

        Try
            srFile = New StreamReader(File.OpenRead(sAPEXBasLoc + "\APEX001.ntt"))
            For i = 1 To 3
                tempa = srFile.ReadLine
            Next

            Do While srFile.EndOfStream <> True
                tempa = srFile.ReadLine
                year = Val(Mid(tempa, 8, 4))
                subs = Val(Mid(tempa, 1, 5))
                If year < APEXStartYear Then Continue Do 'take years greater or equal thanApexStartYear.
                _oneResult = New ResultsData
                If subs = subAnt Then
                    Continue Do 'if subs and subant equal means there are more than one CROP. So info is going to be duplicated. Just one record saved
                End If
                subAnt = subs
                _oneResult.Sub1 = subs
                _oneResult.Year = year
                _oneResult.FLOW = Val(Mid(tempa, 32, 9)) * mm_to_in
                _oneResult.Qdr = Val(Mid(tempa, 127, 9)) * mm_to_in
                _oneResult.SurfaceFlow = Val(Mid(tempa, 255, 9)) * mm_to_in
                _oneResult.SED = Val(Mid(tempa, 41, 9)) * tha_to_tac
                _oneResult.YMNU = Val(Mid(tempa, 181, 9)) * tha_to_tac
                _oneResult.ORGP = Val(Mid(tempa, 59, 9)) * kg_to_lbs / ha_to_ac
                _oneResult.PO4 = Val(Mid(tempa, 77, 9)) * kg_to_lbs / ha_to_ac
                _oneResult.ORGN = Val(Mid(tempa, 50, 9)) * kg_to_lbs / ha_to_ac
                _oneResult.NO3 = Val(Mid(tempa, 68, 9)) * kg_to_lbs / ha_to_ac
                _oneResult.QDRN = Val(Mid(tempa, 145, 9)) * kg_to_lbs / ha_to_ac
                _oneResult.QDRP = Val(Mid(tempa, 264, 9)) * kg_to_lbs / ha_to_ac
                _oneResult.QN = Val(Mid(tempa, 246, 9)) * kg_to_lbs / ha_to_ac
                _oneResult.Dprk = Val(Mid(tempa, 136, 9)) * mm_to_in
                _oneResult.IRRI = Val(Mid(tempa, 238, 8)) * mm_to_in
                _oneResult.N2O = Val(Mid(tempa, 154, 8))
                _oneResult.PRKN = Val(Mid(tempa, 14, 9))
                _oneResult.PRKP = Val(Mid(tempa, 110, 9))
                If subAnt = 0 Then
                    _oneResult.N2O = n2o_Annual / sub_total
                    n2o_Annual = 0
                    sub_total = 0
                Else
                    sub_total += 1
                    n2o_Annual += _oneResult.N2O
                End If
                If subs = 0 Then
                    _oneResult.Dprk = dprkSum / totalSubs
                    _oneResult.IRRI = irriSum / totalSubs
                    _oneResult.PRKN = prknSum / totalSubs
                    _oneResult.PRKP = prknSum / totalSubs
                    irriSum = 0
                    dprkSum = 0
                    prknSum = 0
                    prkpSum = 0
                    totalSubs = 0
                Else
                    irriSum += _oneResult.IRRI
                    dprkSum += _oneResult.Dprk
                    totalSubs += 1
                End If
                _oneResult.PCP = Val(Mid(tempa, 230, 8)) * mm_to_in

                _resultsData.Add(_oneResult)
            Loop

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Error").Value)
        Finally
            If Not srFile Is Nothing Then
                srFile.Close()
                srFile.Dispose()
                srFile = Nothing
            End If
        End Try
    End Sub

    Private Sub LoadCropResults(ByRef _cropsInfo As List(Of CropsInfo), APEXStartYear As UShort)
        Dim srFile As StreamReader = Nothing
        Dim tempa As String = String.Empty
        Dim subs As UShort = 0
        Dim subAnt As UShort = 99
        Dim year1 As UShort = 0
        Dim conversionFactor As Single = 0, dryMatter As Single = 0
        Dim _oneCropInfo As CropsInfo

        Try
            srFile = New StreamReader(File.OpenRead(sAPEXBasLoc + "\APEX001.ACY"))

            For i = 1 To 9
                tempa = srFile.ReadLine
            Next

            Do While srFile.EndOfStream <> True
                tempa = srFile.ReadLine
                year1 = Val(Mid(tempa, 19, 4))
                subs = Val(Mid(tempa, 5, 5))
                If year1 < APEXStartYear Then Continue Do 'take years greater or equal thanApexStartYear.
                _oneCropInfo = New CropsInfo
                _oneCropInfo.name = Mid(tempa, 29, 4)
                conversionFactor = 1 * ac_to_ha
                dryMatter = 100
                For Each crop In _crops.Where(Function(x) x.Code = _oneCropInfo.name.Trim)
                    conversionFactor = crop.ConversionFactor * ac_to_ha
                    dryMatter = crop.DryMatter
                    Exit For
                Next
                If _oneCropInfo.name = "COTS" Or _oneCropInfo.name = "COTP" Then
                    _oneCropInfo.yield = Val(Mid(tempa, 34, 9)) * conversionFactor / (dryMatter / 100)
                Else
                    _oneCropInfo.yield = Val(Mid(tempa, 34, 9)) * conversionFactor / (dryMatter / 100) + Val(Mid(tempa, 44, 9)) * conversionFactor / (dryMatter / 100)
                End If
                _oneCropInfo.ws = Val(Mid(tempa, 64, 9)) * conversionFactor / (dryMatter / 100)
                _oneCropInfo.ns = Val(Mid(tempa, 74, 9)) * conversionFactor / (dryMatter / 100)
                _oneCropInfo.ps = Val(Mid(tempa, 84, 9)) * conversionFactor / (dryMatter / 100)
                _oneCropInfo.ts = Val(Mid(tempa, 94, 9)) * conversionFactor / (dryMatter / 100)
                _oneCropInfo.as1 = Val(Mid(tempa, 104, 9)) * conversionFactor / (dryMatter / 100)
                _oneCropInfo.year = year1
                _cropsInfo.Add(_oneCropInfo)
            Loop

            'Dim cropsByYear As IEnumerable = (From _crops In _cropsInfo Group By _crops.name, _crops.year Into Average(_crops.yield))
            'Dim cropsBySub As IEnumerable = (From _crops In _cropsInfo Group By _crops._crops.name, _crops.year Into Average(_crops.yield))
            If _scenariosToRun(simulationsCount).Scenario <> "Subproject" Then
                AverageCropsAll(_fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._results.SoilResults.Crops, _cropsInfo)
            Else
                AverageCropsAll(_subprojectName(_scenariosToRun(simulationsCount).FieldIndex)._results.SoilResults.Crops, _cropsInfo)
            End If

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Error").Value)
        Finally
            If Not srFile Is Nothing Then
                srFile.Close()
                srFile.Dispose()
                srFile = Nothing
            End If
        End Try
    End Sub

    Private Sub LoadMonthValues(APEXStartYear As UShort, ByRef results As ScenariosData.APEXResults)
        Dim fileMSW = sAPEXBasLoc + "\APEX001.msw"      'monthly values for flow, nutrients, and sediment
        Dim fileMWS = sAPEXBasLoc + "\APEX001.mws"      'monthly values for precipitation
        Dim i As UShort = 0
        Dim srFileTotal As StreamReader = Nothing
        Dim tempa As String = String.Empty

        With results
            ReDim .annualFlow(23)
            ReDim .annualSediment(23)
            ReDim .annualOrgN(23)
            ReDim .annualN2o(23)
            ReDim .annualOrgP(23)
            ReDim .annualNO3(23)
            ReDim .annualPO4(23)
            ReDim .annualPrecipitation(23)
            ReDim .annualCropYield(23)
            'ReDim YearAnt(0)

            totalArea = 0
            'calculate monthly averages starting after first rotation.
            srFileTotal = New StreamReader(File.OpenRead(fileMSW))
            'read titles .
            For i = 0 To 9
                srFileTotal.ReadLine()
            Next

            Dim lastYear As Short = 0   ' keep track of the years to get the average for monthly values.
            Do While srFileTotal.EndOfStream <> True
                tempa = srFileTotal.ReadLine
                If tempa.Substring(1, 4).Trim = String.Empty Then Exit Do
                lastYear = CShort(tempa.Substring(1, 4).Trim)
                If lastYear >= APEXStartYear Then
                    'accumulate the monthly values of simulation for graphs. Index from 12 to 23
                    i = 11 + tempa.Substring(6, 4)
                    .annualFlow(i) = .annualFlow(i) + CSng(tempa.Substring(12, 10)) * mm_to_in
                    .annualSediment(i) = .annualSediment(i) + CSng(tempa.Substring(23, 10)) * tha_to_tac
                    .annualOrgN(i) = .annualOrgN(i) + CSng(tempa.Substring(34, 10)) * 10 * kg_to_lbs / ha_to_ac  'this values is multiply by 10 because the MSW file does this total divided by 10 comparing withthe value in the output file.
                    .annualOrgP(i) = .annualOrgP(i) + CSng(tempa.Substring(45, 10)) * 20 * kg_to_lbs / ha_to_ac  'this values is multiply by 20 because the MSW file does this total divided by 20 comparing withthe value in the output file.
                    .annualNO3(i) = .annualNO3(i) + CSng(tempa.Substring(56, 10)) * kg_to_lbs / ha_to_ac
                    .annualPO4(i) = .annualPO4(i) + CSng(tempa.Substring(67, 10)) * kg_to_lbs / ha_to_ac
                End If
            Loop

            srFileTotal.Close()
            srFileTotal.Dispose()
            srFileTotal = Nothing

            srFileTotal = New StreamReader(File.OpenRead(fileMWS))
            For i = 0 To 8
                srFileTotal.ReadLine()
            Next
            Dim currentCol As UShort = 5    'first month start at 5 and then increase by offset
            Dim offset As UShort = 9        'lenght of each month value
            Dim month As UShort = 1         'month number
            Dim year As UShort = APEXStartYear          'year nubmer
            Dim lastCol As UShort = 113     'Last column available
            Dim lastYear1 As UShort = 0

            Do While srFileTotal.EndOfStream <> True
                tempa = srFileTotal.ReadLine
                If tempa.Substring(1, 4).Trim = String.Empty Then Exit Do
                month = 1
                currentCol = 5
                Short.TryParse(tempa.Substring(1, 4).Trim, lastYear1)
                'lastYear = CShort(tempa.Substring(1, 4).Trim)
                If lastYear1 >= year Then
                    'accumulate the monthly values of simulation for graphs. Index from 12 to 23
                    Do While currentCol < lastCol
                        i = 11 + month
                        .annualPrecipitation(i) += CSng(tempa.Substring(currentCol, offset)) * mm_to_in
                        currentCol += offset
                        month += 1
                    Loop
                End If
            Loop

            srFileTotal.Close()
            srFileTotal.Dispose()
            srFileTotal = Nothing

            lastYear = lastYear - APEXStartYear + 1
            For i = 12 To 23
                .annualFlow(i) /= lastYear
                .annualSediment(i) /= lastYear
                .annualOrgN(i) /= lastYear
                .annualOrgP(i) /= lastYear
                .annualNO3(i) /= lastYear
                .annualPO4(i) /= lastYear
                .annualPrecipitation(i) /= lastYear
            Next

        End With

    End Sub

    Private Sub AverageAll(ByRef _resultsdata As List(Of ResultsData), ByRef soilResult As ScenariosData.APEXResultsAll, i As UShort)
        'Dim i As UShort = 0
        Dim j As UShort = 0
        Dim totalRecords As UShort = 0
        Dim found As Boolean = False
        Dim totalCrops As UShort = 0

        With soilResult
            .OrgN = _resultsdata.Where(Function(x) x.Sub1 = i).Average(Function(x) x.ORGN)
            .SubsurfaceN = _resultsdata.Where(Function(x) x.Sub1 = i).Average(Function(x) x.NO3 - x.QN)
            .RunoffN = _resultsdata.Where(Function(x) x.Sub1 = i).Average(Function(x) x.QN)
            .tileDrainN = _resultsdata.Where(Function(x) x.Sub1 = i).Average(Function(x) x.QDRN)
            .tileDrainP = _resultsdata.Where(Function(x) x.Sub1 = i).Average(Function(x) x.QDRP)
            .OrgP = _resultsdata.Where(Function(x) x.Sub1 = i).Average(Function(x) x.ORGP)
            .PO4 = _resultsdata.Where(Function(x) x.Sub1 = i).Average(Function(x) x.PO4)
            .runoff = _resultsdata.Where(Function(x) x.Sub1 = i).Average(Function(x) x.SurfaceFlow)
            .subsurfaceFlow = _resultsdata.Where(Function(x) x.Sub1 = i).Average(Function(x) x.FLOW - x.SurfaceFlow)
            .tileDrainFlow = _resultsdata.Where(Function(x) x.Sub1 = i).Average(Function(x) x.Qdr)
            .Sediment = _resultsdata.Where(Function(x) x.Sub1 = i).Average(Function(x) x.SED)
            .ManureErosion = _resultsdata.Where(Function(x) x.Sub1 = i).Average(Function(x) x.YMNU)
            .irrigation = _resultsdata.Where(Function(x) x.Sub1 = i).Average(Function(x) x.IRRI)
            .deepPerFlow = _resultsdata.Where(Function(x) x.Sub1 = i).Average(Function(x) x.Dprk)
            .n2o = _resultsdata.Where(Function(x) x.Sub1 = 1).Average(Function(x) x.N2O)
            .LeachedN = _resultsdata.Where(Function(x) x.Sub1 = 1).Average(Function(x) x.PRKN)
            .LeachedP = _resultsdata.Where(Function(x) x.Sub1 = 1).Average(Function(x) x.PRKP)
        End With
    End Sub

    Private Sub AverageTotals(ByRef _resultsData As List(Of ResultsData))
        If _scenariosToRun(simulationsCount).Scenario <> "Subproject" Then
            _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber).CleanResults()
            AverageAll(_resultsData, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._results.SoilResults, 0)
        Else
            _subprojectName(_scenariosToRun(simulationsCount).FieldIndex).CleanResults()
            AverageAll(_resultsData, _subprojectName(_scenariosToRun(simulationsCount).FieldIndex)._results.SoilResults, 0)
        End If
    End Sub

    Private Sub AverageCropsAll(ByRef _cropsResults As ScenariosData.annualCrops, _cropsInfo As List(Of CropsInfo))
        Dim i As UShort = 0
        Dim cropYield As Single = 0
        Dim cropName As String = String.Empty
        Const tStatisticEvaluated As Single = 1.645   't statistic evaluated at 90% confidence iterval
        Dim j As UShort = 0
        Dim _cropsTotal As IEnumerable = (From _crops In _cropsInfo Where _crops.yield > 0 Group By _crops.name Into Average(_crops.yield), Count())

        For Each crop In _cropsTotal
            j += 1
            Exit For
        Next

        If j = 0 Then
            _cropsTotal = From _crops In _cropsInfo Group By _crops.name Into Distinct()
        End If

        With _cropsResults
            For Each crop In _cropsTotal
                ReDim Preserve .cropName(i)
                ReDim Preserve .cropYield(i)
                ReDim Preserve .cropYieldCI(i)
                ReDim Preserve .cropYieldSD(i)
                ReDim Preserve .cropRecords(i)
                .cropName(i) = crop.name.Trim
                cropName = crop.Name.trim
                If j = 0 Then .cropYield(i) = 0 Else .cropYield(i) = crop.Average
                cropYield = .cropYield(i)
                If j = 0 Then .cropYieldCI(i) = 0 Else .cropYieldCI(i) = Math.Sqrt(_cropsInfo.Where(Function(x) x.name.Trim = cropName).Sum(Function(x) (cropYield - x.yield) ^ 2) / crop.count) / Math.Sqrt(crop.count) * tStatisticEvaluated
                i += 1
            Next
        End With

    End Sub

    Private Sub AverageBySoil(ByRef _resultsData As List(Of ResultsData), _cropsData As List(Of CropsInfo))
        Dim totalRecords As UShort = 0
        Dim totalCrops As UShort = 0, totalCropRecords As UShort = 0
        Dim found As Boolean = False
        Dim k As Short = 0
        Dim numberOfSoils As UShort = 0

        For i = 0 To _fieldsInfo1(currentFieldNumber)._soilsInfo.Where(Function(x) x.Selected = True).Count - 1
            found = False
            totalRecords = 0
            _fieldsInfo1(currentFieldNumber)._soilsInfo(i)._scenariosInfo(currentScenarioNumber).CleanResults()
            'average of all but crop yield
            AverageAll(_resultsData, _fieldsInfo1(currentFieldNumber)._soilsInfo(i)._scenariosInfo(currentScenarioNumber)._results.SoilResults, i + 1)
            With _fieldsInfo1(currentFieldNumber)._soilsInfo(i)._scenariosInfo(currentScenarioNumber)._results.SoilResults
                For j = 0 To _cropsData.Count - 1
                    found = False
                    totalCrops = 0
                    If Not .Crops.cropName Is Nothing Then
                        For k = 0 To .Crops.cropName.Count - 1  'look for the crop on soils
                            If Not (.Crops.cropName(k) Is Nothing) AndAlso .Crops.cropName(k).Trim = _cropsData(j).name.Trim Then
                                found = True : Exit For
                            End If
                        Next
                        totalCrops = .Crops.cropName.Count
                    End If
                    If found = False Then  'if croop no found on list means it has to be added.
                        ReDim Preserve .Crops.cropName(totalCrops)
                        ReDim Preserve .Crops.cropYield(totalCrops)
                        ReDim Preserve .Crops.cropYieldCI(totalCrops)
                        ReDim Preserve .Crops.cropYieldSD(totalCrops)
                        ReDim Preserve .Crops.cropRecords(totalCrops)
                        k = .Crops.cropName.Count - 1
                        .Crops.cropName(k) = _cropsData(j).name.Trim
                    End If
                    .Crops.cropYield(k) += _cropsData(j).yield
                    If _cropsData(j).yield > 0 Then .Crops.cropRecords(k) += 1
                Next

                For k = 0 To .Crops.cropName.Count - 1
                    .Crops.cropYield(k) /= .Crops.cropRecords(k)
                Next
            End With
            numberOfSoils += 1
        Next

    End Sub

    Private Sub LoadAnnualValues(ByRef _resultsData As List(Of ResultsData), nAvgyears As UShort, ByRef _cropsInfo As List(Of CropsInfo))
        Dim i As UShort = 0
        Dim results As ScenariosData.APEXResults

        If ddlType.SelectedIndex = 0 Then
            results = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._results
        Else
            results = _subprojectName(ddlSubproject.SelectedIndex)._results
        End If
        With results
            For j = 0 To _resultsData.Count - 1
                If nAvgyears <= _resultsData(j).Year Then
                    If _resultsData(j).Sub1 = 0 Then
                        'accumulate the last 12 years of simulation for graphs. Indexes 0 to 11
                        .annualFlow(i) = _resultsData(j).FLOW
                        .annualSediment(i) = _resultsData(j).SED
                        .annualOrgP(i) = _resultsData(j).ORGP
                        .annualPO4(i) = _resultsData(j).PO4
                        .annualOrgN(i) = _resultsData(j).ORGN
                        .annualNO3(i) = _resultsData(j).NO3
                        .annualN2o(i) = _resultsData(j).N2O
                        i += 1
                    Else
                        If _resultsData(j).Sub1 = 1 Then
                            .annualPrecipitation(i) = _resultsData(j).PCP
                        End If

                    End If
                End If
            Next

            'redim all of the annualcrop years (0-11) to the total number of crops
            Dim cropsNumber = .SoilResults.Crops.cropName.Count - 1
            For i = 0 To .annualCropYield.Count - 1
                With .annualCropYield(i)
                    ReDim .cropName(cropsNumber)
                    ReDim .cropYield(cropsNumber)
                    ReDim .cropRecords(cropsNumber)
                End With
            Next

            'average crop yield for each year for each crop
            i = 0
            For j = 0 To _cropsInfo.Count - 1
                If nAvgyears <= _cropsInfo(j).year Then
                    'accumulate the last 12 years of simulation for graphs. Indexes 0 to 11
                    i = _cropsInfo(j).year - nAvgyears
                    For k = 0 To .SoilResults.Crops.cropName.Count - 1
                        If .SoilResults.Crops.cropName(k) = _cropsInfo(j).name Then
                            .annualCropYield(i).cropName(k) = _cropsInfo(j).name
                            .annualCropYield(i).cropYield(k) += _cropsInfo(j).yield
                            .annualCropYield(i).cropRecords(k) += 1
                        End If
                    Next
                    i += 1
                End If
            Next

            For i = 0 To 11
                For k = 0 To .annualCropYield(i).cropYield.Count - 1
                    .annualCropYield(i).cropYield(k) /= .annualCropYield(i).cropRecords(k)
                Next
            Next
        End With

    End Sub

    Private Sub CalculateCIbySoil(ByRef _resultsData As List(Of ResultsData), ByRef _cropsData As List(Of CropsInfo))
        Dim totalRecords As UShort = 0, totalCrops As UShort = 0, totalCrops0 As UShort = 0
        Const tStatisticEvaluated As Single = 1.645   't statistic evaluated at 90% confidence iterval
        Dim found As Boolean = False, found0 As Boolean = False
        Dim k As UShort = 0, k0 As UShort = 0

        If _scenariosToRun(simulationsCount).Scenario <> "Subproject" Then
            For i = 0 To _fieldsInfo1(_scenariosToRun(simulationsCount).FieldIndex)._soilsInfo.Where(Function(x) x.Selected).Count - 1
                CalculateCIAll(_resultsData, _fieldsInfo1(currentFieldNumber)._soilsInfo(i)._scenariosInfo(_scenariosToRun(simulationsCount).ScenarioIndex)._results.SoilResults, i + 1)
                With _fieldsInfo1(_scenariosToRun(simulationsCount).FieldIndex)._soilsInfo(i)._scenariosInfo(_scenariosToRun(simulationsCount).ScenarioIndex)._results.SoilResults

                    For Each crop In _cropsData
                        totalCrops = 0

                        If Not .Crops.cropName Is Nothing Then
                            For k = 0 To .Crops.cropName.Count - 1  'look for the crop on soils
                                If Not (.Crops.cropName(k) Is Nothing) AndAlso .Crops.cropName(k).Trim = crop.name.Trim Then found = True : Exit For
                            Next
                            totalCrops = .Crops.cropName.Count
                        End If
                        If found = False Then  'if croop no found on list means it has to be added.
                            k = .Crops.cropName.Count - 1
                        End If
                        .Crops.cropYieldSD(k) += (.Crops.cropYield(k) - crop.yield) ^ 2
                    Next
                    For k = 0 To .Crops.cropName.Count - 1
                        .Crops.cropYieldSD(k) = Math.Sqrt(.Crops.cropYieldSD(k) / .Crops.cropRecords(k))
                        .Crops.cropYieldCI(k) = tStatisticEvaluated * (.Crops.cropYieldSD(k) / Math.Sqrt(.Crops.cropRecords(k)))
                    Next
                End With
            Next
        Else
        End If
    End Sub

    Private Sub CalculateCIAll(ByRef _resultsdata As List(Of ResultsData), ByRef soilResult As ScenariosData.APEXResultsAll, i As UShort)
        Dim totalRecords As UShort = _resultsdata.Where(Function(x) x.Sub1 = i).Count  'count to .count because there is total records minus 
        Const tStatisticEvaluated As Single = 1.645   't statistic evaluated at 90% confidence iterval
        Dim runoff, subsurfaceflow, tileDrainFlow, orgP, orgN, po4, tileDrainP, runoffN, subsurfaceN, tileDranN, deepPerFlow, irrigation, Sediment, ManureErosion As Single

        With soilResult
            runoff = .runoff : subsurfaceflow = .subsurfaceFlow : tileDrainFlow = .tileDrainFlow : orgP = .OrgP : orgN = .OrgN : po4 = .PO4 : runoffN = .RunoffN
            subsurfaceN = .SubsurfaceN : tileDranN = .tileDrainN : deepPerFlow = .deepPerFlow : irrigation = .irrigation : Sediment = .Sediment : ManureErosion = .ManureErosion

            .runoffCI = Math.Sqrt(_resultsdata.Where(Function(x) x.Sub1 = i).Sum(Function(x) (runoff - x.SurfaceFlow) ^ 2) / totalRecords) / Math.Sqrt(totalRecords) * tStatisticEvaluated
            .subsurfaceFlowCI = Math.Sqrt(_resultsdata.Where(Function(x) x.Sub1 = i).Sum(Function(x) (subsurfaceflow - (x.FLOW - x.SurfaceFlow)) ^ 2) / totalRecords) / Math.Sqrt(totalRecords) * tStatisticEvaluated
            .OrgPCI = Math.Sqrt(_resultsdata.Where(Function(x) x.Sub1 = i).Sum(Function(x) (orgP - x.ORGP) ^ 2) / totalRecords) / Math.Sqrt(totalRecords) * tStatisticEvaluated
            .PO4CI = Math.Sqrt(_resultsdata.Where(Function(x) x.Sub1 = i).Sum(Function(x) (po4 - x.PO4) ^ 2) / totalRecords) / Math.Sqrt(totalRecords) * tStatisticEvaluated
            .tileDrainPCI = Math.Sqrt(_resultsdata.Where(Function(x) x.Sub1 = i).Sum(Function(x) (tileDrainP - x.QDRP) ^ 2) / totalRecords) / Math.Sqrt(totalRecords) * tStatisticEvaluated
            .OrgNCI = Math.Sqrt(_resultsdata.Where(Function(x) x.Sub1 = i).Sum(Function(x) (orgN - x.ORGN) ^ 2) / totalRecords) / Math.Sqrt(totalRecords) * tStatisticEvaluated
            .runoffNCI = Math.Sqrt(_resultsdata.Where(Function(x) x.Sub1 = i).Sum(Function(x) (runoff - x.NO3) ^ 2) / totalRecords) / Math.Sqrt(totalRecords) * tStatisticEvaluated
            .subsurfaceNCI = Math.Sqrt(_resultsdata.Where(Function(x) x.Sub1 = i).Sum(Function(x) (subsurfaceN - (x.NO3 - x.QN)) ^ 2) / totalRecords) / Math.Sqrt(totalRecords) * tStatisticEvaluated
            .tileDrainNCI = Math.Sqrt(_resultsdata.Where(Function(x) x.Sub1 = i).Sum(Function(x) (tileDranN - x.QDRN) ^ 2) / totalRecords) / Math.Sqrt(totalRecords) * tStatisticEvaluated
            .deepPerFlowCI = Math.Sqrt(_resultsdata.Where(Function(x) x.Sub1 = i).Sum(Function(x) (deepPerFlow - x.Dprk) ^ 2) / totalRecords) / Math.Sqrt(totalRecords) * tStatisticEvaluated
            .irrigationCI = Math.Sqrt(_resultsdata.Where(Function(x) x.Sub1 = i).Sum(Function(x) (irrigation - x.IRRI) ^ 2) / totalRecords) / Math.Sqrt(totalRecords) * tStatisticEvaluated
            .SedimentCI = Math.Sqrt(_resultsdata.Where(Function(x) x.Sub1 = i).Sum(Function(x) (Sediment - x.SED) ^ 2) / totalRecords) / Math.Sqrt(totalRecords) * tStatisticEvaluated
            .ManureErosionCI = Math.Sqrt(_resultsdata.Where(Function(x) x.Sub1 = i).Sum(Function(x) (ManureErosion - x.YMNU) ^ 2) / totalRecords) / Math.Sqrt(totalRecords) * tStatisticEvaluated
            'For Each result In _resultsdata
            'If result.Sub1 = i Then
            '.runoffSD += (.runoff - result.SurfaceFlow) ^ 2
            '.subsurfaceFlowSD += (.subsurfaceFlow - (result.FLOW - result.SurfaceFlow)) ^ 2
            '.tileDrainFlowSD += (.tileDrainFlow - result.Qdr) ^ 2
            '.OrgPSD += (.OrgP - result.ORGP) ^ 2
            '.PO4SD += (.PO4 - result.PO4) ^ 2
            '.tileDrainPSD += (.tileDrainP - result.QDRP) ^ 2
            '.OrgNSD += (.OrgN - result.ORGN) ^ 2
            '.runoffNSD += (.RunoffN - result.NO3) ^ 2
            '.subsurfaceNSD += (.SubsurfaceN - (result.NO3 - result.QN)) ^ 2
            '.tileDrainNSD += (.tileDrainN - result.QDRN) ^ 2
            '.deepPerFlowSD += (.deepPerFlow - result.Dprk) ^ 2
            '.irrigationSD += (.irrigation - result.IRRI) ^ 2
            '.SedimentSD += (.Sediment - result.SED) ^ 2
            '.ManureErosionSD += (.ManureErosion - result.YMNU) ^ 2
            'totalRecords += 1
            'End If
            'Next
            '.runoffSD = Math.Sqrt(.runoffSD / totalRecords)
            '.subsurfaceFlowSD = Math.Sqrt(.subsurfaceFlowSD / totalRecords)
            '.tileDrainFlowSD = Math.Sqrt(.tileDrainFlowSD / totalRecords)
            '.OrgPSD = Math.Sqrt(.OrgPSD / totalRecords)
            '.PO4SD = Math.Sqrt(.PO4SD / totalRecords)
            '.tileDrainPSD = Math.Sqrt(.tileDrainPSD / totalRecords)
            '.OrgNSD = Math.Sqrt(.OrgNSD / totalRecords)
            '.runoffNSD = Math.Sqrt(.runoffNSD / totalRecords)
            '.subsurfaceNSD = Math.Sqrt(.subsurfaceNSD / totalRecords)
            '.tileDrainNSD = Math.Sqrt(.tileDrainNSD / totalRecords)
            '.deepPerFlowSD = Math.Sqrt(.deepPerFlowSD / totalRecords)
            '.irrigationSD = Math.Sqrt(.irrigationSD / totalRecords)
            '.SedimentSD = Math.Sqrt(.SedimentSD / totalRecords)
            '.ManureErosionSD = Math.Sqrt(.ManureErosionSD / totalRecords)
            ''calculate CI
            '.runoffCI = tStatisticEvaluated * (.runoffSD / Math.Sqrt(totalRecords))
            '.subsurfaceFlowCI = tStatisticEvaluated * (.subsurfaceFlowSD / Math.Sqrt(totalRecords))
            '.tileDrainFlowCI = tStatisticEvaluated * (.tileDrainFlowSD / Math.Sqrt(totalRecords))
            '.OrgPCI = tStatisticEvaluated * (.OrgPSD / Math.Sqrt(totalRecords))
            '.PO4CI = tStatisticEvaluated * (.PO4SD / Math.Sqrt(totalRecords))
            '.tileDrainPCI = tStatisticEvaluated * (.tileDrainPSD / Math.Sqrt(totalRecords))
            '.OrgNCI = tStatisticEvaluated * (.OrgNSD / Math.Sqrt(totalRecords))
            '.runoffNCI = tStatisticEvaluated * (.runoffNSD / Math.Sqrt(totalRecords))
            '.subsurfaceNCI = tStatisticEvaluated * (.subsurfaceNSD / Math.Sqrt(totalRecords))
            '.tileDrainNCI = tStatisticEvaluated * (.tileDrainNSD / Math.Sqrt(totalRecords))
            '.deepPerFlowCI = tStatisticEvaluated * (.deepPerFlowSD / Math.Sqrt(totalRecords))
            '.irrigationCI = tStatisticEvaluated * (.irrigationSD / Math.Sqrt(totalRecords))
            '.SedimentCI = tStatisticEvaluated * (.SedimentSD / Math.Sqrt(totalRecords))
            '.ManureErosionCI = tStatisticEvaluated * (.ManureErosionSD / Math.Sqrt(totalRecords))
        End With

    End Sub

    Private Sub CalculateCITotal(ByRef _resultsdata As List(Of ResultsData))
        If _scenariosToRun(simulationsCount).Scenario <> "Subproject" Then
            CalculateCIAll(_resultsdata, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._results.SoilResults, 0)
        Else
            CalculateCIAll(_resultsdata, _subprojectName(_scenariosToRun(simulationsCount).FieldIndex)._results.SoilResults, 0)
        End If
    End Sub

    Public Sub ReadAPEXResults(ByVal numberOfSoils As Integer, yearRotation As Short, chkCBSelected As Single)
        Dim tempa As String
        Dim m(0), APEXStartYear As Integer
        Dim resultFS(11) As Single
        Dim rowLink As DataRow = Nothing
        Dim dsPagesUser As DataSet = Nothing
        Dim nYearRotation As Integer
        Dim ddNBYR1, nStartYear, nAvgyears As Short
        Dim irrigMin As Single = 9999999, irrigMax As Single = 0 'variables to hold irrigatin max and min
        Dim _cropYield As New List(Of ResultsACY)
        Dim cropYield As ResultsACY = Nothing
        Dim srFile1 As StreamReader = Nothing
        Dim _resultsData As New List(Of ResultsData)
        Dim _cropsInfo As New List(Of CropsInfo)
        Dim currentFieldNumberAnt = currentFieldNumber
        Dim currentScenarioNumberAnt = currentScenarioNumber
        Try
            currentFieldNumber = _scenariosToRun(simulationsCount).FieldIndex
            currentScenarioNumber = _scenariosToRun(simulationsCount).ScenarioIndex

            srFile1 = New StreamReader(File.OpenRead(sAPEXBasLoc & "\Apexcont.dat"))
            tempa = srFile1.ReadLine()
            srFile1.Close()
            srFile1.Dispose()
            srFile1 = Nothing

            ddNBYR1 = CShort(tempa.Substring(4, 4))
            '// get config values
            nAPEXYears = CShort(tempa.Substring(0, 4))
            nStartYear = CShort(tempa.Substring(4, 4))
            nAvgyears = nAPEXYears - 12 + nStartYear

            'todo check that this is being received correctly from main
            nYearRotation = yearRotation
            If ddNBYR1 > 0 Then
                APEXStartYear = ddNBYR1 + nYearRotation
            Else
                APEXStartYear = nStartYear + nYearRotation
            End If
            APEXStartYear = nStartYear + 1
            'take results from .NTT file for all but crops into _resultsData
            LoadResults(_resultsData, APEXStartYear)
            'inicialize results and average all of the totals but crops
            AverageTotals(_resultsData)
            'take results from .ACY for crops into _cropsInfo
            LoadCropResults(_cropsInfo, APEXStartYear)
            If _scenariosToRun(simulationsCount).Scenario <> "Subproject" Then
                AverageBySoil(_resultsData, _cropsInfo)  'if there is a subproject not averages by soil.
            End If

            CalculateCITotal(_resultsData)
            CalculateCIbySoil(_resultsData, _cropsInfo)
            If ddlType.SelectedIndex = 0 Then
                LoadMonthValues(APEXStartYear, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._results)
            Else
                LoadMonthValues(APEXStartYear, _subprojectName(ddlSubproject.SelectedIndex)._results)
            End If


            LoadAnnualValues(_resultsData, nAvgyears, _cropsInfo)

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        Finally
            currentFieldNumber = currentFieldNumberAnt
            currentScenarioNumber = currentScenarioNumberAnt
        End Try
    End Sub

    Private Sub createWeatherFile()
        Dim weatherService As New GetWeatherInfo.ServiceSoapClient
        Dim weatherinfo As System.Collections.Generic.List(Of String)
        Try
            If File.Exists(sAPEXBasLoc & "\APEX.wth") Then File.Delete(sAPEXBasLoc & "\APEX.wth")
            If Not _startInfo.currentWeatherPath Is Nothing AndAlso _startInfo.currentWeatherPath <> "" Then
            Else
                Dim weather As New Weather
                weather.GetPrism()
            End If

            If _startInfo.stationWay <> "Own" Then
                Dim sw As StreamWriter = New StreamWriter(sAPEXBasLoc & "\APEX.wth")

                Dim newWeather As String = _startInfo.currentWeatherPath.Replace("US", "1981-2015")
                If _startInfo.weatherFinalYear = 2015 Then
                    _startInfo.currentWeatherPath = newWeather
                End If

                weatherinfo = weatherService.GetWeather(_startInfo.currentWeatherPath)
                For Each item In weatherinfo
                    sw.WriteLine(item)
                Next
                sw.Close()
                sw.Dispose()
                sw = Nothing
            Else
                File.Copy(_startInfo.currentWeatherPath, sAPEXBasLoc & "\APEX.wth", True)
            End If

            With _fieldsInfo1(_scenariosToRun(simulationsCount).FieldIndex)._scenariosInfo(_scenariosToRun(simulationsCount).ScenarioIndex)._bmpsInfo
                If .CcPrecipitation <> 0 Or .CcMinimumTeperature <> 0 Or .CcMaximumTeperature <> 0 Then
                    Dim sw As StreamWriter = New StreamWriter(sAPEXBasLoc & "\APEXNew.wth")
                    Dim sr As StreamReader = New StreamReader(sAPEXBasLoc & "\APEX.wth")
                    Dim temp As String = String.Empty
                    Dim date1 As String = String.Empty
                    Dim maxTemp As Single = 0
                    Dim minTemp As Single = 0
                    Dim pcpTemp As Single = 0

                    Do While sr.EndOfStream <> True
                        temp = sr.ReadLine
                        date1 = temp.Substring(0, 20)
                        Single.TryParse(temp.Substring(20, 6), maxTemp)
                        Single.TryParse(temp.Substring(26, 6), minTemp)
                        Single.TryParse(temp.Substring(32, 7), pcpTemp)
                        maxTemp += .CcMaximumTeperature
                        minTemp += .CcMinimumTeperature
                        pcpTemp += pcpTemp * .CcPrecipitation / 100
                        sw.WriteLine(date1 & Math.Round(maxTemp, 1).ToString.PadLeft(6) & Math.Round(minTemp, 1).ToString.PadLeft(6) & Math.Round(pcpTemp, 2).ToString.PadLeft(7))
                    Loop
                    sr.Close()
                    sr = Nothing
                    sw.Close()
                    sw = Nothing
                    File.Copy(sAPEXBasLoc & "\APEXNew.wth", sAPEXBasLoc & "\APEX.wth", True)
                End If
            End With
            totalyears = _startInfo.stationFinalYear - _startInfo.stationInitialYear + 1

            createWindFile()
            createWp1File()

            Create_wp1_from_weather(sAPEXBasLoc, _startInfo.Wp1Name, _controlValues(5).Value1)

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Private Sub Create_wp1_from_weather(loc As String, wp1Name As String, controlValue5 As Integer)
        Dim sr As StreamReader = New StreamReader(loc & "\APEX.wth")
        Dim sw As StreamWriter = New StreamWriter(loc & "\" & wp1Name.Trim & ".tmp")
        Dim sr1 As StreamReader = New StreamReader(loc & "\" & wp1Name.Trim & ".wp1")
        Dim wthData As WthData
        Dim wthDatas As New List(Of WthData)
        Dim wthMonthData As WthData = Nothing
        Dim wthMonthDatas As New List(Of WthData)
        Dim temp As String = String.Empty
        Dim maxTemp As Single = 0
        Dim minTemp As Single = 0
        Dim pcp As Single = 0
        Dim solarR As Single = 0
        Dim relativeH As Single = 0
        Dim windS As Single = 0
        Dim month As UShort
        Dim year As UShort
        Dim day As UShort
        Dim wp1Data As Wp1Data = Nothing
        Dim wp1Datas As New List(Of Wp1Data)
        Dim wp1MonSD As New Wp1Data
        Dim wp1MonSDs As New List(Of Wp1Data)
        Dim monthAnt As UShort = 0
        Dim firstYear As UShort = 0
        Dim lastYear As UShort = 0
        Dim newMonth As Boolean = False
        Dim dry_day_ant As Boolean = False

        Try
            Do While sr.EndOfStream <> True
                temp = sr.ReadLine
                UShort.TryParse(temp.Substring(2, 4), year)
                UShort.TryParse(temp.Substring(6, 4), month)
                UShort.TryParse(temp.Substring(10, 4), day)
                Single.TryParse(temp.Substring(14, 6), solarR)
                Single.TryParse(temp.Substring(20, 6), maxTemp)
                Single.TryParse(temp.Substring(26, 6), minTemp)
                If temp.Length < 39 Then
                    Single.TryParse(temp.Substring(32, 6), pcp)
                Else
                    Single.TryParse(temp.Substring(32, 7), pcp)
                End If
                If temp.Length >= 49 Then
                    Single.TryParse(temp.Substring(44, 6), windS)
                End If
                If temp.Length >= 43 Then
                    Single.TryParse(temp.Substring(38, 6), relativeH)
                End If
                wthData = New WthData
                wthData.Day = day
                wthData.Month = month
                wthData.Year = year
                If wp1Datas.Count < month Then
                    wp1Data = New Wp1Data
                    newMonth = True
                    wp1Datas.Add(wp1Data)
                End If
                If solarR > -900 And solarR < 900 Then wp1Datas(month - 1).Obsl += solarR : wp1Datas(month - 1).Days_obsl += 1
                wthData.MaxTemp = maxTemp
                If maxTemp > -900 And maxTemp < 900 Then wp1Datas(month - 1).Obmx += maxTemp : wp1Datas(month - 1).Days_obmx += 1
                wthData.MinTemp = minTemp
                If minTemp > -900 And minTemp < 900 Then wp1Datas(month - 1).Obmn += minTemp : wp1Datas(month - 1).Days_obmn += 1
                wthData.Pcp = pcp
                If pcp > -900 And pcp < 900 Then wp1Datas(month - 1).Rmo += pcp : wp1Datas(month - 1).Days_rmo += 1
                wp1Datas(month - 1).Rmosd += pcp
                wp1Datas(month - 1).Rh += relativeH
                wp1Datas(month - 1).Uav0 += windS




                wp1Datas(month - 1).Days_rh += 1
                wp1Datas(month - 1).Days_Uav0 += 1
                wthDatas.Add(wthData)
                If monthAnt <> month Then
                    If monthAnt <> 0 Then
                        wthMonthData.MaxTemp /= wthMonthData.Day
                        wthMonthData.MinTemp /= wthMonthData.Day
                        wthMonthData.Pcp /= wthMonthData.Day
                        wthMonthDatas.Add(wthMonthData)
                        'calculate SD for each month and add to wp1datas
                        wthMonthData.MaxTemp = Math.Sqrt(wp1MonSDs.Where(Function(x) x.Obmx < 999).Sum(Function(x) (wthMonthData.MaxTemp - x.Obmx) ^ 2) / wp1MonSDs.Where(Function(x) x.Obmx < 999).Count)
                        wthMonthData.MinTemp = Math.Sqrt(wp1MonSDs.Where(Function(x) x.Obmn < 999).Sum(Function(x) (wthMonthData.MinTemp - x.Obmn) ^ 2) / wp1MonSDs.Where(Function(x) x.Obmn < 999).Count)
                        wp1MonSDs.Clear()
                    Else
                        firstYear = year
                    End If
                    monthAnt = month
                    wthMonthData = New WthData
                    wthMonthData.Year = year
                    wthMonthData.Month = month
                End If
                wp1MonSD = New Wp1Data
                wthMonthData.Day += 1
                wthMonthData.MaxTemp += maxTemp
                wp1MonSD.Obmx = maxTemp
                wthMonthData.MinTemp += minTemp
                wp1MonSD.Obmn = minTemp
                wthMonthData.Pcp += pcp
                wp1MonSD.Rmo = pcp
                wp1MonSDs.Add(wp1MonSD)
                If pcp > 0 Then
                    wthMonthData.WetDay += 1
                    If dry_day_ant = True Then wthMonthData.dd_wd += 1
                    dry_day_ant = False
                Else
                    dry_day_ant = True
                End If
            Loop
            'calculate the number of years
            Dim years = year - firstYear + 1
            'calculate averages for each month
            For Each mon In wp1Datas
                mon.Obmx /= mon.Days_obmx
                mon.Obmn /= mon.Days_obmn
                mon.Rmo /= years
                mon.Rmosd /= mon.Days_rmo
                mon.Obsl /= mon.Days_obsl
                mon.Rh /= mon.Days_rh
                mon.Uav0 /= mon.Days_Uav0
                mon.Wi = 0
            Next
            '************************add the last month of the last year*********************
            wthMonthData.MaxTemp /= wthMonthData.Day
            wthMonthData.MinTemp /= wthMonthData.Day
            wthMonthData.Pcp /= wthMonthData.Day
            If pcp > 0 Then wthMonthData.WetDay += 1
            wthMonthDatas.Add(wthMonthData)
            'calculate SD for each month and add to wp1datas
            wthMonthData.MaxTemp = Math.Sqrt(wp1MonSDs.Where(Function(x) x.Obmx < 999).Sum(Function(x) (wthMonthData.MaxTemp - x.Obmx) ^ 2) / wp1MonSDs.Where(Function(x) x.Obmx < 999).Count)
            wthMonthData.MinTemp = Math.Sqrt(wp1MonSDs.Where(Function(x) x.Obmn < 999).Sum(Function(x) (wthMonthData.MinTemp - x.Obmn) ^ 2) / wp1MonSDs.Where(Function(x) x.Obmn < 999).Count)
            wp1MonSDs.Clear()
            '********************************************************************************
            'calculate total days per month in the whole period
            Dim day_30 As UShort = years * 30
            Dim day_31 As UShort = years * 31
            Dim day_Feb As UShort = years * 28 + years \ 4
            Dim pwd As Single = 0 'probability of wet day
            Dim b1 = 0.75
            Dim numerator, denominator As Single

            For i = 1 To 12
                month = i
                'calculate b1 
                b1 = wthMonthDatas.Where(Function(x) x.Month = month).Sum(Function(x) x.dd_wd) / wthMonthDatas.Where(Function(x) x.Month = month).Sum(Function(x) x.WetDay)
                wp1Datas(i - 1).Sdtmx = wthMonthDatas.Where(Function(x) x.Month = month).Average(Function(x) x.MaxTemp)
                wp1Datas(i - 1).Sdtmn = wthMonthDatas.Where(Function(x) x.Month = month).Average(Function(x) x.MinTemp)
                wp1Datas(i - 1).Rst2 = Math.Sqrt(wthDatas.Where(Function(x) x.Month = month And x.Pcp < 999).Sum(Function(x) (wp1Datas(month - 1).Rmosd - x.Pcp) ^ 2) / wthDatas.Where(Function(x) x.Month = month And x.Pcp < 999).Count)
                numerator = wthDatas.Where(Function(x) x.Month = month And x.Pcp < 999).Sum(Function(x) (x.Pcp - wp1Datas(month - 1).Rmosd) ^ 3) / wthDatas.Where(Function(x) x.Month = month And x.Pcp < 999).Count
                denominator = (wthDatas.Where(Function(x) x.Month = month And x.Pcp < 999).Sum(Function(x) (x.Pcp - wp1Datas(month - 1).Rmosd) ^ 2) / (wthDatas.Where(Function(x) x.Month = month And x.Pcp < 999).Count - 1)) ^ (3 / 2)
                wp1Datas(i - 1).Rst3 = numerator / denominator
                wp1Datas(i - 1).Uavm = wthMonthDatas.Where(Function(x) x.Month = month).Average(Function(x) x.WetDay)
                Select Case month
                    Case 1, 3, 5, 7, 8, 10, 12
                        pwd = wthDatas.Where(Function(x) x.Month = month And x.Pcp < 999 And x.Pcp > 0).Count / day_31
                    Case 2
                        pwd = wthDatas.Where(Function(x) x.Month = month And x.Pcp < 999 And x.Pcp > 0).Count / day_Feb
                    Case 4, 6, 9, 11
                        pwd = wthDatas.Where(Function(x) x.Month = month And x.Pcp < 999 And x.Pcp > 0).Count / day_30
                End Select
                wp1Datas(i - 1).Prw1 = b1 * pwd   'taking from http://www.nrcs.usda.gov/Internet/FSE_DOCUMENTS/nrcs143_013182.pdf page 5
                wp1Datas(i - 1).Prw2 = 1.0 - b1 + wp1Data.Prw1   'taking from http://www.nrcs.usda.gov/Internet/FSE_DOCUMENTS/nrcs143_013182.pdf page 5
            Next

            Dim lines(13) As String

            'print the first two lines from the original .wp1 file
            sw.WriteLine(sr1.ReadLine)
            sw.WriteLine(sr1.ReadLine)
            For Each wp1 In wp1Datas
                lines(0) &= Math.Round(wp1.Obmx, 2).ToString("N2").PadLeft(6)
                lines(1) &= Math.Round(wp1.Obmn, 2).ToString("N2").PadLeft(6)
                lines(2) &= Math.Round(wp1.Sdtmx, 2).ToString("N2").PadLeft(6)
                lines(3) &= Math.Round(wp1.Sdtmn, 2).ToString("N2").PadLeft(6)
                lines(4) &= Math.Round(wp1.Rmo, 1).ToString("N1").PadLeft(6)
                lines(5) &= Math.Round(wp1.Rst2, 1).ToString("N1").PadLeft(6)
                lines(6) &= Math.Round(wp1.Rst3, 2).ToString("N2").PadLeft(6)
                lines(7) &= Math.Round(wp1.Prw1, 3).ToString("N3").PadLeft(6)
                lines(8) &= Math.Round(wp1.Prw2, 3).ToString("N3").PadLeft(6)
                lines(9) &= Math.Round(wp1.Uavm, 2).ToString("N2").PadLeft(6)
                lines(10) &= Math.Round(0, 2).ToString("N2").PadLeft(6)
                lines(11) &= Math.Round(wp1.Obsl, 2).ToString("N2").PadLeft(6)
                lines(12) &= Math.Round(wp1.Rh, 2).ToString("N2").PadLeft(6)
                lines(13) &= Math.Round(wp1.Uav0, 2).ToString("N2").PadLeft(6)
            Next
            For line = 0 To 13
                'For Each line In lines. If lines 14-16 has information in wth file it will be calculated if not it will be zeros.
                'SR, RH, and Wind Speed. Line 13 is always zeros.
                sr1.ReadLine()
                sw.WriteLine(lines(line))
            Next
            If Not sr Is Nothing Then
                sr.Close()
                sr.Dispose()
                sr = Nothing
            End If
            If Not sr1 Is Nothing Then
                sr1.Close()
                sr1.Dispose()
                sr1 = Nothing
            End If
            If Not sw Is Nothing Then
                sw.Close()
                sw.Dispose()
                sw = Nothing
            End If

            File.Copy(sAPEXBasLoc & "\" & _startInfo.Wp1Name.Trim & ".wp1", sAPEXBasLoc & "\" & _startInfo.Wp1Name.Trim & ".org", True)
            File.Copy(sAPEXBasLoc & "\" & _startInfo.Wp1Name.Trim & ".tmp", sAPEXBasLoc & "\" & _startInfo.Wp1Name.Trim & ".wp1", True)
        Catch ex As Exception
            Dim msg As String = ex.Message
        Finally
            If Not sr Is Nothing Then
                sr.Close()
                sr.Dispose()
                sr = Nothing
            End If
            If Not sr1 Is Nothing Then
                sr1.Close()
                sr1.Dispose()
                sr1 = Nothing
            End If
            If Not sw Is Nothing Then
                sw.Close()
                sw.Dispose()
                sw = Nothing
            End If
        End Try
    End Sub

    Private Sub createWindFile()
        Dim weatherService As New GetWeatherInfo.ServiceSoapClient
        Try
            If File.Exists(sAPEXBasLoc & "\APEX.wnd") Then File.Delete(sAPEXBasLoc & "\APEX.wnd")
            If _startInfo.WindName Is Nothing Then
                _startInfo.WindName = "CHINAG"
                _startInfo.WindCode = 999
            Else
                If _startInfo.WindName = "" Then
                    _startInfo.WindName = "CHINAG"
                    _startInfo.WindCode = 999
                End If
            End If
            File.Copy(windLocation & "\" & _startInfo.WindName.Trim & ".wnd", sAPEXBasLoc & "\" & _startInfo.WindName.Trim & ".wnd", True)

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try

    End Sub

    Private Sub createWp1File()
        Try
            If File.Exists(sAPEXBasLoc & "\APEX.wp1") Then File.Delete(sAPEXBasLoc & "\APEX.wp1")
            'weatherinfo = weatherService.GetWeather("E:/Weather/wp1File/" + _startInfo.Wp1Name + ".wp1")
            If _startInfo.Wp1Name Is Nothing Then
                _startInfo.Wp1Name = "CHINAG"
                _startInfo.Wp1Code = 999
            Else
                If _startInfo.Wp1Name = "" Then
                    _startInfo.Wp1Name = "CHINAG"
                    _startInfo.Wp1Code = 999
                End If
            End If
            File.Copy(wp1Location & "\" & _startInfo.Wp1Name.Trim & ".wp1", sAPEXBasLoc & "\" & _startInfo.Wp1Name.Trim & ".wp1", True)

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try

    End Sub

    Public Sub changeTillForHE(oper As Integer, depthAnt As String)
        Dim newLine As String = "  " & oper

        newLine &= " CLEA103  SELF    260.    275.    100.   2500.      0.     1.0     .20    .150   1.600    .100    .305    .560    .885   1.000    .000    .000    .000"
        newLine &= "-199.898    .000    .000    .000    .000   2.000"
        newLine &= depthAnt.PadLeft(8)
        newLine &= "    .900    .000    .000    .000    .150  CLEARCUT4" & oper
        changeTillHE.Add(newLine)
    End Sub

    Private Sub gvSimulations_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSimulations.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            'e.Row.Cells(0).Attributes.Add("onclick", "return DeleteRow(" & e.Row.RowIndex.ToString() & ")")
            'e.Row.Cells(0).Controls(0).ID = "btnDelete"
            'Dim btnDelete As New Object
            'btnDelete = e.Row.Cells(0).FindControl("btnDelete")
            'btnDelete.Text = cntDoc.Descendants("Delete").Value
            e.Row.Cells(11).Style.Item("display") = "none"
            e.Row.Cells(12).Style.Item("display") = "none"
            'btnDelete.Text = cntDoc.Descendants("Delete").Value
        End If
    End Sub

    Private Sub ddlType_ServerChange(sender As Object, e As System.EventArgs) Handles ddlType.ServerChange
        _scenariosToRun.Clear()
        'gvSimulations.DataSource = _scenariosToRun
        'gvSimulations.DataBind()
        Select Case ddlType.SelectedIndex
            Case 0
                divScenario.Style.Item("display") = ""
                divSubproject.Style.Item("display") = "none"
            Case 1
                divScenario.Style.Item("display") = "none"
                divSubproject.Style.Item("display") = "none"
                btnAddSubproject_Click()
        End Select
    End Sub
    Private Sub ArrangeInfo(openSave As String)
        Select Case openSave
            'this is done after the open subrutine
            Case "Open"
                _startInfo = Session("projects")._StartInfo
                _fieldsInfo1 = Session("projects")._fieldsInfo1
                _subprojectName = Session("projects")._subprojectName
                _sitesInfo = Session("projects")._sitesInfo
                _parmValues = Session("projects")._parmValues
                _controlValues = Session("projects")._controlValues
                _crops = Session("crops")
                _otherTemp = Session("projects")._otherTemp
                _structureTemp = Session("projects")._structureTemp
                _feedTemp = Session("projects")._feedTemp
                _equipmentTemp = Session("projects")._equipmentTemp
                If Not Session("scenariosToRun") Is Nothing Then _scenariosToRun = Session("scenariosToRun")
                'this is done before the save subroutine
            Case "Save"
                Session("projects")._StartInfo = _startInfo
                Session("projects")._fieldsInfo1 = _fieldsInfo1
                Session("projects")._subprojectName = _subprojectName
                Session("projects")._sitesInfo = _sitesInfo
                Session("projects")._parmValues = _parmValues
                Session("projects")._controlValues = _controlValues
                Session("projects")._otherTemp = _otherTemp
                Session("projects")._structureTemp = _structureTemp
                Session("projects")._feedTemp = _feedTemp
                Session("projects")._equipmentTemp = _equipmentTemp
        End Select
    End Sub

    Private Sub ddlScenario_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlScenario.SelectedIndexChanged
        currentScenarioNumber = ddlScenario.SelectedIndex
        Session("currentScenarioNumber") = currentScenarioNumber
    End Sub
End Class