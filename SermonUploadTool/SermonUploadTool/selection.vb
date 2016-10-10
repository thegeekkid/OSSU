Public Class selection
    Private Sub selection_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim dirinfo As System.IO.FileInfo
        For Each dir As String In System.IO.Directory.GetFiles(My.Forms.Form1.filelocation)
            dirinfo = New System.IO.FileInfo(dir)
            If ((dirinfo.Extension = ".wav") Or (dirinfo.Extension = ".mp3")) Then
                Me.ComboBox1.Items.Add(dirinfo.Name)
            End If
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Not Me.ComboBox1.SelectedItem = "" Then
            My.Forms.Form1.filename = Me.ComboBox1.SelectedItem
            Me.Visible = False
            My.Forms.Form1.Visible = True
            My.Forms.Form1.loaddata()
            Me.Close()
        Else
            MsgBox("Error: Please select a file.")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        My.Forms.Form1.Close()
        Me.Close()
        End
    End Sub
End Class