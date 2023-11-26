$(document).ready(function () {

    loadDataUsers();
    $('#btnAddUser').on('click', function () {
        addUser();
    });
    $('#btnEditUser').on('click', function () {
        editUser();
    });
    $('#btnSearchUser').on('click', function () {
        let valRole = $('#role-search').val();
        let valSearch = $('#example-search-input').val();
        loadDataUsers(valRole, valSearch)
    });
    $('#onAddUser').on('click', function () {
        clearModalFields();

        $('#addUserLabel').text(`Add user`)
        $('#addUser').modal('show');
        $('#btnEditUser').hide();
        $('#btnAddUser').show();
    });


});
function editUser() {
    if (!validateForm()) {
        return;
    }
    $('#validationMessage').hide();
    var employeeId = $('#employee_id').val();
    var firstName = $('#firstName').val();
    var lastName = $('#lastName').val();
    var email = $('#email').val();
    var dateOfBirth = $('#dateOfBirth').val();
    var sex = $('input[name="sex"]:checked').val();
    var status = $('input[name="status"]:checked').val();
    var password = $('#password').val();
    var selectedRole = $("#role").val();
    var image = $('#image')[0].files[0]; // Get the file object

    // Update FormData object
    var formData = new FormData();
    formData.append('EmployeeId', employeeId);
    formData.append('FirstName', firstName);
    formData.append('LastName', lastName);
    formData.append('Password', password);
    formData.append('Email', email);
    formData.append('DateOfBirth', dateOfBirth);
    formData.append('Sex', parseInt(sex));
    formData.append('Status', parseInt(status));
    formData.append('Role', parseInt(selectedRole));
    formData.append('image', image);

    // AJAX POST request
    $.ajax({
        type: 'POST',
        url: '/Users/EditUser', // Replace with your server endpoint
        data: formData,
        processData: false, // Important to prevent jQuery from automatically processing the data
        contentType: false, // Important for letting jQuery handle the contentType
        success: function (response) {
            if (response.status == 1) {
                toastr.success(response.mess, "Success");
                loadDataUsers();
                $('#addUser').modal('hide');
            } else if (response.status == 2) {
                toastr.error(response.mess, "Error");
            }else
            {
                toastr.error(response.mess, "Error");
            }
        },
        error: function (error) {
            toastr.error(error, "Error");
        }
    });


}

function onEditUser(employeeId) {
    $.ajax({
        url: '/Users/GetUserByEmployeeId',
        type: 'GET',
        data: { employeeId: employeeId },
        dataType: 'json',
        success: function (user) {
            clearModalFields();

            $('#btnAddUser').hide();
            $('#btnEditUser').show();
            $('#addUserLabel').text(`Update user :${user.FirstName}`)
            // Handle successful response
            console.log('User data:', user);

            $('#employee_id').val(user.EmployeeId);
            $('#firstName').val(user.FirstName);
            $('#lastName').val(user.LastName);
            $('#email').val(user.Email);
            $('#password').val(user.Password);
            $('#confirmPassword').val(user.Password);
            $('#role').val(user.Role);
            $('input[name="sex"][value="' + user.Sex + '"]').prop('checked', true);
            $('input[name="status"][value="' + user.Status + '"]').prop('checked', true);
            $('#selectedImage').attr('src', '/admin/assets/img/' + user.Image);
            $('#selectedImage').show();
            var formattedDate = user.DateOfBirth.split('T')[0];
            $('#dateOfBirth').val(formattedDate);
            $('#addUser').modal('show');


        },
        error: function (xhr, status, error) {
            // AJAX POST request
            $.ajax({
                type: 'POST',
                url: '/Users/AddUser', // Replace with your server endpoint
                data: formData,
                processData: false, // Important to prevent jQuery from automatically processing the data
                contentType: false, // Important for letting jQuery handle the contentType
                success: function (response) {
                    if (response.status == 1) {
                        toastr.success(response.mess, "Success");
                        loadDataUsers();
                        $('#addUser').modal('hide');
                    } else {
                        toastr.error(response.mess, "Error");
                    }
                },
                error: function (error) {
                    toastr.error(error, "Error");
                }
            });
        }
    });

}
function validateForm() {

    let isValid = true;
    $('#validationMessage').empty(); // Clear previous validation messages

    // Validation for first name
    const firstName = $('#firstName').val();
    if (!firstName.trim()) {
        $('#validationMessage').text('Please enter your first name.');
        isValid = false;
    }

    // Validation for last name
    const lastName = $('#lastName').val();
    if (!lastName.trim()) {
        $('#validationMessage').text('Please enter your last name.');
        isValid = false;
    }

    // Validation for email
     const email = $('#email').val();
     var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
     if (!emailRegex.test(email)) {
         $('#validationMessage').text('Please enter a valid email address.');
         isValid = false;
     }

    // // Validation for role selection
    // const role = $('#role').val();
    // if (!role) { // Assuming 'Admin' is the default value
    //     $('#validationMessage').text('Please select a role.');
    //     isValid = false;
    // }


    // Validation for password and confirm password
    const password = $('#password').val();
    const confirmPassword = $('#confirmPassword').val();
    if (password !== confirmPassword) {
        $('#validationMessage').text('Passwords do not match.');
        isValid = false;
    }


    // Validation for date of birth
    const dateOfBirth = $('#dateOfBirth').val();
    if (!dateOfBirth) {
        $('#validationMessage').text('Please enter your date of birth.');
        isValid = false;
    }

    // Validation for gender selection
    const gender = $('input[name="sex"]:checked').val();
    if (!gender) {
        $('#validationMessage').text('Please select your gender.');
        isValid = false;
    }

    // Validation for status selection
    const status = $('input[name="status"]:checked').val();
    if (!status) {
        $('#validationMessage').text('Please select a status.');
        isValid = false;
    }

    return isValid;
}
function onDeleteUser(id) {
    Swal.fire({
        title: 'Are you sure you want to delete this user??',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        denyButtonText: `No`,
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Users/DeleteUser',
                type: 'POST',
                data: { userId: id },
                success: function (response) {
                    if (response.status == 1) {
                        toastr.success(response.mess, "Success");
                        loadDataUsers();

                    } else {
                        toastr.error(response.mess, "Error");
                    }
                },
                error: function (xhr, status, error) {
                    toastr.error(error, "Error");
                }
            });
        }
    })

}

