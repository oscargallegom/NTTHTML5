Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data
Imports System.Security.Cryptography

Public Module Database
    Private Const sDBConnectionDefault As String = "Server=DBSERVER;initial catalog=DBCATALOG;persist security info=False"
    Private Const sNTTLvl1UN As String = "sa"
    Private Const sNTTLvl1PW As String = "password"
    Private SQLEX As SqlCommand
    Private sConnectString As String = String.Empty
    Private sServer = String.Empty
    Private sInstance = String.Empty
    Private sCatalog = String.Empty
    Private sUN = String.Empty
    Private sPW = String.Empty
    Private configurationAppSettings As Global.System.Collections.Specialized.NameValueCollection = Global.System.Configuration.ConfigurationManager.AppSettings

    ReadOnly Property dbConnectString(db As String) As String
        Get
            If System.Net.Dns.GetHostName = "T-NN" Then
                sServer = configurationAppSettings.Get("NNServer")
                sInstance = String.Empty
                sInstance = configurationAppSettings.Get("NNInstance")
                sCatalog = configurationAppSettings.Get("NNCatalog")
                sUN = configurationAppSettings.Get("NNUN")
                sPW = configurationAppSettings.Get("NNPW")
                If db = "Soil" Then sCatalog = configurationAppSettings.Get("NNSoilCatalog") : sServer = sServer & "1"
                sServer = sServer + "\" + sInstance
                sConnectString = sDBConnectionDefault.Replace("DBSERVER", sServer)
                sConnectString = sConnectString.Replace("DBCATALOG", sCatalog)
                sConnectString = sConnectString.ToString & ";user id=" & sUN & "; password=" & sPW
                sConnectString = sConnectString & ";Pooling=false"
            Else
                sServer = configurationAppSettings.Get("RSServer")
                sInstance = String.Empty
                sInstance = configurationAppSettings.Get("RSInstance")
                sCatalog = configurationAppSettings.Get("RSCatalog")
                sUN = configurationAppSettings.Get("RSUN")
                sPW = configurationAppSettings.Get("RSPW")
                If db = "Soil" Then sCatalog = configurationAppSettings.Get("RSSoilCatalog")
                sServer = sServer + "\" + sInstance
                sConnectString = sDBConnectionDefault.Replace("DBSERVER", sServer)
                sConnectString = sConnectString.Replace("DBCATALOG", sCatalog)
                sConnectString = sConnectString.ToString & ";user id=" & sUN & "; password=" & sPW
                sConnectString = sConnectString & ";Pooling=false"
            End If
            Return sConnectString
        End Get
    End Property

    Public Sub GetMissingParms(_parmValues As List(Of ParmsData), state As String, parmNumber As Short)
        Dim parm As ParmsData
        Try
            Dim parms = GetParmDesc(state, parmNumber)
            For Each c In parms
                parm = New ParmsData
                With parm
                    .Name = c.item("Name")
                    .Code = c.item("Code")
                    .Value1 = c.item("Value")
                    .Range1 = c.item("Range1")
                    .Range2 = c.item("Range2")
                End With
                If Not _parmValues.Contains(parm) Then
                    _parmValues.Add(parm)
                End If
            Next

        Catch ex As System.Exception
        End Try
    End Sub

    Public Function ValidateLogin(ByVal email As String, password As String, name As String, company As String, date1 As Date) As String
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing
        ValidateLogin = String.Empty
        Try
            sSQL = "Select * from Accounts where email = '" + email + "' AND password = '" + password + "' COLLATE Latin1_General_CS_AS"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)

            If dr.HasRows Then
                ValidateLogin = "OK"
                If company = String.Empty And name = String.Empty Then
                    sSQL = "UPDATE Accounts SET Date_Logged_In = '" & Now & "' WHERE email = '" + email + "' AND password = '" + password + "'"
                    dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
                End If
            Else
                ValidateLogin = String.Empty
                If company <> String.Empty And name <> String.Empty Then
                    sSQL = "INSERT INTO Accounts (eMail, [password], [name], company, Date_Logged_In, [Status]) VALUES ('" & email & "', '" & password & "', '" & name & "', '" & company & "', '" & date1 & "', " & 1 & ")"
                    dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
                    ValidateLogin = "OK"
                End If
            End If
            Return ValidateLogin

        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    Public Function ChangePassword(ByVal email As String, password As String, newPassword As String) As String
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing
        ChangePassword = String.Empty
        Try
            sSQL = "Select * from Accounts where email = '" + email + "' AND password = '" + password + "'"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)

            If dr.HasRows Then
                dr.Read()
                ChangePassword = "OK"
                sSQL = "UPDATE Accounts SET password = '" + newPassword + "' WHERE email = '" + email + "' AND [password] = '" + password + "'"
                dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            End If
            Return ChangePassword

        Catch ex As Exception
            Return ChangePassword
        End Try
    End Function

    Public Function CreatePassword(ByVal email As String) As String
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing
        Dim password As String = RandomPassword.Generate(8, 10)
        CreatePassword = String.Empty
        Try
            sSQL = "Select * from Accounts where email = '" + email + "'"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)

            If dr.HasRows Then
                dr.Read()
                CreatePassword = dr.Item("name") & "|" & password
                sSQL = "UPDATE Accounts SET password = '" + password + "' WHERE email = '" + email + "'"
                dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            Else
                CreatePassword = String.Empty
            End If

            Return CreatePassword

        Catch ex As Exception
            Return CreatePassword
        End Try
    End Function

    Public Function GetWStation(ByVal sCountyCode As String) As List(Of StationInfo)
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing
        Dim station As StationInfo
        GetWStation = New List(Of StationInfo)

        Try
            sSQL = "Select * from WStation where CountyCode like '" + sCountyCode + "' AND DStatus = 1 ORDER BY Name"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)

            If dr.HasRows Then
                Do While dr.Read
                    station = New StationInfo
                    station.Code = dr.Item("code")
                    station.FinalYear = dr.Item("finalYear")
                    station.InitialYear = dr.Item("initialYear")
                    station.Name = dr.Item("name")
                    station.WindCode = dr.Item("windName").ToString.Split(",")(0)
                    station.WindName = dr.Item("windName").ToString.Split(",")(1)
                    station.Wp1Code = dr.Item("wp1Name").ToString.Split(",")(0)
                    station.Wp1Name = dr.Item("wp1Name").ToString.Split(",")(1)
                    station.WSType = "Station"
                    GetWStation.Add(station)
                Loop
            End If
            Return GetWStation

        Catch ex As Exception
            Return GetWStation
        End Try
    End Function

    Public Function GetWeatherCoor(ByVal nlat As Single, ByVal nlon As Single, ByVal latLess As Single, ByVal latPlus As Single, ByVal lonLess As Single, ByVal lonPlus As Single) As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            sSQL = "SELECT TOP 1 lat, lon, FileName, initialYear, finalYear, (lat - " & nlat & ") + (lon + " & nlon & ") as distance FROM weatherCoor " & _
                   "WHERE lat > " & latLess & " and lat < " & latPlus & " and lon > " & lonLess & " and lon < " & lonPlus & " ORDER BY distance"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function

    Public Function GetSSA(countyCode As String) As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            sSQL = "SELECT name, code FROM SSArea WHERE CountyCode = '" & countyCode & "' ORDER BY Name"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function

    Public Function GetCropMatrix(stateAbrev As String) As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            sSQL = "SELECT CMCrop, CMVar12, CMTillage FROM CropMatrixTypical WHERE StateAbrev = '" & stateAbrev & "' AND DStatus = 1 ORDER BY CMCrop, CMTillage"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function

    Public Function GetFerts() As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            sSQL = "SELECT * FROM APEXFert WHERE DStatus = 1 ORDER BY Name"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function

    Public Function GetOpTypes() As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            sSQL = "SELECT * FROM OpTypes WHERE Status = 'True' ORDER BY Name"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function

    Public Function GetOperations() As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            sSQL = "SELECT * FROM operations ORDER BY Code"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function

    Public Function GetCrops(stateAbrev As String, sql As UShort) As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            Select Case sql
                Case 0
                    'sSQL = "SELECT * FROM APEXCROPS JOIN crops_events ON crops_events.ApexcROP = APEXCrops.CropNumber " & _
                    '       "WHERE APEXCrops.StateAbrev = '**' AND (Crops_events.TEStatecode = '" & stateAbrev & "' OR crops_Events.TEStateCode = 'ALL') " & _
                    '       "ORDER BY APEXCrops.CropName"

                    sSQL = "SELECT * FROM APEXCROPS, EventsTypical " & _
                            "WHERE(EventsTypical.ApexCrop = APEXCrops.CropNumber) AND EventsTypical.TEStateCode = '" & stateAbrev & "'" & _
                            "ORDER BY APEXCrops.CropName"
                Case 1
                    sSQL = "SELECT * FROM APEXCROPS JOIN crops_events ON crops_events.ApexcROP = APEXCrops.CropNumber " & _
                           "WHERE APEXCrops.StateAbrev = '**' AND crops_Events.TEStateCode = 'ALL' " & _
                           "ORDER BY APEXCrops.CropName"
                Case 2
                    sSQL = "SELECT * FROM APEXCROPS " & _
                           "Where APEXCrop.StateAbrev = '**'" & _
                           "ORDER BY APEXCrops.CropName"
                Case 3
                    sSQL = "SELECT * FROM APEXCROPS " & _
                           "Where (StateAbrev = '**' And FilterStrip Like '%FS%') Or CropNumber = " & cropRoad & " Or (StateAbrev = '**' And LUNumber = 28)" & _
                           "ORDER BY CropName"
                Case 4
                    sSQL = "SELECT * FROM APEXCROPS " & _
                           "Where (StateAbrev = '**' And FilterStrip Like '%FS%') Or CropNumber = " & cropRoad & " Or (StateAbrev = '**')" & _
                           "ORDER BY CropName"
                Case 9   'Select additional crops not in eventTypical table but in crop_state table
                    sSQL = "SELECT * FROM Crop_State_Crop " & _
                           "Where (State_Abbrev = '" & stateAbrev & "')"
            End Select


                    dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
                    Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function

    Public Function GetCroppingOperations(var12 As String, stateAbrev As String) As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            sSQL = "SELECT * FROM EventsTypicalExtended WHERE VAR12 = '" & var12 & "' AND TEStateCode = '" & stateAbrev & "' ORDER BY [Year], [Month], [Day], EVENTID"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function

    Public Function GetAnimalUnits() As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            'Dim animalUnits = (From AnimalUnit In dc.AnimalUnits Select AnimalUnit Where AnimalUnit.Status = True)
            sSQL = "SELECT * FROM AnimalUnit WHERE Status = 'True' AND DryManure > 0"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function

    Public Function GetFeeds() As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            'Dim feedQuery = From feed In dc.Feeds Order By feed.Name Select feed
            sSQL = "SELECT * FROM feeds ORDER BY [Name]"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function
    Public Function GetStructures() As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            'Dim animalUnits = (From AnimalUnit In dc.AnimalUnits Select AnimalUnit Where AnimalUnit.Status = True)
            sSQL = "SELECT * FROM Structures ORDER BY [Name]"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function
    Public Function GetOtherInputs() As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            'Dim animalUnits = (From AnimalUnit In dc.AnimalUnits Select AnimalUnit Where AnimalUnit.Status = True)
            sSQL = "SELECT * FROM OtherInput ORDER BY [Name]"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function
    Public Function GetEquipments() As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            'Dim animalUnits = (From AnimalUnit In dc.AnimalUnits Select AnimalUnit Where AnimalUnit.Status = True)
            sSQL = "SELECT * FROM Equipment ORDER BY [Name]"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function
    Public Function GetStates() As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            '_states = From state In dc.States Order By state.Name Where state.DStatusSL = "1" Or state.DStatusSL = "9" Select state
            sSQL = "SELECT * FROM State WHERE DStatusSL = 1 OR DStatusSL = 9 ORDER BY [Name]"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function
    Public Function GetCounties() As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            'Dim countyQuery = From cnty In dc.Counties Order By cnty.Name Where cnty.DStatusSL = "1" Or cnty.DStatusSL = "9" Select cnty
            sSQL = "SELECT * FROM County WHERE DStatusSL = 1 OR DStatusSL = 9 ORDER BY [Name]"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function
    Public Function GetControlsDesc(stateAbrev As String) As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            sSQL = "SELECT * FROM ControlsDesc WHERE State = '" & stateAbrev & "'"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            If Not dr.HasRows Then
                sSQL = "SELECT * FROM ControlsDesc WHERE State = '" & "**" & "'"
                dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            End If
            Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function
    Public Function GetParmDesc(stateAbrev As String, parmNumber As Short) As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            sSQL = "SELECT * FROM ParmsDesc WHERE State = '" & stateAbrev & "' AND code > " & parmNumber & " ORDER BY code"
            dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            If Not dr.HasRows Then
                sSQL = "SELECT * FROM ParmsDesc WHERE State = '" & "**" & "' AND code > " & parmNumber & " ORDER BY code"
                dr = SqlHelper.ExecuteReader(dbConnectString("No"), CommandType.Text, sSQL)
            End If
            Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function

    Public Function GetSoils(SSA As String, StateAbrev As String) As SqlDataReader
        Dim sSQL As String = String.Empty
        Dim dr As SqlDataReader = Nothing

        Try
            '(From Soil In dcSoil.MDSOILs Order By Soil.MUNAME Where Soil.TSSSACode = ddlSSA.Items(ddlSSA.SelectedIndex).Value.Trim Select Soil.MUID, key = Soil.HORIZDESC1 & " | " & Soil.MUNAME).Distinct
            sSQL = "SELECT DISTINCT muid, horizdesc1 + ' | ' + muname AS [key], muname FROM " & StateAbrev & "SOILs WHERE TSSSACode = '" & SSA & "' ORDER BY muname"
            dr = SqlHelper.ExecuteReader(dbConnectString("Soil"), CommandType.Text, sSQL)
            Return dr

        Catch ex As Exception
            Return dr
        End Try
    End Function

    Public Class RandomPassword

        ' Define default min and max password lengths.
        Private Shared DEFAULT_MIN_PASSWORD_LENGTH As Integer = 8
        Private Shared DEFAULT_MAX_PASSWORD_LENGTH As Integer = 10

        ' Define supported password characters divided into groups.
        ' You can add (or remove) characters to (from) these groups.
        Private Shared PASSWORD_CHARS_LCASE As String = "abcdefgijkmnopqrstwxyz"
        Private Shared PASSWORD_CHARS_UCASE As String = "ABCDEFGHJKLMNPQRSTWXYZ"
        Private Shared PASSWORD_CHARS_NUMERIC As String = "23456789"
        Private Shared PASSWORD_CHARS_SPECIAL As String = "*$-+?_&=!%{}/"

        ' <summary>
        ' Generates a random password.
        ' </summary>
        ' <returns>
        ' Randomly generated password.
        ' </returns>
        ' <remarks>
        ' The length of the generated password will be determined at
        ' random. It will be no shorter than the minimum default and
        ' no longer than maximum default.
        ' </remarks>
        Public Shared Function Generate() As String
            Generate = Generate(DEFAULT_MIN_PASSWORD_LENGTH, _
                                DEFAULT_MAX_PASSWORD_LENGTH)
        End Function

        ' <summary>
        ' Generates a random password of the exact length.
        ' </summary>
        ' <param name="length">
        ' Exact password length.
        ' </param>
        ' <returns>
        ' Randomly generated password.
        ' </returns>
        Public Shared Function Generate(length As Integer) As String
            Generate = Generate(length, length)
        End Function

        ' <summary>
        ' Generates a random password.
        ' </summary>
        ' <param name="minLength">
        ' Minimum password length.
        ' </param>
        ' <param name="maxLength">
        ' Maximum password length.
        ' </param>
        ' <returns>
        ' Randomly generated password.
        ' </returns>
        ' <remarks>
        ' The length of the generated password will be determined at
        ' random and it will fall with the range determined by the
        ' function parameters.
        ' </remarks>
        Public Shared Function Generate(minLength As Integer, _
                                        maxLength As Integer) _
            As String

            ' Make sure that input parameters are valid.
            If (minLength <= 0 Or maxLength <= 0 Or minLength > maxLength) Then
                Generate = Nothing
            End If

            ' Create a local array containing supported password characters
            ' grouped by types. You can remove character groups from this
            ' array, but doing so will weaken the password strength.
            Dim charGroups As Char()() = New Char()() _
            { _
                PASSWORD_CHARS_LCASE.ToCharArray(), _
                PASSWORD_CHARS_UCASE.ToCharArray(), _
                PASSWORD_CHARS_NUMERIC.ToCharArray(), _
                PASSWORD_CHARS_SPECIAL.ToCharArray() _
            }

            ' Use this array to track the number of unused characters in each
            ' character group.
            Dim charsLeftInGroup As Integer() = New Integer(charGroups.Length - 1) {}

            ' Initially, all characters in each group are not used.
            Dim I As Integer
            For I = 0 To charsLeftInGroup.Length - 1
                charsLeftInGroup(I) = charGroups(I).Length
            Next

            ' Use this array to track (iterate through) unused character groups.
            Dim leftGroupsOrder As Integer() = New Integer(charGroups.Length - 1) {}

            ' Initially, all character groups are not used.
            For I = 0 To leftGroupsOrder.Length - 1
                leftGroupsOrder(I) = I
            Next

            ' Because we cannot use the default randomizer, which is based on the
            ' current time (it will produce the same "random" number within a
            ' second), we will use a random number generator to seed the
            ' randomizer.

            ' Use a 4-byte array to fill it with random bytes and convert it then
            ' to an integer value.
            Dim randomBytes As Byte() = New Byte(3) {}

            ' Generate 4 random bytes.
            Dim rng As RNGCryptoServiceProvider = New RNGCryptoServiceProvider()

            rng.GetBytes(randomBytes)

            ' Convert 4 bytes into a 32-bit integer value.
            Dim seed As Integer = BitConverter.ToInt32(randomBytes, 0)

            ' Now, this is real randomization.
            Dim random As Random = New Random(seed)

            ' This array will hold password characters.
            Dim password As Char() = Nothing

            ' Allocate appropriate memory for the password.
            If (minLength < maxLength) Then
                password = New Char(random.Next(minLength - 1, maxLength)) {}
            Else
                password = New Char(minLength - 1) {}
            End If

            ' Index of the next character to be added to password.
            Dim nextCharIdx As Integer

            ' Index of the next character group to be processed.
            Dim nextGroupIdx As Integer

            ' Index which will be used to track not processed character groups.
            Dim nextLeftGroupsOrderIdx As Integer

            ' Index of the last non-processed character in a group.
            Dim lastCharIdx As Integer

            ' Index of the last non-processed group.
            Dim lastLeftGroupsOrderIdx As Integer = leftGroupsOrder.Length - 1

            ' Generate password characters one at a time.
            For I = 0 To password.Length - 1

                ' If only one character group remained unprocessed, process it;
                ' otherwise, pick a random character group from the unprocessed
                ' group list. To allow a special character to appear in the
                ' first position, increment the second parameter of the Next
                ' function call by one, i.e. lastLeftGroupsOrderIdx + 1.
                If (lastLeftGroupsOrderIdx = 0) Then
                    nextLeftGroupsOrderIdx = 0
                Else
                    nextLeftGroupsOrderIdx = random.Next(0, lastLeftGroupsOrderIdx)
                End If

                ' Get the actual index of the character group, from which we will
                ' pick the next character.
                nextGroupIdx = leftGroupsOrder(nextLeftGroupsOrderIdx)

                ' Get the index of the last unprocessed characters in this group.
                lastCharIdx = charsLeftInGroup(nextGroupIdx) - 1

                ' If only one unprocessed character is left, pick it; otherwise,
                ' get a random character from the unused character list.
                If (lastCharIdx = 0) Then
                    nextCharIdx = 0
                Else
                    nextCharIdx = random.Next(0, lastCharIdx + 1)
                End If

                ' Add this character to the password.
                password(I) = charGroups(nextGroupIdx)(nextCharIdx)

                ' If we processed the last character in this group, start over.
                If (lastCharIdx = 0) Then
                    charsLeftInGroup(nextGroupIdx) = _
                                    charGroups(nextGroupIdx).Length
                    ' There are more unprocessed characters left.
                Else
                    ' Swap processed character with the last unprocessed character
                    ' so that we don't pick it until we process all characters in
                    ' this group.
                    If (lastCharIdx <> nextCharIdx) Then
                        Dim temp As Char = charGroups(nextGroupIdx)(lastCharIdx)
                        charGroups(nextGroupIdx)(lastCharIdx) = _
                                    charGroups(nextGroupIdx)(nextCharIdx)
                        charGroups(nextGroupIdx)(nextCharIdx) = temp
                    End If

                    ' Decrement the number of unprocessed characters in
                    ' this group.
                    charsLeftInGroup(nextGroupIdx) = _
                               charsLeftInGroup(nextGroupIdx) - 1
                End If

                ' If we processed the last group, start all over.
                If (lastLeftGroupsOrderIdx = 0) Then
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1
                    ' There are more unprocessed groups left.
                Else
                    ' Swap processed group with the last unprocessed group
                    ' so that we don't pick it until we process all groups.
                    If (lastLeftGroupsOrderIdx <> nextLeftGroupsOrderIdx) Then
                        Dim temp As Integer = _
                                    leftGroupsOrder(lastLeftGroupsOrderIdx)
                        leftGroupsOrder(lastLeftGroupsOrderIdx) = _
                                    leftGroupsOrder(nextLeftGroupsOrderIdx)
                        leftGroupsOrder(nextLeftGroupsOrderIdx) = temp
                    End If

                    ' Decrement the number of unprocessed groups.
                    lastLeftGroupsOrderIdx = lastLeftGroupsOrderIdx - 1
                End If
            Next

            ' Convert password characters into a string and return the result.
            Generate = New String(password)
        End Function
    End Class

End Module
