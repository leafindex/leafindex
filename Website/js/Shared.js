 function makeNavMenu() {
	var m=$("<div id='navmenu'/>")
		.html("<div id='navmenuLogo'><a href='Home.htm'><img src='images/logo.png' alt='LeafIndex' width='52' height='60' border='0'/></a></div>"
			+ "<div id='navmenuName'><a href='Home.htm'>LeafIndex</a></div>"
			+ "<div id='navmenuLinks'><a href='Home.htm'>Home</a>"
			+ "<a href='#' class='dropdownMenuing assaults' id='assaults'>Ambulance Call Outs To Assaults</a>"
			+ "<a href='PopulationCrash.htm'>Population Crash</a>"
			+ "<a href='BlairAtChilcot.htm'>Blair at Chilcot</a>"
			+ "<a href='http://leafindex.blogspot.com'>Blog</a>"
			+ "<a href='Credits.htm'>Credits</a>"
			+ "</div>");
	makeDropdownMenu(
		m.find(".dropdownMenuing").filter(".assaults"),
			"<a href='ACOAssaults1.aspx'>Summary</a>","<a href='ACOMapR.aspx'>Map</a>","<a href='ACOAssaults.htm'>Query</a>");
	m.prependTo("body");
	$("<div id='bottombar'/>").html("&copy;2010 Chris Heywood / Jonathan Clarke" ).appendTo("body");
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
	if(!id){
		return;
	}
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
		.css({"left":(o.left+menuLink.outerWidth()-d.outerWidth())+"px","top":(o.top+menuLink.outerHeight()+7)+"px"})
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
$.fn.superTable=function(options){
	return this.each(function(){
		var table,rows,colCnt,subRowCache,hasSubRowCache,reHeight=false;
		function uid() {
			function S4() {
				return (((1+Math.random())*0x10000)|0).toString(16).substring(1);
			}
			return ("i"+S4()+S4()+S4()+S4());
		}
		function setColCnt(){
			if(!colCnt){	
				colCnt=rows.filter(":first").children().length;
			}
		}
		function resetHeight(){
			if(reHeight){
				table.css("height","auto");
				reHeight=false;
			}
		}
		function sortable(thisTable){
			if(!thisTable){
				thisTable=table;
			}
			$('thead th',thisTable)
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
							if(th.hasClass("month")){
								th.data("sortFunction",function(cell) {
									return Date.parse("1 "+cell.text());
								});
							}
							else{
								th.data("sortFunction",function(cell){
									return cell.text().toUpperCase();
								});
							}
						}
					}
				});
		}
		function sort(th){
			var sortFunc=th.data("sortFunction"),sortRows;
			var direction=(th.data("sortDirection")==1)?-1:1;
			th.data("sortDirection",direction);
			var column=th.index();
			var sortTable=th.closest("table"),mainTable=true,tHeight;
			if(sortTable[0]==table[0]){
				tHeight=table.height();
				sortRows=rows.get();
			}
			else{
				mainTable=false;
				sortRows=sortTable.find("tbody > tr").get();
			}
			$.each(sortRows,function(i,row){
				row.sortKey=sortFunc($(row).children("td:eq("+column+")"));
			});
			sortRows.sort(function(a,b){
				return (a.sortKey<b.sortKey)?-direction:((a.sortKey>b.sortKey)?direction:0);
			});
			var tbody=sortTable.children("tbody");
			$.each(sortRows,function(i,row){
				tbody.append(row);
				row.sortKey=null;
			});
			if(mainTable){
				if(hasSubRowCache){
					$.each(rows,function(i,row){
						$.each(subRowCache[i],function(t,subRow){
							if(subRow.is(":visible")){
								$(row).after(subRow);
								reHeight=true;
							}
						});
					});
					if(reHeight&&(table.height()!=tHeight)){//ie8?!
						table.height(tHeight);
					}
				}
			}
		}
		function searchable(){
			var ctl,cache;
			function filter(){
				function subRowing(action,rowIndex){
					if(hasSubRowCache){
						var hideClass="searchablehide";
						$.each(subRowCache[rowIndex],function(t,subRow){
							if(subRow.is(action==="show"?"."+hideClass:":visible")){
								subRow.toggleClass(hideClass);
								if(action==="show"){
									subRow.show();
								}
								else{
									subRow.hide();
								}
							}
						});
						resetHeight();
					}
				}
				function subRowingAllShow(){
					if(hasSubRowCache){
						$.each(subRowCache,function(i){
							subRowing("show",i);
						});
					}
				}
				var term=$.trim(ctl.val().toLowerCase());
				if(!term){
					rows.show();
					subRowingAllShow();
				}
				else{
					rows.hide();
					cache.each(function(i){
						if(this.indexOf(term)>-1){
							$(rows[i]).show();
							subRowing("show",i);
						}
						else{
							subRowing("hide",i);
						}
					});
				}
			}
			if(typeof(options)=="object"&&typeof(options.searchable)=="object"){
				setColCnt();
				var searchCol=parseInt(options.searchable.searchColumn,10);
				searchCol=isNaN(searchCol)||searchCol>colCnt?1:searchCol;
				cache=rows.find("td:eq("+(searchCol-1)+")").map(function(){
					return $(this).text().toLowerCase();
				});
				var sc=options.searchable.searchCtl;
				if(typeof(sc)=="string"){
					ctl=$(sc);
					ctl
						.focus()
						.keyup(filter).keyup()
						.parents("form").submit(function(){
							return false;
						});
				}
			}
		}
		function goodlooking(thisTable){
			if(!thisTable){
				thisTable=table;
			}
			var th=thisTable.find("thead > tr > th");
			th.filter(".number").each(function(){
				thisTable.find("tbody > tr > td:nth-child("+(th.index(this)+1)+")").addClass("number");
			});
		}
		function addSubTable(row,type,subTable,colsToUse){
			if(subTable){
				setColCnt();
				var c=parseInt(colsToUse,10);
				c=isNaN(c)||c>colCnt?colCnt:c;
				var prefix="<tr><td"+(c>1?" colspan=\""+c+"\"":"")+"><div class=\"superTableSubTableClose\"><a href=\"#\">▲</a></div>";
				var suffix=c===colCnt?"</td></tr>":"</td><td"+(colCnt-c>1?" colspan=\""+(colCnt-c)+"\"":"")+">&nbsp;</td></tr>";
				var st=$(prefix+subTable+suffix)
					.insertAfter(row)
					.addClass("superTableSubTable");
				subRowCache[rows.index(row)][type]=st;
				var stt=st.children("td:first")
					.addClass("highlight")
					.children("table");
				goodlooking(stt);
				sortable(stt);
				row.addClass("highlight");
			}
		}
		function subTabling(cell,func,type){
			var tr=cell.parent();
			var done="",highlight=false;
			$.each(subRowCache[rows.index(tr)],function(t,subRow){
				if(t===type){
					if(subRow.is(":visible")){
						subRow.hide();
						done="hide";
					}
					else{
						if(tr.next()[0]!=subRow[0]){
							tr.after(subRow);
						}
						subRow.show();
						done="show";
					}
				}
				else{
					if(!highlight){
						highlight=subRow.is(":visible");
					}
				}
				if(done&&highlight){
					return false;
				}
			});
			if(done){
				if(!highlight){
					if(done=="hide"){
						tr.removeClass("highlight");
					}
					else{
						tr.addClass("highlight");
					}
				}
			}
			else{
				func(tr,function(subTable,colsToUse){
					addSubTable(tr,type,subTable,colsToUse);
				});
			}
			resetHeight();
		}
		function subTableClose(subRow){
			var row,highlight=false;
			row=subRow.prev();
			while(row.is(".superTableSubTable")){
				row=row.prev();
			}
			subRow.hide();
			subRow=subRow[0];
			$.each(subRowCache[rows.index(row)],function(t,sr){
				if(sr[0]!=subRow){
					if(!highlight){
						highlight=sr.is(":visible");
						if(highlight){
							return false;
						}
					}
				}
			});
			if(!highlight){
				row.removeClass("highlight");
			}
			resetHeight();
		}
		function click(e){
			var targ=$(e.target).closest("div, td, th");
			if(targ.is("th")){
				sort(targ);
				return false;
			}
			else{
				if(targ.is("div")){
					if(targ.hasClass("superTableSubTableClose")){
						subTableClose(targ.closest("tr"));
						return false;
					}
				}
				else{
					var f=table.data("superTableClick"+(targ.index()+1));
					if(f){
						f(targ);
						return false;
					}
				}
			}
			return true;
		}
		function clicking(){
			if(typeof(options)=="object"&&typeof(options.clickable)=="object"){
				hasSubRowCache=false;
				$.each(options.clickable,function(i,click){
					var c=parseInt(click.column,10);
					if(!isNaN(c)){
						rows
							.find("td:nth-child("+c+")")
							.wrapInner("<a href='#'/>")
							.addClass("hover");
						if(typeof(click.func)=="function"){
							table.data("superTableClick"+c,click.func);
						}
						else{
							if(click.superFunc=="subTable"&&typeof(click.superFuncFunc)=="function"){
								hasSubRowCache=true;
								var type=uid();
								table.data("superTableClick"+c,function(cell){
									subTabling(cell,click.superFuncFunc,type);
								});
							}
						}
					}
				});
				if(hasSubRowCache){
					subRowCache=rows.map(function(){
						return {};
					});
				}
			}
			table.click(click);
		}
		table=$(this);
		table.addClass("superTable");
		rows=table.find("tbody > tr");
		goodlooking();
		clicking();
		sortable();
		searchable();
	});
};
function Sayuser(msg){
	$("#lblSayuser").text(msg);
}
function DebugSayuser(msg){
	$("#lblDebugSayuser").text(msg);
}
function ShowProgress(onoff) {
  if(onoff)
      $("#lblProgress").html("<img src='images/progress.gif' width='13' height='13'/>");
  else
      $("#lblProgress").html("");
}
$(function(){
	makeNavMenu();
});