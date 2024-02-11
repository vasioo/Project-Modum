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