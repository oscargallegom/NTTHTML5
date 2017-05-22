Public Class Field
    Inherits System.Web.UI.Page
    'Classes definition
    Private _project As New ProjectsData
    Private _startInfo As New StartInfo
    Private _fieldsInfo1 As New List(Of FieldsData)
    Private currentFieldNumber As Short
    'Controls used in sitemaster page
    Private lblMessage As Label
    Private imgIcon As Image

    Public Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
        ArrangeInfo("Open")
        Select Case True
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("btnSave")
                btnSave_Click("Yes")
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("btnContinue")
                btnContinue_Click()
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("chkForestry")
                Forestry_Checked(_startInfo)
                btnSave_Click("")
            Case Not Page.Request.Params("__EVENTARGUMENT") Is Nothing AndAlso Page.Request.Params("__EVENTARGUMENT").Contains("Delete")
                btnSave_Click("")       'save first to be sure the values from screan are being taken
                gvField_RowDeleting(Page.Request.Params("__EVENTARGUMENT").Split("$")(1))
                ArrangeInfo("Save")
            Case Not Page.Request.Params("__EVENTTARGET") Is Nothing AndAlso Page.Request.Params("__EVENTTARGET").Contains("btnAddField")
                btnAddField_ServerClick()
                btnSave_Click("")
        End Select

        openXMLLanguagesFile()
        ChangeLanguageContent()
        LoadDataFields()
    End Sub

    Protected Sub btnSave_Click(ByVal cont As String)
        Try
            'soil text boxes
            Dim tTXTName As HtmlInputText
            Dim tTXTArea As HtmlInputText
            Dim tDdlRchc As DropDownList
            Dim tDdlRchk As DropDownList

            'take all of the fields from gvFields and put them in a temporary fields class - _gvFields
            For i = 0 To gvFields.Rows.Count - 1
                'update fields information
                tTXTName = DirectCast(gvFields.Rows(i).FindControl("txtName"), HtmlInputText)
                tTXTArea = DirectCast(gvFields.Rows(i).FindControl("txtArea"), HtmlInputText)
                tDdlRchc = DirectCast(gvFields.Rows(i).FindControl("ddlRchc"), DropDownList)
                tDdlRchk = DirectCast(gvFields.Rows(i).FindControl("ddlRchk"), DropDownList)
                _fieldsInfo1(i).Name = tTXTName.Value
                _fieldsInfo1(i).Number = i + 1
                _fieldsInfo1(i).Area = tTXTArea.Value
                _fieldsInfo1(i).AvgSlope = gvFields.Rows(i).Cells(6).Text
                _fieldsInfo1(i).Rchc = tDdlRchc.SelectedItem.Text
                _fieldsInfo1(i).Rchk = tDdlRchk.SelectedItem.Text
                _fieldsInfo1(i).RchcVal = tDdlRchc.SelectedItem.Value
                _fieldsInfo1(i).RchkVal = tDdlRchk.SelectedItem.Value
                'update changes for rchc and rchk if they changed.
                For Each soilTemp In _fieldsInfo1(i)._soilsInfo
                    For Each scenarioTemp In soilTemp._scenariosInfo
                        scenarioTemp._subareasInfo._line5(0).Rchc = _fieldsInfo1(i).RchcVal
                    Next
                Next
                'validate name and area
                If _fieldsInfo1(i).Name = "" Or _fieldsInfo1(i).Name = String.Empty Then Throw New Global.System.Exception(msgDoc.Descendants("FieldName").Value)
                If _fieldsInfo1(i).Area <= 0 Then Throw New Global.System.Exception(msgDoc.Descendants("FieldArea").Value)
            Next

            ''verify what fields were deleted from the original list to delete them from the original list
            'For i = _fieldsInfo1.Count - 1 To 0 Step -1
            '    found = False
            '    For Each fieldgv In _gvFields
            '        If _fieldsInfo1(i).Name.Trim = fieldgv.Name.Trim Then
            '            found = True
            '        End If
            '    Next
            '    If found = False Then _fieldsInfo1.RemoveAt(i) 'Delete field no in gridview
            'Next

            ''check what fields are new and add them to the permanent field list and check what should be just modified. It keeps all the other information unthoched such as soils, scenarios, etc.
            'For Each fieldgv In _gvFields
            '    found = False
            '    For Each field1 In _fieldsInfo1
            '        If field1.Name.Trim = fieldgv.Name.Trim Then
            '            found = True    'then the field is modified
            '            field1.Name = fieldgv.Name
            '            field1.Number = fieldgv.Number
            '            field1.Area = fieldgv.Area
            '            field1.AvgSlope = fieldgv.AvgSlope
            '            field1.Rchc = fieldgv.Rchc
            '            field1.Rchk = fieldgv.Rchk
            '            field1.RchcVal = fieldgv.RchcVal
            '            field1.RchkVal = fieldgv.RchkVal
            '            Exit For
            '        End If
            '    Next
            '    If found = False Then
            '        _gvField = New FieldsData
            '        'update fields information
            '        _gvField.Name = fieldgv.Name
            '        _gvField.Number = fieldgv.Number
            '        _gvField.Area = fieldgv.Area
            '        _gvField.AvgSlope = fieldgv.AvgSlope
            '        _gvField.Rchc = fieldgv.Rchc
            '        _gvField.Rchk = fieldgv.Rchk
            '        _gvField.RchcVal = fieldgv.RchcVal
            '        _gvField.RchkVal = fieldgv.RchkVal
            '        _fieldsInfo1.Add(_gvField)
            '    End If
            'Next

            ArrangeInfo("Save")
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", ex.Message)
        End Try
    End Sub

    Protected Sub btnContinue_Click()
        Response.Redirect("Soil.aspx", False)
    End Sub

    Private Sub LoadDataFields()
        gvFields.DataSource = _fieldsInfo1
        gvFields.DataBind()
        If _fieldsInfo1.Count > 0 Then Session("currentFieldNumber") = 0
    End Sub

    Private Sub ChangeLanguageContent()
        btnAddField.Value = cntDoc.Descendants("AddNewField").Value
        btnSave.Text = cntDoc.Descendants("Save").Value
        btnContinue.Text = cntDoc.Descendants("Continue").Value
        lblFieldsInfo.InnerText = cntDoc.Descendants("FieldsInfo").Value
        gvFields.Columns(0).HeaderText = cntDoc.Descendants("Delete").Value
        gvFields.Columns(2).HeaderText = cntDoc.Descendants("FieldName").Value
        gvFields.Columns(3).HeaderText = cntDoc.Descendants("AreaHeading").Value
        gvFields.Columns(4).HeaderText = cntDoc.Descendants("ChannelErodibility").Value
        gvFields.Columns(5).HeaderText = cntDoc.Descendants("ChannelCondition").Value
        gvFields.Columns(6).HeaderText = cntDoc.Descendants("AverageSlope").Value
        gvFields.Columns(7).HeaderText = cntDoc.Descendants("Forestry").Value
        'tooltips
        btnAddField.Attributes.Add("title", msgDoc.Descendants("ttAddField").Value)
        btnSave.ToolTip = msgDoc.Descendants("ttSaveAndContinue").Value
    End Sub

    Protected Sub gvField_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        'I have to keep this subrourtine to avoid a crash but it does nothing - 07152015
    End Sub

    Protected Sub gvField_RowDeleting(fieldNumber)
        _fieldsInfo1.RemoveAt(fieldNumber)
    End Sub

    Private Sub gvFields_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFields.RowDataBound
        Dim result As Single = 0
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim txtName As HtmlInputText
            Dim txtArea As HtmlInputText
            txtName = e.Row.FindControl("txtName")
            txtArea = e.Row.FindControl("txtArea")
            If txtName.Value = "" Or txtName.Value = String.Empty Then txtName.Style.Item("background-color") = "Red"
            If Single.TryParse(txtArea.Value, result) Then
                If txtArea.Value <= 0 Then txtArea.Style.Item("background-color") = "Red"
            Else
                txtArea.Style.Item("background-color") = "Red"
            End If
        End If
    End Sub

    Private Sub gvFields_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFields.RowCreated
        Dim item As ListItem

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Attributes.Add("onclick", "return DeleteRow(" & e.Row.RowIndex.ToString() & ")")
            e.Row.Cells(0).Controls(0).ID = "btnDelete"
            Dim btnDelete As New Object
            btnDelete = e.Row.Cells(0).FindControl("btnDelete")
            btnDelete.Text = cntDoc.Descendants("Delete").Value

            Dim chkBox As New HtmlInputCheckBox
            chkBox = e.Row.FindControl("chkForestry")
            If _fieldsInfo1.Count > 0 AndAlso (_fieldsInfo1(e.Row.RowIndex).Name.Contains(road) Or _fieldsInfo1(e.Row.RowIndex).Name.Contains(smz)) Then
                Dim txtBox As HtmlInputText
                chkBox.Disabled = True
                txtBox = e.Row.FindControl("txtName")
                txtBox.Attributes.Item("readonly") = "readonly"
            End If
            chkBox.Attributes.Add("onclick", "onClick(" & e.Row.RowIndex.ToString() & ")")
            'e.Row.Attributes.Add("onclick", "onClick(" & e.Row.RowIndex.ToString() & ")")
            Dim ddlRchc As DropDownList = e.Row.FindControl("ddlRchc")
            If Not ddlRchc Is Nothing Then
                item = New ListItem
                item.Text = cntDoc.Descendants("VeryLowChannelErodibility").Value
                item.Value = 0.0001
                ddlRchc.Items.Add(item)

                item = New ListItem
                item.Text = cntDoc.Descendants("LowChannelErodibility").Value
                item.Value = 0.001
                ddlRchc.Items.Add(item)

                item = New ListItem
                item.Text = cntDoc.Descendants("ModerateChannelErodibility").Value
                item.Value = 0.2
                ddlRchc.Items.Add(item)

                item = New ListItem
                item.Text = cntDoc.Descendants("HighChannelErodibility").Value
                item.Value = 0.4
                ddlRchc.Items.Add(item)

                item = New ListItem
                item.Text = cntDoc.Descendants("VeryHighChannelErodibility").Value
                item.Value = 0.6
                ddlRchc.Items.Add(item)
            End If

            Dim ddlRchk As DropDownList = e.Row.FindControl("ddlRchk")
            If Not ddlRchk Is Nothing Then
                item = New ListItem
                item.Text = cntDoc.Descendants("VeryPoorChannelVegetation").Value
                item.Value = 0.0001
                ddlRchk.Items.Add(item)

                item = New ListItem
                item.Text = cntDoc.Descendants("PoorChannelVegetation").Value
                item.Value = 0.001
                ddlRchk.Items.Add(item)

                item = New ListItem
                item.Text = cntDoc.Descendants("ModerateChannelVegetation").Value
                item.Value = 0.2
                ddlRchk.Items.Add(item)

                item = New ListItem
                item.Text = cntDoc.Descendants("GoodChannelVegetation").Value
                item.Value = 0.4
                ddlRchk.Items.Add(item)

                item = New ListItem
                item.Text = cntDoc.Descendants("VeryGoodChannelVegetation").Value
                item.Value = 0.6
                ddlRchk.Items.Add(item)
            End If
            'hide cells not used at this moment.
            e.Row.Cells(4).Visible = False
            e.Row.Cells(5).Visible = False
            'add tooltips
            e.Row.Cells(0).ToolTip = msgDoc.Descendants("ttDelete").Value.Replace("first", "_field")
            e.Row.Cells(2).ToolTip = msgDoc.Descendants("ttFieldName").Value
            e.Row.Cells(3).ToolTip = msgDoc.Descendants("ttFieldArea").Value
            e.Row.Cells(6).ToolTip = msgDoc.Descendants("ttFieldAverage").Value
            e.Row.Cells(7).ToolTip = msgDoc.Descendants("ttFieldForestry").Value
        End If
    End Sub

    Private Sub btnAddField_ServerClick()
        AddNewField()
    End Sub

    Private Sub AddNewField()
        Try
            Dim fieldData As FieldsData
            'Dim data1 As List(Of FieldsData) = New List(Of FieldsData)

            fieldData = New FieldsData
            If _fieldsInfo1.Count > 0 Then
                fieldData.Number = _fieldsInfo1(_fieldsInfo1.Count - 1).Number
            Else
                fieldData.Number = 0
            End If
            fieldData.Selected = True
            fieldData.Number += 1
            'fieldData.Number = lastField
            fieldData.Area = 0
            fieldData.Coordinates = "None"
            fieldData.Name = cntDoc.Descendants("NewFieldName").Value & fieldData.Number
            'If strLanguage <> spanish Then fieldData.Name = My.Resources.ContentResourcesSpanish.NewFieldName & fieldData.Number
            fieldData.Rchc = cntDoc.Descendants("ModerateChannelVegetation").Value
            'If strLanguage <> spanish Then fieldData.Rchc = My.Resources.ContentResourcesSpanish.ModerateChannelVegetation
            fieldData.RchcVal = 0.2
            fieldData.Rchk = cntDoc.Descendants("ModerateChannelErodibility").Value
            'If strLanguage <> spanish Then fieldData.Rchk = My.Resources.ContentResourcesSpanish.ModerateChannelErodibility
            fieldData.RchkVal = 0.2
            fieldData.Soils = New Button
            _fieldsInfo1.Add(fieldData)
            'LoadDataFields()
            showMessage(lblMessage, imgIcon, "Green", "GoIcon.jpg", "Field " & msgDoc.Descendants("RecordAdded").Value)

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try

    End Sub

    Public Sub Forestry_Checked(ByRef _StartInfo As StartInfo)
        btnSave_Click("No")
        currentFieldNumber = Page.Request.Params("__EVENTARGUMENT")
        Dim fieldName As String = _fieldsInfo1(currentFieldNumber).Name & road
        Dim _currentSoilInfo As SoilsData
        Dim chkForestry As HtmlInputCheckBox = gvFields.Rows(currentFieldNumber).FindControl("chkForestry")


        If chkForestry.Checked = True Then
            _fieldsInfo1(currentFieldNumber).Forestry = True
            'add route field
            AddNewField()
            _fieldsInfo1(_fieldsInfo1.Count - 1).Forestry = True
            _fieldsInfo1(_fieldsInfo1.Count - 1).Selected = True
            _fieldsInfo1(_fieldsInfo1.Count - 1).Name = fieldName
            _fieldsInfo1(_fieldsInfo1.Count - 1).Area = _fieldsInfo1(currentFieldNumber).Area * 0.05
            _fieldsInfo1(_fieldsInfo1.Count - 1).Number = _fieldsInfo1(currentFieldNumber).Number
            _fieldsInfo1(_fieldsInfo1.Count - 1).Rchc = _fieldsInfo1(currentFieldNumber).Rchc
            _fieldsInfo1(_fieldsInfo1.Count - 1).RchcVal = _fieldsInfo1(currentFieldNumber).RchcVal
            _fieldsInfo1(_fieldsInfo1.Count - 1).Rchk = _fieldsInfo1(currentFieldNumber).Rchk
            _fieldsInfo1(_fieldsInfo1.Count - 1).RchkVal = _fieldsInfo1(currentFieldNumber).RchkVal
            _fieldsInfo1(_fieldsInfo1.Count - 1).AvgSlope = _fieldsInfo1(currentFieldNumber).AvgSlope

            Dim soilPage As New Soil
            For Each Soil In _fieldsInfo1(currentFieldNumber)._soilsInfo
                _currentSoilInfo = New SoilsData
                _currentSoilInfo.Tsla = 10
                _currentSoilInfo.Xids = 1
                _currentSoilInfo.Key = Soil.Key
                _currentSoilInfo.Symbol = Soil.Symbol
                _currentSoilInfo.Component = Soil.Component
                _currentSoilInfo.Albedo = Soil.Albedo
                _currentSoilInfo.Slope = Soil.Slope
                _currentSoilInfo.Percentage = Soil.Percentage
                _currentSoilInfo.Name = Soil.Name
                _currentSoilInfo.Group = Soil.Group
                _fieldsInfo1(_fieldsInfo1.Count - 1).AddSoil(_currentSoilInfo, _StartInfo, _fieldsInfo1(currentFieldNumber).Area, _fieldsInfo1(currentFieldNumber).RchcVal, _fieldsInfo1(currentFieldNumber).RchkVal)
            Next
            'add CMZ field
            fieldName = _fieldsInfo1(currentFieldNumber).Name & smz
            AddNewField()
            _fieldsInfo1(_fieldsInfo1.Count - 1).Forestry = True
            _fieldsInfo1(_fieldsInfo1.Count - 1).Selected = True
            _fieldsInfo1(_fieldsInfo1.Count - 1).Name = fieldName
            _fieldsInfo1(_fieldsInfo1.Count - 1).Area = _fieldsInfo1(currentFieldNumber).Area * 0.1
            _fieldsInfo1(_fieldsInfo1.Count - 1).Number = _fieldsInfo1(currentFieldNumber).Number
            _fieldsInfo1(_fieldsInfo1.Count - 1).Rchc = _fieldsInfo1(currentFieldNumber).Rchc
            _fieldsInfo1(_fieldsInfo1.Count - 1).RchcVal = _fieldsInfo1(currentFieldNumber).RchcVal
            _fieldsInfo1(_fieldsInfo1.Count - 1).Rchk = _fieldsInfo1(currentFieldNumber).Rchk
            _fieldsInfo1(_fieldsInfo1.Count - 1).RchkVal = _fieldsInfo1(currentFieldNumber).RchkVal
            _fieldsInfo1(_fieldsInfo1.Count - 1).AvgSlope = _fieldsInfo1(currentFieldNumber).AvgSlope
            For Each soil In _fieldsInfo1(currentFieldNumber)._soilsInfo
                _currentSoilInfo = New SoilsData
                _currentSoilInfo.Tsla = 10
                _currentSoilInfo.Xids = 1
                _currentSoilInfo.Key = soil.Key
                _currentSoilInfo.Symbol = soil.Symbol
                _currentSoilInfo.Component = soil.Component
                _currentSoilInfo.Albedo = soil.Albedo
                _currentSoilInfo.Slope = soil.Slope
                _currentSoilInfo.Percentage = soil.Percentage
                _currentSoilInfo.Name = soil.Name
                _currentSoilInfo.Group = soil.Group
                _fieldsInfo1(_fieldsInfo1.Count - 1).AddSoil(_currentSoilInfo, _StartInfo, _fieldsInfo1(currentFieldNumber).Area, _fieldsInfo1(currentFieldNumber).RchcVal, _fieldsInfo1(currentFieldNumber).RchkVal)
            Next
        Else
            _fieldsInfo1(currentFieldNumber).Forestry = False
            'delete route field
            fieldName = _fieldsInfo1(currentFieldNumber).Name & road
            For i = 0 To _fieldsInfo1.Count - 1
                If _fieldsInfo1(i).Name = fieldName Then
                    _fieldsInfo1.RemoveAt(i)
                    Exit For
                End If
            Next
            'delete route field
            fieldName = _fieldsInfo1(currentFieldNumber).Name & smz
            For i = 0 To _fieldsInfo1.Count - 1
                If _fieldsInfo1(i).Name = fieldName Then
                    _fieldsInfo1.RemoveAt(i)
                    Exit For
                End If
            Next
        End If
        For i = 0 To _fieldsInfo1.Count - 1
            _fieldsInfo1(i).Number = i + 1
        Next
        gvFields.DataBind()
        gvFields.SelectedIndex = 0
    End Sub
    Private Sub ArrangeInfo(openSave As String)
        Select Case openSave
            'this is done after the open subrutine
            Case "Open"
                _startInfo = Session("projects")._StartInfo
                _fieldsInfo1 = Session("projects")._fieldsInfo1
                'this is done before the save subroutine
            Case "Save"
                Session("projects")._StartInfo = _startInfo
                Session("projects")._fieldsInfo1 = _fieldsInfo1
        End Select
    End Sub

End Class