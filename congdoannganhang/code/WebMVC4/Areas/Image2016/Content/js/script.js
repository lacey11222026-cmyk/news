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
$('.bnrSlider').slick({
    autoplay: true,
    autoplaySpeed: 2000,
		slidesToShow: 1,
    fade: true,
    dots: true,
    arrows: false
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



// Slider 01
$(function(){
  $('#diaporama').html($("#images").html());
  $('#diaporama').mixSlide({
    fullscreen:false,
    thumbs:true,
    controls:true,
    transition:{name:"circle"},
    animation:{
      delay:5,
      speed:2
    },
    labels:true,
    layout:MXS_LAYOUT_1
  });
  
  // Slider 02
  $('#diaporama02').html($("#images02").html());
  $('#diaporama02').mixSlide({
    fullscreen:false,
    thumbs:true,
    controls:true,
    transition:{name:"circle"},
    animation:{
      delay:5,
      speed:2
    },
    labels:true,
    layout:MXS_LAYOUT_1
  });
  
  // Slider 03
  $('#diaporama03').html($("#images03").html());
  $('#diaporama03').mixSlide({
    fullscreen:false,
    thumbs:true,
    controls:true,
    transition:{name:"circle"},
    animation:{
      delay:5,
      speed:2
    },
    labels:true,
    layout:MXS_LAYOUT_1
  });
  
  // Slider 04
  $('#diaporama04').html($("#images04").html());
  $('#diaporama04').mixSlide({
    fullscreen:false,
    thumbs:true,
    controls:true,
    transition:{name:"circle"},
    animation:{
      delay:5,
      speed:2
    },
    labels:true,
    layout:MXS_LAYOUT_1
  });
  
  // Slider 05
  $('#diaporama05').html($("#images05").html());
  $('#diaporama05').mixSlide({
    fullscreen:false,
    thumbs:true,
    controls:true,
    transition:{name:"circle"},
    animation:{
      delay:5,
      speed:2
    },
    labels:true,
    layout:MXS_LAYOUT_1
  });
  
  // Slider 06
  $('#diaporama06').html($("#images06").html());
  $('#diaporama06').mixSlide({
    fullscreen:false,
    thumbs:true,
    controls:true,
    transition:{name:"circle"},
    animation:{
      delay:5,
      speed:2
    },
    labels:true,
    layout:MXS_LAYOUT_1
  });
  
  // Slider 07
  $('#diaporama07').html($("#images07").html());
  $('#diaporama07').mixSlide({
    fullscreen:false,
    thumbs:true,
    controls:true,
    transition:{name:"circle"},
    animation:{
      delay:5,
      speed:2
    },
    labels:true,
    layout:MXS_LAYOUT_1
  });
  
  // Slider 02
  $('#diaporama08').html($("#images08").html());
  $('#diaporama08').mixSlide({
    fullscreen:false,
    thumbs:true,
    controls:true,
    transition:{name:"circle"},
    animation:{
      delay:5,
      speed:2
    },
    labels:true,
    layout:MXS_LAYOUT_1
  });
  
  // Slider 09
  $('#diaporama09').html($("#images09").html());
  $('#diaporama09').mixSlide({
    fullscreen:false,
    thumbs:true,
    controls:true,
    transition:{name:"circle"},
    animation:{
      delay:5,
      speed:2
    },
    labels:true,
    layout:MXS_LAYOUT_1
  });
  
  // Slider 10
  $('#diaporama10').html($("#images10").html());
  $('#diaporama10').mixSlide({
    fullscreen:false,
    thumbs:true,
    controls:true,
    transition:{name:"circle"},
    animation:{
      delay:5,
      speed:2
    },
    labels:true,
    layout:MXS_LAYOUT_1
  });

});