(function($) {
    $.fn.validateForm = function(n) {
        var p = [];
        this.find("input,select,textarea").each(function() {
            var w = $(this).validateField();
            if (w) p.push(w);
        });
        return p;
    };
    $.fn.attachFieldRestrictions = function() {
        this.each(function() {
            if ($(this).attr("validators")) {
                var v = $(this).attr("validators").split("|");
                for (var i = 0; i < v.length; i++) {
                    var w = $validators[v[i]];
                    if (w && w.restrict) w.restrict($(this));
                }
            }
        });
        return this;
    };
    $.fn.validateField = function(n) {
        if ($(this).attr("validators")) {
            var v = $(this).attr("validators").split("|");
            for (var i = 0; i < v.length; i++) {
                var w = $validators[v[i]];
                if (w && w.test && w.test(this, $(this).fieldValue()) != true) {
                    return { key: $(this).attr("name"), val: (($locale[w.msg]) ? __formatFailedMessage($locale[w.msg], this, $(this).fieldValue()) : "") };
                }
            }
        }
        return null;
    };
    $.fn.defaultRestrict = function(length, reg) {
        var f = function(e) {
            var v = $(this).val();
            if (e.which && isCommandCharacter(e.which) == false && (v.length >= length || (reg && isCharacterMatch(e.which, reg)))) e.preventDefault();
            var w = v;
            if (reg) w = removeUnmatchedCharacters(w, reg);
            w = trimString(w, length);
            if (w != v) $(this).val(w);
        };
        $(this).keypress(f).keyup(f).change(f);
        return this;
    };

    __formatFailedMessage = function(s, fld, val) {
        var name = $(fld).attr("vname") || $(fld).attr("name");
        s = s.replace(/\$\(name\)/gi, name).replace(/\$\(value\)/gi, val);
        for (var i = 1; i < 5; i++) {
            s = s.replace(new RegExp("\\$\\(arg" + i + "\\)", "gi"), $(fld).attr("arg" + i) || "");
        }
        return s;
    };

})(jQuery);

function isCommandCharacter(c) {
    return (c != 0 && c != 8 && c != 9 && c != 13 && c != 35 && c != 36 && c != 37 && c != 39 && c != 46) == false;
}

function isCharacterMatch(c, reg) {
    return String.fromCharCode(c).match(reg) != null;
}
function removeUnmatchedCharacters(str, reg)
{
    return str.replace(reg, "");
}
function trimString(s, l) {
    if (s) return s.substring(0,Math.min(s.length,l));
    return s;
}

$(document).ready(function() {
    $("input,select,textarea").attachFieldRestrictions();
});