
$(".user-ava").click(function(){
	$(".user-ava .setting-user").toggle('200');
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
//document.getElementById('moar').onclick = function() {
//	var section = document.createElement('section');
//	section.className = 'section--purple wow fadeInDown';
//	this.parentNode.insertBefore(section, this);
//};

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




//sing out chooose
function myFunction() {
	document.getElementById("myDropdown").classList.toggle("show");
}

// Close the dropdown if the user clicks outside of it
window.onclick = function(event) {
	if (!event.target.matches('.dropbtn')) {

		var dropdowns = document.getElementsByClassName("dropdown-content");
		var i;
		for (i = 0; i < dropdowns.length; i++) {
			var openDropdown = dropdowns[i];
			if (openDropdown.classList.contains('show')) {
				openDropdown.classList.remove('show');
			}
		}
	}
}




