Public Class SubareaLine4
    'line 4
    Private _lineNumber As UShort = 4
    Private _wsa As Single    'should be updated for each soil
    Private _chl As Single
    Private _chd As Single
    Private _chs As Single
    Private _chn As Single
    Private _slp As Single
    Private _slpg As Single
    Private _upn As Single = 0
    Private _ffpq As Single = 0
    Private _urbf As Single = 0

    Property LineNumber() As UShort
        Get
            Return _lineNumber
        End Get
        Set(ByVal value As UShort)
            _lineNumber = value
        End Set
    End Property

    Property Wsa() As Single
        Get
            Return _wsa
        End Get
        Set(ByVal value As Single)
            _wsa = Math.Round(value, 4)
        End Set
    End Property
    Property chl() As Single
        Get
            Return _chl
        End Get
        Set(ByVal value As Single)
            _chl = Math.Round(value, 4)
        End Set
    End Property
    Property Chd() As Single
        Get
            Return _chd
        End Get
        Set(ByVal value As Single)
            _chd = value
        End Set
    End Property
    Property Chs() As Single
        Get
            Return _chs
        End Get
        Set(ByVal value As Single)
            _chs = value
        End Set
    End Property
    Property Chn() As Single
        Get
            Return _chn
        End Get
        Set(ByVal value As Single)
            _chn = value
        End Set
    End Property
    Property Slp() As Single
        Get
            Return _slp
        End Get
        Set(ByVal value As Single)
            _slp = value
        End Set
    End Property
    Property Slpg() As Single
        Get
            Return _slpg
        End Get
        Set(ByVal value As Single)
            _slpg = value
        End Set
    End Property
    Property Upn() As Single
        Get
            Return _upn
        End Get
        Set(ByVal value As Single)
            _upn = value
        End Set
    End Property
    Property Ffpq() As Single
        Get
            Return _ffpq
        End Get
        Set(ByVal value As Single)
            _ffpq = value
        End Set
    End Property
    Property Urbf() As Single
        Get
            Return _urbf
        End Get
        Set(ByVal value As Single)
            _urbf = value
        End Set
    End Property

End Class
