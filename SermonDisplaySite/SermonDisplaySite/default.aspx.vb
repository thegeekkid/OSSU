Imports System.IO
Imports System.Net
Public Class _default
    Inherits System.Web.UI.Page
    'Put the url where this site will be located below.  Be sure to include a "/" at the end.
    Dim url As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim playlist As String
        Dim address As String = (url & "playlist.txt")
        Dim client As WebClient = New WebClient()
        Dim reader As StreamReader = New StreamReader(client.OpenRead(address))
        playlist = reader.ReadToEnd

        Dim html As String = ""
        Dim strdata As String = ""
        Dim mp3name As String = ""

        For Each line As String In playlist.Split(vbCrLf)
            If line.Contains("<New />") Then
                html = (html & "<div class=""audblock"">")
            End If
            If line.Contains("Date: ") Then
                strdata = line.Replace("Date: ", "")
                strdata = (strdata.Replace(vbCrLf, ""))
                strdata = (strdata.Replace(vbCr, ""))
                strdata = (strdata.Replace(vbLf, ""))
                html = (html & vbCrLf & "  <p>" & strdata & " - ")
            End If
            If line.Contains("Speaker: ") Then
                strdata = line.Replace("Speaker: ", "")
                strdata = (strdata.Replace(vbCrLf, ""))
                strdata = (strdata.Replace(vbCr, ""))
                strdata = (strdata.Replace(vbLf, ""))
                html = (html & strdata & "</p>")
            End If
            If line.Contains("Title: ") Then
                strdata = line.Replace("Title: ", "")
                strdata = (strdata.Replace(vbCrLf, ""))
                strdata = (strdata.Replace(vbCr, ""))
                strdata = (strdata.Replace(vbLf, ""))
                html = (html & vbCrLf & "  <p>" & strdata & "</p>")
            End If
            If line.Contains("mp3: ") Then
                mp3name = line.Replace("mp3: ", "")
                mp3name = (mp3name.Replace(vbCrLf, ""))
                mp3name = (mp3name.Replace(vbCr, ""))
                mp3name = (mp3name.Replace(vbLf, ""))
                html = (html & vbCrLf & "  <audio controls>" & vbCrLf & "    <source src=""" & mp3name & """>")
            End If
            If line.Contains("ogg: ") Then
                strdata = line.Replace("ogg: ", "")
                strdata = (strdata.Replace(vbCrLf, ""))
                strdata = (strdata.Replace(vbCr, ""))
                strdata = (strdata.Replace(vbLf, ""))
                html = (html & vbCrLf & "    <source src=""" & strdata & """>" & vbCrLf & "  </audio>" & vbCrLf & "  <p><a href=""" & mp3name & """>Download</a> (Right click - Save As)</p>" & vbCrLf & "</div>" & vbCrLf)


            End If
        Next

        content.InnerHtml = html
    End Sub

End Class