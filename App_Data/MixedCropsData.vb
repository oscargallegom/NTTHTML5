Public Class MixedCropsData
    Private _selected As Boolean
    Private _code As String
    Private _name As String
    Private _percentage As Single
    Private _hu As Single
    Private _cn As Single
    Private _pp As Single
    Private _ppDefault As Single

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

    Property Percentage() As Single
        Get
            Return _percentage
        End Get
        Set(ByVal value As Single)
            _percentage = value
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

    Property HU() As Single
        Get
            Return _hu
        End Get
        Set(ByVal value As Single)
            _hu = value
        End Set
    End Property

    Property CN() As Single
        Get
            Return _cn
        End Get
        Set(ByVal value As Single)
            _cn = value
        End Set
    End Property

    Property PP() As Single
        Get
            Return _pp
        End Get
        Set(ByVal value As Single)
            _pp = value
        End Set
    End Property

    Property PPDefault() As Single
        Get
            Return _ppDefault
        End Get
        Set(ByVal value As Single)
            _ppDefault = value
        End Set
    End Property
End Class
