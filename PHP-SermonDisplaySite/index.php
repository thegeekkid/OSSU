<?php
    //Put the url where this site will be located in the variable below.  Be sure to include a "/" at the end.
    $url = '';

    //Nothing below should need to be updated.
    $playlist = file_get_contents($url . 'playlist.txt');

    $html = "";

    $eol = "\r\n";
    $line = strtok($playlist, $eol);

    $strdata = "";
    $mp3name = "";

    while ($line !== false) {
        if (strpos($line, '<New />') !== false) {
            $html = $html . '<div class="audblock">';
        }
        if (strpos($line, 'Date: ') !== false) {
            $strdata = str_replace("Date: ", "", $line);
            $strdata = str_replace((array("\r", "\n")), "", $strdata);
            $html = $html . "\r\n" . ' <p>' . $strdata . ' - ';
        }
        if (strpos($line, 'Speaker: ') !== false) {
            $strdata = str_replace("Speaker: ", "", $line);
            $strdata = str_replace((array("\r", "\n")), "", $strdata);
            $html = $html . $strdata . '</p>';
        }
        if (strpos($line, 'Title: ') !== false) {
            $strdata = str_replace("Title: ", "", $line);
            $strdata = str_replace((array("\r", "\n")), "", $strdata);
            $html = $html . "\r\n" . "  <p>" . $strdata . '</p>';
        }
        if (strpos($line, 'mp3: ') !== false) {
            $mp3name = str_replace("mp3: ", "", $line);
            $mp3name = str_replace((array("\r", "\n")), "", $mp3name);
            $html = $html . "\r\n" . "  <audio controls>" . "\r\n" . '    <source src="' . $mp3name . '">';
        }
        if (strpos($line, 'ogg: ') !== false) {
            $strdata = str_replace("ogg: ", "", $line);
            $strdata = str_replace((array("\r", "\n")), "", $line);
            $html = $html . "\r\n" . '    <source src="' . $strdata . '>' . "\r\n" . '  </audio>' . "\r\n";
            $html = $html . '<p><a href="' . $mp3name . '">Download</a> (Right click - Save As)</p>' . "\r\n" . '</div>';
        }
        $line = strtok( $eol );
    }

?>


<!DOCTYPE html>
    <head>
       <title>Sermons - Your Church Name Here</title>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <link rel="stylesheet" media="screen" href="screen.css" />
    </head>
    <body>
        <div id="content">
            <?php echo $html; ?>
        </div>
    </body>
</html>