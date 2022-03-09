/*! fuji-xerox 2016-11-04 */
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

}(RR || {}, jQuery));;var RR = (function(parent, $, undefined){
    'use strict';

    var $quicklinks = $('.quick-links'),
        $article    = $('.article'),
        $topBanner  = $('.top-banner'),
        offsetTop   = 0;

    (function(){
        if(!$quicklinks.length) return;

        var $ulEl = $('<ul/>'),
            $liEl = $('<li/>'),
            $anchorEl = $('<a href = ""/>'),
            $anchorLinks = $('a.anchor-link', $article),
            linkNum = $anchorLinks.length,
            $newliEl,
            $newAnchorEl,
            $target;

        for(var i = 1; i < linkNum + 1; i++) {
            $newliEl = $liEl.clone();
            $newAnchorEl = $anchorEl.clone();
            $target = $anchorLinks.eq(i - 1);

            $target.attr('id', 'content' + i);

            $newAnchorEl.text($target.data('label'));
            $newAnchorEl.attr('href', '#' + $target.attr('id'));

            $newliEl.append($newAnchorEl);
            $ulEl.append($newliEl);
        }

        $('.wingspan', $quicklinks).append($ulEl);

        $('a', $quicklinks).on('click', function(e){
            e.preventDefault();

            var $target = $($(this).attr('href'));

             $('html, body').stop(true, true).animate({
                scrollTop: $target.offset().top - offsetTop
            }, 500);
        });
    }());

    var setupSticky = function(){

        if(!$quicklinks.length) return;

        // var quicklinksOffset = $quicklinks.offset().top;

         offsetTop  =  $quicklinks.height() + 20;

        $(window).on('scroll.sticky', function(){
            var scrollAmt = $(this).scrollTop(),
                topbannerBottom = $topBanner.offset().top + $topBanner.height();

            if(topbannerBottom < scrollAmt) {
                $quicklinks.addClass('sticky');
                $article.css({'padding-top':offsetTop});
            } else {
                $quicklinks.removeClass('sticky');
                $article.css({'padding-top': ''});
            }
        }).trigger('scroll.sticky');
    };

    var destroySticky = function() {
        offsetTop = 10;

        $(window).off('scroll.sticky');
        $quicklinks.removeClass('sticky');
        $article.css({'padding-top': ''});
    };

    parent.articleQuicklinks = {
        setupSticky: setupSticky,
        destroySticky: destroySticky
    };

    return parent;

}(RR || {}, jQuery));;(function(parent, $){
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

}(RR || {}, jQuery));;var RR = (function(parent, $, undefined){
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

}(RR || {}, jQuery));;var RR = (function(parent, $, undefined){

    var $modals = $('.modal'),
        $html   = $('html'),
        $fixedEle = $('.side-menu, .quick-links'),
        $win    = $(window),
        resizeTimeout;

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
                     console.log('close')

                     clearInterval(resizeTimeout);
                });
            });
        });

        $win.on('resize.modalbox scroll.modalbox', function(){
            var scrollTop      =  $win.scrollTop(),
                viewportHeight = $win.innerHeight();

            // if($win.width() < 1024) {
            //     viewportHeight += 200;
            // }

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
            productid = $link.data('productid'),
            iframeURL = $link.data('iframe');

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
            $target.find('.modal-form').html('<iframe src="' + iframeURL + '" />');
            $('iframe').load(function() {
                clearInterval(resizeTimeout);
                var iframe = this,
                oriHeight = $(this.contentWindow.document.body).height();
                this.style.height = ($(this.contentWindow.document.body).height() + 37) + 'px';

                resizeTimeout = setInterval(function() {
                    if ($(iframe.contentWindow.document.body).height() != oriHeight) {
                        iframe.style.height = ($(iframe.contentWindow.document.body).height() + 37) + 'px';
                        clearInterval(resizeTimeout);
                    }
                }, 200);
            });

        } else {
            $('#productId').val('undefined');
        }
    });

    return parent;

}(RR || {}, jQuery));;var RR = (function(parent, $, undefined){
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

}(RR || {}, jQuery));;var RR = (function(parent, $, undefined){
    'use strict';

    var $newsroom = $('.newsroom'),
        $newsListing = $('.newsroom-listing', $newsroom),
        buildJS = true,
        corporateNewsURL  = '/fuji-xerox/json/news.json',
        standardNewsURL   = '/fuji-xerox/json/news.json';

    if (buildJS) {
        corporateNewsURL  = '/api/getNewsByCategory';
        standardNewsURL   = '/api/getNewsByCategory';
    }



    (function(){
        if(!$newsListing.length) return;

        var $corporateNews    = $('.corporate-news', $newsListing),
            $standardNews     = $('.standard-news', $newsListing),
            $yearFilter       = $('.listing-content__sort-result select', $newsListing),
            corporateNewsPage = 1,
            standardNewsPage  = 1,
            corporateEnd      = false,
            standardEnd       = false,
            source            = $('#results-template').html(),
            template          = Handlebars.compile(source);

        /**
         * load more standard news
         */
        $('.more-link a', $standardNews).on('click', function(e) {
            e.preventDefault();

            loadNews('standard', standardNewsURL, $standardNews, standardNewsPage);

            $(this).blur();
        });

        /**
         * load more corporate news
         */
        $('.more-link a', $corporateNews).on('click', function(e) {
            e.preventDefault();

            loadNews('corporate', corporateNewsURL, $corporateNews, corporateNewsPage);

            $(this).blur();
        });


        /**
         * Load all news sections
         */
        $('.load-all-news-btn a', $newsroom).on('click', function(e){
            e.preventDefault();

            loadNews('standard', standardNewsURL, $standardNews, standardNewsPage);
            loadNews('corporate', corporateNewsURL, $corporateNews, corporateNewsPage);

            $(this).blur();
        });

        /**
         * On year archive filter
         */
         $yearFilter.on('change', function(){
            $('.news-results', $standardNews).empty();
            $('.news-results', $corporateNews).empty();

            corporateNewsPage = 1;
            standardNewsPage  = 1;

            loadNews('standard', standardNewsURL, $standardNews, standardNewsPage);
            loadNews('corporate', corporateNewsURL, $corporateNews, corporateNewsPage);
        });

         /**
          * On page load
          */
        $(window).on('load.newsroom', function(){
            loadNews('standard', standardNewsURL, $standardNews, standardNewsPage);
            loadNews('corporate', corporateNewsURL, $corporateNews, corporateNewsPage);

            $('.newsroom-featured__block .desc, .newsroom-featured__download-block .desc').matchHeight();
            $('.newsroom-featured > .col-sm-7, .newsroom-featured > .col-sm-5').matchHeight();
            $('.newsroom-featured__block .desc, .newsroom-featured__download-block .desc').matchHeight._afterUpdate = function() {
                $('.newsroom-featured > .col-sm-7, .newsroom-featured > .col-sm-5').matchHeight();
            };
        });

        function loadNews(type, url, $newsObj, pageNum) {

            var $btn = $('.more-link a', $newsObj);

            $('.loader-animation', $newsObj).css({'display': 'inline-block'});

            $.ajax({
                url: url,
                data: {'category': $newsObj.data('category'), 'page': pageNum, 'year': $yearFilter.val()},
                dataType: 'json'
            }).done(function(data) {
                var pageCount,
                    $noMoreResult = $('<li class = "no-result">No more news</li>');

                $('.loader-animation', $newsObj).css({'display': ''});

                // Type of news request
                if(type === 'standard') {
                    standardNewsPage++;
                    pageCount = standardNewsPage;
                } else {
                    corporateNewsPage++;
                    pageCount = corporateNewsPage;
                }

                // Append results if end page is not reached
                if(pageCount - 2 < parseInt(data.totalPage)) {
                    $('.news-results', $newsObj).append($(template(data)));
                } else {
                    $('.news-results', $newsObj).append($noMoreResult);

                    setTimeout(function(){
                        $noMoreResult.slideUp(function(){
                            $(this).remove();
                        });
                    }, 2000);
                }

                if(pageCount > parseInt(data.totalPage)){
                    $btn.hide();
                } else {
                    $btn.show();
                }
            });
        }
    }());


    return parent;

}(RR || {}, jQuery));;var RR = (function(parent, $, undefined){
    'use strict';

    var $listingSideFilters = $('.listing-side-filters'),
        $filterGroups       = $('.filter-group' , $listingSideFilters),
        loadLimit           = 10,
        currpage            = 1,
        buildJS             = true,
        filterURL           = '/fuji-xerox/json/product-listing.json';

    if (buildJS) {
        filterURL = '/api/GetProductListing';
    }

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

        Handlebars.registerHelper("printButtonLink", function(popupLink, buttonLink, productName)
        {
            if (popupLink !== '')
            {
                buttonLink = popupLink + "?formId=" + encodeURI(buttonLink) + "&amp;product%20Name=" + encodeURI(productName);
            }
            return buttonLink;
        });
        Handlebars.registerHelper('ifCond', function (v1, operator, v2, options)
        {
            switch (operator) {
                case '==':
                    return (v1 == v2) ? options.fn(this) : options.inverse(this);
                case '===':
                    return (v1 === v2) ? options.fn(this) : options.inverse(this);
                case '<':
                    return (v1 < v2) ? options.fn(this) : options.inverse(this);
                case '<=':
                    return (v1 <= v2) ? options.fn(this) : options.inverse(this);
                case '>':
                    return (v1 > v2) ? options.fn(this) : options.inverse(this);
                case '>=':
                    return (v1 >= v2) ? options.fn(this) : options.inverse(this);
                case '&&':
                    return (v1 && v2) ? options.fn(this) : options.inverse(this);
                case '||':
                    return (v1 || v2) ? options.fn(this) : options.inverse(this);
                case '!==':
                    return (v1 !== v2) ? options.fn(this) : options.inverse(this);
                default:
                    return options.inverse(this);
            }
        });


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

            if(listing === encodeURI(categoryValue)) {
                $('.categories-toggle li a').removeClass('active');
                $anchor.addClass('active');

                swapMenu($anchor.data('filter'), $anchor.data('value'));
            }

            $anchor.on('click', function(e){
                e.preventDefault();

                $('.categories-toggle li a').removeClass('active');

                $(this).addClass('active');

                swapMenu($(this).data('filter'), $anchor.data('value'));
            });
        });


        function swapMenu(classname, categoryValue) {
            $categoryField.val(categoryValue);
            $filterGroups.hide();

            $(classname, $listingSideFilters).show();

            fetchProducts($('.submit-btn', $listingSideFilters).parents('form'), true);
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

        fetchProducts( $('.submit-btn', $listingSideFilters).parents('form'), true);

        /**
         * Fetch Products data from server
         */
        function fetchProducts($form, categoryChange) {
            categoryChange = categoryChange || false;

            $loader.css({'display': 'inline-block'});

            $.ajax({
                url: filterURL,
                method: 'post',
                data: $form.serialize(),
                dataType: 'json'
            }).done(function(data) {
                $('.listing-content__results').empty();

                $loader.css({'display': ''});

                resultsCopy = data.results;

                results = resultsCopy.slice().concat();

                if (typeof recommendedProducts !== 'undefined') {
                    // Get recommended products' id and update to existing result json
                    var recommendedProductIds = [];

                    recommendedProducts.forEach(function(product, index) {
                        recommendedProductIds.push(product.id);
                    });

                    resultsCopy.forEach(function(result, index){
                        if ($.inArray(result.productid, recommendedProductIds) > -1) {
                            result['recommended'] = true;
                        };
                    });
                }

                currpage = 1;
                if(results.length > 0) {
                    if (!categoryChange) {
                        $('.total-results', $status).text(results.length);
                        $('.success-msg', $status).slideDown();
                    } else {
                        $('.success-msg', $status).slideUp();
                    }
                    $('.error-msg', $status).slideUp();

                    toggleSort($sortSelect.val());
                    parsePage(1);
                    toggleLoadMore();
                } else {
                    $('.error-msg', $status).slideDown();
                    $('.total-results', $status).text(0);
                    $('.success-msg', $status).slideUp();
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
                            if (new Date(a.date) < new Date(b.date)) return 1;
                            if (new Date(a.date) > new Date(b.date)) return -1;
                            if (new Date(a.date) == new Date(b.date)) return 0;
                        }
                    );

                    break;
                case 'promotion' :
                    results.sort(function(a,b){
                            if(a.promoted == true && b.promoted == true) return 0;
                            if(a.promoted == true && b.promoted == false) return -1;
                            if(a.promoted == false && b.promoted == true) return 1;
                        }
                    );
                    break;
                case 'best seller' :
                    results.sort(function(a,b){
                            if(a.bestseller == true && b.bestseller == true) return 0;
                            if(a.bestseller == true && b.bestseller == false) return -1;
                            if(a.bestseller == false && b.bestseller == true) return 1;
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

            // bubblePromotion(); // bubbble promotion to the top of the list
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
            $('.listing-content__results > li').matchHeight();

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

}(RR || {}, jQuery));;var RR = (function(parent, $, undefined){
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

}(RR || {}, jQuery));;var RR = (function(parent, $, undefined){
    'use strict';

    var $mapper = $('.solution-mapper'),
        buildJS = true,     //switch to true for deployment
        filterResultURL = '/fuji-xerox/json/solutionMapper.php',
        filterResultALL  = '/fuji-xerox/json/filter-results.json',
        filterOptionURL = '/fuji-xerox/json/selection.json';

    if (buildJS) {
        filterResultURL = '/api/getFilterResults';
        filterResultALL = '/api/getFilterResults';
        filterOptionURL = '/api/getFilterOptions';
    }

    (function(){

        if(!$mapper.length) return;

        var $filterIndustry      = $('#industryFilter'),
        $filterDepartment    = $('#departmentFilter'),
        $filterService       = $('#serviceFilter'),
        $filterBusiness      = $('#businessFilter'),
        $filterIndustryGrp   = $filterIndustry.parent(),
        $filterDepartmentGrp = $filterDepartment.parent(),
        $filterServiceGrp    = $filterService.parent(),
        $filterBusinessGrp   = $filterBusiness.parent(),
        $mapperIntro         = $('.solution-mapper__intro', $mapper),
        $mapperResults       = $('.solution-mapper__results', $mapper),
        $clearFilterBtn      = $('.btn-clear-filter', $mapper),
        $btnShowAll          = $('.btn-show-all', $mapper),
        selectionData = {},
        cookiesData = [
            {
                "name": "industry",
                "value": Cookies.get('industry')
            },
            {
                "name": "department",
                "value": Cookies.get('department')
            },
            {
                "name": "service",
                "value": Cookies.get('service')
            },
            {
                "name": "business",
                "value": Cookies.get('business')
            }
        ],
        filters = [
            {
                "name" : "industry",
                "groupName" : "industries",
                "filter" : $filterIndustry,
                "group": $filterIndustryGrp
            },
            {
                "name" : "department",
                "groupName": "departments",
                "filter" : $filterDepartment,
                "group": $filterDepartmentGrp
            },
            {
                "name" : "service",
                "groupName" : "services",
                "filter" : $filterService,
                "group": $filterServiceGrp
            },
            {
                "name" : "business",
                "groupName" : "businesses",
                "filter" : $filterBusiness,
                "group": $filterBusinessGrp
            }
        ];

        /**
         * Filter Functions
         */
         function filterSetup($obj, $nextFilterGrp, idx) {
            $obj.on('change.filter', function(){
                var $filter = $(this),
                value   = $filter.val();

                if(value !== "") {
                    updateOnChange($nextFilterGrp, idx, true);
                } else {
                    clearCookies(idx);
                    hideFilters(idx);
                }

                getResult();
            });
        }

        function updateOnChange($nextFilterGrp, idx, toSetCookies) {
            if (toSetCookies) setCookies(idx);

            if (typeof $nextFilterGrp !== 'undefined') {
                populateNextFilter($nextFilterGrp, idx);
                // $nextFilterGrp.fadeIn();
            }
        }

        /**
         * jsonQuery
         */
        var jsonIndustryChoice,
            jsonDepartmentChoice,
            jsonServiceChoice,
            jsonBusinessChoice;
        function jsonQuery(value, json, idx) {
            var industryChoice,
            departmentChoice,
            serviceChoice,
            businessChoice;

            switch(idx) {
                case 4:
                    jsonServiceChoice = jsonQuery(value, json, idx - 1);    //recursive call
                    json.industries[jsonIndustryChoice].departments[jsonDepartmentChoice].services[jsonServiceChoice].businesses.forEach(function(business, bizIndex) {
                        if (business.id == Cookies.get('business')) {
                            businessChoice = bizIndex;
                        }
                    });
                    return businessChoice;
                case 3:
                    jsonDepartmentChoice = jsonQuery(value, json, idx - 1); //recursive call
                    json.industries[jsonIndustryChoice].departments[jsonDepartmentChoice].services.forEach(function(service, serIndex) {
                        if (service.id == Cookies.get('service')) {
                            serviceChoice = serIndex;
                        }
                    });
                    return serviceChoice;
                case 2:
                    jsonIndustryChoice = jsonQuery(value, json, idx - 1);   //recursive call
                    json.industries[jsonIndustryChoice].departments.forEach(function(department, depIndex){
                        if (department.id == Cookies.get('department')) {
                            departmentChoice = depIndex;
                        }
                    });
                    return departmentChoice;
                case 1:
                    json.industries.forEach(function(industry, indIndex){
                        if (industry.id == Cookies.get('industry')) {
                            industryChoice = indIndex;
                        }
                    });
                    return industryChoice;
            }
        }

        /**
         * Set cookies
         */
         function setCookies(idx) {
            var industryChoice, departmentChoice, serviceChoice, businessChoice;
            switch(idx) {
                case 4:
                    businessChoice = $filterBusiness.val();
                    Cookies.set('business', businessChoice, {expires: 365});
                    /* falls through */
                case 3:
                    serviceChoice = $filterService.val();
                    Cookies.set('service', serviceChoice, {expires: 365});
                    /* falls through */
                case 2:
                    departmentChoice = $filterDepartment.val();
                    Cookies.set('department', departmentChoice, {expires: 365});
                    /* falls through */
                case 1:
                    industryChoice = $filterIndustry.val();
                    Cookies.set('industry', industryChoice, {expires: 365});
                    /* falls through */
            }
        }

        /**
         * Clear cookies
         */
         function clearCookies(idx) {
            switch(idx) {
                case 1:
                    Cookies.remove('industry');
                    /* falls through */
                case 2:
                    Cookies.remove('department');
                    /* falls through */
                case 3:
                    Cookies.remove('service');
                    /* falls through */
                case 4:
                    Cookies.remove('business');
                    /* falls through */
            }
        }

        /**
         * Populate filter after one change
         */
         function populateNextFilter($nextFilterGrp, idx) {
            var industryChoice, departmentChoice, serviceChoice, businessChoice,
            template, context,
            child;
            switch(idx) {
                case 1:
                industryChoice = jsonQuery(Cookies.get('industry'), selectionData, 1);
                context = selectionData.industries[industryChoice];
                child = context.departments;
                break;
                case 2:
                industryChoice = jsonQuery(Cookies.get('industry'), selectionData, 1);
                departmentChoice = jsonQuery(Cookies.get('department'), selectionData, 2);
                context = selectionData.industries[industryChoice].departments[departmentChoice];
                child = context.services;
                break;
                case 3:
                industryChoice = jsonQuery(Cookies.get('industry'), selectionData, 1);
                departmentChoice = jsonQuery(Cookies.get('department'), selectionData, 2);
                serviceChoice = jsonQuery(Cookies.get('service'), selectionData, 3);
                context = selectionData.industries[industryChoice].departments[departmentChoice].services[serviceChoice];
                child = context.businesses;
                break;
            }
            if (child.length) {
                compileTemplate($nextFilterGrp, context, "");
                $nextFilterGrp.fadeIn();
            } else {
                $nextFilterGrp.fadeOut();
            }
            hideFilters(idx + 1);
        }


        /**
         * Compile selection template
         */
         function compileTemplate($filterGrp, context, value) {
            var template = Handlebars.compile($filterGrp.find('script').html());
            $filterGrp.find('select').html(template(context));
            $filterGrp.find('select').val(value).trigger('change.customselect');
        }

        /**
         * Hide Filters
         */
         function hideFilters(idx) {
            switch(idx) {
                case 1:
                    $filterDepartmentGrp.fadeOut();
                    $filterDepartment.val(null);
                    /* falls through */
                case 2:
                    $filterServiceGrp.fadeOut();
                    $filterService.val(null);
                    /* falls through */
                case 3:
                    $filterBusinessGrp.fadeOut();
                    $filterBusiness.val(null);
                    /* falls through */
                default:
                    break;
            }
        }

        /**
         * Get Results
         */
         function getResult() {
            if($filterIndustry.val() !== "") {

                $.ajax({
                  url: filterResultURL,
                  method: 'post',
                  data: {
                    industry: $filterIndustry.val(),
                    department: $filterDepartment.val(),
                    service: $filterService.val(),
                    business: $filterBusiness.val()
                },
                dataType: 'json'
            }).done(function(data) {

                var source = $('#results-template').html();
                var template = Handlebars.compile(source);

                $mapperResults.find('div:first-child').html(template(data));

                $mapperIntro.slideUp();
            });

            $btnShowAll.fadeOut(function(){
                $clearFilterBtn.fadeIn();
            });

            } else {
                clearAllResult();
            }
        }

        /**
         * Show All
         */
         function getAllResult() {
            $.ajax({
              url: filterResultALL,
              data: {
                queryAll : 'true'
            },
            dataType: 'json'
        }).done(function(data) {

            var source = $('#results-template').html();
            var template = Handlebars.compile(source);

            $mapperResults.find('div:first-child').html(template(data));

            $mapperIntro.slideUp();

            $btnShowAll.text($btnShowAll.data('hide-label'));
            $btnShowAll.parent().addClass('showed-all');

        });

        scrollToTop();

        $btnShowAll.fadeOut(function(){
            $clearFilterBtn.fadeIn();
        });
    }

        /**
         * Clear All Results
         */
         function clearAllResult() {
            $mapperIntro.slideDown();
            $mapperResults.find('div:first-child').empty();
            $btnShowAll.text($btnShowAll.data('show-label'));
            $btnShowAll.parent().removeClass('showed-all');

            $clearFilterBtn.fadeOut(function(){
                $btnShowAll.fadeIn();
            });

         }

        /**
         * Automatic Scroll after filter
         */
         function scrollToTop() {
            $('html, body').animate({
                scrollTop: $mapper.offset().top
            });
        }

        /**
         * Preselect with cookies
         */
         var elementIndex = 0,
            choice = [];
         function cookiePreselect() {
            if (typeof cookiesData[elementIndex].value !== 'undefined') {
                var value = cookiesData[elementIndex].value,
                context;

                choice.push(jsonQuery(value, selectionData, elementIndex + 1));
                switch (elementIndex) {
                    case 0:
                        filters[elementIndex].filter.val(value).trigger('change.customselect');
                        if (selectionData.industries[choice[elementIndex]].departments.length) {
                            $filterDepartmentGrp.fadeIn();
                            compileTemplate($filterDepartmentGrp, selectionData.industries[choice[elementIndex]], Cookies.get('department'));
                        }
                        break;
                    case 1:
                        if (selectionData.industries[choice[elementIndex - 1]].departments[choice[elementIndex]].services.length) {
                            $filterServiceGrp.fadeIn();
                            compileTemplate($filterServiceGrp, selectionData.industries[choice[elementIndex - 1]].departments[choice[elementIndex]], Cookies.get('service'));
                        }
                        break;
                    case 2:
                        if (selectionData.industries[choice[elementIndex - 2]].departments[choice[elementIndex - 1]].services[choice[elementIndex]].businesses.length) {
                            $filterBusinessGrp.fadeIn();
                            compileTemplate($filterBusinessGrp, selectionData.industries[choice[elementIndex - 2]].departments[choice[elementIndex - 1]].services[choice[elementIndex]], Cookies.get('business'));
                        }
                        break;
                }

                elementIndex++;
                if (elementIndex < 3 && typeof cookiesData[elementIndex].value !== 'undefined') {
                    cookiePreselect();
                } else {
                    getResult();
                }
            }
        }


        filterSetup($filterIndustry, $filterDepartmentGrp, 1);
        filterSetup($filterDepartment, $filterServiceGrp, 2);
        filterSetup($filterService, $filterBusinessGrp, 3);
        filterSetup($filterBusiness, undefined, 4);

        hideFilters(1);

        /**
         * Retrieve selection data
         */
         (function getSelectionData() {
            $.ajax({
                url: filterOptionURL,
                dataType: 'json'
            }).done(function(data) {
                selectionData = data;
                compileTemplate($filterIndustryGrp, selectionData, "");
                cookiePreselect();
            });
        }());

         $clearFilterBtn.on('click', function(e){
            e.preventDefault();

            $filterIndustry.val($('option:first-child', $filterIndustry).val());
            $filterIndustry.trigger('change');

            //clear all cookies
            clearCookies(1);

            hideFilters(1);
        });

         $btnShowAll.on('click', function(e){
            e.preventDefault();
            getAllResult();
        });
     }());


return parent;

}(RR || {}, jQuery));