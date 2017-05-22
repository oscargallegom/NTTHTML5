Public Class SubareaLine3
    'line 3
    Private _lineNumber As UShort = 3
    Private _sno As Single = 0
    Private _stdo As Single = 0
    Private _yct As Single = 0
    Private _xct As Single = 0
    Private _azm As Single = 0
    Private _fl As Single = 0
    Private _fw As Single = 0
    Private _angl As Single = 0

    Property LineNumber() As UShort
        Get
            Return _lineNumber
        End Get
        Set(ByVal value As UShort)
            _lineNumber = value
        End Set
    End Property

    Property Sno() As Single
        Get
            Return _sno
        End Get
        Set(ByVal value As Single)
            _sno = value
        End Set
    End Property
    Property Stdo() As Single
        Get
            Return _stdo
        End Get
        Set(ByVal value As Single)
            _stdo = value
        End Set
    End Property
    Property Yct() As Single
        Get
            Return _yct
        End Get
        Set(ByVal value As Single)
            _yct = value
        End Set
    End Property
    Property Xct() As Single
        Get
            Return _xct
        End Get
        Set(ByVal value As Single)
            _xct = value
        End Set
    End Property
    Property Azm() As Single
        Get
            Return _azm
        End Get
        Set(ByVal value As Single)
            _azm = value
        End Set
    End Property
    Property Fl() As Single
        Get
            Return _fl
        End Get
        Set(ByVal value As Single)
            _fl = value
        End Set
    End Property
    Property Fw() As Single
        Get
            Return _fw
        End Get
        Set(ByVal value As Single)
            _fw = value
        End Set
    End Property
    Property Angl() As Single
        Get
            Return _angl
        End Get
        Set(ByVal value As Single)
            _angl = value
        End Set
    End Property
End Class
