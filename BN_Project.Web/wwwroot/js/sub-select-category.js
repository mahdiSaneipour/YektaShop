const subCategory = document.getElementById("sub-category");

function SetSubGroup(element) {
    const groupId = element.value;
    console.log("value : " + groupId)
    var xhr = new XMLHttpRequest();

    xhr.responseType = 'json';

    var url = "../../Api/AdminApi/GetSubCategoryByCategoryId/" + groupId;

    xhr.open('GET', url, true);
    xhr.send();

    xhr.onreadystatechange = async function () {

        if (this.readyState == 4 && this.status == 200) {
            subCategory.innerHTML = null;
            if (this.response[0] != null) {

                this.response.forEach(element => {
                    const option = document.createElement("option");
                    option.innerHTML = element.text;
                    option.value = element.value;
                    subCategory.appendChild(option)
                });
            }
        }

    }
}