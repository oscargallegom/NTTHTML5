Public Class Modifications
    Inherits System.Web.UI.Page

    Private lblMessage As Label
    Private lblStatus As Label
    Private imgIcon As Image

    Private Property currentSubareaNumber As UShort
    Private Property currentLayerNumber As UShort
    Private currentSoilNumber As Short
    Private _subareasInfo As New List(Of SubareasData)
    Enum APEXFiles
        Control = 0
        Parameters = 1
        Subarea = 2
        Soil = 3
        Layer = 4
        Operation = 5
    End Enum

    'Classes definition
    Private _projects As New ProjectsData
    Private _startInfo As New StartInfo
    Private _fieldsInfo1 As New List(Of FieldsData)
    Private _sitesInfo As New List(Of SiteData)
    Private _parmValues As New List(Of ParmsData)
    Private _controlValues As New List(Of ParmsData)
    Private _crops As New List(Of CropsData)
    Public _equipmentTemp As List(Of EquipmentData)
    Private _scenariosToRun As New List(Of ScenariosToRun)
    Public _feedTemp As List(Of FeedData)
    Public _structureTemp As List(Of StructureData)
    Public _otherTemp As List(Of OtherData)
    Private _subprojectName As New List(Of SubprojectNameData)
    'retrieve the currentfieldnumber
    Private currentFieldNumber As Short = 0
    Private currentScenarioNumber As Short = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("userGuide") = "" Then
            Response.Redirect("~/Default.aspx", False)
            Exit Sub
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
        If Not IsPostBack Then
            currentFieldNumber = Session("currentFieldNumber")
            ddlFields.SelectedIndex = currentFieldNumber
            currentScenarioNumber = Session("currentScenarioNumber")
            ddlScenarios.SelectedIndex = currentScenarioNumber
            SelectTable(0)
            LoadFields(ddlFields, _fieldsInfo1, currentFieldNumber)
        End If

        currentFieldNumber = ddlFields.SelectedIndex
        currentScenarioNumber = ddlScenarios.SelectedIndex

        If Not Page.Request.Params("__EVENTTARGET") Is Nothing Then
            Select Case True
                Case Page.Request.Params("__EVENTTARGET").Contains("btnSaveContinue") AndAlso ddlFile.SelectedItem.Text.Contains(APEXFiles.Control.ToString)
                    SaveControl() 'Save control APEX File.
                Case Page.Request.Params("__EVENTTARGET").Contains("btnSaveContinue") AndAlso ddlFile.SelectedItem.Text.Contains(APEXFiles.Parameters.ToString)
                    SaveParms() 'Save Parm APEX File.
                Case Page.Request.Params("__EVENTTARGET").Contains("btnSaveContinue") AndAlso ddlFile.SelectedItem.Text.Contains(APEXFiles.Subarea.ToString)
                    SaveSubarea() 'Save Subarea APEX Files.
                Case Page.Request.Params("__EVENTTARGET").Contains("btnSaveContinue") AndAlso ddlFile.SelectedItem.Text.Contains(APEXFiles.Soil.ToString)
                    SaveSoil() 'Save Soil APEX Files.
                Case Page.Request.Params("__EVENTTARGET").Contains("btnSaveContinue") AndAlso ddlFile.SelectedItem.Text.Contains(APEXFiles.Layer.ToString)
                    SaveLayer() 'Save Layers APEX information in soil Files.
                Case Page.Request.Params("__EVENTTARGET").Contains("btnSaveContinue") AndAlso ddlFile.SelectedItem.Text.Contains(APEXFiles.Operation.ToString)
                    SaveOperation() 'Save Operation APEX Files.
                Case Page.Request.Params("__EVENTTARGET").Contains("btnDownloadAPEX")
                    DownloadAPEXFolder() 'Save Operation APEX Files.
                Case Page.Request.Params("__EVENTTARGET").Contains("btnDefaultParms")
                    ResetDefaultParms() 'Reset parm values to database.
                Case Page.Request.Params("__EVENTTARGET").Contains("btnDefaultControl")
                    ResetDefaultControl() 'Reset control values to database.
            End Select
            ArrangeInfo("Save")
        End If
    End Sub

    Private Sub ResetDefaultParms()
        Dim location As New Location
        'Dim simulation As New Simulation
        location.LoadParmFile(_parmValues, _startInfo.StateAbrev)
        'simulation.LoadParmFile(_parmValues)
        ArrangeInfo("Save")
        SelectTable(1)
    End Sub

    Private Sub ResetDefaultControl()
        Dim location As New Location
        location.LoadControlFile(_controlValues, _startInfo.StateAbrev)
        ArrangeInfo("Save")
        SelectTable(0)
    End Sub

    Public Function zipFolder(zipPath As String, type As String) As String
        Try
            Using zip As New Ionic.Zip.ZipFile
                zip.UseUnicodeAsNecessary = True
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression
                zip.Comment = "This zip was created at " & System.DateTime.Now.ToString("G")
                Select Case type.ToLower
                    Case "dndc"
                        zip.AddDirectory(zipPath & "\DnDc")
                        zip.Save(NTTZipFolder & Session("userGuide") & ".zip")
                    Case "apex"
                        zip.AddDirectory(zipPath)
                        zip.Save(NTTZipFolder & Session("userGuide") & ".zip")
                End Select

            End Using
            Return "OK"
        Catch ex As System.Exception
            Return ex.Message
        End Try
    End Function

    Private Sub PrepareFiles(folderPath As String)
        Dim file As String
        Dim folderZip As String = folderPath.Replace("APEX1", "APEX")
        Dim APEXFiles As System.IO.StreamReader
        APEXFiles = New System.IO.StreamReader(folder & "/Resources/APEX1Files.txt")
        System.IO.Directory.CreateDirectory(folderZip)

        Do While APEXFiles.EndOfStream <> True
            file = APEXFiles.ReadLine()
            If file.ToLower.Contains("apex") Or file.ToLower.Contains(".wnd") Or file.ToLower.Contains(".wp1") Or file.ToLower.Contains(".dat") Then
                If System.IO.File.Exists(folderPath & "\" & file.Trim) Then
                    System.IO.File.Copy(folderPath & "\" & file.Trim, folderZip & "\" & file.Trim, True)
                End If
            End If
        Loop

        Dim files As New IO.DirectoryInfo(folderPath)
        For Each file1 As IO.FileInfo In files.GetFiles("*.wp1", IO.SearchOption.AllDirectories)
            System.IO.File.Copy(folderPath & "\" & file1.Name, folderZip & "\" & file1.Name, True)
        Next

        For Each file1 As IO.FileInfo In files.GetFiles("*.wnd", IO.SearchOption.AllDirectories)
            System.IO.File.Copy(folderPath & "\" & file1.Name, folderZip & "\" & file1.Name, True)
        Next

        For Each file1 As IO.FileInfo In files.GetFiles("*.opc", IO.SearchOption.AllDirectories)
            System.IO.File.Copy(folderPath & "\" & file1.Name, folderZip & "\" & file1.Name, True)
        Next

        For Each file1 As IO.FileInfo In files.GetFiles("*.sol", IO.SearchOption.AllDirectories)
            System.IO.File.Copy(folderPath & "\" & file1.Name, folderZip & "\" & file1.Name, True)
        Next
    End Sub

    Private Sub DownloadAPEXFolder()
        Dim sFile As String = String.Empty
        Try
            If Not System.IO.Directory.Exists(NTTFilesFolder + "APEX1" + Session("userGuide")) Then
                showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("FolderNoExist").Value)
                Exit Sub
            End If
            PrepareFiles(NTTFilesFolder + "APEX1" + Session("userGuide"))
            zipFolder(NTTFilesFolder + "APEX" + Session("userGuide"), "APEX")

            sFile = NTTZipFolder & Session("userGuide") & ".zip"
            If IO.File.Exists(sFile) Then
                Response.Clear()
                Response.ContentType = "text/xml"
                Response.AddHeader("Content-Disposition", "attachment;filename=APEX1" & Session("userGuide") & ".zip")
                Response.WriteFile(sFile)
                'Response.TransmitFile(sFile)       'this generate an error 
                Response.End()                     'this generate an error 
            End If
            showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", msgDoc.Descendants("DownloadFolder").Value.Replace("First", "APEX"))

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", ex.Message)
        End Try
    End Sub

    Private Sub SaveControl()
        Dim txtValue1 As HtmlInputText

        For i = 0 To gvControls.Rows.Count - 1
            txtValue1 = gvControls.Rows(i).FindControl("txtValue1")
            _controlValues(i).Value1 = txtValue1.Value
        Next
    End Sub

    Private Sub SaveParms()
        Dim txtValue1 As HtmlInputText

        For i = 0 To gvParms.Rows.Count - 1
            txtValue1 = gvParms.Rows(i).FindControl("txtValue1")
            _parmValues(i).Value1 = txtValue1.Value
        Next
    End Sub

    Private Sub SaveSubarea()
        Dim soilNumber As UShort = 0
        Dim subareaType As String = String.Empty
        For Each row In gvSubarea.Rows
            soilNumber = row.Cells(1).text
            subareaType = row.Cells(0).text
            For Each line In row.Cells(4).Controls
                If line.Controls.Count = 0 Then Continue For
                For Each variable In line.controls(0).controls(1).controls
                    If variable.Controls.Count = 0 Then Continue For
                    If subareaType.Contains("Soil") Then
                        'save subarea info
                        SaveSubareaOrBuffer(soilNumber, line.id, variable, _fieldsInfo1(currentFieldNumber)._soilsInfo(soilNumber)._scenariosInfo(currentScenarioNumber)._subareasInfo)
                    Else
                        'save buffer info
                        SaveSubareaOrBuffer(soilNumber, line.id, variable, _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo(soilNumber))
                    End If
                Next
            Next
        Next
        ArrangeInfo("Save")
    End Sub

    Private Sub SaveSubareaOrBuffer(soilNumber As UShort, lineId As String, variable As Object, subareaInfo As SubareasData)
        Select Case lineId
            Case "gvLine2"
                With subareaInfo._line2(0)
                    Select Case variable.Controls(1).id
                        Case "txtIops"
                            .Iops = variable.controls(1).Value
                        Case "txtInps"
                            .Inps = variable.controls(1).Value
                        Case "txtIapl"
                            .Iapl = variable.controls(1).Value
                        Case "txtIi"
                            .Ii = variable.controls(1).Value
                        Case "txtImw"
                            .Imw = variable.controls(1).Value
                        Case "txtIow"
                            .Iow = variable.controls(1).Value
                        Case "txtIpts"
                            .Ipts = variable.controls(1).Value
                        Case "txtIsao"
                            .Isao = variable.controls(1).Value
                        Case "txtIwth"
                            .Iwth = variable.controls(1).Value
                        Case "txtLuns"
                            .Luns = variable.controls(1).Value
                        Case "txtNvcn"
                            .Nvcn = variable.controls(1).Value
                    End Select
                End With
            Case "gvLine3"
                With subareaInfo._line3(0)
                    Select Case variable.Controls(1).id
                        Case "txtSno"
                            .Sno = variable.controls(1).Value
                        Case "txtStdo"
                            .Stdo = variable.controls(1).Value
                        Case "txtStdo"
                            .Stdo = variable.controls(1).Value
                        Case "txtYct"
                            .Yct = variable.controls(1).Value
                        Case "txtXct"
                            .Xct = variable.controls(1).Value
                        Case "txtAzm"
                            .Azm = variable.controls(1).Value
                        Case "txtFl"
                            .Fl = variable.controls(1).Value
                        Case "txtFw"
                            .Fw = variable.controls(1).Value
                        Case "txtAngl"
                            .Angl = variable.controls(1).Value
                    End Select
                End With
            Case "gvLine4"
                With subareaInfo._line4(0)
                    Select Case variable.Controls(1).id
                        Case "txtWsa"
                            .Wsa = variable.controls(1).Value
                        Case "txtChl"
                            .chl = variable.controls(1).Value
                        Case "txtChd"
                            .Chd = variable.controls(1).Value
                        Case "txtChs"
                            .Chs = variable.controls(1).Value
                        Case "txtChn"
                            .Chn = variable.controls(1).Value
                        Case "txtSlp"
                            .Slp = variable.controls(1).Value
                        Case "txtSlpg"
                            .Slpg = variable.controls(1).Value
                        Case "txtUpn"
                            .Upn = variable.controls(1).Value
                        Case "txtFfpq"
                            .Ffpq = variable.controls(1).Value
                        Case "txtUrbf"
                            .Urbf = variable.controls(1).Value
                    End Select
                End With
            Case "gvLine5"
                With subareaInfo._line5(0)
                    Select Case variable.Controls(1).id
                        Case "txtRchl"
                            .Rchl = variable.controls(1).Value
                        Case "txtRchd"
                            .Rchd = variable.controls(1).Value
                        Case "txtRcbw"
                            .Rcbw = variable.controls(1).Value
                        Case "txtRctw"
                            .Rctw = variable.controls(1).Value
                        Case "txtRchs"
                            .Rchs = variable.controls(1).Value
                        Case "txtRchn"
                            .Rchn = variable.controls(1).Value
                        Case "txtRchc"
                            .Rchc = variable.controls(1).Value
                        Case "txtRchk"
                            .Rchk = variable.controls(1).Value
                        Case "txtRfpw"
                            .Rfpw = variable.controls(1).Value
                        Case "txtRfpl"
                            .Rfpl = variable.controls(1).Value
                    End Select
                End With
            Case "gvLine6"
                With subareaInfo._line6(0)
                    Select Case variable.Controls(1).id
                        Case "txtRsee"
                            .Rsee = variable.controls(1).Value
                        Case "txtRsae"
                            .Rsae = variable.controls(1).Value
                        Case "txtRsve"
                            .Rsve = variable.controls(1).Value
                        Case "txtRsep"
                            .Rsep = variable.controls(1).Value
                        Case "txtRsap"
                            .Rsap = variable.controls(1).Value
                        Case "txtRsvp"
                            .Rsvp = variable.controls(1).Value
                        Case "txtRsv"
                            .Rsv = variable.controls(1).Value
                        Case "txtRsrr"
                            .Rsrr = variable.controls(1).Value
                        Case "txtRsys"
                            .Rsys = variable.controls(1).Value
                        Case "txtRsyn"
                            .Rsyn = variable.controls(1).Value
                    End Select
                End With
            Case "gvLine7"
                With subareaInfo._line7(0)
                    Select Case variable.Controls(1).id
                        Case "txtRshc"
                            .Rshc = variable.controls(1).Value
                        Case "txtRsdp"
                            .Rsdp = variable.controls(1).Value
                        Case "txtRsbd"
                            .Rsbd = variable.controls(1).Value
                        Case "txtPcof"
                            .Pcof = variable.controls(1).Value
                        Case "txtBcof"
                            .Bcof = variable.controls(1).Value
                        Case "txtBffl"
                            .Bffl = variable.controls(1).Value
                    End Select
                End With
            Case "gvLine8"
                With subareaInfo._line8(0)
                    Select Case variable.Controls(1).id
                        Case "txtNirr"
                            .Nirr = variable.controls(1).Value
                        Case "txtIri"
                            .Iri = variable.controls(1).Value
                        Case "txtIfa"
                            .Ifa = variable.controls(1).Value
                        Case "txtLm"
                            .Lm = variable.controls(1).Value
                        Case "txtIfd"
                            .Ifd = variable.controls(1).Value
                        Case "txtIdr"
                            .Idr = variable.controls(1).Value
                        Case "txtIdf1"
                            .Idf1 = variable.controls(1).Value
                        Case "txtIdf2"
                            .Idf2 = variable.controls(1).Value
                        Case "txtIdf3"
                            .Idf3 = variable.controls(1).Value
                        Case "txtIdf4"
                            .Idf4 = variable.controls(1).Value
                        Case "txtIdf5"
                            .Idf5 = variable.controls(1).Value
                    End Select
                End With
            Case "gvLine9"
                With subareaInfo._line9(0)
                    Select Case variable.Controls(1).id
                        Case "txtBir"
                            .Bir = variable.controls(1).Value
                        Case "txtEfi"
                            .Efi = variable.controls(1).Value
                        Case "txtVimx"
                            .Vimx = variable.controls(1).Value
                        Case "txtArmn"
                            .Armn = variable.controls(1).Value
                        Case "txtArmx"
                            .Armx = variable.controls(1).Value
                        Case "txtBft"
                            .Bft = variable.controls(1).Value
                        Case "txtFnp4"
                            .Fnp4 = variable.controls(1).Value
                        Case "txtFmx"
                            .Fmx = variable.controls(1).Value
                        Case "txtDrt"
                            .Drt = variable.controls(1).Value
                        Case "txtFdsf"
                            .Fdsf = variable.controls(1).Value
                    End Select
                End With
            Case "gvLine10"
                With subareaInfo._line10(0)
                    Select Case variable.Controls(1).id
                        Case "txtPec"
                            .Pec = variable.controls(1).Value
                        Case "txtDalg"
                            .Dalg = variable.controls(1).Value
                        Case "txtVlgn"
                            .Vlgn = variable.controls(1).Value
                        Case "txtCoww"
                            .Coww = variable.controls(1).Value
                        Case "txtDdlg"
                            .Ddlg = variable.controls(1).Value
                        Case "txtSolq"
                            .Solq = variable.controls(1).Value
                        Case "txtSflg"
                            .Sflg = variable.controls(1).Value
                        Case "txtFnp2"
                            .Fnp2 = variable.controls(1).Value
                        Case "txtFnp5"
                            .Fnp5 = variable.controls(1).Value
                        Case "txtFirg"
                            .Firg = variable.controls(1).Value
                    End Select
                End With
            Case "gvLine11"
                With subareaInfo._line11(0)
                    Select Case variable.Controls(1).id
                        Case "txtNy1"
                            .Ny1 = variable.controls(1).Value
                        Case "txtNy2"
                            .Ny2 = variable.controls(1).Value
                        Case "txtNy3"
                            .Ny3 = variable.controls(1).Value
                        Case "txtNy4"
                            .Ny4 = variable.controls(1).Value
                    End Select
                End With
            Case "gvLine12"
                With subareaInfo._line12(0)
                    Select Case variable.Controls(1).id
                        Case "txtXtp1"
                            .Xtp1 = variable.controls(1).Value
                        Case "txtXtp2"
                            .Xtp2 = variable.controls(1).Value
                        Case "txtXtp3"
                            .Xtp3 = variable.controls(1).Value
                        Case "txtXtp4"
                            .Xtp4 = variable.controls(1).Value
                    End Select
                End With
        End Select

    End Sub

    Private Sub SaveSoil()
        For Each row In gvSoils.Rows
            For i = 3 To row.Cells.Count - 1
                With _fieldsInfo1(currentFieldNumber)._soilsInfo(row.RowIndex)
                    Select Case row.Cells(i).Controls(1).id
                        Case "txtAlbedo"
                            .Albedo = row.Cells(i).Controls(1).Value()
                            'Case "txtSlope"
                            '    .Slope = row.Cells(i).Controls(1).Value()
                            'Case "txtPercentage"
                            '    .Percentage = row.Cells(i).Controls(1).Value()
                        Case "txtFfc"
                            .Ffc = row.Cells(i).Controls(1).Value()
                        Case "txtWtmn"
                            .Wtmn = row.Cells(i).Controls(1).Value()
                        Case "txtWtmx"
                            .Wtmx = row.Cells(i).Controls(1).Value()
                        Case "txtWtbl"
                            .Wtbl = row.Cells(i).Controls(1).Value()
                        Case "txtGwst"
                            .Gwst = row.Cells(i).Controls(1).Value()
                        Case "txtGwmx"
                            .Gwmx = row.Cells(i).Controls(1).Value()
                        Case "txtRftt"
                            .Rftt = row.Cells(i).Controls(1).Value()
                        Case "txtRfpk"
                            .Rfpk = row.Cells(i).Controls(1).Value()
                        Case "txtXids"
                            .Xids = row.Cells(i).Controls(1).Value()
                        Case "txtRtn1"
                            .Rtn1 = row.Cells(i).Controls(1).Value()
                        Case "txtXidk"
                            .Xidk = row.Cells(i).Controls(1).Value()
                        Case "txtZqt"
                            .Zqt = row.Cells(i).Controls(1).Value()
                        Case "txtZf"
                            .Zf = row.Cells(i).Controls(1).Value()
                        Case "txtZtk"
                            .Ztk = row.Cells(i).Controls(1).Value()
                        Case "txtFbm"
                            .Fbm = row.Cells(i).Controls(1).Value()
                        Case "txtFhp"
                            .Fhp = row.Cells(i).Controls(1).Value()
                    End Select
                End With
            Next
        Next
    End Sub

    Private Sub SaveLayer()
        For Each row In gvLayer.Rows
            For i = 3 To row.Cells.Count - 1
                With _fieldsInfo1(currentFieldNumber)._soilsInfo(currentSoilNumber)._layersInfo(row.RowIndex)
                    Select Case row.Cells(i).Controls(1).id
                        Case "txtDepth"
                            .Depth = row.Cells(i).Controls(1).Value()
                        Case "txtBD"
                            .BD = row.Cells(i).Controls(1).Value()
                        Case "txtUW"
                            .Uw = row.Cells(i).Controls(1).Value()
                        Case "txtSand"
                            .Sand = row.Cells(i).Controls(1).Value()
                        Case "txtSilt"
                            .Silt = row.Cells(i).Controls(1).Value()
                        Case "txtWn"
                            .Wn = row.Cells(i).Controls(1).Value()
                        Case "txtPH"
                            .PH = row.Cells(i).Controls(1).Value()
                        Case "txtSmb"
                            .Smb = row.Cells(i).Controls(1).Value()
                        Case "txtWoc"
                            .Rok = row.Cells(i).Controls(1).Value()
                        Case "txtCac"
                            .Cac = row.Cells(i).Controls(1).Value()
                        Case "txtCec"
                            .Cec = row.Cells(i).Controls(1).Value()
                        Case "txtRok"
                            .Rok = row.Cells(i).Controls(1).Value()
                        Case "txtCnds"
                            .Cnds = row.Cells(i).Controls(1).Value()
                        Case "txtSoilP"
                            .SoilP = row.Cells(i).Controls(1).Value()
                        Case "txtRsd"
                            .Rsd = row.Cells(i).Controls(1).Value()
                        Case "txtBdd"
                            .Bdd = row.Cells(i).Controls(1).Value()
                        Case "txtPsp"
                            .Psp = row.Cells(i).Controls(1).Value()
                        Case "txtSatc"
                            .Satc = row.Cells(i).Controls(1).Value()
                    End Select
                End With
            Next
        Next
    End Sub

    Private Sub SaveOperation()
        For Each row In gvOperation.Rows
            For i = 0 To row.Cells.Count - 1
                With _fieldsInfo1(currentFieldNumber)._soilsInfo(currentSoilNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo(row.RowIndex)
                    Select Case row.Cells(i).Controls(1).id
                        Case "txtYear"
                            .Year = row.Cells(i).Controls(1).Value()
                        Case "txtMonth"
                            .Month = row.Cells(i).Controls(1).Value()
                        Case "txtDay"
                            .Day = row.Cells(i).Controls(1).Value()
                        Case "txtApexOp"
                            .ApexTillCode = row.Cells(i).Controls(1).Value()
                        Case "txtTractorId"
                            .TractorId = row.Cells(i).Controls(1).Value()
                        Case "txtApexCrop"
                            .ApexCrop = row.Cells(i).Controls(1).Value()
                        Case "txtApexOpType"
                            .ApexOpType = row.Cells(i).Controls(1).Value()
                        Case "txtOpVal1"
                            .OpVal1 = row.Cells(i).Controls(1).Value()
                        Case "txtOpVal2"
                            .OpVal2 = row.Cells(i).Controls(1).Value()
                        Case "txtOpVal3"
                            .OpVal3 = row.Cells(i).Controls(1).Value()
                        Case "txtOpVal4"
                            .OpVal4 = row.Cells(i).Controls(1).Value()
                        Case "txtOpVal5"
                            .OpVal5 = row.Cells(i).Controls(1).Value()
                        Case "txtOpVal6"
                            .OpVal6 = row.Cells(i).Controls(1).Value()
                        Case "txtOpVal7"
                            .OpVal7 = row.Cells(i).Controls(1).Value()
                    End Select
                End With
            Next
        Next
    End Sub

    Protected Sub ddlType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlFile.TextChanged
        If ddlSoils.Items.Count > 0 Then ddlSoils.SelectedIndex = 0 : currentSoilNumber = 0 Else ddlSoils.SelectedIndex = -1 : currentSoilNumber = -1
        Select Case ddlFile.SelectedIndex
            Case 2
                LoadScenarios(ddlScenarios, _fieldsInfo1(currentFieldNumber)._scenariosInfo, currentScenarioNumber)
            Case 5
                LoadSoils()
                LoadScenarios(ddlScenarios, _fieldsInfo1(currentFieldNumber)._scenariosInfo, currentScenarioNumber)
        End Select
        ddlFields.SelectedIndex = 0
        currentFieldNumber = 0
        SelectTable(ddlFile.SelectedIndex)
    End Sub

    Private Sub ddlFields_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlFields.SelectedIndexChanged
        currentFieldNumber = ddlFields.SelectedIndex
        Session("currentFieldNumber") = currentFieldNumber
        LoadSoils()
        LoadScenarios(ddlScenarios, _fieldsInfo1(currentFieldNumber)._scenariosInfo, currentScenarioNumber)
        SelectTable(ddlFile.SelectedIndex)
    End Sub

    Private Sub ddlScenario_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlScenarios.SelectedIndexChanged
        currentScenarioNumber = ddlScenarios.SelectedIndex
        Session("currentScenarioNumber") = currentScenarioNumber
        SelectTable(ddlFile.SelectedIndex)
    End Sub

    Private Sub ddlSoils_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlSoils.SelectedIndexChanged
        currentSoilNumber = ddlSoils.SelectedIndex
        If ddlScenarios.Items.Count > 0 Then
            ddlScenarios.SelectedIndex = 0
            currentScenarioNumber = ddlScenarios.SelectedIndex
        End If

        SelectTable(ddlFile.SelectedIndex)
    End Sub

    Private Sub LoadSoils()
        ddlSoils.Items.Clear()
        For Each Soil In _fieldsInfo1(currentFieldNumber)._soilsInfo.Where(Function(x) x.Selected)
            ddlSoils.Items.Add(Soil.Symbol & "  " & Soil.Name & "  " & Soil.Component & " " & Soil.Slope)
        Next
        ddlSoils.SelectedIndex = 0
        currentSoilNumber = 0
    End Sub

    Private Sub SelectTable(index)
        lblFields.Style.Item("display") = "none"
        lblSubarea.Style.Item("display") = "none"
        lblScenarios.Style.Item("display") = "none"
        lblSoils.Style.Item("display") = "none"
        ddlFields.Style.Item("display") = "none"
        ddlScenarios.Style.Item("display") = "none"
        ddlSoils.Style.Item("display") = "none"

        gvControls.DataSource = Nothing
        gvControls.DataBind()
        fsetControlfile.Style.Item("display") = "none"

        gvParms.DataSource = Nothing
        gvParms.DataBind()
        fsetParamfile.Style.Item("display") = "none"

        gvSubarea.DataSource = Nothing
        gvSubarea.DataBind()
        fsetSubareafile.Style.Item("display") = "none"

        gvSoils.DataSource = Nothing
        gvSoils.DataBind()
        fsetSoilfile.Style.Item("display") = "none"

        gvLayer.DataSource = Nothing
        gvLayer.DataBind()
        fsetLayerfile.Style.Item("display") = "none"

        gvOperation.DataSource = Nothing
        gvOperation.DataBind()
        fsetOperationfile.Style.Item("display") = "none"

        Select Case ddlFile.SelectedIndex
            Case 0 'Control File
                gvControls.DataSource = _controlValues
                gvControls.DataBind()
                fsetControlfile.Style.Item("display") = ""
            Case 1 'Parameters File
                gvParms.DataSource = _parmValues
                gvParms.DataBind()
                fsetParamfile.Style.Item("display") = ""
            Case 2 'Subarea File
                lblFields.Style.Item("display") = ""
                lblScenarios.Style.Item("display") = ""
                ddlFields.Style.Item("display") = ""
                ddlScenarios.Style.Item("display") = ""
                fsetSubareafile.Style.Item("display") = ""
                Dim _subarea As SubareasData
                Dim l As UShort = 0
                If Not (_fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.CBCWidth > 0 And _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.CBBWidth > 0 And _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bmpsInfo.CBCrop > 0) Then
                    For Each soil In _fieldsInfo1(currentFieldNumber)._soilsInfo
                        If soil.Selected = True Then
                            _subarea = New SubareasData
                            _subarea = soil._scenariosInfo(currentScenarioNumber)._subareasInfo
                            _subarea.SubareaNumber = l  'Assign the soil number to be sure the modifications are done in the correct soil information
                            _subareasInfo.Add(_subarea)
                        End If
                        l += 1
                    Next
                End If

                l = 0
                For Each buffer1 In _fieldsInfo1(currentFieldNumber)._scenariosInfo(currentScenarioNumber)._bufferInfo
                    If buffer1.SbaType <> "Soil" And buffer1.SbaType <> "" Then
                        _subarea = New SubareasData
                        _subarea = buffer1
                        _subarea.SubareaNumber = l
                        _subareasInfo.Add(_subarea)
                    End If
                    l += 1
                Next
                gvSubarea.DataSource = _subareasInfo
                gvSubarea.DataBind()
            Case 3 'Soil file
                lblFields.Style.Item("display") = ""
                ddlFields.Style.Item("display") = ""
                fsetSoilfile.Style.Item("display") = ""
                gvSoils.DataSource = _fieldsInfo1(currentFieldNumber)._soilsInfo
                gvSoils.DataBind()
            Case 4 'Layer file
                lblFields.Style.Item("display") = ""
                ddlFields.Style.Item("display") = ""
                lblSoils.Style.Item("display") = ""
                ddlSoils.Style.Item("display") = ""
                If ddlSoils.Items.Count <= 0 Then LoadSoils()
                fsetLayerfile.Style.Item("display") = ""
                gvLayer.DataSource = _fieldsInfo1(currentFieldNumber)._soilsInfo(currentSoilNumber)._layersInfo
                gvLayer.DataBind()
            Case 5 'Operation File
                lblFields.Style.Item("display") = ""
                ddlFields.Style.Item("display") = ""
                lblScenarios.Style.Item("display") = ""
                ddlScenarios.Style.Item("display") = ""
                lblSoils.Style.Item("display") = ""
                ddlSoils.Style.Item("display") = ""
                lblScenarios.Style.Item("display") = ""
                ddlScenarios.Style.Item("display") = ""
                fsetOperationfile.Style.Item("display") = ""
                gvOperation.DataSource = _fieldsInfo1(currentFieldNumber)._soilsInfo(currentSoilNumber)._scenariosInfo(currentScenarioNumber)._operationsInfo
                gvOperation.DataBind()
        End Select
    End Sub

    Private Sub ChangeLanguageContent()
        lblFields.Text = cntDoc.Descendants("Field").Value
        lblFile.Text = cntDoc.Descendants("File").Value
        lblSoils.Text = cntDoc.Descendants("Soil").Value
        btnSaveContinue.Text = cntDoc.Descendants("SaveFile").Value
        btnDownloadAPEX1.Value = cntDoc.Descendants("DownloadAPEX").Value
        lblControl.InnerHtml = cntDoc.Descendants("ControlFile").Value
        lblParmFile.InnerHtml = cntDoc.Descendants("ParametersFile").Value
        lblSubarea.InnerHtml = cntDoc.Descendants("SubareaFile").Value
        lblSoil.InnerHtml = cntDoc.Descendants("SoilFile").Value
        lblLayerFile.InnerHtml = cntDoc.Descendants("LayerFile").Value
        lblOperation.InnerHtml = cntDoc.Descendants("OperationFile").Value
        'Cotrols
        gvControls.Columns(0).HeaderText = cntDoc.Descendants("Code").Value
        gvControls.Columns(1).HeaderText = cntDoc.Descendants("Name").Value
        gvControls.Columns(2).HeaderText = cntDoc.Descendants("Value1").Value
        gvControls.Columns(3).HeaderText = cntDoc.Descendants("Range1").Value
        gvControls.Columns(4).HeaderText = cntDoc.Descendants("Range2").Value
        'Parameters
        gvParms.Columns(0).HeaderText = cntDoc.Descendants("Code").Value
        gvParms.Columns(1).HeaderText = cntDoc.Descendants("Name").Value
        gvParms.Columns(2).HeaderText = cntDoc.Descendants("Value1").Value
        gvParms.Columns(3).HeaderText = cntDoc.Descendants("Range1").Value
        gvParms.Columns(4).HeaderText = cntDoc.Descendants("Range2").Value
        'Subareas
        gvSubarea.Columns(0).HeaderText = cntDoc.Descendants("Type").Value
        gvSubarea.Columns(2).HeaderText = cntDoc.Descendants("Title").Value
        gvSubarea.Columns(3).HeaderText = cntDoc.Descendants("Action").Value
        gvSubarea.Columns(4).HeaderText = cntDoc.Descendants("Detail").Value
        'Soils
        gvSoils.Columns(0).HeaderText = cntDoc.Descendants("Name").Value
        gvSoils.Columns(1).HeaderText = cntDoc.Descendants("Soil").Value & "#"
        'Layers
        gvLayer.Columns(0).HeaderText = cntDoc.Descendants("LayerNumber").Value
        gvLayer.Columns(1).HeaderText = cntDoc.Descendants("LayerDepth").Value
        'Layers
        gvOperation.Columns(0).HeaderText = cntDoc.Descendants("Year").Value
        gvOperation.Columns(1).HeaderText = cntDoc.Descendants("Month").Value
        gvOperation.Columns(2).HeaderText = cntDoc.Descendants("Day").Value
        gvOperation.Columns(3).HeaderText = cntDoc.Descendants("Operation").Value
        gvOperation.Columns(5).HeaderText = cntDoc.Descendants("Crop").Value
    End Sub

    Private Sub gvSubarea_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSubarea.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'e.Row.Cells(3).Attributes.Add("onclick", "TurnTableOnOff(" & e.Row.RowIndex.ToString() & ")")

            Dim btnShow As HtmlInputButton = e.Row.FindControl("btnShow")
            If Not btnShow Is Nothing Then
                btnShow.Attributes.Add("onclick", "TurnTableOnOff(" & e.Row.RowIndex.ToString() & ")")
                btnShow.Value = cntDoc.Descendants("Show").Value
            End If

            e.Row.Cells(4).Style.Item("display") = "none"
            Dim gvLine2 As New GridView
            Dim gvLine3 As New GridView
            Dim gvLine4 As New GridView
            Dim gvLine5 As New GridView
            Dim gvLine6 As New GridView
            Dim gvLine7 As New GridView
            Dim gvLine8 As New GridView
            Dim gvLine9 As New GridView
            Dim gvLine10 As New GridView
            Dim gvLine11 As New GridView
            Dim gvLine12 As New GridView

            gvLine2 = TryCast(e.Row.FindControl("gvLine2"), GridView)
            gvLine2.DataSource = _subareasInfo(e.Row.RowIndex)._line2
            gvLine2.DataBind()

            gvLine3 = TryCast(e.Row.FindControl("gvLine3"), GridView)
            gvLine3.DataSource = _subareasInfo(e.Row.RowIndex)._line3
            gvLine3.DataBind()

            gvLine4 = TryCast(e.Row.FindControl("gvLine4"), GridView)
            gvLine4.DataSource = _subareasInfo(e.Row.RowIndex)._line4
            gvLine4.DataBind()

            gvLine5 = TryCast(e.Row.FindControl("gvLine5"), GridView)
            gvLine5.DataSource = _subareasInfo(e.Row.RowIndex)._line5
            gvLine5.DataBind()

            gvLine6 = TryCast(e.Row.FindControl("gvLine6"), GridView)
            gvLine6.DataSource = _subareasInfo(e.Row.RowIndex)._line6
            gvLine6.DataBind()

            gvLine7 = TryCast(e.Row.FindControl("gvLine7"), GridView)
            gvLine7.DataSource = _subareasInfo(e.Row.RowIndex)._line7
            gvLine7.DataBind()

            gvLine8 = TryCast(e.Row.FindControl("gvLine8"), GridView)
            gvLine8.DataSource = _subareasInfo(e.Row.RowIndex)._line8
            gvLine8.DataBind()

            gvLine9 = TryCast(e.Row.FindControl("gvLine9"), GridView)
            gvLine9.DataSource = _subareasInfo(e.Row.RowIndex)._line9
            gvLine9.DataBind()

            gvLine10 = TryCast(e.Row.FindControl("gvLine10"), GridView)
            gvLine10.DataSource = _subareasInfo(e.Row.RowIndex)._line10
            gvLine10.DataBind()

            gvLine11 = TryCast(e.Row.FindControl("gvLine11"), GridView)
            gvLine11.DataSource = _subareasInfo(e.Row.RowIndex)._line11
            gvLine11.DataBind()

            gvLine12 = TryCast(e.Row.FindControl("gvLine12"), GridView)
            gvLine12.DataSource = _subareasInfo(e.Row.RowIndex)._line12
            gvLine12.DataBind()

        End If
    End Sub

    'Private Sub gvSoils_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSoils.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.DataItem.slope = Math.Round(e.Row.DataItem.slope, 2)
    '    End If
    'End Sub

    Private Sub ArrangeInfo(openSave As String)
        Select Case openSave
            'this is done after the open subrutine
            Case "Open"
                _startInfo = Session("projects")._StartInfo
                _fieldsInfo1 = Session("projects")._fieldsInfo1
                _subprojectName = Session("projects")._subprojectName
                _sitesInfo = Session("projects")._sitesInfo
                _parmValues = Session("projects")._parmValues
                _controlValues = Session("projects")._controlValues
                _otherTemp = Session("projects")._otherTemp
                _structureTemp = Session("projects")._structureTemp
                _feedTemp = Session("projects")._feedTemp
                _equipmentTemp = Session("projects")._equipmentTemp
                'this is done before the save subroutine
            Case "Save"
                Session("projects")._StartInfo = _startInfo
                Session("projects")._fieldsInfo1 = _fieldsInfo1
                Session("projects")._subprojectName = _subprojectName
                Session("projects")._sitesInfo = _sitesInfo
                Session("projects")._parmValues = _parmValues
                Session("projects")._controlValues = _controlValues
                Session("projects")._otherTemp = _otherTemp
                Session("projects")._structureTemp = _structureTemp
                Session("projects")._feedTemp = _feedTemp
                Session("projects")._equipmentTemp = _equipmentTemp
        End Select
    End Sub
End Class