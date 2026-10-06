//Menu mobile
$(function() {
	$('div#menu').mmenu();
});	


//maxheight
$(function(){
	$('.maxheight01').matchHeight(); 
	$('.maxheight02').matchHeight(); 

});
// banner slider
$('.bnrTopSlider').slick({
	arrows: false,
	dots: false,
  infinite: true,
  speed: 500,
  fade: true,
	autoplay:true
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
						topBtn.stop().animate({'bottom' : '100px'}, 300);
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

// nav fixed
$("document").ready(function($){
		// Create a clone of the nav_top, right next to original.
		$('.nav_top').addClass('original').clone().insertAfter('.nav_top').addClass('cloned').css('position','fixed').css('top','0').css('margin-top','0').css('z-index','500').removeClass('original').hide();
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
				// not scrolled past the nav_top; only show the original nav_top.
				$('.cloned').hide();
				$('.original').css('visibility','visible');
			}
		}
	});$("document").ready(function($){
		// Create a clone of the nav_top, right next to original.
		$('.nav_top').addClass('original').clone().insertAfter('.nav_top').addClass('cloned').css('position','fixed').css('top','0').css('margin-top','0').css('z-index','500').removeClass('original').hide();
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
				// not scrolled past the nav_top; only show the original nav_top.
				$('.cloned').hide();
				$('.original').css('visibility','visible');
			}
		}
	});		