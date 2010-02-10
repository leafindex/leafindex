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
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <span class="Stress">Crimes</span>
    <asp:DropDownList ID="ddlCrime" runat="server" OnSelectedIndexChanged="RefreshClick" AutoPostBack="true" />
    <asp:DropDownList ID="ddlYear" runat="server" OnSelectedIndexChanged="RefreshClick" AutoPostBack="true" />
    <a href="javascript:ShowColumn(2);">Number of Offences</a>
    <a href="javascript:ShowColumn(3);">per 1,000 Population</a>
    <span class="Stress">Arts</span>
    <asp:DropDownList ID="ddlArtType" runat="server" OnSelectedIndexChanged="RefreshClick" AutoPostBack="true" />
    <span class="Stress">Begging</span>
    <asp:DropDownList ID="ddlBeggingYear" runat="server" OnSelectedIndexChanged="RefreshClick" AutoPostBack="true" />
    
    <span id="lblSayuser"></span>
    <div style="height:5px;"></div>
    <div id="notepad"></div>
    </div>
    </form>
</body>
</html>
