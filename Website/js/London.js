var _elt_array;
var _column_index = 2;
var _borough_data = [];
var _tab_selected = 'Arts';

$(document).ready(function() {
    SetupTabLinks();
    SelectTab(_tab_selected);
    LoadKmlMaps();
    GetAjaxData();
    //Is AJAX nippy enough to work with ddl.change() = autopostback?
    //$(".DisplayButton").click(function() { GetAjaxData(); });
    $(".DisplayButton").hide();
    $(".OptionDdl").change(function() { GetAjaxData(); });
});
function GetAjaxData(){
    $.ajax({
        url: "./handler/LondonData.ashx?" + GetAjaxArgs(),
        cache: false,
        data: $(":not(#__VIEWSTATE,#__EVENTVALIDATION)", "#form1").serialize(),
        dataType: "json",
        success: function(data) {
            _borough_data = []; // todo - hold each set as used
            $.each(data, function(i, v) {
                if (v && typeof v === 'object') {
                    var a = []
                    for (var j in v) {
                        if (v.hasOwnProperty(j)) {
                            a.push(v[j]);
                        }
                    }
                    _borough_data.push(a);
                }
            });
            DrawMap();
        },
        error: function(r) {
            alert("error: " + r.status + ": " + r.statusText);
        }
    });
}

function GetAjaxArgs() {
    var i, j, msg, result, result_array = [];
    if (_tab_selected == "Arts") 
        result_array = new Array( _tab_selected, $("#ddlArtType").val() );
    if (_tab_selected == "Begging")            
        result_array = new Array( _tab_selected, $("#ddlBeggingYear").val() );
    if (_tab_selected == "Crimes")            
        result_array = new Array( _tab_selected, $("#ddlCrime").val(), $("#ddlYear").val() );
    
    msg = result = "";
    for( i = 0; i < result_array.length; i++ ) {
        if( i > 0 ) {
            msg += " / ";
            result += "&";
        }
        j = i + 1;
        result += "arg" + j + "=" + encodeURI(result_array[i]);
        msg += result_array[i];
    }
    Sayuser( msg );
    return result;
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
    _tab_selected = tabname;
    SelectTab(tabname);
    Sayuser("Tab now " + _tab_selected);
    GetAjaxData();
//    $("#hdnTabSelected").val(tabname);
//    $("#Button1").click();
}

function SelectTab(tabname) {
//		if(!tabname){
//			tabname=$("#hdnTabSelected").val();
//			if(tabname==="NotSet"){
//				tabname=FirstTabName();
//			}
    //		}
    $(".TabStop").removeClass("TabStopSelected");
    $(".TabOptions").hide();
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