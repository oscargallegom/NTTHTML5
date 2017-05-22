Public Class LayersData
    Private _selected As Boolean
    Private _soilKey As Integer
    Private _layerNumber As String
    Private _depth As Single
    Private _bd As Single
    Private _sand As Single
    Private _silt As Single
    Private _om As Single
    Private _ph As Single
    Private _bdd As Single
    Private _satc As Single
    Private _soilP As Single
    Private _wn As Single
    Private _uw As Single
    Private _smb As Single
    Private _rsd As Single
    Private _rok As Single
    Private _psp As Single
    Private _fc As Single
    Private _cnds As Single
    Private _cac As Single
    Private _cec As Single

    Property Selected() As Boolean
        Get
            Return _selected
        End Get
        Set(ByVal value As Boolean)
            _selected = value
        End Set
    End Property

    Property LayerNumber() As Short
        Get
            Return _layerNumber
        End Get
        Set(ByVal value As Short)
            _layerNumber = value
        End Set
    End Property

    Property Depth() As Single
        Get
            Return _depth
        End Get
        Set(ByVal value As Single)
            _depth = value
        End Set
    End Property

    Property SoilP() As Single
        Get
            Return _soilP
        End Get
        Set(ByVal value As Single)
            _soilP = value
        End Set
    End Property

    Property BD() As Single
        Get
            Return _bd
        End Get
        Set(ByVal value As Single)
            _bd = value
        End Set
    End Property

    Property Sand() As Single
        Get
            Return _sand
        End Get
        Set(ByVal value As Single)
            _sand = value
        End Set
    End Property

    Property Silt() As Single
        Get
            Return _silt
        End Get
        Set(ByVal value As Single)
            _silt = value
        End Set
    End Property

    Property OM() As Single
        Get
            Return _om
        End Get
        Set(ByVal value As Single)
            _om = value
        End Set
    End Property

    Property PH() As Single
        Get
            Return _ph
        End Get
        Set(ByVal value As Single)
            _ph = value
        End Set
    End Property

    Property Bdd() As Single
        Get
            Return _bdd
        End Get
        Set(ByVal value As Single)
            _bdd = value
        End Set
    End Property

    Property Satc() As Single
        Get
            Return _satc
        End Get
        Set(ByVal value As Single)
            _satc = value
        End Set
    End Property

    Property Wn() As Single
        Get
            Return _wn
        End Get
        Set(ByVal value As Single)
            _wn = value
        End Set
    End Property
    Property Uw() As Single
        Get
            Return _uw
        End Get
        Set(ByVal value As Single)
            _uw = value
        End Set
    End Property
    Property Smb() As Single
        Get
            Return _smb
        End Get
        Set(ByVal value As Single)
            _smb = value
        End Set
    End Property
    Property Rsd() As Single
        Get
            Return _rsd
        End Get
        Set(ByVal value As Single)
            _rsd = value
        End Set
    End Property
    Property Rok() As Single
        Get
            Return _rok
        End Get
        Set(ByVal value As Single)
            _rok = value
        End Set
    End Property
    Property Psp() As Single
        Get
            Return _psp
        End Get
        Set(ByVal value As Single)
            _psp = value
        End Set
    End Property
    Property Fc() As Single
        Get
            Return _fc
        End Get
        Set(ByVal value As Single)
            _fc = value
        End Set
    End Property
    Property Cnds() As Single
        Get
            Return _cnds
        End Get
        Set(ByVal value As Single)
            _cnds = value
        End Set
    End Property
    Property Cac() As Single
        Get
            Return _cac
        End Get
        Set(ByVal value As Single)
            _cac = value
        End Set
    End Property
    Property Cec() As Single
        Get
            Return _cec
        End Get
        Set(ByVal value As Single)
            _cec = value
        End Set
    End Property
End Class
