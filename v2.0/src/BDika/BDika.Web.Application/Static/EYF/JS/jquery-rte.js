var allowedTags = ["u", "i", "b", "div", "h3", "h4", "h5", "br", "ul", "li", "ol"];
var youtubebase = "http://img.youtube.com/vi/";

function formatToHTML(_txt) { return __formatToHTML(_txt, "<img src=\"" + youtubebase + "$1" + "/0.jpg\" class=\"flashmovie\"/>"); }

function __formatToHTML(_txt, _embdCode) {
    _txt = _txt.replace(/&amp;/gi, "&").replace(new RegExp("\\[a=(.*?)\\](.*?)\\[\/a\\]", "gi"), "<a href=\"$1\">$2</a>");
    _txt = _txt.replace(new RegExp("\\[youtube\\=http\\:\\/\\/www\\.youtube\\.com\\/watch\\?v\\=(.*?)\\]", "gi"), _embdCode);
    _txt = _txt.replace(new RegExp("\\[img=(.*?)\\]", "gi"), "<img src=\"$1\"/>");
    for (var i = 0; i < allowedTags.length; i++) {
        _txt = _txt.replace(new RegExp("\\[" + allowedTags[i] + "\\]", "gi"), "<" + allowedTags[i] + ">");
        _txt = _txt.replace(new RegExp("\\[\/" + allowedTags[i] + "\\]", "gi"), "</" + allowedTags[i] + ">");
    }
    return _txt;
}

var lastChecked = "";
var lastPassed = "";

function formatToText(_html) {
    if (_html == lastChecked)
        return lastPassed;

    lastChecked = _html;

    _html = _html.replace(new RegExp("<em>", "gi"), "<i>").replace(new RegExp("</em>", "gi"), "</i>");
    _html = _html.replace(new RegExp("<p>", "gi"), "<br>").replace(new RegExp("</p>", "gi"), "");
    _html = _html.replace(new RegExp("<strong>", "gi"), "<b>").replace(new RegExp("</strong>", "gi"), "</b>");
    _html = _html.replace(new RegExp("<font([^>]*)>", "gi"), "").replace(new RegExp("</font>", "gi"), "");
    _html = _html.replace(new RegExp("\<a href=\"(.*?)\".*?\>(.*?)\<\/a\>", "gi"), "[a=$1]$2[/a]").replace(/&amp;/gi, "&");
    _html = _html.replace(new RegExp("\<img([^>]*?)src=\"" + youtubebase + "(.*?)/0.jpg\"([^>]*?)\>", "gi"), "[youtube=http://www.youtube.com/watch?v=$2]");
    _html = _html.replace(new RegExp("\<img src=\"(.*?)\"(.*?)\>", "gi"), "[img=$1]");
    for (var i = 0; i < allowedTags.length; i++) {
        _html = _html.replace(new RegExp("<" + allowedTags[i] + "(\s_moz_dirty\=\"\"){0,1}>", "gi"), "[" + allowedTags[i] + "]");
        _html = _html.replace(new RegExp("<[\/]" + allowedTags[i] + ">", "gi"), "[/" + allowedTags[i] + "]");
    }
    _html = _html.replace(/<(.*?)>/gi, "");
    _html = _html.replace(/</gi, "");
    _html = _html.replace(/>/gi, "");
    return lastPassed = _html;
}


