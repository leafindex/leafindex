function makeNavMenu(){
	$("<div id='navmenu'/>")
		.html("<a href='ACOAssaults.htm'>ACOAssaults</a>"+
			"<a href='ACOAssaults1.aspx'>ACOAssaults1</a>"+
			"<a href='AJCTests.htm'>AJCTests</a>")
		.prependTo("body")
}
function Sayuser(msg){
	$("#lblSayuser").text(msg);
}
function DebugSayuser(msg){
	$("#lblDebugSayuser").text(msg);
}