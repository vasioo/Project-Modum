var genderTemplate = (function () {
    function init($container) {

        let $prevBtn = $container.find('#arrow-left'),
            $nextBtn = $container.find('#arrow-right'),
            $seasonImage = $container.find('#image-for-season'),
            $shopNowBtn = $container.find('#button-for-custom-season-overflow'),
            $ltcBtn = $container.find('#button-for-limited-time-campaign');

        $shopNowBtn.click(function () {
            var currentDomain = window.location.origin;

            var mainCategory = $('.genderCall-page').attr('id');

            var newURL = currentDomain + "/Home/_UserProductsPartial?" + "mainCategoryId" + mainCategory;
            setTimeout(function () {
                window.location.href = newURL;
            }, 0);
        });

        $ltcBtn.click(function () {
            var currentDomain = window.location.origin;

            var mainCategory = $('.genderCall-page').attr('id');

            var newURL = currentDomain + "/Home/_UserProductsPartial?" + "mainCategoryId" + mainCategory;
            setTimeout(function () {
                window.location.href = newURL;
            }, 0);
        });

        const date = new Date();
        let month = date.getMonth() + 1;

        if (month >= 3 && month <= 5) {
            $seasonImage.attr("src", "/clothes-shop/images/season-specialized/spring-main-2.jpg")
        }
        else if (month >= 6 && month <= 8) {
            $seasonImage.attr("src", "/clothes-shop/images/season-specialized/summer-main-2.jpg")
        }
        else if (month >= 9 && month <= 11) {
            $seasonImage.attr("src", "/clothes-shop/images/season-specialized/autumn-main-2.jpg")
        }
        else {
            $seasonImage.attr("src", "/clothes-shop/images/season-specialized/winter-main-2.jpg")
        }

        const endDateElement = document.getElementById("countdown-container");
        if (!endDateElement) {
            return;
        }

        const endDateAttribute = endDateElement.getAttribute("data-enddate");
        if (!endDateAttribute) {
            return;
        }

        const endDate = new Date(endDateAttribute).getTime();

        if (isNaN(endDate)) {
            return;
        }

        const countdownInterval = setInterval(updateCountdown, 1000);

        function updateCountdown() {
            const now = new Date().getTime();
            const distance = endDate - now;

            if (distance < 0) {
                clearInterval(countdownInterval);
                document.getElementById("countdown").innerHTML = "EXPIRED";
            } else {
                const days = Math.floor(distance / (1000 * 60 * 60 * 24));
                const hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                const seconds = Math.floor((distance % (1000 * 60)) / 1000);

                var htmlCountdown = `
        
               <div class="col pl-2 pr-2">
                   <div class="bg-dark pt-2 rounded-top data-shower">
                       ${days}
                   </div>
                   <div class="bg-dark pl-1 pr-1 pt-4 pb-2 rounded-bottom label-data-shower">Days</div>
               </div>
               <div class="col pl-2 pr-2">
                   <div class="bg-dark pt-2 rounded-top data-shower">
                       ${hours}
                   </div>
                   <div class="bg-dark pl-1 pr-1 pt-4 pb-2 rounded-bottom label-data-shower">Hours</div>
               </div>
               <div class="col pl-2 pr-2">
                   <div class="bg-dark pt-2 rounded-top data-shower">
                       ${minutes}
                   </div>
                   <div class="bg-dark pl-1 pr-1 pt-4 pb-2 rounded-bottom label-data-shower">Minutes</div>
               </div>
               <div class="col pl-2 pr-2">
                   <div class="bg-dark pt-2 rounded-top data-shower">
                      ${seconds}
                   </div>
                   <div class="bg-dark pl-1 pr-1 pt-4 pb-2 rounded-bottom label-data-shower">Seconds</div>
               </div>
            
            `;

                document.getElementById("countdown").innerHTML = htmlCountdown;
            }
        }

    }

    return {
        init
    };
})();