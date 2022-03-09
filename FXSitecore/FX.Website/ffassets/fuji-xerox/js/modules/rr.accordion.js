/**
 * Accordion Widgets
 */
var RR = (function(parent, $, undefined){
    'use strict';

    var $accordionWidget = $('.accordion-widget');

    var setup = function(){

        if(!$accordionWidget.length) return;

        $accordionWidget.each(function(){
            accordionSetup($(this));
        });
    };

    /**
     * Setup each accordion Widget
     */
    var accordionSetup = function($obj){
        var $blocks = $('.accordion-widget__block', $obj);

        if(!$blocks.length) return;

        $blocks.each(function(){
            var $block = $(this),
                $content = $('.accordion-widget__block-content', $block),
                $title = $('.accordion-widget__block-title', $block),
                $toggleIcon = $('<i class = "toggle icon-angle-down"/>');

            $title.append($toggleIcon);

            if($block.index() !== 0) {
                $content.hide();
            } else {
                $block.addClass('active');

                $('.toggle', $title)
                    .removeClass('icon-angle-down')
                    .addClass('icon-angle-up');
            }

            $title.on('click', function(e){
                e.preventDefault();

                //$('.accordion-widget__block--title', $blocks).removeClass('active');
                $('.toggle', $block)
                    .removeClass('icon-angle-up')
                    .addClass('icon-angle-down');

                //$('.accordion-widget__block--content', $blocks).stop().slideUp();
                if($block.hasClass('active')) {
                    $block.removeClass('active');
                    $content.stop().slideUp();

                    $('.toggle', $title)
                        .addClass('icon-angle-down')
                        .removeClass('icon-angle-up');
                } else {
                    var $activeBlock = $accordionWidget.find('.accordion-widget__block.active');

                    $content.stop().slideDown(function() {
                        scrollToTarget($block);
                        $block.addClass('active');
                    });

                    $('.toggle', $title)
                        .removeClass('icon-angle-down')
                        .addClass('icon-angle-up');

                    $activeBlock.find('.accordion-widget__block-content').slideUp(function() {
                        $activeBlock.find('.accordion-widget__block-title .toggle')
                            .removeClass('icon-angle-up')
                            .addClass('icon-angle-down');
                        $activeBlock.removeClass('active');
                    });

                }
            });
        });
    };

    var scrollToTarget = function($target) {
        var $quickLinks = $('.quick-links');

        var scrollTop = ($quickLinks.length) ? $target.offset().top - $quickLinks.height() : $target.offset().top;

        $('html, body').stop().animate({
            scrollTop: scrollTop
        });
    };

    parent.accordion = {
        setup: setup
    };

    return parent;

}(RR || {}, jQuery));