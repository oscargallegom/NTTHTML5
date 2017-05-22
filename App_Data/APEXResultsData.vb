Public Class APEXResultsData
    Public message As String
    Public area As Single
    Public i As Integer
    Public FIFertilizer As String
    Public lastSimulation As Date
    Public SoilResults As New APEXResultsAll
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
    Public annualCropYield As New List(Of annualCrops)
    Public Sub RedimAnnualCropYield(NewSize)
        'ReDim annualCropYield(NewSize)  todo
    End Sub
End Class

Public Class annualCrops
    Public cropRecords As UShort
    Public cropName As String
    Public cropYield As Single
    Public cropYieldSD As Single
    Public cropYieldCI As Single
    Public ws As Single  'array because stress factor is for each crop.
    Public ns As Single
    Public ps As Single
    Public ts As Single
End Class
