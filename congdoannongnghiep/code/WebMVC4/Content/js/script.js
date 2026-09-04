//Menu mobile
$(function() {
	$('div#menu').mmenu();
});	


//maxheight
$(function(){
	$('.maxheight01').matchHeight(); 
	$('.maxheight02').matchHeight(); 

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
// news slider
$('.newsSlider').slick({
	arrows: false,
	dots: true,
  infinite: true,
  speed: 1000,
  fade: true,
	autoplay:true
});
//bnrSmallSlider
$('.bnrSmallSlider').slick({
	autoplay: true,
	infinite: true,
	dots: true,
	arrows: true,
	speed: 300,
	slidesToShow: 3,
	slidesToScroll: 3,
	responsive: [
			{
					breakpoint: 991,
					settings: {
							slidesToShow: 2,
							slidesToScroll: 2
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
						topBtn.stop().animate({'bottom' : '65px'}, 300);
					}
				} else {
					if (showFlag) {
						showFlag = false;
						topBtn.stop().animate({'bottom' : '-50px'}, 300);
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

// search box
$(".search-box").on("click", function () {
  $(".search-box").removeClass("active");
  $(this).addClass("active");
});

//mess
$(document).ready(function () {
  $('.mess-inner').hide();
  $('.btn-mess').on('click', function () {
    $('.mess-inner').show();
    $('.btn-mess').hide();
  });
  $('.mess-close').on('click', function () {
    $('.mess-inner').hide();
    $('.btn-mess').show();
  });
});

$(document).ready(function() {
	// Smooth scroll to the target section when a link is clicked
	$(".control a").on("click", function(event) {
			var target = this.hash;
			$("html, body").animate({
					scrollTop: $(target).offset().top
			}, 500);
	});
});

