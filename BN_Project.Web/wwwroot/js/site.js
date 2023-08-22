function is_delete(e) {
    event.preventDefault();
    Swal.fire({
        title: 'آیا مطمئن هستید؟',
        text: "آیا از انجام این عملیات مطمئن هستید؟",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'مطمئنم',
        cancelButtonText: 'منصرف شدم'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location = e.href;
            Swal.fire(
                'انجام شد!',
                'عملیات با موفقیت به پایان رسید.',
                'success'
            )
        }
    })
}

function ShowAlert() {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: 'Something went wrong!',
        footer: '<a href="">Why do I have this issue?</a>'
    })
}