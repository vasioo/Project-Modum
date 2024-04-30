var userItemsSelectionList = (function () {
    function init($container) {
        let $mainCategoryName = $container.find('input[type="radio"][name="exampleRadios"]:checked').val(),
            $minPrice = $container.find('#fromInput'),
            $maxPrice = $container.find('#toInput'),
            $subcategoryItems = $container.find('.subcategory-item'),
            $sizeColumns = $container.find('.size-col'),
            $colorColumns = $container.find('.color-col'),
            $sortByDropdown = $container.find('#sortBy'),
            $sortByBrands = $container.find('.brand-checkbox'),
            $sortByLTCs = $container.find('.ltc-checkbox');

        function controlFromSlider(fromSlider, toSlider, fromInput) {
            const [from, to] = getParsed(fromSlider, toSlider);
            fillSlider(fromSlider, toSlider, '#C6C6C6', '#CC5500', toSlider);
            if (from > to) {
                fromSlider.value = to;
                fromInput.value = to;
            } else {
                fromInput.value = from;
            }
        }

        function controlToSlider(fromSlider, toSlider, toInput) {
            const [from, to] = getParsed(fromSlider, toSlider);
            fillSlider(fromSlider, toSlider, '#C6C6C6', '#CC5500', toSlider);
            setToggleAccessible(toSlider);
            if (from <= to) {
                toSlider.value = to;
                toInput.value = to;
            } else {
                toInput.value = from;
                toSlider.value = from;
            }
        }

        function getParsed(currentFrom, currentTo) {
            const from = parseInt(currentFrom.value, 10);
            const to = parseInt(currentTo.value, 10);
            return [from, to];
        }

        function fillSlider(from, to, sliderColor, rangeColor, controlSlider) {
            const rangeDistance = to.max - to.min;
            const fromPosition = from.value - to.min;
            const toPosition = to.value - to.min;
            controlSlider.style.background = `linear-gradient(
          to right,
          ${sliderColor} 0%,
          ${sliderColor} ${(fromPosition) / (rangeDistance) * 100}%,
          ${rangeColor} ${((fromPosition) / (rangeDistance)) * 100}%,
          ${rangeColor} ${(toPosition) / (rangeDistance) * 100}%, 
          ${sliderColor} ${(toPosition) / (rangeDistance) * 100}%, 
          ${sliderColor} 100%)`;
        }

        function setToggleAccessible(currentTarget) {
            const toSlider = document.querySelector('#toSlider');
            if (Number(currentTarget.value) <= 0) {
                toSlider.style.zIndex = 2;
            } else {
                toSlider.style.zIndex = 0;
            }
        }

        if ($mainCategoryName != null) {
            const fromSlider = document.querySelector('#fromSlider');
            const toSlider = document.querySelector('#toSlider');
            const fromInput = document.querySelector('#fromInput');
            const toInput = document.querySelector('#toInput');

            fillSlider(fromSlider, toSlider, '#C6C6C6', '#CC5500', toSlider);
            setToggleAccessible(toSlider);
            fromSlider.oninput = () => controlFromSlider(fromSlider, toSlider, fromInput);
            toSlider.oninput = () => controlToSlider(fromSlider, toSlider, toInput);
            fromInput.oninput = () => controlFromInput(fromSlider, fromInput, toInput, toSlider);
            toInput.oninput = () => controlToInput(toSlider, fromInput, toInput, toSlider);

            fromSlider.onchange = () => filterRequest();
            toSlider.onchange = () => filterRequest();
            fromInput.onchange = () => filterRequest();
            toInput.onchange = () => filterRequest();
        }

        $('.filter-by').click(function () {
            var $closestChevron = $(this).closest('.filter-by').find('.fa-chevron-down');
            $closestChevron.toggleClass("down");
        });

        $('.filter-collapse').click(function () {
            var $closestChevron = $(this).closest('.filter-collapse').find('.fa-chevron-down');
            $closestChevron.toggleClass("down");
        });

        $subcategoryItems.click(function () {
            $(this).toggleClass('subcategory-selected');
            filterRequest();
        });

        $('input[type="radio"][name="exampleRadios"]').change(function () {
            $mainCategoryName = $(this).val();
            filterRequest();
        });

        $sizeColumns.click(function () {
            $(this).toggleClass('selected-size');
            filterRequest();
        });

        $colorColumns.click(function () {
            $(this).toggleClass('selected-color');
            filterRequest();
        });

        $sortByBrands.click(function () {
            filterRequest();
        });

        $sortByLTCs.click(function () {
            filterRequest();
        });

        function filterRequest() {
            commonFuncs.showLoader();
            var brandsValues = $('.brand-checkbox:checked').map(function () {
                return $(this).next('label').text().trim();
            }).get().join('_');

            var ltcsValues = $('.ltc-checkbox:checked').map(function () {
                return $(this).next('label').text().trim();
            }).get().join('_');

            var productColours = $('.selected-color').map(function () {
                return $(this).attr('id');
            })
                .get()
                .join('_');
            var sizes = $('.selected-size').map(function () {
                return $(this).text();
            })
                .get()
                .join('_');

            var subcategories = $('.subcategory-selected').map(function () {
                return $(this).attr('id');
            })
                .get()
                .join('_');

            const filterJS = {
                SelectedSubcategories: subcategories,
                ProductColours: productColours,
                MinPrice: $minPrice.val(),
                MaxPrice: $maxPrice.val(),
                Sizes: sizes,
                Brands: brandsValues,
                LTCs: ltcsValues,
                MainCategoryName: $mainCategoryName
            };
            var queryString = Object.keys(filterJS)
                .filter(function (key) {
                    return (
                        filterJS[key] !== null &&
                        filterJS[key] !== "" &&
                        filterJS[key] !== 0 &&
                        (Array.isArray(filterJS[key]) ? filterJS[key].length > 0 : true)
                    );
                })
                .map(function (key) {
                    return "filter." + key + "=" + encodeURIComponent(filterJS[key]);
                })
                .join("&");
            

            var currentDomain = window.location.origin;

            var newURL = currentDomain + "/Home/_UserProductsPartial?" + queryString + "&sortBy=" + $sortByDropdown.val();
            setTimeout(function () {
                window.location.href = newURL;
            }, 0);
            commonFuncs.hideLoader();
        }

        $('#clear-all-filters').click(function () {
            var currentDomain = window.location.origin;

            var newURL = currentDomain + "/Home/_UserProductsPartial?mainCategoryId=" + $mainCategoryName + "";
            setTimeout(function () {
                window.location.href = newURL;
            }, 0);
        });

        $sortByDropdown.change(function () {
            filterRequest();
        });

        if ($('#search-input').val() !== '') {
            $('#search-input').addClass('filled'); 
        }

        $('#search-input').on('input', function () {
            if ($(this).val() === '') {
                $(this).removeClass('filled'); 
            } else {
                $(this).addClass('filled'); 
            }
        });
    }
    return {
        init
    };
})();