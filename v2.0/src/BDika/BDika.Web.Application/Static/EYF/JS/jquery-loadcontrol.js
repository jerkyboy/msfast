var ___loaded_static_files = [];
var cachedControls = {};

$.loadControlPopup = function(_url,_t,_cb) {
    var w = "temp" + Math.round(900000 * Math.random());
    var t = "<div id=\"" + w + "\" class=\"loading\"></div>";
    showPopup(t, _t);
    $("div#" + w).loadControl(_url,_cb);
}
$.fn.renderLoadedControl = function(xml,calback, append)
{
    var inst = this;
    while($(this).hasClass("loading")) $(this).removeClass("loading");
    var p = "";
    var scr = "";
    var sty = "";
    xml = $(xml);    
    xml.find("page").each(function() { p += $(this).text(); });
    xml.find("scriptinline").each(function() { scr += $(this).text(); });
    xml.find("styleinline").each(function() { sty += $(this).text(); });
    xml.find("styleinclude").each(function() {
        if ($("link#" + $(this).attr("id")).length == 0) {
            $("head").append("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + $(this).attr("filename") + "\" id=\"" + $(this).attr("id") + "\"/>");
        }
    });

    var w = 0;

    var doneAll = function() {
        p += "<scr" + "ipt>" + scr + "</scr" + "ipt>";
        p += "<st" + "yle>" + sty + "</styl" + "e>";
        $(document).trigger("doneWorking", inst);
        if(append) $(inst).append(p);
        else $(inst).html(p); 
        ___centerPopup();
        if(calback) calback($(inst));
    }

    xml.find("scriptinclude").each(function() {
        if ($("script#" + $(this).attr("id")).length == 0 && ___loaded_static_files[$(this).attr("id") + "_s"] == undefined) {
            w++;
            ___loaded_static_files[$(this).attr("id") + "_s"] = $(this).attr("filename");
            $.getScript($(this).attr("filename"), function() {
                w--;
                if (w <= 0) { doneAll(); }
            });
        }
    });
    if (w <= 0)
    {
        doneAll();
    }
}
$.fn.loadControl = function(url,cb,cached) {
    var inst = this;
    $(document).trigger("startWorking", this);
    
    if(cached && cachedControls[url]){
        $(inst).renderLoadedControl(cachedControls[url], cb);
        return;
    }
    
    $.ajax({ type: "GET", url: url, dataType: "xml",
        success: function(xml) {
            cachedControls[url] = xml;
            $(inst).renderLoadedControl(xml, cb);
        }
    });
};
