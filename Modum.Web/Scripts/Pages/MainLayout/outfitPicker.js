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

                    const personImage = $('#person-image')[0].files[0];
                    const clothImage = $('#clothing-image')[0].files[0];

                    const form = new FormData();
                    form.append('personImage', personImage);
                    form.append('clothImage', clothImage);

                    const settings = {
                        async: true,
                        crossDomain: true,
                        url: 'https://texel-virtual-try-on.p.rapidapi.com/try-on-file',
                        method: 'POST',
                        headers: {
                            'X-RapidAPI-Key': '118c0ba3f2msh2f51cd3a9d5b701p13df6cjsnc24c9759f30b',
                            'X-RapidAPI-Host': 'texel-virtual-try-on.p.rapidapi.com'
                        },
                        processData: false,
                        contentType: false,
                        mimeType: 'multipart/form-data',
                        data: form
                    };

                    $.ajax(settings)
                        .done(function (response) {
                            if (true) {
                                var imagePath = response.response.ouput_path_img;

                                $('#additionalPicturePreview').attr('src', imagePath);  
                                $('#resultText').attr('src', response.message);  

                                $('#hiddenInput').css('display', 'block');
                                $('.retry-btn').css('display', 'block');
                                $('.create-image').css('display', 'none');

                                commonFuncs.hideLoader();
                            } else {
                                alert("An unexpected error occurred. Please try again later.");
                            }
                            commonFuncs.hideLoader();
                        })
                        .fail(function (xhr, status, error) {
                            console.error(xhr.responseText);
                            alert('Error occurred. Check console for details.');
                        });
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