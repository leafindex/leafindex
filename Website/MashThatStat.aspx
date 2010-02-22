<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MashThatStat.aspx.cs" Inherits="Website.MashThatStat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Mash That Stat</title>
    <link href="Shared.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.1/jquery.min.js"></script>
    <script type="text/javascript" src="./js/Shared.js"></script>   
    <script type="text/javascript" src="./js/raphael-min.js"></script>
    <script type="text/javascript" src="./js/MashThatStat.js"></script>
    <style type="text/css">
    #divButtons { 
        margin-bottom:5px;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="divButtons">
    <a href="#" id="lnkNext">Next</a>
    </div>
    <div id="notepad"></div>
    </div>
    </form>
</body>
</html>
