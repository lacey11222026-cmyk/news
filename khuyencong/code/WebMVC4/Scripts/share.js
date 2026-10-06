
/* Go share object define */
var GoShareObj = {
    isLoaded: false,
    popupTitle: "Chia sẻ liên kết lên Mạng Việt Nam - Go.vn",
    hlkElm: "mygo_hlk_sharelink",
    linkElm: "#mygo_hlk_sharelink",
    iframeElm: "mygo_iframe_sharelink",
    overlayElm: "mygo_overlay",
    popupElm: "mygo_popup_sharelink",
    popupContentElm: "mygo_popup_content",
    closeElm: "#btnCancel",
    mygoUrl: "http://my.go.vn/",
    mygoShareUrl: "http://my.go.vn/share.aspx?url=",
    UrlMedia: "http://static.gox.vn/media/homepage/",
    urlUICss: "css/go.share.ui.css",
    version: 9.2
};
var maxlengthUrlShare = 10000;
if (typeof GoShareContent == "undefined")
    var GoShareContent = {};
/* Go share object define */
/* function common */

/* get static media */
function mygo_appendCss(url) {
    var head = document.getElementsByTagName('head')[0];
    var link = document.createElement('link');
    link.rel = 'stylesheet';
    link.type = 'text/css';
    link.href = GoShareObj.UrlMedia + url + '?v=' + GoShareObj.version;
    head.appendChild(link);
}
function mygo_appendScript(url) {
    var head = document.getElementsByTagName('head')[0];
    var script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = GoShareObj.UrlMedia + url;
    head.appendChild(script);
}
String.prototype.removeHtmlGo = function () {
    return this.replace(/<(?:.|\n)*?>/gm, '');
};

function Go_createIframe(id, src, width, height) {
    var ifrm = document.createElement("iframe");
    ifrm.setAttribute("name", id);
    ifrm.setAttribute("id", id);
    ifrm.setAttribute("src", src);
    ifrm.style.width.value = width + "px";
    ifrm.style.height.value = height + "px";
    document.body.appendChild(ifrm);
}
/* Popup window size*/
var Go_PopupManager = {
    getCenterCoords: function (width, height) {
        var parentPos = this.getParentCoords();
        var parentSize = this.getWindowInnerSize();
        var xPos = parentPos.width + Math.max(0, Math.floor((parentSize.width - width) / 2));
        var yPos = parentPos.height + Math.max(0, Math.floor((parentSize.height - height) / 2));
        return { x: xPos, y: yPos };
    },
    getWindowInnerSize: function () {
        var w = 0;
        var h = 0;

        if ('innerWidth' in window) {
            /* For non-IE */
            w = window.innerWidth;
            h = window.innerHeight;
        } else {
            /* For IE*/
            var elem = null;
            if (('BackCompat' === window.document.compatMode) && ('body' in window.document)) {
                elem = window.document.body;
            } else if ('documentElement' in window.document) {
                elem = window.document.documentElement;
            }
            if (elem !== null) {
                w = elem.offsetWidth;
                h = elem.offsetHeight;
            }
        }
        return { width: w, height: h };
    },
    getParentCoords: function () {
        var w = 0;
        var h = 0;

        if ('screenLeft' in window) {
            /* IE-compatible variants*/
            w = window.screenLeft;
            h = window.screenTop;
        } else if ('screenX' in window) {
            /* Firefox-compatible*/
            w = window.screenX;
            h = window.screenY;
        }
        return { width: w, height: h };
    }
}
function Go_getScrollXY() {
    var scrOfX = 0, scrOfY = 0;
    if (typeof (window.pageYOffset) == 'number') {
        //Netscape compliant
        scrOfY = window.pageYOffset;
        scrOfX = window.pageXOffset;
    } else if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
        //DOM compliant
        scrOfY = document.body.scrollTop;
        scrOfX = document.body.scrollLeft;
    } else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
        //IE6 standards compliant mode
        scrOfY = document.documentElement.scrollTop;
        scrOfX = document.documentElement.scrollLeft;
    }
    return [scrOfX, scrOfY];
}
function Go_GetDocumentWidth() {
    var x = 0;
    if (self.innerHeight) {
        x = self.innerWidth;
    }
    else if (document.documentElement && document.documentElement.clientHeight) {
        x = document.documentElement.clientWidth;
    }
    else if (document.body) {
        x = document.body.clientWidth;
    }
    return x;
}

