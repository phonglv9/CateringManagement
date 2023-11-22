$(document).ready(function () {
    loadDataIngredients();

    $('#add-btn').on('click', function () {
        showAddModal();
    });

    $('#save-btn').on('click', function () {
        addIngredient();
    });

    $('#add-quantity-btn').on('click', function () {
        showAddQuantityModal();
    });

    $('#import-ingredient-btn').on('click', function () {
        addQuantity();
    });
});

function loadDataIngredients() {
    $('#ingredients-table').DataTable({
        "ajax": {
            "url": "/Ingredient/GetListIngredients",
            "type": "GET",
            "dataType": "json",
            "dataSrc": ""
        },
        "columns": [
            { "data": "Name" },
            { "data": "Unit" },
            { "data": "UnitPrice" },
            { "data": "Quantity" },
            { "data": "Price" },
            { "data": "Status" },
            {
                "data": null, "render": function (data, type, row) {
                    let html = `<button type="button" class="btn btn-warning m-1" title="Edit" onclick="showEditModal('${row.Id}')"><i class="fas fa-edit"></i></i></button>`;
                    html += `<button type="button" class="btn btn-danger m-1" title="Delete" onclick="showDeleteModal('${row.Id}')"><i class="fas fa-trash"></i></i></button>`
                    return html;
                }
            },

        ]
    });
}

function loadDataTable() {
    $('#ingredients-table').DataTable().ajax.reload();
}

function showAddModal() {
    $('#name').val('');
    $('#unit').val('');
    $('#unit-price').val('');
    $('#add-ingredient-modal').modal('show');
}

function addIngredient() {
    var dataObj = {
        Name: $('#name').val(),
        Unit: parseInt($('#unit').val()),
        PriceUnit: parseFloat($('#unit-price').val())
    };

    $.ajax({
        url: "/Ingredient/AddIngredient",
        data: JSON.stringify(dataObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                toastr.success(result.mess, "Success");
                $('#add-ingredient-modal').modal('hide');
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
    $.ajax({
        url: "/Ingredient/GetIngredientDetail/" + id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                $('#id-edit').val(id);
                $('#name-edit').val(result.data.name);
                $('#unit-edit').val(result.data.unit);
                $('#unit-price-edit').val(result.data.unitPrice);
                $('#edit-ingredient-modal').modal('show');
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function updateIngredient() {
    var dataObj = {
        Id: $('#id-edit').val(),
        Name: $('#name-edit').val(),
    };

    $.ajax({
        url: "/Ingredient/UpdateIngredient",
        data: JSON.stringify(dataObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                toastr.success(result.mess, "Success");
                $('#edit-ingredient-modal').modal('hide');
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
    $.ajax({
        url: "/Ingredient/GetIngredientDetail/" + id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                $('#id-delete').val(id);
                $('#name-delete').html(result.data.name);
                $('#unit-delete').html(result.data.unit);
                $('#unit-price-delete').html(result.data.unitPrice);
                $('#delete-ingredient-modal').modal('show');
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function deleteIngredient() {
    $.ajax({
        url: "/Ingredient/DeleteIngredient/" + $('#id-delete').val(),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                toastr.success(result.mess, "Success");
                $('#delete-ingredient-modal').modal('hide');
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

function showAddQuantityModal(id) {
    $.ajax({
        url: "/Ingredient/GetSimpleListIngredients",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                var htmlSelect = '<option selected>Select ingredient</option>';
                $.each(result.data, function (key, item) {
                    htmlSelect += `<option value="${item.id}">${item.name}</option>`;
                });
                $('#select-ingredient').html(htmlSelect);
                $('#quantity').val(0);
                $('#add-quantity-modal').modal('show');
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

$('#select-ingredient').on('change', function () {
    var id = this.value;
    $('#selected-ingredient-import').val(id);
    calculateTotalPriceForImport(id, $('#quantity').val());
});

$('#quantity').on('change', function () {
    console.log($('#selected-ingredient-import').val());
    calculateTotalPriceForImport($('#selected-ingredient-import').val(), $('#quantity').val());
});

function changeQuantity() {
    calculateTotalPriceForImport($('#selected-ingredient-import').val(), $('#quantity').val());
}

function calculateTotalPriceForImport(id, quantity) {
    if (id == '') {
        return;
    }

    $.ajax({
        url: "/Ingredient/GetTotalPriceForImport",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: { id: id, quantity: quantity },
        success: function (result) {
            if (result.status == 1) {
                $('#total-price').val(result.data);
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function addQuantity() {
    if ($('#selected-ingredient-import').val() == '') {
        return;
    }

    var dataObj = {
        IngredientId: $('#selected-ingredient-import').val(),
        Quantity: parseInt($('#quantity').val()),
        TotalPrice: parseInt($('#total-price').val()),
    };

    $.ajax({
        url: "/Ingredient/ImportIngredientToStorage",
        data: JSON.stringify(dataObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                toastr.success(result.mess, "Success");
                $('#add-quantity-modal').modal('hide');
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