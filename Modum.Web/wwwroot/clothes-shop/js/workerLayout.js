var addLtc = (function () {
    function init($container) {
        let $ltcImages = $container.find('.image-upload'),
            $titleInputField = $container.find('#title-input-field'),
            $descriptionInputField = $container.find('#description-input-field'),
            $contentInputField = $container.find('#content-input-field'),
            $startDateInputField = $container.find('#startdate-input-field'),
            $saveProductsBtn = $container.find('.btn-save'),
            $discountInputField = $container.find('#discount-input-field'),
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
                Description:$descriptionInputField.val(),
                Content: $contentInputField.val(),
                StartDate: $startDateInputField.val(),
                EndDate: $endDateInputField.val(),
                PercentageOfDiscount: $discountInputField.val()
            };
            const image = {
                Image: $('.uploaded-image:first').attr('src'),
            };

            const validator = new FormValidator(),
                isValid = validator.validateLTCAndImages(ltc, image);
            if (isValid) {
                $.post('/Worker/AddLTCPost', {
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
                if (!ltc.PercentageOfDiscount || ltc.PercentageOfDiscount.trim() === '') {
                    this.errors.push('Discount field is required.');
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
var addProductPage = (function () {
    function init($container) {
        let $productImages = $container.find('.image-upload'),
            $saveProductsBtn = $container.find('.btn-save'),
            $mainCategoriesRadioBtns = $container.find('.main-category-input-fields'),
            $selectCategoryDropdown = $container.find('#selectCategoryDropdownId'),
            $selectSubcategoryDropdown = $container.find('#selectSubcategoryDropdownId'),
            $sizeInputChkBoxes = $container.find('.sizes-input-fields'),
            $availableItemsContainer = $container.find('#available-items-container'),
            $titleInputField = $container.find('#title-input-field'),
            $brandInputField = $container.find('#brand-input-field'),
            $priceInputField = $container.find('#price-input-field'),
            $productColour = $container.find('#item-colour'),
            $discountFromPriceInputField = $container.find('#discount-from-price-input-field'),
            $descriptionInputField = $container.find('#description-input-field'),
            $returnPolicyInputField = $container.find('#return-policy-input-field');


        $productImages.change(function (event) {
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

        $('.choosing-season-button').click(function () {
            $('.choosing-season-button').removeClass('selected');
            $(this).addClass('selected');
        });

        $saveProductsBtn.click(function () {
            commonFuncs.showLoader();
            const images = [];

            const selectedSeason = $('.choosing-season-button.selected').text();

            const sizes = [];

            $('.sizes-input-fields:checked').each(function () {
                var checkedSizeId = $(this).attr('id');
                var $availableItems = $container.find('#size_' + checkedSizeId);
                sizes.push(`${checkedSizeId}-${$availableItems.val()}`);
            });

            const ltcs = [];
            $('.ltc-input-box:checked').each(function () {
                var ltcInput = $(this).attr('id');
                ltcs.push(ltcInput);
            });

            const product = {
                Title: $titleInputField.val(),
                Brand: $brandInputField.val(),
                Sizes: sizes,
                Colour: $productColour.val(),
                Price: $priceInputField.val(),
                DiscountFromPrice: $discountFromPriceInputField.val(),
                Description: $descriptionInputField.val(),
                ReturnPolicy: $returnPolicyInputField.val(),
                MainCategoryId: $mainCategoriesRadioBtns.val(),
                CategoryId: $selectCategoryDropdown.val(),
                SubcategoryId: $selectSubcategoryDropdown.val(),
                Season: selectedSeason,
                LTCs: ltcs
            };

            $('.uploaded-image').each(function () {
                const image = {
                    Image: $(this).attr('src'),
                }
                images.push(image);
            });

            const validator = new FormValidator(),
                isValid = validator.validateProductAndImages(product, images);
            if (isValid) {
                $.post('/Worker/AddProduct', {
                    productDTO: product,
                    imagesDTO: images
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
                        console.log('AJAX request failed:', error);
                        commonFuncs.hideLoader();

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


            validateProductAndImages(product, images) {
                this.errors = [];

                this.validateProduct(product);
                this.validateImages(images);

                return this.errors.length === 0;
            }

            validateProduct(product) {
                var parts = product.Sizes;

                parts.forEach((part) => {

                    part = part.split('-');

                    if (part.length > 1) {
                        var AvailableItems = parseInt(part[1]);

                        var parsedAvailableItems = parseInt(AvailableItems);


                        if (parsedAvailableItems != AvailableItems) {
                            this.errors.push('The field Available Items can contain only whole number values');
                        }
                    }
                    else {
                        this.errors.push('Size field is required.');
                    }
                });

                if (!product.Title || product.Title.trim() === '') {
                    this.errors.push('Title field is required.');
                }
                if (!product.Brand || product.Brand.trim() === '') {
                    this.errors.push('Brand field is required.');
                }
                if (isNaN(product.Price)) {
                    this.errors.push('Price field cannot contain letters/symbols.');
                }
                if (isNaN(product.DiscountFromPrice)) {
                    this.errors.push('Discount from price field cannot contain letters/symbols.');
                }
                if (!product.Price || product.Price.trim() === '') {
                    this.errors.push('Price field is required.');
                }
                if (!product.DiscountFromPrice || product.DiscountFromPrice.trim() === '') {
                    this.errors.push('Discount From Price Field is required.');
                }
                if (!product.Description || product.Description.trim() === '') {
                    this.errors.push('Description field is required.');
                }
                if (!product.ReturnPolicy || product.ReturnPolicy.trim() === '') {
                    this.errors.push('Return Policy field is required.');
                }

                if (!product.MainCategoryId || product.MainCategoryId.trim() === '') {
                    this.errors.push('Main Category field is required.');
                }
                if (!product.CategoryId || product.CategoryId.trim() === '') {
                    this.errors.push('Category field is required.');
                }
                if (!product.SubcategoryId || product.SubcategoryId.trim() === '') {
                    this.errors.push('Subcategory field is required.');
                }
                if (!product.Season || product.Season.trim() === '') {
                    this.errors.push('Season selection is required.');
                }

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

        $mainCategoriesRadioBtns.click(function () {

            $.post('/Worker/FilterMainCategoryData', {
                mainCategoryId: $(this).attr('id'),
                categoryId: $selectCategoryDropdown.val()
            },
                function (response) {
                    $selectCategoryDropdown.empty();
                    $selectCategoryDropdown.append('<option id="0">--Select existing category--</option>');
                    $.each(response.categories, function (index, item) {
                        $selectCategoryDropdown.append('<option value="' + item.id + '">' + item.name + '</option>');
                    });

                    $selectSubcategoryDropdown.empty();
                    $selectSubcategoryDropdown.append('<option id="0">--Select existing subcategory--</option>');
                    $.each(response.subcategories, function (index, item) {
                        $selectSubcategoryDropdown.append('<option value="' + item.id + '">' + item.name + '</option>');
                    });
                }).fail(function (error) {
                    console.log('AJAX request failed:', error);
                });
        });

        $selectCategoryDropdown.change(function () {
            var $checkedRadio = $("input[name='mainCategory']:checked").val();
            $.post('/Worker/FilterMainCategoryData', {
                mainCategoryId: $checkedRadio,
                categoryId: $(this).val()
            },
                function (response) {
                    $selectSubcategoryDropdown.empty();
                    $selectSubcategoryDropdown.append('<option id="0">--Select existing subcategory--</option>');
                    $.each(response, function (index, item) {
                        $selectSubcategoryDropdown.append('<option value="' + item.id + '">' + item.name + '</option>');
                    });

                }).fail(function (error) {
                    console.log('AJAX request failed:', error);
                });
        });

        $sizeInputChkBoxes.on('click', function () {
            let $currentId = $(this).attr("id");
            if ($(this).is(':checked')) {
                let htmlAvailableItemsString =
                    '<div id="div_' + $currentId + '">' +
                    '<label>' +
                    'Number of available items for ' + $currentId +
                    '<input type="number" id="size_' + $currentId + '" placeholder="Available Items For ' + $currentId + '" class="form-control" /> ' +
                    '</label>' +
                    '<hr />' +
                    '</div>';

                $availableItemsContainer.append(htmlAvailableItemsString);
            }
            else {
                $('#div_' + $currentId).remove();
            }
        });

        $selectSubcategoryDropdown.change(function () {
            var selectedSubcategoryId = $(this).find('option:selected');
            $.post('/Worker/GetCategoryBySubcategory', {
                subcategoryId: selectedSubcategoryId.val()
            },
                function (response) {
                    $selectCategoryDropdown.val(response);
                }).fail(function (error) {
                    console.log('AJAX request failed:', error);
                });
        });
    }
    return {
        init
    };
})();
var createDocumentPage = (function () {
    function init($container) {
        $(document).ready(function () {
            $('#submit-btn').click(function (event) {
                var contentData = tinyMCE.activeEditor.getContent();
                var antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();

                var formDataObject = {
                    Title: $('#title').val(),
                    Content: contentData,
                    Id: $('#docId').val(),
                    __RequestVerificationToken: antiForgeryToken
                };
                $.post('/Docs/CreatePost', {
                    doc: formDataObject,
                    blogImage: $('#sent-image').attr('src')
                }, function (response) {
                    if (response.status) {
                        commonFuncs.hideLoader();
                        Swal.fire({
                            icon: 'success',
                            title: 'Успех',
                            text: response.message,
                            showConfirmButton: false,
                            allowOutsideClick: false,
                            timerProgressBar: true,
                            timer: 3000
                        });
                        location.reload();
                    } else {
                        commonFuncs.hideLoader();
                        Swal.fire({
                            icon: 'error',
                            title: 'Грешка',
                            text: response.message,
                            showConfirmButton: false,
                            allowOutsideClick: false,
                            timerProgressBar: true,
                            timer: 3000
                        });
                    }

                    location.reload();
                }).fail(function (error) {
                    commonFuncs.hideLoader();
                    console.log('AJAX request failed:', error);
                });
            });
        });
    }
    return {
        init
    };
})();
var editDocumentPage = (function () {
    function init($container) {
        $(document).ready(function () {
            $('#submit-btn').click(function (event) {
                var contentData = tinyMCE.activeEditor.getContent();
                var antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();

                var formDataObject = {
                    Title: $('#title').val(),
                    Content: contentData,
                    DateOfCreation: $('#DateOfCreation').val(),
                    Id: $('#docId').val(),
                    __RequestVerificationToken:antiForgeryToken
                };
                $.post('/Docs/EditDocumentPost', {
                    doc: formDataObject,
                    blogImage: $('#sent-image').attr('src')
                }, function (response) {
                    if (response.status) {
                        commonFuncs.hideLoader();
                        Swal.fire({
                            icon: 'success',
                            title: 'Успех',
                            text: response.message,
                            showConfirmButton: false,
                            allowOutsideClick: false,
                            timerProgressBar: true,
                            timer: 3000
                        });
                        location.reload();
                    } else {
                        commonFuncs.hideLoader();
                        Swal.fire({
                            icon: 'error',
                            title: 'Грешка',
                            text: response.message,
                            showConfirmButton: false,
                            allowOutsideClick: false,
                            timerProgressBar: true,
                            timer: 3000
                        });
                    }

                    location.reload();
                }).fail(function (error) {
                    commonFuncs.hideLoader();
                    console.log('AJAX request failed:', error);
                });
            });
        });
        document.addEventListener("DOMContentLoaded", function () {
            var elements = document.getElementsByClassName('deleteBlogPostButton');

            Array.from(elements).forEach(function (element) {
                element.addEventListener('click', function (event) {
                    event.preventDefault();

                    Swal.fire({
                        title: 'Are you sure?',
                        text: "You won't be able to revert this blog!",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'Yes, Delete!'
                    }).then(function (result) {
                        if (result.isConfirmed) {
                            var id = element.getAttribute('id');

                            var antiForgeryToken = document.querySelector('input[name="__RequestVerificationToken"]').value;

                            fetch('/Docs/DeleteDocumentPost', {
                                method: 'POST',
                                headers: {
                                    'Content-Type': 'application/x-www-form-urlencoded',
                                    'RequestVerificationToken': antiForgeryToken
                                },
                                body: 'id=' + id
                            })
                                .then(function (response) {
                                    if (!response.ok) {
                                        throw new Error('Network response was not ok');
                                    }
                                    return response.json();
                                })
                                .then(function (data) {
                                    var currentDomain = window.location.host
                                    var updatedUrl = "https://" + currentDomain + "/Footer/Blog";

                                    window.location.href = updatedUrl;

                                })
                                .catch(function (error) {
                                    console.error('Fetch error:', error);
                                });
                        }
                    });
                });
            });
        });

    }
    return {
        init
    };
})();
var editLtc = (function () {
    function init($container) {
        let $ltcImages = $container.find('.image-upload'),
            $titleInputField = $container.find('#title-input-field'),
            $descriptionInputField = $container.find('#description-input-field'),
            $contentInputField = $container.find('#content-input-field'),
            $startDateInputField = $container.find('#startdate-input-field'),
            $saveProductsBtn = $container.find('.btn-save'),
            $discountInputField = $container.find('#discount-input-field'),
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
                Id: $('#model-id').data('model-id'),
                Title: $titleInputField.val(),
                Description: $descriptionInputField.val(),
                Content: $contentInputField.val(),
                StartDate: $startDateInputField.val(),
                EndDate: $endDateInputField.val(),
                PercentageOfDiscount:$discountInputField.val()
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
                if (!ltc.PercentageOfDiscount || ltc.PercentageOfDiscount.trim() === '') {
                    this.errors.push('Discount field is required.');
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
    var editProductPage = (function () {
    function init($container) {
        let $productImages = $container.find('.image-upload'),
            $saveEditProductsBtn = $container.find('.edit-btn-save'),
            $mainCategoriesRadioBtnsEdit = $container.find('.main-category-input-fields-edit'),
            $selectCategoryDropdown = $container.find('#selectCategoryDropdownId'),
            $selectSubcategoryDropdown = $container.find('#selectSubcategoryDropdownId'),
            $titleInputField = $container.find('#title-input-field'),
            $brandInputField = $container.find('#brand-input-field'),
            $priceInputField = $container.find('#price-input-field'),
            $productColour = $container.find('#item-colour'),
            $discountFromPriceInputField = $container.find('#discount-from-price-input-field'),
            $descriptionInputField = $container.find('#description-input-field'),
            $returnPolicyInputField = $container.find('#return-policy-input-field');


        $saveEditProductsBtn.click(function () {
            commonFuncs.showLoader();
            const images = [];

            const sizes = [];

            $('.sizes-input-fields:checked').each(function () {
                var checkedSizeId = $(this).attr('id');
                var $availableItems = $container.find('#size_' + checkedSizeId);
                var $parentContainerId = $availableItems.closest('div').attr('id');
                var idParts = $parentContainerId.split('-');
                var lastItem = idParts[idParts.length - 1]; 
                sizes.push(`${checkedSizeId}-${$availableItems.val()}-${$availableItems[0].classList[1]}-${lastItem}`);
            });

            const ltcs = [];
            $('.ltc-input-box:checked').each(function () {
                var ltcInput = $(this).attr('id');
                ltcs.push(ltcInput);
            });

            const product = {
                Id: $saveEditProductsBtn.attr('id'),
                Title: $titleInputField.val(),
                Brand: $brandInputField.val(),
                Sizes: sizes,
                Price: $priceInputField.val(),
                DiscountFromPrice: $discountFromPriceInputField.val(),
                Description: $descriptionInputField.val(),
                ReturnPolicy: $returnPolicyInputField.val(),
                MainCategoryId: $mainCategoriesRadioBtnsEdit.val(),
                CategoryId: $selectCategoryDropdown.val(),
                Colour: `${$productColour.val()}`,
                SubcategoryId: $selectSubcategoryDropdown.val(),
                ImageContainerId: $('.image-container').attr('id'),
                LTCs: ltcs
            };

            $('.uploaded-image').each(function () {

                const image = {
                    Image: $(this).attr('src'),
                }
                images.push(image);
            });

            const validator = new FormValidator(),
                isValid = validator.validateProductAndImages(product, images);
            if (isValid) {
                $.post('/Worker/EditProduct', {
                    productDTO: product,
                    imagesDTO: images
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
                        console.log('AJAX request failed:', error);
                        commonFuncs.hideLoader();
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


            validateProductAndImages(product, images) {
                this.errors = [];

                this.validateProduct(product);
                this.validateImages(images);

                return this.errors.length === 0;
            }

            validateProduct(product) {
                var parts = product.Sizes;

                parts.forEach((part) => {

                    part = part.split('-');

                    if (part.length > 1) {
                        var Size = part[0];
                        var AvailableItems = parseInt(part[1]);

                        var parsedAvailableItems = parseInt(AvailableItems);


                        if (parsedAvailableItems != AvailableItems) {
                            this.errors.push('The field Available Items can contain only whole number values');
                        }
                    }
                    else {
                        this.errors.push('Size field is required.');
                    }
                });

                if (!product.Title || product.Title.trim() === '') {
                    this.errors.push('Title field is required.');
                }
                if (!product.Brand || product.Brand.trim() === '') {
                    this.errors.push('Brand field is required.');
                }
                if (isNaN(product.Price)) {
                    this.errors.push('Price field cannot contain letters/symbols.');
                }
                if (isNaN(product.DiscountFromPrice)) {
                    this.errors.push('Discount from price field cannot contain letters/symbols.');
                }


                if (!product.Price || product.Price.trim() === '') {
                    this.errors.push('Price field is required.');
                }
                if (!product.DiscountFromPrice || product.DiscountFromPrice.trim() === '') {
                    this.errors.push('Discount From Price Field is required.');
                }
                if (!product.Description || product.Description.trim() === '') {
                    this.errors.push('Description field is required.');
                }
                if (!product.ReturnPolicy || product.ReturnPolicy.trim() === '') {
                    this.errors.push('Return Policy field is required.');
                }

                if (!product.MainCategoryId || product.MainCategoryId.trim() === '') {
                    this.errors.push('Main Category field is required.');
                }
                if (!product.CategoryId || product.CategoryId.trim() === '') {
                    this.errors.push('Category field is required.');
                }
                if (!product.SubcategoryId || product.SubcategoryId.trim() === '') {
                    this.errors.push('Subcategory field is required.');
                }

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


        $productImages.change(function (event) {
            const $self = $(this),
                $uploadedImage = $self.parent().find('.uploaded-image'),
                file = event.target.files[0];

            // Display only if it is an image file
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

        $(document).ready(function () {
            $mainCategoriesRadioBtnsEdit.trigger('click');
            $selectCategoryDropdown.trigger('change');
        });


        $mainCategoriesRadioBtnsEdit.click(function () {
            $.post('/Worker/FilterMainCategoryData', {
                mainCategoryId: $(this).attr('id'),
                categoryId: $selectCategoryDropdown.val()
            },
                function (response) {
                    $selectCategoryDropdown.empty();
                    $selectCategoryDropdown.append('<option id="0">--Select existing category--</option>');
                    $.each(response.categories, function (index, item) {
                        $selectCategoryDropdown.append('<option value="' + item.id + '">' + item.name + '</option>');
                    });

                    $selectSubcategoryDropdown.empty();
                    $selectSubcategoryDropdown.append('<option id="0">--Select existing subcategory--</option>');
                    $.each(response.subcategories, function (index, item) {
                        $selectSubcategoryDropdown.append('<option value="' + item.id + '">' + item.name + '</option>');
                    });
                }).fail(function (error) {
                    console.log('AJAX request failed:', error);
                });

        });

        $selectCategoryDropdown.on('change', function () {
            var $checkedRadio = $("#mainCategoriesRadioBtns input[type='radio']:checked");
            $.post('/Worker/FilterMainCategoryData', {
                mainCategoryId: $checkedRadio,
                categoryId: $(this).val()
            }).fail(function (error) {
                console.log('AJAX request failed:', error);
            });

        });

        $selectSubcategoryDropdown.change(function () {
            var selectedSubcategoryId = $(this).find('option:selected');
            var selectedCategoryId = $selectCategoryDropdown.val();
            $.post('/Worker/GetCategoryBySubcategory', {
                subcategoryId: selectedSubcategoryId.val()
            },
                function (response) {
                    $selectCategoryDropdown.val(response);
                }).fail(function (error) {
                    console.log('AJAX request failed:', error);
                });
        });
    }
    return {
        init
    };
})();
var manageLtcs = (function () {
    function init($container) {

        let $editButton = $container.find('');
        $container.on('click', '.redirect-to-edit-page', function () {
            const editPageUrl = '/Worker/EditLTC?ltcId=' + $(this).attr('id');

            window.location.href = editPageUrl;
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
                    $.post('/Worker/DeleteLTC', { ltcId: $(this).attr('id') }, function (response) {
                        commonFuncs.hideLoader();
                        location.reload();
                    }).fail(function (error) {
                        commonFuncs.hideLoader();
                        alert('AJAX request failed: ', error);
                    });
                }
            })
        });

    }
    return {
        init
    };
})();
var manageOrders = (function () {
    function init($container) {
        $('.view-details').click(function () {
            var modalId = $(this).attr('data-target');
            $(modalId).modal('show');
        });
        $(document).on('click', '.order-btn', function () {
            var orderId = $(this).attr('data-orderId');
            var newStatus = $(this).attr('data-orderStatus');

            $.ajax({
                url: '/Worker/ChangeDeliveryStatus',
                method: 'POST',
                data: { orderId: orderId, newStatus: newStatus },
                success: function (response) {
                    if (response.status) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Success',
                            text: response.Message
                        });
                        location.reload();
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.Message
                        });
                    }
                },
                error: function (xhr, status, error) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'An error occurred: ' + error
                    });
                }
            });
        });
    }
    return {
        init
    };
})();
var manageProductsPage = (function () {
    function init($container) {

        let $editButton = $container.find('');
        $container.on('click', '.redirect-to-edit-page', function () {
            const editPageUrl = '/Worker/EditProduct?productId=' + $(this).attr('id');

            window.location.href = editPageUrl;
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

    }
    return {
        init
    };
})();
var manageSubSelectionsPage = (function () {
    function init($container) {
        let $submitBtn = $container.find('#save-btn'),
            $btnAddCategoryRow = $container.find('#add-category-row-btn'),
            $selectMainCategoryDropdown = $container.find('#selectMainCategoryDropdownId'),
            $mainCategoryTableDiv = $container.find('#mainCategoryTable'),
            counter = 0,
            newTemplateCategoryRow =
                '<tr class="cat-row">' +
                '   <td><input type="text" class="form-control category-name" required></td>' +
                '   <td id="for-subcategories">' +
                '       <a class="btn btn-primary col" data-toggle="collapse" href="" role="button" aria-expanded="true" aria-controls="">' +
                '           Subcategories' +
                '       </a> ' +
                '       <div class="pt-3 subcategoryTable collapse show" id="">' +
                '           <table class="col-12">' +
                '              <thead>' +
                '                  <tr>' +
                '                      <th>Subcategory Name</th>' +
                '                      <th></th>' +
                '                  </tr>' +
                '              </thead>' +
                '              <tbody class="subcategory-tbody">' +
                '              </tbody>' +
                '          </table>' +
                '          <div class="pt-2">'+
                '               <button type="button" class="btn btn-primary add-subcategory-row-btn" id=""><i class="fas fa-plus"></i> Add Subcategory</button>' +
                '          </div>' +
                '       </div>' +
                '   </td>' +
                '   <td>' +
                '       <button type="button" class="btn btn-danger delete-row m-1"><i class="fa fa-trash"></i></button>' +
                '   </td>' +
                '</tr>';

        $submitBtn.click(function () {
            commonFuncs.showLoader();
            let selectedMainCategoryId = $selectMainCategoryDropdown.val(),
                selectedCategoriesDTO = [],
                selectedSubcategoriesDTO = [];

            let $categoriesTable = $mainCategoryTableDiv.find('.category-table');

            $categoriesTable.find('tbody .cat-row').each(function () {
                const $row = $(this),
                    categoryName = $row.find('.category-name').val().trim();
                const category = {
                    CategoryName: categoryName,
                };

                $row.find('tbody .sub-row').each(function () {
                    const $subrow = $(this),
                        subcategoryName = $subrow.find('.subcategory-name').val().trim();
                    const subcategory = {
                        SubcategoryName: subcategoryName,
                        CategoryName: categoryName
                    };

                    selectedSubcategoriesDTO.push(subcategory);
                });

                selectedCategoriesDTO.push(category);
            });

            const validator = new FormValidator(),
                isValid = validator.validateMainCategory(selectedMainCategoryId, selectedCategoriesDTO, selectedSubcategoriesDTO);
            if (isValid) {

                $.post('/Worker/ManageSubSelection', {
                    mainCategoryId: selectedMainCategoryId,
                    categoriesDTO: selectedCategoriesDTO,
                    subcategoriesDTO: selectedSubcategoriesDTO
                }, function (response) {
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


            validateMainCategory(mainCategory, categories, subcategories) {
                this.errors = [];

                this.validateCategories(categories);
                this.validateSubcategories(subcategories);

                return this.errors.length === 0;
            }

            validateCategories(categories) {

                if (!categories || categories.length === 0) {
                    this.errors.push('At least one category is required.');
                } else {
                    categories.forEach((category) => {

                        if (!category.CategoryName || category.CategoryName.trim() === '') {
                            this.errors.push('Category name is required.');
                        }

                    });
                }
            }

            validateSubcategories(subcategories) {

                if (!subcategories || subcategories.length === 0) {
                    this.errors.push('At least one subcategory is required.');
                } else {
                    subcategories.forEach((subcategory) => {

                        if (!subcategory.SubcategoryName || subcategory.SubcategoryName.trim() === '') {
                            this.errors.push('Subcategory name is required.');
                        }

                    });
                }
            }

            getErrors() {
                return this.errors;
            }
        }

        $container.on('click', '.add-subcategory-row-btn', function () {

            var $button = $(this);
            var $tempTableDiv = $button.closest('.subcategoryTable');
            var $nearestTbody = $tempTableDiv.find('tbody');

            let subcategoryRow = '<tr class="sub-row">' +
                '   <td><input type="text" class="form-control subcategory-name" required></td>' +
                '   <td>' +
                '       <button type="button" class="btn btn-danger delete-row m-1"><i class="fa fa-trash"></i></button>' +
                '   </td>' +
                '</tr>';

            let $newRow = $(subcategoryRow);

            $nearestTbody.append($newRow);

        });

        $selectMainCategoryDropdown.change(function () {
            var selectedMainCategoryId = $(this).val();
            $mainCategoryTableDiv.find('tbody').empty();
            $mainCategoryTableDiv.css("display", $selectMainCategoryDropdown.val() !== "" ? "block" : "none");
            $submitBtn.css("display", $selectMainCategoryDropdown.val() !== "" ? "block" : "none");


            if (selectedMainCategoryId) {
                commonFuncs.showLoader();
                $.post('/Worker/LoadMainCategoryData', { mainCategoryId: selectedMainCategoryId }, function (response) {

                    let categories = (response.categories),
                        $categoryBody = $mainCategoryTableDiv.find('tbody');

                    categories.forEach(function (category) {
                        counter++;
                        let $newRow = $(newTemplateCategoryRow);
                        $newRow.find('.category-name').val(category.name);

                        let $aTag = $newRow.find('a');
                        let $subcategoryDiv = $newRow.find('.subcategoryTable');

                        $subcategoryDiv.attr('id', 'subcategoryTable-' + counter + '');

                        $aTag.attr('aria-controls', 'subcategoryTable-' + counter + '');
                        $aTag.attr('href', '#subcategoryTable-' + counter + '');

                        category.subcategories.forEach(function (subcategory) {

                            let subcategoryRow = '<tr class="sub-row">' +
                                '   <td><input type="text" class="form-control subcategory-name" required value="' + subcategory.name + '"></td>' +
                                '   <td>' +
                                '       <button type="button" class="btn btn-danger delete-row m-1"><i class="fa fa-trash"></i></button>' +
                                '   </td>' +
                                '</tr>';

                            $newRow.find('.subcategory-tbody').append(subcategoryRow);

                        });
                        $categoryBody.append($newRow);
                        commonFuncs.hideLoader();
                    });

                }).fail(function (error) {
                    commonFuncs.hideLoader();
                    alert('AJAX request failed: ', error);
                });
            } commonFuncs.hideLoader();
        });

        $btnAddCategoryRow.click(function () {

            counter++;

            let $newRow = $(newTemplateCategoryRow);
            let $aTag = $newRow.find('a');
            let $subcategoryDiv = $newRow.find('.subcategoryTable');

            $subcategoryDiv.attr('id', 'subcategoryTable-' + counter + '');

            $aTag.attr('aria-controls', 'subcategoryTable-' + counter + '');
            $aTag.attr('href', '#subcategoryTable-' + counter + '');


            $mainCategoryTableDiv.find('#category-tbody').append($newRow);
        });

        $(document).on('click', '.delete-row', function () {
            var clickedElement = this;
            Swal.fire({
                title: 'Are you sure?',
                text: "You won't be able to revert this row!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, Delete!'
            }).then((result) => {
                if (result.isConfirmed) {
                    $(clickedElement).closest('tr').remove();
                    Swal.fire({
                        icon: 'success',
                        title: 'Success!',
                        text: "The item was removed temporary until Save is clicked(for returning refresh page)",
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'OK'
                    });
                }
            });
        });

        $container.on('change', '.category-name', function () {
            let $subcategoryTable = $mainCategoryTableDiv.closest('#subcategoryTable')
            $subcategoryTable.attr('id', 'for-subcategories-' + $(this).val() + '');
            $subcategoryTable.attr('href', '#for-subcategories-' + $(this).val() + '');
        });
    }
    return {
        init
    };
})();