function Go_GetDocumentHeight() {
    var y = 0;
    if (document.height) {
        y = document.height;
    }
    else if (self.innerHeight) {
        y = self.innerHeight;
    }
    else if (document.body) {
        y = document.body.clientHeight;
    }
    else if (document.documentElement && document.documentElement.clientHeight) {
        y = document.documentElement.clientHeight;
    }

    return y;
}

function Go_onKeyPress(e) {
    var KeyID = (window.event) ? event.keyCode : e.keyCode;
    switch (KeyID) {
        case 27: /* escape */
            Go_closePopup();
            break;
    }
}
function Go_closePopup() {
    document.getElementById(GoShareObj.popupElm).style.display = "none";
    document.getElementById(GoShareObj.overlayElm).style.display = "none";
}
function Go_createPopup(id, title) {
    var h = '';
    h = '<div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix" style="background-color:#5C5C5C;">'
    + '<span class="ui-dialog-title" id="ui-dialog-title-mygo_popup_sharelink" title="Đóng">'
    + title + '</span><a href="javascript:void(0);" class="ui-dialog-titlebar-close ui-corner-all" role="button">'
    + '<span class="ui-icon ui-icon-closethick" onclick="Go_closePopup();">close</span></a></div>'
    + '<div style="width: auto; display: block; min-height: 0px; height: 269px;" id="' + id + '" class="ui-dialog-content ui-widget-content">'
    + '</div>';
    return h;
}
function Go_GetMetaValue(meta_name) {
    var my_arr = document.getElementsByTagName("META");
    for (var counter = 0; counter < my_arr.length; counter++) {
        if (my_arr[counter].name.toLowerCase() == meta_name.toLowerCase()) {
            return my_arr[counter].content;
        }
    }
    return "";
}
String.prototype.goSubWords = function (length) {
    if (this == "" || length <= 0) { return ""; }
    if (this.length <= length) { return this; }
    var trim = length;
    if (this[trim] != undefined) {
        while (trim > 0 & this[trim] != " ") { trim--; }
    }
    return this.substring(0, trim);
};

/* end function common */
/* Go share open window popup */
function bindHtml_mygoShareJs(redirect) {
    /*onclick="click_mygoShareJs(\'' + redirect + '\');" href="javascript:void(0);" */
    var h = '<a href="' + redirect + '" target="_blank" id="' + GoShareObj.hlkElm + '" title="' + GoShareObj.popupTitle + '">';
    h += '<img width="20" height="20" src="/images/share/go.vn20x20.gif" alt="Go.vn" style="border:none;">';
    h += '</a>';
    return h;
}
function click_mygoShareJs(redirect) {
    (function () {
        var d = document, f = redirect, l = d.location, e = encodeURIComponent, p = '&amp;title=' + e(d.title) + '&amp;description=&amp;picture=&amp;name=' + location.hostname;
        if (!window.open(f + p, 'sharer', 'toolbar=0,status=0,resizable=1,width=800,height=450')) l.href = f + p
    })();
}

/* end Go share open window popup */

/* Go share object, show popup dialog */

/* bind html nút share */
function bindHtml_mygoShare() {
    var h = '<a onclick="onclick_mygoShare();" href="javascript:void(0);" id="' + GoShareObj.hlkElm + '" title="' + GoShareObj.popupTitle + '">';
    //    if (typeof window.GoIconType !== "undefined")
    //        if (window.GoIconType == 1)
    //            h += '<img width="16" height="16" src="' + GoShareObj.mygoUrl + 'img/go.vn16x16.gif" alt="Go.vn" style="border:none;">';
    //        else
    //            h += '<img width="20" height="20" src="' + GoShareObj.mygoUrl + 'img/go.vn20x20.gif" alt="Go.vn" style="border:none;">';

    if (typeof window.GoIconType !== "undefined") {
        switch (GoIconType) {
            case 0:
                h += 'Loan tin';
                break;
            case 1:
                h += '<img width="16" height="16" src="/images/share/go.vn20x20.gif" alt="Go.vn" style="border:none;">';
                break;
            default:
                h += '<img width="20" height="20" src="/images/share/go.vn20x20.gif" alt="Go.vn" style="border:none;">';
                break;
        }
    }
    else
        h += '<img width="20" height="20" src="/images/share/go.vn20x20.gif" alt="Go.vn" style="border:none;">';
    h += '</a>';
    //  h += '<div id="' + GoShareObj.popupElm + '" style="display:none;"></div>';
    return h;
}

