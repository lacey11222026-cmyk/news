//Menu mobile
$(function() {
	$('div#menu').mmenu();
});	



//maxheight
$(function(){
	$('.maxheight01').matchHeight(); 
	$('.maxheight02').matchHeight(); 

});


// Slider logo
$(document).ready(function() {
    // Function to initialize the slider
    function initializeSlider() {
        $('.imgSlider').slick({
            autoplay: true,
            infinite: true,
            dots: true,
            arrows: true,
            speed: 300,
            slidesToShow: 1,
            slidesToScroll: 1,
            responsive: [
                {
                    breakpoint: 991,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 3
                    }
                },
                {
                    breakpoint: 575,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }
            ]
        });
    }

    // Check if screen size is below 991px before initializing the slider
    if ($(window).width() < 991) {
        initializeSlider();
    }

    // Reinitialize slider on window resize if needed
    $(window).resize(function() {
        var windowWidth = $(window).width();
        var sliderExists = $('.imgSlider').hasClass('slick-initialized');

        if (windowWidth < 991 && !sliderExists) {
            initializeSlider();
        } else if (windowWidth >= 991 && sliderExists) {
            $('.imgSlider').slick('unslick');
        }
    });
});
// Slider banner center
$('.bnrSlider').slick({
	arrows: false,
	dots: true,
  infinite: true,
  speed: 500,
  fade: true,
	autoplay:true,
	responsive: [									
			{
				breakpoint: 767,
				settings: {					
					arrows: true
				}
			}
		]
});


// dropdown__inner
$('.dropdown__inner').click(function(event) {
    event.preventDefault(); // Prevent default anchor action
    $(this).toggleClass('dropdown_toggle');
  });


//backto top
$(function() {
			var showFlag = false;
			var topBtn = $('#backToTop');
			topBtn.css('bottom', '-100px');
			var showFlag = false;
			$(window).scroll(function () {
				if ($(this).scrollTop() > 100) {
					if (showFlag == false) {
						showFlag = true;
						topBtn.stop().animate({'bottom' : '30px'}, 300);
					}
				} else {
					if (showFlag) {
						showFlag = false;
						topBtn.stop().animate({'bottom' : '-20px'}, 300);
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
