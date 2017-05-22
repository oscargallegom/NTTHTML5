Module Validations
    Public Const FractionMin As Single = 0.01
    Public Const FractionMax As Single = 1
    Public Const LLSlopeRedMin As Single = 0.01
    Public Const LLSlopeRedMax As Single = 100
    Public Const ValueMin As Single = 0.01
    Public Const ValueMax As Single = 999999
    Public Const GrassFieldPortionMin As Single = 0.25
    Public Const GrassFieldPortionMax As Single = 0.75
    Public Const SFAnimalsMin As Single = 0.01
    Public Const SFAnimalsMax As Single = 999999
    Public Const SFDaysMin As UShort = 1
    Public Const SFDaysMax As UShort = 365
    Public Const SFHoursMin As Single = 0.01
    Public Const SFHoursMax As Single = 24
    Public Const SFDryManureMin As Single = 0.01
    Public Const SFDryManureMax As Single = 999999
    Public Const sidesMin As Single = 1
    Public Const sidesMax As Single = 4
    Public Const minChange As Single = -100
    Public Const maxChange As Single = 100

    Function validateAutoIrrigation(sender As Object, _bmpsInfo As BmpsData)
        Dim err As Boolean = False
        Try
            With _bmpsInfo
                'validate autoirrigation
                If .AIType > 0 Or .AIEff > 0 Or .AIFreq > 0 Then
                    If Not (.AIType > 0 And .AIEff > 0 And .AIFreq > 0) Then
                        err = True
                    Else
                        If .AIType = 7 And Not (.AISafetyFactor > FractionMin And .AISafetyFactor <= FractionMax) Then
                            err = True
                        End If
                        If .AIType = 8 And Not .AIResArea > 0 Then
                            err = True
                        End If
                    End If
                End If

                If err Then
                    Throw New Global.System.Exception("There are missing values in Auto Irrigation" & vbCrLf)
                End If
            End With

            Return String.Empty
        Catch ex As System.Exception
            'sender.style.item("background-color") = "#000000"
            Return ex.Message
        End Try
    End Function

    Function validateAutoFertigation(sender As Object, _bmpsInfo As BmpsData)
        Dim err As Boolean = False
        Try
            With _bmpsInfo
                'validate autoirrigation
                If .AFType > 0 Or .AFEff > 0 Or .AFFreq > 0 Or .AFNConc Then
                    If Not (.AFType > 0 And .AFEff > 0 And .AFFreq > 0 And .AFMaxSingleApp > 0) Then
                        err = True
                    Else
                        If .AFType = 7 And Not (.AFSafetyFactor > FractionMin And .AFSafetyFactor <= FractionMax) Then
                            err = True
                        End If
                    End If
                End If
            End With

            If err Then
                Throw New Global.System.Exception("There are missing values in Autofertigation" & vbCrLf)
            End If

            Return String.Empty
        Catch ex As System.Exception
            sender.background = "#000000"
            Return ex.Message
        End Try
    End Function

    Function validatePadsAndPipesNoDitchImprovement(sender As Object, _bmpsInfo As BmpsData)
        Dim err As Boolean = False
        Try
            With _bmpsInfo
                If .PPNDWidth > 0 Or .PPNDSides > 0 Then
                    If Not (.PPNDWidth > 0 And .PPNDSides > 0) Then
                        err = True
                    End If
                End If
            End With

            If err Then
                Throw New Global.System.Exception("There are missing values in Pads and Pipes No Ditch Improvement" & vbCrLf)
            End If

            Return String.Empty
        Catch ex As System.Exception
            sender.background = "#000000"
            Return ex.Message
        End Try
    End Function

    Function validatePadsAndPipesTwoStagaeDitchSystem(sender As Object, _bmpsInfo As BmpsData)
        Dim err As Boolean = False
        Try
            With _bmpsInfo
                If .PPDSWidth > 0 Or .PPDSSides > 0 Then
                    If Not (.PPDSWidth > 0 And .PPDSSides > 0) Then
                        err = True
                    End If
                End If
            End With

            If err Then
                Throw New Global.System.Exception("There are missing values in Pads and Pipes Two-stage Ditch system" & vbCrLf)
            End If

            Return String.Empty
        Catch ex As System.Exception
            sender.background = "#000000"
            Return ex.Message
        End Try
    End Function

    Function validatePadsAndPipesDitchEnlargment(sender As Object, _bmpsInfo As BmpsData)
        Dim err As Boolean = False
        Try
            With _bmpsInfo
                If .PPDEWidth > 0 Or .PPDESides > 0 Or .PPDEResArea > 0 Then
                    If Not (.PPDEWidth > 0 And .PPDESides > 0 And .PPDEResArea > 0) Then
                        err = True
                    End If
                End If
            End With

            If err Then
                Throw New Global.System.Exception("There are missing values in Pads and Pipes Ditch Enlargement" & vbCrLf)
            End If

            Return String.Empty
        Catch ex As System.Exception
            sender.background = "#000000"
            Return ex.Message
        End Try
    End Function

    Function validatePadsAndPipesTailwaterIrrigation(sender As Object, _bmpsInfo As BmpsData)
        Dim err As Boolean = False
        Try
            With _bmpsInfo
                If .PPTWWidth > 0 Or .PPTWSides > 0 Or .PPTWResArea Then
                    If Not (.PPTWWidth > 0 And .PPTWSides > 0 And .PPTWResArea > 0) Then
                        err = True
                    Else
                        If validateAutoIrrigation(sender, _bmpsInfo) <> String.Empty Then err = True
                    End If
                End If
            End With

            If err Then
                Throw New Global.System.Exception("There are missing values in Pads and Pipes Tailwater Irrigation" & vbCrLf)
            End If

            Return String.Empty
        Catch ex As System.Exception
            sender.background = "#000000"
            Return ex.Message
        End Try
    End Function

    Function validateFencing(sender As Object, _bmpsInfo As BmpsData)
        Dim err As Boolean = False
        Try
            With _bmpsInfo
                If .SFAnimals > 0 Or .SFDays > 0 Or .SFHours > 0 Or .SFCode > 0 Or .SFDryManure > 0 Or .SFNo3 > 0 Or .SFPo4 > 0 Or .SFOrgN > 0 Or .SFOrgP > 0 Then
                    If Not (.SFAnimals > 0 And .SFDays > 0 And .SFHours > 0 And .SFCode > 0 And .SFDryManure > 0) Or Not (.SFNo3 > 0 Or .SFPo4 > 0 Or .SFOrgN > 0 Or .SFOrgP > 0) Then
                        err = True
                    End If
                End If
            End With

            If err Then
                Throw New Global.System.Exception("There are missing values in stream fencing (livestock access control)" & vbCrLf)
            End If

            Return String.Empty
        Catch ex As System.Exception
            sender.background = "#000000"
            Return ex.Message
        End Try
    End Function

    Function validateRiparianForest(sender As Object, _bmpsInfo As BmpsData)
        Dim err As Boolean = False
        Try
            With _bmpsInfo
                'validate autoirrigation
                If .RFArea > 0 Or .RFGrassFieldPortion > 0 Or .RFslopeRatio > 0 Or .RFWidth Then
                    If Not (.RFArea > 0 And .RFGrassFieldPortion > 0 And .RFslopeRatio > 0 And .RFWidth) Then
                        err = True
                    End If
                End If
            End With

            If err Then
                Throw New Global.System.Exception("There are missing values in Riparian Forest" & vbCrLf)
            End If

            Return String.Empty
        Catch ex As System.Exception
            sender.background = "#000000"
            Return ex.Message
        End Try
    End Function

    Function validateFilterStrip(sender As Object, _bmpsInfo As BmpsData)
        Dim err As Boolean = False
        Try
            With _bmpsInfo
                'validate autoirrigation
                If .FSArea > 0 Or .FSCrop > 0 Or .FSWidth > 0 Or .FSslopeRatio Then
                    If Not (.FSArea > 0 And .FSCrop > 0 And .FSslopeRatio > 0 And .FSWidth) Then
                        err = True
                    End If
                End If
            End With

            If err Then
                Throw New Global.System.Exception("There are missing values in Filter Strip" & vbCrLf)
            End If

            Return String.Empty
        Catch ex As System.Exception
            sender.background = "#000000"
            Return ex.Message
        End Try
    End Function

    Function validateWaterway(sender As Object, _bmpsInfo As BmpsData)
        Dim err As Boolean = False
        Try
            With _bmpsInfo
                'validate autoirrigation
                If .WWCrop > 0 Or .WWWidth Then
                    If Not (.WWCrop > 0 And .WWWidth) Then
                        err = True
                    End If
                End If
            End With

            If err Then
                Throw New Global.System.Exception("There are missing values in Riparian Forest" & vbCrLf)
            End If

            Return String.Empty
        Catch ex As System.Exception
            sender.background = "#000000"
            Return ex.Message
        End Try
    End Function

    Function validateContourBuffer(sender As Object, _bmpsInfo As BmpsData)
        Dim err As Boolean = False
        Try
            With _bmpsInfo
                'validate autoirrigation
                If .CBCrop > 0 Or .CBCWidth Or .CBBWidth Then
                    If Not (.CBCrop > 0 And .CBCWidth And .CBBWidth) Then
                        err = True
                    End If
                End If
            End With

            If err Then
                Throw New Global.System.Exception("There are missing values in Contour Buffer" & vbCrLf)
            End If

            Return String.Empty
        Catch ex As System.Exception
            sender.background = "#000000"
            Return ex.Message
        End Try
    End Function

End Module
