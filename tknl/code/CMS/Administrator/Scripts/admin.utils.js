/**
*
*  Base64 encode / decode
*  http://www.webtoolkit.info/
*
**/

var Base64 = {

    // private property
    _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",

    // public method for encoding
    encode: function (input) {
        var output = "";
        var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
        var i = 0;

        input = Base64._utf8_encode(input);

        while (i < input.length) {

            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);

            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;

            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }

            output = output +
			this._keyStr.charAt(enc1) + this._keyStr.charAt(enc2) +
			this._keyStr.charAt(enc3) + this._keyStr.charAt(enc4);

        }

        return output;
    },

    // public method for decoding
    decode: function (input) {
        var output = "";
        var chr1, chr2, chr3;
        var enc1, enc2, enc3, enc4;
        var i = 0;

        input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

        while (i < input.length) {

            enc1 = this._keyStr.indexOf(input.charAt(i++));
            enc2 = this._keyStr.indexOf(input.charAt(i++));
            enc3 = this._keyStr.indexOf(input.charAt(i++));
            enc4 = this._keyStr.indexOf(input.charAt(i++));

            chr1 = (enc1 << 2) | (enc2 >> 4);
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
            chr3 = ((enc3 & 3) << 6) | enc4;

            output = output + String.fromCharCode(chr1);

            if (enc3 != 64) {
                output = output + String.fromCharCode(chr2);
            }
            if (enc4 != 64) {
                output = output + String.fromCharCode(chr3);
            }

        }

        output = Base64._utf8_decode(output);

        return output;

    },

    // private method for UTF-8 encoding
    _utf8_encode: function (string) {
        string = string.replace(/\r\n/g, "\n");
        var utftext = "";

        for (var n = 0; n < string.length; n++) {

            var c = string.charCodeAt(n);

            if (c < 128) {
                utftext += String.fromCharCode(c);
            }
            else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            }
            else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }

        return utftext;
    },

    // private method for UTF-8 decoding
    _utf8_decode: function (utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;

        while (i < utftext.length) {

            c = utftext.charCodeAt(i);

            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            }
            else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            }
            else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }

        }

        return string;
    }

}


// Strip HTML Tags (form) script- By JavaScriptKit.com (http://www.javascriptkit.com)
// For this and over 400+ free scripts, visit JavaScript Kit- http://www.javascriptkit.com/
// This notice must stay intact for use

function stripHTML(inputString) {
    var re = /<\/?[^>]+>/gi;
    return inputString.replace(re, "");
}

function trimEmpty(inputString, separate) {
    var re = /\s/g;
    var _separate = " ";
    if (separate != undefined && separate != null)
        _separate = separate;

    return inputString.replace(re, _separate);
}


/*
* Date Format 1.2.3
* (c) 2007-2009 Steven Levithan <stevenlevithan.com>
* MIT license
*
* Includes enhancements by Scott Trenda <scott.trenda.net>
* and Kris Kowal <cixar.com/~kris.kowal/>
*
* Accepts a date, a mask, or a date and a mask.
* Returns a formatted version of the given date.
* The date defaults to the current date/time.
* The mask defaults to dateFormat.masks.default.
*/