function init_mygoObjShare(elm, obj) {
    if (typeof obj != "undefined")
        init_mygoCustomShare(elm, obj);
    else
        init_mygoGetShare();
}
/* Go share không cần truyền tham số */
function init_mygoGetShare() {
    init_GoShareContent();
    document.write(bindHtml_mygoShare());
}
function init_GoShareContent() {
    GoShareContent = {
        link: document.location.href,
        message: '',
        picture: ''
        , title: document.title
        , description: Go_GetMetaValue('description')
        , name: window.location.hostname,
        elm: ''
    };
}
/* Go share truyền tham số */
function init_mygoCustomShare(elm, obj) {
    try {
        GoShareContent = {
            link: obj.link,
            message: obj.message,
            picture: obj.picture,
            title: (obj.title).removeHtmlGo(),
            description: (obj.description).removeHtmlGo(),
            name: (obj.name).removeHtmlGo(),
            elm: elm
        };
    }
    catch (ex) {
        init_GoShareContent();
    }
    if (document.getElementById(elm)) {
        document.getElementById(elm).innerHTML = bindHtml_mygoShare();
    }
    else document.write(bindHtml_mygoShare());
}
function init_govnSharePopup() {
    document.write(bindHtml_mygoShare());
}
function onclick_mygoShare() {

    var e = encodeURIComponent, esc = escape;
    if (navigator.userAgent.indexOf('MSIE 6') != -1) {
        redirect = GoShareObj.mygoShareUrl + esc(e(document.location.href));
        click_mygoShareJs(redirect);
        return;
    }
    if (typeof govn_link == "undefined") {
        var p = "?url=" + esc(e(GoShareContent.link))
		+ "&message=" + GoShareContent.message
		+ "&picture=" + esc(e(GoShareContent.picture))
		+ "&title=" + esc(e(GoShareContent.title))
		+ "&description=" + esc(e(GoShareContent.description))
		+ "&name=" + GoShareContent.name;

        redirect = GoShareObj.mygoUrl + "GoShare.aspx" + p;
        var description = GoShareContent.description;

        var overlengthUrl = redirect.length - maxlengthUrlShare;
        if (overlengthUrl > 0) {

            if (description)
                description = description.goSubWords(Math.abs(description.length - overlengthUrl));
            else description = "";

            redirect = GoShareObj.mygoUrl + "GoShare.aspx" + "?url="
			+ esc(e(GoShareContent.link))
			+ "&message=" + GoShareContent.message
			+ "&picture=" + esc(e(GoShareContent.picture))
			+ "&title=" + esc(e(GoShareContent.title))
			+ "&description=" + esc(e(description))
			+ "&name=" + GoShareContent.name;
        }
    }
    else {
        if (typeof govn_detectMedia == "undefined") { govn_detectMedia = 0; }

        var p = "?url=" + esc(e(govn_link))
		+ "&message=" + govn_message.removeHtmlGo()
		+ "&picture=" + esc(e(govn_picture))
		+ "&title=" + esc(e(govn_title.removeHtmlGo()))
		+ "&description=" + esc(e(govn_description.removeHtmlGo()))
		+ "&name=" + govn_name.removeHtmlGo()
        + "&detectMedia=" + govn_detectMedia;

        var redirect = GoShareObj.mygoUrl + "GoShare.aspx" + p;
    }
    var htmlIframe = '<iframe id="' + GoShareObj.iframeElm + '" scrolling="no" border="0" frameborder="0" src="' + redirect
    + '" style="height: 269px;width: 100%;border:none">';
    Go_displayIframe(htmlIframe);
    return false;
}
function Go_displayIframe(htmlIframe) {
    var widthPopup = 500, heightPopup = 300;

    if (GoShareObj.isLoaded) {
        if (navigator.userAgent.toLowerCase().indexOf('chrome') > -1) {
            mygo_appendCss(GoShareObj.urlUICss);
        }

        //  var heightWindow = Go_GetDocumentHeight();
        var widthWindow = Go_GetDocumentWidth();
        var windowInnerSize = Go_PopupManager.getWindowInnerSize();
        //   var center = Go_PopupManager.getCenterCoords(widthPopup, heightPopup);

        var offsetLeft = (widthWindow - widthPopup) / 2;
        var offsetTop = (windowInnerSize.height - heightPopup) / 2;
        var scroll = Go_getScrollXY();
        document.getElementById(GoShareObj.overlayElm).style.display = "block";
        var iframeContentObj = document.getElementById(GoShareObj.popupElm);
        iframeContentObj.style["top"] = offsetTop + scroll[1] + "px";
        iframeContentObj.style["left"] = offsetLeft + scroll[0] + "px";
        iframeContentObj.style.display = "block";
        // iframeContentObj.style.border = "1px solid #3A4045";

        iframeContentObj.setAttribute("class", "ui-dialog ui-widget ui-widget-content ui-corner-all GoVnUIDialog");
        iframeContentObj.style.position = "absolute";
        iframeContentObj.innerHTML = Go_createPopup(GoShareObj.popupContentElm, GoShareObj.popupTitle);
        document.getElementById(GoShareObj.popupContentElm).innerHTML = htmlIframe;
    }
    else {

        mygo_appendCss(GoShareObj.urlUICss);

        windowInnerSize = Go_PopupManager.getWindowInnerSize();
        scroll = Go_getScrollXY();

        var heightWindow = Go_GetDocumentHeight();
        widthWindow = Go_GetDocumentWidth();

        // var center = Go_PopupManager.getCenterCoords(widthPopup, heightPopup);

        offsetLeft = (windowInnerSize.width - widthPopup) / 2;

        offsetTop = (windowInnerSize.height - heightPopup) / 2;

        /*overlay div*/

        var overlayObj = document.createElement('div');
        overlayObj.setAttribute("id", GoShareObj.overlayElm);
        overlayObj.setAttribute("class", "ui-widget-overlay");
        overlayObj.style["width"] = widthWindow + "px";
        overlayObj.style["height"] = heightWindow + "px";
        document.body.appendChild(overlayObj);

        /*popup div*/
        iframeContentObj = document.createElement('div');

        iframeContentObj.style["top"] = offsetTop + scroll[1] + "px";
        iframeContentObj.style["left"] = offsetLeft + scroll[0] + "px";
        iframeContentObj.style["width"] = widthPopup + "px";

        iframeContentObj.setAttribute("id", GoShareObj.popupElm);
        iframeContentObj.setAttribute("class", "ui-dialog ui-widget ui-widget-content ui-corner-all GoVnUIDialog");

        iframeContentObj.style.display = "block";
        iframeContentObj.style.position = "absolute";
        iframeContentObj.innerHTML = Go_createPopup(GoShareObj.popupContentElm, GoShareObj.popupTitle);
        document.body.appendChild(iframeContentObj);

        /*popup iframe div*/
        document.getElementById(GoShareObj.popupContentElm).innerHTML = htmlIframe;

        GoShareObj.isLoaded = true;
    }
    document.onkeypress = Go_onKeyPress;
}
/*************************************************\
* end share len go vn 
**************************************************/

