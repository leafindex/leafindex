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
function chartScaleAndEncode(vals){
	function encode(val){
		var chars='ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-.',numChars=64,maxVal=4095;
		if(val===null){
			return "__";
		}		
		var numVal=Number(val);
		if(isNaN(numVal)){
			return "__";
		}
		if(numVal<0||numVal>maxVal){
			return "__";
		}
		var q=Math.floor(numVal/numChars);
		var r=numVal-(numChars*q);
		return chars.charAt(q)+chars.charAt(r);
	}
	var chartvals=[],maxVal=4095;
	var scale=Math.max.apply(Math,vals);
	var j=vals.length;
	for(var i=0;i<j;i++){
		chartvals.push(encode(vals[i]===null?null:(vals[i]/scale)*maxVal));
	}
	return chartvals.join("");
}
function chartBHS(data,dataNameLabel,dataNameData,chartWidth,barColour,labelColour,imgClass,imgAlt){
	var barHeight=17,barNum=data.length;
	var yLabels="",barData=[];
	for(var i=0;i<barNum;i++){
		yLabels=encodeURIComponent(data[i][dataNameLabel])+"|"+yLabels;
		barData.push(data[i][dataNameData]);
	}
	return "<img width=\""+chartWidth+"\" height=\""+(barHeight*barNum)+"\" "+
		(imgClass?" class=\""+imgClass+"\"":"")+
		(imgAlt?" alt=\""+imgAlt+"\"":"")+
		"src=\"http://chart.apis.google.com/chart?chs="+chartWidth+"x"+(barHeight*barNum)+
		"&cht=bhs&chbh=a&chco="+barColour+
		"&chxt=y,x,r&chxtc=0,0|1,0|2,-"+chartWidth+
		"&chxs=0,"+labelColour+",11,1,t|1,ffffff,1,1,t|2,"+labelColour+",11,-1,t,"+barColour+
		"&chd=e:"+chartScaleAndEncode(barData)+
		"&chxl=0:|"+yLabels+"2:|"+barData.reverse().join("|")+"|\" />";
}
function drillSwitch(targ){
	var p=targ.prevAll("img,table");
	var a=targ.children("a");
	var dataNotChart=a.text().toLowerCase().indexOf("data")==0;
	p.filter("img").toggle(!dataNotChart);
	p.filter("table").toggle(dataNotChart);
	a.text(dataNotChart?"Chart view →":"Data view →");
	a.blur();
}
function wardsDrill(tr,callback){
	var borough=tr.children("td:first").text();
	if(borough){
		$.ajax({
			url:"/handler/ACOAssaultsWardsPerBorough.ashx",
			cache:false,
			data:"borough="+borough,
			dataType:"json",
			success:function(data){
				var s="";
				$.each(data,function(i,row){
					s+="<tr><td>"+htmlEncode(row.Ward)+"</td><td>"+htmlEncode(row.Assaults)+"</td></tr>";
				});
				s="<div class=\"drillHead\">Number of Assaults per Ward</div>"+
					chartBHS(data,"Ward","Assaults",300,"7dbaff","783e25","drill","chart")+
					"<table cellspacing=\"0\" class=\"hasDrillHeadFoot hidden\"><thead><tr><th>Ward</th><th class=\"number\">Number of Assaults</th></tr></thead><tbody>"+s+"</tbody></table>"+
					"<div class=\"drillFoot superTableOwnClicker\"><a href=\"#\">Data view →</a></div>";
				callback(s,2,[{"selector":".drillFoot","dataKey":"ownClicker","dataVal":drillSwitch}]);
			}
		});
	}
}
function monthsDrill(tr,callback){
	var borough=tr.children("td:first").text();
	if(borough){
		$.ajax({
			url:"/handler/ACOAssaultsMonthsPerBorough.ashx",
			cache:false,
			data:"borough="+borough,
			dataType:"json",
			success:function(data){
				var s="";
				$.each(data,function(i,row){
					s+="<tr><td>"+htmlEncode(row.Month)+"</td><td>"+htmlEncode(row.Assaults)+"</td></tr>";
				});
				s="<table cellspacing=\"0\"><thead><tr><th class=\"month\">Month</th><th class=\"number\">Number of Assaults</th></tr></thead><tbody>"+s+"</tbody></table>";
				callback(s,2);
			}
		});
	}
}
$(function(){
	$("#boroughTable").superTable({
		"searchable":{"searchCtl":"#boroughSearch","searchColumn":1},
		"clickable":[{"column":3,"superFunc":"drill","superFuncFunc":wardsDrill},
			{"column":2,"superFunc":"drill","superFuncFunc":monthsDrill}]
	});
});