var _elt_array;
var _column_index = 2;

$(document).ready(function() {
    SelectCurrentTab();
    FillBoroughData();
    LoadKmlMaps();
    DrawMap();
    $("#WaitLoading").hide();
    $(".DisplayButton").click(function() { $("#WaitLoading").show(); })
});

function SelectTab(tabname) {
    $(".TabStop").removeClass("TabStopSelected");
    $("#Tab" + tabname).addClass("TabStopSelected");
    $(".TabOptions").hide();
    $("#Options" + tabname).show();
    $("#hdnTabSelected").val(tabname);
}

function ShowColumn(idx) {
    _column_index = idx;
    DrawMap();
}

function DrawMap() {
    var width, height, i, j, x, y, kmap, colour;
    width = 1000;
    height = 500;

    _elt_array = new Array();

    $("#notepad").html("");

    var paper = Raphael("notepad", width + 1, height + 1);
    for (i = 0; i < _kmlmap.length; i++) {
        kmap = _kmlmap[i];
        for (j = 2; j < kmap.length; j++) {
            elt = paper.path(kmap[j]);
            elt.attr("fill", GetBoroughColour(kmap[1]));
            _elt_array[_elt_array.length] = new Array(elt, kmap[1]);
            elt.mouseover(function(event) {
                DoMouseOver(this);
            });
        }
    }
    x = 400;
    y = 10;
    paper.text(x + 10, y + 40 + 10, GetMinValue());
    for (i = 0; i <= 10; i++) {
        elt = paper.rect(x, y, 20, 40);
        elt.attr("fill", GetStrengthColour(i * 10));
        x += 20;
    }
    paper.text(x - 10, y + 40 + 10, GetMaxValue());
}

function GetMinValue() {
    var i, itm, result = null;

    for (i = 0; i < _borough_data.length; i++) {
        itm = _borough_data[i];
        if (result == null || itm[_column_index] < result)
            result = itm[_column_index];
    }
    return result;
}
function GetMaxValue() {
    var i, itm, result = null;
    for (i = 0; i < _borough_data.length; i++) {
        itm = _borough_data[i];
        if (result == null || itm[_column_index] > result)
            result = itm[_column_index];
    }
    return result;
}

function GetBoroughColour(boro) {
    var perc, b = GetBoroughItem(boro);
    if (b == null)
        return GetStrengthColour(0);
    perc = 100 * (b[_column_index] - GetMinValue() ) / ( GetMaxValue() - GetMinValue() )
    return GetStrengthColour( Math.floor( perc + 0.5 ) );
}

function GetBoroughItem(borocode) {
    var i, itm, result = null;
    for (i = 0; i < _borough_data.length; i++) {
        itm = _borough_data[i];
        if (itm[1] == borocode)
            return itm;
    }
    return null;  
}

function GetStrengthColour(perc) {
    perc2 = 100 - perc;
    return "rgb(" + perc2 + "%," + perc2 + "%,100%)";
}

function DoMouseOver(elt) {
    var i, pair, b;
    for (i = 0; i < _elt_array.length; i++) {
        pair = _elt_array[i];
        if (pair[0] == elt) {
            b = GetBoroughItem(pair[1]);
            if (b == null)
                Sayuser(pair[1]);
            else
                Sayuser(b[0] + " " + b[_column_index]);
            return;
        }
    }
    Sayuser("Looked through " + _elt_array.length + " elements");
}

function Sayuser(str) {
    $("#lblSayuser").text(str);
}