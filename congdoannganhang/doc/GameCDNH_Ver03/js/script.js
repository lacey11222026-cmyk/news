$(document).ready(function() {
  $(".screen1b").hide();
  setTimeout(function() { 
      $(".screen1a").hide(500);
      $(".screen1b").show(1000);
  }, 4000);
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
    