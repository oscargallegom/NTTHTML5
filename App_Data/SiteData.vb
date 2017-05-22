Imports System.ComponentModel

Public Class SiteData
    Private _latitude As Single
    Private _longitude As Single
    Private _elevation As Single
    Private _apm As Single = 1.0
    Private _co2x As Single
    Private _cqnx As Single
    Private _rfnx As Single
    Private _upr As Single
    Private _unr As Single

    Property Latitude() As Single
        Get
            Return _latitude
        End Get
        Set(ByVal value As Single)
            _latitude = value
        End Set
    End Property

    Property Longitude() As Single
        Get
            Return _longitude
        End Get
        Set(ByVal value As Single)
            _longitude = value
        End Set
    End Property

    <DefaultValue(1395.0)> _
    Property Elevation() As Single
        Get
            Return _elevation
        End Get
        Set(ByVal value As Single)
            _elevation = value
        End Set
    End Property

    <DefaultValue(1.0)> _
    Property Apm() As Single
        Get
            Return _apm
        End Get
        Set(ByVal value As Single)
            _apm = value
        End Set
    End Property

    <DefaultValue(0.0)> _
    Property Co2x() As Single
        Get
            Return _co2x
        End Get
        Set(ByVal value As Single)
            _co2x = value
        End Set
    End Property

    <DefaultValue(0.0)> _
    Property Cqnx() As Single
        Get
            Return _cqnx
        End Get
        Set(ByVal value As Single)
            _cqnx = value
        End Set
    End Property

    <DefaultValue(0.0)> _
    Property Rfnx() As Single
        Get
            Return _rfnx
        End Get
        Set(ByVal value As Single)
            _rfnx = value
        End Set
    End Property

    <DefaultValue(0.0)> _
    Property Upr() As Single
        Get
            Return _upr
        End Get
        Set(ByVal value As Single)
            _upr = value
        End Set
    End Property

    <DefaultValue(0.0)> _
    Property Unr() As Single
        Get
            Return _unr
        End Get
        Set(ByVal value As Single)
            _unr = value
        End Set
    End Property
End Class
