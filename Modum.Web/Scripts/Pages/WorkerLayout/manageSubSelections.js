﻿var manageSubSelectionsPage = (function () {
    function init($container) {
        let $submitBtn = $container.find('#save-btn'),
            $btnAddCategoryRow = $container.find('#add-category-row-btn'),
            $selectMainCategoryDropdown = $container.find('#selectMainCategoryDropdownId'),
            $saveBtnDivRow = $container.find('#save-btn-div-row'),
            $mainCategoryTableDiv = $container.find('#mainCategoryTable'),
            counter = 0,
            newTemplateCategoryRow =
                '<tr class="cat-row">' +
                '   <td><input type="text" class="form-control category-name" required></td>' +
                '   <td id="for-subcategories">' +
                '       <a class="btn btn-primary col" data-toggle="collapse" href="" role="button" aria-expanded="false" aria-controls="">' +
                '           Subcategories' +
                '       </a> ' +
                '       <div class="card subcategoryTable" id="">' +
                '               <div class="card-header"></div>' +
                '               <div class="card-body">' +
                '                 <table>' +
                '                     <thead>' +
                '                         <tr>' +
                '                             <th>Subcategory Name</th>' +
                '                             <th></th>' +
                '                         </tr>' +
                '                     </thead>' +
                '                     <tbody class="subcategory-tbody">' +
                '                         <!--Add options for change-->' +
                '                     </tbody>' +
                '                 </table>' +
                '                 <button type="button" class="btn btn-primary add-subcategory-row-btn" id=""><i class="fas fa-plus"></i> Add Subcategory</button>' +
                '              </div>' +
                '           </div>' +
                '   </td>' +
                '   <td>' +
                '       <button type="button" class="btn btn-danger delete-row m-1"><i class="fa fa-trash"></i></button>' +
                '   </td>' +
                '</tr>';

        $submitBtn.click(function () {
            //when submitting the event should be binded to find the nearest maincategory / category so that 
            //the element could be binded to its parent
            commonFuncs.showLoader();
            let selectedMainCategoryId = $selectMainCategoryDropdown.val(),
                selectedCategoriesDTO = [],
                selectedSubcategoriesDTO = [];

            let $categoriesTable = $mainCategoryTableDiv.find('.category-table');

            $categoriesTable.find('tbody .cat-row').each(function () {
                const $row = $(this),
                    categoryName = $row.find('.category-name').val().trim();
                // Create an object representing the category field and add it to the array
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

                // If form data is valid, make the AJAX POST request

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
                    // Handle the AJAX request failure
                    // This function will be executed if the AJAX request encounters an error
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

            bindRowEvents($newRow);
        });

        $selectMainCategoryDropdown.change(function () {
            var selectedMainCategoryId = $(this).val();
            $mainCategoryTableDiv.find('tbody').empty();
            $mainCategoryTableDiv.css("display", $selectMainCategoryDropdown.val() !== "" ? "block" : "none");
            $saveBtnDivRow.css("display", $selectMainCategoryDropdown.val() !== "" ? "block" : "none");


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
                                '       <button type="button" class="btn btn-danger delete-subcategory-row m-1"><i class="fa fa-trash"></i></button>' +
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
            }
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

            bindRowEvents($newRow);
        });

        function bindRowEvents($newRow) {
            $newRow.find('.delete-row').click(function () {
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
                        $(this).closest('tr').remove();
                    }
                })
            });
        }

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