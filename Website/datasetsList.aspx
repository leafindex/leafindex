<%@ Page Language="C#" CodeBehind="datasetsList.aspx.cs" Inherits="Website.datasetsList"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en"> 
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1"/>
	<title>LeafIndex: Ambulance Call Outs to Assaults - Summary</title>
  <link href="Shared.css" rel="stylesheet" type="text/css" />
  <style type="text/css">
		#datasetsTable tbody {
		  font-size: 11px
		}
	</style>
  <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.1/jquery.min.js"></script>
  <script type="text/javascript" src="./js/Shared.js"></script>
  <script type="text/javascript">
		$(function(){
			$("#datasetsTable").superTable()
		});
  </script>
</head>
<body>
<div>
</div>
	<h2>Datasets populated in our data warehouse</h2>
	<%=Datasets%>
</body>
</html>
