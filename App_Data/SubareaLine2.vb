Public Class SubareaLine2
    Private _lineNumber As UShort = 2
    Private _inps As UShort
    Private _iops As UShort
    Private _iow As Short
    Private _ii As Short
    Private _iapl As Short
    Private _nvcn As Short
    Private _iwth As Short = 1
    Private _ipts As Short
    Private _isao As Short
    Private _luns As Short
    Private _imw As Short

    Property LineNumber() As UShort
        Get
            Return _lineNumber
        End Get
        Set(ByVal value As UShort)
            _lineNumber = value
        End Set
    End Property

    Property Inps() As UShort
        Get
            Return _inps
        End Get
        Set(ByVal value As UShort)
            _inps = value
        End Set
    End Property
    Property Iops() As UShort
        Get
            Return _iops
        End Get
        Set(ByVal value As UShort)
            _iops = value
        End Set
    End Property
    Property Iow() As Short
        Get
            Return _iow
        End Get
        Set(ByVal value As Short)
            _iow = value
        End Set
    End Property
    Property Ii() As Short
        Get
            Return _ii
        End Get
        Set(ByVal value As Short)
            _ii = value
        End Set
    End Property
    Property Iapl() As Short
        Get
            Return _iapl
        End Get
        Set(ByVal value As Short)
            _iapl = value
        End Set
    End Property
    Property Nvcn() As Short
        Get
            Return _nvcn
        End Get
        Set(ByVal value As Short)
            _nvcn = value
        End Set
    End Property
    Property Iwth() As Short
        Get
            Return _iwth
        End Get
        Set(ByVal value As Short)
            _iwth = value
        End Set
    End Property
    Property Ipts() As Short
        Get
            Return _ipts
        End Get
        Set(ByVal value As Short)
            _ipts = value
        End Set
    End Property
    Property Isao() As Short
        Get
            Return _isao
        End Get
        Set(ByVal value As Short)
            _isao = value
        End Set
    End Property
    Property Luns() As Short
        Get
            Return _luns
        End Get
        Set(ByVal value As Short)
            _luns = value
        End Set
    End Property
    Property Imw() As Short
        Get
            Return _imw
        End Get
        Set(ByVal value As Short)
            _imw = value
        End Set
    End Property
End Class
