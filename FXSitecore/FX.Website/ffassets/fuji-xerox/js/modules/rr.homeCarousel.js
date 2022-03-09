/**
 * Homepage Carousel
 */
var RR = (function(parent, $, undefined){
    'use strict';

    var $homeCarousel = $('.home-carousel'),
        $slider,
        $slides,
        $customControl;

    (function(){

        if(!$homeCarousel.length) return;

        var $playToggle = $('.slider-custom-control__play-toggle'),
            $playToggleIcon = $('.icon', $playToggle),
            intervalSpeed = $homeCarousel.data('carousel-interval');

        if(typeof intervalSpeed === "undefined") intervalSpeed = 10000;

        $slider =  $('.slider', $homeCarousel);
        $customControl = $('.slider-custom-control', $homeCarousel);
        $slides = $('.slide', $homeCarousel);

        $slider.on('beforeChange', function(event, slick, currentSlide, nextSlide){
            $('.slider-custom-control__pager a', $customControl).removeClass('active');
            $('.slider-custom-control__pager li', $customControl)
                .eq(nextSlide)
                .children('a')
                .addClass('active'); 
        });

        $slider.slick({
            dots: false,
            arrows: false,
            infinite: true,
            speed: 300,
            autoplay: true,
            autoplaySpeed: parseInt(intervalSpeed),
            responsive: [
                {
                    breakpoint: 960,
                    settings: {
                        dots: true,
                        autoplay: false
                    }
                }]
        });

        $('a', $playToggle).on('click', function(){
            if($playToggle.hasClass('autoplay')) {
                $slider.slick('slickPause');
                $playToggle.removeClass('autoplay');
                $playToggleIcon
                    .removeClass('icon-controller-paus')
                    .addClass('icon-controller-play');
            } else {
                $slider.slick('slickPlay');
                $playToggle.addClass('autoplay');
                $playToggleIcon
                    .removeClass('icon-controller-play')
                    .addClass('icon-controller-paus');
            }
        });
    }());

    /**
     * Create and setup custom carousel pager
     */
    var setupCustomControl = function(){
        if($('.slider-custom-control__pager', $customControl).length || !$homeCarousel.length) return;

        var $customControlPager = $('<div class = "slider-custom-control__pager" />'),
            $ulEl          = $('<ul/>'),
            $liEl          = $('<li/>');

        $slides.each(function(){
            var $this = $(this),
                $liElClone = $liEl.clone();

            $liElClone.html($this.html());

            if($this.index() === 1) {
                $('a', $liElClone).addClass('active');
            }

            $ulEl.append( $liElClone );
        });

        $('a', $ulEl).on('click', function(e){
            var $this = $(this);

            e.preventDefault();

            $('a', $ulEl).removeClass('active');

            $this.addClass('active');

            $slider.slick('slickGoTo', $this.parent().index());

        });

        $customControlPager.append($ulEl);
        $customControl.prepend( $customControlPager);
    };

    var setupMobileBanner = function(){
        if(!$homeCarousel.length) return;

        $slides.each(function(){
            var $slide = $(this);

            $slide.css({'background-image': 'url('+ $slide.data('mobile-banner')+')'});
        });
    };

    parent.homeCarousel = {
        setupCustomControl: setupCustomControl,
        setupMobileBanner: setupMobileBanner
    };

    return parent;

}(RR || {}, jQuery));