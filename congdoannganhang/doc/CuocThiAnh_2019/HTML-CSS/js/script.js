//
	$(function() {
		$('div#menu').mmenu();
		$('.n_box3').matchHeight();
		$('.thumbnail').matchHeight();
		$('.maso').matchHeight();
	});


// slide
$(window).load(function() {
        SlideShow.init();

        $("#flexiselDemo3").flexisel({
            visibleItems: 4,
            animationSpeed: 1000,
            autoPlay: true,
            autoPlaySpeed: 3000,
            pauseOnHover: true,
            enableResponsiveBreakpoints: true,
            responsiveBreakpoints: {
                portrait: {
                    changePoint:480,
                    visibleItems: 1
                },
                landscape: {
                    changePoint:640,
                    visibleItems: 2
                },
                tablet: {
                    changePoint:768,
                    visibleItems: 3
                }
            }
        });
    });

// go top
$(function() {
        var showFlag = false;
        var topBtn = $('#top_gototop');
        topBtn.css('bottom', '-100px');
        var showFlag = false;
        $(window).scroll(function () {
            if ($(this).scrollTop() > 100) {
                if (showFlag == false) {
                    showFlag = true;
                    topBtn.stop().animate({'bottom' : '90px'}, 400);
                }
            } else {
                if (showFlag) {
                    showFlag = false;
                    topBtn.stop().animate({'bottom' : '-100px'}, 300);
                }
            }
        });
        topBtn.click(function () {
            $('body,html').animate({
                scrollTop: 0
            }, 500);
            return false;
        });
    });
