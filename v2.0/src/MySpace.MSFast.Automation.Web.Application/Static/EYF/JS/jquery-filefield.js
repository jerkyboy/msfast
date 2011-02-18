(function($) {
    $.fn.fileField = function(op) {
        op = $.extend({ defaultText: "fileField_select", withFile: "fileField_change", uploading: "fileField_upload" }, op || {});
        this.each(function() {
            var inst = $(this);
            inst.parents("form").bind("beforeSubmit", function() {
                inst.parent().siblings(".fctrlt").html($locale[op.uploading]); 
                inst.parent().find(".fctrlm").html(""); 
            });
            inst.parents("form").bind("formCompleted", function(e, results) {
                if (!results.kill) inst.parent().siblings(".fctrlt").html("");
                else inst.parent().siblings(".fctrlt").html(inst.val());
                inst.parent().find(".fctrlm").html((!results.kill) ? $locale[op.defaultText] : $locale[op.withFile]);
            });
            inst.change(function() {
                inst.parent().find(".fctrlm").html($locale[op.withFile]);
                inst.parent().siblings(".fctrlt").html(inst.val());
                var instp = inst.parents(".fctrl");
                instp.css("right", "1px");
                setTimeout(function() { instp.css("right", "0px"); }, 100);
                inst.trigger("onfile", inst.val());
            });
            $(this).parent().find(".fctrlm").html($locale[op.defaultText]);
        });
        return this;
    }
})(jQuery);
