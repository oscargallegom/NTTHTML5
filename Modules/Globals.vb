
Module Globals
    'Grazing
    Public biomassLimit As Single = 0.005      'minimun plant material (t/ha) that must be present for grazing to occur
    'Private dc As New NTTDBDataContext
    'classes variables constant names
    Public tStartInfo As String = "StartInfo"
    Public tFieldInfo As String = "fieldInfo"
    Public tSoilInfo As String = "soilInfo"
    Public tScenarioInfo As String = "scenarioInfo"
    'General variables
    Public sessionNumber As Integer = 0
    Public mainFolder As String = My.Computer.FileSystem.CurrentDirectory
    Public folder As String = String.Empty
    Public Const NTTDefaultXMLFile As String = "NTTXMLTemplate.xml"
    Public Const NTTxmlFolder As String = "App_xml"
    Public Const selectOne As String = "Select One"
    Private _NTTFilesFolder As String = ":\NTTHTML5Files\"
    Private _NTTZipFolder As String = ":\NTTSLZip\"
    Private _weatherPrismFiles As String = ":\Weather\weatherFiles\US"
    Private _weatherStationFiles As String = ":\Weather\Climate2006"
    Private _wp1Files As String = ":\Weather\wp1File"
    Private _windFiles As String = ":\Weather\wndFile"
    Private _drive As String = "E:"
    Public _crops As New List(Of CropsData)

    ReadOnly Property ConnectionString(sAPEXBasLoc As String, table As String) As String
        Get
            If System.Net.Dns.GetHostName = "T-NN" Then
                Return "PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source= " & sAPEXBasLoc & table
            Else
                Return "PROVIDER=Microsoft.ACE.OLEDB.12.0;Data Source= " & sAPEXBasLoc & table
            End If
        End Get
    End Property
    ReadOnly Property Drive() As String
        Get
            If System.Net.Dns.GetHostName = "T-NN" Then
                Return "E:"
            Else
                Return "C:"
            End If
        End Get
    End Property

    ReadOnly Property NTTFilesFolder() As String
        Get
            If System.Net.Dns.GetHostName = "T-NN" Then
                Return "E" & _NTTFilesFolder
            Else
                Return "C" & _NTTFilesFolder
            End If
        End Get
    End Property
    ReadOnly Property NTTZipFolder() As String
        Get
            If System.Net.Dns.GetHostName = "T-NN" Then
                Return "E" & _NTTZipFolder
            Else
                Return "C" & _NTTZipFolder
            End If
        End Get
    End Property
    ReadOnly Property weatherPrismFiles() As String
        Get
            If System.Net.Dns.GetHostName = "T-NN" Then
                Return "E" & _weatherPrismFiles
            Else
                Return "E" & _weatherPrismFiles
            End If
        End Get
    End Property
    ReadOnly Property weatherStationFiles() As String
        Get
            If System.Net.Dns.GetHostName = "T-NN" Then
                Return "E" & _weatherStationFiles
            Else
                Return "E" & _weatherStationFiles
            End If
        End Get
    End Property
    ReadOnly Property wp1Files() As String
        Get
            If System.Net.Dns.GetHostName = "T-NN" Then
                Return "E" & _wp1Files
            Else
                Return "C" & _wp1Files
            End If
        End Get
    End Property

    ReadOnly Property windFiles() As String
        Get
            If System.Net.Dns.GetHostName = "T-NN" Then
                Return "E" & _windFiles
            Else
                Return "C" & _windFiles
            End If
        End Get
    End Property

    Public Const version As String = "042015"
    'main parms
    Public Const stateCounty As String = "State/County"
    Public Const googleMaps As String = "googleMap"
    Public Const userInput As String = "userInput"
    Public Const station As String = "Station"
    Public Const prism As String = "Prism"
    Public Const own As String = "Own"
    Public Const coordinates As String = "Coordinates"
    'convertions
    Public Const kg_to_lbs = 2.204622621849, mm_to_in = 0.03937007874
    Public Const tha_to_tac = 0.446   'http://bioenergy.ornl.gov/papers/misc/energy_conv.html
    Public Const ha_to_ac As Single = 2.471053814672
    'lenguage variables
    Public msgDoc As System.Xml.Linq.XDocument = Nothing        'XML Messages document
    Public cntDoc As System.Xml.Linq.XDocument = Nothing        'XML content docuement
    Public aplDoc As System.Xml.Linq.XDocument = Nothing        'Titles and instructinos docuement
    Public Const spanish As String = "Español"
    Public Const english As String = "English"
    Public Const portuguese As String = "Português"
    'Public strLanguage As String = "English"   'use to keep track of the current lenguage selected.  
    'End Structure
    Public Const road As String = "_Road", smz As String = "_SMZ"
    'Soils globals
    Public Const soilDBConnection = "Data Source=T-NN1\SQLEXPRESS;Initial Catalog=SSURGOSOILDB2014;User ID=sa;Password=pass$word"   'surgo soil connection
    Public Const degrees_to_radians = 0.0174532925
    Public Const BDMin As Single = 1.1
    Public Const BDMax As Single = 1.79
    Public Const SandMin As Integer = 0
    Public Const SandMax As Integer = 100
    Public Const SiltMin As Integer = 0
    Public Const SiltMax As Integer = 100
    Public Const SoilPMin As Single = 0.0
    Public Const SoilPMax As Single = 500
    Public Const SoilPDefault As Single = 3.0   'change from 0.1 to 5.0 according to Ali on 1/4/17
    Public Const SoilPMaxForSoilDepth As Single = 15.24
    Public Const OCMin As Single = 0.0      'change from 0.5 to 0 according to Ali. APEX calculate it if 0.
    Public Const OCMax As Single = 2.5
    Public Const OMDefault As Single = 0  'change from 2.5 to 0 according to Ali. APEX calculate it if 0.
    Public Const OMMin As Single = 0     'change from 0.86 to 0 according to Ali. APEX calculate it if 0.
    Public Const OMMax As Single = 4.3
    Public Const PHDefault As Single = 7.0
    Public Const PHMin As Single = 3.5
    Public Const PHMax As Single = 9.0
    Public Const armxDefault As Single = 3.0
    Public Const waterStress As Single = 0.8
    Public Const vimxDefault As Single = 5000
    Public Const drt As Single = 2
    Public Const AlbedoMin As Single = 0
    Public Const AlbedoMax As Single = 1
    'Scenarios globals
    Public _irrigationType As New List(Of IrrigationTypes)   'irrigationtype1 plus pads and pipes irrigation
    Public _irrigationType1 As New List(Of IrrigationTypes)  'regular irrigation types
    Public _bufferCrops As List(Of CropsData) = New List(Of CropsData)
    'start information globals
    Public tMax() As Single
    Public tMin() As Single
    'operations globals
    Public _operations As List(Of CodeAndName)
    Public _days As List(Of CodeAndName)
    Public _months As List(Of CodeAndName)
    Public _years As List(Of CodeAndName)
    Public _opTypes As List(Of AnimalUnitsData)
    Public _ferts As List(Of AnimalUnitsData)
    Public _monthsEnglish() As String = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}
    Public _monthsSpanish() As String = {"Ene", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"}
    Public Const cropRoad As UShort = 300
    Public Const cropMixedGrass As UShort = 367
    'BMPs globals
    Public Const maxStrips = 6  'Max number of strip sets allow when contour buffer bmp is selected.

    Public Sub InitializeLists()
        _operations = New List(Of CodeAndName)
        _days = New List(Of CodeAndName)
        _months = New List(Of CodeAndName)
        _years = New List(Of CodeAndName)
        _opTypes = New List(Of AnimalUnitsData)
        _ferts = New List(Of AnimalUnitsData)
    End Sub

    Public Sub LoadDays()
        Dim day As CodeAndName

        _days.Clear()
        For i = 0 To 31
            day = New CodeAndName
            day.Name = i
            If i = 0 Then
                day.Name = cntDoc.Descendants("SelectOne").Value
            End If
            day.Code = i
            _days.Add(day)
        Next
    End Sub

    Public Sub LoadMonths()
        _months.Clear()
        Dim smonth As String = String.Empty
        Dim month As CodeAndName
        For i = 0 To 12
            month = New CodeAndName
            If System.Web.HttpContext.Current.Session("Language") <> spanish Then
                If i = 0 Then
                    smonth = cntDoc.Descendants("SelectOne").Value
                Else
                    smonth = Globals._monthsEnglish(i - 1)
                End If
            Else
                If i = 0 Then
                    smonth = cntDoc.Descendants("SelectOne").Value
                Else
                    smonth = Globals._monthsSpanish(i - 1)
                End If
            End If
            month.Name = smonth
            month.Code = i
            _months.Add(month)
        Next
    End Sub

    Public Sub LoadYears(ByRef _StartInfo As StartInfo)
        _years.Clear()
        Dim year As CodeAndName
        For i = 0 To _StartInfo.stationYears
            year = New CodeAndName
            year.Name = i
            If i = 0 Then
                year.Name = cntDoc.Descendants("SelectOne").Value
            End If
            year.Code = i
            _years.Add(year)
        Next
    End Sub

    'Operation Information
    Public Const planting As String = "PLNT"
    Public Const fertilizer As String = "NUTC"
    Public Const irrigation As String = "IRRI"
    Public Const grazing As String = "GRAZ"
    Public Const harvest As String = "HARV"
    Public Const tillage As String = "TILL"
    Public Const kill As String = "KILL"
    Public Const stopGrazing As String = "STOP"
    Public Const burn As String = "BURN"
    Public Const liming As String = "LIME"
    'convertion constants
    Public Const om_to_oc As Single = 1.724
    Public Const ac_to_m2 As Single = 4046.8564224
    Public Const ac_to_ha As Single = 0.40468564224
    Public Const ac_to_km2 As Single = 0.0040468564224
    Public Const ha_to_m2 As Single = 10000
    Public Const km_to_m As Single = 1000
    Public Const in_to_mm As Single = 25.4
    Public Const lbs_to_kg As Single = 0.453592
    Public Const ft_to_m As Single = 0.3048
    Public Const ft_to_km As Single = 0.0003048
    Public Const ft_to_mm As Single = 304.8
    Public Const in_to_cm As Single = 2.54
    'animals information
    Public _animals As List(Of AnimalUnitsData) = New List(Of AnimalUnitsData)

    Public Sub openXMLLanguagesFile()
        Select Case System.Web.HttpContext.Current.Session("Language")
            Case spanish
                msgDoc = System.Xml.Linq.XDocument.Load(folder & "/Resources/MessagesSpanish.xml")
                cntDoc = System.Xml.Linq.XDocument.Load(folder & "/Resources/ContentSpanish.xml")
                aplDoc = System.Xml.Linq.XDocument.Load(folder & "/Resources/ApplicationStringsSpanish.xml")
            Case portuguese
                msgDoc = System.Xml.Linq.XDocument.Load(folder & "/Resources/MessagesPortuguese.xml")
                cntDoc = System.Xml.Linq.XDocument.Load(folder & "/Resources/ContentPortuguese.xml")
                aplDoc = System.Xml.Linq.XDocument.Load(folder & "/Resources/ApplicationStringsPortuguese.xml")
            Case Else  'this is for english or any othor case. Meaning it is the default
                msgDoc = System.Xml.Linq.XDocument.Load(folder & "/Resources/MessagesEnglish.xml")
                cntDoc = System.Xml.Linq.XDocument.Load(folder & "/Resources/ContentEnglish.xml")
                aplDoc = System.Xml.Linq.XDocument.Load(folder & "/Resources/ApplicationStringsEnglish.xml")
        End Select
    End Sub

    Public Function createSummaryText(ByVal page As String) As String
        If System.Web.HttpContext.Current.Session("projects") Is Nothing Then Return ""
        Dim projectName As String = "" : Dim countyName As String = "" : Dim stateName As String = ""
        Dim strName, strCounty, strState, strScenario, strField, strSoil As String
        Dim currentSoilNumber As UShort = 0 : Dim currentFieldNumber As Short = 0 : Dim currentScenarioNumber As UShort = 0
        Dim _fieldsInfo1 As List(Of FieldsData) = New List(Of FieldsData)

        projectName = System.Web.HttpContext.Current.Session("projects")._startInfo.projectName
        countyName = System.Web.HttpContext.Current.Session("projects")._startInfo.countyName
        stateName = System.Web.HttpContext.Current.Session("projects")._startInfo.stateName
        currentFieldNumber = System.Web.HttpContext.Current.Session("currentFieldNumber")
        If System.Web.HttpContext.Current.Session("currentScenarioNumber") > 0 Then currentScenarioNumber = System.Web.HttpContext.Current.Session("currentScenarioNumber")
        currentSoilNumber = System.Web.HttpContext.Current.Session("currentSoilNumber")
        _fieldsInfo1 = System.Web.HttpContext.Current.Session("projects")._fieldsInfo1

        strName = cntDoc.Descendants("ProjectName").Value : strCounty = cntDoc.Descendants("County").Value : strState = cntDoc.Descendants("State").Value : strScenario = cntDoc.Descendants("Scenario").Value : strField = "Field" : strSoil = cntDoc.Descendants("Soil").Value
        createSummaryText = String.Empty

        If Not projectName Is Nothing Then
            createSummaryText &= strName & ": " & projectName & " "
        End If
        If Not countyName Is Nothing AndAlso Not countyName Is String.Empty Then
            createSummaryText &= strCounty & ": " & countyName.Trim & " "
        End If
        If Not stateName Is Nothing AndAlso Not stateName Is String.Empty Then
            createSummaryText &= strState & ": " & stateName.Trim & " "
        End If
        If page = "soil" AndAlso _fieldsInfo1.Count > 0 Then
            If currentFieldNumber < 0 Then currentFieldNumber = 0
            createSummaryText &= strField & ": " & _fieldsInfo1(currentFieldNumber).Name & " "
        End If
        If page = "layer" AndAlso _fieldsInfo1.Count > 0 AndAlso _fieldsInfo1(currentFieldNumber)._soilsInfo.Count > 0 Then
            If currentFieldNumber < 0 Then currentFieldNumber = 0
            createSummaryText &= strField & ": " & _fieldsInfo1(currentFieldNumber).Name & " "
            createSummaryText &= strSoil & ": " & _fieldsInfo1(currentFieldNumber)._soilsInfo(currentSoilNumber).Key & " "
        End If
        If page = "management" AndAlso _fieldsInfo1.Count > 0 AndAlso _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count > 0 Then
            If currentFieldNumber < 0 Then currentFieldNumber = 0
            createSummaryText &= strField & ": " & _fieldsInfo1(currentFieldNumber).Name & " "
            createSummaryText &= strScenario & ": " & _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber).Name & " "
        End If
        If page = "report" AndAlso _fieldsInfo1.Count > 0 AndAlso _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count > 0 Then
            If currentFieldNumber < 0 Then currentFieldNumber = 0
            createSummaryText &= strField & ": " & _fieldsInfo1(currentFieldNumber).Name & " "
            createSummaryText &= strScenario & ": " & _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber).Name & " "
        End If
        If page = "economics" AndAlso _fieldsInfo1.Count > 0 AndAlso _fieldsInfo1(currentFieldNumber)._scenariosInfo.Count > 0 Then
            If currentFieldNumber < 0 Then currentFieldNumber = 0
            createSummaryText &= strField & ": " & _fieldsInfo1(currentFieldNumber).Name & " "
            createSummaryText &= strScenario & ": " & _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber).Name & " "
        End If

        Return createSummaryText
    End Function

    Public Sub showMessage(ByRef txtMessages As Label, ByRef imgMessage As Image, ByVal clr As String, ByVal img As String, ByVal msg As String)
        txtMessages.Text = msg
        txtMessages.Style.Item("display") = ""
        imgMessage.ImageUrl = "~/Resources/" & img
        'imgMessage.ImageUrl = "~/Resources/GoIcon.jpg"
        imgMessage.Width = "20"
        imgMessage.Style.Item("display") = ""
        Select Case clr.ToLower
            Case "green"
                txtMessages.ForeColor = Drawing.Color.Green
            Case "orange"
                txtMessages.ForeColor = Drawing.Color.Orange
            Case "red"
                txtMessages.ForeColor = Drawing.Color.Red
        End Select
        txtMessages.Visible = True
        txtMessages.Font.Size = 12
    End Sub

    Public Function calcSlopeLength(ByVal soilSlope As Single)
        Dim slopeLength As Single

        Const ft_to_m = 0.3048
        Select Case Math.Round(soilSlope, 4)
            Case 0 To 0.5, 11.0001 To 12
                slopeLength = 100 * ft_to_m
            Case 0.5001 To 1, 2.0001 To 3
                slopeLength = 200 * ft_to_m
            Case 1.0001 To 2
                slopeLength = 300 * ft_to_m
            Case 3.0001 To 4
                slopeLength = 180 * ft_to_m
            Case 4.0001 To 5
                slopeLength = 160 * ft_to_m
            Case 5.0001 To 6
                slopeLength = 150 * ft_to_m
            Case 6.0001 To 7
                slopeLength = 140 * ft_to_m
            Case 7.0001 To 8
                slopeLength = 130 * ft_to_m
            Case 8.0001 To 9
                slopeLength = 125 * ft_to_m
            Case 9.0001 To 10
                slopeLength = 120 * ft_to_m
            Case 10.0001 To 11
                slopeLength = 110 * ft_to_m
            Case 12.0001 To 13
                slopeLength = 90 * ft_to_m
            Case 13.0001 To 14
                slopeLength = 80 * ft_to_m
            Case 14.0001 To 15
                slopeLength = 70 * ft_to_m
            Case 15.0001 To 17
                slopeLength = 60 * ft_to_m
            Case Is > 17
                slopeLength = 50 * ft_to_m
        End Select

        Return slopeLength
    End Function

    Public Sub LoadAnimalUnits()
        Dim animalUnits = GetAnimalUnits()
        'clear animalunits list
        _animals.Clear()

        Dim animalName As String = String.Empty
        For Each c In animalUnits
            Select Case System.Web.HttpContext.Current.Session("Language")
                Case english
                    animalName = c.item("AnimalType").Trim
                Case spanish
                    animalName = c.item("AnimalSpanish").Trim()
                Case portuguese
                    animalName = c.item("AnimalPortuguese").Trim()
            End Select

            _animals.Add(New AnimalUnitsData With {.Number = c.item("AnimalCode") & "|" & c.item("ConversionUnit") & "|" & c.item("NO3N") & "|" & c.item("OrgN") & "|" & c.item("OrgP") & "|" & c.item("PO4P") & "|" & c.item("DryManure") & "|" & c.item("NH3"), .Name = animalName, .ConversionUnit = c.item("ConversionUnit"), .DryManure = c.item("DryManure")})
        Next

        _animals.Sort(New sortByCodeName())
        _animals.Insert(0, New AnimalUnitsData With {.Number = 0 & "|" & 0 & "|" & 0 & "|" & 0 & "|" & 0 & "|" & 0 & "|" & 0 & "|" & 0, .Name = selectOne, .ConversionUnit = 0, .DryManure = 0})
    End Sub

    Function calcHU(ByVal crop As Short, _crops As List(Of CropsData), _startInfo As StartInfo) As Single
        Dim ivar(300), cNum, jPlace As Short
        Dim j, phuc, nt As Single
        Dim mo, hd(300) As Single
        Dim itil As Short = 0
        Dim cname As String = String.Empty
        Dim to1 As Single = 0 : Dim dd As Single = 0 : Dim daym As Single = 0 : Dim iPlant As Single = 0 : Dim tb As Single = 0
        Dim phu As Single = 0 : Dim phus As Single = 0
        Dim temp As String = String.Empty
        Dim ta As Single = 0
        Dim sep As String = " "
        Dim phux As Single
        Dim ida As Short = 0
        Dim k As Short = 0
        Dim ncc() As Short = {0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366}
        Dim nc() As Short = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}

        Try
            nt = 1
            phuc = 0
            Dim i As Short
            For i = 0 To 299
                ivar(i) = 0
            Next

            jPlace = 0
            nt = 1

            cNum = crop
            j = 0

            For Each item In _crops
                If item.Number = cNum Then
                    itil = item.Itil
                    to1 = item.To1
                    tb = item.Tb
                    dd = item.Dd
                    daym = item.Daym
                    Exit For
                End If
                j += 1
            Next
            jPlace = j
            '*****varieties
            phux = 30 * 9.5 / ((_startInfo.wp1aLat + 1) ^ 0.5)
            '*****calculate plant date for summer crop without plant date input
            If daym > 0 Then
                phuc = 0
                For mo = 0 To 11
                    For ida = 0 To nc(mo) - 1
                        ta = (_startInfo.tMax(mo) + _startInfo.tMin(mo)) / 2 - (tb + 3)
                        If ta > 0 Then
                            phuc = phuc + ta
                            If phuc > phux Then GoTo 50
                        End If
                    Next
                Next
