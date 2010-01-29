<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ACOMap.aspx.cs" Inherits="Website.ACOMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Map of Ambulance Incidents</title>
  <link href="Shared.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.0/jquery.min.js"></script>
  <script type="text/javascript" src="Shared.js"></script>
  <script type="text/javascript" src="ACOMap.js"></script>
  <style type="text/css">
  .MonthLink { width:100px; background-color:#ccccff; margin-bottom:3px; padding:3px; }
  .MonthSelected { background-color:#9999ff; }
  </style>
  </head>
  <body>
    <form id="form1" runat="server">
    <div>
<div>
<%=MyScript %>
<a href="#" id="lnkReset">Reset</a> <a href="#" id="lnkData">Data</a> 


<span id="lblSayuser"></span><br />
<canvas id="canvas" width="800" height="640" style="border:solid 1px black;float:left"></canvas> 
<div style="margin-left:10px;float:left;">
<div id="month0" class="MonthLink">Dec 2007</div>
<div  id="month1" class="MonthLink">Jan 2008</div>
<div  id="month2" class="MonthLink">Feb</div>
<div  id="month3" class="MonthLink">Mar</div>
<div  id="month4" class="MonthLink">Apr</div>
<div  id="month5" class="MonthLink">May</div>
<div  id="month6" class="MonthLink">Jun</div>
<div  id="month7" class="MonthLink">Jul</div>
<div  id="month8" class="MonthLink">Aug</div>
<div  id="month9" class="MonthLink">Sep</div>
<div  id="month10" class="MonthLink">Oct</div>
<div  id="month11" class="MonthLink">Nov</div>
</div>

</div>
    
<div>
<img src="images/boroughmap.gif" id="imgMap" width="0" height="0" />
</div>    

    </div>
    </form>
</body>
</html>
