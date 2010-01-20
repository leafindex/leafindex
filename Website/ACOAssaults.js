// JScript File

$(document).ready(function(){
    InitNavMenu();
    $(".userinput")
        .focus( HighlightOnFocus )
        .blur( HighlightOffOnBlur )
        .blur( ClearHelpText )
        .blur( DoSearch );
    
    $("#txtBorough").focus( function() { SetHelpText( $("#hlpBorough").text() ); } );
    $("#txtFrequency").focus( function() { SetHelpText( $("#hlpFrequency").text() ); } );
    
    $("#txtBorough").focus();
    Sayuser( "Ready" );
    
    $("#addone").click( function() { AddOne(); } );
});

function AddOne()
{
    DebugSayuser( "Add one" );
}

function DoSearch()
{
    FetchResults( $("#txtBorough").val(), $("#txtFrequency").val() );
}

function FetchResults( borough, frequency )
{
    var data, callback;
    data = {borough: borough, frequency:frequency };
    callback = function(xml) { DisplayResults( xml ); };
    $.post("ACOAssaults.ashx", data, callback ); 
    Sayuser( "Searching for " + borough + "/" + frequency );
}

function DisplayResults( xml )
{
    var msg = $("grail", xml).text() + " gave " + $("count", xml).text() + " results";
    var count = 0;
    Sayuser( msg );
    
    
    $("#details").text( "" );
    $("#details").append('<ol id="appended">');
    $("ward",xml).each(function(i) {
        $("#appended").append( "<li>" + $(this).find("name").text() 
            + " (" + $(this).find("total").text() + ")</li>" );
        count++;
    });
}

function ClearHelpText()
{
    SetHelpText( "" );
}

function SetHelpText( msg )
{
    $("#lblHelp").text( msg );
}

function HighlightOnFocus()
{
    $(this).addClass( "HasFocus" );
}
function HighlightOffOnBlur()
{
    $(this).removeClass( "HasFocus" );
}