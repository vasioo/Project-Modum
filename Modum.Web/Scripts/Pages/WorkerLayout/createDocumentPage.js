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