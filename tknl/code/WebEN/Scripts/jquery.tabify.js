(function (a) {
    a.fn.extend({
        tabify: function (e) {
            function c(b) {
                hash = a(b).find("a").attr("rel");
                alert(hash);
                return hash = hash.substring(0, hash.length - 4)
            }

            function f(b) {
                a(b).addClass("chon");
                a(c(b)).show();
                a(b).siblings("li").each(function () {
                    a(this).removeClass("chon");
                    a(c(this)).hide()
                })
            }
            return this.each(function () {
                function b() {
                    location.hash && a(d).find("a[rel=" + location.hash + "]").length > 0 && f(a(d).find("a[rel=" + location.hash + "]").parent())
                }
                var d = this,
                    g = {
                        ul: a(d)
                    };
                a(this).find("li a").each(function () {
                    a(this).attr("rel", a(this).attr("rel") + "-tab")
                });
                location.hash && b();
                setInterval(b, 100);
                a(this).find("li").each(function () {
                    a(this).hasClass("chon") ? a(c(this)).show() : a(c(this)).hide()
                });
                e && e(g)
            })
        }
    })
})(jQuery);