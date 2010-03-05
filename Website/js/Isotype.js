function isotypeCars() {
    var width = 1200, height, demodata, i, w, h;
    ClearResults();
    Sayuser("isotypeCars");

    h = 25;
    demodata = MakeCarsData();
    height = demodata.length * (h + 2) + 20;

    $("#notepad").html("");
    var paper = Raphael("notepad", width, height);

    Sayuser(demodata.length + " items, columns = " + demodata[0].length);
    for (i = 0; i < demodata.length; i++)
        Sayuser(demodata[i]);

    var ia = new IsotypeArtist();
    ia.SetData(demodata);
    ia.SetPaper(paper, width, height);
    ia.SetMaxRepeats(20);
    ia.SetScale(5000);
    ia.SetTextWidth(200);
    ia.ShowKey(false);
    w = 17;
    
    ia.AddIsotype(1, "./images/House0a.PNG", w, h);
    ia.AddIsotype(2, "./images/House1a.PNG", w, h);
    ia.AddIsotype(3, "./images/House2a.PNG", w, h);
    ia.AddIsotype(4, "./images/House3a.PNG", w, h);
    ia.AddIsotype(5, "./images/House4a.PNG", w, h);
    ia.Draw();
}

function MakeCarsData() {
    var i = 0, result = [];

    result[i++] = new Array("City of London",2691,1417,184,29,17);
    result[i++] = new Array("Barking and Dagenham",25511,30279,9688,1441,354);
    result[i++] = new Array("Barnet",33925,57039,28680,5648,1652);
    result[i++] = new Array("Bexley",21217,41958,20986,4160,1130);
    result[i++] = new Array("Brent",37287,42606,16207,3135,756);
    result[i++] = new Array("Bromley",28950,57751,31154,6180,1831);
    result[i++] = new Array("Camden",50946,33084,6280,984,309);
    result[i++] = new Array("Croydon",41461,63286,27640,5066,1546);
    result[i++] = new Array("Ealing",37372,54259,21761,3742,889);
    result[i++] = new Array("Enfield",31496,50201,22814,4586,1301);
    result[i++] = new Array("Greenwich",37883,40160,12260,1976,509);
    result[i++] = new Array("Hackney",48219,31876,5018,689,240);
    result[i++] = new Array("Hammersmith and Fulham",36630,30461,7032,1058,257);
    result[i++] = new Array("Haringey",42820,38005,9622,1374,349);
    result[i++] = new Array("Harrow",17972,34900,20789,4286,1165);
    result[i++] = new Array("Havering",21374,42078,22131,4734,1405);
    result[i++] = new Array("Hillingdon",20972,43116,25690,5225,1640);
    result[i++] = new Array("Hounslow",24049,38920,16868,3316,841);
    result[i++] = new Array("Islington",47413,29194,4795,670,209);
    result[i++] = new Array("Kensington and Chelsea",39870,31041,6633,1184,418);
    result[i++] = new Array("Kingston upon Thames",14621,29049,14336,2669,751);
    result[i++] = new Array("Lambeth",60338,46080,10166,1446,417);
    result[i++] = new Array("Lewisham",45941,46679,12484,1831,477);
    result[i++] = new Array("Merton",23775,38143,13803,2517,646);
    result[i++] = new Array("Newham",44866,37811,7789,1089,266);
    result[i++] = new Array("Redbridge",24198,43047,20140,3856,1047);
    result[i++] = new Array("Richmond upon Thames",18047,37723,16871,2811,694);
    result[i++] = new Array("Southwark",54940,40947,8454,1120,345);
    result[i++] = new Array("Sutton",17790,35264,18470,3750,1128);
    result[i++] = new Array("Tower Hamlets",44582,28997,4250,545,156);
    result[i++] = new Array("Waltham Forest",34975,39562,12512,2177,562);
    result[i++] = new Array("Wandsworth",47066,51440,14437,2164,546);
    result[i++] = new Array("Westminster", 51452, 32108, 6241, 1012, 359);
    return result;
}



