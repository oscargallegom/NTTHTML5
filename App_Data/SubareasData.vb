Imports System.Math

Public Class SubareasData
    'buffer, reservoir, soil or divided when there is just one soil type
    Private _sbaType As String
    'soil information
    Private _soilKey As String
    Private _soilSymbol As String
    Private _soilGroup As String
    Private _soilComponent As String
    'line 1
    Private _subareaNumber As UShort
    Private _subareaTitle As String
    Private _inps As UShort
    Public _line2 As New List(Of SubareaLine2)
    Public _line3 As New List(Of SubareaLine3)
    Public _line4 As New List(Of SubareaLine4)
    Public _line5 As New List(Of SubareaLine5)
    Public _line6 As New List(Of SubareaLine6)
    Public _line7 As New List(Of SubareaLine7)
    Public _line8 As New List(Of SubareaLine8)
    Public _line9 As New List(Of SubareaLine9)
    Public _line10 As New List(Of SubareaLine10)
    Public _line11 As New List(Of SubareaLine11)
    Public _line12 As New List(Of SubareaLine12)
    'Created to have individual operation for each soil due to the change in curve number.
    Public _operationsInfo As New List(Of OperationsData)

    Sub New()
        Dim line2 As New SubareaLine2
        _line2.Add(line2)
        Dim line3 As New SubareaLine3
        _line3.Add(line3)
        Dim line4 As New SubareaLine4
        _line4.Add(line4)
        Dim line5 As New SubareaLine5
        _line5.Add(line5)
        Dim line6 As New SubareaLine6
        _line6.Add(line6)
        Dim line7 As New SubareaLine7
        _line7.Add(line7)
        Dim line8 As New SubareaLine8
        _line8.Add(line8)
        Dim line9 As New SubareaLine9
        _line9.Add(line9)
        Dim line10 As New SubareaLine10
        _line10.Add(line10)
        Dim line11 As New SubareaLine11
        _line11.Add(line11)
        Dim line12 As New SubareaLine12
        _line12.Add(line12)
    End Sub

    Property SbaType() As String
        Get
            Return _sbaType
        End Get
        Set(ByVal value As String)
            _sbaType = value
        End Set
    End Property
    'soil information
    Property SoilKey() As String
        Get
            Return _soilKey
        End Get
        Set(ByVal value As String)
            _soilKey = value
        End Set
    End Property
    Property SoilSymbol() As String
        Get
            Return _soilSymbol
        End Get
        Set(ByVal value As String)
            _soilSymbol = value
        End Set
    End Property
    Property SoilGroup() As String
        Get
            Return _soilGroup
        End Get
        Set(ByVal value As String)
            _soilGroup = value
        End Set
    End Property
    Property SoilComponent() As String
        Get
            Return _soilComponent
        End Get
        Set(ByVal value As String)
            _soilComponent = value
        End Set
    End Property
    'line 1
    Property SubareaNumber() As UShort
        Get
            Return _subareaNumber
        End Get
        Set(ByVal value As UShort)
            _subareaNumber = value
        End Set
    End Property
    Property SubareaTitle() As String
        Get
            Return _subareaTitle
        End Get
        Set(ByVal value As String)
            _subareaTitle = value
        End Set
    End Property
End Class
