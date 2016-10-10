Public Class Form1
    'Basic settings:
    Dim defaultinstalldir As String = "C:\Program Files\Semrau Software Consulting\OSSU\"
    Dim mirror As String = "https://github.com/thegeekkid/OSSU/raw/master/SermonUploadTool/SermonUploadTool/bin/Debug/SermonUploadTool.exe"
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.TextBox7.Text = defaultinstalldir
        Me.FolderBrowserDialog3.SelectedPath = defaultinstalldir
    End Sub

    Private Sub FolderBrowserDialog1_select(sender As Object, e As EventArgs) Handles FolderBrowserDialog1.Disposed
        Me.TextBox1.Text = FolderBrowserDialog1.SelectedPath
    End Sub

    Private Sub FolderBrowserDialog2_select(sender As Object, e As EventArgs) Handles FolderBrowserDialog2.Disposed
        Me.TextBox2.Text = FolderBrowserDialog2.SelectedPath
    End Sub

    Private Sub FolderBrowserDialog3_select(sender As Object, e As EventArgs) Handles FolderBrowserDialog3.Disposed
        Me.TextBox7.Text = FolderBrowserDialog3.SelectedPath
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If Me.CheckBox2.Checked Then
            Me.Label4.Visible = True
            Me.TextBox2.Visible = True
            Me.Button2.Visible = True
        Else
            Me.Label4.Visible = False
            Me.TextBox2.Visible = False
            Me.Button2.Visible = False
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If Me.CheckBox3.Checked Then
            Me.Panel1.Visible = True
        Else
            Me.Panel1.Visible = False
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If Me.CheckBox4.Checked Then
            Me.Label9.Visible = True
            Me.TextBox6.Visible = True
        Else
            Me.Label9.Visible = False
            Me.TextBox6.Visible = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.FolderBrowserDialog1.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.FolderBrowserDialog2.ShowDialog()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Me.FolderBrowserDialog3.ShowDialog()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If testftp() = True Then
            MsgBox("FTP connection successful!  (Only verified read permissions though... if you get errors in the actual program, check the FTP permissions.)")
        Else
            MsgBox("FTP connection unsuccessful.")
        End If
    End Sub

    Private Function testftp() As Boolean
        Dim success As Boolean = False
        If Not Me.TextBox3.Text.StartsWith("ftp://") Then
            Dim result As Integer = MessageBox.Show("Do you want to use ftp://" & Me.TextBox3.Text & "as the host name instead of " & Me.TextBox3.Text & "?", "Host confirmation", MessageBoxButtons.YesNo)
            If result = MsgBoxResult.Yes Then
                Me.TextBox3.Text = "ftp://" & Me.TextBox3.Text
            End If
        End If
        Try
            Dim _UploadPath As String = Me.TextBox3.Text
            Dim _FtpWebRequest As System.Net.FtpWebRequest = CType(System.Net.FtpWebRequest.Create(New Uri(_UploadPath)), System.Net.FtpWebRequest)
            _FtpWebRequest.Credentials = New System.Net.NetworkCredential(Me.TextBox4.Text, Me.TextBox5.Text)
            _FtpWebRequest.KeepAlive = False
            _FtpWebRequest.Timeout = 20000
            _FtpWebRequest.Method = System.Net.WebRequestMethods.Ftp.ListDirectory
            _FtpWebRequest.UseBinary = True

            _FtpWebRequest.GetResponse()

            success = True
        Catch ex As Exception
            success = False
            MsgBox(ex.ToString)
        End Try
        Return success
    End Function
End Class
