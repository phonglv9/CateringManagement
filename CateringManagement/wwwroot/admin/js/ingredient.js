$(document).ready(function () {
    loadDataIngredients();

    $('#add-btn').on('click', function () {
        showAddModal();
    });

    $('#save-btn').on('click', function () {
        addIngredient();
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
            {
                "data": null, "render": function (data, type, row) {
                    let html = ``;
                    if (row.Status == 0) {
                        html += `<span class="badge badge-secondary">Unavailable</span>`
                    }
                    else {
                        html += `<span class="badge badge-success">Unavailable</span>`
                    }
                    return html;
                }
            },
            {
                "data": null, "render": function (data, type, row) {
                    let html = `<button type="button" class="btn btn-warning m-1" title="Edit"><i class="fas fa-edit"></i></i></button>`;
                    html += `<button type="button" class="btn btn-danger m-1" title="Delete"><i class="fas fa-trash"></i></i></button>`
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