(function($) {
    $.fn.icon = function(n) {
        this.each(function() {
            if ($(this).children("span.icon." + n).length > 0) return;
            if ($(this).css("position") == "static") $(this).css("position", "relative");
            $(this).append("<span class=\"icon " + n + "\"></span>");
        });
        return this;
    }
    $.fn.rounded = function(options) {
        this.icon("lb").icon("lt").icon("rb").icon("rt");
        return this;
    };
})(jQuery);
