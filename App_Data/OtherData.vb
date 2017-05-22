Public Class OtherData
    Public _selected As Boolean
    Public _id As Short
    Public _name As String
    Public _values As Single

    Property Selected() As Boolean
        Get
            Return _selected
        End Get
        Set(ByVal value As Boolean)
            _selected = value
        End Set
    End Property

    Property Id() As Short
        Get
            Return _id
        End Get
        Set(ByVal value As Short)
            _id = value
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

    Property Values() As Single
        Get
            Return _values
        End Get
        Set(ByVal value As Single)
            _values = value
        End Set
    End Property

End Class
