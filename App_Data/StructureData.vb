Public Class StructureData
    Public _selected As Boolean
    Public _id As Short
    Public _name As String
    Public _leaseRate As Single
    Public _newPrice As Single
    Public _newLife As Single
    Public _currentPrice As Single
    Public _lifeRemaining As Single
    Public _maintenanceCoefficient As Single
    Public _loanInterestRate As Single
    Public _lengthLoan As Single
    Public _interestRateEquity As Single
    Public _proportionDebt As Single
    Public _year As Short

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
    Property NewLife() As Single
        Get
            Return _newLife
        End Get
        Set(ByVal value As Single)
            _newLife = value
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
    Property LifeRemaining() As Single
        Get
            Return _lifeRemaining
        End Get
        Set(ByVal value As Single)
            _lifeRemaining = value
        End Set
    End Property
    Property MaintenanceCoefficient() As Single
        Get
            Return _maintenanceCoefficient
        End Get
        Set(ByVal value As Single)
            _maintenanceCoefficient = value
        End Set
    End Property
    Property LoanInterestRate() As Single
        Get
            Return _loanInterestRate
        End Get
        Set(ByVal value As Single)
            _loanInterestRate = value
        End Set
    End Property
    Property LengthLoan() As Single
        Get
            Return _lengthLoan
        End Get
        Set(ByVal value As Single)
            _lengthLoan = value
        End Set
    End Property

    Property InterestRateEquity() As Single
        Get
            Return _interestRateEquity
        End Get
        Set(ByVal value As Single)
            _interestRateEquity = value
        End Set
    End Property

    Property ProportionDebt() As Single
        Get
            Return _proportionDebt
        End Get
        Set(ByVal value As Single)
            _proportionDebt = value
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
End Class
