Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("userGuide") = "" Then
            sessionNumber += 1
            Session("userGuide") = System.Guid.NewGuid.ToString & sessionNumber
        End If
        folder = Server.MapPath("")

        Select Case Page.Request.Params("__EVENTARGUMENT")
            Case english
                Session("Language") = english
            Case spanish
                Session("Language") = spanish
        End Select
        openXMLLanguagesFile()
        lblDescription.Text = aplDoc.Document.Descendants("WelcomeToEat").Value.Replace("br", "<br/>").Replace("bold/", "</b>")
        lblDescription.Text = lblDescription.Text.Replace("bold", "<b>")
        If Not IsPostBack Then Globals.InitializeLists()
    End Sub
End Class