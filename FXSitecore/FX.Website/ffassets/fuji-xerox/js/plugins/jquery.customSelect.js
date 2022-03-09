/**
 * Custom Select
 */
$.fn.customSelect = function(){
    return this.each(function(){
        var $select = $(this),
            $label = $('<span class = "label"/>'),
            $icon = $('<span class = "icon-dropdown-arrow"/>'),
            $wrapper = $('<div class = "custom-select-wrapper"/>');

        $select.wrap( $wrapper );
        $select.after($label);
        $select.after($icon);

        $label.text($select.find(':selected').text());

        $select.on('change.customselect', function(){
            var $option = $(this).find(':selected'),
                url = $option.val();

            $label.text($option.text());
        });
    });
};
