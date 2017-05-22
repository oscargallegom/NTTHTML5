Public Class ScenariosData
    Private _name As String
    Public _operationsInfo As New List(Of OperationsData)
    Public _bmpsInfo As New BmpsData        'only used in fields_scenarios
    Public _results As New APEXResults
    Public _subareasInfo As New SubareasData
    Public _bufferInfo As New List(Of SubareasData)  'only used in fields_scenarios
    Public _femInfo As New FEMData

    Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Public Structure APEXResults
        Public message As String
        Public area As Single
        Public i As Integer
        Public FIFertilizer As String
        Public lastSimulation As Date
        Public SoilResults As ScenariosData.APEXResultsAll
        Public annualPrecipitation() As Single
        Public Sub RedimAnnualPrecipitation(NewSize)
            ReDim annualPrecipitation(NewSize)
        End Sub
        Public annualFlow() As Single
        Public Sub RedimAnnualFlow(NewSize)
            ReDim annualFlow(NewSize)
        End Sub
        Public annualSediment() As Single
        Public Sub RedimAnnualSediment(NewSize)
            ReDim annualSediment(NewSize)
        End Sub
        Public annualOrgN() As Single
        Public Sub RedimAnnualOrgN(NewSize)
            ReDim annualOrgN(NewSize)
        End Sub
        Public annualN2o() As Single
        Public Sub RedimAnnualN2o(NewSize)
            ReDim annualN2o(NewSize)
        End Sub
        Public annualOrgP() As Single
        Public Sub RedimAnnualOrgP(NewSize)
            ReDim annualOrgP(NewSize)
        End Sub
        Public annualNO3() As Single
        Public Sub RedimAnnualNO3(NewSize)
            ReDim annualNO3(NewSize)
        End Sub
        Public annualPO4() As Single
        Public Sub RedimAnnualPO4(NewSize)
            ReDim annualPO4(NewSize)
        End Sub
        Public annualCropYield() As annualCrops
        Public Sub RedimAnnualCropYield(NewSize)
            ReDim annualCropYield(NewSize)
        End Sub
    End Structure

    Public Structure annualCrops
        Public year As UShort
        Public cropRecords() As UShort
        Public cropName() As String
        Public cropYield() As Single
        Public cropYieldSD() As Single
        Public cropYieldCI() As Single
        Public ws() As Single  'array because stress factor is for each crop.
        Public ns() As Single
        Public ps() As Single
        Public ts() As Single
    End Structure

    Public Structure APEXResultsAll
        'Nitrogen information
        Public OrgN As Single   'OrgN column 50
        Public SubsurfaceN As Single 'NO3 - QN Columns 68-246
        Public RunoffN As Single   'QN column 246
        Public tileDrainN As Single 'QDRN column 145
        Public _totalN As Single
        ReadOnly Property TotalN() As Single
            Get
                Return OrgN + SubsurfaceN + tileDrainN + RunoffN
            End Get
        End Property
        Public OrgNCI As Single
        Public OrgNSD As Single
        Public subsurfaceNSD As Single
        Public subsurfaceNCI As Single
        Public tileDrainNSD As Single
        Public tileDrainNCI As Single
        Public runoffNCI As Single
        Public runoffNSD As Single
        Public _totalNCI As Single
        ReadOnly Property TotalNCI() As Single
            Get
                Return OrgNCI + subsurfaceNCI + tileDrainNCI + runoffNCI
            End Get
        End Property
        'Phosphorous Info
        Public OrgP As Single
        Public PO4 As Single
        Public tileDrainP As Single
        Public tileDrainPCI As Single
        Public tileDrainPSD As Single
        Public _totalP As Single
        ReadOnly Property TotalP() As Single
            Get
                Return OrgP + PO4 + tileDrainP
            End Get
        End Property
        Public _totalPCI As Single
        ReadOnly Property TotalPCI() As Single
            Get
                Return OrgPCI + PO4CI + tileDrainPCI
            End Get
        End Property
        Public OrgPSD As Single
        Public PO4SD As Single
        Public OrgPCI As Single
        Public PO4CI As Single
        'Sedimanet Info
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
        Public _totalSedimentCI
        ReadOnly Property TotalSedimentCI() As Single
            Get
                Return SedimentCI + ManureErosionCI
            End Get
        End Property
        Public Crops As annualCrops
        Public LeachedN As Single
        Public LeachedP As Single
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
        Public deepPerN As Single
        Public deepPerP As Single
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
    End Structure

    Public Sub AddOperations(operationsList As List(Of OperationsData), scenarioIndex As UShort)
        For Each oper In operationsList
            AddOperation(oper)
        Next
    End Sub

    Public Sub AddOperation(oper As OperationsData)
        Dim operation As OperationsData

        operation = New OperationsData
        operation.ApexCrop() = oper.ApexCrop
        operation.ApexCropName = oper.ApexCropName
        operation.ApexOp = oper.ApexOp
        operation.ApexOpAbbreviation = oper.ApexOpAbbreviation
        operation.ApexOpName = oper.ApexOpName
        operation.ApexOpType = oper.ApexOpType
        operation.ApexOpTypeName = oper.ApexOpTypeName
        operation.ApexOpv1 = oper.ApexOpv1
        operation.ApexOpv2 = oper.ApexOpv2
        operation.ApexTillCode = oper.ApexTillCode
        operation.ApexTillName = oper.ApexTillName
        operation.ConvertionUnit = oper.ConvertionUnit
        operation.Day = oper.Day
        operation.Month = oper.Month
        operation.Year = oper.Year
        operation.EventId = oper.EventId
        operation.Index = oper.Index
        operation.LuNumber = oper.LuNumber
        operation.NO3 = oper.NO3
        operation.PO4 = oper.PO4
        operation.OrgN = oper.OrgN
        operation.OrgP = oper.OrgP
        operation.K = oper.K
        operation.NH3 = oper.NH3
        operation.OpVal1 = oper.OpVal1
        operation.OpVal2 = oper.OpVal2
        operation.OpVal3 = oper.OpVal3
        operation.OpVal4 = oper.OpVal4
        operation.OpVal5 = oper.OpVal5
        operation.OpVal6 = oper.OpVal6
        operation.OpVal7 = oper.OpVal7
        operation.Period = oper.Period
        operation.Scenario = oper.Scenario
        operation.Selected = oper.Selected
        operation.TractorId = oper.TractorId
        operation.var9 = oper.var9
        operation.MixedCropData = oper.MixedCropData
        _operationsInfo.Add(operation)
        If operation.ApexOpAbbreviation = grazing Then
            _subareasInfo._line11(0).Ny1 = 1
            _subareasInfo._line12(0).Xtp1 = biomassLimit
        End If
    End Sub

    Public Sub CleanResults()
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
            .deepPerN = 0
            .deepPerP = 0
            .OrgN = 0
            .OrgNSD = 0
            .OrgNCI = 0
            .RunoffN = 0
            .runoffNCI = 0
            .runoffNSD = 0
            .SubsurfaceN = 0
            .tileDrainN = 0
            .SubsurfaceNSD = 0
            .SubsurfaceNCI = 0
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

    'Public Sub UpdateOperation(operation As OperationsData, index As Short)
    '    _operationsInfo(index).ApexCrop = operation.ApexCrop
    '    _operationsInfo(index).ApexCropName = operation.ApexCropName
    '    _operationsInfo(index).ApexOp = operation.ApexOp
    '    _operationsInfo(index).ApexOpName = operation.ApexOpName
    'End Sub
End Class
