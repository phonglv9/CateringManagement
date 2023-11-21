$(document).ready(function () {

    loadDataUsers();

});
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
            {
                "data": null, "render": function (data, type, row) {
                    return `<td>${row.Status == 1}</td>`;
                }
            },
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