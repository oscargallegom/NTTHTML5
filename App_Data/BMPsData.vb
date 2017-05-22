Imports System.ComponentModel

Public Class BmpsData
    Private _tileDrainDepth As Single
    Private _pndF As Single
    Private _sbs As Boolean
    Private _slopeRed As Single
    Private _ts As Boolean
    Private _lm As Boolean
    Private _aoc As Boolean
    Private _gc As Boolean
    Private _sa As Boolean
    Private _wwCrop As Short
    Private _wwWidth As Single
    Private _fsCrop As Short
    Private _fsWidth As Single
    Private _fsEff As Single
    Private _fsArea As Single
    Private _fsSlopeRatio As Single
    Private _sdgCrop As Short
    Private _sdgWidth As Single
    Private _sdgEff As Single
    Private _sdgArea As Single
    Private _sdgSlopeRatio As Single
    Private _rfWidth As Single
    Private _rfEff As Single
    Private _rfArea As Single
    Private _rfGrassFieldPortion As Single
    Private _rfSlopeRatio As Single
    Private _wlArea As Single
    Private _aiType As Short
    Private _aiWaterStressFactor As Single = 0.8
    Private _aiEff As Single
    Private _aiFreq As Single
    Private _aiMaxSingleApp As Single = 3.0
    Private _aiSafetyFactor As Single
    Private _aiResArea As Single
    Private _afType As Short
    Private _afWaterStressFactor As Single = 0.8
    Private _afEff As Single
    Private _afFreq As Single
    Private _afMaxSingleApp As Single = 3.0
    Private _afNConc As Single
    Private _afSafetyFactor As Single
    Private _sfCode As UShort = 0
    Private _sfName As String = String.Empty
    Private _sfAnimals As Single = 0
    Private _sfDays As UShort = 0
    Private _sfHours As Single = 0
    Private _sfDryManure As Single = 0
    Private _sfNo3 As Single = 0
    Private _sfPo4 As Single = 0
    Private _sfOrgN As Single = 0
    Private _sfOrgP As Single = 0
    Private _sfNh3 As Single = 0
    Private _ppNDWidth As Single = 0
    Private _ppNDSides As UShort = 0
    Private _ppDSWidth As Single = 0
    Private _ppDSSides As UShort = 0
    Private _ppDEWidth As Single = 0
    Private _ppDESides As UShort = 0
    Private _ppDEResArea As Single = 0
    Private _ppTWWidth As Single = 0
    Private _ppTWSides As UShort = 0
    Private _ppTWResArea As Single = 0
    Private _cbBWidth As Single = 0
    Private _cbCwidth As Single = 0
    Private _cbCrop As Short = 0
    Private _ccMinimumTeperature As Short = 0
    Private _ccMaximumTeperature As Short = 0
    Private _ccPrecipitation As Short = 0
    Private _mcNO3_N As Single = 0
    Private _mcPO4_P As Single = 0
    Private _mcOrgN As Single = 0
    Private _mcOrgP As Single = 0
    Private _mcOM As Single = 0
    Private _mcType As Short

    Property CcMinimumTeperature() As Single
        Get
            Return _ccMinimumTeperature
        End Get
        Set(ByVal value As Single)
            _ccMinimumTeperature = value
        End Set
    End Property

    Property CcMaximumTeperature() As Single
        Get
            Return _ccMaximumTeperature
        End Get
        Set(ByVal value As Single)
            _ccMaximumTeperature = value
        End Set
    End Property

    Property CcPrecipitation() As Single
        Get
            Return _ccPrecipitation
        End Get
        Set(ByVal value As Single)
            _ccPrecipitation = value
        End Set
    End Property

    Property SdgslopeRatio() As Single
        Get
            Return _sdgSlopeRatio
        End Get
        Set(ByVal value As Single)
            _sdgSlopeRatio = value
        End Set
    End Property

    <DefaultValue(0.9)> _
    Property SdgEff() As Single
        Get
            If _sdgEff = 0 Then
                Return 0.9
            Else
                Return _sdgEff
            End If
        End Get
        Set(ByVal value As Single)
            _sdgEff = value
        End Set
    End Property

    Property SdgArea() As Single
        Get
            Return _sdgArea
        End Get
        Set(ByVal value As Single)
            _sdgArea = value
        End Set
    End Property

    Property SdgWidth() As Single
        Get
            Return _sdgWidth
        End Get
        Set(ByVal value As Single)
            _sdgWidth = value
        End Set
    End Property

    Property SdgCrop() As Short
        Get
            Return _sdgCrop
        End Get
        Set(ByVal value As Short)
            _sdgCrop = value
        End Set
    End Property

    Property CBCrop() As Single
        Get
            Return _cbCrop
        End Get
        Set(ByVal value As Single)
            _cbCrop = value
        End Set
    End Property

    Property CBCWidth() As Single
        Get
            Return _cbCwidth
        End Get
        Set(ByVal value As Single)
            _cbCwidth = value
        End Set
    End Property

    Property CBBWidth() As Single
        Get
            Return _cbBWidth
        End Get
        Set(ByVal value As Single)
            _cbBWidth = value
        End Set
    End Property

    Property PPTWResArea() As Single
        Get
            Return _ppTWResArea
        End Get
        Set(ByVal value As Single)
            _ppTWResArea = value
        End Set
    End Property

    Property PPTWSides() As UShort
        Get
            Return _ppTWSides
        End Get
        Set(ByVal value As UShort)
            _ppTWSides = value
        End Set
    End Property

    Property PPTWWidth() As Single
        Get
            Return _ppTWWidth
        End Get
        Set(ByVal value As Single)
            _ppTWWidth = value
        End Set
    End Property

    Property PPDEResArea() As Single
        Get
            Return _ppDEResArea
        End Get
        Set(ByVal value As Single)
            _ppDEResArea = value
        End Set
    End Property

    Property PPDESides() As UShort
        Get
            Return _ppDESides
        End Get
        Set(ByVal value As UShort)
            _ppDESides = value
        End Set
    End Property

    Property PPDEWidth() As Single
        Get
            Return _ppDEWidth
        End Get
        Set(ByVal value As Single)
            _ppDEWidth = value
        End Set
    End Property

    Property PPDSSides() As UShort
        Get
            Return _ppDSSides
        End Get
        Set(ByVal value As UShort)
            _ppDSSides = value
        End Set
    End Property

    Property PPDSWidth() As Single
        Get
            Return _ppDSWidth
        End Get
        Set(ByVal value As Single)
            _ppDSWidth = value
        End Set
    End Property

    Property PPNDSides() As UShort
        Get
            Return _ppNDSides
        End Get
        Set(ByVal value As UShort)
            _ppNDSides = value
        End Set
    End Property

    Property PPNDWidth() As Single
        Get
            Return _ppNDWidth
        End Get
        Set(ByVal value As Single)
            _ppNDWidth = value
        End Set
    End Property

    Property SFOrgP() As Single
        Get
            Return _sfOrgP
        End Get
        Set(ByVal value As Single)
            _sfOrgP = value
        End Set
    End Property

    Property SFOrgN() As Single
        Get
            Return _sfOrgN
        End Get
        Set(ByVal value As Single)
            _sfOrgN = value
        End Set
    End Property

    Property SFPo4() As Single
        Get
            Return _sfPo4
        End Get
        Set(ByVal value As Single)
            _sfPo4 = value
        End Set
    End Property

    Property SFNo3() As Single
        Get
            Return _sfNo3
        End Get
        Set(ByVal value As Single)
            _sfNo3 = value
        End Set
    End Property

    Property SFNH3() As Single
        Get
            Return _sfNh3
        End Get
        Set(ByVal value As Single)
            _sfNh3 = value
        End Set
    End Property

    Property SFDryManure() As Single
        Get
            Return _sfDryManure
        End Get
        Set(ByVal value As Single)
            _sfDryManure = value
        End Set
    End Property

    Property SFHours() As Single
        Get
            Return _sfHours
        End Get
        Set(ByVal value As Single)
            _sfHours = value
        End Set
    End Property

    Property SFDays() As UShort
        Get
            Return _sfDays
        End Get
        Set(ByVal value As UShort)
            _sfDays = value
        End Set
    End Property

    Property SFAnimals() As Single
        Get
            Return _sfAnimals
        End Get
        Set(ByVal value As Single)
            _sfAnimals = value
        End Set
    End Property

    Property SFName() As String
        Get
            Return _sfName
        End Get
        Set(ByVal value As String)
            _sfName = value
        End Set
    End Property

    Property SFCode() As UShort
        Get
            Return _sfCode
        End Get
        Set(ByVal value As UShort)
            _sfCode = value
        End Set
    End Property

    Property AFSafetyFactor() As Single
        Get
            Return _afSafetyFactor
        End Get
        Set(ByVal value As Single)
            _afSafetyFactor = value
        End Set
    End Property

    Property AFNConc() As Single
        Get
            Return _afNConc
        End Get
        Set(ByVal value As Single)
            _afNConc = value
        End Set
    End Property


    <DefaultValue(3.0)> _
    Property AFMaxSingleApp() As Single
        Get
            Return _afMaxSingleApp
        End Get
        Set(ByVal value As Single)
            _afMaxSingleApp = value
        End Set
    End Property

    Property AFFreq() As Single
        Get
            Return _afFreq
        End Get
        Set(ByVal value As Single)
            _afFreq = value
        End Set
    End Property

    Property AFEff() As Single
        Get
            Return _afEff
        End Get
        Set(ByVal value As Single)
            _afEff = value
        End Set
    End Property

    Property AFWaterStressFactor() As Single
        Get
            Return _afWaterStressFactor
        End Get
        Set(ByVal value As Single)
            _afWaterStressFactor = value
        End Set
    End Property

    Property AFType() As Short
        Get
            Return _afType
        End Get
        Set(ByVal value As Short)
            _afType = value
        End Set
    End Property

    Property AIResArea() As Single
        Get
            Return _aiResArea
        End Get
        Set(ByVal value As Single)
            _aiResArea = value
        End Set
    End Property

    Property AISafetyFactor() As Single
        Get
            Return _aiSafetyFactor
        End Get
        Set(ByVal value As Single)
            _aiSafetyFactor = value
        End Set
    End Property

    <DefaultValue(3.0)> _
    Property AIMaxSingleApp() As Single
        Get
            Return _aiMaxSingleApp
        End Get
        Set(ByVal value As Single)
            _aiMaxSingleApp = value
        End Set
    End Property

    Property AIFreq() As Single
        Get
            Return _aiFreq
        End Get
        Set(ByVal value As Single)
            _aiFreq = value
        End Set
    End Property

    Property AIEff() As Single
        Get
            Return _aiEff
        End Get
        Set(ByVal value As Single)
            _aiEff = value
        End Set
    End Property

    Property AIWaterStressFactor() As Single
        Get
            Return _aiWaterStressFactor
        End Get
        Set(ByVal value As Single)
            _aiWaterStressFactor = value
        End Set
    End Property

    Property AIType() As Short
        Get
            Return _aiType
        End Get
        Set(ByVal value As Short)
            _aiType = value
        End Set
    End Property

    Property WLArea() As Single
        Get
            Return _wlArea
        End Get
        Set(ByVal value As Single)
            _wlArea = value
        End Set
    End Property

    Property RFGrassFieldPortion() As Single
        Get
            Return _rfGrassFieldPortion
        End Get
        Set(ByVal value As Single)
            _rfGrassFieldPortion = value
        End Set
    End Property

    Property RFslopeRatio() As Single
        Get
            Return _rfSlopeRatio
        End Get
        Set(ByVal value As Single)
            _rfSlopeRatio = value
        End Set
    End Property

    Property RFEff() As Single
        Get
            If _rfEff = 0 Then
                Return 0.9
            Else
                Return _rfEff
            End If
        End Get
        Set(ByVal value As Single)
            _rfEff = value
        End Set
    End Property

    Property RFArea() As Single
        Get
            Return _rfArea
        End Get
        Set(ByVal value As Single)
            _rfArea = value
        End Set
    End Property

    Property RFWidth() As Single
        Get
            Return _rfWidth
        End Get
        Set(ByVal value As Single)
            _rfWidth = value
        End Set
    End Property

    Property FSslopeRatio() As Single
        Get
            Return _fsSlopeRatio
        End Get
        Set(ByVal value As Single)
            _fsSlopeRatio = value
        End Set
    End Property

    <DefaultValue(0.9)> _
    Property FSEff() As Single
        Get
            If _fsEff = 0 Then
                Return 0.9
            Else
                Return _fsEff
            End If
        End Get
        Set(ByVal value As Single)
            _fsEff = value
        End Set
    End Property

    Property FSArea() As Single
        Get
            Return _fsArea
        End Get
        Set(ByVal value As Single)
            _fsArea = value
        End Set
    End Property

    Property FSWidth() As Single
        Get
            Return _fsWidth
        End Get
        Set(ByVal value As Single)
            _fsWidth = value
        End Set
    End Property

    Property FSCrop() As Short
        Get
            Return _fsCrop
        End Get
        Set(ByVal value As Short)
            _fsCrop = value
        End Set
    End Property


    Property WWWidth() As Short
        Get
            Return _wwWidth
        End Get
        Set(ByVal value As Short)
            _wwWidth = value
        End Set
    End Property

    Property WWCrop() As Short
        Get
            Return _wwCrop
        End Get
        Set(ByVal value As Short)
            _wwCrop = value
        End Set
    End Property

    Property Lm() As Boolean
        Get
            Return _lm
        End Get
        Set(ByVal value As Boolean)
            _lm = value
        End Set
    End Property

    Property Ts() As Boolean
        Get
            Return _ts
        End Get
        Set(ByVal value As Boolean)
            _ts = value
        End Set
    End Property

    Property AoC() As Boolean
        Get
            Return _aoc
        End Get
        Set(ByVal value As Boolean)
            _aoc = value
        End Set
    End Property
    Property Gc() As Boolean
        Get
            Return _gc
        End Get
        Set(ByVal value As Boolean)
            _gc = value
        End Set
    End Property
    Property Sa() As Boolean
        Get
            Return _sa
        End Get
        Set(ByVal value As Boolean)
            _sa = value
        End Set
    End Property

    Property SlopeRed() As Single
        Get
            Return _slopeRed
        End Get
        Set(ByVal value As Single)
            _slopeRed = value
        End Set
    End Property

    Property Sbs() As Boolean
        Get
            Return _sbs
        End Get
        Set(ByVal value As Boolean)
            _sbs = value
        End Set
    End Property

    Property PndF() As Single
        Get
            Return _pndF
        End Get
        Set(ByVal value As Single)
            _pndF = value
        End Set
    End Property

    Property TileDrainDepth() As Single
        Get
            Return _tileDrainDepth
        End Get
        Set(ByVal value As Single)
            _tileDrainDepth = value
        End Set
    End Property

    Property mcNO3_N() As Single
        Get
            Return _mcNO3_N
        End Get
        Set(ByVal value As Single)
            _mcNO3_N = value
        End Set
    End Property

    Property mcOrgN() As Single
        Get
            Return _mcOrgN
        End Get
        Set(ByVal value As Single)
            _mcOrgN = value
        End Set
    End Property
    Property mcPO4_P() As Single
        Get
            Return _mcPO4_P
        End Get
        Set(ByVal value As Single)
            _mcPO4_P = value
        End Set
    End Property
    Property mcOrgP() As Single
        Get
            Return _mcOrgP
        End Get
        Set(ByVal value As Single)
            _mcOrgP = value
        End Set
    End Property
    Property mcOM() As Single
        Get
            Return _mcOM
        End Get
        Set(ByVal value As Single)
            _mcOM = value
        End Set
    End Property
    Property mcType() As Single
        Get
            Return _mcType
        End Get
        Set(ByVal value As Single)
            _mcType = value
        End Set
    End Property
End Class
