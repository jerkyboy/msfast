String.prototype.matchs = function(re)
{
    var m = this.match(re);
    var res = [];

    if (m)
    {
        var l = 0;
        var s = re.toString();

        for (var i = 0; i < m.length; i++) {
            res.push({ index: this.indexOf(m[i], l), length: m[i].length, value: m[i] });
            l = res[res.length - 1].index + res[res.length - 1].length;
        }
    }
    
    return res;
};