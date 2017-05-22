Public Class APEXResultsAll
    Public LeachedP As Single
    Public OrgN As Single   'OrgN column 50
    Public SubsurfaceN As Single 'NO3 - QN Columns 68-246
    Public RunoffN As Single   'QN column 246
    Public _totalN As Single
    Public tileDrainN As Single 'QDRN column 145
    ReadOnly Property TotalN() As Single
        Get
            Return OrgN + SubsurfaceN + tileDrainN + RunoffN
        End Get
    End Property
    Public OrgP As Single
    Public PO4 As Single
    Public _totalP As Single
    Public tileDrainP As Single
    'Public ReadOnly Property TileDrainP As Single
    '    Get
    '        Return tileDrainN * 0.07
    '    End Get
    'End Property
    Public tileDrainPCI As Single
    Public tileDrainPSD As Single
    ReadOnly Property TotalP() As Single
        Get
            Return OrgP + PO4 + TileDrainP
        End Get
    End Property
    Public _totalPCI As Single
    ReadOnly Property TotalPCI() As Single
        Get
            Return OrgPCI + PO4CI
        End Get
    End Property
    Public Sediment As Single
    Public SedimentSD As Single
    Public SedimentCI As Single
    Public ManureErosion As Single
    Public ManureErosionSD As Single
    Public ManureErosionCI As Single
    Public _totalSediment As Single
    ReadOnly Property TotalSediment() As Single
        Get
            Return Sediment + ManureErosion
        End Get
    End Property
    Public Crops As New List(Of annualCrops)
    Public LeachedN As Single
    Public volatizationN As Single
    Public runoff As Single
    Public subsurfaceFlow As Single
    Public tileDrainFlow As Single
    Public runoffSD As Single
    Public subsurfaceFlowSD As Single
    Public tileDrainFlowSD As Single
    Public runoffCI As Single
    Public subsurfaceFlowCI As Single
    Public tileDrainFlowCI As Single
    Public _totalFlow As Single
    ReadOnly Property TotalFlow() As Single
        Get
            Return runoff + subsurfaceFlow + tileDrainFlow
        End Get
    End Property
    ReadOnly Property TotalFlowCI() As Single
        Get
            Return runoffCI + subsurfaceFlowCI + tileDrainFlowCI
        End Get
    End Property
    Public n2o As Single
    Public deepPerFlow As Single
    Public irrigation As Single
    Public deepPerFlowSD As Single
    Public irrigationSD As Single
    Public deepPerFlowCI As Single
    Public irrigationCI As Single
    Public _totalOtherFlowInfo As Single
    ReadOnly Property TotalOtherFlowInfo() As Single
        Get
            Return irrigation + deepPerFlow
        End Get
    End Property
    Public co2 As Single
    Public OrgPSD As Single
    Public PO4SD As Single
    Public OrgPCI As Single
    Public PO4CI As Single
    Public OrgNCI As Single
    Public OrgNSD As Single
    Public subsurfaceNSD As Single
    Public subsurfaceNCI As Single
    Public tileDrainNSD As Single
    Public tileDrainNCI As Single
    Public runoffNCI As Single
    Public runoffNSD As Single
    Public totalNSD As Single
    Public totalNCI As Single
End Class