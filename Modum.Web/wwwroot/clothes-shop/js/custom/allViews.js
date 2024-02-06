$("#search-link").mousedown(function (event) {
    var searchInput = document.getElementById("search-input").value;
    if (searchInput != null) {
        var searchUrl = "/Home/_UserProductsPartial?searchProducts=" + encodeURIComponent(searchInput);
        window.location.href = searchUrl;
    }
});
var closePopUp = document.getElementById('closePopupHamburgerMenu');

closePopUp.addEventListener('click', function () {
    document.querySelector('.main-selector').style.display = 'block';
    document.querySelector('#hamburger-menu-popup').style.display = 'none';
});
window.addEventListener("load", (event) => {
    document.querySelector('#hamburger-menu-popup').style.display = 'none';
});

$(document).ready(function () {
    $('.subcategories-list').click(function () {
        $('.subcategories-list').not(this).find('.fa-chevron-down').addClass("down");
        var $closestChevron = $(this).closest('.subcategories-list').find('.fa-chevron-down');
        $closestChevron.toggleClass("down");
    });
    $('#hamburger-menu').click(function () {
        document.querySelector('.main-selector').style.display = 'none';
        document.querySelector('#hamburger-menu-popup').style.display = 'block';
    });

});

$(document).ready(function () {
    var itemsPerPage = 4;
    var currentPage = 0;

    function showButtons(activator) {
        activator.closest('.product-cards-container').find('.prevBtn, .nextBtn').css({
            'display': 'block',
            'cursor': 'pointer',
            'border': 'none'
        });
    }

    function hideButtons(activator) {
        activator.closest('.product-cards-container').find('.prevBtn, .nextBtn').css('display', 'none');
    }

    function showHideButtons(activator) {
        var container = activator.closest('.product-cards-container');
        container.find('.prevBtn').css('display', currentPage === 0 ? 'none' : 'block');
        container.find('.nextBtn').css('display', currentPage === container.find('.cards li').length - itemsPerPage ? 'none' : 'block');
    }

    $('.prod-container-scroller-activator').mouseenter(function () {
        var activator = $(this);
        showButtons(activator);
        showHideButtons(activator);
    });

    $('.prod-container-scroller-activator').mouseleave(function () {
        var activator = $(this);
        hideButtons(activator);
    });

    $('.nextBtn').click(function () {
        var container = $(this).closest('.product-cards-container');
        if (currentPage < container.find('.cards li').length - itemsPerPage) {
            currentPage++;
            var nextPosition = currentPage * container.find('.cards li').outerWidth(true);
            container.find('#card-slider').animate({
                scrollLeft: nextPosition,
                opacity: 1
            }, {
                duration: 200,
                easing: 'easeOutExpo'
            });
        }
        showHideButtons(container.find('.prod-container-scroller-activator'));
    });

    $('.prevBtn').click(function () {
        var container = $(this).closest('.product-cards-container');
        if (currentPage > 0) {
            currentPage--;
            var prevPosition = currentPage * container.find('.cards li').outerWidth(true);
            container.find('#card-slider').animate({
                scrollLeft: prevPosition,
                opacity: 1
            }, {
                duration: 200,
                easing: 'easeOutExpo'
            });
        }
        showHideButtons(container.find('.prod-container-scroller-activator'));
    });
});
