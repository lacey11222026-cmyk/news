$(document).ready(function () {
    if (window.location.host == 'www.congdoancongthuong.org.vn' || window.location.host == 'congdoancongthuong.org.vn' || window.location.host == 'www.vuit.org.vn') {
        window.location.href = 'http://vuit.org.vn';
    }
    $(".block_mobile .click").bind('click', function () {
        $(".block_mobile .date").hide();
        $(".block_mobile .search_m").animate({ width: "75%" });
    });
});


