var _iteration = 0;
var _width = 600;
var _height = 429;

$(document).ready(function() {
    $("#lnkNext").click( function() { DoNext(); } );
    DrawCurrent();
});

function DoNext()
{
    _iteration++;
    if (_iteration > 1)
        _iteration = 0;
    DrawCurrent();        
}

function DrawCurrent() {
    if (_iteration == 0)
        DrawDull();
    else
        DrawFun();
}

function DrawFun() {
    var x, y, msg1, msg2, uri;

    $("#notepad").html("");
    var paper = Raphael("notepad", _width, _height);
    msg1 = "In Wandsworth, three out of four people\ngo to a museum or gallery for";
    msg2 = "and the fourth is my brother Cyril";

    uri = "http://www.camellie.com//images/20090704020254_fun_colour.jpg";
    paper.image(uri, 0, 0, _width, _height);

    // paper.rect(0, 0, _width - 1, _height - 1).attr({ "stroke": "black", "fill": "gray" });
    x = _width / 2;
    y = 30;
    paper.text(x, y, msg1).attr({ "font-family": "Sans Serif", "font-size": 18, "fill": "white" }); //.attr({ "font": "Sans Serif", "font-size": 18, "fill":"white" });

    y = 400;
    paper.text(x, y, msg2).attr({ "font-family": "Sans Serif", "font-size": 18, "fill": "white" }); //.attr({ "font": "Sans Serif", "font-size": 18, "fill": "white" });
}

function DrawDull() {
    var x, y, msg;

    $("#notepad").html("");
    var paper = Raphael("notepad", _width, _height);
    x = _width / 2;
    y = _height / 2;
    msg = "In Wandsworth, 73% of people visited\na museum or gallery in the last 12 months";
    paper.rect(0, 0, _width - 1, _height - 1).attr({ "stroke": "black", "fill": "gray" });
    paper.text(x, y, msg).attr({"font-family":"Courier New","font-size":18});
}