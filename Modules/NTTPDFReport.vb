'Imports Word = Microsoft.Office.Interop.Word
Imports Syncfusion.Pdf
Imports Syncfusion.Pdf.Shared
Imports System.Drawing

Public Class NTTPDFReport
    Dim doc As PDFDocument = New PDFDocument()
    Dim netFont As Font = New Font("Verdana", 10, GraphicsUnit.Point)
    Dim font As IPDFFont = doc.Fonts.Add(netFont)
    Dim netFontB As Font = New Font("Verdana", 10, FontStyle.Bold, GraphicsUnit.Point)
    Dim fontB As IPDFFont = doc.Fonts.Add(netFontB)
    Dim headerFont As IPDFFont = doc.Fonts.Add(FontBaseFamily.HelveticaBold)
    Dim myX, myY As Integer
    Dim rightnow As Date = Date.Now
    Private _startInfo As New StartInfo
    Private _fieldsInfo1 As New List(Of FieldsData)
    Private currentFieldNumber As UShort = 0

    Function docum(ByRef sPDFFile As String, ByVal sPath As String, ByVal sGUID As String, indexScenario1 As Short, indexScenario2 As Short, indexScenario3 As Short, _fieldsInfo As List(Of FieldsData), currentField As UShort, _startInfo1 As StartInfo) As String
        Dim spacing As Single = 0.0F
        Dim columnWidth As Single = doc.LastPage.DrawingWidth / 2 - spacing
        Dim columnHeight As Single = doc.LastPage.DrawingHeight / 2
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()
        Dim sFilesave As String = sPath & "\" & sGUID & ".pdf"
        Dim subTitle As String

        Try
            _fieldsInfo1 = _fieldsInfo
            _startInfo = _startInfo1
            currentFieldNumber = currentField
            headerFont.Size = 16
            AddHeader(doc, "Nutrient Tracking Tool - Report")
            AddFooter(doc, False)
            'Create input information report
            subTitle = "Input Information Report"
            addSubTitle(subTitle)
            CreateProjectInformation()
            CreateLocationInformation()
            CreateWeatherInformation()
            CreateFieldInformation()
            CreateSoilInformation()
            'New page for Layers
            doc.CreatePage()
            CreateLayersInformation()
            If indexScenario1 >= 0 Then
                'New page for scenario one
                doc.CreatePage()
                CreateManagementInformation(indexScenario1, "Scenario One", 1)
                'New page for BMPs scenario 1
                CreateBMPsInformation(indexScenario1, "Scenario One")
            End If
            If indexScenario2 >= 0 Then
                'New page for Scenario two.
                doc.CreatePage()
                CreateManagementInformation(indexScenario2, "Scenario Two", 2)
                'New page for BMPs scenario two
                CreateBMPsInformation(indexScenario2, "Scenario Two")
            End If
            If indexScenario3 >= 0 Then
                'New page for Scenario three.
                doc.CreatePage()
                CreateManagementInformation(indexScenario3, "Scenario Three", 3)
                'New page for BMPs scenario three
                CreateBMPsInformation(indexScenario3, "Scenario Three")
            End If
            'Create summary report - create results comparisson between scenario 1 and 2
            doc.CreatePage()
            subTitle = "Results (frist and second scenarios)"
            addSubTitle(subTitle)
            CreateResults(indexScenario1, indexScenario2)
            If indexScenario3 >= 0 Then
                'Create summary report - create results comparisson between scenario 1 and 3
                doc.CreatePage()
                subTitle = "Results (frist and third scenarios)"
                addSubTitle(subTitle)
                CreateResults(indexScenario1, indexScenario3)
            End If
            'createDetailReport()
            '// report starts here with name
            doc.Save(sFilesave)
            sPDFFile = sFilesave
            doc.Close()

            Return "OK"
        Catch ex As Exception
            'WriteToErrorFile(sMeName, "lbtnReportPDF_Click", "Exception failure", ex)
            Return ex.Message
        End Try
    End Function

    Private Sub CreateProjectInformation()
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()
        Dim fontC As IPDFFont = doc.Fonts.Add(netFontB)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 3
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0

        With _StartInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = "Name:"
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Right
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .projectName.ToString.Trim()
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = "Description:"
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Right
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .description.ToString.Trim()
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = "Date Created:"
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Right
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .dates.ToString.Trim()

        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 120

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
        myY += 60
        'myX = 5 : myY += 20
        'Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)
        'doc.LastPage.Graphics.DrawRectangle(airect)
        'propCalcSumm.TextAlignment = StringAlignment.Near
        'propCalcSumm.BackColor = Color.White
        'doc.LastPage.Graphics.DrawMultiText(airect, "Name: " & _StartInfo.projectName, fontC, propCalcSumm)

        '    myX = 5 : myY += 20
        '    airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 40)
        '    doc.LastPage.Graphics.DrawRectangle(airect)
        '    propCalcSumm.TextAlignment = StringAlignment.Near
        '    propCalcSumm.BackColor = Color.White
        '    doc.LastPage.Graphics.DrawMultiText(airect, "Description: " & _StartInfo.description, fontC, propCalcSumm)

        '    myX = 5 : myY += 20
        '    airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 40)
        '    doc.LastPage.Graphics.DrawRectangle(airect)
        '    propCalcSumm.TextAlignment = StringAlignment.Near
        '    propCalcSumm.BackColor = Color.White
        '    doc.LastPage.Graphics.DrawMultiText(airect, "Date Created: " & _StartInfo.dates, fontC, propCalcSumm)
    End Sub

    Private Sub CreateLocationInformation()
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Location Info line
        myX = 5 : myY += 30
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)
        doc.LastPage.Graphics.DrawRectangle(airect)
        propCalcSumm.TextAlignment = StringAlignment.Center
        propCalcSumm.BackColor = Color.Gainsboro
        doc.LastPage.Graphics.DrawMultiText(airect, "Location and Additional Site Information", fontB, propCalcSumm)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _StartInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = "State:"
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Right
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .StateName.ToString.Trim()
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = "County:"
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Right
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .countyName.ToString.Trim()
            'nCntCol = 0
            'nCntRow += 1
            'aiTable.Rows(nCntRow).Cells(nCntCol).Value = "Station:"
            'aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Right
            'aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            'nCntCol += 1
            'aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            'aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            'aiTable.Rows(nCntRow).Cells(nCntCol).Value = .stationName.ToString.Trim()
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 120

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub CreateWeatherInformation()
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Location Info line
        myX = 5 : myY += 30
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)
        doc.LastPage.Graphics.DrawRectangle(airect)
        propCalcSumm.TextAlignment = StringAlignment.Center
        propCalcSumm.BackColor = Color.Gainsboro
        doc.LastPage.Graphics.DrawMultiText(airect, "Weather Information", fontB, propCalcSumm)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 4
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _StartInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = "Weather Option:"
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Right
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .stationWay.ToString.Trim()
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = "Station Name:"
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Right
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .stationName.ToString.Trim()
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = "Weather Period:"
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Right
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .weatherInitialYear.ToString.Trim() & " to " & .weatherFinalYear
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = "Simulation Period:"
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Right
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .stationInitialYear.ToString.Trim() & " to " & .stationFinalYear
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 120

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub CreateFieldInformation()
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Location Info line
        myX = 5 : myY += 60
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)
        doc.LastPage.Graphics.DrawRectangle(airect)
        propCalcSumm.TextAlignment = StringAlignment.Center
        propCalcSumm.BackColor = Color.Gainsboro
        doc.LastPage.Graphics.DrawMultiText(airect, "Field Information", fontB, propCalcSumm)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 5
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = "Name:"
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Right
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .Name.ToString.Trim()
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = "Area:"
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Right
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .Area.ToString.Trim()
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = "Channel Veg. Cover Cond:"
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Right
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .Rchc
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = "Channel Erodibility:"
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Right
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .Rchk
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = "Average Slope:"
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Right
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = Math.Round(.AvgSlope, 2)
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 120

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub CreateSoilInformation()
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()
        Dim netFontB As Font = New Font("Verdana", 10, FontStyle.Regular, GraphicsUnit.Point)
        Dim fontC As IPDFFont = doc.Fonts.Add(netFontB)
        Dim i As Short
        ''//---------------------------------------------------- Soil Info table
        myX = 5 : myY += 60
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)
        doc.LastPage.Graphics.DrawRectangle(airect)
        propCalcSumm.TextAlignment = StringAlignment.Center
        propCalcSumm.BackColor = Color.Gainsboro
        doc.LastPage.Graphics.DrawMultiText(airect, "Soil Information", fontB, propCalcSumm)
        'Create a Additional Info Table
        Dim nCols As Short = 6
        Dim nRows As Short = 1
        Dim nCntCol, nCntRow As Short
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        '// 2 columns and 6 rows
        nCntCol = 0
        Dim width As Short
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            Select Case nCntCol
                Case 0
                    width = 200
                Case 1
                    width = 48
                Case 2
                    width = 48
                Case 3, 4
                    width = 60
                Case 5
                    width = 70
                Case Else
                    width = 30
            End Select
            aiTable.Columns(nCntCol).Width = width
            nCntCol += 1
        Loop

        nCntRow = 0
        nCntCol = 0
        Dim rowTitles As ITableRow = aiTable.Rows.CreateRow()
        nCntCol = 0
        rowTitles.Cells.CreateCell()
        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
        aiTable.Rows(0).Cells(nCntCol).Value = "Name"
        nCntCol += 1
        rowTitles.Cells.CreateCell()
        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
        aiTable.Rows(0).Cells(nCntCol).Value = "Symbol"
        nCntCol += 1
        rowTitles.Cells.CreateCell()
        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
        aiTable.Rows(0).Cells(nCntCol).Value = "Group"
        nCntCol += 1
        rowTitles.Cells.CreateCell()
        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
        aiTable.Rows(0).Cells(nCntCol).Value = "Albedo"
        nCntCol += 1
        rowTitles.Cells.CreateCell()
        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
        aiTable.Rows(0).Cells(nCntCol).Value = "Slope"
        nCntCol += 1
        rowTitles.Cells.CreateCell()
        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
        aiTable.Rows(0).Cells(nCntCol).Value = "Percentage"

        For i = 0 To _fieldsInfo1(currentFieldNumber)._soilsInfo.Where(Function(x) x.Selected).Count - 1
            With _fieldsInfo1(currentFieldNumber)._soilsInfo(i)
                If .Percentage <= 0 Then Exit For
                Dim row As ITableRow = aiTable.Rows.CreateRow()
                nCntCol = 0
                row.Cells.CreateCell()
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Left
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = fontC
                aiTable.Rows(i + 1).Cells(nCntCol).Value = .Name
                nCntCol += 1
                row.Cells.CreateCell()
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Left
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = fontC
                aiTable.Rows(i + 1).Cells(nCntCol).Value = .Symbol
                nCntCol += 1
                row.Cells.CreateCell()
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Left
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = fontC
                aiTable.Rows(i + 1).Cells(nCntCol).Value = .Group
                nCntCol += 1
                row.Cells.CreateCell()
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = fontC
                aiTable.Rows(i + 1).Cells(nCntCol).Value = .Albedo
                nCntCol += 1
                row.Cells.CreateCell()
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = fontC
                aiTable.Rows(i + 1).Cells(nCntCol).Value = Math.Round(.Slope, 2)
                nCntCol += 1
                row.Cells.CreateCell()
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = fontC
                aiTable.Rows(i + 1).Cells(nCntCol).Value = .Percentage
            End With
        Next
        myY += 20
        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
        myY += aitableSize.Height

    End Sub

    Private Sub CreateLayersInformation()
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()
        Dim fontC As IPDFFont = doc.Fonts.Add(netFontB)

        Dim i As Short
        myY = 0        ' New page
        ''//---------------------------------------------------- Soil Info table
        myX = 5 : myY += 10
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)
        doc.LastPage.Graphics.DrawRectangle(airect)
        propCalcSumm.TextAlignment = StringAlignment.Center
        propCalcSumm.BackColor = Color.Gainsboro
        doc.LastPage.Graphics.DrawMultiText(airect, "Layers Information", fontB, propCalcSumm)

        For Each _soil In _fieldsInfo1(currentFieldNumber)._soilsInfo.Where(Function(x) x.Selected)
            myY += 20
            airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)
            doc.LastPage.Graphics.DrawRectangle(airect)
            propCalcSumm.TextAlignment = StringAlignment.Near
            propCalcSumm.BackColor = Color.White
            doc.LastPage.Graphics.DrawMultiText(airect, "Soil Name: " & _soil.Name, fontC, propCalcSumm)
            'Create a Additional Info Table
            Dim nCols As Short = 8
            Dim nRows As Short = 1
            Dim nCntCol, nCntRow As Short
            Dim aiTable As ITable = doc.CreateTable()
            aiTable.Style.BordersWidth.All = 0
            aiTable.Style.CellSpacing = 0

            '// 2 columns and 6 rows
            nCntCol = 0
            Dim width As Short
            Do While nCntCol < nCols
                aiTable.Columns.CreateColumn()
                Select Case nCntCol
                    Case 0
                        width = 20
                    Case 1
                        width = 48
                    Case 2
                        width = 50
                    Case 3
                        width = 90
                    Case 4, 5
                        width = 48
                    Case 6
                        width = 70
                    Case Else
                        width = 48
                End Select
                aiTable.Columns(nCntCol).Width = width
                nCntCol += 1
            Loop

            nCntRow = 0
            nCntCol = 0
            Dim rowTitles As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            rowTitles.Cells.CreateCell()
            aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
            aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
            aiTable.Rows(0).Cells(nCntCol).Value = " # "
            nCntCol += 1
            rowTitles.Cells.CreateCell()
            aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
            aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
            aiTable.Rows(0).Cells(nCntCol).Value = cntDoc.Descendants("LayerDepth").Value
            nCntCol += 1
            rowTitles.Cells.CreateCell()
            aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
            aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
            aiTable.Rows(0).Cells(nCntCol).Value = cntDoc.Descendants("LayerSoilP").Value
            nCntCol += 1
            rowTitles.Cells.CreateCell()
            aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
            aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
            aiTable.Rows(0).Cells(nCntCol).Value = cntDoc.Descendants("LayerBulkDensity").Value
            nCntCol += 1
            rowTitles.Cells.CreateCell()
            aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
            aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
            aiTable.Rows(0).Cells(nCntCol).Value = cntDoc.Descendants("LayerSand").Value
            nCntCol += 1
            rowTitles.Cells.CreateCell()
            aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
            aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
            aiTable.Rows(0).Cells(nCntCol).Value = cntDoc.Descendants("LayerSilt").Value
            nCntCol += 1
            rowTitles.Cells.CreateCell()
            aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
            aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
            aiTable.Rows(0).Cells(nCntCol).Value = cntDoc.Descendants("LayerOrganicMatter").Value
            nCntCol += 1
            rowTitles.Cells.CreateCell()
            aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
            aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
            aiTable.Rows(0).Cells(nCntCol).Value = cntDoc.Descendants("LayerpH").Value

            For Each _layer In _soil._layersInfo
                With _layer
                    Dim row As ITableRow = aiTable.Rows.CreateRow()
                    nCntCol = 0
                    row.Cells.CreateCell()
                    aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
                    aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = font
                    aiTable.Rows(i + 1).Cells(nCntCol).Value = .LayerNumber
                    nCntCol += 1
                    row.Cells.CreateCell()
                    aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
                    aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = font
                    aiTable.Rows(i + 1).Cells(nCntCol).Value = Math.Round(.Depth, 2)
                    nCntCol += 1
                    row.Cells.CreateCell()
                    aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
                    aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = font
                    aiTable.Rows(i + 1).Cells(nCntCol).Value = .SoilP
                    nCntCol += 1
                    row.Cells.CreateCell()
                    aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
                    aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = font
                    aiTable.Rows(i + 1).Cells(nCntCol).Value = .BD
                    nCntCol += 1
                    row.Cells.CreateCell()
                    aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
                    aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = font
                    aiTable.Rows(i + 1).Cells(nCntCol).Value = .Sand
                    nCntCol += 1
                    row.Cells.CreateCell()
                    aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
                    aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = font
                    aiTable.Rows(i + 1).Cells(nCntCol).Value = .Silt
                    nCntCol += 1
                    row.Cells.CreateCell()
                    aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
                    aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = font
                    aiTable.Rows(i + 1).Cells(nCntCol).Value = .OM
                    nCntCol += 1
                    row.Cells.CreateCell()
                    aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
                    aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = font
                    aiTable.Rows(i + 1).Cells(nCntCol).Value = .PH
                End With
                i += 1
            Next
            i = 0
            myY += 20
            'Setting the location to publish the table.
            Dim aitableLocation As PointF = New PointF(myX, myY)
            'Getting the size of the table and publishing in the pdf document.
            Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
            myY += aitableSize.Height
        Next
    End Sub

    Private Sub CreateManagementInformation(indexScenario As UShort, scenario As String, scenarioNumber As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()
        Dim netFontB As Font = New Font("Verdana", 10, FontStyle.Regular, GraphicsUnit.Point)
        Dim fontC As IPDFFont = doc.Fonts.Add(netFontB)
        Dim i As Short
        Dim airect As Rectangle
        ''//---------------------------------------------------- management information table
        myX = 5
        myY = 10
        If scenarioNumber = 1 Then
            airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 10, 20)
            doc.LastPage.Graphics.DrawRectangle(airect)
            propCalcSumm.TextAlignment = StringAlignment.Center
            propCalcSumm.BackColor = Color.Gainsboro
            doc.LastPage.Graphics.DrawMultiText(airect, "Management Information", fontB, propCalcSumm)
            myY += 10
        End If
        myY += 10
        airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)
        doc.LastPage.Graphics.DrawRectangle(airect)
        propCalcSumm.TextAlignment = StringAlignment.Near
        propCalcSumm.BackColor = Color.White
        doc.LastPage.Graphics.DrawMultiText(airect, scenario & ": " & _fieldsInfo1(currentFieldNumber)._scenariosInfo(indexScenario).Name, fontC, propCalcSumm)
        myY += 10
        'Create a Additional Info Table
        Dim nCols As Short = 9
        Dim nRows As Short = 1
        Dim nCntCol, nCntRow As Short
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        '// 2 columns and 6 rows
        nCntCol = 0
        Dim width As Short
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            Select Case nCntCol
                Case 0
                    width = 100
                Case 1
                    width = 70
                Case 2, 3, 4
                    width = 20
                Case 5
                    width = 120
                Case 6, 7
                    width = 48
                Case 8
                    width = 70
                Case Else
                    width = 30
            End Select
            aiTable.Columns(nCntCol).Width = width
            nCntCol += 1
        Loop

        nCntRow = 0
        nCntCol = 0
        Dim rowTitles As ITableRow = aiTable.Rows.CreateRow()
        nCntCol = 0
        rowTitles.Cells.CreateCell()
        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
        aiTable.Rows(0).Cells(nCntCol).Value = "Crop"
        nCntCol += 1
        rowTitles.Cells.CreateCell()
        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
        aiTable.Rows(0).Cells(nCntCol).Value = "Operation"
        nCntCol += 1
        rowTitles.Cells.CreateCell()
        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
        aiTable.Rows(0).Cells(nCntCol).Value = "Y"
        nCntCol += 1
        rowTitles.Cells.CreateCell()
        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
        aiTable.Rows(0).Cells(nCntCol).Value = "M"
        nCntCol += 1
        rowTitles.Cells.CreateCell()
        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
        aiTable.Rows(0).Cells(nCntCol).Value = "D"
        nCntCol += 1
        rowTitles.Cells.CreateCell()
        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
        aiTable.Rows(0).Cells(nCntCol).Value = "Operation Method"
        nCntCol += 1
        rowTitles.Cells.CreateCell()
        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
        aiTable.Rows(0).Cells(nCntCol).Value = "Val One"
        nCntCol += 1
        rowTitles.Cells.CreateCell()
        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
        aiTable.Rows(0).Cells(nCntCol).Value = "Val Two"
        nCntCol += 1
        rowTitles.Cells.CreateCell()
        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
        aiTable.Rows(0).Cells(nCntCol).Value = "NO3, PO4, OrgN, OrgP"

        For i = 0 To _fieldsInfo1(currentFieldNumber)._scenariosInfo(indexScenario)._operationsInfo.Count - 1
            With _fieldsInfo1(currentFieldNumber)._scenariosInfo(indexScenario)._operationsInfo(i)
                Dim row As ITableRow = aiTable.Rows.CreateRow()
                nCntCol = 0
                row.Cells.CreateCell()
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Left
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = fontC
                aiTable.Rows(i + 1).Cells(nCntCol).Value = .ApexCropName
                nCntCol += 1
                row.Cells.CreateCell()
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Left
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = fontC
                aiTable.Rows(i + 1).Cells(nCntCol).Value = .ApexOpName
                nCntCol += 1
                row.Cells.CreateCell()
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Left
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = fontC
                aiTable.Rows(i + 1).Cells(nCntCol).Value = .Year
                nCntCol += 1
                row.Cells.CreateCell()
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = fontC
                aiTable.Rows(i + 1).Cells(nCntCol).Value = .Month
                nCntCol += 1
                row.Cells.CreateCell()
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = fontC
                aiTable.Rows(i + 1).Cells(nCntCol).Value = .Day
                nCntCol += 1
                row.Cells.CreateCell()
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = fontC
                aiTable.Rows(i + 1).Cells(nCntCol).Value = .ApexTillName
                nCntCol += 1
                row.Cells.CreateCell()
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = fontC
                aiTable.Rows(i + 1).Cells(nCntCol).Value = .ApexOpv1
                nCntCol += 1
                row.Cells.CreateCell()
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = fontC
                aiTable.Rows(i + 1).Cells(nCntCol).Value = .ApexOpv2
                nCntCol += 1
                row.Cells.CreateCell()
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
                aiTable.Rows(i + 1).Cells(nCntCol).Style.TextFont = fontC
                aiTable.Rows(i + 1).Cells(nCntCol).Value = .NO3.ToString.Trim & ", " & .PO4.ToString.Trim & ", " & .OrgN.ToString.Trim & ", " & .OrgP.ToString.Trim
            End With
        Next
        myY += 20
        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
        myY += aitableSize.Height

    End Sub

    Private Sub CreateBMPsInformation(indexScenario As UShort, scenario As String)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()
        Dim fontC As IPDFFont = doc.Fonts.Add(netFontB)

        ''//---------------------------------------------------- BMPs Info table
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)
        doc.LastPage.Graphics.DrawRectangle(airect)
        propCalcSumm.TextAlignment = StringAlignment.Center
        propCalcSumm.BackColor = Color.Gainsboro
        doc.LastPage.Graphics.DrawMultiText(airect, "Best Management Practices (BMPs) Information", fontB, propCalcSumm)

        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(indexScenario)._bmpsInfo
            If .AIType > 0 Then AddAI(indexScenario)
            If .AFType > 0 Then AddAF(indexScenario)
            If .TileDrainDepth > 0 Then AddTD(indexScenario)
            If .PPNDWidth > 0 Then AddPPND(indexScenario)
            If .PPDSWidth > 0 Then AddPPDS(indexScenario)
            If .PPDEResArea > 0 Then AddPPDE(indexScenario)
            If .PPTWResArea > 0 Then AddPPTW(indexScenario)
            If .WLArea > 0 Then AddWL(indexScenario)
            If .PndF > 0 Then AddPND(indexScenario)
            If .SFCode > 0 Then AddSF(indexScenario)
            If .Sbs = True Then AddSBS(indexScenario)
            If .RFWidth > 0 Then AddRF(indexScenario)
            If .FSCrop > 0 Then AddFS(indexScenario)
            If .WWCrop > 0 Then AddWW(indexScenario)
            If .CBCrop > 0 Then AddCB(indexScenario)
            If .SlopeRed > 0 Then AddLL(indexScenario)
            If .Ts = True Then AddTS(indexScenario)
            'If .Lm = True Then AddLM(indexScenario)
            If .AoC > 0 Then AddAoC(indexScenario)
            If .Gc > 0 Then AddGC(indexScenario)
            If .Sa > 0 Then AddSA(indexScenario)
            If .SdgCrop > 0 Then AddSdg(indexScenario)
        End With
    End Sub

    Private Sub AddTitle(title As String, ByRef aiTable As ITable, nCntCol As UShort, width As UShort)
        aiTable.Columns.CreateColumn()
        aiTable.Columns(nCntCol).Width = width
        aiTable.Rows(0).Cells.CreateCell()
        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
        aiTable.Rows(0).Cells(nCntCol).Value = title
    End Sub

    Private Sub AddResult(title As String, bold As Boolean, scen1 As Single, scen2 As Single, ci1 As Single, ci2 As Single, nCntRow As UShort, ByRef aiTable As ITable, area As Single)
        Dim nCntCol As UShort = 0
        Dim newFont As IPDFFont
        Dim value1 As String = scen1
        Dim value2 As String = scen2
        Dim textAligment As UShort = 2      'right
        'determine font
        If bold Then
            newFont = doc.Fonts.Add(netFontB)
            textAligment = 0            'left
        Else
            newFont = doc.Fonts.Add(netFont)
            'value1 &= "  ±" & Math.Round(ci1, 2)
            'value2 &= "  ±" & Math.Round(ci2, 2)
        End If
        'create the new row
        aiTable.Rows.CreateRow()
        'add description cell
        aiTable.Rows(nCntRow).Cells.CreateCell()
        aiTable.Rows(nCntRow).Cells(nCntCol).Value = title
        aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = textAligment
        aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = newFont
        nCntCol += 1
        aiTable.Rows(nCntRow).Cells.CreateCell()
        aiTable.Rows(nCntRow).Cells(nCntCol).Value = value1
        aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = textAligment
        aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = newFont
        nCntCol += 1
        aiTable.Rows(nCntRow).Cells.CreateCell()
        aiTable.Rows(nCntRow).Cells(nCntCol).Value = ci1
        aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = textAligment
        aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = newFont
        nCntCol += 1
        aiTable.Rows(nCntRow).Cells.CreateCell()
        aiTable.Rows(nCntRow).Cells(nCntCol).Value = value2
        aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = textAligment
        aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = newFont
        nCntCol += 1
        aiTable.Rows(nCntRow).Cells.CreateCell()
        aiTable.Rows(nCntRow).Cells(nCntCol).Value = ci2
        aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = textAligment
        aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = newFont
        Dim diff = (scen1 - scen2)
        nCntCol += 1
        aiTable.Rows(nCntRow).Cells.CreateCell()
        aiTable.Rows(nCntRow).Cells(nCntCol).Value = Math.Round(diff, 2) * -1
        aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = textAligment
        aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = newFont
        Dim reduc As Single
        If scen1 = 0 Then
            If scen1 = scen2 Then
                reduc = 0
            Else
                reduc = 100
            End If
        Else
            reduc = (100 - (scen2 / scen1) * 100)
        End If
        nCntCol += 1
        aiTable.Rows(nCntRow).Cells.CreateCell()
        aiTable.Rows(nCntRow).Cells(nCntCol).Value = Math.Round(reduc, 1)
        aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = textAligment
        aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = newFont
        nCntCol += 1
        aiTable.Rows(nCntRow).Cells.CreateCell()
        aiTable.Rows(nCntRow).Cells(nCntCol).Value = Math.Round(diff * area, 1) * -1
        aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = textAligment
        aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = newFont
    End Sub

    Private Sub CreateResults(indexScenario1 As Short, indexScenario2 As Short)
        If indexScenario1 < 0 Then Exit Sub 'if scenario1 is not selected no results are printed

        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()
        Dim netFontB As Font = New Font("Verdana", 10, FontStyle.Regular, GraphicsUnit.Point)
        Dim fontC As IPDFFont = doc.Fonts.Add(netFontB)
        Dim myX, myY As Integer

        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 15, 20)
        doc.LastPage.Graphics.DrawRectangle(airect)
        propCalcSumm.TextAlignment = StringAlignment.Center
        propCalcSumm.BackColor = Color.Gainsboro
        doc.LastPage.Graphics.DrawMultiText(airect, "Annual Results for Nutrients, Flow, Sediment, and Crop Yield", fontB, propCalcSumm)

        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0
        aiTable.Rows.CreateRow()
        AddTitle("Description", aiTable, 0, 134)
        AddTitle(_fieldsInfo1(currentFieldNumber)._scenariosInfo(indexScenario1).Name, aiTable, 1, 62)
        AddTitle("±", aiTable, 2, 40)
        AddTitle(_fieldsInfo1(currentFieldNumber)._scenariosInfo(indexScenario2).Name, aiTable, 3, 62)
        AddTitle("±", aiTable, 4, 40)
        If indexScenario2 >= 0 Then
            AddTitle("Difference", aiTable, 5, 64)
            AddTitle("Reduction (%)", aiTable, 6, 62)
            AddTitle("Total Area", aiTable, 7, 59)

            With _fieldsInfo1(currentFieldNumber)
                'totalArea = _fieldsInfo1(currentFieldNumber).Area
                'todo calculate total area for scenarios reducing bmps
                Dim nCntRow As UShort = 1
                AddResult(cntDoc.Descendants("TotalArea2").Value, True, .Area, .Area, 0, 0, nCntRow, aiTable, .Area)
                'print Total N
                nCntRow += 1
                AddResult(cntDoc.Descendants("TotalN").Value, True, ._scenariosInfo(indexScenario1)._results.SoilResults.TotalN.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.TotalN.ToString("F2"), ._scenariosInfo(indexScenario1)._results.SoilResults.TotalNCI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.TotalNCI.ToString("F2"), nCntRow, aiTable, .Area)
                'print Org N
                nCntRow += 1
                AddResult(cntDoc.Descendants("OrgN").Value, False, ._scenariosInfo(indexScenario1)._results.SoilResults.OrgN.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.OrgN.ToString("F2"), ._scenariosInfo(indexScenario1)._results.SoilResults.OrgNCI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.OrgNCI.ToString("F2"), nCntRow, aiTable, .Area)
                nCntRow += 1
                AddResult(cntDoc.Descendants("RunoffN").Value, False, ._scenariosInfo(indexScenario1)._results.SoilResults.RunoffN.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.RunoffN.ToString("F2"), ._scenariosInfo(indexScenario1)._results.SoilResults.runoffNCI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.runoffNCI.ToString("F2"), nCntRow, aiTable, .Area)
                nCntRow += 1
                AddResult(cntDoc.Descendants("SubsurfaceN").Value, False, ._scenariosInfo(indexScenario1)._results.SoilResults.SubsurfaceN.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.SubsurfaceN.ToString("F2"), ._scenariosInfo(indexScenario1)._results.SoilResults.subsurfaceNCI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.subsurfaceNCI.ToString("F2"), nCntRow, aiTable, .Area)
                nCntRow += 1
                AddResult(cntDoc.Descendants("TileDrainN").Value, False, ._scenariosInfo(indexScenario1)._results.SoilResults.tileDrainN.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.tileDrainN.ToString("F2"), ._scenariosInfo(indexScenario1)._results.SoilResults.tileDrainNCI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.tileDrainNCI.ToString("F2"), nCntRow, aiTable, .Area)
                'print total p
                nCntRow += 1
                AddResult(cntDoc.Descendants("TotalP").Value, True, ._scenariosInfo(indexScenario1)._results.SoilResults.TotalP.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.TotalP.ToString("F2"), ._scenariosInfo(indexScenario1)._results.SoilResults.TotalPCI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.TotalPCI.ToString("F2"), nCntRow, aiTable, .Area)
                'print org p
                nCntRow += 1
                AddResult(cntDoc.Descendants("OrgP").Value, False, ._scenariosInfo(indexScenario1)._results.SoilResults.OrgP.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.OrgP.ToString("F2"), ._scenariosInfo(indexScenario1)._results.SoilResults.OrgPCI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.OrgPCI.ToString("F2"), nCntRow, aiTable, .Area)
                'print Po4
                nCntRow += 1
                AddResult(cntDoc.Descendants("PO4").Value, False, ._scenariosInfo(indexScenario1)._results.SoilResults.PO4.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.PO4.ToString("F2"), ._scenariosInfo(indexScenario1)._results.SoilResults.PO4CI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.PO4CI.ToString("F2"), nCntRow, aiTable, .Area)
                'print Tile drain P
                nCntRow += 1
                AddResult(cntDoc.Descendants("TileDrainP").Value, False, ._scenariosInfo(indexScenario1)._results.SoilResults.tileDrainP.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.tileDrainP.ToString("F2"), ._scenariosInfo(indexScenario1)._results.SoilResults.tileDrainPCI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.tileDrainPCI.ToString("F2"), nCntRow, aiTable, .Area)
                'print total flow
                nCntRow += 1
                AddResult(cntDoc.Descendants("TotalFlow").Value, True, ._scenariosInfo(indexScenario1)._results.SoilResults.TotalFlow.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.TotalFlow.ToString("F2"), ._scenariosInfo(indexScenario1)._results.SoilResults.TotalFlowCI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.TotalFlowCI.ToString("F2"), nCntRow, aiTable, .Area)
                'print surface flow
                nCntRow += 1
                AddResult(cntDoc.Descendants("SurfaceRunoff").Value, False, ._scenariosInfo(indexScenario1)._results.SoilResults.runoff.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.runoff.ToString("F2"), ._scenariosInfo(indexScenario1)._results.SoilResults.runoffCI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.runoffCI.ToString("F2"), nCntRow, aiTable, .Area)
                'print subsurface flow
                nCntRow += 1
                AddResult(cntDoc.Descendants("SubsurfaceRunoff").Value, False, ._scenariosInfo(indexScenario1)._results.SoilResults.subsurfaceFlow.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.subsurfaceFlow.ToString("F2"), ._scenariosInfo(indexScenario1)._results.SoilResults.subsurfaceFlowCI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.subsurfaceFlowCI.ToString("F2"), nCntRow, aiTable, .Area)
                'print Tile drain flow
                nCntRow += 1
                AddResult(cntDoc.Descendants("TileDrainFlow").Value, False, ._scenariosInfo(indexScenario1)._results.SoilResults.tileDrainFlow.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.tileDrainFlow.ToString("F2"), ._scenariosInfo(indexScenario1)._results.SoilResults.tileDrainFlowCI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.tileDrainFlowCI.ToString("F2"), nCntRow, aiTable, .Area)
                'print total other flow info
                nCntRow += 1
                AddResult(cntDoc.Descendants("OtherWaterInfo").Value, True, ._scenariosInfo(indexScenario1)._results.SoilResults.TotalOtherFlowInfo.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.TotalOtherFlowInfo.ToString("F2"), (._scenariosInfo(indexScenario1)._results.SoilResults.irrigationCI + ._scenariosInfo(indexScenario1)._results.SoilResults.deepPerFlowCI).ToString("F2"), (._scenariosInfo(indexScenario2)._results.SoilResults.irrigationCI + ._scenariosInfo(indexScenario2)._results.SoilResults.deepPerFlowCI).ToString("F2"), nCntRow, aiTable, .Area)
                'print surface flow
                nCntRow += 1
                AddResult(cntDoc.Descendants("Irrigation").Value, False, ._scenariosInfo(indexScenario1)._results.SoilResults.irrigation.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.irrigation.ToString("F2"), ._scenariosInfo(indexScenario1)._results.SoilResults.irrigationCI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.irrigationCI.ToString("F2"), nCntRow, aiTable, .Area)
                'print subsurface flow
                nCntRow += 1
                AddResult(cntDoc.Descendants("DeepPercolation").Value, False, ._scenariosInfo(indexScenario1)._results.SoilResults.deepPerFlow.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.deepPerFlow.ToString("F2"), ._scenariosInfo(indexScenario1)._results.SoilResults.deepPerFlowCI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.deepPerFlowCI.ToString("F2"), nCntRow, aiTable, .Area)
                'print total sediment
                nCntRow += 1
                AddResult(cntDoc.Descendants("TotalSediment").Value, True, ._scenariosInfo(indexScenario1)._results.SoilResults.TotalSediment.ToString("F4"), ._scenariosInfo(indexScenario2)._results.SoilResults.TotalSediment.ToString("F4"), ._scenariosInfo(indexScenario1)._results.SoilResults.TotalSedimentCI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.TotalSedimentCI.ToString("F2"), nCntRow, aiTable, .Area)
                'print Sediment
                nCntRow += 1
                AddResult(cntDoc.Descendants("Sediment").Value, False, ._scenariosInfo(indexScenario1)._results.SoilResults.Sediment.ToString("F4"), ._scenariosInfo(indexScenario2)._results.SoilResults.Sediment.ToString("F4"), ._scenariosInfo(indexScenario1)._results.SoilResults.SedimentCI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.SedimentCI.ToString("F2"), nCntRow, aiTable, .Area)
                'print manure sediment
                nCntRow += 1
                AddResult(cntDoc.Descendants("ManureErosion").Value, False, ._scenariosInfo(indexScenario1)._results.SoilResults.ManureErosion.ToString("F4"), ._scenariosInfo(indexScenario2)._results.SoilResults.ManureErosion.ToString("F4"), ._scenariosInfo(indexScenario1)._results.SoilResults.ManureErosionCI.ToString("F2"), ._scenariosInfo(indexScenario2)._results.SoilResults.ManureErosionCI.ToString("F2"), nCntRow, aiTable, .Area)
                'print crop yield
                nCntRow += 1
                AddResult(cntDoc.Descendants("CropYield").Value, True, 0, 0, 0, 0, nCntRow, aiTable, .Area)
                Dim crop As String
                Dim yield1, yield2, ci1, ci2 As Single
                For i = 0 To ._scenariosInfo(indexScenario1)._results.SoilResults.Crops.cropName.Count - 1
                    crop = ._scenariosInfo(indexScenario1)._results.SoilResults.Crops.cropName(i)
                    yield1 = ._scenariosInfo(indexScenario1)._results.SoilResults.Crops.cropYield(i)
                    ci1 = ._scenariosInfo(indexScenario1)._results.SoilResults.Crops.cropYieldCI(i)
                    Dim found As Boolean = False
                    Dim j As UShort = 0
                    For j = 0 To ._scenariosInfo(indexScenario2)._results.SoilResults.Crops.cropName.Count - 1
                        If ._scenariosInfo(indexScenario1)._results.SoilResults.Crops.cropName(i) = ._scenariosInfo(indexScenario2)._results.SoilResults.Crops.cropName(j) Then
                            found = True
                            Exit For
                        End If
                    Next
                    If found = True Then
                        yield2 = ._scenariosInfo(indexScenario2)._results.SoilResults.Crops.cropYield(j)
                        ci2 = ._scenariosInfo(indexScenario2)._results.SoilResults.Crops.cropYieldCI(j)
                    Else
                        yield2 = 0
                        ci2 = 0
                    End If
                    nCntRow += 1
                    AddResult(crop, False, yield1.ToString("F0"), yield2.ToString("F0"), ci1.ToString("F2"), ci2.ToString("F2"), nCntRow, aiTable, .Area)
                Next

                For i = 0 To ._scenariosInfo(indexScenario2)._results.SoilResults.Crops.cropName.Count - 1
                    crop = ._scenariosInfo(indexScenario2)._results.SoilResults.Crops.cropName(i)
                    yield2 = ._scenariosInfo(indexScenario2)._results.SoilResults.Crops.cropYield(i)
                    ci2 = ._scenariosInfo(indexScenario2)._results.SoilResults.Crops.cropYieldCI(i)
                    Dim found As Boolean = False
                    Dim j As UShort = 0
                    For j = 0 To ._scenariosInfo(indexScenario1)._results.SoilResults.Crops.cropName.Count - 1
                        If ._scenariosInfo(indexScenario1)._results.SoilResults.Crops.cropName(j) = ._scenariosInfo(indexScenario2)._results.SoilResults.Crops.cropName(i) Then
                            found = True
                            Exit For
                        End If
                    Next
                    If found = False Then
                        yield1 = 0
                        ci1 = 0
                        nCntRow += 1
                        AddResult(crop, False, yield1.ToString("F0"), yield2.ToString("F0"), ci1.ToString("F2"), ci2.ToString("F2"), nCntRow, aiTable, .Area)
                    End If
                Next
            End With
        End If
        myY += 20
        ''Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
        myY += aitableSize.Height
        'If dsxml.Tables("NNStartInfo").Rows(0).Item("NNSINbrFIP") = 1 Then Exit For
    End Sub

    'Private Sub createDetailReport()
    '    Dim totalNA, TotalNB, totalArea As Single
    '    Dim j As Byte
    '    Dim maxrows As Short = 480
    '    Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()
    '    Dim netFontB As Font = New Font("Verdana", 10, FontStyle.Regular, GraphicsUnit.Point)
    '    Dim fontC As IPDFFont = doc.Fonts.Add(netFontB)
    '    'Dim myX, myY As Integer

    '    myX = 5 : myY = 20
    '    Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)
    '    doc.LastPage.Graphics.DrawRectangle(airect)
    '    propCalcSumm.TextAlignment = StringAlignment.Center
    '    propCalcSumm.BackColor = Color.Gainsboro
    '    doc.LastPage.Graphics.DrawMultiText(airect, "Annual Results for Nutrients, Flow, Sediment, and Crop Yield", fontB, propCalcSumm)

    '    myY += 20
    '    For j = 0 To dsxml.Tables("NNStartInfo").Rows(0).Item("NNSINbrFIP")
    '        If j = 0 Then
    '            airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)
    '            doc.LastPage.Graphics.DrawRectangle(airect)
    '            propCalcSumm.TextAlignment = StringAlignment.Center
    '            propCalcSumm.BackColor = Color.White
    '            propCalcSumm.ForeColor = Color.IndianRed
    '            doc.LastPage.Graphics.DrawMultiText(airect, "Total Results", fontB, propCalcSumm)
    '        Else
    '            airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)
    '            doc.LastPage.Graphics.DrawRectangle(airect)
    '            propCalcSumm.TextAlignment = TextAlignment.Center
    '            propCalcSumm.BackColor = Color.White
    '            propCalcSumm.ForeColor = Color.IndianRed
    '            doc.LastPage.Graphics.DrawMultiText(airect, dsxml.Tables("NNFieldInfo").Rows(j - 1).Item("NNFISSeriesName"), fontB, propCalcSumm)
    '        End If
    '        Dim nCols As Short = 6
    '        Dim nRows As Short = 9
    '        Dim nCntCol, nCntRow As Short
    '        Dim aiTable As ITable = doc.CreateTable()
    '        aiTable.Style.BordersWidth.All = 0
    '        aiTable.Style.CellSpacing = 0
    '        nCntCol = 0
    '        Dim width As Short
    '        Do While nCntCol < nCols
    '            aiTable.Columns.CreateColumn()
    '            Select Case nCntCol
    '                Case 0
    '                    width = 130
    '                Case 1
    '                    width = 60
    '                Case 2, 3
    '                    width = 70
    '                Case 4, 5, 6
    '                    width = 85
    '                Case 5, 6
    '                    width = 110
    '                Case Else
    '                    width = 30
    '            End Select
    '            aiTable.Columns(nCntCol).Width = width
    '            nCntCol += 1
    '        Loop
    '        nCntRow = 0
    '        nCntCol = 0
    '        Dim rowTitles As ITableRow = aiTable.Rows.CreateRow()
    '        nCntCol = 0
    '        rowTitles.Cells.CreateCell()
    '        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
    '        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
    '        aiTable.Rows(0).Cells(nCntCol).Value = ""
    '        nCntCol += 1
    '        rowTitles.Cells.CreateCell()
    '        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
    '        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
    '        aiTable.Rows(0).Cells(nCntCol).Value = "Baseline"
    '        nCntCol += 1
    '        rowTitles.Cells.CreateCell()
    '        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
    '        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
    '        aiTable.Rows(0).Cells(nCntCol).Value = "Alternative"
    '        nCntCol += 1
    '        rowTitles.Cells.CreateCell()
    '        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
    '        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
    '        aiTable.Rows(0).Cells(nCntCol).Value = "Difference"
    '        nCntCol += 1
    '        rowTitles.Cells.CreateCell()
    '        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
    '        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
    '        aiTable.Rows(0).Cells(nCntCol).Value = "Reduction(%)"
    '        nCntCol += 1
    '        rowTitles.Cells.CreateCell()
    '        aiTable.Rows(0).Cells(nCntCol).Style.TextAlignment = TextAlignment.Center
    '        aiTable.Rows(0).Cells(nCntCol).Style.TextFont = fontB
    '        aiTable.Rows(0).Cells(nCntCol).Value = "Total for Area"
    '        With dsxml.Tables("NNResultsInfo").Rows(j)
    '            totalArea = .Item("areaB")
    '            Dim row As ITableRow = aiTable.Rows.CreateRow()
    '            nCntRow = 1
    '            nCntCol = 0
    '            row.Cells.CreateCell()
    '            TotalNB = Val(.Item("orgNB"))
    '            totalNA = Val(.Item("orgNA"))
    '            'print Org N
    '            aiTable.Rows(1).Cells(nCntCol).Value = "Org N" + "" + "(lbs/ac)"
    '            aiTable.Rows(1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Left
    '            aiTable.Rows(1).Cells(nCntCol).Style.TextFont = fontB
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(1).Cells(nCntCol).Value = Math.Round(TotalNB, 2)
    '            aiTable.Rows(1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(1).Cells(nCntCol).Style.TextFont = font
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(1).Cells(nCntCol).Value = Math.Round(totalNA, 2)
    '            aiTable.Rows(1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(1).Cells(nCntCol).Style.TextFont = font
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(1).Cells(nCntCol).Value = Math.Round(TotalNB - totalNA, 2)
    '            aiTable.Rows(1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(1).Cells(nCntCol).Style.TextFont = font
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(1).Cells(nCntCol).Value = Math.Round((1 - totalNA / TotalNB) * 100, 1)
    '            aiTable.Rows(1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(1).Cells(nCntCol).Style.TextFont = font
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(1).Cells(nCntCol).Value = Math.Round((TotalNB - totalNA) * totalArea, 2)
    '            aiTable.Rows(1).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(1).Cells(nCntCol).Style.TextFont = font
    '            ' '' print runoff N
    '            nCntRow += 1
    '            nCntCol = 0
    '            row = aiTable.Rows.CreateRow()
    '            row.Cells.CreateCell()
    '            TotalNB = Val(.Item("NO3B"))
    '            totalNA = Val(.Item("NO3A"))
    '            aiTable.Rows(2).Cells(nCntCol).Value = "Runoff N" + "" + "(lbs/ac)"
    '            aiTable.Rows(2).Cells(nCntCol).Style.TextAlignment = TextAlignment.Left
    '            aiTable.Rows(2).Cells(nCntCol).Style.TextFont = fontB
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(2).Cells(nCntCol).Value = Math.Round(TotalNB, 2)
    '            aiTable.Rows(2).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(2).Cells(nCntCol).Style.TextFont = font
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(2).Cells(nCntCol).Value = Math.Round(totalNA, 2)
    '            aiTable.Rows(2).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(2).Cells(nCntCol).Style.TextFont = font
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(2).Cells(nCntCol).Value = Math.Round(TotalNB - totalNA, 2)
    '            aiTable.Rows(2).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(2).Cells(nCntCol).Style.TextFont = font
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(2).Cells(nCntCol).Value = Math.Round((1 - totalNA / TotalNB) * 100, 1)
    '            aiTable.Rows(2).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(2).Cells(nCntCol).Style.TextFont = font
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(2).Cells(nCntCol).Value = Math.Round((TotalNB - totalNA) * totalArea, 2)
    '            aiTable.Rows(2).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(2).Cells(nCntCol).Style.TextFont = font
    '            nCntRow += 1
    '            nCntCol = 0
    '            row = aiTable.Rows.CreateRow()
    '            row.Cells.CreateCell()
    '            ' '' print Organic P
    '            TotalNB = Val(.Item("orgPB"))
    '            totalNA = Val(.Item("orgPA"))
    '            aiTable.Rows(3).Cells(nCntCol).Value = "Org P" + "" + "(lbs/ac)"
    '            aiTable.Rows(3).Cells(nCntCol).Style.TextAlignment = TextAlignment.Left
    '            aiTable.Rows(3).Cells(nCntCol).Style.TextFont = fontB
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(3).Cells(nCntCol).Value = Math.Round(TotalNB, 2)
    '            aiTable.Rows(3).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(3).Cells(nCntCol).Style.TextFont = font
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(3).Cells(nCntCol).Value = Math.Round(totalNA, 2)
    '            aiTable.Rows(3).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(3).Cells(nCntCol).Style.TextFont = font
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(3).Cells(nCntCol).Value = Math.Round(TotalNB - totalNA, 2)
    '            aiTable.Rows(3).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(3).Cells(nCntCol).Style.TextFont = font
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(3).Cells(nCntCol).Value = Math.Round((1 - totalNA / TotalNB) * 100, 1)
    '            aiTable.Rows(3).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(3).Cells(nCntCol).Style.TextFont = font
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(3).Cells(nCntCol).Value = Math.Round((TotalNB - totalNA) * totalArea, 2)
    '            aiTable.Rows(3).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(3).Cells(nCntCol).Style.TextFont = font
    '            '' print Runoff P
    '            nCntRow += 1
    '            nCntCol = 0
    '            row = aiTable.Rows.CreateRow()
    '            row.Cells.CreateCell()
    '            TotalNB = Val(.Item("PO4B"))
    '            totalNA = Val(.Item("PO4A"))
    '            aiTable.Rows(4).Cells(nCntCol).Value = "Runoff P" + "" + "(lbs/ac)"
    '            aiTable.Rows(4).Cells(nCntCol).Style.TextAlignment = TextAlignment.Left
    '            aiTable.Rows(4).Cells(nCntCol).Style.TextFont = fontB
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(4).Cells(nCntCol).Value = Math.Round(TotalNB, 2)
    '            aiTable.Rows(4).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(4).Cells(nCntCol).Style.TextFont = font
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(4).Cells(nCntCol).Value = Math.Round(totalNA, 2)
    '            aiTable.Rows(4).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(4).Cells(nCntCol).Style.TextFont = font
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(4).Cells(nCntCol).Value = Math.Round(TotalNB - totalNA, 2)
    '            aiTable.Rows(4).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(4).Cells(nCntCol).Style.TextFont = font
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(4).Cells(nCntCol).Value = Math.Round((1 - totalNA / TotalNB) * 100, 1)
    '            aiTable.Rows(4).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(4).Cells(nCntCol).Style.TextFont = font
    '            nCntCol += 1
    '            row.Cells.CreateCell()
    '            aiTable.Rows(4).Cells(nCntCol).Value = Math.Round((TotalNB - totalNA) * totalArea, 2)
    '            aiTable.Rows(4).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '            aiTable.Rows(4).Cells(nCntCol).Style.TextFont = font

    '            If j > 0 Then
    '                '' print TileDrain N
    '                nCntRow += 1
    '                nCntCol = 0
    '                row = aiTable.Rows.CreateRow()
    '                row.Cells.CreateCell()
    '                TotalNB = Val(.Item("TileDrainNB"))
    '                totalNA = Val(.Item("TileDrainNA"))
    '                aiTable.Rows(5).Cells(nCntCol).Value = "Tile Drain N" + "" + "(lbs/ac)"
    '                aiTable.Rows(5).Cells(nCntCol).Style.TextAlignment = TextAlignment.Left
    '                aiTable.Rows(5).Cells(nCntCol).Style.TextFont = fontB
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(5).Cells(nCntCol).Value = Math.Round(TotalNB, 2)
    '                aiTable.Rows(5).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(5).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(5).Cells(nCntCol).Value = Math.Round(totalNA, 2)
    '                aiTable.Rows(5).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(5).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(5).Cells(nCntCol).Value = Math.Round(TotalNB - totalNA, 2)
    '                aiTable.Rows(5).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(5).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(5).Cells(nCntCol).Value = Math.Round((1 - totalNA / TotalNB) * 100, 1)
    '                aiTable.Rows(5).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(5).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(5).Cells(nCntCol).Value = Math.Round((TotalNB - totalNA) * totalArea, 2)
    '                aiTable.Rows(5).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(5).Cells(nCntCol).Style.TextFont = font
    '                '' print Leached N
    '                'nCntRow += 1
    '                'nCntCol = 0
    '                'row = aiTable.Rows.CreateRow()
    '                'row.Cells.CreateCell()
    '                'TotalNB = Val(.Item("LeachedNB"))
    '                'totalNA = Val(.Item("LeachedNA"))
    '                'aiTable.Rows(6).Cells(nCntCol).Value = "Leached N" + "" + "(lbs/ac)"
    '                'aiTable.Rows(6).Cells(nCntCol).Style.TextAlignment = TextAlignment.Left
    '                'aiTable.Rows(6).Cells(nCntCol).Style.TextFont = fontB
    '                'nCntCol += 1
    '                'row.Cells.CreateCell()
    '                'aiTable.Rows(6).Cells(nCntCol).Value = Math.Round(TotalNB, 2)
    '                'aiTable.Rows(6).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                'aiTable.Rows(6).Cells(nCntCol).Style.TextFont = font
    '                'nCntCol += 1
    '                'row.Cells.CreateCell()
    '                'aiTable.Rows(6).Cells(nCntCol).Value = Math.Round(totalNA, 2)
    '                'aiTable.Rows(6).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                'aiTable.Rows(6).Cells(nCntCol).Style.TextFont = font
    '                'nCntCol += 1
    '                'row.Cells.CreateCell()
    '                'aiTable.Rows(6).Cells(nCntCol).Value = Math.Round(TotalNB - totalNA, 2)
    '                'aiTable.Rows(6).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                'aiTable.Rows(6).Cells(nCntCol).Style.TextFont = font
    '                'nCntCol += 1
    '                'row.Cells.CreateCell()
    '                'aiTable.Rows(6).Cells(nCntCol).Value = Math.Round((1 - totalNA / TotalNB) * 100, 1)
    '                'aiTable.Rows(6).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                'aiTable.Rows(6).Cells(nCntCol).Style.TextFont = font
    '                'nCntCol += 1
    '                'row.Cells.CreateCell()
    '                'aiTable.Rows(6).Cells(nCntCol).Value = Math.Round((TotalNB - totalNA) * totalArea, 2)
    '                'aiTable.Rows(6).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                'aiTable.Rows(6).Cells(nCntCol).Style.TextFont = font
    '                '' print TileDrain P
    '                nCntRow += 1
    '                nCntCol = 0
    '                row = aiTable.Rows.CreateRow()
    '                row.Cells.CreateCell()
    '                TotalNB = Val(.Item("TileDrainPB"))
    '                totalNA = Val(.Item("TileDrainPA"))
    '                aiTable.Rows(6).Cells(nCntCol).Value = "Tile Drain P" + "(lbs/ac)"
    '                aiTable.Rows(6).Cells(nCntCol).Style.TextAlignment = TextAlignment.Left
    '                aiTable.Rows(6).Cells(nCntCol).Style.TextFont = fontB
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(6).Cells(nCntCol).Value = Math.Round(TotalNB, 2)
    '                aiTable.Rows(6).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(6).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(6).Cells(nCntCol).Value = Math.Round(totalNA, 2)
    '                aiTable.Rows(6).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(6).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(6).Cells(nCntCol).Value = Math.Round(TotalNB - totalNA, 2)
    '                aiTable.Rows(6).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(6).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(6).Cells(nCntCol).Value = Math.Round((1 - totalNA / TotalNB) * 100, 1)
    '                aiTable.Rows(6).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(6).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(6).Cells(nCntCol).Value = Math.Round((TotalNB - totalNA) * totalArea, 2)
    '                aiTable.Rows(6).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(6).Cells(nCntCol).Style.TextFont = font
    '                '' print Leached P
    '                'nCntRow += 1
    '                'nCntCol = 0
    '                'row = aiTable.Rows.CreateRow()
    '                'row.Cells.CreateCell()
    '                'TotalNB = Val(.Item("LeachedPB"))
    '                'totalNA = Val(.Item("LeachedPA"))
    '                'aiTable.Rows(8).Cells(nCntCol).Value = "Leached P" + "" + "(lbs/ac)"
    '                'aiTable.Rows(8).Cells(nCntCol).Style.TextAlignment = TextAlignment.Left
    '                'aiTable.Rows(8).Cells(nCntCol).Style.TextFont = fontB
    '                'nCntCol += 1
    '                'row.Cells.CreateCell()
    '                'aiTable.Rows(8).Cells(nCntCol).Value = Math.Round(TotalNB, 2)
    '                'aiTable.Rows(8).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                'aiTable.Rows(8).Cells(nCntCol).Style.TextFont = font
    '                'nCntCol += 1
    '                'row.Cells.CreateCell()
    '                'aiTable.Rows(8).Cells(nCntCol).Value = Math.Round(totalNA, 2)
    '                'aiTable.Rows(8).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                'aiTable.Rows(8).Cells(nCntCol).Style.TextFont = font
    '                'nCntCol += 1
    '                'row.Cells.CreateCell()
    '                'aiTable.Rows(8).Cells(nCntCol).Value = Math.Round(TotalNB - totalNA, 2)
    '                'aiTable.Rows(8).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                'aiTable.Rows(8).Cells(nCntCol).Style.TextFont = font
    '                'nCntCol += 1
    '                'row.Cells.CreateCell()
    '                'aiTable.Rows(8).Cells(nCntCol).Value = Math.Round((1 - totalNA / TotalNB) * 100, 1)
    '                'aiTable.Rows(8).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                'aiTable.Rows(8).Cells(nCntCol).Style.TextFont = font
    '                'nCntCol += 1
    '                'row.Cells.CreateCell()
    '                'aiTable.Rows(8).Cells(nCntCol).Value = Math.Round((TotalNB - totalNA) * totalArea, 2)
    '                'aiTable.Rows(8).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                'aiTable.Rows(8).Cells(nCntCol).Style.TextFont = font
    '                '' print TileDrain Flow
    '                nCntRow += 1
    '                nCntCol = 0
    '                row = aiTable.Rows.CreateRow()
    '                row.Cells.CreateCell()
    '                TotalNB = Val(.Item("TileDrainFlowB"))
    '                totalNA = Val(.Item("TileDrainFlowA"))
    '                aiTable.Rows(7).Cells(nCntCol).Value = "Tile Drain Flow" + "(in)"
    '                aiTable.Rows(7).Cells(nCntCol).Style.TextAlignment = TextAlignment.Left
    '                aiTable.Rows(7).Cells(nCntCol).Style.TextFont = fontB
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(7).Cells(nCntCol).Value = Math.Round(TotalNB, 2)
    '                aiTable.Rows(7).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(7).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(7).Cells(nCntCol).Value = Math.Round(totalNA, 2)
    '                aiTable.Rows(7).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(7).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(7).Cells(nCntCol).Value = Math.Round(TotalNB - totalNA, 2)
    '                aiTable.Rows(7).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(7).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(7).Cells(nCntCol).Value = Math.Round((1 - totalNA / TotalNB) * 100, 1)
    '                aiTable.Rows(7).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(7).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(7).Cells(nCntCol).Value = Math.Round((TotalNB - totalNA) * totalArea, 2)
    '                aiTable.Rows(7).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(7).Cells(nCntCol).Style.TextFont = font
    '                '' Deep Percolation Flow
    '                nCntRow += 1
    '                nCntCol = 0
    '                row = aiTable.Rows.CreateRow()
    '                row.Cells.CreateCell()
    '                TotalNB = Val(.Item("DeepPerFlowB"))
    '                totalNA = Val(.Item("DeepPerFlowA"))
    '                aiTable.Rows(8).Cells(nCntCol).Value = "Deep Percolation" + "(in)"
    '                aiTable.Rows(8).Cells(nCntCol).Style.TextAlignment = TextAlignment.Left
    '                aiTable.Rows(8).Cells(nCntCol).Style.TextFont = fontB
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(8).Cells(nCntCol).Value = Math.Round(TotalNB, 2)
    '                aiTable.Rows(8).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(8).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(8).Cells(nCntCol).Value = Math.Round(totalNA, 2)
    '                aiTable.Rows(8).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(8).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(8).Cells(nCntCol).Value = Math.Round(TotalNB - totalNA, 2)
    '                aiTable.Rows(8).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(8).Cells(nCntCol).Style.TextFont = font
    '                row.Cells.CreateCell()
    '                aiTable.Rows(8).Cells(nCntCol).Value = Math.Round((1 - totalNA / TotalNB) * 100, 1)
    '                aiTable.Rows(8).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(8).Cells(nCntCol).Style.TextFont = font
    '                row.Cells.CreateCell()
    '                aiTable.Rows(8).Cells(nCntCol).Value = Math.Round((TotalNB - totalNA) * totalArea, 2)
    '                aiTable.Rows(8).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(8).Cells(nCntCol).Style.TextFont = font
    '                '' print Nitous Oxide
    '                nCntRow += 1
    '                nCntCol = 0
    '                row = aiTable.Rows.CreateRow()
    '                row.Cells.CreateCell()
    '                TotalNB = Val(.Item("N2OB"))
    '                totalNA = Val(.Item("N2OA"))
    '                aiTable.Rows(9).Cells(nCntCol).Value = "Nitrous Oxide" + "(lbs/ac)"
    '                aiTable.Rows(9).Cells(nCntCol).Style.TextAlignment = TextAlignment.Left
    '                aiTable.Rows(9).Cells(nCntCol).Style.TextFont = fontB
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(9).Cells(nCntCol).Value = Math.Round(TotalNB, 2)
    '                aiTable.Rows(9).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(9).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(9).Cells(nCntCol).Value = Math.Round(totalNA, 2)
    '                aiTable.Rows(9).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(9).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(9).Cells(nCntCol).Value = Math.Round(TotalNB - totalNA, 2)
    '                aiTable.Rows(9).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(9).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(9).Cells(nCntCol).Value = Math.Round((1 - totalNA / TotalNB) * 100, 1)
    '                aiTable.Rows(9).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(9).Cells(nCntCol).Style.TextFont = font
    '                nCntCol += 1
    '                row.Cells.CreateCell()
    '                aiTable.Rows(9).Cells(nCntCol).Value = Math.Round((TotalNB - totalNA) * totalArea, 2)
    '                aiTable.Rows(9).Cells(nCntCol).Style.TextAlignment = TextAlignment.Right
    '                aiTable.Rows(9).Cells(nCntCol).Style.TextFont = font
    '            End If
    '        End With

    '        myY += 20
    '        ''Setting the location to publish the table.
    '        Dim aitableLocation As PointF = New PointF(myX, myY)
    '        'Getting the size of the table and publishing in the pdf document.
    '        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    '        myY += aitableSize.Height + 5
    '        If myY > maxrows Then
    '            myX = 5 : myY = 0
    '            doc.CreatePage()
    '            addSubTitle("DETAILED REPORT")
    '            myX = 5 : myY += 20
    '            airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)
    '            doc.LastPage.Graphics.DrawRectangle(airect)
    '            propCalcSumm.TextAlignment = StringAlignment.Center
    '            propCalcSumm.BackColor = Color.Gainsboro
    '            propCalcSumm.ForeColor = Color.Black
    '            netFontB = New Font("Verdana", 10, FontStyle.Bold, GraphicsUnit.Point)
    '            fontB = doc.Fonts.Add(netFontB)
    '            doc.LastPage.Graphics.DrawMultiText(airect, "Annual Results for Nutrients, Flow, Sediment, and Crop Yield", fontB, propCalcSumm)
    '            myY += 20
    '        End If
    '        If dsxml.Tables("NNStartInfo").Rows(0).Item("NNSINbrFIP") = 1 Then Exit For
    '    Next
    'End Sub
    Private Sub AddHeader(ByVal doc As PDFDocument, ByVal title As String)
        Dim props As IPDFGraphicState = doc.CreateGraphicState()

        If doc Is Nothing Then
            Throw New ArgumentNullException("doc")
        End If

        If title Is Nothing Then
            Throw New ArgumentNullException("title")
        End If

        Dim offset As Integer = 10
        ' Last version
        Dim header As PDFTemplateArea = doc.Templates.Add(New SizeF(doc.LastPage.DrawingWidth, 80))
        header.Dock = PDFDockStyle.Top
        Dim font As IPDFFont = doc.Fonts.Add(FontBaseFamily.HelveticaBold)
        font.Size = 24
        Dim doubleHeight As Single = font.Height * 2
        Dim activeColor As Color = Color.FromArgb(132, 32, 0)

        '' add logo
        'Dim imageSize As SizeF = New SizeF(193, 37)
        'Dim imageLocation As PointF = New PointF(doc.LastPage.DrawingWidth - imageSize.Width, 5)
        'props.Padding = New PDFPadding(offset, offset, 0, 0)

        ''// outside rectangle
        'Dim pdfFont As IPDFFont = doc.Fonts.Add(FontBaseFamily.Courier)
        'props.ForeColor = New PDFColor(33, 67, 126)
        'doc.LastPage.Graphics.DrawRectangle(New RectangleF(PointF.Empty, New SizeF(doc.LastPage.DrawingWidth - 10, doc.LastPage.DrawingHeight - 10)))

        ' add title tag
        props = doc.CreateGraphicState()
        props.TextAlignment = TextAlign.Left
        props.ForeColor = activeColor
        font.Size -= 3
        'props.Padding = New PDFPadding(offset * 2, 0, CInt(imageSize.Width), 0)
        header.Graphics.DrawMultiText(0, 20, doc.LastPage.DrawingWidth, font.Height, title, font, props)

        ' add description tag
        Dim headerSize As SizeF
        headerSize = header.Size
        font.Size -= 4
        props = doc.CreateGraphicState()
        props.TextAlignment = TextAlign.Left
        props.Padding = New PDFPadding(offset, offset, 0, 0)
        props.ForeColor = activeColor

        ' add border effect
        Dim width As Single = doc.LastPage.DrawingWidth
        headerSize = header.Size
        headerSize.Height -= 10
        activeColor = Color.FromArgb(132, 32, 0)
        props = doc.CreateGraphicState()
        props.LineWidth = 2
        props.ForeColor = activeColor
        header.Graphics.DrawLine(0, headerSize.Height - 2, width, headerSize.Height - 2, props)

        doc.Templates.Header = header
    End Sub

    Private Sub AddFooter(ByVal doc As PDFDocument, ByVal isLeftRightBarPresents As Boolean)
        If doc Is Nothing Then
            Throw New ArgumentNullException("doc")
        End If

        Dim footer As PDFTemplateArea = doc.Templates.Add(New SizeF(doc.LastPage.DrawingWidth, 40))
        Dim font As IPDFFont = doc.Fonts.Add(FontBaseFamily.HelveticaBold)
        font.Size = 10
        Dim text As String = "Page #p;"
        Dim size As SizeF = footer.Graphics.MeasureString(text, font)
        Dim bounds As RectangleF = New RectangleF(0, 15, footer.Size.Width, footer.Size.Height)
        Dim props As IPDFGraphicState = doc.CreateGraphicState()
        Dim textFont1 As IPDFFont = doc.Fonts.Add(FontBaseFamily.HelveticaOblique)

        props.TextAlignment = TextAlignment.Left
        props.BreakBehavior = AutoBreakBehavior.Cropping
        footer.Graphics.DrawMultiText(bounds, text, font, props)
        textFont1.Size = 8

        props = doc.CreateGraphicState()
        props.ForeColor = Color.Gray
        Dim textFont As IPDFFont = doc.Fonts.Add(FontBaseFamily.HelveticaOblique)
        textFont.Size = 8
        props.BreakBehavior = AutoBreakBehavior.Cropping

        Dim width As Single = doc.LastPage.DrawingWidth

        ' add border effect on demand
        If isLeftRightBarPresents Then
            Dim footerSize As SizeF = footer.Size
            footerSize.Height -= 10

            Dim activeColor As Color = Color.FromArgb(139, 164, 204)

            props = doc.CreateGraphicState()
            props.ForeColor = activeColor
            footer.Graphics.DrawLine(0, 0, width, 0)
            footer.Graphics.DrawLine(0, footerSize.Height, width, footerSize.Height)

            props = doc.CreateGraphicState()
            props.LineWidth = 2
            props.ForeColor = activeColor
            footer.Graphics.DrawLine(0, 2, width, 2, props)
            footer.Graphics.DrawLine(0, footerSize.Height - 2, width, footerSize.Height - 2, props)

            footer.Graphics.DrawLine(0, 5, width, 5)
            footer.Graphics.DrawLine(0, footerSize.Height - 5, width, footerSize.Height - 5)
        End If

        doc.Templates.Footer = footer

    End Sub

    Sub addSubTitle(ByRef subTitle As String)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()
        Dim props As IPDFGraphicState = doc.CreateGraphicState()
        'add subtitle and date
        Dim airect = New Rectangle(0, 0, doc.LastPage.DrawingWidth - 50, 20)
        propCalcSumm.TextAlignment = StringAlignment.Center
        propCalcSumm.ForeColor = Color.DarkRed
        doc.LastPage.Graphics.DrawMultiText(airect, subTitle, fontB, propCalcSumm)
        'add Data
        Dim airect1 = New Rectangle(0, 0, doc.LastPage.DrawingWidth - 20, 70)
        propCalcSumm.TextAlignment = TextAlignment.Right
        doc.LastPage.Graphics.DrawMultiText(airect1, rightnow, font, propCalcSumm)
    End Sub

    Private Sub AddAI(index As UShort)
        'verify if _irrigationTypes have information. If not take them again..
        If _irrigationType.Count = 0 Then
            Dim mgt As New Management
            mgt.InitializeDDLs()
        End If
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Location Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 6
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("AutoIrrigationHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.ForeColor = Color.Blue
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("IrrigationTypeHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = _irrigationType(.AIType).Name
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("WaterStressHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .AIWaterStressFactor
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("IrrigationEfficiencyHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .AIEff
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("FrequencyHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .AIFreq
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("MaxApplicationHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .AIMaxSingleApp
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddAF(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Location Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 6
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("AutoFertigationHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.ForeColor = Color.Blue
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("IrrigationTypeHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = _irrigationType(.AFType).Name
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("WaterStressHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .AFWaterStressFactor
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("IrrigationEfficiencyHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .AFEff
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("FrequencyHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .AFFreq
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("MaxApplicationHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .AFMaxSingleApp
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("NitrogenConcentrationHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .AFMaxSingleApp
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddTD(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Location Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("TileDrainHeading").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("TileDrainDepthHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .TileDrainDepth
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddPPND(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Pads and Pipes no-Ditch-Improvement Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("PPNoImprovementHeading").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("PPWidthHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .PPNDWidth
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("NumberCoveredByPipesHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .PPNDSides
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddPPDS(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Pads and Pipes no-Ditch-Improvement Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("PPTwoStageDitchHeading").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("PPWidthHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .PPDSWidth
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("NumberCoveredByPipesHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .PPDSSides
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddPPDE(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Pads and Pipes no-Ditch-Improvement Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("PPDitchReservoirHeading").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("PPWidthHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .PPDEWidth
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("NumberCoveredByPipesHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .PPDESides
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("ReservoirAreaHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .PPDEResArea
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddPPTW(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Pads and Pipes no-Ditch-Improvement Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("PPTailwaterHeading").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("PPWidthHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .PPTWWidth
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("NumberCoveredByPipesHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .PPTWSides
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("ReservoirAreaHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .PPTWResArea
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddWL(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Location Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("WetlandsHeading").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("AreaHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .WLArea
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddPND(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Location Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("PondsHeading").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("PondsAreaHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .PndF
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddSF(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Pads and Pipes no-Ditch-Improvement Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("StreamFencingHeading").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("NumberOfAnimalsHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .SFAnimals
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("DaysInStreamHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .SFDays
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("HoursDayInStreamHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .SFHours
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("SelectAnimalHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .SFName
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("DryManureHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .SFDryManure
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("NutrientComposition").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .SFNo3 & ", " & .SFPo4 & ", " & .SFOrgN & ", " & .SFOrgP
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddSBS(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Location Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("StreambankStabilizationHeading").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("StreambankStabilizationHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .Sbs
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddRF(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Pads and Pipes no-Ditch-Improvement Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("RiparianForestHeading").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("AreaHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .RFArea
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("WidthHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .RFWidth
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("RiparianGrassPortionHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .RFGrassFieldPortion
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("RiparianBufferSlopeHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .RFslopeRatio
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddFS(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Pads and Pipes no-Ditch-Improvement Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("FilterStripHeading").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("Vegetation").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .FSCrop
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("AreaHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .FSArea
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("WidthHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .FSWidth
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("StripSlopeHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .FSslopeRatio
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddWW(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Pads and Pipes no-Ditch-Improvement Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("WaterwayHeading").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("Vegetation").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .FSCrop
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("WaterwayWidthHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .FSWidth
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddCB(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Pads and Pipes no-Ditch-Improvement Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("ContourBufferHeading2").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("Vegetation").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .FSCrop
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("BufferWidthHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .FSWidth
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("CropWidthHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .FSWidth
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddLL(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Location Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("LandLevelingHeading").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("SlopeReduction").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .SlopeRed
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddTS(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Location Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("TerraceSystemHeading").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("TerraceSystemHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .Ts
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddLM(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Location Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("LimingHeading").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("LimingHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .Lm
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddAoC(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Location Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("AsfaltOrConcrete").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("AsfaltOrConcrete").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .AoC
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddGC(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Location Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("GrassCover").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("GrassCover").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .Gc
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddSA(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Location Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("SlopeAdjustment").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("SlopeAdjustment").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .Sa
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub

    Private Sub AddSdg(index As UShort)
        Dim propCalcSumm As IPDFGraphicState = doc.CreateGraphicState()

        ''//---------------------------------------------------- Pads and Pipes no-Ditch-Improvement Info line
        myX = 5 : myY += 20
        Dim airect = New Rectangle(myX, myY, doc.LastPage.DrawingWidth - 50, 20)

        'Create a Additional Info Table
        Dim nCols As Integer = 2
        Dim nRows As Integer = 2
        Dim nCntCol, nCntRow As Integer
        Dim aiTable As ITable = doc.CreateTable()
        aiTable.Style.BordersWidth.All = 0
        aiTable.Style.CellSpacing = 0

        myX = 5 : myY += 20
        '// 2 columns and 6 rows
        nCntCol = 0
        Do While nCntCol < nCols
            aiTable.Columns.CreateColumn()
            nCntCol += 1
        Loop

        nCntRow = 0
        Do While nCntRow < nRows
            Dim row As ITableRow = aiTable.Rows.CreateRow()
            nCntCol = 0
            Do While nCntCol < nCols
                row.Cells.CreateCell()
                aiTable.Rows(nCntRow).Height = 12
                aiTable.Rows(nCntRow).Cells(nCntCol).Style.BordersWidth.All = 0
                nCntCol += 1
            Loop
            nCntRow += 1
        Loop
        nCntRow = 0
        nCntCol = 0
        With _fieldsInfo1(currentFieldNumber)._scenariosInfo(index)._bmpsInfo
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("Shading").Value & ": "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("Vegetation").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .SdgCrop
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("AreaHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .SdgArea
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("WidthHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .SdgWidth
            nCntCol = 0
            nCntRow += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = cntDoc.Descendants("StripSlopeHeading").Value & " "
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = fontB
            nCntCol += 1
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextAlignment = TextAlign.Left
            aiTable.Rows(nCntRow).Cells(nCntCol).Style.TextFont = font
            aiTable.Rows(nCntRow).Cells(nCntCol).Value = .SdgslopeRatio
        End With
        aiTable.Columns(0).Width = 120
        aiTable.Columns(1).Width = 60

        'Setting the location to publish the table.
        Dim aitableLocation As PointF = New PointF(myX, myY)
        'Getting the size of the table and publishing in the pdf document.
        Dim aitableSize As SizeF = aiTable.Publish(aitableLocation, doc.LastPage)
    End Sub
End Class
