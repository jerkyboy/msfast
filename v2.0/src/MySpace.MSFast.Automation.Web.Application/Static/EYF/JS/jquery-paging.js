(function($) {
    $.fn.browseForm = function(options) {
        var frm = this;
        var last = frm.find("input[name=h]").val();
        if(last){
            last = str2xml(last);
            if(last && options.cntr){
                var cb = function(){
                    frm.trigger("browseDataLoaded", frm);
                };
                $(options.cntr).renderLoadedControl($(last), cb);
            }
        }
        
        frm.attr("container",options.cntr);
        
        this.find(".srt").click(function(e) {
            e.preventDefault();
            var brwProp = frm.parseBrowseForm();
            var s = $(this).attr("sort");
            var o = brwProp.sort[s];
            brwProp.sort = {};
            brwProp.sort[s] = ((o == "1") ? "0" : "1");
            brwProp.index = 0;
            $(this).attr("asc", (brwProp.sort[s] == "1").toString());
            frm.resetBrowseParameters(brwProp);
            frm.reloadBrowseResults();
        });

        this.find(".next").click(function(e) {
            e.preventDefault();
            frm.nextPage();
        });
        this.find(".prev").click(function(e) {
            e.preventDefault();
            frm.prevPage();
        });
        this.find(".appendnext").click(function(e) {
            e.preventDefault();
            frm.nextPage(true);
        });

        this.resetBrowseView();
        return this;
    };

    $.fn.nextPage = function(append) {
        if (this.canBrowseNext()) {
            var brwProp = this.parseBrowseForm();
            brwProp.index = Math.min(brwProp.index + brwProp.length, brwProp.total);
            this.resetBrowseParameters(brwProp);
            this.reloadBrowseResults(append);
        }
    };

    $.fn.prevPage = function() {
        if (this.canBrowsePrev()) {
            var brwProp = this.parseBrowseForm();
            brwProp.index = Math.max(0, brwProp.index - brwProp.resperp);
            this.resetBrowseParameters(brwProp);
            this.reloadBrowseResults();
        }
    };

    $.fn.resetBrowseView = function() {
        if (this.canBrowsePrev()) this.find(".prev").show();
        else this.find(".prev").hide();

        if (this.canBrowseNext()) this.find(".next,.appendnext").show();
        else this.find(".next,.appendnext").hide();

        var frm = this;

        var brwProp = frm.parseBrowseForm();
        
        if(brwProp.total == 0) this.find(".count").hide();
        else this.find(".count").show().html($locale["showing_res"] + " " + (brwProp.index + 1) + " - " + (brwProp.index + brwProp.length) + ((brwProp.total > (brwProp.index + brwProp.length)) ? (" " + $locale["showing_outof"] + " " + brwProp.total) : ""));
        
        var pag = this.find(".pages");
        var lnkc = 2;
        
        if (pag.length > 0) {
            
            var pages = Math.ceil(brwProp.total / brwProp.resperp);
            var current = Math.floor(brwProp.index / brwProp.resperp);
            
            pag.html("");
            
            if (pages > 1) {
                var htl = "";
                
                var st = (current + Math.max(-current, -lnkc));
                var to = Math.min(st + (lnkc * 2), pages);
                var _getlnk = function(_id, _cl) { return "<a href=\"#\" page=\"" + _id + "\"" + ((_cl) ? " class=\"" + _cl + "\"" : "") + ">" + (_id+1) + "</a>"; }
                
                st = Math.max(0,to - (lnkc * 2));
                
                
                for (var i = st; i < to; i++) {
                    if (current != i) {
                        htl += _getlnk(i);
                    } else {
                        htl += _getlnk(i, "cr");
                    }
                }
                
                if (to < pages - 1) htl += "<span>...</span>" + _getlnk(pages-1);
                else if (to < pages) htl += _getlnk(pages - 1);
                
                if (st >= lnkc) htl = _getlnk(0) + "..." + htl;
                else if (st + 1 == lnkc) htl = _getlnk(0) + htl;
                
                pag.html(htl).find("a").click(function(e) {
                    e.preventDefault();
                    var brwProp = frm.parseBrowseForm();
                    brwProp.index = ($(this).attr("page") * brwProp.resperp);
                    frm.resetBrowseParameters(brwProp);
                    frm.reloadBrowseResults();
                });
            }
        }

        this.find(".srt").each(function() {
            var n = $(this).attr("sort");
            $(this).removeClass("slc");
            $(this).removeClass("dsc");
            $(this).removeClass("asc");
            $(this).addClass(($(this).attr("asc") == "true") ? "asc" : "dsc");
            if (n && brwProp.sort[n]) {
                $(this).addClass("slc");
            }
        });
    };

    $.fn.resetBrowseParameters = function(options) {
        var brwProp = $.extend(this.parseBrowseForm(), options);
        this.resetBrowseForm(brwProp);
    };


    $.fn.parseBrowseForm = function() {
        return {
            index: parseInt(this.children("input[name=i]").val()),
            length: parseInt(this.children("input[name=l]").val()),
            resperp: parseInt(this.children("input[name=r]").val()),
            total: parseInt(this.children("input[name=t]").val()),
            sort: ___vk(this.children("input[name=s]").val()),
            args: ___vk(this.children("input[name=f]").val()),
            popTotal: (this.children("input[name=p]").val() === "true"),
            browseType: this.children("input[name=b]").val()
        };
    };

    $.fn.removePagingItem = function(_it) {
        if (_it) {
            _it.remove();
            this.resetBrowseView();
            var brwProp = this.parseBrowseForm();
            brwProp.total = Math.max(0, brwProp.total - 1);
            brwProp.length = Math.max(0, brwProp.length - 1);

            this.resetBrowseParameters(brwProp);
            this.resetBrowseView();

            if (brwProp.length == 0 && brwProp.total > 0) {
                if (this.canBrowsePrev()) {
                    this.prevPage();
                } else {
                    this.reloadBrowseResults();
                }
            }
        }
    };

    $.fn.addPagingItem = function(_it) {
        if (this.attr("container") && _it) {
            $(this.attr("container")).prepend(_it);
            var brwProp = this.parseBrowseForm();
            brwProp.length++;
            brwProp.total++;
            this.resetBrowseParameters(brwProp);
            this.resetBrowseView();
        }
    };

    $.fn.displayedItemsCount = function() {
        return this.parseBrowseForm().length;
    };

    $.fn.totalItemsCount = function() {
        return this.parseBrowseForm().total;
    };

    $.fn.resetBrowseForm = function(brwProp) {
        this.children("input[name=i]").val(brwProp.index);
        this.children("input[name=t]").val(brwProp.total);
        this.children("input[name=l]").val(brwProp.length);
        this.children("input[name=r]").val(brwProp.resperp);
        this.children("input[name=p]").val(brwProp.popTotal);
        this.children("input[name=s]").val(___str(brwProp.sort));
        this.children("input[name=f]").val(___str(brwProp.args));
        this.children("input[name=b]").val(brwProp.browseType);
        this.children("input[name=h]").val(brwProp.lastresults);
    }

    $.fn.canBrowseNext = function() {
        var brwProp = this.parseBrowseForm();
        return brwProp.index + brwProp.length < brwProp.total;
    }

    $.fn.canBrowsePrev = function() {
        var brwProp = this.parseBrowseForm();
        return brwProp.index > 0;
    }

    $.fn.reloadBrowseResults = function(append) {
        this.find("input[name=h]").val("");
        if(append) this.attr("append","1");
        else this.attr("append",""); 
        $(document).trigger("startWorking", this);
        this.submit();
    };

    $.fn.setNextHref = function(h) {
        this.attr("action", h);
    }

    $.fn.setBrowseArg = function(a, v) {
        var brwProp = this.parseBrowseForm();
        brwProp.args[a] = v;
        this.resetBrowseForm(brwProp);
    }

    $.fn.getBrowseType = function() {
        var brwProp = this.parseBrowseForm();
        return brwProp.browseType;
    }

    $.fn.setBrowseType = function(v) {
        var brwProp = this.parseBrowseForm();
        brwProp.browseType = v;
        this.resetBrowseForm(brwProp);
    }

    $.fn.getBrowseArg = function(a) {
        var brwProp = this.parseBrowseForm();
        return brwProp.args[a];
    }

    function ___str(vk) {
        if (typeof vk === "string")
            return vk;
        var f = "";
        for (var k in vk) if (k) { if (f.length > 0) f += "|"; f += k + ":" + ((vk[k]) ? vk[k] : ""); }
        return f;
    }

    function ___vk(s) {
        if (!s) return {};
        var sr = s.split("|");
        var sa = "";
        var ob = {};
        for (var i = 0; i < sr.length; i++) {
            sa = sr[i].split(":");
            ob[sa[0]] = sa[1];
        }
        return ob;
    }

})(jQuery);

