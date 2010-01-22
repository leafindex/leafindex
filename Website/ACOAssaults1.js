$(function(){
	$("#boroughSearch")
		.liveUpdate("#boroughTable")
		.focus();
	setupTable("#boroughTable");
});
function setupTable(t){
	t=$(t);
	var td=t.find("thead tr th");
	td.filter(".number").each(function(){
		t.find("tbody tr td:nth-child("+(td.index(this)+1)+")").addClass("number");
	});
}
$.fn.liveUpdate=function(list){
	list=$(list);
	if(list.length){
		var rows=list.find("tbody tr");
		var	cache=rows.find("td:first").map(function(){
				return $(this).text().toLowerCase();
			});
		this
			.keyup(filter).keyup()
			.parents("form").submit(function(){
				return false;
			});
	}
	return this;
		
	function filter(){
		var term=$.trim($(this).val().toLowerCase()),scores=[];
		if(!term){
			rows.show();
		}else{
			rows.hide();
			cache.each(function(i){
				if(this.indexOf(term)>-1){
					jQuery(rows[i]).show();
				}
			});
		}
	}
};