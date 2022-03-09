/**
 * Modals
 */
var RR = (function(parent, $, undefined){

    var $modals = $('.modal'),
        $html   = $('html'),
        $fixedEle = $('.side-menu, .quick-links'),
        $win    = $(window);

    (function(){
        if(!$modals.length) return;

        $modals.each(function(){
            var $modal = $(this),
                $closeBtn = $('.modal-close-button a', $modal);

            $closeBtn.on('click', function(e){
                e.preventDefault();

                $modal.fadeOut(function(){
                     $html.removeClass('fancybox-margin fancybox-lock');
                     $fixedEle.removeClass('fancybox-margin');
                });
            });
        });

        $win.on('resize.modalbox scroll.modalbox', function(){
            var viewportHeight = $win.innerHeight(),
                scrollTop      =  $win.scrollTop();

            if($win.width() < 1024) {
                viewportHeight += 200;
            }

            $modals.css({ 'top': scrollTop, 'height': viewportHeight });

        }).trigger('resize.modalbox');
    }());

    /**
     * Modal Box Trigger
     */
    $('body').on('click', '.modalbox', function(e){
        e.preventDefault();

        var $link = $(this),
            $target = $($link.attr('href')),
            productid = $link.data('productid');

        $html.addClass('fancybox-margin fancybox-lock');
        $.each($fixedEle, function() {
            if ($(this).css('position') == 'fixed') {
                $(this).addClass('fancybox-margin');
            }
        });
        $win.trigger('resize.modalbox');
        $target.fadeIn();

        if(typeof productid !== 'undefined') {
            $('#productId').val(productid);
            $('#productField').val($('.product-name', $link.closest('.desc')).text());
        } else {
            $('#productId').val('undefined');
        }
    });

    return parent;

}(RR || {}, jQuery));