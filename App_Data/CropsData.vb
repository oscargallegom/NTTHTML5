Public Class sortByName  'sort by crop name
    Inherits Comparer(Of CropsData)
    Public Overrides Function Compare(ByVal x As CropsData, ByVal y As CropsData) As Integer
        Dim x1 As String = x.Name
        Dim y1 As String = y.Name
        If x1 = y1 Then Return y1.CompareTo(x1) Else Return x1.CompareTo(y1)
    End Function
End Class
Public Class CropsData
    Private _number As Short
    Private _code As String
    Private _name As String
    Private _dndc_code As Short
    Private _luNumber As Short
    Private _harvestCode As Short
    Private _plantPopulation As Single
    Private _plantPopulationAcres As Single
    Private _cn_A As Single
    Private _cn_B As Single
    Private _cn_C As Single
    Private _cn_D As Single
    Private _filterStrip As String
    Private _itil As Short
    Private _to1 As Single
    Private _tb As Single
    Private _dd As Single
    Private _daym As Single
    Private _yieldUnit As String
    Private _conversionFactor As Single
    Private _dryMatter As Single
    Private _codeName As String
    Private _state As String
    Private _additional As Boolean
    Private _heat_unit As Single

    Property Number() As Short
        Get
            Return _number
        End Get
        Set(ByVal value As Short)
            _number = value
        End Set
    End Property

    Property Code() As String
        Get
            Return _code
        End Get
        Set(ByVal value As String)
            _code = value
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

    Property dndc_Code() As Short
        Get
            Return _dndc_code
        End Get
        Set(ByVal value As Short)
            _dndc_code = value
        End Set
    End Property

    Property LuNumber() As Short
        Get
            Return _luNumber
        End Get
        Set(ByVal value As Short)
            _luNumber = value
        End Set
    End Property

    Property HarvestCode() As Short
        Get
            Return _harvestCode
        End Get
        Set(ByVal value As Short)
            _harvestCode = value
        End Set
    End Property

    Property PlantPopulation() As Single
        Get
            Return _plantPopulation
        End Get
        Set(ByVal value As Single)
            _plantPopulation = value
        End Set
    End Property
    Property PlantPopulationAcres() As Single
        Get
            Return _plantPopulationAcres
        End Get
        Set(ByVal value As Single)
            _plantPopulationAcres = value
        End Set
    End Property
    Property CN_A() As Single
        Get
            Return _cn_A
        End Get
        Set(ByVal value As Single)
            _cn_A = value
        End Set
    End Property

    Property CN_B() As Single
        Get
            Return _cn_B
        End Get
        Set(ByVal value As Single)
            _cn_B = value
        End Set
    End Property
    Property CN_C() As Single
        Get
            Return _cn_C
        End Get
        Set(ByVal value As Single)
            _cn_C = value
        End Set
    End Property
    Property CN_D() As Single
        Get
            Return _cn_D
        End Get
        Set(ByVal value As Single)
            _cn_D = value
        End Set
    End Property

    Property FilterStrip() As String
        Get
            Return _filterStrip
        End Get
        Set(ByVal value As String)
            _filterStrip = value
        End Set
    End Property

    Property Itil() As Short
        Get
            Return _itil
        End Get
        Set(ByVal value As Short)
            _itil = value
        End Set
    End Property

    Property To1() As Single
        Get
            Return _to1
        End Get
        Set(ByVal value As Single)
            _to1 = value
        End Set
    End Property
    Property Tb() As Single
        Get
            Return _tb
        End Get
        Set(ByVal value As Single)
            _tb = value
        End Set
    End Property
    Property Dd() As Single
        Get
            Return _dd
        End Get
        Set(ByVal value As Single)
            _dd = value
        End Set
    End Property
    Property Daym() As Single
        Get
            Return _daym
        End Get
        Set(ByVal value As Single)
            _daym = value
        End Set
    End Property
    Property YieldUnit() As String
        Get
            Return _yieldUnit
        End Get
        Set(ByVal value As String)
            _yieldUnit = value
        End Set
    End Property
    Property ConversionFactor() As Single
        Get
            Return _conversionFactor
        End Get
        Set(ByVal value As Single)
            _conversionFactor = value
        End Set
    End Property
    Property DryMatter() As Single
        Get
            Return _dryMatter
        End Get
        Set(ByVal value As Single)
            _dryMatter = value
        End Set
    End Property
    Property codeName() As String
        Get
            Return _codeName
        End Get
        Set(ByVal value As String)
            _codeName = value
        End Set
    End Property
    Property state() As String
        Get
            Return _state
        End Get
        Set(ByVal value As String)
            _state = value
        End Set
    End Property

    Property additional() As Boolean
        Get
            Return _additional
        End Get
        Set(ByVal value As Boolean)
            _additional = value
        End Set
    End Property

    Property heat_unit() As Single
        Get
            Return _heat_unit
        End Get
        Set(ByVal value As Single)
            _heat_unit = value
        End Set
    End Property
End Class
