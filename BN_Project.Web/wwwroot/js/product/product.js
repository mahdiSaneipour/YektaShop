var radios = document.querySelectorAll(".custom-radio-circle .custom-radio-circle-input");
const showPrice = document.querySelector("#show-price");
const showCount = document.querySelector("#show-count");
const sellSection = document.querySelector("#sell-section");
const notAvailable = document.querySelector("#not-available");


for (var i = 0; i < radios.length; i++) {
    radios[i].addEventListener("click", function () {
        var xhr = new XMLHttpRequest();

        xhr.responseType = 'json';
        xhr.open('GET', '/Api/ProductApi/GetPriceByColorId/' + this.id, true);
        xhr.send();

        xhr.onreadystatechange = async function () {

            if (this.readyState == 4 && this.status == 200) {
                showPrice.innerHTML = commafy(this.response.price);
            }
        }
    });

    radios[i].addEventListener("click", function () {
        var xhr = new XMLHttpRequest();

        xhr.responseType = 'json';
        xhr.open('GET', '/Api/ProductApi/GetCountByColorId/' + this.id, true);
        xhr.send();

        xhr.onreadystatechange = async function () {

            if (this.readyState == 4 && this.status == 200) {

                showCount.innerHTML = "";
                if (this.response.count != 0) {
                    sellSection.style.display = "block";
                    notAvailable.style.display = "none";
                    if (this.response.count <= 5) {
                        var parentCount = document.createElement("span");

                        parentCount.innerHTML = "تنها <span class='max-2'>" + this.response.count + "</span> عدد در انبار باقیست, پیش از اتمام بخرید";

                        showCount.appendChild(parentCount);
                    }
                } else {
                    sellSection.style.display = "none";
                    notAvailable.style.display = "block";
                }
            }
        }
    });
}

function commafy(num) {
    var str = num.toString().split('.');
    if (str[0].length >= 3) {
        str[0] = str[0].replace(/(\d)(?=(\d{3})+$)/g, '$1,');
    }
    if (str[1] && str[1].length >= 3) {
        str[1] = str[1].replace(/(\d{3})/g, '$1 ');
    }
    return str.join('.');
}