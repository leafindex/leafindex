$(document).ready(function() {
    makeNavMenu();
    LoadData();
    LoadLocations();

    RDraw();
    $(".MonthLink").mouseenter(function() { MonthClick(this); });
    $("#month0").addClass("MonthSelected");
    $("#notepad").click(function(e) {
        var x, y, offset;
        offset = $("#notepad").offset();
        x = e.pageX - offset.left;
        y = e.pageY - offset.top;
        ClickAt(x, y, offset);
    });
});

var _show_month = 0;

function MonthClick(elt) {
    str = elt.id.substr(5);
    var x = 1 * str;
    _show_month = x;
    RDraw();
    $(".MonthLink").removeClass("MonthSelected");
    $(elt).addClass("MonthSelected");
    $(".BoroughDetail, .BoroughGraph").remove();
}

function ClickAt(x, y, offset) {
    var i, boro, elt;
    for (i = 0; i < _locations.length; i++) {
        boro = _locations[i];
        if (PointForBorough(boro, x, y)) {
            elt = $("<div class='BoroughDetail'>" + MakeBoroughDetail(boro) + "</div>");
            elt.css({ "left": (x + offset.left) + "px", "top": (y + offset.top) + "px" });
            elt.show();
            elt.appendTo("body");
            return;
        }
    }
}

function PointForBorough(boro, x, y) {
    if (PointsNear(boro[1], boro[2], x, y, 4))
        return true;
    if (PointsNear(boro[3], boro[4], x, y, boro[5]))
        return true;
    return false;
}

function PointsNear(x0, y0, x1, y1, tolerance) {
    var diff = x0 - x1;
    if (diff < 0)
        diff = 0 - diff;
    if (diff > tolerance)
        return false;
    diff = y0 - y1;
    if (diff < 0)
        diff = 0 - diff;
    if (diff > tolerance)
        return false;
    return true;
}

function MakeBoroughDetail(boro) {
    return "<a class='MonthGraphLink' onClick='javascript:ShowGraph(this);' href='#'>" + boro[0] + "</a><br/><b>" + GetBoroughFigure(boro[0], _show_month) + "</b>";
}

function ShowGraph(elt) {
    var offset = $(elt).parent().offset();
    ShowBoroughGraphAt($(elt).text(), offset.left, offset.top);
}

var BGWIDTH = 200;
var BGHEIGHT = 100;
function ShowBoroughGraphAt(borough, x, y) {
    var graphid = "bgraph" + $(".BoroughGraph").length;

    elt = $("<div class='BoroughGraph'>" + MakeBoroughHeading(borough) + "<br/><div id='" + graphid + "' width='" + BGWIDTH + "' height='" + BGHEIGHT + "'></div></div>");
    elt.css({ "left": x + "px", "top": y + "px" });
    elt.show();
    elt.appendTo("body");

    DrawBoroughGraph(borough, graphid);
}

function MakeBoroughHeading(borough) {
    return borough + " <b>" + GetBoroughFigure(borough, _show_month) + "</b>";
}

var NUM_POINTS = 24;

function DrawBoroughGraph(borough, graphid) {
    var x, xdiff, i, y, blockwidth, mycontext, c, pathstring, colour;
    var paper = Raphael(graphid, BGWIDTH + 1, BGHEIGHT + 1 );

    xdiff = BGWIDTH / (NUM_POINTS + 3);
    blockwidth = 1;

    c = paper.path(MakePathString(0, 0, 0, BGHEIGHT));
    c.attr("stroke", "#000000" );
    c = paper.path(MakePathString(0, BGHEIGHT, BGWIDTH, BGHEIGHT ));
    c.attr("stroke", "#000000");

    x = xdiff;
    
    for (i = 0; i < NUM_POINTS; i++) {
        y = BGHEIGHT * (GetBoroughFigure(borough, i) * 1.0 / _max_assaults);

        if (i == _show_month)
            colour = "#0000ff";
        else
            colour = "#999999";

        c = paper.path(MakePathString(x, BGHEIGHT - y, x, BGHEIGHT - 1));
        c.attr("stroke", colour);
        c.attr("stroke-width", 3);          

//        c = paper.rect(x, BGHEIGHT - y, 3, y - 1 );
//        c.attr("fill", colour);
//        c.attr("stroke", colour);      
        x += xdiff;
        if (i % 12 == 0)
            x += xdiff;
    }
}


