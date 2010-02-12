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
        border-left:solid 1px #a659aa;
        border-top:solid 1px #a659aa;
        border-right:solid 1px #a659aa;
        margin-right:5px;
        padding:5px;
        background-color:#cccccc;
    }
    .TabStopSelected, .TabOptions
    {
        background-color:white;
    }
    .TabOptions
    {   
        border-left:solid 1px #a659aa;
        border-bottom:solid 1px #a659aa;
    }
    .Wait
    {
        border:solid 3px black;
        background-color:#cccccc;
        position:absolute;
        top:200px;
        left:200px;
        padding:20px;
        font-size:1.2em;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div class="TabStopContainer">
    <span id="TabArts" class="TabStop"><a href="javascript:ClickOnTab('Arts');">Arts</a></span>
    <span id="TabBegging" class="TabStop"><a href="javascript:ClickOnTab('Begging');">Begging</a></span>
    <span id="TabCrimes" class="TabStop"><a href="javascript:ClickOnTab('Crimes');">Crimes</a></span>
    <span id="lblSayuser"></span>
    <input type="hidden" id="hdnTabSelected" value="NotSet" runat="server" />
    
    </div>
    <div id="OptionsCrimes" class="TabOptions" >
    <asp:Button ID="Button2" runat="server" Text="Display" OnClick="RefreshClick" CssClass="DisplayButton" />
    <asp:DropDownList ID="ddlCrime" runat="server" />
    <asp:DropDownList ID="ddlYear" runat="server" />
    <a href="javascript:ShowColumn(2);">Number of Offences</a>
    <a href="javascript:ShowColumn(3);">per 1,000 Population</a>
    </div>
    <div id="OptionsArts" class="TabOptions" >
    <asp:Button ID="Button1" runat="server" Text="Display" OnClick="RefreshClick" CssClass="DisplayButton" />
    <asp:DropDownList ID="ddlArtType" runat="server" />
    </div>
    <div id="OptionsBegging" class="TabOptions" >            
    <asp:Button ID="Button3" runat="server" Text="Display" OnClick="RefreshClick" CssClass="DisplayButton" />
    <asp:DropDownList ID="ddlBeggingYear" runat="server" />
    </div>
    
    <div style="height:5px;"></div>
    <div id="notepad"></div>
    
    <div id="WaitLoading" class="Wait">Loading the new data set</div>
    </div>
    </form>
</body>
</html>
