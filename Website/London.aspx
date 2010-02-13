<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="London.aspx.cs" Inherits="Website.London" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>London</title>
    <link href="Shared.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.1/jquery.min.js"></script>
    <script type="text/javascript" src="./js/Shared.js"></script>   
    <script type="text/javascript" src="./js/raphael-min.js"></script>
    <%=MyScript %>
    <script type="text/javascript" src="./js/London.js"></script>
    <style type="text/css">
    .TabStopContainer
    {
        padding-bottom:5px;
    }
    .TabStop { 
        background-color:#cccccc;
        border-left:solid 1px #a659aa;
        border-right:solid 1px #a659aa;
        border-top:solid 1px #a659aa;
        margin-right:5px;
        padding:5px;
    }
    .TabStopSelected, .TabOptions
    {
        background-color:white;
    }
    .TabOptions
    {   
        border-bottom:solid 1px #a659aa;
        border-left:solid 1px #a659aa;
        display: none;
    }
    .OptionDdl
    {
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="Tabbing">
    <div class="TabStopContainer">
    <span id="TabArts" class="TabStop"><a href="#">Arts</a></span>
    <span id="TabBegging" class="TabStop"><a href="#">Begging</a></span>
    <span id="TabCrimes" class="TabStop"><a href="#">Crimes</a></span>
    <span id="lblSayuser"></span>
    <input type="hidden" id="hdnTabSelected" value="NotSet" runat="server" />
    
    </div>
    <div id="OptionsCrimes" class="TabOptions" >
    <select id="ddlCrime" class="OptionDdl"> 
        <option selected="selected">Total</option> 
	    <option>Against Vehicles</option> 
	    <option>Burglary</option> 
	    <option>Criminal Damage</option> 
	    <option>Drug</option> 
	    <option>Fraud And Forgery</option> 
	    <option>Other</option> 
	    <option>Other Theft Offences</option> 
	    <option>Robbery</option> 
	    <option>Sexual</option> 
	    <option>Violence Against The Person</option> 
    </select> 
    <select id="ddlYear" class="OptionDdl"> 
	    <option>2002/03</option> 
	    <option>2003/04</option> 
	    <option>2004/05</option> 
	    <option>2005/06</option> 
	    <option>2006/07</option> 
	    <option>2007/08</option> 
	    <option selected="selected">2008/09</option> 
    </select>

    <a href="javascript:ShowColumn(2);">Number of Offences</a>
    <a href="javascript:ShowColumn(3);">per 1,000 Population</a>
    </div>
    <div id="OptionsArts" class="TabOptions" >
    <select id="ddlArtType" class="OptionDdl"> 
	    <option>Arts Event or Activity</option> 
	    <option selected="selected">Museum or Gallery</option> 
	    <option>Public Library</option> 
    </select>    
    </div>
    <div id="OptionsBegging" class="TabOptions" >
    <select id="ddlBeggingYear" class="OptionDdl">
        <option>2008</option>
        <option selected="selected">2009</option>
    </select>            
    </div>
    
    <div style="height:5px;"></div>
    <div id="notepad"></div>
    </div>
    </form>
</body>
</html>
