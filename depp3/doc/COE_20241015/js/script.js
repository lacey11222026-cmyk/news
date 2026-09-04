//Menu mobile
$(function() {
	$('div#menu').mmenu();
});	

$(function(){
			$('.news-name').matchHeight();
	});

// banner
 $('.main__Mission__Slide').slick({
		autoplay: true,
		autoplaySpeed: 3000,
		fade: true,
		dots: true,
		arrows: false,	
		responsive: [
					{
							breakpoint: 767,
							settings: {
									autoplaySpeed: 9750,
							}
					},
			]
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
	arrows: true,
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