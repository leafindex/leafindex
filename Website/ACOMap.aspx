<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ACOMap.aspx.cs" Inherits="Website.ACOMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Map of Ambulance Incidents</title>
  <link href="Shared.css" rel="stylesheet" type="text/css" />
  <!--[if IE]><script src="/js/excanvas.js"></script><![endif]-->
  <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.1/jquery.min.js"></script>
  <script type="text/javascript" src="./js/Shared.js"></script>
  <script type="text/javascript" src="./js/ACOMap.js"></script>
  <style type="text/css">
  .MonthLink { width:100px; background-color:#ccccff; margin-bottom:3px; padding:3px; font-size:0.8em; }
  .MonthSelected { background-color:#9999ff; }
  .YearDivider { height:5px; }
  .BoroughDetail { border:solid 1px #9999ff; padding:3px; background-color:White; text-align:center;
    font-size:0.8em; position:absolute; }
  .BoroughDetail A { text-decoration:none; }    
  .BoroughDetail A:Hover { text-decoration:underline; }    
  .BoroughGraph { border:solid 1px #9999ff; padding:3px; background-color:White; 
    font-size:0.8em; position:absolute;
    width:200px; }
  </style>
  </head>
  <body>
    <form id="form1" runat="server">
    <div>
<div>
<%=MyScript %>

<div style="width:1000px;float:left;">
<canvas id="canvas" width="800" height="640" style="border:solid 1px black;float:left;"></canvas> 

<div style="margin-left:10px;float:left;">
<div id="month0" class="MonthLink">Dec 2007</div>
<div class="YearDivider"></div>
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
<div  id="month12" class="MonthLink">Dec</div>
<div class="YearDivider"></div>
<div  id="month13" class="MonthLink">Jan 2009</div>
<div  id="month14" class="MonthLink">Feb</div>
<div  id="month15" class="MonthLink">Mar</div>
<div  id="month16" class="MonthLink">Apr</div>
<div  id="month17" class="MonthLink">May</div>
<div  id="month18" class="MonthLink">Jun</div>
<div  id="month19" class="MonthLink">Jul</div>
<div  id="month20" class="MonthLink">Aug</div>
<div  id="month21" class="MonthLink">Sep</div>
<div  id="month22" class="MonthLink">Oct</div>
<div  id="month23" class="MonthLink">Nov</div>
<div class="YearDivider"></div>

</div>
</div>
<div style="clear:both;"></div>
<span id="lblSayuser"></span><br />

</div>
    
<div>
<img src="images/boroughmap.gif" id="imgMap" width="0" height="0" />
</div>    

    </div>
    </form>
</body>
</html>
