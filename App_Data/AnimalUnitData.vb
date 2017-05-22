Public Class sortByCodeName  'sort by crop name
    Inherits Comparer(Of AnimalUnitsData)
    Public Overrides Function Compare(ByVal x As AnimalUnitsData, ByVal y As AnimalUnitsData) As Integer
        Dim x1 As String = x.Name
        Dim y1 As String = y.Name
        If x1 = y1 Then Return y1.CompareTo(x1) Else Return x1.CompareTo(y1)
    End Function
End Class
Public Class AnimalUnitsData
    Private _number As String
    Private _name As String
    Private _abbreviation As String
    Private _dndcCode As Short
    Private _dryManure As Single
    Private _conversionUnit As Single
    Private _no3 As Single
    Private _po4 As Single
    Private _orgN As Single
    Private _orgP As Single
    Private _nh3 As Single
    Private _k As Single

    Property Number() As String
        Get
            Return _number
        End Get
        Set(ByVal value As String)
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

    Property Abbreviation() As String
        Get
            Return _abbreviation
        End Get
        Set(ByVal value As String)
            _abbreviation = value
        End Set
    End Property

    Property DndcCode() As Short
        Get
            Return _dndcCode
        End Get
        Set(ByVal value As Short)
            _dndcCode = value
        End Set
    End Property

    Property DryManure() As Single
        Get
            Return _dryManure
        End Get
        Set(ByVal value As Single)
            _dryManure = value
        End Set
    End Property

    Property ConversionUnit() As Single
        Get
            Return _conversionUnit
        End Get
        Set(ByVal value As Single)
            _conversionUnit = value
        End Set
    End Property

    Property No3() As Single
        Get
            Return _no3
        End Get
        Set(ByVal value As Single)            
            _no3 = value
        End Set
    End Property

    Property Po4() As Single
        Get
            Return _po4
        End Get
        Set(ByVal value As Single)
            _po4 = value
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

    Property K() As Single
        Get
            Return _k
        End Get
        Set(ByVal value As Single)
            _k = value
        End Set
    End Property

    Property Nh3() As Single
        Get
            Return _nh3
        End Get
        Set(ByVal value As Single)
            _nh3 = value
        End Set
    End Property
End Class