$.fn.sortableTable=function(){
	return this.each(function(){
		var table=$(this);
		$('thead th',table)
			.addClass("sortable hover")
			.wrapInner("<a href='#'/>")
			.each(function(column){
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
					return false;
				});
			});
	});
};
$.fn.tableSearch=function(table){
	var rows,cache;
	function subRowsHide(r){
		if(r.length){
			r.removeClass("highlight").hide();
		}
	}
	function subRowsShow(r){
		//r.filter
	}
	function filter(){
		var subRows=table.find("tbody > tr.subTable:visible");
		subRowsHide(subRows);
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
		subRowsShow(subRows);
	}
	table=$(table);
	if(table.length){
		table.mouseup(function(){
			alert(rows.length);
		});
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
function htmlEncode(s){
	if(!s){
		return "&nbsp;";
	}
	var s1="",s2=s.toString(),i=s2.length;
	for(var j=0;j<i;j++){
		var c=s2.charAt(j);
		if(c<" "||c>"~"){
			c="&#"+c.charCodeAt()+";";
		}
		else {
			switch(c){
				case "&":
					c="&amp;";break;
				case "<":
					c="&lt;";break;
				case ">":
					c="&gt;";break;
				case "\"":
					c="&quot;";break;
				case "'":
					c="&#39;";break;
			}
		}
		s1+=c;
	}
	return s1;
}
function wardsDrill(cell){
	var tr=cell.parent();
	tr.toggleClass("highlight");
	var wd=tr.next();
	if(wd.length){
		if(wd.hasClass("wards")){
			if(wd.is(":visible")){
				wd.hide();
			}
			else{
				wd.show();
			}
			return;
		}
	}
	var s="";
	var borough=tr.children("td:first").text();
	if(borough){
		$.ajax({
			url:"ACOAssaultsPerBorough.ashx",
			cache:false,
			data:"borough="+borough,
			dataType:"json",
			success:function(data){
				$.each(data, function(i,row){
					s+="<tr><td>"+htmlEncode(row.Ward)+"</td><td>"+htmlEncode(row.Assaults)+"</td></tr>";
				});
				s="<tr><td colspan=\"2\"><table cellspacing=\"0\"><tr><th>Ward</th><th>Number of Assaults</th></tr>"+s;
				s+="</table></td><td>&nbsp;</td></tr>";
				$(s)
					.insertAfter(tr)
					.filter("tr:first")
						.addClass("subTable wards")
						.find("td:first").has("table")
							.addClass("highlight")
							.end()
						.find("table")
							.find("tr > :nth-child(2)").addClass("number");
			}
		});
	}
}
function mainTableClick(e){
	var t=($(e.target).closest("td"));
	switch(t.index()){
		case 2:
			wardsDrill(t);
			break;
	}
	return false;
}
function setupTable(t){
	t=$(t);
	var th=t.find("thead > tr > th");
	th.filter(".number").each(function(){
		t.find("tbody > tr > td:nth-child("+(th.index(this)+1)+")").addClass("number");
	});
	t.children("tbody:first")
		.click(mainTableClick)
		.find("tr > td:nth-child(3)")
			.wrapInner("<a href='#'/>")
			.addClass("hover");
	t.sortableTable();
}
$(function(){
	makeNavMenu();
	$("#boroughSearch")
		.tableSearch("#boroughTable")
		.focus();
	setupTable("#boroughTable");
});
