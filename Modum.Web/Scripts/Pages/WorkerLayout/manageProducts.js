var manageProductsPage = (function () {
    function init($container) {

        let $editButton = $container.find('');
        $container.on('click', '.redirect-to-edit-page', function () {
            const editPageUrl = '/Worker/EditProduct?productId=' + $(this).attr('id');

            window.location.href = editPageUrl;
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

    }
    return {
        init
    };
})();