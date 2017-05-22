Public Class ScenariosToRun
    Private _field As String
    Public _fieldIndex As UShort
    Private _scenario As String
    Public _ScenarioIndex As UShort
    Private _project As String
    Private _location As String
    Private _weather As String
    Private _fields As String
    Private _soils As String
    Private _management As String
    Private _date As Date
    Private _message As String

    Property Field() As String
        Get
            Return _field
        End Get
        Set(ByVal value As String)
            _field = value
        End Set
    End Property

    Property FieldIndex() As UShort
        Get
            Return _fieldIndex
        End Get
        Set(ByVal value As UShort)
            _fieldIndex = value
        End Set
    End Property

    Property Scenario() As String
        Get
            Return _scenario
        End Get
        Set(ByVal value As String)
            _scenario = value
        End Set
    End Property

    Property ScenarioIndex() As UShort
        Get
            Return _ScenarioIndex
        End Get
        Set(ByVal value As UShort)
            _ScenarioIndex = value
        End Set
    End Property

    Property Project() As String
        Get
            Return _project
        End Get
        Set(ByVal value As String)
            _project = value
        End Set
    End Property

    Property Location() As String
        Get
            Return _location
        End Get
        Set(ByVal value As String)
            _location = value
        End Set
    End Property
    Property Weather() As String
        Get
            Return _weather
        End Get
        Set(ByVal value As String)
            _weather = value
        End Set
    End Property
    Property Fields() As String
        Get
            Return _fields
        End Get
        Set(ByVal value As String)
            _fields = value
        End Set
    End Property
    Property Soils() As String
        Get
            Return _soils
        End Get
        Set(ByVal value As String)
            _soils = value
        End Set
    End Property
    Property Management() As String
        Get
            Return _management
        End Get
        Set(ByVal value As String)
            _management = value
        End Set
    End Property
    Property Date1() As Date
        Get
            Return _date
        End Get
        Set(value As Date)
            _date = value
        End Set
    End Property
    Property Message() As String
        Get
            Return _message
        End Get
        Set(ByVal value As String)
            _message = value
        End Set
    End Property
End Class
