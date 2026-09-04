/**
 * JS FOR DETAIL PAGE
 * Load imgs by color for Product
 */
/* ===================================================================================== */
var fst_time = 0;

/**
 * loadColr : click and load color by img
 * var [ colr ] : #pro-colr
 */
function loadColr(colr) {
    if (!$(colr).length) {return;}
    if ($(colr).find('span').length <= 1) {return;}

    // VAR
    $url_ajax = $(colr).data('url');
    $a_colr   = $(colr).find('span');

    $a_colr.each(function(n,e) {
        $(this).on('click', function(event){
            $this = $(this);
            $('.tips').hide();
            
            if ($this.hasClass('active')) {return;}
            // Handle on click
               if (fst_time == 0) {
                   // store into cookie
                    $.ajax({
                      url: baseUrl + $url_ajax,
                      type: "POST",
                      data: { colr : $this.data('color-id') },
                      dataType: "json"
                    }).done(function( data ) {
                        $('.fea-img-blk').data('procolor', JSON.stringify(data));
                        bHTML(data, $this, $this.data('color-id'));
                        fst_time++;
                    });  // END: Calling ajax
               } else {
                   // Get data form cookie
                   data = JSON.parse($('.fea-img-blk').data('procolor'));
                   bHTML(data, $this, $this.data('color-id'));
               }
             // END: SET UP DATA.
        });
    });
}

/**
 * bHTML : build HTML For feature imgs slider in detail page
 * @param {json}               html_data
 * @param {elemet html object} this_alink
 * @param {string}             ac_color
 */
function bHTML(html_data, this_alink, ac_color) {

    if (!html_data.length) {return;}
    // BUILD HTML
    html_8k = '<div id="fea-slider" class="flexslider"><ul class="slides">';
    for (a= 0; a < html_data.length; a++) {
        /* 800px IMG */
        if (html_data[a].colr == ac_color) {
            acolor = html_data[a];
            for (i= 0; i < acolor.i_big.length; i++) {
                tmp = acolor.i_big[i];
                html_8k +=   '<li data-src="'+ baseUrl +'/images/product/' + tmp.img +'">'
                           +   '<a href="javascript:;" class="z-fea">'
                           +     '<img src="'+ baseUrl +'/images/product/' + tmp.img + '" alt="' + tmp.img + '"/>'
                           +   '</a>'
                           +  '</li>';
            }
        }
    }

    html_8k += '</ul></div>';
    /* 400px IMG */
    html_4k = '<div id="fea-nav" class="flexslider"><ul class="slides">';
    for (a= 0; a < html_data.length; a++) {
        if (html_data[a].colr == ac_color) {
             acolor = html_data[a];
            for (i= 0; i < acolor.i_big.length; i++) {
                tmp = acolor.i_big[i];
                html_4k +=   '<li style="width: 90px; float: left; display: block;">'
                           +   '<a href="javascript:;" class="z-fea">'
                           +     '<img src="'+ baseUrl +'/images/product/' + tmp.img + '" alt="' + tmp.img + '"/>'
                           +   '</a>'
                           +  '</li>';
            }
        }
    }
    html_4k += '</ul></div>';

    // APPLY HTML AND SLIDER
    $('#pro-colr span').removeClass('active');
    $('.fea-img-blk').html(html_8k + html_4k);
    runFeaSlider('.fea-img-blk #fea-slider', '.fea-img-blk #fea-nav');
    runFeaZoom(".fea-img-blk #fea-slider ul");
    this_alink.addClass('active');
}

/**
 * hoverFeaImg : hover Fea Img and show Zoom ico
 * var [ feaimg ]     : #fea-slider
 * var [ parent_fea ] : .specs-cp-pic
 */
function hoverFeaImg (parent_fea, feaimg) {
    if (!$(feaimg).length) {return;}
    if (!$(parent_fea).length) {return;}

    $(parent_fea).on( "mouseover", feaimg, function() {
        $(parent_fea).find('.ico-zoom').addClass('shw');
    }).on( "mouseout", feaimg, function() {
        $(parent_fea).find('.ico-zoom').removeClass('shw');
    });
}

/** FOR : FEATURE IMAGE - DETAIl PAGE **/
/**
 * runFeaSlider in Product Detail Page
 * Call SLIDER [ feaslider ] : #fea-slider
 * Navigator [ fea_nav ] : #fea-nav
 */
