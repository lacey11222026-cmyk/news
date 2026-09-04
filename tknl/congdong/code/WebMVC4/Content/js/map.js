
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
  if ($(window).width() < 768) {
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