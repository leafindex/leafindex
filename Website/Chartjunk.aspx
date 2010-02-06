<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chartjunk.aspx.cs" Inherits="Website.Chartjunk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Chartjunk</title>
    <link href="Shared.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.1/jquery.min.js"></script>
    <script type="text/javascript" src="./js/Shared.js"></script>   
    <script type='text/javascript' src='http://www.google.com/jsapi'></script>
    <%= MyScript %>     
    <script type="text/javascript" src="./js/Chartjunk.js"></script>

    <link href="Shared.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .floatleftwithmargin { float:left; margin-right:10px; }
    .filepicker { background-color:#ccccff; margin-bottom:3px; padding:3px; font-size:0.8em; }
    .FileSelected { background-color:#9999ff; }
    .subhead { border:solid 1px #9999ff; margin-bottom:3px; padding:3px; font-size:0.8em; } 
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h2>Chartjunk</h2>
    <div>
    <div id="filelist" class="floatleftwithmargin"></div>
    <div id="chart_div" class="floatleftwithmargin"></div>
    <div class="floatleftwithmargin" style="width:300px" >
    The gauge shows the percentage of styles that are defined in the header rather than inline.
    We aim for high numbers - 100% is good.
    <p />
    This is an example of <span class="Stress">Chartjunk</span>, where most of the graphic is
    given over to design, little to data. In this case a huge picture to represent one number.
    <p />
    <span id="lblSayuser"></span>
    </div>
    </div>
    <div class="ClearBoth"></div>
    </div>
    </form>
</body>
</html>
