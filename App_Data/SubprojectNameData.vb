Public Class SubprojectNameData
    Private _name As String
    Private _date As Date
    Private _totalArea As Single
    Public _subproject As New List(Of SubProjectData)
    Public _results As New ScenariosData.APEXResults

    Property TotalArea() As Single
        Get
            Return _totalArea
        End Get
        Set(ByVal value As Single)
            _totalArea = value
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

    Property Date1() As Date
        Get
            Return _date
        End Get
        Set(value As Date)
            _date = value
        End Set
    End Property

    Public Class SubProjectData
        Private _field As String
        Private _scenario As String
        Private _area As Single

        Property Area() As Single
            Get
                Return _area
            End Get
            Set(ByVal value As Single)
                _area = value
            End Set
        End Property

        Property Field() As String
            Get
                Return _field
            End Get
            Set(ByVal value As String)
                _field = value
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
    End Class

    Public Sub CleanResults()
        '_results.SoilResults = New ScenariosData.APEXResultsAll
        With _results.SoilResults
            .co2 = 0
            .subsurfaceFlow = 0
            .runoff = 0
            .tileDrainFlow = 0
            .irrigation = 0
            .irrigationSD = 0
            .irrigationCI = 0
            .deepPerFlow = 0
            .deepPerFlowSD = 0
            .deepPerFlowCI = 0
            .LeachedN = 0
            .LeachedP = 0
            .ManureErosion = 0
            .n2o = 0
            .OrgN = 0
            .OrgNSD = 0
            .OrgNCI = 0
            .RunoffN = 0
            .runoffNCI = 0
            .runoffNSD = 0
            .SubsurfaceN = 0
            .tileDrainN = 0
            .subsurfaceNSD = 0
            .subsurfaceNCI = 0
            .tileDrainNSD = 0
            .tileDrainNCI = 0
            .OrgP = 0
            .OrgPSD = 0
            .OrgPCI = 0
            .PO4 = 0
            .PO4SD = 0
            .PO4CI = 0
            .Sediment = 0
            .SedimentCI = 0
            .SedimentSD = 0
            .Crops.cropName = Nothing
            .Crops.cropYield = Nothing
            .Crops.cropYieldSD = Nothing
            .Crops.cropYieldCI = Nothing
            ReDim .Crops.cropRecords(0)
            .Crops.ns = Nothing
            .Crops.ps = Nothing
            .Crops.ts = Nothing
            .Crops.ws = Nothing
        End With
    End Sub


End Class
