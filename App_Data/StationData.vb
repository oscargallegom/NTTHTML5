Public Class StationInfo
    Private _initialYear As Short
    Private _finalYear As Short
    Private _code As String
    Private _name As String
    Private _windCode As String
    Private _windName As String
    Private _wp1Code As String
    Private _wp1Name As String
    Private _WSType As String

    Property InitialYear() As Short
        Get
            Return _initialYear
        End Get
        Set(ByVal value As Short)
            _initialYear = value
        End Set
    End Property

    Property FinalYear() As Short
        Get
            Return _finalYear
        End Get
        Set(ByVal value As Short)
            _finalYear = value
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
    Property WindName() As String
        Get
            Return _windName
        End Get
        Set(ByVal value As String)
            _windName = value
        End Set
    End Property
    Property WindCode() As String
        Get
            Return _windCode
        End Get
        Set(ByVal value As String)
            _windCode = value
        End Set
    End Property
    Property Wp1Name() As String
        Get
            Return _wp1Name
        End Get
        Set(ByVal value As String)
            _wp1Name = value
        End Set
    End Property
    Property Wp1Code() As String
        Get
            Return _wp1Code
        End Get
        Set(ByVal value As String)
            _wp1Code = value
        End Set
    End Property
    Property WSType() As String
        Get
            Return _WSType
        End Get
        Set(ByVal value As String)
            _WSType = value
        End Set
    End Property

End Class
