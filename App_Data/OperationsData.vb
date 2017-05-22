Imports System.ComponentModel
Imports HUCal

Public Class sortByDate
    Inherits Comparer(Of OperationsData)
    Public Overrides Function Compare(ByVal x As OperationsData, ByVal y As OperationsData) As Integer
        Dim x1 As String = x.Year.ToString("D2") & x.Month.ToString("D2") & x.Day.ToString("D2") & x.EventId.ToString("D1")
        Dim y1 As String = y.Year.ToString("D2") & y.Month.ToString("D2") & y.Day.ToString("D2") & y.EventId.ToString("D1")
        If x1 = y1 Then Return y1.CompareTo(x1) Else Return x1.CompareTo(y1)
    End Function
End Class

Public Class OperationsData
    Implements INotifyPropertyChanged

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) _
        Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(info As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
    End Sub

    Private _selected As Boolean
    Private _eventId As UShort
    Private _year As UShort
    Private _month As UShort
    Private _day As UShort
    Private _period As UShort
    Private _apexOp As Short
    Private _apexOpName As String
    Private _apexOpAbbreviation As String
    Private _tractorId As UShort = 0
    Private _apexCrop As Short
    Private _apexCropName As String
    Private _ApexTillCode As Short
    Private _ApexTillName As String
    Private _var9 As Single
    Private _apexOpType As Short
    Private _apexOpTypeName As String
    Private _apexOpv1 As Single
    Private _apexOpv2 As Single
    Private _opVal1 As Single
    Private _opVal2 As Single
    Private _opVal3 As Single
    Private _opVal4 As Single
    Private _opVal5 As Single
    Private _opVal6 As Single
    Private _opVal7 As Single
    Private _orgN As Single
    Private _no3 As Single
    Private _orgP As Single
    Private _po4 As Single
    Private _k As Single
    Private _nh3 As Single
    Private _index As Short
    Private _scenario As String
    Private _luNumber As UShort
    Private _convertionUnit As Single
    'Private _crops As New List(Of CropsData)
    Private _mixedCropsData As String

    Public Sub setCN(value As Single, soilGroup As String, fieldNumber As UShort, soilGroup1 As String, currentFieldNumber As UShort, _startInfo As StartInfo, sess As String)
        Dim groupCode As String = soilGroup
        Dim hu As New NTTCalcHU.ServiceSoapClient  'CALCULATE HU USING CROP, LAT, and LONG Location: ISS=T-NN\NTTCalHU Published C:\NTTDeployed\NTTWebService\calcHU - Source=E:\Borrar\NTTWebServer
        'Dim hu1 As New NTTCalcHU1.Service1SoapClient  'CALCULATE HU USING CROP, LAT, LONG, AND PLANTING DATE. Location: ISS=T-NN\NTTCalHU1 Published C:\NTTDeployed\NTTWebService\calcHU1 - Source=E:\Borrar\NTTWebServer
        If Not ApexOpAbbreviation Is Nothing AndAlso fieldNumber > currentFieldNumber AndAlso ApexOpAbbreviation.Trim = planting Then
            _crops = System.Web.HttpContext.Current.Session("crops")
            Dim getHu As New GetHU
            If _startInfo.Wp1Name Is Nothing Then
                _startInfo.Wp1Name = "CHINAG" : _startInfo.Wp1Code = 999
            Else
                If _startInfo.Wp1Name.Trim = "" Then _startInfo.Wp1Name = "CHINAG" : _startInfo.Wp1Code = 999
            End If
            If _startInfo.WindName Is Nothing Then
                _startInfo.WindName = "CHINAG" : _startInfo.WindCode = 999
            Else
                If _startInfo.WindName.Trim = "" Then _startInfo.WindName = "CHINAG" : _startInfo.WindCode = 999
            End If
            OpVal1 = getHu.calcHU(value, wp1Files & "\" & _startInfo.Wp1Name.Trim & ".WP1", folder & "\App_Data\PHUCRP.DAT")
            If _startInfo.stationWay <> own Then
                OpVal1 = Math.Round(calcHU(value, _crops, _startInfo), 2)
                'OpVal1 = Math.Round(hu.getHU(value, _startInfo.weatherLat, _startInfo.weatherLon), 2, sAPEXBasLoc1) 'since we do not have the folder yet we calculate in the simualtion process.
                'Changed back to take the HU from the database according to Ali 11/2/2016  - Oscar Gallego 11/2/2016
                OpVal1 = _crops.Where(Function(x) x.Number = value).Select(Function(x) x.heat_unit).SingleOrDefault
                'If System.Net.Dns.GetHostName = "T-NN" Then
                '    OpVal1 = Math.Round(hu1.getHU(value, _startInfo.weatherLat, _startInfo.weatherLon, hu1.jdt(Day, Month, 0)), 2)
                'End If
            End If
            If OpVal1 = 0 Then
                OpVal1 = _crops.Where(Function(x) x.Number = value).Select(Function(x) x.heat_unit).SingleOrDefault
            End If
            For Each crop In _crops
                If crop.Number = value Then
                    'take curve number
                    OpVal2 = 0
                    If soilGroup = "" Then
                        'groupCode = _fieldsInfo1(currentFieldNumber)._soilsInfo(currentSoilNumber).Group
                        groupCode = soilGroup1
                    End If
                    Select Case True
                        Case groupCode.Contains("A")
                            OpVal2 = crop.CN_A
                        Case groupCode.Contains("B")
                            OpVal2 = crop.CN_B
                        Case groupCode.Contains("C")
                            OpVal2 = crop.CN_C
                        Case groupCode.Contains("D")
                            OpVal2 = crop.CN_D
                    End Select
                    OpVal2 *= -1
                    Exit For
                End If
            Next
        End If
    End Sub

    Public Sub setAnimalRate(kindOfanimal As Short, totalAcres As Single)
        Dim animalCode As Short = kindOfanimal
        If kindOfanimal = 0 Then animalCode = _ApexTillCode
        Select Case ApexOpAbbreviation.Trim
            Case grazing
                For Each animal In _animals
                    If animal.Number.Split("|")(0) = animalCode Or animal.Name = _ApexTillName Then
                        'take convertion unit
                        ConvertionUnit = animal.ConversionUnit
                        OpVal1 = Math.Round((totalAcres * ac_to_ha) / (ConvertionUnit * ApexOpv1), 4)
                        Exit For
                        '_herdInfo.Ncow = ConvertionUnit * ApexOpv1
                        'Select Case ApexTillName
                        '    Case "Dairy"    '1
                        '        _herdInfo.Dump = 3.9
                        '        _herdInfo.Gzrt = 9.1
                        '        _herdInfo.Vurn = 11.8
                        '        _herdInfo.Idmu = 43
                        '    Case "Dairy-dry cow"    '2
                        '        _herdInfo.Dump = 5.5
                        '        _herdInfo.Gzrt = 9.1
                        '        _herdInfo.Vurn = 11.8
                        '        _herdInfo.Idmu = 43
                        '    Case "Dairy-calf and heifer"     '3
                        '        _herdInfo.Dump = 5.5
                        '        _herdInfo.Gzrt = 9.1
                        '        _herdInfo.Vurn = 11.8
                        '        _herdInfo.Idmu = 43
                        '    Case "Dairy bull"     '4
                        '        _herdInfo.Dump = 3.9
                        '        _herdInfo.Gzrt = 9.1
                        '        _herdInfo.Vurn = 8.2
                        '        _herdInfo.Idmu = 43
                        '    Case "Beef"    '5
                        '        _herdInfo.Dump = 3.9
                        '        _herdInfo.Gzrt = 9.1
                        '        _herdInfo.Vurn = 8.2
                        '        _herdInfo.Idmu = 44
                        '    Case "Beef-bull"     '6
                        '        _herdInfo.Dump = 3.9
                        '        _herdInfo.Gzrt = 9.1
                        '        _herdInfo.Vurn = 8.2
                        '        _herdInfo.Idmu = 44
                        '    Case "Beef-feeder yearling"    '7
                        '        _herdInfo.Dump = 3.9
                        '        _herdInfo.Gzrt = 9.1
                        '        _herdInfo.Vurn = 8.2
                        '        _herdInfo.Idmu = 44
                        '    Case "Beef-calf"    '8
                        '        _herdInfo.Dump = 3.9
                        '        _herdInfo.Gzrt = 9.1
                        '        _herdInfo.Vurn = 8.2
                        '        _herdInfo.Idmu = 44
                        '    Case "Sheep"    '9
                        '        _herdInfo.Dump = 5
                        '        _herdInfo.Gzrt = 9.0
                        '        _herdInfo.Vurn = 6.8
                        '        _herdInfo.Idmu = 47
                        '    Case "Horse"   '10
                        '        _herdInfo.Dump = 6.8
                        '        _herdInfo.Gzrt = 9.1
                        '        _herdInfo.Vurn = 4.5
                        '        _herdInfo.Idmu = 49
                        '    Case "Llama"    '11
                        '        _herdInfo.Dump = 5
                        '        _herdInfo.Gzrt = 9.1
                        '        _herdInfo.Vurn = 6.8
                        '        _herdInfo.Idmu = 52
                        '    Case "Alpaca"   '12
                        '        _herdInfo.Dump = 5.1
                        '        _herdInfo.Gzrt = 9.1
                        '        _herdInfo.Vurn = 6.8
                        '        _herdInfo.Idmu = 52
                        '    Case "Buffalo"   '13
                        '        _herdInfo.Dump = 3.9
                        '        _herdInfo.Gzrt = 9.1
                        '        _herdInfo.Vurn = 8.2
                        '        _herdInfo.Idmu = 52
                        '    Case "Emu (breeding stock)"   '14
                        '        _herdInfo.Dump = 10
                        '        _herdInfo.Gzrt = 9.1
                        '        _herdInfo.Vurn = 6.8
                        '        _herdInfo.Idmu = 52
                        '    Case "Emu (young birds)"    '15
                        '        _herdInfo.Dump = 9.8
                        '        _herdInfo.Gzrt = 9.0
                        '        _herdInfo.Vurn = 6.8
                        '        _herdInfo.Idmu = 52
                        '    Case "Swine"    '16
                        '        _herdInfo.Dump = 5
                        '        _herdInfo.Gzrt = 9.1
                        '        _herdInfo.Vurn = 17.7
                        '        _herdInfo.Idmu = 46
                        '    Case "Broiler"    '17
                        '        _herdInfo.Dump = 10.4
                        '        _herdInfo.Gzrt = 10
                        '        _herdInfo.Vurn = 6.8
                        '        _herdInfo.Idmu = 52
                        '    Case Else
                        '        _herdInfo.Dump = 12
                        '        _herdInfo.Gzrt = 9.08
                        '        _herdInfo.Idmu = 56
                        'End Select
                    End If
                Next
        End Select
    End Sub

    Public Sub setOpval1(value As Single, soilGroup As String)
        If ApexOpAbbreviation Is Nothing Then Exit Sub
        If ApexOpAbbreviation.Trim = String.Empty Or ApexOpAbbreviation.Trim = "" Then Exit Sub
        Select Case ApexOpAbbreviation.Trim
            Case fertilizer
                If Val(ApexTillCode) = 57 Or ApexOpType.ToString.Contains("Liquid Manure") Then
                    '8.25=lbs/gal, 0.5% dry matter, and 1121.8 kg/ha
                    OpVal1 = Math.Round(ApexOpv1 * 8.25 * 0.005 * 1121.8, 2)  'kg/ha of fertilizer applied converted from liquid manure
                Else
                    OpVal1 = Math.Round(ApexOpv1 * lbs_to_kg / ac_to_ha, 2)  'kg/ha of fertilizer applied converted from lbs/ac
                End If
            Case irrigation
                OpVal1 = ApexOpv1 * in_to_mm  'irrigation volume from inches to mm.
            Case planting
                If _crops.Count <= 0 Then _crops = System.Web.HttpContext.Current.Session("crops")
                For Each crop In _crops
                    If crop.Number = ApexCrop Then
                        'take lu number
                        LuNumber = crop.LuNumber
                        If ApexOpv1 = 0 Then
                            If ApexCrop = cropRoad Then Exit For
                            'ApexOpv1 = crop.PlantPopulation
                        Else
                            If ApexOpv1 / ac_to_m2 < 1 Then
                                OpVal5 = Math.Round(ApexOpv1 / ac_to_m2, 6)  'plant population converte from ac to m2 if it is not tree
                            Else
                                OpVal5 = Math.Round(ApexOpv1 / ac_to_m2, 0)  'plant population converte from ac to m2 if it is not tree
                            End If
                            If LuNumber = 28 Then
                                OpVal5 = Math.Round(ApexOpv1 / ac_to_ha, 0)  'plant population converte from ac to ha if it is tree
                            End If
                        End If
                        Exit For
                    End If
                Next
            Case liming
                OpVal1 = ApexOpv1 / tha_to_tac        'converts input t/ac to APEX t/ha
        End Select
    End Sub

    Private Sub setOpval2(value As Single)
        If ApexOpAbbreviation Is Nothing Then Exit Sub
        Select Case ApexOpAbbreviation.Trim
            Case fertilizer
                OpVal2 = ApexOpv2 * in_to_mm
            Case irrigation
                OpVal4 = 1 - Val(ApexOpv2)
        End Select
    End Sub

    Property Year() As Short
        Get
            Return _year
        End Get
        Set(ByVal value As Short)
            _year = value
        End Set
    End Property

    Property Month() As Short
        Get
            Return _month
        End Get
        Set(ByVal value As Short)
            _month = value
        End Set
    End Property

    Property Day() As Short
        Get
            Return _day
        End Get
        Set(ByVal value As Short)
            _day = value
        End Set
    End Property

    Property Period() As Short
        Get
            Return _period
        End Get
        Set(ByVal value As Short)
            _period = value
        End Set
    End Property

    Property ApexOp() As Short
        Get
            Return _apexOp
        End Get
        Set(ByVal value As Short)
            _apexOp = value
        End Set
    End Property

    Property EventId() As Short
        Get
            Return _eventId
        End Get
        Set(ByVal value As Short)
            _eventId = value
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

    Property TractorId() As UShort
        Get
            Return _tractorId
        End Get
        Set(ByVal value As UShort)
            _tractorId = value
        End Set
    End Property

    Property var9() As Single
        Get
            Return _var9
        End Get
        Set(ByVal value As Single)
            _var9 = value
        End Set
    End Property

    Property ApexCrop() As Short
        Get
            Return _apexCrop
        End Get
        Set(ByVal value As Short)
            _apexCrop = value
        End Set
    End Property

    Property ApexCropName() As String
        Get
            Return _apexCropName
        End Get
        Set(ByVal value As String)
            _apexCropName = value
        End Set
    End Property

    Property ApexOpName() As String
        Get
            Return _apexOpName
        End Get
        Set(ByVal value As String)
            _apexOpName = value
        End Set
    End Property

    Property ApexOpAbbreviation() As String
        Get
            Return _apexOpAbbreviation
        End Get
        Set(ByVal value As String)
            _apexOpAbbreviation = value
        End Set
    End Property

    Property ApexTillCode() As Short
        Get
            Return _ApexTillCode
        End Get
        Set(ByVal value As Short)
            _ApexTillCode = value
        End Set
    End Property

    Property ApexTillName() As String
        Get
            Return _ApexTillName
        End Get
        Set(ByVal value As String)
            _ApexTillName = value
        End Set
    End Property

    Property ApexOpType() As Short
        Get
            Return _apexOpType
        End Get
        Set(ByVal value As Short)
            _apexOpType = value
        End Set
    End Property

    Property ApexOpTypeName() As String
        Get
            Return _apexOpTypeName
        End Get
        Set(ByVal value As String)
            _apexOpTypeName = value
        End Set
    End Property

    Property ApexOpv1() As Single
        Get
            Return _apexOpv1
        End Get
        Set(ByVal value As Single)
            _apexOpv1 = value
            'Me.NotifyPropertyChanged("ApexOpv1")
            Call setOpval1(value, "")
        End Set
    End Property

    Property ApexOpv2() As Single
        Get
            Return _apexOpv2
        End Get
        Set(ByVal value As Single)
            _apexOpv2 = value
            Call setOpval2(value)
        End Set
    End Property

    Property OpVal1() As Single
        Get
            Return _opVal1
        End Get
        Set(ByVal value As Single)
            _opVal1 = value
        End Set
    End Property
    Property OpVal2() As Single
        Get
            Return _opVal2
        End Get
        Set(ByVal value As Single)
            _opVal2 = value
        End Set
    End Property
    Property OpVal3() As Single
        Get
            Return _opVal3
        End Get
        Set(ByVal value As Single)
            _opVal3 = value
        End Set
    End Property
    Property OpVal4() As Single
        Get
            Return _opVal4
        End Get
        Set(ByVal value As Single)
            _opVal4 = value
        End Set
    End Property
    Property OpVal5() As Single
        Get
            Return _opVal5
        End Get
        Set(ByVal value As Single)
            _opVal5 = value
        End Set
    End Property
    Property OpVal6() As Single
        Get
            Return _opVal6
        End Get
        Set(ByVal value As Single)
            _opVal6 = value
        End Set
    End Property
    Property OpVal7() As Single
        Get
            Return _opVal7
        End Get
        Set(ByVal value As Single)
            _opVal7 = value
        End Set
    End Property

    Property OrgN() As Single
        Get
            Return _orgN
        End Get
        Set(ByVal value As Single)
            _orgN = value
        End Set
    End Property

    Property OrgP() As Single
        Get
            Return _orgP
        End Get
        Set(ByVal value As Single)
            _orgP = value
        End Set
    End Property

    Property NO3() As Single
        Get
            Return _no3
        End Get
        Set(ByVal value As Single)
            _no3 = value
        End Set
    End Property

    Property PO4() As Single
        Get
            Return _po4
        End Get
        Set(ByVal value As Single)
            _po4 = value
        End Set
    End Property

    Property K() As Single
        Get
            Return _k
        End Get
        Set(ByVal value As Single)
            _k = value
        End Set
    End Property

    Property NH3() As Single
        Get
            Return _nh3
        End Get
        Set(ByVal value As Single)
            _nh3 = value
        End Set
    End Property

    Property Index() As Short
        Get
            Return _index
        End Get
        Set(ByVal value As Short)
            _index = value
        End Set
    End Property

    Property Scenario() As String
        Get
            Return _scenario
        End Get
        Set(ByVal value As String)
            _scenario = value
        End Set
    End Property

    Property LuNumber() As UShort
        Get
            Return _luNumber
        End Get
        Set(ByVal value As UShort)
            _luNumber = value
        End Set
    End Property

    Property ConvertionUnit() As Single
        Get
            Return _convertionUnit
        End Get
        Set(ByVal value As Single)
            _convertionUnit = value
        End Set
    End Property

    Property MixedCropData() As String
        Get
            Return _mixedCropsData
        End Get
        Set(ByVal value As String)
            _mixedCropsData = value
        End Set
    End Property

    Private Function getHu(p1 As Short, Wp1Name As String) As Single
        Throw New NotImplementedException
    End Function

End Class
