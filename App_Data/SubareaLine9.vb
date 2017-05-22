Public Class SubareaLine9
    'line 9
    Private _lineNumber As UShort = 9
    Private _bir As Single = 1      'Water Stress Factor
    Private _efi As Single = 0
    Private _vimx As Single = 0    ' Max annual appl
    Private _armn As Single = 0
    Private _armx As Single = 0    ' Max single appl.
    Private _bft As Single = 0        'N stress for fartigation
    Private _fnp4 As Single = 0
    Private _fmx As Single = 0
    Private _drt As Single = 0        'Tile drain days
    Private _fdsf As Single = 0

    Property LineNumber() As UShort
        Get
            Return _lineNumber
        End Get
        Set(ByVal value As UShort)
            _lineNumber = value
        End Set
    End Property

    Property Bir() As Single
        Get
            Return _bir
        End Get
        Set(ByVal value As Single)
            _bir = value
        End Set
    End Property
    Property Efi() As Single
        Get
            Return _efi
        End Get
        Set(ByVal value As Single)
            _efi = value
        End Set
    End Property
    Property Vimx() As Single
        Get
            Return _vimx
        End Get
        Set(ByVal value As Single)
            _vimx = value
        End Set
    End Property
    Property Armn() As Single
        Get
            Return _armn
        End Get
        Set(ByVal value As Single)
            _armn = value
        End Set
    End Property
    Property Armx() As Single
        Get
            Return _armx
        End Get
        Set(ByVal value As Single)
            _armx = value
        End Set
    End Property
    Property Bft() As Single
        Get
            Return _bft
        End Get
        Set(ByVal value As Single)
            _bft = value
        End Set
    End Property
    Property Fnp4() As Single
        Get
            Return _fnp4
        End Get
        Set(ByVal value As Single)
            _fnp4 = value
        End Set
    End Property
    Property Fmx() As Single
        Get
            Return _fmx
        End Get
        Set(ByVal value As Single)
            _fmx = value
        End Set
    End Property
    Property Drt() As Single
        Get
            Return _drt
        End Get
        Set(ByVal value As Single)
            _drt = value
        End Set
    End Property
    Property Fdsf() As Single
        Get
            Return _fdsf
        End Get
        Set(ByVal value As Single)
            _fdsf = value
        End Set
    End Property
End Class
