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
                $.post('/Admin/EditProduct', {
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
            $.post('/Admin/FilterMainCategoryData', {
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
            $.post('/Admin/FilterMainCategoryData', {
                mainCategoryId: $checkedRadio,
                categoryId: $(this).val()
            }).fail(function (error) {
                console.log('AJAX request failed:', error);
            });

        });

        $selectSubcategoryDropdown.change(function () {
            var selectedSubcategoryId = $(this).find('option:selected');
            var selectedCategoryId = $selectCategoryDropdown.val();
            $.post('/Admin/GetCategoryBySubcategory', {
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