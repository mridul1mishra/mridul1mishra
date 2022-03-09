/**
 * Solution Mapper
 */
 var RR = (function(parent, $, undefined){
    'use strict';

    var $mapper = $('.solution-mapper'),
        buildJS = true,     //switch to true for deployment
        filterResultURL = '/fuji-xerox/json/solutionMapper.php',
        filterResultALL  = '/fuji-xerox/json/filter-results.json',
        filterOptionURL = '/fuji-xerox/json/selection.json';

    if (buildJS) {
        filterResultURL = 'api/getFilterResults';
        filterResultALL = 'api/getFilterResults';
        filterOptionURL = 'api/getFilterOptions';
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