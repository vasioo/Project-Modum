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

            $.post('/Home/RemoveFromCart', {
                productId: $(this).attr('id'),
                addToFavourites: true,
                size: originalText
            }, function (response) {
                commonFuncs.hideLoader();
                Swal.fire({
                    icon: 'success',
                    title: 'Completed',
                    text: 'The action has been completed successfully.',
                    showConfirmButton: false,
                    timer: 2000 
                }).then(() => {
                    location.reload();
                });
            }).fail(function (error) {
                commonFuncs.hideLoader();
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'An error occurred while processing your request.',
                    showConfirmButton: true 
                });
                console.error('AJAX request failed: ', error);
            });

        });
    }
    return {
        init
    };
})();
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
var deliveryLocationPage = (function () {
    function init($container) {
        $($container).ready(function () {
            function initializeAutocompleteWithPrefix(inputId, prefix) {
                var input = $('#' + inputId);
                var autocomplete = new google.maps.places.Autocomplete(input[0], {
                    types: ['geocode', 'establishment'],
                    componentRestrictions: { country: 'bg' },
                    maxHeight: '100px'
                });
                var prefixLength = prefix.length;

                input.val(prefix);

                function prependPrefix() {
                    var value = input.val();

                    if (!value.toLowerCase().startsWith(prefix.toLowerCase())) {
                        input.val(prefix);
                    }
                }

                input.on('input', function () {
                    var value = input.val();

                    if (value.length < prefixLength) {
                        Swal.fire({
                            icon: 'warning',
                            title: 'Oops...',
                            text: 'The prefix cannot be deleted.',
                            input: false,
                            timer: 1500,
                            timerProgressBar: true,
                            showConfirmButton: false
                        });

                        prependPrefix();
                    }
                });

                autocomplete.addListener('place_changed', function () {
                    var place = autocomplete.getPlace();
                    if (place) {
                        prependPrefix();
                    }
                });
            }

            function initializeAutocomplete(inputId) {
                var input = $('#' + inputId);
                var autocomplete = new google.maps.places.Autocomplete(input[0], {
                    types: ['geocode', 'establishment'],
                    componentRestrictions: { country: 'bg' },
                    maxHeight: '100px'
                });

                autocomplete.addListener('place_changed', function () {
                    var place = autocomplete.getPlace().toLowerCase();
                    if (place) {
                        var inputVal = input.val().toLowerCase();
                        if (inputVal.startsWith('еконт') || inputVal.startsWith('спиди')) {
                            input.val('');
                        }
                    }
                });
            }

            initializeAutocomplete('custom-location-input');

            initializeAutocompleteWithPrefix('econt-location-input', 'Еконт');

            initializeAutocompleteWithPrefix('speedy-location-input', 'Спиди');

            function initMap() {
                var map;
                var marker;

                map = new google.maps.Map(document.getElementById('map'), {
                    center: { lat: 42.6977, lng: 23.3219 },
                    zoom: 7,
                    disableDefaultUI: true,
                    draggable: true,
                    clickableIcons: false,
                    streetViewControl: false,
                    zoomControl: true,
                    maxZoom: 0,
                    minZoom: 6
                });

                var input = document.getElementById('location-input');
                var customLocationInput = document.getElementById('custom-location-input');
                var autocomplete = new google.maps.places.Autocomplete(input);
                autocomplete.bindTo('bounds', map);

                autocomplete.addListener('place_changed', function () {
                    var place = autocomplete.getPlace();
                    if (!place.geometry) {
                        console.log("Autocomplete's returned place contains no geometry");
                        return;
                    }

                    if (place.geometry.viewport) {
                        map.fitBounds(place.geometry.viewport);
                    } else {
                        map.setCenter(place.geometry.location);
                        map.setZoom(17);
                    }

                    marker.setPosition(place.geometry.location);
                });

                map.addListener('click', function (event) {
                    if (isLocationInBulgaria(event.latLng)) {
                        $('#custom-location-input').show();

                        var geocoder = new google.maps.Geocoder();
                        geocoder.geocode({
                            location: event.latLng,
                            language: 'bg',
                        }, function (results, status) {
                            if (status === 'OK') {
                                if (results[0]) {
                                    var countryComponent = results[0].address_components.find(function (component) {
                                        return component.types.includes('country');
                                    });
                                    if (countryComponent && countryComponent.short_name === 'BG') {
                                        var formattedAddress = formatAddressComponents(results[0].address_components);
                                        customLocationInput.value = formattedAddress;
                                    } else {
                                        Swal.fire({
                                            icon: 'error',
                                            title: 'Oops...',
                                            text: 'Please select a location within Bulgaria.'
                                        });
                                    }
                                } else {
                                    console.error('No results found');
                                }
                            } else {
                                console.error('Geocoder failed due to: ' + status);
                            }
                        });

                        $('#custom-accordion').show();

                        if (!marker) {
                            marker = new google.maps.Marker({
                                position: event.latLng,
                                map: map,
                                draggable: true
                            });

                            marker.addListener('dragend', function () {
                                marker.setPosition(marker.getPosition());
                                geocoder.geocode({
                                    location: marker.getPosition(),
                                    language: 'bg'
                                }, function (results, status) {
                                    if (status === 'OK') {
                                        if (results[0]) {
                                            var countryComponent = results[0].address_components.find(function (component) {
                                                return component.types.includes('country');
                                            });
                                            if (countryComponent && countryComponent.short_name === 'BG') {
                                                var formattedAddress = formatAddressComponents(results[0].address_components);
                                                customLocationInput.value = formattedAddress;
                                            } else {
                                                Swal.fire({
                                                    icon: 'error',
                                                    title: 'Oops...',
                                                    text: 'Please select a location within Bulgaria.'
                                                });
                                            }
                                        } else {
                                            console.error('No results found');
                                        }
                                    } else {
                                        console.error('Geocoder failed due to: ' + status);
                                    }
                                });
                            });
                        } else {
                            marker.setPosition(event.latLng);
                            geocoder.geocode({
                                location: event.latLng,
                                language: 'bg'
                            }, function (results, status) {
                                if (status === 'OK') {
                                    if (results[0]) {
                                        var countryComponent = results[0].address_components.find(function (component) {
                                            return component.types.includes('country');
                                        });
                                        if (countryComponent && countryComponent.short_name === 'BG') {
                                            var formattedAddress = formatAddressComponents(results[0].address_components);
                                            customLocationInput.value = formattedAddress;
                                        } else {
                                            Swal.fire({
                                                icon: 'error',
                                                title: 'Oops...',
                                                text: 'Please select a location within Bulgaria.'
                                            });
                                        }
                                    } else {
                                        console.error('No results found');
                                    }
                                } else {
                                    console.error('Geocoder failed due to: ' + status);
                                }
                            });
                        }

                        $('.form-input').not('#custom-accordion').hide();
                        $('.selector-list-delivery').removeClass('active');
                        $('#custom').addClass('active');
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'You can only place pins within Bulgaria\'s boundaries.'
                        });
                    }

                });

                $.getJSON('https://ipapi.co/json/')
                    .done(function (data) {
                        var userLocation = { lat: parseFloat(data.latitude), lng: parseFloat(data.longitude) };
                        map.setCenter(userLocation);
                        map.setZoom(18);

                        marker = new google.maps.Marker({
                            position: userLocation,
                            map: map,
                            title: 'Your Location'
                        });
                    })
                    .fail(function (error) {
                        console.error('Error:', error);
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'Error fetching user location. Please try again later.'
                        });
                    });
            }

            function isLocationInBulgaria(location) {
                var lat = location.lat();
                var lng = location.lng();
                return lat >= 41.235 && lat <= 44.214 && lng >= 22.357 && lng <= 28.612;
            }

            initMap();
        });

        function formatAddressComponents(addressComponents) {
            var formattedAddress = '';
            var hasStreetOnly = false;
            var streetNumber = '';
            addressComponents.forEach(function (component) {
                if (component.types.includes('street_number')) {
                    streetNumber += component.long_name + ', ';
                }
                if (component.types.includes('route')) {
                    if (/^[A-Z0-9+]+$/.test(component.short_name)) {
                        hasStreetOnly = true;
                    } else {
                        formattedAddress += component.short_name + ' ' + streetNumber;

                    }
                }
                if (component.types.includes('locality')) {
                    formattedAddress += component.long_name;
                }
            });

            if (formattedAddress.endsWith(', ')) {
                formattedAddress = formattedAddress.slice(0, -2);
            }

            if (hasStreetOnly) {
                formattedAddress = formattedAddress.split(',').slice(1).join(',').trim();
            }

            return formattedAddress;
        }

        $('.selector-list-delivery').click(function () {
            $('.form-input').hide();

            $('.selector-list-delivery').removeClass('active');
            $(this).addClass('active');

            $('.active-delivery-option').removeClass('active-delivery-option');

            $('.custom-input-field-main-section-div input[type="radio"]').prop('checked', false);

            var id = $(this).attr('id') + '-accordion';
            $('#' + id).css('display', 'flex');
        });

        $('.custom-input-field-main-section-div').click(function () {
            $('.custom-input-field-main-section-div').not(this).removeClass('active-delivery-option');

            $(this).addClass('active-delivery-option');

            $(this).find('input[type="radio"]').prop('checked', true);
        });

      

        function getLocationData() {
            var activeSubsectionId = $('.selector-list-delivery.active').attr('id');
            var locationInput = $('#' + activeSubsectionId + '-location-input').val().trim();
            return { activeSubsection: activeSubsectionId, locationInput: locationInput };
        }

        function validateLocation(location) {
            var regex = /^[^,]*\p{L}{2,}[^,]*,\s*[^,]*\p{L}{2,}[^,]*$/u;
            return regex.test(location);
        }

        function validateAndCheckLocation(location) {
            return new Promise(function (resolve, reject) {
                var geocoder = new google.maps.Geocoder();
                geocoder.geocode({ address: location }, function (results, status) {
                    if (status === 'OK' && results && results.length > 0) {
                        var country = results[0].address_components.find(function (component) {
                            return component.types.includes('country');
                        });
                        if (!country || !country.short_name.toLowerCase() === 'bg') {
                            reject('The provided location is not within Bulgaria.');
                        } else {
                            resolve(true);
                        }
                    } else {
                        reject('Error occurred while checking the location.');
                    }
                });
            });
        }

        $('#process-request-btn').click(function () {
            var locationData = getLocationData();
            if (validateLocation(locationData.locationInput)) {
                validateAndCheckLocation(locationData.locationInput)
                    .then(function (result) {
                        var deliveryOption = $(`.active-delivery-option input`).attr('id');

                        if (deliveryOption) {
                            var deliveryPlan = deliveryOption;

                            var formData = {
                                location: locationData.locationInput,
                                deliveryPlan: deliveryPlan
                            };

                            var form = $('<form>').attr({
                                method: 'POST',
                                action: '/Home/Checkout'
                            });

                            // Add hidden input fields for each key-value pair in formData
                            $.each(formData, function (key, value) {
                                $('<input>').attr({
                                    type: 'hidden',
                                    name: key,
                                    value: value
                                }).appendTo(form);
                            });

                            form.appendTo('body').submit();
                        }
                        else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: 'You need to select at least one option for delivering.'
                            });
                        }

                    })
                    .catch(function (error) {
                        console.error(error);
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: error
                        });
                    });

            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Invalid Location',
                    text: 'Location should be in format (street,city).'
                });
            }
        });

        $('input[name="delivery-option"]').change(function () {
            $('.custom-input-field-main-section-div').removeClass('active-delivery-option');
            if ($(this).is(':checked')) {
                $(this).closest('.custom-input-field-main-section-div').addClass('active-delivery-option');
            }
        });
    }
    return {
        init
    };
})();
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
var genderTemplate = (function () {
    function init($container) {

        let $prevBtn = $container.find('#arrow-left'),
            $nextBtn = $container.find('#arrow-right'),
            $seasonImage = $container.find('#image-for-season'),
            $shopNowBtn = $container.find('#button-for-custom-season-overflow'),
            $ltcBtn = $container.find('#button-for-limited-time-campaign');

        $shopNowBtn.click(function () {
            var currentDomain = window.location.origin;

            var mainCategory = $('.genderCall-page').attr('id');

            var newURL = currentDomain + "/Home/_UserProductsPartial?" + "mainCategoryId" + mainCategory;
            setTimeout(function () {
                window.location.href = newURL;
            }, 0);
        });

        $ltcBtn.click(function () {
            var currentDomain = window.location.origin;

            var mainCategory = $('.genderCall-page').attr('id');

            var newURL = currentDomain + "/Home/_UserProductsPartial?" + "mainCategoryId" + mainCategory;
            setTimeout(function () {
                window.location.href = newURL;
            }, 0);
        });

        const date = new Date();
        let month = date.getMonth() + 1;

        if (month >= 3 && month <= 5) {
            $seasonImage.attr("src", "/clothes-shop/images/season-specialized/spring-main-2.jpg")
        }
        else if (month >= 6 && month <= 8) {
            $seasonImage.attr("src", "/clothes-shop/images/season-specialized/summer-main-2.jpg")
        }
        else if (month >= 9 && month <= 11) {
            $seasonImage.attr("src", "/clothes-shop/images/season-specialized/autumn-main-2.jpg")
        }
        else {
            $seasonImage.attr("src", "/clothes-shop/images/season-specialized/winter-main-2.jpg")
        }

        const endDateElement = document.getElementById("countdown-container");
        if (!endDateElement) {
            return;
        }

        const endDateAttribute = endDateElement.getAttribute("data-enddate");
        if (!endDateAttribute) {
            return;
        }

        const endDate = new Date(endDateAttribute).getTime();

        if (isNaN(endDate)) {
            return;
        }

        const countdownInterval = setInterval(updateCountdown, 1000);

        function updateCountdown() {
            const now = new Date().getTime();
            const distance = endDate - now;

            if (distance < 0) {
                clearInterval(countdownInterval);
                document.getElementById("countdown").innerHTML = "EXPIRED";
            } else {
                const days = Math.floor(distance / (1000 * 60 * 60 * 24));
                const hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                const seconds = Math.floor((distance % (1000 * 60)) / 1000);

                var htmlCountdown = `
        
               <div class="col pl-2 pr-2">
                   <div class="bg-dark pt-2 rounded-top data-shower">
                       ${days}
                   </div>
                   <div class="bg-dark pl-1 pr-1 pt-4 pb-2 rounded-bottom label-data-shower">Days</div>
               </div>
               <div class="col pl-2 pr-2">
                   <div class="bg-dark pt-2 rounded-top data-shower">
                       ${hours}
                   </div>
                   <div class="bg-dark pl-1 pr-1 pt-4 pb-2 rounded-bottom label-data-shower">Hours</div>
               </div>
               <div class="col pl-2 pr-2">
                   <div class="bg-dark pt-2 rounded-top data-shower">
                       ${minutes}
                   </div>
                   <div class="bg-dark pl-1 pr-1 pt-4 pb-2 rounded-bottom label-data-shower">Minutes</div>
               </div>
               <div class="col pl-2 pr-2">
                   <div class="bg-dark pt-2 rounded-top data-shower">
                      ${seconds}
                   </div>
                   <div class="bg-dark pl-1 pr-1 pt-4 pb-2 rounded-bottom label-data-shower">Seconds</div>
               </div>
            
            `;

                document.getElementById("countdown").innerHTML = htmlCountdown;
            }
        }

    }

    return {
        init
    };
})();
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
var outfitPicker = (function () {
    function init($container) {
        $(document).ready(function () {
            $('.image-upload').change(function (event) {
                const $self = $(this),
                    $uploadedImage = $self.parent().find('.uploaded-image'),
                    file = event.target.files[0];

                commonFuncs.validateAndResizeImage(file, function (isValid, imageData) {
                    if (isValid) {

                        $uploadedImage.attr('src', imageData);

                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'Image validation failed:' + imageData
                        })
                    }
                });
            });
            $('.retry-btn').click(function () {
                location.reload();
            });

            $('.create-image').click(function (e) {
                commonFuncs.showLoader();
                e.preventDefault();

                const images = [];

                $('.uploaded-image').each(function () {
                    const image = {
                        Image: $(this).attr('src'),
                    }
                    images.push(image);
                });

                const validator = new FormValidator(),
                    isValid = validator.validateImagesField(images);

                if (isValid) {
                    const personImageJs = $('#person-image')[0].files[0];
                    const clothImageJs = $('#clothing-image')[0].files[0];

                    const formData = new FormData();
                    formData.append('personImage', personImageJs);
                    formData.append('clothImage', clothImageJs);

                    $.ajax({
                        url: '/Footer/OutfitPickerOnPostQuery',
                        type: 'POST',
                        data: formData,
                        processData: false,
                        contentType: false,
                        success: function (resp) {
                            var actualResponse = JSON.parse(resp.message);
                            var imagePath = actualResponse.response.ouput_path_img;

                            $('#additionalPicturePreview').attr('src', imagePath);
                            $('#resultText').text(actualResponse.message); 

                            $('#hiddenInput').css('display', 'block');
                            $('.retry-btn').css('display', 'block');
                            $('.create-image').css('display', 'none');

                            commonFuncs.hideLoader();
                        },
                        error: function (error) {
                            commonFuncs.hideLoader();
                            alert('AJAX request failed: ' + error);
                        }

                    });

                    commonFuncs.hideLoader();
                }
    
                else {
                    let errors = validator.getErrors(),
                        errorList = document.createElement("ul");

                    errors.forEach((error) => {
                        const listItem = document.createElement("li");
                        listItem.textContent = error;
                        listItem.style.color = "red";
                        listItem.style.textAlign = "left";
                        errorList.appendChild(listItem);
                    });


                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        html: `${errorList.innerHTML}`,
                        showClass: {
                            popup: 'animate__animated animate__fadeInDown'
                        },
                        hideClass: {
                            popup: 'animate__animated animate__fadeOutUp'
                        }
                    });
                }

                commonFuncs.hideLoader();
            });

            class FormValidator {
                constructor() {
                    this.errors = [];
                }


                validateImagesField(images) {
                    this.errors = [];

                    this.validateImages(images);

                    return this.errors.length === 0;
                }

                validateImages(images) {
                    images.forEach((image) => {
                        if (!image.Image || image.ImageName == "" || image.ImageName == " ") {
                            this.errors.push('Image is required');
                            return;
                        }
                    });
                }

                getErrors() {
                    return this.errors;
                }
            }
        });
    }
    return {
        init
    };
})();
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
                        },
                        timer: 1500, 
                        timerProgressBar: true, 
                        didOpen: () => {
                            Swal.showLoading(); 
                        },
                        willClose: () => {
                            location.reload();
                        }
                    });
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
var userItemsSelectionList = (function () {
    function init($container) {
        let $mainCategoryName = $container.find('input[type="radio"][name="exampleRadios"]:checked').val(),
            $minPrice = $container.find('#fromInput'),
            $maxPrice = $container.find('#toInput'),
            $subcategoryItems = $container.find('.subcategory-item'),
            $sizeColumns = $container.find('.size-col'),
            $colorColumns = $container.find('.color-col'),
            $sortByDropdown = $container.find('#sortBy'),
            $sortByBrands = $container.find('.brand-checkbox'),
            $sortByLTCs = $container.find('.ltc-checkbox');

        function controlFromSlider(fromSlider, toSlider, fromInput) {
            const [from, to] = getParsed(fromSlider, toSlider);
            fillSlider(fromSlider, toSlider, '#C6C6C6', '#CC5500', toSlider);
            if (from > to) {
                fromSlider.value = to;
                fromInput.value = to;
            } else {
                fromInput.value = from;
            }
        }

        function controlToSlider(fromSlider, toSlider, toInput) {
            const [from, to] = getParsed(fromSlider, toSlider);
            fillSlider(fromSlider, toSlider, '#C6C6C6', '#CC5500', toSlider);
            setToggleAccessible(toSlider);
            if (from <= to) {
                toSlider.value = to;
                toInput.value = to;
            } else {
                toInput.value = from;
                toSlider.value = from;
            }
        }

        function getParsed(currentFrom, currentTo) {
            const from = parseInt(currentFrom.value, 10);
            const to = parseInt(currentTo.value, 10);
            return [from, to];
        }

        function fillSlider(from, to, sliderColor, rangeColor, controlSlider) {
            const rangeDistance = to.max - to.min;
            const fromPosition = from.value - to.min;
            const toPosition = to.value - to.min;
            controlSlider.style.background = `linear-gradient(
          to right,
          ${sliderColor} 0%,
          ${sliderColor} ${(fromPosition) / (rangeDistance) * 100}%,
          ${rangeColor} ${((fromPosition) / (rangeDistance)) * 100}%,
          ${rangeColor} ${(toPosition) / (rangeDistance) * 100}%, 
          ${sliderColor} ${(toPosition) / (rangeDistance) * 100}%, 
          ${sliderColor} 100%)`;
        }

        function setToggleAccessible(currentTarget) {
            const toSlider = document.querySelector('#toSlider');
            if (Number(currentTarget.value) <= 0) {
                toSlider.style.zIndex = 2;
            } else {
                toSlider.style.zIndex = 0;
            }
        }

        if ($mainCategoryName != null) {
            const fromSlider = document.querySelector('#fromSlider');
            const toSlider = document.querySelector('#toSlider');
            const fromInput = document.querySelector('#fromInput');
            const toInput = document.querySelector('#toInput');

            fillSlider(fromSlider, toSlider, '#C6C6C6', '#CC5500', toSlider);
            setToggleAccessible(toSlider);
            fromSlider.oninput = () => controlFromSlider(fromSlider, toSlider, fromInput);
            toSlider.oninput = () => controlToSlider(fromSlider, toSlider, toInput);
            fromInput.oninput = () => controlFromInput(fromSlider, fromInput, toInput, toSlider);
            toInput.oninput = () => controlToInput(toSlider, fromInput, toInput, toSlider);

            fromSlider.onchange = () => filterRequest();
            toSlider.onchange = () => filterRequest();
            fromInput.onchange = () => filterRequest();
            toInput.onchange = () => filterRequest();
        }

        $('.filter-by').click(function () {
            var $closestChevron = $(this).closest('.filter-by').find('.fa-chevron-down');
            $closestChevron.toggleClass("down");
        });

        $('.filter-collapse').click(function () {
            var $closestChevron = $(this).closest('.filter-collapse').find('.fa-chevron-down');
            $closestChevron.toggleClass("down");
        });

        $subcategoryItems.click(function () {
            $(this).toggleClass('subcategory-selected');
            filterRequest();
        });

        $('input[type="radio"][name="exampleRadios"]').change(function () {
            $mainCategoryName = $(this).val();
            filterRequest();
        });

        $sizeColumns.click(function () {
            $(this).toggleClass('selected-size');
            filterRequest();
        });

        $colorColumns.click(function () {
            $(this).toggleClass('selected-color');
            filterRequest();
        });

        $sortByBrands.click(function () {
            filterRequest();
        });

        $sortByLTCs.click(function () {
            filterRequest();
        });

        function filterRequest() {
            commonFuncs.showLoader();
            var brandsValues = $('.brand-checkbox:checked').map(function () {
                return $(this).next('label').text().trim();
            }).get().join('_');

            var ltcsValues = $('.ltc-checkbox:checked').map(function () {
                return $(this).next('label').text().trim();
            }).get().join('_');

            var productColours = $('.selected-color').map(function () {
                return $(this).attr('id');
            })
                .get()
                .join('_');
            var sizes = $('.selected-size').map(function () {
                return $(this).text();
            })
                .get()
                .join('_');

            var subcategories = $('.subcategory-selected').map(function () {
                return $(this).attr('id');
            })
                .get()
                .join('_');

            const filterJS = {
                SelectedSubcategories: subcategories,
                ProductColours: productColours,
                MinPrice: $minPrice.val(),
                MaxPrice: $maxPrice.val(),
                Sizes: sizes,
                Brands: brandsValues,
                LTCs: ltcsValues,
                MainCategoryName: $mainCategoryName
            };
            var queryString = Object.keys(filterJS)
                .filter(function (key) {
                    return (
                        filterJS[key] !== null &&
                        filterJS[key] !== "" &&
                        filterJS[key] !== 0 &&
                        (Array.isArray(filterJS[key]) ? filterJS[key].length > 0 : true)
                    );
                })
                .map(function (key) {
                    return "filter." + key + "=" + encodeURIComponent(filterJS[key]);
                })
                .join("&");
            

            var currentDomain = window.location.origin;

            var newURL = currentDomain + "/Home/_UserProductsPartial?" + queryString + "&sortBy=" + $sortByDropdown.val();
            setTimeout(function () {
                window.location.href = newURL;
            }, 0);
            commonFuncs.hideLoader();
        }

        $('#clear-all-filters').click(function () {
            var currentDomain = window.location.origin;

            var newURL = currentDomain + "/Home/_UserProductsPartial?mainCategoryId=" + $mainCategoryName + "";
            setTimeout(function () {
                window.location.href = newURL;
            }, 0);
        });

        $sortByDropdown.change(function () {
            filterRequest();
        });

        if ($('#search-input').val() !== '') {
            $('#search-input').addClass('filled'); 
        }

        $('#search-input').on('input', function () {
            if ($(this).val() === '') {
                $(this).removeClass('filled'); 
            } else {
                $(this).addClass('filled'); 
            }
        });
    }
    return {
        init
    };
})();