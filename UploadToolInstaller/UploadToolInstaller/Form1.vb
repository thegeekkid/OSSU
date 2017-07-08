Imports System.IO.Compression
Imports IWshRuntimeLibrary
Public Class Form1
    'Basic settings:
    Dim defaultinstalldir As String = "C:\Program Files\Semrau Software Consulting\OSSU\"
    Dim settingslocation As String = "SOFTWARE\Semrau Software Consulting\OSSU"
    Dim mirror As String = "https://downloads.semrauconsulting.com/ossu/SermonUploadTool.exe"
    'Will probably have to update both of the next vars when we update libav.
    Dim libavmirror As String = "https://downloads.semrauconsulting.com/ossu/libav-11.3-win32.zip"
    Dim libavsubpath As String = "libav-11.3-win32\win32\usr\bin\"

    'Don't touch - used in execution
    Dim status As String = ""
    Dim installing As Boolean = False
    Dim progress As Integer = 0
    Dim installdir As String = ""
    Dim failedon As String = ""


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.TextBox7.Text = defaultinstalldir
        'Me.FolderBrowserDialog3.RootFolder = defaultinstalldir
        Me.FolderBrowserDialog3.SelectedPath = defaultinstalldir
    End Sub
    Private Sub Form1_Close(sender As Object, e As EventArgs) Handles MyBase.FormClosing
        My.Forms.License.Close()
        Me.Close()
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
        If Me.FolderBrowserDialog1.ShowDialog.OK Then
            Me.TextBox1.Text = Me.FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.FolderBrowserDialog2.ShowDialog()
        If Me.FolderBrowserDialog2.ShowDialog.OK Then
            Me.TextBox2.Text = Me.FolderBrowserDialog2.SelectedPath
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Me.FolderBrowserDialog3.ShowDialog()
        If Me.FolderBrowserDialog3.ShowDialog.OK Then
            Me.TextBox7.Text = Me.FolderBrowserDialog3.SelectedPath
        End If
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

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        lockform()
        If Not TextBox1.Text.EndsWith("\") Then
            TextBox1.Text = TextBox1.Text + "\"
        End If
        If Not TextBox2.Text.EndsWith("\") Then
            TextBox2.Text = TextBox1.Text + "\"
        End If
        If Not TextBox7.Text.EndsWith("\") Then
            TextBox7.Text = TextBox1.Text + "\"
        End If
        Dim trd As System.Threading.Thread = New System.Threading.Thread(AddressOf doinstall)
        trd.IsBackground = True
        trd.Start()
        installing = True
        Me.Label11.Text = status
        Me.Label11.Visible = True
        Me.ProgressBar1.Visible = True
        Do While (installing = True)
            Me.Label11.Text = status
            Dim prog As Integer = progress
            If prog < 100 Then
                Me.ProgressBar1.Value = prog
            Else
                Me.ProgressBar1.Value = 100
            End If
            Sleep(2000)
        Loop
        Me.Label11.Text = status
        Me.ProgressBar1.Value = 100
        If failedon = "" Then
            MsgBox("Installation complete!")
            My.Forms.License.Close()
            Me.Close()
        Else
            failedon = ""
            unlockform()
            Me.ProgressBar1.Visible = False
            Me.Label11.Visible = False
        End If
    End Sub
    Private Sub lockform()
        Me.CheckBox2.Enabled = False
        Me.CheckBox3.Enabled = False
        Me.CheckBox4.Enabled = False
        Me.CheckBox5.Enabled = False
        Me.TextBox1.Enabled = False
        Me.TextBox2.Enabled = False
        Me.TextBox3.Enabled = False
        Me.TextBox4.Enabled = False
        Me.TextBox5.Enabled = False
        Me.TextBox6.Enabled = False
        Me.TextBox7.Enabled = False
        Me.Button1.Enabled = False
        Me.Button2.Enabled = False
        Me.Button3.Enabled = False
        Me.Button4.Enabled = False
        Me.Button5.Enabled = False
    End Sub
    Private Sub unlockform()
        Me.CheckBox2.Enabled = True
        Me.CheckBox3.Enabled = True
        Me.CheckBox4.Enabled = True
        Me.CheckBox5.Enabled = True
        Me.TextBox1.Enabled = True
        Me.TextBox2.Enabled = True
        Me.TextBox3.Enabled = True
        Me.TextBox4.Enabled = True
        Me.TextBox5.Enabled = True
        Me.TextBox6.Enabled = True
        Me.TextBox7.Enabled = True
        Me.Button1.Enabled = True
        Me.Button2.Enabled = True
        Me.Button3.Enabled = True
        Me.Button4.Enabled = True
        Me.Button5.Enabled = True
    End Sub
    Private Sub doinstall()
        Try
            status = "Running pre-install checks..."
            progress = 1
            If installchecks() = True Then
                status = "Pre-install checks passed.  Downloading core program..."
                progress = 5
                Dim fatalerror As Boolean = False
                Try
                    My.Computer.Network.DownloadFile(mirror, installdir & "SermonUploadTool.exe")
                Catch ex As Exception
                    MsgBox("Fatal error: Could not download program files: " & vbCrLf & ex.ToString)
                    fatalerror = True
                End Try
                If fatalerror = False Then
                    status = "Core program downloaded.  Downloading Libav from default mirror..."
                    progress = 25
                    Try
                        My.Computer.Network.DownloadFile(libavmirror, installdir & "libav.zip")
                        ZipFile.ExtractToDirectory(installdir & "libav.zip", installdir & "libav")
                    Catch ex As Exception
                        MsgBox("Fatal error: Could not download libav." & vbCrLf & ex.ToString)
                        fatalerror = True
                    End Try
                End If
                If fatalerror = False Then
                    status = "Libav downloaded.  Extracting Libav..."
                    progress = 50
                    Try
                        If My.Computer.FileSystem.FileExists(installdir & "libav.zip") Then
                            My.Computer.FileSystem.DeleteFile(installdir & "libav.zip")
                        End If
                    Catch ex As Exception
                        MsgBox("Non-critical error: Could not delete the zipped libav library.  This shouldn't hurt anything, but if you want to keep things clean, go delete " & installdir & "libav.zip manually.")
                    End Try
                End If
                If fatalerror = False Then
                    status = "Libav extracted.  Creating registry keys..."
                    progress = 75
                    Try
                        Dim setloc As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey(settingslocation)
                        If setloc Is Nothing Then
                            My.Computer.Registry.LocalMachine.CreateSubKey(settingslocation)
                        End If
                    Catch ex As Exception
                        MsgBox("Fatal error: Could not create the necessary registry key. " & vbCrLf & ex.ToString)
                    End Try
                End If
                If fatalerror = False Then
                    status = "Registry keys created.  Setting registry values..."
                    progress = 80
                    Try
                        setsetting("move_file", Me.CheckBox2.Checked)
                        progress = 82
                        setsetting("upload_file", Me.CheckBox3.Checked)
                        progress = 84
                        setsetting("update_playlist", Me.CheckBox4.Checked)
                        progress = 86
                        setsetting("delete_converted_files", Me.CheckBox5.Checked)
                        progress = 88
                        setsetting("staging_location", Me.TextBox1.Text)
                        progress = 90
                        setsetting("libav_path", installdir & libavsubpath)
                        progress = 92
                        If Me.CheckBox2.Checked Then
                            setsetting("move_location", Me.TextBox2.Text)
                        End If
                        progress = 94
                        If Me.CheckBox3.Checked Then
                            setsetting("ftp_host", Me.TextBox3.Text)
                            progress = 96
                            setsetting("ftp_user", Me.TextBox4.Text)
                            progress = 98
                            setsetting("ftp_pw", Me.TextBox5.Text)
                        End If
                        progress = 99
                        If Me.CheckBox4.Checked Then
                            setsetting("url", Me.TextBox6.Text)
                        End If
                    Catch ex As Exception
                        MsgBox("Fatal error: Could not set some of the registry values: " & vbCrLf & ex.ToString)
                    End Try
                End If
                If fatalerror = False Then
                    status = "Creating shortcut on desktop."
                    CreateShortCut(installdir & "SermonUploadTool.exe", installdir, "Sermon Upload Tool")
                    status = "Installation finished!"
                    progress = 100
                    installing = False
                End If
            Else
                status = "Pre-checks failed..."
                progress = 0
                MsgBox("Error: Pre-installation checks failed.  The check that failed was: " & failedon & ".")
                installing = False
            End If
        Catch ex As Exception
            MsgBox("Fatal installation error: " & vbCrLf & ex.ToString)
        End Try
    End Sub
    Private Sub setsetting(ByVal settingname As String, ByVal value As String)
        Try
            My.Computer.Registry.SetValue("HKEY_LOCAL_MACHINE\" & settingslocation, settingname, value)
        Catch ex As Exception
            MsgBox("Critical error reading settings values.  Please re-run setup. " & vbCrLf & ex.ToString)
        End Try
    End Sub
    Private Sub CreateShortCut(ByVal FileName As String, ByVal path As String, ByVal Title As String)
        Try
            Dim wsh As Object = CreateObject("WScript.Shell")

            wsh = CreateObject("WScript.Shell")

            Dim MyShortcut, DesktopPath

            ' Read desktop path using WshSpecialFolders object

            DesktopPath = wsh.SpecialFolders("Desktop")

            ' Create a shortcut object on the desktop

            MyShortcut = wsh.CreateShortcut(DesktopPath & "\" & Title & ".lnk")

            ' Set shortcut object properties and save it

            MyShortcut.TargetPath = wsh.ExpandEnvironmentStrings(FileName)

            MyShortcut.WorkingDirectory = wsh.ExpandEnvironmentStrings(path)

            MyShortcut.WindowStyle = 4

            'Save the shortcut

            MyShortcut.Save()
        Catch ex As System.Exception
            MsgBox("Non critical error: Could not create the shortcut.  Please manually create any shortcuts that you want.")
        End Try
    End Sub
    Private Function installchecks() As Boolean
        Dim good2go As Boolean = True
        If My.Computer.FileSystem.DirectoryExists(Me.TextBox7.Text) Then
            Dim result As Integer = MessageBox.Show("The installation directory that you selected already exists.  If you continue, all contents of this directory will be deleted.  Are you sure that you want to continue?", "Are you sure?", MessageBoxButtons.YesNo)
            If result = MsgBoxResult.Yes Then
                Try
                    My.Computer.FileSystem.DeleteDirectory(Me.TextBox7.Text, FileIO.DeleteDirectoryOption.DeleteAllContents)
                    My.Computer.FileSystem.CreateDirectory(Me.TextBox7.Text)
                    If Me.TextBox7.Text.EndsWith("\") Then
                        installdir = Me.TextBox7.Text
                    Else
                        installdir = Me.TextBox7.Text & "\"
                    End If
                Catch ex As Exception
                    MsgBox("Error clearing installation directory: " & vbCrLf & ex.ToString)
                    failedon = "Making installation directory"
                    good2go = False
                End Try
            Else
                good2go = False
            End If
        Else
            Try
                My.Computer.FileSystem.CreateDirectory(Me.TextBox7.Text)
                If Me.TextBox7.Text.EndsWith("\") Then
                    installdir = Me.TextBox7.Text
                Else
                    installdir = Me.TextBox7.Text & "\"
                End If
            Catch ex As Exception
                MsgBox("Error clearing installation directory: " & vbCrLf & ex.ToString)
                failedon = "Making installation directory"
                good2go = False
            End Try
        End If
        If good2go = True Then
            If Me.CheckBox2.Checked Then
                If Me.TextBox1.Text = Me.TextBox2.Text Then
                    good2go = False
                    MsgBox("Error - the source folder and destination folder can not be the same if you are moving files.")
                    failedon = "Verifying source and target directories"
                End If
            End If
        End If
        If good2go = True Then
            If Me.CheckBox3.Checked Then
                If testftp() = False Then
                    good2go = False
                    failedon = "Verifying FTP connection"
                End If
            End If
        End If
        If good2go = True Then
            If Me.CheckBox4.Checked Then
                If testwebsite(Me.TextBox6.Text) = False Then
                    good2go = False
                    failedon = "Verifying access to website"
                End If
            End If
        End If
        Try
            If Not My.Computer.FileSystem.DirectoryExists(Me.TextBox1.Text) Then
                My.Computer.FileSystem.CreateDirectory(Me.TextBox1.Text)
            End If
            If Not My.Computer.FileSystem.DirectoryExists(Me.TextBox2.Text) Then
                My.Computer.FileSystem.CreateDirectory(Me.TextBox2.Text)
            End If
        Catch ex As Exception
            MsgBox("Non critical error: We couldn't create the directories that you specified for either the source or destination of the files.  This is normal if the folder(s) already exist.  Just make sure that both of these folders have been created before using the product.")
        End Try
        Return good2go
    End Function
    Private Function testwebsite(ByVal url As String)
        Dim good As Boolean = False

        Dim testfile As String = ""
        If Me.TextBox6.Text.EndsWith("/") Then
            testfile = Me.TextBox6.Text & "playlist.txt"
        Else
            testfile = Me.TextBox6.Text & "/" & "playlist.txt"
        End If
        Try
            My.Computer.Network.DownloadFile(testfile, installdir & "playlist.txt")
            good = True
        Catch ex As Exception
            Dim result As Integer = MessageBox.Show("Error downloading playlist file.  If you don't have the display site up yet, this is normal.  Do you want to ignore this error?  The error we found was: " & vbCrLf & ex.ToString, "Ignore playlist file download error?", MessageBoxButtons.YesNo)
            If result = MsgBoxResult.Yes Then
                good = True
            Else
                good = False
            End If
        End Try
        Try
            If My.Computer.FileSystem.FileExists(installdir & "playlist.txt") Then
                My.Computer.FileSystem.DeleteFile(installdir & "playlist.txt")
            End If
        Catch ex As Exception
            MsgBox("Non-fatal error: could not delete the test playlist file that we downloaded.  You might want to go and delete it manually; however, it most likely will not hurt anything by being there.  The path is: " & installdir & "playlist.txt")
        End Try
        Return good
    End Function

    Private Sub Sleep(ByRef MilliSeconds As Integer)
        Dim i As Integer, HalfSeconds As Integer = MilliSeconds / 500
        For i = 1 To HalfSeconds
            Threading.Thread.Sleep(500) : Application.DoEvents()
        Next i
    End Sub
End Class
