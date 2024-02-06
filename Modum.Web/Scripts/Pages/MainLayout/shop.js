var shopPage = (function () {
    function init($container) {
        let $addToCart = $container.find('.add-to-cart-shop'),
            $addToFavourites = $container.find('.add-to-favourites-shop'),
            $closePopUp = $container.find('#closePopup'),
            $confirmSizeSelection = $container.find('.confirm-size-selection'),
            $sizeSelectionPopUp = $container.find('#sizeSelectionPopup');
        var productId = "";

        $addToCart.click(function () {
            $sizeSelectionPopUp.css('display', 'block');
            productId = $(this).attr('id');
        });
        $confirmSizeSelection.click(function () {

            $('.checked-sizes').each(function (index) {
                var originalText = $(this).closest('li').find('label').data('original-text');
                originalText = originalText.replace(/\s+/g, ' ').trim();

                $.post('/Home/AddToCart', { productId: productId, size: originalText }, function (response) {
                    Swal.fire({
                        icon: 'success',
                        title: 'The changes to the cart have been made!',
                        showClass: {
                            popup: 'animate__animated animate__fadeInDown'
                        },
                        hideClass: {
                            popup: 'animate__animated animate__fadeOutUp'
                        }
                    })
                }).fail(function (error) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Something went wrong!',
                    })
                    alert('AJAX request failed: ' + error);
                });
            });
            $sizeSelectionPopUp.css('display', 'none');
        });
        $closePopUp.click(function () {
            $sizeSelectionPopUp.css('display', 'none');
        });

        $container.on('click', 'li', function () {
            $(this).toggleClass('checked-sizes');
        });

        $addToFavourites.click(function () {
            commonFuncs.showLoader();
            var product = $(this).attr('id');
            var $heartIcon = $(this).find('.heart-icon');

            if ($heartIcon.hasClass('fa-solid')) {

                $.post('/Home/RemoveFromFavourites', {
                    productId: product,
                    addToCart: false,
                    size: ""
                }, function (response) {
                    commonFuncs.hideLoader();
                    Swal.fire({
                        icon: 'success',
                        title: 'The changes to the favourites have been made!',
                        showClass: {
                            popup: 'animate__animated animate__fadeInDown'
                        },
                        hideClass: {
                            popup: 'animate__animated animate__fadeOutUp'
                        }
                    })
                    location.reload();
                }).fail(function (error) {
                    commonFuncs.hideLoader();
                    console.log('AJAX request failed:', error);
                });
            } else {

                $.post('/Home/AddToFavourites', {
                    productId: product
                }, function (response) {
                    commonFuncs.hideLoader();
                    location.reload();
                }).fail(function (error) {
                    commonFuncs.hideLoader();
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'Something went wrong!',
                    })
                    console.log('AJAX request failed:', error);
                });
            }
        });

        $container.on('click', '.delete', function () {
            Swal.fire({
                title: 'Are you sure?',
                text: "You won't be able to revert this product!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Delete!'
            }).then((result) => {
                if (result.isConfirmed) {
                    commonFuncs.showLoader();
                    $(this).closest('tr').remove();
                    $.post('/Worker/DeleteProduct', { productId: $(this).attr('id') }, function (response) {
                        commonFuncs.hideLoader();
                        location.reload();
                    }).fail(function (error) {
                        commonFuncs.hideLoader();
                        alert('AJAX request failed: ', error);
                    });
                }
            })
        });

        $container.on('click', '.redirect-to-edit-page', function () {
            const editPageUrl = '/Worker/EditProduct?productId=' + $(this).attr('id');

            window.location.href = editPageUrl;
        });
    }
    return {
        init
    };
})();