# Open Source Sermon Uploader (OSSU)

### Overview:

OSSU is a tool designed to automate the process of uploading a recording of a sermon to a church's website at the end of the service.
It offers the following features - all except the conversion features are optional and can be configured automatically during the installation with the installation tool, or manually by editing the registry.
* Conversion of source file from WAV to MP3 and OGG or conversion from MP3 to OGG if the source is MP3.  (Conversion provided by Libav which will be installed automatically by the pre-compiled installer, or can be installed manually from [https://libav.org/download/](https://libav.org/download/)).
* Archival of source file by moving it to designated directory.
* Automated upload of file to FTP site.
* Included ASP.NET display site that will parse through a playlist.txt file and write HTML5 markup that is easy to integrate with CSS via classes.  OSSU can download this playlist.txt file and update it with the new audio files automatically.
* Deletion of source files after upload and/or move to the archival folder.

### Naming convention:

OSSU is configured to use the following naming convention:
MM-DD-YYYY-Speaker_Name-Sermon_Title.wav (or .mp3)
If that naming convention is followed, OSSU will automatically parse that data and use it where necessary.  Otherwise, the person uploading the file can manually enter the data in the confirmation form.
Keep in mind that using spaces in file names is a very bad idea - especially when being uploaded to the web.  This is why we *strongly* recommend using underscores instead of spaces.

#### Manual installation:

If you would like to install OSSU manually, please follow the steps below:

1.  Download the OSSU core executable from GitHub.
2.  Save this to a path of your choosing.
3.  Download Libav from the mirror of your choice.
4.  Extract Libav and make note of where the avconv.exe file is - you will need this later.
5.  Make note of the registry location specified in the variable at the top of OSSU's From1.vb (settingslocation).  By default, it is "HKEY_LOCAL_MACHINE\SOFTWARE\Semrau Software Consulting\OSSU".
6.  Create this location in your registry if it doesn't already exist.
7.  Create the following values:
  1.  move_file: True or False (Do you want to enable the file moving feature?)
    * If True: Create move_location with the path that you want OSSU to move the source files to after it is finished.
  2.  upload_file: True or False (Do you want to upload the mp3 and ogg files via FTP?)
    * If True: Create the following values with the FTP settings:
      * ftp_host (The host of your FTP site including "ftp://")
      * ftp_user (The user account to use when uploading)
      * ftp_pw (The password to use when uploading)
    * Note about FTP uploading: The program will upload the mp3 and ogg files to the sub directory "files"; however, if you are using the included sermon display site and the playlist included with that; it will upload the playlist.txt to the root of the ftp host that you specify.
  3.  update_playlist: True or False (Do you want to update the playlist.txt file?  Only choose true if you plan to use the included display site or will be following the playlist format that we created for the display site)
    * If True: Create "url" with the value of your display site including "http(s)://" and a "/" at the end of the url.
  4.  delete_converted_files: True or False (Do you want to delete the files in the source directory when OSSU is done with them?)
  5.  staging_location: The path on your computer that you put the source files in.  Include a "\" at the end.
  6.  avlib_path: The path contain's libav's "avconv.exe" file.  Do not include "avconv.exe" at the end; just the directory that contains it.  *Do* include "\" at the end of the path.
  7.  If desired, create a shortcut on the desktop to SermonUploadTool.exe.

### Security considerations:

* Currently OSSU stores your FTP credentials in plain text in the registry.  Do not install OSSU with FTP uploading enabled on a public computer.
  * We will be releasing a version of OSSU that encrypts the credentials eventually; however, the fact that credentials are stored in any form (even encrypted) is technically a security issue.  While encrypted credentials will help, we will continue to recommend that OSSU not be installed on a public computer if it has FTP uploading enabled.

## Contributing:

We welcome contributions.  Please submit a pull request if you would like to contribute.  (Please follow common contributing guidelines... fork the project to your account, create a branch, and make your changes in that branch.  Test your changes - don't break the build.)

#### Example use case:

Here is how we personally use OSSU at my church:
1.  Recording engineer records the sermon with Audacity and exports the audio as .wav to a staging folder using the naming convention mentioned above.
2.  Recording engineer opens OSSU; which sees only one file, loads it and parses out the date, speaker, and title from the naming convention.
3.  Recording engineer confirms that the data was parsed correctly and makes changes if necessary, then presses "confirm".
4.  OSSU converts the .wav file to .mp3 and .ogg.
5.  OSSU downloads the current playlist.txt file from the target display site.
6.  OSSU updates the playlist.txt file so that it contains the newest files.
7.  OSSU uploads the .mp3, .ogg, then playlist.txt files.
8.  OSSU moves the .wav file to our archival folder on a network share.
9.  OSSU deletes all of the files from the staging folder.

## Disclaimers:

1.  Read the license of OSSU, and Libav.  We did not create Libav.  We do not compile Libav into our program; although we do download and install it for your convience if you use our automated installer.  Because we do not compile Libav into our program, we are seen as "a work that uses the library" and are not covered under the Lesser GPL or GPLv2... we use the MIT license.
2.  Don't be stupid.  If you don't have the proper copyrights to upload parts of your service (such as the music), don't upload them.  Consult whatever service you use for copyrights (such as CCLI) before uploading any parts of your service.  We are not copyright lawyers.  We will not be held liable for any copyright mistakes that you might make while using this tool.