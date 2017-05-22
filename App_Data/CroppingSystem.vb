Public Class CroppingSystem
    Private _code As String
    Private _name As String
    Private _till As String

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

    Property Till() As String
        Get
            Return _till
        End Get
        Set(ByVal value As String)
            _till = value
        End Set
    End Property
End Class
