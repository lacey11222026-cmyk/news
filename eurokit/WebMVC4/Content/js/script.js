$(document).on("click", ".mobile-navigation", function () {
    $(this).closest(".main-menu-wapper").find(".navigation-main-menu").toggle();
    $("#box-vertical-megamenus .vertical-menu-content").hide();
    return false;
});
$(document).on("click", ".form-search-9 .icon", function () {
    
    $(this).closest(".form-search-9").find(".form-search-inner").fadeIn(600);
    $(this).closest(".form-search-9").find(".form-search-inner .input-serach input").focus();
});
$(document).ready(function () {
    h();
});
function h() {
    var r = (Modernizr.touch) ? true : false;
    
    if (r === true) {
        $(document).on("click", ".navigation .menu-item-has-children > a", function (s) {
            var u = $(this).closest("li");
            var v = $(".navigation .menu-item-has-children");
            if (!u.hasClass("show-submenu")) {
                v.removeClass("show-submenu");
                u.parents().each(function () {
                    if ($(this).hasClass("menu-item-has-children")) {
                        $(this).addClass("show-submenu")
                    }
                    if ($(this).hasClass(".navigation")) {
                        return false
                    }
                });
                u.addClass("show-submenu");
                if (!u.hasClass("show-submenu")) {
                    u.find("li").removeClass("show-submenu")
                }
                return false;
                s.preventDefault()
            } else {
                var t = $(this).attr("href");
                if (a.trim(t) == "" || a.trim(t) == "#") {
                    u.toggleClass("show-submenu")
                } else {
                    window.location = t
                }
            }
            if (!u.hasClass("show-submenu")) { }
            s.stopPropagation()
        });
        $(document).on("click", function (s) {
            var t = $(s.target);
            if (!t.closest(".show-submenu").length || !t.closest(".navigation").length) {
                $(".show-submenu").removeClass("show-submenu")
            }
        })
    } else {
        $(".navigation .menu-item-has-children").hover(function () {
            $(this).addClass("show-submenu");
        }, function () {
            $(this).removeClass("show-submenu");
        })
    }
}