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
    <script type="text/javascript" src="./js/Isotype.js"></script>
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
    .TabStop A
    {
        margin:1px;
    }
    .TabStopSelected, .TabOptions
    {
        background-color:white;
    }
    .TabOptions
    {   
        border-bottom:solid 1px #a659aa;
        border-left:solid 1px #a659aa;
        padding:2px 5px 2px 5px;
        display: none;
    }
    .OptionDdl
    {
    }
    .Explanation
    {
    
    }
    #FloatingInfoBox
    {
        position:absolute;
        padding:2px;
        border:solid 1px #a659aa;
        color:#a659aa;
        font-size:0.8em;
        top:0px;
        left:0px;
        background-color:#ffefd1;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="Tabbing">
    <div class="TabStopContainer">
    <span id="TabArts" class="TabStop"><a href="#">Arts</a></span>
    <span id="TabBegging" class="TabStop"><a href="#">Begging</a></span>
    <span id="TabCars" class="TabStop"><a href="#">Cars</a></span>
    <span id="TabCrimes" class="TabStop"><a href="#">Crimes</a></span>
    <span id="TabSuicide" class="TabStop"><a href="#">Suicide</a></span>
    <span id="TabTourism" class="TabStop"><a href="#">Tourism</a></span>
    <span id="TabWaste" class="TabStop"><a href="#">Waste</a></span>
    <span id="lblSayuser"></span>
    <input type="hidden" id="hdnTabSelected" value="NotSet" runat="server" />
    
    </div>
    <div id="OptionsCrimes" class="TabOptions" >
    <select id="ddlCrime" class="OptionDdl"> 
	    <option selected="selected">Against Vehicles</option> 
	    <option>Burglary</option> 
	    <option>Criminal Damage</option> 
	    <option>Drug</option> 
	    <option>Fraud And Forgery</option> 
	    <option>Other</option> 
	    <option>Other Theft Offences</option> 
	    <option>Robbery</option> 
	    <option>Sexual</option> 
	    <option>Violence Against The Person</option> 
        <option>Total</option> 
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
    <a href="javascript:DrawDistribution();">Distribution</a>
    <input type="checkbox" id="chkIncludeCity" checked="checked" /> include City of London
    <br />
    <span class="Explanation">Counts and rates of crime at local authority level for England and Wales, 2002 to 2009.
    <a target="_blank" href="http://www.homeoffice.gov.uk/rds/soti.html">More...</a></span>
    </div>
    <div id="OptionsArts" class="TabOptions" >
    <select id="ddlArtType" class="OptionDdl"> 
	    <option selected="selected">Arts Event or Activity</option> 
	    <option>Museum or Gallery</option> 
	    <option>Public Library</option> 
    </select>    
    <a href="javascript:createData();">Map</a> 
    <a href="javascript:ShowSideBySide('arts');">Side by Side</a> 
    <br /> 
    <span class="Explanation">The percentage of adults who have used a library, visited a museum/gallery, or attended/participated in an arts activity in the past 12 months.
    <a target="_blank" href="http://data.london.gov.uk/datastore/package/use-public-libraries-visits-museums-and-galleries-and-engagement-arts">More...</a></span>
    </div>
    
    <div id="OptionsBegging" class="TabOptions" >
    <select id="ddlBeggingYear" class="OptionDdl">
        <option>2008</option>
        <option selected="selected">2009</option>
    </select>  
    <br /> 
    <span class="Explanation">Count of begging incidents recorded by British Transport Police.
    <a target="_blank" href="http://data.london.gov.uk/datastore/package/begging-incidents-recorded-british-transport-police">More...</a></span>           
    </div>
    
    <div id="OptionsSuicide" class="TabOptions" >
    <select id="Select1" class="OptionDdl">
        <option selected="selected">Number of Deaths</option>
        <option>Directly Standardised Rate of years lost</option>
    </select>  
    <select id="Select4" class="OptionDdl">
        <option>2004-2006</option>
        <option selected="selected">2005-2007</option>
    </select>  
    <br /> 
    <span class="Explanation">Directly Standardised Rate of years lost due to suicide, based on those under age 75 and on the original underlying cause of death.
    <a target="_blank" href="http://data.london.gov.uk/datastore/package/years-life-lost-due-suicide">More...</a></span>           
    </div>

    <div id="OptionsCars" class="TabOptions" >
    <a href="javascript:ShowColumn(2);">No cars households</a>
    <a href="javascript:ShowColumn(3);">One car</a>
    <a href="javascript:ShowColumn(4);">Two cars</a>
    <a href="javascript:ShowColumn(5);">Three cars</a>
    <a href="javascript:ShowColumn(6);">Four or more cars</a>
    <a href="javascript:DrawIsotype();">Isotype</a>
    
    &nbsp;<br /> 
    <span class="Explanation">Census 2001 Key Statistics 17: Cars and Vans.
    <a target="_blank" href="http://data.london.gov.uk/datastore/package/census-2001-key-statistics-17-cars-and-vans">More...</a></span>           
    </div>
    
    <div id="OptionsTourism" class="TabOptions" >
    <select id="Select2" class="OptionDdl">
        <option>Day</option>
        <option>Domestic staying</option>
        <option>Overseas</option>
    </select>  
    <select id="Select5" class="OptionDdl">
        <option>2004</option>
        <option>2005</option>
        <option>2006</option>
        <option selected="selected">2007</option>
    </select>  
    <br /> 
    <span class="Explanation">Borough-level estimates of tourism volume.
    <a target="_blank" href="http://data.london.gov.uk/datastore/package/tourism-trips">More...</a></span>   
    </div>
    
    <div id="OptionsWaste" class="TabOptions" >
    <select id="Select3" class="OptionDdl">
        <option>Number of charity shops</option>
        <option>Number of reuse organisations</option>
        <option selected="selected">Re-use activity weight (tonnes)</option>
    </select>  
    <select id="Select6" class="OptionDdl">
        <option>2007</option>
    </select>  
    <br /> 
    <span class="Explanation">Number of Re-Use Centres and their activity by weight - tonnes of furniture, appliances and IT equipment reused.
    <a target="_blank" href="http://data.london.gov.uk/datastore/package/waste-re-use-centres">More...</a></span>   
    </div>
                
    <div style="height:5px;"></div>
    <div id="notepad"></div>
    <div id="FloatingInfoBox">FloatingInfoBox</div>
    </div>
    
    </form>
</body>
</html>
