var SlideShow = {
    init: function () {
        $("a[data-to]").click(function () {
            SlideShow.showSelectedImage(this);
        });
    },

    showSelectedImage: function (elem) {
        var index = parseInt($(elem).data('to')) - 1;
        $("#post-slide").carousel(index);
    }
}
