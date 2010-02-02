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
				s="<table cellspacing=\"0\"><thead><tr><th>Ward</th><th class=\"number\">Number of Assaults</th></tr></thead><tbody>"+s+"</tbody></table>";
				callback(s,2);
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
		"clickable":[{"column":3,"superFunc":"subTable","superFuncFunc":wardsDrill},
			{"column":2,"superFunc":"subTable","superFuncFunc":monthsDrill}]
	});
});