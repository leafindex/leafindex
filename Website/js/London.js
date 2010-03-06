var _column_index = 2;
var _borough_data;
var _do_distribution = false;

$(document).ready(function() {
    SetupTabLinks();
    SetupInputChanges();
    SelectTab();
    LoadKmlMaps();
    createData();
    FloatingInfoBoxHide();
    //AJCTest();
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
	$(".TabStop").click(function(){
		SelectTab(TabNameFromId(this.id),true);
		return false;
	});
}
function SetupInputChanges(){
	$("#Tabbing").find(":input:not(button,[type=submit])").change(function(){
		createData();
	});
}

function SelectTab(tabname, getdata) {
    _column_index = 2;
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
    _do_distribution = false;
    _column_index = idx;
    DrawMap();
}

function DrawMap() {
    var width, height, i, j, x, y, kmap, colour, msg, b;
    width = 1000;
    height = 500;

    if (_do_distribution && SelectedTabName() == "Crimes") {
        DrawDistribution();
        return;
    }
    if (SelectedTabName() == "Commuters") {
        _column_index = $("#ddlGoingTo").val();
    }
    RemoveCityIfNotSelected();

    $("#notepad").html("");
    FloatingInfoBoxHide();

    var paper = Raphael("notepad", width + 1, height + 1);
    for (i = 0; i < _kmlmap.length; i++) {
        kmap = _kmlmap[i];
        for (j = 3; j < kmap.length; j++) {
            elt = paper.path(kmap[j]);
            elt.attr("fill", GetBoroughColour(kmap[1]));

            b = GetBoroughItem(kmap[1]);
            if (b == null)
                msg = "";
            else
                msg = b[0] + " " + b[_column_index];
            elt.mouseover(function(str) { return function() { Sayuser(str); } } (msg));
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



function Sayuser(str) {
    $("#lblSayuser").text(str);
}

function DrawIsotype() {
    var width, height, w, h;
    w = 17;
    h = 25;
    width = 1000;
    height = _borough_data.length * (h + 2) + 20;
    $("#notepad").html("");
    FloatingInfoBoxHide();

    var paper = Raphael("notepad", width, height);

    var ia = new IsotypeArtist();
    ia.SetData(_borough_data);
    ia.SetPaper(paper, width, height);
    ia.SetMaxRepeats(20);
    ia.SetScale(5000);
    ia.SetTextWidth(200);
    ia.ShowKey(false);

    ia.AddIsotype(2, "./images/House0a.PNG", w, h);
    ia.AddIsotype(3, "./images/House1a.PNG", w, h);
    ia.AddIsotype(4, "./images/House2a.PNG", w, h);
    ia.AddIsotype(5, "./images/House3a.PNG", w, h);
    ia.AddIsotype(6, "./images/House4a.PNG", w, h);
    ia.Draw();

    Sayuser("Each house represents 5,000 households");
}

function DrawFlows( bidx ) {
    var width, height, i, j, x, y, kmap, colour, msg, b, fromboro, minval, maxval, linewidth;
    width = 1000;
    height = 500;

    $("#notepad").html("");
    FloatingInfoBoxHide();

    var paper = Raphael("notepad", width + 1, height + 1);
    for (i = 0; i < _kmlmap.length; i++) {
        kmap = _kmlmap[i];
        msg = i;
        if (i == bidx) {
            Sayuser("Commuter numbers from " + kmap[0]);
            colour = "yellow";
        }
        else {
            colour = "white";
        }
        for (j = 3; j < kmap.length; j++) {
            elt = paper.path(kmap[j]).attr( { fill:colour, stroke:"#ccc" } );
            elt.click(function(arg) { return function() { DrawFlows(arg); } } (i));
        }
        /*elt = paper.circle(kmap[2][0], kmap[2][1], 4).attr("fill", colour);
        elt.click(function(arg) { return function() { DrawFlows(arg); } } (i)); */
    }
    b = _borough_data[bidx];
    kmap = _kmlmap[bidx];
    x = kmap[2][0];
    y = kmap[2][1];


    minval = maxval = null;
    for (i = 0; i < _kmlmap.length; i++) {
        if (i == bidx)
            continue;
        if (minval == null || b[2 + i] < minval)
            minval = b[2 + i];
        if (maxval == null || b[2 + i] > maxval)
            maxval = b[2 + i];
    }
    
    for (i = 0; i < _kmlmap.length; i++) {
        if (i == bidx)
            continue;

        perc = 100 * (b[2 + i] - minval) / (maxval - minval);
        linewidth = 1 + ( 5 * perc / 100 );
        perc = 50 + perc / 2;
        colour = GetStrengthColour(Math.floor(perc + 0.5));
            
        kmap = _kmlmap[i];
        elt = paper.path(MakePathString(x, y, kmap[2][0], kmap[2][1])).attr({ stroke:colour, "stroke-width":linewidth } );
        msg = b[0] + " to " + kmap[0] + " : " + b[2 + i];
        elt.mouseover(function(arg) { return function() { Sayuser(arg); } }(msg));
    }
    for (i = 0; i < _kmlmap.length; i++) {
        if (i == bidx)
            continue;
        kmap = _kmlmap[i];
        /*paper.path(MakePathString(x, y, kmap[2][0], kmap[2][1]));*/
        elt = paper.text(kmap[2][0], kmap[2][1], b[2 + i]);
        elt.click(function(arg) { return function() { DrawFlows(arg); } } (i));
    }
}

var DIST_COL0 = 2;
var DIST_COL1 = 3;

function DrawDistribution() {
    var pop, stats0, stats1;
    var width, height, x, series_count, idx, series_width, bidx, b, pathstr, elt, bcolour, left_margin, right_margin, msg;

    RemoveCityIfNotSelected();
    pop = Population(_borough_data);
    stats0 = new pop(DIST_COL0);
    stats1 = new pop(DIST_COL1);
    _do_distribution = true;
    
    width = 1000;
    height = 500;
    
    left_margin = 80;   
    right_margin = 180; 

    $("#notepad").html("");
    FloatingInfoBoxHide();

    var paper = Raphael("notepad", width + 1, height + 1);
    MakeYAxisTicks(paper, stats1, 400, 100, left_margin );
    MakeXAxisTicks(paper, stats0, left_margin, width - right_margin, 400);

    for (bidx = 0; bidx < _borough_data.length; bidx++) {
        b = _borough_data[bidx];
        bcolour = GetColour(bidx);

        x = DistribPos(b[DIST_COL0], stats0, left_margin, width - right_margin);
        y = DistribPos(b[DIST_COL1], stats1, 400, 100);

        msg = b[0] + " " + b[DIST_COL0] + " " + b[DIST_COL1];
        elt = paper.circle(x, y, 3).attr({ stroke: bcolour });
        elt.mouseover(function(str) { return function(event) { FloatingInfoBox(event, str); } } (msg));

        elt = paper.text(width - right_margin / 2, 10 + bidx * (480 / _borough_data.length), b[0]).attr({ stroke: bcolour });
        elt.mouseover(function(str) { return function(event) { FloatingInfoBox(event, str); } } (msg));
    }
}

function RemoveCityIfNotSelected() {
    if (SelectedTabName() != "Crimes")
        return;
    var do_city = $("#chkIncludeCity").is(':checked');
    if (do_city == false) {
        for (bidx = 0; bidx < _borough_data.length; bidx++) {
            b = _borough_data[bidx];
            if (b[1] == "00AA") {
                _borough_data.splice(bidx, 1);
                return;
            }
        }
    }
}

function MakeXAxisTicks(paper, stats, zero_pos, max_pos, margin) {
    var pos0, pos1, pos, ticklen, gap;

    ticklen = 5;
    gap = 30;
    pos0 = DistribPos(stats.MinVal(), stats, zero_pos, max_pos);
    pos1 = DistribPos(stats.MaxVal(), stats, zero_pos, max_pos);
    paper.path(MakePathString(pos0, margin, pos1, margin ));

    paper.text(pos0, margin + ticklen + gap, stats.MinVal() + "\nMinimum");
    paper.text(pos1, margin + ticklen + gap, stats.MaxVal() + "\nMaximum");

    paper.path(MakePathString(pos0, margin + ticklen, pos0, margin));
    paper.path(MakePathString(pos1, margin + ticklen, pos1, margin));

    pos = DistribPos(stats.Median(), stats, zero_pos, max_pos);
    paper.path(MakePathString(pos, margin + ticklen, pos, margin));
    paper.text(pos, margin + ticklen + gap, stats.Median() + "\nMedian");

    pos = DistribPos(stats.LowerQuartile(), stats, zero_pos, max_pos);
    paper.path(MakePathString(pos, margin + ticklen, pos, margin));
    paper.text(pos, margin + ticklen + gap, stats.LowerQuartile() + "\nLower\nQuartile");

    pos = DistribPos(stats.UpperQuartile(), stats, zero_pos, max_pos);
    paper.path(MakePathString(pos, margin + ticklen, pos, margin));
    paper.text(pos, margin + ticklen + gap, stats.UpperQuartile() + "\nUpper\nQuartile");
}

function MakeYAxisTicks(paper, stats, zero_pos, max_pos, margin) {
    var pos0, pos1, pos, ticklen;

    ticklen = 5;
    pos0 = DistribPos(stats.MinVal(), stats, zero_pos, max_pos);
    pos1 = DistribPos(stats.MaxVal(), stats, zero_pos, max_pos);
    paper.path(MakePathString(margin, pos0, margin, pos1));

    paper.text(margin / 2, pos0, stats.MinVal() + " Min");
    paper.text(margin / 2, pos1, stats.MaxVal() + " Max");

    paper.path(MakePathString(margin - ticklen, pos0, margin, pos0));
    paper.path(MakePathString(margin - ticklen, pos1, margin, pos1));
    
    pos = DistribPos(stats.Median(), stats, zero_pos, max_pos);
    paper.path(MakePathString(margin - ticklen, pos, margin, pos));
    paper.text(margin / 2, pos, stats.Median() + " Med");

    pos = DistribPos(stats.LowerQuartile(), stats, zero_pos, max_pos);
    paper.path(MakePathString(margin - ticklen, pos, margin, pos));
    paper.text(margin / 2, pos, stats.LowerQuartile() + " LQ");

    pos = DistribPos(stats.UpperQuartile(), stats, zero_pos, max_pos);
    paper.path(MakePathString(margin - ticklen, pos, margin, pos));
    paper.text(margin / 2, pos, stats.UpperQuartile() + " UQ");
} 

function DistribPos(val, stats, zero_pos, max_pos) {
    var ratio = val / stats.MaxVal();   //(val - stats[MIN_IDX]) / (stats[MAX_IDX] - stats[MIN_IDX]);
    return Math.floor(zero_pos + ratio * (max_pos - zero_pos));
}

function ShowSideBySide(tabname) {
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
    var width, height, x, series_count, idx, series_width, bidx, b, pathstr, elt, bcolour, left_margin, right_margin, msg;
    width = 1000;
    height = 500;
    right_margin = 180;
    left_margin = 80;

    series_count = _borough_data[0].length - 2;
    series_width = (width - (left_margin + right_margin)) / (series_count - 1);

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

        msg = b[0] + " " + b[2] + "% " + b[3] + "% " + b[4] + "%"
        elt = paper.text(width - right_margin / 2, 10 + bidx * (480 / _borough_data.length), b[0]).attr({ stroke: bcolour });
        elt.mouseover(function(str) { return function(event) { FloatingInfoBox(event, str); } } (msg));

        elt = paper.path(pathstr).attr({ stroke: bcolour, 'stroke-width': 2 });
        elt.mouseover(function(str) { return function(event) { FloatingInfoBox(event, str); } } (msg ));
    }
}

function GetSeriesName(idx) {
    if (idx == 0)
        return "Arts Event or Activity";
    if (idx == 1)
        return "Museum or Gallery";
    return "Public Library";                
}

function FloatingInfoBox(event, str) {
    var x, y;
    x = event.pageX;
    y = event.pageY;
    if (x == null) {
        x = event.x;
        y = event.y;
    }
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

    "#FF8C00", "#bbbb00", "#777700",
    "#ff00ff", "#bb00bb", "#770077",
    "#00ffff", "#00bbbb", "#007777",

    "#8B4513", "#bbbb44", "#777744",
    "#ff44ff", "#bb44bb", "#774477",
    "#44ffff", "#44bbbb", "#447777",

    "#8E388E", "#bbbb88", "#777788",
    "#ff88ff", "#bb88bb", "#778877",
    "#88ffff", "#88bbbb", "#887777",

    "#ffbb77", "#bbff77", "#bb77ff", "#ff77bb", "#77ffbb", "#77bbff"
);

function GetColour(i) {
    var idx = i % colours.length;
    return colours[idx];
}

var Population=function(dataset){
  function ColumnToSortedArray(column_idx){
		var temparray = [];
		for (var i = 0; i < dataset.length; i++){
			temparray[i] = dataset[i][column_idx];
		}
		return temparray.sort(function(a, b) {return a - b;});
	}
	function PopPrototype(column_idx){
	    this._array = ColumnToSortedArray(column_idx);
	}
	PopPrototype.prototype = {
		Mean: function(){
			var total = 0;
			for (var i = 0; i < this._array.length; i++){
				total += this._array[i];
			}
			return total / this._array.length;
		},
		MinVal: function() {
				return this._array[0];
		},
		MaxVal: function() {
				return this._array[this._array.length - 1];
		},
		Median: function() {
				return this._array[Math.floor(this._array.length / 2)];
		},
		LowerQuartile: function() {
				return this._array[Math.floor(this._array.length / 4)];
		},
		UpperQuartile: function() {
				return this._array[Math.floor(this._array.length * 3 / 4)];
		}
	};
	return PopPrototype;
};
/*
function Population(dataset, column_idx) {
    var i, temparray, total;
    temparray = [];
    for (i = 0; i < dataset.length; i++)
        temparray[i] = dataset[i][column_idx];
    this._array = temparray.sort(function(a, b) { return a - b });
    total = 0;
    for (i = 0; i < this._array.length; i++)
        total += this._array[i];
    this._mean = total / this._array.length;  
        
    this.MinVal = function() {
        return this._array[0];
    }
    this.MaxVal = function() {
        return this._array[this._array.length - 1];
    }
    this.Median = function() {
        return this._array[Math.floor(this._array.length / 2)];
    }
    this.LowerQuartile = function() {
        return this._array[Math.floor(this._array.length / 4)];
    }
    this.UpperQuartile = function() {
        return this._array[Math.floor(this._array.length * 3 / 4)];
    }
    this.Mean = function() {
        return this._mean;
    }
}
*/