50:             iPlant = jdt(ida + 1, mo + 1, nt)
                '*****iplant=julian day of planting
                '*****daym=crop maturity days
                k = iPlant + daym
                phu = ahu(iPlant, k, mo + 1, _startInfo.tMax, _startInfo.tMin, tb, to1)
                If (daym <= 0) Then phu = 0
                '*****determine planting date for winter crops
                If (itil = 2) Then
                    phuc = 0
                    phux = phu / 2
                    Dim mo1 As Short
                    For mo = 0 To 11
                        mo1 = 11 - mo
                        For ida = 0 To nc(mo1)
                            ta = (_startInfo.tMax(mo1) + _startInfo.tMin(mo1)) / 2 - tb
                            If (ta > 0) Then phuc = phuc + ta
                            If (phuc > phux) Then GoTo 80
                        Next
                    Next
80:                 ida = nc(mo1) - ida
                    iPlant = jdt(ida + 1, mo1 + 1, nt)
                End If
            End If

            '*****select the proper variety
            '     sum heat units from plant to end of year
            If (phu <= 0) Then GoTo 130
            If (itil = 1) Then
                phuc = ahu(iPlant, 365, mo + 1, _startInfo.tMax, _startInfo.tMin, tb, to1)
            Else
                phuc = ahu(1, 365, mo + 1, _startInfo.tMax, _startInfo.tMin, tb, to1)
            End If
            '****changed constant 1.2 to input variable dd(*)+1.    02/23/95 nbs
            hd(j) = 1 + (dd * ((1 - ((_startInfo.wp1aLat + 1) ^ 0.5 / 9.5))) ^ 0.1)
            phuc = phuc / hd(j)
            If (phuc > phu) Then GoTo 120

            '120:            If (ic > 1) Then ic = 1  'ic con not be > 1 because ic went form 1 to 1 
