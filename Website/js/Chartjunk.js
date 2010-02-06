$(function() {
    var i, str, f;

    LoadData();
    str = "<div id='ft' class='filepicker'>" + _data.length + " files</div>";
    str += "<div class='subhead'>With Styles</div>";
    for (i = 0; i < _data.length; i++) {
        f = _data[i];
        if (f[2] > 0)
            str += "<div id='f" + i + "' class='filepicker'>" + f[0] + "</div>";
    }
    str += "<div class='subhead'>No Styles</div>";
    for (i = 0; i < _data.length; i++) {
        f = _data[i];
        if (f[2] == 0)
            str += "<div id='f" + i + "' class='filepicker'>" + f[0] + "</div>";
    }
    
    $("#filelist").html(str);
    $(".filepicker").mouseenter(function() { FileClick(this); });
    FileClickById("ft");
});

google.load('visualization', '1', { packages: ['gauge'] });
function drawChart(val) {
    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Label');
    data.addColumn('number', 'Value');
    data.addRows(1);
    data.setValue(0, 0, 'Styles');
    data.setValue(0, 1, val);

    var chart = new google.visualization.Gauge(document.getElementById('chart_div'));
    var options = { width: 400, height: 400, redFrom: 0, redTo: 20,
        yellowFrom: 20, yellowTo: 40, minorTicks: 4,
        majorTicks: new Array("0", "20", "40", "60", "80", "100")
    };
    chart.draw(data, options);
}

function FileClick(elt) {
    FileClickById(elt.id);
    }
function FileClickById( id )
{  
    $(".filepicker").removeClass("FileSelected");
    $("#" + id).addClass("FileSelected");
    if ( id == 'ft')
        drawChart(_total_perc);
    else {
        var f, str = id.substr(1);
        var x = 1 * str;
        
        if (x >= 0 && x < _data.length) {
            f = _data[x];
            drawChart(f[1]);
        }
    }
}
