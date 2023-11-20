window.addEventListener('DOMContentLoaded', event => {

    // Toggle the side navigation
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        // Uncomment Below to persist sidebar toggle between refreshes
        // if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
        //     document.body.classList.toggle('sb-sidenav-toggled');
        // }
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
        });
    }

});
//Hiển thị sweet alert lỗi
function MessageError(text) {
    Swal.fire({
        title: "Thông báo",
        icon: 'error',
        text: text,
        button: false,
        //timer: 2000
    });
}

//Hiển thị sweet alert thành công
function MessageSucces(text) {
    Swal.fire({
        title: 'Thông báo',
        text: text,
        button: false,
        icon: 'success',
        timer: 1000
    });
}

//Hiển thị sweet alert cảnh báo
function MessageWarning(text, timer) {
    Swal.fire({
        title: 'Thông báo',
        text: text,
        button: false,
        icon: 'warning',
        timer: timer
    });
}