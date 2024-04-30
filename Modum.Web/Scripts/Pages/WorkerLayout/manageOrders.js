var manageOrders = (function () {
    function init($container) {
        $('.view-details').click(function () {
            var modalId = $(this).attr('data-target');
            $(modalId).modal('show');
        });
        $(document).on('click', '.order-btn', function () {
            var orderId = $(this).attr('data-orderId');
            var newStatus = $(this).attr('data-orderStatus');

            $.ajax({
                url: '/Worker/ChangeDeliveryStatus',
                method: 'POST',
                data: { orderId: orderId, newStatus: newStatus },
                success: function (response) {
                    if (response.status) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Success',
                            text: response.Message
                        });
                        location.reload();
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.Message
                        });
                    }
                },
                error: function (xhr, status, error) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'An error occurred: ' + error
                    });
                }
            });
        });
    }
    return {
        init
    };
})();