function xml2Str(xmlNode) {
   try {
      return (new XMLSerializer()).serializeToString(xmlNode);
  }
  catch (e) {
     try {
        return xmlNode.xml;
     }
     catch (e) {  
     }
   }
   return false;
}
function str2xml(string){
    try{
        return (new DOMParser()).parseFromString(string, 'text/xml');
    }catch(e){
        try{
            var doc = new ActiveXObject('Microsoft.XMLDOM');
            doc.async = 'false'
            doc.loadXML(string);
            return doc;
        }catch(e){
        }
    }
    return false;
}

function browseResultsReady(e, results) {

    if (results.data && results.kill != true){
    
        var xml = $(results.data);

        var i = parseInt(xml.find("index").text());
        var r = parseInt(xml.find("ResultsPerPage").text());
        var l = parseInt(xml.find("length").text());
        var t = parseInt(xml.find("total").text());
        var s = xml.find("sort").text();
        var f = xml.find("args").text();
        var b = xml.find("type").text();
        var h = xml2Str(results.data);
        
        var op = {};
        if (i >= 0) op["index"] = i;
        if (r >= 0) op["resperp"] = r;
        if (l >= 0) op["length"] = l;
        if (t >= 0) op["total"] = t;
        if (s) op["sort"] = s;
        if (f) op["args"] = f;
        if (b) op["browseType"] = b;
        if (h) op["lastresults"] = h;

        op["popTotal"] = false;

        results.frm.resetBrowseParameters(op);

        var cb = function() {
            results.frm.resetBrowseView();
            results.frm.trigger("browseDataLoaded", results.frm);
        };

        if (e.data) {
            if (e.data.html) {
                e.data.renderLoadedControl(xml, cb, results.frm.attr("append"));
            }
            else
            {
                var rob = results.frm.find(e.data);
                if(rob.length > 0){
                    rob.renderLoadedControl(xml, cb, results.frm.attr("append"));
                }else{
                    $(e.data).renderLoadedControl(xml, cb, results.frm.attr("append"));
                }
            }
        } else {
            cb();
        }
    }
    $(document).trigger("doneWorking", ((results) ? results.frm : undefined));
}
