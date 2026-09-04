$(document).ready(function () {
    if (window.location.host == 'www.vecea.vn') {
        window.location.href = 'http://vecea.vn';
    }
});

//tự động refresh trang sau 300s, nếu user đang active(mouse over, scroll) thì reset lại thời gian
var refreshPageInterval = 300;
setInterval("countDownPageRefresh()", 3000);
$(document).mouseover(function () {
    funcResetRefreshPageInterval();
});
$(window).scroll(function () {
    funcResetRefreshPageInterval();
});
window.onkeypress = funcResetRefreshPageInterval;
function funcResetRefreshPageInterval() {
    refreshPageInterval = 300;
}
function countDownPageRefresh() {
    refreshPageInterval = refreshPageInterval - 5;
    if (refreshPageInterval <= 0) {
        document.location.reload(true);
    }
}



// Menu mobile
$(function() {
	$('nav#menu').mmenu();
});



// Search box mobile
$(document).ready(function(){
	$(".click").click(function(){
		$(".click").hide();
		$(".tool_phu .date").hide();
		$(".submit_m").fadeIn();
		$(".text_m").fadeIn("slow");
	});

});

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