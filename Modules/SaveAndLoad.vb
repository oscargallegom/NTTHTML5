Module SaveAndLoad
    Private output As StringBuilder = New StringBuilder()

    Public Function btnOpen_File(file As String, ByRef _project As ProjectsData) As String
        Dim message As String = "OK"

        If System.IO.File.Exists(file) Then
            LoadInfo(file, _project)
        End If

        Return message
    End Function

    Public Function SaveProject(session As String, ByRef _project As ProjectsData) As String
        Dim sFile As String = String.Empty
        Dim swFile As IO.StreamWriter = Nothing

        Try
            CreateXMLFile(output, _project)   'Save the current selection and values in the project to a xml file
            sFile = folder + "\" + NTTxmlFolder + "\" + session + ".xml"
            swFile = New IO.StreamWriter(sFile)
            swFile.Write(output)
            swFile.Close()
            swFile.Dispose()
            swFile = Nothing
            Return "OK"
        Catch ex As Exception
            Return msgDoc.Descendants("Errors").Value
        End Try
    End Function

    Public Function LoadFarmInfo(info As System.Xml.Linq.XElement, ByRef _StartInfo As StartInfo) As String
        'load farm info from a saved xml file.
        Try
            _StartInfo.farmCoordinates = info.Descendants("Coordinates").Value
            _StartInfo.farmName = info.Descendants("Name").Value
            _StartInfo.farmCentrois = info.Descendants("centroid").Value
            Return "OK"
        Catch e1 As Exception
            Return msgDoc.Descendants("Errors").Value & e1.Message
        End Try
    End Function

    Public Sub LoadStartInfo(info As System.Xml.Linq.XElement, ByRef _StartInfo As StartInfo)
        'load start info from a saved xml file.
        Try
            Dim tMax(11), tMin(11) As String
            _StartInfo.Status = info.Descendants("Status").Value
            _StartInfo.description = info.Descendants("Description").Value
            _StartInfo.projectName = info.Descendants("projectName").Value
            _StartInfo.dates = info.Descendants("dates").Value
            _StartInfo.StateAbrev = info.Descendants("StateAbrev").Value
            _StartInfo.StateName = info.Descendants("StateName").Value
            _StartInfo.countyCode = info.Descendants("CountyCode").Value
            _StartInfo.countyName = info.Descendants("CountyName").Value
            _StartInfo.stationCode = info.Descendants("StationCode").Value
            _StartInfo.stationName = info.Descendants("StationName").Value
            _StartInfo.stationWay = info.Descendants("StationWay").Value
            _StartInfo.stationYears = Val(info.Descendants("StationYears").Value)
            _StartInfo.stationInitialYear = Val(info.Descendants("StationInitialYear").Value)
            _StartInfo.stationFinalYear = Val(info.Descendants("StationFinalYear").Value)
            _StartInfo.weatherFinalYear = Val(info.Descendants("WeatherFinalYear").Value)
            _StartInfo.weatherInitialYear = Val(info.Descendants("WeatherInitialYear").Value)
            _StartInfo.yearsRotation = Val(info.Descendants("YearsRotation").Value)
            _StartInfo.currentWeatherPath = info.Descendants("CurrentWeatherPath").Value
            If IsNumeric(info.Descendants("WeatherLon").Value) Then _StartInfo.weatherLon = info.Descendants("WeatherLon").Value
            If IsNumeric(info.Descendants("WeatherLat").Value) Then _StartInfo.weatherLat = info.Descendants("WeatherLat").Value
            ReDim _StartInfo.tMax(11)
            ReDim _StartInfo.tMin(11)
            tMax = info.Descendants("tMax").Value.Split("|")
            tMin = info.Descendants("tMin").Value.Split("|")
            Dim result As Single = 0
            For i = 0 To 11
                Single.TryParse(tMax(i), result)
                'If result <= 0 Then
                '_StartInfo.tMax(i) = 0
                'Else
                _StartInfo.tMax(i) = tMax(i)
                'End If
                Single.TryParse(tMin(i), result)
                'If result <= 0 Then
                '_StartInfo.tMin(i) = 0
                'Else
                _StartInfo.tMin(i) = tMin(i)
                'End If
            Next
            _StartInfo.wp1aLat = info.Descendants("wp1aLat").Value
            _StartInfo.WindName = info.Descendants("windName").Value
            _StartInfo.Wp1Name = info.Descendants("wp1Name").Value
            _StartInfo.WindCode = info.Descendants("windCode").Value
            _StartInfo.Wp1Code = info.Descendants("wp1Code").Value
            '_sitesInfo(0).Latitude = info.Descendants("Latitude").Value
            '_sitesInfo(0).Longitude = info.Descendants("Longitude").Value
            _StartInfo.Versions = info.Descendants("Versions").Value

        Catch e1 As Exception
            'Return msgDoc.Descendants("Errors").Value & e1.Message
        End Try
    End Sub

    Public Sub loadFieldInfo(info As System.Xml.Linq.XElement, ByRef _fieldsInfo1 As List(Of FieldsData))
        'load field information
        Try
            Dim i As UShort = 0
            Dim Field As New FieldsData
            Field.Forestry = info.Descendants("Forestry").Value
            Field.Number = info.Descendants("Number").Value
            Field.Name = info.Descendants("Name").Value
            Field.Area = info.Descendants("Area").Value
            Field.Coordinates = info.Descendants("Coordinates").Value
            Field.Rchc = info.Descendants("rchc").Value
            Field.Rchk = info.Descendants("rchk").Value
            Field.RchcVal = info.Descendants("rchcVal").Value
            Field.RchkVal = info.Descendants("rchkVal").Value
            Field.AvgSlope = info.Descendants("AvgSlope").Value
            _fieldsInfo1.Add(Field)
            'currentSoilNumber = -1
            For Each soils In info.Descendants("SoilInfo")
                'load soils information
                'currentSoilNumber += 1
                loadSoilInfo(soils, _fieldsInfo1(_fieldsInfo1.Count - 1))
            Next
            For Each scenarios In info.Descendants("ScenarioInfo")
                _fieldsInfo1(_fieldsInfo1.Count - 1)._scenariosInfo.Add(loadScenariosInfo(scenarios, Field.Area))
            Next
        Catch e1 As Exception
        End Try
    End Sub
    'load scenarios for each field.
    'todo activate other loads later
    Private Function loadScenariosInfo(ByVal scenarios As System.Xml.Linq.XElement, ByVal area As Single) As ScenariosData
        Dim scenario1 = New ScenariosData
        Dim buffer As SubareasData
        scenario1.Name = scenarios.Descendants("Name").Value
        For Each operations In scenarios.Descendants("Operations")
            If operations.Descendants("Scenario").Value <> "" Then
                loadOperationInfo(operations, scenario1._operationsInfo, area)
            End If
        Next
        For Each sba In scenarios.Descendants("Buffers")
            buffer = New SubareasData
            loadSubareaInfo(sba, buffer)
            scenario1._bufferInfo.Add(buffer)
        Next
        For Each bmp1 In scenarios.Descendants("Bmps")
            loadBMPInfo(bmp1, scenario1)
        Next

        If scenarios.Descendants("FEMresults").Count > 0 Then LoadFEMResultInfo(scenarios.Descendants("FEMresults")(0), scenario1)

        For Each result In scenarios.Descendants("Results")
            loadResultInfo(result, scenario1._results)
        Next

        Return scenario1
    End Function

    'load scenarios for each soil
    Private Function loadScenariosInfo(ByVal scenarios As System.Xml.Linq.XElement, ByVal area As Single, ByVal soils As System.Xml.Linq.XElement) As ScenariosData
        Dim scenario1 = New ScenariosData
        Dim buffer As SubareasData
        Try
            scenario1.Name = scenarios.Descendants("Name").Value
            For Each operations In scenarios.Descendants("Operations")
                loadOperationInfo(operations, scenario1._operationsInfo, area)
            Next
            'no buffers in soils
            For Each sba In scenarios.Descendants("Buffers")
                'loadSubareaInfo(sba, scenario1._subareasInfo)
                Buffer = New SubareasData
                loadSubareaInfo(sba, Buffer)
                scenario1._bufferInfo.Add(Buffer)
            Next
            'no BMPs in soils.
            For Each bmp1 In scenarios.Descendants("Bmps")
                loadBMPInfo(bmp1, scenario1)
            Next
            'no FEM results in soils
            If scenarios.Descendants("FEMresults").Count > 0 Then LoadFEMResultInfo(scenarios.Descendants("FEMresults")(0), scenario1)

            For Each result In scenarios.Descendants("Results")
                loadResultInfo(result, scenario1._results)
            Next

            'soils.subarea start here
            For Each sba In scenarios.Descendants("Subareas")
                loadSubareaInfo(sba, scenario1._subareasInfo)
            Next

            Return scenario1

        Catch ex As Exception
            ''showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
            Return scenario1
        End Try
    End Function

    Public Sub loadOperationInfo(ByVal operations As System.Xml.Linq.XElement, ByRef operationsInfo As List(Of OperationsData), ByVal area As Single)
        'load operations for each scenario
        Try
            Dim operation = New OperationsData
            operation.Scenario = operations.Descendants("Scenario").Value
            operation.EventId = operations.Descendants("EventId").Value
            operation.ApexOp = operations.Descendants("ApexOp").Value
            operation.ApexOpAbbreviation = operations.Descendants("ApexOpAbbreviation").Value
            operation.ApexOpName = operations.Descendants("ApexOpName").Value
            operation.ApexCrop = operations.Descendants("ApexCrop").Value
            operation.ApexCropName = operations.Descendants("ApexCropName").Value
            operation.Year = operations.Descendants("Year").Value
            operation.Month = operations.Descendants("Month").Value
            operation.Day = operations.Descendants("Day").Value
            operation.Period = operations.Descendants("Period").Value
            operation.ApexTillCode = operations.Descendants("ApexTillCode").Value
            operation.ApexTillName = operations.Descendants("ApexTillName").Value
            operation.var9 = operations.Descendants("var9").Value
            operation.ApexOpType = operations.Descendants("ApexFert").Value
            operation.ApexOpv1 = operations.Descendants("ApexOpv1").Value
            If Not operation.ApexOpAbbreviation Is Nothing AndAlso operation.ApexOpAbbreviation.Trim = grazing Then operation.setAnimalRate(operation.ApexTillCode, area)
            operation.ApexOpv2 = operations.Descendants("ApexOpv2").Value
            operation.OrgN = operations.Descendants("OrgN").Value
            operation.NO3 = operations.Descendants("NO3").Value
            operation.OrgP = operations.Descendants("OrgP").Value
            operation.PO4 = operations.Descendants("PO4").Value
            operation.NH3 = operations.Descendants("NH3").Value
            operation.K = operations.Descendants("K").Value
            operation.TractorId = operations.Descendants("TractorId").Value
            operation.OpVal1 = operations.Descendants("OpVal1").Value
            operation.OpVal2 = operations.Descendants("OpVal2").Value
            operation.OpVal3 = operations.Descendants("OpVal3").Value
            operation.OpVal4 = operations.Descendants("OpVal4").Value
            operation.OpVal5 = operations.Descendants("OpVal5").Value
            operation.OpVal6 = operations.Descendants("OpVal6").Value
            operation.OpVal7 = operations.Descendants("OpVal7").Value
            operation.MixedCropData = operations.Descendants("MixedCropData").Value
            operation.Index = operations.Descendants("Index").Value
            operation.LuNumber = operations.Descendants("LuNumber").Value
            operation.ConvertionUnit = operations.Descendants("ConvertionUnit").Value
            operationsInfo.Add(operation)
        Catch ex As Exception
            ''showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    'Private Sub loadBufferInfo(ByVal sba As System.Xml.Linq.XElement, ByRef scenario1 As ScenariosData)
    '    Try
    '        Dim buffer = New SubareasData
    '        With buffer._line2(0)
    '            .Iapl = sba.Descendants("Iapl").Value
    '            .Ii = sba.Descendants("Ii").Value
    '            .Imw = sba.Descendants("Imw").Value
    '            .Inps = sba.Descendants("Inps").Value
    '            .Iops = sba.Descendants("Iops").Value
    '            .Iow = sba.Descendants("Iow").Value
    '            .Ipts = sba.Descendants("Ipts").Value
    '            .Isao = sba.Descendants("Isao").Value
    '            .Iwth = sba.Descendants("Iwth").Value
    '            .Luns = sba.Descendants("Luns").Value
    '            .Nvcn = sba.Descendants("Nvcn").Value
    '        End With
    '        With buffer
    '            .SbaType = sba.Descendants("SbaType").Value
    '            .SoilComponent = sba.Descendants("SoilComponent").Value
    '            .SoilGroup = sba.Descendants("SoilGroup").Value
    '            .SoilKey = sba.Descendants("SoilKey").Value
    '            .SoilSymbol = sba.Descendants("SoilSymbol").Value
    '            .SubareaNumber = sba.Descendants("SubareaNumber").Value
    '            .SubareaTitle = sba.Descendants("SubareaTitle").Value
    '        End With
    '        With buffer._line3(0)
    '            .Angl = sba.Descendants("Angl").Value
    '            .Azm = sba.Descendants("Azm").Value
    '            .Fl = sba.Descendants("Fl").Value
    '            .Fw = sba.Descendants("Fw").Value
    '            .Sno = sba.Descendants("Sno").Value
    '            .Stdo = sba.Descendants("Stdo").Value
    '            .Xct = sba.Descendants("Xct").Value
    '            .Yct = sba.Descendants("Yct").Value
    '        End With
    '        With buffer._line4(0)
    '            .Chd = sba.Descendants("Chd").Value
    '            .chl = sba.Descendants("chl").Value
    '            .Chn = sba.Descendants("Chn").Value
    '            .Chs = sba.Descendants("Chs").Value
    '            .Ffpq = sba.Descendants("Ffpq").Value
    '            .Slp = sba.Descendants("Slp").Value
    '            .Slpg = sba.Descendants("Slpg").Value
    '            .Upn = sba.Descendants("Upn").Value
    '            .Urbf = sba.Descendants("Urbf").Value
    '            .Wsa = sba.Descendants("Wsa").Value
    '        End With
    '        With buffer._line5(0)
    '            .Rcbw = sba.Descendants("Rcbw").Value
    '            .Rchc = sba.Descendants("Rchc").Value
    '            .Rchd = sba.Descendants("Rchd").Value
    '            .Rchk = sba.Descendants("Rchk").Value
    '            .Rchl = sba.Descendants("Rchl").Value
    '            .Rchn = sba.Descendants("Rchn").Value
    '            .Rchs = sba.Descendants("Rchs").Value
    '            .Rctw = sba.Descendants("Rctw").Value
    '            .Rfpl = sba.Descendants("Rfpl").Value
    '            .Rfpw = sba.Descendants("Rfpw").Value
    '        End With
    '        With buffer._line6(0)
    '            .Rsae = sba.Descendants("Rsae").Value
    '            .Rsap = sba.Descendants("Rsap").Value
    '            .Rsee = sba.Descendants("Rsee").Value
    '            .Rsep = sba.Descendants("Rsep").Value
    '            .Rsrr = sba.Descendants("Rsrr").Value
    '            .Rsv = sba.Descendants("Rsv").Value
    '            .Rsve = sba.Descendants("Rsve").Value
    '            .Rsvp = sba.Descendants("Rsvp").Value
    '            .Rsyn = sba.Descendants("Rsyn").Value
    '            .Rsys = sba.Descendants("Rsys").Value
    '        End With
    '        With buffer._line7(0)
    '            .Bcof = sba.Descendants("Bcof").Value
    '            .Bffl = sba.Descendants("Bffl").Value
    '            .Pcof = sba.Descendants("Pcof").Value
    '            .Rsbd = sba.Descendants("Rsbd").Value
    '            .Rsdp = sba.Descendants("Rsdp").Value
    '            .Rshc = sba.Descendants("Rshc").Value
    '        End With
    '        With buffer._line8(0)
    '            .Idf1 = sba.Descendants("Idf1").Value
    '            .Idf2 = sba.Descendants("Idf2").Value
    '            .Idf3 = sba.Descendants("Idf3").Value
    '            .Idf4 = sba.Descendants("Idf4").Value
    '            .Idf5 = sba.Descendants("Idf5").Value
    '            .Idr = sba.Descendants("Idr").Value
    '            .Ifa = sba.Descendants("Ifa").Value
    '            .Ifd = sba.Descendants("Ifd").Value
    '            .Iri = sba.Descendants("Iri").Value
    '            .Lm = sba.Descendants("Lm").Value
    '            .Nirr = sba.Descendants("Nirr").Value
    '        End With
    '        With buffer._line9(0)
    '            .Armn = sba.Descendants("Armn").Value
    '            .Armx = sba.Descendants("Armx").Value
    '            .Bft = sba.Descendants("Bft").Value
    '            .Bir = sba.Descendants("Bir").Value
    '            .Drt = sba.Descendants("Drt").Value
    '            .Efi = sba.Descendants("Efi").Value
    '            .Fdsf = sba.Descendants("Fdsf").Value
    '            .Fmx = sba.Descendants("Fmx").Value
    '            .Fnp4 = sba.Descendants("Fnp4").Value
    '            .Vimx = sba.Descendants("Vimx").Value
    '        End With
    '        With buffer._line10(0)
    '            .Coww = sba.Descendants("Coww").Value
    '            .Dalg = sba.Descendants("Dalg").Value
    '            .Ddlg = sba.Descendants("Ddlg").Value
    '            .Firg = sba.Descendants("Firg").Value
    '            .Fnp2 = sba.Descendants("Fnp2").Value
    '            .Fnp5 = sba.Descendants("Fnp5").Value
    '            .Pec = sba.Descendants("Pec").Value
    '            .Sflg = sba.Descendants("Sflg").Value
    '            .Solq = sba.Descendants("Solq").Value
    '            .Vlgn = sba.Descendants("Vlgn").Value
    '        End With
    '        With buffer._line11(0)
    '            .Ny1 = sba.Descendants("Ny1").Value
    '            .Ny2 = sba.Descendants("Ny2").Value
    '            .Ny3 = sba.Descendants("Ny3").Value
    '            .Ny4 = sba.Descendants("Ny4").Value
    '        End With
    '        With buffer._line12(0)
    '            .Xtp1 = sba.Descendants("Xtp1").Value
    '            .Xtp2 = sba.Descendants("Xtp2").Value
    '            .Xtp3 = sba.Descendants("Xtp3").Value
    '            .Xtp4 = sba.Descendants("Xtp4").Value
    '        End With
    '        scenario1._bufferInfo.Add(buffer)
    '    Catch ex As Exception
    '        'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
    '    End Try
    'End Sub

    Private Sub loadBMPInfo(ByVal bmp1 As System.Xml.Linq.XElement, ByRef scenario1 As ScenariosData)
        Try
            scenario1._bmpsInfo.AFEff = bmp1.Descendants("AFEff").Value
            scenario1._bmpsInfo.AFFreq = bmp1.Descendants("AFFreq").Value
            scenario1._bmpsInfo.AFMaxSingleApp = bmp1.Descendants("AFMaxSingleApp").Value
            scenario1._bmpsInfo.AFType = bmp1.Descendants("AFType").Value
            scenario1._bmpsInfo.AFWaterStressFactor = bmp1.Descendants("AFWaterStressFactor").Value
            scenario1._bmpsInfo.AFSafetyFactor = bmp1.Descendants("AFSafetyFactor").Value
            scenario1._bmpsInfo.AFNConc = bmp1.Descendants("AFNConc").Value
            scenario1._bmpsInfo.AIEff = bmp1.Descendants("AIEff").Value
            scenario1._bmpsInfo.AIFreq = bmp1.Descendants("AIFreq").Value
            scenario1._bmpsInfo.AIMaxSingleApp = bmp1.Descendants("AIMaxSingleApp").Value
            scenario1._bmpsInfo.AIType = bmp1.Descendants("AIType").Value
            scenario1._bmpsInfo.AIWaterStressFactor = bmp1.Descendants("AIWaterStressFactor").Value
            scenario1._bmpsInfo.AISafetyFactor = bmp1.Descendants("AISafetyFactor").Value
            scenario1._bmpsInfo.FSEff = bmp1.Descendants("FSEff").Value
            scenario1._bmpsInfo.FSArea = bmp1.Descendants("FSArea").Value
            scenario1._bmpsInfo.FSCrop = bmp1.Descendants("FSCrop").Value
            scenario1._bmpsInfo.FSslopeRatio = bmp1.Descendants("FSslopeRatio").Value
            scenario1._bmpsInfo.FSWidth = bmp1.Descendants("FSWidth").Value
            scenario1._bmpsInfo.SdgEff = bmp1.Descendants("SdgEff").Value
            scenario1._bmpsInfo.SdgArea = bmp1.Descendants("SdgArea").Value
            scenario1._bmpsInfo.SdgCrop = bmp1.Descendants("SdgCrop").Value
            scenario1._bmpsInfo.SdgslopeRatio = bmp1.Descendants("SdgslopeRatio").Value
            scenario1._bmpsInfo.SdgWidth = bmp1.Descendants("SdgWidth").Value
            scenario1._bmpsInfo.Lm = bmp1.Descendants("Lm").Value
            scenario1._bmpsInfo.PndF = bmp1.Descendants("PndF").Value
            scenario1._bmpsInfo.RFArea = bmp1.Descendants("RFArea").Value
            scenario1._bmpsInfo.RFEff = bmp1.Descendants("RFEff").Value
            scenario1._bmpsInfo.RFGrassFieldPortion = bmp1.Descendants("RFGrassFieldPortion").Value
            scenario1._bmpsInfo.RFslopeRatio = bmp1.Descendants("RFslopeRatio").Value
            scenario1._bmpsInfo.RFWidth = bmp1.Descendants("RFWidth").Value
            scenario1._bmpsInfo.Sbs = bmp1.Descendants("Sbs").Value
            scenario1._bmpsInfo.SlopeRed = bmp1.Descendants("SlopeRed").Value
            scenario1._bmpsInfo.TileDrainDepth = bmp1.Descendants("TileDrainDepth").Value
            scenario1._bmpsInfo.Ts = bmp1.Descendants("Ts").Value
            scenario1._bmpsInfo.WLArea = bmp1.Descendants("WLArea").Value
            scenario1._bmpsInfo.WWCrop = bmp1.Descendants("WWCrop").Value
            scenario1._bmpsInfo.WWWidth = bmp1.Descendants("WWWidth").Value
            scenario1._bmpsInfo.PPNDWidth = bmp1.Descendants("PPNDWidth").Value
            scenario1._bmpsInfo.PPDSWidth = bmp1.Descendants("PPDSWidth").Value
            scenario1._bmpsInfo.PPDEWidth = bmp1.Descendants("PPDEWidth").Value
            scenario1._bmpsInfo.PPTWWidth = bmp1.Descendants("PPTWWidth").Value
            scenario1._bmpsInfo.PPNDSides = bmp1.Descendants("PPNDSides").Value
            scenario1._bmpsInfo.PPDSSides = bmp1.Descendants("PPDSSides").Value
            scenario1._bmpsInfo.PPDESides = bmp1.Descendants("PPDESides").Value
            scenario1._bmpsInfo.PPTWSides = bmp1.Descendants("PPTWSides").Value
            scenario1._bmpsInfo.PPDEResArea = bmp1.Descendants("PPDEResArea").Value
            scenario1._bmpsInfo.PPTWResArea = bmp1.Descendants("PPTWResArea").Value
            scenario1._bmpsInfo.CBCrop = bmp1.Descendants("CBCrop").Value
            scenario1._bmpsInfo.CBBWidth = bmp1.Descendants("CBBWidth").Value
            scenario1._bmpsInfo.CBCWidth = bmp1.Descendants("CBCWidth").Value
            scenario1._bmpsInfo.SFAnimals = bmp1.Descendants("SFAnimals").Value
            scenario1._bmpsInfo.SFCode = bmp1.Descendants("SFCode").Value
            scenario1._bmpsInfo.SFDays = bmp1.Descendants("SFDays").Value
            scenario1._bmpsInfo.SFDryManure = bmp1.Descendants("SFDryManure").Value
            scenario1._bmpsInfo.SFHours = bmp1.Descendants("SFHours").Value
            scenario1._bmpsInfo.SFName = bmp1.Descendants("SFName").Value
            scenario1._bmpsInfo.SFNo3 = bmp1.Descendants("SFNo3").Value
            scenario1._bmpsInfo.SFOrgN = bmp1.Descendants("SFOrgN").Value
            scenario1._bmpsInfo.SFOrgP = bmp1.Descendants("SFOrgP").Value
            scenario1._bmpsInfo.SFPo4 = bmp1.Descendants("SFPo4").Value
            scenario1._bmpsInfo.CcMaximumTeperature = bmp1.Descendants("CcMaximumTeperature").Value
            scenario1._bmpsInfo.CcMinimumTeperature = bmp1.Descendants("CcMinimumTeperature").Value
            scenario1._bmpsInfo.CcPrecipitation = bmp1.Descendants("CcPrecipitation").Value
        Catch ex As Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Private Sub LoadFEMResultInfo(ByVal result As System.Xml.Linq.XElement, ByRef scenario1 As ScenariosData)

        Try        '***** FEM Result information ****
            scenario1._femInfo.TotalRevenue = result.Descendants("TotalRevenue").Value
            scenario1._femInfo.TotalCost = result.Descendants("TotalCost").Value
            scenario1._femInfo.NetReturn = result.Descendants("NetReturn").Value
            scenario1._femInfo.NetCashFlow = result.Descendants("NetCashFlow").Value
            '***** End FEM Results information ****
        Catch e1 As Exception
            ''showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & e1.Message)
        End Try
    End Sub

    Private Sub loadResultInfo(ByVal result As System.Xml.Linq.XElement, ByRef results As ScenariosData.APEXResults)
        Dim i As UShort = 0
        Try
            results.area = result.Descendants("area").Value
            results.lastSimulation = result.Descendants("lastSimulation").Value
            With results.SoilResults.Crops
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropName")
                    ReDim Preserve .cropName(i)
                    .cropName(i) = crop.Value
                    i += 1
                Next
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropRecord")
                    ReDim Preserve .cropRecords(i)
                    .cropRecords(i) = crop.Value
                    i += 1
                Next
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropYield")
                    ReDim Preserve .cropYield(i)
                    .cropYield(i) = crop.Value
                    i += 1
                Next
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropYieldCI")
                    ReDim Preserve .cropYieldCI(i)
                    .cropYieldCI(i) = crop.Value
                    i += 1
                Next
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropYieldSD")
                    ReDim Preserve .cropYieldSD(i)
                    .cropYieldSD(i) = crop.Value
                    i += 1
                Next
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropns")
                    ReDim Preserve .ns(i)
                    .ns(i) = crop.Value
                    i += 1
                Next
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropps")
                    ReDim Preserve .ps(i)
                    .ps(i) = crop.Value
                    i += 1
                Next
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropts")
                    ReDim Preserve .ts(i)
                    .ts(i) = crop.Value
                    i += 1
                Next
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropws")
                    ReDim Preserve .ws(i)
                    .ws(i) = crop.Value
                    i += 1
                Next
            End With
            results.FIFertilizer = result.Descendants("FIFertilizer").Value
            results.i = result.Descendants("i").Value

            For Each soilResult In result.Descendants("Soil")
                results.SoilResults.co2 = soilResult.Descendants("co2").Value
                results.SoilResults.deepPerFlow = result.Descendants("deepPerFlow").Value
                results.SoilResults.deepPerFlowCI = result.Descendants("deepPerFlowCI").Value
                results.SoilResults.runoff = result.Descendants("runoff").Value
                results.SoilResults.runoffCI = result.Descendants("runoffCI").Value
                results.SoilResults.runoffSD = result.Descendants("runoffSD").Value
                results.SoilResults.subsurfaceFlow = result.Descendants("subsurfaceFlow").Value
                results.SoilResults.subsurfaceFlowCI = result.Descendants("subsurfaceFlowCI").Value
                results.SoilResults.subsurfaceFlowSD = result.Descendants("subsurfaceFlowSD").Value
                results.SoilResults.irrigation = result.Descendants("irrigation").Value
                results.SoilResults.irrigationCI = result.Descendants("irrigationCI").Value
                results.SoilResults.LeachedN = result.Descendants("LeachedN").Value
                results.SoilResults.LeachedP = result.Descendants("LeachedP").Value
                results.SoilResults.n2o = result.Descendants("n2o").Value
                results.SoilResults.RunoffN = result.Descendants("runoffN").Value
                results.SoilResults.runoffNCI = result.Descendants("runoffNCI").Value
                results.SoilResults.runoffNSD = result.Descendants("runoffNSD").Value
                results.SoilResults.SubsurfaceN = result.Descendants("subsurfaceN").Value
                results.SoilResults.subsurfaceNCI = result.Descendants("subsurfaceNCI").Value
                results.SoilResults.subsurfaceNSD = result.Descendants("subsurfaceNSD").Value
                results.SoilResults.OrgN = result.Descendants("OrgN").Value
                results.SoilResults.OrgNCI = result.Descendants("OrgNCI").Value
                results.SoilResults.OrgNSD = result.Descendants("OrgNSD").Value
                results.SoilResults.OrgP = result.Descendants("OrgP").Value
                results.SoilResults.OrgPCI = result.Descendants("OrgPCI").Value
                results.SoilResults.OrgPSD = result.Descendants("OrgPSD").Value
                results.SoilResults.PO4 = result.Descendants("PO4").Value
                results.SoilResults.PO4CI = result.Descendants("PO4CI").Value
                results.SoilResults.PO4SD = result.Descendants("PO4SD").Value
                results.SoilResults.Sediment = result.Descendants("Sediment").Value
                results.SoilResults.SedimentCI = result.Descendants("SedimentCI").Value
                results.SoilResults.SedimentSD = result.Descendants("SedimentSD").Value
                results.SoilResults.tileDrainFlow = result.Descendants("tileDrainFlow").Value
                results.SoilResults.tileDrainFlowCI = result.Descendants("tileDrainFlowCI").Value
                results.SoilResults.tileDrainFlowSD = result.Descendants("tileDrainFlowSD").Value
                results.SoilResults.tileDrainN = result.Descendants("tileDrainN").Value
                results.SoilResults.tileDrainNCI = result.Descendants("tileDrainNCI").Value
                results.SoilResults.tileDrainNSD = result.Descendants("tileDrainNSD").Value
                results.SoilResults.tileDrainP = result.Descendants("tileDrainP").Value
                results.SoilResults.tileDrainPCI = result.Descendants("tileDrainPCI").Value
                results.SoilResults.tileDrainPSD = result.Descendants("tileDrainPSD").Value
                i = 0
                results.RedimAnnualFlow(result.Descendants("annualFlow").Descendants("flow").Count - 1)
                For Each value In result.Descendants("annualFlow").Descendants("flow")
                    results.annualFlow(i) = value.Value
                    i += 1
                Next

                i = 0
                results.RedimAnnualNO3(result.Descendants("annualNO3").Descendants("NO3").Count - 1)
                For Each value In result.Descendants("annualNO3").Descendants("NO3")
                    results.annualNO3(i) = value.Value
                    i += 1
                Next
                i = 0
                results.RedimAnnualOrgN(result.Descendants("annualOrgN").Descendants("OrgN").Count - 1)
                For Each value In result.Descendants("annualOrgN").Descendants("OrgN")
                    results.annualOrgN(i) = value.Value
                    i += 1
                Next
                i = 0
                results.RedimAnnualN2o(result.Descendants("annualN2O").Descendants("N2O").Count - 1)
                For Each value In result.Descendants("annualN2O").Descendants("N2O")
                    results.annualN2o(i) = value.Value
                    i += 1
                Next
                i = 0
                results.RedimAnnualOrgP(result.Descendants("annualOrgP").Descendants("OrgP").Count - 1)
                For Each value In result.Descendants("annualOrgP").Descendants("OrgP")
                    results.annualOrgP(i) = value.Value
                    i += 1
                Next
                i = 0
                results.RedimAnnualPO4(result.Descendants("annualPO4").Descendants("PO4").Count - 1)
                For Each value In result.Descendants("annualPO4").Descendants("PO4")
                    results.annualPO4(i) = value.Value
                    i += 1
                Next
                i = 0
                results.RedimAnnualSediment(result.Descendants("annualSediment").Descendants("Sediment").Count - 1)
                For Each value In result.Descendants("annualSediment").Descendants("Sediment")
                    results.annualSediment(i) = value.Value
                    i += 1
                Next

                i = 0
                results.RedimAnnualPrecipitation(result.Descendants("annualPrecipitation").Descendants("precipitation").Count - 1)
                For Each value In result.Descendants("annualPrecipitation").Descendants("precipitation")
                    results.annualPrecipitation(i) = value.Value
                    i += 1
                Next

                Dim l As UShort = 0
                i = 0
                results.RedimAnnualCropYield(result.Descendants("annualCropYield").Descendants("crop").Count - 1)
                For Each value In result.Descendants("annualCropYield").Descendants("crop")
                    ReDim results.annualCropYield(i).cropName(value.Descendants("cropName").Count - 1)
                    ReDim results.annualCropYield(i).cropYield(value.Descendants("cropYield").Count - 1)
                    l = 0
                    For Each name1 In value.Descendants("cropName")
                        results.annualCropYield(i).cropName(l) = name1.Value
                        l += 1
                    Next
                    l = 0
                    For Each yield In value.Descendants("cropYield")
                        results.annualCropYield(i).cropYield(l) = yield.Value
                        l += 1
                    Next
                    i += 1
                Next
            Next

        Catch ex As Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Private Sub loadSoilInfo(ByVal soils As System.Xml.Linq.XElement, ByRef Field As FieldsData)
        Dim _soil As SoilsData
        _soil = New SoilsData
        _soil.Selected = soils.Descendants("Selected").Value
        _soil.SoilNumber = soils.Descendants("SoilNumber").Value
        _soil.Field = soils.Descendants("Field").Value
        _soil.Name = soils.Descendants("Name").Value
        _soil.Percentage = soils.Descendants("Area").Value
        _soil.Albedo = soils.Descendants("Albedo").Value
        _soil.Slope = soils.Descendants("Slope").Value
        _soil.SSACode = soils.Descendants("SSACode").Value
        _soil.SSAName = soils.Descendants("SSAName").Value
        _soil.Key = soils.Descendants("Key").Value
        _soil.Symbol = soils.Descendants("Symbol").Value
        _soil.Group = soils.Descendants("Group").Value
        _soil.Component = soils.Descendants("Component").Value
        _soil.Ffc = soils.Descendants("Ffc").Value
        _soil.Wtmn = soils.Descendants("Wtmn").Value
        _soil.Wtmx = soils.Descendants("Wtmx").Value
        _soil.Wtbl = soils.Descendants("Wtbl").Value
        _soil.Gwst = soils.Descendants("Gwst").Value
        _soil.Gwmx = soils.Descendants("Gwmx").Value
        _soil.Rftt = soils.Descendants("Rftt").Value
        _soil.Rfpk = soils.Descendants("Rfpk").Value
        _soil.Tsla = soils.Descendants("tsla").Value
        _soil.Xids = soils.Descendants("xids").Value
        _soil.Rtn1 = soils.Descendants("Rtn1").Value
        _soil.Xidk = soils.Descendants("Xidk").Value
        _soil.Zqt = soils.Descendants("Zqt").Value
        _soil.Zf = soils.Descendants("Zf").Value
        _soil.Ztk = soils.Descendants("Ztk").Value
        _soil.Fbm = soils.Descendants("Fbm").Value
        _soil.Fhp = soils.Descendants("Fhp").Value
        'soil.scenarios start here
        Field._soilsInfo.Add(_soil)
        For Each scen In soils.Descendants("SoilScenarioInfo")
            _soil._scenariosInfo.Add(loadScenariosInfo(scen, _soil.Percentage * Field.Area, soils))
        Next
        For Each layers In soils.Descendants("LayerInfo")
            loadLayerInfo(layers, _soil)
        Next
    End Sub

    Private Sub loadLayerInfo(layers As System.Xml.Linq.XElement, ByRef Soil As SoilsData)
        'load soil layers
        Try
            Dim Layer = New LayersData
            Layer.LayerNumber = layers.Descendants("LayerNumber").Value
            Layer.Depth = layers.Descendants("Depth").Value
            Layer.SoilP = layers.Descendants("SoilP").Value
            Layer.BD = layers.Descendants("BD").Value
            Layer.Sand = layers.Descendants("Sand").Value
            Layer.Silt = layers.Descendants("Silt").Value
            Layer.OM = layers.Descendants("OM").Value
            Layer.PH = layers.Descendants("PH").Value
            Layer.Satc = layers.Descendants("SatC").Value
            Layer.Bdd = layers.Descendants("Bdd").Value
            Layer.Wn = layers.Descendants("Wn").Value
            Layer.Uw = layers.Descendants("Uw").Value
            Layer.Smb = layers.Descendants("Smb").Value
            Layer.Rsd = layers.Descendants("Rsd").Value
            Layer.Rok = layers.Descendants("Rok").Value
            Layer.Psp = layers.Descendants("Psp").Value
            Layer.Fc = layers.Descendants("Fc").Value
            Layer.Cnds = layers.Descendants("Cnds").Value
            Layer.Cac = layers.Descendants("cac").Value
            Layer.Cec = layers.Descendants("cec").Value
            'If Layer.LayerNumber = 1 And Layer.Depth > 3.94 Then
            '    Dim layer0 = New LayersData
            '    layer0.LayerNumber = 0
            '    layer0.Depth = 3.94
            '    layer0.SoilP = Layer.SoilP
            '    layer0.BD = Layer.BD
            '    layer0.Sand = Layer.Sand
            '    layer0.Silt = Layer.Silt
            '    layer0.OM = Layer.OM
            '    layer0.PH = Layer.PH
            '    layer0.Satc = Layer.Satc
            '    layer0.Bdd = Layer.Bdd
            '    layer0.Wn = Layer.Wn
            '    layer0.Uw = Layer.Uw
            '    layer0.Smb = Layer.Smb
            '    layer0.Rsd = Layer.Rsd
            '    layer0.Rok = Layer.Rok
            '    layer0.Psp = Layer.Psp
            '    layer0.Fc = Layer.Fc
            '    layer0.Cnds = Layer.Cnds
            '    layer0.Cac = Layer.Cac
            '    layer0.Cec = Layer.Cec
            '    Soil._layersInfo.Add(layer0)
            'End If
            'If Layer.Depth > 8 Then Layer.SoilP = 0
            Soil._layersInfo.Add(Layer)
        Catch e1 As Exception
        End Try
    End Sub

    Private Sub loadSiteInfo(info As System.Xml.Linq.XElement, ByRef _sitesInfo As List(Of SiteData), ByRef _startInfo As StartInfo)
        'load start info from a saved xml file.
        Try
            Dim newSite As SiteData = New SiteData
            newSite.Latitude = info.Descendants("Latitude").Value
            newSite.Longitude = info.Descendants("Longitude").Value
            If _StartInfo.weatherLat = 0 Then _StartInfo.weatherLat = newSite.Latitude
            If _StartInfo.weatherLon = 0 Then _StartInfo.weatherLon = newSite.Longitude
            newSite.Elevation = info.Descendants("Elevation").Value
            newSite.Apm = info.Descendants("Apm").Value
            newSite.Co2x = info.Descendants("Co2").Value
            newSite.Cqnx = info.Descendants("Cqn").Value
            newSite.Rfnx = info.Descendants("Rfn").Value
            newSite.Upr = info.Descendants("Upr").Value
            newSite.Unr = info.Descendants("Unr").Value
            _sitesInfo.Add(newSite)
        Catch e1 As Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & e1.Message)
        End Try
    End Sub

    Private Sub loadControlInfo(value As System.Xml.Linq.XElement, ByRef _controlValues As List(Of ParmsData))
        'load apexcontrol values
        Try
            Dim parmValue As New ParmsData
            parmValue.Code = value.Descendants("Code").Value
            parmValue.Name = value.Descendants("Name").Value
            parmValue.Value1 = value.Descendants("Value").Value
            parmValue.Range1 = value.Descendants("Range1").Value
            parmValue.Range2 = value.Descendants("Range2").Value
            parmValue.State1 = value.Descendants("State").Value
            _controlValues.Add(parmValue)
        Catch ex As Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Private Sub loadParmInfo(value As System.Xml.Linq.XElement, ByRef _parmValues As List(Of ParmsData))
        'load parm values
        Try
            Dim parmValue As New ParmsData
            parmValue = New ParmsData
            parmValue.Code = value.Descendants("Code").Value
            parmValue.Name = value.Descendants("Name").Value
            parmValue.Value1 = value.Descendants("Value").Value
            parmValue.Range1 = value.Descendants("Range1").Value
            parmValue.Range2 = value.Descendants("Range2").Value
            parmValue.State1 = value.Descendants("State").Value
            _parmValues.Add(parmValue)
        Catch ex As Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Private Sub loadSubareaInfo(sba As System.Xml.Linq.XElement, ByRef subareasInfo As SubareasData)
        'load subareas information. these are linked to soils. There is one subarea for each scenarion
        Try
            With subareasInfo
                .SoilComponent = sba.Descendants("SoilComponent").Value
                .SoilGroup = sba.Descendants("SoilGroup").Value
                .SoilKey = sba.Descendants("SoilKey").Value
                .SoilSymbol = sba.Descendants("SoilSymbol").Value
                .SubareaNumber = sba.Descendants("SubareaNumber").Value
                .SubareaTitle = sba.Descendants("SubareaTitle").Value
                .SbaType = sba.Descendants("SbaType").Value
            End With
            With subareasInfo._line2(0)
                .Iapl = sba.Descendants("Iapl").Value
                .Ii = sba.Descendants("Ii").Value
                .Imw = sba.Descendants("Imw").Value
                .Inps = sba.Descendants("Inps").Value
                .Iops = sba.Descendants("Iops").Value
                .Iow = sba.Descendants("Iow").Value
                .Ipts = sba.Descendants("Ipts").Value
                .Isao = sba.Descendants("Isao").Value
                .Iwth = sba.Descendants("Iwth").Value
                .Luns = sba.Descendants("Luns").Value
                .Nvcn = sba.Descendants("Nvcn").Value
            End With
            With subareasInfo._line3(0)
                .Angl = sba.Descendants("Angl").Value
                .Azm = sba.Descendants("Azm").Value
                .Fl = sba.Descendants("Fl").Value
                .Fw = sba.Descendants("Fw").Value
                .Sno = sba.Descendants("Sno").Value
                .Stdo = sba.Descendants("Stdo").Value
                .Xct = sba.Descendants("Xct").Value
                .Yct = sba.Descendants("Yct").Value
            End With
            With subareasInfo._line4(0)
                .Chd = sba.Descendants("Chd").Value
                .chl = sba.Descendants("chl").Value
                .Chn = sba.Descendants("Chn").Value
                .Chs = sba.Descendants("Chs").Value
                .Ffpq = sba.Descendants("Ffpq").Value
                .Slp = sba.Descendants("Slp").Value
                .Slpg = sba.Descendants("Slpg").Value
                .Upn = sba.Descendants("Upn").Value
                .Urbf = sba.Descendants("Urbf").Value
                .Wsa = sba.Descendants("Wsa").Value
            End With
            With subareasInfo._line5(0)
                .Rcbw = sba.Descendants("Rcbw").Value
                .Rchc = sba.Descendants("Rchc").Value
                .Rchd = sba.Descendants("Rchd").Value
                .Rchk = sba.Descendants("Rchk").Value
                .Rchl = sba.Descendants("Rchl").Value
                .Rchn = sba.Descendants("Rchn").Value
                .Rchs = sba.Descendants("Rchs").Value
                .Rctw = sba.Descendants("Rctw").Value
                .Rfpl = sba.Descendants("Rfpl").Value
                .Rfpw = sba.Descendants("Rfpw").Value
            End With
            With subareasInfo._line6(0)
                .Rsae = sba.Descendants("Rsae").Value
                .Rsap = sba.Descendants("Rsap").Value
                .Rsee = sba.Descendants("Rsee").Value
                .Rsep = sba.Descendants("Rsep").Value
                .Rsrr = sba.Descendants("Rsrr").Value
                .Rsv = sba.Descendants("Rsv").Value
                .Rsve = sba.Descendants("Rsve").Value
                .Rsvp = sba.Descendants("Rsvp").Value
                .Rsyn = sba.Descendants("Rsyn").Value
                .Rsys = sba.Descendants("Rsys").Value
            End With
            With subareasInfo._line7(0)
                .Bcof = sba.Descendants("Bcof").Value
                .Bffl = sba.Descendants("Bffl").Value
                .Pcof = sba.Descendants("Pcof").Value
                .Rsbd = sba.Descendants("Rsbd").Value
                .Rsdp = sba.Descendants("Rsdp").Value
                .Rshc = sba.Descendants("Rshc").Value
            End With
            With subareasInfo._line8(0)
                .Idf1 = sba.Descendants("Idf1").Value
                .Idf2 = sba.Descendants("Idf2").Value
                .Idf3 = sba.Descendants("Idf3").Value
                .Idf4 = sba.Descendants("Idf4").Value
                .Idf5 = sba.Descendants("Idf5").Value
                .Idr = sba.Descendants("Idr").Value
                .Ifa = sba.Descendants("Ifa").Value
                .Ifd = sba.Descendants("Ifd").Value
                .Iri = sba.Descendants("Iri").Value
                .Lm = sba.Descendants("Lm").Value
                .Nirr = sba.Descendants("Nirr").Value
            End With
            With subareasInfo._line9(0)
                .Armn = sba.Descendants("Armn").Value
                .Armx = sba.Descendants("Armx").Value
                .Bft = sba.Descendants("Bft").Value
                .Bir = sba.Descendants("Bir").Value
                .Drt = sba.Descendants("Drt").Value
                .Efi = sba.Descendants("Efi").Value
                .Fdsf = sba.Descendants("Fdsf").Value
                .Fmx = sba.Descendants("Fmx").Value
                .Fnp4 = sba.Descendants("Fnp4").Value
                .Vimx = sba.Descendants("Vimx").Value
            End With
            With subareasInfo._line10(0)
                .Coww = sba.Descendants("Coww").Value
                .Dalg = sba.Descendants("Dalg").Value
                .Ddlg = sba.Descendants("Ddlg").Value
                .Firg = sba.Descendants("Firg").Value
                .Fnp2 = sba.Descendants("Fnp2").Value
                .Fnp5 = sba.Descendants("Fnp5").Value
                .Pec = sba.Descendants("Pec").Value
                .Sflg = sba.Descendants("Sflg").Value
                .Solq = sba.Descendants("Solq").Value
                .Vlgn = sba.Descendants("Vlgn").Value
            End With
            With subareasInfo._line11(0)
                .Ny1 = sba.Descendants("Ny1").Value
                .Ny2 = sba.Descendants("Ny2").Value
                .Ny3 = sba.Descendants("Ny3").Value
                .Ny4 = sba.Descendants("Ny4").Value
            End With
            With subareasInfo._line12(0)
                .Xtp1 = sba.Descendants("Xtp1").Value
                .Xtp2 = sba.Descendants("Xtp2").Value
                .Xtp3 = sba.Descendants("Xtp3").Value
                .Xtp4 = sba.Descendants("Xtp4").Value
            End With
            For Each operation In sba.Descendants("Operations")
                loadOperationInfo(operation, subareasInfo._operationsInfo, 0)
            Next
        Catch ex As Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Private Sub LoadFEMEquipmentInfo(equips As System.Xml.Linq.XElement, ByRef _equipmentTemp As List(Of EquipmentData))
        'load start info from a saved xml file.
        Try
            Dim equip As New EquipmentData
            equip.Selected = equips.Descendants("selected").Value
            equip.Name = equips.Descendants("name").Value
            equip.LeaseRate = equips.Descendants("leaseRate").Value
            equip.NewPrice = equips.Descendants("newPrice").Value
            equip.NewHours = equips.Descendants("newHours").Value
            equip.CurrentPrice = equips.Descendants("currentPrice").Value
            equip.HoursRemaining = equips.Descendants("hoursRemaining").Value
            equip.Width = equips.Descendants("width").Value
            equip.Speed = equips.Descendants("speed").Value
            equip.FieldEfficiency = equips.Descendants("fieldEfficiency").Value
            equip.HorsePower = equips.Descendants("horsePower").Value
            equip.Rf1 = equips.Descendants("RF1").Value
            equip.Rf2 = equips.Descendants("RF2").Value
            equip.IrLoan = equips.Descendants("irloan").Value
            equip.IrEquity = equips.Descendants("irequity").Value
            equip.PDebt = equips.Descendants("pDebt").Value
            equip.Year = equips.Descendants("year").Value
            equip.Rv1 = equips.Descendants("RV1").Value
            equip.Rv2 = equips.Descendants("RV2").Value
            _equipmentTemp.Add(equip)
        Catch e1 As Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & e1.Message)
        End Try
    End Sub

    Private Sub LoadFEMFeedInfo(feeds As System.Xml.Linq.XElement, ByRef _feedTemp As List(Of FeedData))
        'load start info from a saved xml file.
        Try
            Dim feed As New FeedData
            feed.Selected = feeds.Descendants("selected").Value
            feed.Name = feeds.Descendants("name").Value
            feed.SellingPrice = feeds.Descendants("sellingPrice").Value
            feed.PurchasePrice = feeds.Descendants("purchasePrice").Value
            feed.Concentrate = feeds.Descendants("concentrate").Value
            feed.Forage = feeds.Descendants("forage").Value
            feed.Grain = feeds.Descendants("grain").Value
            feed.Hay = feeds.Descendants("hay").Value
            feed.Pasture = feeds.Descendants("pasture").Value
            feed.Silage = feeds.Descendants("silage").Value
            feed.Supplement = feeds.Descendants("supplement").Value
            _feedTemp.Add(feed)

        Catch e1 As Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & e1.Message)
        End Try
    End Sub
    Private Sub LoadFEMStructureInfo(structs As System.Xml.Linq.XElement, ByRef _structureTemp As List(Of StructureData))
        'load start info from a saved xml file.
        Try
            Dim struct As New StructureData
            struct.Selected = structs.Descendants("selected").Value
            struct.Name = structs.Descendants("name").Value
            struct.LeaseRate = structs.Descendants("leaseRate").Value
            struct.NewPrice = structs.Descendants("newPrice").Value
            struct.NewLife = structs.Descendants("newLife").Value
            struct.CurrentPrice = structs.Descendants("currentPrice").Value
            struct.LifeRemaining = structs.Descendants("lifeRemaining").Value
            struct.MaintenanceCoefficient = structs.Descendants("maintenanceCoefficient").Value
            struct.LoanInterestRate = structs.Descendants("loanInterestRate").Value
            struct.LengthLoan = structs.Descendants("lenghtofLoan").Value
            struct.InterestRateEquity = structs.Descendants("interestRateonEquity").Value
            struct.ProportionDebt = structs.Descendants("proportionofDebt").Value
            struct.Year = structs.Descendants("year").Value
            _structureTemp.Add(struct)

        Catch e1 As Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & e1.Message)
        End Try
    End Sub
    Private Sub LoadFEMOtherInfo(others As System.Xml.Linq.XElement, ByRef _otherTemp As List(Of OtherData))
        'load start info from a saved xml file.
        Try
            Dim other As New OtherData
            other.Selected = others.Descendants("selected").Value
            other.Name = others.Descendants("name").Value
            other.Values = others.Descendants("values").Value
            _otherTemp.Add(other)

        Catch e1 As Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & e1.Message)
        End Try
    End Sub

    Private Sub loadSubprojectNameInfo(item As System.Xml.Linq.XElement, ByRef _subprojectName As List(Of SubprojectNameData))
        'load subproject names values
        Try
            Dim subprojectName As New SubprojectNameData
            subprojectName.Name = item.Descendants("Name").Value
            subprojectName.TotalArea = item.Descendants("TotalArea").Value

            For Each subproject In item.Descendants("Subproject")
                'load subproject information
                loadSubprojectInfo(subproject, subprojectName)
            Next
            For Each result In item.Descendants("Results")
                loadResultInfo(result, subprojectName._results)
            Next
            _subprojectName.Add(subprojectName)

        Catch ex As Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Private Sub loadSubprojectInfo(subproject As System.Xml.Linq.XElement, ByRef subprojectName As SubprojectNameData)
        'load subproject information
        Try
            Dim subprojectScenario As New SubprojectNameData.SubProjectData
            subprojectScenario.Field = subproject.Descendants("Field").Value
            subprojectScenario.Scenario = subproject.Descendants("Scenario").Value
            subprojectScenario.Area = subproject.Descendants("Area").Value
            subprojectName._subproject.Add(subprojectScenario)
        Catch ex As Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Private Sub loadSubprojectResult(result As System.Xml.Linq.XElement, ByRef subprojectName As SubprojectNameData)
        'load subproject results
        Dim i As UShort = 0
        Try
            Dim _results As New ScenariosData.APEXResults
            _results.area = result.Descendants("area").Value
            _results.lastSimulation = result.Descendants("lastSimulation").Value
            'load crops results
            With subprojectName._results.SoilResults.Crops
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropName")
                    ReDim Preserve .cropName(i)
                    .cropName(i) = crop.Value
                    i += 1
                Next
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropRecord")
                    ReDim Preserve .cropRecords(i)
                    .cropRecords(i) = crop.Value
                    i += 1
                Next
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropYield")
                    ReDim Preserve .cropYield(i)
                    .cropYield(i) = crop.Value
                    i += 1
                Next
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropYieldCI")
                    ReDim Preserve .cropYieldCI(i)
                    .cropYieldCI(i) = crop.Value
                    i += 1
                Next
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropYieldSD")
                    ReDim Preserve .cropYieldSD(i)
                    .cropYieldSD(i) = crop.Value
                    i += 1
                Next
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropns")
                    ReDim Preserve .ns(i)
                    .ns(i) = crop.Value
                    i += 1
                Next
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropps")
                    ReDim Preserve .ps(i)
                    .ps(i) = crop.Value
                    i += 1
                Next
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropts")
                    ReDim Preserve .ts(i)
                    .ts(i) = crop.Value
                    i += 1
                Next
                i = 0
                For Each crop In result.Descendants("Crops").Descendants("cropws")
                    ReDim Preserve .ws(i)
                    .ws(i) = crop.Value
                    i += 1
                Next
            End With

            i = 0
            _results.RedimAnnualFlow(result.Descendants("annualFlow").Descendants("flow").Count - 1)
            For Each value In result.Descendants("annualFlow").Descendants("flow")
                _results.annualFlow(i) = value.Value
                i += 1
            Next
            i = 0
            _results.RedimAnnualNO3(result.Descendants("annualNO3").Descendants("NO3").Count - 1)
            For Each value In result.Descendants("annualNO3").Descendants("NO3")
                _results.RedimAnnualNO3(i)
                _results.annualNO3(i) = value.Value
                i += 1
            Next
            i = 0
            _results.RedimAnnualOrgN(result.Descendants("annualOrgN").Descendants("OrgN").Count - 1)
            For Each value In result.Descendants("annualOrgN").Descendants("OrgN")
                _results.RedimAnnualOrgN(i)
                _results.annualOrgN(i) = value.Value
                i += 1
            Next
            i = 0
            _results.RedimAnnualN2o(result.Descendants("annualN2O").Descendants("N2O").Count - 1)
            For Each value In result.Descendants("annualN2O").Descendants("N2O")
                _results.RedimAnnualN2o(i)
                _results.annualN2o(i) = value.Value
                i += 1
            Next
            i = 0
            _results.RedimAnnualOrgP(result.Descendants("annualOrgP").Descendants("OrgP").Count - 1)
            For Each value In result.Descendants("annualOrgP").Descendants("OrgP")
                _results.RedimAnnualOrgP(i)
                _results.annualOrgP(i) = value.Value
                i += 1
            Next
            i = 0
            _results.RedimAnnualPO4(result.Descendants("annualPO4").Descendants("PO4").Count - 1)
            For Each value In result.Descendants("annualPO4").Descendants("PO4")
                _results.RedimAnnualPO4(i)
                _results.annualPO4(i) = value.Value
                i += 1
            Next
            i = 0
            _results.RedimAnnualSediment(result.Descendants("annualSediment").Descendants("Sediment").Count - 1)
            For Each value In result.Descendants("annualSediment").Descendants("Sediment")
                _results.RedimAnnualSediment(i)
                _results.annualSediment(i) = value.Value
                i += 1
            Next
            i = 0
            _results.RedimAnnualPrecipitation(result.Descendants("annualPrecipitation").Descendants("precipitation").Count - 1)
            For Each value In result.Descendants("annualprecipitation").Descendants("precipitation")
                _results.annualPrecipitation(i) = value.Value
                i += 1
            Next

            Dim l As UShort = 0
            i = 0
            _results.RedimAnnualCropYield(result.Descendants("annualCropYield").Descendants("crop").Count - 1)
            For Each value In result.Descendants("annualCropYield").Descendants("crop")
                ReDim _results.annualCropYield(i).CropName(result.Descendants("annualCropYield").Descendants("crop").Count - 1)
                ReDim _results.annualCropYield(i).cropYield(result.Descendants("annualCropYield").Descendants("crop").Count - 1)
                l = 0
                For Each crop In value.Descendants("cropName")
                    _results.annualCropYield(i).CropName(l) = crop.Value
                Next
                l = 0
                For Each crop In value.Descendants("cropYield")
                    _results.annualCropYield(i).cropYield(l) = crop.Value
                Next
                i += 1
            Next
            i = 0
            For Each soilResult In result.Descendants("SoilResult")
                _results.SoilResults.co2 = soilResult.Descendants("co2").Value
                'todo
                'Dim j As Short = 0
                'For Each count In soilResult.Descendants("CountCrops").Descendants
                '    _results.SoilResults.RedimCountCrops(j)
                '    _results.SoilResults.CountCrops(j) = count.Value
                '    j += 1
                'Next

                'j = 0
                'For Each yield In soilResult.Descendants("Yields").Descendants
                '    _results.SoilResults.RedimYields(j)
                '    _results.SoilResults.Yields(j) = yield.Value
                '    j += 1
                'Next

                'j = 0
                'For Each yield In soilResult.Descendants("YieldsCI").Descendants
                '    _results.SoilResults.RedimYieldsCI(j)
                '    _results.SoilResults.YieldsCI(j) = yield.Value
                '    j += 1
                'Next

                'j = 0
                'For Each yield In soilResult.Descendants("YieldsSD").Descendants
                '    _results.SoilResults.RedimYieldsSD(j)
                '    _results.SoilResults.YieldsSD(j) = yield.Value
                '    j += 1
                'Next
                _results.SoilResults.deepPerFlow = result.Descendants("deepPerFlow").Value
                _results.SoilResults.deepPerFlowCI = result.Descendants("deepPerFlowCI").Value
                _results.SoilResults.runoff = result.Descendants("runoff").Value
                _results.SoilResults.runoffCI = result.Descendants("runoffCI").Value
                _results.SoilResults.runoffSD = result.Descendants("runoffSD").Value
                _results.SoilResults.subsurfaceFlow = result.Descendants("subsurfaceflow").Value
                _results.SoilResults.subsurfaceFlowCI = result.Descendants("subsurfaceflowCI").Value
                _results.SoilResults.subsurfaceFlowSD = result.Descendants("subsurfaceflowSD").Value
                _results.SoilResults.tileDrainFlow = result.Descendants("tileDrainFlow").Value
                _results.SoilResults.tileDrainFlowCI = result.Descendants("tileDrainFlowCI").Value
                _results.SoilResults.tileDrainFlowSD = result.Descendants("tileDrainFlowSD").Value
                _results.SoilResults.irrigation = result.Descendants("irrigation").Value
                _results.SoilResults.irrigationCI = result.Descendants("irrigationCI").Value
                _results.SoilResults.LeachedN = result.Descendants("LeachedN").Value
                _results.SoilResults.LeachedP = result.Descendants("LeachedP").Value
                _results.SoilResults.n2o = result.Descendants("n2o").Value
                _results.SoilResults.RunoffN = result.Descendants("runoffN").Value
                _results.SoilResults.runoffNCI = result.Descendants("runoffNCI").Value
                _results.SoilResults.runoffNSD = result.Descendants("runoffNDS").Value
                _results.SoilResults.SubsurfaceN = result.Descendants("SubsurfaceN").Value
                _results.SoilResults.subsurfaceNCI = result.Descendants("SubsurfaceNCI").Value
                _results.SoilResults.subsurfaceNSD = result.Descendants("SubsurfaceNDS").Value
                _results.SoilResults.OrgN = result.Descendants("OrgN").Value
                _results.SoilResults.OrgNCI = result.Descendants("OrgNCI").Value
                _results.SoilResults.OrgNSD = result.Descendants("OrgNSD").Value
                _results.SoilResults.OrgP = result.Descendants("OrgP").Value
                _results.SoilResults.OrgPCI = result.Descendants("OrgPCI").Value
                _results.SoilResults.OrgPSD = result.Descendants("OrgPSD").Value
                _results.SoilResults.PO4 = result.Descendants("PO4").Value
                _results.SoilResults.PO4CI = result.Descendants("PO4CI").Value
                _results.SoilResults.PO4SD = result.Descendants("PO4SD").Value
                _results.SoilResults.Sediment = result.Descendants("Sediment").Value
                _results.SoilResults.SedimentCI = result.Descendants("SedimentCI").Value
                _results.SoilResults.SedimentSD = result.Descendants("SedimentSD").Value
            Next
            subprojectName._results = _results
        Catch ex As Exception
            ''showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Private Function AddStartInfo(ByRef output As StringBuilder, ByRef _StartInfo As StartInfo) As String
        Try
            '***** Begining of start info ****
            output.Append("<StartInfo>" & vbCr)
            output.Append("<Status>")
            output.Append(_StartInfo.Status)
            output.Append("</Status>")
            output.Append("<dates>")
            output.Append(_StartInfo.dates)
            output.Append("</dates>")
            output.Append("<Description>")
            output.Append(_StartInfo.description)
            output.Append("</Description>")
            output.Append("<projectName>")
            output.Append(_StartInfo.projectName)
            output.Append("</projectName>")
            output.Append("<StateAbrev>")
            output.Append(_StartInfo.StateAbrev)
            output.Append("</StateAbrev>")
            output.Append("<StateName>")
            output.Append(_StartInfo.StateName)
            output.Append("</StateName>")
            output.Append("<CountyCode>")
            output.Append(_StartInfo.countyCode)
            output.Append("</CountyCode>")
            output.Append("<CountyName>")
            output.Append(_StartInfo.countyName)
            output.Append("</CountyName>")
            output.Append("<StationCode>")
            output.Append(_StartInfo.stationCode)
            output.Append("</StationCode>")
            output.Append("<StationName>")
            output.Append(_StartInfo.stationName)
            output.Append("</StationName>")
            output.Append("<StationWay>")
            output.Append(_StartInfo.stationWay)
            output.Append("</StationWay>")
            output.Append("<StationYears>")
            output.Append(_StartInfo.stationYears)
            output.Append("</StationYears>")
            output.Append("<StationInitialYear>")
            output.Append(_StartInfo.stationInitialYear)
            output.Append("</StationInitialYear>")
            output.Append("<StationFinalYear>")
            output.Append(_StartInfo.stationFinalYear)
            output.Append("</StationFinalYear>")
            output.Append("<WeatherInitialYear>")
            output.Append(_StartInfo.weatherInitialYear)
            output.Append("</WeatherInitialYear>")
            output.Append("<WeatherFinalYear>")
            output.Append(_StartInfo.weatherFinalYear)
            output.Append("</WeatherFinalYear>")
            output.Append("<YearsRotation>")
            output.Append(_StartInfo.yearsRotation)
            output.Append("</YearsRotation>")
            output.Append("<CurrentWeatherPath>")
            output.Append(_StartInfo.currentWeatherPath)
            output.Append("</CurrentWeatherPath>")
            output.Append("<WeatherLon>")
            output.Append(_StartInfo.weatherLon)
            output.Append("</WeatherLon>")
            output.Append("<WeatherLat>")
            output.Append(_StartInfo.weatherLat)
            output.Append("</WeatherLat>")
            output.Append("<tMax>")
            If Not _StartInfo.tMax Is Nothing Then
                For i = 0 To 11
                    output.Append(_StartInfo.tMax(i) & "|")
                Next
            End If
            output.Append("</tMax>")
            output.Append("<tMin>")
            If Not _StartInfo.tMax Is Nothing Then
                For i = 0 To 11
                    output.Append(_StartInfo.tMin(i) & "|")
                Next
            End If
            output.Append("</tMin>")
            output.Append("<wp1aLat>")
            output.Append(_StartInfo.wp1aLat)
            output.Append("</wp1aLat>")
            output.Append("<windName>")
            output.Append(_StartInfo.WindName)
            output.Append("</windName>")
            output.Append("<wp1Name>")
            output.Append(_StartInfo.Wp1Name)
            output.Append("</wp1Name>")
            output.Append("<windCode>")
            output.Append(_StartInfo.WindCode)
            output.Append("</windCode>")
            output.Append("<wp1Code>")
            output.Append(_StartInfo.Wp1Code)
            output.Append("</wp1Code>")
            output.Append("<Versions>")
            output.Append(_StartInfo.Versions)
            output.Append("</Versions>")
            output.Append("</StartInfo>")
            '***** end of start info ****
            Return "OK"
        Catch ex As System.Exception
            Return msgDoc.Descendants("Errors").Value & ex.Message
        End Try
    End Function

    Private Sub AddFEMFeed(_feed As List(Of FeedData))
        Try
            For Each feed In _feed
                output.Append("<FeedInfo>" & vbCr)
                output.Append("<selected>")
                output.Append(feed.Selected)
                output.Append("<\selected>")
                output.Append("<name>")
                output.Append(feed.Name)
                output.Append("</name>")
                output.Append("<sellingPrice>")
                output.Append(feed.SellingPrice)
                output.Append("</sellingPrice>")
                output.Append("<purchasePrice>")
                output.Append(feed.PurchasePrice)
                output.Append("</purchasePrice>")
                output.Append("<concentrate>")
                output.Append(feed.Concentrate)
                output.Append("</concentrate>")
                output.Append("<forage>")
                output.Append(feed.Forage)
                output.Append("</forage>")
                output.Append("<grain>")
                output.Append(feed.Grain)
                output.Append("</grain>")
                output.Append("<hay>")
                output.Append(feed.Hay)
                output.Append("</hay>")
                output.Append("<pasture>")
                output.Append(feed.Pasture)
                output.Append("</pasture>")
                output.Append("<silage>")
                output.Append(feed.Silage)
                output.Append("</silage>")
                output.Append("<supplement>")
                output.Append(feed.Supplement)
                output.Append("</supplement>")
                output.Append("</FeedInfo>" & vbCr)
            Next
        Catch ex As Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value)

        End Try
    End Sub

    Private Sub AddFEMEquipment(_equipment As List(Of EquipmentData))
        Try
            For Each equip In _equipment
                output.Append("<EquipmentInfo>" & vbCr)
                output.Append("<selected>")
                output.Append(equip.Selected)
                output.Append("<\selected>")
                output.Append("<name>")
                output.Append(equip.Name)
                output.Append("</name>")
                output.Append("<leaseRate>")
                output.Append(equip.LeaseRate)
                output.Append("</leaseRate>")
                output.Append("<newPrice>")
                output.Append(equip.NewPrice)
                output.Append("</newPrice>")
                output.Append("<newHours>")
                output.Append(equip.NewHours)
                output.Append("</newHours>")
                output.Append("<currentPrice>")
                output.Append(equip.CurrentPrice)
                output.Append("</currentPrice>")
                output.Append("<hoursRemaining>")
                output.Append(equip.HoursRemaining)
                output.Append("</hoursRemaining>")
                output.Append("<width>")
                output.Append(equip.Width)
                output.Append("</width>")
                output.Append("<speed>")
                output.Append(equip.Speed)
                output.Append("</speed>")
                output.Append("<fieldEfficiency>")
                output.Append(equip.FieldEfficiency)
                output.Append("</fieldEfficiency>")
                output.Append("<horsePower>")
                output.Append(equip.HorsePower)
                output.Append("</horsePower>")
                output.Append("<RF1>")
                output.Append(equip.Rf1)
                output.Append("</RF1>")
                output.Append("<RF2>")
                output.Append(equip.Rf2)
                output.Append("</RF2>")
                output.Append("<irloan>")
                output.Append(equip.IrLoan)
                output.Append("</irloan>")
                output.Append("<lloan>")
                output.Append(equip.LLoan)
                output.Append("</lloan>")
                output.Append("<irequity>")
                output.Append(equip.IrEquity)
                output.Append("</irequity>")
                output.Append("<pDebt>")
                output.Append(equip.PDebt)
                output.Append("</pDebt>")
                output.Append("<year>")
                output.Append(equip.Year)
                output.Append("</year>")
                output.Append("<RV1>")
                output.Append(equip.Rv1)
                output.Append("</RV1>")
                output.Append("<RV2>")
                output.Append(equip.Rv2)
                output.Append("</RV2>")
                output.Append("</EquipmentInfo>" & vbCr)
            Next
        Catch ex As Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value)
        End Try
    End Sub

    Private Sub AddFEMStructure(_structure As List(Of StructureData))
        Try
            For Each struct In _structure
                output.Append("<StructureInfo>" & vbCr)
                output.Append("<selected>")
                output.Append(struct.Selected)
                output.Append("<\selected>")
                output.Append("<name>")
                output.Append(struct.Name)
                output.Append("</name>")
                output.Append("<leaseRate>")
                output.Append(struct.LeaseRate)
                output.Append("</leaseRate>")
                output.Append("<newPrice>")
                output.Append(struct.NewPrice)
                output.Append("</newPrice>")
                output.Append("<newLife>")
                output.Append(struct.NewLife)
                output.Append("</newLife>")
                output.Append("<currentPrice>")
                output.Append(struct.CurrentPrice)
                output.Append("</currentPrice>")
                output.Append("<lifeRemaining>")
                output.Append(struct.LifeRemaining)
                output.Append("</lifeRemaining>")
                output.Append("<maintenanceCoefficient>")
                output.Append(struct.MaintenanceCoefficient)
                output.Append("</maintenanceCoefficient>")
                output.Append("<loanInterestRate>")
                output.Append(struct.LoanInterestRate)
                output.Append("</loanInterestRate>")
                output.Append("<lenghtofLoan>")
                output.Append(struct.LengthLoan)
                output.Append("</lenghtofLoan>")
                output.Append("<interestRateonEquity>")
                output.Append(struct.InterestRateEquity)
                output.Append("</interestRateonEquity>")
                output.Append("<proportionofDebt>")
                output.Append(struct.ProportionDebt)
                output.Append("</proportionofDebt>")
                output.Append("<year>")
                output.Append(struct.Year)
                output.Append("</year>")
                output.Append("</StructureInfo>" & vbCr)

            Next
        Catch ex As Exception
            ''showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value)
        End Try
    End Sub

    Private Sub AddFEMInput(_other As List(Of OtherData))
        Try
            For Each other In _other
                output.Append("<OtherInputInfo>" & vbCr)
                output.Append("<selected>")
                output.Append(other.Selected)
                output.Append("<\selected>")
                output.Append("<name>")
                output.Append(other.Name)
                output.Append("</name>")
                output.Append("<values>")
                output.Append(other.Values)
                output.Append("</values>")
                output.Append("</OtherInputInfo>" & vbCr)
            Next
        Catch ex As Exception
            ''showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value)
        End Try
    End Sub

    Private Function AddFarmInfo(ByRef _StartInfo) As String
        Try
            '***** farm information ****
            output.Append("<FarmInfo>")
            output.Append("<Coordinates>")
            output.Append(_StartInfo.farmCoordinates)
            output.Append("</Coordinates>")
            output.Append("<Name>")
            output.Append(_StartInfo.farmName)
            output.Append("</Name>")
            output.Append("<centroid>")
            output.Append(_StartInfo.farmCentrois)
            output.Append("</centroid>")
            output.Append("</FarmInfo>")
            Return "OK"
            '*****end of farm information ****
        Catch ex As System.Exception
            Return ex.Message
        End Try
    End Function

    Private Sub AddControlInfo(ByRef _controlValues As List(Of ParmsData))
        Try
            '*****control values *****
            If Not _controlValues Is Nothing Then
                For Each value In _controlValues
                    output.Append("<ControlValues>" & vbCr)
                    output.Append("<Code>")
                    output.Append(value.Code.Trim)
                    output.Append("</Code>")
                    output.Append("<Name>")
                    output.Append(value.Name)
                    output.Append("</Name>")
                    output.Append("<Value>")
                    output.Append(value.Value1)
                    output.Append("</Value>")
                    output.Append("<Range1>")
                    output.Append(value.Range1)
                    output.Append("</Range1>")
                    output.Append("<Range2>")
                    output.Append(value.Range2)
                    output.Append("</Range2>")
                    output.Append("<State>")
                    output.Append(value.State1)
                    output.Append("</State>")
                    output.Append("</ControlValues>" & vbCr)
                Next
            End If
            '*****end control values *****
        Catch ex As System.Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value)
        End Try
    End Sub

    Private Sub AddParmInfo(ByRef _parmValues As List(Of ParmsData))
        Try
            '*****parm values *****
            If Not _parmValues Is Nothing Then
                For Each value In _parmValues
                    output.Append("<ParmValues>" & vbCr)
                    output.Append("<Code>")
                    output.Append(value.Code)
                    output.Append("</Code>")
                    output.Append("<Name>")
                    output.Append(value.Name)
                    output.Append("</Name>")
                    output.Append("<Value>")
                    output.Append(value.Value1)
                    output.Append("</Value>")
                    output.Append("<Range1>")
                    output.Append(value.Range1)
                    output.Append("</Range1>")
                    output.Append("<Range2>")
                    output.Append(value.Range2)
                    output.Append("</Range2>")
                    output.Append("<State>")
                    output.Append(value.State1)
                    output.Append("</State>")
                    output.Append("</ParmValues>" & vbCr)
                Next
            End If
            '*****end control values *****
        Catch ex As System.Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value)
        End Try
    End Sub

    Private Sub AddSubprojectInfo(ByRef _subprojectName As List(Of SubprojectNameData))
        Try
            '***** subproject information
            If Not _subprojectName Is Nothing Then
                For Each subproject In _subprojectName
                    output.Append("<SubprojectName>" & vbCr)
                    output.Append("<Name>")
                    output.Append(subproject.Name)
                    output.Append("</Name>")
                    output.Append("<TotalArea>")
                    output.Append(subproject.TotalArea)
                    output.Append("</TotalArea>")
                    '***** Results information ****
                    AddScenarioResultsInfo(subproject._results, output)
                    'output.Append("<Results>" & vbCr)
                    'output.Append("<area>")
                    'output.Append(subproject._results.area)
                    'output.Append("</area>")
                    'output.Append("<lastSimulation>")
                    'output.Append(subproject._results.lastSimulation)
                    'output.Append("</lastSimulation>")
                    ''todo
                    ''If Not subproject._results.SoilResults.Crops Is Nothing Then
                    ''    output.Append("<Crops>")
                    ''    For Each crop In subproject._results.SoilResults.Crops
                    ''        output.Append("<crop>")
                    ''        output.Append(crop)
                    ''        output.Append("</crop>")
                    ''    Next
                    ''    output.Append("</Crops>")
                    ''End If
                    'output.Append("<FIFertilizer>")
                    'output.Append(subproject._results.FIFertilizer)
                    'output.Append("</FIFertilizer>")
                    'output.Append("<i>")
                    'output.Append(subproject._results.i)
                    'output.Append("</i>")
                    ''If Not subproject._results.SoilResults Is Nothing Then
                    ''For Each soil In subproject._results.SoilResults
                    ''***** SoilsResults information ****
                    'output.Append("<SoilResult>")
                    'output.Append("<co2>")
                    'output.Append(subproject._results.SoilResults.co2)
                    'output.Append("</co2>")
                    'output.Append("<deepPerFlow>")
                    'output.Append(subproject._results.SoilResults.deepPerFlow)
                    'output.Append("</deepPerFlow>")
                    'output.Append("<deepPerFlowCI>")
                    'output.Append(subproject._results.SoilResults.deepPerFlowCI)
                    'output.Append("</deepPerFlowCI>")
                    'output.Append("<runoff>")
                    'output.Append(subproject._results.SoilResults.runoff)
                    'output.Append("</runoff>")
                    'output.Append("<runoffCI>")
                    'output.Append(subproject._results.SoilResults.runoffCI)
                    'output.Append("</runoffCI>")
                    'output.Append("<runoffSD>")
                    'output.Append(subproject._results.SoilResults.runoffSD)
                    'output.Append("</runoffSD>")
                    'output.Append("<subsurfaceFlow>")
                    'output.Append(subproject._results.SoilResults.subsurfaceFlow)
                    'output.Append("</subsurfaceFlow>")
                    'output.Append("<subsurfaceFlowCI>")
                    'output.Append(subproject._results.SoilResults.subsurfaceFlowCI)
                    'output.Append("</subsurfaceFlowCI>")
                    'output.Append("<subsurfaceFlowSD>")
                    'output.Append(subproject._results.SoilResults.subsurfaceFlowSD)
                    'output.Append("</subsurfaceFlowSD>")
                    'output.Append("<tileDrainFlow>")
                    'output.Append(subproject._results.SoilResults.tileDrainFlow)
                    'output.Append("</tileDrainFlow>")
                    'output.Append("<tileDrainFlowCI>")
                    'output.Append(subproject._results.SoilResults.tileDrainFlowCI)
                    'output.Append("</tileDrainFlowCI>")
                    'output.Append("<tileDrainFlowSD>")
                    'output.Append(subproject._results.SoilResults.tileDrainFlowSD)
                    'output.Append("</tileDrainFlowSD>")
                    'output.Append("<irrigation>")
                    'output.Append(subproject._results.SoilResults.irrigation)
                    'output.Append("</irrigation>")
                    'output.Append("<irrigationCI>")
                    'output.Append(subproject._results.SoilResults.irrigationCI)
                    'output.Append("</irrigationCI>")
                    'output.Append("<LeachedN>")
                    'output.Append(subproject._results.SoilResults.LeachedN)
                    'output.Append("</LeachedN>")
                    'output.Append("<LeachedP>")
                    'output.Append(subproject._results.SoilResults.LeachedP)
                    'output.Append("</LeachedP>")
                    'output.Append("<n2o>")
                    'output.Append(subproject._results.SoilResults.n2o)
                    'output.Append("</n2o>")
                    'output.Append("<runoffN>")
                    'output.Append(subproject._results.SoilResults.RunoffN)
                    'output.Append("</runoffN>")
                    'output.Append("<runoffNCI>")
                    'output.Append(subproject._results.SoilResults.runoffNCI)
                    'output.Append("</runoffNCI>")
                    'output.Append("<runoffNSD>")
                    'output.Append(subproject._results.SoilResults.runoffNSD)
                    'output.Append("</runoffNSD>")
                    'output.Append("<subsurfaceN>")
                    'output.Append(subproject._results.SoilResults.SubsurfaceN)
                    'output.Append("</subsurfaceN>")
                    'output.Append("<subsurfaceNCI>")
                    'output.Append(subproject._results.SoilResults.subsurfaceNCI)
                    'output.Append("</subsurfaceNCI>")
                    'output.Append("<subsurfaceNSD>")
                    'output.Append(subproject._results.SoilResults.subsurfaceNSD)
                    'output.Append("</subsurfaceNSD>")
                    'output.Append("<OrgN>")
                    'output.Append(subproject._results.SoilResults.OrgN)
                    'output.Append("</OrgN>")
                    'output.Append("<OrgNCI>")
                    'output.Append(subproject._results.SoilResults.OrgNCI)
                    'output.Append("</OrgNCI>")
                    'output.Append("<OrgNSD>")
                    'output.Append(subproject._results.SoilResults.OrgNSD)
                    'output.Append("</OrgNSD>")
                    'output.Append("<OrgP>")
                    'output.Append(subproject._results.SoilResults.OrgP)
                    'output.Append("</OrgP>")
                    'output.Append("<OrgPCI>")
                    'output.Append(subproject._results.SoilResults.OrgPCI)
                    'output.Append("</OrgPCI>")
                    'output.Append("<OrgPSD>")
                    'output.Append(subproject._results.SoilResults.OrgPSD)
                    'output.Append("</OrgPSD>")
                    'output.Append("<PO4>")
                    'output.Append(subproject._results.SoilResults.PO4)
                    'output.Append("</PO4>")
                    'output.Append("<PO4CI>")
                    'output.Append(subproject._results.SoilResults.PO4CI)
                    'output.Append("</PO4CI>")
                    'output.Append("<PO4SD>")
                    'output.Append(subproject._results.SoilResults.PO4SD)
                    'output.Append("</PO4SD>")
                    'output.Append("<Sediment>")
                    'output.Append(subproject._results.SoilResults.Sediment)
                    'output.Append("</Sediment>")
                    'output.Append("<SedimentCI>")
                    'output.Append(subproject._results.SoilResults.SedimentCI)
                    'output.Append("</SedimentCI>")
                    'output.Append("<SedimentSD>")
                    'output.Append(subproject._results.SoilResults.SedimentSD)
                    'output.Append("</SedimentSD>")
                    'output.Append("<tileDrainFlow>")
                    'output.Append(subproject._results.SoilResults.tileDrainFlow)
                    'output.Append("</tileDrainFlow>")
                    'output.Append("<tileDrainN>")
                    'output.Append(subproject._results.SoilResults.tileDrainN)
                    'output.Append("</tileDrainN>")
                    'output.Append("<tileDrainP>")
                    'output.Append(subproject._results.SoilResults.tileDrainP)
                    'output.Append("</tileDrainP>")
                    'output.Append("<volatizationN>")
                    'output.Append(subproject._results.SoilResults.volatizationN)
                    'output.Append("</volatizationN>")
                    ''add crops results
                    'output.Append("<Crops>")
                    'If Not subproject._results.SoilResults.Crops.cropName Is Nothing Then
                    '    For Each crop In subproject._results.SoilResults.Crops.cropName
                    '        output.Append("<cropName>")
                    '        output.Append(crop)
                    '        output.Append("</cropName>")
                    '    Next
                    'End If
                    'If Not subproject._results.SoilResults.Crops.cropRecords Is Nothing Then
                    '    For Each crop In subproject._results.SoilResults.Crops.cropRecords
                    '        output.Append("<cropRecord>")
                    '        output.Append(crop)
                    '        output.Append("</cropRecord>")
                    '    Next
                    'End If
                    'If Not subproject._results.SoilResults.Crops.cropYield Is Nothing Then
                    '    For Each crop In subproject._results.SoilResults.Crops.cropYield
                    '        output.Append("<cropYield>")
                    '        output.Append(crop)
                    '        output.Append("</cropYield>")
                    '    Next
                    'End If
                    'If Not subproject._results.SoilResults.Crops.cropYieldCI Is Nothing Then
                    '    For Each crop In subproject._results.SoilResults.Crops.cropYieldCI
                    '        output.Append("<cropYieldCI>")
                    '        output.Append(crop)
                    '        output.Append("</cropYieldCI>")
                    '    Next
                    'End If
                    'If Not subproject._results.SoilResults.Crops.cropYieldSD Is Nothing Then
                    '    For Each crop In subproject._results.SoilResults.Crops.cropYieldSD
                    '        output.Append("<cropYieldSD>")
                    '        output.Append(crop)
                    '        output.Append("</cropYieldSD>")
                    '    Next
                    'End If
                    'If Not subproject._results.SoilResults.Crops.ns Is Nothing Then
                    '    For Each crop In subproject._results.SoilResults.Crops.ns
                    '        output.Append("<cropns>")
                    '        output.Append(crop)
                    '        output.Append("</cropns>")
                    '    Next
                    'End If
                    'If Not subproject._results.SoilResults.Crops.ps Is Nothing Then
                    '    For Each crop In subproject._results.SoilResults.Crops.ps
                    '        output.Append("<cropps>")
                    '        output.Append(crop)
                    '        output.Append("</cropps>")
                    '    Next
                    'End If
                    'If Not subproject._results.SoilResults.Crops.ts Is Nothing Then
                    '    For Each crop In subproject._results.SoilResults.Crops.ts
                    '        output.Append("<cropts>")
                    '        output.Append(crop)
                    '        output.Append("</cropts>")
                    '    Next
                    'End If
                    'If Not subproject._results.SoilResults.Crops.ws Is Nothing Then
                    '    For Each crop In subproject._results.SoilResults.Crops.ws
                    '        output.Append("<cropws>")
                    '        output.Append(crop)
                    '        output.Append("</cropws>")
                    '    Next
                    'End If
                    'output.Append("</Crops>")
                    ''Add annual and monthly values for graphs.
                    'If Not subproject._results.annualFlow Is Nothing Then
                    '    output.Append("<annualFlow>")
                    '    For Each value In subproject._results.annualFlow
                    '        output.Append("<flow>")
                    '        output.Append(value)
                    '        output.Append("</flow>")
                    '    Next
                    '    output.Append("</annualFlow>")
                    'End If
                    'If Not subproject._results.annualNO3 Is Nothing Then
                    '    output.Append("<annualNO3>")
                    '    For Each value In subproject._results.annualNO3
                    '        output.Append("<NO3>")
                    '        output.Append(value)
                    '        output.Append("</NO3>")
                    '    Next
                    '    output.Append("</annualNO3>")
                    'End If
                    'If Not subproject._results.annualOrgN Is Nothing Then
                    '    output.Append("<annualOrgN>")
                    '    For Each value In subproject._results.annualOrgN
                    '        output.Append("<OrgN>")
                    '        output.Append(value)
                    '        output.Append("</OrgN>")
                    '    Next
                    '    output.Append("</annualOrgN>")
                    'End If
                    'If Not subproject._results.annualOrgP Is Nothing Then
                    '    output.Append("<annualOrgP>")
                    '    For Each value In subproject._results.annualOrgP
                    '        output.Append("<OrgP>")
                    '        output.Append(value)
                    '        output.Append("</OrgP>")
                    '    Next
                    '    output.Append("</annualOrgP>")
                    'End If
                    'If Not subproject._results.annualPO4 Is Nothing Then
                    '    output.Append("<annualPO4>")
                    '    For Each value In subproject._results.annualPO4
                    '        output.Append("<PO4>")
                    '        output.Append(value)
                    '        output.Append("</PO4>")
                    '    Next
                    '    output.Append("</annualPO4>")
                    'End If
                    'If Not subproject._results.annualSediment Is Nothing Then
                    '    output.Append("<annualSediment>")
                    '    For Each value In subproject._results.annualSediment
                    '        output.Append("<Sediment>")
                    '        output.Append(value)
                    '        output.Append("</Sediment>")
                    '    Next
                    '    output.Append("</annualSediment>")
                    'End If
                    'If Not subproject._results.annualPrecipitation Is Nothing Then
                    '    output.Append("<annualPrecipitation>")
                    '    For Each value In subproject._results.annualPrecipitation
                    '        output.Append("<precipitation>")
                    '        output.Append(value)
                    '        output.Append("</precipitation>")
                    '    Next
                    '    output.Append("</annualPrecipitation>")
                    'End If
                    'If Not subproject._results.annualCropYield Is Nothing Then
                    '    output.Append("<annualCropYield>")
                    '    For Each value In subproject._results.annualCropYield
                    '        output.Append("<crop>")
                    '        If Not value.cropName Is Nothing Then
                    '            For Each value1 In value.cropName
                    '                output.Append("<cropName>")
                    '                output.Append(value1)
                    '                output.Append("</cropName>")
                    '            Next
                    '            For Each value1 In value.cropYield
                    '                output.Append("<cropYield>")
                    '                output.Append(value1)
                    '                output.Append("</cropYield>")
                    '            Next
                    '        End If
                    '        output.Append("</crop>")
                    '    Next
                    '    output.Append("</annualCropYield>")
                    'End If
                    ''end annual results for graphs.
                    'output.Append("</SoilResult>")
                    ''***** End SoilsResult Information ****
                    ''Next
                    'output.Append("</Results>" & vbCr)
                    '***** End Results information ****
                    If Not subproject._subproject Is Nothing Then
                        For Each item In subproject._subproject
                            output.Append("<Subproject>" & vbCr)
                            output.Append("<Field>")
                            output.Append(item.Field)
                            output.Append("</Field>")
                            output.Append("<Scenario>")
                            output.Append(item.Scenario)
                            output.Append("</Scenario>")
                            output.Append("<Area>")
                            output.Append(item.Area)
                            output.Append("</Area>")
                            output.Append("</Subproject>" & vbCr)
                        Next
                    End If
                    output.Append("</SubprojectName>" & vbCr)
                Next
            End If
            '***** end subproject information
        Catch ex As System.Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value)
        End Try
    End Sub

    Private Sub AddFieldInfo(ByRef output As StringBuilder, ByRef _fieldsInfo1 As List(Of FieldsData))
        Try
            '***** field information ****
            If Not _fieldsInfo1 Is Nothing Then
                For Each Field In _fieldsInfo1
                    output.Append("<FieldInfo>" & vbCr)
                    output.Append("<Forestry>")
                    output.Append(Field.Forestry)
                    output.Append("</Forestry>")
                    output.Append("<Number>")
                    output.Append(Field.Number)
                    output.Append("</Number>")
                    output.Append("<Name>")
                    output.Append(Field.Name)
                    output.Append("</Name>")
                    output.Append("<Area>")
                    output.Append(Field.Area)
                    output.Append("</Area>")
                    output.Append("<Coordinates>")
                    output.Append(Field.Coordinates)
                    output.Append("</Coordinates>")
                    output.Append("<rchc>")
                    output.Append(Field.Rchc)
                    output.Append("</rchc>")
                    output.Append("<rchk>")
                    output.Append(Field.Rchk)
                    output.Append("</rchk>")
                    output.Append("<rchcVal>")
                    output.Append(Field.RchcVal)
                    output.Append("</rchcVal>")
                    output.Append("<rchkVal>")
                    output.Append(Field.RchkVal)
                    output.Append("</rchkVal>")
                    output.Append("<AvgSlope>")
                    output.Append(Field.AvgSlope)
                    output.Append("</AvgSlope>")
                    '***** soil information ****
                    AddSoilsInfo(Field)
                    '***** Scenarios information ****
                    AddScenariosInfo(Field)
                    output.Append("</FieldInfo>" & vbCr)
                Next
            End If
            '*****end of field information ****
        Catch ex As System.Exception
            'Return msgDoc.Descendants("Errors").Value & ex.Message
        End Try
    End Sub

    Private Sub AddSiteInfo(ByRef _sitesInfo As List(Of SiteData))
        Try
            If _sitesInfo.Count <= 0 Then Exit Sub
            '***** Begginig of site info ****
            output.Append("<SiteInfo>" & vbCr)
            output.Append("<Latitude>")
            output.Append(_sitesInfo(0).Latitude)
            output.Append("</Latitude>")
            output.Append("<Longitude>")
            output.Append(_sitesInfo(0).Longitude)
            output.Append("</Longitude>")
            output.Append("<Elevation>")
            output.Append(_sitesInfo(0).Elevation)
            output.Append("</Elevation>")
            output.Append("<Apm>")
            output.Append(_sitesInfo(0).Apm)
            output.Append("</Apm>")
            output.Append("<Co2x>")
            output.Append(_sitesInfo(0).Co2x)
            output.Append("</Co2x>")
            output.Append("<Cqnx>")
            output.Append(_sitesInfo(0).Cqnx)
            output.Append("</Cqnx>")
            output.Append("<Rfnx>")
            output.Append(_sitesInfo(0).Rfnx)
            output.Append("</Rfnx>")
            output.Append("<Upr>")
            output.Append(_sitesInfo(0).Upr)
            output.Append("</Upr>")
            output.Append("<Unr>")
            output.Append(_sitesInfo(0).Unr)
            output.Append("</Unr>")
            output.Append("</SiteInfo>")
            '***** end of site info ****
        Catch ex As System.Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value)
        End Try
    End Sub

    Private Sub AddSoilsInfo(ByRef field As FieldsData)
        Try
            If Not field._soilsInfo Is Nothing Then
                For Each soil In field._soilsInfo
                    output.Append("<SoilInfo>" & vbCr)
                    output.Append("<Selected>")
                    output.Append(soil.Selected)
                    output.Append("</Selected>")
                    output.Append("<SoilNumber>")
                    output.Append(soil.SoilNumber)
                    output.Append("</SoilNumber>")
                    output.Append("<Field>")
                    output.Append(soil.Field)
                    output.Append("</Field>")
                    output.Append("<Name>")
                    output.Append(soil.Name)
                    output.Append("</Name>")
                    output.Append("<Area>")
                    output.Append(soil.Percentage)
                    output.Append("</Area>")
                    output.Append("<Albedo>")
                    output.Append(soil.Albedo)
                    output.Append("</Albedo>")
                    output.Append("<Slope>")
                    output.Append(soil.Slope)
                    output.Append("</Slope>")
                    output.Append("<SSACode>")
                    output.Append(soil.SSACode)
                    output.Append("</SSACode>")
                    output.Append("<SSAName>")
                    output.Append(soil.SSAName)
                    output.Append("</SSAName>")
                    output.Append("<Key>")
                    output.Append(soil.Key)
                    output.Append("</Key>")
                    output.Append("<Symbol>")
                    output.Append(soil.Symbol)
                    output.Append("</Symbol>")
                    output.Append("<Group>")
                    output.Append(soil.Group)
                    output.Append("</Group>")
                    output.Append("<Component>")
                    output.Append(soil.Component)
                    output.Append("</Component>")
                    output.Append("<xids>")
                    output.Append(soil.Xids)
                    output.Append("</xids>")
                    output.Append("<Ffc>")
                    output.Append(soil.Ffc)
                    output.Append("</Ffc>")
                    output.Append("<Wtmn>")
                    output.Append(soil.Wtmn)
                    output.Append("</Wtmn>")
                    output.Append("<Wtmx>")
                    output.Append(soil.Wtmx)
                    output.Append("</Wtmx>")
                    output.Append("<Wtbl>")
                    output.Append(soil.Wtbl)
                    output.Append("</Wtbl>")
                    output.Append("<Gwst>")
                    output.Append(soil.Gwst)
                    output.Append("</Gwst>")
                    output.Append("<Gwmx>")
                    output.Append(soil.Gwmx)
                    output.Append("</Gwmx>")
                    output.Append("<Rftt>")
                    output.Append(soil.Rftt)
                    output.Append("</Rftt>")
                    output.Append("<Rfpk>")
                    output.Append(soil.Rfpk)
                    output.Append("</Rfpk>")
                    output.Append("<tsla>")
                    output.Append(soil.tsla)
                    output.Append("</tsla>")
                    output.Append("<Rtn1>")
                    output.Append(soil.Rtn1)
                    output.Append("</Rtn1>")
                    output.Append("<Xidk>")
                    output.Append(soil.Xidk)
                    output.Append("</Xidk>")
                    output.Append("<Zqt>")
                    output.Append(soil.Zqt)
                    output.Append("</Zqt>")
                    output.Append("<Zf>")
                    output.Append(soil.Zf)
                    output.Append("</Zf>")
                    output.Append("<Ztk>")
                    output.Append(soil.Ztk)
                    output.Append("</Ztk>")
                    output.Append("<Fbm>")
                    output.Append(soil.Fbm)
                    output.Append("</Fbm>")
                    output.Append("<Fhp>")
                    output.Append(soil.Fhp)
                    output.Append("</Fhp>")
                    '***** Layers information ****
                    If Not soil._layersInfo Is Nothing Then
                        For Each layer In soil._layersInfo
                            output.Append("<LayerInfo>" & vbCr)
                            output.Append("<LayerNumber>")
                            output.Append(layer.LayerNumber)
                            output.Append("</LayerNumber>")
                            output.Append("<Depth>")
                            output.Append(layer.Depth)
                            output.Append("</Depth>")
                            output.Append("<SoilP>")
                            output.Append(layer.SoilP)
                            output.Append("</SoilP>")
                            output.Append("<BD>")
                            output.Append(layer.BD)
                            output.Append("</BD>")
                            output.Append("<Sand>")
                            output.Append(layer.Sand)
                            output.Append("</Sand>")
                            output.Append("<Silt>")
                            output.Append(layer.Silt)
                            output.Append("</Silt>")
                            output.Append("<OM>")
                            output.Append(layer.OM)
                            output.Append("</OM>")
                            output.Append("<PH>")
                            output.Append(layer.PH)
                            output.Append("</PH>")
                            output.Append("<SatC>")
                            output.Append(layer.Satc)
                            output.Append("</SatC>")
                            output.Append("<Bdd>")
                            output.Append(layer.Bdd)
                            output.Append("</Bdd>")
                            output.Append("<Wn>")
                            output.Append(layer.Wn)
                            output.Append("</Wn>")
                            output.Append("<Uw>")
                            output.Append(layer.Uw)
                            output.Append("</Uw>")
                            output.Append("<Smb>")
                            output.Append(layer.Smb)
                            output.Append("</Smb>")
                            output.Append("<Rsd>")
                            output.Append(layer.Rsd)
                            output.Append("</Rsd>")
                            output.Append("<Rok>")
                            output.Append(layer.Rok)
                            output.Append("</Rok>")
                            output.Append("<Psp>")
                            output.Append(layer.Psp)
                            output.Append("</Psp>")
                            output.Append("<Fc>")
                            output.Append(layer.Fc)
                            output.Append("</Fc>")
                            output.Append("<Cnds>")
                            output.Append(layer.Cnds)
                            output.Append("</Cnds>")
                            output.Append("<cac>")
                            output.Append(layer.Cac)
                            output.Append("</cac>")
                            output.Append("<cec>")
                            output.Append(layer.Cec)
                            output.Append("</cec>")
                            output.Append("</LayerInfo>" & vbCr)
                        Next
                    End If
                    '****** Scenarios Information ******
                    If Not soil._scenariosInfo Is Nothing Then
                        For Each scenarios In soil._scenariosInfo
                            output.Append("<SoilScenarioInfo>" & vbCr)
                            output.Append("<Name>")
                            output.Append(scenarios.Name)
                            output.Append("</Name>")
                            '***** subarea information ****
                            If Not scenarios._subareasInfo Is Nothing Then
                                AddSubareasInfo(scenarios._subareasInfo, "Subareas")
                                'Next
                            End If
                            '***** End Subarea Info.  ****
                            '***** Operation information ****
                            If Not scenarios._operationsInfo Is Nothing Then
                                For Each oper In scenarios._operationsInfo
                                    output.Append("<Operations>" & vbCr)
                                    output.Append("<Scenario>")
                                    output.Append(oper.Scenario)
                                    output.Append("</Scenario>")
                                    output.Append("<EventId>")
                                    output.Append(oper.EventId)
                                    output.Append("</EventId>")
                                    output.Append("<ApexCrop>")
                                    output.Append(oper.ApexCrop)
                                    output.Append("</ApexCrop>")
                                    output.Append("<ApexCropName>")
                                    output.Append(oper.ApexCropName)
                                    output.Append("</ApexCropName>")
                                    output.Append("<ApexOp>")
                                    output.Append(oper.ApexOp)
                                    output.Append("</ApexOp>")
                                    output.Append("<ApexOpAbbreviation>")
                                    output.Append(oper.ApexOpAbbreviation)
                                    output.Append("</ApexOpAbbreviation>")
                                    output.Append("<ApexOpName>")
                                    output.Append(oper.ApexOpName)
                                    output.Append("</ApexOpName>")
                                    output.Append("<Year>")
                                    output.Append(oper.Year)
                                    output.Append("</Year>")
                                    output.Append("<Month>")
                                    output.Append(oper.Month)
                                    output.Append("</Month>")
                                    output.Append("<Day>")
                                    output.Append(oper.Day)
                                    output.Append("</Day>")
                                    output.Append("<Period>")
                                    output.Append(oper.Period)
                                    output.Append("</Period>")
                                    output.Append("<ApexTillCode>")
                                    output.Append(oper.ApexTillCode)
                                    output.Append("</ApexTillCode>")
                                    output.Append("<ApexTillName>")
                                    output.Append(oper.ApexTillName)
                                    output.Append("</ApexTillName>")
                                    output.Append("<var9>")
                                    output.Append(oper.var9)
                                    output.Append("</var9>")
                                    output.Append("<ApexFert>")
                                    output.Append(oper.apexOpType)
                                    output.Append("</ApexFert>")
                                    output.Append("<ApexOpv1>")
                                    output.Append(oper.ApexOpv1)
                                    output.Append("</ApexOpv1>")
                                    output.Append("<ApexOpv2>")
                                    output.Append(oper.ApexOpv2)
                                    output.Append("</ApexOpv2>")
                                    output.Append("<OrgN>")
                                    output.Append(oper.OrgN)
                                    output.Append("</OrgN>")
                                    output.Append("<NO3>")
                                    output.Append(oper.NO3)
                                    output.Append("</NO3>")
                                    output.Append("<OrgP>")
                                    output.Append(oper.OrgP)
                                    output.Append("</OrgP>")
                                    output.Append("<PO4>")
                                    output.Append(oper.PO4)
                                    output.Append("</PO4>")
                                    output.Append("<NH3>")
                                    output.Append(oper.NH3)
                                    output.Append("</NH3>")
                                    output.Append("<K>")
                                    output.Append(oper.K)
                                    output.Append("</K>")
                                    output.Append("<TractorId>")
                                    output.Append(oper.TractorId)
                                    output.Append("</TractorId>")
                                    output.Append("<OpVal1>")
                                    output.Append(oper.OpVal1)
                                    output.Append("</OpVal1>")
                                    output.Append("<OpVal2>")
                                    output.Append(oper.OpVal2)
                                    output.Append("</OpVal2>")
                                    output.Append("<OpVal3>")
                                    output.Append(oper.OpVal3)
                                    output.Append("</OpVal3>")
                                    output.Append("<OpVal4>")
                                    output.Append(oper.OpVal4)
                                    output.Append("</OpVal4>")
                                    output.Append("<OpVal5>")
                                    output.Append(oper.OpVal5)
                                    output.Append("</OpVal5>")
                                    output.Append("<OpVal6>")
                                    output.Append(oper.OpVal6)
                                    output.Append("</OpVal6>")
                                    output.Append("<OpVal7>")
                                    output.Append(oper.OpVal7)
                                    output.Append("</OpVal7>")
                                    output.Append("<MixedCropData>")
                                    output.Append(oper.MixedCropData)
                                    output.Append("</MixedCropData>")
                                    output.Append("<Index>")
                                    output.Append(oper.Index)
                                    output.Append("</Index>")
                                    output.Append("<LuNumber>")
                                    output.Append(oper.LuNumber)
                                    output.Append("</LuNumber>")
                                    output.Append("<ConvertionUnit>")
                                    output.Append(oper.ConvertionUnit)
                                    output.Append("</ConvertionUnit>")
                                    output.Append("</Operations>" & vbCr)
                                    '*****end of Operation information ****
                                Next
                            End If
                            '***** Results information ****
                            output.Append("<Results>")
                            output.Append("<area>")
                            output.Append(scenarios._results.area)
                            output.Append("</area>")
                            output.Append("<lastSimulation>")
                            output.Append(scenarios._results.lastSimulation)
                            output.Append("</lastSimulation>")
                            output.Append("<Crops>")
                            If Not scenarios._results.SoilResults.Crops.cropName Is Nothing Then
                                For Each crop In scenarios._results.SoilResults.Crops.cropName
                                    output.Append("<cropName>")
                                    output.Append(crop)
                                    output.Append("</cropName>")
                                Next
                            End If
                            If Not scenarios._results.SoilResults.Crops.cropRecords Is Nothing Then
                                For Each crop In scenarios._results.SoilResults.Crops.cropRecords
                                    output.Append("<cropRecord>")
                                    output.Append(crop)
                                    output.Append("</cropRecord>")
                                Next
                            End If
                            If Not scenarios._results.SoilResults.Crops.cropYield Is Nothing Then
                                For Each crop In scenarios._results.SoilResults.Crops.cropYield
                                    output.Append("<cropYield>")
                                    output.Append(crop)
                                    output.Append("</cropYield>")
                                Next
                            End If
                            If Not scenarios._results.SoilResults.Crops.cropYieldCI Is Nothing Then
                                For Each crop In scenarios._results.SoilResults.Crops.cropYieldCI
                                    output.Append("<cropYieldCI>")
                                    output.Append(crop)
                                    output.Append("</cropYieldCI>")
                                Next
                            End If
                            If Not scenarios._results.SoilResults.Crops.cropYieldSD Is Nothing Then
                                For Each crop In scenarios._results.SoilResults.Crops.cropYieldSD
                                    output.Append("<cropYieldSD>")
                                    output.Append(crop)
                                    output.Append("</cropYieldSD>")
                                Next
                            End If
                            If Not scenarios._results.SoilResults.Crops.ns Is Nothing Then
                                For Each crop In scenarios._results.SoilResults.Crops.ns
                                    output.Append("<cropns>")
                                    output.Append(crop)
                                    output.Append("</cropns>")
                                Next
                            End If
                            If Not scenarios._results.SoilResults.Crops.ps Is Nothing Then
                                For Each crop In scenarios._results.SoilResults.Crops.ps
                                    output.Append("<cropps>")
                                    output.Append(crop)
                                    output.Append("</cropps>")
                                Next
                            End If
                            If Not scenarios._results.SoilResults.Crops.ts Is Nothing Then
                                For Each crop In scenarios._results.SoilResults.Crops.ts
                                    output.Append("<cropts>")
                                    output.Append(crop)
                                    output.Append("</cropts>")
                                Next
                            End If
                            If Not scenarios._results.SoilResults.Crops.ws Is Nothing Then
                                For Each crop In scenarios._results.SoilResults.Crops.ws
                                    output.Append("<cropws>")
                                    output.Append(crop)
                                    output.Append("</cropws>")
                                Next
                            End If
                            output.Append("</Crops>")
                            output.Append("<FIFertilizer>")
                            output.Append(scenarios._results.FIFertilizer)
                            output.Append("</FIFertilizer>")
                            output.Append("<i>")
                            output.Append(scenarios._results.i)
                            output.Append("</i>")
                            '***** SoilsResults information ****
                            output.Append("<Soil>")
                            output.Append("<co2>")
                            output.Append(scenarios._results.SoilResults.co2)
                            output.Append("</co2>")
                            output.Append("<deepPerFlow>")
                            output.Append(scenarios._results.SoilResults.deepPerFlow)
                            output.Append("</deepPerFlow>")
                            output.Append("<deepPerFlowCI>")
                            output.Append(scenarios._results.SoilResults.deepPerFlowCI)
                            output.Append("</deepPerFlowCI>")
                            output.Append("<runoff>")
                            output.Append(scenarios._results.SoilResults.runoff)
                            output.Append("</runoff>")
                            output.Append("<runoffCI>")
                            output.Append(scenarios._results.SoilResults.runoffCI)
                            output.Append("</runoffCI>")
                            output.Append("<runoffSD>")
                            output.Append(scenarios._results.SoilResults.runoffSD)
                            output.Append("</runoffSD>")
                            output.Append("<subsurfaceFlow>")
                            output.Append(scenarios._results.SoilResults.subsurfaceFlow)
                            output.Append("</subsurfaceFlow>")
                            output.Append("<subsurfaceFlowCI>")
                            output.Append(scenarios._results.SoilResults.subsurfaceFlowCI)
                            output.Append("</subsurfaceFlowCI>")
                            output.Append("<subsurfaceFlowSD>")
                            output.Append(scenarios._results.SoilResults.subsurfaceFlowSD)
                            output.Append("</subsurfaceFlowSD>")
                            output.Append("<tileDrainFlow>")
                            output.Append(scenarios._results.SoilResults.tileDrainFlow)
                            output.Append("</tileDrainFlow>")
                            output.Append("<tileDrainFlowCI>")
                            output.Append(scenarios._results.SoilResults.tileDrainFlowCI)
                            output.Append("</tileDrainFlowCI>")
                            output.Append("<tileDrainFlowSD>")
                            output.Append(scenarios._results.SoilResults.tileDrainFlowSD)
                            output.Append("</tileDrainFlowSD>")
                            output.Append("<irrigation>")
                            output.Append(scenarios._results.SoilResults.irrigation)
                            output.Append("</irrigation>")
                            output.Append("<irrigationCI>")
                            output.Append(scenarios._results.SoilResults.irrigationCI)
                            output.Append("</irrigationCI>")
                            output.Append("<LeachedN>")
                            output.Append(scenarios._results.SoilResults.LeachedN)
                            output.Append("</LeachedN>")
                            output.Append("<LeachedP>")
                            output.Append(scenarios._results.SoilResults.LeachedP)
                            output.Append("</LeachedP>")
                            output.Append("<n2o>")
                            output.Append(scenarios._results.SoilResults.n2o)
                            output.Append("</n2o>")
                            output.Append("<runoffN>")
                            output.Append(scenarios._results.SoilResults.RunoffN)
                            output.Append("</runoffN>")
                            output.Append("<runoffNCI>")
                            output.Append(scenarios._results.SoilResults.runoffNCI)
                            output.Append("</runoffNCI>")
                            output.Append("<runoffNSD>")
                            output.Append(scenarios._results.SoilResults.runoffNSD)
                            output.Append("</runoffNSD>")
                            output.Append("<subsurfaceN>")
                            output.Append(scenarios._results.SoilResults.SubsurfaceN)
                            output.Append("</subsurfaceN>")
                            output.Append("<subsurfaceNCI>")
                            output.Append(scenarios._results.SoilResults.subsurfaceNCI)
                            output.Append("</subsurfaceNCI>")
                            output.Append("<subsurfaceNSD>")
                            output.Append(scenarios._results.SoilResults.subsurfaceNSD)
                            output.Append("</subsurfaceNSD>")
                            output.Append("<OrgN>")
                            output.Append(scenarios._results.SoilResults.OrgN)
                            output.Append("</OrgN>")
                            output.Append("<OrgNCI>")
                            output.Append(scenarios._results.SoilResults.OrgNCI)
                            output.Append("</OrgNCI>")
                            output.Append("<OrgNSD>")
                            output.Append(scenarios._results.SoilResults.OrgNSD)
                            output.Append("</OrgNSD>")
                            output.Append("<OrgP>")
                            output.Append(scenarios._results.SoilResults.OrgP)
                            output.Append("</OrgP>")
                            output.Append("<OrgPCI>")
                            output.Append(scenarios._results.SoilResults.OrgPCI)
                            output.Append("</OrgPCI>")
                            output.Append("<OrgPSD>")
                            output.Append(scenarios._results.SoilResults.OrgPSD)
                            output.Append("</OrgPSD>")
                            output.Append("<PO4>")
                            output.Append(scenarios._results.SoilResults.PO4)
                            output.Append("</PO4>")
                            output.Append("<PO4CI>")
                            output.Append(scenarios._results.SoilResults.PO4CI)
                            output.Append("</PO4CI>")
                            output.Append("<PO4SD>")
                            output.Append(scenarios._results.SoilResults.PO4SD)
                            output.Append("</PO4SD>")
                            output.Append("<Sediment>")
                            output.Append(scenarios._results.SoilResults.Sediment)
                            output.Append("</Sediment>")
                            output.Append("<SedimentCI>")
                            output.Append(scenarios._results.SoilResults.SedimentCI)
                            output.Append("</SedimentCI>")
                            output.Append("<SedimentSD>")
                            output.Append(scenarios._results.SoilResults.SedimentSD)
                            output.Append("</SedimentSD>")
                            output.Append("<tileDrainFlow>")
                            output.Append(scenarios._results.SoilResults.tileDrainFlow)
                            output.Append("</tileDrainFlow>")
                            output.Append("<tileDrainN>")
                            output.Append(scenarios._results.SoilResults.tileDrainN)
                            output.Append("</tileDrainN>")
                            output.Append("<tileDrainP>")
                            output.Append(scenarios._results.SoilResults.TileDrainP)
                            output.Append("</tileDrainP>")
                            output.Append("<tileDrainFlowCI>")
                            output.Append(scenarios._results.SoilResults.tileDrainFlowCI)
                            output.Append("</tileDrainFlowCI>")
                            output.Append("<tileDrainNCI>")
                            output.Append(scenarios._results.SoilResults.tileDrainNCI)
                            output.Append("</tileDrainNCI>")
                            output.Append("<tileDrainPCI>")
                            output.Append(scenarios._results.SoilResults.tileDrainPCI)
                            output.Append("</tileDrainPCI>")
                            output.Append("<tileDrainFlowSD>")
                            output.Append(scenarios._results.SoilResults.tileDrainFlowSD)
                            output.Append("</tileDrainFlowSD>")
                            output.Append("<tileDrainNSD>")
                            output.Append(scenarios._results.SoilResults.tileDrainNSD)
                            output.Append("</tileDrainNSD>")
                            output.Append("<tileDrainPSD>")
                            output.Append(scenarios._results.SoilResults.tileDrainPSD)
                            output.Append("</tileDrainPSD>")
                            output.Append("<volatizationN>")
                            output.Append(scenarios._results.SoilResults.volatizationN)
                            output.Append("</volatizationN>")
                            'Add annual and monthly values for graphs.
                            If Not scenarios._results.annualPrecipitation Is Nothing Then
                                output.Append("<annualPrecipitation>")
                                For Each value In scenarios._results.annualPrecipitation
                                    output.Append("<precipitation>")
                                    output.Append(value)
                                    output.Append("</precipitation>")
                                Next
                                output.Append("</annualPrecipitation>")
                            End If
                            If Not scenarios._results.annualFlow Is Nothing Then
                                output.Append("<annualFlow>")
                                For Each value In scenarios._results.annualFlow
                                    output.Append("<flow>")
                                    output.Append(value)
                                    output.Append("</flow>")
                                Next
                                output.Append("</annualFlow>")
                            End If
                            If Not scenarios._results.annualNO3 Is Nothing Then
                                output.Append("<annualNO3>")
                                For Each value In scenarios._results.annualNO3
                                    output.Append("<NO3>")
                                    output.Append(value)
                                    output.Append("</NO3>")
                                Next
                                output.Append("</annualNO3>")
                            End If
                            If Not scenarios._results.annualOrgN Is Nothing Then
                                output.Append("<annualOrgN>")
                                For Each value In scenarios._results.annualOrgN
                                    output.Append("<OrgN>")
                                    output.Append(value)
                                    output.Append("</OrgN>")
                                Next
                                output.Append("</annualOrgN>")
                            End If
                            If Not scenarios._results.annualN2o Is Nothing Then
                                output.Append("<annualN2O>")
                                For Each value In scenarios._results.annualN2o
                                    output.Append("<N2O>")
                                    output.Append(value)
                                    output.Append("</N2O>")
                                Next
                                output.Append("</annualN2O>")
                            End If
                            If Not scenarios._results.annualOrgP Is Nothing Then
                                output.Append("<annualOrgP>")
                                For Each value In scenarios._results.annualOrgP
                                    output.Append("<OrgP>")
                                    output.Append(value)
                                    output.Append("</OrgP>")
                                Next
                                output.Append("</annualOrgP>")
                            End If
                            If Not scenarios._results.annualPO4 Is Nothing Then
                                output.Append("<annualPO4>")
                                For Each value In scenarios._results.annualPO4
                                    output.Append("<PO4>")
                                    output.Append(value)
                                    output.Append("</PO4>")
                                Next
                                output.Append("</annualPO4>")
                            End If
                            If Not scenarios._results.annualSediment Is Nothing Then
                                output.Append("<annualSediment>")
                                For Each value In scenarios._results.annualSediment
                                    output.Append("<Sediment>")
                                    output.Append(value)
                                    output.Append("</Sediment>")
                                Next
                                output.Append("</annualSediment>")
                            End If
                            'end annual results for graphs.
                            '*****end SoilsResults information ****
                            output.Append("</Soil>")
                            output.Append("</Results>")
                            '***** End Results information ****
                            output.Append("</SoilScenarioInfo>" & vbCr)
                        Next
                    End If
                    output.Append("</SoilInfo>" & vbCr)
                    '*****end of Layers information ****
                Next
                '*****end of soil information ****
            End If
        Catch e1 As Exception
            ''showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & e1.Message)

        End Try
    End Sub

    Private Sub AddSubareasInfo(ByRef _subareasInfo As SubareasData, type As String)
        output.Append("<" & type & ">" & vbCr)
        With _subareasInfo
            output.Append("<SoilComponent>")
            output.Append(.SoilComponent)
            output.Append("</SoilComponent>")
            output.Append("<SoilGroup>")
            output.Append(.SoilGroup)
            output.Append("</SoilGroup>")
            output.Append("<SoilKey>")
            output.Append(.SoilKey)
            output.Append("</SoilKey>")
            output.Append("<SoilSymbol>")
            output.Append(.SoilSymbol)
            output.Append("</SoilSymbol>")
            output.Append("<SubareaNumber>")
            output.Append(.SubareaNumber)
            output.Append("</SubareaNumber>")
            output.Append("<SubareaTitle>")
            output.Append(.SubareaTitle)
            output.Append("</SubareaTitle>")
            output.Append("<SbaType>")
            output.Append(.SbaType)
            output.Append("</SbaType>")
        End With
        With _subareasInfo._line2(0)
            output.Append("<Iapl>")
            output.Append(.Iapl)
            output.Append("</Iapl>")
            output.Append("<Ii>")
            output.Append(.Ii)
            output.Append("</Ii>")
            output.Append("<Imw>")
            output.Append(.Imw)
            output.Append("</Imw>")
            output.Append("<Inps>")
            output.Append(.Inps)
            output.Append("</Inps>")
            output.Append("<Iops>")
            output.Append(.Iops)
            output.Append("</Iops>")
            output.Append("<Iow>")
            output.Append(.Iow)
            output.Append("</Iow>")
            output.Append("<Ipts>")
            output.Append(.Ipts)
            output.Append("</Ipts>")
            output.Append("<Isao>")
            output.Append(.Isao)
            output.Append("</Isao>")
            output.Append("<Iwth>")
            output.Append(.Iwth)
            output.Append("</Iwth>")
            output.Append("<Luns>")
            output.Append(.Luns)
            output.Append("</Luns>")
            output.Append("<Nvcn>")
            output.Append(.Nvcn)
            output.Append("</Nvcn>")
        End With
        With _subareasInfo._line3(0)
            output.Append("<Angl>")
            output.Append(.Angl)
            output.Append("</Angl>")
            output.Append("<Azm>")
            output.Append(.Azm)
            output.Append("</Azm>")
            output.Append("<Fl>")
            output.Append(.Fl)
            output.Append("</Fl>")
            output.Append("<Fw>")
            output.Append(.Fw)
            output.Append("</Fw>")
            output.Append("<Sno>")
            output.Append(.Sno)
            output.Append("</Sno>")
            output.Append("<Stdo>")
            output.Append(.Stdo)
            output.Append("</Stdo>")
            output.Append("<Xct>")
            output.Append(.Xct)
            output.Append("</Xct>")
            output.Append("<Yct>")
            output.Append(.Yct)
            output.Append("</Yct>")
        End With
        With _subareasInfo._line4(0)
            output.Append("<Wsa>")
            output.Append(.Wsa)
            output.Append("</Wsa>")
            output.Append("<Chd>")
            output.Append(.Chd)
            output.Append("</Chd>")
            output.Append("<chl>")
            output.Append(.chl)
            output.Append("</chl>")
            output.Append("<Chn>")
            output.Append(.Chn)
            output.Append("</Chn>")
            output.Append("<Chs>")
            output.Append(.Chs)
            output.Append("</Chs>")
            output.Append("<Ffpq>")
            output.Append(.Ffpq)
            output.Append("</Ffpq>")
            output.Append("<Slp>")
            output.Append(.Slp)
            output.Append("</Slp>")
            output.Append("<Slpg>")
            output.Append(.Slpg)
            output.Append("</Slpg>")
            output.Append("<Upn>")
            output.Append(.Upn)
            output.Append("</Upn>")
            output.Append("<Urbf>")
            output.Append(.Urbf)
            output.Append("</Urbf>")
        End With
        With _subareasInfo._line5(0)
            output.Append("<Rchl>")
            output.Append(.Rchl)
            output.Append("</Rchl>")
            output.Append("<Rcbw>")
            output.Append(.Rcbw)
            output.Append("</Rcbw>")
            output.Append("<Rctw>")
            output.Append(.Rctw)
            output.Append("</Rctw>")
            output.Append("<Rchc>")
            output.Append(.Rchc)
            output.Append("</Rchc>")
            output.Append("<Rchd>")
            output.Append(.Rchd)
            output.Append("</Rchd>")
            output.Append("<Rchk>")
            output.Append(.Rchk)
            output.Append("</Rchk>")
            output.Append("<Rchn>")
            output.Append(.Rchn)
            output.Append("</Rchn>")
            output.Append("<Rchs>")
            output.Append(.Rchs)
            output.Append("</Rchs>")
            output.Append("<Rfpl>")
            output.Append(.Rfpl)
            output.Append("</Rfpl>")
            output.Append("<Rfpw>")
            output.Append(.Rfpw)
            output.Append("</Rfpw>")
        End With
        With _subareasInfo._line6(0)
            output.Append("<Rsae>")
            output.Append(.Rsae)
            output.Append("</Rsae>")
            output.Append("<Rsap>")
            output.Append(.Rsap)
            output.Append("</Rsap>")
            output.Append("<Rsee>")
            output.Append(.Rsee)
            output.Append("</Rsee>")
            output.Append("<Rsep>")
            output.Append(.Rsep)
            output.Append("</Rsep>")
            output.Append("<Rsrr>")
            output.Append(.Rsrr)
            output.Append("</Rsrr>")
            output.Append("<Rsv>")
            output.Append(.Rsv)
            output.Append("</Rsv>")
            output.Append("<Rsve>")
            output.Append(.Rsve)
            output.Append("</Rsve>")
            output.Append("<Rsvp>")
            output.Append(.Rsvp)
            output.Append("</Rsvp>")
            output.Append("<Rsyn>")
            output.Append(.Rsyn)
            output.Append("</Rsyn>")
            output.Append("<Rsys>")
            output.Append(.Rsys)
            output.Append("</Rsys>")
        End With
        With _subareasInfo._line7(0)
            output.Append("<Rshc>")
            output.Append(.Rshc)
            output.Append("</Rshc>")
            output.Append("<Rsbd>")
            output.Append(.Rsbd)
            output.Append("</Rsbd>")
            output.Append("<Rsdp>")
            output.Append(.Rsdp)
            output.Append("</Rsdp>")
            output.Append("<Pcof>")
            output.Append(.Pcof)
            output.Append("</Pcof>")
            output.Append("<Bcof>")
            output.Append(.Bcof)
            output.Append("</Bcof>")
            output.Append("<Bffl>")
            output.Append(.Bffl)
            output.Append("</Bffl>")
        End With
        With _subareasInfo._line8(0)
            output.Append("<Idf1>")
            output.Append(.Idf1)
            output.Append("</Idf1>")
            output.Append("<Idf2>")
            output.Append(.Idf2)
            output.Append("</Idf2>")
            output.Append("<Idf3>")
            output.Append(.Idf3)
            output.Append("</Idf3>")
            output.Append("<Idf4>")
            output.Append(.Idf4)
            output.Append("</Idf4>")
            output.Append("<Idf5>")
            output.Append(.Idf5)
            output.Append("</Idf5>")
            output.Append("<Idr>")
            output.Append(.Idr)
            output.Append("</Idr>")
            output.Append("<Ifa>")
            output.Append(.Ifa)
            output.Append("</Ifa>")
            output.Append("<Ifd>")
            output.Append(.Ifd)
            output.Append("</Ifd>")
            output.Append("<Iri>")
            output.Append(.Iri)
            output.Append("</Iri>")
            output.Append("<Lm>")
            output.Append(.Lm)
            output.Append("</Lm>")
            output.Append("<Nirr>")
            output.Append(.Nirr)
            output.Append("</Nirr>")
        End With
        With _subareasInfo._line9(0)
            output.Append("<Armn>")
            output.Append(.Armn)
            output.Append("</Armn>")
            output.Append("<Armx>")
            output.Append(.Armx)
            output.Append("</Armx>")
            output.Append("<Bft>")
            output.Append(.Bft)
            output.Append("</Bft>")
            output.Append("<Bir>")
            output.Append(.Bir)
            output.Append("</Bir>")
            output.Append("<Fnp4>")
            output.Append(.Fnp4)
            output.Append("</Fnp4>")
            output.Append("<Efi>")
            output.Append(.Efi)
            output.Append("</Efi>")
            output.Append("<Fmx>")
            output.Append(.Fmx)
            output.Append("</Fmx>")
            output.Append("<Vimx>")
            output.Append(.Vimx)
            output.Append("</Vimx>")
            output.Append("<Drt>")
            output.Append(.Drt)
            output.Append("</Drt>")
            output.Append("<Fdsf>")
            output.Append(.Fdsf)
            output.Append("</Fdsf>")
        End With
        With _subareasInfo._line10(0)
            output.Append("<Pec>")
            output.Append(.Pec)
            output.Append("</Pec>")
            output.Append("<Dalg>")
            output.Append(.Dalg)
            output.Append("</Dalg>")
            output.Append("<Vlgn>")
            output.Append(.Vlgn)
            output.Append("</Vlgn>")
            output.Append("<Coww>")
            output.Append(.Coww)
            output.Append("</Coww>")
            output.Append("<Ddlg>")
            output.Append(.Ddlg)
            output.Append("</Ddlg>")
            output.Append("<Solq>")
            output.Append(.Solq)
            output.Append("</Solq>")
            output.Append("<Sflg>")
            output.Append(.Sflg)
            output.Append("</Sflg>")
            output.Append("<Fnp2>")
            output.Append(.Fnp2)
            output.Append("</Fnp2>")
            output.Append("<Fnp5>")
            output.Append(.Fnp5)
            output.Append("</Fnp5>")
            output.Append("<Firg>")
            output.Append(.Firg)
            output.Append("</Firg>")
        End With
        With _subareasInfo._line11(0)
            output.Append("<Ny1>")
            output.Append(.Ny1)
            output.Append("</Ny1>")
            output.Append("<Ny2>")
            output.Append(.Ny2)
            output.Append("</Ny2>")
            output.Append("<Ny3>")
            output.Append(.Ny3)
            output.Append("</Ny3>")
            output.Append("<Ny4>")
            output.Append(.Ny4)
            output.Append("</Ny4>")
        End With
        With _subareasInfo._line12(0)
            output.Append("<Xtp1>")
            output.Append(.Xtp1)
            output.Append("</Xtp1>")
            output.Append("<Xtp2>")
            output.Append(.Xtp2)
            output.Append("</Xtp2>")
            output.Append("<Xtp3>")
            output.Append(.Xtp3)
            output.Append("</Xtp3>")
            output.Append("<Xtp4>")
            output.Append(.Xtp4)
            output.Append("</Xtp4>")
        End With
        AddOperationsInfo(_subareasInfo._operationsInfo, output)
        output.Append("</" & type & ">" & vbCr)
    End Sub

    Private Sub AddScenariosInfo(ByRef field As FieldsData)
        Try
            If Not field._scenariosInfo Is Nothing Then
                For Each scenarios In field._scenariosInfo
                    output.Append("<ScenarioInfo>" & vbCr)
                    output.Append("<Name>")
                    output.Append(scenarios.Name)
                    output.Append("</Name>")
                    '***** Operation information ****
                    If Not scenarios._operationsInfo Is Nothing Then
                        AddOperationsInfo(scenarios._operationsInfo, output)
                    End If
                    '***** Subareas Information *****
                    If Not scenarios._bufferInfo Is Nothing Then
                        AddBufferInfo(scenarios._bufferInfo)
                    End If
                    '***** bmps information ****
                    If Not scenarios._bmpsInfo Is Nothing Then
                        AddBMPsInfo(scenarios._bmpsInfo, output)
                    End If
                    '***** FEM Result information ****
                    If Not scenarios._femInfo Is Nothing Then
                        AddFEMResultsInfo(scenarios._femInfo, output)
                    End If
                    AddScenarioResultsInfo(scenarios._results, output)
                    output.Append("</ScenarioInfo>" & vbCr)
                    '***** End Operation information ****
                Next
            End If
        Catch e1 As Exception
            'showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & e1.Message)
        End Try
    End Sub

    Public Sub AddOperationsInfo(ByRef _operationsInfo As List(Of OperationsData), ByRef output As StringBuilder)
        For Each oper In _operationsInfo
            output.Append("<Operations>" & vbCr)
            output.Append("<Scenario>")
            output.Append(oper.Scenario)
            output.Append("</Scenario>")
            output.Append("<EventId>")
            output.Append(oper.EventId)
            output.Append("</EventId>")
            output.Append("<ApexCrop>")
            output.Append(oper.ApexCrop)
            output.Append("</ApexCrop>")
            output.Append("<ApexCropName>")
            output.Append(oper.ApexCropName)
            output.Append("</ApexCropName>")
            output.Append("<ApexOp>")
            output.Append(oper.ApexOp)
            output.Append("</ApexOp>")
            output.Append("<ApexOpAbbreviation>")
            output.Append(oper.ApexOpAbbreviation)
            output.Append("</ApexOpAbbreviation>")
            output.Append("<ApexOpName>")
            output.Append(oper.ApexOpName)
            output.Append("</ApexOpName>")
            output.Append("<Year>")
            output.Append(oper.Year)
            output.Append("</Year>")
            output.Append("<Month>")
            output.Append(oper.Month)
            output.Append("</Month>")
            output.Append("<Day>")
            output.Append(oper.Day)
            output.Append("</Day>")
            output.Append("<Period>")
            output.Append(oper.Period)
            output.Append("</Period>")
            output.Append("<ApexTillCode>")
            output.Append(oper.ApexTillCode)
            output.Append("</ApexTillCode>")
            output.Append("<ApexTillName>")
            output.Append(oper.ApexTillName)
            output.Append("</ApexTillName>")
            output.Append("<var9>")
            output.Append(oper.var9)
            output.Append("</var9>")
            output.Append("<ApexFert>")
            output.Append(oper.ApexOpType)
            output.Append("</ApexFert>")
            output.Append("<ApexOpv1>")
            output.Append(oper.ApexOpv1)
            output.Append("</ApexOpv1>")
            output.Append("<ApexOpv2>")
            output.Append(oper.ApexOpv2)
            output.Append("</ApexOpv2>")
            output.Append("<OrgN>")
            output.Append(oper.OrgN)
            output.Append("</OrgN>")
            output.Append("<NO3>")
            output.Append(oper.NO3)
            output.Append("</NO3>")
            output.Append("<OrgP>")
            output.Append(oper.OrgP)
            output.Append("</OrgP>")
            output.Append("<PO4>")
            output.Append(oper.PO4)
            output.Append("</PO4>")
            output.Append("<NH3>")
            output.Append(oper.NH3)
            output.Append("</NH3>")
            output.Append("<K>")
            output.Append(oper.K)
            output.Append("</K>")
            output.Append("<TractorId>")
            output.Append(oper.TractorId)
            output.Append("</TractorId>")
            output.Append("<OpVal1>")
            output.Append(oper.OpVal1)
            output.Append("</OpVal1>")
            output.Append("<OpVal2>")
            output.Append(oper.OpVal2)
            output.Append("</OpVal2>")
            output.Append("<OpVal3>")
            output.Append(oper.OpVal3)
            output.Append("</OpVal3>")
            output.Append("<OpVal4>")
            output.Append(oper.OpVal4)
            output.Append("</OpVal4>")
            output.Append("<OpVal5>")
            output.Append(oper.OpVal5)
            output.Append("</OpVal5>")
            output.Append("<OpVal6>")
            output.Append(oper.OpVal6)
            output.Append("</OpVal6>")
            output.Append("<OpVal7>")
            output.Append(oper.OpVal7)
            output.Append("</OpVal7>")
            output.Append("<MixedCropData>")
            output.Append(oper.MixedCropData)
            output.Append("</MixedCropData>")
            output.Append("<Index>")
            output.Append(oper.Index)
            output.Append("</Index>")
            output.Append("<LuNumber>")
            output.Append(oper.LuNumber)
            output.Append("</LuNumber>")
            output.Append("<ConvertionUnit>")
            output.Append(oper.ConvertionUnit)
            output.Append("</ConvertionUnit>")
            output.Append("</Operations>" & vbCr)
            '*****end of Operation information ****
        Next
    End Sub

    Private Sub AddBufferInfo(ByRef bufferInfo As List(Of SubareasData))
        For Each sba In bufferInfo
            AddSubareasInfo(sba, "Buffers")
            'output.Append("<Subareas>" & vbCr)
            'output.Append("<Angl>")
            'output.Append(sba.Angl)
            'output.Append("</Angl>")
            'output.Append("<Armn>")
            'output.Append(sba.Armn)
            'output.Append("</Armn>")
            'output.Append("<Armx>")
            'output.Append(sba.Armx)
            'output.Append("</Armx>")
            'output.Append("<Azm>")
            'output.Append(sba.Azm)
            'output.Append("</Azm>")
            'output.Append("<Bcof>")
            'output.Append(sba.Bcof)
            'output.Append("</Bcof>")
            'output.Append("<Bffl>")
            'output.Append(sba.Bffl)
            'output.Append("</Bffl>")
            'output.Append("<Bft>")
            'output.Append(sba.Bft)
            'output.Append("</Bft>")
            'output.Append("<Bir>")
            'output.Append(sba.Bir)
            'output.Append("</Bir>")
            'output.Append("<Chd>")
            'output.Append(sba.Chd)
            'output.Append("</Chd>")
            'output.Append("<chl>")
            'output.Append(sba.chl)
            'output.Append("</chl>")
            'output.Append("<Chn>")
            'output.Append(sba.Chn)
            'output.Append("</Chn>")
            'output.Append("<Chs>")
            'output.Append(sba.Chs)
            'output.Append("</Chs>")
            'output.Append("<Coww>")
            'output.Append(sba.Coww)
            'output.Append("</Coww>")
            'output.Append("<Dalg>")
            'output.Append(sba.Dalg)
            'output.Append("</Dalg>")
            'output.Append("<Ddlg>")
            'output.Append(sba.Ddlg)
            'output.Append("</Ddlg>")
            'output.Append("<Drt>")
            'output.Append(sba.Drt)
            'output.Append("</Drt>")
            'output.Append("<Efi>")
            'output.Append(sba.Efi)
            'output.Append("</Efi>")
            'output.Append("<Fdsf>")
            'output.Append(sba.Fdsf)
            'output.Append("</Fdsf>")
            'output.Append("<Ffpq>")
            'output.Append(sba.Ffpq)
            'output.Append("</Ffpq>")
            'output.Append("<Firg>")
            'output.Append(sba.Firg)
            'output.Append("</Firg>")
            'output.Append("<Fl>")
            'output.Append(sba.Fl)
            'output.Append("</Fl>")
            'output.Append("<Fmx>")
            'output.Append(sba.Fmx)
            'output.Append("</Fmx>")
            'output.Append("<Fnp2>")
            'output.Append(sba.Fnp2)
            'output.Append("</Fnp2>")
            'output.Append("<Fnp4>")
            'output.Append(sba.Fnp4)
            'output.Append("</Fnp4>")
            'output.Append("<Fnp5>")
            'output.Append(sba.Fnp5)
            'output.Append("</Fnp5>")
            'output.Append("<Fw>")
            'output.Append(sba.Fw)
            'output.Append("</Fw>")
            'output.Append("<Iapl>")
            'output.Append(sba._line2(0).Iapl)
            'output.Append("</Iapl>")
            'output.Append("<Idf1>")
            'output.Append(sba.Idf1)
            'output.Append("</Idf1>")
            'output.Append("<Idf2>")
            'output.Append(sba.Idf2)
            'output.Append("</Idf2>")
            'output.Append("<Idf3>")
            'output.Append(sba.Idf3)
            'output.Append("</Idf3>")
            'output.Append("<Idf4>")
            'output.Append(sba.Idf4)
            'output.Append("</Idf4>")
            'output.Append("<Idf5>")
            'output.Append(sba.Idf5)
            'output.Append("</Idf5>")
            'output.Append("<Idr>")
            'output.Append(sba.Idr)
            'output.Append("</Idr>")
            'output.Append("<Ifa>")
            'output.Append(sba.Ifa)
            'output.Append("</Ifa>")
            'output.Append("<Ifd>")
            'output.Append(sba.Ifd)
            'output.Append("</Ifd>")
            'output.Append("<Ii>")
            'output.Append(sba._line2(0).Ii)
            'output.Append("</Ii>")
            'output.Append("<Imw>")
            'output.Append(sba._line2(0).Imw)
            'output.Append("</Imw>")
            'output.Append("<Inps>")
            'output.Append(sba._line2(0).Inps)
            'output.Append("</Inps>")
            'output.Append("<Iops>")
            'output.Append(sba._line2(0).Iops)
            'output.Append("</Iops>")
            'output.Append("<Iow>")
            'output.Append(sba._line2(0).Iow)
            'output.Append("</Iow>")
            'output.Append("<Ipts>")
            'output.Append(sba._line2(0).Ipts)
            'output.Append("</Ipts>")
            'output.Append("<Iri>")
            'output.Append(sba.Iri)
            'output.Append("</Iri>")
            'output.Append("<Isao>")
            'output.Append(sba._line2(0).Isao)
            'output.Append("</Isao>")
            'output.Append("<Iwth>")
            'output.Append(sba._line2(0).Iwth)
            'output.Append("</Iwth>")
            'output.Append("<Lm>")
            'output.Append(sba.Lm)
            'output.Append("</Lm>")
            'output.Append("<Luns>")
            'output.Append(sba._line2(0).Luns)
            'output.Append("</Luns>")
            'output.Append("<Nirr>")
            'output.Append(sba.Nirr)
            'output.Append("</Nirr>")
            'output.Append("<Nvcn>")
            'output.Append(sba._line2(0).Nvcn)
            'output.Append("</Nvcn>")
            'output.Append("<Ny1>")
            'output.Append(sba.Ny1)
            'output.Append("</Ny1>")
            'output.Append("<Ny2>")
            'output.Append(sba.Ny2)
            'output.Append("</Ny2>")
            'output.Append("<Ny3>")
            'output.Append(sba.Ny3)
            'output.Append("</Ny3>")
            'output.Append("<Ny4>")
            'output.Append(sba.Ny4)
            'output.Append("</Ny4>")
            'output.Append("<Pcof>")
            'output.Append(sba.Pcof)
            'output.Append("</Pcof>")
            'output.Append("<Pec>")
            'output.Append(sba.Pec)
            'output.Append("</Pec>")
            'output.Append("<Rcbw>")
            'output.Append(sba.Rcbw)
            'output.Append("</Rcbw>")
            'output.Append("<Rchc>")
            'output.Append(sba.Rchc)
            'output.Append("</Rchc>")
            'output.Append("<Rchd>")
            'output.Append(sba.Rchd)
            'output.Append("</Rchd>")
            'output.Append("<Rchk>")
            'output.Append(sba.Rchk)
            'output.Append("</Rchk>")
            'output.Append("<Rchl>")
            'output.Append(sba.Rchl)
            'output.Append("</Rchl>")
            'output.Append("<Rchn>")
            'output.Append(sba.Rchn)
            'output.Append("</Rchn>")
            'output.Append("<Rchs>")
            'output.Append(sba.Rchs)
            'output.Append("</Rchs>")
            'output.Append("<Rctw>")
            'output.Append(sba.Rctw)
            'output.Append("</Rctw>")
            'output.Append("<Rfpl>")
            'output.Append(sba.Rfpl)
            'output.Append("</Rfpl>")
            'output.Append("<Rfpw>")
            'output.Append(sba.Rfpw)
            'output.Append("</Rfpw>")
            'output.Append("<Rsae>")
            'output.Append(sba.Rsae)
            'output.Append("</Rsae>")
            'output.Append("<Rsap>")
            'output.Append(sba.Rsap)
            'output.Append("</Rsap>")
            'output.Append("<Rsbd>")
            'output.Append(sba.Rsbd)
            'output.Append("</Rsbd>")
            'output.Append("<Rsdp>")
            'output.Append(sba.Rsdp)
            'output.Append("</Rsdp>")
            'output.Append("<Rsee>")
            'output.Append(sba.Rsee)
            'output.Append("</Rsee>")
            'output.Append("<Rsep>")
            'output.Append(sba.Rsep)
            'output.Append("</Rsep>")
            'output.Append("<Rshc>")
            'output.Append(sba.Rshc)
            'output.Append("</Rshc>")
            'output.Append("<Rsrr>")
            'output.Append(sba.Rsrr)
            'output.Append("</Rsrr>")
            'output.Append("<Rsv>")
            'output.Append(sba.Rsv)
            'output.Append("</Rsv>")
            'output.Append("<Rsve>")
            'output.Append(sba.Rsve)
            'output.Append("</Rsve>")
            'output.Append("<Rsvp>")
            'output.Append(sba.Rsvp)
            'output.Append("</Rsvp>")
            'output.Append("<Rsyn>")
            'output.Append(sba.Rsyn)
            'output.Append("</Rsyn>")
            'output.Append("<Rsys>")
            'output.Append(sba.Rsys)
            'output.Append("</Rsys>")
            'output.Append("<SbaType>")
            'output.Append(sba.SbaType)
            'output.Append("</SbaType>")
            'output.Append("<Sflg>")
            'output.Append(sba.Sflg)
            'output.Append("</Sflg>")
            'output.Append("<Slp>")
            'output.Append(sba.Slp)
            'output.Append("</Slp>")
            'output.Append("<Slpg>")
            'output.Append(sba.Slpg)
            'output.Append("</Slpg>")
            'output.Append("<Sno>")
            'output.Append(sba.Sno)
            'output.Append("</Sno>")
            'output.Append("<SoilComponent>")
            'output.Append(sba.SoilComponent)
            'output.Append("</SoilComponent>")
            'output.Append("<SoilGroup>")
            'output.Append(sba.SoilGroup)
            'output.Append("</SoilGroup>")
            'output.Append("<SoilKey>")
            'output.Append(sba.SoilKey)
            'output.Append("</SoilKey>")
            'output.Append("<SoilSymbol>")
            'output.Append(sba.SoilSymbol)
            'output.Append("</SoilSymbol>")
            'output.Append("<Solq>")
            'output.Append(sba.Solq)
            'output.Append("</Solq>")
            'output.Append("<Stdo>")
            'output.Append(sba.Stdo)
            'output.Append("</Stdo>")
            'output.Append("<SubareaNumber>")
            'output.Append(sba.SubareaNumber)
            'output.Append("</SubareaNumber>")
            'output.Append("<SubareaTitle>")
            'output.Append(sba.SubareaTitle)
            'output.Append("</SubareaTitle>")
            'output.Append("<Upn>")
            'output.Append(sba.Upn)
            'output.Append("</Upn>")
            'output.Append("<Urbf>")
            'output.Append(sba.Urbf)
            'output.Append("</Urbf>")
            'output.Append("<Vimx>")
            'output.Append(sba.Vimx)
            'output.Append("</Vimx>")
            'output.Append("<Vlgn>")
            'output.Append(sba.Vlgn)
            'output.Append("</Vlgn>")
            'output.Append("<Wsa>")
            'output.Append(sba.Wsa)
            'output.Append("</Wsa>")
            'output.Append("<Xct>")
            'output.Append(sba.Xct)
            'output.Append("</Xct>")
            'output.Append("<Xtp1>")
            'output.Append(sba.Xtp1)
            'output.Append("</Xtp1>")
            'output.Append("<Xtp2>")
            'output.Append(sba.Xtp2)
            'output.Append("</Xtp2>")
            'output.Append("<Xtp3>")
            'output.Append(sba.Xtp3)
            'output.Append("</Xtp3>")
            'output.Append("<Xtp4>")
            'output.Append(sba.Xtp4)
            'output.Append("</Xtp4>")
            'output.Append("<Yct>")
            'output.Append(sba.Yct)
            'output.Append("</Yct>")
            'output.Append("</Subareas>" & vbCr)
        Next
    End Sub

    Private Sub AddBMPsInfo(ByRef bmpsInfo As BmpsData, ByRef output As StringBuilder)
        output.Append("<Bmps>" & vbCr)
        output.Append("<AFEff>")
        output.Append(bmpsInfo.AFEff)
        output.Append("</AFEff>")
        output.Append("<AFFreq>")
        output.Append(bmpsInfo.AFFreq)
        output.Append("</AFFreq>")
        output.Append("<AFMaxSingleApp>")
        output.Append(bmpsInfo.AFMaxSingleApp)
        output.Append("</AFMaxSingleApp>")
        output.Append("<AFType>")
        output.Append(bmpsInfo.AFType)
        output.Append("</AFType>")
        output.Append("<AFWaterStressFactor>")
        output.Append(bmpsInfo.AFWaterStressFactor)
        output.Append("</AFWaterStressFactor>")
        output.Append("<AFSafetyFactor>")
        output.Append(bmpsInfo.AFSafetyFactor)
        output.Append("</AFSafetyFactor>")
        output.Append("<AFNConc>")
        output.Append(bmpsInfo.AFNConc)
        output.Append("</AFNConc>")
        output.Append("<AIEff>")
        output.Append(bmpsInfo.AIEff)
        output.Append("</AIEff>")
        output.Append("<AIFreq>")
        output.Append(bmpsInfo.AIFreq)
        output.Append("</AIFreq>")
        output.Append("<AIMaxSingleApp>")
        output.Append(bmpsInfo.AIMaxSingleApp)
        output.Append("</AIMaxSingleApp>")
        output.Append("<AIType>")
        output.Append(bmpsInfo.AIType)
        output.Append("</AIType>")
        output.Append("<AIWaterStressFactor>")
        output.Append(bmpsInfo.AIWaterStressFactor)
        output.Append("</AIWaterStressFactor>")
        output.Append("<AISafetyFactor>")
        output.Append(bmpsInfo.AISafetyFactor)
        output.Append("</AISafetyFactor>")
        output.Append("<FSArea>")
        output.Append(bmpsInfo.FSArea)
        output.Append("</FSArea>")
        output.Append("<FSCrop>")
        output.Append(bmpsInfo.FSCrop)
        output.Append("</FSCrop>")
        output.Append("<FSEff>")
        output.Append(bmpsInfo.FSEff)
        output.Append("</FSEff>")
        output.Append("<FSslopeRatio>")
        output.Append(bmpsInfo.FSslopeRatio)
        output.Append("</FSslopeRatio>")
        output.Append("<FSWidth>")
        output.Append(bmpsInfo.FSWidth)
        output.Append("</FSWidth>")
        output.Append("<SdgArea>")
        output.Append(bmpsInfo.SdgArea)
        output.Append("</SdgArea>")
        output.Append("<SdgCrop>")
        output.Append(bmpsInfo.SdgCrop)
        output.Append("</SdgCrop>")
        output.Append("<SdgEff>")
        output.Append(bmpsInfo.SdgEff)
        output.Append("</SdgEff>")
        output.Append("<SdgslopeRatio>")
        output.Append(bmpsInfo.SdgslopeRatio)
        output.Append("</SdgslopeRatio>")
        output.Append("<SdgWidth>")
        output.Append(bmpsInfo.SdgWidth)
        output.Append("</SdgWidth>")
        output.Append("<Lm>")
        output.Append(bmpsInfo.Lm)
        output.Append("</Lm>")
        output.Append("<PndF>")
        output.Append(bmpsInfo.PndF)
        output.Append("</PndF>")
        output.Append("<RFArea>")
        output.Append(bmpsInfo.RFArea)
        output.Append("</RFArea>")
        output.Append("<RFEff>")
        output.Append(bmpsInfo.RFEff)
        output.Append("</RFEff>")
        output.Append("<RFGrassFieldPortion>")
        output.Append(bmpsInfo.RFGrassFieldPortion)
        output.Append("</RFGrassFieldPortion>")
        output.Append("<RFslopeRatio>")
        output.Append(bmpsInfo.RFslopeRatio)
        output.Append("</RFslopeRatio>")
        output.Append("<RFWidth>")
        output.Append(bmpsInfo.RFWidth)
        output.Append("</RFWidth>")
        output.Append("<Sbs>")
        output.Append(bmpsInfo.Sbs)
        output.Append("</Sbs>")
        output.Append("<SlopeRed>")
        output.Append(bmpsInfo.SlopeRed)
        output.Append("</SlopeRed>")
        output.Append("<TileDrainDepth>")
        output.Append(bmpsInfo.TileDrainDepth)
        output.Append("</TileDrainDepth>")
        output.Append("<Ts>")
        output.Append(bmpsInfo.Ts)
        output.Append("</Ts>")
        output.Append("<WLArea>")
        output.Append(bmpsInfo.WLArea)
        output.Append("</WLArea>")
        output.Append("<WWCrop>")
        output.Append(bmpsInfo.WWCrop)
        output.Append("</WWCrop>")
        output.Append("<WWWidth>")
        output.Append(bmpsInfo.WWWidth)
        output.Append("</WWWidth>")
        output.Append("<PPNDWidth>")
        output.Append(bmpsInfo.PPNDWidth)
        output.Append("</PPNDWidth>")
        output.Append("<PPDSWidth>")
        output.Append(bmpsInfo.PPDSWidth)
        output.Append("</PPDSWidth>")
        output.Append("<PPDEWidth>")
        output.Append(bmpsInfo.PPDEWidth)
        output.Append("</PPDEWidth>")
        output.Append("<PPTWWidth>")
        output.Append(bmpsInfo.PPTWWidth)
        output.Append("</PPTWWidth>")
        output.Append("<PPNDSides>")
        output.Append(bmpsInfo.PPNDSides)
        output.Append("</PPNDSides>")
        output.Append("<PPDSSides>")
        output.Append(bmpsInfo.PPDSSides)
        output.Append("</PPDSSides>")
        output.Append("<PPDESides>")
        output.Append(bmpsInfo.PPDESides)
        output.Append("</PPDESides>")
        output.Append("<PPTWSides>")
        output.Append(bmpsInfo.PPTWSides)
        output.Append("</PPTWSides>")
        output.Append("<PPDEResArea>")
        output.Append(bmpsInfo.PPDEResArea)
        output.Append("</PPDEResArea>")
        output.Append("<PPTWResArea>")
        output.Append(bmpsInfo.PPTWResArea)
        output.Append("</PPTWResArea>")
        output.Append("<CBCrop>")
        output.Append(bmpsInfo.CBCrop)
        output.Append("</CBCrop>")
        output.Append("<CBBWidth>")
        output.Append(bmpsInfo.CBBWidth)
        output.Append("</CBBWidth>")
        output.Append("<CBCWidth>")
        output.Append(bmpsInfo.CBCWidth)
        output.Append("</CBCWidth>")
        output.Append("<SFAnimals>")
        output.Append(bmpsInfo.SFAnimals)
        output.Append("</SFAnimals>")
        output.Append("<SFCode>")
        output.Append(bmpsInfo.SFCode)
        output.Append("</SFCode>")
        output.Append("<SFDays>")
        output.Append(bmpsInfo.SFDays)
        output.Append("</SFDays>")
        output.Append("<SFDryManure>")
        output.Append(bmpsInfo.SFDryManure)
        output.Append("</SFDryManure>")
        output.Append("<SFHours>")
        output.Append(bmpsInfo.SFHours)
        output.Append("</SFHours>")
        output.Append("<SFName>")
        output.Append(bmpsInfo.SFName)
        output.Append("</SFName>")
        output.Append("<SFNo3>")
        output.Append(bmpsInfo.SFNo3)
        output.Append("</SFNo3>")
        output.Append("<SFOrgN>")
        output.Append(bmpsInfo.SFOrgN)
        output.Append("</SFOrgN>")
        output.Append("<SFOrgP>")
        output.Append(bmpsInfo.SFOrgP)
        output.Append("</SFOrgP>")
        output.Append("<SFPo4>")
        output.Append(bmpsInfo.SFPo4)
        output.Append("</SFPo4>")
        output.Append("<CcMaximumTeperature>")
        output.Append(bmpsInfo.CcMaximumTeperature)
        output.Append("</CcMaximumTeperature>")
        output.Append("<CcMinimumTeperature>")
        output.Append(bmpsInfo.CcMinimumTeperature)
        output.Append("</CcMinimumTeperature>")
        output.Append("<CcPrecipitation>")
        output.Append(bmpsInfo.CcPrecipitation)
        output.Append("</CcPrecipitation>")
        output.Append("</Bmps>" & vbCr)
    End Sub

    Private Sub AddFEMResultsInfo(ByRef FEMResultsInfo As FEMData, ByRef output As StringBuilder)
        output.Append("<FEMresults>" & vbCr)
        output.Append("<TotalRevenue>")
        output.Append(FEMResultsInfo.TotalRevenue)
        output.Append("</TotalRevenue>")
        output.Append("<TotalCost>")
        output.Append(FEMResultsInfo.TotalCost)
        output.Append("</TotalCost>")
        output.Append("<NetReturn>")
        output.Append(FEMResultsInfo.NetReturn)
        output.Append("</NetReturn>")
        output.Append("<NetCashFlow>")
        output.Append(FEMResultsInfo.NetCashFlow)
        output.Append("</NetCashFlow>")
        output.Append("</FEMresults>" & vbCr)
    End Sub

    Private Sub AddScenarioResultsInfo(ByRef ScenarioResultsInfo As ScenariosData.APEXResults, ByRef output As StringBuilder)
        output.Append("<Results>" & vbCr)
        output.Append("<area>")
        output.Append(ScenarioResultsInfo.area)
        output.Append("</area>")
        output.Append("<lastSimulation>")
        output.Append(ScenarioResultsInfo.lastSimulation)
        output.Append("</lastSimulation>")
        output.Append("<Crops>")
        If Not ScenarioResultsInfo.SoilResults.Crops.cropName Is Nothing Then
            For Each crop In ScenarioResultsInfo.SoilResults.Crops.cropName
                output.Append("<cropName>")
                output.Append(crop)
                output.Append("</cropName>")
            Next
        End If
        If Not ScenarioResultsInfo.SoilResults.Crops.cropRecords Is Nothing Then
            For Each crop In ScenarioResultsInfo.SoilResults.Crops.cropRecords
                output.Append("<cropRecord>")
                output.Append(crop)
                output.Append("</cropRecord>")
            Next
        End If
        If Not ScenarioResultsInfo.SoilResults.Crops.cropYield Is Nothing Then
            For Each crop In ScenarioResultsInfo.SoilResults.Crops.cropYield
                output.Append("<cropYield>")
                output.Append(crop)
                output.Append("</cropYield>")
            Next
        End If
        If Not ScenarioResultsInfo.SoilResults.Crops.cropYieldCI Is Nothing Then
            For Each crop In ScenarioResultsInfo.SoilResults.Crops.cropYieldCI
                output.Append("<cropYieldCI>")
                output.Append(crop)
                output.Append("</cropYieldCI>")
            Next
        End If
        If Not ScenarioResultsInfo.SoilResults.Crops.cropYieldSD Is Nothing Then
            For Each crop In ScenarioResultsInfo.SoilResults.Crops.cropYieldSD
                output.Append("<cropYieldSD>")
                output.Append(crop)
                output.Append("</cropYieldSD>")
            Next
        End If
        If Not ScenarioResultsInfo.SoilResults.Crops.ns Is Nothing Then
            For Each crop In ScenarioResultsInfo.SoilResults.Crops.ns
                output.Append("<cropns>")
                output.Append(crop)
                output.Append("</cropns>")
            Next
        End If
        If Not ScenarioResultsInfo.SoilResults.Crops.ps Is Nothing Then
            For Each crop In ScenarioResultsInfo.SoilResults.Crops.ps
                output.Append("<cropps>")
                output.Append(crop)
                output.Append("</cropps>")
            Next
        End If
        If Not ScenarioResultsInfo.SoilResults.Crops.ts Is Nothing Then
            For Each crop In ScenarioResultsInfo.SoilResults.Crops.ts
                output.Append("<cropts>")
                output.Append(crop)
                output.Append("</cropts>")
            Next
        End If
        If Not ScenarioResultsInfo.SoilResults.Crops.ws Is Nothing Then
            For Each crop In ScenarioResultsInfo.SoilResults.Crops.ws
                output.Append("<cropws>")
                output.Append(crop)
                output.Append("</cropws>")
            Next
        End If
        output.Append("</Crops>")
        output.Append("<FIFertilizer>")
        output.Append(ScenarioResultsInfo.FIFertilizer)
        output.Append("</FIFertilizer>")
        output.Append("<i>")
        output.Append(ScenarioResultsInfo.i)
        output.Append("</i>")
        '***** SoilsResults information ****
        output.Append("<Soil>")
        output.Append("<co2>")
        output.Append(ScenarioResultsInfo.SoilResults.co2)
        output.Append("</co2>")
        output.Append("<deepPerFlow>")
        output.Append(ScenarioResultsInfo.SoilResults.deepPerFlow)
        output.Append("</deepPerFlow>")
        output.Append("<deepPerFlowCI>")
        output.Append(ScenarioResultsInfo.SoilResults.deepPerFlowCI)
        output.Append("</deepPerFlowCI>")
        output.Append("<runoff>")
        output.Append(ScenarioResultsInfo.SoilResults.runoff)
        output.Append("</runoff>")
        output.Append("<runoffCI>")
        output.Append(ScenarioResultsInfo.SoilResults.runoffCI)
        output.Append("</runoffCI>")
        output.Append("<runoffSD>")
        output.Append(ScenarioResultsInfo.SoilResults.runoffSD)
        output.Append("</runoffSD>")
        output.Append("<subsurfaceFlow>")
        output.Append(ScenarioResultsInfo.SoilResults.subsurfaceFlow)
        output.Append("</subsurfaceFlow>")
        output.Append("<subsurfaceFlowCI>")
        output.Append(ScenarioResultsInfo.SoilResults.subsurfaceFlowCI)
        output.Append("</subsurfaceFlowCI>")
        output.Append("<subsurfaceFlowSD>")
        output.Append(ScenarioResultsInfo.SoilResults.subsurfaceFlowSD)
        output.Append("</subsurfaceFlowSD>")
        output.Append("<irrigation>")
        output.Append(ScenarioResultsInfo.SoilResults.irrigation)
        output.Append("</irrigation>")
        output.Append("<irrigationCI>")
        output.Append(ScenarioResultsInfo.SoilResults.irrigationCI)
        output.Append("</irrigationCI>")
        output.Append("<LeachedN>")
        output.Append(ScenarioResultsInfo.SoilResults.LeachedN)
        output.Append("</LeachedN>")
        output.Append("<LeachedP>")
        output.Append(ScenarioResultsInfo.SoilResults.LeachedP)
        output.Append("</LeachedP>")
        output.Append("<n2o>")
        output.Append(ScenarioResultsInfo.SoilResults.n2o)
        output.Append("</n2o>")
        output.Append("<runoffN>")
        output.Append(ScenarioResultsInfo.SoilResults.RunoffN)
        output.Append("</runoffN>")
        output.Append("<runoffNCI>")
        output.Append(ScenarioResultsInfo.SoilResults.runoffNCI)
        output.Append("</runoffNCI>")
        output.Append("<runoffNSD>")
        output.Append(ScenarioResultsInfo.SoilResults.runoffNSD)
        output.Append("</runoffNSD>")
        output.Append("<subsurfaceN>")
        output.Append(ScenarioResultsInfo.SoilResults.SubsurfaceN)
        output.Append("</subsurfaceN>")
        output.Append("<subsurfaceNCI>")
        output.Append(ScenarioResultsInfo.SoilResults.subsurfaceNCI)
        output.Append("</subsurfaceNCI>")
        output.Append("<subsurfaceNSD>")
        output.Append(ScenarioResultsInfo.SoilResults.subsurfaceNSD)
        output.Append("</subsurfaceNSD>")
        output.Append("<OrgN>")
        output.Append(ScenarioResultsInfo.SoilResults.OrgN)
        output.Append("</OrgN>")
        output.Append("<OrgNCI>")
        output.Append(ScenarioResultsInfo.SoilResults.OrgNCI)
        output.Append("</OrgNCI>")
        output.Append("<OrgNSD>")
        output.Append(ScenarioResultsInfo.SoilResults.OrgNSD)
        output.Append("</OrgNSD>")
        output.Append("<OrgP>")
        output.Append(ScenarioResultsInfo.SoilResults.OrgP)
        output.Append("</OrgP>")
        output.Append("<OrgPCI>")
        output.Append(ScenarioResultsInfo.SoilResults.OrgPCI)
        output.Append("</OrgPCI>")
        output.Append("<OrgPSD>")
        output.Append(ScenarioResultsInfo.SoilResults.OrgPSD)
        output.Append("</OrgPSD>")
        output.Append("<PO4>")
        output.Append(ScenarioResultsInfo.SoilResults.PO4)
        output.Append("</PO4>")
        output.Append("<PO4CI>")
        output.Append(ScenarioResultsInfo.SoilResults.PO4CI)
        output.Append("</PO4CI>")
        output.Append("<PO4SD>")
        output.Append(ScenarioResultsInfo.SoilResults.PO4SD)
        output.Append("</PO4SD>")
        output.Append("<Sediment>")
        output.Append(ScenarioResultsInfo.SoilResults.Sediment)
        output.Append("</Sediment>")
        output.Append("<SedimentCI>")
        output.Append(ScenarioResultsInfo.SoilResults.SedimentCI)
        output.Append("</SedimentCI>")
        output.Append("<SedimentSD>")
        output.Append(ScenarioResultsInfo.SoilResults.SedimentSD)
        output.Append("</SedimentSD>")
        output.Append("<tileDrainFlow>")
        output.Append(ScenarioResultsInfo.SoilResults.tileDrainFlow)
        output.Append("</tileDrainFlow>")
        output.Append("<tileDrainFlowCI>")
        output.Append(ScenarioResultsInfo.SoilResults.tileDrainFlowCI)
        output.Append("</tileDrainFlowCI>")
        output.Append("<tileDrainFlowSD>")
        output.Append(ScenarioResultsInfo.SoilResults.tileDrainFlowSD)
        output.Append("</tileDrainFlowSD>")
        output.Append("<tileDrainN>")
        output.Append(ScenarioResultsInfo.SoilResults.tileDrainN)
        output.Append("</tileDrainN>")
        output.Append("<tileDrainNCI>")
        output.Append(ScenarioResultsInfo.SoilResults.tileDrainNCI)
        output.Append("</tileDrainNCI>")
        output.Append("<tileDrainNSD>")
        output.Append(ScenarioResultsInfo.SoilResults.tileDrainNSD)
        output.Append("</tileDrainNSD>")
        output.Append("<tileDrainP>")
        output.Append(ScenarioResultsInfo.SoilResults.tileDrainP)
        output.Append("</tileDrainP>")
        output.Append("<tileDrainPCI>")
        output.Append(ScenarioResultsInfo.SoilResults.tileDrainPCI)
        output.Append("</tileDrainPCI>")
        output.Append("<tileDrainPSD>")
        output.Append(ScenarioResultsInfo.SoilResults.tileDrainPSD)
        output.Append("</tileDrainPSD>")
        output.Append("<volatizationN>")
        output.Append(ScenarioResultsInfo.SoilResults.volatizationN)
        output.Append("</volatizationN>")
        'Add annual and monthly values for graphs.
        If Not ScenarioResultsInfo.annualFlow Is Nothing Then
            output.Append("<annualFlow>")
            For Each value In ScenarioResultsInfo.annualFlow
                output.Append("<flow>")
                output.Append(value)
                output.Append("</flow>")
            Next
            output.Append("</annualFlow>")
        End If
        If Not ScenarioResultsInfo.annualNO3 Is Nothing Then
            output.Append("<annualNO3>")
            For Each value In ScenarioResultsInfo.annualNO3
                output.Append("<NO3>")
                output.Append(value)
                output.Append("</NO3>")
            Next
            output.Append("</annualNO3>")
        End If
        If Not ScenarioResultsInfo.annualOrgN Is Nothing Then
            output.Append("<annualOrgN>")
            For Each value In ScenarioResultsInfo.annualOrgN
                output.Append("<OrgN>")
                output.Append(value)
                output.Append("</OrgN>")
            Next
            output.Append("</annualOrgN>")
        End If
        If Not ScenarioResultsInfo.annualN2o Is Nothing Then
            output.Append("<annualN2O>")
            For Each value In ScenarioResultsInfo.annualN2o
                output.Append("<N2O>")
                output.Append(value)
                output.Append("</N2O>")
            Next
            output.Append("</annualN2O>")
        End If
        If Not ScenarioResultsInfo.annualOrgP Is Nothing Then
            output.Append("<annualOrgP>")
            For Each value In ScenarioResultsInfo.annualOrgP
                output.Append("<OrgP>")
                output.Append(value)
                output.Append("</OrgP>")
            Next
            output.Append("</annualOrgP>")
        End If
        If Not ScenarioResultsInfo.annualPO4 Is Nothing Then
            output.Append("<annualPO4>")
            For Each value In ScenarioResultsInfo.annualPO4
                output.Append("<PO4>")
                output.Append(value)
                output.Append("</PO4>")
            Next
            output.Append("</annualPO4>")
        End If
        If Not ScenarioResultsInfo.annualSediment Is Nothing Then
            output.Append("<annualSediment>")
            For Each value In ScenarioResultsInfo.annualSediment
                output.Append("<Sediment>")
                output.Append(value)
                output.Append("</Sediment>")
            Next
            output.Append("</annualSediment>")
        End If
        If Not ScenarioResultsInfo.annualPrecipitation Is Nothing Then
            output.Append("<annualPrecipitation>")
            For Each value In ScenarioResultsInfo.annualPrecipitation
                output.Append("<precipitation>")
                output.Append(value)
                output.Append("</precipitation>")
            Next
            output.Append("</annualPrecipitation>")
        End If
        If Not ScenarioResultsInfo.annualCropYield Is Nothing Then
            output.Append("<annualCropYield>")
            For Each value In ScenarioResultsInfo.annualCropYield
                output.Append("<crop>")
                If Not value.cropName Is Nothing Then
                    For Each value1 In value.cropName
                        output.Append("<cropName>")
                        output.Append(value1)
                        output.Append("</cropName>")
                    Next
                    For Each value1 In value.cropYield
                        output.Append("<cropYield>")
                        output.Append(value1)
                        output.Append("</cropYield>")
                    Next
                End If
                output.Append("</crop>")
            Next
            output.Append("</annualCropYield>")
        End If
        'end annual results for graphs.
        output.Append("</Soil>")
        output.Append("</Results>" & vbCr)
        '***** End Results information ****
    End Sub

    Public Function LoadInfo(file As String, ByRef _projects As ProjectsData) As String
        Dim contentLen As Integer = 0
        'textDialog1 = New OpenFileDialog
        Dim doc As System.Xml.Linq.XDocument = Nothing
        Dim i As UShort
        Dim fileStream As System.IO.Stream = Nothing

        Try
            doc = System.Xml.Linq.XDocument.Load(file)
            'clean the FEM equipment list.
            _projects._equipmentTemp.Clear()
            '_equipmentTemp.Clear()
            For Each equip In doc.Descendants("EquipmentInfo")
                LoadFEMEquipmentInfo(equip, _projects._equipmentTemp)
            Next

            'clean the FEM feed list.
            _projects._feedTemp.Clear()
            '_feedTemp.Clear()
            For Each feed In doc.Descendants("FeedInfo")
                LoadFEMFeedInfo(feed, _projects._feedTemp)
            Next

            ''clean the FEM structure list.
            _projects._structureTemp.Clear()
            '_structureTemp.Clear()
            For Each struct In doc.Descendants("StructureInfo")
                LoadFEMStructureInfo(struct, _projects._structureTemp)
            Next

            ''clean the FEM other input list.
            _projects._otherTemp.Clear()
            '_otherTemp.Clear()
            For Each other In doc.Descendants("OtherInputInfo")
                LoadFEMOtherInfo(other, _projects._otherTemp)
            Next

            For Each Info In doc.Descendants("StartInfo")
                LoadStartInfo(Info, _projects._StartInfo)
            Next

            Dim management As New Management
            Dim _crops As New List(Of CropsData)
            management.LoadCrops(_crops, _projects._StartInfo.stateAbrev)

            For Each Info In doc.Descendants("FarmInfo")
                LoadFarmInfo(Info, _projects._StartInfo)
            Next

            _projects._controlValues.Clear()
            For Each value In doc.Descendants("ControlValues")
                loadControlInfo(value, _projects._controlValues)
            Next

            _projects._parmValues.Clear()
            For Each value In doc.Descendants("ParmValues")
                loadParmInfo(value, _projects._parmValues)
            Next
            GetMissingParms(_projects._parmValues, _projects._StartInfo.StateAbrev, doc.Descendants("ParmValues").Count)

            For Each value In doc.Descendants("SiteInfo")
                _projects._sitesInfo.Clear()
                loadSiteInfo(value, _projects._sitesInfo, _projects._StartInfo)
            Next

            _projects._subprojectName.Clear()
            For Each item In doc.Descendants("SubprojectName")
                loadSubprojectNameInfo(item, _projects._subprojectName)
            Next
            i = 0
            _projects._fieldsInfo1.Clear()
            For Each Info In doc.Descendants("FieldInfo")
                i += 1
                loadFieldInfo(Info, _projects._fieldsInfo1)
            Next

            Return "OK"

        Catch e1 As Exception
            Return e1.Message
        End Try
    End Function

    Public Function CreateXMLFile(ByRef output As StringBuilder, ByRef _projects As ProjectsData) As String
        Dim message As String = "OK"
        Try
            'If _StartInfo.projectName Is Nothing Or _StartInfo.projectName = String.Empty Then
            'Return msgDoc.Descendants("ProjectNotCreated").Value ''showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("ProjectNotCreated").Value)
            'End If
            output.Length = 0
            output.Append("<?xml version='1.0'?>" & vbCr)
            output.Append("<Project>" & vbCr)
            message = AddStartInfo(output, _projects._StartInfo)
            AddFieldInfo(output, _projects._fieldsInfo1)
            message = AddFarmInfo(_projects._StartInfo)
            AddFEMEquipment(_projects._equipmentTemp)
            AddFEMFeed(_projects._feedTemp)
            AddFEMStructure(_projects._structureTemp)
            AddFEMInput(_projects._otherTemp)
            AddControlInfo(_projects._controlValues)
            AddParmInfo(_projects._parmValues)
            AddSiteInfo(_projects._sitesInfo)
            AddSubprojectInfo(_projects._subprojectName)
            output.Append("</Project>" & vbCr)

            Return message
        Catch e1 As Exception
            Return msgDoc.Descendants("Errors").Value & e1.Message ''showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & e1.Message)
        End Try
    End Function


End Module
