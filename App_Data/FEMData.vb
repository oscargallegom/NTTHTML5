Public Class FEMData
    Private _message As String
    Private _totalRevenue As Single
    Private _totalCost As Single
    Private _netReturn As Single
    Private _netCashFlow As Single
    Private _totalNCostEffectiveness As Single
    Private _totalPCostEffectiveness As Single
    Private _sedimentCostEffectiveness As Single

    Property Message() As String
        Get
            Return _message
        End Get
        Set(value As String)
            _message = value
        End Set
    End Property

    Property TotalRevenue() As Single
        Get
            Return _totalRevenue
        End Get
        Set(value As Single)
            _totalRevenue = value
        End Set
    End Property

    Property TotalCost() As Single
        Get
            Return _totalCost
        End Get
        Set(value As Single)
            _totalCost = value
        End Set
    End Property
    Property NetReturn() As Single
        Get
            Return _netReturn
        End Get
        Set(value As Single)
            _netReturn = value
        End Set
    End Property
    Property NetCashFlow() As Single
        Get
            Return _netCashFlow
        End Get
        Set(value As Single)
            _netCashFlow = value
        End Set
    End Property
    Property TotalNCostEffectiveness() As Single
        Get
            Return _totalNCostEffectiveness
        End Get
        Set(value As Single)
            _totalNCostEffectiveness = value
        End Set
    End Property
    Property TotalPCostEffectiveness() As Single
        Get
            Return _totalPCostEffectiveness
        End Get
        Set(value As Single)
            _totalPCostEffectiveness = value
        End Set
    End Property
    Property SedimentCostEffectiveness() As Single
        Get
            Return _sedimentCostEffectiveness
        End Get
        Set(value As Single)
            _sedimentCostEffectiveness = value
        End Set
    End Property
End Class
