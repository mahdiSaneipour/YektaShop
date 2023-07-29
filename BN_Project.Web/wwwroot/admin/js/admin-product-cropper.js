var image = document.getElementById("image");
var file = document.getElementById("file");
var saveProfile = document.getElementById("save-profile");
var cropperBg = document.getElementsByClassName("cropper-bg")[0];
var bannerImage = document.getElementById("banner-image");
var previousBanner = document.getElementById("previous-banner");

var imageValue = "";

let cropper;

image.addEventListener("click", function () {
    file.click();
})

file.addEventListener("change", function (event) {

    var extFile = file.value.substr(file.value.lastIndexOf(".") + 1, file.value.length).toLowerCase();

    if (extFile == "jpg" || extFile == "jpeg" || extFile == "png") {


        image = document.getElementById("image")

        let file = event.target.files[0];
        let reader = new FileReader();

        reader.addEventListener("load", function () {

            /*previousProfile = image.getAttribute("src");*/

            image.src = reader.result;
        }, false)

        if (file) {
            reader.readAsDataURL(file);
        }

    } else {
        alert("فقط میتوانید عکس انتخاب کنید");
    }
})

image.addEventListener("load", function () {
    if (image.src != previousBanner.value) {
        cropper = new Cropper(image, {
            aspectRatio: 1 / 1
        });
    }
})

saveProfile.addEventListener("click", function () {

    cropper.getCroppedCanvas({

    }).toBlob(function (blob) {


        let avatarModel = new FormData();


        avatarModel.set("file", blob);
        avatarModel.set("previous", previousBanner.value);

        var xhr = new XMLHttpRequest();

        xhr.responseType = 'json';
        xhr.open('POST', '/Api/AdminApi/UploadProductImage', true);
        xhr.send(avatarModel);

        xhr.onreadystatechange = async function () {

            if (this.readyState == 4 && this.status == 200) {

                bannerImage.value = this.response.name;

                previousBanner.value = this.response.name;

                image.src = "../../../images/products/normal/" + this.response.name;

                cropper.destroy();

                image = null;
                file.value = null;
            }
        }
    })

})