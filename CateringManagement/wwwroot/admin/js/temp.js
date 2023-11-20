$(document).ready(function () {
    $('#file').change(function () {
        var input = this;

        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#previewImage').attr('src', e.target.result);
                $('#previewImage').show();
            };
            reader.readAsDataURL(input.files[0]);
        } else {
            $('#previewImage').attr('src', '#'); // Xóa hình ảnh nếu không có tệp nào được chọn
            $('#previewImage').hide();
        }
    });
    $('.data_sinhvien').chosen();
    $('.data_monhoc').chosen();
});

function deleteSinhVien(id) {
    Swal.fire({
        title: 'Bạn có chắc muốn xóa sinh viên này không  ?',
        showCancelButton: true,
        confirmButtonText: 'Có',
        denyButtonText: `Không`,
    }).then((result) => {

        if (result.isConfirmed) {
            var data = {
                masv: id,

            };
            // Make an AJAX POST request to the server
            $.ajax({
                type: "POST",
                url: "/SinhVien/Delete", // Replace with the actual controller and action URL
                data: data,
                dataType: "json",
                success: function (result) {
                    if (result === 1) {
                      /*  MessageSucces("Xóa thành công");*/
                        location.reload();
                    } else {
                        MessageError("Xóa thất bại");
                    }
                }
            });

        }
    })
}
function UpdateDiem() {

    var diem = $("#diem_update").val();
    var madiem = $("#madiem_update").val();

    // Kiểm tra nếu điểm không hợp lệ
    if (diem < 1 || diem > 10) {
        $("#messUpdateDiem").text("Điểm phải nằm trong khoảng từ 1 đến 10");
    } else {
        // Xóa thông báo lỗi nếu tất cả đều hợp lệ
        $("#messUpdateDiem").text("");

        var formData = {
            madiem: madiem,
            diemthi: diem,
        };
        $.ajax({
            type: "POST",
            url: "/Diem/UpdateDiem",
            data: formData,
            success: function (result) {
                if (result == 1) {
                    MessageSucces('Sửa điểm thành công');
                    loadData();
                    $('#modalUpdateDiem').modal('hide')
                } else {
                    MessageError('Sửa điểm thất bại')
                }
            }
        });




    }
}
function deleteDiem(id) {
    Swal.fire({
        title: 'Bạn có chắc muốn xóa điểm này ?',
        showCancelButton: true,
        confirmButtonText: 'Có',
        denyButtonText: `Không`,
    }).then((result) => {

        if (result.isConfirmed) {
            var data = {
                madiem: id,

            };
            // Make an AJAX POST request to the server
            $.ajax({
                type: "POST",
                url: "/Diem/DeleteDiem", // Replace with the actual controller and action URL
                data: data,
                dataType: "json",
                success: function (result) {
                    if (result === 1) {
                        MessageSucces("Xóa thành công");
                        loadData();
                    } else {
                        MessageError("Xóa thất bại");
                    }
                }
            });

        }
    })

}