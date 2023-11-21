$(document).ready(function () {

    loadDataUsers();
    $('#onAddUser').on('click', function () {
        addUser();
    });

});

function addUser() {
    var firstName = $('#firstName').val();
    var lastName = $('#lastName').val();
    var email = $('#email').val();
    var dateOfBirth = $('#dateOfBirth').val();
    var sex = $('input[name="sex"]:checked').val();
    var status = $('input[name="status"]:checked').val();
    var password = $('#password').val();
    var confirmPassword = $('#confirmPassword').val();
    var selectedRole = $("input[name='role']:checked").val();
    var image = $('#image')[0].files[0]; // Get the file object

    // Reset validation messages
    $('#validationMessage').hide();
    $('.validation-error').text('');

    // Kiểm tra điều kiện validate
    if (firstName === '' || lastName === '' || email === '' || dateOfBirth === '' || sex === undefined || status === undefined || image === '') {
        $('#validationMessage').text('Please fill in all required information.');
        $('#validationMessage').show();
    } else {
        $('#validationMessage').hide();
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
        console.log(formData);
        // AJAX POST request
        $.ajax({
            type: 'POST',
            url: '/Users/AddUser', // Replace with your server endpoint
            data: formData,
            processData: false, // Important to prevent jQuery from automatically processing the data
            contentType: false, // Important for letting jQuery handle the contentType
            success: function (response) {
                if (response.status == 1) {

                    MessageSucces(response.mess);
                } else {
                    MessageError(response.mess);
                }
            },
            error: function (error) {
                MessageError(error);
            }
        });
        
    }
}
function loadDataUsers() {
    $('#tblUsers').DataTable({
        "ajax": {
            "url": "/Users/GetListUsers",
            "type": "GET",
            "dataType": "json",
            "dataSrc": ""
        },
        "columns": [
            { "data": "Image"},
            { "data": "FirstName" },
            { "data": "LastName" },
            { "data": "Email" },
            { "data": "DateOfBirth" },
            { "data": "Sex" },
            { "data": "Status" },
            { "data": "CreateDate" },
            { "data": "UpdateDate" },
            {
                "data": null, "render": function (data, type, row) {
                    return `<button type="button" class="btn btn-warning" onclick="return onEditUser('${row.EmployeeId}')" ><i class="fas fa-edit"></i></i></button>
                                <button type="button" class="btn btn-danger" onclick="return onDeleteUser('${row.EmployeeId}')" ><i class="fas fa-sync"></i></button>`;
                }
            },

        ]
    });
}