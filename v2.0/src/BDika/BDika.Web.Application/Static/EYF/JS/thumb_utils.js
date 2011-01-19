(function($) {
    $.fn.replaceThumb = function(val) {
        if(val.indexOf("?") == -1) val += "?" + Math.random();
        else val += "&" + Math.random(); 
        this.each(function() {
            $(this).attr("src",val);
        });
        return this;
    };
})(jQuery);

