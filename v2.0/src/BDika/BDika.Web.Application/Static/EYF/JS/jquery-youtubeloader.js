function revalidateYouTubeFlash(){
    if (window["isDisableFlash"]) return;
    $("img.flashmovie").each(function() {
        var id = "fl" + ((Math.random() * 500) + "").replace(/\./gi, "").substring(0, 5) + "fl";
        $(this).before("<div id=\"" + id + "\" class=\"flashmovie\"></div>");
        $(this).remove();
        swfobject.embedSWF($(this).attr("src").replace(new RegExp("http://img.youtube.com/vi/(.*?)/0.jpg", "i"), "http://www.youtube.com/v/$1&fs=1"), id, "505", "315", "9.0.0", "", "", {wmode : "transparent"});
    });
}
$(document).ready(function() {
    revalidateYouTubeFlash();
});
