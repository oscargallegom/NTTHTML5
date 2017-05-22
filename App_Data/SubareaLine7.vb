Public Class SubareaLine7
    'line 7
    Private _lineNumber As UShort = 7
    Private _rshc As Single = 0
    Private _rsdp As Single = 0
    Private _rsbd As Single = 0
    Private _pcof As Single = 0
    Private _bcof As Single = 0
    Private _bffl As Single = 0

    Property LineNumber() As UShort
        Get
            Return _lineNumber
        End Get
        Set(ByVal value As UShort)
            _lineNumber = value
        End Set
    End Property

    Property Rshc() As Single
        Get
            Return _rshc
        End Get
        Set(ByVal value As Single)
            _rshc = value
        End Set
    End Property
    Property Rsdp() As Single
        Get
            Return _rsdp
        End Get
        Set(ByVal value As Single)
            _rsdp = value
        End Set
    End Property
    Property Rsbd() As Single
        Get
            Return _rsbd
        End Get
        Set(ByVal value As Single)
            _rsbd = value
        End Set
    End Property
    Property Pcof() As Single
        Get
            Return _pcof
        End Get
        Set(ByVal value As Single)
            _pcof = value
        End Set
    End Property
    Property Bcof() As Single
        Get
            Return _bcof
        End Get
        Set(ByVal value As Single)
            _bcof = value
        End Set
    End Property
    Property Bffl() As Single
        Get
            Return _bffl
        End Get
        Set(ByVal value As Single)
            _bffl = value
        End Set
    End Property
End Class
