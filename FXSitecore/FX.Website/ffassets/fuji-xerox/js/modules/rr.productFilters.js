/**
 * Product listing filters
 */
var RR = (function(parent, $, undefined){
    'use strict';

    var $listingSideFilters = $('.listing-side-filters'),
        $filterGroups       = $('.filter-group' , $listingSideFilters),
        loadLimit           = 10,
        currpage            = 1;

    function getQueryVariable(variable)
    {
           var query = window.location.search.substring(1);
           var vars = query.split("&");
           for (var i=0;i<vars.length;i++) {
                   var pair = vars[i].split("=");
                   if(pair[0] == variable){return pair[1];}
           }
           return(false);
    }

    (function(){

        if(!$filterGroups.length) return;

        var $clearSelectionBtn = $('.clear-filters-selection-btn', $listingSideFilters),
            $categoryField     = $('#categoryField'),
            $status            = $('.listing-content__result-status'),
            $resultMsg         = $('.msg', $status),
            $loader            = $('.loader-animation', $status),
            $sortSelect        = $('.listing-content__sort-result select'),
            source             = $('#results-template').html(),
            template           = Handlebars.compile(source),
            results,
            resultsCopy;

        $clearSelectionBtn.on('click', function(e){
            e.preventDefault();

            $('input', $listingSideFilters).prop('checked', false);

            fetchProducts( $('.submit-btn', $listingSideFilters).parents('form'));
        });

        /**
         * For each filters Group...
         */
        $filterGroups.each(function(){
            var $this = $(this),
                $filterSubgroups = $('.filter-subgroup', $this),
                $advancedFilters = $('.advanced-filters', $this),
                $advancedFiltersToggle = $('.advanced-options-toggle', $this);

            // Toggle advanced filters
            if($advancedFilters.length) {
                $advancedFiltersToggle.on('click', function(e){
                    e.preventDefault();

                    if($advancedFilters.css('display') === 'block') {
                        $advancedFilters.slideUp();
                        $('.icon', $advancedFiltersToggle)
                            .addClass('icon-plus')
                            .removeClass('icon-minus');
                    } else {
                        $advancedFilters.slideDown();
                        $('.icon', $advancedFiltersToggle)
                            .addClass('icon-minus')
                            .removeClass('icon-plus');
                    }
                });
            }

            /**
             * For each filter...
             */
            $filterSubgroups.each(function(){
                var $subgroup = $(this),
                    $fields = $('.filter-subgroup__fields', $subgroup),
                    $toggle = $('h4', $subgroup);

                if($subgroup.index() === 0) {
                    $fields.show();
                    $toggle.append($('<i class = "icon icon-minus"/>'));
                } else {
                    $toggle.append($('<i class = "icon icon-plus"/>'));
                }

                $toggle.attr('tabindex', 0);

                /**
                 * Collapse/Expand the fields
                 */
                $toggle.on('click keypress', function(){
                    var $this = $(this),
                        $filters = $this.parent().parent();

                    if($fields.css('display') === 'none') {
                        $('.filter-subgroup__fields', $filters).stop().slideUp();
                        $('h4 .icon', $subgroup.parent())
                            .removeClass('icon-minus')
                            .addClass('icon-plus');

                        $fields.stop().slideDown();

                        $('.icon', $toggle)
                            .removeClass('icon-plus')
                            .addClass('icon-minus');
                    } else {
                        $fields.stop().slideUp();
                        $('.icon', $toggle)
                            .removeClass('icon-minus')
                            .addClass('icon-plus');
                    }
                });

                /**
                 * Check/uncheck all inputs in a group with scope 'all'
                 */
                $('input[type="checkbox"]', $fields).each(function(){
                    var $cb = $(this);

                    if($cb.data('scope') === 'all') {

                        $cb.on('change', function(){
                           if($cb[0].checked) {
                                $('input[type="checkbox"]', $fields).prop('checked', true);
                            } else {
                                $('input[type="checkbox"]', $fields).prop('checked', false);
                            }
                        });
                    }
                });
            });
        });

        /**
         * Switch product listing filter options
         */
        $('.categories-toggle li').each(function(){
            var $this = $(this),
                $anchor = $('a', $this),
                categoryValue = $anchor.text().toLowerCase(),
                listing = getQueryVariable('listing');

            if(listing === categoryValue) {
                $('.categories-toggle li a').removeClass('active');
                $anchor.addClass('active');

                swapMenu($anchor.data('filter'));
            }

            $anchor.on('click', function(e){
                e.preventDefault();

                $('.categories-toggle li a').removeClass('active');

                $(this).addClass('active');

                swapMenu($(this).data('filter'));
            });
        });
        

        function swapMenu(classname, categoryValue) {
            $categoryField.val(categoryValue);
            $filterGroups.hide();

            $(classname, $listingSideFilters).show();
        }

        /**
         * Change sorting option
         */
        $sortSelect.on('change', function(){

            if(typeof results !== 'undefined') {
                $('.listing-content__results').empty();
                toggleSort($sortSelect.val());

                currpage = 1;

                parsePage(1);
                toggleLoadMore();
            }
        });

        /**
         * Submitting
         */
        $('.submit-btn', $listingSideFilters).parents('form').on('submit', function(e){
            e.preventDefault();

            var $form = $(this);

            fetchProducts($form);
        });

        fetchProducts( $('.submit-btn', $listingSideFilters).parents('form'));

        /**
         * Fetch Products data from server
         */
        function fetchProducts($form) {
            $loader.css({'display': 'inline-block'});

            $.ajax({
                url: '/fuji-xerox/json/product-listing.json',
                data: $form.serialize(),
                dataType: 'json'
            }).done(function(data) {
                $('.listing-content__results').empty();

                $loader.css({'display': ''});

                resultsCopy = data.results;

                results = resultsCopy.slice().concat();

                currpage = 1;

                if(results.length > 0) {
                    $('.total-results', $status).text(results.length);
                    $('.success-msg', $status).slideDown();

                    toggleSort($sortSelect.val());
                    parsePage(1);
                    toggleLoadMore();
                } else {
                    $('.error-msg', $status).slideDown();
                }
            });
        }

        /**
         * Load more results
         */
        $('.load-more-btn').on('click', function(e){
            e.preventDefault();

            parsePage(currpage);
            toggleLoadMore();
        });

        /**
         * Toggle Filter Option
         */
        function toggleSort(searchParam) {

            results = resultsCopy.slice().concat(); // Deep copy results from original data to reset sorting.

            switch(searchParam) {
                case 'latest' :
                    results.sort(function(a,b){
                            return new Date(a.date) < new Date(b.date) ? 1 : -1;
                        }
                    );

                    break;
                case 'new' :
                    results.sort(function(a,b){
                            if(typeof a.newitem !== 'undefined' && typeof b.newitem !== 'undefined') return 0;
                            if(typeof a.newitem !== 'undefined' && typeof b.newitem === 'undefined') return -1;
                            if(typeof a.newitem === 'undefined' && typeof b.newitem !== 'undefined') return 1;
                        }
                    );
                    break;
                case 'best seller' :
                    results.sort(function(a,b){
                            if(typeof a.bestseller !== 'undefined' && typeof b.bestseller !== 'undefined') return 0;
                            if(typeof a.bestseller !== 'undefined' && typeof b.bestseller === 'undefined') return -1;
                            if(typeof a.bestseller === 'undefined' && typeof b.bestseller !== 'undefined') return 1;
                        }
                    );
                    break;
                case 'recommended' :
                    results.sort(function(a,b){
                            if(typeof a.recommended !== 'undefined' && typeof b.recommended !== 'undefined') return 0;
                            if(typeof a.recommended !== 'undefined' && typeof b.recommended === 'undefined') return -1;
                            if(typeof a.recommended === 'undefined' && typeof b.recommended !== 'undefined') return 1;
                        }
                    );
                    break;
            }

            bubblePromotion(); // bubbble promotion to the top of the list
        }

        /**
         * Bubble promotion in results to the top
         */
        function bubblePromotion() {
            var temp;

            for (var i = 0; i < results.length - 1; i++) {

                if(typeof results[i].promoted !== 'undefined') {

                     for (var x = i; x > 0; x--) {
                        temp = results[x-1];
                        results[x-1] = results[x];
                        results[x] = temp;
                     }
                }
            }
        }

        /**
         * Toggle load more button visibility
         */
        function toggleLoadMore() {
            if(currpage * loadLimit < results.length) {
                currpage++;
                $('.load-more-results').show();
            } else {
                $('.load-more-results').hide();
            }
        }

        /**
         * Render page result on load more
         */
        function parsePage(page) {
            var lowerLimit = (page - 1) * loadLimit,
                upperLimit = page * loadLimit,
                pageResults = {"results" : []},
                interval = 200;

            for(var i = lowerLimit; i < upperLimit; i++) {
                if(typeof results[i] !== 'undefined')
                    pageResults.results.push(results[i]);
            }

            $('.listing-content__results').append($(template(pageResults)));

            /**
             * Fade In
             */
            $('.listing-content__results li').each(function(){
                var $this = $(this);

                if($this.hasClass('visible')) return;

                setTimeout(function(){
                    $this.addClass('visible');

                }, interval);

                interval = interval + 25;
            });
        }
    }());

    return parent;

}(RR || {}, jQuery));