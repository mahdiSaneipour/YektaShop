function is_delete(e) {
    event.preventDefault();
    swal({
        title: 'آیا مطمئن هستید؟',
        text: 'این داده برای همیشه حذف خواهد شد!',
        confirmButtonText: "تایید",
        cancelButtonText: "لغو",
        showCancelButton: true,
    }).then((willDelete) => {
        console.log(willDelete);
        if (willDelete) {

            window.location = e.href;
        }
    });
}