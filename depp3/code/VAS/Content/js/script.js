//Menu mobile
$(function() {
	$('div#menu').mmenu();
});	

// Slider banner center
$('.bnrSlider').slick({
	arrows: false,
	dots: true,
  infinite: true,
  speed: 500,
  fade: true,
	autoplay:true
});

// bnrSmallSlider02
$('.bnrSmallSlider02').slick({
	autoplay: true,
	infinite: true,
	dots: false,
	arrows: false,
	speed: 300,
	slidesToShow: 5,
	slidesToScroll: 5,
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
							slidesToShow: 2,
							slidesToScroll: 2
					}
			}
	]
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

//Map
document.addEventListener("DOMContentLoaded", function () {
  const areas = document.querySelectorAll('area');
  const mapItems = document.querySelectorAll('.map-item');

  areas.forEach(area => {
    area.addEventListener('mouseenter', function () {
      const targetClass = this.className + '-content';
      mapItems.forEach(item => {
        item.style.display = 'none'; // Hide all map items
      });
      const targetContent = document.querySelector(`.${targetClass}`);
      if (targetContent) {
        targetContent.style.display = 'block'; // Show the specific content
      }
    });

    area.addEventListener('mouseleave', function () {
      const targetClass = this.className + '-content';
      const targetContent = document.querySelector(`.${targetClass}`);
      if (targetContent) {
        targetContent.style.display = 'none'; // Hide the content on mouse leave
      }
    });
  });

  mapItems.forEach(item => {
    item.addEventListener('mouseenter', function () {
      this.style.display = 'block'; // Ensure content stays visible if hovered
    });

    item.addEventListener('mouseleave', function () {
      this.style.display = 'none'; // Hide content when mouse leaves
    });
  });
});
// Map mobile
$(function() {
  if ($(window).width() < 991) {
    $('.map-item').on('click', function() {
      $(this).toggleClass('dropdown_toggle').children('.area-content').slideToggle(200);
    });

    $(document).on('click', function(e) {
      if (!$(e.target).closest('.map-item').length) {
        $('.area-content').hide();
      }
    });
  }
});
	
// Slider 
	$('.sliderAboutUs').slick({
		dots: true,
		arrows: true,
		infinite: true,
		speed: 300,
		slidesToShow: 1,
		adaptiveHeight: true,
	});