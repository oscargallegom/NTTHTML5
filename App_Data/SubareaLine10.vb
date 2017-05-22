Public Class SubareaLine10
    'line 10
    Private _lineNumber As UShort = 10
    Private _pec As Single = 0
    Private _dalg As Single = 0
    Private _vlgn As Single = 0
    Private _coww As Single = 0
    Private _ddlg As Single = 0
    Private _solq As Single = 0
    Private _sflg As Single = 0
    Private _fnp2 As Single = 0
    Private _fnp5 As Single = 0
    Private _firg As Single = 0

    Property LineNumber() As UShort
        Get
            Return _lineNumber
        End Get
        Set(ByVal value As UShort)
            _lineNumber = value
        End Set
    End Property

    Property Pec() As Single
        Get
            Return _pec
        End Get
        Set(ByVal value As Single)
            _pec = value
        End Set
    End Property
    Property Dalg() As Single
        Get
            Return _dalg
        End Get
        Set(ByVal value As Single)
            _dalg = value
        End Set
    End Property
    Property Vlgn() As Single
        Get
            Return _vlgn
        End Get
        Set(ByVal value As Single)
            _vlgn = value
        End Set
    End Property
    Property Coww() As Single
        Get
            Return _coww
        End Get
        Set(ByVal value As Single)
            _coww = value
        End Set
    End Property
    Property Ddlg() As Single
        Get
            Return _ddlg
        End Get
        Set(ByVal value As Single)
            _ddlg = value
        End Set
    End Property
    Property Solq() As Single
        Get
            Return _solq
        End Get
        Set(ByVal value As Single)
            _solq = value
        End Set
    End Property
    Property Sflg() As Single
        Get
            Return _sflg
        End Get
        Set(ByVal value As Single)
            _sflg = value
        End Set
    End Property
    Property Fnp2() As Single
        Get
            Return _fnp2
        End Get
        Set(ByVal value As Single)
            _fnp2 = value
        End Set
    End Property
    Property Fnp5() As Single
        Get
            Return _fnp5
        End Get
        Set(ByVal value As Single)
            _fnp5 = value
        End Set
    End Property
    Property Firg() As Single
        Get
            Return _firg
        End Get
        Set(ByVal value As Single)
            _firg = value
        End Set
    End Property
End Class