/* share len zing me */
var ZingMeShareObj = {
    shareUrl: "http://link.apps.zing.vn/share?",
    popupTitle: "Chia sẻ liên kết lên Zing Me",
    href: ""
}
function bindHtml_zingmeShare() {
    var h = '<a href="javascript:void(0);" onclick="click_zingmeShare();" title="'
    + ZingMeShareObj.popupTitle + '" style="text-decoration:none;margin-left:5px;">';
    if (typeof GoIconType !== "undefined") {
        switch (GoIconType) {
            case 0:
                h += 'Zing';
                break;
            case 1:
                h += '<img width="16" height="16" src="/images/share/big_zing_icon.png" alt="zingme" style="border:none;">';
                break;
            default:
                h += '<img width="20" height="20" src="/images/share/big_zing_icon.png" alt="zingme" style="border:none;">';
                break;
        }

    }
    //        if (GoIconType == 1)
    //            h += '<img width="16" height="16" src="' + GoShareObj.mygoUrl + 'img/small_zing_icon.png" alt="zingme" style="border:none;">';
    //        else
    //            h += '<img width="20" height="20" src="' + GoShareObj.mygoUrl + 'img/big_zing_icon.png" alt="zingme" style="border:none;">';
    else
        h += '<img src="/images/share/big_zing_icon.png" height="20px" width="20px" alt="" style="" />';
    h += '</a>';
    return h;
}
/* Zing me share object*/
function init_zingmeObjShare(e, obj) {
    if (typeof obj != "undefined")
        init_zingmeShare(e, obj);

}
function click_zingmeShare() {
    (function () {
        var d = document, f = ZingMeShareObj.href, l = d.location, e = encodeURIComponent;
        //p = 'u=' + e(l.href) + '&amp;title=' + e(d.title) + '&amp;source=govn';
        if (!window.open(f, 'sharer', 'toolbar=0,status=0,resizable=1,width=800,height=450')) l.href = f;
    })();
}
function init_zingmeShare(e, obj) {
    if (typeof e != "undefined") {
        if (typeof obj.link == "undefined") {
            obj.link = document.location.href;
        }
        var en = encodeURIComponent;
        ZingMeShareObj.href = ZingMeShareObj.shareUrl
            + "u=" + en(obj.link)
            + "&images=" + obj.picture
            + "&t=" + en(obj.title)
            + "&desc=" + en(obj.description)
            + "&media=" + "&width=0" + "&height=0";
        var h = bindHtml_zingmeShare();
        if (document.getElementById(e)) {
            document.getElementById(e).innerHTML += h;
        }
        else
            document.writeln(h);
    }
}
/* end share len zing me */

