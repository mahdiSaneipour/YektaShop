function is_delete(id) {
    console.log(id);
    alert(id)
    event.preventDefault();
    swal({
        title: 'آیا مطمئن هستید؟',
        text: 'این داده برای همیشه حذف خواهد شد!',
        showCancelButton: true,
    }).then((willDelete) => {

        if (willDelete) {
            var xhr = new XMLHttpRequest();
            xhr.responseType = 'json';
            xhr.open('GET', '/Api/AdminApi/DeleteProductByProductId/' + id, true);
            xhr.send();

            xhr.onreadystatechange = async function () {

                if (this.readyState == 4 && this.status == 200) {

                    location.reload();

                }
            }

        }
    });
}