function isotypePeople() {
    var width = 1000, height = 600, demodata, i;
    ClearResults();
    Sayuser("isotypeComplete");

    demodata = MakePeopleData();

    $("#notepad").html("");
    var paper = Raphael("notepad", width, height);

    Sayuser(demodata.length + " items, columns = " + demodata[0].length);
    for (i = 0; i < demodata.length; i++)
        Sayuser(demodata[i]);

    var ia = new IsotypeArtist();
    ia.SetData(demodata);
    ia.SetPaper(paper, width, height);
    ia.SetMaxRepeats(20);
    ia.ShowKey(false);
    ia.AddIsotype(1, "GMDH02_Man.PNG", 32, 95);
    ia.AddIsotype(2, "GMDH02_Woman.PNG", 27, 95);
    ia.Draw();
}

function isotypeComplete() {
    var width = 400, height = 250, demodata, i;
    ClearResults();
    Sayuser("isotypeComplete");

    demodata = MakeDemoData();
    
    $("#notepad").html("");
    var paper = Raphael("notepad", width, height);

    Sayuser(demodata.length + " items, columns = " + demodata[0].length);
    for( i = 0; i < demodata.length; i++ )
        Sayuser(demodata[i]);

    var ia = new IsotypeArtist();
    ia.SetData(demodata);
    ia.SetPaper(paper, width, height);
    ia.AddIsotype(1, "RedBlob.png", 11, 11);
    ia.AddIsotype(2, "GreenBlob.png", 11, 11);
    ia.Draw();
}

function MakePeopleData() {
    var i = 0, result = [];

    result[i++] = new Array("Whitespace", 17, 2);
    result[i++] = new Array("MCS", 4, 2 );
    result[i++] = new Array("MPS", 3, 3);

    return result;
}

function MakeDemoData() {
    var i = 0, result = [];

    result[i++] = new Array("Gloucester", 2500, 12);
    result[i++] = new Array("Swindon", 4120, 14);
    result[i++] = new Array("Stroud", 1030, 19);
    result[i++] = new Array("Didcot Parkway", 921, 12);
    result[i++] = new Array("Reading", 3513, 19);
    
    return result;
}

function IsotypeArtist() {
    this._data = [];
    this._paper = null;
    this._width = 0;
    this._height = 0;
    this._isotypearray = [];
    this._columnarray = [];
    this._maxrepeats = 10;
    this._labelcolumn = 0;
    this._scale = 0;
    this._textwidth = 120;
    this._showkey = true;
    this.SetData = function( data ) {
        this._data = data;
    }
    this.SetPaper = function(paper, width, height) {
        this._paper = paper;
        this._width = width;
        this._height = height;
    }
    this.SetMaxRepeats = function(maxrepeats) {
        this._maxrepeats = maxrepeats;
    }
    this.SetScale = function(scale) {
        this._scale = scale;
    }
    this.SetTextWidth = function(textwidth) {
        this._textwidth = textwidth;
    }
    this.ShowKey = function(showkey) {
        this._showkey = showkey;
    }
    this.AddIsotype = function(columnidx, imgurl, width, height) {
        this._isotypearray[this._isotypearray.length] = new Isotype(imgurl, width, height);
        this._columnarray[this._columnarray.length] = columnidx;
    }
    this.Draw = function() {
        var i, j, itm, isot, repeats, x, y, height, str, scalearray = this.ChooseScales();
        height = this.GetTallest();
        y = 10;
        for (i = 0; i < this._data.length; i++) {

            itm = this._data[i];
            this._paper.text(this._textwidth / 2, y + height / 2, itm[this._labelcolumn]);
            x = this._textwidth;

            for (j = 0; j < this._columnarray.length; j++) {
                isot = this._isotypearray[j];
                repeats = itm[this._columnarray[j]] / scalearray[j];
                isot.DrawHorizontal(this._paper, repeats, x, y);
                x += isot.WidthRequired(repeats) + 10;
            }
            // this._paper.text(textwidth / 2, y + height / 2, itm[this._labelcolumn]);
            y += height;
        }
        if (this._showkey) {
            y += height;
            for (j = 0; j < this._columnarray.length; j++) {
                str = "One blob = " + scalearray[j];
                this._paper.text(this._textwidth / 2, y + height / 2, str);
                x = this._textwidth;
                isot = this._isotypearray[j];
                isot.DrawHorizontal(this._paper, 1, x, y);
                y += height;
            }
        }
    }
    this.GetTallest = function() {
        var i, result = 0;
        for (i = 0; i < this._isotypearray.length; i++)
            if (this._isotypearray[i].Height() > result)
            result = this._isotypearray[i].Height();
        return result;
    }
    this.ChooseScales = function() {
        var i, result = [];
        for (i = 0; i < this._columnarray.length; i++) {
            if (this._scale > 0)
                result[i] = this._scale;
            else
                result[i] = this.ChooseColumnScale(this._columnarray[i]);
        }
        return result;
    }
    this.ChooseColumnScale = function(idx) {
        var i, itm, result, max = 0, msg = ": ";
        for (i = 0; i < this._data.length; i++) {
            itm = this._data[i];
            msg += itm[idx] + " : ";
            if (itm[idx] > max)
                max = itm[idx];
        }
        result = this.ChooseScaleForMax(max);
        return result;
    }
    this.ChooseScaleForMax = function(max) {
        var result = 1, i, multipliers = new Array(2, 2.5, 2);
        i = 0;
        while (result * this._maxrepeats < max) {
            result *= multipliers[i++];
            if (i >= multipliers.length)
                i = 0;
        }
        return result;
    }
} 


