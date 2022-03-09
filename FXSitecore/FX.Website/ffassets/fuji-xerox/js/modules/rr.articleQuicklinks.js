/**
 * Article Quick Links
 */
var RR = (function(parent, $, undefined){
    'use strict';

    var $quicklinks = $('.quick-links'),
        $article    = $('.article'),
        $topBanner  = $('.top-banner'),
        offsetTop   = 0;

    (function(){
        if(!$quicklinks.length) return;

        var $ulEl = $('<ul/>'),
            $liEl = $('<li/>'),
            $anchorEl = $('<a href = ""/>'),
            $anchorLinks = $('a.anchor-link', $article),
            linkNum = $anchorLinks.length,
            $newliEl,
            $newAnchorEl,
            $target;

        for(var i = 1; i < linkNum + 1; i++) {
            $newliEl = $liEl.clone();
            $newAnchorEl = $anchorEl.clone();
            $target = $anchorLinks.eq(i - 1);

            $target.attr('id', 'content' + i);

            $newAnchorEl.text($target.data('label'));
            $newAnchorEl.attr('href', '#' + $target.attr('id'));

            $newliEl.append($newAnchorEl);
            $ulEl.append($newliEl);
        }

        $('.wingspan', $quicklinks).append($ulEl);

        $('a', $quicklinks).on('click', function(e){
            e.preventDefault();

            var $target = $($(this).attr('href'));

             $('html, body').stop(true, true).animate({
                scrollTop: $target.offset().top - offsetTop
            }, 500);
        });
    }());

    var setupSticky = function(){

        if(!$quicklinks.length) return;

        // var quicklinksOffset = $quicklinks.offset().top;

         offsetTop  =  $quicklinks.height() + 20;

        $(window).on('scroll.sticky', function(){
            var scrollAmt = $(this).scrollTop(),
                topbannerBottom = $topBanner.offset().top + $topBanner.height();

            if(topbannerBottom < scrollAmt) {
                $quicklinks.addClass('sticky');
                $article.css({'padding-top':offsetTop});
            } else {
                $quicklinks.removeClass('sticky');
                $article.css({'padding-top': ''});
            }
        }).trigger('scroll.sticky');
    };

    var destroySticky = function() {
        offsetTop = 10;

        $(window).off('scroll.sticky');
        $quicklinks.removeClass('sticky');
        $article.css({'padding-top': ''});
    };

    parent.articleQuicklinks = {
        setupSticky: setupSticky,
        destroySticky: destroySticky
    };

    return parent;

}(RR || {}, jQuery));