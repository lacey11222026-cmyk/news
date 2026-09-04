
//
	$(function() {
		$('div#menu').mmenu();
    $("#flexiselDemo2").flexisel({
            visibleItems: 4 ,
            animationSpeed: 1000,
            autoPlay: true,
            autoPlaySpeed: 3000,
            pauseOnHover: true,
            enableResponsiveBreakpoints: true,
            responsiveBreakpoints: {
                portrait: {
                    changePoint:480,
                    visibleItems: 2
                },
                landscape: {
                    changePoint:640,
                    visibleItems: 3
                },
                tablet: {
                    changePoint:991,
                    visibleItems: 4
                }
            }
        });
	});


// slide



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
                    topBtn.stop().animate({'bottom' : '70px'}, 400);
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
