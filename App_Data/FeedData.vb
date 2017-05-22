Public Class FeedData
    Public _selected As Boolean
    Public _id As Short
    Public _name As String
    Public _sellingPrice As Single
    Public _purchasePrice As Single
    Public _concentrate As Single
    Public _forage As Single
    Public _grain As Single
    Public _hay As Single
    Public _pasture As Single
    Public _silage As Single
    Public _supplement As Single

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

    Property SellingPrice() As Single
        Get
            Return _sellingPrice
        End Get
        Set(ByVal value As Single)
            _sellingPrice = value
        End Set
    End Property

    Property PurchasePrice() As Single
        Get
            Return _purchasePrice
        End Get
        Set(ByVal value As Single)
            _purchasePrice = value
        End Set
    End Property

    Property Concentrate() As Single
        Get
            Return _concentrate
        End Get
        Set(ByVal value As Single)
            _concentrate = value
        End Set
    End Property
    Property Forage() As Single
        Get
            Return _forage
        End Get
        Set(ByVal value As Single)
            _forage = value
        End Set
    End Property
    Property Grain() As Single
        Get
            Return _grain
        End Get
        Set(ByVal value As Single)
            _grain = value
        End Set
    End Property
    Property Hay() As Single
        Get
            Return _hay
        End Get
        Set(ByVal value As Single)
            _hay = value
        End Set
    End Property
    Property Pasture() As Single
        Get
            Return _pasture
        End Get
        Set(ByVal value As Single)
            _pasture = value
        End Set
    End Property
    Property Silage() As Single
        Get
            Return _silage
        End Get
        Set(ByVal value As Single)
            _silage = value
        End Set
    End Property
    Property Supplement() As Single
        Get
            Return _supplement
        End Get
        Set(ByVal value As Single)
            _supplement = value
        End Set
    End Property
End Class