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


// maxheight
	$(function(){
		$('.maxheight01').matchHeight();
		$('.tab-pane').matchHeight();

	});

// Slide chan trang
$(window).load(function() {
	$("#flexiselDemo3").flexisel({
		visibleItems: 5,
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
				visibleItems: 3
			},
			tablet: {
				changePoint:768,
				visibleItems: 3
			}
		}
	});
});