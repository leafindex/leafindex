var _elt_array;
var _column_index = 2;
var _borough_data;

$(document).ready(function() {
    SetupTabLinks();
    SetupInputChanges();
    SelectTab();
    LoadKmlMaps();
    createData();
    FloatingInfoBoxHide();
});
function createData(){
	var args="arg1="+encodeURIComponent(SelectedTabName());
	var ins=$("#Tabbing").find(":input:not(button,[type=submit]):visible");
	var l=ins.length;
	for(var j=0;j<l&&j<2;j++){
		args+="&arg"+(j+2)+"="+encodeURIComponent(ins.eq(j).val());
	}
	$.ajax({
		url:"./handler/LondonData.ashx",
		cache:false,
		data:args,
		dataType:"json",
		success:function(data){
			_borough_data=[];
			$.each(data,function(i,v){
				if (v && typeof v==='object'){
					var a=[];
					for(var j in v){
						if(v.hasOwnProperty(j)){
							a.push(v[j]);
						}
					}
					_borough_data.push(a);
				}
			});
	    DrawMap();
		}
	});
}
function TabNameFromId(id){
	return id.substring(3);
}
function FirstTabName(){
	return TabNameFromId($(".TabStop:first","#Tabbing").attr("id"));
}
function SelectedTabName(){
	return TabNameFromId($(".TabStopSelected","#Tabbing").attr("id"));
}
function SetupTabLinks(){
	$("#TabArts,#TabBegging,#TabCrimes").click(function(){
		SelectTab(TabNameFromId(this.id),true);
		return false;
	});
}
function SetupInputChanges(){
	$("#Tabbing").find(":input:not(button,[type=submit])").change(function(){
		createData();
	});
}

function SelectTab(tabname,getdata) {
		if(!tabname){
			tabname=$("#hdnTabSelected").val();
			if(tabname==="NotSet"){
				tabname=FirstTabName();
			}
		}
		$("#Tabbing")
			.find(".TabStop")
				.removeClass("TabStopSelected")
				.end()
			.find(".TabOptions")
				.hide();
    $("#Tab" + tabname).addClass("TabStopSelected");
    $("#Options" + tabname).show();
    if(getdata){
			createData();
		}
}

function ShowColumn(idx) {
    _column_index = idx;
    DrawMap();
}

function DrawMap() {
    var width, height, i, j, x, y, kmap, colour;
    width = 1000;
    height = 500;

    _elt_array = [];

    $("#notepad").html("");
    FloatingInfoBoxHide();

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
    paper.text(x + 10, y + 40 + 10, GetMinValue(_column_index));
    for (i = 0; i <= 10; i++) {
        elt = paper.rect(x, y, 20, 40);
        elt.attr("fill", GetStrengthColour(i * 10));
        x += 20;
    }
    paper.text(x - 10, y + 40 + 10, GetMaxValue(_column_index));
}

function GetMinValue(idx) {
    var i, itm, result = null;

    if (_borough_data == null)
        return 0;
    for (i = 0; i < _borough_data.length; i++) {
        itm = _borough_data[i];
        if (result == null || itm[idx] < result)
            result = itm[idx];
    }
    return result;
}
function GetMaxValue(idx) {
    var i, itm, result = null;
    if (_borough_data == null)
        return 100;
    for (i = 0; i < _borough_data.length; i++) {
        itm = _borough_data[i];
        if (result == null || itm[idx] > result)
            result = itm[idx];
    }
    return result;
}

function GetBoroughColour(boro) {
    var perc, b = GetBoroughItem(boro);
    if (b == null)
        return GetStrengthColour(0);
    perc = 100 * (b[_column_index] - GetMinValue(_column_index)) / (GetMaxValue(_column_index) - GetMinValue(_column_index))
    return GetStrengthColour( Math.floor( perc + 0.5 ) );
}

