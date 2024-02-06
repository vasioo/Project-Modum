var registerUser = (function () {
    function init($container) {
        let $userCountryInput = $container.find('#Input_Country'),
            $facebookLink = $container.find('#facebookLink'),
            $googleLink = $container.find('#googleLink'),
            $facebookInput = $container.find('#facebookInput'),
            $googleInput = $container.find('#googleInput');

        $(document).ready(function () {
            if ($userCountryInput.length != 0) {
                $.ajax({
                    url: 'https://restcountries.com/v3.1/all',
                    method: 'GET',
                    success: function (response) {
                        let countries = response.map(function (country) {
                            return country.name.common;
                        });
                        countries.sort();


                        $userCountryInput.autocomplete({
                            source: countries,
                            minLength: 0,
                            select: function (event, ui) {
                                let selectedCountry = ui.item.value;
                                $userCountryInput.text(selectedCountry);
                            }
                        });
                        $userCountryInput.on('click', function () {
                            $userCountryInput.autocomplete('search', '');
                        });


                        $userCountryInput.on('blur', function () {
                            var enteredCountry = $userCountryInput.val().trim();
                            if (countries.indexOf(enteredCountry) === -1) {
                                $userCountryInput.val('');
                                $userCountryInput.attr('placeholder', '');
                            }
                        });

                    },
                    error: function () {
                        alert('Failed to fetch country list.');
                    }
                });
            }
        });

        $facebookLink.click(function () {
            $facebookInput.click();
        });

        $googleLink.click(function () {
            $googleInput.click();
        });


        $('#toggleConfirmPassword').click(function (e) {
            e.preventDefault();
            var passwordInput = $('#custom-confirm-password-eye-orienter');
            var type = passwordInput.attr('type') === 'password' ? 'text' : 'password';
            passwordInput.attr('type', type);
            $(this).toggleClass('fa-eye fa-eye-slash');
        });
    }
    return {
        init
    };
})();