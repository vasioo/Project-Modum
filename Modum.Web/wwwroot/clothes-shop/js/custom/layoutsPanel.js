document.addEventListener("DOMContentLoaded", function () {
    var hamburgerMenu = document.getElementById('hamburger-menu');
    hamburgerMenu.addEventListener("click", function () {
        var navbarNav = document.querySelector('#navbarCollapse');
        navbarNav.classList.toggle('show');
    });
   
    var navbarNav = document.querySelector('#navbarCollapse');
    navbarNav.style.display = 'none';
});


$('.image-upload').change(function (event) {
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

