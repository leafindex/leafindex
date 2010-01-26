$.fn.superTable=function(options){
	return this.each(function(){
		var table;
		function sortable(){
			$('thead th',table)
				.addClass("sortable hover")
				.wrapInner("<a href='#'/>")
				.each(function(){
					var th=$(this);
					if(th.hasClass("number")){
						th.data("sortFunction",function(cell){
							var key=parseFloat(cell.text().replace(/[^\d.]*/g,""));
							return isNaN(key)?0:key;
						});
					}
					else{
						if(th.hasClass("date")){
							th.data("sortFunction",function(cell) {
								return Date.parse(cell.text());
							});
						}
						else{
							th.data("sortFunction",function(cell){
								return cell.text().toUpperCase();
							});
						}
					}
				});
		}
		function sort(th){
			var sortFunc=th.data("sortFunction");
			var rows=table.find("tbody > tr").get();
			var column=th.index();
			var direction=(th.data("sortDirection")==1)?-1:1;
			th.data("sortDirection",direction);
			$.each(rows,function(i,row){
				row.sortKey=sortFunc($(row).children("td:eq("+column+")"));
			});
			rows.sort(function(a,b){
				return (a.sortKey<b.sortKey)?-direction:((a.sortKey>b.sortKey)?direction:0);
			});
			$.each(rows,function(i,row){
				table.children("tbody").append(row);
				row.sortKey=null;
			});
		}
		function searchable(ctl){
			var rows,cache;
			function subRowsHide(r){
				if(r.length){
					r.removeClass("highlight").hide();
				}
			}
			function subRowsShow(r){
			}
			function filter(){
				var subRows=table.find("tbody > tr.subTable:visible");
				subRowsHide(subRows);
				var term=$.trim(ctl.val().toLowerCase());
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
			rows=table.find("tbody > tr");
			cache=rows.find("td:first").map(function(){
					return $(this).text().toLowerCase();
				});
			ctl=$(ctl);
			ctl
				.focus()
				.keyup(filter).keyup()
				.parents("form").submit(function(){
					return false;
				});
		}
		function goodlooking(){
			var th=table.find("thead > tr > th");
			th.filter(".number").each(function(){
				table.find("tbody > tr > td:nth-child("+(th.index(this)+1)+")").addClass("number");
			});
		}
		function click(e){
			var cell=$(e.target).closest("td, th");
			if(cell.is("th")){
				sort(cell);
			}
			else{
				var f=table.data("superTableEvent"+(cell.index()+1));
				if(f){
					f(cell);
				}
			}
			return false;
		}
		function clicking(){
			if(options.clickable){
				var tr=table.find("tbody > tr");
				$.each(options.clickable,function(i,click){
					var c=click.col;
					table.data("superTableEvent"+c,click.event);
					tr
						.find("td:nth-child("+c+")")
						.wrapInner("<a href='#'/>")
						.addClass("hover");
				});
			}
			table.click(click);
		}
		table=$(this);
		if(table.length){
			goodlooking();
			clicking();
			sortable();
			if(options.searchable){
				searchable(options.searchable);
			}
		}
	});
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
$(function(){
	makeNavMenu();
	$("#boroughTable").superTable({
		"searchable": "#boroughSearch",
		"clickable": [{"col": 3, "event": wardsDrill}]
	});
});
