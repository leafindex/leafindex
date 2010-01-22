<%@ Page Language="C#" CodeBehind="ACOAssaults1.aspx.cs" Inherits="Website.ACOAssaults"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en"> 
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1"/>
	<title>LeafIndex: ACOAssaults</title>
  <link href="Shared.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.0/jquery.min.js"></script>
  <script type="text/javascript" src="Shared.js"></script>
  <script type="text/javascript" src="ACOAssaults1.js"></script>
</head>
<body>
<div id="navmenu"></div>
	<form action="">
		<div>
		<label for="boroughSearch">Borough: </label><input type="text" id="boroughSearch"/>
		</div>
	</form>
	<table id="boroughTable">
	<thead><tr><th>Borough</th><th class="number">Total Number of Assaults</th><th class="number">Number of Wards in Borough</th></tr></thead>
	<tbody>
	<%=BoroughList%>
	</tbody>
	</table>
</body>
</html>
