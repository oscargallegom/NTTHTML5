Public Class Economics1
    Inherits System.Web.UI.Page
    Public _equipmentTemp As List(Of EquipmentData)
    Public _feedTemp As List(Of FeedData)
    Public _structureTemp As List(Of StructureData)
    Public _otherTemp As List(Of OtherData)
    Public _equipment As New List(Of EquipmentData)
    Public _feed As New List(Of FeedData)
    Public _structure As New List(Of StructureData)
    Public _other As New List(Of OtherData)

    'Controls used in sitemaster page
    Private lblMessage As Label
    Private imgIcon As Image

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        folder = My.Computer.FileSystem.GetParentPath(Server.MapPath(""))
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

        ArrangeInfo("Open")
        Select Case Page.Request.Params("__EVENTARGUMENT")
            Case english
                Session("Language") = english
            Case spanish
                Session("Language") = spanish
        End Select
        Select Case hdnSave.Value
            Case "1"
                SaveCheckRows(gvFeeds, "feeds")
            Case "2"
                SaveCheckRows(gvEquipment1, "equipment")
            Case "3"
                SaveCheckRows(gvStructure, "structure")
            Case "4"
                SaveCheckRows(gvOtherInputs, "other")
        End Select

        If Not Page.Request.Params("__EVENTTARGET") Is Nothing Then
            Select Case True
                Case Page.Request.Params("__EVENTTARGET").Contains("btnSave") Or Page.Request.Params("__EVENTTARGET").Contains("ddlkindOfInfo")
                    SaveLastTable()
            End Select
            If Page.Request.Params("__EVENTTARGET").Contains("btnContinue") Then Response.Redirect("Simulation.aspx", False)

        End If

        openXMLLanguagesFile()
        ChangeLanguageContent()
    End Sub

    Private Sub AddAttribute(e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Attributes.Add("onclick", "onEdit(" & e.Row.RowIndex.ToString() & ", this)")
        End If
    End Sub

    Private Sub gvFeeds_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFeeds.RowDataBound
        AddAttribute(e)
    End Sub
    Private Sub gvEquipment1_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvEquipment1.RowDataBound
        AddAttribute(e)
    End Sub
    Private Sub gvStructure_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvStructure.RowDataBound
        AddAttribute(e)
    End Sub
    Private Sub gvOtherInputs_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvOtherInputs.RowDataBound
        AddAttribute(e)
    End Sub

    Protected Sub gvFeeds_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles gvFeeds.RowEditing
        'save chkbox checked value
        SaveSelectedRow(e.NewEditIndex, gvFeeds, "feeds", e.NewEditIndex)
    End Sub
    Protected Sub gvEquipment1_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles gvEquipment1.RowEditing
        'save chkbox checked value
        SaveSelectedRow(e.NewEditIndex, gvEquipment1, "equipment", e.NewEditIndex)
    End Sub
    Protected Sub gvStructure_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles gvStructure.RowEditing
        'save chkbox checked value
        SaveSelectedRow(e.NewEditIndex, gvStructure, "structure", e.NewEditIndex)
    End Sub
    Protected Sub gvOtherInputs_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles gvOtherInputs.RowEditing
        'save chkbox checked value
        SaveSelectedRow(e.NewEditIndex, gvOtherInputs, "other", e.NewEditIndex)
    End Sub

    Protected Sub gvFeeds_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs) Handles gvFeeds.RowUpdating
        With Session("feeds")(e.RowIndex)
            .sellingPrice = e.NewValues(0)
            .purchasePrice = e.NewValues(1)
            .concentrate = e.NewValues(2)
            .forage = e.NewValues(3)
            .grain = e.NewValues(4)
            .hay = e.NewValues(5)
            .pasture = e.NewValues(6)
            .silage = e.NewValues(7)
            .supplement = e.NewValues(8)
        End With
        SaveSelectedRow(-1, gvFeeds, "feeds", e.RowIndex)
    End Sub
    Protected Sub gvEquipment1_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs) Handles gvEquipment1.RowUpdating
        With Session("equipment")(e.RowIndex)
            .LeaseRate = e.NewValues(0)
            .NewPrice = e.NewValues(1)
            .NewHours = e.NewValues(2)
            .CurrentPrice = e.NewValues(3)
            .HoursRemaining = e.NewValues(4)
            .Width = e.NewValues(5)
            .Speed = e.NewValues(6)
            .fieldEfficiency = e.NewValues(7)
            .HorsePower = e.NewValues(8)
            .RF1 = e.NewValues(9)
            .RF2 = e.NewValues(10)
            .IrLoan = e.NewValues(11)
            .LLoan = e.NewValues(12)
            .IrEquity = e.NewValues(13)
            .PDebt = e.NewValues(14)
            .Year = e.NewValues(15)
            .Rv1 = e.NewValues(16)
            .Rv2 = e.NewValues(17)
        End With
        SaveSelectedRow(-1, gvEquipment1, "equipment", e.RowIndex)
    End Sub
    Protected Sub gvStructure_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs) Handles gvStructure.RowUpdating
        With Session("structure")(e.RowIndex)
            .LeaseRate = e.NewValues(0)
            .NewPrice = e.NewValues(1)
            .NewLife = e.NewValues(2)
            .CurrentPrice = e.NewValues(3)
            .LifeRemaining = e.NewValues(4)
            .MaintenanceCoefficient = e.NewValues(5)
            .LoanInterestRate = e.NewValues(6)
            .LengthLoan = e.NewValues(7)
            .InterestRateEquity = e.NewValues(8)
            .proportionDebt = e.NewValues(9)
            .year = e.NewValues(10)
        End With
        SaveSelectedRow(-1, gvStructure, "structure", e.RowIndex)
    End Sub
    Protected Sub gvOtherInputs_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs) Handles gvOtherInputs.RowUpdating
        With Session("other")(e.RowIndex)
            .values = e.NewValues(0)
        End With
        SaveSelectedRow(-1, gvOtherInputs, "other", e.RowIndex)
    End Sub

    Protected Sub gvFeeds_RowCancelinEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs) Handles gvFeeds.RowCancelingEdit
        'save chkbox checked value
        SaveSelectedRow(-1, gvFeeds, "feeds", e.RowIndex)
    End Sub
    Protected Sub gvEquipment1_RowCancelinEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs) Handles gvEquipment1.RowCancelingEdit
        'save chkbox checked value
        SaveSelectedRow(-1, gvEquipment1, "equipment", e.RowIndex)
    End Sub
    Protected Sub gvStructure_RowCancelinEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs) Handles gvStructure.RowCancelingEdit
        'save chkbox checked value
        SaveSelectedRow(-1, gvStructure, "structure", e.RowIndex)
    End Sub
    Protected Sub gvOtherInputs_RowCancelinEdit(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs) Handles gvOtherInputs.RowCancelingEdit
        'save chkbox checked value
        SaveSelectedRow(-1, gvOtherInputs, "other", e.RowIndex)
    End Sub

    Private Sub ddlKindOfInfo_Click(sender As Object, e As EventArgs) Handles ddlkindOfInfo.SelectedIndexChanged
        If hdnEdit.Value = "Edit" Then
            ddlkindOfInfo.SelectedIndex = hdnSave.Value
            showMessage(lblMessage, imgIcon, "Red", "stopIcon", msgDoc.Descendants("UpdateRow").Value)
            Exit Sub
        End If
        hdnSave.Value = ddlkindOfInfo.SelectedIndex

        gvFeeds.Visible = False
        gvEquipment1.Visible = False
        gvStructure.Visible = False
        gvOtherInputs.Visible = False
        Session("feeds") = Nothing
        Session("equipment") = Nothing
        Session("structure") = Nothing
        Session("other") = Nothing

        If ddlkindOfInfo.SelectedIndex > 0 Then
            Select Case ddlkindOfInfo.SelectedIndex
                Case 1
                    gvFeeds.Visible = True
                    If Session("feeds") Is Nothing Then
                        LoadFeed()
                        Session("feeds") = _feed
                    End If
                    gvFeeds.DataSource = Session("feeds")
                    gvFeeds.DataBind()
                Case 2
                    gvEquipment1.Visible = True
                    If Session("equipment") Is Nothing Then
                        LoadEquipment()
                        Session("equipment") = _equipment
                    End If
                    gvEquipment1.DataSource = Session("equipment")
                    gvEquipment1.DataBind()
                Case 3
                    gvStructure.Visible = True
                    If Session("structure") Is Nothing Then
                        LoadStructure()
                        Session("structure") = _structure
                    End If
                    gvStructure.DataSource = Session("structure")
                    gvStructure.DataBind()
                Case 4
                    gvOtherInputs.Visible = True
                    If Session("other") Is Nothing Then
                        LoadOtherInputs()
                        Session("other") = _other
                    End If
                    gvOtherInputs.DataSource = Session("other")
                    gvOtherInputs.DataBind()
            End Select
        End If
    End Sub

    Private Sub LoadOtherInputs()
        Try
            Dim found As Boolean = False
            Dim OtherInputsQuery = GetOtherInputs()
            Dim OtherInputsTemp As New OtherData

            For Each item In OtherInputsQuery
                found = False
                For Each OtherInputsTemp In _otherTemp
                    If item.item("Name") = OtherInputsTemp.Name Then
                        'add the values from the uploaded project
                        LoadOtherInputsValues(OtherInputsTemp, item, 1)
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    'add the values coming from server.
                    LoadOtherInputsValues(OtherInputsTemp, item, 2)
                End If
            Next
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Private Sub LoadOtherInputsValues(OtherInputsTemp As OtherData, item As Object, type As UShort)
        Dim OtherInputs As New OtherData
        If type = 1 Then
            OtherInputs.Selected = True
            OtherInputs.Id = OtherInputsTemp.Id
            OtherInputs.Name = OtherInputsTemp.Name
            OtherInputs.Values = OtherInputsTemp.Values
        Else
            OtherInputs.Selected = False
            OtherInputs.Id = item.item("Id")
            OtherInputs.Name = item.item("Name")
            OtherInputs.Values = item.item("Values")
        End If

        _other.Add(OtherInputs)
    End Sub

    Private Sub LoadFeed()
        Try
            Dim found As Boolean = False
            Dim feedQuery = GetFeeds()
            Dim feedTemp As FeedData = Nothing

            For Each item In feedQuery
                found = False
                For Each feedTemp In _feedTemp
                    If item.item("Name") = feedTemp.Name Then
                        'add the values from the uploaded project
                        LoadFeedValues(feedTemp, item, 1)
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    'add the values coming from server.
                    LoadFeedValues(feedTemp, item, 2)
                End If
            Next
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Private Sub LoadFeedValues(feedTemp As FeedData, item As Object, type As UShort)
        Dim feed As New FeedData
        If type = 1 Then
            feed.Selected = True
            feed.Id = feedTemp.Id
            feed.Name = feedTemp.Name
            feed.SellingPrice = feedTemp.SellingPrice
            feed.PurchasePrice = feedTemp.PurchasePrice
            feed.Concentrate = feedTemp.Concentrate
            feed.Forage = feedTemp.Forage
            feed.Grain = feedTemp.Grain
            feed.Hay = feedTemp.Hay
            feed.Pasture = feedTemp.Pasture
            feed.Silage = feedTemp.Silage
            feed.Supplement = feedTemp.Supplement
        Else
            feed.Selected = False
            feed.Id = item.item("Id")
            feed.Name = item.item("Name")
            feed.SellingPrice = item.item("SellingPrice")
            feed.PurchasePrice = item.item("PurchasePrice")
            feed.Concentrate = item.item("Concentrate")
            feed.Forage = item.item("Forage")
            feed.Grain = item.item("Grain")
            feed.Hay = item.item("Hay")
            feed.Pasture = item.item("Pasture")
            feed.Silage = item.item("Silage")
            feed.Supplement = item.item("Suplement")
        End If

        _feed.Add(feed)
    End Sub

    Private Sub LoadEquipment()
        Try
            Dim found As Boolean = False
            'Dim EquipmentQuery = From equipment In dc.Equipments Order By equipment.Name Select equipment
            Dim EquipmentQuery = GetEquipments()
            Dim equipmentTemp As EquipmentData = Nothing

            For Each item In EquipmentQuery
                found = False
                For Each equipmentTemp In _equipmentTemp
                    If item.item("Name") = equipmentTemp.Name Then
                        'add the values from the uploaded project
                        LoadEquipmentValues(equipmentTemp, item, 1)
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    'add the values coming from server.
                    LoadEquipmentValues(equipmentTemp, item, 2)
                End If
            Next
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Private Sub LoadEquipmentValues(EquipmentTemp As EquipmentData, item As Object, type As UShort)
        Dim Equipment As New EquipmentData
        If type = 1 Then
            Equipment.Selected = True
            Equipment.Id = EquipmentTemp.Id
            Equipment.Name = EquipmentTemp.Name
            Equipment.LeaseRate = EquipmentTemp.LeaseRate
            Equipment.NewPrice = EquipmentTemp.NewPrice
            Equipment.NewHours = EquipmentTemp.NewHours
            Equipment.CurrentPrice = EquipmentTemp.CurrentPrice
            Equipment.HoursRemaining = EquipmentTemp.HoursRemaining
            Equipment.Width = EquipmentTemp.Width
            Equipment.Speed = EquipmentTemp.Speed
            Equipment.FieldEfficiency = EquipmentTemp.FieldEfficiency
            Equipment.HorsePower = EquipmentTemp.HorsePower
            Equipment.Rf1 = EquipmentTemp.Rf1
            Equipment.Rf2 = EquipmentTemp.Rf2
            Equipment.IrLoan = EquipmentTemp.IrLoan
            Equipment.LLoan = EquipmentTemp.LLoan
            Equipment.IrEquity = EquipmentTemp.IrEquity
            Equipment.PDebt = EquipmentTemp.PDebt
            Equipment.Year = EquipmentTemp.Year
            Equipment.Rv1 = EquipmentTemp.Rv1
            Equipment.Rv2 = EquipmentTemp.Rv2
        Else
            Equipment.Selected = False
            Equipment.Id = item.item("Id")
            Equipment.Name = item.item("Name")
            Equipment.LeaseRate = item.item("LeaseRate")
            Equipment.NewPrice = item.item("NewPrice")
            Equipment.NewHours = item.item("NewHours")
            Equipment.CurrentPrice = item.item("CurrentPrice")
            Equipment.HoursRemaining = item.item("HoursRemaining")
            Equipment.Width = item.item("Width")
            Equipment.Speed = item.item("Speed")
            Equipment.FieldEfficiency = item.item("FieldEfficiency")
            Equipment.HorsePower = item.item("HorsePower")
            Equipment.Rf1 = item.item("Rf1")
            Equipment.Rf2 = item.item("Rf2")
            Equipment.IrLoan = item.item("IrLoan")
            Equipment.LLoan = item.item("LLoan")
            Equipment.IrEquity = item.item("IrEquity")
            Equipment.PDebt = item.item("PDebt")
            Equipment.Year = item.item("Year")
            Equipment.Rv1 = item.item("Rv1")
            Equipment.Rv2 = item.item("Rv2")
        End If

        _equipment.Add(Equipment)
    End Sub

    Private Sub LoadStructure()
        Try
            Dim found As Boolean = False
            'Dim structureQuery = From structures In dc.Structures Order By structures.Name Select structures
            Dim structureQuery = GetStructures()
            Dim structureTemp As StructureData = Nothing

            For Each item In structureQuery
                found = False
                For Each structureTemp In _structureTemp
                    If item.item("Name") = structureTemp.Name Then
                        'add the values from the uploaded project
                        LoadStructureValues(structureTemp, item, 1)
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    'add the values coming from server.
                    LoadStructureValues(structureTemp, item, 2)
                End If
            Next
        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", msgDoc.Descendants("Errors").Value & ex.Message)
        End Try
    End Sub

    Private Sub LoadStructureValues(structureTemp As StructureData, item As Object, type As UShort)
        Dim structures As New StructureData
        If type = 1 Then
            structures.Selected = True
            structures.Id = structureTemp.Id
            structures.Name = structureTemp.Name
            structures.LeaseRate = structureTemp.LeaseRate
            structures.NewPrice = structureTemp.NewPrice
            structures.NewLife = structureTemp.NewLife
            structures.CurrentPrice = structureTemp.CurrentPrice
            structures.LifeRemaining = structureTemp.LifeRemaining
            structures.MaintenanceCoefficient = structureTemp.MaintenanceCoefficient
            structures.LoanInterestRate = structureTemp.LoanInterestRate
            structures.LengthLoan = structureTemp.LengthLoan
            structures.InterestRateEquity = structureTemp.InterestRateEquity
            structures.ProportionDebt = structureTemp.ProportionDebt
            structures.Year = structureTemp.Year
        Else
            structures.Selected = False
            structures.Id = item.item("Id")
            structures.Name = item.item("Name")
            structures.LeaseRate = item.item("LeaseRate")
            structures.NewPrice = item.item("NewPrice")
            structures.NewLife = item.item("NewLife")
            structures.CurrentPrice = item.item("CurrentPrice")
            structures.LifeRemaining = item.item("LifeRemaining")
            structures.MaintenanceCoefficient = item.item("MaintenanceCoefficient")
            structures.LoanInterestRate = item.item("LoanInterestRate")
            structures.LengthLoan = item.item("LengthLoan")
            structures.InterestRateEquity = item.item("InterestRateEquity")
            structures.ProportionDebt = item.item("ProportionDebt")
            structures.Year = item.item("Year")
        End If

        _structure.Add(structures)
    End Sub

    Private Sub ChangeLanguageContent()
        btnSave.Text = cntDoc.Descendants("Save").Value
        btnContinue.Text = cntDoc.Descendants("Continue").Value
        'tooltips
        btnSave.ToolTip = msgDoc.Descendants("ttSaveAndContinue").Value
    End Sub

    Private Sub SaveLastTable()
        Try
            Select Case hdnSave.Value
                Case "1"
                    SaveFeedsTable()
                Case "2"
                    SaveEquipmentTable()
                Case "3"
                    SaveStructureTable()
                Case "4"
                    SaveOtherInputsTable()
            End Select

            ArrangeInfo("Save")
            showMessage(lblMessage, imgIcon, "Green", "GoIcon", msgDoc.Descendants("InformationSaved").Value)

        Catch ex As Exception
            showMessage(lblMessage, imgIcon, "Red", "StopIcon.jpg", ex.Message)
        End Try
    End Sub

    Private Sub SaveSelectedRow(index As Short, ByRef gv As GridView, table As String, editIndex As UShort)
        gv.EditIndex = index
        gv.DataSource = Session(table)
        gv.DataBind()
        Dim ctrl As Control = gv.Rows(editIndex).Cells(0).Controls(0)
        gv.SelectedIndex = editIndex
        SetFocus(ctrl)
    End Sub

    Private Sub SaveFeedsTable()
        Dim feedTemp As FeedData
        _feedTemp.Clear()
        Dim chkSelected As HtmlInputCheckBox
        For Each row In gvFeeds.Rows
            chkSelected = row.findcontrol("chkSelected")
            If chkSelected.Checked Then
                feedTemp = New FeedData
                'if row was selected it is being add to the temp list with any modification
                With feedTemp
                    .Selected = True
                    '.Name = row.Cells(1).Text
                    For i = 2 To row.Cells.Count - 2
                        Select Case gvFeeds.Columns(i - 1).HeaderText
                            Case "Name"
                                .Name = row.cells(i).text.ToString.Replace("&lt;", "<")
                            Case "Selling Price"
                                .SellingPrice = row.Cells(i).text
                            Case "Purchase Price"
                                .PurchasePrice = row.Cells(i).text
                            Case "Concentrate"
                                .Concentrate = row.Cells(i).text
                            Case "Forage"
                                .Forage = row.Cells(i).text
                            Case "Grain"
                                .Grain = row.Cells(i).text
                            Case "Hay"
                                .Hay = row.Cells(i).text
                            Case "Pasture"
                                .Pasture = row.Cells(i).text
                            Case "Silage"
                                .Silage = row.Cells(i).text
                            Case "Supplement"
                                .Supplement = row.Cells(i).text
                        End Select
                    Next
                    _feedTemp.Add(feedTemp)
                End With
            End If
        Next
    End Sub
    Private Sub SaveEquipmentTable()
        Dim equipmentTemp As EquipmentData
        Dim chk As HtmlInputCheckBox
        _equipmentTemp.Clear()
        For Each row In gvEquipment1.Rows
            chk = New HtmlInputCheckBox
            chk = row.FindControl("chkSelected")
            If chk.Checked Then
                'If row.Cells(row.Cells.Count - 1).Controls(1).Value.ToString.ToLower = "true" Then
                equipmentTemp = New EquipmentData
                'if row was selected it is being add to the temp list with any modification
                With equipmentTemp
                    .Selected = True
                    '.Name = row.Cells(1).Text
                    For i = 2 To row.Cells.Count - 1
                        Select Case gvEquipment1.Columns(i - 1).HeaderText
                            Case "Name"
                                .Name = row.cells(i).text
                            Case "Lease Rate"
                                .LeaseRate = row.Cells(i).text
                            Case "New Price"
                                .NewPrice = row.Cells(i).text
                            Case "New Hours"
                                .NewHours = row.Cells(i).text
                            Case "Current Price"
                                .CurrentPrice = row.Cells(i).text
                            Case "Hours Remaining"
                                .HoursRemaining = row.Cells(i).text
                            Case "Width"
                                .Width = row.Cells(i).text
                            Case "Speed"
                                .Speed = row.Cells(i).text
                            Case "Field Efficiency"
                                .FieldEfficiency = row.Cells(i).text
                            Case "Horse Power"
                                .HorsePower = row.Cells(i).text
                            Case "Rf1"
                                .Rf1 = row.Cells(i).text
                            Case "Rf2"
                                .Rf2 = row.Cells(i).text
                            Case "Ir Loan"
                                .IrLoan = row.Cells(i).text
                            Case "L Loan"
                                .LLoan = row.Cells(i).text
                            Case "Ir Equity"
                                .IrEquity = row.Cells(i).text
                            Case "Horse Power"
                                .PDebt = row.Cells(i).text
                            Case "Year"
                                .Year = row.Cells(i).text
                            Case "Rv1"
                                .Rv1 = row.Cells(i).text
                            Case "Rv2"
                                .Rv2 = row.Cells(i).text
                        End Select
                    Next
                    _equipmentTemp.Add(equipmentTemp)
                End With
            End If
        Next
    End Sub

    Private Sub SaveStructureTable()
        Dim structureTemp As StructureData
        Dim chk As HtmlInputCheckBox
        _structureTemp.Clear()
        For Each row In gvStructure.Rows
            chk = New HtmlInputCheckBox
            chk = row.FindControl("chkSelected")
            If chk.Checked Then
                structureTemp = New StructureData
                'if row was selected it is being add to the temp list with all of the modifications
                With structureTemp
                    .Selected = True
                    '.Name = row.Cells(1).Text
                    For i = 2 To row.Cells.Count - 1
                        Select Case gvStructure.Columns(i - 1).HeaderText
                            Case "Name"
                                .Name = row.cells(i).text
                            Case "Lease Rate"
                                .LeaseRate = row.Cells(i).text
                            Case "New Price"
                                .NewPrice = row.Cells(i).text
                            Case "New Life"
                                .NewLife = row.Cells(i).text
                            Case "Current Price"
                                .CurrentPrice = row.Cells(i).text
                            Case "Life Remaining"
                                .LifeRemaining = row.Cells(i).text
                            Case "Maintenance Coefficient"
                                .MaintenanceCoefficient = row.Cells(i).text
                            Case "Loan Interest Rate"
                                .LoanInterestRate = row.Cells(i).text
                            Case "Length Loan"
                                .LengthLoan = row.Cells(i).text
                            Case "Interest Rate Equity"
                                .InterestRateEquity = row.Cells(i).text
                            Case "Proportion Debt"
                                .ProportionDebt = row.Cells(i).text
                            Case "Year"
                                .Year = row.Cells(i).text
                        End Select
                    Next
                    _structureTemp.Add(structureTemp)
                End With
            End If
        Next
    End Sub

    Private Sub SaveOtherInputsTable()
        Dim otherTemp As OtherData
        Dim chk As HtmlInputCheckBox
        _otherTemp.Clear()
        For Each row In gvOtherInputs.Rows
            chk = New HtmlInputCheckBox
            chk = row.FindControl("chkSelected")
            If chk.Checked Then
                otherTemp = New OtherData
                'if row was selected it is being add to the temp list with all of the modifications
                With otherTemp
                    .Selected = True
                    '.Name = row.Cells(1).Text
                    For i = 2 To row.Cells.Count - 1
                        Select Case gvOtherInputs.Columns(i - 1).HeaderText
                            Case "Name"
                                .Name = row.cells(i).text
                            Case "Values"
                                .Values = row.Cells(i).text
                        End Select
                    Next
                    _otherTemp.Add(otherTemp)
                End With
            End If
        Next
    End Sub

    Private Sub ArrangeInfo(openSave As String)
        Select Case openSave
            'this is done after the open subrutine
            Case "Open"
                _otherTemp = Session("projects")._otherTemp
                _structureTemp = Session("projects")._structureTemp
                _feedTemp = Session("projects")._feedTemp
                _equipmentTemp = Session("projects")._equipmentTemp
                'this is done before the save subroutine
            Case "Save"
                Session("projects")._otherTemp = _otherTemp
                Session("projects")._structureTemp = _structureTemp
                Session("projects")._feedTemp = _feedTemp
                Session("projects")._equipmentTemp = _equipmentTemp
        End Select
    End Sub

    Private Sub SaveCheckRows(ByRef gv As GridView, table As String)
        Dim chk As HtmlInputCheckBox
        For i = 0 To gv.Rows.Count - 1
            chk = gv.Rows(i).Cells(0).FindControl("chkSelected")
            Session(table)(i).Selected = chk.Checked
        Next
    End Sub
End Class