var dateFormat = function () {
    var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
		timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
		timezoneClip = /[^-+\dA-Z]/g,
		pad = function (val, len) {
		    val = String(val);
		    len = len || 2;
		    while (val.length < len) val = "0" + val;
		    return val;
		};

    // Regexes and supporting functions are cached through closure
    return function (date, mask, utc) {
        var dF = dateFormat;

        // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
        if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
            mask = date;
            date = undefined;
        }

        // Passing date through Date applies Date.parse, if necessary
        date = date ? new Date(date) : new Date;
        if (isNaN(date)) throw SyntaxError("invalid date");

        mask = String(dF.masks[mask] || mask || dF.masks["default"]);

        // Allow setting the utc argument via the mask
        if (mask.slice(0, 4) == "UTC:") {
            mask = mask.slice(4);
            utc = true;
        }

        var _ = utc ? "getUTC" : "get",
			d = date[_ + "Date"](),
			D = date[_ + "Day"](),
			m = date[_ + "Month"](),
			y = date[_ + "FullYear"](),
			H = date[_ + "Hours"](),
			M = date[_ + "Minutes"](),
			s = date[_ + "Seconds"](),
			L = date[_ + "Milliseconds"](),
			o = utc ? 0 : date.getTimezoneOffset(),
			flags = {
			    d: d,
			    dd: pad(d),
			    ddd: dF.i18n.dayNames[D],
			    dddd: dF.i18n.dayNames[D + 7],
			    m: m + 1,
			    mm: pad(m + 1),
			    mmm: dF.i18n.monthNames[m],
			    mmmm: dF.i18n.monthNames[m + 12],
			    yy: String(y).slice(2),
			    yyyy: y,
			    h: H % 12 || 12,
			    hh: pad(H % 12 || 12),
			    H: H,
			    HH: pad(H),
			    M: M,
			    MM: pad(M),
			    s: s,
			    ss: pad(s),
			    l: pad(L, 3),
			    L: pad(L > 99 ? Math.round(L / 10) : L),
			    t: H < 12 ? "a" : "p",
			    tt: H < 12 ? "am" : "pm",
			    T: H < 12 ? "A" : "P",
			    TT: H < 12 ? "AM" : "PM",
			    Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
			    o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
			    S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
			};

        return mask.replace(token, function ($0) {
            return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
        });
    };
}();

// Some common format strings
dateFormat.masks = {
    "default": "ddd mmm dd yyyy HH:mm:ss",
    shortDate: "m/d/yy",
    mediumDate: "mmm d, yyyy",
    longDate: "mmmm d, yyyy",
    fullDate: "dddd, mmmm d, yyyy",
    shortTime: "h:MM TT",
    mediumTime: "h:MM:ss TT",
    longTime: "h:MM:ss TT Z",
    isoDate: "yyyy-mm-dd",
    isoTime: "HH:mm:ss",
    isoDateTime: "yyyy-mm-dd'T'HH:mm:ss",
    isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:mm:ss'Z'"
};

// Internationalization strings
dateFormat.i18n = {
    dayNames: [
		"Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
		"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
    ],
    monthNames: [
		"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
		"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
    ]
};

// For convenience...
Date.prototype.format = function (mask, utc) {
    return dateFormat(this, mask, utc);
};

function Log(data) {
    var browser = $.browser;

    if (browser.mozilla)
        console.log(data);
    else
        return;
}

function DestroyJAlert() {
    $('#jalert_box_cont_edit').remove();
}

function GetImageUrl(id, entity, isthumb) {
    var url = '';

    url = IMAGE_UPLOAD_URL + entity + '/' + Math.floor(id / 1000000) + '/' + Math.floor(id / 1000) + '/' + id + '/';
    if (isthumb)
        url += 'Thumb/';

    return url;
}

function OpenUiLightBox(html, width) {
    var rightPosition = Math.ceil(screen.width / 3.5);
    var topPosition = Math.ceil(screen.height / 5.5);
    if (navigator.appName == "Microsoft Internet Explorer") {
        rightPosition = rightPosition - 90;
        topPosition = topPosition - 84;
    }

    if (!($("#social_lightbox").length > 0))
        $("#" + form1ClientID).append("<div id=\"social_lightbox\"></div>");

    var social_lightbox = $("#social_lightbox");

    DestroyDialogByElm(social_lightbox);

    $(social_lightbox).html(html);

    $(social_lightbox).dialog({
        modal: true,
        //buttons: buttons,
        closeOnEscape: true,
        //title: title,
        width: width,
        closeText: false,
        position: [rightPosition, topPosition],
        show: "clip",
        hide: "clip",
        //position: 'center',
        close: function (event, ui) {
            DestroyDialogByElm(social_lightbox);
        }
    });

    $('.ui-widget-overlay').css({
        //'background': 'url("' + MEDIA_STATIC_URL + 'Images/backgrounds/ui-bg_flat_0_aaaaaa_40x100.png") repeat-x scroll 100% 100% #000000',
        'background-color': '#000000',
        'opacity': '0.8'
    }).click(function () {
        DestroyDialogByElm(social_lightbox);
    });

    $('.ui-dialog .ui-dialog-content').css({
        'border': '0'
    });

    $('.ui-dialog .ui-widget .ui-widget-content .ui-corner-all .ui-draggable .ui-resizable').css({
        'z-index': '9999'
    });
}


