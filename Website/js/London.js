var _elt_array;
var _column_index = 2;
var _borough_data;

$(document).ready(function() {
    _borough_data = new Array();
    SetupTabLinks();
    SelectTab();
    // FillBoroughData();
    GetAjaxData();
    LoadKmlMaps();
    DrawMap();
});
function GetAjaxData(){
	$.ajax({
		url:"/handler/LondonData.ashx",
		cache:false,
		data:$(":not(#__VIEWSTATE,#__EVENTVALIDATION)","#form1").serialize(),
		dataType:"json",
		success:function(data){
			alert("good");
			$.each(data,function(i,item){
				console.log(item(0));
				//access item.? & item.? etc
			});
		},
		error:function(r){
			alert("error: " + r.status + ": " + r.statusText);
//			alert(r.responseText);
		}
	});
}
function TabNameFromId(id){
	return id.substring(3);
}
function FirstTabName(){
	return TabNameFromId($(".TabStop:first","#Tabbing").attr("id"));
}
function SetupTabLinks(){
	$("#TabArts,#TabBegging,#TabCrimes").click(function(){
		ClickOnTab(TabNameFromId(this.id));
		return false;
	});
}

function ClickOnTab(tabname) {
    $("#hdnTabSelected").val(tabname);
    $("#Button1").click();
}

function SelectTab(tabname) {
		if(!tabname){
			tabname=$("#hdnTabSelected").val();
			if(tabname==="NotSet"){
				tabname=FirstTabName();
			}
		}
    $("#Tab" + tabname).addClass("TabStopSelected").show();
    $("#Options" + tabname).show();
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

    if (_borough_data == null)
        return 0;
    for (i = 0; i < _borough_data.length; i++) {
        itm = _borough_data[i];
        if (result == null || itm[_column_index] < result)
            result = itm[_column_index];
    }
    return result;
}
function GetMaxValue() {
    var i, itm, result = null;
    if (_borough_data == null)
        return 100;
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