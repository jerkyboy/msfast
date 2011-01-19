if($(".comboxbg").length == 0)
    $("body").append("<div class=\"comboxbg t\"></div><div class=\"comboxbg b\"></div>");

(function($){
    var showComboxBG = function(s){
        var wh = $(window).height();
        var ww = $(window).width();
        var dm = {t: s.offset().top, l: s.offset().left, w : s.outerWidth(true),h:s.outerHeight(true)};
        $("div.comboxbg.t").css({left:0, top:0, height: dm.t});
        $("div.comboxbg.b").css({left:0, top:dm.t + dm.h,height: wh - (dm.t + dm.h)});
        return $("div.comboxbg").show();
    };
    var hideComboxBG = function(){
        return $("div.comboxbg").hide();
    };
    
    $.fn.comboxVal = function(e){
        if(arguments.length > 0){
            $(this).find("input[type=text]").val(arguments[0].label);
            $(this).find("input[type=hidden]").val(arguments[0].val);
            if(arguments[0].ico){
                $(this).attr("ico",arguments[0].ico);
                $(this).find("span.df").removeClass().addClass("ico df ico-" + arguments[0].ico);
            }else if(arguments[0].img){
                $(this).attr("img",arguments[0].img);
                $(this).find("span.df").removeClass().addClass("imgico df").css({backgroundImage : "url('" + arguments[0].img + "')"});
            }
            $(this).trigger("onselectedvalue", arguments[0]);
        }else{
            return {label : $(this).find("input[type=text]").val(), 
                    val : $(this).find("input[type=hidden]").val() , 
                    ico : $(this).attr("ico"),
                    img : $(this).attr("img")};
        }
        return this;
    };
    
    $.fn.combox = function(options)
    {
        if($(this).length > 1){
            $(this).each(function(){
               $(this).combox(options);
            });
            return $(this);
        }
        var KEY = {UP: 38,DOWN: 40,DEL: 46,TAB: 9,RETURN: 13,ESC: 27,COMMA: 188,PAGEUP: 33,PAGEDOWN: 34,BACKSPACE: 8};
        var op = $.extend({data:[],name:"combox"}, options);
        var lst,las,foc = undefined;
        var dv = $(this);
        var sel = $("<input type=\"text\" name=\"" + op.name + (Math.floor(Math.random()*10000)) + "_o\" value=\"\" class=\"txt combox\"/>");
        var hid = $("<input type=\"hidden\" name=\"" + op.name + "\" value=\"\" />");
        dv.append("<span class=\"ico df\"></span>").append(sel).append(hid);
        
        var showList = function(str){
           var lsthtm = "";
           var labl,rg = undefined;
           if(str["match"]) rg = new RegExp("(" + str["match"] + ")","gi");
           for(var i = 0 ; i < op.data.length; i++){
               labl = op.data[i].label;
               if(rg){
                 if(labl.match(str["match"])) labl = labl.replace(rg,"<strong>$1</strong>");
                 else labl = undefined;
               }
               if(labl){
                   lsthtm += "<a class=\"comboxitm\" val=\"" + op.data[i].val + "\" ico=\"" + (op.data[i].ico || "") + "\" img=\"" + (op.data[i].img || "") + "\" href=\"#\">" + 
                             (op.data[i].ico ? "<span class=\"ico ico-" + op.data[i].ico + "\"></span>" : "") +
                             (op.data[i].img ? "<span class=\"imgico\" style=\"background-image:url('" + op.data[i].img + "')\"></span>" : "") +
                             "<span class=\"label\">" + labl + "</span></a>\r\n";
               }
           }
           if(lsthtm == "") lsthtm = "<span class=\"nores\">No Match...</span>";
           var h = "<div class=\"comboxlst\">" + lsthtm + "</div>";
           if(las == h) return;
           var pos = sel.position();
           las = h;
           lst = $(h);
           lst.find("a").click(function(e){
               e.preventDefault();
               select({val : $(this).attr("val"), label: $(this).find("span.label").text(), ico : $(this).attr("ico"), img : $(this).attr("img")});
           }).mouseover(function(){
                $(this).addClass("over");          
           }).mouseout(function(){
                $(this).removeClass("over");
           });
           lst.keydown(keydown).find("*").keydown(keydown);
           showComboxBG(sel).bind("click",bgclick);
           sel.openTooltip({dock:  ToolTipDock.bottom | ToolTipDock.left, html: lst});
        };        
        var hideList = function(){
            sel.closeTooltip();
            hideComboxBG().unbind("click",bgclick); 
            las = "";
            if(op.defaultVal){
                var v = sel.val();
                var f = false;
                for(var i = 0 ; i < op.data.length; i++){
                    if(op.data[i].label == v){
                        f = true;
                        break;
                    }
                }
                if(f == false) dv.comboxVal(op.defaultVal);
            }
        };
        
        var bgclick = function(){hideList();};
        var select = function(v){dv.comboxVal(v);hideList();};

        var focusSelected = function(){
            if(!lst) return;
            var i = 1;
            var a,l = lst.find("a.over:first");
            a = l;
            var oh = a.outerHeight(true);
            while((l = l.prev("a")).length > 0) i++;
            var h = oh * i;
            var st = lst.scrollTop();
            var sh = lst.height();
            if(st + sh < (h+oh)){
                lst.scrollTop(h-sh);
            }else if(st >= h-oh){
                lst.scrollTop(h - oh);
            }
        };        
        var next = function(){
           if(!lst) return;
           var al = lst.find("a.over:last");
           if(al.length != 0 && al.next("a").length == 0) return;
           lst.find("a.over").removeClass("over");
           al.next("a").addClass("over");
           if(lst.find("a.over").length == 0) lst.find("a:first").addClass("over");
           focusSelected();
        };
        var prev = function(){
           if(!lst) return;
           var al = lst.find("a.over:last");
           if(al.length != 0 && al.prev("a").length == 0) return;
           lst.find("a.over").removeClass("over");
           al.prev("a").addClass("over");
           if(lst.find("a.over").length == 0) lst.find("a:first").addClass("over");
           focusSelected();
        };
        var selectCurrent = function(){
           if(!lst) return;
           var selctd = lst.find("a.over:first");
           if(selctd.length == 0) hideList();
           else select({val: selctd.attr("val"), label: selctd.text(), ico : selctd.attr("ico"), img : selctd.attr("img") });
           focusSelected();
        };
        var pageUp = function(){
           if(!lst) return;
           var al = lst.find("a.over:last");
           lst.find("a.over").removeClass("over");
           var i = 0;
           while(al && al.length > 0 && i < 7){
            al = al.prev("a");
            i++;
           }
           al.addClass("over");
           if(lst.find("a.over").length == 0) lst.find("a:first").addClass("over");
           focusSelected();
        };
        var pageDown = function(){
           if(!lst) return;
           var al = lst.find("a.over:last");
           lst.find("a.over").removeClass("over");
           var i = 0;
           while(al && al.length > 0 && i < 7){
            al = al.next("a");
            i++;
           }
           al.addClass("over");
           if(lst.find("a.over").length == 0) lst.find("a:last").addClass("over");
           focusSelected();
        };
        var revalidateList = function(){ showList({match : sel.val()} ); };
        
        var keydown = function(e){
            var p = false;
            if(foc != sel) sel.focus();
		    switch(e.keyCode){
			    case KEY.UP:
				    p = true;
				    prev();
				    break;
			    case KEY.DOWN:
				    p = true;
				    next();
			    	break;
			    case KEY.PAGEUP:
				    p = true;
					pageUp();
    				break;
			    case KEY.PAGEDOWN:
                    p = true;
					pageDown();
    				break;
    			case KEY.TAB:
	    		case KEY.RETURN:
		    		selectCurrent();
				    break;
    			case KEY.ESC:
	    			hideList();
		    		break;
		        default :
		            setTimeout(revalidateList,50);
		            break;
            }
            if(p) e.preventDefault();
         }         
         sel.focus(showList)
         .focus(function(){foc = sel;})
         .click(showList)
         .keydown(keydown)
         .blur(function(){foc = undefined;});
         
         var fc;
         fc = function(){
            sel.val("");
            sel.unbind("focus",fc);
         }
         
         if(op.initVal){
            dv.comboxVal(op.initVal);
            sel.bind("focus",fc);
         }
         return $(this);
    };
})(jQuery);


    
