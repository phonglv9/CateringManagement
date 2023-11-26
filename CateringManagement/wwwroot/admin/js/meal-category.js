$(document).ready(function () {
    loadDataCategories();

    $('#add-btn').on('click', function () {
        showAddModal();
    });

    $('#save-btn').on('click', function () {
        addCategory();
    });

    $('#update-btn').on('click', function () {
        updateCategory();
    });
});

function loadDataCategories() {
    $('#categories-table').DataTable({
        "ajax": {
            "url": "/MealCategory/GetListMealCategories",
            "type": "GET",
            "dataType": "json",
            "dataSrc": ""
        },
        "columns": [
            { "data": "Name" },
            {
                "data": null, "render": function (data, type, row) {
                    let html = ``;
                    html += `<button type="button" class="btn btn-warning m-1" title="Edit" onclick="showEditModal('${row.Id}')"><i class="fas fa-edit"></i></i></button>`;
                    if (row.CanDelete == 1) {
                        html += `<button type="button" class="btn btn-danger m-1" title="Delete" onclick="showDeleteModal('${row.Id}')"><i class="fas fa-trash"></i></i></button>`;
                    }
                    
                    return html;
                }
            },

        ]
    });
}

function loadDataTable() {
    $('#categories-table').DataTable().ajax.reload();
}

function showAddModal() {
    $('#name').val('');
    $('#add-category-modal').modal('show');
}

function addCategory() {
    if ($('#name').val() == '') {
        toastr.error("Please enter name", "Error");
        return;
    }

    var dataObj = {
        Name: $('#name').val()
    };

    $.ajax({
        url: "/MealCategory/AddMealCategory",
        data: JSON.stringify(dataObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                toastr.success(result.mess, "Success");
                $('#add-category-modal').modal('hide');
                loadDataTable();
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function showEditModal(id) {
    $('#id-edit').val(id);
    $.ajax({
        url: "/MealCategory/GetDetail",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                $('#edit-category-modal').modal('show');
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function updateCategory(id) {
    if ($('#name-edit').val() == '') {
        toastr.error("Please enter name", "Error");
        return;
    }

    var dataObj = {
        Name: $('#name-edit').val()
    };

    $.ajax({
        url: "/MealCategory/UpdateMealCategory/" + $('#id-edit').val(),
        data: JSON.stringify(dataObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                toastr.success(result.mess, "Success");
                $('#edit-category-modal').modal('hide');
                loadDataTable();
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function showDeleteModal(id) {
    $('#id-delete').val(id);
    $('#delete-category-modal').modal('show');
}

function deleteCategory() {
    $.ajax({
        url: "/MealCategory/DeleteMealCategory/" + $('#id-delete').val(),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                toastr.success(result.mess, "Success");
                $('#delete-category-modal').modal('hide');
                loadDataTable();
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}