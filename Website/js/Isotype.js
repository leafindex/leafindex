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