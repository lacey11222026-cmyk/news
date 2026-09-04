$("document").ready(function($){
		// Create a clone of the nav-top, right next to original.
		$('.nav-top').addClass('original').clone().insertAfter('.nav-top').addClass('cloned').css('position','fixed').css('top','0').css('margin-top','0').css('z-index','500').removeClass('original').hide();
		scrollIntervalID = setInterval(stickIt, 10);

		function stickIt() {
			var orgElementPos = $('.original').offset();
			orgElementTop = orgElementPos.top;
			if ($(window).scrollTop() >= (orgElementTop)) {
				// scrolled past the original position; now only show the cloned, sticky element.
				// Cloned element should always have same left position and width as original element.
				orgElement = $('.original');
				coordsOrgElement = orgElement.offset();
				leftOrgElement = coordsOrgElement.left;
				widthOrgElement = orgElement.css('width');
				$('.cloned').css('left',leftOrgElement+'px').css('top',0).css('width',widthOrgElement).show();
				$('.original').css('visibility','hidden');
			} else {
				// not scrolled past the nav-top; only show the original nav-top.
				$('.cloned').hide();
				$('.original').css('visibility','visible');
			}
		}
	});
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
