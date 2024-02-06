var favouritesPage = (function () {
    function init($container) {
        let $removeProductFromFavourites = $container.find('.remove-from-favourites-button'),
            $addProductToCart = $container.find('.add-to-cart-button'),
            $closePopUp = $container.find('.closePopup'),
            $confirmSizeSelection = $container.find('.confirm-size-selection'),
            $sizeSelectionPopUp = $container.find('#sizeSelectionPopup');

        $removeProductFromFavourites.click(function () {
            Swal.fire({
                title: 'Are you sure?',
                text: "Do you want to delete the product from the favourites page?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Delete!'
            }).then((result) => {
                if (result.isConfirmed) {
                    commonFuncs.showLoader();
                    $.post('/Home/RemoveFromFavourites', { productId: $(this).attr('id') }, function (response) {
                        commonFuncs.hideLoader();
                        location.reload();
                    }).fail(function (error) {
                        alert('AJAX request failed: ' + error);
                    });
                }
            })
        });

        var productId = "";

        $addProductToCart.click(function () {
            productId = $(this).attr('id');
            let sizeselstr = 'sizeSelectionPopup_' + productId.trim();
            $sizeSelectionPopUp = $container.find('#' + sizeselstr);
            $sizeSelectionPopUp.css('display', 'block');

        });

        $confirmSizeSelection.click(function () {

            $('.checked-sizes').each(function (index) {
                var originalText = $(this).closest('li').find('label').data('original-text');
                originalText = originalText.replace(/\s+/g, ' ').trim();

                $.post('/Home/RemoveFromFavourites', { productId: productId, addToCart: true, size: originalText }, function (response) {
                }).fail(function (error) {
                    alert('AJAX request failed: ' + error);
                });
            });
            $('.unchecked-sizes').each(function (index) {
                var originalText = $(this).closest('li').find('label').data('original-text');
                originalText = originalText.replace(/\s+/g, ' ').trim();
                $.post('/Home/RemoveFromCart', { productId: productId, addToFavourites: false, size: originalText }, function (response) {
                }).fail(function (error) {
                    alert('AJAX request failed: ' + error);
                });
            });
            $sizeSelectionPopUp.css('display', 'none');
            location.reload();
        });

        $closePopUp.click(function () {
            $sizeSelectionPopUp.css('display', 'none');
        });

        $container.on('click', 'li', function () {
            $(this).toggleClass('checked-sizes');
        });
    }
    return {
        init
    };
})();