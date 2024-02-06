var loginPage = (function () {
    function init($container) {
        $('#togglePassword').click(function (e) {
            e.preventDefault();
            var passwordInput = $('#custom-password-eye-orienter');
            var type = passwordInput.attr('type') === 'password' ? 'text' : 'password';
            passwordInput.attr('type', type);
            $(this).toggleClass('fa-eye fa-eye-slash');
        });
    }

    return {
        init
    };
})();