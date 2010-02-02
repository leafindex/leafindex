<%@ Page Language="C#" CodeBehind="ACOAssaults1.aspx.cs" Inherits="Website.ACOAssaults"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en"> 
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1"/>
	<title>LeafIndex: Ambulance Call Outs to Assaults - Summary</title>
  <link href="Shared.css" rel="stylesheet" type="text/css" />
  <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.0/jquery.min.js"></script>
  <script type="text/javascript" src="/js/Shared.js"></script>
  <script type="text/javascript" src="/js/ACOAssaults1.js"></script>
</head>
<body>
	<h2>Ambulance Call Outs to Assault Incidents</h2>
	<ul class="linkList"><li>Summary</li><li><a href='ACOMapR.aspx'>Map</a></li><li><a href='ACOAssaults.htm'>Query</a></li></ul><br class="clear"/>
	<form action="">
		<div>
		<label for="boroughSearch">Borough: </label><input type="text" id="boroughSearch"/>
		</div>
	</form>
	<%=Boroughs%>
</body>
</html>
