var cartPage = (function () {
    function init($container) {
        let $removeProductFromCart = $container.find('.remove-from-cart-button'),
            $addProductToFavourites = $container.find('.add-to-favourites-button');


        $("#checkoutButton").on("click", function (event) {
            commonFuncs.showLoader();
            var totalAmount = parseFloat($("#totalAmount").text().replace("$", ""));

            if (totalAmount === 0) {
                event.preventDefault();
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'You cannot pass a cart with 0 items in it!',
                })
            }
            commonFuncs.hideLoader();
        });

        $removeProductFromCart.click(function () {
            Swal.fire({
                title: 'Are you sure?',
                text: "Do you want to delete the product from the cart?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Delete!'
            }).then((result) => {
                if (result.isConfirmed) {
                    commonFuncs.showLoader();
                    var originalText = $(this).data('original-text');
                    originalText = originalText.replace(/\s+/g, ' ').trim();
                    $.post('/Home/RemoveFromCart', { productId: $(this).attr('id'), size: originalText }, function (response) {
                        commonFuncs.hideLoader();
                        location.reload();
                    }).fail(function (error) {
                        commonFuncs.hideLoader();
                        alert('AJAX request failed: ', error);
                    });
                }
            })
        });

        $addProductToFavourites.click(function () {
            commonFuncs.showLoader();
            var originalText = $(this).data('original-text');
            originalText = originalText.replace(/\s+/g, ' ').trim();

            $.post('/Home/RemoveFromCart', { productId: $(this).attr('id'), addToFavourites: true, size: originalText }, function (response) {
                commonFuncs.hideLoader();
                location.reload();
            }).fail(function (error) {
                alert('AJAX request failed: ', error);
            });
        });
    }
    return {
        init
    };
})();