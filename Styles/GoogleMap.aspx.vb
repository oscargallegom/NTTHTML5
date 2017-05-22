Imports System.IO
Imports Ionic.Zip

Public Class GoogleMap
    Inherits System.Web.UI.Page

    Protected preDrawnAOI As String = ""
    Protected prelatlng As String = ""
    Protected preAddress As String = ""
    Private dbConnectString As String = "Server=T-NN\SQLEXPRESS;initial catalog=NTTDB;persist security info=False;user id=sa; password=pass$word;Pooling=false"
    'Classes definition
    Private _startInfo As New StartInfo
    Private _fieldsInfo1 As New List(Of FieldsData)
    Private currentFieldNumber As UShort = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        folder = My.Computer.FileSystem.GetParentPath(Server.MapPath(""))
        If Session("userGuide") = "" Then
            Response.Redirect("Default.aspx", False)
            Exit Sub
        End If
        ArrangeInfo("Open")
        If Not IsPostBack Then
            openXMLLanguagesFile()
            ChangeLanguageContent()
            'load states and counties select control
            GetStates()
            GetCounty(stateselect.Value)
            preDrawnAOI = ""
            If _startInfo.farmName <> "" Then preDrawnAOI = "farm: " & _startInfo.farmName & ", " & _startInfo.farmCoordinates
            For Each _field In _fieldsInfo1
                If _field.Coordinates <> "" And Not _field.Coordinates Is Nothing And _field.Coordinates <> "None" Then preDrawnAOI &= " field: " & _field.Name & ", area:" & _field.Area & ", " & _field.Coordinates
            Next
            'preDrawnAOI = "farm: farm, -99.022157,32.943753 -99.016407,32.943861 -99.016321,32.935055 -99.022307,32.934821  field: Field1, area:361158.5271661438, -99.022071,32.943591 -99.016407,32.943753 -99.016471,32.938440 -99.019282,32.937864 -99.019346,32.937108 -99.022200,32.936856 field: Field2, area:198556.95101509723, -99.016514,32.938440 -99.016385,32.934929 -99.022222,32.934875 -99.022222,32.937972 "
            'Dealing with the string single quote mark problem
            Dim index As Integer = preDrawnAOI.LastIndexOf("'")
            If (index <> -1) Then
                preDrawnAOI = preDrawnAOI.Remove(index, 1)
            End If
        End If
    End Sub

    Private Sub ChangeLanguageContent()
        lblShapeFile.InnerHtml = cntDoc.Descendants("UploadShapefile").Value
        lblNavigation.InnerHtml = cntDoc.Descendants("Navigation").Value
        lblEditingOptions.InnerHtml = cntDoc.Descendants("EditingOptions").Value
        lblFarm.InnerHtml = cntDoc.Descendants("Farm").Value
        lblField.InnerHtml = cntDoc.Descendants("Field").Value
        lblDelete.Value = cntDoc.Descendants("Remove").Value
        lblFarmShp.Text = cntDoc.Descendants("ChooseFarmShapefile").Value
        lblFieldShp.Text = cntDoc.Descendants("ChooseFieldShapefile1").Value
        _lblNote1.InnerHtml = cntDoc.Descendants("Note1").Value
        lblNote2.InnerHtml = cntDoc.Descendants("Note2").Value
        bntUpload.Text = cntDoc.Descendants("UploadShapefile").Value
        txtAddress.Value = cntDoc.Descendants("Address").Value
        lblLatLon.Value = cntDoc.Descendants("LatLon").Value
        lblZoomState.Value = cntDoc.Descendants("ZoomState").Value
        lblZoomCounty.Value = cntDoc.Descendants("ZoomCounty").Value
        submit.Text = cntDoc.Descendants("Submit").Value
        lblNoteNavigation.InnerHtml = cntDoc.Descendants("MapNavigation").Value
        lblTools.InnerHtml = cntDoc.Descendants("Tools").Value
        lblToolsNote1.InnerHtml = cntDoc.Descendants("ToolsNote1").Value
        lblToolsNote2.InnerHtml = cntDoc.Descendants("ToolsNote2").Value
        lblToolsNote3.InnerHtml = cntDoc.Descendants("ToolsNote3").Value
        lblToolsNote4.InnerHtml = cntDoc.Descendants("ToolsNote4").Value
        lblFieldName.InnerHtml = cntDoc.Descendants("NewFieldName").Value.Replace(" 1", "")
    End Sub

    Public Sub GetStates()
        Dim sSQL As String
        Dim dr As System.Data.SqlClient.SqlDataReader
        Dim item As ListItem

        Try
            sSQL = "SELECT Name, StateAbrev FROM State WHERE DStatusSL = 1 or DStatusSL = 9 ORDER BY Name"
            dr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(dbConnectString, CommandType.Text, sSQL)
            If dr.HasRows Then
                Do While dr.Read
                    item = New ListItem
                    item.Text = dr("Name").ToString
                    item.Value = dr("StateAbrev").ToString
                    stateselect.Items.Add(item)
                Loop
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub GetCounty(ByVal sStAbrev As String)
        Dim sSQL As String
        Dim dr As System.Data.SqlClient.SqlDataReader
        Dim item As ListItem

        Try
            sSQL = "SELECT Code, Name, StateAbrev FROM County WHERE DStatusSL = 1 or DStatusSL=9 ORDER BY Name"
            dr = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteReader(dbConnectString, CommandType.Text, sSQL)
            item = New ListItem
            item.Text = lblZoomCounty.Value
            item.Value = ""
            countyAll.Items.Add(item)
            If dr.HasRows Then
                Do While dr.Read
                    item = New ListItem
                    item.Text = dr("Name").ToString
                    item.Value = dr("StateAbrev").ToString & "-" & dr("Name").ToString
                    countyAll.Items.Add(item)
                Loop
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles submit.Click
        'MsgBox1.alert("runing map")
        Dim webClient As New System.Net.WebClient
        Dim requestCoordinates As StringBuilder = New StringBuilder
        Dim j As Short
        Dim area As Double = 0
        Dim xmlSoils As DataSet = New DataSet
        Dim _fieldsArea() As String = Nothing
        Dim _fieldsName() As String = Nothing
        Dim _orgFieldsXY() As String = Nothing
        Dim lastCoordinate() As String = Nothing
        Dim point() As System.Drawing.PointF = Nothing
        Dim points() As String
        Dim centroid As System.Drawing.PointF
        Dim state As String = StateName.Value
        Dim stateAbr As String = StateAbbr.Value
        Dim county As String = CountyName.Value
        'set states dropdown list to -1 to avoid the program zooms to it instead of the polygon.
        stateselect.SelectedIndex = -1

        'validate if state is active. If not stop and give a message
        Dim found As Boolean = False
        For i = 0 To stateselect.Items.Count - 1
            If stateselect.Items(i).Value = stateAbr Then found = True
        Next
        If found = False Then
            MsgBox(msgDoc.Descendants("StateNoAvailable").Value)
            Exit Sub
        End If

        _fieldsArea = Split(FieldsArea.Value, ",")
        _fieldsName = Split(FieldsName.Value, ",")
        _orgFieldsXY = Split(FieldsXY.Value, " ,")
        Dim _fieldsXY(_orgFieldsXY.Length - 1) As String
        For i = 0 To _orgFieldsXY.Length - 1
            lastCoordinate = Split(_orgFieldsXY(i), " ")
            _fieldsXY(i) = _orgFieldsXY(i).Trim & " " & lastCoordinate(0).Trim
        Next

        'If Request.QueryString("file") <> "" Then
        Dim conn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Dim sql As String = String.Empty
        Dim da As System.Data.SqlClient.SqlDataAdapter
        Dim dtResult As System.Data.DataTable
        conn.ConnectionString = "Data Source=T-NN\SQLEXPRESS;Initial Catalog=NTTDB;User ID=sa;Password=pass$word"
        conn.Open()
        'xmlSoils.ReadXml(Request.QueryString("file") & "\XMLFile.xml")
        'save last key to control save and continue in NTT
        'If Not (Request.QueryString("key") Is Nothing) Then xmlSoils.Tables("StartInfo").Rows(0).Item("key") = Request.QueryString("key")
        'save county and state information
        'xmlSoils.Tables("StartInfo").Rows(0).Item("StateAbrev") = stateAbr
        'xmlSoils.Tables("StartInfo").Rows(0).Item("StateName") = state
        'xmlSoils.Tables("StartInfo").Rows(0).Item("CountyName") = county
        _StartInfo.StateAbrev = stateAbr
        _StartInfo.StateName = state
        _StartInfo.countyName = county
        'get county code from database needed to use the correct shp file for soils and slope.
        sql = "SELECT code, name, DStatusSL FROM county WHERE name like '" & county.Split(" ")(0) & "%' AND stateAbrev = '" & stateAbr & "'"
        da = New System.Data.SqlClient.SqlDataAdapter(sql, conn)
        dtResult = New System.Data.DataTable
        da.Fill(dtResult)
        If dtResult.Rows.Count > 0 Then
            'xmlSoils.Tables("StartInfo").Rows(0).Item("CountyCode") = dtResult.Rows(0).Item("code")
            If dtResult.Rows(0).Item("DStatusSL") <> 1 And dtResult.Rows(0).Item("DStatusSL") <> 9 Then
                MsgBox(msgDoc.Descendants("CountyNoAvailable").Value)
                Exit Sub
            End If
            _startInfo.countyCode = dtResult.Rows(0).Item("code")
        End If
        da.Dispose()
        'initialized poligono to redraw in postback
        preDrawnAOI = "farm: " & FarmName.Value & ", " & FarmXY.Value & " "

        'get county code from database needed to use the correct shp file for soils and slope.
        sql = "SELECT windName, wp1Name FROM WStation WHERE countyCode like '" & _startInfo.countyCode & "%' AND stateAbrev = '" & stateAbr & "'"
        da = New System.Data.SqlClient.SqlDataAdapter(sql, conn)
        dtResult = New System.Data.DataTable
        da.Fill(dtResult)
        For i = 0 To dtResult.Rows.Count - 1
            _startInfo.WindCode = dtResult.Rows(0).Item("windName").split(",")(0).trim
            _startInfo.Wp1Code = dtResult.Rows(0).Item("wp1Name").split(",")(0).trim
            _startInfo.WindName = dtResult.Rows(0).Item("windName").split(",")(1).trim
            _startInfo.Wp1Name = dtResult.Rows(0).Item("wp1Name").split(",")(1).trim
            Dim weather As New Weather
            weather.wp1File(_startInfo)
            Exit For
        Next
        da.Dispose()
        conn.Close()
        conn.Dispose()

        'delete exiting farm, fields, and soils to save the new selection.
        'For i = xmlSoils.Tables("FarmInfo").Rows.Count - 1 To 0 Step -1
        '    xmlSoils.Tables("FarmInfo").Rows(i).Delete()   'delete farm
        'Next
        'For i = xmlSoils.Tables("FieldInfo").Rows.Count - 1 To 0 Step -1
        '    xmlSoils.Tables("FieldInfo").Rows(i).Delete()    'delete fields
        'Next
        'save farm information
        'Dim newFarm As DataRow = xmlSoils.Tables("FarmInfo").NewRow
        'newFarm.Item("coordinates") = FarmXY.Value
        'newFarm.Item("name") = FarmName.Value
        points = FarmXY.Value.Trim.Split(" ")
        For j = 0 To points.Length - 1
            ReDim Preserve point(j)
            Dim y As Double = System.Convert.ToDouble(points(j).Split(",")(1))
            Dim x As Double = System.Convert.ToDouble(points(j).Split(",")(0))
            point(j).X = x
            point(j).Y = y
        Next
        centroid = CalculateCentroid(point, j - 1)
        'newFarm.Item("centroid") = centroid.X & "," & centroid.Y
        'xmlSoils.Tables("FarmInfo").Rows.Add(newFarm)
        _startInfo.farmCentrois = centroid.X & "," & centroid.Y
        _startInfo.weatherLat = centroid.Y
        _startInfo.weatherLon = centroid.X
        _startInfo.farmCoordinates = FarmXY.Value
        _startInfo.farmName = FarmName.Value
        _startInfo.currentWeatherPath = ""
        'End If

        found = False

        For j = _fieldsInfo1.Count - 1 To 0 Step -1
            found = False
            For i = 0 To _fieldsXY.Length - 1
                If _fieldsInfo1(j).Name.ToString.ToLower = _fieldsName(i).ToString.ToLower Then
                    found = True
                    Exit For
                End If
            Next    'NEXT i new fields
            If found = False Then
                _fieldsInfo1.Remove(_fieldsInfo1(j))
            End If
        Next    'next j old fields

        Dim field As FieldsData
        For i = 0 To _fieldsXY.Length - 1
            found = False
            preDrawnAOI = preDrawnAOI & "field: " & _fieldsName(i) & ", " & _fieldsArea(i) & ", " & _orgFieldsXY(i) & " "
            For j = 0 To _fieldsInfo1.Count - 1
                If _fieldsInfo1(j).Name.ToString.ToLower = _fieldsName(i).ToString.ToLower Then
                    _fieldsInfo1(j).Name = _fieldsName(i)
                    _fieldsInfo1(j).Area = Math.Round(_fieldsArea(i) / ac_to_m2, 2)
                    _fieldsInfo1(j).Coordinates = _fieldsXY(i)
                    _fieldsInfo1(j).Number = i + 1
                    _fieldsInfo1(j).Rchc = "Moderate"
                    _fieldsInfo1(j).Rchk = "Moderate"
                    _fieldsInfo1(j).RchcVal = 0.2
                    _fieldsInfo1(j).RchkVal = 0.2
                    found = True
                    Exit For
                End If
            Next
            If found = False Then
                field = New FieldsData
                field.Name = _fieldsName(i)
                field.Area = Math.Round(_fieldsArea(i) / ac_to_m2, 2)
                field.Coordinates = _fieldsXY(i)
                field.Number = i + 1
                field.Rchc = "Moderate"
                field.Rchk = "Moderate"
                field.RchcVal = 0.2
                field.RchkVal = 0.2
                _fieldsInfo1.Add(field)
            End If
        Next

        For i = 0 To _fieldsInfo1.Count - 1
            currentFieldNumber = i
            GetSoilsInfo(_fieldsInfo1(i).Coordinates, i)
        Next
        ArrangeInfo("Save")
        Dim message As String = "OK"
        If _fieldsInfo1.Count > 0 Then currentFieldNumber = 0
        Session("currentFieldNumber") = currentFieldNumber
        Session.Item("MapStatus") = "Ended"
        dvForm.Style.Item("display") = ""
        dvWait.Style.Item("display") = "none"
    End Sub

    Public Sub GetSoilsInfo(Coordinates As String, ByVal fieldNumber As Short)
        Dim found As Boolean = False
        Dim log As StreamWriter = Nothing
        Dim tableName As String = _startInfo.StateAbrev & "SOILS"
        Dim slope As Single = 0
        Dim sql As String = String.Empty
        Dim service As New GetSoilsInfo.SoilsSoapClient

        Try
            Dim soilsxx As New DataTable
            Dim coordinatesArray() As String = Coordinates.Split(" ")
            Dim newCoordinates As String = String.Empty

            newCoordinates = "("
            For j = 0 To coordinatesArray.Length - 2
                newCoordinates &= coordinatesArray(j).Split(",")(1) & "," & coordinatesArray(j).Split(",")(0) & ")"
                If j < coordinatesArray.Length - 2 Then newCoordinates &= ",("
            Next
            soilsxx = service.GetSoilsInfo(newCoordinates, StateName.Value.ToString.ToLower)
            If soilsxx Is Nothing Then
                Throw New Global.System.Exception("The field number " & fieldNumber + 1 & " does not have soils available")
            Else
                If soilsxx.Rows.Count = 0 Then
                    Throw New Global.System.Exception("The field " & fieldNumber + 1 & " does not have soils available")
                End If
            End If

            _fieldsInfo1(fieldNumber).DeleteSoil(0, True)

            For Each item In soilsxx.Rows
                sql = "SELECT top 1 seriesname, albedo, (slopeh-slopel)/2 as slope, horizdesc2,  muname  FROM " & tableName & " WHERE muid=" & item(2) & " AND TSSSACode='" & item(1) & "' AND HORIZDESC1='" & item(3) & "' AND horizdesc2<>''"
                slope = (item(5) * degrees_to_radians) * 100  'System.Convert degrees received from soil slope program in degrees to radians and then to %.
                _fieldsInfo1(fieldNumber).AddSingleSoil(sql, item(2), "", item(3), item(1), slope, item(4), _startInfo, 0)
            Next

            _fieldsInfo1(fieldNumber)._soilsInfo.Sort(New sortByArea())
            For i = 0 To _fieldsInfo1(fieldNumber)._soilsInfo.Count - 1
                If i > 2 Then Exit For
                _fieldsInfo1(fieldNumber)._soilsInfo(i).Selected = True
            Next
            Dim totalSoilsArea = _fieldsInfo1(fieldNumber)._soilsInfo.Sum(Function(x) x.Percentage)
            For i = 0 To _fieldsInfo1(fieldNumber)._soilsInfo.Count - 1
                _fieldsInfo1(fieldNumber)._soilsInfo(i).Percentage = Math.Round(_fieldsInfo1(fieldNumber)._soilsInfo(i).Percentage / totalSoilsArea * 100, 2)
            Next

        Catch ex As System.Exception
        End Try
    End Sub

    Function CalculateCentroid(ByVal points As System.Drawing.PointF(), ByVal lastPointIndex As Integer) As System.Drawing.PointF
        Dim area As Double = 0.0
        Dim Cx As Double = 0.0
        Dim Cy As Double = 0.0
        Dim tmp As Double = 0.0
        Dim k As Integer

        For i As Integer = 0 To lastPointIndex
            k = (i + 1) Mod (lastPointIndex + 1)
            tmp = points(i).X * points(k).Y - points(k).X * points(i).Y
            area += tmp
            Cx += (points(i).X + points(k).X) * tmp
            Cy += (points(i).Y + points(k).Y) * tmp
        Next
        area *= 0.5
        Cx *= 1.0F / (6.0 * area)
        Cy *= 1.0F / (6.0 * area)
        Return New System.Drawing.PointF(Cx, Cy)
    End Function

    Protected Sub bntUpload_Click(sender As Object, e As EventArgs) Handles bntUpload.Click
        If fileUploadFarmShapefile.HasFile And fileUploadFieldShapefile.HasFile Then
            Try
                Dim strbuilderAOIInfo As New StringBuilder
                Dim result As Boolean
                Dim sfUnprojectedFarm = New MapWinGIS.Shapefile
                Dim sfUnprojectedField = New MapWinGIS.Shapefile
                Dim i, j, k As Integer
                Dim strFarmName As String = "Farm"
                Dim strShpFarmPath As String = ""
                Dim strArrayFildesFP As New List(Of String)
                'Dim strArrayFildesFP As New ArrayList 'changed because it was creating an error. Check it.
                Dim strShpFieldPath As String = ""
                'Dim extractPath As String = Server.MapPath("~/Files/FarmShp/")
                Dim strPath As String = Request.QueryString("file") & "\Files\"
                DeleteDirectory(strPath)
                'Request.QueryString("file") on 6/24/2014
                Dim extractPath As String = Request.QueryString("file") & "\Files\FarmShp\"
                Using zip As ZipFile = ZipFile.Read(fileUploadFarmShapefile.PostedFile.InputStream)
                    zip.ExtractAll(extractPath, ExtractExistingFileAction.OverwriteSilently)
                End Using
                ' Get files.
                Dim directoryName As String
                For Each directoryName In Directory.GetFiles(extractPath)
                    Dim extension As String = Path.GetExtension(directoryName)
                    If extension = ".shp" Then
                        strShpFarmPath = directoryName
                    End If
                Next
                'extractPath = Server.MapPath("~/Files/FieldsShp/")
                'on 6/24/2014
                extractPath = Request.QueryString("file") & "\Files\FieldsShp\"
                Using zip As ZipFile = ZipFile.Read(fileUploadFieldShapefile.PostedFile.InputStream)
                    zip.ExtractAll(extractPath, ExtractExistingFileAction.OverwriteSilently)
                End Using
                ' Get files.
                For Each directoryName In Directory.GetFiles(extractPath)
                    Dim extension As String = Path.GetExtension(directoryName)
                    If extension = ".shp" Then
                        strShpFieldPath = directoryName
                        strArrayFildesFP.Add(strShpFieldPath)
                    End If
                Next
                'Add farm information
                'preDrawnAOI = "farm: " & FarmName.Value & ", " & FarmXY.Value & " "
                'farm: farm, -98.379795,32.172195 -98.383064,32.164259 -98.375074,32.165573 -98.369430,32.167556 -98.371146,32.169959
                strbuilderAOIInfo.Append("farm: " + strFarmName + ", ")
                result = sfUnprojectedFarm.Open(strShpFarmPath)
                If result = False Then
                    MsgBox((sfUnprojectedFarm.ErrorMsg(sfUnprojectedFarm.LastErrorCode)))
                End If
                For i = 0 To sfUnprojectedFarm.NumShapes - 1
                    Dim shpPolygon As New MapWinGIS.Shape
                    shpPolygon = sfUnprojectedFarm.Shape(i)
                    k = shpPolygon.numPoints - 2
                    For j = 0 To k
                        Dim pnt As MapWinGIS.Point
                        pnt = shpPolygon.Point(j)
                        Dim x As Double = pnt.x
                        Dim y As Double = pnt.y
                        Dim strFarmXY As String = ""
                        strFarmXY = x.ToString() + "," + y.ToString()
                        strbuilderAOIInfo.Append(strFarmXY + " ")
                    Next j
                Next i
                sfUnprojectedFarm.Close()
                'fields
                'Add filed(s)' name and area information
                'field: field1, 167320.99598818127, -98.379696,32.172111 -98.378277,32.171748 -98.379036,32.166075 -98.381413,32.165796 -98.381842,32.164287 -98.382998,32.164231
                'preDrawnAOI = preDrawnAOI & "field: " & _fieldsName(i) & ", " & _fieldsArea(i) & ", " & _orgFieldsXY(i) & " "
                Dim intNo As Integer
                intNo = 1
                Dim pos1, pos2 As UShort
                Dim fieldName As String = String.Empty
                For Each strShpFieldPath In strArrayFildesFP
                    result = sfUnprojectedField.Open(strShpFieldPath)
                    If result = False Then
                        MsgBox((sfUnprojectedField.ErrorMsg(sfUnprojectedField.LastErrorCode)))
                    End If
                    For i = 0 To sfUnprojectedField.NumShapes - 1
                        pos1 = strShpFieldPath.LastIndexOf("\")
                        pos2 = strShpFieldPath.LastIndexOf(".")
                        Dim shpPolygon As New MapWinGIS.Shape
                        shpPolygon = sfUnprojectedField.Shape(i)
                        Dim strbuilderFieldXY As New StringBuilder
                        Dim listX As List(Of Double) = New List(Of Double)
                        Dim listY As List(Of Double) = New List(Of Double)
                        k = shpPolygon.numPoints - 2
                        For j = 0 To k
                            Dim pnt As MapWinGIS.Point
                            pnt = shpPolygon.Point(j)
                            Dim x As Double = pnt.x
                            Dim y As Double = pnt.y
                            Dim intZone As Integer
                            intZone = Math.Ceiling((x + 180) / 6)
                            Dim oldProj As String = "+proj=longlat +ellps=GRS80 +no_defs"
                            Dim newProj As String = "+proj=utm +zone=" + intZone.ToString() + " +ellps=GRS80 +units=m +no_defs"

                            Dim success As Boolean = MapWinGeoProc.SpatialReference.ProjectPoint(x, y, oldProj, newProj)

                            listX.Add(x)
                            listY.Add(y)
                            strbuilderFieldXY.Append(pnt.x.ToString() + "," + pnt.y.ToString() + " ")
                        Next j
                        ' Convert the list to an array.
                        Dim arrayX As Double() = listX.ToArray
                        Dim arrayY As Double() = listY.ToArray
                        Dim innerString As String = ""
                        If sfUnprojectedField.NumShapes = 1 Then
                            fieldName = "field: " & strShpFieldPath.Substring(pos1 + 1, pos2 - pos1 - 1) & ", "
                        Else
                            fieldName = "field: " & strShpFieldPath.Substring(pos1 + 1, pos2 - pos1 - 1) & (i + 1).ToString() + ", "
                        End If
                        'Dim FieldArea As String = shpPolygon.Area.ToString() + ", "
                        Dim FieldArea As String = polygonArea(arrayX, arrayY, listX.Count).ToString() + ", "
                        'Dim FieldArea As String = "0.0" + ", "
                        innerString = fieldName + FieldArea + strbuilderFieldXY.ToString()
                        strbuilderAOIInfo.Append(innerString)
                    Next i
                    sfUnprojectedField.Close()
                    intNo = intNo + 1
                Next
                '
                'hdf_Test.Value = strbuilderAOIInfo.ToString()
                preDrawnAOI = strbuilderAOIInfo.ToString()
            Catch ex As Exception
                MsgBox("The shpefile you uploaded is using a geographic coordinate system (latitude and longitude). Please try to use a geographic coordinate system such as WGS 1984.")
            End Try
        End If
    End Sub

    Function polygonArea(ByVal x() As Double, ByVal y() As Double, ByVal numPoints As Integer) As Double
        Dim area As Double = 0.0
        Dim i As Integer = 0
        Dim j As Integer = numPoints - 1
        For i = 0 To numPoints - 1
            area = area + (x(j) + x(i)) * (y(j) - y(i))
            j = i
        Next
        area = area / 2
        If area < 0 Then
            area = area * (-1)
        End If
        Return area
    End Function

    Private Sub DeleteDirectory(path As String)
        If Directory.Exists(path) Then
            'Delete all files from the Directory
            For Each filepath As String In Directory.GetFiles(path)
                File.Delete(filepath)
            Next
            'Delete all child Directories
            For Each dir As String In Directory.GetDirectories(path)
                DeleteDirectory(dir)
            Next
            'Delete a Directory
            Directory.Delete(path)
        End If
    End Sub

    Protected Sub imgAddress_Click(sender As Object, e As EventArgs) Handles imgAddress.Click
        Dim address As String = TextAddress.Value
        preAddress = address
        trNavigation.Style.Item("display") = "none"
        trTools.Style.Item("display") = ""
    End Sub

    Protected Sub imgLatlng_Click(sender As Object, e As EventArgs) Handles imgLatlng.Click
        Dim latlng As String = Textlatlng.Value
        prelatlng = latlng
        trNavigation.Style.Item("display") = "none"
        trTools.Style.Item("display") = ""
    End Sub
    Private Sub ArrangeInfo(openSave As String)
        Select Case openSave
            'this is done after the open subrutine
            Case "Open"
                _startInfo = Session("projects")._StartInfo
                _fieldsInfo1 = Session("projects")._fieldsInfo1
            Case "Save"
                Session("projects")._StartInfo = _startInfo
                Session("projects")._fieldsInfo1 = _fieldsInfo1
        End Select
    End Sub

    Protected Sub btnCopyFarm_Click(sender As Object, e As EventArgs) Handles btnCopyFarm.Click
        If txtFieldName.Value = "" Then txtFieldName.Value = "Field1"
        stateselect.SelectedIndex = 0
        'initialized poligono to redraw in postback
        preDrawnAOI = "farm: " & FarmName.Value & ", " & FarmXY.Value & " "   'take farm coordinates
        Dim _orgFieldsXY() As String = Split(FieldsXY.Value, " ,")

        preDrawnAOI = preDrawnAOI & "field: " & txtFieldName.Value & ", area:" & 0 & ", " & FarmXY.Value  'take field coordinates
        preDrawnAOI = preDrawnAOI & FarmXY.Value.Split(" ")(0)   'close the polygon adding the first set of coordinates at the end
        dvForm.Style.Item("display") = ""
        dvWait.Style.Item("display") = "none"
    End Sub

End Class