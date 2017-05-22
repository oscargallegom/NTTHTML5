Public Class SubareaLine5
    'line 5
    Private _lineNumber As UShort = 5
    Private _rchl As Single
    Private _rchd As Single = 0
    Private _rcbw As Single = 0
    Private _rctw As Single = 0
    Private _rchs As Single = 0
    Private _rchn As Single = 0
    Private _rchc As Single
    Private _rchk As Single
    Private _rfpw As Single = 0
    Private _rfpl As Single = 0

    Property LineNumber() As UShort
        Get
            Return _lineNumber
        End Get
        Set(ByVal value As UShort)
            _lineNumber = value
        End Set
    End Property

    Property Rchl() As Single
        Get
            Return _rchl
        End Get
        Set(ByVal value As Single)
            _rchl = Math.Round(value, 4)
        End Set
    End Property
    Property Rchd() As Single
        Get
            Return _rchd
        End Get
        Set(ByVal value As Single)
            _rchd = value
        End Set
    End Property
    Property Rcbw() As Single
        Get
            Return _rcbw
        End Get
        Set(ByVal value As Single)
            _rcbw = value
        End Set
    End Property
    Property Rctw() As Single
        Get
            Return _rctw
        End Get
        Set(ByVal value As Single)
            _rctw = value
        End Set
    End Property
    Property Rchs() As Single
        Get
            Return _rchs
        End Get
        Set(ByVal value As Single)
            _rchs = value
        End Set
    End Property
    Property Rchn() As Single
        Get
            Return _rchn
        End Get
        Set(ByVal value As Single)
            _rchn = value
        End Set
    End Property
    Property Rchc() As Single
        Get
            Return _rchc
        End Get
        Set(ByVal value As Single)
            _rchc = value
        End Set
    End Property
    Property Rchk() As Single
        Get
            Return _rchk
        End Get
        Set(ByVal value As Single)
            _rchk = value
        End Set
    End Property
    Property Rfpw() As Single
        Get
            Return _rfpw
        End Get
        Set(ByVal value As Single)
            _rfpw = value
        End Set
    End Property
    Property Rfpl() As Single
        Get
            Return _rfpl
        End Get
        Set(ByVal value As Single)
            _rfpl = value
        End Set
    End Property
End Class
