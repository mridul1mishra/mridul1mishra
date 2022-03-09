/**
 * Side Menu
 */
var RR = (function(parent, $, undefined){
    'use strict';

    var $sideMenu = $('.side-menu');

    (function(){
        if(!$sideMenu.length) return;

        var $toggle = $('<a href = "#" class = "side-menu-toggle"><i class = "icon-navicon"></i></a>');

        function hideMenu(){
            $sideMenu.removeClass('active');
            setTimeout(function(){
                $('ul', $sideMenu).css({'display' : 'none'});
            }, 200);
        }

        $toggle.on('click', function(e){
            e.preventDefault();

            if($sideMenu.hasClass('active')) {
                hideMenu();
            } else {
                $('ul', $sideMenu).show();
                $sideMenu.addClass('active');
            }
        });

        $('li a', $sideMenu).on('click.sidemenu', function(){
            hideMenu();
        });

        $('body').on('click.sidemenu', function(e){
            var $target = $(e.target);

            if(!$target.hasClass('side-menu') && !$target.closest('.side-menu').length) {
                hideMenu();
            }
        });

        $sideMenu.append($toggle);

        $('ul', $sideMenu).css({'display' : 'none'});
    }());

    return parent;

}(RR || {}, jQuery));