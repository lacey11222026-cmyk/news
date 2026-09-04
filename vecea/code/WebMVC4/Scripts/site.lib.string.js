

(function ($) {

    $.doubleSeparate = function (double, separation) {

        //var _price = price;
        if (double == 0 || double == undefined || double == null || double == '')
            return 0;

        var _separation = separation;
        if (_separation == undefined || _separation == null || _separation == '')
            _separation = '.';

        var strDouble = '' + double + '';
        var _double = strDouble.split('');
        var length = _double.length;

        for (var i = length; i > 0; i = i - 3) {
            if (i != length)
                _double.splice(i, 0, _separation);
        }

        return _double.join('');
    };

    $.formatLength = function (str, max) {

        str = $.trim(str);
        var length = str.length;

        if (length >= max) {

            str = str.substring(0, max - 3);
            str += '...';

        }

        return str;
    };


})(jQuery);

/**
*
*  Javascript trim, ltrim, rtrim
*  http://www.webtoolkit.info/
*
**/

function trimStr(str, chars)
{
    return ltrim(rtrim(str, chars), chars);
}

function ltrim(str, chars)
{
    chars = chars || "\\s";
    return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
}

function rtrim(str, chars)
{
    chars = chars || "\\s";
    return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
}


// ------------- thaitq End add 23/8/2010

function addslashes(str)
{
    str = str.replace(/\\/g, '\\\\');
    str = str.replace(/\'/g, '\\\'');
    str = str.replace(/\"/g, '\\"');
    str = str.replace(/\0/g, '\\0');
    return str;
}

function stripslashes(str)
{
    str = str.replace(/\\'/g, '\'');
    str = str.replace(/\\"/g, '"');
    str = str.replace(/\\0/g, '\0');
    str = str.replace(/\\\\/g, '\\');
    return str;
}