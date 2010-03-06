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
    .TabRemainder
    {
        border-top:solid 1px white;
        border-left:solid 1px #a659aa;  
        padding:6px 10px 6px 10px;
        float:left;
    }
    .TabStopContainer
    {
        list-style:none;
        margin:0;
        padding:0 0 5px 0;
    }
    #TabContent
    {
        border:solid 1px #a659aa;
        margin-top:-1px;
    }
    .TabStop { 
        background-color:#eeeeee;
        border-left:solid 1px #a659aa;
        border-top:solid 1px #a659aa;
        border-bottom:solid 1px #a659aa;
        padding:6px 10px 6px 10px;
        float:left;
        margin:0px;
        display:list-item;
        list-style:none;
    }
    .TabStop A
    {
        margin:1px;
    }
    .TabStopSelected
    {
        background-color:white;
        border-bottom:solid 1px white;
        border-top:solid 4px  #a659aa;
        border-left:solid 1px #a659aa;
        padding-top:3px;
    }
    .TabOptions
    {   
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
    <ul class="TabStopContainer">
        <li id="TabArts" class="TabStop"><a href="#">Arts</a></li>
        <li id="TabBegging" class="TabStop"><a href="#">Begging</a></li>
        <li id="TabCars" class="TabStop"><a href="#">Cars</a></li>
        <li id="TabCommuters" class="TabStop"><a href="#">Commuters</a></li>
        <li id="TabCrimes" class="TabStop"><a href="#">Crimes</a></li>
        <li id="TabSuicide" class="TabStop"><a href="#">Suicide</a></li>
        <li id="TabTourism" class="TabStop"><a href="#">Tourism</a></li>
        <li id="TabWaste" class="TabStop"><a href="#">Waste</a></li>
        <li class="TabRemainder"><span id="lblSayuser"></span>&nbsp;</li>
    </ul>       
    <div class="ClearBoth"></div>
    <div id="TabContent">
    
    <input type="hidden" id="hdnTabSelected" value="NotSet" runat="server" />
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
    
    <div id="OptionsCommuters" class="TabOptions">
    Going to
        <select id="ddlGoingTo">
            <option value="2">City of London</option>	
            <option value="3">Barking and Dagenham</option>	
            <option value="4">Barnet</option>
            <option value="5">Bexley</option>
            <option value="6">Brent</option>
            <option value="7">Bromley</option>
            <option value="8">Camden</option>
            <option value="9">Croydon</option>
            <option value="10">Ealing</option>
            <option value="11">Enfield</option>
            <option value="12">Greenwich</option>
            <option value="13">Hackney</option>
            <option value="14">Hammersmith and Fulham</option>
            <option value="15">Haringey</option>
            <option value="16">Harrow</option>
            <option value="17">Havering</option>
            <option value="18">Hillingdon</option>
            <option value="19">Hounslow</option>
            <option value="20">Islington</option>
            <option value="21">Kensington and Chelsea</option>
            <option value="22">Kingston upon Thames</option>	
            <option value="23">Lambeth</option>
            <option value="24">Lewisham</option>
            <option value="25">Merton</option>
            <option value="26">Newham</option>
            <option value="27">Redbridge</option>
            <option value="28">Richmond upon Thames</option>	
            <option value="29">Southwark</option>	
            <option value="30">Sutton</option>	
            <option value="31">Tower Hamlets</option>	
            <option value="32">Waltham Forest</option>	
            <option value="33">Wandsworth</option>	
            <option value="34">Westminster</option>	
            <option value="35">East</option>	
            <option value="36">East Midlands</option>	
            <option value="37">North East</option>	
            <option value="38">North West</option>	
            <option value="39">South East</option>	
            <option value="40">South West</option>	
            <option value="41">Wales</option>	
            <option value="42">West Midlands</option>	
            <option value="43">Yorkshire and the Humber</option>	
            <option value="44">Northern Ireland</option>	
            <option value="45">Scotland</option>	
            <option value="46">Offshore</option>	
            <option value="47">Outside UK</option>	
            <option value="48">Residents commuting (excludes home-workers)</option>	
            <option value="49">Within Borough (includes home-workers)</option>	
            <option value="50">Within Borough (excludes home-workers)</option>	
            <option value="51" selected="selected">Into area (A)</option>	
            <option value="52">Out of area (B)</option>
            <option value="52">In-Out (A-B)</option>
        </select>
        <a href="javascript:DrawFlows(27);">Flows</a>
        <br />
    <span class="Explanation">Commuter movements of Londoners by borough, from 2001 Census.
    <a target="_blank" href="http://data.london.gov.uk/datastore/package/borough-commuting-patterns">More...</a></span>           
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
    </div>
    </div>
                
    <div style="height:5px;"></div>
    <div id="notepad"></div>
    <div id="FloatingInfoBox">FloatingInfoBox</div>
    
    </form>
</body>
</html>
