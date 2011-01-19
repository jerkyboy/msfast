(function($) {
    $.fn.tabControl = function(options) {
        var o = options;

        if (o.tabs) {
            var t = o.tabs;
            t.click(function(e) {
                e.preventDefault();
                if ($(this).hasClass("disabled")) {
                    return;
                }
                t.each(function() {
                    $(this).removeClass("selected");
                });
                $(this).addClass("selected");

                if (o.callback) {
                    o.callback($(this).attr("bt"),$(this));
                }
                $(this).blur();
            });
        }

        return this;
    }
})(jQuery);
