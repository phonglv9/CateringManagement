$(document).ready(function () {
    loadDataOrders();

    $('#add-btn').on('click', function () {
        showAddModal();
    });

    $('#save-btn').on('click', function () {
        addOrder();
    });

    $('#update-btn').on('click', function () {
        updateOrder();
    });
});

function loadDataOrders() {
    $('#orders-table').DataTable({
        "ajax": {
            "url": "/Order/GetListOrders",
            "type": "GET",
            "dataType": "json",
            "dataSrc": ""
        },
        "columns": [
            { "data": "Code" },
            { "data": "CreatedDate" },
            { "data": "PickupDate" },
            { "data": "CustomerName" },
            { "data": "CustomerPhone" },
            { "data": "Status" },
            {
                "data": null, "render": function (data, type, row) {
                    let html = ``;
                    html += `<button type="button" class="btn btn-primary m-1" title="View detail" onclick="showDetailModal('${row.Id}')"><i class="fas fa-eye"></i></i></button>`;
                    html += `<button type="button" class="btn btn-warning m-1" title="Edit" onclick="showEditModal('${row.Id}')"><i class="fas fa-edit"></i></i></button>`;
                    html += `<button type="button" class="btn btn-danger m-1" title="Delete" onclick="showDeleteModal('${row.Id}')"><i class="fas fa-trash"></i></i></button>`;
                    return html;
                }
            },

        ]
    });
}

function loadDataTable() {
    $('#orders-table').DataTable().ajax.reload();
}

function showAddModal() {
    $('#customer-name').val('');
    $('#customer-phone').val('');
    $('#pickup-date').val('');
    $('#order-price').val(0);
    $("#select-meal").val("0").change();
    $('#meal-price').val('');
    $('#quantity').val('');
    $('#table-row-count').val(0);
    $('#meals-table-body').html(`<tr id="meals-table-no-row"><td colspan="5"align="center">No order</td></tr>`);

    $.ajax({
        url: "/Order/GetSimpleListMeals",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                var htmlSelect = '<option value="0" selected>Select Meal</option>';
                $.each(result.data, function (key, item) {
                    htmlSelect += `<option value="${item.id}" id="meal-option-${item.id}" data-price="${item.price}">${item.name}</option>`;
                });
                $('#select-meal').html(htmlSelect);

                $('#add-order-modal').modal('show');
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function changeMeal(mode = '') {
    let selectedMeal = $('#select-meal' + mode).find(":selected");
    if (selectedMeal.val() == 0) {
        $('#unit-group-label' + mode).html('---');
    }
    $('#unit-group-label' + mode).html(selectedMeal.attr('data-unit'));
}


function addMealToTable() {

}