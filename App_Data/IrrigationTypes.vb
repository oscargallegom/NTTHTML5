Public Class IrrigationTypes
    Private _code As Short
    Private _name As String

    Property Code() As Short
        Get
            Return _code
        End Get
        Set(ByVal value As Short)
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
End Class
