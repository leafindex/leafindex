function sortableTable(table){
	$('thead th',table).each(function(column){
    var getSortKey;
		if($(this).hasClass("number")){
			getSortKey=function(cell){
				var key=parseFloat(cell.text().replace(/[^\d.]*/g,""));
				return isNaN(key)?0:key;
			};
		}
		else{
			if($(this).hasClass("date")){
	      getSortKey=function(cell) {
	        return Date.parse(cell.text());
				};
			}
			else{
				getSortKey=function(cell){
					console.log(cell.text());
					return cell.text().toUpperCase();
				};
			}
		}
		$(this).click(function(){
			var t=$(this);
			var direction=(t.data("sort")==1)?-1:1;
			t.data("sort",direction);
      var rows=table.find("tbody > tr").get();
      $.each(rows,function(i,row){
	      row.sortKey=getSortKey($(row).children("td:eq("+column+")"));
      });
      rows.sort(function(a,b){
				return (a.sortKey<b.sortKey)?-direction:((a.sortKey>b.sortKey)?direction:0);
      });
      $.each(rows,function(i,row){
	      table.children("tbody").append(row);
        row.sortKey=null;
      });
    });
	});
}
function setupTable(t){
	t=$(t);
	var td=t.find("thead > tr > th");
	td.filter(".number").each(function(){
		t.find("tbody > tr > td:nth-child("+(td.index(this)+1)+")").addClass("number");
	});
	sortableTable(t);
}
$(function(){
	InitNavMenu();
	$("#boroughSearch")
		.tableSearch("#boroughTable")
		.focus();
	setupTable("#boroughTable");
});
$.fn.tableSearch=function(table){
	var rows,cache;
	function filter(){
		var term=$.trim($(this).val().toLowerCase());
		if(!term){
			rows.show();
		}
		else{
			rows.hide();
			cache.each(function(i){
				if(this.indexOf(term)>-1){
					$(rows[i]).show();
				}
			});
		}
	}
	table=$(table);
	if(table.length){
		rows=table.find("tbody > tr");
		cache=rows.find("td:first").map(function(){
				return $(this).text().toLowerCase();
			});
		this
			.keyup(filter).keyup()
			.parents("form").submit(function(){
				return false;
			});
	}
	return this;
};
