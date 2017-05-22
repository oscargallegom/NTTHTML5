Public Class ParmsData
    Private _code As String
    Private _name As String
    Private _value1 As Single
    Private _range1 As Single
    Private _range2 As Single
    Private _state As String

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

    Property Value1() As Single
        Get
            Return _value1
        End Get
        Set(ByVal value As Single)
            _value1 = value
        End Set
    End Property

    Property Range1() As Single
        Get
            Return _range1
        End Get
        Set(ByVal value As Single)
            _range1 = value
        End Set
    End Property

    Property Range2() As Single
        Get
            Return _range2
        End Get
        Set(ByVal value As Single)
            _range2 = value
        End Set
    End Property

    Property State1 As String
        Get
            Return _state
        End Get
        Set(value As String)
            _state = value
        End Set
    End Property


End Class
