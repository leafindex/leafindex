$(document).ready(function() {
    makeNavMenu();
    LoadData();
    LoadLocations();
    $("#BoroughDetail").hide();
    $(".MonthLink").mouseenter(function() { MonthClick(this); });
    $("#month0").addClass("MonthSelected");
    draw();
    $("#canvas").click(function(e) {
        var x, y, offset;
        // offset = $(e.target).offset(); // fails in IE
        offset = $("#canvas").offset();
        x = e.pageX - offset.left;
        y = e.pageY - offset.top;
        ClickAt(x, y, offset);
    });
    $("#imgMap").load(function() { draw(); });
});

var _show_month = 0;

function MonthClick(elt) {
    str = elt.id.substr(5);
    var x = 1 * str;
    _show_month = x;
    draw();
    $(".MonthLink").removeClass("MonthSelected");
    $(elt).addClass("MonthSelected");
    $(".BoroughDetail").remove();
    $(".BoroughGraph").remove();
}

function ClickAt(x, y, offset ) {
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
    //return boro[0] + "<br/>" + MakeMonthYear(_show_month) + "<br/>" + GetBoroughFigure(boro[0], _show_month) + " Incidents";
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

    elt = $("<div class='BoroughGraph'>" + MakeBoroughHeading( borough ) + "<br/><canvas id='" + graphid + "' width='" + BGWIDTH + "' height='" + BGHEIGHT + "'></canvas></div>");
    elt.css({ "left": x + "px", "top": y + "px" });
    elt.show();
    elt.appendTo("body");

    DrawBoroughGraph(borough, graphid);
}

function MakeBoroughHeading(borough) {
    return borough + " <b>" + GetBoroughFigure(borough, _show_month) + "</b>";
}

var NUM_POINTS = 24;

// http://www.mail-archive.com/jquery-en@googlegroups.com/msg49278.html is a complicated solution...
// http://blog.poundbang.in/post/29578240/excanvas-js-getting-it-to-work-with-dynamically-added looked good
function DrawBoroughGraph(borough, graphid) {
    var x, xdiff, i, y, blockwidth, mycontext;
    var canvas = document.getElementById(graphid);

    if (typeof G_vmlCanvasManager != 'undefined') {
        canvas = G_vmlCanvasManager.initElement(canvas);
    }    

    xdiff = BGWIDTH / (NUM_POINTS + 3);
    blockwidth = 1;

    if (canvas.getContext) {
        var ctx = canvas.getContext("2d");

        ctx.beginPath();
        ctx.moveTo(0, 0);
        ctx.lineTo(0, BGHEIGHT);
        ctx.lineTo(BGWIDTH, BGHEIGHT);
        ctx.stroke();

        x = xdiff;
        ctx.lineWidth = xdiff - 2;

        for (i = 0; i < NUM_POINTS; i++) {
            y = BGHEIGHT * (GetBoroughFigure(borough, i) * 1.0 / _max_assaults);

            if (i == _show_month)
                ctx.strokeStyle = "#0000ff";
            else
                ctx.strokeStyle = "#999999";
            ctx.beginPath();
            ctx.moveTo(x, BGHEIGHT - y);
            ctx.lineTo(x, BGHEIGHT);
            ctx.stroke();
            x += xdiff;
            if (i % 12 == 0)
                x += xdiff;
        }
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
    alert(borough + " not found");
    return 0;
}

var _locations;

function LoadLocations()
{
    var i = 0;
    _locations = new Array();
    AddLocation( 230, 210, "Harrow");
    AddLocation( 280, 240, "Brent");

    AddLocation( 350, 280, "Westminster");

    AddLocation( 330, 190, "Barnet");
    AddLocation( 350, 240, "Camden");
    AddLocation( 380, 160, "Enfield");

    AddLocation( 380, 210, "Haringey");
    AddLocation( 380, 240, "Islington");
    AddLocation( 440, 200, "Waltham Forest");
    AddLocation( 410, 250, "Hackney");

    AddLocation( 500, 210, "Redbridge");
    AddLocation( 600, 220, "Havering");
    AddLocation( 530, 250, "Barking and Dagenham");
    AddLocation( 460, 260, "Newham");
    AddLocation( 440, 270, "Tower Hamlets");

    AddLocation( 530, 340, "Bexley");
    AddLocation( 480, 340, "Greenwich");
    AddLocation( 490, 410, "Bromley");
    AddLocation( 440, 340, "Lewisham");

    AddLocation( 410, 320, "Southwark");
    AddLocation( 400, 400, "Croydon");
    AddLocation( 380, 340, "Lambeth");

    AddLocation( 340, 420, "Sutton");

    AddLocation( 340, 380, "Merton");
    AddLocation( 340, 340, "Wandsworth");
    AddLocation( 300, 380, "Kingston upon Thames");
    AddLocation( 250, 370, "Richmond upon Thames");

    AddLocation( 330, 290, "Kensington and Chelsea");
    AddLocation( 220, 330, "Hounslow");
    AddLocation( 310, 290, "Hammersmith and Fulham");
    AddLocation( 240, 270, "Ealing");
    AddLocation(190, 240, "Hillingdon");
}

var RADIUS = 40;

function AddLocation(x, y, borough) {
    var i = _locations.length;
    var radius = (1.0 * RADIUS * GetBoroughFigure(borough, 1)) / _max_assaults;
    _locations[i] = new Array(borough, x, y, CalcBlobX(i), CalcBlobY(i), radius );
}

function draw() {
    var canvas = document.getElementById("canvas");
    if (canvas.getContext) {
        var ctx = canvas.getContext("2d");
        ctx.clearRect(0, 0, 800, 640);

        var img = document.getElementById("imgMap");
        var left_offset = (800 - 500) / 2;
        var top_offset = (600 - 398) / 2;
        var blobx, bloby, i;
        
        ctx.drawImage(img, left_offset, top_offset);

        for (i = 0; i < _locations.length; i++) {
            DoBlob(ctx, i);
        }
        var now = new Date();
        Sayuser("Draw() at " + now);
    }        
};

function CalcBlobX( i )
{
    if (i < 10) return 40 + i * 80;
    if( i < 17 ) return 760;
    if( i < 26 ) return 760 - (i - 16) * 80;
    return 40;
}
function CalcBlobY( i )
{
    if (i < 10) return 40;
    if( i < 17 ) return 40 + (i - 9) * 80;
    if( i < 26 ) return 600;
    return 600 - ( i - 25 ) * 80;
}

function DoBlob(ctx, i) {
    var boro, borough, mapx, mapy, blobx, bloby;
    boro = _locations[i];
    borough = boro[0];
    mapx = boro[1];
    mapy = boro[2];
    blobx = boro[3];
    bloby = boro[4];

    var radius = RADIUS * Math.sqrt(GetBoroughFigure(borough, _show_month)) / Math.sqrt(_max_assaults);

    SetStyleForBlob(ctx, i);

    ctx.beginPath();
    ctx.moveTo(mapx, mapy);
    ctx.lineTo(blobx, bloby);
    ctx.stroke();

    ctx.beginPath();
    ctx.arc(blobx, bloby, radius, 0, Math.PI * 2, true);
    ctx.fill();

    ctx.arc(mapx, mapy, 2, 0, Math.PI * 2, true);
    ctx.fill();    
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

function SetStyleForBlob(ctx, i) {
    var idx = i % colours.length;
    ctx.strokeStyle = colours[idx];
    ctx.fillStyle = colours[idx];
}
