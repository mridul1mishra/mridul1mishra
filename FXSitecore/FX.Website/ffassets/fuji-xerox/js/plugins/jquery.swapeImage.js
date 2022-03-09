/**
 * Swap banner image between background image and inline image
 * using a single breakpoint as trigger point.
 */
$.fn.swapeImage = function(options){
    var defaults = {
        breakpoint: 1024,
        reverse: false
    };

    options = $.extend(defaults, options);

    return this.each(function(){

        var $this = $(this),
             $img  = $this.find('img');

        var desktop = function(){
            $this.css({'background-image': ''});
            $img.show();
        };

        var mobile = function(){
            $this.css({'background-image': 'url('+ $img.attr('src') +')'});
            $img.hide();
        };

        $(window).on('resize.swapimage', function(){
            var winWidth = $(this)[0].innerWidth;

            if(winWidth < options.breakpoint) {
                if(options.reverse){
                    desktop();
                } else {
                    mobile();
                }
            } else {
                if(options.reverse){
                    mobile();
                } else {
                    desktop();
                }
            }

        }).trigger('resize.swapimage');
    });
};
