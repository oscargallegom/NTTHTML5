Public Class SubareaLine12
    'line 12
    Private _lineNumber As UShort = 12
    Private _xtp1 As Single = 0
    Private _xtp2 As Single = 0
    Private _xtp3 As Single = 0
    Private _xtp4 As Single = 0

    Property LineNumber() As UShort
        Get
            Return _lineNumber
        End Get
        Set(ByVal value As UShort)
            _lineNumber = value
        End Set
    End Property

    Property Xtp1() As Single
        Get
            Return _xtp1
        End Get
        Set(ByVal value As Single)
            _xtp1 = value
        End Set
    End Property

    Property Xtp2() As Single
        Get
            Return _xtp2
        End Get
        Set(ByVal value As Single)
            _xtp2 = value
        End Set
    End Property
    Property Xtp3() As Single
        Get
            Return _xtp3
        End Get
        Set(ByVal value As Single)
            _xtp3 = value
        End Set
    End Property
    Property Xtp4() As Single
        Get
            Return _xtp4
        End Get
        Set(ByVal value As Single)
            _xtp4 = value
        End Set
    End Property
End Class