function GetBoroughFigure(borough, idx) {
    var b, i = 0;
    for (i = 0; i < _data.length; i++) {
        b = _data[i];
        if (b[0] == borough) {
            return b[idx + 1];
        }
    }
    return 0;
}

var _locations;

function LoadLocations() {
    var i = 0;
    _locations = new Array();
    AddLocation(230, 210, "Harrow");
    AddLocation(280, 240, "Brent");

    AddLocation(350, 280, "Westminster");

    AddLocation(330, 190, "Barnet");
    AddLocation(350, 240, "Camden");
    AddLocation(380, 160, "Enfield");

    AddLocation(380, 210, "Haringey");
    AddLocation(380, 240, "Islington");
    AddLocation(440, 200, "Waltham Forest");
    AddLocation(410, 250, "Hackney");

    AddLocation(500, 210, "Redbridge");
    AddLocation(600, 220, "Havering");
    AddLocation(530, 250, "Barking and Dagenham");
    AddLocation(460, 260, "Newham");
    AddLocation(440, 270, "Tower Hamlets");

    AddLocation(530, 340, "Bexley");
    AddLocation(480, 340, "Greenwich");
    AddLocation(490, 410, "Bromley");
    AddLocation(440, 340, "Lewisham");

    AddLocation(410, 320, "Southwark");
    AddLocation(400, 400, "Croydon");
    AddLocation(380, 340, "Lambeth");

    AddLocation(340, 420, "Sutton");

    AddLocation(340, 380, "Merton");
    AddLocation(340, 340, "Wandsworth");
    AddLocation(300, 380, "Kingston upon Thames");
    AddLocation(250, 370, "Richmond upon Thames");

    AddLocation(330, 290, "Kensington and Chelsea");
    AddLocation(220, 330, "Hounslow");
    AddLocation(310, 290, "Hammersmith and Fulham");
    AddLocation(240, 270, "Ealing");
    AddLocation(190, 240, "Hillingdon");
}

var RADIUS = 40;

function AddLocation(x, y, borough) {
    var i = _locations.length;
    var radius = (1.0 * RADIUS * GetBoroughFigure(borough, 1)) / _max_assaults;
    _locations[i] = new Array(borough, x, y, CalcBlobX(i), CalcBlobY(i), radius);
}

function RDraw() {
		$("#notepad").empty();
    var paper = Raphael("notepad", 800, 640);
    var left_offset = (800 - 500) / 2;
    var top_offset = (600 - 398) / 2;
    var c = paper.image("images/boroughmap.gif", left_offset, top_offset, 500, 398);
    for (i = 0; i < _locations.length; i++) {
        DoBlob(paper, i);
    }
};

function CalcBlobX(i) {
    if (i < 10) return 40 + i * 80;
    if (i < 17) return 760;
    if (i < 26) return 760 - (i - 16) * 80;
    return 40;
}
function CalcBlobY(i) {
    if (i < 10) return 40;
    if (i < 17) return 40 + (i - 9) * 80;
    if (i < 26) return 600;
    return 600 - (i - 25) * 80;
}

function DoBlob(paper, i) {
    var boro, borough, mapx, mapy, blobx, bloby, pathstring, c, colour;
    boro = _locations[i];
    borough = boro[0];
    mapx = boro[1];
    mapy = boro[2];
    blobx = boro[3];
    bloby = boro[4];

    var radius = RADIUS * Math.sqrt(GetBoroughFigure(borough, _show_month)) / Math.sqrt(_max_assaults);

    colour = GetColour(i);

    pathstring = MakePathString(mapx, mapy, blobx, bloby);
    c = paper.path(pathstring);
    c.attr("stroke", colour);

    c = paper.circle(mapx, mapy, 2);
    c.attr("fill", colour);
    c.attr("stroke", colour);
    c = paper.circle(blobx, bloby, radius);
    c.attr("fill", colour);
    c.attr("stroke", colour);
}

function MakePathString( x0, y0, x1, y1) {
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
