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

            DoBlob(ctx, 380, 210, "Haringey");
            DoBlob(ctx, 380, 240, "Islington");
            DoBlob(ctx, 440, 200, "Waltham Forest");
            DoBlob(ctx, 410, 250, "Hackney");

            DoBlob(ctx, 500, 210, "Redbridge");
            DoBlob(ctx, 600, 220, "Havering");
            DoBlob(ctx, 530, 250, "Barking");
            DoBlob(ctx, 460, 260, "Newham");
            DoBlob(ctx, 440, 270, "Tower Hamlets");

            DoBlob(ctx, 530, 340, "Bexley");
            DoBlob(ctx, 480, 340, "Greenwich");
            DoBlob(ctx, 490, 410, "Bromley");
            DoBlob(ctx, 440, 340, "Lewisham");

            DoBlob(ctx, 410, 320, "Southwark");
            DoBlob(ctx, 400, 400, "Croydon");
            DoBlob(ctx, 380, 340, "Lambeth");

            DoBlob(ctx, 340, 420, "Sutton");

            DoBlob(ctx, 340, 380, "Merton");
            DoBlob(ctx, 340, 340, "Wandsworth");
            DoBlob(ctx, 300, 380, "Kingston");
            DoBlob(ctx, 250, 370, "Richmond");

            DoBlob(ctx, 330, 290, "Kensington");
            DoBlob(ctx, 220, 330, "Hounslow");
            DoBlob(ctx, 310, 280, "Hammersmith");
            DoBlob(ctx, 240, 280, "Ealing");
            DoBlob(ctx, 190, 250, "Hillingdon");
            
            // DoBlob(ctx, 400, 280, "City");
        }
        img.src = 'images/boroughmap.gif';
    }
}

function DoBlob(ctx, mapx, mapy, borough) {
    //ctx.fillStyle = 'rgb(102,204,0)';
    var blobx, bloby;

    if (_blobno < 10) {
        blobx = 40 + _blobno * 80;
        bloby = 40;
    }
    else if( _blobno < 17 ) {
        blobx = 760;
        bloby = 40 + (_blobno - 9) * 80;
    }
    else if( _blobno < 26 ) {
        blobx = 760 - (_blobno - 16) * 80;
        bloby = 600;
    }
    else
    {
        blobx = 40;
        bloby = 600 - ( _blobno - 25 ) * 80;
    }
    SetStyleForBlob(ctx);
    //ctx.strokeStyle = "#9CFF00";
    ctx.beginPath();
    ctx.moveTo(mapx, mapy);
    ctx.lineTo(blobx, bloby);
    ctx.stroke();

    // arc(x, y, radius, startAngle, endAngle, anticlockwise)
    ctx.beginPath();
    var radius = 10 + Math.floor(Math.random() * 30);
    ctx.arc(blobx, bloby, radius, 0, Math.PI * 2, true);
    ctx.fill();

    ctx.arc(mapx, mapy, 2, 0, Math.PI * 2, true);
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