function runFeaSlider(feaslider, fea_nav) {
    if (!$(feaslider).length) {return;}
    if (!$(fea_nav).length) {return;}

    $(feaslider).flexslider({
        directionNav: false,
        controlNav: false,
        animationLoop: false,
        slideshow: false,
        animationSpeed: 400,
        sync: fea_nav
    });

    // The slider being synced must be initialized first
    $(fea_nav).flexslider({
        animation: "slide",
        controlNav: false,
        animationLoop: false,
        slideshow: false,
        itemWidth: 70,
        itemMargin: 4,
        animationSpeed: 300,
        prevText: "<i class='icon-left-open-big'></i>",
        nextText: "<i class='icon-right-open-big'></i>",
        asNavFor: feaslider
    });

}

/**
 * runFeaZoom : Zoom Feature img in Detail Page
 * var [ fea_zoom ] : #fea-slider ul
 */
function runFeaZoom(fea_zoom) {

    if (!$(fea_zoom).length) {return;}
    $(fea_zoom).lightGallery({
        mode:"slide",
        speed : 400,
        lang: {
            allPhotos: 'Có tất cả'
        },
    });
}

/* ===================================================================================== */
/* ON LOAD */
jQuery(document).ready(function($){
  // DETAIL PAGE
  runFeaSlider('#fea-slider', '#fea-nav');

      $('.tab-detail li').on('click', function(){
        var id = $(this).data('id');

        $('.tab-detail li').removeClass('active');
        $(this).addClass('active');
        $('.tab-content').removeClass('active').hide();
        $('#'+id).addClass('active').show();
        if (id=='tab-tech' || id=='tab-intro') {
          $('#tab-segment').show();
        }

      });

  /*Response.action(function() {
    if ( Response.band(799) ) {  //>=600

      $('#tab-intro').addClass('active').show();
      $('#tab-segment').show();
      $('.tab-detail li.tab-segment').show();

      $('.tab-detail')
        .after($('#tab-intro'))
        .after($('#tab-tech'))
        .after($('#tab-gadget'));
      $('.tab-detail li').on('click', function(){
        var id = $(this).data('id');
        if (id=='tab-segment') {
          return false;
        }
        $('.tab-detail li').removeClass('active');
        $(this).addClass('active');
        $('.tab-content').removeClass('active').hide();
        $('#'+id).addClass('active').show();
        if (id=='tab-tech' || id=='tab-intro') {
          $('#tab-segment').show();
        }

      });


    }
    else {  //< 600

      $('.tab-intro').append($('#tab-intro'));
      $('.tab-tech').append($('#tab-tech'));
      $('.tab-gadget').append($('#tab-gadget'));
      
      $('.tab-content').hide();
      $('.tab-detail li').removeClass('active');
      //$('.tab-content.active').show();
      
      $('.tab-detail li.tab-segment').hide();

      $('.tab-detail li').on('click', function(){
        var id = $(this).data('id');
        $('.tab-content').removeClass('active').hide();
        if(!$(this).hasClass('active')) {
          $('.tab-detail li').removeClass('active');
          $(this).addClass('active');
          $('#'+id).addClass('active').show();
        } else {
          $('#'+id).removeClass('active');
          $('.tab-detail li').removeClass('active');
        }
      });
    }

  });
*/
 

  var windowsize = $(window).width();

  $(window).resize(function() {
    windowsize = $(window).width();
  });

  if (windowsize > 799) {
    $('.tab-detail li').click(function(){
      if ($(this).hasClass('tab-gadget')){
        $('.tab-detail li.tab-segment').hide();
      }
      else {
        $('.tab-detail li.tab-segment').show(); 
      }
      
    });
  }
  else {
    $('.tab-detail li.tab-segment').show();
  }

  $('#service-center').click(function() {
      $('#service-center').hide();
      return false;
  });

  $('#service-center-open').click(function (){
      $('#service-center').show();
      return false;
  })

});
/**
 * [onload Page : Main Category Page]
 */
$(window).load(function() {
    loadColr('#pro-colr');
    hoverFeaImg('.specs-cp-pic', '#fea-slider');
    runFeaZoom("#fea-slider ul");
});
