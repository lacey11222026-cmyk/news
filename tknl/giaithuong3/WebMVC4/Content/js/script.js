	
    $('div#menu').mmenu();
    
    $('.slickSlide').slick({
      autoplay: true,
      autoplaySpeed: 5000,
      fade: true,
      dots: true,
      arrows: false
    });



	// Slider 
	$('.gallery-slider').slick({
      centerMode: true,
      slidesToShow: 3,
      centerPadding: '0',
      arrows: true,
      dots: true,
      infinite: true,
      responsive: [
        {
            breakpoint: 767,
            settings: {
                slidesToShow: 1,
                centerMode: false // hoặc true nếu muốn vẫn có hiệu ứng center
            }
        }
    ]
  });

  // Nav fix top
    const nav = document.querySelector('.nav-pc');
    const navOffset = nav.getBoundingClientRect().top + window.scrollY;

    window.addEventListener('scroll', () => {
        if (window.scrollY > navOffset) {
            nav.classList.add('fixed');
        } else {
            nav.classList.remove('fixed');
        }
    });

  // backto top
  $(window).on('scroll', function () {

    if ($(window).scrollTop() > $(window).height()) {
        $('.backTop').addClass('show');
    } else {
        $('.backTop').removeClass('show');
    }

});

$('.backTop').on('click', function () {
    $('html, body').animate({
        scrollTop: 0
    }, 500);
});






