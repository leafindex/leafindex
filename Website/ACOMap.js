$(document).ready(function() {
    makeNavMenu();
    draw();
});

var _blobno;

function draw() {
    var canvas = document.getElementById("canvas");
    if (canvas.getContext) {
        var ctx = canvas.getContext("2d");

        var img = new Image();
        var left_offset = ( 800 - 500 ) / 2;
        var top_offset = (600 - 398) / 2;
        var blobx, bloby;
        
        img.onload = function() {
            ctx.drawImage(img, left_offset, top_offset);
            _blobno = 0;

            DoBlob(ctx, 230, 210, "Harrow");
            DoBlob(ctx, 280, 240, "Brent");

            DoBlob(ctx, 350, 280, "Westminster");

            DoBlob(ctx, 330, 190, "Barnet");
            DoBlob(ctx, 350, 240, "Camden");
            DoBlob(ctx, 380, 160, "Enfield");
            DoBlob(ctx, 380, 340, "Lambeth");
            DoBlob(ctx, 410, 320, "Southwark");
        }
        img.src = 'images/boroughmap.gif';
    }
}

function DoBlob(ctx, mapx, mapy, borough) {
    //ctx.fillStyle = 'rgb(102,204,0)';
    var blobx, bloby;

    blobx = 40 + _blobno * 80;
    bloby = 40;
    
    SetStyleForBlob(ctx);
    //ctx.strokeStyle = "#9CFF00";
    ctx.beginPath();
    ctx.moveTo(mapx, mapy);
    ctx.lineTo(blobx, bloby);
    ctx.stroke();

    // arc(x, y, radius, startAngle, endAngle, anticlockwise)
    ctx.beginPath();
    ctx.arc(blobx, bloby, 25, 0, Math.PI * 2, true);
    ctx.fill();    
    _blobno++;    
}

var colours = new Array( "#ff0000", "#00ff00", "#0000ff",
    "#ffff00", "#ff00ff", "#00ffff",
    "#ff0099", "#ff9900", "#99ff00", "#00ff99", "#9900ff", "#0099ff" );

function SetStyleForBlob(ctx) {
    var idx = _blobno % colours.length;
    ctx.strokeStyle = colours[idx];
    ctx.fillStyle = colours[idx];
    //ctx.strokeStyle = "#9CFF00";
    
}
