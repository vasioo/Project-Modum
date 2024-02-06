var partnershipPage = (function () {
    function init($container) {

        let $email = $container.find('#email'),
            $name = $container.find('#username'),
            $receiveEmailButton = $container.find('#receive-email-from-sponsor'),
            $message = $container.find('#message');

        $receiveEmailButton.click(function () {
            if ($email.val() && $email.val().trim() !== "" && $message.val() && $message.val().trim() !== "" && $name.val() && $name.val().trim() !== "") {
                commonFuncs.showLoader();
                $.ajax({
                    url: '/Footer/PartnershipSendEmail',
                    type: 'POST',
                    dataType: 'json',
                    data: { email: $email.val(), bodyText: $message.val(), name: $name.val() },
                    success: function (result) {
                        if (result.status) {
                            commonFuncs.hideLoader();
                            Swal.fire({
                                title: "Wow!",
                                text: "The email was sent!",
                                icon: "success"
                            });
                            location.reload();
                        } else {
                            // Error occurred
                            commonFuncs.hideLoader();
                            console.error(result.Message);
                        }
                    },
                    error: function (xhr, status, error) {
                        commonFuncs.hideLoader();
                        // Handle errors here
                        console.error('Error:', error);
                    }
                });
            }
            else {
                commonFuncs.hideLoader();
                Swal.fire({
                    title: "Oops!",
                    text: "Please fill in all required fields.",
                    icon: "error"
                });
            }
        });
    }
    return {
        init
    };
})();