/* share len link hay */
var LinkhayShareObj = {
    shareUrl: "http://linkhay.com/submit",
    popupTitle: "Chia sẻ liên kết lên Link hay"
}
function click_linkhayShare() {
    (function () {
        var d = document, f = LinkhayShareObj.shareUrl, l = d.location, e = encodeURIComponent, p = '?url=' + e(l.href) + '&amp;title=' + e(d.title) + '&amp;utm_source=govn&amp;utm_medium=embedded&amp;utm_campaign=lh_button';
        if (!window.open(f + p, 'sharer', 'toolbar=0,status=0,resizable=1,width=800,height=450')) l.href = f + p
    })();
}
function bindHtml_linkhayShare() {
    var h = '<a href="javascript:void(0);" onclick="click_linkhayShare();" title="'
    + LinkhayShareObj.popupTitle + '" style="margin-left:5px;">';
    if (typeof GoIconType !== "undefined") {
        switch (GoIconType) {
            case 0:
                h += 'LinkHay';
                break;
            case 1:
                h += '<img width="16" height="16" src="/images/share/linkhay_button5.jpg" alt="linkhay" style="border:none;">';
                break;
            default:
                h += '<img width="20" height="20" src="/images/share/linkhay_button5.jpg" alt="linkhay" style="border:none;">';
                break;
        }
    }
    //        if (GoIconType == 1)
    //            h += '<img width="16" height="16" src="' + GoShareObj.mygoUrl + 'img/linkhay_button5.jpg" alt="linkhay" style="border:none;">';
    //        else
    //            h += '<img width="20" height="20" src="' + GoShareObj.mygoUrl + 'img/linkhay_button5.jpg" alt="linkhay" style="border:none;">';
    else
        h += '<img src="/images/share/linkhay_button5.jpg" width="20px" height="20px" alt="linkhay" />';
    h += '</a>';
    return h;
}
function init_linkhayShare(e) {
    if (typeof e != "undefined") {
        var h = bindHtml_linkhayShare();
        if (document.getElementById(e)) {
            document.getElementById(e).innerHTML += h;
        }
        else
            document.writeln(h);
    }
}
/* end share len link hay */

