const tabs = document.querySelectorAll(".nav-link");

tabs.forEach((element, index) => {
    element.addEventListener("click", function () {

        var url = "OtherOrders/" + element.id;

        $.ajax({
            type: "GET", //Method type
            url: url, //url to load partial view
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $('#tab-content').html(response);
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    })
});
