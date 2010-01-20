// JScript File

function InitNavMenu()
{
    var delim = "&nbsp;-&nbsp;";
    $("#navmenu").append( delim );
    $("#navmenu").append( "<a href='HTMLPage.htm'>Menu</a>" + delim );
    $("#navmenu").append( "<a href='ACOAssaults.htm'>ACOAssaults</a>" + delim );
    $("#navmenu").append( "<a href='Lists.htm'>Lists</a>" + delim );
}

function Sayuser( msg )
{
    $("#lblSayuser").text( msg );
}
function DebugSayuser( msg )
{
    $("#lblDebugSayuser").text( msg );
}