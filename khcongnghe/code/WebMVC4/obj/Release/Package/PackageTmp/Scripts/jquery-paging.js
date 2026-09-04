
/*
* jQuery pager plugin
* Version 1.0 (12/22/2008)
* @requires jQuery v1.2.6 or later
*
* Example at: http://jonpauldavies.github.com/JQuery/Pager/PagerDemo.html
*
* Copyright (c) 2008-2009 Jon Paul Davies
* Dual licensed under the MIT and GPL licenses:
* http://www.opensource.org/licenses/mit-license.php
* http://www.gnu.org/licenses/gpl.html
* 
* Read the related blog post and contact the author at http://www.j-dee.com/2008/12/22/jquery-pager-plugin/
*
* This version is far from perfect and doesn't manage it's own state, therefore contributions are more than welcome!
*
* Usage: .pager({ pagenumber: 1, pagecount: 15, buttonClickCallback: PagerClickTest });
*
* Where pagenumber is the visible page number
*       pagecount is the total number of pages to display
*       buttonClickCallback is the method to fire when a pager button is clicked.
*
* buttonClickCallback signiture is PagerClickTest = function(pageclickednumber) 
* Where pageclickednumber is the number of the page clicked in the control.
*
* The included Pager.CSS file is a dependancy but can obviously tweaked to your wishes
* Tested in IE6 IE7 Firefox & Safari. Any browser strangeness, please report.
*/
(function ($)
{
    $.fn.pager = function (options)
    {

        var opts = $.extend({}, $.fn.pager.defaults, options);

        return this.each(function ()
        {

            // empty out the destination element and then render out the pager with the supplied options
            $(this).empty().append(renderpager(parseInt(options.pagenumber), parseInt(options.pagecount), options.buttonClickCallback, options.html));

            // specify correct cursor activity
            //$('.pages a').mouseover(function () { document.body.style.cursor = "pointer"; }).mouseout(function () { document.body.style.cursor = "auto"; });
        });
    };

    // render and return the pager with the supplied options
    function renderpager(pagenumber, pagecount, buttonClickCallback, html)
    {

        if (pagecount < 2)
            return '';

        // setup $pager to hold render
        //var $pager = $('<div class="pages"></div>');
        var $pager = $(html.container);

        // add in the previous and next buttons
        if (pagenumber > 3)
        {
            $pager.append(renderButton('first', html.firstbtn, pagenumber, pagecount, buttonClickCallback)).append(renderButton('prev', html.prevbtn, pagenumber, pagecount, buttonClickCallback));
        }

        // pager currently only handles 10 viewable pages ( could be easily parameterized, maybe in next version ) so handle edge cases
        var startPoint = (pagecount <= 5) ? 1 : ((pagenumber + 3) > pagecount) ? (pagecount - 4) : ((pagenumber - 2) <= 0) ? 1 : pagenumber - 2;
        var endPoint = (pagecount <= 5) ? pagecount : ((pagenumber + 3) > pagecount) ? pagecount : startPoint + 4;

        // loop thru visible pages and render buttons
        var pageContainer = $(html.wrraper);
        for (var page = startPoint; page <= endPoint; page++)
        {
            var currentButton = $('<a>' + (page) + '</a>');
            page == pagenumber ? currentButton.addClass('selected') : currentButton.click(function () { buttonClickCallback(this.firstChild.data); });
            pageContainer.find(".page").append(currentButton);
        }

        pageContainer.appendTo($pager);

        // render in the next and last buttons before returning the whole rendered control back.
        if ((pagenumber + 3) <= pagecount)
        {
            $pager.append(renderButton('next', html.nextbtn, pagenumber, pagecount, buttonClickCallback)).append(renderButton('last', html.lastbtn, pagenumber, pagecount, buttonClickCallback));
        }

        return $pager;
    }

    // renders and returns a 'specialized' button, ie 'next', 'previous' etc. rather than a page number button
    function renderButton(buttonName, buttonLabel, pagenumber, pagecount, buttonClickCallback)
    {
        var $Button = $(buttonLabel);
        var destPage = 1;
        // work out destination page for required button type
        switch (buttonName)
        {
            case "first":
                destPage = 1;
                break;
            case "prev":
                destPage = pagenumber - 1;
                break;
            case "next":
                destPage = pagenumber + 1;
                break;
            case "last":
                destPage = pagecount;
                break;
        }

        $Button.click(function () { buttonClickCallback(destPage); });

        return $Button;
    }

    // pager defaults. hardly worth bothering with in this case but used as placeholder for expansion in the next version
    $.fn.pager.defaults = {
        pagenumber: 1,
        pagecount: 1
    };

})(jQuery);