(function($) {

    $.fn.setRTEVal = function(_val) {
        var textarea = $(this);
        textarea.val(_val);
        var iframe = document.getElementById(textarea.attr('class') + "Iframe");
        var content = formatToHTML(_val);
        if ($.trim(content) == '') content = '<br />';
        if (iframe) $(iframe).contents().find("body").html(content);
    };

    $.fn.rte = function(options) {

        $.fn.rte.html = function(iframe) {
            return iframe.contentWindow.document.getElementsByTagName("body")[0].innerHTML;
        };

        $.fn.rte.defaults = {
            media_url: "",
            content_css_url: "rte.css",
            content_style: "body{color:#777;font-size:14px;margin:0;padding:15px;}*{font-family:'Helvetica Neue','Helvetica','Arial',sans-serif;line-height:18px}a{color:#5a8c35}a:hover{color:#AAA}h3{color:#f93;font-size:16px;margin:0;padding:10px 0;}h6,h5,h4{font-size:12px;font-weight:bold;margin:0;padding:10px 0;}div{padding:2px 0;margin:4px 0;}",
            default_style: "img.flashmovie{display:block;margin:20px 0px;width:505px;height:315px;padding:0 0 25px 0;background:url(/Static/EYF/IMG/youtube.gif) 100% 100%}",
            dot_net_button_class: null,
            max_height: 350
        };

        var opts = $.extend($.fn.rte.defaults, options);

        return this.each(function() {

            var textarea = $(this);
            var iframe;
            var element_id = textarea.attr("id");


            $(this).parents("form").bind("beforeSerialize", function() {
                if ($(iframe).is(":visible")) {
                    updateTextarea();
                }
            });


            // enable design mode
            function enableDesignMode() {
                var content = formatToHTML(textarea.val());
                if ($.trim(content) == '') {
                    content = '<br />';
                }
                if (iframe) {
                    textarea.hide();
                    $(iframe).contents().find("body").html(content);
                    $(iframe).show();
                    $("#toolbar-" + element_id).remove();
                    textarea.before(toolbar());
                    return true;
                }

                iframe = document.getElementById(textarea.attr('class') + "Iframe");

                if (navigator.userAgent.indexOf("Firefox") == -1) {
                    $(iframe).remove();
                    iframe = document.createElement("iframe");
                }

                iframe.frameBorder = 0;
                iframe.frameMargin = 0;
                iframe.framePadding = 0;
                iframe.height = 200;

                if (textarea.attr('class')) {
                    iframe.className = textarea.attr('class');
                    iframe.id = textarea.attr('class') + "Iframe";
                }
                if (textarea.attr('id')) iframe.id = textarea.attr('id');
                if (textarea.attr('name')) iframe.title = textarea.attr('name');

                if (navigator.userAgent.indexOf("Firefox") == -1) textarea.after(iframe);

                var css = "";
                
                if (opts.content_style) css += opts.content_style;
                if (opts.default_style) css += opts.default_style;
                
                var doc = "<html><head><style>" + css + "</style></head><body class='frameBody'>" + content + "</body></html>";

                tryEnableDesignMode(doc, function() {
                    $("#toolbar-" + element_id).remove();
                    textarea.before(toolbar()); // hide textarea
                    textarea.hide();
                });
            }

            function tryEnableDesignMode(doc, callback) {
                if (!iframe) { return false; }

                try {
                    iframe.contentWindow.document.open();
                    iframe.contentWindow.document.write(doc);
                    iframe.contentWindow.document.close();
                } catch (error) {
                }

                if (document.contentEditable) {
                    iframe.contentWindow.document.designMode = "On";
                    callback();
                    return true;
                }
                else if (document.designMode != null) {
                    try {
                        iframe.contentWindow.document.designMode = "on";
                        callback();
                        return true;
                    } catch (error) {
                    }
                }
                setTimeout(function() { tryEnableDesignMode(doc, callback) }, 500);
                return false;
            }

            function disableDesignMode(submit) {
                if ($(iframe).is(":visible")) {
                    updateTextarea();
                }
                textarea.val(formatToText($(iframe).contents().find("body").html()));
                if (submit != true) {
                    textarea.show();
                    $(iframe).hide();
                }
            }
            function updateTextarea() {
                var content = $(iframe).contents().find("body").html();
                textarea.val(formatToText($(iframe).contents().find("body").html()));
                try { textarea.trigger("change"); } catch (e) { }
            }
            // create toolbar and bind events to it's elements
            function toolbar() {
                var tb = $("<div class='rte-toolbar' id='toolbar-" + element_id + "'>\
                <p class='tf'><a href='#' c='bold' class='b btn ico'></a><a href='#' c='underline' class='u btn ico'></a><a href='#' c='italic' class='i btn ico'></a></p>\
                <p class='ls'><a href='#' c='insertorderedlist' class='o btn ico'></a><a href='#' c='insertunorderedlist' class='u btn ico'></a></p>\
                <p class='st'><a href='#' c='removeformat' class='n btn ico'></a><a href='#' c='formatblock' ad='h3' class='h btn ico'></a></p>\
                <p class='so'><a href='#' class='video ico'></a><a href='#' class='image ico'></a><a href='#' class='link ico'></a></p>\
                <p class='switchMode'><a href='#' class='disable'>" + $locale["rte_html"] + "</a></p></div>");

                $('a.btn', tb).click(function(e) {
                    e.preventDefault();
                    formatText($(this).attr("c"), ($(this).attr("ad") ? '<' + $(this).attr("ad") + '>' : undefined));
                });

                var POPTEXT = "<input type=\"text\" class=\"poptxt\"/>";

                $('.link', tb).click(function(e) {
                    e.preventDefault();
                    var now = $(iframe).contents().find("body").html();
                    var loc = ("LOC" + Math.random() + "LOC").replace(/\./, "");
                    var sel = (iframe.contentWindow.getSelection ? iframe.contentWindow.getSelection() : (iframe.contentWindow.document.selection ? iframe.contentWindow.document.selection.createRange().text : ""));
                    if (sel && sel != "") {
                        formatText('CreateLink', loc);
                    } else { sel = ""; }
                    hidePopupError();

                    showPopupButtons([{ id: "AD", text: $locale["pop_addlink"], cls:"btn1" }, { id: "CN", text: $locale["pop_cancel"], cls:"btn2"}],
                            "<p class=\"t\">" + $locale["pop_addlink_desc"] + "</p><p>Text to display:<br/><input type=\"text\" class=\"atxt\"/></p><p>Link to:<br/>" + POPTEXT + "</p>", $locale["pop_addlink"],
                            function(_inst, _id) {
                                var url = $("input.poptxt").val();
                                if (_id == "AD") {
                                    if (isValidSecuredURL(url)) {
                                        if (sel && sel != "") {
                                            $(iframe).contents().find("a[href=" + loc + "]").attr("href", url).text($("input.atxt").val());
                                        } else {
                                            $(iframe).contents().find("body").append("<a href=\"" + url + "\">" + $("input.atxt").val() + "</a>");
                                        }
                                        now = "";
                                        hidePopup();
                                        return;
                                    } else {
                                        showPopupError($locale["pop_addlink_invalid"]); return;
                                    }
                                }
                                $(iframe).contents().find("body").html(now);
                                hidePopup();
                                return;
                            }, function() { if (now != "") $(iframe).contents().find("body").html(now); $("#ppds").removeClass(); });
                    $("input.atxt").val(sel).focus();
                    $("input.poptxt").val("http://");
                    $("#ppds").removeClass().addClass("alnk");
                });

                $('.image', tb).click(function(e) {
                    e.preventDefault();
                    hidePopupError();
                    $("#ppds").removeClass().addClass("imag");
                    showPopupButtons([{ id: "AD", text: $locale["pop_addimg"], cls:"btn1"}, { id: "CN", text: $locale["pop_cancel"], cls:"btn2"}],
                            "<p class=\"t\">" + $locale["pop_addimg_desc"] + "</p><p>Enter your image's URL:<br/>" + POPTEXT + "</p>", $locale["pop_addimg"],
                            function(_inst, _id) {
                                var url = $("input.poptxt").val();
                                if (_id == "AD") {
                                    if (isValidSecuredURL(url) == false) {
                                        showPopupError($locale["pop_addimg_invalid"]); return;
                                    } else { formatText('InsertImage', url); }
                                }
                                hidePopup();
                                return;
                            }, function() { $("#ppds").removeClass(); });
                    $("#ppds").removeClass().addClass("imag");
                });

                $('.video', tb).click(function(e) {
                    e.preventDefault();
                    hidePopupError();
                    $("#ppds").removeClass().addClass("vid");
                    showPopupButtons([{ id: "AD", text: $locale["pop_addvideo"], cls:"btn1"}, { id: "CN", text: $locale["pop_cancel"], cls:"btn2"}],
                            "<p class=\"t\">" + $locale["pop_addvideo_desc"] + "</p><p>Enter your video's URL:<br/>" + POPTEXT + "</p>", $locale["pop_addvideo"],
                            function(_inst, _id) {
                                var url = $("input.poptxt").val();
                                if (_id == "AD") {
                                    if (isValidSecuredURL(url) == false) {
                                        showPopupError($locale["pop_addvideo_invalid"]); return;
                                    } else {
                                        var vid = (url.match(/http\:\/\/www\.youtube\.com\/watch\?v\=([a-z0-9\-_=])*((&.*?)|$)/gi) + "").replace(/http\:\/\/www\.youtube\.com\/watch\?v\=/gi, "").replace(/\&/gi, "");
                                        if (!vid || vid == "null" || vid == null || vid == undefined || vid == "" || vid.match(/[^0-9a-z\-_=]/gi)) {
                                            showPopupError($locale["pop_addvideo_invalid"]); return;
                                        } else {
                                            var vidurl = youtubebase + vid + "/0.jpg";
                                            formatText('InsertImage', vidurl);
                                            $(iframe).contents().find("body img[src=" + vidurl + "]").addClass("flashmovie");
                                        }
                                    }
                                }
                                hidePopup(); return;
                            }, function() { $("#ppds").removeClass(); });
                    $("#ppds").removeClass().addClass("vid");
                });

                $('.disable', tb).click(function() {
                    disableDesignMode();
                    var edm = $('<p class="switchMode"><a class="rte-edm" href="#">' + $locale["rte_edit"] + '</a></p>');
                    tb.empty().append(edm);
                    edm.click(function(e) {
                        e.preventDefault();
                        enableDesignMode();
                        $(this).remove();
                    });
                    return false;
                });

                var iframeDoc = $(iframe.contentWindow.document);
                var upd = function() { updateTextarea(); return true; };
                iframeDoc.mouseup(upd).blur(upd).keyup(function() {
                    updateTextarea();
                    var body = $('body', iframeDoc);
                    if (body.scrollTop() > 0) {
                        var iframe_height = parseInt(iframe.style['height'])
                        if (isNaN(iframe_height))
                            iframe_height = 0;
                        var h = Math.min(opts.max_height, iframe_height + body.scrollTop()) + 'px';
                        iframe.style['height'] = h;
                    }
                    return true;
                });

                return tb;
            };
            function isValidSecuredURL(_url) {
                return (_url.indexOf("http://") == 0 && _url.indexOf("javascript:") == -1);
            }
            function showPopupError(err) { hidePopupError(); $("#ppds h3").after("<div class=\"notifications err\"><div class=\"wrp\"><span class=\"ico ico-error\"></span><ul><li class=\"li0\">" + err + "</li></ul></div></div>"); }
            function hidePopupError() { $("#ppds div.err").remove(); }

            function formatText(command, option) {
                iframe.contentWindow.focus();
                try { iframe.contentWindow.document.execCommand("styleWithCSS", false, false); } catch (e) { }
                try { iframe.contentWindow.document.execCommand(command, false, option); } catch (e) { }
                iframe.contentWindow.focus();
            };
            enableDesignMode();
        });
    };
})(jQuery);