/*** share len facebook ***/
var FacebookShareObj = {
    homeUrl: "http://www.facebook.com/",
    feedAppUrl: "http://www.facebook.com/dialog/feed",
    shareUrl: "http://www.facebook.com/sharer/sharer.php",
    popupTitle: "Chia sẻ liên kết lên Facebook",
    urlIcon: "/images/share/icon_fb.gif"
}
function click_facebookShare() {
    var d = document, f = FacebookShareObj.feedAppUrl, l = d.location, e = encodeURIComponent;

    var p = '?app_id=132607496783912'
    + '&redirect_uri=' + FacebookShareObj.homeUrl
    + '&link=' + e(GoShareContent.link)
    + '&picture=' + GoShareContent.picture
    + '&description=' + e(GoShareContent.description)
    + '&name=' + e(GoShareContent.title)
    + '&caption=' + GoShareContent.name
    + '&display=page';
    if (!window.open(f + p, 'sharer', 'toolbar=0,status=0,resizable=1,width=800,height=450')) l.href = f + p;
}
function bindHtml_facebookShare() {
    var h = '<a href="javascript:void(0);" onclick="click_facebookShare();" title="'
    + FacebookShareObj.popupTitle + '" style="margin-left: 5px; margin-right: 5px;">';
    if (typeof GoIconType !== "undefined") {
        switch (GoIconType) {
            case 0:
                h += 'Facebook';
                break;
            case 1:
                h += '<img width="16" height="16" src="' + FacebookShareObj.urlIcon + '" alt="Facebook" style="border:none;">';
                break;
            default:
                h += '<img width="20" height="20" src="' + FacebookShareObj.urlIcon + '" alt="Facebook" style="border:none;">';
                break;
        }
        //        if (GoIconType == 1)
        //            h += '<img width="16" height="16" src="' + FacebookShareObj.urlIcon + '" alt="Facebook" style="border:none;">';
        //        else
        //            h += '<img width="20" height="20" src="' + FacebookShareObj.urlIcon + '" alt="Facebook" style="border:none;">';
    }
    else
        h += '<img src="' + FacebookShareObj.urlIcon + '" width="20px" height="20px" alt="Facebook" />';
    h += '</a>';
    return h;
}
function init_facebookShare(e, obj) {
    if (typeof e != "undefined") {
        try {
            GoShareContent = {
                link: obj.link,
                message: obj.message,
                picture: obj.picture,
                title: obj.title,
                description: obj.description,
                name: obj.name
            }
        }
        catch (ex) {
            init_GoShareContent();
        }
        var h = bindHtml_facebookShare();
        if (document.getElementById(e)) {

            document.getElementById(e).innerHTML += h;
        }
        else
            document.writeln(h);
    }
}
/* end share len facebook */

/* end share len google+1 */
function bindHtml_ggplusone() {

    var size = ' size="medium" annotation="none"';
    //    if (typeof GoIconType !== "undefined")
    //        if (GoIconType == 1)
    //            size = "medium";
    //        else
    //            size = "small";
    //    else
    //        size = ' size="' + size + '"';


    var h = '<g:plusone' + size + '></g:plusone>';

    return h;
}
function init_ggplusoneShare(e) {
    if (typeof e != "undefined") {
        var h = bindHtml_ggplusone();
        if (document.getElementById(e)) {
            document.getElementById(e).innerHTML += h;
        }
        else
            document.writeln(h);
        window.___gcfg = { lang: 'vi' };

        (function () {
            var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
            po.src = 'https://apis.google.com/js/plusone.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
        })();
    }
}
/* end share len google+1 */

/* share len Go.vn, Zing me, Link hay */
function init_shareAll(e, obj) {
    if (typeof e != "undefined" && typeof obj != "undefined") {
        init_mygoObjShare(e, obj);
        init_zingmeObjShare(e, obj);
        init_linkhayShare(e);
        init_facebookShare(e, obj);
//        init_ggplusoneShare(e);
    }
}
function init_shareAll_text(e, obj) {
    if (typeof e != "undefined" && typeof obj != "undefined") {
        init_mygoObjShare(e, obj);
        init_zingmeObjShare(e, obj);
        init_linkhayShare(e);
        init_facebookShare(e, obj);
        
    }
}

/* process type of share*/

function init_goShare() {
    if (typeof GoShareType !== "undefined") {
        try {
            if (typeof GoShareElm == "undefined")
                var GoShareElm = "goVn_Elm";
            switch (GoShareType) {
                case "govn":
                    init_govnSharePopup();
                    init_mygoObjShare(GoShareElm, GoShareContent);
                    break;
                case "govnShareLink":
                    init_govnShareLink();
                    break;
                case "zingme":
                    init_zingmeObjShare(GoShareElm, GoShareContent);
                    break;
                case "linkhay":
                    init_linkhayShare(GoShareElm);
                    break;
                case "facebook":
                    init_facebookShare(GoShareElm);
                    break;
                case "shareAll":
                    init_shareAll(GoShareElm, GoShareContent);
                    break;
                case "shareAllText":
                    init_shareAll_text(GoShareElm, GoShareContent);
                    break;
                default:
                    init_shareAll(GoShareElm, GoShareContent);
                    break;
            }
        }
        catch (ex) { }
    }
}
init_goShare();

/*end of file*/