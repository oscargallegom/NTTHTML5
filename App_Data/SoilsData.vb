Public Class sortByArea
    Inherits Comparer(Of SoilsData)
    Public Overrides Function Compare(ByVal x As SoilsData, ByVal y As SoilsData) As Integer
        Return y.Percentage.CompareTo(x.Percentage)
    End Function
End Class

Public Class SoilsData
    Private _soilNumber As UShort
    Private _scenarioName As String
    Private _selected As Boolean
    Private _field As String
    Private _key As Integer
    Private _symbol As String
    Private _Group As String
    Private _name As String
    Private _component As String
    Private _albedo As Single
    Private _slope As Single
    Private _percentage As Single
    Private _ffc As Single
    Private _wtmn As Single
    Private _wtmx As Single     '0=No selection=0, 1=Well drained=0, 2=Poorly drained=4, 3=Somewhat poorly drained=6
    Private _wtbl As Single
    Private _gwst As Single
    Private _gwmx As Single
    Private _rftt As Single
    Private _rfpk As Single
    Private _tsla As Single
    Private _xids As Single
    Private _rtn1 As Single
    Private _xidk As Single
    Private _zqt As Single
    Private _zf As Single
    Private _ztk As Single
    Private _fbm As Single
    Private _fhp As Single
    Public SSACode As String
    Public SSAName As String
    Public _layersInfo As New List(Of LayersData)
    Public _scenariosInfo As New List(Of ScenariosData)

    'added to control when information is not available
    Private texture() As String = {"sandy clay loam", "silty clay loam", "loamy sand", "sandy loam", "sandy clay", "silt loam", "clay loam", "silty clay", "sand", "loam", "silt", "clay"}
    Private sands() As Single = {53.2, 8.9, 80.2, 63.4, 52, 15, 29.1, 7.7, 84.6, 41.2, 4.9, 12.7}
    Private silts() As Single = {20.6, 58.9, 14.6, 26.3, 6, 67, 39.3, 45.8, 11.1, 40.2, 85, 32.7}
    Private satcs() As Single = {9.24, 11.4, 94.66, 48.01, 0.8, 15.55, 7.74, 5.29, 107.83, 19.98, 10.64, 2.1}
    Private bds() As Single = {1.49, 1.2, 1.44, 1.46, 1.49, 1.31, 1.33, 1.21, 1.45, 1.4, 1.42, 1.24}

    'one subArea for each soil
    Public Structure subAreaInfo
    End Structure

    Property SoilNumber() As UShort
        Get
            Return _soilNumber
        End Get
        Set(ByVal value As UShort)
            _soilNumber = value
        End Set
    End Property

    Property ScenarioName() As String
        Get
            Return _scenarioName
        End Get
        Set(ByVal value As String)
            _scenarioName = value
        End Set
    End Property

    Property Selected() As Boolean
        Get
            Return _selected
        End Get
        Set(ByVal value As Boolean)
            _selected = value
        End Set
    End Property

    Property Field() As String
        Get
            Return _field
        End Get
        Set(ByVal value As String)
            _field = value
        End Set
    End Property

    Property Key() As Integer
        Get
            Return _key
        End Get
        Set(ByVal value As Integer)
            _key = value
        End Set
    End Property

    Property Symbol() As String
        Get
            Return _symbol
        End Get
        Set(ByVal value As String)
            _symbol = value
        End Set
    End Property

    Property Group() As String
        Get
            Return _Group
        End Get
        Set(ByVal value As String)
            _Group = value
        End Set
    End Property

    Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Property Component() As String
        Get
            Return _component
        End Get
        Set(ByVal value As String)
            _component = value
        End Set
    End Property

    Property Albedo() As Single
        Get
            Return _albedo
        End Get
        Set(ByVal value As Single)
            _albedo = value
        End Set
    End Property

    Property Slope() As Single
        Get
            Return _slope
        End Get
        Set(ByVal value As Single)
            _slope = value
        End Set
    End Property

    Property Percentage() As Single
        Get
            Return _percentage
        End Get
        Set(ByVal value As Single)
            _percentage = Math.Round(value, 2)
        End Set
    End Property

    Property Ffc() As Single
        Get
            Return _ffc
        End Get
        Set(ByVal value As Single)
            _ffc = value
        End Set
    End Property
    Property Wtmn() As Single
        Get
            Return _wtmn
        End Get
        Set(ByVal value As Single)
            _wtmn = value
        End Set
    End Property
    Property Wtmx() As Single
        Get
            Return _wtmx
        End Get
        Set(ByVal value As Single)
            _wtmx = value
        End Set
    End Property
    Property Wtbl() As Single
        Get
            Return _wtbl
        End Get
        Set(ByVal value As Single)
            _wtbl = value
        End Set
    End Property
    Property Gwst() As Single
        Get
            Return _gwst
        End Get
        Set(ByVal value As Single)
            _gwst = value
        End Set
    End Property
    Property Gwmx() As Single
        Get
            Return _gwmx
        End Get
        Set(ByVal value As Single)
            _gwmx = value
        End Set
    End Property
    Property Rftt() As Single
        Get
            Return _rftt
        End Get
        Set(ByVal value As Single)
            _rftt = value
        End Set
    End Property
    Property Rfpk() As Single
        Get
            Return _rfpk
        End Get
        Set(ByVal value As Single)
            _rfpk = value
        End Set
    End Property
    Property Tsla() As Single
        Get
            Return _tsla
        End Get
        Set(ByVal value As Single)
            _tsla = value
        End Set
    End Property
    Property Xids() As Single
        Get
            Return _xids
        End Get
        Set(ByVal value As Single)
            _xids = value
        End Set
    End Property
    Property Rtn1() As Single
        Get
            Return _rtn1
        End Get
        Set(ByVal value As Single)
            _rtn1 = value
        End Set
    End Property
    Property Xidk() As Single
        Get
            Return _xidk
        End Get
        Set(ByVal value As Single)
            _xidk = value
        End Set
    End Property
    Property Zqt() As Single
        Get
            Return _zqt
        End Get
        Set(ByVal value As Single)
            _zqt = value
        End Set
    End Property
    Property Zf() As Single
        Get
            Return _zf
        End Get
        Set(ByVal value As Single)
            _zf = value
        End Set
    End Property
    Property Ztk() As Single
        Get
            Return _ztk
        End Get
        Set(ByVal value As Single)
            _ztk = value
        End Set
    End Property
    Property Fbm() As Single
        Get
            Return _fbm
        End Get
        Set(ByVal value As Single)
            _fbm = value
        End Set
    End Property
    Property Fhp() As Single
        Get
            Return _fhp
        End Get
        Set(ByVal value As Single)
            _fhp = value
        End Set
    End Property

    Public Sub AddScenario(ByVal scenarioName As String, forestry As Boolean, fieldArea As Single, rchcval As Single, rchkval As Single, soilNumber As UShort, precentage As Single, slope As Single, _scenario As ScenariosData, fieldName As String, fieldForestry As Boolean, currentScenarioNumber As UShort)
        Dim newScenario As New ScenariosData
        'Dim newOper As New OperationsData
        newScenario.Name = scenarioName
        _scenariosInfo.Add(newScenario)
        'If forestry And _fieldsInfo1(currentFieldNumber).Name.Contains(road) Then
        '    newOper = New OperationsData
        '    newOper.ApexCrop = cropRoad
        '    newOper.ApexCropName = "Road"
        '    newOper.ApexOp = 1
        '    newOper.ApexOpAbbreviation = planting
        '    newOper.ApexOpName = "Planting"
        '    newOper.ApexOpTypeName = "PLANTER REGULAR 12 ROW"
        '    newOper.ApexOpType = 136
        '    newOper.ApexTillCode = 136
        '    newOper.ApexTillName = "PLANTER REGULAR 12 ROW"
        '    newOper.Day = 1
        '    newOper.Month = 1
        '    newOper.Year = 1
        '    newOper.EventId = 1
        '    newOper.Index = 0
        '    newOper.LuNumber = 0
        '    newOper.Scenario = scenarioName
        '    _scenariosInfo(_scenariosInfo.Count - 1).AddOperation(newOper)
        'End If

        'every time a scenario is added a subarea is needed.
        AddSubarea(1, soilNumber, fieldArea, Percentage, slope, rchcval, rchkval, "Soil", _scenario, fieldName, fieldForestry, currentScenarioNumber)
    End Sub

    Public Sub AddOperations(operationsList As List(Of OperationsData), scenarioIndex As UShort)
        Dim operation As OperationsData

        For Each oper In operationsList
            operation = New OperationsData
            operation.ApexCrop = oper.ApexCrop
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
            _scenariosInfo(scenarioIndex).AddOperation(operation)
        Next
    End Sub

    Public Sub DeleteScenario(ByVal index As Short)
        _scenariosInfo.RemoveAt(index)
    End Sub

    Public Sub AddSubarea(owner As UShort, soilNumber As UShort, Area As Single, Percentage As Single, slope As Single, rchc As Single, rchk As Single, sbaType As String, _scenario As ScenariosData, fieldName As String, fieldForestry As Boolean, currentScenarioNumber As UShort)
        With _scenariosInfo(currentScenarioNumber)._subareasInfo
            .SbaType = sbaType
            'line 1
            .SubareaNumber = soilNumber
            .SubareaTitle = "0000000000000000  .sub file Subbasin:1  Date: " & Date.Now
            'line 2
            ._line2(0).Inps = soilNumber
            ._line2(0).Iops = soilNumber + 1
            ._line2(0).Iow = owner
            'line 4
            With ._line4(0)
                .Wsa = Area * Percentage / 100 * ac_to_ha
                .chl = Math.Sqrt(.Wsa * 0.01)
                .Slp = slope / 100
                .Slpg = calcSlopeLength(slope)
                If fieldName.Contains(smz) And fieldForestry Then
                    .Chn = 0.1
                    .Upn = 0.24
                    .Ffpq = 0.9
                End If
            End With
            'line 5
            With ._line5(0)
                If soilNumber = 1 And sbaType = "Soil" Then
                    .Rchl = _scenariosInfo(currentScenarioNumber)._subareasInfo._line4(0).chl
                Else
                    '.rchl = .chl * 0.9
                    .Rchl = _scenariosInfo(currentScenarioNumber)._subareasInfo._line4(0).chl      'changed to add all fields
                    '.Wsa *= -1               'cahanged to add all fields
                End If
                If rchc <> Nothing AndAlso rchc > 0 Then
                    .Rchc = rchc
                Else
                    .Rchc = 0.2
                End If
                If rchk <> Nothing AndAlso rchk > 0 Then
                    .Rchk = rchk
                Else
                    .Rchk = 0.2
                End If
            End With
            If fieldForestry And fieldName.Contains(road) Then
                ._line10(0).Pec = 0
            Else
                ._line10(0).Pec = 1
            End If
            'line 11 and 12 grazing
            'If _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count > 0 Then
            'For i = 0 To _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo.Count - 1
            'If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo(i).ApexOpAbbreviation.Trim = grazing Then
            For i = 0 To _scenario._operationsInfo.Count - 1
                If _scenario._operationsInfo(i).ApexOpAbbreviation.Trim = grazing Then
                    ._line11(0).Ny1 = 1
                    ._line12(0).Xtp1 = 0.5
                End If
            Next
            'End If
        End With

    End Sub

    Public Sub AddEmptyLayer()
        Dim layerData As LayersData

        layerData = New LayersData
        layerData.BD = 0
        layerData.Depth = 0
        layerData.LayerNumber = 0
        layerData.OM = 0
        layerData.PH = 0
        layerData.Sand = 0
        layerData.Silt = 0
        layerData.SoilP = 0
        layerData.LayerNumber = _layersInfo.Count + 1
        _layersInfo.Add(layerData)
    End Sub

    Public Sub AddLayer(ByRef _StartInfo)
        Dim sSql As String = String.Empty
        sSql = "SELECT distinct ldep as ldep, sand as sand, silt as silt, om as om, bd as bd, muid, "
        sSql = sSql & " albedo as albedo, horizdesc2 as horizdesc2, ph as ph, cec as cec, coarse as coarse, ksat as ksat, textr as texture "
        sSql = sSql & " FROM " & _StartInfo.StateAbrev.Trim & "SOILS"
        sSql = sSql & " WHERE series= '" & Name & "' AND horizdesc1= '" & Symbol & "' AND horizdesc2 like '" & Group & "%' AND muid = '" & Key & "' AND seriesName ='" & Component + "'"
        sSql = sSql & " ORDER BY ldep"

        Dim da As System.Data.SqlClient.SqlDataAdapter
        Dim dtResult As System.Data.DataTable
        Dim layer As LayersData
        Dim i As UShort = 1
        Dim j As UShort = 0
        Dim ldep As Single = 0
        Dim dep_ant As Single = 0
        Try
            Dim conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
            'conn.ConnectionString = soilDBConnection
            conn.ConnectionString = dbConnectString("Soil")
            conn.Open()
            da = New System.Data.SqlClient.SqlDataAdapter(sSql, conn)
            dtResult = New System.Data.DataTable
            da.Fill(dtResult)
            'determine values for sand, silt, and bd depending on the texture just in case they are needed due to lack of information from soil database
            For j = 0 To 11
                If dtResult.Rows(0).Item("texture").ToLower.Contains(texture(j)) Then Exit For
            Next

            For Each dataRow In dtResult.Rows
                If IsDBNull(dataRow.item("ldep")) Or dataRow.item("ldep") <= 0 Or dataRow.item("ldep") = ldep Then Continue For
                layer = New LayersData
                layer.Depth = Math.Round(dataRow.Item("ldep") / in_to_cm, 2)
                ldep = dataRow.item("ldep")
                If Not IsDBNull(dataRow.item("sand")) AndAlso Not dataRow.item("sand") <= 0 Then layer.Sand = dataRow.Item("sand")
                ValidateSand(layer)
                If Not IsDBNull(dataRow.item("silt")) AndAlso Not dataRow.item("silt") <= 0 Then layer.Silt = dataRow.Item("silt")
                ValidateSilt(layer, j)
                If Not IsDBNull(dataRow.item("om")) AndAlso Not dataRow.item("om") <= 0 Then layer.OM = dataRow.Item("om")
                ValidateOM(layer)
                If Not IsDBNull(dataRow.item("bd")) AndAlso Not dataRow.item("bd") <= 0 Then layer.BD = dataRow.Item("bd")
                ValidateBD(layer, j)
                If Not IsDBNull(dataRow.item("ph")) AndAlso Not dataRow.item("ph") <= 0 Then layer.PH = dataRow.Item("ph")
                ValidatePH(layer)
                If Not IsDBNull(dataRow.item("cec")) AndAlso Not dataRow.item("cec") <= 0 Then layer.Cec = dataRow.Item("cec")
                ValidateCEC(layer)
                layer.SoilP = 0
                If ldep > 10 And i = 1 Then
                    Dim first_layer As New LayersData
                    first_layer.LayerNumber = i
                    first_layer.Depth = Math.Round(10 / in_to_cm, 2)
                    first_layer.Sand = layer.Sand
                    first_layer.Silt = layer.Silt
                    first_layer.OM = layer.OM
                    first_layer.BD = layer.BD
                    first_layer.PH = layer.PH
                    first_layer.Cec = layer.Cec
                    first_layer.SoilP = layer.SoilP
                    _layersInfo.Add(first_layer)
                    i += 1
                End If
                layer.LayerNumber = i
                _layersInfo.Add(layer)
                i += 1
            Next

            conn.Close()
            conn.Dispose()
            conn = Nothing
        Catch ex As System.Exception
        End Try
    End Sub

    Public Sub ValidateOM(ByRef layer As LayersData)
        If layer.OM <= 0 Then
            If layer.LayerNumber > 1 Then
                layer.OM = _layersInfo(layer.LayerNumber - 2).OM
            Else
                layer.OM = OMDefault
            End If
        Else
            If layer.OM < OMMin Or layer.OM > OMMax Then
                layer.OM = OMDefault
            End If
        End If
    End Sub

    Private Sub ValidateSand(ByRef layer As LayersData)
        If layer.Sand <= 0 AndAlso layer.LayerNumber > 1 Then
            layer.Sand = _layersInfo(layer.LayerNumber - 2).Sand
        End If
    End Sub

    Private Sub ValidateSilt(ByRef layer As LayersData, j As UShort)
        If layer.Silt <= 0 AndAlso layer.LayerNumber > 1 Then
            layer.Silt = _layersInfo(layer.LayerNumber - 2).Silt
        End If
        If layer.Silt + layer.Sand <= 0 AndAlso layer.LayerNumber = 1 Then
            layer.Silt = silts(j)
            layer.Sand = sands(j)
        End If

    End Sub

    Private Sub ValidateBD(ByRef layer As LayersData, j As UShort)
        If layer.BD <= 0 Then
            If layer.LayerNumber > 1 Then
                layer.BD = _layersInfo(layer.LayerNumber - 2).BD
            Else
                layer.BD = bds(j)
                'layer.BD = 0
            End If
        End If
        If layer.BD < BDMin Then layer.BD = BDMin
        If layer.BD > BDMax Then layer.BD = BDMax
    End Sub

    Private Sub ValidatePH(ByRef layer As LayersData)
        If layer.PH <= 0 Then
            If layer.LayerNumber > 1 Then
                layer.PH = _layersInfo(layer.LayerNumber - 2).PH
            Else
                layer.PH = PHDefault
            End If
        Else
            If layer.PH < PHMin Or layer.PH > PHMax Then
                layer.PH = PHDefault
            End If
        End If
    End Sub

    Private Sub ValidateCEC(ByRef layer As LayersData)
        If layer.Cec <= 0 Then
            If layer.LayerNumber > 1 Then
                layer.Cec = _layersInfo(layer.LayerNumber - 2).Cec
            Else
                layer.Cec = 0
            End If
        End If
    End Sub
    Public Sub DeleteLayer(index As Short)
        _layersInfo.RemoveAt(index)
    End Sub

    Public Sub UpdateLayer(depth As String, soilp As String, bd As String, sand As String, silt As String, om As String, ph As String, index As Short)
        Single.TryParse(depth, _layersInfo(index).Depth)
        Single.TryParse(soilp, _layersInfo(index).SoilP)
        Single.TryParse(bd, _layersInfo(index).BD)
        Single.TryParse(sand, _layersInfo(index).Sand)
        Single.TryParse(silt, _layersInfo(index).Silt)
        Single.TryParse(om, _layersInfo(index).OM)
        Single.TryParse(ph, _layersInfo(index).PH)
    End Sub
End Class
