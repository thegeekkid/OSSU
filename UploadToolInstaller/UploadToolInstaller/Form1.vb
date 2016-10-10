Public Class Form1
    'Basic settings:
    Dim defaultinstalldir As String = "C:\Program Files\Semrau Software Consulting\OSSU\"
    Dim mirror As String = ""
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
End Class
