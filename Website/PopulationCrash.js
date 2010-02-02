$(document).ready(function() {
    LoadData();
    ShowData();
});

var _country_data;

function LoadData() {
    _country_data = new Array();
    
AddPoint("Virgin Islands",-31.5425148169457);
AddPoint("Bulgaria",-28.5344275580049);
AddPoint("Guyana",-26.766234140942);
AddPoint("Niue",-25.1184834123223);
AddPoint("Belarus", -24.4825190482878);
AddPoint("Moldova",-24.1330248929792);
AddPoint("Ukraine",-23.3694715820601);
AddPoint("Georgia",-23.3120040147096);
AddPoint("Lithuania",-21.5360985252911);
AddPoint("Bosnia and Herzegovina",-20.1360969728764);
AddPoint("Japan",-20.0522247337871);
AddPoint("Romania",-18.7815309524492);
//AddPoint("Eastern Europe",-17.9561462397953);
AddPoint("Russia",-17.5878296101754);
AddPoint("Latvia",-17.5614685408574);
AddPoint("Poland",-15.9175778479369);
AddPoint("Germany",-14.1939363711109);
AddPoint("Croatia",-13.3846417910921);
AddPoint("Cuba",-13.2043130331715);
AddPoint("Greenland",-12.9295750065451);
AddPoint("Hungary",-10.5945943133071);
    
}

function AddPoint(country, change) {
    var rounded = Math.round(change * 100) / 100.0;
    _country_data[_country_data.length] = new Array(country, rounded);
}

function ShowData() {
    var i, c;
    var tbl = "<table><tr><th>Country</th><th>%age change</th>";
    for (i = 0; i < _country_data.length; i++) {
        c = _country_data[i];
        tbl += "<tr><td>" + c[0] + "</td><td>" + c[1] + "</td></tr>";
        ShowFlag(c);
    }
    tbl += "</table>";
    $("#RawData").html(tbl);
}

function ShowFlag(c) {
    var width = 122, height = 62, fc, rwidth, elt;
    fc = "FlagContainer" + TrimmedName( c );
    $("#Flags").append("<div class='FlagContainer' id='" + fc + "'></div>");
    
    var paper = Raphael(fc, width, height);
    var src = "images/flags/" + TrimmedName(c) + ".gif";
    var left_offset = 0;
    var top_offset = 0;
    var elt = paper.image(src, left_offset, top_offset, width, height);

    rwidth = width * (0.0 - c[1]) / 100.0;
    elt = paper.rect(0, 0, rwidth, height);
    elt.attr("fill", "#000000");
    elt.attr("opacity", 0.5);
}

function TrimmedName(c) {
    return c[0].replace(/ /gi, "");
}