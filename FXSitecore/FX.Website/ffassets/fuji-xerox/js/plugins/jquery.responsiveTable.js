/**
 * Responsive Table
 */
$.fn.responsiveTable = function(){
    return this.each(function(){
        var $table = $(this);

        if($table.hasClass('stacktable') && $table.hasClass('small-only')) return;

        //scrollCheck
        $(window).on('resize.responsiveTable', function(){
             if($('body').width() < $table.width()) {
                $table.wrap('<div class = "scrollable-table"/>');
                $(window).off('resize.responsiveTable');
            }
        }).trigger('resize.responsiveTable');
    });
};