function GetBoroughItem(borocode) {
    var i, itm, result = null;
    if (_borough_data == null)
        return null;
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

function ShowSideBySide(tabname) {
    Sayuser("SideBySide " + tabname);
    var args = "arg1=" + encodeURIComponent(tabname) + "&arg2=all";
    $.ajax({
        url: "./handler/LondonData.ashx",
        cache: false,
        data: args,
        dataType: "json",
        success: function(data) {
            _borough_data = [];
            $.each(data, function(i, v) {
                if (v && typeof v === 'object') {
                    var a = [];
                    for (var j in v) {
                        if (v.hasOwnProperty(j)) {
                            a.push(v[j]);
                        }
                    }
                    _borough_data.push(a);
                }
            });
            DrawSideBySide();
        }
    });
}

function DrawSideBySide() {
    var msg;
    msg = _borough_data.length + " boroughs " + _borough_data[0].length + " data points";
    Sayuser(msg);

    var width, height, x, series_count, idx, series_width, bidx, b, pathstr, elt, bcolour, left_margin, right_margin;
    width = 1000;
    height = 500;
    right_margin = 180;
    left_margin = 80;

    series_count = _borough_data[0].length - 2;
    series_width = (width - (left_margin + right_margin)) / (series_count - 1);

    _elt_array = [];

    $("#notepad").html("");
    FloatingInfoBoxHide();

    var paper = Raphael("notepad", width + 1, height + 1);
    x = left_margin;
    for (idx = 0; idx < series_count; idx++) {
        paper.path(MakePathString(x, 100, x, 400));

        paper.text(x, 410, GetMinValue(2 + idx) + "%");
        paper.text(x, 90, GetMaxValue(2 + idx) + "%");
        paper.text(x, 80, GetSeriesName(idx));
        
        x += series_width;
    }
    //Raphael.getColor.reset();
    for (bidx = 0; bidx < _borough_data.length; bidx++) {
        x = left_margin;
        b = _borough_data[bidx];
        bcolour = GetColour(bidx);
        pathstr = "";
        for (idx = 0; idx < series_count; idx++) {
            if (idx == 0)
                pathstr += "M";
            else
                pathstr += "L";
            pathstr += x + " " + CalcSideBySideY(b[2 + idx], GetMinValue(2 + idx), GetMaxValue(2 + idx));
            x += series_width;
        }
        elt = paper.text(width - right_margin / 2, 10 + bidx * (480 / _borough_data.length), b[0]).attr({ stroke: bcolour });
        _elt_array[_elt_array.length] = new Array(elt, b);
        elt.mouseover(function(event) {
            DoSideBySideMouseOver(this, event);
        });
        
        elt = paper.path(pathstr).attr({ stroke: bcolour, 'stroke-width': 2 }); ;
        _elt_array[_elt_array.length] = new Array(elt, b);
        elt.mouseover(function(event) {
            DoSideBySideMouseOver(this, event );
        });        
    }
}

function GetSeriesName(idx) {
    if (idx == 0)
        return "Arts Event or Activity";
    if (idx == 1)
        return "Museum or Gallery";
    return "Public Library";                
}

function DoSideBySideMouseOver(elt, event ) {
    var i, pair, b;
    for (i = 0; i < _elt_array.length; i++) {
        pair = _elt_array[i];
        if (pair[0] == elt) {
            b = pair[1];
            if (b == null)
                Sayuser(pair[1]);
            else
                FloatingInfoBox(event.x + 20, event.y, b[0] + " " + b[2] + "% " + b[3] + "% " + b[4] + "%" );
            return;
        }
    }
    Sayuser("Looked through " + _elt_array.length + " elements");
}

function FloatingInfoBox(x, y, str) {
    $("#FloatingInfoBox").text(str).show().css({ top: y, left:x });
}
function FloatingInfoBoxHide() {
    $("#FloatingInfoBox").hide();
}
function CalcSideBySideY(val, min, max) {
    var ratio = ((1.0 * val) - min) / (max - min);
    return 400 - Math.floor(300 * ratio);
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