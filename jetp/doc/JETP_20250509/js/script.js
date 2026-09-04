
// nav mobile
$(document).ready(function () {
  $(".ico-menu").click(function () {
    $(".header-mobile-content").animate({ left: "0" }, 100);
    $("#overlay").fadeIn(); // Show overlay
  });

  $(".ico-close, #overlay").click(function () {
    $(".header-mobile-content").animate({ left: "-100%" }, 100);
    $("#overlay").fadeOut(); // Hide overlay
  });

  $(".nav-mb-list .ico-dropdown").click(function (event) {
    var $submenu = $(this).next(".nav-mb-sub");
    if ($submenu.length) {
      event.preventDefault();
      $(this).toggleClass("active");
      $submenu.slideToggle();
    }
  });
});

  $(document).ready(function () {
  $(".bnt-lang").click(function (event) {
    event.stopPropagation();
    $(".list-lang").toggleClass("active");
  });
  $(document).click(function (event) {
    if (!$(event.target).closest(".lang").length) {
      $(".list-lang").removeClass("active");
    }
  });
});

  // Slider
document.addEventListener("DOMContentLoaded", function () {
    const initSwiper = (selector, options) => {
        try {
            new Swiper(selector, options);
        } catch (error) {
            console.warn(`Không thể khởi tạo Swiper: ${selector} - Lỗi:`, error);
        }
    };

    initSwiper(".newslideSwiper", {
        loop: true,
        slidesPerView: 3,
        spaceBetween: 20,
        navigation: {
            nextEl: ".swiper-button-next",
            prevEl: ".swiper-button-prev",
        },
        autoplay: { delay: 3000, disableOnInteraction: false },
        breakpoints: {
            0: { slidesPerView: 1, spaceBetween: 0 },
            991: { slidesPerView: 2, spaceBetween: 20 },
            1280: { slidesPerView: 3, spaceBetween: 20 }
        }
    });

    initSwiper(".homeslideSwiper", {
        allowTouchMove: true,
        slidesPerView: 1,
        loop: false,
        spaceBetween: 0,
        centeredSlides: false,
        speed: 2000,
        autoplay: { delay: 3000 },
        effect: "fade",
        fadeEffect: {
            crossFade: true, 
        },
        pagination: {
            el: ".swiper-pagination",
            clickable: true, 
        },
    });
});

function Backtotop() {
    var btn = $("#btnBottomUp");

    $(window).scroll(function () {
        if ($(window).scrollTop() > 300) {
            btn.addClass("show");
        } else {
            btn.removeClass("show");
        }
    });

    btn.on("click", function (e) {
        e.preventDefault();
        $("html, body").animate({ scrollTop: 0 }, 300);
    });
}

$(document).ready(function () {
    Backtotop();
});

//Search top
// $(document).ready(function() {
//     $(".search-btn").click(function(event) {
//         event.preventDefault();
//         $(".search-box").toggleClass("active");
//     });
//     $(document).click(function(event) {
//         if (!$(event.target).closest(".search-box, .search-btn").length || $(event.target).is(".search-box")) {
//             $(".search-box").removeClass("active");
//         }
//     });
// });
$(document).ready(function() {
    $(".search-btn").click(function(event) {
        event.preventDefault();
        $(".search-box").toggleClass("active");
    });
    $(document).click(function(event) {
        if (!$(event.target).closest(".search-box, .search-btn").length) {
            $(".search-box").removeClass("active");
        }
    });
});


