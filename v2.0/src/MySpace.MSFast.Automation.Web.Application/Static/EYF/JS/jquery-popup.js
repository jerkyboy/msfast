var popupStatus = 0;
var popupClosedCallback = undefined;

document.write("<div id=\"ppc\"><span class=\"ico ico-popup-title\"></span><a href=\"#\" class=\"clspp\">X</a><div id=\"ppds\"><h3 id=\"ppct\"></h3><div id=\"ppca\"></div></div></div><div id=\"ppbg\"></div>");
        
function showPopupObject(_obj,_tit,_callback){
    $("#ppca").removeClass().html("");
    var p = _obj.parent();
    var o = _obj;
    _obj.appendTo("#ppca");
    $("#ppct").html(_tit);
    ___centerPopup();
    ___loadPopup(function(){
        o.appendTo($(p));
        try{_callback();}catch(e){}
    });
}
function showPopupUrl(_url, _tit,_callback){
    showPopup("<iframe src='" + _url + "'></iframe>",_tit,_callback);
}
function showPopup(_htm,_tit,_callback){
    $("#ppca").removeClass().html(_htm);
    $("#ppct").html(_tit);
    ___centerPopup();
    ___loadPopup(_callback);
}
function showPopupButtons(btns, _htm, _tit, _callback,_closecallback){
    var w = _htm + "<p class=\"btnbar\">";
    for (var i = 0; i < btns.length; i++) w += "<label class=\"btn" + ((btns[i].cls) ? " " + btns[i].cls : "") + "\"><input type=\"button\" bt=\"" + btns[i].id + "\" value=\"" + btns[i].text + "\" name=\"button\"></label>";
    w += "</p>";
    $("#ppca").removeClass().addClass("alert").html(w);
    $("#ppct").html(_tit);
    var inst = $("#ppca");    
    $("#ppca").find("input[type=button]").click(function(){
        _callback(inst,$(this).attr("bt"));    });
    
    ___centerPopup();
    ___loadPopup(_closecallback);
}

function ___loadPopup(_callback){
    if (popupStatus == 0) {
        $("#ppbg").css({ "opacity": "0.5" });
        $("#ppbg").show();
        $("#ppc").show();
        var cc = function(e){
            hidePopup();
            e.preventDefault();
            $(this).unbind("click",cc);
        };
        $("a.clspp").bind("click",cc);
        popupStatus = 1;
        popupClosedCallback = _callback;
    }
}

function hidePopup(){
    if (popupStatus == 1) {
        $("#ppbg").hide();
        $("#ppc").hide();
        popupStatus = 0;
        try{popupClosedCallback();}catch(e){}
    }
}

function ___centerPopup() {
    
    var windowWidth = $(window).width();
    var windowHeight = $(window).height();
    var popupHeight = $("#ppc").height();
    var popupWidth = $("#ppc").width();
    
    var top = Math.max(50, mouseposition.y - (popupHeight+50));    
    
    $("#ppc").css({ 
        "position": "absolute", 
        "top": top, 
        "left": windowWidth / 2 - popupWidth / 2 });
    $("#ppbg").css({ "height": windowHeight });
}

var mouseposition = {x : 0, y : 0};

$(function() {
    $(document).mousemove(function(e){
       mouseposition.y = e.pageY;
       mouseposition.x = e.pageX;
    });
    $("#ppcls").click(function(){hidePopup();});
    $("#ppbg").click(function() {hidePopup();});
    $(document).keypress(function(e) {if (e.keyCode == 27 && popupStatus == 1) {hidePopup();}});
});
