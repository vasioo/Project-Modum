var contactUs = (function () {
    function init($container) {
            let $email = $container.find('#formEmail'),
                $name = $container.find('#formName'),
                $receiveEmailButton = $container.find('#receive-email-from-user'),
                $message = $container.find('#formMessage');

        $receiveEmailButton.click(function (event) {
            commonFuncs.showLoader();
            event.preventDefault();
                if ($email.val() && $email.val().trim() !== "" && $message.val() && $message.val().trim() !== "" && $name.val() && $name.val().trim() !== "") {
                    $.ajax({
                        url: '/Footer/UserSendEmail',
                        type: 'POST',
                        dataType: 'json',
                        data: { email: $email.val(), bodyText: $message.val(),name:$name.val() },
                        success: function (result) {
                            if (result.status) {
                                commonFuncs.hideLoader();
                                Swal.fire({
                                    title: "Wow!",
                                    text: "The email was sent!",
                                    icon: "success"
                                });
                                setTimeout(function () {
                                    location.reload();
                                }, 3000);
                            } else {
                                console.error(result.Message);
                            }
                        },
                        error: function (xhr, status, error) {
                            commonFuncs.hideLoader();
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