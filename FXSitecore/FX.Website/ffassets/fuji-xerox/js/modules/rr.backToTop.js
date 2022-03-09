/**
 *  RR - Back to top
 */
(function(parent, $){
    'use strict';

    /**
     * Setup back to top link
     */
    var setup = function(className) {
        var $wrapper = $('<div/>'),
            $anchor = $('<i class="icon-angle-up"></i><span class="label">Top</span>');

        $wrapper
            .addClass(className)
            .append($anchor);

        $('body').append($wrapper);

        /**
         * Bind Behaviours
         */
        $wrapper
            .on('click', function(e){
                $('html, body').stop().animate({'scrollTop': 0});
                e.preventDefault();
            });


        var scrolling = function(){
            var scrollTop = $(window).scrollTop();

            if(scrollTop > $(window).height()) {
                $wrapper.fadeIn();
            } else {
                $wrapper.fadeOut();
            }
        };

        /**
         * On Scroll
         */
        $(window)
            .on('scroll.window', scrolling)
            .trigger('scroll.window');
    };


    setup('back-to-top');

    return parent;

}(RR || {}, jQuery));