Public Class FieldsData
    Private _forestry As Boolean
    'Private _updated As Boolean
    Private _selected As Boolean
    Private _number As Short
    Private _name As String
    Private _area As Single
    Private _coordinates As String
    Private _rchc As String
    Private _rchk As String
    Private _rchcVal As Single
    Private _rchkVal As Single
    Private _avgSlope As Single
    Private _soils As Button
    Public _soilsInfo As New List(Of SoilsData)
    Public _scenariosInfo As New List(Of ScenariosData)

    Property Forestry() As Boolean
        Get
            Return _forestry
        End Get
        Set(ByVal value As Boolean)
            _forestry = value
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

    Property Number() As Short
        Get
            Return _number
        End Get
        Set(ByVal value As Short)
            _number = value
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

    Property Area() As Single
        Get
            Return _area
        End Get
        Set(ByVal value As Single)
            _area = Math.Round(value, 2)
        End Set
    End Property

    Property Coordinates() As String
        Get
            Return _coordinates
        End Get
        Set(ByVal value As String)
            _coordinates = value
        End Set
    End Property

    Property Rchc() As String
        Get
            Return _rchc
        End Get
        Set(ByVal value As String)
            _rchc = value
        End Set
    End Property

    Property Rchk() As String
        Get
            Return _rchk
        End Get
        Set(ByVal value As String)
            _rchk = value
        End Set
    End Property

    Property RchcVal() As Single
        Get
            Return _rchcVal
        End Get
        Set(ByVal value As Single)
            _rchcVal = value
        End Set
    End Property

    Property RchkVal() As Single
        Get
            Return _rchkVal
        End Get
        Set(ByVal value As Single)
            _rchkVal = value
        End Set
    End Property

    Property AvgSlope() As Single
        Get
            Return _avgSlope
        End Get
        Set(ByVal value As Single)
            _avgSlope = value
        End Set
    End Property

    Property Soils() As Button
        Get
            Return _soils
        End Get
        Set(ByVal value As Button)
            _soils = value
        End Set
    End Property

    Public Sub AddScenario(ByVal scenarioName As String, fieldArea As Single, rchcval As Single, rchkval As Single, currentScenarioNumber As Short)
        Dim newScenario As New ScenariosData
        newScenario.Name = scenarioName
        _scenariosInfo.Add(newScenario)
        currentScenarioNumber = _scenariosInfo.Count - 1
        _scenariosInfo(currentScenarioNumber)._bmpsInfo.Lm = True
        Dim i As UShort = 0
        For Each _soil In _soilsInfo
            _soil.AddScenario(newScenario.Name, Forestry, fieldArea, rchcval, rchkval, i, _soil.Percentage, _soil.Slope, _scenariosInfo(currentScenarioNumber), Name, Forestry, currentScenarioNumber)
            i += 1
        Next
    End Sub

    Public Sub DeleteScenario(ByVal index As Short)
        _scenariosInfo.RemoveAt(index)
    End Sub

    Public Sub AddSoil(soil As SoilsData, ByRef _StartInfo As StartInfo, fieldArea As Single, rchcval As Single, rchkval As Single)
        _soilsInfo.Add(soil)
        _soilsInfo(_soilsInfo.Count - 1).SoilNumber = _soilsInfo.Count
        _soilsInfo(_soilsInfo.Count - 1).AddLayer(_StartInfo)
        'if the soils is added after scenarios have been created then add this scenarios and operations.
        Dim i As UShort = 0
        For Each scen In _scenariosInfo
            _soilsInfo(_soilsInfo.Count - 1).AddScenario(scen.Name, Forestry, fieldArea, rchcval, rchkval, i, _soilsInfo(_soilsInfo.Count - 1).Percentage, _soilsInfo(_soilsInfo.Count - 1).Slope, _scenariosInfo(i), Name, Forestry, i)
            If scen._operationsInfo.Count > 0 Then
                _soilsInfo(_soilsInfo.Count - 1).AddOperations(scen._operationsInfo, i)
            End If
            i += 1
        Next
    End Sub

    Public Sub AddSingleSoil(ByVal sql As String, ByVal muid As String, ByVal muname As String, ByVal musymbol As String, ByVal ssaCode As String, slope As Single, area As Single, ByRef _StartInfo As StartInfo, currenScenarioNumber As UShort)
        Dim conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Dim da As System.Data.SqlClient.SqlDataAdapter
        Dim dtResult As System.Data.DataTable
        Dim soil As SoilsData
        Try

            conn.ConnectionString = dbConnectString("Soil")
            conn.Open()
            da = New System.Data.SqlClient.SqlDataAdapter(sql, conn)
            dtResult = New System.Data.DataTable
            da.Fill(dtResult)

            For Each dataRow In dtResult.Rows
                soil = New SoilsData
                soil.Component = dataRow(0)
                If dataRow(1) Is DBNull.Value Then soil.Albedo = 0.7 Else soil.Albedo = dataRow(1)
                If slope = 0 Then soil.Slope = dataRow(2) Else soil.Slope = slope
                soil.Group = dataRow(3)
                soil.Key = muid
                If muname = "" Then soil.Name = dataRow(4) Else soil.Name = muname
                soil.Symbol = musymbol
                soil.SSACode = ssaCode
                soil.Percentage = area
                soil.Tsla = 10
                soil.Xids = 1
                soil.Wtmn = 0
                soil.Wtbl = 0
                soil.Zqt = 0
                soil.Ztk = 0
                Select Case True
                    Case dataRow("horizgen").ToString.ToLower.Contains("well") Or dataRow("horizgen").ToString.ToLower.Contains("excessively")
                        soil.Wtmx = 0
                    Case dataRow("horizgen").ToString.ToLower = ""
                        soil.Wtmx = 0
                    Case dataRow("horizgen").ToString.ToLower = "very poorly drained" Or dataRow("horizgen").ToString.ToLower = "poorly drained"
                        soil.Wtmx = 5
                        soil.Wtmn = 1
                        soil.Wtbl = 2
                        soil.Zqt = 2
                        soil.Ztk = 1
                    Case dataRow("horizgen").ToString.ToLower = "somewhat poorly drained"
                        soil.Wtmx = 6
                        soil.Wtmn = 1
                        soil.Wtbl = 2
                        soil.Zqt = 2
                        soil.Ztk = 1
                    Case Else
                        soil.Wtmx = 0
                End Select
                'soil.AddLayer()
                AddSoil(soil, _StartInfo, area, RchcVal, RchkVal)
                'If _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count > 0 Then AddAdditionalInfoToSoils(_soilsInfo.Count - 1, currentFieldNumber)
                Exit For
            Next

            conn.Close()
            conn.Dispose()
            conn = Nothing

        Catch ex As System.Exception
            'Return errors
        End Try

    End Sub

    Public Sub UpdateSoil(key As String, symbol As String, group As String, name As String, albedo As String, slope As String, percentage As String, index As Short, selected As Boolean, drCl As Short)
        _soilsInfo(index).Selected = selected
        Integer.TryParse(key, _soilsInfo(index).Key)
        _soilsInfo(index).Symbol = symbol
        _soilsInfo(index).Group = group
        _soilsInfo(index).Name = name
        Single.TryParse(albedo, _soilsInfo(index).Albedo)
        Single.TryParse(slope, _soilsInfo(index).Slope)
        Single.TryParse(percentage, _soilsInfo(index).Percentage)
        _soilsInfo(index).Wtmx = drCl
        If drCl = 0 Then
            _soilsInfo(index).Wtmn = 0
            _soilsInfo(index).Wtbl = 0
        Else
            _soilsInfo(index).Wtmn = 1
            '_soilsInfo(index).Wtmx = 4
            _soilsInfo(index).Wtbl = 2
        End If
        _soilsInfo(index).Ztk = 1
        _soilsInfo(index).Zqt = 2
        'update slope and slope lenght for the sba corresponing to this soil.
        For Each scenario In _soilsInfo(index)._scenariosInfo
            scenario._subareasInfo._line4(0).Slp = _soilsInfo(index).Slope / 100
            scenario._subareasInfo._line4(0).Slpg = calcSlopeLength(_soilsInfo(index).Slope)
        Next
    End Sub

    Public Sub DeleteSoil(index As Short, All As Boolean)
        If All = True Then
            _soilsInfo.Clear()
        Else
            _soilsInfo.RemoveAt(index)
        End If
    End Sub

End Class
