// Chặn tấn công CSRF (XSRF)
function addAntiForgeryToken(data) {
    if (!data) {
        data = {};
    }
    var tokenInput = $('input[name=__RequestVerificationToken]');
    if (tokenInput.length) {
        data.__RequestVerificationToken = tokenInput.val();
    }
    return data;
};
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
} ();

// Some common format strings
dateFormat.masks = {
    "default": "ddd mmm dd yyyy HH:MM:ss",
    shortDate: "m/d/yy",
    mediumDate: "mmm d, yyyy",
    longDate: "mmmm d, yyyy",
    fullDate: "dddd, mmmm d, yyyy",
    shortTime: "h:MM TT",
    mediumTime: "h:MM:ss TT",
    longTime: "h:MM:ss TT Z",
    isoDate: "yyyy-mm-dd",
    isoTime: "HH:MM:ss",
    isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
    isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
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


function OpenPopUp(src, href) {

    //var html = '<link href="Scripts/Popup/popup.css" rel="stylesheet" type="text/css" />';
    var html = '';
    html += '<div id="popupContact">';
    html += '<a href="javascript:void(0);" id="popupContactClose">Đóng lại</a>';
    html += '<p id="contactArea">';
    html += '<a target="_blank" href="' + href + '">';
    html += '<img alt="" src="' + src + '">';
    html += '</a>';
    html += '</p>';
    html += '</div>';
    html += '<div id="backgroundPopup"></div>';
    $('#' + form1ClientID).append(html);

    //load popup
    loadPopup();

    //LOADING POPUP
    //centerPopup();
    
    //CLOSING POPUP
    //Click the x event!
    $("#popupContactClose").click(function () {
        disablePopup();
    });
    //Click out event!
    $("#backgroundPopup").click(function () {
        disablePopup();
    });
    //Press Escape event!
    $(document).keypress(function (e) {
        if (e.keyCode == 27 && popupStatus == 1) {
            disablePopup();
        }
    });

}

var popupStatus = 0;
//loading popup with jQuery magic!
function loadPopup() {
    //loads popup only if it is disabled
    if (popupStatus == 0) {
        $("#backgroundPopup").css({
            "opacity": "0.8"
        });
        $("#backgroundPopup").fadeIn("slow");
        $("#popupContact").fadeIn("slow");
        popupStatus = 1;
    }
}
//disabling popup with jQuery magic!
function disablePopup() {
    //disables popup only if it is enabled
    if (popupStatus == 1) {
        $("#backgroundPopup").fadeOut("slow");
        $("#popupContact").fadeOut("slow");
        popupStatus = 0;
    }
}
//centering popup
function centerPopup() {
    //request data for centering
    var windowWidth = document.documentElement.clientWidth;
    var windowHeight = document.documentElement.clientHeight;
    var popupHeight = $("#popupContact").height();
    var popupWidth = $("#popupContact").width();
    Log(windowWidth);
    Log(popupWidth);
    //centering
    $("#popupContact").css({
        "position": "absolute",
        "top": windowHeight / 2 - popupHeight / 2,
        "left": windowWidth / 2 - popupWidth / 2
    });
    //only need force for IE6
    $("#backgroundPopup").css({
        "height": windowHeight
    });
}


function LoadJCorouselLite(elm, btnNext, btnPrev, btnGo) {

    var _btnNext = '';
    if (btnNext != undefined && btnNext != null && btnNext != '')
        _btnNext = btnNext;
    var _btnPrev = '';
    if (btnPrev != undefined && btnPrev != null && btnPrev != '')
        _btnPrev = btnPrev;
    var _btnGo = [];
    if (btnGo != undefined && btnGo != null && btnGo != '')
        _btnGo = btnGo;

    $(elm).jCarouselLite({
        auto: 3000,
        btnNext: _btnNext,
        btnPrev: _btnPrev,
        btnGo: _btnGo,
        speed: 500,
        visible: 1,
        afterEnd: function (a) {
            var index = $(a).find("a").attr("rel");

            $(elm + " > ol").find("li").each(function (i, e) {

                if ($(e).hasClass("current") && !$(e).hasClass(index)) {
                    $(e).removeClass("current");
                }
                else {
                    if ($(e).hasClass(index))
                        $(e).addClass("current");
                }

            });

        }
        //easing: "backout"
    });
}

function getCookie(c_name) {
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == c_name) {
            return unescape(y);
        }
    }
}

function setCookie(c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
    document.cookie = c_name + "=" + c_value;
}


function Log(data) {
    var browser = $.browser;

    if (browser.mozilla)
        console.log(data);
    else
        return;
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
        $("#" + form1ClientID).append(html);

    var content = msg;
    //content += "<div id=\"lava-dialog-logo\"></div>";

    $("#lava-dialog").html(content);

    if (tempurl != undefined && tempurl != null)
        $("#lava-dialog").load(tempurl);

    $("#lava-dialog").dialog({
        modal: modal,
        buttons: buttons,
        closeOnEscape: true,
        title: title,
        width: width,
        closeText: false,
        position: [rightPosition, topPosition],
        //position: 'center',
        close: function (event, ui) {
            $("#lava-dialog").dialog("destroy");
        }
    });

    $(".ui-dialog-titlebar-close").each(function (i, e) {
        $(e).remove();
    });
}

function OpenLoadingDialog() {
    DestroyDialog();

    var msg = "<div align=\"center\" style=\"padding:5px\">Đang tải dữ liệu...</div>";
    var html = "<div id=\"lava-dialog\">";
    html += "</div>";
    if (!($("#lava-dialog").length > 0))
        $("#" + form1ClientID).append(html);

    var content = msg;
    //content += "<div id=\"lava-dialog-logo\"></div>";
    $("#lava-dialog").html(content);

    $("#lava-dialog").dialog({
        modal: true,
        buttons: {},
        closeOnEscape: false,
        title: "Hệ thống đang xử lý",
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
    $("#lava-dialog").dialog("destroy");
}

function DestroyDialogByElm(elm) {
    $(elm).dialog("destroy");
}
