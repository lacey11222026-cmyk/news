$(function(){
		$('#menu').mmenu();
	
	
	
	
	
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
						
		
		$("#flexiselDemo1").flexisel({
			visibleItems: 3,
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
					visibleItems: 2
				},
				tablet: {
					changePoint:768,
					visibleItems: 3
				}
			}
		});
		$("#flexiselDemo2").flexisel({
			visibleItems: 3,
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
					visibleItems: 2
				},
				tablet: {
					changePoint:768,
					visibleItems: 3
				}
			}
		});
		
		$(".click").click(function(){
			$(".click").hide();
			$(".tool_phu .date").hide();
			$(".submit_m").fadeIn();
			$(".text_m").fadeIn("slow");
		});

		$(".block_mobile .click").bind('click', function(){
			$(".block_mobile .date").hide();
			$(".block_mobile .search_m").animate({width: "80%"});
		});

		$(".icon-search").click(function(){
			$(".search_box").show(300);

		});
		
		$('.slide-news li h3').matchHeight();
		$('.maxheight02').matchHeight();
		$('.maxheight03').matchHeight();								
		$('.maxheight04').matchHeight();				
		$('.maxheight05').matchHeight();				
		$('.maxheight06').matchHeight();				
		$('.maxheight07').matchHeight();				
		$('.maxheight01').matchHeight();	
		
		
	});




