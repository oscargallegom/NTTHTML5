Public Class FieldsSelectedData
    Private _selected As Boolean
    Private _fieldNumber As UShort
    Private _fieldName As String
    Private _fieldArea As Single
    Private _scenario1Number As UShort
    Private _scenario1Name As String
    Private _scenario2Number As UShort
    Private _scenario2Name As String

    Property Selected() As Boolean
        Get
            Return _selected
        End Get
        Set(ByVal value As Boolean)
            _selected = value
        End Set
    End Property

    Property FieldNumber() As UShort
        Get
            Return _FieldNumber
        End Get
        Set(ByVal value As UShort)
            _FieldNumber = value
        End Set
    End Property

    Property FieldArea() As Single
        Get
            Return _fieldArea
        End Get
        Set(ByVal value As Single)
            _fieldArea = value
        End Set
    End Property

    Property FieldName() As String
        Get
            Return _fieldName
        End Get
        Set(ByVal value As String)
            _fieldName = value
        End Set
    End Property

    Property Scenario1Name() As String
        Get
            Return _scenario1Name
        End Get
        Set(ByVal value As String)
            _scenario1Name = value
        End Set
    End Property

    Property Scenario1Number() As UShort
        Get
            Return _scenario1Number
        End Get
        Set(ByVal value As UShort)
            _scenario1Number = value
        End Set
    End Property

    Property Scenario2Name() As String
        Get
            Return _scenario2Name
        End Get
        Set(ByVal value As String)
            _scenario2Name = value
        End Set
    End Property

    Property Scenario2Number() As UShort
        Get
            Return _scenario2Number
        End Get
        Set(ByVal value As UShort)
            _scenario2Number = value
        End Set
    End Property
End Class
