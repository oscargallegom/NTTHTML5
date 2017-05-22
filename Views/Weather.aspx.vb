Public Class Weather
    Inherits System.Web.UI.Page
    'Dim dc As New NTTDBDataContext
    Private _stations As List(Of StationInfo)

    'Controls used in sitemaster page
    Private lblMessage As Label
    Private imgIcon As Image
    Public _counties As New List(Of CodeAndName)
    'Classes definition
    Private _startInfo As New StartInfo

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        folder = My.Computer.FileSystem.GetParentPath(Server.MapPath(""))
        If Session("userGuide") = "" Then
            If Page.Request.QueryString(0) <> String.Empty Then
                Session("userGuide") = Page.Request.QueryString(0)
            Else
                Response.Redirect("~/Default.aspx", False)
                Exit Sub
            End If
        End If
        lblMessage = New Label
        lblMessage = CType(Master.FindControl("lblMessage"), Label)
        imgIcon = New Image
        imgIcon = CType(Master.FindControl("imgIcon"), Image)
        lblMessage.Style.Item("display") = "none"
        imgIcon.Style.Item("display") = "none"
        lblMessage.Text = ""
        Select Case Page.Request.Params("__EVENTARGUMENT")
            Case english
                Session("Language") = english
            Case spanish
                Session("Language") = spanish
        End Select
        openXMLLanguagesFile()

        ChangeLanguageContent()

        ArrangeInfo("Open")
        'showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", _startInfo.weatherLat & " -- " & _startInfo.weatherLon)
        If Not Page.Request.Params("__EVENTTARGET") Is Nothing Then
            Select Case True
                Case Page.Request.Params("__EVENTTARGET").Contains("btnUpload")
                    btnUploadWeatherFile_Click()
            End Select
        End If

        TurnOnOffControls()
    End Sub

    Protected Sub TurnOnOffControls()
        fsetStations.Style.Item("display") = "none"
        fsetUploadWeatherFile.Style.Item("display") = "none"
        fsetCoordinates.Style.Item("display") = "none"
        rbtnCoordinates.Checked = False
        rbtnOwn.Checked = False
        rbtnStation.Checked = False

        Select Case True
            Case (_startInfo.Status = googleMaps And _startInfo.stationWay = "Prism") Or (_startInfo.Status = googleMaps And _startInfo.stationWay = googleMaps)
                rbtnStation.Checked = True
                lblStation.InnerText = cntDoc.Descendants("WeatherStationOption").Value
                If _startInfo.currentWeatherPath = "" Then GetPrism()
            Case _startInfo.Status = googleMaps And _startInfo.stationWay = "Own"
                rbtnOwn.Checked = True
                fsetUploadWeatherFile.Style.Item("display") = ""
            Case _startInfo.Status = googleMaps And _startInfo.stationWay = "Coordinates"
                rbtnCoordinates.Checked = True
                fsetCoordinates.Style.Item("display") = ""
            Case _startInfo.Status = googleMaps And (_startInfo.stationWay = "" Or _startInfo.stationWay Is Nothing)
                lblStation.InnerText = cntDoc.Descendants("WeatherStationOption").Value
                rbtnStation.Checked = True
                _startInfo.weatherLat = _startInfo.farmCentrois.Split(",")(1)
                _startInfo.weatherLon = _startInfo.farmCentrois.Split(",")(0)
                GetPrism()
            Case _startInfo.Status = stateCounty And _startInfo.stationWay = "Station"
                rbtnStation.Checked = True
                fsetStations.Style.Item("display") = "none"
                lblStation.InnerText = cntDoc.Descendants("WeatherStationOption").Value
                'changed on 12/9/15 to take the weather information from PRISM database
                LoadStations()  'keeping this function to get station information such us wp1, wnd files
                GetPrism()
                ddlStations.Visible = False
            Case _startInfo.Status = stateCounty And _startInfo.stationWay = own
                rbtnOwn.Checked = True
                fsetUploadWeatherFile.Style.Item("display") = ""
            Case _startInfo.Status = stateCounty And _startInfo.stationWay = coordinates
                rbtnCoordinates.Checked = True
                fsetCoordinates.Style.Item("display") = ""
            Case _startInfo.Status = stateCounty And (_startInfo.stationWay = "" Or _startInfo.stationWay Is Nothing)
                rbtnStation.Checked = True
                fsetStations.Style.Item("display") = "none"
                lblStation.InnerText = cntDoc.Descendants("WeatherStationOption").Value
                'changed on 12/9/15 to take the weather information from PRISM database
                LoadStations()      'keeping this function to get station information such us wp1, wnd files
                GetPrism()
                ddlStations.Visible = False
            Case _startInfo.Status = userInput And _startInfo.stationWay = own
                rbtnOwn.Checked = True
                fsetUploadWeatherFile.Style.Item("display") = ""
            Case Else

        End Select

        If Not IsPostBack Then
            'take period to show in weather screen
            lblWeatherYears.Text = _startInfo.weatherInitialYear & " - " & _startInfo.weatherFinalYear
            txtPeriodFromSimulate.Text = _startInfo.stationInitialYear
            txtPeriodToSimulate.Text = _startInfo.stationFinalYear
            txtLat.Text = _startInfo.weatherLat
            txtLong.Text = _startInfo.weatherLon
        End If
    End Sub

    Protected Sub LoadStations()
        Dim countyCode As String = _startInfo.countyCode & "%"
        _stations = GetWStation(countyCode)

        If _startInfo.currentWeatherPath = "" Then
            If _startInfo.StateAbrev <> "" AndAlso Not (_startInfo.stationCode Is Nothing) Then
                GetStationInfo(_startInfo.stationCode)
            Else
                If _stations.Count > 0 Then
                    GetStationInfo(_stations(0).Code)
                End If
            End If
        End If

        ddlStations.DataTextField = "name"
        ddlStations.DataValueField = "code"
        ddlStations.DataSource = _stations
        ddlStations.DataBind()

        If _startInfo.StateAbrev <> "" AndAlso Not (_startInfo.stationCode Is Nothing) Then
            ddlStations.Value = _startInfo.stationCode
        Else
            ddlStations.SelectedIndex = 0
        End If

    End Sub

    Public Sub GetPrism()
        Try
            Const latDif As Single = 0.04
            Const lonDif As Single = 0.09
            Dim latLess, latPlus, lonLess, lonPlus As Double
            Dim sSQL As String = String.Empty
            Dim nlat As Single = _startInfo.weatherLat
            Dim nlon As Single = _startInfo.weatherLon

            If nlat = 0 Or nlon = 0 Then
                'find coordinates for the current county
                For Each County In _counties
                    If County.Code.Trim = _startInfo.countyCode.Trim Then
                        _startInfo.weatherLat = County.Lat
                        nlat = County.Lat
                        _startInfo.weatherLon = County.Lon
                        nlon = County.Lon
                    End If
                Next
            End If

            latLess = nlat - latDif : latPlus = nlat + latDif
            lonLess = nlon - lonDif : lonPlus = nlon + lonDif

            Dim weatherFileQuery As Object = GetWeatherCoor(nlat, nlon, latLess, latPlus, lonLess, lonPlus)
            If Not weatherFileQuery Is Nothing Then
                Do While weatherFileQuery.Read
                    'Dim newWeather As String = weatherPrismFiles.Replace("US", "1981-2015")
                    'If System.IO.File.Exists(System.IO.Path.Combine(newWeather, weatherFileQuery("fileName"))) Then
                    '_startInfo.currentWeatherPath = newWeather & "\" & weatherFileQuery("fileName")
                    '_startInfo.weatherFinalYear = weatherFileQuery("finalYear") + 2
                    'Else
                    _startInfo.currentWeatherPath = weatherPrismFiles & "\" & weatherFileQuery("fileName")
                    _startInfo.weatherFinalYear = weatherFileQuery("finalYear")
                    'End If

                    _startInfo.weatherInitialYear = weatherFileQuery("initialYear")
                    _startInfo.stationInitialYear = _startInfo.weatherInitialYear + 5
                    _startInfo.stationFinalYear = _startInfo.weatherFinalYear
                    '_startInfo.currentWeatherPath = weatherPrismFiles & "\" & weatherFileQuery("fileName")
                    _startInfo.stationYears = _startInfo.stationFinalYear - _startInfo.stationInitialYear + 1 + 5
                Loop
            End If

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", ex.Message)
        End Try
    End Sub

    Private Sub ChangeLanguageContent()
        lblWeatherTitle.InnerText = cntDoc.Descendants("SelectWeather").Value
        lblStation.InnerText = cntDoc.Descendants("WeatherStationOption").Value
        lblStations.InnerText = cntDoc.Descendants("WeatherStationOption").Value
        lblOwn.InnerText = cntDoc.Descendants("OwnWeatherOption").Value
        lblCoordinates.InnerText = cntDoc.Descendants("WeatherCoordinatesOption").Value
        lblLong.Text = cntDoc.Descendants("Longitude").Value
        lblLat.Text = cntDoc.Descendants("Latitude").Value
        lblYearsOfInfo.Text = cntDoc.Descendants("YearsOfWeatherInformation").Value
        lblSimulationNote.Text = cntDoc.Descendants("PeriodSimulation").Value
        lblSimulationPeriod.Text = cntDoc.Descendants("PeriodToSimulate").Value
        lblTo.Text = cntDoc.Descendants("to").Value
        btnSaveCoordinates.Text = cntDoc.Descendants("SaveCoordinates").Value
        lblSimulationPeriod.Text = cntDoc.Descendants("simulationperiod").Value
        btnSaveCoordinates.Text = cntDoc.Descendants("SaveCoordinates").Value
        btnSave.Text = cntDoc.Descendants("Save").Value
        btnContinue.Text = cntDoc.Descendants("Continue").Value
        lblPeriodToSimulate.Text = cntDoc.Descendants("PeriodToSimulate").Value
        'tool tips
        btnSave.ToolTip = msgDoc.Descendants("ttSaveAndContinue").Value
        rbtnStation.Attributes.Add("title", msgDoc.Descendants("ttPrismData").Value)
        lblStation.Attributes.Add("title", msgDoc.Descendants("ttPrismData").Value)
        rbtnOwn.Attributes.Add("title", msgDoc.Descendants("ttLoadOwnWeather").Value)
        lblOwn.Attributes.Add("title", msgDoc.Descendants("ttLoadOwnWeather").Value)
        rbtnCoordinates.Attributes.Add("title", msgDoc.Descendants("ttSpecificCoordinates").Value)
        lblCoordinates.Attributes.Add("title", msgDoc.Descendants("ttSpecificCoordinates").Value)
        txtPeriodFromSimulate.ToolTip = msgDoc.Descendants("ttInitialPeriodToSimulate").Value
        txtPeriodToSimulate.ToolTip = msgDoc.Descendants("ttFinalPeriodToSimulate").Value
    End Sub

    Protected Sub btnSaveCoordinates_Click(sender As Object, e As EventArgs) Handles btnSaveCoordinates.Click
        Try
            If txtLong.Text < -180 And txtLong.Text > -60 Then Throw New Global.System.Exception(msgDoc.Descendants("LongitudeError").Value & vbCrLf)
            If txtLat.Text < 18 And txtLong.Text > 75 Then Throw New Global.System.Exception(msgDoc.Descendants("LatitudeError").Value & vbCrLf)
            _startInfo.weatherLon = txtLong.Text
            _startInfo.weatherLat = txtLat.Text
            _startInfo.stationWay = "Coordinates"
            GetPrism()
            txtPeriodFromSimulate.Text = _startInfo.stationInitialYear
            txtPeriodToSimulate.Text = _startInfo.stationFinalYear
            lblWeatherYears.Text = _startInfo.weatherInitialYear & " - " & _startInfo.weatherFinalYear
            TurnOnOffControls()
            SaveInfoFromScreen()
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", ex.Message)
        End Try
    End Sub

    Private Sub btnContinue_Click(sender As Object, e As System.EventArgs) Handles btnContinue.Click
        Response.Redirect("Field.aspx", False)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim msg As String = "OK"
        Try
            msg = SaveInfoFromScreen()
            If msg = "OK" Then
                showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("InformationSaved").Value)
            Else
                Throw New Exception(msg)
            End If

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Error").Value & ex.Message)
        End Try

    End Sub

    Private Function SaveInfoFromScreen() As String
        Try
            If txtPeriodToSimulate.Text <= txtPeriodFromSimulate.Text Then
                Throw New Exception(msgDoc.Descendants("SimulatedPeriodMessage").Value.Replace("First", txtPeriodFromSimulate.Text).Replace("Second", txtPeriodToSimulate.Text))
            End If
            If txtPeriodFromSimulate.Text < _startInfo.weatherInitialYear + 5 Or txtPeriodFromSimulate.Text > _startInfo.weatherFinalYear Then
                txtPeriodFromSimulate.BackColor = Drawing.Color.Red
                Throw New Exception(msgDoc.Descendants("SimulatedPeriodMessage").Value.Replace("First", _startInfo.weatherInitialYear).Replace("Second", _startInfo.weatherFinalYear))
            Else
                _startInfo.stationInitialYear = txtPeriodFromSimulate.Text
            End If
            If txtPeriodToSimulate.Text > _startInfo.weatherFinalYear Or txtPeriodToSimulate.Text < _startInfo.weatherInitialYear + 5 Then
                txtPeriodToSimulate.BackColor = Drawing.Color.Red
                Throw New Exception(msgDoc.Descendants("SimulatedPeriodMessage").Value.Replace("Second", _startInfo.weatherFinalYear).Replace("First", _startInfo.weatherInitialYear))
            Else
                _startInfo.stationFinalYear = txtPeriodToSimulate.Text
            End If
            ArrangeInfo("Save")
            Return "OK"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Protected Sub btnUploadWeatherFile_Click()
        Dim srWeather As System.IO.StreamReader = Nothing
        Dim swWeather As System.IO.StreamWriter = Nothing
        Dim records() As String = Nothing
        Try
            Dim temp As String = "inicial"
            flUpload.PostedFile.SaveAs(folder & "\App_xml\" & Session("userGuide") & ".wth")
            srWeather = New System.IO.StreamReader(folder & "\App_xml\" & Session("userGuide").ToString & ".wth")
            swWeather = New System.IO.StreamWriter(folder & "\App_xml\APEX.wth")
            records = srWeather.ReadLine().Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
            'temp = srWeather.ReadLine().Substring(2, 4)
            _startInfo.weatherInitialYear = records(0)
            _startInfo.stationInitialYear = _startInfo.weatherInitialYear + 5
            Do While srWeather.EndOfStream <> True And records.Length <> 0
                Select Case records.Length
                    Case 7
                        swWeather.WriteLine("  " & records(0).PadLeft(4) & records(1).PadLeft(4) & records(2).PadLeft(4) & records(3).PadLeft(6) & records(4).PadLeft(6) & records(5).PadLeft(6) & records(6).PadLeft(6))
                    Case 8
                        swWeather.WriteLine("  " & records(0).PadLeft(4) & records(1).PadLeft(4) & records(2).PadLeft(4) & records(3).PadLeft(6) & records(4).PadLeft(6) & records(5).PadLeft(6) & records(6).PadLeft(6) & records(7).PadLeft(6))
                    Case 9
                        swWeather.WriteLine("  " & records(0).PadLeft(4) & records(1).PadLeft(4) & records(2).PadLeft(4) & records(3).PadLeft(6) & records(4).PadLeft(6) & records(5).PadLeft(6) & records(6).PadLeft(6) & records(7).PadLeft(6) & records(8).PadLeft(6))
                End Select
                temp = records(0)
                records = srWeather.ReadLine().Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                If records.Length = 0 Then
                    Exit Do
                End If
                If records.Length < 7 Then
                    Dim a As Single = 0
                    a = 1

                End If
            Loop
            _startInfo.stationFinalYear = temp
            _startInfo.stationYears = _startInfo.stationFinalYear - _startInfo.stationInitialYear + 1 + 5
            _startInfo.weatherFinalYear = temp
            _startInfo.currentWeatherPath = folder & "\App_xml\APEX.wth"

            txtPeriodFromSimulate.Text = _startInfo.stationInitialYear
            txtPeriodToSimulate.Text = _startInfo.stationFinalYear
            lblWeatherYears.Text = _startInfo.weatherInitialYear & " - " & _startInfo.weatherFinalYear
            _startInfo.stationWay = "Own"
            SaveInfoFromScreen()
            showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("WeatherUploaded").Value.Replace("1", _startInfo.stationYears))
            fsetUploadWeatherFile.Style.Item("display") = "none"
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Error").Value & " - " & ex.Message)
        Finally
            srWeather.Close()
            srWeather.Dispose()
            srWeather = Nothing
            swWeather.Close()
            swWeather.Dispose()
            swWeather = Nothing
        End Try
    End Sub

    Public Sub wp1File(ByRef _startInfo As StartInfo)
        Try
            Dim wp1Info As String = String.Empty
            Dim temp As String
            Dim srFileWP1 As System.IO.StreamReader = New System.IO.StreamReader(System.IO.File.OpenRead(wp1Files & "\" & _startInfo.Wp1Name.Trim & ".wp1"))
            Dim offset As Integer = 6
            srFileWP1.ReadLine()
            _startInfo.wp1aLat = srFileWP1.ReadLine.Substring(8, 7).Trim
            temp = srFileWP1.ReadLine
            ReDim _startInfo.tMax(11)
            For i = 0 To 11
                _startInfo.tMax(i) = Val(temp.Substring(offset * i, 6))
            Next
            ReDim _startInfo.tMin(11)
            temp = srFileWP1.ReadLine
            For i = 0 To 11
                _startInfo.tMin(i) = Val(temp.Substring(offset * i, 6))
            Next
        Catch ex As System.Exception
            'Return ex.Message
        End Try
    End Sub

    Private Sub ddlStations_ServerChange(sender As Object, e As System.EventArgs) Handles ddlStations.ServerChange
        Dim countyCode As String = _startInfo.countyCode & "%"
        _stations = GetWStation(countyCode)
        GetStationInfo(ddlStations.Items(ddlStations.SelectedIndex).Value)
    End Sub

    Private Sub GetStationInfo(stationCode As String)
        For Each station1 In _stations
            If stationCode.Trim = station1.Code.Trim Then
                _startInfo.weatherInitialYear = station1.InitialYear
                _startInfo.weatherFinalYear = station1.FinalYear
                _startInfo.stationInitialYear = station1.InitialYear + 5
                _startInfo.stationFinalYear = station1.FinalYear
                _startInfo.stationYears = _startInfo.stationFinalYear - _startInfo.stationInitialYear + 1 + 5
                _startInfo.stationCode = station1.Code
                _startInfo.stationName = station1.Name
                _startInfo.WindCode = station1.WindCode
                _startInfo.WindName = station1.WindName
                _startInfo.Wp1Code = station1.Wp1Code
                _startInfo.Wp1Name = station1.Wp1Name
                _startInfo.stationWay = "Station"
                If station1.WSType.Trim = "County" Then
                    GetPrism()
                Else
                    _startInfo.currentWeatherPath = weatherStationFiles & "\" & _startInfo.stationCode.Trim
                End If
                wp1File(_startInfo)
                Exit For
            End If
        Next
    End Sub
    Private Sub ArrangeInfo(openSave As String)
        Select Case openSave
            'this is done after the open subrutine
            Case "Open"
                _startInfo = Session("projects")._StartInfo
                'this is done before the save subroutine
            Case "Save"
                Session("projects")._StartInfo = _startInfo
        End Select
    End Sub

End Class