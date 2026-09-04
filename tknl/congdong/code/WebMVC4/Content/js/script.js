$(function () {


    // nav mobile
    $(".ico-menu").click(function () {
        $(".header-mobile-content").animate({ left: "0" }, 100);
    });

    $(".ico-close").click(function () {
        $(".header-mobile-content").animate({ left: "-100%" }, 20);
    });

    $(".nav-mb-list .ico-dropdown").click(function (event) {
        var $submenu = $(this).next(".nav-mb-sub");
        if ($submenu.length) {
            event.preventDefault();
            $(this).toggleClass("active");
            $submenu.slideToggle();
        }
    });

    // back to top
    var showFlag = false;
    var topBtn = $('#backToTop');
    topBtn.css('bottom', '-100px');

    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            if (!showFlag) {
                showFlag = true;
                topBtn.stop().animate({ 'bottom': '65px' }, 300);
            }
        } else {
            if (showFlag) {
                showFlag = false;
                topBtn.stop().animate({ 'bottom': '-50px' }, 300);
            }
        }
    });

    topBtn.click(function () {
        $('body,html').animate({ scrollTop: 0 }, 500);
        return false;
    });

    // Search
    $('.btn-search').click(function (e) {
        e.preventDefault();
        $('.search-results').slideDown();
        $('.btn-collapse').show();
    });

    $('.btn-collapse').click(function (e) {
        e.preventDefault();
        $('.search-results').slideUp();
        $(this).hide();
    });

    $('.choose-voice').click(function (event) {
        event.preventDefault();
        $(this).toggleClass('dropdown_toggle');
    });

});
