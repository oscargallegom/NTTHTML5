Public Class SubareaLine8
    'line 8
    Private _lineNumber As UShort = 8
    Private _nirr As Short = 0
    Private _iri As Short = 0
    Private _ifa As Short = 0
    Private _lm As Short = 0
    Private _ifd As Short = 0
    Private _idr As Short = 0       'Drain Depth (ft)
    Private _idf1 As Short = 0      'for fertigation from lagoon
    Private _idf2 As Short = 0
    Private _idf3 As Short = 0
    Private _idf4 As Short = 1      'for authomatic fertigation  'Ali ask to put 1 always. (8/17/15)
    Private _idf5 As Short = 0

    Property LineNumber() As UShort
        Get
            Return _lineNumber
        End Get
        Set(ByVal value As UShort)
            _lineNumber = value
        End Set
    End Property

    Property Nirr() As Short
        Get
            Return _nirr
        End Get
        Set(ByVal value As Short)
            _nirr = value
        End Set
    End Property
    Property Iri() As Short
        Get
            Return _iri
        End Get
        Set(ByVal value As Short)
            _iri = value
        End Set
    End Property
    Property Ifa() As Short
        Get
            Return _ifa
        End Get
        Set(ByVal value As Short)
            _ifa = value
        End Set
    End Property
    Property Lm() As Short
        Get
            Return _lm
        End Get
        Set(ByVal value As Short)
            _lm = value
        End Set
    End Property
    Property Ifd() As Short
        Get
            Return _ifd
        End Get
        Set(ByVal value As Short)
            _ifd = value
        End Set
    End Property
    Property Idr() As Short
        Get
            Return _idr
        End Get
        Set(ByVal value As Short)
            _idr = value
        End Set
    End Property
    Property Idf1() As Short
        Get
            Return _idf1
        End Get
        Set(ByVal value As Short)
            _idf1 = value
        End Set
    End Property
    Property Idf2() As Short
        Get
            Return _idf2
        End Get
        Set(ByVal value As Short)
            _idf2 = value
        End Set
    End Property
    Property Idf3() As Short
        Get
            Return _idf3
        End Get
        Set(ByVal value As Short)
            _idf3 = value
        End Set
    End Property
    Property Idf4() As Short
        Get
            Return _idf4
        End Get
        Set(ByVal value As Short)
            _idf4 = value
        End Set
    End Property
    Property Idf5() As Short
        Get
            Return _idf5
        End Get
        Set(ByVal value As Short)
            _idf5 = value
        End Set
    End Property

End Class