// render datetime picker - jquery ui
function RenderDateTimePicker(elm, val, format) {
    if (elm == null || elm == undefined)
        return;
    var _format = format;
    if (format == null || format == undefined || format == '')
        _format = 'dd/mm/yy';

    $(elm).datepicker({
        showOn: "button",
        buttonImage: "/Administrator/Images/calendar.gif",
        buttonImageOnly: true,
        dateFormat: _format,
        setDate: val
    });

}

// render dialog - jquery ui
function OpenDialog(modal, buttons, title, msg, width, tempurl) {
    var rightPosition = Math.ceil(screen.width / 2.7);
    var topPosition = Math.ceil(screen.height / 5.5);
    if (navigator.appName == "Microsoft Internet Explorer") {
        rightPosition = rightPosition - 90;
        topPosition = topPosition - 84;
    }

    DestroyDialog();

    var html = "<div id=\"lava-dialog\" title=\"" + title + " \">";
    html += "</div>";
    if (!($("#lava-dialog").length > 0))
        $("#bg_content").append(html);

    var content = msg;
    //content += "<div id=\"lava-dialog-logo\"></div>";

    $("#lava-dialog").html(content);

    if (tempurl != undefined && tempurl != null)
        $("#lava-dialog").load(tempurl);

    $("#lava-dialog").dialog({
        //modal: modal,
        autoOpen: true,
        buttons: buttons,
        // closeOnEscape: true,
        title: title,
        width: width,
        //closeText: true,
        position: [rightPosition, topPosition]

    });

    $(".ui-dialog-titlebar-close").each(function (i, e) {
        $(e).remove();
    });
}

function OpenLoadingDialog(text) {

    DestroyDialog();

    var msg = "Đang tải dữ liệu...";
    if (text != undefined && text != null && text != '')
        msg = text;

    var html = "<div id=\"lava-dialog\">";
    html += "</div>";
    if (!($("#lava-dialog").length > 0))
        $("#" + form1ClientID).append(html);

    var content = msg;

    $("#lava-dialog").html(content);

    $("#lava-dialog").dialog({
        modal: true,
        buttons: {},
        closeOnEscape: false,
        title: "Thông báo",
        width: 400,
        closeText: false,
        close: function (event, ui) {
            DestroyDialog();
        }
    });

    $(".ui-dialog-titlebar-close").each(function (i, e) {
        $(e).remove();
    });
}

function OpenAlertDialog(message, title, width) {
    DestroyDialog();

    var alertOkbuttons = {
        "Đóng": function () {
            DestroyDialog();
        }
    };

    var _title = "Thông báo";

    if (title != null && title != undefined && title != "")
        _title = title;

    var _width = 400;
    if (width != undefined || width != null)
        _width = width;

    var msg = "<div align=\"center\" style=\"padding:5px\">" + message + "</div>";

    OpenDialog(true, alertOkbuttons, _title, msg, _width);
}

function OpenConfirmDialog(message, confirmFunc, cancelFunc) {
    DestroyDialog();

    var buttons = {
        "Hủy": function () {
            $(this).dialog("destroy");
            if (cancelFunc != undefined && cancelFunc != null)
                cancelFunc.call();
        },
        "Đồng ý": function () {
            $(this).dialog("destroy");
            if (confirmFunc != undefined && confirmFunc != null)
                confirmFunc.call();
        }
    };

    var msg = "<div align=\"center\" style=\"padding:5px\">" + message + "</div>";

    OpenDialog(true, buttons, "Xác nhận", msg, 400);
}

function DestroyDialog() {
    $("#lava-dialog").dialog("close");
}

function DestroyDialogByElm(elm) {
    $(elm).dialog("destroy");
}

