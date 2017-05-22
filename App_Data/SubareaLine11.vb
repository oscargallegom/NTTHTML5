Public Class SubareaLine11
    'line 11
    Private _lineNumber As UShort = 11
    Private _ny1 As Short = 0
    Private _ny2 As Short = 0
    Private _ny3 As Short = 0
    Private _ny4 As Short = 0

    Property LineNumber() As UShort
        Get
            Return _lineNumber
        End Get
        Set(ByVal value As UShort)
            _lineNumber = value
        End Set
    End Property

    Property Ny1() As Short
        Get
            Return _ny1
        End Get
        Set(ByVal value As Short)
            _ny1 = value
        End Set
    End Property

    Property Ny2() As Short
        Get
            Return _ny2
        End Get
        Set(ByVal value As Short)
            _ny2 = value
        End Set
    End Property
    Property Ny3() As Short
        Get
            Return _ny3
        End Get
        Set(ByVal value As Short)
            _ny3 = value
        End Set
    End Property
    Property Ny4() As Short
        Get
            Return _ny4
        End Get
        Set(ByVal value As Short)
            _ny4 = value
        End Set
    End Property
End Class