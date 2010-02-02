$(document).ready(function() {
    $(".userinput")
			.focus(HighlightOnFocus)
			.blur(HighlightOffOnBlur)
			.blur(ClearHelpText)
			.blur(DoSearch);
    $("#txtBorough").focus(function() {
        SetHelpText($("#hlpBorough").text());
    }).focus();
    $("#txtFrequency").focus(function() {
        SetHelpText($("#hlpFrequency").text());
    });
    Sayuser("Ready");
});

function DoSearch(){
	FetchResults($("#txtBorough").val(), $("#txtFrequency").val());
}

function FetchResults(borough, frequency){
    Sayuser("Searching for " + borough + "/" + frequency);
    ShowProgress(true);
	var data = {borough: borough, frequency:frequency};
	var callback = function(xml){
		DisplayResults(xml);
	};
	$.post("/handler/ACOAssaults.ashx", data, callback ); 
}

function DisplayResults(xml) {
    xml = DataUtilities.processXML(xml);
    var msg = $(xml).find('grail').text() + " gave " + $(xml).find('count').text() + " results.";
    var list = "", count = 0;
    $(xml).find('ward').each(function() {
        count = count + 1;
        list += "<li>" + $(this).find("name").text() + " ("
				+ $(this).find("total").text() + ")</li>";
    });
    $("#lblList").html("<ol id='appended'>" + list + "</ol>");
    Sayuser( msg );
    ShowProgress(false);
    //$("#txtFrequency").focus();
}

function ClearHelpText(){
	SetHelpText("");
}
function SetHelpText(msg){
	$("#lblHelp").text(msg);
}
function HighlightOnFocus(){
	$(this).addClass("HasFocus");
}
function HighlightOffOnBlur(){
	$(this).removeClass("HasFocus");
}

// see Justin's comment in
// http://think2loud.com/using-jquery-and-xml-to-populate-a-drop-down-box/
var DataUtilities = (function() 
{
    var self = { "processXML": function(xml) 
    {
        if (!jQuery.support.htmlSerialize) 
        {
            //If IE 6+
            var xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
            xmlDoc.loadXML(xml);
            xml = xmlDoc;
        }
        return xml;
    }
};
return self;
})();