function Clear(elm) {
    $('#' + elm).val('');
}


function ClearSearchText(element) {
    $(element).attr("value", "");
}

// Check all checkbox
function CheckAll(element) {
    var isChecked;
    isChecked = $(element).attr("checked");
    $(".adminlist").find("input[type='checkbox']").each(function (i, e) {
        $(this).attr("checked", isChecked);
    });
}

function AppendExtendProductAttribute(elm, prefix) {
    var html = '<tr>';
    html += '<td class="key">';
    html += '<label for="">';
    html += '<input id="txtLabel' + prefix + '" class="inputbox" type="text" />';
    html += '</label>';
    html += '</td>';
    html += '<td>';
    html += '<input id="txt' + prefix + '" class="inputbox" type="text" /><a href="#">Xóa</a>';
    html += '</td>';
    html += '</tr>';

    $(elm).parent().parent().before(html);
}


// Hàm ứng với TinyMCE version 3.x
function LoadRichTextEditor(elm, w, h, th, b2, b3, b4) {

    var _w = '800';
    if (w != undefined && w != null && w != '')
        _w = w;
    var _h = '600';
    if (h != undefined && h != null && h != '')
        _h = h;

    var _th = "advanced";
    if (th != undefined && th != null)
        _th = th;

    var _b2 = "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,removeformat,table,delete_table,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor";
    if (b2 != undefined && b2 != null)
        _b2 = b2;

    var _b3 = "removeformat";
    //var _b3 = "";
    if (b3 != undefined && b3 != null)
        _b3 = b3;

    //var _b4 = "insertlayer,moveforward,movebackward,absolute,|,styleprops,spellchecker,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,blockquote,pagebreak,|,insertfile,insertimage";
    var _b4 = "";
    if (b4 != undefined && b4 != null)
        _b4 = b4;

    tinyMCE.init({
        // General options
        mode: "exact",
        elements: elm,
        width: _w,
        height: _h,
        theme: _th,
        plugins: "ajaxUpload,media,table,youtubeIframe,wordcount",
        //        plugins: "autolink,lists,spellchecker,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template",
        // Theme options
        theme_advanced_buttons1: "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,styleselect,formatselect,fontselect,fontsizeselect",
        theme_advanced_buttons2: _b2,
        theme_advanced_buttons3: _b3,
        theme_advanced_buttons4: _b4,
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "left",
        theme_advanced_statusbar_location: "bottom",
        theme_advanced_resizing: true

        // Skin options
        //        skin: "o2k7",
        //        skin_variant: "silver",

        // Example content CSS (should be your site CSS)
        //content_css: "css/example.css",

        // Drop lists for link/image/media/template dialogs
        //template_external_list_url: "js/template_list.js",
        //external_link_list_url: "js/link_list.js",
        //external_image_list_url: "js/image_list.js",
        //media_external_list_url: "js/media_list.js"

    });
}

// Hàm ứng với TinyMCE version 4.x
//function LoadRichTextEditor(elm, w, h, th, b2, b3, b4) {

//    var _w = '800';
//    if (w != undefined && w != null && w != '')
//        _w = w;
//    var _h = '600';
//    if (h != undefined && h != null && h != '')
//        _h = h;

//    tinymce.init({
//        selector: '#' + elm,
//        theme: "modern",
//        language: 'vi',
//        width: _w,
//        height: _h,
//        plugins: [
//            "advlist autolink lists link image charmap print preview hr anchor pagebreak",
//            "searchreplace wordcount visualblocks visualchars code fullscreen",
//            "insertdatetime media nonbreaking save table contextmenu directionality",
//            "emoticons template paste textcolor colorpicker textpattern"
//        ],
//        toolbar1: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify",
//        toolbar2: "link image media | print preview | forecolor backcolor | bullist numlist outdent indent",
//        image_advtab: true,

//        external_plugins: {
//            'moxiemanager': 'http://cms.vneec.gov.vn/moxiemanager/plugin.min.js'
//        }
//    });
//}
// Replaces all instances of the given substring.
