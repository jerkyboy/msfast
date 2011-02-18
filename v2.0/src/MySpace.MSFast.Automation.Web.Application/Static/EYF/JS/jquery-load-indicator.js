$(function(){
    var loadindicator = "<div class=\"loadindicator\"><div class=\"loadindicatorBackground\"></div><div class=\"loadindicatorIcon\"></div></div>";
    
    $(document).bind("doneWorking", function(_d, _p) {
    
        $("div.loadindicator").hide();
    
    }).bind("startWorking", function(_d, _p) {
        if ($(_p).length != 1)
            return;
            
        var off = $(_p).offset();
        var ld = $("div.loadindicator");
        if(ld.length == 0) $("body").append(ld = $(loadindicator));
        ld.css({
            "left": Math.round(off.left) + "px", "top": Math.round(off.top) + "px",
            "width": Math.round($(_p).outerWidth(true)) + "px", "height": Math.round($(_p).outerHeight(true)) + "px"
        }).show();
    });
});