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