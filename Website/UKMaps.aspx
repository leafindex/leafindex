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
    var _borough_data;
    $(document).ready(function() {
        var width, height, i;
        width = 1000;
        height = 500;
        //        var paper = Raphael("notepad", width, height);
        //        LoadData();
        //        for (i = 0; i < _borough_data.length; i++)
        //            PlotPolyLine(paper, _borough_data[i], i);

        LoadKmlMap();
        var paper = Raphael("notepad", width + 1, height + 1);
        paper.rect(0, 0, width, height);
        for (i = 0; i < _kmlmap.length; i++) {
            elt = paper.path(_kmlmap[i]);
            elt.attr("fill", GetColour(i));
        }

    });
    
    function PlotPolyLine( paper, coords, i )
    {
        elt = paper.path(MakePathFromCoords(coords));
        elt.attr("stroke", GetColour(i));
        elt.attr("fill", GetColour(i) );
    }

    function LoadData() {
        _borough_data = new Array();
        AddBorough(new Array("Bromley", 257, 271, 263, 264, 272, 268, 281, 268, 287, 264, 303, 275, 304, 274, 312, 264, 325, 264, 326, 260, 318, 252, 321, 248, 335, 263, 342, 264, 348, 258, 363, 274, 381, 277, 385, 276, 393, 282, 393, 287, 396, 295, 391, 302, 394, 313, 383, 335, 377, 334, 371, 341, 375, 349, 373, 351, 364, 354, 354, 366, 354, 379, 354, 384, 340, 388, 325, 374, 315, 385, 315, 385, 310, 363, 305, 348, 303, 331, 290, 318, 287, 302, 280, 297, 275, 297, 267, 287));
        AddBorough(new Array("Croydon", 253, 269, 235, 269, 226, 280, 230, 288, 230, 293, 225, 298, 234, 326, 234, 341, 233, 344, 225, 339, 221, 345, 221, 363, 213, 363, 215, 377, 226, 381, 233, 391, 238, 391, 252, 374, 259, 362, 271, 360, 274, 356, 274, 348, 281, 343, 290, 343, 296, 348, 302, 347, 302, 334, 285, 317, 285, 306, 277, 300, 258, 281));
        AddBorough(new Array("Sutton", 165, 304, 160, 309, 160, 316, 169, 320, 177, 335, 175, 345, 175, 351, 189, 340, 209, 356, 213, 362, 219, 361, 218, 343, 225, 337, 232, 340, 232, 329, 223, 298, 214, 304, 200, 296, 195, 302, 182, 302, 179, 299, 173, 308));
        AddBorough(new Array("Merton", 188, 253, 165, 254, 156, 259, 156, 270, 160, 280, 160, 293, 164, 298, 174, 306, 178, 300, 195, 300, 200, 294, 209, 299, 216, 301, 229, 293, 225, 284, 224, 279, 216, 277, 215, 272, 201, 270, 192, 264));

        AddBorough(new Array("Kingston", 115, 357, 122, 351, 126, 340, 134, 333, 134, 327, 152, 306, 162, 305, 163, 300, 156, 292, 153, 259, 151, 257, 145, 263, 131, 265, 126, 260, 122, 261, 126, 267, 123, 291, 120, 298, 126, 306, 126, 314, 115, 332));
        AddBorough(new Array("Wandsworth", 224, 210, 208, 211, 200, 215, 193, 229, 179, 231, 169, 223, 166, 223, 164, 231, 153, 231, 149, 234, 154, 240, 155, 255, 165, 250, 187, 251, 190, 249, 194, 262, 210, 269, 217, 269, 221, 265, 221, 259, 213, 249, 217, 242, 212, 236, 211, 226));
        AddBorough(new Array("Wandsworth", 224, 210, 208, 211, 200, 215, 193, 229, 179, 231, 169, 223, 166, 223, 164, 231, 153, 231, 149, 234, 154, 240, 155, 255, 165, 250, 187, 251, 190, 249, 194, 262, 210, 269, 217, 269, 221, 265, 221, 259, 213, 249, 217, 242, 212, 236, 211, 226));
        AddBorough(new Array("Richmond", 119, 260, 126, 258, 132, 263, 143, 263, 153, 255, 150, 241, 146, 235, 153, 228, 161, 228, 164, 220, 169, 221, 161, 206, 150, 225, 141, 221, 132, 209, 123, 220, 116, 228, 126, 236, 126, 241, 115, 234, 87, 235, 77, 245, 85, 250, 85, 256, 76, 264, 77, 271, 81, 280, 93, 275, 114, 294, 118, 289, 119, 266));
        AddBorough(new Array("Hounslow", 46, 238, 50, 219, 59, 211, 60, 198, 69, 203, 77, 203, 84, 206, 92, 198, 106, 201, 123, 202, 132, 193, 144, 198, 149, 198, 153, 196, 154, 201, 149, 218, 136, 203, 128, 204, 116, 219, 110, 224, 115, 232, 86, 233, 74, 245, 75, 249, 84, 249, 74, 265, 74, 271, 63, 266, 54, 258, 51, 262, 36, 250, 36, 241));
        AddBorough(new Array("Ealing", 153, 165, 146, 163, 138, 168, 130, 166, 129, 161, 118, 165, 116, 153, 103, 141, 100, 145, 93, 139, 82, 147, 74, 147, 62, 160, 54, 160, 63, 166, 73, 167, 80, 172, 71, 182, 71, 187, 62, 195, 71, 201, 78, 200, 83, 203, 92, 195, 107, 199, 122, 199, 134, 190, 143, 196, 150, 195, 151, 186, 149, 178, 155, 171));
        AddBorough(new Array("Hammersmith &amp; Fulham", 194, 214, 186, 224, 181, 223, 165, 200, 157, 202, 155, 196, 153, 193, 155, 183, 152, 178, 156, 171, 156, 164, 164, 165, 164, 174, 169, 182, 177, 190, 187, 208, 190, 215));
        AddBorough(new Array("Kensington &amp; Chelsea", 210, 202, 205, 190, 198, 196, 182, 174, 173, 167, 166, 166, 166, 171, 170, 180, 175, 185, 190, 207, 192, 213));
        AddBorough(new Array("Westminster", 233, 180, 226, 185, 224, 203, 214, 203, 208, 191, 204, 189, 199, 194, 184, 173, 175, 166, 180, 162, 185, 164, 198, 156, 206, 161, 209, 161, 221, 181));
        AddBorough(new Array("Lambeth", 255, 267, 236, 268, 224, 279, 218, 275, 217, 270, 222, 266, 222, 259, 215, 247, 218, 241, 214, 230, 226, 213, 226, 209, 231, 204, 231, 193, 234, 188, 234, 193, 238, 199, 235, 206, 245, 220, 245, 228, 243, 232, 242, 244, 245, 248, 245, 256));
        AddBorough(new Array("Southwark", 279, 195, 277, 186, 268, 193, 258, 193, 251, 188, 237, 188, 237, 193, 240, 199, 238, 207, 248, 221, 247, 230, 244, 232, 244, 243, 247, 248, 248, 255, 257, 265, 259, 255, 264, 252, 264, 244, 269, 238, 277, 234, 268, 213, 270, 202));
        AddBorough(new Array("Lewisham", 280, 198, 282, 205, 282, 207, 289, 212, 287, 216, 288, 221, 292, 222, 296, 218, 306, 218, 303, 225, 305, 230, 312, 236, 312, 242, 318, 247, 316, 252, 325, 260, 322, 263, 312, 260, 303, 268, 303, 273, 299, 272, 286, 261, 280, 265, 271, 265, 264, 262, 259, 262, 265, 253, 266, 246, 271, 241, 279, 237, 276, 227, 270, 215, 271, 205));
        AddBorough(new Array("Greenwich", 302, 189, 312, 200, 312, 200, 344, 199, 356, 185, 369, 184, 369, 210, 354, 216, 347, 224, 351, 233, 351, 240, 346, 251, 346, 256, 338, 262, 322, 245, 316, 245, 312, 235, 306, 228, 309, 216, 297, 215, 291, 219, 288, 215, 292, 211, 285, 206, 297, 209, 305, 202, 302, 193));
        AddBorough(new Array("Bexley", 371, 182, 374, 181, 386, 186, 398, 192, 399, 201, 409, 210, 424, 210, 421, 219, 420, 230, 408, 242, 402, 241, 393, 256, 389, 266, 390, 273, 393, 279, 383, 273, 369, 273, 348, 256, 354, 239, 353, 232, 350, 226, 360, 216, 374, 211, 373, 192));
        AddBorough(new Array("Havering", 383, 75, 398, 77, 405, 73, 420, 73, 429, 65, 434, 65, 434, 72, 444, 79, 445, 91, 449, 95, 458, 96, 455, 104, 463, 110, 473, 111, 481, 118, 482, 133, 494, 147, 479, 149, 459, 154, 454, 160, 455, 168, 450, 168, 448, 158, 438, 172, 441, 180, 432, 180, 423, 198, 411, 200, 411, 191, 398, 178, 392, 178, 393, 168, 410, 144, 405, 126, 392, 128, 385, 119, 380, 112, 385, 101));
        AddBorough(new Array("Barking &amp; Dagenham", 382, 95, 373, 105, 374, 122, 373, 127, 373, 140, 355, 148, 341, 148, 339, 159, 350, 163, 355, 168, 355, 175, 366, 175, 378, 172, 385, 177, 390, 177, 392, 164, 407, 145, 406, 136, 404, 130, 387, 130, 383, 123, 383, 117, 378, 112, 382, 104));
        AddBorough(new Array("Newham", 289, 145, 297, 146, 309, 140, 317, 140, 319, 133, 325, 135, 331, 136, 339, 147, 336, 156, 339, 162, 348, 165, 354, 168, 353, 176, 351, 176, 337, 189, 311, 188, 307, 185, 307, 180, 295, 172, 292, 163, 287, 158, 287, 147));
        AddBorough(new Array("Tower Hamlets", 285, 150, 269, 160, 261, 160, 259, 167, 256, 173, 258, 178, 257, 183, 262, 186, 272, 181, 282, 181, 285, 187, 286, 198, 295, 203, 298, 200, 294, 194, 294, 187, 300, 181, 305, 182, 297, 174, 292, 164, 284, 156));
        AddBorough(new Array("Hackney", 241, 122, 242, 126, 238, 129, 238, 132, 249, 135, 254, 146, 245, 163, 250, 170, 255, 170, 257, 168, 259, 160, 267, 157, 284, 147, 285, 140, 279, 134, 269, 134, 263, 124, 262, 120));
        AddBorough(new Array("Islington", 215, 129, 228, 122, 236, 127, 236, 133, 246, 136, 248, 139, 251, 146, 243, 164, 248, 170, 237, 170, 230, 163, 226, 148, 216, 139));
        AddBorough(new Array("Camden", 177, 142, 192, 127, 208, 126, 213, 128, 215, 136, 224, 150, 228, 163, 234, 172, 232, 177, 229, 177, 223, 179, 215, 169, 209, 159, 197, 156, 187, 161, 179, 148, 173, 142));
        AddBorough(new Array("Redbridge", 308, 72, 311, 78, 336, 93, 348, 93, 354, 84, 364, 84, 377, 75, 381, 78, 381, 93, 373, 103, 372, 122, 370, 125, 371, 139, 360, 141, 354, 146, 350, 143, 345, 147, 340, 147, 333, 136, 325, 133, 318, 130, 316, 138, 309, 138, 303, 133, 307, 125, 306, 111, 303, 100, 308, 84, 305, 80));
        AddBorough(new Array("Waltham Forest", 283, 61, 287, 60, 290, 57, 304, 59, 307, 61, 307, 70, 302, 79, 306, 84, 301, 101, 305, 115, 304, 129, 303, 136, 308, 139, 296, 144, 289, 142, 280, 134, 271, 133, 264, 123, 264, 117, 272, 107, 274, 97, 278, 92, 278, 82, 282, 73));
        AddBorough(new Array("Haringey", 198, 122, 204, 113, 204, 98, 209, 99, 209, 99, 215, 94, 215, 90, 223, 90, 245, 93, 262, 90, 273, 93, 271, 96, 271, 106, 262, 118, 240, 120, 239, 127, 228, 120, 215, 127));
        AddBorough(new Array("Enfield", 189, 36, 201, 21, 201, 5, 213, 15, 226, 12, 245, 18, 285, 21, 285, 31, 285, 46, 282, 54, 280, 71, 274, 84, 274, 90, 260, 87, 241, 90, 217, 87, 216, 80, 222, 69, 212, 60, 207, 48, 190, 41));
        AddBorough(new Array("Hillingdon", 56, 90, 37, 84, 31, 92, 7, 76, 7, 90, 5, 106, 21, 139, 14, 155, 12, 167, 14, 180, 19, 192, 9, 213, 15, 226, 35, 230, 41, 234, 41, 239, 46, 237, 48, 218, 55, 212, 56, 199, 68, 186, 68, 182, 76, 171, 61, 167, 52, 162, 52, 157, 60, 158, 73, 147, 73, 141));
        AddBorough(new Array("Brent", 139, 100, 133, 109, 133, 117, 133, 117, 120, 114, 107, 122, 109, 127, 104, 140, 117, 150, 119, 159, 130, 160, 134, 164, 147, 162, 152, 163, 160, 161, 174, 166, 178, 162, 183, 163, 186, 160, 172, 140, 166, 126, 151, 131, 149, 113, 144, 107, 144, 103));
        AddBorough(new Array("Harrow", 61, 89, 60, 98, 75, 139, 75, 145, 82, 145, 94, 138, 101, 142, 107, 129, 104, 120, 104, 120, 121, 112, 130, 114, 131, 107, 136, 99, 116, 73, 116, 73, 116, 73, 120, 58, 92, 77));
        AddBorough(new Array("Barnet", 121, 60, 118, 69, 140, 98, 146, 102, 152, 111, 152, 128, 163, 126, 169, 128, 174, 142, 191, 126, 198, 123, 202, 110, 202, 95, 210, 98, 214, 93, 215, 82, 220, 71, 212, 63, 208, 53, 188, 41, 186, 36, 154, 53, 154, 53));
        AddBorough(new Array("City of London", 234, 176, 238, 171, 253, 172, 257, 179, 253, 186, 235, 186));
    }

    function AddBorough(a) {
        _borough_data[_borough_data.length] = a;
    }
        

    function MakePathFromCoords(coords) {
        var i, str = "M ";
        for (i = 1; i + 1 < coords.length; i += 2 ) {
            str += " " + coords[i] + " " + coords[i + 1] + " L ";
        }
        str += coords[1] + " " + coords[2];
        return str;
    }
    
    function Sayuser( str )
    {
        $("#lblSayuser").text( str );
    }

    function MakePathString(x0, y0, x1, y1) {
        return "M" + x0 + " " + y0 + "L" + x1 + " " + y1;
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
    <a href="UKMaps.aspx?name=England">England</a>
    <a href="UKMaps.aspx?name=Wales">Wales</a>
    <a href="UKMaps.aspx?name=Scotland">Scotland</a>
    <a href="UKMaps.aspx?name=Northern+Ireland">Northern Ireland</a>
    <a href="UKMaps.aspx?name=Bailiwick+of+Guernsey">Bailiwick of Guernsey</a>
    <a href="UKMaps.aspx?name=Isle+of+Man">Isle of Man</a>
    <span id="lblSayuser"></span>
    
    

<div id="notepad"></div>
    
<div class="Invisible">
<img alt="London borough map" border="0" height="398" id="boroughmap" src="images/boroughmap.gif" usemap="#m_boroughmap" width="500" />    
<map id="m_boroughmap" name="m_boroughmap">
<area alt="Bromley" coords="257,271,263,264,272,268,281,268,287,264,303,275,304,274,312,264,325,264,326,260,318,252,321,248,335,263,342,264,348,258,363,274,381,277,385,276,393,282,393,287,396,295,391,302,394,313,383,335,377,334,371,341,375,349,373,351,364,354,354,366,354,379,354,384,340,388,325,374,315,385,315,385,310,363,305,348,303,331,290,318,287,302,280,297,275,297,267,287" href="http://www.londoncouncils.gov.uk/culturetourismand2012/barking-greenwich.htm#bromley" shape="poly" title="Bromley" />
<area alt="Croydon" coords="253,269,235,269,226,280,230,288,230,293,225,298,234,326,234,341,233,344,225,339,221,345,221,363,213,363,215,377,226,381,233,391,238,391,252,374,259,362,271,360,274,356,274,348,281,343,290,343,296,348,302,347,302,334,285,317,285,306,277,300,258,281" href="http://www.londoncouncils.gov.uk/culturetourismand2012/barking-greenwich.htm#croydon" shape="poly" title="Croydon" />
<area alt="Sutton" coords="165,304,160,309,160,316,169,320,177,335,175,345,175,351,189,340,209,356,213,362,219,361,218,343,225,337,232,340,232,329,223,298,214,304,200,296,195,302,182,302,179,299,173,308" href="http://www.londoncouncils.gov.uk/culturetourismand2012/southwark-westminster.htm#sutton" shape="poly" title="Sutton" />
<area alt="Merton" coords="188,253,165,254,156,259,156,270,160,280,160,293,164,298,174,306,178,300,195,300,200,294,209,299,216,301,229,293,225,284,224,279,216,277,215,272,201,270,192,264" href="http://www.londoncouncils.gov.uk/culturetourismand2012/kingston-richmond.htm#merton" shape="poly" title="Merton" />
<area alt="Kingston" coords="115,357,122,351,126,340,134,333,134,327,152,306,162,305,163,300,156,292,153,259,151,257,145,263,131,265,126,260,122,261,126,267,123,291,120,298,126,306,126,314,115,332" href="http://www.londoncouncils.gov.uk/culturetourismand2012/kingston-richmond.htm#kingston" shape="poly" title="Kingston" />
<area alt="Wandsworth" coords="224,210,208,211,200,215,193,229,179,231,169,223,166,223,164,231,153,231,149,234,154,240,155,255,165,250,187,251,190,249,194,262,210,269,217,269,221,265,221,259,213,249,217,242,212,236,211,226" href="http://www.londoncouncils.gov.uk/culturetourismand2012/southwark-westminster.htm#wandsworth" shape="poly" title="Wandsworth" />
<area alt="Richmond" coords="119,260,126,258,132,263,143,263,153,255,150,241,146,235,153,228,161,228,164,220,169,221,161,206,150,225,141,221,132,209,123,220,116,228,126,236,126,241,115,234,87,235,77,245,85,250,85,256,76,264,77,271,81,280,93,275,114,294,118,289,119,266" href="http://www.londoncouncils.gov.uk/culturetourismand2012/kingston-richmond.htm#richmond" shape="poly" title="Richmond" />
<area alt="Hounslow" coords="46,238,50,219,59,211,60,198,69,203,77,203,84,206,92,198,106,201,123,202,132,193,144,198,149,198,153,196,154,201,149,218,136,203,128,204,116,219,110,224,115,232,86,233,74,245,75,249,84,249,74,265,74,271,63,266,54,258,51,262,36,250,36,241" href="http://www.londoncouncils.gov.uk/culturetourismand2012/hackney-kensington.htm#hounslow" shape="poly" title="Hounslow" />
<area alt="Ealing" coords="153,165,146,163,138,168,130,166,129,161,118,165,116,153,103,141,100,145,93,139,82,147,74,147,62,160,54,160,63,166,73,167,80,172,71,182,71,187,62,195,71,201,78,200,83,203,92,195,107,199,122,199,134,190,143,196,150,195,151,186,149,178,155,171" href="http://www.londoncouncils.gov.uk/culturetourismand2012/barking-greenwich.htm#ealing" shape="poly" title="Ealing" />
<area alt="Hammersmith &amp; Fulham" coords="194,214,186,224,181,223,165,200,157,202,155,196,153,193,155,183,152,178,156,171,156,164,164,165,164,174,169,182,177,190,187,208,190,215" href="http://www.londoncouncils.gov.uk/culturetourismand2012/hackney-kensington.htm#hammersmith" shape="poly" title="Hammersmith &amp; Fulham" />
<area alt="Kensington &amp; Chelsea" coords="210,202,205,190,198,196,182,174,173,167,166,166,166,171,170,180,175,185,190,207,192,213" href="http://www.londoncouncils.gov.uk/culturetourismand2012/hackney-kensington.htm#kensington" shape="poly" title="Kensington &amp; Chelsea" />
<area alt="Westminster" coords="233,180,226,185,224,203,214,203,208,191,204,189,199,194,184,173,175,166,180,162,185,164,198,156,206,161,209,161,221,181" href="http://www.londoncouncils.gov.uk/culturetourismand2012/southwark-westminster.htm#westminster" shape="poly" title="Westminster" />
<area alt="Lambeth" coords="255,267,236,268,224,279,218,275,217,270,222,266,222,259,215,247,218,241,214,230,226,213,226,209,231,204,231,193,234,188,234,193,238,199,235,206,245,220,245,228,243,232,242,244,245,248,245,256" href="http://www.londoncouncils.gov.uk/culturetourismand2012/kingston-richmond.htm#lambeth" shape="poly" title="Lambeth" />
<area alt="Southwark" coords="279,195,277,186,268,193,258,193,251,188,237,188,237,193,240,199,238,207,248,221,247,230,244,232,244,243,247,248,248,255,257,265,259,255,264,252,264,244,269,238,277,234,268,213,270,202" href="http://www.londoncouncils.gov.uk/culturetourismand2012/southwark-westminster.htm#southwark" shape="poly" title="Southwark" />
<area alt="Lewisham" coords="280,198,282,205,282,207,289,212,287,216,288,221,292,222,296,218,306,218,303,225,305,230,312,236,312,242,318,247,316,252,325,260,322,263,312,260,303,268,303,273,299,272,286,261,280,265,271,265,264,262,259,262,265,253,266,246,271,241,279,237,276,227,270,215,271,205" href="http://www.londoncouncils.gov.uk/culturetourismand2012/kingston-richmond.htm#lewisham" shape="poly" title="Lewisham" />
<area alt="Greenwich" coords="302,189,312,200,312,200,344,199,356,185,369,184,369,210,354,216,347,224,351,233,351,240,346,251,346,256,338,262,322,245,316,245,312,235,306,228,309,216,297,215,291,219,288,215,292,211,285,206,297,209,305,202,302,193" href="http://www.londoncouncils.gov.uk/culturetourismand2012/barking-greenwich.htm#greenwich" shape="poly" title="Greenwich" />
<area alt="Bexley" coords="371,182,374,181,386,186,398,192,399,201,409,210,424,210,421,219,420,230,408,242,402,241,393,256,389,266,390,273,393,279,383,273,369,273,348,256,354,239,353,232,350,226,360,216,374,211,373,192" href="http://www.londoncouncils.gov.uk/culturetourismand2012/barking-greenwich.htm#bexley" shape="poly" title="Bexley" />
<area alt="Havering" coords="383,75,398,77,405,73,420,73,429,65,434,65,434,72,444,79,445,91,449,95,458,96,455,104,463,110,473,111,481,118,482,133,494,147,479,149,459,154,454,160,455,168,450,168,448,158,438,172,441,180,432,180,423,198,411,200,411,191,398,178,392,178,393,168,410,144,405,126,392,128,385,119,380,112,385,101" href="http://www.londoncouncils.gov.uk/culturetourismand2012/hackney-kensington.htm#havering" shape="poly" title="Havering" />
<area alt="Barking &amp; Dagenham" coords="382,95,373,105,374,122,373,127,373,140,355,148,341,148,339,159,350,163,355,168,355,175,366,175,378,172,385,177,390,177,392,164,407,145,406,136,404,130,387,130,383,123,383,117,378,112,382,104" href="http://www.londoncouncils.gov.uk/culturetourismand2012/barking-greenwich.htm#barking" shape="poly" title="Barking &amp; Dagenham" />
<area alt="Newham" coords="289,145,297,146,309,140,317,140,319,133,325,135,331,136,339,147,336,156,339,162,348,165,354,168,353,176,351,176,337,189,311,188,307,185,307,180,295,172,292,163,287,158,287,147" href="http://www.londoncouncils.gov.uk/culturetourismand2012/kingston-richmond.htm#newham" shape="poly" title="Newham" />
<area alt="Tower Hamlets" coords="285,150,269,160,261,160,259,167,256,173,258,178,257,183,262,186,272,181,282,181,285,187,286,198,295,203,298,200,294,194,294,187,300,181,305,182,297,174,292,164,284,156" href="http://www.londoncouncils.gov.uk/culturetourismand2012/southwark-westminster.htm#tower" shape="poly" title="Tower Hamlets" />
<area alt="Hackney" coords="241,122,242,126,238,129,238,132,249,135,254,146,245,163,250,170,255,170,257,168,259,160,267,157,284,147,285,140,279,134,269,134,263,124,262,120" href="http://www.londoncouncils.gov.uk/culturetourismand2012/hackney-kensington.htm#hackney" shape="poly" title="Hackney" />
<area alt="Islington" coords="215,129,228,122,236,127,236,133,246,136,248,139,251,146,243,164,248,170,237,170,230,163,226,148,216,139" href="http://www.londoncouncils.gov.uk/culturetourismand2012/hackney-kensington.htm#islington" shape="poly" title="Islington" />
<area alt="Camden" coords="177,142,192,127,208,126,213,128,215,136,224,150,228,163,234,172,232,177,229,177,223,179,215,169,209,159,197,156,187,161,179,148,173,142" href="http://www.londoncouncils.gov.uk/culturetourismand2012/barking-greenwich.htm#camden" shape="poly" title="Camden" />
<area alt="Redbridge" coords="308,72,311,78,336,93,348,93,354,84,364,84,377,75,381,78,381,93,373,103,372,122,370,125,371,139,360,141,354,146,350,143,345,147,340,147,333,136,325,133,318,130,316,138,309,138,303,133,307,125,306,111,303,100,308,84,305,80" href="http://www.londoncouncils.gov.uk/culturetourismand2012/kingston-richmond.htm#redbridge" shape="poly" title="Redbridge" />
<area alt="Waltham Forest" coords="283,61,287,60,290,57,304,59,307,61,307,70,302,79,306,84,301,101,305,115,304,129,303,136,308,139,296,144,289,142,280,134,271,133,264,123,264,117,272,107,274,97,278,92,278,82,282,73" href="http://www.londoncouncils.gov.uk/culturetourismand2012/southwark-westminster.htm#waltham" shape="poly" title="Waltham Forest" />
<area alt="City of London" coords="234,176,238,171,253,172,257,179,253,186,235,186" href="http://www.londoncouncils.gov.uk/culturetourismand2012/barking-greenwich.htm#city" shape="poly" title="City of London" />
<area alt="Haringey" coords="198,122,204,113,204,98,209,99,209,99,215,94,215,90,223,90,245,93,262,90,273,93,271,96,271,106,262,118,240,120,239,127,228,120,215,127" href="http://www.londoncouncils.gov.uk/culturetourismand2012/hackney-kensington.htm#haringey" shape="poly" title="Haringey" />
<area alt="Enfield" coords="189,36,201,21,201,5,213,15,226,12,245,18,285,21,285,31,285,46,282,54,280,71,274,84,274,90,260,87,241,90,217,87,216,80,222,69,212,60,207,48,190,41" href="http://www.londoncouncils.gov.uk/culturetourismand2012/barking-greenwich.htm#enfield" shape="poly" title="Enfield" />
<area alt="Hillingdon" coords="56,90,37,84,31,92,7,76,7,90,5,106,21,139,14,155,12,167,14,180,19,192,9,213,15,226,35,230,41,234,41,239,46,237,48,218,55,212,56,199,68,186,68,182,76,171,61,167,52,162,52,157,60,158,73,147,73,141" href="http://www.londoncouncils.gov.uk/culturetourismand2012/hackney-kensington.htm#hillingdon" shape="poly" title="Hillingdon" />
<area alt="Brent" coords="139,100,133,109,133,117,133,117,120,114,107,122,109,127,104,140,117,150,119,159,130,160,134,164,147,162,152,163,160,161,174,166,178,162,183,163,186,160,172,140,166,126,151,131,149,113,144,107,144,103" href="http://www.londoncouncils.gov.uk/culturetourismand2012/barking-greenwich.htm#brent" shape="poly" title="Brent" />
<area alt="Harrow" coords="61,89,60,98,75,139,75,145,82,145,94,138,101,142,107,129,104,120,104,120,121,112,130,114,131,107,136,99,116,73,116,73,116,73,120,58,92,77" href="http://www.londoncouncils.gov.uk/culturetourismand2012/hackney-kensington.htm#harrow" shape="poly" title="Harrow" />
<area alt="Barnet" coords="121,60,118,69,140,98,146,102,152,111,152,128,163,126,169,128,174,142,191,126,198,123,202,110,202,95,210,98,214,93,215,82,220,71,212,63,208,53,188,41,186,36,154,53,154,53" href="http://www.londoncouncils.gov.uk/culturetourismand2012/barking-greenwich.htm#barnet" shape="poly" title="Barnet" /></map>    
</div>

    </div>
    </form>
</body>
</html>