function showSelectedImage(input) {
    var file = input.files[0];
    if (file) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#selectedImage').attr('src', e.target.result);
            $('#selectedImage').show();
        }

        reader.readAsDataURL(file);
    }
}
function clearModalFields() {
    $('#validationMessage').empty(); // Clear any validation messages

    // Clear input values
    $('#image').val('');
    $('#selectedImage').attr('src', '').hide();
    $('#firstName').val('');
    $('#lastName').val('');
    $('#email').val('');
    $('#role').val('0');
    $('#password').val('');
    $('#confirmPassword').val('');
    $('#dateOfBirth').val('');
    $('input[name="sex"]').prop('checked', false);
    $('input[name="status"]').prop('checked', false);
}
function addUser() {
    if (!validateForm()) {
        return;
    }
    $('#validationMessage').hide();
    var firstName = $('#firstName').val();
    var lastName = $('#lastName').val();
    var email = $('#email').val();
    var dateOfBirth = $('#dateOfBirth').val();
    var sex = $('input[name="sex"]:checked').val();
    var status = $('input[name="status"]:checked').val();
    var password = $('#password').val();
    var confirmPassword = $('#confirmPassword').val();
    var selectedRole = $("#role").val();
    var image = $('#image')[0].files[0]; // Get the file object
    // Create FormData object
    var formData = new FormData();
    formData.append('FirstName', firstName);
    formData.append('LastName', lastName);
    formData.append('Password', password);
    formData.append('Email', email);
    formData.append('DateOfBirth', dateOfBirth);
    formData.append('Sex', parseInt(sex));
    formData.append('Status', parseInt(status));
    formData.append('Role', parseInt(selectedRole));
    formData.append('image', image);

    // AJAX POST request
    $.ajax({
        type: 'POST',
        url: '/Users/AddUser', // Replace with your server endpoint
        data: formData,
        processData: false, // Important to prevent jQuery from automatically processing the data
        contentType: false, // Important for letting jQuery handle the contentType
        success: function (response) {
            if (response.status == 1) {
                toastr.success(response.mess, "Success");
                loadDataUsers();
                $('#addUser').modal('hide');
            } else {
                toastr.error(response.mess, "Error");
            }
        },
        error: function (error) {
            toastr.error(error, "Error");
        }
    });

}
function loadDataUsers(role, searching) {
    $('#tblUsers').dataTable().fnDestroy();
    $('#tblUsers').DataTable({
        "ajax": {
            "url": "/Users/GetListUsers",
            "type": "GET",
            "dataType": "json",
            "dataSrc": "",
            "data": {
                "role": role,
                "searching": searching
            }
        },
        "searching": false,
        "columns": [
            {
                "data": null, "render": function (data, type, row) {
                    return `<img src="/admin/assets/img/${row.Image}" width="35px" />`;
                }
            },
            { "data": "FirstName" },
            { "data": "LastName" },
            { "data": "Email" },
            { "data": "DateOfBirth" },
            { "data": "Role" },
            { "data": "Sex" },
            { "data": "Status" },
            { "data": "CreateDate" },
            { "data": "UpdateDate" },
            {
                "data": null, "render": function (data, type, row) {
                    return `<button type="button" class="btn btn-warning" onclick="return onEditUser('${row.EmployeeId}')" ><i class="fas fa-edit"></i></i></button>
                                                                            <button type="button" class="btn btn-danger" onclick="return onDeleteUser('${row.EmployeeId}')" ><i class="fas fa-trash"></i></button>`;
                }
            },

        ]
    });
}