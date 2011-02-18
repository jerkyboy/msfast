(function($) {
    $.fn.bindFormGUI = function(n) {
        if (this.validateField) this.bind("formCompleted", n, formUI_formCompleted_showFailedFields);
        this.bind("formSubmitting", n, formUI_formSubmitting).bind(
                    "formCompleted", n, formUI_formCompleted_notifications).bind(
                    "formPostCompleted", n, formUI_formPostCompleted);
    };
    $.fn.unbindFormGUI = function(n) {
        if (this.validateField) this.unbind("formCompleted", formUI_formCompleted_showFailedFields);
        this.unbind(
                    "formSubmitting", formUI_formSubmitting).unbind(
                    "formCompleted", formUI_formCompleted_notifications).unbind(
                    "formPostCompleted", formUI_formPostCompleted);
    };
})(jQuery);

function formUI_formSubmitting(e, results) {
    $(e.data.container || results.frm).hideNotifications();
    results.frm.find(" button").attr("disabled", "disabled");
    results.frm.find("a.sb span.txt").each(function() {
        $(this).attr("orval", $(this).text());
        $(this).html($locale["pleaseWaitFormSubmit"]);
    });
    results.frm.find(" input[type='submit']").each(function() {
        $(this).attr("disabled", "disabled");
        $(this).attr("orval", $(this).attr("value"));
        $(this).attr("value", $locale["pleaseWaitFormSubmit"]);
    });
}

function formUI_formPostCompleted(e, r) {
    if (r.redirectRequested) return;

    r.frm.find("button").removeAttr("disabled");

    r.frm.find("a.sb span.txt").each(function() {
        $(this).html($(this).attr("orval"));
    });

    r.frm.find("input[type='submit']").each(function() {
        $(this).attr("value", $(this).attr("orval"));
        $(this).removeAttr("disabled");
    });
}

function formUI_formCompleted_notifications(e, r) {
    var frm = $(e.data.container || r.frm).hideNotifications();
    if (r.data && r.data.errs) frm.showErrors(r.data.errs);
    if (r.data && r.data.msgs) frm.showMessages(r.data.msgs);
}

$.fn.showErrors = function(errs){ return $(this).showNotifications(errs,"err","ico-error"); }
$.fn.showMessages = function(msgs){ return $(this).showNotifications(msgs,"msg","ico-ok"); }
$.fn.showNotifications = function(notifications, cnm, ico){
    return $(this).each(function(){
        if(notifications && notifications.length){
            var htm = "<div style=\"display:none\" class=\"notifications " + cnm + "\"><div class=\"wrp\"><span class=\"ico " + ico + "\"></span><ul>";
            for (var i = 0; i < notifications.length; i++) htm += "<li class=\"li" + (i % 6) + "\">" + notifications[i].val + "</li>";
            htm += "</ul></div></div>";  
            
            var d = $(htm);
            
            if ($(this).find("div." + cnm).length > 0) { var w = $(this).find("div." + cnm); w.after(d);w.remove(); }
            else if ($(this).find("h3:first").length > 0) { $(this).find("h3:first").after(d); }
            else { $(this).prepend(d);}
            d.show();
            try{$('html,body').animate({scrollTop: d.offset().top}, 250);}catch(e){}          
        }
    });
}
$.fn.hideNotifications = function(){
    return $(this).each(function(){
        $(this).find("div.notifications").hide("fast");
    });
}




function __clearFailedField(frm) {
    $("input,select,textarea").each(function() {
        if ($(this).hasClass("failed")) $(this).removeClass("failed");
    });
}
function formUI_formCompleted_showFailedFields(e, results) {
    __clearFailedField(results.frm);
    if (results.data && results.data.errs && results.data.errs.length > 0) {
        for (var i = 0; i < results.data.errs.length; i++) {
            if (results.data.errs[i].key) {
                $("input[name=" + results.data.errs[i].key + "],select[name=" + results.data.errs[i].key + "],textarea[name=" + results.data.errs[i].key + "]").each(function() {
                   if($(this).hasClass("failed") == false) $(this).addClass("failed");
                   if ($(this).attr("type") != "file") $(this).bind("focus", __focusValidationMessage_show).bind("blur", __focusValidationMessage_hide);
                });
            }
        }
    }
}

function __focusValidationMessage_show(e) {
    var fl = $(this).validateField();
    if (fl && fl.val) {
        var sb = $("span.validationErr");
        if (sb.length == 0) { $(document.body).append("<span style=\"display:none\" class=\"validationErr\"></span>"); sb = $("span.validationErr"); }
        var pos = $(this).position();
        var off = $(this).offset();
        var d = $(this).height();
        sb.html(fl.val);
        sb.css({ "left": Math.round(off.left) + "px", "top": Math.round(off.top) + d + "px" });
        sb.show();
    }
}

function __focusValidationMessage_hide(e) {
    if ($(this).validateField() == null) {
        if ($(this).hasClass("failed")) $(this).removeClass("failed");
        $(this).unbind("focus", __focusValidationMessage_show).unbind("blur", __focusValidationMessage_hide);
    }
    $("span.validationErr").hide();
}
