function makeNavMenu() {
	var m=$("<div id='navmenu'/>")
		.html("<a href='/'>Home</a>"
			+ "<a class='dropdownMenuing assaults' href='#' id='assaults'>Ambulance Call Outs To Assaults</a>"
			+ "<a href='PopulationCrash.htm'>Population Crash</a>"
			+ "<a href='http://leafindex.blogspot.com'>Blog</a>");
	makeDropdownMenu(
		m.find(".dropdownMenuing").filter(".assaults"),
			"<a href='ACOAssaults1.aspx'>Summary</a>","<a href='ACOMapR.aspx'>Map</a>","<a href='ACOAssaults.htm'>Query</a>");
	m.prependTo("body");
  $("<div id='bottombar'>").html( "&copy;2010 Chris Heywood / Jonathan Clarke" ).appendTo( "body" );
}
function makeDropdownMenu(menuLink){
	var args=arguments;
	menuLink
		.wrapInner("<span class='dropdownMenuLink'/>")
		.append("<span class='dropdownMenuThing'>▼</span>")
		.click(function(){
			dropdownMenu.apply(this,args);
			return false;
		});
}
function dropdownMenu(menuLink){
	var i=arguments.length;
	if(i<=1){
		return;
	}
	var id=menuLink.attr("id");
	var d=$("#dropdownMenu"+id);
	if(d.length){
		d.remove();
		return;
	}
	d=$("<div id=\"dropdownMenu"+id+"\"/>")
		.addClass("dropdownMenu")
		.hide();
	for(var j=1;j<i;j++){
    d.append(arguments[j]);
  }
  var o=menuLink.offset();
  d.appendTo("body")
  d
		.css({"left":(o.left+menuLink.outerWidth()-d.outerWidth())+"px","top":(o.top+menuLink.outerHeight()+5)+"px"})
		.show();
	$(document).bind("mousedown.dropdownMenu",function(e){
		var t=$(e.target);
		if(!t.is(".dropdownMenuing,.dropdownMenuLink,.dropdownMenuThing")){
			if(!t.closest("div.dropdownMenu").length){
				$("div.dropdownMenu").remove();
				$(document).unbind(".dropdownMenu");
			}
		}
	});
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
$(function(){
	makeNavMenu();
});
