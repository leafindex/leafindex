function makeNavMenu(){
	$("<div id='navmenu'/>")
		.html("<a href='ACOAssaults.htm'>ACOAssaults</a>"+
			"<a href='ACOAssaults1.aspx'>ACOAssaults1</a>" +
			"<a href='ACOLinq.aspx'>ACOLinq</a>" +
			"<a href='ACOMap.aspx'>ACOMap</a>" +
			"<a href='ACOMapR.aspx'>ACOMapR</a>" +
			"<a href='ACOMapR.aspx'>ACOMapR</a>" +
			"<a href='PopulationCrash.htm'>Population Crash</a>")
		.prependTo("body")
}
function Sayuser(msg){
	$("#lblSayuser").text(msg);
}
function DebugSayuser(msg){
	$("#lblDebugSayuser").text(msg);
}
function ShowProgress(onoff) {
    if (onoff)
        $("#lblProgress").html("<img src='images/progress.gif' width='13' height='13'/>");
    else
        $("#lblProgress").html("");
}

