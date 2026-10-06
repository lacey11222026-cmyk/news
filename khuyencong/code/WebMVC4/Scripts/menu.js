/**
* @version		$Id: menu.js 10702 2008-08-21 09:31:31Z eddieajau $
* @copyright	Copyright (C) 2005 - 2008 Open Source Matters. All rights reserved.
* @license		GNU/GPL, see LICENSE.php
* Joomla! is free software. This version may have been modified pursuant
* to the GNU General Public License, and as distributed it includes or
* is derivative of works licensed under the GNU General Public License or
* other free or open source software licenses.
* See COPYRIGHT.php for copyright notices and details.
*/

/**
* JMenu javascript behavior
*
* @package		Joomla
* @since		1.5
* @version     1.0
*/
(function ($)
{
    // Compliant with jquery.noConflict()
    $.JMenu = function (el)
    {

        var elements = $(el + '> li');
        var nested = null;
        for (var i = 0; i < elements.length; i++)
        {
            var element = elements[i];
            $(element).mouseenter(function ()
            {
                $(this).addClass('hover');
            }).mouseleave(function ()
            {
                $(this).removeClass('hover');
            });

            nested = $(element).find("ul");
            if (!nested)
            {
                continue;
            }
            //declare width
            var offsetWidth = 0;
            //find longest child
            for (k = 0; k < $(nested).find('li').length; k++)
            {
                var node = $(nested).find('li')[k];
                var offset = $(node).offset();              
                offsetWidth = (offsetWidth >= node.offsetWidth) ? offsetWidth : Math.floor(node.offsetWidth);
            }

         
            for (l = 0; l < $(nested).find('li').length; l++)
            {

                var node = $(nested).find('li')[l];

                $(node).css('width', offsetWidth + 'px');

                $(node).mouseenter(function ()
                {
                    $(this).addClass('hover');
                }).mouseleave(function ()
                {
                    $(this).removeClass('hover');
                });
            }

            $(nested).css('width', offsetWidth + 'px');
        }
    }

})(jQuery);
