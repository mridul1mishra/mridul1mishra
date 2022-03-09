/**
 * Main Navigation
 */
var RR = (function(parent, $, undefined){
    'use strict';

    var $newsroom = $('.newsroom'),
        $newsListing = $('.newsroom-listing', $newsroom);


    (function(){
        if(!$newsListing.length) return;

        var $corporateNews    = $('.corporate-news', $newsListing),
            $standardNews     = $('.standard-news', $newsListing),
            $yearFilter       = $('.listing-content__sort-result select', $newsListing),
            corporateNewsURL  = '/fuji-xerox/json/news.json',
            standardNewsURL   = '/fuji-xerox/json/news.json',
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
        });

        function loadNews(type, url, $newsObj, pageNum) {

            var $btn = $('.more-link a', $newsObj);

            $('.loader-animation', $newsObj).css({'display': 'inline-block'});

            $.ajax({
                url: url,
                data: {'page': pageNum, 'year': $yearFilter.val()},
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

}(RR || {}, jQuery));