Public Class SummaryResults
    Private _sign As Boolean
    Private _description As String
    Private _unit As String
    Private _scenario1 As String
    Private _ci1 As String
    Private _scenario2 As String
    Private _ci2 As String
    Private _difference2 As Single
    Private _reduction2 As Single
    Private _totalArea2 As Single
    Private _scenario3 As String
    Private _ci3 As String
    Private _difference3 As Single
    Private _reduction3 As Single
    Private _totalArea3 As Single
    Private _type As String

    Property Sign() As Boolean
        Get
            Return _sign
        End Get
        Set(value As Boolean)
            _sign = value
        End Set
    End Property

    Property Description() As String
        Get
            Return _description
        End Get
        Set(value As String)
            _description = value
        End Set
    End Property

    Property Unit() As String
        Get
            Return _unit
        End Get
        Set(value As String)
            _unit = value
        End Set
    End Property

    Property Scenario1() As String
        Get
            Return _scenario1
        End Get
        Set(ByVal value As String)
            _scenario1 = value
        End Set
    End Property

    Property Ci1() As String
        Get
            Return _ci1
        End Get
        Set(ByVal value As String)
            _ci1 = value
        End Set
    End Property

    Property Scenario2() As String
        Get
            Return _scenario2
        End Get
        Set(ByVal value As String)
            _scenario2 = value
        End Set
    End Property

    Property Ci2() As String
        Get
            Return _ci2
        End Get
        Set(ByVal value As String)
            _ci2 = value
        End Set
    End Property

    Property Difference2() As Single
        Get
            Return _difference2
        End Get
        Set(ByVal value As Single)
            _difference2 = value
        End Set
    End Property

    Property Reduction2() As Single
        Get
            Return _reduction2
        End Get
        Set(ByVal value As Single)
            _reduction2 = value
        End Set
    End Property

    Property TotalArea2() As Single
        Get
            Return _totalArea2
        End Get
        Set(ByVal value As Single)
            _totalArea2 = value
        End Set
    End Property

    Property Scenario3() As String
        Get
            Return _scenario3
        End Get
        Set(ByVal value As String)
            _scenario3 = value
        End Set
    End Property

    Property Ci3() As String
        Get
            Return _ci3
        End Get
        Set(ByVal value As String)
            _ci3 = value
        End Set
    End Property

    Property Difference3() As Single
        Get
            Return _difference3
        End Get
        Set(ByVal value As Single)
            _difference3 = value
        End Set
    End Property

    Property Reduction3() As Single
        Get
            Return _reduction3
        End Get
        Set(ByVal value As Single)
            _reduction3 = value
        End Set
    End Property

    Property TotalArea3() As Single
        Get
            Return _totalArea3
        End Get
        Set(ByVal value As Single)
            _totalArea3 = value
        End Set
    End Property

    Property Type() As String
        Get
            Return _type
        End Get
        Set(ByVal value As String)
            _type = value
        End Set
    End Property
End Class

