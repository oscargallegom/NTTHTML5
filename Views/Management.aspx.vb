Public Class Management
    Inherits System.Web.UI.Page
    'Private dc As New NTTDBDataContext
    'Controls used in sitemaster page
    Private lblMessage As Label
    Private imgIcon As Image
    Private _crops1 As New List(Of CropsData)
    Private msgs As String = String.Empty
    'Classes definition
    Private _startInfo As New StartInfo
    Private _fieldsInfo1 As New List(Of FieldsData)
    'Private _sitesInfo As New List(Of SiteData)
    Private _crops As New List(Of CropsData)
    Public _croppingSystem As New List(Of CroppingSystem)
    'retrieve the currentfieldnumber
    Private currentFieldNumber As Short = 0
    Private currentScenarioNumber As Short = 0
    Private opError As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("userGuide") = "" Then
            Response.Redirect("~/Default.aspx", False)
            Exit Sub
        End If

        If Not IsPostBack Then
            currentFieldNumber = Session("currentFieldNumber")
            ddlFields.SelectedIndex = currentFieldNumber
        End If

        lblMessage = New Label
        lblMessage = CType(Master.FindControl("lblMessage"), Label)
        imgIcon = New Image
        imgIcon = CType(Master.FindControl("imgIcon"), Image)
        lblMessage.Style.Item("display") = "none"
        imgIcon.Style.Item("display") = "none"
        lblMessage.Text = ""

        ArrangeInfo("Open")
        currentFieldNumber = ddlFields.SelectedIndex
        currentScenarioNumber = ddlScenarios.SelectedIndex

        Select Case True
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("btnSaveMixedCrops")   'Save mixed Crops for crop code 367
                btnSave_Click(True) 'Save BMPs
                SaveOperations() 'Save Operations
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("btnCopyCropping")
                btnSave_Click(False) 'Save BMPs
                If ctlSave.Value = "Yes" Then SaveOperations() 'Save Operations
                btnCopyCropping_Click()
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("btnUploadCroppingSystem")
                btnSave_Click(False) 'Save BMPs
                If ctlSave.Value = "Yes" Then SaveOperations() 'Save Operations
                btnUpload_Click()
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("btnSaveCropRotation") AndAlso ctlSave.Value = "Yes"
                btnSave_Click(False) 'Save BMPs
                SaveOperations() 'Save Operations
                btnSaveCropRotation_Click() 'Save whole rotation to a file
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("btnSaveOperations") AndAlso ctlSave.Value = "Yes"
                SaveOperations() 'Save Operations
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("btnSave")
                btnSave_Click(True) 'Save BMPs
                SaveOperations() 'Save Operations
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("btnContinue")
                btnContinue_Click() 'Save BMPs
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("btnAddNewScenario")
                btnSave_Click(False) 'Save BMPs
                SaveOperations() 'Save Operations
                If lblMessage.Text = String.Empty Or lblMessage.Text = "" Then
                    btnAddNewScenario_Click()
                End If
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("btnRenameScenario")
                btnSave_Click(False) 'Save BMPs
                If ctlSave.Value = "Yes" Then SaveOperations() 'Save Operations
                btnRenameScenario_Click()
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("btnDeleteScenario")
                btnSave_Click(False) 'Save BMPs
                If ctlSave.Value = "Yes" Then SaveOperations() 'Save Operations
                btnDeleteScenario_Click()
                lblMessage.Text = ""
                imgIcon.Style.Item("display") = "none"
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("ddlScenario")
                currentScenarioNumber = hdnScenario.Value
                btnSave_Click(False) 'Save BMPs
                SaveOperations() 'Save Operations
                If lblMessage.Text = String.Empty Or lblMessage.Text = "" Then
                    ddlScenarios_SelectedIndexChanged()
                Else
                    ddlScenarios.SelectedIndex = currentScenarioNumber
                End If
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("ddlFields")
                currentFieldNumber = hdnField.Value
                If ddlScenarios.Items.Count > 0 Then ddlScenarios.SelectedIndex = 0 Else ddlScenarios.SelectedIndex = -1
                btnSave_Click(False) 'Save BMPs
                SaveOperations() 'Save Operations
                If lblMessage.Text = String.Empty Or lblMessage.Text = "" Then
                    ddlFields_SelectedIndexChanged()
                Else
                    ddlFields.SelectedIndex = currentFieldNumber
                End If
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("uploadCropRotation")
                btnSave_Click(False) 'Save BMPs
                btnUpload_Click1()
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("btnAddNewOperation")
                btnSave_Click(False) 'Save BMPs
                If ctlSave.Value = "Yes" Then SaveOperations() 'Save Operations
                btnAddOperation_ServerClick()
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("btnDeleteOperation")
                btnSave_Click(False) 'Save BMPs
                SaveOperations() 'Save Operations
        End Select

        openXMLLanguagesFile()

        If Not IsPostBack Then
            tvMain.Focus()
            currentFieldNumber = Session("currentFieldNumber")
            ddlFields.SelectedIndex = currentFieldNumber
            currentScenarioNumber = Session("currentScenarioNumber")
            ddlScenarios.SelectedIndex = currentScenarioNumber
            LoadCroppingSystem()
            LoadFields(ddlFields, _fieldsInfo1, currentFieldNumber)
            LoadFields(ddlFromField, _fieldsInfo1, currentFieldNumber)
            LoadScenarios(ddlScenarios, _fieldsInfo1(currentFieldNumber)._scenariosInfo, currentScenarioNumber)
            LoadScenarios(ddlFromScenario, _fieldsInfo1(currentFieldNumber)._scenariosInfo, currentScenarioNumber)
            LoadAllScenarios(ddlAllScenario, _fieldsInfo1, currentScenarioNumber)
            LoadOper()
            If Session("crops") Is Nothing Then LoadCrops(_crops, _startInfo.StateAbrev)
            LoadAnimalUnits()
            LoadOpTypes()
            LoadFerts()
            LoadDays()
            ddlDays.DataTextField = "name"
            ddlDays.DataValueField = "code"
            ddlDays.DataSource = _days
            ddlDays.DataBind()
            LoadMonths()
            ddlMonths.DataTextField = "code"
            ddlMonths.DataValueField = "code"
            ddlMonths.DataSource = _months
            ddlMonths.DataBind()
            LoadYears(_startInfo)
            ddlYears.DataTextField = "name"
            ddlYears.DataValueField = "code"
            ddlYears.DataSource = _years
            ddlYears.DataBind()
            ExpandTreeView(999)
            ddlFields_SelectedIndexChanged()
            InitializeDDLs()
        Else
            currentFieldNumber = ddlFields.SelectedIndex
            currentScenarioNumber = ddlScenarios.SelectedIndex
        End If
        LoadBMPs()      'bmps need to be load everytime to be able to update them in the screen
        If _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count <= 0 Then
            tvMain.Style.Item("display") = "none"
            tdCurrentScenario.Style.Item("display") = "none"
        Else
            tvMain.Style.Item("display") = ""
            tdCurrentScenario.Style.Item("display") = ""
        End If
        If Page.Request.Params("__EVENTARGUMENT") = english Or Page.Request.Params("__EVENTARGUMENT") = spanish Then
            'change crops, operations, and types if the language was changed in this page.
            Session("Language") = Page.Request.Params("__EVENTARGUMENT")
            ChangeOperationsLanguage()
            Session("ManagementLanguage") = Page.Request.Params("__EVENTARGUMENT")
            InitializeDDLs()
        Else
            'change crops, operations, and types if the language was changed in any other page page.
            If Session("ManagementLanguage") <> Session("Language") Then
                ChangeOperationsLanguage()
                Session("ManagementLanguage") = Session("Language")
                InitializeDDLs()
            End If
        End If
        ChangeLanguageContent()
        'LoadMixedCropsTable()
    End Sub

    Private Sub btnSaveMixedCrops_Click(operation As OperationsData)
        Try
            'mixed crops text boxes
            Dim chkSelected As HtmlInputCheckBox
            Dim lblCodeCrop As HtmlInputText
            Dim lblNameCrop As HtmlInputText
            Dim txtPercentage As HtmlInputText
            Dim txtPP As HtmlInputText
            Dim coma = ""

            For i = 0 To gvCrops.Rows.Count - 1
                chkSelected = gvCrops.Rows(i).Cells(0).FindControl("chkSelect")
                If chkSelected.Checked Then
                    lblCodeCrop = DirectCast(gvCrops.Rows(i).FindControl("lblCode"), HtmlInputText)
                    lblNameCrop = DirectCast(gvCrops.Rows(i).FindControl("lblName"), HtmlInputText)
                    txtPercentage = DirectCast(gvCrops.Rows(i).FindControl("txtPercentage"), HtmlInputText)
                    txtPP = DirectCast(gvCrops.Rows(i).FindControl("txtPP"), HtmlInputText)
                    operation.MixedCropData &= coma & lblCodeCrop.Value & "|" & txtPercentage.Value & "|" & txtPP.Value
                    coma = ","
                End If
            Next
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", ex.Message)
        End Try

    End Sub

    Private Sub LoadMixedCropsTable()
        gvCrops.DataSource = Nothing
        gvCrops.DataBind()
        Dim _mixedCropsInfo As New List(Of MixedCropsData)
        Dim mixedCrop As MixedCropsData

        For Each crop In _bufferCrops
            mixedCrop = New MixedCropsData
            mixedCrop.Code = crop.Number
            mixedCrop.Name = crop.Name
            mixedCrop.PP = crop.PlantPopulation
            mixedCrop.PPDefault = crop.PlantPopulation
            mixedCrop.Percentage = 0
            mixedCrop.Selected = False
            _mixedCropsInfo.Add(mixedCrop)
        Next

        gvCrops.DataSource = _mixedCropsInfo
        gvCrops.DataBind()
    End Sub

    Private Sub ChangeOperationsLanguage()
        'load first the ddl with the new language selected
        LoadOper()
        LoadOpTypes()
        LoadCrops(_crops, _startInfo.StateAbrev)
        'change the crop, operation, and type to the new language selected.
        Dim cropAnt As String = String.Empty
        Dim cropCurrent As String = String.Empty
        Dim operAnt As String = String.Empty
        Dim operCurrent As String = String.Empty
        Dim typeAnt As String = String.Empty
        Dim typeCurrent As String = String.Empty

        For Each _field In _fieldsInfo1
            For Each _scenario In _field._scenariosInfo
                For Each _oper In _scenario._operationsInfo
                    'change crop language
                    If cropAnt <> _oper.ApexCropName Then
                        cropAnt = _oper.ApexCropName
                        For Each _crop In _crops
                            If _crop.Number = _oper.ApexCrop Then
                                cropCurrent = _crop.Name
                                Exit For
                            End If
                        Next
                    End If
                    _oper.ApexCropName = cropCurrent

                    'change oper language
                    If operAnt <> _oper.ApexOpName Then
                        operAnt = _oper.ApexOpName
                        For Each _operation In _operations
                            If _operation.Abbreviation.Trim = _oper.ApexOpAbbreviation.Trim Then
                                operCurrent = _operation.Name
                                Exit For
                            End If
                        Next
                    End If
                    _oper.ApexOpName = operCurrent.Trim

                    'change type/tillage language
                    If typeAnt <> _oper.ApexTillName Then
                        typeAnt = _oper.ApexTillName
                        For Each _type In _opTypes
                            If _type.Number.Split(",")(0).Trim = _oper.ApexTillCode Then
                                typeCurrent = _type.Name
                                Exit For
                            End If
                        Next
                    End If
                    _oper.ApexTillName = typeCurrent.Trim
                Next
            Next
        Next
    End Sub

    Private Sub ExpandTreeView(bmpNode As UShort)
        If bmpNode = 999 Then        'initial status fo tree view
            tvMain.Nodes(0).ExpandAll()
            tvMain.Nodes(1).CollapseAll()
        Else
            tvMain.Nodes(1).Expand() : tvMain.Nodes(1).ChildNodes(bmpNode).Expanded = True
            'hide pads and pipes if state is different than MO and MS
            '******* NOTE: Did not do this yet because when delete a childnode the sequence of the tree change so the nodes down change or crash. Fixed.
            If _startInfo.StateAbrev <> "MO" And _startInfo.StateAbrev <> "MS" Then
                With tvMain.Nodes(1).ChildNodes(1)
                    If .ChildNodes.Count > 1 Then
                        .ChildNodes.RemoveAt(4)
                        .ChildNodes.RemoveAt(3)
                        .ChildNodes.RemoveAt(2)
                        .ChildNodes.RemoveAt(1)
                    End If
                End With
            End If
            'hide roads BMPs for those with forestry fields. It is not working because the tvMain is not load again in post back.
            'If Not _fieldsInfo1(currentFieldNumber).Forestry Then If tvMain.Nodes(1).ChildNodes.Count > 6 Then tvMain.Nodes(1).ChildNodes.RemoveAt(6)
        End If
    End Sub

    Private Sub LoadCroppingSystem()
        Dim cropSysAnt As String = String.Empty : Dim tillAnt As String = String.Empty  'will help to detect duplicates cropping system
        If _croppingSystem.Count <= 0 Then
            Dim cs = GetCropMatrix(_startInfo.StateAbrev)
            _croppingSystem.Clear()
            If Not cs.HasRows Then
                cs = GetCropMatrix("99")
            End If
            For Each c In cs
                If c.item("CMCrop").Trim <> cropSysAnt.Trim Or c.item("CMTillage").Trim <> tillAnt.Trim Then _croppingSystem.Add(New CroppingSystem With {.Name = c.item("CMCrop"), .Code = c.item("CMVar12"), .Till = c.item("CMTillage")})
                cropSysAnt = c.item("CMCrop")
                tillAnt = c.item("CMTillage")
            Next
        End If

        If ddlCroppingSystemsAll.Items.Count <= 0 Then
            ddlCroppingSystemsAll.DataTextField = "name"
            ddlCroppingSystemsAll.DataValueField = "code"
            ddlCroppingSystemsAll.DataSource = _croppingSystem
            ddlCroppingSystemsAll.DataBind()
            ddlCroppingSystemsAll.SelectedIndex = 0
            ddlTillageAll.DataTextField = "till"
            ddlTillageAll.DataValueField = "name"
            ddlTillageAll.DataSource = _croppingSystem
            ddlTillageAll.DataBind()
            ddlTillageAll.SelectedIndex = 0
        End If
    End Sub

    Private Sub LoadOpTypes()
        'Dim tills = (From OpType In dc.item("OpTypes Where OpType.Status = True Order By OpType.Name Select OpType)
        Dim tills = GetOpTypes()
        Dim codeName As String = String.Empty
        Dim codeAnt As UShort = 0
        Dim c_k As Single = 0
        'clear list
        _opTypes.Clear()

        For Each c In tills
            Select Case Session("Language")
                Case english
                    codeName = c.item("EQP").Trim
                Case spanish
                    codeName = c.item("SpanishName")
                Case portuguese
                    codeName = c.item("PortugueseName")
            End Select
            If c.item("K") Is DBNull.Value Then c_k = 0 Else c_k = c.item("K")
            If Not c.item("Abbrevation") Is DBNull.Value AndAlso c.item("Code") <> codeAnt Then _opTypes.Add(New AnimalUnitsData With {.Number = c.item("Code") & "," & c.item("Abbrevation") & "|" & c.item("NO3N") & "|" & c.item("OrgN") & "|" & c.item("OrgP") & "|" & c.item("PO4P") & "|" & c_k & "|" & c.item("NH3"), .DndcCode = c.item("DNDC_code"), .Name = codeName, _
                                                    .Abbreviation = c.item("Abbrevation"), .ConversionUnit = c.item("ConversionUnit"), .DryManure = c.item("DryManure"), .No3 = c.item("NO3N"), .Po4 = c.item("PO4P"), .OrgN = c.item("OrgN"), .OrgP = c.item("OrgP"), .Nh3 = c.item("NH3"), .K = c_k})
            codeAnt = c.item("Code")
        Next
        _opTypes.Sort(New sortByCodeName())  'sort by name all of the codes before adding operations that should be as they are in the database.
        _opTypes.Insert(0, New AnimalUnitsData With {.Number = 0 & ",*" & "|" & 0 & "|" & 0 & "|" & 0 & "|" & 0 & "|" & 0, .Name = selectOne, .Abbreviation = "*"})

        If ddlTypes Is Nothing Then Exit Sub
        ddlTypes.DataValueField = "number"
        ddlTypes.DataTextField = "name"
        ddlTypes.DataSource = _opTypes
        ddlTypes.DataBind()
    End Sub

    Private Sub LoadFerts()
        Dim ferts = GetFerts()
        Dim codeName As String = String.Empty
        Dim codeAnt As UShort = 0
        'clear list
        _ferts.Clear()

        For Each c In ferts
            If c.item("Code") <> codeAnt Then _ferts.Add(New AnimalUnitsData With {.Number = c.item("Code"), .Name = c.item("Name").Trim, .No3 = c.item("qn"), .Po4 = c.item("qp"), .OrgN = c.item("yn"), .OrgP = c.item("yp"), .Nh3 = c.item("nh3"), .K = c.item("k")})
            codeAnt = c.item("Code")
        Next
        _ferts.Sort(New sortByCodeName())  'sort by name all of the codes before adding operations that should be as they are in the database.

    End Sub

    Private Sub LoadOper()
        Dim opers = GetOperations()
        Dim itemName As String = String.Empty
        _operations.Clear()

        _operations.Add(New CodeAndName With {.Code = 0, .Name = "Select One", .Abbreviation = "Opers"})
        For Each c In opers
            Select Case Session("Language")
                Case english
                    itemName = c.item("Name")
                Case spanish
                    itemName = c.item("NameSpanish")
                Case portuguese
                    itemName = c.item("NamePortuguese")
            End Select
            _operations.Add(New CodeAndName With {.Code = c.item("Code"), .Name = itemName, .Abbreviation = c.item("Abbreviation")})
        Next
        If ddlOpers Is Nothing Then Exit Sub
        ddlOpers.DataValueField = "code"
        ddlOpers.DataTextField = "name"
        ddlOpers.DataSource = _operations
        ddlOpers.DataBind()
    End Sub

    Public Sub LoadCrops(ByRef _crops As List(Of CropsData), stateAbrev As String)
        Dim crops = GetCrops(stateAbrev, 0)
        Dim cropAnt As UShort = 0
        'clear crops
        _crops.Clear()
        _bufferCrops.Clear()

        If Not crops.HasRows Then
            crops = GetCrops(stateAbrev, 1)
        End If

        If _startInfo.StateAbrev = "User" Then
            crops = GetCrops(stateAbrev, 2)
        End If
        Dim cropName As String = String.Empty
        For Each c In crops
            Select Case Session("Language")
                Case english
                    cropName = c.item("CropName").Trim
                Case spanish
                    cropName = c.item("cropNameSpanish").Trim()
                Case portuguese
                    cropName = c.item("cropNamePortuguese").Trim()
            End Select
            If c.item("CropNumber") <> cropAnt Then
                _crops.Add(New CropsData With {.Number = c.item("CropNumber"), .dndc_Code = c.item("DNDC_Code"), .Name = cropName, .Code = c.item("CropCode").Trim, .LuNumber = c.item("LUNumber"), .HarvestCode = c.item("HarvestCode"), .PlantPopulation = c.item("PlantPopulationAcres"), .CN_A = c.item("A"), .CN_B = c.item("B"), .CN_C = c.item("C"), .CN_D = c.item("D"), .FilterStrip = c.item("FilterStrip"), .Itil = c.item("itil"), .To1 = c.item("to1"), .Tb = c.item("tb"), .Dd = c.item("dd"), .Daym = c.item("daym"), _
                                              .YieldUnit = c.item("YieldUnit"), .ConversionFactor = c.item("ConversionFactor"), .DryMatter = c.item("DryMatter"), .additional = False, .state = c.item("StateAbrev"), .heat_unit = c.item("HeatUnits")})
                cropAnt = c.item("CropNumber")
            End If
        Next

        'crops = (From APEXCrop In dc.item("APEXCROPs Order By APEXCrop.CropName Where (APEXCrop.StateAbrev = "**" And APEXCrop.FilterStrip Like "*FS*") Or APEXCrop.CropNumber = cropRoad Or (APEXCrop.StateAbrev = "**" And APEXCrop.LUNumber = 28) Select APEXCrop)  'select all of the grass for buffers regardless of the state. 
        crops = GetCrops(stateAbrev, 3) 'select all of the grass for buffers regardless of the state. 

        For Each c In crops
            Select Case Session("Language")
                Case english
                    cropName = c.item("CropName").Trim
                Case spanish
                    cropName = c.item("cropNameSpanish").Trim()
                Case portuguese
                    cropName = c.item("cropNamePortuguese").Trim()
            End Select

            If c.item("CropNumber") <> cropAnt Then
                _crops.Add(New CropsData With {.Number = c.item("CropNumber"), .dndc_Code = c.item("DNDC_Code"), .Name = cropName, .Code = c.item("CropCode").Trim, .LuNumber = c.item("LUNumber"), .HarvestCode = c.item("HarvestCode"), .PlantPopulation = c.item("PlantPopulationAcres"), .CN_A = c.item("A"), .CN_B = c.item("B"), .CN_C = c.item("C"), .CN_D = c.item("D"), .FilterStrip = c.item("FilterStrip"), .Itil = c.item("itil"), .To1 = c.item("to1"), .Tb = c.item("tb"), .Dd = c.item("dd"), .Daym = c.item("daym"), _
                                              .YieldUnit = c.item("YieldUnit"), .ConversionFactor = c.item("ConversionFactor"), .DryMatter = c.item("DryMatter"), .additional = True, .state = c.item("StateAbrev"), .heat_unit = c.item("HeatUnits")})
                If Not c.item("FilterStrip") Is Nothing AndAlso c.item("FilterStrip").Contains("FS") Then _bufferCrops.Add(New CropsData With {.Number = c.item("CropNumber"), .dndc_Code = c.item("DNDC_Code"), .Name = cropName, .Code = c.item("CropCode").Trim, .LuNumber = c.item("LUNumber"), .HarvestCode = c.item("HarvestCode"), .PlantPopulation = c.item("PlantPopulationAcres"), .CN_A = c.item("A"), .CN_B = c.item("B"), .CN_C = c.item("C"), .CN_D = c.item("D"), .FilterStrip = c.item("FilterStrip"), .Itil = c.item("itil"), .To1 = c.item("to1"), .Tb = c.item("tb"), .Dd = c.item("dd"), .Daym = c.item("daym"), _
                                                  .YieldUnit = c.item("YieldUnit"), .ConversionFactor = c.item("ConversionFactor"), .DryMatter = c.item("DryMatter"), .additional = True, .state = c.item("StateAbrev"), .heat_unit = c.item("HeatUnits")})
                cropAnt = c.item("CropNumber")
            End If
        Next

        'crops = (From APEXCrop In dc.item("APEXCROPs Order By APEXCrop.CropName Where (APEXCrop.StateAbrev = "**" And APEXCrop.FilterStrip Like "*FS*") Or APEXCrop.CropNumber = cropRoad Or (APEXCrop.StateAbrev = stateAbrev) Select APEXCrop)  'select all of the crops for the specific state
        crops = GetCrops(stateAbrev, 4)  'select all of the crops for the specific state

        For Each c In crops
            Select Case Session("Language")
                Case english
                    cropName = c.item("CropName").Trim
                Case spanish
                    cropName = c.item("cropNameSpanish").Trim()
                Case portuguese
                    cropName = c.item("cropNamePortuguese").Trim()
            End Select

            If c.item("CropNumber") <> cropAnt Then
                _crops.Add(New CropsData With {.Number = c.item("CropNumber"), .dndc_Code = c.item("DNDC_Code"), .Name = cropName, .Code = c.item("CropCode").Trim, .LuNumber = c.item("LUNumber"), .HarvestCode = c.item("HarvestCode"), .PlantPopulation = c.item("PlantPopulationAcres"), .CN_A = c.item("A"), .CN_B = c.item("B"), .CN_C = c.item("C"), .CN_D = c.item("D"), .FilterStrip = c.item("FilterStrip"), .Itil = c.item("itil"), .To1 = c.item("to1"), .Tb = c.item("tb"), .Dd = c.item("dd"), .Daym = c.item("daym"), _
                                              .YieldUnit = c.item("YieldUnit"), .ConversionFactor = c.item("ConversionFactor"), .DryMatter = c.item("DryMatter"), .additional = True, .state = c.item("StateAbrev"), .heat_unit = c.item("HeatUnits")})
                cropAnt = c.item("CropNumber")
            End If
        Next

        crops = GetCrops(stateAbrev, 9)  'get all of the crop and compare with a list of state additional crops wanted in the list (crop_state_crop view)

        For Each c In crops
            Select Case Session("Language")
                Case english
                    cropName = c.item("CropName").Trim
                Case spanish
                    cropName = c.item("cropNameSpanish").Trim()
                Case portuguese
                    cropName = c.item("cropNamePortuguese").Trim()
            End Select

            If c.item("CropNumber") <> cropAnt Then
                _crops.Add(New CropsData With {.Number = c.item("CropNumber"), .dndc_Code = c.item("DNDC_Code"), .Name = cropName, .Code = c.item("CropCode").Trim, .LuNumber = c.item("LUNumber"), .HarvestCode = c.item("HarvestCode"), .PlantPopulation = c.item("PlantPopulationAcres"), .CN_A = c.item("A"), .CN_B = c.item("B"), .CN_C = c.item("C"), .CN_D = c.item("D"), .FilterStrip = c.item("FilterStrip"), .Itil = c.item("itil"), .To1 = c.item("to1"), .Tb = c.item("tb"), .Dd = c.item("dd"), .Daym = c.item("daym"), _
                                              .YieldUnit = c.item("YieldUnit"), .ConversionFactor = c.item("ConversionFactor"), .DryMatter = c.item("DryMatter"), .additional = True, .state = c.item("state_abbrev"), .heat_unit = c.item("HeatUnits")})
                cropAnt = c.item("CropNumber")
            End If
        Next

        _bufferCrops.Sort(New sortByName())
        _bufferCrops.Insert(0, New CropsData With {.Name = selectOne, .Number = 0})
        _crops.Sort(New sortByName())
        For i = _crops.Count - 1 To 0 Step -1
            If _crops(i).Number = cropAnt Or _crops(i).Number = 367 Then  '367 is Mixed pasture. is out for now until we text it very well
                If _crops(i).additional = False Then _crops(i + 1).additional = False
                If _crops(i).state <> "**" Then
                    _crops.RemoveAt(i + 1)
                Else
                    _crops.RemoveAt(i)
                End If
            Else
                cropAnt = _crops(i).Number
            End If
        Next
        For i = _bufferCrops.Count - 1 To 0 Step -1
            If _bufferCrops(i).Number = cropAnt Or _crops(i).Number = 367 Then  '367 is Mixed pasture. is out for now until we text it very well
                _bufferCrops.RemoveAt(i)
            Else
                cropAnt = _bufferCrops(i).Number
            End If
        Next
        Session("crops") = _crops
    End Sub

    Private Sub ChangeLanguageContent()
        tvMain.Nodes.Item(0).Text = cntDoc.Descendants("BMPCroppingSystemHeading").Value
        tvMain.Nodes.Item(0).ChildNodes.Item(0).Text = cntDoc.Descendants("SelectCroppingSystemHeading").Value
        tvMain.Nodes.Item(0).ChildNodes.Item(1).Text = cntDoc.Descendants("UploadSavedCropRotation").Value
        tvMain.Nodes.Item(0).ChildNodes.Item(2).Text = cntDoc.Descendants("CopyCropRotation").Value
        tvMain.Nodes.Item(0).ChildNodes.Item(3).Text = cntDoc.Descendants("SaveCurrentCropRotation").Value
        tvMain.Nodes.Item(0).ChildNodes.Item(4).Text = cntDoc.Descendants("ManagementOperation").Value
        tvMain.Nodes.Item(1).Text = cntDoc.Descendants("BestManagementPracticesHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(0).Text = cntDoc.Descendants("IrrigacionFertigationHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(0).ChildNodes.Item(0).Text = cntDoc.Descendants("AutoIrrigationHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(0).ChildNodes.Item(1).Text = cntDoc.Descendants("AutoFertigationHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(1).Text = cntDoc.Descendants("DrainageHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(1).ChildNodes.Item(0).Text = cntDoc.Descendants("TileDrainHeading").Value
        If tvMain.Nodes.Item(1).ChildNodes.Item(1).ChildNodes.Count > 1 Then
            tvMain.Nodes.Item(1).ChildNodes.Item(1).ChildNodes.Item(1).Text = cntDoc.Descendants("PPNoImprovementHeading").Value
            tvMain.Nodes.Item(1).ChildNodes.Item(1).ChildNodes.Item(2).Text = cntDoc.Descendants("PPTwoStageDitchHeading").Value
            tvMain.Nodes.Item(1).ChildNodes.Item(1).ChildNodes.Item(3).Text = cntDoc.Descendants("PPDitchReservoirHeading").Value
            tvMain.Nodes.Item(1).ChildNodes.Item(1).ChildNodes.Item(4).Text = cntDoc.Descendants("PPTailwaterHeading").Value
        End If
        tvMain.Nodes.Item(1).ChildNodes.Item(2).Text = cntDoc.Descendants("WetlandsPondsHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(2).ChildNodes.Item(0).Text = cntDoc.Descendants("WetlandsHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(2).ChildNodes.Item(1).Text = cntDoc.Descendants("PondsHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(3).Text = cntDoc.Descendants("StreamAndRiparianHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(3).ChildNodes.Item(0).Text = cntDoc.Descendants("StreamFencingHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(3).ChildNodes.Item(1).Text = cntDoc.Descendants("StreambankStabilizationHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(3).ChildNodes.Item(2).Text = cntDoc.Descendants("RiparianForestHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(3).ChildNodes.Item(3).Text = cntDoc.Descendants("FilterStripHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(3).ChildNodes.Item(4).Text = cntDoc.Descendants("WaterwayHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(4).Text = cntDoc.Descendants("ContourBufferHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(4).ChildNodes.Item(0).Text = cntDoc.Descendants("ContourBufferHeading2").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(5).Text = cntDoc.Descendants("LandGradingManagementHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(5).ChildNodes.Item(0).Text = cntDoc.Descendants("LandLevelingHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(5).ChildNodes.Item(1).Text = cntDoc.Descendants("TerraceSystemHeading").Value
        tvMain.Nodes.Item(1).ChildNodes.Item(5).ChildNodes.Item(2).Text = cntDoc.Descendants("ManureControlHeading").Value
        'tvMain.Nodes.Item(1).ChildNodes.Item(6).Text = cntDoc.Descendants("ClimateChange").Value
        'tvMain.Nodes.Item(1).ChildNodes.Item(6).ChildNodes.Item(0).Text = cntDoc.Descendants("TempAndPcpChanges").Value
        If tvMain.Nodes.Item(1).ChildNodes.Count > 6 Then
            tvMain.Nodes.Item(1).ChildNodes.Item(6).Text = cntDoc.Descendants("Roads").Value
            tvMain.Nodes.Item(1).ChildNodes.Item(6).ChildNodes.Item(0).Text = cntDoc.Descendants("AsfaltOrConcrete").Value
            tvMain.Nodes.Item(1).ChildNodes.Item(6).ChildNodes.Item(1).Text = cntDoc.Descendants("GrassCover").Value
            tvMain.Nodes.Item(1).ChildNodes.Item(6).ChildNodes.Item(2).Text = cntDoc.Descendants("SlopeAdjustment").Value
            tvMain.Nodes.Item(1).ChildNodes.Item(6).ChildNodes.Item(3).Text = cntDoc.Descendants("Shading").Value
        End If

        btnSave.Text = cntDoc.Descendants("Save").Value
        btnContinue.Text = cntDoc.Descendants("Continue").Value
        lblFields.Value = cntDoc.Descendants("SelectFieldHeading").Value
        lblScenarios.Value = cntDoc.Descendants("SelectScenarioHeading").Value
        btnDeleteScenario.Text = cntDoc.Descendants("DeleteScenario").Value
        btnAddNewScenario.Text = cntDoc.Descendants("AddNewScenario").Value
        btnUpload.Text = cntDoc.Descendants("SelectScenarioHeading").Value
        lblTillage.Value = cntDoc.Descendants("SelectTillageHeading").Value
        lblCroppingSystem.InnerText = cntDoc.Descendants("SelectCroppingSystemHeading").Value
        lblSelectCroppingSystem.Value = cntDoc.Descendants("SelectCroppingSystemHeading").Value
        btnUpload.Text = cntDoc.Descendants("Upload").Value
        btnAIClear.Text = cntDoc.Descendants("Clear").Value
        btnAFClear.Text = cntDoc.Descendants("Clear").Value
        btnTDClear.Text = cntDoc.Descendants("Clear").Value
        btnPPDEClear.Text = cntDoc.Descendants("Clear").Value
        btnPPDSClear.Text = cntDoc.Descendants("Clear").Value
        btnPPDSClear.Text = cntDoc.Descendants("Clear").Value
        btnPPTWClear.Text = cntDoc.Descendants("Clear").Value
        btnWLClear.Text = cntDoc.Descendants("Clear").Value
        btnPNDClear.Text = cntDoc.Descendants("Clear").Value
        btnSFClear.Text = cntDoc.Descendants("Clear").Value
        btnSBSClear.Text = cntDoc.Descendants("Clear").Value
        btnRFClear.Text = cntDoc.Descendants("Clear").Value
        btnFSClear.Text = cntDoc.Descendants("Clear").Value
        btnWWClear.Text = cntDoc.Descendants("Clear").Value
        btnCBClear.Text = cntDoc.Descendants("Clear").Value
        btnLLClear.Text = cntDoc.Descendants("Clear").Value
        btnTSClear.Text = cntDoc.Descendants("Clear").Value
        btnLMClear.Text = cntDoc.Descendants("Clear").Value
        btnAoCClear.Text = cntDoc.Descendants("Clear").Value
        btnGCClear.Text = cntDoc.Descendants("Clear").Value
        btnSAClear.Text = cntDoc.Descendants("Clear").Value
        btnSdgClear.Text = cntDoc.Descendants("Clear").Value
        btnRenameScenario.Text = cntDoc.Descendants("RenameScenario").Value
        lblLoadCropping.InnerHtml = cntDoc.Descendants("UploadSavedCropRotation").Value
        lblCopyCropping.InnerHtml = cntDoc.Descendants("CopyCropRotation").Value
        lblFromField.Value = cntDoc.Descendants("FromField").Value
        lblFromScenario.Value = cntDoc.Descendants("FromScenario").Value
        btnCopyCropping.Text = cntDoc.Descendants("Copy").Value
        lblOperation.InnerHtml = cntDoc.Descendants("ManagementOperation").Value
        btnAddOperation.Value = cntDoc.Descendants("AddNewOperation").Value
        btnDeleteOperation.Value = cntDoc.Descendants("DeleteOperationsSelected").Value
        btnUploader.Value = cntDoc.Descendants("UploadLabel").Value
        lblPNDFraction.Text = cntDoc.Descendants("PondsAreaHeading").Value

        aAIHide.InnerText = cntDoc.Descendants("Hide").Value
        aAFHide.InnerText = cntDoc.Descendants("Hide").Value
        aTDHide.InnerText = cntDoc.Descendants("Hide").Value
        aPPNDHide.InnerText = cntDoc.Descendants("Hide").Value
        aPPDSHide.InnerText = cntDoc.Descendants("Hide").Value
        aPPDEHide.InnerText = cntDoc.Descendants("Hide").Value
        aPPTWHide.InnerText = cntDoc.Descendants("Hide").Value
        aWLHide.InnerText = cntDoc.Descendants("Hide").Value
        aPNDHide.InnerText = cntDoc.Descendants("Hide").Value
        aSFHide.InnerText = cntDoc.Descendants("Hide").Value
        aSBSHide.InnerText = cntDoc.Descendants("Hide").Value
        aRFHide.InnerText = cntDoc.Descendants("Hide").Value
        aFSHide.InnerText = cntDoc.Descendants("Hide").Value
        aWWHide.InnerText = cntDoc.Descendants("Hide").Value
        aCBHide.InnerText = cntDoc.Descendants("Hide").Value
        aLLHide.InnerText = cntDoc.Descendants("Hide").Value
        aTSHide.InnerText = cntDoc.Descendants("Hide").Value
        aLMHide.InnerText = cntDoc.Descendants("Hide").Value
        aAoCHide.InnerText = cntDoc.Descendants("Hide").Value
        aGCHide.InnerText = cntDoc.Descendants("Hide").Value
        aSAHide.InnerText = cntDoc.Descendants("Hide").Value
        aSdgHide.InnerText = cntDoc.Descendants("Hide").Value
        aOpReadHide.InnerText = cntDoc.Descendants("Hide").Value
        If aTvMain.InnerText = "Hide" Or aTvMain.InnerText = "Ocultar" Then aTvMain.InnerText = cntDoc.Descendants("Hide").Value
        If aTvMain.InnerText = "Show" Or aTvMain.InnerText = "Mostrar" Then aTvMain.InnerText = cntDoc.Descendants("Show").Value

        'BMPs parameters
        'autoIrrigation
        lblAIIrrigationType.Text = cntDoc.Descendants("IrrigationTypeHeading").Value
        lblAIWaterStress.Text = cntDoc.Descendants("WaterStressHeading").Value
        lblAIEfficiancy.Text = cntDoc.Descendants("IrrigationEfficiencyHeading").Value
        lblAIFrequency.Text = cntDoc.Descendants("FrequencyHeading").Value
        lblAIMaxSingleAppl.Text = cntDoc.Descendants("MaxApplicationHeading").Value
        'autofertigation
        lblAFIrrigationType.Text = cntDoc.Descendants("IrrigationTypeHeading").Value
        lblAFWaterStress.Text = cntDoc.Descendants("WaterStressHeading").Value
        lblAFEfficiancy.Text = cntDoc.Descendants("IrrigationEfficiencyHeading").Value
        lblAFFrequency.Text = cntDoc.Descendants("FrequencyHeading").Value
        lblAFMaxSingleAppl.Text = cntDoc.Descendants("MaxApplicationHeading").Value
        lblAFNConc.Text = cntDoc.Descendants("ApplicationRate").Value
        'Tile Dranage
        lblTDDepth.Text = cntDoc.Descendants("TileDrainDepthHeading").Value
        'Filter Strip
        lblFSArea.Text = cntDoc.Descendants("AreaHeading").Value & " *opt)"
        lblFSRatio.Text = cntDoc.Descendants("RiparianBufferSlopeHeading").Value
        'fencing
        lblSF.InnerHtml = cntDoc.Descendants("StreamFencingHeading").Value
        lblSFAimals.Text = cntDoc.Descendants("NumberOfAnimalsHeading").Value
        lblSFDays.Text = cntDoc.Descendants("DaysInStreamHeading").Value
        lblSFHours.Text = cntDoc.Descendants("HoursDayInStreamHeading").Value
        lblSFType.Text = cntDoc.Descendants("SelectAnimalHeading").Value
        lblSFManure.Text = cntDoc.Descendants("DryManureHeading").Value
        lblSFNutrients.Text = cntDoc.Descendants("NutrientFractions").Value
        'sbs
        lblSBS.InnerHtml = cntDoc.Descendants("StreambankStabilizationHeading").Value
        chkSBS.Text = cntDoc.Descendants("Select").Value
        'Riparian forest
        lblRF.InnerHtml = cntDoc.Descendants("RiparianForestHeading").Value
        lblRFArea.Text = cntDoc.Descendants("AreaHeading").Value
        lblRFWidth.Text = cntDoc.Descendants("WidthHeading").Value
        lblRFGrass.Text = cntDoc.Descendants("RiparianGrassPortionHeading").Value
        lblRFRatio.Text = cntDoc.Descendants("RiparianBufferSlopeHeading").Value
        'Filter Strip
        lblFS.InnerHtml = cntDoc.Descendants("FilterStripHeading").Value
        lblFSArea.Text = cntDoc.Descendants("AreaHeading").Value
        lblFSWidth.Text = cntDoc.Descendants("WidthHeading").Value
        lblFSRatio.Text = cntDoc.Descendants("RiparianBufferSlopeHeading").Value
        lblFSType.Text = cntDoc.Descendants("VegetationHeading").Value
        'Water Ways
        lblWW.InnerHtml = cntDoc.Descendants("WaterwayHeading").Value
        lblWWWidth.Text = cntDoc.Descendants("WidthHeading").Value
        lblWWType.Text = cntDoc.Descendants("VegetationHeading").Value
        'Contour buffer
        lblCB.InnerHtml = cntDoc.Descendants("ContourBufferHeading").Value
        lblCBType.Text = cntDoc.Descendants("VegetationHeading").Value
        lblCBBufferWith.Text = cntDoc.Descendants("BufferWidthHeading").Value
        lblCBCropWidth.Text = cntDoc.Descendants("CropWidthHeading").Value
        'land leveling
        lblLL.InnerHtml = cntDoc.Descendants("LandLevelingHeading").Value
        lblReduction.Text = cntDoc.Descendants("SlopeReduction").Value
        'terrace system
        lblTS.InnerHtml = cntDoc.Descendants("TerraceSystemHeading").Value
        chkTS.Text = cntDoc.Descendants("Select").Value
        'manure Control
        lblMC.InnerHtml = cntDoc.Descendants("ManureControlHeading").Value
        'lblNO3NAdj.Text = cntDoc.Descendants("NReduction").Value
        'lblOrgNAdj.Text = cntDoc.Descendants("PReduction").Value
        'lblPO4PAdj.Text = cntDoc.Descendants("NReduction").Value
        'lblOrgPAdj.Text = cntDoc.Descendants("PReduction").Value
        'lblOMAdj.Text = cntDoc.Descendants("PReduction").Value
        lblMCType.Text = cntDoc.Descendants("ManureControlHeading").Value
        'climate change
        lblCC.InnerHtml = cntDoc.Descendants("TempAndPcpChange").Value
        lblCCMinTmp.Text = cntDoc.Descendants("TempAndPcpChange").Value
        lblCCMaxTmp.Text = cntDoc.Descendants("TempAndPcpChange").Value
        lblCCPcp.Text = cntDoc.Descendants("TempAndPcpChange").Value
        'Asfalt or concret
        lblAoC.InnerHtml = cntDoc.Descendants("AsfaltOrConcrete").Value
        chkAoC.Text = cntDoc.Descendants("Select").Value
        'Grass cover
        lblGC.InnerHtml = cntDoc.Descendants("GrassCover").Value
        chkGC.Text = cntDoc.Descendants("Select").Value
        'Slope Adjustment
        lblSA.InnerHtml = cntDoc.Descendants("SlopeAdjustment").Value
        chkSA.Text = cntDoc.Descendants("Select").Value
        'Sharing
        lblSdg.InnerHtml = cntDoc.Descendants("FilterStripHeading").Value
        lblSdgArea.Text = cntDoc.Descendants("AreaHeading").Value
        lblSdgWidth.Text = cntDoc.Descendants("WidthHeading").Value
        lblSdgSlopeRatio.Text = cntDoc.Descendants("RiparianBufferSlopeHeading").Value
        lblSdgCrop.Text = cntDoc.Descendants("VegetationHeading").Value

        gvOperations.Columns(0).HeaderText = cntDoc.Descendants("Select").Value
        gvOperations.Columns(1).HeaderText = cntDoc.Descendants("Crop").Value
        gvOperations.Columns(2).HeaderText = cntDoc.Descendants("Operation").Value
        gvOperations.Columns(3).HeaderText = cntDoc.Descendants("Year").Value
        gvOperations.Columns(4).HeaderText = cntDoc.Descendants("Month").Value
        gvOperations.Columns(5).HeaderText = cntDoc.Descendants("Day").Value
        gvOperations.Columns(6).HeaderText = cntDoc.Descendants("Type").Value
        gvOperations.Columns(7).HeaderText = cntDoc.Descendants("Amount").Value
        gvOperations.Columns(8).HeaderText = cntDoc.Descendants("Depth").Value
        gvOperations.Columns(9).HeaderText = cntDoc.Descendants("NO3").Value.Replace(" (lbs/ac)", "")
        gvOperations.Columns(10).HeaderText = cntDoc.Descendants("PO4").Value.Replace(" (lbs/ac)", "")
        gvOperations.Columns(11).HeaderText = cntDoc.Descendants("OrgN").Value.Replace(" (lbs/ac)", "")
        gvOperations.Columns(12).HeaderText = cntDoc.Descendants("OrgP").Value.Replace(" (lbs/ac)", "")
        gvOperations.Columns(13).HeaderText = "K"
        gvOperations.Columns(14).HeaderText = cntDoc.Descendants("NH3").Value.Replace(" (lbs/ac)", "")
        'tooltips
        btnSave.ToolTip = msgDoc.Descendants("ttSaveAndContinue").Value

        LoadOperations()
    End Sub

    Public Sub InitializeDDLs()
        _irrigationType.Clear()
        Dim irrigationType As IrrigationTypes
        irrigationType = New IrrigationTypes
        irrigationType.Name = cntDoc.Descendants("SelectOne").Value
        irrigationType.Code = 0
        _irrigationType.Add(irrigationType)
        _irrigationType1.Add(irrigationType)

        irrigationType = New IrrigationTypes
        irrigationType.Name = cntDoc.Descendants("Sprinkle").Value
        irrigationType.Code = 1
        _irrigationType.Add(irrigationType)
        _irrigationType1.Add(irrigationType)

        irrigationType = New IrrigationTypes
        irrigationType.Name = cntDoc.Descendants("FurrowFlood").Value
        irrigationType.Code = 2
        _irrigationType.Add(irrigationType)
        _irrigationType1.Add(irrigationType)

        irrigationType = New IrrigationTypes
        irrigationType.Name = cntDoc.Descendants("Drip").Value
        irrigationType.Code = 3
        _irrigationType.Add(irrigationType)
        _irrigationType1.Add(irrigationType)

        irrigationType = New IrrigationTypes
        irrigationType.Name = cntDoc.Descendants("FurrowDiking").Value
        irrigationType.Code = 7
        _irrigationType.Add(irrigationType)
        _irrigationType1.Add(irrigationType)

        irrigationType = New IrrigationTypes
        irrigationType.Name = cntDoc.Descendants("PPTailwaterHeading").Value
        irrigationType.Code = 8
        _irrigationType.Add(irrigationType)

        ddlAIIrrigationType.DataSource = _irrigationType
        ddlAIIrrigationType.DataValueField = "Code"
        ddlAIIrrigationType.DataTextField = "Name"
        ddlAIIrrigationType.DataBind()

        ddlAFIrrigationType.DataSource = _irrigationType
        ddlAFIrrigationType.DataValueField = "Code"
        ddlAFIrrigationType.DataTextField = "Name"
        ddlAFIrrigationType.DataBind()
        ddlAFIrrigationType.Items(ddlAFIrrigationType.Items.Count - 1).Enabled = False

        ddlSdgCrop.DataSource = _bufferCrops
        ddlSdgCrop.DataValueField = "Number"
        ddlSdgCrop.DataTextField = "Name"
        ddlSdgCrop.DataBind()

        ddlWWType.DataSource = _bufferCrops
        ddlWWType.DataValueField = "Number"
        ddlWWType.DataTextField = "Name"
        ddlWWType.DataBind()

        ddlCBType.DataSource = _bufferCrops
        ddlCBType.DataValueField = "Number"
        ddlCBType.DataTextField = "Name"
        ddlCBType.DataBind()

        ddlFSType.DataSource = _bufferCrops
        ddlFSType.DataValueField = "Number"
        ddlFSType.DataTextField = "Name"
        ddlFSType.DataBind()

        ddlSFType.DataSource = _animals
        ddlSFType.DataValueField = "Number"
        ddlSFType.DataTextField = "Name"
        ddlSFType.DataBind()

        Dim item As ListItem
        item = New ListItem
        item.Text = "Select One"
        item.Value = "0"
        ddlMCType.Items.Add(item)
        item = New ListItem
        item.Text = "Digeste"
        item.Value = "1.286, 0.74, 0.06, 1.53, 0.33"
        ddlMCType.Items.Add(item)
        item = New ListItem
        item.Text = "Primary Screen"
        item.Value = "1.12, 0.67, 0.23, 1.76, 0.59"
        ddlMCType.Items.Add(item)
        item = New ListItem
        item.Text = "DAF"
        item.Value = "1.02, 0.21, 0.23, 0.08, 0.835"
        ddlMCType.Items.Add(item)
        item = New ListItem
        item.Text = "NH3 Stipper"
        item.Value = "0.3, 0.21, 0.23, 0.08, 0.851"
        ddlMCType.Items.Add(item)
        'item = New ListItem
        'item.Text = "UF/RO"
        'item.Value = "44, 33"
        'ddlMCType.Items.Add(item)
    End Sub

    Protected Sub LoadBMPs()
        fsetAI.Style.Item("display") = "none"
        fsetAF.Style.Item("display") = "none"
        fsetTD.Style.Item("display") = "none"
        fsetPPDE.Style.Item("display") = "none"
        fsetPPDS.Style.Item("display") = "none"
        fsetPPND.Style.Item("display") = "none"
        fsetPPTW.Style.Item("display") = "none"
        fsetWL.Style.Item("display") = "none"
        fsetPND.Style.Item("display") = "none"
        fsetSF.Style.Item("display") = "none"
        fsetSBS.Style.Item("display") = "none"
        fsetRF.Style.Item("display") = "none"
        fsetFS.Style.Item("display") = "none"
        fsetWW.Style.Item("display") = "none"
        fsetCB.Style.Item("display") = "none"
        fsetLL.Style.Item("display") = "none"
        fsetTS.Style.Item("display") = "none"
        fsetMC.Style.Item("display") = "none"
        fsetLM.Style.Item("display") = "none"
        fsetSdg.Style.Item("display") = "none"
        fsetAoC.Style.Item("display") = "none"
        fsetGC.Style.Item("display") = "none"
        fsetSA.Style.Item("display") = "none"
        fsetCC.Style.Item("display") = "none"

        If _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count <= 0 Then
            Exit Sub
        End If

        LoadAI()
        LoadAF()
        LoadTD()
        LoadPPND()
        LoadPPDS()
        LoadPPDE()
        LoadPPTW()
        LoadWL()
        LoadPND()
        LoadSF()
        LoadSBS()
        LoadRF()
        LoadFS()
        LoadWW()
        LoadCB()
        LoadLL()
        LoadTS()
        LoadLM()
        LoadMC()
        LoadSdg()
        LoadAoC()
        LoadGC()
        LoadSA()
        LoadCC()
    End Sub

    Private Sub LoadSA()
        'Land leveling
        chkSA.Checked = False
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            chkSA.Checked = .Sa
            If .Sa Then fsetSA.Style.Item("display") = "" : ExpandTreeView(7)
        End With
    End Sub

    Private Sub LoadGC()
        'Land leveling
        chkGC.Checked = False
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            chkGC.Checked = .Gc
            If .Gc Then fsetGC.Style.Item("display") = "" : ExpandTreeView(7)
        End With
    End Sub

    Private Sub LoadAoC()
        'Asfalt or concrete
        chkAoC.Checked = False
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            chkAoC.Checked = .AoC
            If .AoC Then fsetAoC.Style.Item("display") = "" : ExpandTreeView(7)
        End With
    End Sub

    Private Sub LoadSdg()
        'shading
        txtSdgArea.Text = "0.00"
        txtSdgSlopeRatio.Text = "0.00"
        txtSdgWidth.Text = "0.00"
        ddlSdgCrop.SelectedIndex = 0
        txtSdgArea.BackColor = Drawing.Color.White
        txtSdgSlopeRatio.BackColor = Drawing.Color.White
        txtSdgWidth.BackColor = Drawing.Color.White
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            txtSdgArea.Text = .SdgArea
            txtSdgSlopeRatio.Text = .SdgslopeRatio
            txtSdgWidth.Text = .SdgWidth
            If .SdgCrop > 0 Then ddlSdgCrop.SelectedValue = .SdgCrop
            If .SdgArea <> 0 Or .SdgslopeRatio <> 0 Or .SdgWidth <> 0 Or .SdgCrop > 0 Then
                fsetSdg.Style.Item("display") = "" : ExpandTreeView(7)
                ValidateLoadBMPValue(txtSdgArea, msgDoc.Descendants("SdgAreaMsg").Value & msgDoc.Descendants("NumericValue").Value & ValueMin & " - " & ValueMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
                ValidateLoadBMPValue(txtSdgWidth, msgDoc.Descendants("SdgWidthMsg").Value & msgDoc.Descendants("NumericValue").Value & ValueMin & " - " & ValueMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
                ValidateLoadBMPValue(txtSdgSlopeRatio, msgDoc.Descendants("SdgslopeRatioMsg").Value & msgDoc.Descendants("NumericValue").Value & "0.25" & " - " & FractionMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, 0.25, FractionMax)
                ValidateLoadBMPDll(ddlSdgCrop, (msgDoc.Descendants("VegetationTypeMsg").Value))
            End If
        End With
    End Sub

    Private Sub LoadCC()
        'Climate change
        txtCCMinTmp.Text = "0.00" : txtCCMinTmp.Attributes("min") = minChange : txtCCMinTmp.Attributes("max") = maxChange
        txtCCMaxTmp.Text = "0.00" : txtCCMaxTmp.Attributes("min") = minChange : txtCCMaxTmp.Attributes("max") = maxChange
        txtCCPcp.Text = "0.00" : txtCCPcp.Attributes("min") = minChange : txtCCPcp.Attributes("max") = maxChange
        txtCCMinTmp.BackColor = Drawing.Color.White
        txtCCMaxTmp.BackColor = Drawing.Color.White
        txtCCPcp.BackColor = Drawing.Color.White
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            txtCCMinTmp.Text = .CcMinimumTeperature
            txtCCMaxTmp.Text = .CcMaximumTeperature
            txtCCPcp.Text = .CcPrecipitation
            If .CcMinimumTeperature <> 0 Or .CcMaximumTeperature <> 0 Or .CcPrecipitation <> 0 Then
                fsetCC.Style.Item("display") = "" : ExpandTreeView(6)
                ValidateLoadBMPValue(txtWLArea, msgDoc.Descendants("WLAreaMsg").Value & msgDoc.Descendants("NumericValue").Value & ValueMin & " - " & ValueMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
                ValidateLoadBMPValue(txtCCMinTmp, msgDoc.Descendants("MinTemp").Value & msgDoc.Descendants("NumericValue").Value & minChange & " - " & maxChange & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, minChange, maxChange)
                ValidateLoadBMPValue(txtCCMaxTmp, msgDoc.Descendants("MaxTemp").Value & msgDoc.Descendants("NumericValue").Value & minChange & " - " & maxChange & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, minChange, maxChange)
                ValidateLoadBMPValue(txtCCPcp, msgDoc.Descendants("Precipitation").Value & msgDoc.Descendants("NumericValue").Value & minChange & " - " & maxChange & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, minChange, maxChange)
            End If
        End With
    End Sub


    Private Sub LoadLM()
        'Liming
        chkLM.Checked = False
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            chkLM.Checked = .Lm
            'If .Lm Then tvMain.Nodes(1).Expand() : tvMain.Nodes(1).ChildNodes(5).Expanded = True : ExpandTreeView(5)
        End With
    End Sub

    Private Sub LoadMC()
        'Manure control
        txtmcNO3N.Text = "0.00"
        txtmcOrgN.Text = "0.00"
        txtmcPO4P.Text = "0.00"
        txtmcOrgP.Text = "0.00"
        txtmcOM.Text = "0.00"
        ddlMCType.SelectedIndex = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            txtmcNO3N.Text = .mcNO3_N
            txtmcOrgN.Text = .mcOrgN
            txtmcPO4P.Text = .mcPO4_P
            txtmcOrgP.Text = .mcOrgP
            txtmcOM.Text = .mcOM
            If .mcType > 0 Then ddlMCType.SelectedIndex = .mcType
            If .mcType > 0 Then
                fsetMC.Style.Item("display") = "" : ExpandTreeView(5)
                ValidateBMPValue(txtmcNO3N, .mcNO3_N, (msgDoc.Descendants("ManureControlMsg").Value.Replace("First", 0).Replace("Second", maxChange)) & Microsoft.VisualBasic.ControlChars.CrLf, "mcFraction", FractionMin, maxChange)
                ValidateBMPValue(txtmcOrgN, .mcOrgN, (msgDoc.Descendants("ManureControlMsg").Value.Replace("First", 0).Replace("Second", maxChange)) & Microsoft.VisualBasic.ControlChars.CrLf, "mcFraction", FractionMin, maxChange)
                ValidateBMPValue(txtmcPO4P, .mcPO4_P, (msgDoc.Descendants("ManureControlMsg").Value.Replace("First", 0).Replace("Second", maxChange)) & Microsoft.VisualBasic.ControlChars.CrLf, "mcFraction", FractionMin, maxChange)
                ValidateBMPValue(txtmcOrgP, .mcOrgP, (msgDoc.Descendants("ManureControlMsg").Value.Replace("First", 0).Replace("Second", maxChange)) & Microsoft.VisualBasic.ControlChars.CrLf, "mcFraction", FractionMin, maxChange)
                ValidateBMPValue(txtmcOM, .mcOM, (msgDoc.Descendants("ManureControlMsg").Value.Replace("First", 0).Replace("Second", maxChange)) & Microsoft.VisualBasic.ControlChars.CrLf, "mcFraction", FractionMin, maxChange)
            End If
        End With

    End Sub

    Private Sub LoadTS()
        'Terrace System
        chkTS.Checked = False
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            chkTS.Checked = .Ts
            If .Ts Then fsetTS.Style.Item("display") = "" : ExpandTreeView(5)
        End With
    End Sub

    Private Sub LoadLL()
        'Land leveling
        txtLLReduction.Text = "0.00"
        txtLLReduction.BackColor = Drawing.Color.White
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            txtLLReduction.Text = .SlopeRed
            If .SlopeRed <> 0 Then
                fsetLL.Style.Item("display") = "" : ExpandTreeView(5)
                ValidateLoadBMPValue(txtLLReduction, (msgDoc.Descendants("LandLevelingMsg").Value.Replace("First", LLSlopeRedMin).Replace("Second", LLSlopeRedMax)) & Microsoft.VisualBasic.ControlChars.CrLf, LLSlopeRedMin, LLSlopeRedMax)
            End If
        End With
    End Sub

    Private Sub LoadCB()
        'Contour Buffer
        txtCBBufferWidth.Text = "0.00"
        txtCBCropWidth.Text = "0.00"
        ddlCBType.SelectedIndex = 0
        txtCBBufferWidth.BackColor = Drawing.Color.White
        txtCBCropWidth.BackColor = Drawing.Color.White
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            txtCBBufferWidth.Text = .CBBWidth
            txtCBCropWidth.Text = .CBCWidth
            ddlCBType.SelectedValue = .CBCrop
            If .CBBWidth <> 0 Or .CBCWidth > 0 Or .CBCrop > 0 Then
                fsetCB.Style.Item("display") = "" : ExpandTreeView(4)
                ValidateLoadBMPValue(txtCBBufferWidth, (msgDoc.Descendants("BufferWidthMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
                ValidateLoadBMPValue(txtCBCropWidth, (msgDoc.Descendants("BufferWidthMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
            End If
        End With
    End Sub

    Private Sub LoadWW()
        'water way
        txtWWWidth.Text = "0.00"
        ddlWWType.SelectedIndex = 0
        txtWWWidth.BackColor = Drawing.Color.White
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            txtWWWidth.Text = .WWWidth
            ddlWWType.SelectedValue = .WWCrop
            If .WWWidth <> 0 Or .WWCrop > 0 Then
                fsetWW.Style.Item("display") = "" : ExpandTreeView(3)
                ValidateLoadBMPValue(txtWWWidth, (msgDoc.Descendants("WaterWayMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
                ValidateLoadBMPDll(ddlWWType, msgDoc.Descendants("VegetationTypeMsg").Value)
            End If
        End With
    End Sub

    Private Sub LoadFS()
        'filter strip
        txtFSArea.Text = "0.00"
        'txtFSEff.Text = "0.50"
        txtFSRatio.Text = "0.00"
        txtFSWidth.Text = "0.00"
        ddlFSType.SelectedIndex = 0
        txtFSArea.BackColor = Drawing.Color.White
        txtFSRatio.BackColor = Drawing.Color.White
        txtFSWidth.BackColor = Drawing.Color.White
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            txtFSArea.Text = .FSArea
            txtFSRatio.Text = .FSslopeRatio
            txtFSWidth.Text = .FSWidth
            If .FSCrop > 0 Then ddlFSType.SelectedValue = .FSCrop
            If .FSArea <> 0 Or .FSslopeRatio <> 0 Or .FSWidth <> 0 Or .FSCrop > 0 Then
                fsetFS.Style.Item("display") = "" : ExpandTreeView(3)
                ValidateLoadBMPValue(txtFSArea, msgDoc.Descendants("FSAreaMsg").Value & msgDoc.Descendants("NumericValue").Value & ValueMin & " - " & ValueMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, 0, ValueMax)
                ValidateLoadBMPValue(txtFSWidth, msgDoc.Descendants("FSWidthMsg").Value & msgDoc.Descendants("NumericValue").Value & ValueMin & " - " & ValueMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
                ValidateLoadBMPValue(txtFSRatio, msgDoc.Descendants("FSslopeRatioMsg").Value & msgDoc.Descendants("NumericValue").Value & 0.25 & " - " & FractionMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, 0.25, FractionMax)
                ValidateLoadBMPDll(ddlFSType, msgDoc.Descendants("VegetationTypeMsg").Value)
            End If
        End With
    End Sub

    Private Sub LoadRF()
        'Riparian forest
        txtRFArea.Text = "0.00"
        txtRFGrass.Text = "0.00"
        txtRFRatio.Text = "0.00"
        txtRFWidth.Text = "0.00"
        txtRFArea.BackColor = Drawing.Color.White
        txtRFGrass.BackColor = Drawing.Color.White
        txtRFRatio.BackColor = Drawing.Color.White
        txtRFWidth.BackColor = Drawing.Color.White
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            txtRFArea.Text = .RFArea
            txtRFGrass.Text = .RFGrassFieldPortion
            txtRFRatio.Text = .RFslopeRatio
            txtRFWidth.Text = .RFWidth
            If .RFArea <> 0 Or .RFGrassFieldPortion <> 0 Or .RFslopeRatio <> 0 Or .RFWidth <> 0 Then
                fsetRF.Style.Item("display") = "" : ExpandTreeView(3)
                ValidateLoadBMPValue(txtRFWidth, msgDoc.Descendants("RFWidthMsg").Value & msgDoc.Descendants("NumericValue").Value & ValueMin & " - " & ValueMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
                ValidateLoadBMPValue(txtRFArea, msgDoc.Descendants("RFAreaMsg").Value & msgDoc.Descendants("NumericValue").Value & ValueMin & " - " & ValueMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
                ValidateLoadBMPValue(txtRFRatio, msgDoc.Descendants("RFslopeRatioMsg").Value & msgDoc.Descendants("NumericValue").Value & "0.25" & " - " & FractionMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, 0.25, FractionMax)
                ValidateLoadBMPValue(txtRFGrass, msgDoc.Descendants("GrassFieldPortionMsg").Value & msgDoc.Descendants("NumericValue").Value & FractionMin & " - " & "0.75" & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, FractionMin, 0.75)
            End If
        End With
    End Sub

    Private Sub LoadSBS()
        'auto irrigation
        chkSBS.Checked = False
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            chkSBS.Checked = .Sbs
            If chkSBS.Checked Then fsetSBS.Style.Item("display") = "" : ExpandTreeView(3)
        End With
    End Sub

    Private Sub LoadSF()
        'stream fencing
        ddlSFType.SelectedIndex = -1
        txtSFAnimals.Text = "0"
        txtSFDays.Text = "0"
        txtSFHours.Text = "0.00"
        txtSFManure.Text = "0.00"
        txtSFNO3.Text = "0.00"
        txtSFPO4.Text = "0.00"
        txtSFOrgN.Text = "0.00"
        txtSFOrgP.Text = "0.00"
        txtSFNH3.Text = "0.00"
        txtSFAnimals.BackColor = Drawing.Color.White
        txtSFDays.BackColor = Drawing.Color.White
        txtSFHours.BackColor = Drawing.Color.White
        txtSFManure.BackColor = Drawing.Color.White
        txtSFNO3.BackColor = Drawing.Color.White
        txtSFPO4.BackColor = Drawing.Color.White
        txtSFOrgN.BackColor = Drawing.Color.White
        txtSFOrgP.BackColor = Drawing.Color.White
        txtSFNH3.BackColor = Drawing.Color.White
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            txtSFAnimals.Text = .SFAnimals
            txtSFDays.Text = .SFDays
            txtSFHours.Text = .SFHours
            txtSFManure.Text = .SFDryManure
            txtSFNO3.Text = .SFNo3
            txtSFPO4.Text = .SFPo4
            txtSFOrgN.Text = .SFOrgN
            txtSFOrgP.Text = .SFOrgP
            txtSFNH3.Text = .SFNH3
            If .SFName <> selectOne And .SFName <> String.Empty Then
                For i = 0 To ddlSFType.Items.Count - 1
                    If ddlSFType.Items(i).Text.Trim = .SFName.Trim Then
                        ddlSFType.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If
            If .SFAnimals <> 0 Or .SFDays <> 0 Or .SFHours <> 0 Then
                fsetSF.Style.Item("display") = "" : ExpandTreeView(3)
                ValidateLoadBMPValue(txtSFDays, (msgDoc.Descendants("DaysMessage").Value.Replace("First", SFDaysMin).Replace("Second", SFDaysMax)) & Microsoft.VisualBasic.ControlChars.CrLf, SFDaysMin, SFDaysMax)
                ValidateLoadBMPValue(txtSFAnimals, (msgDoc.Descendants("AnimalsMessage").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
                ValidateLoadBMPValue(txtSFHours, (msgDoc.Descendants("HoursMessage").Value.Replace("First", SFHoursMin).Replace("Second", SFHoursMax)) & Microsoft.VisualBasic.ControlChars.CrLf, SFHoursMin, SFHoursMax)
                ValidateLoadBMPValue(txtSFManure, (msgDoc.Descendants("DryManureMessage").Value.Replace("First", SFDryManureMin).Replace("Second", SFDryManureMax)) & Microsoft.VisualBasic.ControlChars.CrLf, SFDryManureMin, SFDryManureMax)
                ValidateLoadBMPValue(txtSFNO3, (msgDoc.Descendants("NO3NumericMessage").Value.Replace("First", 0.00001).Replace("Second", FractionMax)) & Microsoft.VisualBasic.ControlChars.CrLf, 0.00001, FractionMax)
                ValidateLoadBMPValue(txtSFPO4, (msgDoc.Descendants("PO3NumericMessage").Value.Replace("First", 0.00001).Replace("Second", FractionMax)) & Microsoft.VisualBasic.ControlChars.CrLf, 0.00001, FractionMax)
                ValidateLoadBMPValue(txtSFOrgN, (msgDoc.Descendants("OrgNNumericMessage").Value.Replace("First", 0.00001).Replace("Second", FractionMax)) & Microsoft.VisualBasic.ControlChars.CrLf, 0.00001, FractionMax)
                ValidateLoadBMPValue(txtSFOrgP, (msgDoc.Descendants("OrgPNumericMessage").Value.Replace("First", 0.00001).Replace("Second", FractionMax)) & Microsoft.VisualBasic.ControlChars.CrLf, 0.00001, FractionMax)
                ValidateLoadBMPValue(txtSFNH3, (msgDoc.Descendants("NH3NumericMessage").Value.Replace("First", 0.00001).Replace("Second", FractionMax)) & Microsoft.VisualBasic.ControlChars.CrLf, 0.00001, FractionMax)
                ValidateLoadBMPDll(ddlSFType, (msgDoc.Descendants("AnimalTypeMsg").Value))
            End If
        End With
    End Sub

    Private Sub LoadPND()
        'Ponds
        txtPNDFraction.Text = "0.00"
        txtPNDFraction.BackColor = Drawing.Color.White
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            txtPNDFraction.Text = .PndF
            If .PndF <> 0 Then
                fsetPND.Style.Item("display") = "" : ExpandTreeView(2)
                ValidateLoadBMPValue(txtPNDFraction, (msgDoc.Descendants("PondFractionMsg").Value.Replace("First", FractionMin).Replace("Second", FractionMax)) & Microsoft.VisualBasic.ControlChars.CrLf, FractionMin, FractionMax)
            End If
        End With
    End Sub

    Private Sub LoadWL()
        'Wetland
        txtWLArea.Text = "0.00" : txtWLArea.Attributes("min") = ValueMin : txtWLArea.Attributes("max") = ValueMax
        txtWLArea.BackColor = Drawing.Color.White
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            txtWLArea.Text = .WLArea
            If .WLArea <> 0 Then
                fsetWL.Style.Item("display") = "" : ExpandTreeView(2)
                ValidateLoadBMPValue(txtWLArea, msgDoc.Descendants("WLAreaMsg").Value & msgDoc.Descendants("NumericValue").Value & ValueMin & " - " & ValueMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
            End If
        End With
    End Sub

    Private Sub LoadPPTW()
        txtPPTWWidth.Text = "0.00" : txtPPTWWidth.Attributes("min") = ValueMin : txtPPTWWidth.Attributes("max") = ValueMax
        txtPPTWSides.Text = "0.00" : txtPPTWSides.Attributes("min") = sidesMin : txtPPTWSides.Attributes("max") = sidesMax
        txtPPTWResArea.Text = "0.00" : txtPPTWResArea.Attributes("min") = ValueMin : txtPPTWResArea.Attributes("max") = ValueMax
        txtPPTWWidth.BackColor = Drawing.Color.White
        txtPPTWSides.BackColor = Drawing.Color.White
        txtPPTWResArea.BackColor = Drawing.Color.White
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            txtPPTWWidth.Text = .PPTWWidth
            txtPPTWSides.Text = .PPTWSides
            txtPPTWResArea.Text = .PPTWResArea
            If .PPTWWidth <> 0 And .PPTWSides <> 0 And .PPTWResArea <> 0 Then
                fsetPPTW.Style.Item("display") = "" : ExpandTreeView(1)
                ValidateLoadBMPValue(txtPPTWWidth, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
                ValidateLoadBMPValue(txtPPTWSides, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", sidesMin).Replace("Second", sidesMax)) & Microsoft.VisualBasic.ControlChars.CrLf, sidesMin, sidesMax)
                ValidateLoadBMPValue(txtPPTWResArea, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
            End If
        End With

    End Sub

    Private Sub LoadPPDE()
        txtPPDEWidth.Text = "0.00" : txtPPDEWidth.Attributes("min") = ValueMin : txtPPDEWidth.Attributes("max") = ValueMax
        txtPPDESides.Text = "0.00" : txtPPDESides.Attributes("min") = sidesMin : txtPPDESides.Attributes("max") = sidesMax
        txtPPDEResArea.Text = "0.00" : txtPPDEResArea.Attributes("min") = ValueMin : txtPPDEResArea.Attributes("max") = ValueMax
        txtPPDEWidth.BackColor = Drawing.Color.White
        txtPPDESides.BackColor = Drawing.Color.White
        txtPPDEResArea.BackColor = Drawing.Color.White
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            txtPPDEWidth.Text = .PPDEWidth
            txtPPDESides.Text = .PPDESides
            txtPPDEResArea.Text = .PPDEResArea
            If .PPDEWidth <> 0 And .PPDESides <> 0 And .PPDEResArea <> 0 Then
                fsetPPDE.Style.Item("display") = "" : ExpandTreeView(1)
                ValidateLoadBMPValue(txtPPDEWidth, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
                ValidateLoadBMPValue(txtPPDESides, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", sidesMin).Replace("Second", sidesMax)) & Microsoft.VisualBasic.ControlChars.CrLf, sidesMin, sidesMax)
                ValidateLoadBMPValue(txtPPDEResArea, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
            End If
        End With
    End Sub

    Private Sub LoadPPDS()
        txtPPDSWidth.Text = "0.00" : txtPPDSWidth.Attributes("min") = ValueMin : txtPPDSWidth.Attributes("max") = ValueMax
        txtPPDSSides.Text = "0.00" : txtPPDSSides.Attributes("min") = sidesMin : txtPPDSSides.Attributes("max") = sidesMax
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            txtPPDSWidth.Text = .PPDSWidth
            txtPPDSSides.Text = .PPDSSides
            txtPPDSWidth.BackColor = Drawing.Color.White
            txtPPDSSides.BackColor = Drawing.Color.White
            If .PPDSWidth <> 0 And .PPDSSides <> 0 Then
                fsetPPDS.Style.Item("display") = "" : ExpandTreeView(1)
                ValidateLoadBMPValue(txtPPDSWidth, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
                ValidateLoadBMPValue(txtPPDSSides, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", sidesMin).Replace("Second", sidesMax)) & Microsoft.VisualBasic.ControlChars.CrLf, sidesMin, sidesMax)
            End If
        End With
    End Sub

    Private Sub LoadPPND()
        txtPPNDWidth.Text = "0.00" : txtPPNDWidth.Attributes("min") = ValueMin : txtPPNDWidth.Attributes("max") = ValueMax
        txtPPNDSides.Text = "0.00" : txtPPNDSides.Attributes("min") = sidesMin : txtPPNDSides.Attributes("max") = sidesMax
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            txtPPNDWidth.Text = .PPNDWidth
            txtPPNDSides.Text = .PPNDSides
            txtPPNDWidth.BackColor = Drawing.Color.White
            txtPPNDSides.BackColor = Drawing.Color.White
            If .PPNDWidth <> 0 And .PPNDSides <> 0 Then
                fsetPPND.Style.Item("display") = "" : ExpandTreeView(1)
                ValidateLoadBMPValue(txtPPNDWidth, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
                ValidateLoadBMPValue(txtPPNDSides, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", sidesMin).Replace("Second", sidesMax)) & Microsoft.VisualBasic.ControlChars.CrLf, sidesMin, sidesMax)
            End If
        End With
    End Sub

    Private Sub LoadTD()
        'auto irrigation
        txtTDDepth.Text = "0.00" : txtTDDepth.Attributes("min") = ValueMin : txtTDDepth.Attributes("max") = ValueMax
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            txtTDDepth.Text = .TileDrainDepth
            txtTDDepth.BackColor = Drawing.Color.White
            If .TileDrainDepth <> 0 Then
                fsetTD.Style.Item("display") = "" : ExpandTreeView(1)
                ValidateLoadBMPValue(txtTDDepth, msgDoc.Descendants("TileDrainDepthMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax) & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
            End If
        End With
    End Sub

    Private Sub LoadAF()
        'auto fertigation
        txtAFWaterStress.Text = "0.80" : txtAFWaterStress.Attributes("min") = FractionMin : txtAFWaterStress.Attributes("max") = FractionMax
        txtAFIrrigationEfficiancy.Text = "0.00" : txtAFIrrigationEfficiancy.Attributes("min") = FractionMin : txtAFIrrigationEfficiancy.Attributes("max") = FractionMax
        txtAFFrequency.Text = "0.00" : txtAFFrequency.Attributes("min") = SFDaysMin : txtAFFrequency.Attributes("max") = SFDaysMax
        txtAFMaxSingleAppl.Text = armxDefault : txtAFMaxSingleAppl.Attributes("min") = ValueMin : txtAFMaxSingleAppl.Attributes("max") = ValueMax
        txtAFSafetyFactor.Text = "0.00" : txtAFSafetyFactor.Attributes("min") = FractionMin : txtAFSafetyFactor.Attributes("max") = FractionMax
        txtAFNConc.Text = "0.00"
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex)._bmpsInfo
            txtAFWaterStress.Text = .AFWaterStressFactor
            txtAFIrrigationEfficiancy.Text = .AFEff
            txtAFFrequency.Text = .AFFreq
            txtAFMaxSingleAppl.Text = .AFMaxSingleApp
            txtAFSafetyFactor.Text = .AFSafetyFactor
            txtAFNConc.Text = .AFNConc
            ddlAFIrrigationType.SelectedValue = .AFType
            Select Case ddlAFIrrigationType.SelectedValue
                Case 7
                    txtAFSafetyFactor.Style.Item("display") = "none"
                    lblAFSafetyFactor.Style.Item("display") = "none"
                Case Else
                    txtAFSafetyFactor.Style.Item("display") = "none"
                    lblAFSafetyFactor.Style.Item("display") = "none"
            End Select
            txtAFIrrigationEfficiancy.BackColor = Drawing.Color.White
            txtAFWaterStress.BackColor = Drawing.Color.White
            txtAFFrequency.BackColor = Drawing.Color.White
            txtAFMaxSingleAppl.BackColor = Drawing.Color.White
            If .AFEff <> 0 Or .AFFreq <> 0 Or .AFType > 0 Then
                fsetAF.Style.Item("display") = "" : ExpandTreeView(0)
                ValidateLoadBMPValue(txtAFIrrigationEfficiancy, msgDoc.Descendants("EfficiencyMessage").Value.Replace("First", FractionMin).Replace("Second", FractionMax) & Microsoft.VisualBasic.ControlChars.CrLf, FractionMin, FractionMax)
                ValidateLoadBMPValue(txtAFWaterStress, msgDoc.Descendants("WaterStressFactorMsg").Value & Microsoft.VisualBasic.ControlChars.CrLf, FractionMin, FractionMax)
                ValidateLoadBMPValue(txtAFFrequency, (msgDoc.Descendants("FrequencyMessage").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
                ValidateLoadBMPValue(txtAFMaxSingleAppl, (msgDoc.Descendants("ApplicationMessage").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
                ValidateLoadBMPDll(ddlAFIrrigationType, (msgDoc.Descendants("IrrigationTypeMsg").Value))
            End If
        End With
    End Sub

    Private Sub LoadAI()
        'auto irrigation default values
        txtAIWaterStress.Text = "0.80"
        txtAIIrrigationEfficiancy.Text = "0.00"
        txtAIFrequency.Text = "0.00"
        txtAIMaxSingleAppl.Text = armxDefault
        txtAIResArea.Text = "0.00"
        txtAISafetyFactor.Text = "0.00"
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            ddlAIIrrigationType.SelectedValue = .AIType
            txtAIWaterStress.Text = .AIWaterStressFactor
            txtAIIrrigationEfficiancy.Text = .AIEff
            txtAIFrequency.Text = .AIFreq
            txtAIMaxSingleAppl.Text = .AIMaxSingleApp
            txtAIMaxSingleAppl.Text = .AIMaxSingleApp
            txtAIResArea.Text = .AIResArea
            txtAISafetyFactor.Text = .AISafetyFactor

            Select Case ddlAIIrrigationType.SelectedValue
                Case 7
                    txtAISafetyFactor.Style.Item("display") = "none"
                    lblAISafetyFactor.Style.Item("display") = "none"
                Case 8
                    txtAIResArea.Style.Item("display") = ""
                    lblAIResArea.Style.Item("display") = ""
                    .AIEff = 0.9
                    'todo control exclusions
                Case Else
                    txtAIResArea.Style.Item("display") = "none"
                    lblAIResArea.Style.Item("display") = "none"
                    txtAISafetyFactor.Style.Item("display") = "none"
                    lblAISafetyFactor.Style.Item("display") = "none"
            End Select
            txtAIIrrigationEfficiancy.BackColor = Drawing.Color.White
            txtAIWaterStress.BackColor = Drawing.Color.White
            txtAIFrequency.BackColor = Drawing.Color.White
            txtAIMaxSingleAppl.BackColor = Drawing.Color.White
            If .AIType > 0 Or .AIEff <> 0 Or .AIFreq <> 0 Then
                fsetAI.Style.Item("display") = ""
                ExpandTreeView(0)
                ValidateLoadBMPValue(txtAIIrrigationEfficiancy, msgDoc.Descendants("EfficiencyMessage").Value.Replace("First", FractionMin).Replace("Second", FractionMax) & Microsoft.VisualBasic.ControlChars.CrLf, FractionMin, FractionMax)
                ValidateLoadBMPValue(txtAIWaterStress, msgDoc.Descendants("WaterStressFactorMsg").Value & Microsoft.VisualBasic.ControlChars.CrLf, FractionMin, FractionMax)
                ValidateLoadBMPValue(txtAIFrequency, (msgDoc.Descendants("FrequencyMessage").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
                ValidateLoadBMPValue(txtAIMaxSingleAppl, (msgDoc.Descendants("ApplicationMessage").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, ValueMin, ValueMax)
                ValidateLoadBMPDll(ddlAIIrrigationType, (msgDoc.Descendants("IrrigationTypeMsg").Value))
            End If
        End With
    End Sub
    Protected Sub LoadOperations()
        'Dim localCurrentScenario As Short = ddlScenarios.SelectedIndex
        'Dim localCurrentField As Short = ddlFields.SelectedIndex
        currentFieldNumber = ddlFields.SelectedIndex
        currentScenarioNumber = ddlScenarios.SelectedIndex
        LoadCrops1()
        gvOperations.DataSource = Nothing
        gvOperations.DataBind()
        If _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count <= 0 Then Exit Sub
        _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo.Sort(New sortByDate())
        gvOperations.DataSource = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo
        gvOperations.DataBind()
    End Sub

    Protected Sub btnDeleteScenario_Click()
        Try
            If currentScenarioNumber < 0 Then
                Throw New Global.System.Exception(msgDoc.Descendants("NoScenario").Value & vbCrLf)
            End If
            _fieldsInfo1(currentFieldNumber).DeleteScenario(ddlScenarios.SelectedIndex)
            currentScenarioNumber = ddlScenarios.SelectedIndex - 1

            'scenario needs to be deleted from soils
            For Each Soil In _fieldsInfo1(currentFieldNumber)._soilsInfo
                Soil.DeleteScenario(ddlScenarios.SelectedIndex)
            Next

            Select Case ddlScenarios.SelectedIndex
                Case Is > 0
                    currentScenarioNumber = ddlScenarios.SelectedIndex - 1
                Case 0
                    currentScenarioNumber = 0
                Case Else
                    currentScenarioNumber = -1
            End Select
            ddlScenarios.SelectedIndex = currentScenarioNumber
            ddlScenarios_SelectedIndexChanged()
            ArrangeInfo("Save")
            'End If
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", ex.Message)
        End Try
    End Sub

    Private Sub btnRenameScenario_Click()
        Try
            If txtScenarioRenamed.Text = "" Or txtScenarioRenamed.Text = String.Empty Then Throw New Global.System.Exception(msgDoc.Descendants("ScenarioName").Value & vbCrLf)

            For i = 0 To _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count - 1
                If txtScenarioRenamed.Text.Trim = _fieldsInfo1(currentFieldNumber)._scenariosInfo(i).Name Then
                    showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("ScenarioExist").Value)
                    Exit Sub
                End If
            Next

            _fieldsInfo1(currentFieldNumber)._scenariosInfo(ddlScenarios.SelectedIndex).Name = txtScenarioRenamed.Text
            For Each soil In _fieldsInfo1(currentFieldNumber)._soilsInfo
                soil._scenariosInfo(ddlScenarios.SelectedIndex).Name = txtScenarioRenamed.Text
            Next

            LoadScenarios(ddlScenarios, _fieldsInfo1(currentFieldNumber)._scenariosInfo, currentScenarioNumber)
            LoadScenarios(ddlFromScenario, _fieldsInfo1(currentFieldNumber)._scenariosInfo, currentScenarioNumber)
            LoadAllScenarios(ddlAllScenario, _fieldsInfo1, currentScenarioNumber)
            ddlScenarios_SelectedIndexChanged()
            txtScenarioRenamed.Text = String.Empty
            ArrangeInfo("Save")
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("ScenarioName").Value)
        End Try
    End Sub

    Protected Sub btnAddNewScenario_Click()
        Try
            If txtScenarioName.Text = "" Or txtScenarioName.Text = String.Empty Then Throw New Global.System.Exception(msgDoc.Descendants("ScenarioName").Value & vbCrLf)
            For i = 0 To _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count - 1
                If txtScenarioName.Text.Trim = _fieldsInfo1(currentFieldNumber)._scenariosInfo(i).Name Then
                    showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("ScenarioExist").Value)
                    Exit Sub
                End If
            Next

            _fieldsInfo1(currentFieldNumber).AddScenario(txtScenarioName.Text, _fieldsInfo1(currentFieldNumber).Area, _fieldsInfo1(currentFieldNumber).RchcVal, _fieldsInfo1(currentFieldNumber).RchkVal, currentScenarioNumber)
            currentScenarioNumber = _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count - 1
            LoadScenarios(ddlScenarios, _fieldsInfo1(currentFieldNumber)._scenariosInfo, currentScenarioNumber)
            LoadScenarios(ddlFromScenario, _fieldsInfo1(currentFieldNumber)._scenariosInfo, currentScenarioNumber)
            LoadAllScenarios(ddlAllScenario, _fieldsInfo1, currentScenarioNumber)
            ddlScenarios_SelectedIndexChanged()
            txtScenarioName.Text = String.Empty
            ArrangeInfo("Save")
            'SaveProject(Session("userGuide"), _projects)
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("ScenarioName").Value)
        End Try
    End Sub

    Protected Sub ddlFields_SelectedIndexChanged()
        currentFieldNumber = ddlFields.SelectedIndex
        If _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count > 0 Then
            currentScenarioNumber = 0
        Else
            currentScenarioNumber = -1
        End If
        hdnField.Value = currentFieldNumber
        Session("currentFieldNumber") = currentFieldNumber
        ddlScenarios.Items.Clear()
        ddlScenarios_SelectedIndexChanged()
    End Sub

    Private Sub LoadCrops1()
        If ddlFields.SelectedIndex < 0 Or _fieldsInfo1.Count = 0 Then Exit Sub
        Dim newCrop As CropsData
        If _crops.Count <= 0 Then If Session("crops") Is Nothing Then LoadCrops(_crops, _startInfo.StateAbrev)
        _crops1.Clear()
        Select Case True
            'Forest land
            Case _fieldsInfo1(ddlFields.SelectedIndex).Forestry And Not (_fieldsInfo1(ddlFields.SelectedIndex).Name.Contains(smz) Or _fieldsInfo1(ddlFields.SelectedIndex).Name.Contains(road))
                For i = 0 To _crops.Count - 1
                    If (_crops(i).LuNumber = 28 And _crops(i).additional = True) Or _crops(i).Number = 0 Or (_crops(i).FilterStrip.Contains("FS") And _crops(i).additional = True) Then
                        newCrop = New CropsData
                        newCrop = _crops(i)
                        _crops1.Add(newCrop)
                    End If
                Next
                'Lands different that forestry
            Case Not _fieldsInfo1(ddlFields.SelectedIndex).Forestry
                For i = 0 To _crops.Count - 1
                    If _crops(i).additional = False Or _crops(i).Number = cropRoad Or _crops(i).Number = 0 Or _crops(i).state <> "**" Or _crops(i).Number = cropMixedGrass Then
                        newCrop = New CropsData
                        newCrop = _crops(i)
                        _crops1.Add(newCrop)
                    End If
                Next
                'SMZ land
            Case _fieldsInfo1(ddlFields.SelectedIndex).Forestry And _fieldsInfo1(ddlFields.SelectedIndex).Name.Contains(smz)
                For i = 0 To _crops.Count - 1
                    If Not _crops(i).FilterStrip Is Nothing AndAlso (_crops(i).FilterStrip.Contains("FS") And _crops(i).additional = True) Or _crops(i).Number = 0 Or (_crops(i).LuNumber = 28 And _crops(i).additional = True) Then
                        newCrop = New CropsData
                        newCrop = _crops(i)
                        _crops1.Add(newCrop)
                    End If
                Next
                'Road land
            Case _fieldsInfo1(ddlFields.SelectedIndex).Forestry And _fieldsInfo1(ddlFields.SelectedIndex).Name.Contains(road)
                For i = 0 To _crops.Count - 1
                    If _crops(i).Number = cropRoad Or _crops(i).Number = 0 Then
                        newCrop = New CropsData
                        newCrop = _crops(i)
                        _crops1.Add(newCrop)
                    End If
                Next
        End Select

        ddlCrops.DataTextField = "Name"
        ddlCrops.DataValueField = "Number"
        ddlCrops.DataSource = _crops1
        ddlCrops.DataBind()
        For i = 0 To _crops1.Count - 1
            If _crops1(i).Number <> 387 Then ddlCrops.Items(i).Attributes.Add("PP", _crops1(i).PlantPopulation)
        Next

    End Sub

    Protected Sub ddlScenarios_SelectedIndexChanged()
        currentScenarioNumber = ddlScenarios.SelectedIndex
        Session("currentScenarioNumber") = currentScenarioNumber
        LoadScenarios(ddlScenarios, _fieldsInfo1(currentFieldNumber)._scenariosInfo, currentScenarioNumber)
        LoadScenarios(ddlFromScenario, _fieldsInfo1(currentFieldNumber)._scenariosInfo, currentScenarioNumber)
        LoadAllScenarios(ddlAllScenario, _fieldsInfo1, currentScenarioNumber)
        Select Case ddlScenarios.SelectedIndex
            Case 0
                tdCurrentScenario.Style.Item("background-color") = Drawing.Color.White.Name
            Case 1
                tdCurrentScenario.Style.Item("background-color") = Drawing.Color.LightGreen.Name
            Case 2
                tdCurrentScenario.Style.Item("background-color") = Drawing.Color.Goldenrod.Name
            Case 3
                tdCurrentScenario.Style.Item("background-color") = Drawing.Color.GreenYellow.Name
            Case 4
                tdCurrentScenario.Style.Item("background-color") = Drawing.Color.LawnGreen.Name
            Case 5
                tdCurrentScenario.Style.Item("background-color") = Drawing.Color.LightGoldenrodYellow.Name
            Case 6
                tdCurrentScenario.Style.Item("background-color") = Drawing.Color.LightSteelBlue.Name
            Case 7
                tdCurrentScenario.Style.Item("background-color") = Drawing.Color.LightGray.Name
            Case 8
                tdCurrentScenario.Style.Item("background-color") = Drawing.Color.SlateGray.Name
            Case 9
                tdCurrentScenario.Style.Item("background-color") = Drawing.Color.LightCoral.Name
        End Select
        currentScenarioNumber = ddlScenarios.SelectedIndex
        hdnScenario.Value = currentScenarioNumber
        LoadOperations()
    End Sub

    Protected Sub btnSave_Click(status As Boolean)
        Try
            If _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count <= 0 Then Exit Sub
            lblMessage.Text = ""      'clean messages   
            'AF and AI are exclusive. To avoid problems the first one to run should be the one no active
            If ddlAFIrrigationType.SelectedValue > 0 Then
                SaveAutoIrrigation()
                SaveAutoFertigation()
            Else
                SaveAutoFertigation()
                SaveAutoIrrigation()
            End If
            SaveTileDrain()
            'PND, PPND, PPSD, PPDE, and PPTW are exclusive. To avoid problems the first one to run should be the one no active
            Select Case True
                Case txtPNDFraction.Text > 0
                    SavePadsAndPipesNoDitch()
                    SavePadsAndPipesDitchSystem()
                    SavePadsAndPipesDitchEnlargement()
                    SavePadsAndPipesTailWater()
                    SavePond()
                Case txtPPDEWidth.Text > 0
                    SavePond()
                    SavePadsAndPipesNoDitch()
                    SavePadsAndPipesDitchSystem()
                    SavePadsAndPipesTailWater()
                    SavePadsAndPipesDitchEnlargement()
                Case txtPPDSWidth.Text > 0
                    SavePond()
                    SavePadsAndPipesNoDitch()
                    SavePadsAndPipesDitchEnlargement()
                    SavePadsAndPipesTailWater()
                    SavePadsAndPipesDitchSystem()
                Case txtPPNDWidth.Text > 0
                    SavePond()
                    SavePadsAndPipesDitchSystem()
                    SavePadsAndPipesDitchEnlargement()
                    SavePadsAndPipesTailWater()
                    SavePadsAndPipesNoDitch()
                Case txtPPTWWidth.Text > 0
                    SavePond()
                    SavePadsAndPipesNoDitch()
                    SavePadsAndPipesDitchSystem()
                    SavePadsAndPipesDitchEnlargement()
                    SavePadsAndPipesTailWater()
                Case Else       'the order here does not matter
                    SavePond()
                    SavePadsAndPipesNoDitch()
                    SavePadsAndPipesDitchSystem()
                    SavePadsAndPipesDitchEnlargement()
                    SavePadsAndPipesTailWater()
            End Select
            SaveWetland()
            SaveStreamFencing()
            SaveStreamBankStabilization()
            SaveRiparianForest()
            SaveFilterStrip()   'save filter strip
            SaveWaterWay()
            SaveContourBuffer()
            SaveLandLeveling()
            If chkSA.Checked Then
                SaveTerraceSystem()
                SaveSlopeAdjustment()
            Else
                SaveSlopeAdjustment()
                SaveTerraceSystem()
            End If
            SaveLiming()
            SaveManureControl()
            SaveAsfaltOrConcrete()
            SaveGrassCover()
            SaveClimateChange()
            SaveShading()
            ArrangeInfo("Save")
            'SaveProject(Session("userGuide"), _projects)

            If lblMessage.Text = "" And opError = String.Empty Then
                If status Then
                    showMessage(lblMessage, imgIcon, "Green", "GoIcon", msgDoc.Descendants("InformationSaved").Value)
                End If
            Else
                If opError <> String.Empty Then lblMessage.Text &= "- Operation errors"
                showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", lblMessage.Text)
            End If

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", ex.Message)
        End Try
    End Sub

    Protected Sub btnContinue_Click()
        Session("currentFieldNumber") = currentFieldNumber
        Session("currentScenarioNumber") = currentScenarioNumber
        Response.Redirect("Simulation.aspx", False)
    End Sub

    Private Sub GetCropCode(cropName As String, ByRef cropCode As UShort, ByRef convUnit As Single, ByRef luNumber As UShort)
        convUnit = 0
        cropCode = 0
        luNumber = 0
        Dim crop As CropsData
        crop = _crops.Find(Function(x) x.Name.Trim = cropName.Trim)
        convUnit = crop.ConversionFactor
        cropCode = crop.Number
        luNumber = crop.LuNumber
    End Sub

    Private Sub GetTillCode(tillName As String, ByRef typeCode As UShort, ByRef tillCode As UShort)
        typeCode = 0
        tillCode = 0
        For Each till In _opTypes
            If till.Name.Trim = tillName.Trim Then
                typeCode = till.Number.Split(",")(0)
                tillCode = typeCode
                Exit For
            End If
        Next
    End Sub

    Private Sub SaveOperations()
        Dim msg As String = String.Empty
        ctlSave.Value = "No"
        'if there are not scenarios just continue
        If currentScenarioNumber < 0 Then Exit Sub
        'save just operations either because of save and continue, reorder, delete, or save Cropping System.
        'Dim txtSelected As HtmlInputHidden
        Dim chkSelected As HtmlInputCheckBox
        Dim txtCrop As HtmlInputText
        Dim txtOper As HtmlInputText
        Dim txtYear As HtmlInputText
        Dim txtMonth As HtmlInputText
        Dim txtDay As HtmlInputText
        Dim txtType As HtmlInputText
        Dim txtAmount As HtmlInputText
        Dim txtDepth As HtmlInputText
        Dim txtNo3 As HtmlInputText
        Dim txtPo4 As HtmlInputText
        Dim txtOrgN As HtmlInputText
        Dim txtOrgP As HtmlInputText
        Dim txtK As HtmlInputText
        Dim txtNH3 As HtmlInputText
        Dim operation As OperationsData
        Dim j As UShort = 0

        'currentSoilNumber = 0
        'clear previous operations
        If _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count <= 0 Then Exit Sub
        _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo.Clear()
        For Each Soil In _fieldsInfo1(currentFieldNumber)._soilsInfo
            Soil._scenariosInfo(currentScenarioNumber)._operationsInfo.Clear()
        Next
        For i = 0 To gvOperations.Rows.Count - 1
            operation = New OperationsData
            'update operations information
            operation.Index = i
            operation.EventId = i + 1
            operation.Scenario = ddlScenarios.SelectedItem.Text
            chkSelected = gvOperations.Rows(i).FindControl("chkSelected")
            If chkSelected.Checked = True Then
                Continue For
            End If
            txtCrop = gvOperations.Rows(i).FindControl("txtCrop")
            operation.ApexCropName = txtCrop.Value
            txtOper = gvOperations.Rows(i).FindControl("txtOper")
            operation.ApexOpName = txtOper.Value
            txtType = gvOperations.Rows(i).FindControl("txtType")
            Select Case operation.ApexOpName
                Case "Planting"
                    operation.ApexOpAbbreviation = planting
                    operation.ApexOp = 1
                Case "Fertilizer"
                    operation.ApexOpAbbreviation = fertilizer
                    operation.ApexOp = 2
                    If Not txtType.Value.ToLower.Contains("manure") Then
                        txtType.Value = "Commercial Fertilizer"
                    End If
                Case "Tillage"
                    operation.ApexOpAbbreviation = tillage
                    operation.ApexOp = 3
                Case "Harvest"
                    operation.ApexOpAbbreviation = harvest
                    operation.ApexOp = 4
                    If txtType.Value = "" Or txtType.Value = String.Empty Then txtType.Value = "HARVEST"
                Case "Kill"
                    operation.ApexOpAbbreviation = kill
                    operation.ApexOp = 5
                Case "Irrigation (Manual)"
                    operation.ApexOpAbbreviation = irrigation
                    operation.ApexOp = 6
                Case "Start Grazing"
                    operation.ApexOpAbbreviation = grazing
                    operation.ApexOp = 7
                Case "Stop Grazing"
                    operation.ApexOpAbbreviation = stopGrazing
                    operation.ApexOp = 8
                Case "Burn"
                    operation.ApexOpAbbreviation = burn
                    operation.ApexOp = 9
                Case "Liming"
                    operation.ApexOpAbbreviation = liming
                    operation.ApexOp = 10
            End Select
            GetCropCode(operation.ApexCropName, operation.ApexCrop, operation.ConvertionUnit, operation.LuNumber)
            txtYear = gvOperations.Rows(i).FindControl("txtYear")
            operation.Year = txtYear.Value
            txtMonth = gvOperations.Rows(i).FindControl("txtMonth")
            operation.Month = txtMonth.Value
            txtDay = gvOperations.Rows(i).FindControl("txtDay")
            operation.Day = txtDay.Value
            If operation.ApexOpName = "Planting" Then
                operation.OpVal7 = 0.0
                If operation.ApexCrop = cropMixedGrass Then
                    btnSaveMixedCrops_Click(operation)
                Else
                    operation.setCN(operation.ApexCrop, "", _fieldsInfo1.Count, _fieldsInfo1(currentFieldNumber)._soilsInfo(0).Group, currentFieldNumber, _startInfo, Session("UserGuide"))
                End If
            End If
            If txtType.Value = selectOne Then txtType.Value = ddlTypes.Items(0).Text
            operation.ApexTillName = txtType.Value
            operation.ApexOpTypeName = operation.ApexTillName
            GetTillCode(operation.ApexTillName, operation.ApexOpType, operation.ApexTillCode)
            txtAmount = gvOperations.Rows(i).FindControl("txtAmount")
            operation.ApexOpv1 = txtAmount.Value
            txtDepth = gvOperations.Rows(i).FindControl("txtDepth")
            operation.ApexOpv2 = txtDepth.Value
            Select Case txtOper.Value
                Case "Fertilizer"
                    operation.ApexTillCode = 580
                Case "Start Grazing"
                    operation.ApexTillCode = 426
                    operation.setAnimalRate(operation.ApexOpType, _fieldsInfo1(currentFieldNumber).Area)
                Case "Kill"
                    operation.ApexTillCode = 451
                Case "Burn"
                    operation.ApexTillCode = 397
                Case "Stop Grazing"
                    operation.ApexTillCode = 427
                Case "Liming"
                    operation.ApexTillCode = 734
                Case Else
                    operation.ApexTillCode = operation.ApexOpType
            End Select
            txtNo3 = gvOperations.Rows(i).FindControl("txtNo3")
            operation.NO3 = txtNo3.Value
            txtPo4 = gvOperations.Rows(i).FindControl("txtPO4")
            operation.PO4 = txtPo4.Value
            txtOrgN = gvOperations.Rows(i).FindControl("txtOrgN")
            operation.OrgN = txtOrgN.Value
            txtOrgP = gvOperations.Rows(i).FindControl("txtOrgP")
            operation.OrgP = txtOrgP.Value
            txtK = gvOperations.Rows(i).FindControl("txtK")
            operation.K = txtK.Value
            txtNH3 = gvOperations.Rows(i).FindControl("txtNH3")
            operation.NH3 = txtNH3.Value
            _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber).AddOperation(operation)
            For Each Soil In _fieldsInfo1(currentFieldNumber)._soilsInfo
                If operation.ApexOpName = "Planting" Then operation.setCN(operation.ApexCrop, "", _fieldsInfo1.Count, Soil.Group, currentFieldNumber, _startInfo, Session("UserGuide"))
                Soil._scenariosInfo(currentScenarioNumber).AddOperation(operation)
                If operation.ApexTillCode = 426 Then operation.setAnimalRate(operation.ApexOpType, Soil.Percentage * _fieldsInfo1(currentFieldNumber).Area / 100)
            Next
        Next

        LoadOperations()
    End Sub

    Private Sub btnUpload_Click1()
        Dim doc As System.Xml.Linq.XDocument = Nothing

        Try
            flUpload.PostedFile.SaveAs(folder & "\App_xml\" & Session("userGuide") & ".xml")
            doc = System.Xml.Linq.XDocument.Load(folder & "\App_xml\" & Session("userGuide") & ".xml")
            Dim project As New Project

            _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo.Clear()
            For Each soil In _fieldsInfo1(currentFieldNumber)._soilsInfo
                soil._scenariosInfo(currentScenarioNumber)._operationsInfo.Clear()
            Next

            For Each operation In doc.Descendants("ScenarioInfo").Descendants("Operations")  'todo activate this two load functions
                loadOperationInfo(operation, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo, _fieldsInfo1(currentFieldNumber).Area)

                For Each Soil In _fieldsInfo1(currentFieldNumber)._soilsInfo
                    loadOperationInfo(operation, Soil._scenariosInfo(currentScenarioNumber)._operationsInfo, Soil.Percentage * _fieldsInfo1(currentFieldNumber).Area)     '_soil.Percentage * Field.Area
                Next
            Next

            LoadOperations()
            'ShowModal()
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Private Sub btnUpload_Click()
        Dim croppingOperations = GetCroppingOperations(ddlCroppingSystemsAll.Items(ddlCroppingSystemsAll.SelectedIndex).Value, _startInfo.StateAbrev)
        Dim list As New List(Of OperationsData)
        Dim opVal1 As Single
        Dim cropNumber As Short = 0
        Dim orgN As Single = 0 : Dim no3 As Single = 0 : Dim orgP As Single = 0 : Dim po4 As Single = 0 : Dim nh3 As Single = 0, k As Single = 0
        Dim opval2 As Single
        Dim period As UShort
        Dim apexOpType As Short
        Dim oper As UShort

        If Not croppingOperations.HasRows Then
            croppingOperations = GetCroppingOperations(ddlCroppingSystemsAll.Items(ddlCroppingSystemsAll.SelectedIndex).Value, "ALL")
            If Not croppingOperations.HasRows Then
                showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("NoOperations").Value & " - State:" & _startInfo.StateAbrev & " - Cropping: " & ddlCroppingSystemsAll.Items(ddlCroppingSystemsAll.SelectedIndex).Value)
            End If
        End If

        _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo.Clear()
        For Each soil In _fieldsInfo1(currentFieldNumber)._soilsInfo
            soil._scenariosInfo(currentScenarioNumber)._operationsInfo.Clear()
        Next

        For Each c In croppingOperations
            opVal1 = 0
            opval2 = 0
            cropNumber = c.item("APEXCrop")
            If Not c.item("APEXOpv2") Is DBNull.Value AndAlso c.item("APEXOpv2").trim <> String.Empty Then opval2 = c.item("APEXOpv2")
            Select Case c.item("EVENTTYPE")
                Case planting
                    apexOpType = c.item("APEXOp")
                    oper = 1
                    Dim crops = (From crops1 In _crops Where crops1.Number = cropNumber Select crops1.PlantPopulation)
                    For Each Val1 In crops
                        opVal1 = Val1
                        Exit For
                    Next
                Case fertilizer
                    oper = 2
                    apexOpType = c.item("APEXFert")
                    opVal1 = c.item("VAR9")
                    If c.item("var2") > "0" Or c.item("var3") > "0" Or c.item("var4") > "0" Or c.item("var5") > "0" Or c.item("var6") > "0" Then
                        orgN = c.item("var2")
                        no3 = c.item("var3")
                        orgP = c.item("var4")
                        po4 = c.item("var5")
                        nh3 = c.item("var6")
                    Else
                        Dim ferts = From ferts1 In _ferts Where ferts1.Number = apexOpType.ToString Select ferts1
                        For Each val1 In ferts
                            orgN = val1.OrgN
                            no3 = val1.No3
                            orgP = val1.OrgP
                            po4 = val1.Po4
                            nh3 = val1.Nh3
                            k = val1.K
                        Next
                        If apexOpType = 1 Then no3 = 1
                        If apexOpType = 2 Then apexOpType = 1 : po4 = 1
                    End If
                Case irrigation
                    oper = 6
                    opVal1 = c.item("APEXOpv1")
                    opval2 = c.item("APEXOpv2")
                Case tillage
                    oper = 3
                    apexOpType = c.item("APEXOp")
                Case harvest
                    oper = 4
                    Dim crops = (From crops1 In _crops Where crops1.Number = cropNumber And crops1.state = _startInfo.StateAbrev Select crops1.HarvestCode)
                    If crops.Count = 0 Then
                        crops = (From crops1 In _crops Where crops1.Number = cropNumber And crops1.state = "**" Select crops1.HarvestCode)
                    End If
                    'apexOpType = c.item("APEXOp")
                    apexOpType = crops.First
                Case (kill)
                    oper = 5
                    apexOpType = c.item("APEXOp")
                Case grazing
                    oper = 7
                    'todo get the animal type here.
                    'apexOpType = c.APEXOp
                    'todo check what goes here in apexopv1 for grazing
                    opVal1 *= c.item("APEXOpv1")
                Case stopGrazing
                    oper = 8
                    apexOpType = c.item("APEXOp")
                Case burn
                    apexOpType = c.item("APEXOp")
                    oper = 9
                Case liming
                    apexOpType = c.item("APEXOp")
                    opVal1 = c.item("APEXOpv1")
                    oper = 10
            End Select
            Select Case c.item("DAY")
                Case Is > 15
                    period = 2
                Case Is = 0
                    period = 0
                Case Is <= 15
                    period = 1
            End Select
            If c.item("EVENTTYPE") = harvest Then
                list.Add(New OperationsData With {.EventId = c.item("EVENTID"), .ApexCropName = c.item("CropName").Trim, .ApexOp = oper, .ApexOpAbbreviation = c.item("EVENTTYPE"), .ApexCrop = c.item("APEXCrop"), .ApexOpName = c.item("OperationName").Trim, .Year = c.item("YEAR"), .Month = c.item("MONTH"), .Day = c.item("DAY"), .Period = period, .ApexTillCode = apexOpType, .ApexTillName = c.item("TillName").Trim, .ApexOpType = apexOpType, .ApexOpv1 = opVal1, .ApexOpv2 = opval2, .OrgN = orgN, .NO3 = no3, .OrgP = orgP, .PO4 = po4, .NH3 = nh3, .K = k})
                If list(list.Count - 1).ApexOpAbbreviation = grazing Then list(list.Count - 1).setAnimalRate(c.item("APEXOp"), _fieldsInfo1(currentFieldNumber).Area)
            Else
                list.Add(New OperationsData With {.EventId = c.item("EVENTID"), .ApexCropName = c.item("CropName").Trim, .ApexOp = oper, .ApexOpAbbreviation = c.item("EVENTTYPE"), .ApexCrop = c.item("APEXCrop"), .ApexOpName = c.item("OperationName").Trim, .Year = c.item("YEAR"), .Month = c.item("MONTH"), .Day = c.item("DAY"), .Period = period, .ApexTillCode = c.item("APEXOp"), .ApexTillName = c.item("TillName").Trim, .ApexOpType = apexOpType, .ApexOpv1 = opVal1, .ApexOpv2 = opval2, .OrgN = orgN, .NO3 = no3, .OrgP = orgP, .PO4 = po4, .NH3 = nh3, .K = k})
                If list(list.Count - 1).ApexOpAbbreviation = grazing Then list(list.Count - 1).setAnimalRate(c.item("APEXOp"), _fieldsInfo1(currentFieldNumber).Area)
            End If
            orgN = 0
            orgP = 0
            no3 = 0
            po4 = 0
            nh3 = 0
            k = 0
        Next

        _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber).AddOperations(list, currentScenarioNumber)

        For Each Soil In _fieldsInfo1(currentFieldNumber)._soilsInfo
            Soil._scenariosInfo(currentScenarioNumber).AddOperations(list, currentScenarioNumber)
        Next

        LoadOperations()
        'ShowModal()
    End Sub

    'Protected Sub btnOpenCropping_Click(sender As Object, e As System.EventArgs) Handles btnOpenCropping.Click
    '    Dim doc As System.Xml.Linq.XDocument = Nothing

    '    Try
    '        Me.Uploader.PostedFile.SaveAs(folder & "\App_xml\" & Session("userGuide") & ".xml")
    '        doc = System.Xml.Linq.XDocument.Load(folder & "\App_xml\" & Session("userGuide") & ".xml")
    '        Dim project As New Project

    '        _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo.Clear()
    '        For Each soil In _fieldsInfo1(currentFieldNumber)._soilsInfo
    '            soil._scenariosInfo(currentScenarioNumber)._operationsInfo.Clear()
    '        Next

    '        For Each operation In doc.Descendants("ScenarioInfo").Descendants("Operations")  'todo activate this two load functions
    '            'project.loadOperationInfo(operation, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber), 0)

    '            For Each Soil In _fieldsInfo1(currentFieldNumber)._soilsInfo
    '                'project.loadOperationInfo(operation, Soil._scenariosInfo(currentScenarioNumber), Soil.Percentage)
    '            Next
    '        Next

    '        LoadOperations()
    '        ShowModal()
    '    Catch ex As Exception
    '        showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
    '    End Try
    'End Sub

    Protected Sub btnCopyCropping_Click()
        Try
            If ddlFromField.SelectedIndex = ddlFields.SelectedIndex And ddlFromScenario.SelectedIndex = ddlScenarios.SelectedIndex Then
                showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("SameCopyMessage").Value)
                Exit Sub
            End If
            _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo.Clear()

            _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber).AddOperations(_fieldsInfo1(ddlFromField.SelectedIndex)._scenariosInfo(ddlFromScenario.SelectedIndex)._operationsInfo, currentScenarioNumber)
            For Each Soil In _fieldsInfo1(currentFieldNumber)._soilsInfo
                Soil._scenariosInfo(currentScenarioNumber)._operationsInfo.Clear()
                Soil._scenariosInfo(currentScenarioNumber).AddOperations(_fieldsInfo1(ddlFromField.SelectedIndex)._scenariosInfo(ddlFromScenario.SelectedIndex)._operationsInfo, currentScenarioNumber)
            Next

            LoadOperations()
            'ShowModal()
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Protected Sub btnSaveCropRotation_Click()
        Dim sFile As String = String.Empty
        Dim swFile As IO.StreamWriter = Nothing
        Dim output As StringBuilder = New StringBuilder()

        Try
            CreateXMLFile(output)   'Save the Cropping System to a xml file
            sFile = folder + "\" + NTTxmlFolder + "\" + Session("userGuide") + ".opr"
            swFile = New IO.StreamWriter(sFile)
            swFile.Write(output)
            swFile.Close()
            swFile.Dispose()
            swFile = Nothing
            If IO.File.Exists(sFile) Then
                Response.Clear()
                Response.ContentType = "text/xml"
                Response.AddHeader("Content-Disposition", "attachment;filename=NTTCropRotationFile.opr")
                Response.WriteFile(sFile)
                Response.End()
            End If

            showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("ProjectSaved").Value)

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value)
        End Try

    End Sub

    Private Sub CreateXMLFile(ByRef output As StringBuilder)
        Try
            If _startInfo.projectName Is Nothing Or _startInfo.projectName = String.Empty Then
                showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("ProjectNotCreated").Value)
                Exit Sub
            End If
            output.Length = 0
            output.Append("<?xml version='1.0'?>" & vbCr)
            output.Append("<ScenarioInfo>" & vbCr)
            'Dim project As New Project
            AddOperationsInfo(_fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo, output)
            output.Append("</ScenarioInfo>" & vbCr)
        Catch e1 As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & e1.Message)
        End Try
    End Sub

    Protected Sub AddHeaderRow()
        Dim row As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)
        Dim cell1 As New TableHeaderCell
        Dim cell2 As New TableHeaderCell
        cell2.Text = ""
        cell2.Text = cntDoc.Descendants("NutrientComposition").Value
        cell1.ColumnSpan = 9
        cell2.ColumnSpan = 5

        row.Cells.AddRange(New TableCell() {cell1, cell2})
        gvOperations.Controls(0).Controls.AddAt(0, row)
    End Sub

    Private Sub gvOperations_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvOperations.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            AddHeaderRow()
        End If
    End Sub

    Private Sub gvOperations_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvOperations.RowDataBound
        If e.Row.DataItem Is Nothing Then Exit Sub

        If e.Row.RowType = DataControlRowType.DataRow Then
            'asign a client function on row click
            e.Row.Attributes.Add("onclick", "onClick(" & e.Row.RowIndex.ToString() & ")")
            e.Row.Attributes.Add("RowNumber", e.Row.RowIndex)
            e.Row.Attributes.Add("mixedCrops", e.Row.DataItem.MixedCropData)
            'e.Row.Cells(1).Attributes.Add("onfocus", "onBlur(" & e.Row.RowIndex.ToString() & ", 1)")
            'e.Row.Cells(2).Attributes.Add("onfocus", "onBlur(" & e.Row.RowIndex.ToString() & ", 2)")
            'e.Row.Cells(3).Attributes.Add("onfocus", "onBlur(" & e.Row.RowIndex.ToString() & ", 3)")
            'e.Row.Cells(4).Attributes.Add("onfocus", "onBlur(" & e.Row.RowIndex.ToString() & ", 4)")
            'e.Row.Cells(5).Attributes.Add("onfocus", "onBlur(" & e.Row.RowIndex.ToString() & ", 5)")
            'e.Row.Cells(6).Attributes.Add("onfocus", "onBlur(" & e.Row.RowIndex.ToString() & ", 6)")
            Dim txtCrop As HtmlInputText = e.Row.FindControl("txtCrop")
            If Not txtCrop Is Nothing Then
                txtCrop.Attributes.Add("CropValue", e.Row.DataItem.ApexCrop)
                Dim pp As Single = 0
                If Not ddlCrops.DataSource Is Nothing Then
                    For i = 0 To ddlCrops.Items.Count - 1
                        If ddlCrops.DataSource(i).Number = e.Row.DataItem.ApexCrop Then pp = ddlCrops.DataSource(i).plantpopulation : Exit For
                    Next
                End If
                txtCrop.Attributes.Add("CropPP", pp)
            End If

            Dim txtOper As HtmlInputText = e.Row.FindControl("txtOper")
            If Not txtOper Is Nothing Then
                txtOper.Attributes.Add("OperValue", e.Row.DataItem.ApexOp)
                txtOper.Attributes.Add("OperAbbrev", e.Row.DataItem.ApexOpAbbreviation)
            End If

            Dim txtType As HtmlInputText = e.Row.FindControl("txtType")
            If Not txtType Is Nothing Then
                txtType.Attributes.Add("TypeValue", e.Row.DataItem.ApexOpType)
            End If

            ValidateOperationInputs(e.Row)
        End If
    End Sub
    Private Sub ValidateOperationInputs(e As GridViewRow)

        Dim chkSelected As HtmlInputCheckBox = e.FindControl("chkSelected")
        Dim txtCrop As HtmlInputText = e.FindControl("txtCrop")
        Dim txtOper As HtmlInputText = e.FindControl("txtOper")
        Dim txtYear As HtmlInputText = e.FindControl("txtYear")
        Dim txtMonth As HtmlInputText = e.FindControl("txtMonth")
        Dim txtDay As HtmlInputText = e.FindControl("txtDay")
        Dim txtType As HtmlInputText = e.FindControl("txtType")
        Dim txtAmount As HtmlInputText = e.FindControl("txtAmount")
        Dim txtDepth As HtmlInputText = e.FindControl("txtDepth")
        Dim txtNO3 As HtmlInputText = e.FindControl("txtNO3")
        Dim txtPO4 As HtmlInputText = e.FindControl("txtPO4")
        Dim txtOrgN As HtmlInputText = e.FindControl("txtOrgN")
        Dim txtOrgP As HtmlInputText = e.FindControl("txtOrgP")
        Dim txtK As HtmlInputText = e.FindControl("txtK")
        Dim txtNH3 As HtmlInputText = e.FindControl("txtNH3")

        chkSelected.Attributes.Add("onfocus", "return onBlur(" & e.RowIndex.ToString() & ", 0)")
        txtCrop.Attributes.Add("onfocus", "return onBlur(" & e.RowIndex.ToString() & ", 1)")
        txtOper.Attributes.Add("onfocus", "return onBlur(" & e.RowIndex.ToString() & ", 2)")
        txtYear.Attributes.Add("onfocus", "return onBlur(" & e.RowIndex.ToString() & ", 3)")
        txtMonth.Attributes.Add("onfocus", "return onBlur(" & e.RowIndex.ToString() & ", 4)")
        txtDay.Attributes.Add("onfocus", "return onBlur(" & e.RowIndex.ToString() & ", 5)")
        txtType.Attributes.Add("onfocus", "return onBlur(" & e.RowIndex.ToString() & ", 6)")
        txtAmount.Attributes.Add("onfocus", "return onBlur(" & e.RowIndex.ToString() & ", 7)")
        txtDepth.Attributes.Add("onfocus", "return onBlur(" & e.RowIndex.ToString() & ", 8)")
        txtNO3.Attributes.Add("onfocus", "return onBlur(" & e.RowIndex.ToString() & ", 9)")
        txtPO4.Attributes.Add("onfocus", "return onBlur(" & e.RowIndex.ToString() & ", 10)")
        txtOrgN.Attributes.Add("onfocus", "return onBlur(" & e.RowIndex.ToString() & ", 11)")
        txtOrgP.Attributes.Add("onfocus", "return onBlur(" & e.RowIndex.ToString() & ", 12)")
        txtK.Attributes.Add("onfocus", "return onBlur(" & e.RowIndex.ToString() & ", 13)")
        txtNH3.Attributes.Add("onfocus", "return onBlur(" & e.RowIndex.ToString() & ", 14)")

        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo(e.RowIndex)
            Select Case .ApexOpAbbreviation
                Case fertilizer
                    If .ApexOpv1 <= 0 Then txtAmount.Style.Item("background-color") = "Red" : opError = "Error"
                    If .ApexOpv2 < 0 Then txtDepth.Style.Item("background-color") = "Red" : opError = "Error"
                    If .NO3 < 0 Or .NO3 > FractionMax Then txtNO3.Style.Item("background-color") = "Red" : opError = "Error"
                    If .PO4 < 0 Or .PO4 > FractionMax Then txtPO4.Style.Item("background-color") = "Red" : opError = "Error"
                    If .OrgN < 0 Or .OrgN > FractionMax Then txtOrgN.Style.Item("background-color") = "Red" : opError = "Error"
                    If .OrgP < 0 Or .OrgP > FractionMax Then txtOrgP.Style.Item("background-color") = "Red" : opError = "Error"
                    If .K < 0 Or .K > FractionMax Then txtK.Style.Item("background-color") = "Red" : opError = "Error"
                    If .NO3 + .PO4 + .OrgN + .OrgP + .K > FractionMax Then
                        txtNO3.Style.Item("background-color") = "Red"
                        txtPO4.Style.Item("background-color") = "Red"
                        txtOrgN.Style.Item("background-color") = "Red"
                        txtOrgP.Style.Item("background-color") = "Red"
                        txtK.Style.Item("background-color") = "Red"
                        opError = "Error"
                    End If
                Case irrigation
                    If .ApexOpv1 <= 0 Then txtAmount.Style.Item("background-color") = "Red" : opError = "Error"
                    If .ApexOpv2 <= 0 Then txtDepth.Style.Item("background-color") = "Red" : opError = "Error"
                Case planting
                    If .ApexOpv1 < 0 Then txtAmount.Style.Item("background-color") = "Red" : opError = "Error"
                Case grazing
                    If .ApexOpv1 <= 0 Then txtAmount.Style.Item("background-color") = "Red" : opError = "Error"
                    If .ApexOpv2 < SFHoursMin Or .ApexOpv2 > SFHoursMax Then txtDepth.Style.Item("background-color") = "Red" : opError = "Error"
                    If .NO3 < 0 Or .NO3 > FractionMax Then txtNO3.Style.Item("background-color") = "Red" : opError = "Error"
                    If .PO4 < 0 Or .PO4 > FractionMax Then txtPO4.Style.Item("background-color") = "Red" : opError = "Error"
                    If .OrgN < 0 Or .OrgN > FractionMax Then txtOrgN.Style.Item("background-color") = "Red" : opError = "Error"
                    If .OrgP < 0 Or .OrgP > FractionMax Then txtOrgP.Style.Item("background-color") = "Red" : opError = "Error"
                    If .K < 0 Or .K > FractionMax Then txtK.Style.Item("background-color") = "Red" : opError = "Error"
                    If .NO3 + .PO4 + .OrgN + .OrgP + .K > FractionMax Then
                        txtNO3.Style.Item("background-color") = "Red"
                        txtPO4.Style.Item("background-color") = "Red"
                        txtOrgN.Style.Item("background-color") = "Red"
                        txtOrgP.Style.Item("background-color") = "Red"
                        txtK.Style.Item("background-color") = "Red"
                        opError = "Error"
                    End If
                Case liming
                    If .ApexOpv1 <= 0 Then txtAmount.Style.Item("background-color") = "Red" : opError = "Error"
            End Select
        End With
    End Sub
    Private Sub btnAddOperation_ServerClick()
        Dim newOper As New OperationsData
        If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo.Count > 0 Then
            newOper.ApexCrop = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo(_fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo.Count - 1).ApexCrop
            newOper.ApexCropName = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo(_fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo.Count - 1).ApexCropName
        Else
            newOper.ApexCrop = ddlCrops.Items(ddlCrops.SelectedIndex).Value
            newOper.ApexCropName = ddlCrops.Items(ddlCrops.SelectedIndex).Text
        End If
        _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber).AddOperation(newOper)
        LoadOperations()
    End Sub

    Private Sub btnDeleteOperation_ServerClick()
        'ShowModal()
    End Sub

    'Private Sub ShowModal()
    '    dvOperations.Style.Item("display") = ""
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Modal", "showModal();", True)
    'End Sub

    Private Sub updateBmp(type As String, value As String)
        Dim _subareaInfo As SubareasData = Nothing
        Dim nirr As String = "  00"

        If type = "nirr" Or type = "afnirr" Then
            Select Case value
                Case 1
                    nirr = "  01"
                Case 2
                    nirr = "  02"
                Case 3
                    nirr = "  05"
                Case 7
                    nirr = "  02"
                Case 8
                    nirr = "  02"
            End Select
        End If

        For i = 0 To _fieldsInfo1(currentFieldNumber)._soilsInfo.Count - 1
            With _fieldsInfo1(currentFieldNumber)._soilsInfo(i)._scenariosInfo(currentScenarioNumber)._subareasInfo
                Select Case type
                    'irrigation and fertigation variables
                    Case "nirr", "afnirr"     'irrigation type
                        If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AIType = 0 And _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFType = 0 Then
                            ._line8(0).Idf4 = 0
                            ._line9(0).Bft = 0
                            ._line8(0).Nirr = 0
                            ._line9(0).Vimx = 0
                            ._line9(0).Bir = 0
                            ._line9(0).Armx = 0
                        Else
                            ._line8(0).Nirr = nirr
                            ._line9(0).Vimx = vimxDefault
                            If ._line9(0).Bir = 0 Then ._line9(0).Bir = 0.8
                            If ._line9(0).Armx = 0 Then ._line9(0).Armx = armxDefault * in_to_mm
                            If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFType > 0 Then
                                ._line8(0).Idf4 = 1
                                ._line9(0).Bft = 0.8
                            End If
                        End If
                    Case "iri", "afiri"      'irrigation frequency
                        If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AIType = 0 And _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFType = 0 Then
                            ._line8(0).Iri = 0
                        Else
                            ._line8(0).Iri = value
                        End If
                    Case "bir", "afbir"      'water stress
                        If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AIType = 0 And _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFType = 0 Then
                            ._line9(0).Bir = 0
                        Else
                            ._line9(0).Bir = value
                        End If
                    Case "efi", "afefi"      'irrigation efficiency
                        If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AIType = 0 And _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFType = 0 Then
                            ._line9(0).Efi = 0
                        Else
                            ._line9(0).Efi = 1 - value
                        End If
                    Case "armx", "afarmx"      'irrigation maximium single applciation"
                        If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AIType = 0 And _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFType = 0 Then
                            ._line9(0).Armx = 0
                        Else
                            ._line9(0).Armx = value * in_to_mm
                        End If
                    Case "fdsf", "affdsf"      'irrigation safety factor for furrow dike
                        If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AIType = 0 And _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFType = 0 Then
                            ._line9(0).Fdsf = 0
                        Else
                            ._line9(0).Fdsf = value
                        End If
                        'tile drain
                    Case "idr"      'Tile Drain Depth in mm
                        ._line8(0).Idr = value * ft_to_mm
                        If value > 0 Then
                            ._line9(0).Drt = drt
                        Else
                            ._line9(0).Drt = 0
                        End If
                        'ponds, pads and pipes
                    Case "pcof", "pcofPP"      'fraction of subarea controlled by ponds
                        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
                            If .PPDEWidth = 0 And .PPDSWidth = 0 And .PPNDWidth = 0 And .PPTWWidth = 0 And .PndF = 0 Then
                                _fieldsInfo1(currentFieldNumber)._soilsInfo(i)._scenariosInfo(currentScenarioNumber)._subareasInfo._line7(0).Pcof = 0
                            Else
                                _fieldsInfo1(currentFieldNumber)._soilsInfo(i)._scenariosInfo(currentScenarioNumber)._subareasInfo._line7(0).Pcof = value
                            End If
                        End With
                    Case "AoC", "GC", "SA", "ts", "sbs"
                        If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Gc = True Then
                            If ._line5(0).Rchc > 0.01 Then ._line5(0).Rchc = 0.01
                        Else
                            ._line5(0).Rchc = _fieldsInfo1(currentFieldNumber).RchcVal
                        End If
                        If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AoC = True Then
                            If ._line5(0).Rchk > 0.01 Then ._line5(0).Rchk = 0.01
                            If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AoC = True Then ._line5(0).Rchn = 0.05
                        Else
                            ._line5(0).Rchk = _fieldsInfo1(currentFieldNumber).RchkVal
                            ._line5(0).Rchn = 0.0
                        End If
                        If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AoC = True Or _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Gc = True Then
                            If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AoC = True Then ._line4(0).Upn = 0.1
                            If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Gc = True Then ._line4(0).Upn = 0.4
                        Else
                            ._line4(0).Upn = 0
                        End If
                        If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Ts = True Or _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Sa = True _
                         Or _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Gc = True Or _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AoC = True _
                         Or _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Sbs = True Or (_fieldsInfo1(currentFieldNumber).Name.Contains("Road") And _fieldsInfo1(currentFieldNumber).Forestry) Then
                            If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Sbs = True Then ._line10(0).Pec = 0.85
                            If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AoC = True Then ._line10(0).Pec = 0.05
                            If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Gc = True Then ._line10(0).Pec = 0.1
                            If _fieldsInfo1(currentFieldNumber).Name.Contains("Road") And _fieldsInfo1(currentFieldNumber).Forestry Then ._line10(0).Pec = 0.0
                            If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Ts = True Or _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Sa = True Then
                                'Determine PEC for Contour Buffer Strip and TS depeding on table manual APEX0604 PAGE 49.
                                Select Case ._line4(0).Slp
                                    Case Is <= 0.02
                                        ._line10(0).Pec = 0.6
                                    Case Is <= 0.08
                                        ._line10(0).Pec = 0.5
                                    Case Is <= 0.12
                                        ._line10(0).Pec = 0.6
                                    Case Is <= 0.16
                                        ._line10(0).Pec = 0.7
                                    Case Is <= 0.2
                                        ._line10(0).Pec = 0.8
                                    Case Is <= 0.25
                                        ._line10(0).Pec = 0.9
                                    Case Else
                                        ._line10(0).Pec = 1.0
                                End Select
                            End If
                        Else
                            ._line10(0).Pec = 1
                        End If
                    Case "llslope"
                        ._line4(0).Slp = _fieldsInfo1(currentFieldNumber)._soilsInfo(i).Slope / 100 - (_fieldsInfo1(currentFieldNumber)._soilsInfo(i).Slope / 100 * value / 100)
                    Case "lm"
                        ._line8(0).Lm = value
                End Select
            End With
        Next
    End Sub

    Private Sub SaveAutoIrrigation()
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetAI.Style.Item("display") = "none"
            msgs = "Auto Irrigation: "
            .AIType = ddlAIIrrigationType.SelectedValue
            .AISafetyFactor = txtAISafetyFactor.Text
            updateBmp("nirr", .AIType)
            ValidateBMPValue(txtAIIrrigationEfficiancy, .AIEff, msgDoc.Descendants("EfficiencyMessage").Value.Replace("First", FractionMin).Replace("Second", FractionMax) & Microsoft.VisualBasic.ControlChars.CrLf, "efi", FractionMin, FractionMax)
            ValidateBMPValue(txtAIWaterStress, .AIWaterStressFactor, msgDoc.Descendants("WaterStressFactorMsg").Value & Microsoft.VisualBasic.ControlChars.CrLf, "bir", FractionMin, FractionMax)
            ValidateBMPValue(txtAIFrequency, .AIFreq, (msgDoc.Descendants("FrequencyMessage").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "iri", ValueMin, ValueMax)
            ValidateBMPValue(txtAIMaxSingleAppl, .AIMaxSingleApp, (msgDoc.Descendants("ApplicationMessage").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "armx", ValueMin, ValueMax)
            If .AIType = 7 Then
                txtAISafetyFactor.Text = "0.5"
                .AISafetyFactor = txtAISafetyFactor.Text
                ValidateBMPValue(txtAISafetyFactor, .AISafetyFactor, (msgDoc.Descendants("SafetyFactorMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "fdsf", ValueMin, ValueMax)
            End If
            If .AIType = 8 Then ValidateBMPValue(txtAIResArea, .AIResArea, "Tailwater Autoirrigatión reservoir area should be numeric between " & ValueMin & "-" & ValueMax & ". Please change the value" & Microsoft.VisualBasic.ControlChars.CrLf, "resArea", ValueMin, ValueMax)
            If .AIType <> 0 Or .AIEff > 0 Or .AIFreq > 0 Then
                'if this is true check if type is >0. If not add error.
                If .AIType <= 0 Then ValidateLoadBMPDll(ddlAIIrrigationType, msgDoc.Descendants("IrrigationTypeMsg").Value)
                fsetAI.Style.Item("display") = ""
                If Not msgs = "Auto Irrigation: " Then lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red : Exit Sub
                If .AIResArea > 0 Then createBuffer("AITW", .AIResArea, 0, 0, 0, _fieldsInfo1, currentFieldNumber, currentScenarioNumber)
            Else
                deleteBuffer("AITW", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
            End If
        End With
    End Sub

    Private Sub SaveAutoFertigation()
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetAF.Style.Item("display") = "none"
            msgs = "Auto Fertigation: "
            .AFType = ddlAFIrrigationType.SelectedValue
            .AFSafetyFactor = txtAFSafetyFactor.Text
            .AFNConc = txtAFNConc.Text
            updateBmp("afnirr", .AFType)
            ValidateBMPValue(txtAFIrrigationEfficiancy, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFEff, msgDoc.Descendants("EfficiencyMessage").Value.Replace("First", FractionMin).Replace("Second", FractionMax) & Microsoft.VisualBasic.ControlChars.CrLf, "efi", FractionMin, FractionMax)
            ValidateBMPValue(txtAFWaterStress, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFWaterStressFactor, msgDoc.Descendants("WaterStressFactorMsg").Value & Microsoft.VisualBasic.ControlChars.CrLf, "bir", FractionMin, FractionMax)
            ValidateBMPValue(txtAFFrequency, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFFreq, (msgDoc.Descendants("FrequencyMessage").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "iri", ValueMin, ValueMax)
            ValidateBMPValue(txtAFMaxSingleAppl, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFMaxSingleApp, (msgDoc.Descendants("ApplicationMessage").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "armx", ValueMin, ValueMax)
            If .AFType = 7 Then
                txtAFSafetyFactor.Text = "0.5"
                .AFSafetyFactor = txtAFSafetyFactor.Text
                ValidateBMPValue(txtAFSafetyFactor, .AFSafetyFactor, (msgDoc.Descendants("SafetyFactorMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "fdsf", ValueMin, ValueMax)
            End If
            If .AFType <> 0 Or .AFEff > 0 Or .AFFreq > 0 Then
                If .AFType <= 0 Then ValidateLoadBMPDll(ddlAFIrrigationType, msgDoc.Descendants("IrrigationTypeMsg").Value)
                fsetAF.Style.Item("display") = ""
                If Not msgs = "Auto Fertigation: " Then lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red
            End If
        End With
    End Sub

    Private Sub SaveTileDrain()
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetTD.Style.Item("display") = "none"
            msgs = "Tile Drain: "
            ValidateBMPValue(txtTDDepth, .TileDrainDepth, (msgDoc.Descendants("TileDrainDepthMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "idr", ValueMin, ValueMax)
            If .TileDrainDepth = 0 Then Exit Sub
            fsetTD.Style.Item("display") = ""
            If Not msgs = "Tile Drain: " Then lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red
        End With
    End Sub

    Private Sub SavePadsAndPipesTailWater()
        'Pads and Pipes TailWater
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetPPTW.Style.Item("display") = "none"
            msgs = "TailWater: "
            ValidateBMPValue(txtPPTWSides, .PPTWSides, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", sidesMin).Replace("Second", sidesMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "", sidesMin, sidesMax)
            ValidateBMPValue(txtPPTWWidth, .PPTWWidth, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "pcofPP", ValueMin, ValueMax)
            ValidateBMPValue(txtPPTWResArea, .PPTWResArea, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "resArea", ValueMin, ValueMax)
            If .AIType = 0 Then
                msgs &= "You need to include Autoirrigation with this BMP"
            End If
            If .PPTWWidth > 0 Or .PPTWSides > 0 Or .PPTWResArea > 0 Then
                fsetPPTW.Style.Item("display") = ""
                If Not msgs = "TailWater: " Then lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red : Exit Sub
                If .PPTWResArea > 0 Then createBuffer("PPTW", .PPTWResArea, 0, 0, 0, _fieldsInfo1, currentFieldNumber, currentScenarioNumber)
            Else
                deleteBuffer("PPTW", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
            End If
        End With
    End Sub

    Private Sub SavePadsAndPipesDitchEnlargement()
        'Pads and Pipes Ditch Enlargement
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetPPDE.Style.Item("display") = "none"
            msgs = "Ditch Enlargement: "
            ValidateBMPValue(txtPPDESides, .PPDESides, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", sidesMin).Replace("Second", sidesMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "", sidesMin, sidesMax)
            ValidateBMPValue(txtPPDEWidth, .PPDEWidth, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "pcofPP", ValueMin, ValueMax)
            ValidateBMPValue(txtPPDEResArea, .PPDEResArea, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "resArea", ValueMin, ValueMax)
            If .PPDEWidth > 0 Or .PPDESides > 0 Or .PPDEResArea > 0 Then
                fsetPPDE.Style.Item("display") = ""
                If Not msgs = "Ditch Enlargement: " Then lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red : Exit Sub
                If .PPDEResArea > 0 Then createBuffer("PPDE", .PPDEResArea, 0, 0, 0, _fieldsInfo1, currentFieldNumber, currentScenarioNumber)
            Else
                deleteBuffer("PPDE", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
            End If
        End With
    End Sub

    Private Sub SavePadsAndPipesDitchSystem()
        'Pads and Pipes Ditch system
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetPPDS.Style.Item("display") = "none"
            msgs = "Ditch System: "
            ValidateBMPValue(txtPPDSSides, .PPDSSides, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", sidesMin).Replace("Second", sidesMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "", sidesMin, sidesMax)
            ValidateBMPValue(txtPPDSWidth, .PPDSWidth, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "pcofPP", ValueMin, ValueMax)
            If .PPDSWidth > 0 Or .PPDSSides > 0 Then
                fsetPPDS.Style.Item("display") = ""
                If Not msgs = "Ditch System: " Then lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red : Exit Sub
            End If
        End With
    End Sub

    Private Sub SavePadsAndPipesNoDitch()
        'Pads and Pipes No ditch Improvement
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetPPND.Style.Item("display") = "none"
            msgs = "Ditch Improvement: "
            ValidateBMPValue(txtPPNDWidth, .PPNDWidth, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "pcofPP", ValueMin, ValueMax)
            ValidateBMPValue(txtPPNDSides, .PPNDSides, (msgDoc.Descendants("PadsPipesMsg").Value.Replace("First", sidesMin).Replace("Second", sidesMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "", sidesMin, sidesMax)
            If .PPNDSides > 0 Or .PPNDWidth > 0 Then
                fsetPPDS.Style.Item("display") = ""
                If Not msgs = "Ditch Improvement: " Then lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red : Exit Sub
            End If
        End With
    End Sub

    Private Sub SaveWetland()
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetWL.Style.Item("display") = "none"
            msgs = "Wetland: "
            ValidateBMPValue(txtWLArea, .WLArea, msgDoc.Descendants("WLAreaMsg").Value & msgDoc.Descendants("NumericValue").Value & ValueMin & " - " & ValueMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, "wlArea", ValueMin, ValueMax)
            If .WLArea = 0 Then
                deleteBuffer("WL", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
                Exit Sub
            End If
            fsetWL.Style.Item("display") = ""
            If Not msgs = "Wetland: " Then lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red : Exit Sub : Exit Sub
            If .WLArea > 0 Then createBuffer("WL", .WLArea, 0, 0, 0, _fieldsInfo1, currentFieldNumber, currentScenarioNumber)
        End With
    End Sub

    Private Sub SavePond()
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetPND.Style.Item("display") = "none"
            msgs = "Pond: "
            ValidateBMPValue(txtPNDFraction, .PndF, (msgDoc.Descendants("PondFractionMsg").Value.Replace("First", FractionMin).Replace("Second", FractionMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "pcof", FractionMin, FractionMax)
            If .PndF = 0 Then Exit Sub
            fsetPND.Style.Item("display") = ""
            If Not msgs = "Pond: " Then lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red
        End With
    End Sub

    Private Sub SaveStreamBankStabilization()
        fsetSBS.Style.Item("display") = "none"
        _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Sbs = chkSBS.Checked
        updateBmp("sbs", chkSBS.Checked)
        If Not _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Sbs Then Exit Sub
        fsetSBS.Style.Item("display") = ""
    End Sub

    Private Sub SaveStreamFencing()
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetSF.Style.Item("display") = "none"
            msgs = "Stream Fencing: "
            If ddlSFType.SelectedValue <> String.Empty Then
                .SFCode = ddlSFType.SelectedValue.Split("|")(0)
                .SFName = ddlSFType.SelectedItem.Text
            End If
            ValidateBMPValue(txtSFDays, .SFDays, (msgDoc.Descendants("DaysMessage").Value.Replace("First", SFDaysMin).Replace("Second", SFDaysMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "", SFDaysMin, SFDaysMax)
            ValidateBMPValue(txtSFAnimals, .SFAnimals, (msgDoc.Descendants("AnimalsMessage").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "", ValueMin, ValueMax)
            ValidateBMPValue(txtSFHours, .SFHours, (msgDoc.Descendants("HoursMessage").Value.Replace("First", SFHoursMin).Replace("Second", SFHoursMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "", SFHoursMin, SFHoursMax)
            ValidateBMPValue(txtSFManure, .SFDryManure, (msgDoc.Descendants("DryManureMessage").Value.Replace("First", SFDryManureMin).Replace("Second", SFDryManureMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "", SFDryManureMin, SFDryManureMax)
            ValidateBMPValue(txtSFNO3, .SFNo3, (msgDoc.Descendants("NO3NumericMessage").Value.Replace("First", 0.00001).Replace("Second", FractionMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "", 0.00001, FractionMax)
            ValidateBMPValue(txtSFPO4, .SFPo4, (msgDoc.Descendants("PO3NumericMessage").Value.Replace("First", 0.00001).Replace("Second", FractionMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "", 0.00001, FractionMax)
            ValidateBMPValue(txtSFOrgN, .SFOrgN, (msgDoc.Descendants("OrgNNumericMessage").Value.Replace("First", 0.00001).Replace("Second", FractionMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "", 0.00001, FractionMax)
            ValidateBMPValue(txtSFOrgP, .SFOrgP, (msgDoc.Descendants("OrgPNumericMessage").Value.Replace("First", 0.00001).Replace("Second", FractionMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "", 0.00001, FractionMax)
            ValidateBMPValue(txtSFNH3, .SFNH3, (msgDoc.Descendants("NH3NumericMessage").Value.Replace("First", 0.00001).Replace("Second", FractionMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "", 0.00001, FractionMax)
            If .SFDays > 0 Or .SFAnimals > 0 Or .SFHours > 0 Or .SFDryManure > 0 Or .SFAnimals > 0 Then
                fsetSF.Style.Item("display") = ""
                If Not msgs = "Stream Fencing: " Then lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red
            End If
        End With
    End Sub

    Private Sub SaveRiparianForest()
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetRF.Style.Item("display") = "none"
            msgs = "Riparian Forest: "
            ValidateBMPValue(txtRFWidth, .RFWidth, msgDoc.Descendants("RFWidthMsg").Value & msgDoc.Descendants("NumericValue").Value & ValueMin & " - " & ValueMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, "", ValueMin, ValueMax)
            ValidateBMPValue(txtRFArea, .RFArea, msgDoc.Descendants("RFAreaMsg").Value & msgDoc.Descendants("NumericValue").Value & ValueMin & " - " & ValueMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, "rfArea", ValueMin, ValueMax)
            ValidateBMPValue(txtRFRatio, .RFslopeRatio, msgDoc.Descendants("RFslopeRatioMsg").Value & msgDoc.Descendants("NumericValue").Value & "0.25" & " - " & FractionMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, "", 0.25, FractionMax)
            ValidateBMPValue(txtRFGrass, .RFGrassFieldPortion, msgDoc.Descendants("GrassFieldPortionMsg").Value & msgDoc.Descendants("NumericValue").Value & FractionMin & " - " & "0.75" & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, "", FractionMin, 0.75)
            If .RFWidth > 0 Or .RFArea > 0 Or .RFslopeRatio > 0 Or .RFGrassFieldPortion > 0 Then
                fsetRF.Style.Item("display") = ""
                If Not msgs = "Riparian Forest: " Then lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red : Exit Sub
                If .RFArea > 0 Or .RFWidth > 0 Then
                    createBuffer("RF", 0, .RFWidth, .RFslopeRatio, .RFGrassFieldPortion, _fieldsInfo1, currentFieldNumber, currentScenarioNumber)
                End If
            Else
                deleteBuffer("RFFS", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)    'delete Filter strip buffer created in RF
                deleteBuffer("RF", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)      'delete Riparian Forest buffer created in RF
            End If
        End With
    End Sub

    Private Sub SaveFilterStrip()
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetFS.Style.Item("display") = "none"
            msgs = "Filter Strip: "
            .FSCrop = ddlFSType.SelectedValue
            ValidateBMPValue(txtFSArea, .FSArea, msgDoc.Descendants("FSAreaMsg").Value & msgDoc.Descendants("NumericValue").Value & ValueMin & " - " & ValueMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, "", 0, ValueMax)
            ValidateBMPValue(txtFSWidth, .FSWidth, msgDoc.Descendants("FSWidthMsg").Value & msgDoc.Descendants("NumericValue").Value & ValueMin & " - " & ValueMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, "", ValueMin, ValueMax)
            ValidateBMPValue(txtFSRatio, .FSslopeRatio, msgDoc.Descendants("FSslopeRatioMsg").Value & msgDoc.Descendants("NumericValue").Value & 0.25 & " - " & FractionMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, "", 0.25, FractionMax)
            If .FSWidth > 0 Or .FSArea > 0 Or .FSslopeRatio > 0 Or .FSCrop > 0 Then
                fsetFS.Style.Item("display") = ""
                If .FSCrop <= 0 Then ValidateLoadBMPDll(ddlFSType, msgDoc.Descendants("VegetationTypeMsg").Value)
                If Not msgs = "Filter Strip: " Then lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red : Exit Sub
                If .FSArea > 0 Or .FSWidth > 0 Then
                    createBuffer("FS", .FSCrop, .FSWidth, .FSslopeRatio, 0, _fieldsInfo1, currentFieldNumber, currentScenarioNumber)
                End If
            Else
                deleteBuffer("FS", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
            End If
        End With
    End Sub

    Private Sub SaveWaterWay()
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetWW.Style.Item("display") = "none"
            msgs = "Waterway: "
            If ddlWWType.SelectedValue <> String.Empty AndAlso ddlWWType.SelectedIndex > 0 Then
                .WWCrop = ddlWWType.SelectedValue
            Else
                .WWCrop = 0
                msgs &= msgDoc.Descendants("VegetationError").Value
            End If
            ValidateBMPValue(txtWWWidth, .WWWidth, (msgDoc.Descendants("WaterWayMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "wwwidth", ValueMin, ValueMax)
            If .WWWidth > 0 Or .WWCrop > 0 Then
                fsetWW.Style.Item("display") = ""
                If Not msgs = "Waterway: " Then lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red : Exit Sub
                createBuffer("WW", .WWCrop, .WWWidth, 0, 0, _fieldsInfo1, currentFieldNumber, currentScenarioNumber)
            Else
                deleteBuffer("WW", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
            End If
        End With
    End Sub

    Private Sub SaveContourBuffer()
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetCB.Style.Item("display") = "none"
            msgs = "Contour Buffer: "
            .CBCrop = 0
            If ddlCBType.SelectedValue <> String.Empty Then
                .CBCrop = ddlCBType.SelectedValue
            End If
            ValidateBMPValue(txtCBBufferWidth, .CBBWidth, (msgDoc.Descendants("BufferWidthMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "", ValueMin, ValueMax)
            ValidateBMPValue(txtCBCropWidth, .CBCWidth, (msgDoc.Descendants("BufferWidthMsg").Value.Replace("First", ValueMin).Replace("Second", ValueMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "", ValueMin, ValueMax)
            If .CBBWidth > 0 Or .CBCWidth > 0 Or .CBCrop > 0 Then
                fsetCB.Style.Item("display") = ""
                If Not msgs = "Contour Buffer: " Then lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red : Exit Sub
                callCreateCBBuffer(_fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.CBCrop, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.CBCWidth, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.CBBWidth)
            Else
                deleteBuffer("CBMain", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
                deleteBuffer("CBFS", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
            End If
        End With
    End Sub

    Private Sub SaveLandLeveling()
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetLL.Style.Item("display") = "none"
            msgs = "Land Leveling: "
            ValidateBMPValue(txtLLReduction, .SlopeRed, (msgDoc.Descendants("LandLevelingMsg").Value.Replace("First", LLSlopeRedMin).Replace("Second", LLSlopeRedMax)) & Microsoft.VisualBasic.ControlChars.CrLf, "llslope", LLSlopeRedMin, LLSlopeRedMax)
            If .SlopeRed = 0 Then Exit Sub
            fsetLL.Style.Item("display") = ""
            If Not msgs = "Land Leveling: " Then lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red
        End With
    End Sub

    Private Sub SaveTerraceSystem()
        fsetTS.Style.Item("display") = "none"
        _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Ts = chkTS.Checked
        updateBmp("ts", chkTS.Checked)
        If Not _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Ts Then Exit Sub
        fsetTS.Style.Item("display") = ""
    End Sub

    Private Sub SaveManureControl()
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetMC.Style.Item("display") = "none"
            msgs = "Manure Control: "
            .mcType = ddlMCType.SelectedIndex
            ValidateBMPValue(txtmcNO3N, .mcNO3_N, (msgDoc.Descendants("ManureControlMsg").Value.Replace("First", 0).Replace("Second", maxChange)) & Microsoft.VisualBasic.ControlChars.CrLf, "mcFraction", FractionMin, maxChange)
            ValidateBMPValue(txtmcOrgN, .mcOrgN, (msgDoc.Descendants("ManureControlMsg").Value.Replace("First", 0).Replace("Second", maxChange)) & Microsoft.VisualBasic.ControlChars.CrLf, "mcFraction", FractionMin, maxChange)
            ValidateBMPValue(txtmcPO4P, .mcPO4_P, (msgDoc.Descendants("ManureControlMsg").Value.Replace("First", 0).Replace("Second", maxChange)) & Microsoft.VisualBasic.ControlChars.CrLf, "mcFraction", FractionMin, maxChange)
            ValidateBMPValue(txtmcOrgP, .mcOrgP, (msgDoc.Descendants("ManureControlMsg").Value.Replace("First", 0).Replace("Second", maxChange)) & Microsoft.VisualBasic.ControlChars.CrLf, "mcFraction", FractionMin, maxChange)
            ValidateBMPValue(txtmcOM, .mcOM, (msgDoc.Descendants("ManureControlMsg").Value.Replace("First", 0).Replace("Second", maxChange)) & Microsoft.VisualBasic.ControlChars.CrLf, "mcFraction", FractionMin, maxChange)
            If .mcType = 0 And .mcNO3_N = 0 And .mcOM = 0 And .mcOrgN = 0 And .mcOrgP = 0 And .mcPO4_P = 0 Then Exit Sub
            If .mcType > 0 Then ValidateLoadBMPDll(ddlMCType, msgDoc.Descendants("ManureControlTypeMsg").Value)
            fsetMC.Style.Item("display") = ""
            If Not msgs = "Manure Control: " Then lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red
        End With
    End Sub

    Private Sub SaveLiming()
        fsetLM.Style.Item("display") = "none"
        _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Lm = chkLM.Checked
        If chkLM.Checked = True Then
            updateBmp("lm", "0")
        Else
            updateBmp("lm", "1")
        End If
        If Not _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Lm Then Exit Sub
        'fsetLM.Style.Item("display") = ""  liming is not display until we found the problem (with or without the liming results are the same).
    End Sub

    Private Sub SaveClimateChange()
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetCC.Style.Item("display") = "none"
            msgs = "Climate Change: "
            ValidateBMPValue(txtCCMinTmp, .CcMinimumTeperature, msgDoc.Descendants("MinTemp").Value & msgDoc.Descendants("NumericValue").Value & minChange & " - " & maxChange & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, "CcMinimumTeperature", minChange, maxChange)
            ValidateBMPValue(txtCCMaxTmp, .CcMaximumTeperature, msgDoc.Descendants("MaxTemp").Value & msgDoc.Descendants("NumericValue").Value & minChange & " - " & maxChange & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, "CcMaximumTeperature", minChange, maxChange)
            ValidateBMPValue(txtCCPcp, .CcPrecipitation, msgDoc.Descendants("Precipitation").Value & msgDoc.Descendants("NumericValue").Value & minChange & " - " & maxChange & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, "CcPrecipitation", minChange, maxChange)
            fsetCC.Style.Item("display") = ""
            If Not msgs = "Climate Change: " Then lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red : Exit Sub
        End With
    End Sub

    Private Sub SaveAsfaltOrConcrete()
        fsetAoC.Style.Item("display") = "none"
        _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AoC = chkAoC.Checked
        updateBmp("AoC", chkAoC.Checked)
        If Not _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AoC Then Exit Sub
        fsetAoC.Style.Item("display") = ""
    End Sub

    Private Sub SaveGrassCover()
        fsetGC.Style.Item("display") = "none"
        _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Gc = chkGC.Checked
        updateBmp("GC", chkGC.Checked)
        If Not _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Gc Then Exit Sub
        fsetGC.Style.Item("display") = ""
    End Sub

    Private Sub SaveSlopeAdjustment()
        fsetSA.Style.Item("display") = "none"
        _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Sa = chkSA.Checked
        updateBmp("SA", chkSA.Checked)
        If Not _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Sa Then Exit Sub
        fsetSA.Style.Item("display") = ""
    End Sub

    Private Sub SaveShading()
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo
            fsetSdg.Style.Item("display") = "none"
            msgs = "Shading: "
            'If ddlSdgCrop.SelectedIndex > 0 Then
            .SdgCrop = ddlSdgCrop.SelectedValue
            'Else
            '    .SdgCrop = 0
            '    msgs &= msgDoc.Descendants("VegetationError").Value
            'End If
            If .SdgArea > 0 Or .SdgWidth > 0 Or .SdgslopeRatio > 0 Or .SdgCrop > 0 Then
                fsetSdg.Style.Item("display") = ""
                ValidateBMPValue(txtSdgArea, .SdgArea, msgDoc.Descendants("SdgAreaMsg").Value & msgDoc.Descendants("NumericValue").Value & ValueMin & " - " & ValueMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, "", ValueMin, ValueMax)
                ValidateBMPValue(txtSdgWidth, .SdgWidth, msgDoc.Descendants("SdgWidthMsg").Value & msgDoc.Descendants("NumericValue").Value & ValueMin & " - " & ValueMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, "", ValueMin, ValueMax)
                ValidateBMPValue(txtSdgSlopeRatio, .SdgslopeRatio, msgDoc.Descendants("SdgslopeRatioMsg").Value & msgDoc.Descendants("NumericValue").Value & "0.25" & " - " & FractionMax & ". " & msgDoc.Descendants("ChangeValue").Value & Microsoft.VisualBasic.ControlChars.CrLf, "", 0.25, FractionMax)
                If .SdgCrop <= 0 Then ValidateLoadBMPDll(ddlSdgCrop, msgDoc.Descendants("VegetationTypeMsg").Value)
                If Not msgs = "Shading: " Then
                    lblMessage.Text &= "&#10;" & msgs : lblMessage.ForeColor = Drawing.Color.Red : Exit Sub
                Else
                    msgs = String.Empty
                End If
                createBuffer("Sdg", .SdgCrop, .SdgWidth, .SdgslopeRatio, 0, _fieldsInfo1, currentFieldNumber, currentScenarioNumber)
            Else
                deleteBuffer("Sdg", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
            End If
        End With
    End Sub

    Private Sub ValidateBMPValue(ByRef sender As TextBox, ByRef placeToSave As Single, ByVal msg As String, ByVal updateValue As String, ByVal minVal As Single, ByVal maxVal As Single)
        Dim result As Single = 0
        If currentFieldNumber < 0 Or currentScenarioNumber < 0 Then Exit Sub
        Dim valueTruncated As Short
        Dim bmpValue As Single = 0

        Try
            sender.BackColor = Drawing.Color.White
            If sender.Text = String.Empty Then sender.Text = 0

            If Single.TryParse(sender.Text, result) Then
                placeToSave = sender.Text
                If sender.ID.Contains("Animal") Or sender.ID.Contains("Days") Or sender.ID.Contains("Días") Then
                    valueTruncated = sender.Text
                    sender.Text = valueTruncated
                End If
                bmpValue = sender.Text
                If updateValue = "pcofPP" Or updateValue = "idr" Then
                    If updateValue = "pcofPP" And (sender.ID = "txtPPDEWidth" Or sender.ID = "txtPPTWWidth" Or sender.ID = "txtPPDSWidth") Then bmpValue = 0.5
                    If updateValue = "pcofPP" And sender.ID = "txtPPNDWidth" Then bmpValue = 0.0
                End If
                updateBmp(updateValue, bmpValue)
                If result < minVal Or result > maxVal Then Throw New Global.System.Exception
            Else
                Throw New Global.System.Exception
            End If

        Catch ex As Exception
            msgs &= msg
            sender.BackColor = Drawing.Color.Red
        End Try
    End Sub

    Private Sub ValidateLoadBMPDll(ByVal sender As DropDownList, ByVal msg As String)
        sender.BackColor = Drawing.Color.White
        If sender.SelectedIndex <= 0 Then
            msgs &= msg
            sender.BackColor = Drawing.Color.Red
        End If
    End Sub

    Private Sub ValidateLoadBMPValue(ByVal sender As TextBox, ByVal msg As String, ByVal minVal As Single, ByVal maxVal As Single)
        Dim result As Single = 0
        If currentFieldNumber < 0 Or currentScenarioNumber < 0 Then Exit Sub
        Dim bmpValue As Single = 0

        Try
            sender.BackColor = Drawing.Color.White
            If sender.Text = String.Empty Then sender.Text = 0

            If Single.TryParse(sender.Text, result) Then
                bmpValue = sender.Text
                If result < minVal Or result > maxVal Then Throw New Global.System.Exception
            Else
                Throw New Global.System.Exception
            End If

        Catch ex As Exception
            msgs &= msg
            sender.BackColor = Drawing.Color.Red
        End Try
    End Sub
    'Private Sub modifyCN(ByVal placeToSave As Single)
    '    For Each oper In _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo
    '        If oper.ApexOpAbbreviation.Trim = planting Then
    '            If placeToSave = 0 Then oper.OpVal2 *= 0.9 Else oper.OpVal2 *= (100 / 90)
    '        End If
    '    Next

    '    For Each scen In _fieldsInfo1(currentFieldNumber)._soilsInfo(currentSoilNumber)._scenariosInfo
    '        For Each oper In scen._operationsInfo
    '            If oper.ApexOpAbbreviation.Trim = planting Then
    '                If placeToSave = 0 Then oper.OpVal2 *= 0.9 Else oper.OpVal2 *= (100 / 90)
    '            End If
    '        Next
    '    Next
    'End Sub

    'Private Sub verifyBuffers()
    '    Dim lastFieldNumber As UShort = currentFieldNumber
    '    Dim lastScenarioNumber As Short = currentScenarioNumber

    '    Try
    '        If _StartInfo.Versions = "" Then
    '            Dim i As UShort = 0, j As UShort = 0
    '            For Each Field In _fieldsInfo1
    '                currentFieldNumber = i
    '                j = 0
    '                For Each scen In Field._scenariosInfo
    '                    currentScenarioNumber = j
    '                    If scen._bufferInfo.Count <= 0 Then
    '                        createBuffer("Sdg", scen._bmpsInfo.SdgCrop, scen._bmpsInfo.SdgWidth, scen._bmpsInfo.SdgslopeRatio, 0)
    '                        createBuffer("FS", scen._bmpsInfo.FSCrop, scen._bmpsInfo.FSWidth, scen._bmpsInfo.FSslopeRatio, 0)
    '                        createBuffer("RF", 0, scen._bmpsInfo.RFWidth, scen._bmpsInfo.RFslopeRatio, scen._bmpsInfo.RFGrassFieldPortion)
    '                        createBuffer("WW", scen._bmpsInfo.WWCrop, scen._bmpsInfo.WWWidth, 0, 0)
    '                        createBuffer("WL", scen._bmpsInfo.WLArea, 0, 0, 0)
    '                        createBuffer("PPDE", scen._bmpsInfo.PPDEResArea, 0, 0, 0)
    '                        createBuffer("PPTW", scen._bmpsInfo.PPTWResArea, 0, 0, 0)
    '                        createBuffer("AITW", scen._bmpsInfo.AIResArea, 0, 0, 0)
    '                    End If
    '                    'verify if there are more bmps to update if this is an old version
    '                    'Auto Irrigation
    '                    If scen._bmpsInfo.AIType > 0 Then
    '                        Select Case scen._bmpsInfo.AIType
    '                            Case 1
    '                                updateBmp("nirr", 1)
    '                            Case 2
    '                                updateBmp("nirr", 2)
    '                            Case 5
    '                                updateBmp("nirr", 5)
    '                                'Case 2
    '                                '    updateBmp("nirr", scen._bmpsInfo.AIType)
    '                        End Select
    '                        updateBmp("bir", scen._bmpsInfo.AIWaterStressFactor)
    '                        updateBmp("efi", scen._bmpsInfo.AIEff)
    '                        updateBmp("iri", scen._bmpsInfo.AIFreq)
    '                        updateBmp("armx", scen._bmpsInfo.AIMaxSingleApp)
    '                        updateBmp("fdsf", scen._bmpsInfo.AISafetyFactor)
    '                    End If
    '                    'Auto Fertigation
    '                    If scen._bmpsInfo.AFType > 0 Then
    '                        updateBmp("afnirr", scen._bmpsInfo.AFType)
    '                        updateBmp("afbir", scen._bmpsInfo.AFWaterStressFactor)
    '                        updateBmp("afefi", scen._bmpsInfo.AFEff)
    '                        updateBmp("afiri", scen._bmpsInfo.AFFreq)
    '                        updateBmp("afarmx", scen._bmpsInfo.AFMaxSingleApp)
    '                        updateBmp("affdsf", scen._bmpsInfo.AFSafetyFactor)
    '                    End If
    '                    'tile drain
    '                    If scen._bmpsInfo.TileDrainDepth > 0 Then updateBmp("idr", scen._bmpsInfo.TileDrainDepth)
    '                    'pads and pipes - no dithc improvement
    '                    If scen._bmpsInfo.PPDSWidth > 0 Or scen._bmpsInfo.PPDEWidth > 0 Or scen._bmpsInfo.PPNDWidth > 0 Or scen._bmpsInfo.PPTWWidth > 0 Then updateBmp("pcofPP", 0.5)
    '                    'ponds
    '                    If scen._bmpsInfo.PndF Then updateBmp("pcof", scen._bmpsInfo.PndF)
    '                    'stream bank stabilization
    '                    If scen._bmpsInfo.Sbs Then updateBmp("sbs", 0)
    '                    'land leveling
    '                    If scen._bmpsInfo.SlopeRed Then updateBmp("llslope", scen._bmpsInfo.SlopeRed)
    '                    'Terrace system
    '                    If scen._bmpsInfo.Ts Then updateBmp("ts", 0)
    '                    'liming                        
    '                    If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Lm Then
    '                        updateBmp("lm", "0")
    '                    Else
    '                        updateBmp("lm", "1")
    '                    End If
    '                    j += 1
    '                Next
    '                i += 1
    '            Next
    '            _StartInfo.Versions = version
    '        End If

    '        currentFieldNumber = lastFieldNumber
    '        currentScenarioNumber = lastScenarioNumber
    '    Catch ex As Exception
    '        showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & "--> " & ex.Message)
    '    End Try
    'End Sub

    Private Sub callCreateCBBuffer(ByVal crop, ByVal value1, ByVal value2)
        If value1 = 0 And value2 = 0 And crop Then
            deleteBuffer("CBMain", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
            deleteBuffer("CBFS", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
        End If

        If value1 > 0 And value2 > 0 And crop Then
            createBuffer("CB", crop, value1, value2, 0, _fieldsInfo1, currentFieldNumber, currentScenarioNumber)
            'validate bmps to recreate all of them in the new contour buffer fields created.
            Dim nirr As String = "  00"

            For j = 0 To _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo.Count - 1
                With _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo(j)
                    'auto irrigation
                    If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AIType > 0 Then
                        With ._line8(0)
                            Select Case _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AIType
                                Case 1
                                    .Nirr = "  01"
                                Case 2
                                    .Nirr = "  02"
                                Case 3
                                    .Nirr = "  05"
                                Case 7
                                    .Nirr = "  02"
                                Case 8
                                    .Nirr = "  02"
                            End Select
                            .Iri = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AIFreq
                        End With
                        With ._line9(0)
                            .Vimx = vimxDefault
                            If .Bir = 0 Then .Bir = 0.8
                            If .Armx = 0 Then .Armx = armxDefault * in_to_mm
                            .Bir = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AIWaterStressFactor
                            .Efi = 1 - _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AIEff
                            .Armx = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AIMaxSingleApp * in_to_mm
                            .Fdsf = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AISafetyFactor
                        End With
                    End If

                    'auto fertigation
                    If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFType > 0 Then
                        With ._line8(0)
                            Select Case _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFType
                                Case 1
                                    .Nirr = "  01"
                                Case 2
                                    .Nirr = "  02"
                                Case 3
                                    .Nirr = "  05"
                                Case 7
                                    .Nirr = "  02"
                                Case 8
                                    .Nirr = "  02"
                            End Select
                            .Iri = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFFreq
                            .Idf4 = 1
                        End With
                        With ._line9(0)
                            .Vimx = vimxDefault
                            .Bft = 1
                            If .Bir = 0 Then .Bir = 0.8
                            If .Armx = 0 Then .Armx = armxDefault * in_to_mm
                            .Bir = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFWaterStressFactor
                            .Efi = 1 - _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFEff
                            .Armx = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFMaxSingleApp * in_to_mm
                            .Fdsf = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AFSafetyFactor
                        End With
                    End If

                    'tile drain
                    If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.TileDrainDepth > 0 Then
                        ._line8(0).Idr = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.TileDrainDepth * ft_to_mm
                        ._line9(0).Drt = drt
                    End If

                    'ponds
                    If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.PndF > 0 Then
                        ._line7(0).Pcof = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.PndF
                    End If

                    'pond in PP
                    If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.PPDEWidth > 0 Or _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.PPDSWidth > 0 Or _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.PPNDWidth > 0 Or _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.PPTWWidth > 0 Then
                        ._line7(0).Pcof = 0.5
                    End If

                    'stream bank stabilization
                    If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Sbs Then
                        If ._line5(0).Rchc > 0.01 Then ._line5(0).Rchc = 0.01
                        If ._line5(0).Rchk > 0.01 Then ._line5(0).Rchk = 0.01
                        ._line5(0).Rchn = 0.2
                        ._line4(0).Chn = 0.1
                    End If

                    'land leveling
                    If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.SlopeRed > 0 Then
                        ._line4(0).Slp = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo(j)._line4(0).Slp - (_fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo(j)._line4(0).Slp * _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.SlopeRed / 100)
                    End If

                    'Terrace System
                    'Determine PEC for Contour Buffer Strip and TS depeding on table manual APEX0604 PAGE 49.
                    'todo default value for pec. if there are others bmps that modify pec it should ask for those to change pec accordingly.
                    If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Ts Then
                        ._line10(0).Pec = 1.0
                        If chkTS.Checked = False Then Exit Sub
                        Select Case ._line4(0).Slp
                            Case Is <= 0.02
                                ._line10(0).Pec = 0.6
                            Case Is <= 0.08
                                ._line10(0).Pec = 0.5
                            Case Is <= 0.12
                                ._line10(0).Pec = 0.6
                            Case Is <= 0.16
                                ._line10(0).Pec = 0.7
                            Case Is <= 0.2
                                ._line10(0).Pec = 0.8
                            Case Is <= 0.25
                                ._line10(0).Pec = 0.9
                            Case Else
                                ._line10(0).Pec = 1.0
                        End Select
                    End If

                    'Manure Control
                    If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.SlopeRed > 0 Then
                        'to do here for manure control
                    End If

                    'lime
                    If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Lm Then
                        ._line8(0).Lm = 0
                    Else
                        ._line8(0).Lm = 1
                    End If
                    'Asfalt or Concreto
                    If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AoC Then
                        ._line5(0).Rchk = 0.01
                        ._line5(0).Rchn = 0.05
                        ._line10(0).Pec = 0.2
                    End If
                    'Grass cover
                    If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Gc Then
                        ._line5(0).Rchc = 0.01
                        ._line4(0).Upn = 0.4
                        ._line10(0).Pec = 0.3
                    End If
                    'Slope Adjustment
                    If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.Sa Then
                        Select Case ._line4(0).Slp
                            Case Is <= 0.02
                                ._line10(0).Pec = 0.6
                            Case Is <= 0.08
                                ._line10(0).Pec = 0.5
                            Case Is <= 0.12
                                ._line10(0).Pec = 0.6
                            Case Is <= 0.16
                                ._line10(0).Pec = 0.7
                            Case Is <= 0.2
                                ._line10(0).Pec = 0.8
                            Case Is <= 0.25
                                ._line10(0).Pec = 0.9
                            Case Else
                                ._line10(0).Pec = 1.0
                        End Select
                    End If
                End With
            Next
        End If
    End Sub

    Function validateAllBMPs() As String
        Dim msg As String = String.Empty

        msg &= validateAutoIrrigation(fsetAI, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo)
        msg &= validateAutoFertigation(fsetAF, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo)
        msg &= validatePadsAndPipesNoDitchImprovement(fsetPPND, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo)
        msg &= validatePadsAndPipesTwoStagaeDitchSystem(fsetPPDS, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo)
        msg &= validatePadsAndPipesDitchEnlargment(fsetPPDE, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo)
        msg &= validatePadsAndPipesTailwaterIrrigation(fsetPPTW, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo)
        msg &= validateFencing(fsetSF, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo)

        If msg = String.Empty Then
            Return "OK"
        Else
            Return msg
        End If
    End Function
    Private Sub ArrangeInfo(openSave As String)
        Select Case openSave
            'this is done after the open subrutine
            Case "Open"
                _startInfo = Session("projects")._StartInfo
                _fieldsInfo1 = Session("projects")._fieldsInfo1
                If Not Session("crops") Is Nothing Then _crops = Session("crops")
                'this is done before the save subroutine
            Case "Save"
                Session("projects")._StartInfo = _startInfo
                Session("projects")._fieldsInfo1 = _fieldsInfo1
        End Select
    End Sub

    Private Sub gvCrops_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCrops.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            If _bufferCrops(e.Row.RowIndex).Number = 0 Or _bufferCrops(e.Row.RowIndex).Number = 367 Then
                e.Row.Style.Item("display") = "none"
            End If
        End If

    End Sub

End Class