function drawIsotype(repeats) {
    var isot;
    ClearResults();
    Sayuser("Isotype");
    Sayuser( "http://gerdarntz.org/isotype/people" );
    var width = 400, height = 250, x;

    $("#notepad").html("");
    var paper = Raphael("notepad", width, height);

    for (x = 0; x <= width; x += 20) {
        paper.path(MakePathString(0, 0, x, height));
    }
    isot = new IsotypeLibrary().GetMan();
    isot.DrawGrid(paper, repeats, isot._width * 6, 10, 10); 
}

function MakePathString(x0, y0, x1, y1) {
    return "M" + x0 + " " + y0 + "L" + x1 + " " + y1;
}

function IsotypeLibrary() {
    this.GetMan = function() {
        return new Isotype("gmdh02_man.png", 32, 95);
    }
}

function Isotype(imgurl, width, height) {
    this._imgurl = imgurl;
    this._width = width;
    this._height = height;
    this._xpadding = 2;
    this._ypadding = 2;
    this.Width = function() {
        return this._width + this._xpadding;
    }
    this.Height = function() {
        return this._height + this._ypadding;
    }
    this.SetPadding = function(xpadding, ypadding) {
        this._xpadding = xpadding;
        this._ypadding = ypadding;
    }
    this.WidthRequired = function(repeats) {
        return this.Width() * repeats;
    }
    this.HeightRequired = function(repeats, allowed_width) {
        var across = Math.floor(allowed_width / this.Width());
        var result = 1;
        if (repeats <= 0)
            return 0;
        while (result * across < repeats)
            result++;
        return result * this.Height();
    }
    this.DrawHorizontal = function(paper, repeats, x, y) {
        var cliprect;
        while (repeats >= 1) {
            paper.image(this._imgurl, x, y, this._width, this._height);
            x += this.Width();
            repeats -= 1;
        }
        if (repeats > 0) {
            cliprect = x + " " + y + " " + (this._width * repeats) + " " + this._height;
            paper.image(this._imgurl, x, y, this._width, this._height).attr("clip-rect", cliprect);
        }
    }
    this.DrawGrid = function(paper, repeats, allowed_width, x0, y0) {
        var cliprect, x, y;
        x = x0;
        y = y0;
        while (repeats >= 1) {
            paper.image(this._imgurl, x, y, this._width, this._height);
            x += this.Width();
            if (x >= allowed_width) {
                x = x0;
                y += this.Height();
            }
            repeats -= 1;
        }
        if (repeats > 0) {
            cliprect = x + " " + y + " " + (this._width * repeats) + " " + this._height;
            paper.image(this._imgurl, x, y, this._width, this._height).attr("clip-rect", cliprect);
        }
    }    
}