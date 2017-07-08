Imports System.IO
Public Class Form1
    Dim settingslocation As String = "HKEY_LOCAL_MACHINE\SOFTWARE\Semrau Software Consulting\OSSU"

    Public filelocation As String = ""
    Public destination As String = ""
    Public debug As Boolean = False

    Dim ftphost As String = ""
    Dim ftpusr As String = ""
    Dim ftppw As String = ""

    Dim url As String = ""
    Dim avlibloc As String = ""

    'Feature options
    Dim movefile As Boolean = False
    Dim updateplaylist As Boolean = False
    Dim upload_file As Boolean = False
    Dim delete_converted As Boolean = False


    'Don't change these
    Public filename As String = ""
    Dim mp3name As String = ""
    Dim oggname As String = ""
    Dim stat As String = "Loading..."
    Dim uploading As Boolean
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        movefile = readsetting("move_file")
        updateplaylist = readsetting("update_playlist")
        upload_file = readsetting("upload_file")
        filelocation = readsetting("staging_location")
        avlibloc = readsetting("libav_path")
        delete_converted = readsetting("delete_converted_files")
        If movefile = True Then
            destination = readsetting("move_location")
        End If
        If upload_file = True Then
            ftphost = readsetting("ftp_host")
            ftpusr = readsetting("ftp_user")
            ftppw = readsetting("ftp_pw")
        End If
        If updateplaylist = True Then
            url = readsetting("url")
        End If
        Dim numberoffiles As Integer = Directory.GetFiles(filelocation).Count
        If numberoffiles = 1 Then
            Dim dirinfo As DirectoryInfo = New DirectoryInfo(Directory.GetFiles(filelocation).GetValue(0))
            If ((dirinfo.Extension = ".wav") Or (dirinfo.Extension = ".mp3")) Then
                filename = dirinfo.Name
                loaddata()
            Else
                MsgBox("Error: file is not in a .wav or .mp3 extension.  The only supported file types are .wav and .mp3.")
            End If

        Else
            If numberoffiles = 0 Then
                MsgBox("Error: no files found.  Please move your files to " & filelocation & " with the naming format of month-day-year-speaker_name-sermon_title.wav.")
            Else
                My.Forms.selection.Visible = True
                Me.Visible = False
            End If
        End If

    End Sub
    Private Function readsetting(ByVal settingname As String) As String
        Dim result As String = ""
        Try
            result = My.Computer.Registry.GetValue(settingslocation, settingname, Nothing)
        Catch ex As Exception
            MsgBox("Critical error reading settings values.  Please re-run setup.")
        End Try
        Return result
    End Function
    Public Sub loaddata()
        If Not filename = "" Then
            Me.TextBox2.Text = filename
            If filename.Split("-").Count = 5 Then
                Dim fieldnum As Integer = 1
                Dim filesplit As Array = filename.Replace(".mp3", "").Replace(".wav", "").Split("-")

                Try
                    Me.TextBox1.Text = (filesplit(0) & "-" & filesplit(1) & "-" & filesplit(2))
                    Me.TextBox3.Text = filesplit(3).ToString.Replace("_", " ")
                    Me.TextBox4.Text = filesplit(4).ToString.Replace("_", " ")
                Catch ex As Exception
                    MsgBox("Error parsing data - please enter the data manually.")
                End Try
            End If
        Else
            MsgBox("Error: naming convention not followed.  Please input the information manually.")
        End If
        Me.TextBox2.ReadOnly = True
    End Sub
    Protected Sub conv2mp3()
        If Not My.Computer.FileSystem.FileExists(avlibloc & "avconv.exe") Then
            MsgBox("Conversion error: couldn't find avconv.exe.  Please download Libavtools and extract the files to " & avlibloc & ".")
        Else
            Dim proc As Process = New Process
            'MsgBox(avlibloc & "avconv.exe" & " " & "-i """ & filename & """ " & """" & mp3name & """")
            proc.StartInfo.FileName = (avlibloc & "avconv.exe")
            proc.StartInfo.Arguments = ("-i """ & filelocation & filename & """ " & """" & filelocation & mp3name & """")
            'proc.StartInfo.RedirectStandardError = True
            'proc.StartInfo.UseShellExecute = False
            'proc.StartInfo.CreateNoWindow = True
            proc.Start()
            proc.WaitForExit()
            Me.status.Text = "File converted to mp3."
        End If
    End Sub
    Protected Sub conv2ogg()
        If Not My.Computer.FileSystem.FileExists(avlibloc & "avconv.exe") Then
            MsgBox("Conversion error: couldn't find avconv.exe.  Please download Libavtools and extract the files to " & avlibloc & ".")
        Else
            Dim proc As Process = New Process
            'MsgBox(avlibloc & "avconv.exe" & " " & "-i """ & filename & """ " & """" & mp3name & """")
            proc.StartInfo.FileName = (avlibloc & "avconv.exe")
            proc.StartInfo.Arguments = ("-i """ & filelocation & filename & """ " & """" & filelocation & oggname & """")
            'proc.StartInfo.RedirectStandardError = True
            'proc.StartInfo.UseShellExecute = False
            'proc.StartInfo.CreateNoWindow = True
            proc.Start()
            proc.WaitForExit()
            Me.status.Text = "File converted to ogg."
        End If
    End Sub

    Public Sub UploadFile(ByVal _FileName As String, ByVal _UploadPath As String, ByVal _FTPUser As String, ByVal _FTPPass As String)
        Dim _FileInfo As New System.IO.FileInfo(_FileName)

        ' Create FtpWebRequest object from the Uri provided
        Dim _FtpWebRequest As System.Net.FtpWebRequest = CType(System.Net.FtpWebRequest.Create(New Uri(_UploadPath)), System.Net.FtpWebRequest)

        ' Provide the WebPermission Credintials
        _FtpWebRequest.Credentials = New System.Net.NetworkCredential(_FTPUser, _FTPPass)

        ' By default KeepAlive is true, where the control connection is not closed
        ' after a command is executed.
        _FtpWebRequest.KeepAlive = False

        ' set timeout for 20 seconds
        _FtpWebRequest.Timeout = 20000

        ' Specify the command to be executed.
        _FtpWebRequest.Method = System.Net.WebRequestMethods.Ftp.UploadFile

        ' Specify the data transfer type.
        _FtpWebRequest.UseBinary = True

        ' Notify the server about the size of the uploaded file
        _FtpWebRequest.ContentLength = _FileInfo.Length

        ' The buffer size is set to 2kb
        Dim buffLength As Integer = 2048
        Dim buff(buffLength - 1) As Byte

        ' Opens a file stream (System.IO.FileStream) to read the file to be uploaded
        Dim _FileStream As System.IO.FileStream = _FileInfo.OpenRead()

        Try
            ' Stream to which the file to be upload is written
            Dim _Stream As System.IO.Stream = _FtpWebRequest.GetRequestStream()

            ' Read from the file stream 2kb at a time
            Dim contentLen As Integer = _FileStream.Read(buff, 0, buffLength)

            ' Till Stream content ends
            Do While contentLen <> 0
                ' Write Content from the file stream to the FTP Upload Stream
                _Stream.Write(buff, 0, contentLen)
                contentLen = _FileStream.Read(buff, 0, buffLength)
            Loop

            ' Close the file stream and the Request Stream
            _Stream.Close()
            _Stream.Dispose()
            _FileStream.Close()
            _FileStream.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        uploading = True
        Dim trd As Threading.Thread = New Threading.Thread(AddressOf confirm)
        trd.IsBackground = True
        trd.Start()
        Me.status.Visible = True
        Do While (uploading = True)
            Me.status.Text = stat
            Sleep(2000)
        Loop
        My.Forms.selection.Close()
        Me.Close()
    End Sub
    Private Sub confirm()
        mp3name = (Me.TextBox1.Text & "-" & Me.TextBox3.Text.Replace(" ", "_") & "-" & Me.TextBox4.Text.Replace(" ", "_") & ".mp3")
        If Not filename.Contains(".mp3") Then
            stat = "Converting file to mp3."
            conv2mp3()
        Else
            If Not filename = mp3name Then
                My.Computer.FileSystem.RenameFile((filelocation & filename), mp3name)
            End If
        End If
        oggname = (Me.TextBox1.Text & "-" & Me.TextBox3.Text.Replace(" ", "_") & "-" & Me.TextBox4.Text.Replace(" ", "_") & ".ogg")
        stat = "Converting file to ogg."
        conv2ogg()
        If updateplaylist = True Then
            stat = "Downloading playlist config file."
            My.Computer.Network.DownloadFile((url & "playlist.txt"), (filelocation & "playlist.txt"))
            stat = "Reading playlist file."
            Dim playlist As String = My.Computer.FileSystem.ReadAllText(filelocation & "playlist.txt")
            stat = "Writing new playlist file."
            playlist = ("<New />" & vbCrLf & "Date: " & Me.TextBox1.Text & vbCrLf & "Speaker: " & Me.TextBox3.Text & vbCrLf & "Title: " & Me.TextBox4.Text & vbCrLf & "mp3: " & url & "files/" & mp3name & vbCrLf & "ogg: " & url & "files/" & oggname & vbCrLf & playlist)
            My.Computer.FileSystem.WriteAllText((filelocation & "playlist.txt"), playlist, False)
        End If
        If upload_file = True Then
            stat = "Uploading mp3 file."
            UploadFile((filelocation & mp3name), (ftphost & "/files/" & mp3name), ftpusr, ftppw)
            stat = "Uploading ogg file."
            UploadFile((filelocation & oggname), (ftphost & "/files/" & oggname), ftpusr, ftppw)
            If updateplaylist = True Then
                stat = "Uploading playlist file."
                UploadFile((filelocation & "playlist.txt"), (ftphost & "/playlist.txt"), ftpusr, ftppw)
                stat = "Deleting local copy of playlist file."
                My.Computer.FileSystem.DeleteFile(filelocation & "playlist.txt")
            End If
        End If
        If movefile = True Then
            stat = "Moving original file."
            My.Computer.FileSystem.MoveFile((filelocation & filename), (destination & filename))
            If My.Computer.FileSystem.FileExists(filelocation & filename) Then
                stat = "Deleting original file."
                My.Computer.FileSystem.DeleteFile(filelocation & filename)
            End If
        End If
        If delete_converted = True Then
            If My.Computer.FileSystem.FileExists(filelocation & mp3name) Then
                stat = "Deleting mp3 file."
                My.Computer.FileSystem.DeleteFile(filelocation & mp3name)
            End If
            If My.Computer.FileSystem.FileExists(filelocation & oggname) Then
                stat = "Deleting ogg file."
                My.Computer.FileSystem.DeleteFile(filelocation & oggname)
            End If
        End If
        stat = "Script completed."
        Dim numberoffiles As Integer = Directory.GetFiles(filelocation).Count
        If numberoffiles = 0 Then
            uploading = False
        Else
            Dim ans As String
            ans = MsgBox("Script complete.  More files were found, run again?", vbYesNo)
            If ans = vbYes Then
                Me.TextBox1.Clear()
                Me.TextBox2.Clear()
                Me.TextBox3.Clear()
                Me.TextBox4.Clear()
                If numberoffiles = 1 Then
                    Dim dirinfo As DirectoryInfo = New DirectoryInfo(Directory.GetFiles(filelocation).GetValue(0))
                    If ((dirinfo.Extension = ".wav") Or (dirinfo.Extension = ".mp3")) Then
                        filename = dirinfo.Name
                        loaddata()
                    Else
                        MsgBox("Error: file is not in a .wav or .mp3 extension.  The only supported file types are .wav and .mp3.")
                    End If

                Else
                    If numberoffiles = 0 Then
                        MsgBox("Error: no files found.  Please move your files to " & filelocation & " with the naming format of month-day-year-speaker_name-sermon_title.wav.")
                    Else
                        uploading = False
                    End If
                End If
            Else
                uploading = False
            End If
        End If
    End Sub
    Public Sub Sleep(ByRef MilliSeconds As Integer)
        Dim i As Integer, HalfSeconds As Integer = MilliSeconds / 500
        For i = 1 To HalfSeconds
            Threading.Thread.Sleep(500) : Application.DoEvents()
        Next i
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        My.Forms.selection.Close()
        Me.Close()
        End
    End Sub
End Class
