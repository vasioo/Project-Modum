var editLtc = (function () {
    function init($container) {
        let $ltcImages = $container.find('.image-upload'),
            $titleInputField = $container.find('#title-input-field'),
            $descriptionInputField = $container.find('#description-input-field'),
            $contentInputField = $container.find('#content-input-field'),
            $startDateInputField = $container.find('#startdate-input-field'),
            $saveProductsBtn = $container.find('.btn-save'),
            $endDateInputField = $container.find('#enddate-input-field');

        $ltcImages.change(function (event) {
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


        $saveProductsBtn.click(function () {
            commonFuncs.showLoader();
            const ltc = {
                Title: $titleInputField.val(),
                Description: $descriptionInputField.val(),
                Content: $contentInputField.val(),
                StartDate: $startDateInputField.val(),
                EndDate: $endDateInputField.val(),

            };
            const image = {
                Image: $('.uploaded-image:first').attr('src'),
            };

            const validator = new FormValidator(),
                isValid = validator.validateLTCAndImages(ltc, image);
            if (isValid) {
                $.post('/Worker/EditLTCPost', {
                    ltcDTO: ltc,
                    imageDTO: image
                },
                    function (response) {
                        commonFuncs.hideLoader();
                        Swal.fire({
                            icon: 'info',
                            title: 'Server response',
                            html: `${response.message}`,
                            showClass: {
                                popup: 'animate__animated animate__fadeInDown'
                            },
                            hideClass: {
                                popup: 'animate__animated animate__fadeOutUp'
                            }
                        });

                        location.reload();

                    }).fail(function (error) {
                        commonFuncs.hideLoader();
                        console.log('AJAX request failed:', error);
                    });
            }
            else {
                commonFuncs.hideLoader();
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
        });

        class FormValidator {
            constructor() {
                this.errors = [];
            }


            validateLTCAndImages(ltc, image) {
                this.errors = [];

                this.validateLTC(ltc);
                this.validateImage(image);

                return this.errors.length === 0;
            }

            validateLTC(ltc) {

                if (!ltc.Title || ltc.Title.trim() === '') {
                    this.errors.push('Title field is required.');
                }
                if (!ltc.Content || ltc.Content.trim() === '') {
                    this.errors.push('Content field is required.');
                }
                if (!ltc.Description || ltc.Description.trim() === '') {
                    this.errors.push('Description field is required.');
                }
                if (!ltc.StartDate || ltc.StartDate.trim() === '') {
                    this.errors.push('Start Date field is required.');
                }
                if (!ltc.EndDate || ltc.EndDate.trim() === '') {
                    this.errors.push('End Date field is required.');
                }

            }

            validateImage(image) {
                if (!image.Image || image.ImageName == "" || image.ImageName == " ") {
                    this.errors.push('Image is required');
                    return;
                }
            }

            getErrors() {
                return this.errors;
            }
        }
    }
    return {
        init
    };
})();