120:        If (phu <= 0) Then
                ivar(j) = 0
            Else
                ivar(j) = 1
            End If
130:        If (ivar(j) = 0 And phu > 0) Then ivar(j) = (phuc * hd(j) / phu) * 100 'this condition never is true because if phu is > 0 ivar=1
            j = 1
            '*****overrides plant optimal temperature in upper limit on heat units
            '*****accumulated on a particular day
            '*****this just simulates the total heat units per year for base 0.
            to1 = 150
            tb = 0
            Dim phutot As Single
            phutot = ahu(1, 365, mo + 1, _startInfo.tMax, _startInfo.tMin, tb, to1)
            j = jPlace
            to1 = 150
            tb = 0
            phus = ahu(1, iPlant, mo + 1, _startInfo.tMax, _startInfo.tMin, tb, to1) / phutot

            Return phu

        Catch ex As System.Exception
            Return "Error 407 - " & ex.Message & " - Error calculating Heat Units"
        End Try
    End Function

    Function ahu(ByVal m As Single, ByVal k As Short, ByVal mo As Short, ByVal tmax() As Single, ByVal tmin() As Single, ByVal tbj As Single, ByVal to1j As Single) As Single
        Dim ahu1 As Single = 0
        Dim l As Short
        Dim ta As Single
        Dim humx As Single
        '     this subroutine accumulates heat units and radiation to calculate
        '     the potential heat units
        For l = m To k
            mo = xmonth(l, mo)
            ta = (tmax(mo - 1) + tmin(mo - 1)) / 2 - tbj
            If (ta > 0) Then
                humx = to1j - tbj
                If (ta > humx) Then ta = humx
                ahu1 = ahu1 + ta
            End If
        Next

        Return ahu1
    End Function

    Function xmonth(ByRef ida As Short, ByRef Month As Short) As Short
        Dim mday As Short
        '     this subroutine determines the month, given the day of the year
        '     +++ ARGUMENTS +++
        '     ida - integer day of the year
        '     month - integer month of the year
        If (ida < 32) Then
            Month = 1
            mday = ida
        ElseIf (ida < 61) Then
            Month = 2
            mday = ida - 31
        ElseIf (ida < 92) Then
            Month = 3
            mday = ida - 60
        ElseIf (ida < 122) Then
            Month = 4
            mday = ida - 91
        ElseIf (ida < 153) Then
            Month = 5
            mday = ida - 121
        ElseIf (ida < 183) Then
            Month = 6
            mday = ida - 152
        ElseIf (ida < 214) Then
            Month = 7
            mday = ida - 182
        ElseIf (ida < 245) Then
            Month = 8
            mday = ida - 213
        ElseIf (ida < 275) Then
            Month = 9
            mday = ida - 244
        ElseIf (ida < 306) Then
            Month = 10
            mday = ida - 274
        ElseIf (ida < 336) Then
            Month = 11
            mday = ida - 305
        Else
            Month = 12
            mday = ida - 335
        End If
        Return Month
    End Function

    Function jdt(ByVal i As Short, ByVal m As Short, ByVal nt As Single) As Short
        '     THIS SUBROUTINE COMPUTES THE DAY OF THE YEAR, GIVEN THE MONTH AND
        '     THE DAY OF THE MONTH.
        Dim nb() As Short = {0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366}
        Dim jdt1 As Short = 0
        'nb = 
        If (m <> 0) Then
            If (m <= 2) Then
                jdt1 = nb(m - 1) + i
                Return jdt1
            End If
            jdt1 = nb(m - 1) - nt + i
        Else
            jdt1 = 0
        End If
        Return jdt1
    End Function

    'Public Sub LoadFields(ByRef cbxField As DropDownList)
    '    Dim sField As ListItem
    '    cbxField.Items.Clear()
    '    Dim index As Short = -1
    '    For Each field In _fieldsInfo1
    '        If currentFieldNumber > 0 Then index = currentFieldNumber
    '        sField = New ListItem
    '        sField.Text = field.Name
    '        sField.Value = field.Number
    '        cbxField.Items.Add(sField)
    '    Next
    '    cbxField.SelectedIndex = index
    'End Sub

    Public Sub LoadFields(ByRef cbxField As DropDownList, ByRef _fieldsInfo1 As List(Of FieldsData), ByRef currentFieldNumber As Short)
        cbxField.DataTextField = "Name"
        cbxField.DataValueField = "Number"
        cbxField.DataSource = _fieldsInfo1
        cbxField.DataBind()
        If cbxField.Items.Count > 0 Then cbxField.SelectedIndex = 0
        If currentFieldNumber > 0 Then cbxField.SelectedIndex = currentFieldNumber
        currentFieldNumber = cbxField.SelectedIndex
    End Sub

    Public Sub LoadFields(ByRef cbxField As HtmlSelect, ByRef _fieldsInfo1 As List(Of FieldsData), ByRef currentFieldNumber As Short)
        cbxField.DataTextField = "Name"
        cbxField.DataValueField = "Number"
        cbxField.DataSource = _fieldsInfo1
        cbxField.DataBind()
        If cbxField.Items.Count > 0 Then cbxField.SelectedIndex = 0
        If currentFieldNumber > 0 Then cbxField.SelectedIndex = currentFieldNumber
        currentFieldNumber = cbxField.SelectedIndex
    End Sub

    Public Sub LoadScenarios(ByRef cbxField As DropDownList, ByRef _scenariosInfo As List(Of ScenariosData), ByRef currentScenarioNumber As Short)
        If _scenariosInfo.Count <= 0 Then Exit Sub
        cbxField.DataTextField = "Name"
        cbxField.DataSource = _scenariosInfo
        cbxField.DataBind()
        If cbxField.Items.Count > 0 Then cbxField.SelectedIndex = 0
        If currentScenarioNumber > 0 Then cbxField.SelectedIndex = currentScenarioNumber
        currentScenarioNumber = cbxField.SelectedIndex
    End Sub

    Public Sub LoadScenarios(ByRef cbxField As HtmlSelect, ByRef _scenariosInfo As List(Of ScenariosData), ByRef currentScenarioNumber As Short)
        If _scenariosInfo.Count <= 0 Then Exit Sub
        cbxField.DataTextField = "Name"
        cbxField.DataSource = _scenariosInfo
        cbxField.DataBind()
        If cbxField.Items.Count > 0 Then cbxField.SelectedIndex = 0
        If currentScenarioNumber > 0 Then cbxField.SelectedIndex = currentScenarioNumber
        currentScenarioNumber = cbxField.SelectedIndex
    End Sub

    Public Sub LoadAllScenarios(ByRef cbxField As HtmlSelect, ByRef _fieldsInfo1 As List(Of FieldsData), ByRef currentScenarioNumber As Short)
        Dim item As ListItem

        cbxField.Items.Clear()

        For Each field In _fieldsInfo1
            For Each scenario In field._scenariosInfo
                item = New ListItem
                item.Value = field.Name
                item.Text = scenario.Name
                cbxField.Items.Add(item)
            Next
        Next
        If cbxField.Items.Count > 0 Then cbxField.SelectedIndex = 0
        If currentScenarioNumber > 0 Then cbxField.SelectedIndex = currentScenarioNumber
        currentScenarioNumber = cbxField.SelectedIndex
    End Sub

    Private Sub AddBufferOperation(subarea As SubareasData, op As UShort, crop As UShort, yearsCult As UShort, val1 As Single, val2 As Single, lunum As UShort, addBuffer As Boolean, ByRef _scenariosInfo As ScenariosData)
        With _scenariosInfo
            If addBuffer Then ._bufferInfo.Add(subarea)
            Dim oper As New OperationsData
            oper.Day = 15 : oper.Month = 1 : oper.Year = 1
            oper.ApexCrop = crop
            oper.ApexOp = op
            oper.OpVal1 = val1
            oper.OpVal2 = val2
            oper.ApexOpType = yearsCult
            oper.LuNumber = lunum
            ._bufferInfo(._bufferInfo.Count - 1)._operationsInfo.Add(oper)
        End With
    End Sub

    Public Sub createBuffer(Type As String, crop As UShort, width As Single, slopeRatio As Single, GrassFieldPortion As Single, _fieldsInfo1 As List(Of FieldsData), currentFieldNumber As UShort, currentScenarioNumber As String)
        Dim subarea As SubareasData
        Dim totalAreas As Single = 0
        Dim subareaNumber As UShort = 0
        Dim inps As UShort = 0, iops As UShort = 0, slp As Single = _fieldsInfo1(currentFieldNumber)._soilsInfo(0).Slope / 100
        Dim nirr, iri, lm, idr As UShort
        Dim bir, efi, vimx, armx As Single

        For Each soil In _fieldsInfo1(currentFieldNumber)._soilsInfo.Where(Function(x) x.Selected = True)
            totalAreas += _fieldsInfo1(currentFieldNumber).Area * soil.Percentage / 100
            For Each _scenario In soil._scenariosInfo
                subareaNumber = _scenario._subareasInfo.SubareaNumber + 1
                inps = _scenario._subareasInfo._line2(0).Inps
                iops = _scenario._subareasInfo._line2(0).Iops
                If slp > soil.Slope / 100 Then slp = soil.Slope / 100
            Next
        Next
        totalAreas = totalAreas * ac_to_km2
        Select Case Type
            Case "FS"
                If crop > 0 And width > 0 And slopeRatio > 0 Then
                    deleteBuffer("FS", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
                    subarea = New SubareasData
                    'add subarea type
                    subarea.SbaType = Type
                    'line 1
                    subarea.SubareaNumber = 101
                    subarea.SubareaTitle = "Filter Strip"
                    'Line 2
                    subarea._line2(0).Inps = inps
                    subarea._line2(0).Iops = iops + 1
                    If subarea._line2(0).Iops = 1 Then subarea._line2(0).Iops = 2
                    subarea._line2(0).Iow = 1
                    'line 4
                    With subarea._line4(0)
                        subarea._line5(0).Rchl = CSng(_fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.FSWidth) * ft_to_km       'convert width to km
                        Dim tempLength As Single = Math.Sqrt(totalAreas)                'Calculate the lenght of the Filter Strip base on the total subarea file.
                        If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.FSArea > 0 Then
                            tempLength = (_fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.FSArea * ac_to_km2) / subarea._line5(0).Rchl
                            .Wsa = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.FSArea * ac_to_ha
                        Else
                            .Wsa = tempLength * subarea._line5(0).Rchl * 100 'Calculate Filter Strip Area and convert from km2 to ha
                            _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.FSArea = .Wsa * ha_to_ac
                        End If
                        If .Wsa <= 0.009 Then .Wsa = 0.01
                        'reduce the area of the others subareas proportionally.
                        updateWsa("-", .Wsa, currentScenarioNumber)
                        .chl = Math.Sqrt((subarea._line5(0).Rchl ^ 2) + ((tempLength / 2) ^ 2))
                        .Slp = slp * slopeRatio
                        .Slpg = calcSlopeLength(.Slp * 100)
                        .Chn = 0.1
                        .Upn = 0.24
                        .Ffpq = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.FSEff
                    End With
                    '/line 5
                    With subarea._line5(0)
                        .Rchn = 0.1
                        .Rchc = _fieldsInfo1(currentFieldNumber).RchcVal
                        .Rchk = _fieldsInfo1(currentFieldNumber).RchkVal
                        If .Rchc > 0.01 Then .Rchc = 0.01
                        Dim rchcBuff As Single = 0.01
                        Dim rchkBuff As Single = 0.2
                        If .Rchc < rchcBuff Then .Rchc = rchcBuff
                        If .Rchk < rchkBuff Then .Rchk = rchkBuff
                    End With
                    '.Rfpw = (.Wsa * ha_to_m2) / (.Rchl * km_to_m)
                    '.Rfpl = .Rchl
                    'line 8 and 9
                    With subarea._line8(0)
                        .Nirr = nirr
                        .Iri = iri
                        .Lm = lm
                        .Idr = idr
                    End With
                    With subarea._line9(0)
                        .Bir = bir
                        .Efi = efi
                        .Vimx = vimx
                        .Armx = armx
                    End With
                    '/line 10
                    subarea._line10(0).Pec = 1.0
                    Dim lu_number As UShort = _crops.Find(Function(x) x.Number = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.FSCrop).LuNumber
                    AddBufferOperation(subarea, 136, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.FSCrop, 0, 1400, 0, lu_number, True, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber))
                End If
            Case "Sdg"
                If crop > 0 And width > 0 And slopeRatio > 0 Then
                    deleteBuffer("Sdg", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
                    subarea = New SubareasData
                    'add subarea type
                    subarea.SbaType = Type
                    'line 1
                    subarea.SubareaNumber = 101
                    subarea.SubareaTitle = "Filter Strip"
                    'Line 2
                    subarea._line2(0).Inps = inps
                    subarea._line2(0).Iops = iops + 1
                    If subarea._line2(0).Iops = 1 Then subarea._line2(0).Iops = 2
                    subarea._line2(0).Iow = 1
                    'line 4
                    With subarea._line4(0)
                        subarea._line5(0).Rchl = CSng(_fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.SdgWidth) * ft_to_km       'convert width to km
                        Dim tempLength As Single = Math.Sqrt(totalAreas)                'Calculate the lenght of the Filter Strip base on the total subarea file.
                        If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.SdgArea > 0 Then
                            tempLength = (_fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.SdgArea * ac_to_km2) / subarea._line5(0).Rchl
                            .Wsa = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.SdgArea * ac_to_ha
                        Else
                            .Wsa = tempLength * subarea._line5(0).Rchl * 100 'Calculate Filter Strip Area and convert from km2 to ha
                        End If
                        If .Wsa <= 0.009 Then .Wsa = 0.01
                        'reduce the area of the others subareas proportionally.
                        updateWsa("-", .Wsa, currentScenarioNumber)
                        .chl = Math.Sqrt((subarea._line5(0).Rchl ^ 2) + ((tempLength / 2) ^ 2))
                        .Slp = slp * slopeRatio
                        .Slpg = calcSlopeLength(.Slp * 100)
                        .Chn = 0.1
                        .Upn = 0.24
                        .Ffpq = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.SdgEff
                    End With
                    '/line 5
                    With subarea._line5(0)
                        .Rchn = 0.1
                        .Rchc = _fieldsInfo1(currentFieldNumber).RchcVal
                        .Rchk = _fieldsInfo1(currentFieldNumber).RchkVal
                        If .Rchc > 0.01 Then .Rchc = 0.01
                        Dim rchcBuff As Single = 0.01
                        Dim rchkBuff As Single = 0.2
                        If .Rchc < rchcBuff Then .Rchc = rchcBuff
                        If .Rchk < rchkBuff Then .Rchk = rchkBuff
                        '.Rfpw = (.Wsa * ha_to_m2) / (.Rchl * km_to_m)
                        '.Rfpl = .Rchl
                    End With
                    'line 8 and 9
                    With subarea._line8(0)
                        .Nirr = nirr
                        .Iri = iri
                        .Lm = lm
                        .Idr = idr
                    End With
                    With subarea._line9(0)
                        .Bir = bir
                        .Efi = efi
                        .Vimx = vimx
                        .Armx = armx
                    End With
                    '/line 10
                    subarea._line10(0).Pec = 1.0
                    Dim lu_number As UShort = _crops.Find(Function(x) x.Number = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.FSCrop).LuNumber
                    AddBufferOperation(subarea, 136, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.FSCrop, 0, 1400, 0, lu_number, True, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber))
                End If
            Case "RF"
                Dim fsArea As Single = 0 : Dim fsWidth As Single = 0
                'Grass part of riparian forest
                If width > 0 And slopeRatio > 0 And GrassFieldPortion > 0 Then
                    deleteBuffer("RF", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
                    deleteBuffer("RFFS", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
                    subarea = New SubareasData
                    'add subarea type
                    subarea.SbaType = "RFFS"
                    'line 1
                    subarea.SubareaNumber = 102
                    subarea.SubareaTitle = "Filter Strip"
                    'Line 2
                    subarea._line2(0).Inps = inps
                    subarea._line2(0).Iops = iops + 1
                    If subarea._line2(0).Iops = 1 Then subarea._line2(0).Iops = 2
                    subarea._line2(0).Iow = 1
                    'line 4
                    Dim tempLength As Single = Math.Sqrt(totalAreas)                'Calculate the lenght of the Filter Strip base on the total subarea file.
                    With subarea._line4(0)
                        fsWidth = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.RFWidth * _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.RFGrassFieldPortion * ft_to_km       'convert width to km
                        subarea._line5(0).Rchl = fsWidth
                        '/Calculate RF area
                        If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.RFArea > 0 Then
                            fsArea = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.RFArea * ac_to_ha * _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.RFGrassFieldPortion
                            .Wsa = fsArea
                        Else
                            .Wsa = tempLength * subarea._line5(0).Rchl * 100 'Calculate Filter Strip Area and convert from km2 to ha
                            fsArea = .Wsa
                        End If
                        If .Wsa <= 0.009 Then .Wsa = 0.01
                        'reduce the area of the others subareas proportionally.
                        updateWsa("-", .Wsa, currentScenarioNumber)
                        .chl = Math.Sqrt((subarea._line5(0).Rchl ^ 2) + ((tempLength / 2) ^ 2))
                        .Slp = slp * slopeRatio
                        .Slpg = calcSlopeLength(.Slp * 100)
                        .Chn = 0.1
                        .Upn = 0.24
                        .Ffpq = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.RFEff
                    End With
                    '/line 5
                    Dim rchcBuff As Single = 0.01
                    Dim rchkBuff As Single = 0.2
                    With subarea._line5(0)
                        'subarea.Rchl = subarea.Rfpl
                        .Rchn = 0.1
                        .Rchc = _fieldsInfo1(currentFieldNumber).RchcVal
                        .Rchk = _fieldsInfo1(currentFieldNumber).RchkVal
                        If .Rchc > 0.01 Then .Rchc = 0.01
                        If .Rchc < rchcBuff Then .Rchc = rchcBuff
                        If .Rchk < rchkBuff Then .Rchk = rchkBuff
                        '.Rfpw = (subarea._line4(0).Wsa * ha_to_m2) / (.Rchl * km_to_m)
                        '.Rfpl = .Rchl
                    End With
                    'line 8 and 9
                    With subarea._line8(0)
                        .Nirr = nirr
                        .Iri = iri
                        .Lm = lm
                        .Idr = idr
                    End With
                    With subarea._line9(0)
                        .Bir = bir
                        .Efi = efi
                        .Vimx = vimx
                        .Armx = armx
                    End With
                    '/line 10
                    subarea._line10(0).Pec = 1.0
                    AddBufferOperation(subarea, 139, 49, 0, 1400, 0, 21, True, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber))
                    '*****************forest part of riparian forest*************************
                    subarea = New SubareasData
                    'add subarea type
                    subarea.SbaType = Type
                    'line 1
                    subarea.SubareaNumber = 103
                    subarea.SubareaTitle = "Riparian Forest"
                    'Line 2
                    subarea._line2(0).Inps = inps
                    subarea._line2(0).Iops = iops + 2
                    subarea._line2(0).Iow = 1
                    'line 4
                    With subarea._line4(0)
                        subarea._line5(0).Rchl = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.RFWidth * ft_to_km - fsWidth       'convert width to km
                        If _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.RFArea > 0 Then
                            .Wsa = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.RFArea * ac_to_ha * (1 - _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.RFGrassFieldPortion)
                        Else
                            .Wsa = tempLength * subarea._line5(0).Rchl * 100 'Calculate Filter Strip Area and convert from km2 to ha
                        End If
                        If .Wsa <= 0.009 Then .Wsa = 0.01
                        'reduce the area of the others subareas proportionally.
                        updateWsa("-", .Wsa, currentScenarioNumber)
                        .chl = Math.Sqrt((subarea._line5(0).Rchl ^ 2) + ((tempLength / 2) ^ 2))
                        .Slp = slp * slopeRatio
                        .Slpg = calcSlopeLength(.Slp * 100)
                        .Chn = 0.2
                        .Upn = 0.3
                        .Ffpq = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.RFEff
                    End With
                    '/line 5
                    With subarea._line5(0)
                        '.Rchl = .Rchl
                        .Rchn = 0.1
                        .Rchc = _fieldsInfo1(currentFieldNumber).RchcVal
                        .Rchk = _fieldsInfo1(currentFieldNumber).RchkVal
                        If .Rchc > 0.01 Then .Rchc = 0.01
                        If .Rchc < rchcBuff Then .Rchc = rchcBuff
                        If .Rchk < rchkBuff Then .Rchk = rchkBuff
                        .Rfpw = (subarea._line4(0).Wsa * ha_to_m2) / (.Rchl * km_to_m)
                        .Rfpl = .Rchl
                    End With
                    'line 8 and 9
                    With subarea._line8(0)
                        .Nirr = nirr
                        .Iri = iri
                        .Lm = lm
                        .Idr = idr
                    End With
                    With subarea._line9(0)
                        .Bir = bir
                        .Efi = efi
                        .Vimx = vimx
                        .Armx = armx
                    End With
                    '/line 10
                    subarea._line10(0).Pec = 1.0
                    AddBufferOperation(subarea, 139, 79, 350, 1900, -64, 28, True, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber))
                    AddBufferOperation(subarea, 139, 49, 0, 1400, 0, 21, False, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber))
                End If
            Case "WW"
                If crop > 0 And width > 0 Then
                    deleteBuffer("WW", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
                    subarea = New SubareasData
                    'add subarea type
                    subarea.SbaType = Type
                    'line 1
                    subarea.SubareaNumber = 104
                    subarea.SubareaTitle = "Filter Strip"
                    'Line 2
                    subarea._line2(0).Inps = inps
                    subarea._line2(0).Iops = iops + 1
                    If subarea._line2(0).Iops = 1 Then subarea._line2(0).Iops = 2
                    subarea._line2(0).Iow = 1
                    'line 4
                    subarea._line5(0).Rchl = CSng(width) * ft_to_km       'convert width to km
                    Dim tempLength As Single = Math.Sqrt(totalAreas)                'Calculate the lenght of the Filter Strip base on the total subarea file.
                    With subarea._line4(0)
                        .Wsa = tempLength * subarea._line5(0).Rchl * 100 'Calculate Filter Strip Area and convert from km2 to ha
                        If .Wsa <= 0.009 Then .Wsa = 0.01
                        'reduce the area of the others subareas proportionally.
                        updateWsa("-", .Wsa, currentScenarioNumber)
                        .chl = Math.Sqrt((subarea._line5(0).Rchl ^ 2) + ((tempLength / 2) ^ 2))
                        .Slp = slp * 0.25
                        .Slpg = calcSlopeLength(.Slp * 100)
                        .Chn = 0.1
                        .Upn = 0.24
                        .Ffpq = 0.9
                    End With
                    '/line 5
                    With subarea._line5(0)
                        '.Rchl = .Rchl
                        .Rchn = 0.1
                        .Rchc = _fieldsInfo1(currentFieldNumber).RchcVal
                        .Rchk = _fieldsInfo1(currentFieldNumber).RchkVal
                        If .Rchc > 0.01 Then .Rchc = 0.01
                        Dim rchcBuff As Single = 0.01
                        Dim rchkBuff As Single = 0.2
                        If .Rchc < rchcBuff Then .Rchc = rchcBuff
                        If .Rchk < rchkBuff Then .Rchk = rchkBuff
                        .Rfpw = (subarea._line4(0).Wsa * ha_to_m2) / (.Rchl * km_to_m)
                        .Rfpl = .Rchl
                    End With
                    'line 8 and 9
                    With subarea._line8(0)
                        .Nirr = nirr
                        .Iri = iri
                        .Lm = lm
                        .Idr = idr
                    End With
                    With subarea._line9(0)
                        .Bir = bir
                        .Efi = efi
                        .Vimx = vimx
                        .Armx = armx
                    End With
                    '/line 10
                    subarea._line10(0).Pec = 1.0
                    Dim lu_number As UShort = _crops.Find(Function(x) x.Number = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.WWCrop).LuNumber
                    AddBufferOperation(subarea, 136, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.WWCrop, 0, 1400, 0, lu_number, True, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber))
                End If
            Case "WL"
                Dim area As Single = 0
                area = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.WLArea
                If area > 0 Then
                    deleteBuffer("WL", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
                    subarea = New SubareasData
                    'add subarea type
                    subarea.SbaType = Type
                    'line 1
                    subarea.SubareaNumber = 105
                    subarea.SubareaTitle = "Wetland"
                    'Line 2
                    subarea._line2(0).Inps = inps
                    subarea._line2(0).Iops = iops
                    If subarea._line2(0).Iops = 1 Then subarea._line2(0).Iops = 2
                    subarea._line2(0).Iow = 1
                    'line 4
                    Dim tempLength As Single = Math.Sqrt(totalAreas)    'Calculate the lenght of the Filter Strip base on the total subarea file.
                    subarea._line5(0).Rchl = area * ac_to_km2 / tempLength
                    With subarea._line4(0)
                        .Wsa = area * ac_to_ha    'convert from ac to ha
                        If .Wsa <= 0.009 Then .Wsa = 0.01
                        'reduce the area of the others subareas proportionally.
                        updateWsa("-", .Wsa, currentScenarioNumber)
                        .chl = Math.Sqrt((subarea._line5(0).Rchl ^ 2) + ((tempLength / 2) ^ 2))
                        .Slp = 0.01
                        .Slpg = calcSlopeLength(.Slp * 100)
                        .Chs = 0.0
                        .Chn = 0.0
                        .Upn = 0.0
                        .Ffpq = 0.0
                    End With
                    '/line 5
                    With subarea._line5(0)
                        .Rchd = 0.0
                        .Rcbw = 0.0
                        .Rctw = 0.0
                        .Rchs = 0
                        .Rchn = 0.0
                    End With
                    'line 6 reservoir information
                    With subarea._line6(0)
                        .Rsee = 0.3
                        .Rsae = subarea._line4(0).Wsa
                        .Rsve = 50
                        .Rsep = 0.3
                        .Rsap = subarea._line4(0).Wsa
                        .Rsvp = 25
                        .Rsv = 20
                        .Rsrr = 20
                        .Rsys = 300
                        .Rsyn = 300
                        subarea._line7(0).Rshc = 0.001
                        subarea._line7(0).Rsdp = 360
                        subarea._line7(0).Rsbd = 0.8
                    End With
                    'line 8 and 9
                    With subarea._line8(0)
                        .Nirr = nirr
                        .Iri = iri
                        .Lm = lm
                        .Idr = idr
                    End With
                    With subarea._line9(0)
                        .Bir = bir
                        .Efi = efi
                        .Vimx = vimx
                        .Armx = armx
                    End With
                    '/line 10
                    subarea._line10(0).Pec = 1.0
                    AddBufferOperation(subarea, 139, 129, 0, 2000, 0, 26, True, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber))
                End If
            Case "PPDE", "PPTW", "AITW"
                Dim area As Single = 0
                Select Case True
                    Case _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.PPDEResArea > 0
                        area = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.PPDEResArea
                    Case _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.PPTWResArea > 0
                        area = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.PPTWResArea
                    Case _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AIResArea > 0
                        area = _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.AIResArea
                End Select
                If area > 0 Then
                    Select Case Type
                        Case "PPDE"
                            deleteBuffer("PPDE", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
                        Case "PPTW"
                            deleteBuffer("PPTW", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
                        Case "AITW"
                            deleteBuffer("AITW", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
                    End Select
                    subarea = New SubareasData
                    'add subarea type
                    subarea.SbaType = Type
                    'line 1
                    subarea.SubareaNumber = 106
                    subarea.SubareaTitle = "0000000000000000  .sub file Reservoir  Date: " & Date.Now
                    'Line 2
                    subarea._line2(0).Inps = inps
                    subarea._line2(0).Iops = iops
                    subarea._line2(0).Iow = 1
                    'line 4
                    Dim tempLength As Single = Math.Sqrt(totalAreas)    'Calculate the lenght of the Filter Strip base on the total subarea file.
                    subarea._line5(0).Rchl = area * ac_to_km2 / tempLength
                    With subarea._line4(0)
                        .Wsa = area * ac_to_ha    'convert from ac to ha
                        If .Wsa <= 0.009 Then .Wsa = 0.01
                        'reduce the area of the others subareas proportionally.
                        updateWsa("-", .Wsa, currentScenarioNumber)
                        .chl = Math.Sqrt((subarea._line5(0).Rchl ^ 2) + ((tempLength / 2) ^ 2))
                        .Slp = slp * 0.25
                        .Slpg = calcSlopeLength(.Slp * 100)
                        .Chs = slp
                        .Chn = 0.05
                        .Upn = 0.41
                        .Ffpq = 0.8
                    End With
                    '/line 5
                    With subarea._line5(0)
                        .Rchd = 0.1
                        .Rcbw = 0.1
                        .Rctw = 0.2
                        .Rchs = slp
                        .Rchn = 0.15
                        .Rchk = 0.01
                        .Rchc = 0.2
                    End With
                    'line 6 reservoir information
                    With subarea._line6(0)
                        .Rsee = 0.1
                        .Rsae = subarea._line4(0).Wsa
                        .Rsve = 75
                        .Rsep = 0.1
                        .Rsap = subarea._line4(0).Wsa
                        .Rsvp = 25
                        .Rsv = 0
                        .Rsrr = 1
                        .Rsys = 300
                        .Rsyn = 300
                    End With
                    'line 7
                    With subarea._line7(0)
                        .Rshc = 0.001
                        .Rsdp = 360
                        .Rsbd = 0.8
                    End With
                    'line 8 and 9
                    With subarea._line8(0)
                        .Nirr = nirr
                        .Iri = iri
                        .Lm = lm
                        .Idr = idr
                    End With
                    With subarea._line9(0)
                        .Bir = bir
                        .Efi = efi
                        .Vimx = vimx
                        .Armx = armx
                    End With
                    '/line 10
                    subarea._line10(0).Pec = 1.0
                    AddBufferOperation(subarea, 139, 130, 0, 2000, 0, 26, True, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber))
                    '_fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo.Add(subarea)
                End If
            Case "CB"
                If crop > 0 And width > 0 And slopeRatio > 0 Then
                    Dim numberOfSoils = _fieldsInfo1(currentFieldNumber)._soilsInfo.Where(Function(x) x.Selected).Count
                    deleteBuffer("CBMain", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
                    deleteBuffer("CBFS", _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo, currentScenarioNumber)
                    Dim totalStrips As Single = Math.Sqrt(totalAreas / ac_to_km2 * ac_to_ha * 10000) / (width * ft_to_m + slopeRatio * ft_to_m)   'width is main crop width and sloperatio is buffer width
                    If totalStrips > maxStrips Then totalStrips = maxStrips
                    Dim k As UShort = 0
                    For i = 1 To totalStrips
                        For j = 1 To 2
                            For l = 0 To numberOfSoils - 1
                                totalAreas = _fieldsInfo1(currentFieldNumber)._soilsInfo(l).Percentage * _fieldsInfo1(currentFieldNumber).Area * ac_to_ha / 100 'km2
                                subarea = New SubareasData
                                'add subarea type
                                If j = 1 Then
                                    subarea.SbaType = "CBMain"
                                Else
                                    subarea.SbaType = "CBFS"
                                End If
                                With subarea._line4(0)
                                    If j = 1 Then
                                        .Wsa = totalAreas * (width / (width + slopeRatio) / totalStrips)  'calculate main crop strip. Width has main crop strip width
                                        subarea._line2(0).Iops = 1
                                    Else
                                        .Wsa = totalAreas * (slopeRatio / (width + slopeRatio) / totalStrips) 'calculate buffer strip. slope ratio has buffer strip width
                                        subarea._line2(0).Iops = 2
                                    End If
                                    If .Wsa = 0 Then .Wsa = 0.01
                                    .chl = Math.Sqrt(.Wsa * 0.01)
                                    subarea._line5(0).Rchl = .chl
                                    k += 1
                                    If k > 1 Then subarea._line5(0).Rchl = .chl * 0.9
                                End With
                                'line 1
                                subarea.SubareaNumber = k
                                subarea.SubareaTitle = "Contour Buffer"
                                'Line 2
                                subarea._line2(0).Inps = inps
                                subarea._line2(0).Iow = 1
                                'line 4
                                With subarea._line4(0)
                                    .Slp = _fieldsInfo1(currentFieldNumber)._soilsInfo(l).Slope / 100
                                    .Slpg = calcSlopeLength(.Slp * 100)
                                    .Chs = 0.0
                                    .Chn = 0.0
                                    .Upn = 0.0
                                    .Ffpq = 0.0
                                    If j = 2 Then
                                        .Chn = 0.1
                                        .Upn = 0.24
                                        .Ffpq = 0.9
                                    End If
                                End With
                                'Line 5
                                With subarea._line5(0)
                                    .Rchn = 0
                                    .Rchc = _fieldsInfo1(currentFieldNumber).RchcVal
                                    .Rchk = _fieldsInfo1(currentFieldNumber).RchkVal
                                    .Rfpw = 0
                                    .Rfpl = 0
                                    If j = 2 Then
                                        .Rchn = 0.1
                                        .Rchc = 0.01
                                        .Rchk = 0.2
                                        If _fieldsInfo1(currentFieldNumber).RchcVal < 0.01 Then .Rchc = _fieldsInfo1(currentFieldNumber).RchcVal
                                        If _fieldsInfo1(currentFieldNumber).RchkVal < 0.2 Then .Rchk = _fieldsInfo1(currentFieldNumber).RchkVal
                                    End If
                                End With
                                '/line 10
                                subarea._line10(0).Pec = 1.0
                                'Select Case i & j
                                Select Case j
                                    Case 1
                                        _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo.Add(subarea)
                                    Case 2
                                        AddBufferOperation(subarea, 139, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.CBCrop, 0, 1400, 0, 25, True, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber))
                                    Case Else
                                        _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo.Add(subarea)
                                End Select
                            Next
                        Next
                    Next
                End If
        End Select
    End Sub

    Public Sub deleteBuffer(type As String, _bufferInfo As List(Of SubareasData), currentScenarioNumber As UShort)
        For i = _bufferInfo.Count - 1 To 0 Step -1
            If _bufferInfo(i).SbaType = type Then
                If type <> "CBMain" And type <> "CBFS" Then updateWsa("+", _bufferInfo(i)._line4(0).Wsa, currentScenarioNumber)
                _bufferInfo.RemoveAt(i)
            End If
        Next
    End Sub

    Sub updateWsa(operation As String, wsa As Single, currentScenarioNumber As UShort)
        Dim currentFieldNumber As UShort = System.Web.HttpContext.Current.Session("currentFieldNumber")
        Dim _soilsInfo As List(Of SoilsData) = System.Web.HttpContext.Current.Session("projects")._fieldsInfo1(currentFieldNumber)._soilsInfo
        'For Each soil In _fieldsInfo1(currentFieldNumber)._soilsInfo.Where(Function(x) x.Selected = True)
        For Each soil In _soilsInfo
            If operation = "+" Then
                soil._scenariosInfo(currentScenarioNumber)._subareasInfo._line4(0).Wsa += wsa * soil.Percentage / 100
            Else
                soil._scenariosInfo(currentScenarioNumber)._subareasInfo._line4(0).Wsa -= wsa * soil.Percentage / 100
            End If
        Next
    End Sub

    'Public Function GetXMLData(ByVal sPath As String, ByVal sFilename As String) As DataSet
    '    Dim ds As New DataSet
    '    Dim sFile As String
    '    Try
    '        WriteToDebugFile(sMeName + "<<Starting - GetXMLData>>" + "((" + sPath.ToString + "~" + sFilename.ToString + "))")

    '        sFile = sPath & sFilename.Trim.ToString
    '        If System.IO.File.Exists(sFile) Then
    '            ds.ReadXml(sFile, XmlReadMode.InferSchema)
    '        End If
    '        Return ds
    '    Catch ex As Exception
    '        WriteToErrorFile(sMeName, "GetXMLData", "Exception failure", ex)
    '        Throw ex
    '    End Try
    'End Function
    Public Sub CalculateAvgSlope(ByRef _fieldsInfo1 As List(Of FieldsData), currentFieldNumber As UShort)
        If _fieldsInfo1.Count > 0 AndAlso _fieldsInfo1(currentFieldNumber)._soilsInfo.Where(Function(y) y.Selected = True).Count > 0 Then
            _fieldsInfo1(currentFieldNumber).AvgSlope = _fieldsInfo1(currentFieldNumber)._soilsInfo.Where(Function(y) y.Selected = True).Average(Function(x) x.Slope)
        End If
    End Sub

    Public Function CalculateSD(ByVal array1 As Array) As Single
        Dim total As Single = array1.Length
        Dim mean As Single = 0
        Dim sum As Single = 0
        For i = 0 To array1.Length - 1
            mean += array1(i)
        Next
        mean /= total
        For i = 0 To array1.Length - 1
            sum += (array1(i) - mean) ^ 2
        Next
        Return Math.Sqrt(sum / total)
    End Function

End Module
