$(function () {

	// Sticky header
	$('.nav_top').addClass('original').clone().insertAfter('.nav_top').addClass('cloned').css({
		'position': 'fixed',
		'top': '0',
		'margin-top': '0',
		'z-index': '500'
	}).removeClass('original').hide();

	setInterval(function stickIt() {
		var orgElement = $('.original');
		var orgElementTop = orgElement.offset().top;
		if ($(window).scrollTop() >= orgElementTop) {
			var coords = orgElement.offset();
			var left = coords.left;
			var width = orgElement.css('width');
			$('.cloned').css({ left: left + 'px', top: 0, width: width }).show();
			orgElement.css('visibility', 'hidden');
		} else {
			$('.cloned').hide();
			orgElement.css('visibility', 'visible');
		}
	}, 10);

	// Các slider
	$('.bnrSliderBig').slick({ arrows: false, dots: true, infinite: true, speed: 500, fade: true, autoplay: true, autoplaySpeed: 15000 });
	$('.bnrSlider').slick({ arrows: false, dots: true, infinite: true, speed: 500, fade: true, autoplay: true });

	// Cuộn mượt
	$('.nav-list a[href^="#"]').on('click', function (event) {
		var href = $(this).attr('href');
		var target = $(href);
		if (target.length && href !== "#") {
			event.preventDefault();
			$('html, body').animate({
				scrollTop: target.offset().top - 30
			}, 600);
		}
	});

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
	
});
