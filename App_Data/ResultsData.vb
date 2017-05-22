Public Class ResultsData
    Private _sub As UShort
    Private _year As UShort
    Private _prkn As Single
    Private _avol As Single
    Private _flow As Single
    Private _sed As Single
    Private _orgn As Single
    Private _orgp As Single
    Private _no3 As Single
    Private _po4 As Single
    'Private _crop As String
    'Private _yld1 As Single
    'Private _yld2 As Single
    Private _prkp As Single
    Private _field As Single
    Private _qdr As Single
    Private _dprk As Single
    Private _qdrn As Single
    Private _qdrp As Single
    Private _n2o As Single
    Private _co2 As Single
    Private _dn As Single
    Private _ymnu As Single
    'Private _ws As Single
    'Private _ns As Single
    'Private _ps As Single
    'Private _ts As Single
    'Private _as As Single
    Private _pcp As Single
    Private _irri As Single
    Private _qn As Single
    Private _surfaceFlow As Single
    Private _totalN As Single
    Private _totalP As Single
    Private _totalflow As Single
    Private _totalOtherFlowInfo As Single
    Private _totalSediment As Single

    Property Sub1() As UShort
        Get
            Return _sub
        End Get
        Set(ByVal value As UShort)
            _sub = value
        End Set
    End Property

    Property Year() As UShort
        Get
            Return _year
        End Get
        Set(ByVal value As UShort)
            _year = value
        End Set
    End Property

    Property PRKN() As Single
        Get
            Return _prkn
        End Get
        Set(ByVal value As Single)
            _prkn = value
        End Set
    End Property
    Property AVOL() As Single
        Get
            Return _avol
        End Get
        Set(ByVal value As Single)
            _avol = value
        End Set
    End Property
    Property FLOW() As Single
        Get
            Return _flow
        End Get
        Set(ByVal value As Single)
            _flow = value
        End Set
    End Property
    Property SED() As Single
        Get
            Return _SED
        End Get
        Set(ByVal value As Single)
            _SED = value
        End Set
    End Property
    Property ORGN() As Single
        Get
            Return _orgn
        End Get
        Set(ByVal value As Single)
            _orgn = value
        End Set
    End Property
    Property ORGP() As Single
        Get
            Return _orgp
        End Get
        Set(ByVal value As Single)
            _orgp = value
        End Set
    End Property
    Property NO3() As Single
        Get
            Return _NO3
        End Get
        Set(ByVal value As Single)
            _NO3 = value
        End Set
    End Property
    Property PO4() As Single
        Get
            Return _PO4
        End Get
        Set(ByVal value As Single)
            _PO4 = value
        End Set
    End Property
    'Property Crop() As String
    '    Get
    '        Return _crop
    '    End Get
    '    Set(ByVal value As String)
    '        _crop = value
    '    End Set
    'End Property
    'Property YLD1() As Single
    '    Get
    '        Return _yld1
    '    End Get
    '    Set(ByVal value As Single)
    '        _yld1 = value
    '    End Set
    'End Property
    'Property YLD2() As Single
    '    Get
    '        Return _yld2
    '    End Get
    '    Set(ByVal value As Single)
    '        _yld2 = value
    '    End Set
    'End Property
    Property PRKP() As Single
        Get
            Return _prkp
        End Get
        Set(ByVal value As Single)
            _prkp = value
        End Set
    End Property
    Property Qdr() As Single
        Get
            Return _qdr
        End Get
        Set(ByVal value As Single)
            _qdr = value
        End Set
    End Property
    Property Dprk() As Single
        Get
            Return _dprk
        End Get
        Set(ByVal value As Single)
            _dprk = value
        End Set
    End Property
    Property QDRN() As Single
        Get
            Return _qdrn
        End Get
        Set(ByVal value As Single)
            _qdrn = value
        End Set
    End Property
    Property QDRP() As Single
        Get
            Return _qdrp
        End Get
        Set(ByVal value As Single)
            _qdrp = value
        End Set
    End Property
    Property N2O() As Single
        Get
            Return _n2o
        End Get
        Set(ByVal value As Single)
            _n2o = value
        End Set
    End Property
    Property CO2() As Single
        Get
            Return _CO2
        End Get
        Set(ByVal value As Single)
            _CO2 = value
        End Set
    End Property
    Property DN() As Single
        Get
            Return _DN
        End Get
        Set(ByVal value As Single)
            _DN = value
        End Set
    End Property
    Property YMNU() As Single
        Get
            Return _ymnu
        End Get
        Set(ByVal value As Single)
            _ymnu = value
        End Set
    End Property
    'Property WS() As Single
    '    Get
    '        Return _WS
    '    End Get
    '    Set(ByVal value As Single)
    '        _WS = value
    '    End Set
    'End Property
    'Property NS() As Single
    '    Get
    '        Return _NS
    '    End Get
    '    Set(ByVal value As Single)
    '        _NS = value
    '    End Set
    'End Property
    'Property PS() As Single
    '    Get
    '        Return _PS
    '    End Get
    '    Set(ByVal value As Single)
    '        _PS = value
    '    End Set
    'End Property
    'Property TS() As Single
    '    Get
    '        Return _TS
    '    End Get
    '    Set(ByVal value As Single)
    '        _TS = value
    '    End Set
    'End Property
    'Property AS1() As Single
    '    Get
    '        Return _AS
    '    End Get
    '    Set(ByVal value As Single)
    '        _AS = value
    '    End Set
    'End Property
    Property PCP() As Single
        Get
            Return _PCP
        End Get
        Set(ByVal value As Single)
            _PCP = value
        End Set
    End Property
    Property IRRI() As Single
        Get
            Return _irri
        End Get
        Set(ByVal value As Single)
            _irri = value
        End Set
    End Property
    Property QN() As Single
        Get
            Return _qn
        End Get
        Set(ByVal value As Single)
            _qn = value
        End Set
    End Property
    Property SurfaceFlow() As Single
        Get
            Return _surfaceFlow
        End Get
        Set(ByVal value As Single)
            _surfaceFlow = value
        End Set
    End Property
    ReadOnly Property TotalN() As Single
        Get
            Return ORGN + NO3 + QDRN + QN
        End Get
    End Property
        ReadOnly Property TotalP() As Single
        Get
            'If QDRN > 0 Then
            Return ORGP + PO4 + QDRP
            'Return ORGP + PO4 + (QDRN * 0.7)
            'Else
            'Return ORGP + PO4
            'End If
        End Get
    End Property
        ReadOnly Property TotalFlow() As Single
        Get
            Return FLOW + SurfaceFlow + QDR
        End Get
    End Property
        ReadOnly Property TotalOtherFlowInfo() As Single
        Get
            Return irrigation + dprk
        End Get
    End Property
    ReadOnly Property TotalSediment() As Single
        Get
            Return SED + YMNU
        End Get
    End Property
End Class