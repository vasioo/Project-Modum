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