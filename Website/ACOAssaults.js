$(document).ready(function(){
    makeNavMenu();
    $(".userinput")
			.focus(HighlightOnFocus)
			.blur(HighlightOffOnBlur)
			.blur(ClearHelpText)
			.blur(DoSearch);
    $("#txtBorough").focus(function(){
			SetHelpText($("#hlpBorough").text());
		}).focus();
    $("#txtFrequency").focus(function(){
			SetHelpText($("#hlpFrequency").text());
		});
    Sayuser("Ready");
});

function DoSearch(){
	FetchResults($("#txtBorough").val(), $("#txtFrequency").val());
}

function FetchResults(borough, frequency){
	Sayuser("Searching for " + borough + "/" + frequency);
	var data = {borough: borough, frequency:frequency};
	var callback = function(xml){
		DisplayResults(xml);
	};
	$.post("ACOAssaults.ashx", data, callback ); 
}

function DisplayResults(xml){
	var msg = $("grail", xml).text() + " gave " + $("count", xml).text() + " results";
	var list = "";
	Sayuser(msg);
	$("ward",xml).each(function(){
			list += "<li>" + $(this).find("name").text() + " (" 
				+ $(this).find("total").text() + ")</li>";
	});
	$("#details").contents().replaceWith("<ol id='appended'>"+list+"</ol>");
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