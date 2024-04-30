var stripePaymentOrdersPage = (function () {
    function init($container) {
        $container.on('click', '.additional-btn', function () {
            const editPageUrl = '/Admin/AdditionalOrderInformation?orderId=' + $(this).attr('id');

            window.location.href = editPageUrl;
        });
    }
    return {
        init
    };
})();