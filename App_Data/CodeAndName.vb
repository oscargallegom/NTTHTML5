Public Class CodeAndName
    Private _code As String
    Private _dndcCode As Short
    Private _name As String
    Private _nameSpanish As String
    Private _abbreviation As String
    Private _countyCode As String
    Private _lat As Single
    Private _lon As Single

    Property Code() As String
        Get
            Return _code
        End Get
        Set(ByVal value As String)
            _code = value
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

    Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Property NameSpanish() As String
        Get
            Return _nameSpanish
        End Get
        Set(ByVal value As String)
            _nameSpanish = value
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

    Property CountyCode() As String
        Get
            Return _countyCode
        End Get
        Set(ByVal value As String)
            _countyCode = value
        End Set
    End Property

    Property Lat() As Single
        Get
            Return _lat
        End Get
        Set(value As Single)
            _lat = value
        End Set
    End Property

    Property Lon() As Single
        Get
            Return _lon
        End Get
        Set(value As Single)
            _lon = value
        End Set
    End Property
End Class
