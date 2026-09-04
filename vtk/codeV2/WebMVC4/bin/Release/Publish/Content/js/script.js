
//<!-- go to top -->
$(function() {
	var showFlag = false;
	var topBtn = $('#top_gototop');
	topBtn.css('bottom', '-100px');
	var showFlag = false;
	$(window).scroll(function () {
		if ($(this).scrollTop() > 100) {
			if (showFlag == false) {
				showFlag = true;
				topBtn.stop().animate({'bottom' : '55px'}, 300);
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

// Menu mobile
	$(function() {
		$('div#menu').mmenu();
	});

// maxheight
	$(function(){
		$('.maxheight01').matchHeight();
		$('.maxheight02').matchHeight();
		$('.maxheight03').matchHeight();
    $('.maxheight04').matchHeight();
    $('.product-wrap').matchHeight();
	});

//support
$(document).ready(function(){
  $(".head-support").click(function(){
    $(".body-support").toggle();
  });
});

// menu pc
$(function(){
	$('.nav-top > ul li').on('click', function(){
		$( this ).parent().find( 'li.active' ).removeClass('active').children('.ul02').hide();
    $(this).addClass('active').children('.ul02').toggle(200);
	});
});

// Slide chan trang
$(window).load(function() {
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
				visibleItems: 3
			},
			tablet: {
				changePoint:768,
				visibleItems: 3
			}
		}
	});
});

//wow js
    wow = new WOW(
      {
        animateClass: 'animated',
        offset:       100,
        callback:     function(box) {
          console.log("WOW: animating <" + box.tagName.toLowerCase() + ">")
        }
      }
    );
    wow.init();
    



//  jQuery(function($) {
//  var nav    = $('.header'),
//      offset = nav.offset();
//
//  $(window).scroll(function () {
//    if($(window).scrollTop() > offset.top) {
//      nav.addClass('fixed');
//    } else {
//      nav.removeClass('fixed');
//    }
//  });
//
//  });

$(function(){
	$('.dropdown__inner').on('click', function(){
    $(this).toggleClass('dropdown_toggle').children('.dropdownSub').slideToggle(200);
	});
  $('.dropdown__inner li a').on("click", function() {
    $(".dropdownSub").hide();
  });
  $(document).on('click', function(e) {
    if (!$(e.target).closest('.dropdown__inner').length) {
      $('.dropdownSub').hide  ();
    }
  });});



window.onscroll = function() {myFunction()};

var navbar = document.getElementById("navbar");
var sticky = navbar.offsetTop;

function myFunction() {
  if (window.pageYOffset >= sticky) {
    navbar.classList.add("sticky")
  } else {
    navbar.classList.remove("sticky");
  }
}
