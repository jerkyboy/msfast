if($("#ttip").length == 0)
    $("body").append("<div id=\"ttip\"></div>");
    
var ToolTipDock = {
    top: 1,
    left : 2,
    bottom : 4,
    right: 8
};
(function($){
    $.fn.tooltip = function(op)
    {
        var ttwrp = $("<div class=\"tooltip\"><span class=\"rc6 rc tl\"></span><span class=\"rc6 rc tr\"></span><span class=\"rc6 rc bl\"></span><span class=\"rc6 rc br\"></span></div>");
        
        op = $.extend({dock : ToolTipDock.bottom | ToolTipDock.right , 
                        open : function(){}, close : function(){},
                        beforeOpen : function(){}, beforeClose : function(){},
                        padding : "0 0 0 0"
                    }, op);
                    
        ttwrp.append(op.html || "");
        
        op.html = ttwrp;
        var o = op;
        
        $(this).mouseover(function(){
            o.beforeOpen.call(this, o);
            o.callback = o.open;
            $(this).openTooltip(o);
        }).mouseout(function(){
            o.beforeClose.call(this, o);
            o.callback = o.close;
            $(this).closeTooltip(o);
        });
    };
    $.fn.openTooltip = function(op)
    {
        op = $.extend({ dock : ToolTipDock.bottom | ToolTipDock.left , 
                        strictpos : true,
                        fixpos : "true",
                        callback : function(){}, 
                        html : "<div></div>"},
                        op);
                        
        $("#ttip").html(op.html);
        if(op.padding) $("#ttip").css({padding:op.padding});
        else $("#ttip").css({padding:"0 0 0 0"});
        var of = $(this).offset();
        var ttipHeight = $("#ttip").height();
        var ttipWidth = $("#ttip").width();
        $("#ttip").hide();
        var sW = $(window).width() + $(window).scrollLeft();
        var sH = $(window).height() + $(window).scrollTop();
        var t = of.top;
        if(((op.dock & ToolTipDock.bottom) == ToolTipDock.bottom)) t += $(this).outerHeight(true);
        else if(((op.dock & ToolTipDock.top) == ToolTipDock.top)) t -= ttipHeight;
                
        var l = (of.left + ($(this).outerWidth(true)/2)) - (ttipWidth/2);
        if(((op.dock & ToolTipDock.left) == ToolTipDock.left)) l = of.left;
        if(((op.dock & ToolTipDock.right) == ToolTipDock.right)) l = of.left + $(this).outerWidth(true);
        
        if(op.fixpos === "true"){
            if(t + ttipHeight > sH){
                if(!op.strictpos) t = sH - ttipHeight;
                else t = of.top - ttipHeight; 
            }
            if(t + ttipHeight > sH) t = sH - ttipHeight;
            if(l + ttipWidth > sW){
                if(!op.strictpos) l = sW - ttipWidth;
                else if(((op.dock & ToolTipDock.right) == ToolTipDock.right)) l = of.left;
                else l = of.left - ttipWidth;
            }
            if(l + ttipWidth > sW) l = sW - ttipWidth;
        }
        $("#ttip").css({"position": "absolute","top": t,"left": l});
        if(op.smooth) $("#ttip").fadeIn("fast");
        else $("#ttip").show();
        op.callback.call(this,$("#ttip"));
        return this;
    };    
    
    $.fn.closeTooltip = function(op)
    {
        op = $.extend({callback : function(){}},op);
        $("#ttip").html("");
        op.callback.call(this);
        return this;
    };
})(jQuery);