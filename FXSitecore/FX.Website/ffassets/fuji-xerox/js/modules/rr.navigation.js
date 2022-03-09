/**
 * Main Navigation
 */
var RR = (function(parent, $, undefined){
    'use strict';

    var $mainNav = $('.main-navigation__menu'),
        $flyovers = $('.flyover', $mainNav),
        timer;

    var setup = function(){

        if(!$mainNav.length) return;

        $('.parent', $mainNav).each(function(){
            var $this = $(this),
                $submenu = $('.flyout', $this);

            $this.on('mouseenter', function(){
                timer = setTimeout(function(){
                    $this.addClass('hover-on');
                }, 100);
            });

             $this.on('mouseleave', function(){
                clearTimeout(timer);
                $('.parent', $mainNav).removeClass('hover-on');
            });
        });
    };

    parent.navigation = {
        setup: setup
    };

    return parent;

}(RR || {}, jQuery));