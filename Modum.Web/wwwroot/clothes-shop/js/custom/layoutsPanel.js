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

$('.btn-purple').mousemove(function (e) {
    var btnWidth = $(this).width();
    var btnHeight = $(this).height();
    var mouseX = e.pageX - $(this).offset().left;
    var mouseY = e.pageY - $(this).offset().top;

    var percentX = mouseX / btnWidth * 100;
    var percentY = mouseY / btnHeight * 100;

    var gradientX = percentX;
    var gradientY = percentY;

    var gradient = 'radial-gradient(circle at ' + gradientX + '% ' + gradientY + '%, rgba(255, 0, 255,1) 0%, rgba(255,0,255,0.5) 20%, rgba(50,39,110,1) 50%, rgba(50,39,110,1) 100%)';

    $(this).css('background-image', gradient);
});

$('.btn-purple').mouseleave(function () {
    $(this).css('background', 'linear-gradient(131deg, rgba(50,39,110,1) 3%, rgba(51,62,204,1) 22%, rgba(50,39,110,1) 100%)');
});
$(document).ready(function () {
    $('#hamburger-menu-custom').click(function () {
        $(this).toggleClass('active');
        $('.overlay').toggle();
        $('#sidebar-mobile').toggleClass('d-block d-none');

    });

    $('.overlay').click(function () {
        $('#hamburger-menu-custom').removeClass('active');
        $(this).hide();
        $('#sidebar-mobile').addClass('d-none');
        $('#sidebar-mobile').removeClass('d-block');
    });
});
