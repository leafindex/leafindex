function InitNavMenu()
{
    var delim = "&nbsp;:&nbsp;";
    var str = delim + "<a href='ACOAssaults.htm'>ACOAssaults</a>" + delim;
    $("#navmenu").html( str );
}

function Sayuser( msg )
{
    $("#lblSayuser").text( msg );
}
function DebugSayuser( msg )
{
    $("#lblDebugSayuser").text( msg );
}