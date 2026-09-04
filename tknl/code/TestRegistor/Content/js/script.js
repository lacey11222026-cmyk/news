// logout dropdown
$(function () {

  $('.user-info').on('click', function (e) {
    e.stopPropagation();

    $('.dropdown-content').fadeToggle(200);
  });

  $(document).on('click', function () {
    $('.dropdown-content').fadeOut(200);
  });

});




//animation
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
document.getElementById('moar').onclick = function() {
	var section = document.createElement('section');
	section.className = 'section--purple wow fadeInDown';
	this.parentNode.insertBefore(section, this);
};

//animation scroll
$(document).ready(function(){
	$('a[href^="#"]').on('click',function (e) {
		e.preventDefault();

		var target = this.hash;
		var $target = $(target);

		$('html, body').stop().animate({
			'scrollTop': $target.offset().top
		}, 400, 'swing', function () {
			window.location.hash = target;
		});
	});

});








