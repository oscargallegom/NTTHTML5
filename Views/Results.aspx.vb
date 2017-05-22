Imports System.Web.UI.DataVisualization.Charting
Imports System.IO

Public Class Results
    Inherits System.Web.UI.Page
    'Controls used in sitemaster page
    Private lblMessage As Label
    Private imgIcon As Image
    Private SetResults As List(Of SummaryResults)

    'local variables
    Private signN, signP, signF, signS As Boolean, signC As Boolean = True, signW, signA, signAreas As Boolean
    'Results
    Private totalBufferArea1 As Single = 0
    Private totalBufferArea2 As Single = 0
    Private totalBufferArea3 As Single = 0
    Private Structure AllResults
        'define nutrient results global in order to avoid passing parm among subrutines.
        Public totalN, totalP, totalFlow, totalSediment, totalOtherWaterInfo As Single
        Public Sediment, runoff, subsurfaceFlow, tileDrainFlow, irrigation, orgN, subsurfaceN, runoffN, orgP, PO4, tileDrainN, tileDrainP, deepPerFlow, ManureErosion, _
            n2o, deepPerN, deepPerP As Single
        Public totalNCI, totalPCI, totalFlowCI, totalSedimentCI, totalOtherWaterInfoCI As Single
        Public sedimentCi, cropYieldCi, orgPCI, PO4CI, tileDrainPCI, orgNCI, runoffNCI, SubsurfaceNCI, tileDrainNCI, runoffCI, subsurfaceFlowCI, tileDrainFlowCI, irrigationCI, _
            deepPerFlowCI, ManureErosionCi, n2oCi, deepPerNCi, deepPerPCi As Single
        Public areaFS, areaPPPipes, areaPPRes, areaCBFS, areaRF, areaWL, areaWW, areaSdg As Single
        Public totalBufferArea As Single
    End Structure
    'Classes definition
    Private _startInfo As New StartInfo
    Private _fieldsInfo1 As New List(Of FieldsData)
    Private _subprojectName As New List(Of SubprojectNameData)
    Private _crops As New List(Of CropsData)
    'retrieve the currentfieldnumber
    Private currentFieldNumber As Short = 0
    Private currentScenarioNumber As Short = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("EPage") = False Then btnEconomics.Style.Item("display") = "none"
        If Session("DnDc") = True Then DownloadDnDc.Enabled = True
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
        currentFieldNumber = ddlField.SelectedIndex
        Select Case Page.Request.Params("__EVENTARGUMENT")
            Case english
                Session("Language") = english
            Case spanish
                Session("Language") = spanish
        End Select
        openXMLLanguagesFile()

        ChangeLanguageContent()

        lblMessage.Text = ""
        ArrangeInfo("Open")
        currentFieldNumber = ddlField.SelectedIndex
        If IsPostBack Then
            Select Case True
                Case Page.Request.Params("__EVENTTARGET").Contains("ddlSoils")
                    ddlSoils_SelectedIndexChanged()
            End Select
        Else
            currentFieldNumber = Session("currentFieldNumber")
            ddlField.SelectedIndex = currentFieldNumber
            fsetSoils.Style.Item("display") = "none"
            sctSubprojects.Style.Item("display") = "none"
            LoadFields(ddlField, _fieldsInfo1, currentFieldNumber)
            ddlField_SelectedIndexChanged(ddlField, e)
            LoadDdlInfo()
            ddlSubproject1.Items.Clear()
            ddlSubproject2.Items.Clear()
            ddlSubproject3.Items.Clear()
            For Each item In _subprojectName
                ddlSubproject1.Items.Add(item.Name)
                ddlSubproject2.Items.Add(item.Name)
                ddlSubproject3.Items.Add(item.Name)
            Next
            ddlSubproject1.Items.Insert(0, selectOne)
            ddlSubproject2.Items.Insert(0, selectOne)
            ddlSubproject3.Items.Insert(0, selectOne)
            ddlSubproject1.SelectedIndex = 0
            ddlSubproject2.SelectedIndex = 0
            ddlSubproject3.SelectedIndex = 0
        End If

        If _crops.Count <= 0 Then
            _crops = Session("crops")
        End If

        btnSummary.CommandArgument = 1
        btnGraphs.CommandArgument = 0
        btnEconomics.CommandArgument = 0
        btnBySoil.CommandArgument = 0
    End Sub

    Private Sub LoadDdlInfo()
        ddlInfo.Items.Add("Flow")
        ddlInfo.Items.Add("Sediment")
        ddlInfo.Items.Add("Org N")
        ddlInfo.Items.Add("Org P")
        ddlInfo.Items.Add("NO3")
        ddlInfo.Items.Add("PO4")
        ddlInfo.Items.Add("Total N")
        ddlInfo.Items.Add("Total P")
        ddlInfo.Items.Add("Precipitation")
        ddlInfo.Items.Add("---")  ' added for N2O. Since it is just for some states and for annual averages only the "---" string is added to show nothing there. If annual is selected and it is any of the states allow to see it it will be N2O.
        ddlInfo.SelectedIndex = 0
    End Sub
    Private Sub ChangeLanguageContent()
        lblField.InnerText = cntDoc.Descendants("Field").Value
        lblScenario1.InnerText = cntDoc.Descendants("FirstScenario").Value
        lblScenario2.InnerText = cntDoc.Descendants("SecondScenario").Value
        lblScenario3.InnerText = cntDoc.Descendants("ThirdScenario").Value
        lblSoils.InnerText = cntDoc.Descendants("Soils").Value
        lblSubproject1.InnerText = cntDoc.Descendants("FirstSubproject").Value
        lblSubproject2.InnerText = cntDoc.Descendants("SecondSubproject").Value
        lblSubproject3.InnerText = cntDoc.Descendants("ThirdSubproject").Value
        lblType.InnerText = cntDoc.Descendants("SelectTypeHeading").Value
        btnSummary.Text = cntDoc.Descendants("TabularResults").Value
        btnBySoil.Text = cntDoc.Descendants("BySoil").Value
        btnGraphs.Text = cntDoc.Descendants("GraphResults").Value
        btnCreatePDF.Text = cntDoc.Descendants("SavePDFResults").Value & "*"
        btnEconomics.Text = cntDoc.Descendants("Economics").Value
        lblDownload.Text = cntDoc.Descendants("DownloadMessage").Value
        lblCI.InnerText = cntDoc.Descendants("ConfidenceInterval").Value

        gvResults.Columns(0).HeaderText = cntDoc.Descendants("Detail").Value
        gvResults.Columns(1).HeaderText = cntDoc.Descendants("Description").Value
        gvResults.Columns(3).HeaderText = "±"
        gvResults.Columns(6).HeaderText = cntDoc.Descendants("Difference").Value
        gvResults.Columns(7).HeaderText = cntDoc.Descendants("Reduction").Value
        gvResults.Columns(8).HeaderText = cntDoc.Descendants("TotalArea").Value
    End Sub

    Protected Sub ddlType_ServerChange(sender As Object, e As System.EventArgs) Handles ddlType.ServerChange
        sctFields.Style.Item("display") = "none"
        sctSubprojects.Style.Item("display") = "none"
        Select Case ddlType.SelectedIndex
            Case 0 'fields/scenarios selected
                sctFields.Style.Item("display") = ""
                btnBySoil.Style.Item("display") = ""
                ddlField_SelectedIndexChanged(ddlField, e)
            Case 1    'subproject selected
                fsetSoils.Style.Item("display") = "none"
                btnBySoil.Style.Item("display") = "none"
                sctSubprojects.Style.Item("display") = ""
                If ddlSubproject1.Items.Count <= 0 And ddlSubproject2.Items.Count <= 0 And ddlSubproject3.Items.Count <= 0 Then
                    showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", "There are not subprojects")
                Else
                    GetResultsSubProjects()
                End If
        End Select
    End Sub

    Private Sub ddlField_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlField.ServerChange
        currentFieldNumber = ddlField.SelectedIndex
        Session("currentFieldNumber") = currentFieldNumber
        ddlScenarios1.Items.Clear()
        ddlScenarios2.Items.Clear()
        ddlScenarios3.Items.Clear()
        currentScenarioNumber = -1
        'currentSoilNumber = -1
        If _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count > 0 Then currentScenarioNumber = 0
        'If _fieldsInfo1(currentFieldNumber)._soilsInfo.Count > 0 Then currentSoilNumber = 0
        For Each item In _fieldsInfo1(currentFieldNumber)._scenariosInfo
            ddlScenarios1.Items.Add(item.Name)
            ddlScenarios2.Items.Add(item.Name)
            ddlScenarios3.Items.Add(item.Name)
        Next
        ddlScenarios1.Items.Insert(0, selectOne)
        ddlScenarios2.Items.Insert(0, selectOne)
        ddlScenarios3.Items.Insert(0, selectOne)
        ddlScenarios1.SelectedIndex = 0
        ddlScenarios2.SelectedIndex = 0
        ddlScenarios3.SelectedIndex = 0

        ddlSoils.Items.Clear()
        ddlSoils.Items.Add("Select One")
        For Each Soil In _fieldsInfo1(currentFieldNumber)._soilsInfo.Where(Function(x) x.Selected)
            ddlSoils.Items.Add(Soil.Symbol & "  " & Soil.Name & "  " & Soil.Component & " " & Soil.Slope)
        Next
        ddlSoils.SelectedIndex = 0

        DisplaySelection()
    End Sub

    'Private Sub addFieldToResults()
    '    Dim fieldToAdd As FieldsSelectedData = New FieldsSelectedData
    '    Try
    '        _fieldsToAdd.Clear()
    '        Select Case ddlType.SelectedIndex
    '            Case 0
    '                If ddlField.SelectedIndex >= 0 And ddlScenario1.SelectedIndex >= 0 And ddlScenario2.SelectedIndex >= 0 Then
    '                    fieldToAdd.FieldName = ddlField.SelectedItem.Text
    '                    fieldToAdd.FieldNumber = ddlField.SelectedIndex
    '                    fieldToAdd.Scenario1Name = ddlScenario1.SelectedItem.Text
    '                    fieldToAdd.Scenario1Number = ddlScenario1.SelectedIndex
    '                    fieldToAdd.Scenario2Name = ddlScenario2.SelectedItem.Text
    '                    fieldToAdd.Scenario2Number = ddlScenario2.SelectedIndex
    '                    fieldToAdd.FieldArea = _fieldsInfo1(fieldToAdd.FieldNumber).Area
    '                    _fieldsToAdd.Add(fieldToAdd)
    '                Else
    '                    showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("SelectFieldMsg").Value)
    '                End If
    '            Case 1
    '                If ddlSubproject1.SelectedIndex >= 0 And ddlSubproject2.SelectedIndex >= 0 Then
    '                    fieldToAdd.FieldName = "Subproject"
    '                    fieldToAdd.FieldNumber = 0
    '                    fieldToAdd.Scenario1Name = ddlSubproject1.SelectedItem.Text
    '                    fieldToAdd.Scenario1Number = ddlSubproject1.SelectedIndex
    '                    fieldToAdd.Scenario2Name = ddlSubproject2.SelectedItem.Text
    '                    fieldToAdd.Scenario2Number = ddlSubproject2.SelectedIndex
    '                    fieldToAdd.FieldArea = _subprojectName(fieldToAdd.FieldNumber).TotalArea
    '                    _fieldsToAdd.Add(fieldToAdd)
    '                Else
    '                    showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("SelectSubprojectMsg").Value)
    '                End If
    '        End Select

    '        gvFieldsToRun.DataSource = _fieldsToAdd
    '        gvFieldsToRun.DataBind()
    '        Select Case True
    '            Case btnSummary.CommandArgument = "1"
    '                'update environmental information.
    '                If ddlType.SelectedIndex = "0" Then GetResultsScenarios() Else GetResultsSubProjects()
    '            Case btnEconomics.CommandArgument = "1"
    '                'update economic information.
    '                If ddlType.SelectedIndex = 0 Then GetEconomics() Else GetEconomics()
    '            Case btnGraphs.CommandArgument = "1"
    '                'todo uncommentarized this two lines for graphs
    '                'If ddlType.SelectedIndex = 0 Then charts() Else Charts1()
    '                'btnGraphs_Click(New Object, New EventArgs())
    '            Case btnBySoil.CommandArgument = "1"
    '                'update environmental information by soil
    '                GetSoilResults(ddlSoils.SelectedIndex - 1)
    '        End Select
    '    Catch ex As Exception
    '        showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & " - " & ex.Message)
    '    End Try
    'End Sub

    Public Sub GetResultsScenarios(header1 As String, header2 As String, header3 As String)
        Try
            Dim _cropsInfo1 As List(Of CropsInfo) = New List(Of CropsInfo)
            Dim _cropsInfo2 As List(Of CropsInfo) = New List(Of CropsInfo)
            Dim _cropsInfo3 As List(Of CropsInfo) = New List(Of CropsInfo)
            Dim totalSelectedArea As Single = _fieldsInfo1(currentFieldNumber).Area
            Dim _allResults1 As AllResults
            Dim _allResults2 As AllResults
            Dim _allResults3 As AllResults
            Dim cropName() As String = Nothing
            Dim i As Short = 0
            gvResults.DataSource = Nothing

            'Crop Yield Scenario No. 1
            For Each tmpScenario In _fieldsInfo1(currentFieldNumber)._scenariosInfo
                If tmpScenario.Name = ddlScenarios1.Items(ddlScenarios1.SelectedIndex).Text Then
                    GetCropResults(_cropsInfo1, tmpScenario._results.SoilResults, cropName)
                End If
            Next

            'Crop Yield Scenario No. 2
            For Each tmpScenario In _fieldsInfo1(currentFieldNumber)._scenariosInfo
                If tmpScenario.Name = ddlScenarios2.Items(ddlScenarios2.SelectedIndex).Text Then
                    GetCropResults(_cropsInfo2, tmpScenario._results.SoilResults, cropName)
                End If
            Next

            'Crop Yield Scenario No. 3
            For Each tmpScenario In _fieldsInfo1(currentFieldNumber)._scenariosInfo
                If tmpScenario.Name = ddlScenarios3.Items(ddlScenarios3.SelectedIndex).Text Then
                    GetCropResults(_cropsInfo3, tmpScenario._results.SoilResults, cropName)
                End If
            Next

            'Nutrients Scenario No. 1
            For Each record In _fieldsInfo1(currentFieldNumber)._scenariosInfo
                If record.Name = ddlScenarios1.Items(ddlScenarios1.SelectedIndex).Text Then
                    CalculateBufferAreas(record, totalSelectedArea, _allResults1)
                    GetNutrientsInfo(record._results, i, totalSelectedArea, _allResults1, _fieldsInfo1(currentFieldNumber).Area)
                    CalculateNutrientsForFencing(record, totalSelectedArea, _allResults1)
                End If
            Next

            'Nutrients Scenario No. 2
            For Each record In _fieldsInfo1(currentFieldNumber)._scenariosInfo
                If record.Name = ddlScenarios2.Items(ddlScenarios2.SelectedIndex).Text Then
                    CalculateBufferAreas(record, totalSelectedArea, _allResults2)
                    GetNutrientsInfo(record._results, i, totalSelectedArea, _allResults2, _fieldsInfo1(currentFieldNumber).Area)
                    CalculateNutrientsForFencing(record, totalSelectedArea, _allResults2)
                End If
            Next

            'Nutrients Scenario No. 3
            For Each record In _fieldsInfo1(currentFieldNumber)._scenariosInfo
                If record.Name = ddlScenarios3.Items(ddlScenarios3.SelectedIndex).Text Then
                    CalculateBufferAreas(record, totalSelectedArea, _allResults3)
                    GetNutrientsInfo(record._results, i, totalSelectedArea, _allResults3, _fieldsInfo1(currentFieldNumber).Area)
                    CalculateNutrientsForFencing(record, totalSelectedArea, _allResults3)
                End If
            Next

            With _allResults1   'add all of the buffer areas for scenario 1
                .totalBufferArea = .areaFS + .areaPPPipes + .areaPPRes + .areaCBFS + .areaRF + .areaWL + .areaWW + .areaSdg
                totalBufferArea1 = .totalBufferArea
            End With
            With _allResults2   'add all of the buffer areas for scenario 2
                .totalBufferArea = .areaFS + .areaPPPipes + .areaPPRes + .areaCBFS + .areaRF + .areaWL + .areaWW + .areaSdg
                totalBufferArea2 = .totalBufferArea
            End With
            With _allResults3   'add all of the buffer areas for scenario 3
                .totalBufferArea = .areaFS + .areaPPPipes + .areaPPRes + .areaCBFS + .areaRF + .areaWL + .areaWW + .areaSdg
                totalBufferArea3 = .totalBufferArea
            End With

            gvResults.DataSource = SetResult(_allResults1, _allResults2, _allResults3, totalSelectedArea, cropName, _cropsInfo1, _cropsInfo2, _cropsInfo3)
            gvResults.DataBind()
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try

    End Sub

    Private Sub SelectColumnsToShow(ByRef row As GridViewRow)
        If ddlType.SelectedIndex = 0 Then
            If ddlScenarios2.SelectedIndex > 0 Then
                row.Cells(4).Visible = True
                row.Cells(5).Visible = True
                row.Cells(6).Visible = True
                row.Cells(7).Visible = True
                row.Cells(8).Visible = True
            Else
                row.Cells(4).Visible = False
                row.Cells(5).Visible = False
                row.Cells(6).Visible = False
                row.Cells(7).Visible = False
                row.Cells(8).Visible = False
            End If
            If ddlScenarios3.SelectedIndex > 0 Then
                row.Cells(9).Visible = True
                row.Cells(10).Visible = True
                row.Cells(11).Visible = True
                row.Cells(12).Visible = True
                row.Cells(13).Visible = True
            Else
                row.Cells(9).Visible = False
                row.Cells(10).Visible = False
                row.Cells(11).Visible = False
                row.Cells(12).Visible = False
                row.Cells(13).Visible = False
            End If
        Else
            If ddlSubproject2.SelectedIndex > 0 Then
                row.Cells(4).Visible = True
                row.Cells(5).Visible = True
                row.Cells(6).Visible = True
                row.Cells(7).Visible = True
                row.Cells(8).Visible = True
            Else
                row.Cells(4).Visible = False
                row.Cells(5).Visible = False
                row.Cells(6).Visible = False
                row.Cells(7).Visible = False
                row.Cells(8).Visible = False
            End If
            If ddlSubproject3.SelectedIndex > 0 Then
                row.Cells(9).Visible = True
                row.Cells(10).Visible = True
                row.Cells(11).Visible = True
                row.Cells(12).Visible = True
                row.Cells(13).Visible = True
            Else
                row.Cells(9).Visible = False
                row.Cells(10).Visible = False
                row.Cells(11).Visible = False
                row.Cells(12).Visible = False
                row.Cells(13).Visible = False
            End If
        End If
    End Sub

    Private Sub GetResultsSubProjects()
        Try
            Dim _cropsInfo1 As List(Of CropsInfo) = New List(Of CropsInfo)
            Dim _cropsInfo2 As List(Of CropsInfo) = New List(Of CropsInfo)
            Dim _cropsInfo3 As List(Of CropsInfo) = New List(Of CropsInfo)
            Dim totalSelectedArea As Single = 0
            Dim _allResults1 As AllResults
            Dim _allResults2 As AllResults
            Dim _allResults3 As AllResults
            Dim cropName() As String = Nothing
            Dim i As Short = 0
            gvResults.DataSource = Nothing

            'get crops subproject No. 1
            If ddlSubproject1.SelectedIndex > 0 Then GetCropResults(_cropsInfo1, _subprojectName(ddlSubproject1.SelectedIndex - 1)._results.SoilResults, cropName)
            'get crops subproject No. 2
            If ddlSubproject2.SelectedIndex > 0 Then GetCropResults(_cropsInfo2, _subprojectName(ddlSubproject2.SelectedIndex - 1)._results.SoilResults, cropName)
            'get crops subproject No. 3
            If ddlSubproject3.SelectedIndex > 0 Then GetCropResults(_cropsInfo3, _subprojectName(ddlSubproject3.SelectedIndex - 1)._results.SoilResults, cropName)

            If ddlSubproject1.SelectedIndex > 0 Then
                'Nutrients Scenario No. 1
                For i = 0 To _subprojectName(ddlSubproject1.SelectedIndex - 1)._subproject.Count - 1
                    For j = 0 To _fieldsInfo1.Count - 1
                        If _subprojectName(ddlSubproject1.SelectedIndex - 1)._subproject(i).Field = _fieldsInfo1(j).Name Then
                            For l = 0 To _fieldsInfo1(j)._scenariosInfo.Count - 1
                                If _fieldsInfo1(j)._scenariosInfo(l).Name = _subprojectName(ddlSubproject1.SelectedIndex - 1)._subproject(i).Scenario Then
                                    CalculateBufferAreas(_fieldsInfo1(j)._scenariosInfo(l), _subprojectName(ddlSubproject1.SelectedIndex - 1).TotalArea, _allResults1)
                                    CalculateNutrientsForFencing(_fieldsInfo1(j)._scenariosInfo(l), _subprojectName(ddlSubproject1.SelectedIndex - 1).TotalArea, _allResults1)
                                End If
                            Next
                        End If
                    Next
                Next
                GetNutrientsInfo(_subprojectName(ddlSubproject1.SelectedIndex - 1)._results, 0, _subprojectName(ddlSubproject1.SelectedIndex - 1).TotalArea, _allResults1, _subprojectName(ddlSubproject1.SelectedIndex - 1).TotalArea)
            End If
            If ddlSubproject2.SelectedIndex > 0 Then
                'Nutrients Scenario No. 2
                For i = 0 To _subprojectName(ddlSubproject2.SelectedIndex - 1)._subproject.Count - 1
                    For j = 0 To _fieldsInfo1.Count - 1
                        If _subprojectName(ddlSubproject2.SelectedIndex - 1)._subproject(i).Field = _fieldsInfo1(j).Name Then
                            For l = 0 To _fieldsInfo1(j)._scenariosInfo.Count - 1
                                If _fieldsInfo1(j)._scenariosInfo(l).Name = _subprojectName(ddlSubproject2.SelectedIndex - 1)._subproject(i).Scenario Then
                                    CalculateBufferAreas(_fieldsInfo1(j)._scenariosInfo(l), _subprojectName(ddlSubproject2.SelectedIndex - 1).TotalArea, _allResults2)
                                    CalculateNutrientsForFencing(_fieldsInfo1(j)._scenariosInfo(l), _subprojectName(ddlSubproject2.SelectedIndex - 1).TotalArea, _allResults2)
                                End If
                            Next
                        End If
                    Next
                Next
                GetNutrientsInfo(_subprojectName(ddlSubproject2.SelectedIndex - 1)._results, 0, _subprojectName(ddlSubproject2.SelectedIndex - 1).TotalArea, _allResults2, _subprojectName(ddlSubproject2.SelectedIndex - 1).TotalArea)

            End If
            If ddlSubproject3.SelectedIndex > 0 Then
                'Nutrients Scenario No. 3
                For i = 0 To _subprojectName(ddlSubproject3.SelectedIndex - 1)._subproject.Count - 1
                    For j = 0 To _fieldsInfo1.Count - 1
                        If _subprojectName(ddlSubproject3.SelectedIndex - 1)._subproject(i).Field = _fieldsInfo1(j).Name Then
                            For l = 0 To _fieldsInfo1(j)._scenariosInfo.Count - 1
                                If _fieldsInfo1(j)._scenariosInfo(l).Name = _subprojectName(ddlSubproject3.SelectedIndex - 1)._subproject(i).Scenario Then
                                    CalculateBufferAreas(_fieldsInfo1(j)._scenariosInfo(l), _subprojectName(ddlSubproject3.SelectedIndex - 1).TotalArea, _allResults3)
                                    CalculateNutrientsForFencing(_fieldsInfo1(j)._scenariosInfo(l), _subprojectName(ddlSubproject3.SelectedIndex - 1).TotalArea, _allResults3)
                                End If
                            Next
                        End If
                    Next
                Next
                GetNutrientsInfo(_subprojectName(ddlSubproject3.SelectedIndex - 1)._results, 0, _subprojectName(ddlSubproject3.SelectedIndex - 1).TotalArea, _allResults3, _subprojectName(ddlSubproject3.SelectedIndex - 1).TotalArea)
            End If
            With _allResults1   'add all of the buffer areas for scenario 1
                .totalBufferArea = .areaFS + .areaPPPipes + .areaPPRes + .areaCBFS + .areaRF + .areaWL + .areaWW + .areaSdg
                totalBufferArea1 = .totalBufferArea
            End With
            With _allResults2   'add all of the buffer areas for scenario 2
                .totalBufferArea = .areaFS + .areaPPPipes + .areaPPRes + .areaCBFS + .areaRF + .areaWL + .areaWW + .areaSdg
                totalBufferArea2 = .totalBufferArea
            End With
            With _allResults3   'add all of the buffer areas for scenario 3
                .totalBufferArea = .areaFS + .areaPPPipes + .areaPPRes + .areaCBFS + .areaRF + .areaWL + .areaWW + .areaSdg
                totalBufferArea3 = .totalBufferArea
            End With

            Select Case True
                Case ddlSubproject1.SelectedIndex > 0
                    totalSelectedArea = _subprojectName(ddlSubproject1.SelectedIndex - 1).TotalArea
                Case ddlSubproject2.SelectedIndex > 0
                    totalSelectedArea = _subprojectName(ddlSubproject2.SelectedIndex - 1).TotalArea
                Case ddlSubproject3.SelectedIndex > 0
                    totalSelectedArea = _subprojectName(ddlSubproject3.SelectedIndex - 1).TotalArea
            End Select
            gvResults.DataSource = SetResult(_allResults1, _allResults2, _allResults3, totalSelectedArea, cropName, _cropsInfo1, _cropsInfo2, _cropsInfo3)
            gvResults.DataBind()
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try

    End Sub

    Private Sub GetEconomics()
        Try
            Dim summaryResults As List(Of SummaryResults) = New List(Of SummaryResults)
            Dim summaryResultsRecord As SummaryResults
            Dim totalRevenue1, totalCost1, netReturn1, netCashFlow1 As Single
            Dim totalRevenue2, totalCost2, netReturn2, netCashFlow2 As Single
            Dim totalRevenue3, totalCost3, netReturn3, netCashFlow3 As Single
            Dim area As Single = 0
            Dim totalSelectedArea As Single = _fieldsInfo1(currentFieldNumber).Area

            gvResults.DataSource = Nothing
            For Each record In _fieldsInfo1(currentFieldNumber)._scenariosInfo
                If record.Name = ddlScenarios1.Items(ddlScenarios1.SelectedIndex).Text Then
                    With record
                        totalRevenue1 = ._femInfo.TotalRevenue
                        totalCost1 = ._femInfo.TotalCost
                        netReturn1 = ._femInfo.NetReturn
                        netCashFlow1 = ._femInfo.NetCashFlow
                    End With
                End If
            Next
            For Each record In _fieldsInfo1(currentFieldNumber)._scenariosInfo
                If record.Name = ddlScenarios2.Items(ddlScenarios2.SelectedIndex).Text Then
                    With record
                        totalRevenue2 = ._femInfo.TotalRevenue
                        totalCost2 = ._femInfo.TotalCost
                        netReturn2 = ._femInfo.NetReturn
                        netCashFlow2 = ._femInfo.NetCashFlow
                    End With
                End If
            Next
            For Each record In _fieldsInfo1(currentFieldNumber)._scenariosInfo
                If record.Name = ddlScenarios3.Items(ddlScenarios3.SelectedIndex).Text Then
                    With record
                        totalRevenue3 = ._femInfo.TotalRevenue
                        totalCost3 = ._femInfo.TotalCost
                        netReturn3 = ._femInfo.NetReturn
                        netCashFlow3 = ._femInfo.NetCashFlow
                    End With
                End If
            Next
            area = totalSelectedArea
            '********************* comparison values for scenario1 and scenario 2 *************************
            'Orginc N
            summaryResultsRecord = New SummaryResults
            summaryResultsRecord = GetScenarioValuesEconomics(cntDoc.Descendants("TotalRevenue").Value, totalRevenue1.ToString("F2"), totalRevenue2.ToString("F2"), totalRevenue3.ToString("F2"), "Detail", False, 0, 0, 0, area)
            summaryResults.Add(summaryResultsRecord)
            summaryResultsRecord = New SummaryResults
            summaryResultsRecord = GetScenarioValuesEconomics(cntDoc.Descendants("TotalCost").Value, totalCost1.ToString("F2"), totalCost2.ToString("F2"), totalCost3.ToString("F2"), "Detail", False, 0, 0, 0, area)
            summaryResults.Add(summaryResultsRecord)
            summaryResultsRecord = New SummaryResults
            summaryResultsRecord = GetScenarioValuesEconomics(cntDoc.Descendants("NetReturn").Value, netReturn1.ToString("F2"), netReturn2.ToString("F2"), netReturn3.ToString("F2"), "Detail", False, 0, 0, 0, area)
            summaryResults.Add(summaryResultsRecord)
            summaryResultsRecord = New SummaryResults
            summaryResultsRecord = GetScenarioValuesEconomics(cntDoc.Descendants("NetCashFlow").Value, netCashFlow1.ToString("F2"), netCashFlow2.ToString("F2"), netCashFlow3.ToString("F2"), "Detail", False, 0, 0, 0, area)
            summaryResults.Add(summaryResultsRecord)

            'summaryResultsRecord = New SummaryResults
            'summaryResultsRecord.Description = cntDoc.Descendants("TotalRevenue").Value
            'summaryResultsRecord.Scenario1 = totalRevenue1.ToString("F2")
            'summaryResultsRecord.Scenario2 = totalRevenue2.ToString("F2")
            'summaryResultsRecord.Difference2 = (summaryResultsRecord.Scenario1 - summaryResultsRecord.Scenario2).ToString("F2")
            'summaryResultsRecord.Reduction2 = ((100 - (summaryResultsRecord.Scenario2 / summaryResultsRecord.Scenario1) * 100)).ToString("F1")
            'summaryResultsRecord.TotalArea2 = 0
            'summaryResults.Add(summaryResultsRecord)

            'summaryResultsRecord = New SummaryResults
            'summaryResultsRecord.Description = cntDoc.Descendants("TotalCost").Value
            'summaryResultsRecord.Scenario1 = totalCost1.ToString("F2")
            'summaryResultsRecord.Scenario2 = totalCost2.ToString("F2")
            'summaryResultsRecord.Difference2 = (summaryResultsRecord.Scenario1 - summaryResultsRecord.Scenario2).ToString("F2")
            'summaryResultsRecord.Reduction2 = ((100 - (summaryResultsRecord.Scenario2 / summaryResultsRecord.Scenario1) * 100)).ToString("F1")
            'summaryResultsRecord.TotalArea2 = (summaryResultsRecord.Difference2 * area).ToString("F1")
            'summaryResults.Add(summaryResultsRecord)

            'summaryResultsRecord = New SummaryResults
            'summaryResultsRecord.Description = cntDoc.Descendants("NetReturn").Value
            'summaryResultsRecord.Scenario1 = netReturn1.ToString("F2")
            'summaryResultsRecord.Scenario2 = netReturn2.ToString("F2")
            'summaryResultsRecord.Difference2 = (summaryResultsRecord.Scenario1 - summaryResultsRecord.Scenario2).ToString("F2")
            'summaryResultsRecord.Reduction2 = ((100 - (summaryResultsRecord.Scenario2 / summaryResultsRecord.Scenario1) * 100)).ToString("F1")
            'summaryResultsRecord.TotalArea2 = (summaryResultsRecord.Difference2 * area).ToString("F1")
            'summaryResults.Add(summaryResultsRecord)

            'summaryResultsRecord = New SummaryResults
            'summaryResultsRecord.Description = cntDoc.Descendants("NetCashFlow").Value
            'summaryResultsRecord.Scenario1 = netCashFlow1.ToString("F2")
            'summaryResultsRecord.Scenario2 = netCashFlow2.ToString("F2")
            'summaryResultsRecord.Difference2 = (summaryResultsRecord.Scenario1 - summaryResultsRecord.Scenario2).ToString("F2")
            'summaryResultsRecord.Reduction2 = ((100 - (summaryResultsRecord.Scenario2 / summaryResultsRecord.Scenario1) * 100)).ToString("F1")
            'summaryResultsRecord.TotalArea2 = (summaryResultsRecord.Difference2 * area).ToString("F1")
            'summaryResults.Add(summaryResultsRecord)

            ''********************* comparison values for scenario1 and scenario3 *************************
            'summaryResultsRecord = New SummaryResults
            'summaryResultsRecord.Description = cntDoc.Descendants("TotalRevenue").Value
            'summaryResultsRecord.Scenario1 = totalRevenue1.ToString("F2")
            'summaryResultsRecord.Scenario3 = totalRevenue3.ToString("F2")
            'summaryResultsRecord.Difference3 = (summaryResultsRecord.Scenario1 - summaryResultsRecord.Scenario3).ToString("F2")
            'summaryResultsRecord.Reduction3 = ((100 - (summaryResultsRecord.Scenario3 / summaryResultsRecord.Scenario1) * 100)).ToString("F1")
            'summaryResultsRecord.TotalArea3 = 0
            'summaryResults.Add(summaryResultsRecord)

            'summaryResultsRecord = New SummaryResults
            'summaryResultsRecord.Description = cntDoc.Descendants("TotalCost").Value
            'summaryResultsRecord.Scenario1 = totalCost1.ToString("F2")
            'summaryResultsRecord.Scenario3 = totalCost3.ToString("F2")
            'summaryResultsRecord.Difference3 = (summaryResultsRecord.Scenario1 - summaryResultsRecord.Scenario3).ToString("F2")
            'summaryResultsRecord.Reduction3 = ((100 - (summaryResultsRecord.Scenario3 / summaryResultsRecord.Scenario1) * 100)).ToString("F1")
            'summaryResultsRecord.TotalArea3 = (summaryResultsRecord.Difference2 * area).ToString("F1")
            'summaryResults.Add(summaryResultsRecord)

            'summaryResultsRecord = New SummaryResults
            'summaryResultsRecord.Description = cntDoc.Descendants("NetReturn").Value
            'summaryResultsRecord.Scenario1 = netReturn1.ToString("F2")
            'summaryResultsRecord.Scenario3 = netReturn3.ToString("F2")
            'summaryResultsRecord.Difference3 = (summaryResultsRecord.Scenario1 - summaryResultsRecord.Scenario3).ToString("F2")
            'summaryResultsRecord.Reduction3 = ((100 - (summaryResultsRecord.Scenario3 / summaryResultsRecord.Scenario1) * 100)).ToString("F1")
            'summaryResultsRecord.TotalArea3 = (summaryResultsRecord.Difference2 * area).ToString("F1")
            'summaryResults.Add(summaryResultsRecord)

            'summaryResultsRecord = New SummaryResults
            'summaryResultsRecord.Description = cntDoc.Descendants("NetCashFlow").Value
            'summaryResultsRecord.Scenario1 = netCashFlow1.ToString("F2")
            'summaryResultsRecord.Scenario3 = netCashFlow3.ToString("F2")
            'summaryResultsRecord.Difference3 = (summaryResultsRecord.Scenario1 - summaryResultsRecord.Scenario3).ToString("F2")
            'summaryResultsRecord.Reduction3 = ((100 - (summaryResultsRecord.Scenario3 / summaryResultsRecord.Scenario1) * 100)).ToString("F1")
            'summaryResultsRecord.TotalArea3 = (summaryResultsRecord.Difference2 * area).ToString("F1")
            'summaryResults.Add(summaryResultsRecord)
            gvResults.DataSource = summaryResults
            gvResults.DataBind()
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try

    End Sub

    Protected Sub btnGraphs_Click(sender As Object, e As EventArgs) Handles btnGraphs.Click
        btnSummary.CommandArgument = 0
        btnGraphs.CommandArgument = 1
        btnEconomics.CommandArgument = 0
        btnBySoil.CommandArgument = 0
        'todo chek this things
        sctSummary.Style.Item("display") = "none"
        fsetSoils.Style.Item("display") = "none"
        sctGraphs.Style.Item("display") = ""
        ddlAverage.SelectedIndex = 0
        ddlInfo.SelectedIndex = 0
        If ddlType.SelectedIndex = 0 Then
            charts()
        Else
            chartsSubPrj()
        End If
    End Sub
    Private Sub ChartsSubPrj()
        If ddlScenarios1.SelectedIndex = 0 And ddlScenarios2.SelectedIndex = 0 And ddlScenarios3.SelectedIndex = 0 Then Exit Sub
        Try
            Dim years(11) As UShort
            Dim scenario1(11) As Single
            Dim scenario2(11) As Single
            Dim scenario3(11) As Single
            Dim totalSelectedArea As Single = _fieldsInfo1(currentFieldNumber).Area
            Dim totalYears As UShort = 0
            Dim point As DataPoint

            Chart1.Series(0).Points.Clear()
            Chart1.Series(1).Points.Clear()
            Chart1.Series(2).Points.Clear()
            Chart1.Series(0).Name = ddlSubproject1.SelectedItem.Text
            If ddlSubproject1.SelectedItem.Text <> ddlSubproject2.SelectedItem.Text Then
                Chart1.Series(1).Name = ddlSubproject2.SelectedItem.Text 'change the series name if it is diferent from scenario1
            Else
                Chart1.Series(1).Name = ddlSubproject2.SelectedItem.Text & "_"  'change the series name if it is diferent from scenario1
            End If
            If ddlSubproject3.SelectedItem.Text = ddlSubproject2.SelectedItem.Text Or ddlSubproject3.SelectedItem.Text = ddlSubproject1.SelectedItem.Text Then
                Chart1.Series(2).Name = ddlSubproject3.SelectedItem.Text & "__"  'change the series name if it is diferent from scenario1
            Else
                Chart1.Series(2).Name = ddlSubproject3.SelectedItem.Text   'change the series name if it is diferent from scenario1
            End If

            Dim results1 As New ScenariosData.APEXResults
            Dim results2 As New ScenariosData.APEXResults
            Dim results3 As New ScenariosData.APEXResults
            If ddlSubproject1.SelectedIndex > 0 Then results1 = _subprojectName(ddlSubproject1.SelectedIndex - 1)._results
            If ddlSubproject2.SelectedIndex > 0 Then results2 = _subprojectName(ddlSubproject2.SelectedIndex - 1)._results
            If ddlSubproject3.SelectedIndex > 0 Then results3 = _subprojectName(ddlSubproject3.SelectedIndex - 1)._results
            If ddlAverage.SelectedIndex = 0 Then
                If _months.Count <= 0 Then LoadMonths()
                totalYears = _startInfo.stationYears - 1 - 1      'calculate the number of years to show in annual graphs. -1 because it started from 0 and -1 because the first year is excluded.
                If totalYears > 11 Then totalYears = 11
                Dim j As UShort = 0

                For i = _startInfo.stationFinalYear - totalYears To _startInfo.stationFinalYear
                    years(j) = i
                    j += 1
                Next
                Dim l As UShort = 0

                For k = 12 To 23
                    point = New DataPoint
                    Select Case ddlInfo.SelectedIndex
                        Case 0  'flow
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("Flow").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(l) = results1.annualFlow(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(l) = results2.annualFlow(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(l) = results3.annualFlow(k)
                        Case 1  'sediment
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("Sediment").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(l) = results1.annualSediment(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(l) = results2.annualSediment(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(l) = results3.annualSediment(k)
                        Case 2  'Org N
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("OrgN").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(l) = results1.annualOrgN(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(l) = results2.annualOrgN(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(l) = results3.annualOrgN(k)
                        Case 3  'Org P
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("OrgP").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(l) = results1.annualOrgP(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(l) = results2.annualOrgP(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(l) = results3.annualOrgP(k)
                        Case 4  'NO3
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("NO3").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(l) = results1.annualNO3(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(l) = results2.annualNO3(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(l) = results3.annualNO3(k)
                        Case 5  'PO4
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("PO4").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(l) = results1.annualPO4(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(l) = results2.annualPO4(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(l) = results3.annualPO4(k)
                        Case 6  'Total N
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("TotalN").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(l) = results1.annualOrgN(k) + results1.annualNO3(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(l) = results2.annualOrgN(k) + results2.annualNO3(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(l) = results3.annualOrgN(k) + results3.annualNO3(k)
                        Case 7  'Total P
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("TotalP").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(l) = results1.annualOrgP(k) + results1.annualPO4(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(l) = results2.annualOrgP(k) + results2.annualPO4(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(l) = results3.annualOrgP(k) + results3.annualPO4(k)
                        Case 8  'Precipitation
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("Precipitation").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(l) = results1.annualPrecipitation(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(l) = results2.annualPrecipitation(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(l) = results3.annualPrecipitation(k)
                        Case 9  'Crop Yield
                    End Select
                    point.SetValueXY(_months(l + 1).Name.ToString, scenario1(l))
                    point.XValue = l + 1
                    Chart1.Series(0).Points.Add(point)
                    point = New DataPoint
                    point.SetValueXY(_months(l + 1).Name.ToString, scenario2(l))
                    point.XValue = l + 1
                    Chart1.Series(1).Points.Add(point)
                    point = New DataPoint
                    point.SetValueXY(_months(l + 1).Name.ToString, scenario3(l))
                    point.XValue = l + 1
                    Chart1.Series(2).Points.Add(point)
                    l += 1
                Next
            Else
                For k = 0 To 11
                    point = New DataPoint
                    Select Case ddlInfo.SelectedIndex
                        Case 0  'flow
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("Flow").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(k) = results1.annualFlow(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(k) = results2.annualFlow(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(k) = results3.annualFlow(k)
                        Case 1  'sediment
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("Sediment").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(k) = results1.annualSediment(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(k) = results2.annualSediment(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(k) = results3.annualSediment(k)
                        Case 2  'Org N
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("OrgN").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(k) = results1.annualOrgN(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(k) = results2.annualOrgN(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(k) = results3.annualOrgN(k)
                        Case 3  'Org P
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("OrgP").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(k) = results1.annualOrgP(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(k) = results2.annualOrgP(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(k) = results3.annualOrgP(k)
                        Case 4  'NO3
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("NO3").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(k) = results1.annualNO3(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(k) = results2.annualNO3(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(k) = results3.annualNO3(k)
                        Case 5  'PO4
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("PO4").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(k) = results1.annualPO4(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(k) = results2.annualPO4(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(k) = results3.annualPO4(k)
                        Case 6  'Total N
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("TotalN").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(k) = results1.annualOrgN(k) + results1.annualNO3(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(k) = results2.annualOrgN(k) + results2.annualNO3(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(k) = results3.annualOrgN(k) + results3.annualNO3(k)
                        Case 7  'Total P
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("TotalP").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(k) = results1.annualOrgP(k) + results1.annualPO4(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(k) = results2.annualOrgP(k) + results2.annualPO4(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(k) = results3.annualOrgP(k) + results3.annualPO4(k)
                        Case 8  'Precipitation
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("Precipitation").Value
                            If ddlSubproject1.SelectedIndex > 0 Then scenario1(k) = results1.annualPrecipitation(k)
                            If ddlSubproject2.SelectedIndex > 0 Then scenario2(k) = results2.annualPrecipitation(k)
                            If ddlSubproject3.SelectedIndex > 0 Then scenario3(k) = results3.annualPrecipitation(k)
                        Case 9  'Crop Yield
                            Dim listItem As ListItem = GetCropConvertions()
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("Crop").Value & " - " & ddlCrop.Items(ddlCrop.SelectedIndex).Text & "(" & listItem.Text & "/ac)"
                            Dim nCrop As UShort = 0
                            If ddlSubproject1.SelectedIndex > 0 Then
                                For Each crop In results1.annualCropYield(k).cropName
                                    If Not crop Is Nothing Then
                                        If crop.Trim = ddlCrop.Items(ddlCrop.SelectedIndex).Text.Trim Then
                                            scenario1(k) = results1.annualCropYield(k).cropYield(nCrop)
                                        End If
                                    Else
                                        scenario1(k) = 0
                                    End If
                                    nCrop += 1
                                Next
                            End If
                            nCrop = 0
                            If ddlSubproject2.SelectedIndex > 0 Then
                                For Each crop In results2.annualCropYield(k).cropName
                                    If Not crop Is Nothing Then
                                        If crop.Trim = ddlCrop.Items(ddlCrop.SelectedIndex).Text.Trim Then
                                            scenario2(k) = results2.annualCropYield(k).cropYield(nCrop)
                                        End If
                                    Else
                                        scenario1(k) = 0
                                    End If
                                    nCrop += 1
                                Next
                            End If
                            nCrop = 0
                            If ddlSubproject3.SelectedIndex > 0 Then
                                For Each crop In results3.annualCropYield(k).cropName
                                    If Not crop Is Nothing Then
                                        If crop.Trim = ddlCrop.Items(ddlCrop.SelectedIndex).Text.Trim Then
                                            scenario3(k) = results3.annualCropYield(k).cropYield(nCrop)
                                        End If
                                    Else
                                        scenario1(k) = 0
                                    End If
                                    nCrop += 1
                                Next
                            End If
                    End Select
                    point.SetValueY(scenario1(k))
                    point.XValue = k + 1
                    Chart1.Series(0).Points.Add(point)
                    point = New DataPoint
                    point.SetValueY(scenario2(k))
                    point.XValue = k + 1
                    Chart1.Series(1).Points.Add(point)
                    point = New DataPoint
                    point.SetValueY(scenario3(k))
                    point.XValue = k + 1
                    Chart1.Series(2).Points.Add(point)
                Next
            End If
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & " " & ex.Message)
        End Try
    End Sub

    Private Sub Charts()
        If ddlScenarios1.SelectedIndex = 0 And ddlScenarios2.SelectedIndex = 0 And ddlScenarios3.SelectedIndex = 0 Then Exit Sub
        Try
            Dim years(11) As UShort
            Dim scenario1(11) As Single
            Dim scenario2(11) As Single
            Dim scenario3(11) As Single
            Dim totalSelectedArea As Single = _fieldsInfo1(currentFieldNumber).Area
            Dim totalYears As UShort = 0
            Dim point As DataPoint
            Dim year As UShort = 0

            Chart1.Series(0).Points.Clear()
            Chart1.Series(1).Points.Clear()
            Chart1.Series(2).Points.Clear()
            Chart1.Series(0).Name = ddlScenarios1.Items(ddlScenarios1.SelectedIndex).Text

            If ddlScenarios1.Items(ddlScenarios1.SelectedIndex).Text <> ddlScenarios2.Items(ddlScenarios2.SelectedIndex).Text Then
                Chart1.Series(1).Name = ddlScenarios2.Items(ddlScenarios2.SelectedIndex).Text  'change the series name if it is diferent from scenario1
            Else
                Chart1.Series(1).Name = ddlScenarios2.Items(ddlScenarios2.SelectedIndex).Text & "_"  'change the series name if it is diferent from scenario1
            End If
            If ddlScenarios3.Items(ddlScenarios3.SelectedIndex).Text = ddlScenarios2.Items(ddlScenarios2.SelectedIndex).Text Or ddlScenarios3.Items(ddlScenarios3.SelectedIndex).Text = ddlScenarios1.Items(ddlScenarios1.SelectedIndex).Text Then
                Chart1.Series(2).Name = ddlScenarios3.Items(ddlScenarios3.SelectedIndex).Text & "__"  'change the series name if it is diferent from scenario1
            Else
                Chart1.Series(2).Name = ddlScenarios3.Items(ddlScenarios3.SelectedIndex).Text   'change the series name if it is diferent from scenario1
            End If

            Dim results1 As New ScenariosData.APEXResults
            Dim results2 As New ScenariosData.APEXResults
            Dim results3 As New ScenariosData.APEXResults
            If ddlScenarios1.SelectedIndex > 0 Then results1 = _fieldsInfo1(ddlField.SelectedIndex)._scenariosInfo(ddlScenarios1.SelectedIndex - 1)._results
            If ddlScenarios2.SelectedIndex > 0 Then results2 = _fieldsInfo1(ddlField.SelectedIndex)._scenariosInfo(ddlScenarios2.SelectedIndex - 1)._results
            If ddlScenarios3.SelectedIndex > 0 Then results3 = _fieldsInfo1(ddlField.SelectedIndex)._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results
            If ddlAverage.SelectedIndex = 0 Then
                If _months.Count <= 0 Then LoadMonths()
                totalYears = _startInfo.stationYears - 1 - 1      'calculate the number of years to show in annual graphs. -1 because it started from 0 and -1 because the first year is excluded.
                If totalYears > 11 Then totalYears = 11
                Dim j As UShort = 0

                For i = _startInfo.stationFinalYear - totalYears To _startInfo.stationFinalYear
                    years(j) = i
                    j += 1
                Next
                Dim l As UShort = 0

                For k = 12 To 23
                    point = New DataPoint
                    Select Case ddlInfo.SelectedIndex
                        Case 0  'flow
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("Flow").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(l) = results1.annualFlow(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(l) = results2.annualFlow(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(l) = results3.annualFlow(k)
                        Case 1  'sediment
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("Sediment").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(l) = results1.annualSediment(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(l) = results2.annualSediment(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(l) = results3.annualSediment(k)
                        Case 2  'Org N
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("OrgN").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(l) = results1.annualOrgN(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(l) = results2.annualOrgN(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(l) = results3.annualOrgN(k)
                        Case 3  'Org P
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("OrgP").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(l) = results1.annualOrgP(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(l) = results2.annualOrgP(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(l) = results3.annualOrgP(k)
                        Case 4  'NO3
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("NO3").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(l) = results1.annualNO3(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(l) = results2.annualNO3(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(l) = results3.annualNO3(k)
                        Case 5  'PO4
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("PO4").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(l) = results1.annualPO4(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(l) = results2.annualPO4(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(l) = results3.annualPO4(k)
                        Case 6  'Total N
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("TotalN").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(l) = results1.annualOrgN(k) + results1.annualNO3(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(l) = results2.annualOrgN(k) + results2.annualNO3(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(l) = results3.annualOrgN(k) + results3.annualNO3(k)
                        Case 7  'Total P
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("TotalP").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(l) = results1.annualOrgP(k) + results1.annualPO4(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(l) = results2.annualOrgP(k) + results2.annualPO4(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(l) = results3.annualOrgP(k) + results3.annualPO4(k)
                        Case 8  'Precipitation
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("MonthlyAverage").Value & " - " & cntDoc.Descendants("Precipitation").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(l) = results1.annualPrecipitation(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(l) = results2.annualPrecipitation(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(l) = results3.annualPrecipitation(k)
                        Case 9  'N2O
                        Case 10  'Crop Yield
                    End Select
                    point.SetValueXY(_months(l + 1).Name.ToString, scenario1(l))
                    point.XValue = l + 1
                    Chart1.Series(0).Points.Add(point)
                    point = New DataPoint
                    point.SetValueXY(_months(l + 1).Name.ToString, scenario2(l))
                    point.XValue = l + 1
                    Chart1.Series(1).Points.Add(point)
                    point = New DataPoint
                    point.SetValueXY(_months(l + 1).Name.ToString, scenario3(l))
                    point.XValue = l + 1
                    Chart1.Series(2).Points.Add(point)
                    l += 1
                Next
            Else
                For k = 0 To 11
                    point = New DataPoint
                    Select Case ddlInfo.SelectedIndex
                        Case 0  'flow
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("Flow").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(k) = results1.annualFlow(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(k) = results2.annualFlow(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(k) = results3.annualFlow(k)
                        Case 1  'sediment
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("Sediment").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(k) = results1.annualSediment(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(k) = results2.annualSediment(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(k) = results3.annualSediment(k)
                        Case 2  'Org N
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("OrgN").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(k) = results1.annualOrgN(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(k) = results2.annualOrgN(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(k) = results3.annualOrgN(k)
                        Case 3  'Org P
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("OrgP").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(k) = results1.annualOrgP(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(k) = results2.annualOrgP(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(k) = results3.annualOrgP(k)
                        Case 4  'NO3
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("NO3").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(k) = results1.annualNO3(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(k) = results2.annualNO3(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(k) = results3.annualNO3(k)
                        Case 5  'PO4
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("PO4").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(k) = results1.annualPO4(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(k) = results2.annualPO4(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(k) = results3.annualPO4(k)
                        Case 6  'Total N
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("TotalN").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(k) = results1.annualOrgN(k) + results1.annualNO3(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(k) = results2.annualOrgN(k) + results2.annualNO3(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(k) = results3.annualOrgN(k) + results3.annualNO3(k)
                        Case 7  'Total P
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("TotalP").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(k) = results1.annualOrgP(k) + results1.annualPO4(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(k) = results2.annualOrgP(k) + results2.annualPO4(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(k) = results3.annualOrgP(k) + results3.annualPO4(k)
                        Case 8  'Precipitation
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("Precipitation").Value
                            If ddlScenarios1.SelectedIndex > 0 Then scenario1(k) = results1.annualPrecipitation(k)
                            If ddlScenarios2.SelectedIndex > 0 Then scenario2(k) = results2.annualPrecipitation(k)
                            If ddlScenarios3.SelectedIndex > 0 Then scenario3(k) = results3.annualPrecipitation(k)
                        Case 9  'N2O
                            If _startInfo.StateAbrev = "PR" Or _startInfo.StateAbrev = "TX" Or _startInfo.StateAbrev = "OK" Or _startInfo.StateAbrev = "KS" Then
                                Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - N2O"
                                If ddlScenarios1.SelectedIndex > 0 Then scenario1(k) = results1.annualN2o(k)
                                If ddlScenarios2.SelectedIndex > 0 Then scenario2(k) = results2.annualN2o(k)
                                If ddlScenarios3.SelectedIndex > 0 Then scenario3(k) = results3.annualN2o(k)
                            End If
                        Case 10  'Crop Yield
                            Dim listItem As ListItem = GetCropConvertions()
                            Chart1.Titles("Title1").Text = cntDoc.Descendants("AnnualAverage").Value & " - " & cntDoc.Descendants("Crop").Value & " - " & ddlCrop.Items(ddlCrop.SelectedIndex).Text & "(" & listItem.Text & "/ac)"
                            Dim nCrop As UShort = 0
                            If ddlScenarios1.SelectedIndex > 0 Then
                                For Each crop In results1.annualCropYield(k).cropName
                                    If Not crop Is Nothing Then
                                        If crop.Trim = ddlCrop.Items(ddlCrop.SelectedIndex).Text.Trim Then
                                            scenario1(k) = results1.annualCropYield(k).cropYield(nCrop)
                                        End If
                                        'Else
                                        'scenario1(k) = 0
                                    End If
                                    nCrop += 1
                                Next
                            End If
                            nCrop = 0
                            If ddlScenarios2.SelectedIndex > 0 Then
                                For Each crop In results2.annualCropYield(k).cropName
                                    If Not crop Is Nothing Then
                                        If crop.Trim = ddlCrop.Items(ddlCrop.SelectedIndex).Text.Trim Then
                                            scenario2(k) = results2.annualCropYield(k).cropYield(nCrop)
                                        End If
                                        'Else
                                        'scenario2(k) = 0
                                    End If
                                    nCrop += 1
                                Next
                            End If
                            nCrop = 0
                            If ddlScenarios3.SelectedIndex > 0 Then
                                For Each crop In results3.annualCropYield(k).cropName
                                    If Not crop Is Nothing Then
                                        If crop.Trim = ddlCrop.Items(ddlCrop.SelectedIndex).Text.Trim Then
                                            scenario3(k) = results3.annualCropYield(k).cropYield(nCrop)
                                        End If
                                        'Else
                                        'scenario3(k) = 0
                                    End If
                                    nCrop += 1
                                Next
                            End If
                    End Select
                    If _startInfo.stationYears > 12 Then
                        year = _startInfo.stationFinalYear - 12 + k + 1
                    Else
                        year = _startInfo.stationFinalYear - (_startInfo.stationFinalYear - _startInfo.stationInitialYear + 5 + 1) + k + 1
                    End If
                    point.SetValueY(scenario1(k))
                    point.XValue = year
                    Chart1.Series(0).Points.Add(point)
                    point = New DataPoint
                    point.SetValueY(scenario2(k))
                    'point.XValue = k + 1
                    point.XValue = year
                    Chart1.Series(1).Points.Add(point)
                    point = New DataPoint
                    point.SetValueY(scenario3(k))
                    point.XValue = year
                    Chart1.Series(2).Points.Add(point)
                Next
            End If
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & " " & ex.Message)
        End Try
    End Sub

    Private Sub GetSoilResults(soilNumber As UShort)
        Dim _cropsInfo1 As List(Of CropsInfo) = New List(Of CropsInfo)
        Dim _cropsInfo2 As List(Of CropsInfo) = New List(Of CropsInfo)
        Dim _cropsInfo3 As List(Of CropsInfo) = New List(Of CropsInfo)
        Dim totalSelectedArea As Single = _fieldsInfo1(currentFieldNumber).Area * _fieldsInfo1(currentFieldNumber)._soilsInfo(soilNumber).Percentage / 100
        Dim _allResults1 As AllResults
        Dim _allResults2 As AllResults
        Dim _allResults3 As AllResults
        Dim cropName() As String = Nothing
        Dim i As Short = 0
        Try
            'Note: The ddlScenario.SelecteIndex substract 1 because I added the selectOne option in the ddl.
            If ddlScenarios1.SelectedIndex > 0 Then
                'Crop Yield Scenario No. 1
                GetCropResults(_cropsInfo1, _fieldsInfo1(ddlField.SelectedIndex)._soilsInfo(soilNumber)._scenariosInfo(ddlScenarios1.SelectedIndex - 1)._results.SoilResults, cropName)
                'Nutrients Scenario No. 1
                GetNutrientsInfo(_fieldsInfo1(currentFieldNumber)._soilsInfo(soilNumber)._scenariosInfo(ddlScenarios1.SelectedIndex - 1)._results, i, totalSelectedArea, _allResults1, _fieldsInfo1(currentFieldNumber).Area)
            End If
            If ddlScenarios2.SelectedIndex > 0 Then
                'Crop Yield Scenario No. 2            
                GetCropResults(_cropsInfo2, _fieldsInfo1(currentFieldNumber)._soilsInfo(soilNumber)._scenariosInfo(ddlScenarios2.SelectedIndex - 1)._results.SoilResults, cropName)
                'Nutrients Scenario No. 2
                GetNutrientsInfo(_fieldsInfo1(currentFieldNumber)._soilsInfo(soilNumber)._scenariosInfo(ddlScenarios2.SelectedIndex - 1)._results, i, totalSelectedArea, _allResults2, _fieldsInfo1(currentFieldNumber).Area)
            End If
            If ddlScenarios3.SelectedIndex > 0 Then
                'Crop Yield Scenario No. 3
                GetCropResults(_cropsInfo3, _fieldsInfo1(currentFieldNumber)._soilsInfo(soilNumber)._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results.SoilResults, cropName)
                'Nutrients Scenario No. 3
                GetNutrientsInfo(_fieldsInfo1(currentFieldNumber)._soilsInfo(soilNumber)._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results, i, totalSelectedArea, _allResults3, _fieldsInfo1(currentFieldNumber).Area)
            End If

            gvResults.DataSource = SetResult(_allResults1, _allResults2, _allResults3, totalSelectedArea, cropName, _cropsInfo1, _cropsInfo2, _cropsInfo3)
            gvResults.DataBind()
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try

    End Sub

    Private Sub GetCropResults(ByRef cropsInfo As List(Of CropsInfo), soilResults As ScenariosData.APEXResultsAll, ByRef cropName() As String)
        Dim found As Boolean = False
        Dim crops As CropsInfo
        Dim l As Short = 0

        If soilResults.Crops.cropName Is Nothing Then Exit Sub
        For j = 0 To soilResults.Crops.cropName.GetUpperBound(0)
            found = False
            For l = 0 To cropsInfo.Count - 1
                If cropsInfo(l).name = soilResults.Crops.cropName(j) And cropsInfo(l).scenario = 1 Then
                    cropsInfo(l).yield += soilResults.Crops.cropYield(j)
                    cropsInfo(l).yieldCI += soilResults.Crops.cropYieldCI(j)
                    cropsInfo(l).area += _fieldsInfo1(currentFieldNumber).Area
                    found = True
                    Exit For
                End If
            Next
            If found = False Then
                crops = New CropsInfo
                crops.name = soilResults.Crops.cropName(j)
                crops.yield = soilResults.Crops.cropYield(j)
                crops.yieldCI = soilResults.Crops.cropYieldCI(j)
                crops.area = _fieldsInfo1(currentFieldNumber).Area
                crops.scenario = 1
                cropsInfo.Add(crops)
            End If
        Next

        Dim cropsLen As UShort = 0
        For Each crop In cropsInfo
            If cropName Is Nothing Then
                cropsLen = 0 : ReDim cropName(cropsLen) : cropName(cropsLen) = crop.name
            Else
                If Not cropName.Contains(crop.name) Then
                    cropsLen += 1 : ReDim Preserve cropName(cropsLen) : cropName(cropsLen) = crop.name
                End If
            End If
        Next
    End Sub

    Private Sub CalculateBufferAreas(record As ScenariosData, area As Single, ByRef _allResults As AllResults)
        'Dim sides As UShort = 0
        Dim areaPPPipes As Single = 0
        With _allResults
            If record._bmpsInfo.PPDSWidth > 0 Or record._bmpsInfo.PPNDWidth > 0 Then
                Select Case True
                    Case record._bmpsInfo.PPDSWidth > 0
                        'sides = record._bmpsInfo.PPDSSides
                        areaPPPipes = record._bmpsInfo.PPDSWidth * record._bmpsInfo.PPDSSides
                    Case record._bmpsInfo.PPNDWidth > 0
                        'sides = record._bmpsInfo.PPNDSides
                        areaPPPipes = record._bmpsInfo.PPNDWidth * record._bmpsInfo.PPNDSides
                End Select
                .areaPPPipes += (Math.Sqrt(area * ac_to_km2) * (areaPPPipes * ft_to_km)) / ac_to_km2 / ac_to_ha
            End If
            For Each Buf In record._bufferInfo
                Select Case True
                    Case Buf.SbaType = "FS"
                        .areaFS += Buf._line4(0).Wsa / ac_to_ha
                    Case Buf.SbaType Like "PP*"
                        Select Case True
                            Case record._bmpsInfo.PPDEWidth > 0
                                'sides = record._bmpsInfo.PPDESides
                                areaPPPipes = record._bmpsInfo.PPDEWidth * record._bmpsInfo.PPDESides
                            Case record._bmpsInfo.PPTWWidth > 0
                                'sides = record._bmpsInfo.PPTWSides
                                areaPPPipes = record._bmpsInfo.PPTWWidth * record._bmpsInfo.PPTWSides
                        End Select
                        .areaPPPipes += (Math.Sqrt(area * ac_to_km2) * (areaPPPipes * ft_to_km)) / ac_to_km2 / ac_to_ha
                        .areaPPRes += Buf._line4(0).Wsa / ac_to_ha
                    Case Buf.SbaType = "CBFS"
                        .areaCBFS += Buf._line4(0).Wsa / ac_to_ha
                    Case Buf.SbaType Like "RF*"
                        .areaRF += Buf._line4(0).Wsa / ac_to_ha
                    Case Buf.SbaType = "WL"
                        .areaWL += Buf._line4(0).Wsa / ac_to_ha
                    Case Buf.SbaType = "WW"
                        .areaWW += Buf._line4(0).Wsa / ac_to_ha
                    Case Buf.SbaType = "Sdg"
                        .areaSdg += Buf._line4(0).Wsa / ac_to_ha
                End Select
            Next
        End With

    End Sub

    Private Sub GetNutrientsInfo(results As ScenariosData.APEXResults, i As Short, totalSelectedArea As Single, ByRef _allResults As AllResults, thisArea As Single)
        With _allResults
            'N calculations
            .totalN += results.SoilResults.TotalN
            .orgN += (results.SoilResults.OrgN)
            .orgNCI += (results.SoilResults.OrgNCI)
            .runoffN += (results.SoilResults.RunoffN)                 'tile drain is substracted because N)3 already has it included.
            .runoffNCI += (results.SoilResults.runoffNCI)                 'tile drain is substracted because N)3 already has it included.
            .subsurfaceN += (results.SoilResults.SubsurfaceN)
            .SubsurfaceNCI += (results.SoilResults.subsurfaceNCI)
            .tileDrainN += (results.SoilResults.tileDrainN)
            .tileDrainNCI += (results.SoilResults.tileDrainNCI)
            .totalNCI += .orgNCI + .runoffNCI + .SubsurfaceNCI + .tileDrainNCI
            'P calculations
            .totalP += results.SoilResults.TotalP
            .orgP += (results.SoilResults.OrgP)
            .orgPCI += results.SoilResults.OrgPCI
            .PO4 += (results.SoilResults.PO4)
            .PO4CI += results.SoilResults.PO4CI
            .tileDrainP += (results.SoilResults.tileDrainP)
            .tileDrainPCI += results.SoilResults.tileDrainP
            .totalPCI += .orgPCI + .PO4CI + .tileDrainPCI
            'Flow calculations
            .totalFlow += (results.SoilResults.TotalFlow)
            .runoff += (results.SoilResults.runoff)
            .runoffCI += (results.SoilResults.runoffCI)
            .subsurfaceFlow += (results.SoilResults.subsurfaceFlow)
            .subsurfaceFlowCI += (results.SoilResults.subsurfaceFlowCI)
            .tileDrainFlow += (results.SoilResults.tileDrainFlow)
            .tileDrainFlowCI += (results.SoilResults.tileDrainFlowCI)
            .totalFlowCI += .runoffCI + .subsurfaceFlowCI + .tileDrainFlowCI
            'Other water info calculations
            .deepPerFlow += (results.SoilResults.deepPerFlow)
            .deepPerFlowCI += (results.SoilResults.deepPerFlowCI)
            .irrigation += (results.SoilResults.irrigation)           'todo irrigationci do not exist
            .irrigationCI += (results.SoilResults.irrigationCI)           'todo irrigationci do not exist
            .totalOtherWaterInfo = .deepPerFlow + .irrigation
            .totalOtherWaterInfoCI = .deepPerFlowCI + .irrigationCI
            'sediment info calculations
            .Sediment += (results.SoilResults.Sediment)
            .sedimentCi += (results.SoilResults.SedimentCI)
            .ManureErosion += (results.SoilResults.ManureErosion)
            .ManureErosionCi += (results.SoilResults.ManureErosionCI)
            .totalSediment = .Sediment + .ManureErosion
            .totalSedimentCI = .sedimentCi + .ManureErosionCi
            'n2o calculations
            .n2o += results.SoilResults.n2o
            'deep per N
            .deepPerN += results.SoilResults.LeachedN
            'deep per P
            .deepPerP += results.SoilResults.LeachedP
        End With
    End Sub

    Private Sub CalculateNutrientsForFencing(record As ScenariosData, totalSelectedArea As Single, ByRef _allResults As AllResults)
        Dim totalManure As Single = 0
        Dim no3 As Single = 0 : Dim po4 As Single = 0 : Dim orgN As Single = 0 : Dim orgP As Single = 0

        With record
            If ._bmpsInfo.SFAnimals > 0 Then
                totalManure = ._bmpsInfo.SFAnimals * record._bmpsInfo.SFHours / 24 * ._bmpsInfo.SFDryManure
                no3 = totalManure * ._bmpsInfo.SFDays * ._bmpsInfo.SFNo3
                po4 = totalManure * ._bmpsInfo.SFDays * ._bmpsInfo.SFPo4
                orgN = totalManure * ._bmpsInfo.SFDays * ._bmpsInfo.SFOrgN
                orgP = totalManure * ._bmpsInfo.SFDays * ._bmpsInfo.SFOrgP
                _allResults.totalN += no3 / totalSelectedArea + orgN / totalSelectedArea
                _allResults.totalP += po4 / totalSelectedArea + orgP / totalSelectedArea
                _allResults.runoffN += no3 / totalSelectedArea
                _allResults.PO4 += po4 / totalSelectedArea
                _allResults.orgN += orgN / totalSelectedArea
                _allResults.orgP += orgP / totalSelectedArea
            End If
        End With
    End Sub

    Private Sub GetScenarioValues(Description As String, firstScenario As Single, secondScenario As Single, thirdScenario As Single, type As String, sign As Boolean, CI1 As String, CI2 As String, CI3 As String, area As Single)
        Dim summaryResultsRecord As SummaryResults
        summaryResultsRecord = New SummaryResults

        summaryResultsRecord.Description = Description
        summaryResultsRecord.Type = type
        If Not (Description.Contains("Crop Yield") Or Description.Contains("Cultivo Cosechado")) Then
            summaryResultsRecord.Scenario1 = firstScenario
            summaryResultsRecord.Scenario2 = secondScenario
            summaryResultsRecord.Scenario3 = thirdScenario
            summaryResultsRecord.Difference2 = -(summaryResultsRecord.Scenario1 - summaryResultsRecord.Scenario2).ToString("F2")
            summaryResultsRecord.Reduction2 = ((100 - (summaryResultsRecord.Scenario2 / summaryResultsRecord.Scenario1) * 100)).ToString("F1")
            If Description.Contains("Yield") Or Description.Contains("Cosechado") Then
                summaryResultsRecord.TotalArea2 = (-1 * ((firstScenario * (area - totalBufferArea1)) - (secondScenario * (area - totalBufferArea2)))).ToString("F1")
            Else
                summaryResultsRecord.TotalArea2 = (summaryResultsRecord.Difference2 * area).ToString("F1")
            End If
            summaryResultsRecord.Difference3 = -(summaryResultsRecord.Scenario1 - summaryResultsRecord.Scenario3).ToString("F2")
            summaryResultsRecord.Reduction3 = ((100 - (summaryResultsRecord.Scenario3 / summaryResultsRecord.Scenario1) * 100)).ToString("F1")
            If Description.Contains("Yield") Or Description.Contains("Cosechado") Then
                summaryResultsRecord.TotalArea3 = (-1 * ((firstScenario * (area - totalBufferArea1)) - (thirdScenario * (area - totalBufferArea3)))).ToString("F1")
            Else
                summaryResultsRecord.TotalArea3 = (summaryResultsRecord.Difference3 * area).ToString("F1")
            End If
            If CI1 <> "" Then
                summaryResultsRecord.Ci1 = (CI1)
                summaryResultsRecord.Ci2 = (CI2)
                summaryResultsRecord.Ci3 = (CI3)
            End If
        End If
        If sign = True Then summaryResultsRecord.Sign = sign
        SetResults.Add(summaryResultsRecord)
    End Sub

    Private Function GetScenarioValuesEconomics(Description As String, firstScenario As Single, secondScenario As Single, thirdScenario As Single, type As String, sign As Boolean, CI1 As String, CI2 As String, CI3 As String, area As Single) As SummaryResults
        Dim summaryResultsRecord As SummaryResults
        summaryResultsRecord = New SummaryResults

        summaryResultsRecord.Description = Description
        summaryResultsRecord.Type = type
        If Not (Description.Contains("Crop Yield") Or Description.Contains("Cultivo Cosechado")) Then
            summaryResultsRecord.Scenario1 = firstScenario
            summaryResultsRecord.Scenario2 = secondScenario
            summaryResultsRecord.Scenario3 = thirdScenario
            summaryResultsRecord.Difference2 = -(summaryResultsRecord.Scenario1 - summaryResultsRecord.Scenario2).ToString("F2")
            summaryResultsRecord.Reduction2 = ((100 - (summaryResultsRecord.Scenario2 / summaryResultsRecord.Scenario1) * 100)).ToString("F1")
            summaryResultsRecord.TotalArea2 = (summaryResultsRecord.Difference2 * area).ToString("F1")
            summaryResultsRecord.Difference3 = -(summaryResultsRecord.Scenario1 - summaryResultsRecord.Scenario3).ToString("F2")
            summaryResultsRecord.Reduction3 = ((100 - (summaryResultsRecord.Scenario3 / summaryResultsRecord.Scenario1) * 100)).ToString("F1")
            summaryResultsRecord.TotalArea3 = (summaryResultsRecord.Difference3 * area).ToString("F1")
            'If CI1 <> "" Then
            '    summaryResultsRecord.Scenario1 &= (" ±" & CI1)
            '    summaryResultsRecord.Scenario2 &= (" ±" & CI2)
            '    summaryResultsRecord.Scenario3 &= (" ±" & CI3)
            'End If
        End If
        If sign = True Then summaryResultsRecord.Sign = sign
        'SetResults.Add(summaryResultsRecord)
        Return summaryResultsRecord
    End Function

    Private Function SetResult(ByRef _allResults1 As AllResults, ByRef _allResults2 As AllResults, ByRef _allResults3 As AllResults, area As Single, cropName() As String, ByRef _cropsInfo1 As List(Of CropsInfo), ByRef _cropsInfo2 As List(Of CropsInfo), ByRef _cropsInfo3 As List(Of CropsInfo)) As List(Of SummaryResults)
        'Dim conversionFactor As Single = 1, dryMatter As Single = 100
        'Dim yieldUnit As String = "t"
        SetResults = New List(Of SummaryResults)
        'Dim bufferAreas As Single = 0

        Try
            signAreas = False
            'Total Area
            GetScenarioValues(cntDoc.Descendants("TotalArea2").Value, area.ToString("F2"), area.ToString("F2"), area.ToString("F2"), "Total", signA, "", "", "", area)
            'Main Area
            GetScenarioValues(cntDoc.Descendants("MainArea").Value, (area - _allResults1.totalBufferArea).ToString("F2"), (area - _allResults2.totalBufferArea).ToString("F2"), (area - _allResults3.totalBufferArea).ToString("F2"), "Detail", False, "", "", "", area)
            signAreas = True
            'Filter Strip Area
            If _allResults1.areaFS > 0 Or _allResults2.areaFS > 0 Or _allResults3.areaFS > 0 Then
                GetScenarioValues(cntDoc.Descendants("FSArea").Value, _allResults1.areaFS.ToString("F2"), _allResults2.areaFS.ToString("F2"), _allResults3.areaFS.ToString("F2"), "Detail", False, "", "", "", area)
            End If
            'Pads and Pipes Area
            If _allResults1.areaPPPipes > 0 Or _allResults2.areaPPPipes > 0 Or _allResults3.areaPPPipes > 0 Then
                GetScenarioValues(cntDoc.Descendants("PPArea").Value, _allResults1.areaPPPipes.ToString("F2"), _allResults2.areaPPPipes.ToString("F2"), _allResults3.areaPPPipes.ToString("F2"), "Detail", False, "", "", "", area)
            End If
            'Pads and Pipes Reservoirs Area
            If _allResults1.areaPPRes > 0 Or _allResults2.areaPPRes > 0 Or _allResults3.areaPPRes > 0 Then
                GetScenarioValues(cntDoc.Descendants("PPResArea").Value, _allResults1.areaPPRes.ToString("F2"), _allResults2.areaPPRes.ToString("F2"), _allResults3.areaPPRes.ToString("F2"), "Detail", False, "", "", "", area)
            End If
            'contour buffer Area
            If _allResults1.areaCBFS > 0 Or _allResults2.areaCBFS > 0 Or _allResults3.areaCBFS > 0 Then
                GetScenarioValues(cntDoc.Descendants("CBArea").Value, _allResults1.areaCBFS.ToString("F2"), _allResults2.areaCBFS.ToString("F2"), _allResults3.areaCBFS.ToString("F2"), "Detail", False, "", "", "", area)
            End If
            ''Riparian forest Area
            If _allResults1.areaRF > 0 Or _allResults2.areaRF > 0 Or _allResults3.areaRF > 0 Then
                GetScenarioValues(cntDoc.Descendants("RFArea").Value, _allResults1.areaRF.ToString("F2"), _allResults2.areaRF.ToString("F2"), _allResults3.areaRF.ToString("F2"), "Detail", False, "", "", "", area)
            End If
            'Wetland Area
            If _allResults1.areaWL > 0 Or _allResults2.areaWL > 0 Or _allResults3.areaWL > 0 Then
                GetScenarioValues(cntDoc.Descendants("WLArea").Value, _allResults1.areaWL.ToString("F2"), _allResults2.areaWL.ToString("F2"), _allResults3.areaWL.ToString("F2"), "Detail", False, "", "", "", area)
            End If
            'Waterways Area
            If _allResults1.areaWW > 0 Or _allResults2.areaWW > 0 Or _allResults3.areaWW > 0 Then
                GetScenarioValues(cntDoc.Descendants("WWArea").Value, _allResults1.areaWW.ToString("F2"), _allResults2.areaWW.ToString("F2"), _allResults3.areaWW.ToString("F2"), "Detail", False, "", "", "", area)
            End If
            'Shading Area
            If _allResults1.areaSdg > 0 Or _allResults2.areaSdg > 0 Or _allResults3.areaSdg > 0 Then
                GetScenarioValues(cntDoc.Descendants("SdgArea").Value, _allResults1.areaSdg.ToString("F2"), _allResults2.areaSdg.ToString("F2"), _allResults3.areaSdg.ToString("F2"), "Detail", False, "", "", "", area)
            End If
            'total N
            GetScenarioValues(cntDoc.Descendants("TotalN").Value, _allResults1.totalN.ToString("F2"), _allResults2.totalN.ToString("F2"), _allResults3.totalN.ToString("F2"), "Total", signN, _allResults1.totalNCI.ToString("F2"), _allResults2.totalNCI.ToString("F2"), _allResults3.totalNCI.ToString("F2"), area)
            'Orginc N
            GetScenarioValues(cntDoc.Descendants("OrgN").Value, _allResults1.orgN.ToString("F2"), _allResults2.orgN.ToString("F2"), _allResults3.orgN.ToString("F2"), "Detail", signN, _allResults1.orgNCI.ToString("F2"), _allResults2.orgNCI.ToString("F2"), _allResults3.orgNCI.ToString("F2"), area)
            'Runoff N
            GetScenarioValues(cntDoc.Descendants("RunoffN").Value, _allResults1.runoffN.ToString("F2"), _allResults2.runoffN.ToString("F2"), _allResults3.runoffN.ToString("F2"), "Detail", False, _allResults1.runoffNCI.ToString("F2"), _allResults2.runoffNCI.ToString("F2"), _allResults3.runoffNCI.ToString("F2"), area)
            'Subsurface runoff
            GetScenarioValues(cntDoc.Descendants("SubsurfaceN").Value, _allResults1.subsurfaceN.ToString("F2"), _allResults2.subsurfaceN.ToString("F2"), _allResults3.subsurfaceN.ToString("F2"), "Detail", False, _allResults1.SubsurfaceNCI.ToString("F2"), _allResults2.SubsurfaceNCI.ToString("F2"), _allResults3.SubsurfaceNCI.ToString("F2"), area)
            'tile Drain N
            GetScenarioValues(cntDoc.Descendants("TileDrainN").Value, _allResults1.tileDrainN.ToString("F2"), _allResults2.tileDrainN.ToString("F2"), _allResults3.tileDrainN.ToString("F2"), "Detail", False, _allResults1.tileDrainNCI.ToString("F2"), _allResults2.tileDrainNCI.ToString("F2"), _allResults3.tileDrainNCI.ToString("F2"), area)
            'leaching N
            GetScenarioValues(cntDoc.Descendants("LeachedN").Value, _allResults1.deepPerN.ToString("F4"), _allResults2.deepPerN.ToString("F4"), _allResults3.deepPerN.ToString("F4"), "Detail", False, _allResults1.deepPerNCi.ToString("F2"), _allResults2.deepPerNCi.ToString("F2"), _allResults3.deepPerNCi.ToString("F2"), area)
            If _startInfo.StateAbrev = "PR" Or _startInfo.StateAbrev = "TX" Or _startInfo.StateAbrev = "OK" Or _startInfo.StateAbrev = "KS" Then
                'n2o
                GetScenarioValues(cntDoc.Descendants("N2o").Value, _allResults1.n2o.ToString("F4"), _allResults2.n2o.ToString("F4"), _allResults3.n2o.ToString("F4"), "Detail", False, _allResults1.n2oCi.ToString("F2"), _allResults2.n2oCi.ToString("F2"), _allResults3.n2oCi.ToString("F2"), area)
            End If
            'total P
            GetScenarioValues(cntDoc.Descendants("TotalP").Value, _allResults1.totalP.ToString("F2"), _allResults2.totalP.ToString("F2"), _allResults3.totalP.ToString("F2"), "Total", signP, _allResults1.totalPCI.ToString("F2"), _allResults2.totalPCI.ToString("F2"), _allResults3.totalPCI.ToString("F2"), area)
            'Orginc P
            GetScenarioValues(cntDoc.Descendants("OrgP").Value, _allResults1.orgP.ToString("F2"), _allResults2.orgP.ToString("F2"), _allResults3.orgP.ToString("F2"), "Detail", False, _allResults1.orgPCI.ToString("F2"), _allResults2.orgPCI.ToString("F2"), _allResults3.orgPCI.ToString("F2"), area)
            'PO4
            GetScenarioValues(cntDoc.Descendants("PO4").Value, _allResults1.PO4.ToString("F2"), _allResults2.PO4.ToString("F2"), _allResults3.PO4.ToString("F2"), "Detail", False, _allResults1.PO4CI.ToString("F2"), _allResults2.PO4CI.ToString("F2"), _allResults3.PO4CI.ToString("F2"), area)
            'tile Drain P
            GetScenarioValues(cntDoc.Descendants("TileDrainP").Value, _allResults1.tileDrainP.ToString("F2"), _allResults2.tileDrainP.ToString("F2"), _allResults3.tileDrainP.ToString("F2"), "Detail", False, _allResults1.tileDrainPCI.ToString("F2"), _allResults2.tileDrainPCI.ToString("F2"), _allResults3.tileDrainPCI.ToString("F2"), area)
                'leaching P
            GetScenarioValues(cntDoc.Descendants("LeachedP").Value, _allResults1.deepPerP.ToString("F4"), _allResults2.deepPerP.ToString("F4"), _allResults3.deepPerP.ToString("F4"), "Detail", False, _allResults1.deepPerPCi.ToString("F2"), _allResults2.deepPerPCi.ToString("F2"), _allResults3.deepPerPCi.ToString("F2"), area)
            'total flow
            GetScenarioValues(cntDoc.Descendants("TotalFlow").Value, _allResults1.totalFlow.ToString("F2"), _allResults2.totalFlow.ToString("F2"), _allResults3.totalFlow.ToString("F2"), "Total", signF, _allResults1.totalFlowCI.ToString("F2"), _allResults2.totalFlowCI.ToString("F2"), _allResults3.totalFlowCI.ToString("F2"), area)
            'Surface runoff
            GetScenarioValues(cntDoc.Descendants("SurfaceRunoff").Value, _allResults1.runoff.ToString("F2"), _allResults2.runoff.ToString("F2"), _allResults3.runoff.ToString("F2"), "Detail", False, _allResults1.runoffCI.ToString("F2"), _allResults2.runoffCI.ToString("F2"), _allResults3.runoffCI.ToString("F2"), area)
            'Subsruface runoff
            GetScenarioValues(cntDoc.Descendants("SubsurfaceRunoff").Value, _allResults1.subsurfaceFlow.ToString("F2"), _allResults2.subsurfaceFlow.ToString("F2"), _allResults3.subsurfaceFlow.ToString("F2"), "Detail", False, _allResults1.subsurfaceFlowCI.ToString("F2"), _allResults2.subsurfaceFlowCI.ToString("F2"), _allResults3.subsurfaceFlowCI.ToString("F2"), area)
            'Subsruface runoff
            GetScenarioValues(cntDoc.Descendants("TileDrainFlow").Value, _allResults1.tileDrainFlow.ToString("F2"), _allResults2.tileDrainFlow.ToString("F2"), _allResults3.tileDrainFlow.ToString("F2"), "Detail", False, _allResults1.tileDrainFlowCI.ToString("F2"), _allResults2.tileDrainFlowCI.ToString("F2"), _allResults3.tileDrainFlowCI.ToString("F2"), area)
            'total Other Water Info
            GetScenarioValues(cntDoc.Descendants("OtherWaterInfo").Value, _allResults1.totalOtherWaterInfo.ToString("F2"), _allResults2.totalOtherWaterInfo.ToString("F2"), _allResults3.totalOtherWaterInfo.ToString("F2"), "Total", signW, _allResults1.totalOtherWaterInfoCI.ToString("F2"), _allResults2.totalOtherWaterInfoCI.ToString("F2"), _allResults3.totalOtherWaterInfoCI.ToString("F2"), area)
            'Irrigation
            GetScenarioValues(cntDoc.Descendants("Irrigation").Value, _allResults1.irrigation.ToString("F2"), _allResults2.irrigation.ToString("F2"), _allResults3.irrigation.ToString("F2"), "Detail", False, _allResults1.irrigationCI.ToString("F2"), _allResults2.irrigationCI.ToString("F2"), _allResults3.irrigationCI.ToString("F2"), area)
            'Deep percolation
            GetScenarioValues(cntDoc.Descendants("DeepPercolation").Value, _allResults1.deepPerFlow.ToString("F2"), _allResults2.deepPerFlow.ToString("F2"), _allResults3.deepPerFlow.ToString("F2"), "Detail", False, _allResults1.deepPerFlowCI.ToString("F2"), _allResults2.deepPerFlowCI.ToString("F2"), _allResults3.deepPerFlowCI.ToString("F2"), area)
            'total Sediment
            GetScenarioValues(cntDoc.Descendants("TotalSediment").Value, _allResults1.totalSediment.ToString("F4"), _allResults2.totalSediment.ToString("F4"), _allResults3.totalSediment.ToString("F4"), "Total", signS, _allResults1.totalSedimentCI.ToString("F2"), _allResults2.totalSedimentCI.ToString("F2"), _allResults3.totalSedimentCI.ToString("F2"), area)
            'Sediment
            GetScenarioValues(cntDoc.Descendants("Sediment").Value, _allResults1.Sediment.ToString("F4"), _allResults2.Sediment.ToString("F4"), _allResults3.Sediment.ToString("F4"), "Detail", False, _allResults1.sedimentCi.ToString("F2"), _allResults2.sedimentCi.ToString("F2"), _allResults3.sedimentCi.ToString("F2"), area)
            'Manure Erosion
            GetScenarioValues(cntDoc.Descendants("ManureErosion").Value, _allResults1.ManureErosion.ToString("F4"), _allResults2.ManureErosion.ToString("F4"), _allResults3.ManureErosion.ToString("F4"), "Detail", False, _allResults1.ManureErosionCi.ToString("F2"), _allResults2.ManureErosionCi.ToString("F2"), _allResults3.ManureErosionCi.ToString("F2"), area)
            'total Crop yield
            GetScenarioValues(cntDoc.Descendants("CropYield").Value, 0, 0, 0, "Total", signC, "", "", "", 0)

            Dim format As String = "F0"

            Dim crop As String
            Dim yield1, yield2, yield3, ci1, ci2, ci3 As Single
            Dim cropInfo(3) As String
            With _fieldsInfo1(currentFieldNumber)
                If ddlScenarios1.SelectedIndex > 0 Then
                    For i = 0 To ._scenariosInfo(ddlScenarios1.SelectedIndex - 1)._results.SoilResults.Crops.cropName.Count - 1
                        crop = ._scenariosInfo(ddlScenarios1.SelectedIndex - 1)._results.SoilResults.Crops.cropName(i)
                        cropInfo = FindCropName(crop)
                        yield1 = ._scenariosInfo(ddlScenarios1.SelectedIndex - 1)._results.SoilResults.Crops.cropYield(i)
                        ci1 = ._scenariosInfo(ddlScenarios1.SelectedIndex - 1)._results.SoilResults.Crops.cropYieldCI(i)
                        Dim found As Boolean = False
                        If ddlScenarios2.SelectedIndex > 0 Then
                            Dim j As UShort = 0
                            For j = 0 To ._scenariosInfo(ddlScenarios2.SelectedIndex - 1)._results.SoilResults.Crops.cropName.Count - 1
                                If ._scenariosInfo(ddlScenarios1.SelectedIndex - 1)._results.SoilResults.Crops.cropName(i) = ._scenariosInfo(ddlScenarios2.SelectedIndex - 1)._results.SoilResults.Crops.cropName(j) Then
                                    found = True
                                    Exit For
                                End If
                            Next
                            If found = True Then
                                yield2 = ._scenariosInfo(ddlScenarios2.SelectedIndex - 1)._results.SoilResults.Crops.cropYield(j)
                                ci2 = ._scenariosInfo(ddlScenarios2.SelectedIndex - 1)._results.SoilResults.Crops.cropYieldCI(j)
                            Else
                                yield2 = 0
                                ci2 = 0
                            End If
                        End If
                        found = False
                        If ddlScenarios3.SelectedIndex > 0 Then
                            Dim j As UShort = 0
                            For j = 0 To ._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results.SoilResults.Crops.cropName.Count - 1
                                If ._scenariosInfo(ddlScenarios1.SelectedIndex - 1)._results.SoilResults.Crops.cropName(i) = ._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results.SoilResults.Crops.cropName(j) Then
                                    found = True
                                    Exit For
                                End If
                            Next
                            If found = True Then
                                yield3 = ._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results.SoilResults.Crops.cropYield(j)
                                ci3 = ._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results.SoilResults.Crops.cropYieldCI(j)
                            Else
                                yield3 = 0
                                ci3 = 0
                            End If
                        End If
                        GetScenarioValues("    " & cropInfo(3) & " " & cntDoc.Descendants("Yield").Value & " (" & cropInfo(0) & "/ac)", yield1.ToString("F0"), yield2.ToString("F0"), yield3.ToString("F0"), "Detail", False, ci1.ToString("F2"), ci2.ToString("F2"), ci2.ToString("F2"), .Area)
                    Next
                End If
                If ddlScenarios2.SelectedIndex > 0 Then
                    For i = 0 To ._scenariosInfo(ddlScenarios2.SelectedIndex - 1)._results.SoilResults.Crops.cropName.Count - 1
                        crop = ._scenariosInfo(ddlScenarios2.SelectedIndex - 1)._results.SoilResults.Crops.cropName(i)
                        cropInfo = FindCropName(crop)
                        yield2 = ._scenariosInfo(ddlScenarios2.SelectedIndex - 1)._results.SoilResults.Crops.cropYield(i)
                        ci2 = ._scenariosInfo(ddlScenarios2.SelectedIndex - 1)._results.SoilResults.Crops.cropYieldCI(i)
                        Dim found As Boolean = False
                        If ddlScenarios1.SelectedIndex > 0 Then
                            Dim j As UShort = 0
                            For j = 0 To ._scenariosInfo(ddlScenarios1.SelectedIndex - 1)._results.SoilResults.Crops.cropName.Count - 1
                                If ._scenariosInfo(ddlScenarios1.SelectedIndex - 1)._results.SoilResults.Crops.cropName(j) = ._scenariosInfo(ddlScenarios2.SelectedIndex - 1)._results.SoilResults.Crops.cropName(i) Then
                                    found = True
                                    Exit For
                                End If
                            Next
                            If found = False Then
                                yield1 = 0
                                ci1 = 0
                                found = False
                                If ddlScenarios3.SelectedIndex > 0 Then
                                    For j = 0 To ._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results.SoilResults.Crops.cropName.Count - 1
                                        If ._scenariosInfo(ddlScenarios2.SelectedIndex - 1)._results.SoilResults.Crops.cropName(i) = ._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results.SoilResults.Crops.cropName(j) Then
                                            found = True
                                            Exit For
                                        End If
                                    Next
                                    If found = True Then
                                        yield3 = ._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results.SoilResults.Crops.cropYield(j)
                                        ci3 = ._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results.SoilResults.Crops.cropYieldCI(j)
                                    Else
                                        yield3 = 0
                                        ci3 = 0
                                    End If
                                End If
                                GetScenarioValues("    " & cropInfo(3) & " " & cntDoc.Descendants("Yield").Value & " (" & cropInfo(0) & "/ac)", yield1.ToString("F0"), yield2.ToString("F0"), yield3.ToString("F0"), "Detail", False, ci1.ToString("F2"), ci2.ToString("F2"), ci3.ToString("F2"), .Area)
                            End If
                        End If
                    Next
                End If
                If ddlScenarios3.SelectedIndex > 0 Then
                    For i = 0 To ._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results.SoilResults.Crops.cropName.Count - 1
                        crop = ._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results.SoilResults.Crops.cropName(i)
                        cropInfo = FindCropName(crop)
                        yield3 = ._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results.SoilResults.Crops.cropYield(i)
                        ci3 = ._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results.SoilResults.Crops.cropYieldCI(i)
                        Dim found As Boolean = False
                        If ddlScenarios1.SelectedIndex > 0 Then
                            Dim j As UShort = 0
                            For j = 0 To ._scenariosInfo(ddlScenarios1.SelectedIndex - 1)._results.SoilResults.Crops.cropName.Count - 1
                                If ._scenariosInfo(ddlScenarios1.SelectedIndex - 1)._results.SoilResults.Crops.cropName(j) = ._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results.SoilResults.Crops.cropName(i) Then
                                    found = True
                                    Exit For
                                End If
                            Next
                            If found = False Then
                                yield1 = 0
                                ci1 = 0
                                found = False
                                If ddlScenarios2.SelectedIndex > 0 Then
                                    For j = 0 To ._scenariosInfo(ddlScenarios2.SelectedIndex - 1)._results.SoilResults.Crops.cropName.Count - 1
                                        If ._scenariosInfo(ddlScenarios2.SelectedIndex - 1)._results.SoilResults.Crops.cropName(j) = ._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results.SoilResults.Crops.cropName(i) Then
                                            found = True
                                            Exit For
                                        End If
                                    Next
                                    If found = False Then
                                        yield2 = 0
                                        ci2 = 0
                                        GetScenarioValues("    " & cropInfo(3) & " " & cntDoc.Descendants("Yield").Value & " (" & cropInfo(0) & "/ac)", yield1.ToString("F0"), yield2.ToString("F0"), yield3.ToString("F0"), "Detail", False, ci1.ToString("F2"), ci2.ToString("F2"), ci3.ToString("F2"), .Area)
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            End With

            Return SetResults

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
            Return SetResults
        End Try

    End Function

    Private Function FindCropName(crop)
        Dim cropInfo(3) As String
        For Each item In _crops
            If crop.Trim = item.Code.Trim Then
                cropInfo(0) = item.YieldUnit.Trim  'unit
                cropInfo(1) = item.ConversionFactor * ac_to_ha 'convertion factor
                cropInfo(2) = item.DryMatter ' dry matter
                cropInfo(3) = item.Name 'crop name
                Exit For
            End If
        Next
        Return cropInfo
    End Function

    Private Sub btnSummary_Click(sender As Object, e As System.EventArgs) Handles btnSummary.Click
        btnSummary.CommandArgument = 1
        btnGraphs.CommandArgument = 0
        btnEconomics.CommandArgument = 0
        btnBySoil.CommandArgument = 0
        GetResultsScenarios(ddlScenarios1.Items(ddlScenarios1.SelectedIndex).Value, ddlScenarios2.Items(ddlScenarios2.SelectedIndex).Value, ddlScenarios3.Items(ddlScenarios3.SelectedIndex).Value)
        sctSummary.Style.Item("display") = ""
        fsetSoils.Style.Item("display") = "none"
        sctGraphs.Style.Item("display") = "none"
    End Sub

    Private Sub btnEconomics_Click(sender As Object, e As System.EventArgs) Handles btnEconomics.Click
        btnSummary.CommandArgument = 0
        btnGraphs.CommandArgument = 0
        btnEconomics.CommandArgument = 1
        btnBySoil.CommandArgument = 0
        GetEconomics()
        sctSummary.Style.Item("display") = ""
        fsetSoils.Style.Item("display") = "none"
        sctGraphs.Style.Item("display") = "none"
    End Sub

    Private Sub btnBySoil_Click(sender As Object, e As System.EventArgs) Handles btnBySoil.Click
        btnSummary.CommandArgument = 0
        btnGraphs.CommandArgument = 0
        btnEconomics.CommandArgument = 0
        btnBySoil.CommandArgument = 1
        sctGraphs.Style.Item("display") = "none"
        fsetSoils.Style.Item("display") = ""
        sctSummary.Style.Item("display") = ""
        fsetSoils.Style.Item("display") = ""
        ddlSoils_SelectedIndexChanged()
    End Sub

    Private Sub DisplaySelection()
        Select Case True
            Case sctGraphs.Style.Item("display") = ""
                UpdateCropsList()
                If ddlType.SelectedIndex = 0 Then
                    Charts()
                Else
                    ChartsSubPrj()
                End If
            Case fsetSoils.Style.Item("display") = ""
                GetSoilResults(ddlSoils.SelectedIndex)
            Case sctSummary.Style.Item("display") = ""
                GetResultsScenarios(ddlScenarios1.Items(ddlScenarios1.SelectedIndex).Value, ddlScenarios2.Items(ddlScenarios2.SelectedIndex).Value, ddlScenarios3.Items(ddlScenarios3.SelectedIndex).Value)
        End Select

    End Sub

    Private Sub ddlScenario1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlScenarios1.ServerChange
        DisplaySelection()
    End Sub

    Protected Sub ddlScenario2_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlScenarios2.ServerChange
        If ddlScenarios1.SelectedIndex = 0 Then
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", "You have to select the first scenario to be able to select the second one")
        Else
            DisplaySelection()
        End If

    End Sub

    Protected Sub ddlScenario3_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlScenarios3.ServerChange
        If ddlScenarios1.SelectedIndex = 0 Or ddlScenarios2.SelectedIndex = 0 Then
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", "You have to select the first and second scenarios to be able to select the third one")
        Else
            DisplaySelection()
        End If
    End Sub

    Protected Sub ddlSubproject1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSubproject1.SelectedIndexChanged
        GetResultsSubProjects()
    End Sub
    Private Sub ddlSubproject2_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlSubproject2.SelectedIndexChanged
        GetResultsSubProjects()
    End Sub

    Private Sub ddlSubproject3_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlSubproject3.SelectedIndexChanged
        GetResultsSubProjects()
    End Sub

    Private Sub gvResults_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResults.RowDataBound
        If e.Row.RowIndex >= 0 Then
            DoStyleDataGrid(e, gvResults)
            SelectColumnsToShow(e.Row)
        Else
            If e.Row.RowType = DataControlRowType.Header Then
                SelectColumnsToShow(e.Row)
                If ddlType.SelectedIndex = 0 Then
                    e.Row.Cells(2).Text = ddlScenarios1.Items(ddlScenarios1.SelectedIndex).Text
                    e.Row.Cells(4).Text = ddlScenarios2.Items(ddlScenarios2.SelectedIndex).Text
                    e.Row.Cells(9).Text = ddlScenarios3.Items(ddlScenarios3.SelectedIndex).Text
                Else
                    e.Row.Cells(2).Text = ddlSubproject1.Items(ddlSubproject1.SelectedIndex).Text
                    e.Row.Cells(4).Text = ddlSubproject2.Items(ddlSubproject2.SelectedIndex).Text
                    e.Row.Cells(9).Text = ddlSubproject3.Items(ddlSubproject3.SelectedIndex).Text
                End If
            End If
        End If
    End Sub

    Private Sub DoStyleDataGrid(e As System.Web.UI.WebControls.GridViewRowEventArgs, summaryDataGrid As GridView)
        Dim chkBox As HtmlInputCheckBox = e.Row.FindControl("chkDetail")
        'find out if the difference is less than 0 to change color. 
        If e.Row.RowType = DataControlRowType.DataRow Then
            If summaryDataGrid.DataSource(e.Row.RowIndex).description.ToString.Contains("Yield") Then
                'For crop yield green for >=0 and yellow for < 0.
                If summaryDataGrid.DataSource(e.Row.RowIndex).difference2 < 0 Then
                    e.Row.Cells(6).ForeColor = Drawing.Color.Orange       'difference2            
                    e.Row.Cells(7).ForeColor = Drawing.Color.Orange       'Reduction2
                Else
                    e.Row.Cells(6).ForeColor = Drawing.Color.Green        'difference2            
                    e.Row.Cells(7).ForeColor = Drawing.Color.Green        'Reduction2
                End If
                If summaryDataGrid.DataSource(e.Row.RowIndex).difference3 < 0 Then
                    e.Row.Cells(11).ForeColor = Drawing.Color.Orange       'difference3            
                    e.Row.Cells(12).ForeColor = Drawing.Color.Orange       'Reduction3
                Else
                    e.Row.Cells(11).ForeColor = Drawing.Color.Green        'difference3            
                    e.Row.Cells(12).ForeColor = Drawing.Color.Green        'Reduction3
                End If
                'check total area because the calculation is different. althogh difference can be + total area can be - or viceversa.
                If summaryDataGrid.DataSource(e.Row.RowIndex).totalArea2 < 0 Then
                    e.Row.Cells(8).ForeColor = Drawing.Color.Orange       'Total for Area2
                Else
                    e.Row.Cells(8).ForeColor = Drawing.Color.Green        'Total for Area2
                End If
                If summaryDataGrid.DataSource(e.Row.RowIndex).totalArea3 < 0 Then
                    e.Row.Cells(13).ForeColor = Drawing.Color.Orange       'Total for Area3
                Else
                    e.Row.Cells(13).ForeColor = Drawing.Color.Green        'Total for Area3
                End If
            Else
                'For nutrients, water, and sediment green for <=0 and yellow for >0
                If summaryDataGrid.DataSource(e.Row.RowIndex).difference2 <= 0 Then
                    e.Row.Cells(6).ForeColor = Drawing.Color.Green        'difference2            
                    e.Row.Cells(7).ForeColor = Drawing.Color.Green        'Reduction2
                    e.Row.Cells(8).ForeColor = Drawing.Color.Green        'Total for Area2
                Else
                    e.Row.Cells(6).ForeColor = Drawing.Color.Orange       'difference2            
                    e.Row.Cells(7).ForeColor = Drawing.Color.Orange       'Reduction2
                    e.Row.Cells(8).ForeColor = Drawing.Color.Orange       'Total for Area2
                End If
                If summaryDataGrid.DataSource(e.Row.RowIndex).difference3 <= 0 Then
                    e.Row.Cells(11).ForeColor = Drawing.Color.Green        'difference3            
                    e.Row.Cells(12).ForeColor = Drawing.Color.Green        'Reduction3
                    e.Row.Cells(13).ForeColor = Drawing.Color.Green        'Total for Area3
                Else
                    e.Row.Cells(11).ForeColor = Drawing.Color.Orange       'difference3            
                    e.Row.Cells(12).ForeColor = Drawing.Color.Orange       'Reduction3
                    e.Row.Cells(13).ForeColor = Drawing.Color.Orange       'Total for Area3
                End If
            End If


            e.Row.Cells(14).Style.Item("display") = "none"
            If summaryDataGrid.DataSource(e.Row.RowIndex).sign = True Then
                chkBox.Checked = True
                chkBox.Value = True
            Else
                chkBox.Checked = False
                chkBox.Value = False
            End If

            If Not e.Row Is System.DBNull.Value Then
                Select Case True
                    Case e.Row.DataItem.Description.contains(cntDoc.Descendants("TotalArea").Value)
                        e.Row.Font.Bold = True
                    Case e.Row.DataItem.Description.contains(cntDoc.Descendants("FSArea").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("PPArea").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("PPResArea").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("CBArea").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("RFArea").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("WLArea").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("WWArea").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("SdgArea").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("MainArea").Value)
                        e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
                        If signA = False Then
                            e.Row.Style.Item("display") = "none"
                        Else
                            e.Row.Style.Item("display") = ""
                        End If
                    Case e.Row.DataItem.Description.contains(cntDoc.Descendants("TotalN").Value)
                        e.Row.Font.Bold = True
                    Case e.Row.DataItem.Description.contains(cntDoc.Descendants("OrgN").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("RunoffN").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("SubsurfaceN").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("TileDrainN").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("N2o").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("LeachedN").Value)
                        e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
                        If signN = False Then
                            e.Row.Style.Item("display") = "none"
                        Else
                            e.Row.Style.Item("display") = ""
                        End If
                    Case e.Row.DataItem.Description.contains(cntDoc.Descendants("TotalP").Value)
                        e.Row.Font.Bold = True
                    Case e.Row.DataItem.Description.contains(cntDoc.Descendants("OrgP").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("PO4").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("TileDrainP").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("LeachedP").Value)
                        e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
                        If signP = False Then
                            e.Row.Style.Item("display") = "none"
                        Else
                            e.Row.Style.Item("display") = ""
                        End If
                    Case e.Row.DataItem.Description.contains(cntDoc.Descendants("TotalFlow").Value)
                        e.Row.Font.Bold = True
                    Case e.Row.DataItem.Description.contains(cntDoc.Descendants("SurfaceRunoff").Value) _
                    Or e.Row.DataItem.Description.contains(cntDoc.Descendants("SubsurfaceRunoff").Value) _
                    Or e.Row.DataItem.Description.contains(cntDoc.Descendants("TileDrainFlow").Value)
                        e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
                        If signF = False Then
                            e.Row.Style.Item("display") = "none"
                        Else
                            e.Row.Style.Item("display") = ""
                        End If
                    Case e.Row.DataItem.Description.contains(cntDoc.Descendants("OtherWaterInfo").Value)
                        e.Row.Font.Bold = True
                    Case e.Row.DataItem.Description.contains(cntDoc.Descendants("Irrigation").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("DeepPercolation").Value)
                        e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
                        If signW = False Then
                            e.Row.Style.Item("display") = "none"
                        Else
                            e.Row.Style.Item("display") = ""
                        End If
                    Case e.Row.DataItem.Description.contains(cntDoc.Descendants("TotalSediment").Value)
                        e.Row.Font.Bold = True
                    Case e.Row.DataItem.Description.contains(cntDoc.Descendants("Sediment").Value) _
                        Or e.Row.DataItem.Description.contains(cntDoc.Descendants("ManureErosion").Value)
                        e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
                        If signS = False Then
                            e.Row.Style.Item("display") = "none"
                        Else
                            e.Row.Style.Item("display") = ""
                        End If
                    Case e.Row.DataItem.Description.contains(cntDoc.Descendants("Crop").Value)
                        e.Row.Font.Bold = True
                    Case e.Row.DataItem.Description.contains(cntDoc.Descendants("Yield").Value)
                        e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
                        If signC = False Then
                            e.Row.Style.Item("display") = "none"
                        Else
                            e.Row.Style.Item("display") = ""
                        End If
                End Select
            End If
        End If
    End Sub

    Private Sub ddlSoils_SelectedIndexChanged()
        If ddlSoils.SelectedIndex = 0 Then
            gvResults.DataSource = Nothing
            gvResults.DataBind()
        Else
            GetSoilResults(ddlSoils.SelectedIndex - 1)
        End If

    End Sub

    Private Function ValidateCropName(cropName)
        Dim found As Boolean = False

        For Each cropCrop In ddlCrop.Items
            If cropCrop.Text = cropName Then
                found = True
            End If
        Next

        Return found
    End Function

    Private Sub UpdateCropsList()
        If ddlInfo.SelectedIndex = 10 Then
            ddlCrop.Items.Clear()
            ddlCrop.Style.Item("display") = ""
            'validate if the crop name already exist in the dropdownlist for each scenario selected.
            If ddlScenarios1.SelectedIndex > 0 Then
                For Each cropName In _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios1.SelectedIndex - 1)._results.SoilResults.Crops.cropName
                    If ValidateCropName(cropName) = False Then ddlCrop.Items.Add(cropName)
                Next
            End If

            If ddlScenarios2.SelectedIndex > 0 Then
                For Each cropName In _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios2.SelectedIndex - 1)._results.SoilResults.Crops.cropName
                    If ValidateCropName(cropName) = False Then ddlCrop.Items.Add(cropName)
                Next
            End If

            If ddlScenarios3.SelectedIndex > 0 Then
                For Each cropName In _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios3.SelectedIndex - 1)._results.SoilResults.Crops.cropName
                    If ValidateCropName(cropName) = False Then ddlCrop.Items.Add(cropName)
                Next
            End If
        End If
    End Sub

    Private Sub ddlInfo_ServerChange(sender As Object, e As System.EventArgs) Handles ddlInfo.ServerChange
        ddlCrop.Style.Item("display") = "none"
        UpdateCropsList()
        Charts()
    End Sub

    Private Sub ddlAverage_ServerChange(sender As Object, e As System.EventArgs) Handles ddlAverage.ServerChange
        ddlInfo.Items(9).Text = "---"
        If ddlAverage.SelectedIndex = 1 Then
            ddlInfo.Items.Add("Crop Yield")
            If _startInfo.StateAbrev = "PR" Or _startInfo.StateAbrev = "TX" Or _startInfo.StateAbrev = "OK" Or _startInfo.StateAbrev = "KS" Then
                ddlInfo.Items(9).Text = "N2O"
            End If
        Else
            ddlInfo.Items.RemoveAt(10)
        End If
        Charts()
    End Sub

    Private Sub ddlCrop_ServerChange(sender As Object, e As System.EventArgs) Handles ddlCrop.ServerChange
        Charts()
    End Sub

    Private Function GetCropConvertions()
        Dim listItem As New ListItem
        listItem.Value = 1
        listItem.Text = "t"

        For Each item In _crops
            If ddlCrop.Items(ddlCrop.SelectedIndex).Text.Trim = item.Code.Trim Then
                listItem.Value = (item.ConversionFactor * ac_to_ha) / (item.DryMatter / 100)
                listItem.Text = item.YieldUnit.Trim
                Exit For
            End If
        Next

        Return listItem
    End Function

    Private Sub btnCreatePDF_Click(sender As Object, e As System.EventArgs) Handles btnCreatePDF.Click
        Dim sReturn As String
        Dim sPDFFile As String = String.Empty
        Dim aNTTPDF As New NTTPDFReport

        Try

            sReturn = aNTTPDF.docum(sPDFFile, NTTFilesFolder, Session("userGuide"), ddlScenarios1.SelectedIndex - 1, ddlScenarios2.SelectedIndex - 1, ddlScenarios3.SelectedIndex - 1, _fieldsInfo1, currentFieldNumber, _startInfo)

            If sReturn = "OK" Then
                If File.Exists(sPDFFile) Then
                    Response.Clear()
                    Select Case Path.GetExtension(sPDFFile)
                        Case ".pdf"
                            Response.ContentType = "application/pdf"
                            Response.AddHeader("Content-Disposition", _
                                "attachment;filename=NTT_" + Path.GetFileName(sPDFFile))
                            Response.TransmitFile(sPDFFile)

                        Case Else
                            ' File Extension not supported.
                    End Select
                    Response.End()
                End If
            Else
            End If

        Catch ex As Exception

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