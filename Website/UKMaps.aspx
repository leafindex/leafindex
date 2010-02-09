<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UKMaps.aspx.cs" Inherits="Website.UKMaps" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>UK Maps</title>
    <link href="Shared.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.1/jquery.min.js"></script>
    <script type="text/javascript" src="./js/Shared.js"></script>   
    <script type="text/javascript" src="./js/raphael-min.js"></script>
    
    <%= MyScript %>
<script type="text/javascript">
    var _elt_array;

    $(document).ready(function() {
        var width, height, i, j, kmap, colour;
        width = 1000;
        height = 500;

        _elt_array = new Array();

        LoadKmlMaps();
        var paper = Raphael("notepad", width + 1, height + 1);
        paper.rect(0, 0, width, height);
        for (i = 0; i < _kmlmap.length; i++) {
            colour = Raphael.getColor();
            kmap = _kmlmap[i];
            for (j = 1; j < kmap.length; j++) {
                elt = paper.path(kmap[j]);
                elt.attr("fill", colour);  //GetColour(i));
                _elt_array[_elt_array.length] = new Array(elt, kmap[0]);
                elt.mouseover(function(event) {
                    DoMouseOver(this);
                });
            }
        }
    });

    function DoMouseOver(elt) {
        var i, pair;
        for (i = 0; i < _elt_array.length; i++) {
            pair = _elt_array[i];
            if (pair[0] == elt) {
                Sayuser(pair[1]);
                return;
            }
        }
        Sayuser("Looked through " + _elt_array.length + " elements");
    }
    
    function Sayuser( str )
    {
        $("#lblSayuser").text( str );
    }

    var colours = new Array(
    "#ff0000", "#bb0000", "#770000", //"#330000",
    "#00ff00", "#00bb00", "#007700", //"#003300",
    "#0000ff", "#0000bb", "#000077", //"#000033",

    "#ffff00", "#bbbb00", "#777700",
    "#ff00ff", "#bb00bb", "#770077",
    "#00ffff", "#00bbbb", "#007777",

    "#ffff44", "#bbbb44", "#777744",
    "#ff44ff", "#bb44bb", "#774477",
    "#44ffff", "#44bbbb", "#447777",

    "#ffff88", "#bbbb88", "#777788",
    "#ff88ff", "#bb88bb", "#778877",
    "#88ffff", "#88bbbb", "#887777",

    "#ffbb77", "#bbff77", "#bb77ff", "#ff77bb", "#77ffbb", "#77bbff"
);

    function GetColour(i) {
        var idx = i % colours.length;
        return colours[idx];
    }    

</script>    
</head>  
<body>
    <form id="form1" runat="server">
    <div>
    <span class="Stress">UK Maps</span>
    <a href="UKMaps.aspx?name=all">All</a>
    <a href="UKMaps.aspx?name=England">England</a>
    <a href="UKMaps.aspx?name=Wales">Wales</a>
    <a href="UKMaps.aspx?name=Scotland">Scotland</a>
    <a href="UKMaps.aspx?name=Northern+Ireland">Northern Ireland</a>
    <a href="UKMaps.aspx?name=Bailiwick+of+Guernsey">Bailiwick of Guernsey</a>
    <a href="UKMaps.aspx?name=Isle+of+Man">Isle of Man</a>
    <a href="UKMaps.aspx?name=England&name2=Scotland&name3=Wales">Great Britain</a>
    <br />
    <span class="Stress">Police</span>
    <a href="UKMaps.aspx?name=all&source=police">All</a>
    <a href="UKMaps.aspx?name=Hertfordshire+Constabulary&source=police">Hertfordshire Constabulary</a>
    <a href="UKMaps.aspx?name=Kent+Police&source=police">Kent Police</a>
    <a href="UKMaps.aspx?name=Devon+and+Cornwall+Constabulary&source=police">Devon and Cornwall Constabulary</a>
    <br />
    <div style="width:1000px;">
    <div style="float:left;">
    <span class="Stress">Others</span>
    <a href="UKMaps.aspx?name=all&source=kml156737">English Regions</a>
    <a href="UKMaps.aspx?name=all&source=kml156738">Counties</a>
    <a href="UKMaps.aspx?source=kml156738&name=Barnet&name2=Bexley&name3=Brent&name4=Bromley&name5=City+of+Westminster&name6=Camden&name7=Croydon&name8=Ealing&name9=Enfield&name10=Greenwich&name11=Hackney&name12=Hammersmith+and+Fulham&name13=Haringey&name14=Harrow&name15=Havering&name16=Hillingdon&name17=Hounslow&name18=Islington&name19=Kensington+and+Chelsea&name20=Kingston+upon+Thames&name21=Lambeth&name22=Lewisham&name23=Merton&name24=Newham&name25=Redbridge&name26=Richmond+upon+Thames&name27=Southwark&name28=Sutton&name29=Tower+Hamlets&name30=Waltham+Forest&name31=Wandsworth&name32=City+of+London">London</a>  
    </div>
    <div style="float:right;">
    <span  class="Stress" id="lblSayuser"></span>
    </div>
    </div>
    

<div id="notepad"></div>

    </div>
    </form>
</body>
</html>
