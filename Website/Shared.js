function InitNavMenu()
{
    var delim = "&nbsp;:&nbsp;";
    var str = delim + "<a href='ACOAssaults.htm'>ACOAssaults</a>" + delim
        + "<a href='ACOAssaults1.aspx'>ACOAssaults1</a>" + delim
        + "<a href='AJCTests.htm'>AJCTests</a>" + delim;
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