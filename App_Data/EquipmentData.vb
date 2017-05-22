'Public Class sortBySelected
'    Inherits Comparer(Of EquipmentData)
'    Public Overrides Function Compare(ByVal x As EquipmentData, ByVal y As EquipmentData) As Integer
'        Return y.Selected.CompareTo(x.Selected)
'    End Function
'End Class

Public Class EquipmentData
    Public _selected As Boolean
    Public _id As Short
    Public _name As String
    Public _leaseRate As Single
    Public _newPrice As Single
    Public _newHours As Single
    Public _currentPrice As Single
    Public _hoursRemaining As Single
    Public _width As Single
    Public _speed As Single
    Public _fieldEfficiency As Single
    Public _horsePower As Single
    Public _rf1 As Single
    Public _rf2 As Single
    Public _irLoan As Single
    Public _lLoan As Single
    Public _irEquity As Single
    Public _pDebt As Single
    Public _year As Short
    Public _rv1 As Single
    Public _rv2 As Single

    Property Selected() As Boolean
        Get
            Return _selected
        End Get
        Set(ByVal value As Boolean)
            _selected = value
        End Set
    End Property

    Property Id() As Short
        Get
            Return _id
        End Get
        Set(ByVal value As Short)
            _id = value
        End Set
    End Property

    Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Property LeaseRate() As Single
        Get
            Return _leaseRate
        End Get
        Set(ByVal value As Single)
            _leaseRate = value
        End Set
    End Property

    Property NewPrice() As Single
        Get
            Return _newPrice
        End Get
        Set(ByVal value As Single)
            _newPrice = value
        End Set
    End Property
    Property NewHours() As Single
        Get
            Return _newHours
        End Get
        Set(ByVal value As Single)
            _newHours = value
        End Set
    End Property
    Property CurrentPrice() As Single
        Get
            Return _currentPrice
        End Get
        Set(ByVal value As Single)
            _currentPrice = value
        End Set
    End Property
    Property HoursRemaining() As Single
        Get
            Return _hoursRemaining
        End Get
        Set(ByVal value As Single)
            _hoursRemaining = value
        End Set
    End Property
    Property Width() As Single
        Get
            Return _width
        End Get
        Set(ByVal value As Single)
            _width = value
        End Set
    End Property
    Property Speed() As Single
        Get
            Return _speed
        End Get
        Set(ByVal value As Single)
            _speed = value
        End Set
    End Property
    Property FieldEfficiency() As Single
        Get
            Return _fieldEfficiency
        End Get
        Set(ByVal value As Single)
            _fieldEfficiency = value
        End Set
    End Property

    Property HorsePower() As Single
        Get
            Return _horsePower
        End Get
        Set(ByVal value As Single)
            _horsePower = value
        End Set
    End Property
    Property Rf1() As Single
        Get
            Return _rf1
        End Get
        Set(ByVal value As Single)
            _rf1 = value
        End Set
    End Property
    Property Rf2() As Single
        Get
            Return _rf2
        End Get
        Set(ByVal value As Single)
            _rf2 = value
        End Set
    End Property
    Property IrLoan() As Single
        Get
            Return _irLoan
        End Get
        Set(ByVal value As Single)
            _irLoan = value
        End Set
    End Property
    Property LLoan() As Single
        Get
            Return _lLoan
        End Get
        Set(ByVal value As Single)
            _lLoan = value
        End Set
    End Property
    Property IrEquity() As Single
        Get
            Return _irEquity
        End Get
        Set(ByVal value As Single)
            _irEquity = value
        End Set
    End Property

    Property PDebt() As Single
        Get
            Return _pDebt
        End Get
        Set(ByVal value As Single)
            _pDebt = value
        End Set
    End Property
    Property Year() As Short
        Get
            Return _year
        End Get
        Set(ByVal value As Short)
            _year = value
        End Set
    End Property
    Property Rv1() As Single
        Get
            Return _rv1
        End Get
        Set(ByVal value As Single)
            _rv1 = value
        End Set
    End Property
    Property Rv2() As Single
        Get
            Return _rv2
        End Get
        Set(ByVal value As Single)
            _rv2 = value
        End Set
    End Property

End Class
