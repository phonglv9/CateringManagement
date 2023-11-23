$(document).ready(function () {
    loadDataMeals();

    $('#add-btn').on('click', function () {
        showAddModal();
    });

    $('#save-btn').on('click', function () {
        addMeal();
    });
});

function loadDataMeals() {
    $('#meals-table').DataTable({
        "ajax": {
            "url": "/Meal/GetListMeals",
            "type": "GET",
            "dataType": "json",
            "dataSrc": ""
        },
        "columns": [
            { "data": "Name" },
            { "data": "Price" },
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
    $('#meals-table').DataTable().ajax.reload();
}

function showAddModal() {
    $('#name').val('');
    $('#unit').val('');
    $('#price').val(0);
    $("#select-ingredient").val("0").change();
    $('#quantity').val('');
    $('#table-row-count').val(0);
    $('#meals-table-body').html(`<tr id="meals-table-no-row"><td colspan="6"align="center">No ingredient</td></tr>`);

    $.ajax({
        url: "/Meal/GetSimpleListIngredients",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                var htmlSelect = '<option value="0" selected>Select Ingredient</option>';
                $.each(result.data, function (key, item) {
                    htmlSelect += `<option value="${item.id}" id="ingredient-option-${item.id}" data-unit="${item.unit}" data-unit-price="${item.unitPrice}">${item.name}</option>`;
                });
                $('#select-ingredient').html(htmlSelect);

                $('#add-meal-modal').modal('show');
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function changeIngredient() {
    let selectedIngredient = $('#select-ingredient').find(":selected");
    if (selectedIngredient.val() == 0) {
        $('#unit-group-label').html('---');
    }
    $('#unit-group-label').html(selectedIngredient.attr('data-unit'));
}

function addIngredientToTable() {
    let selectedIngredient = $('#select-ingredient').find(":selected");
    let ingredientId = selectedIngredient.val();
    let ingredientName = selectedIngredient.text();
    let ingredientUnit = selectedIngredient.attr("data-unit");
    let ingredientUnitPrice = selectedIngredient.attr("data-unit-price");
    let quantity = $('#quantity').val();
    let rowPrice = parseFloat(ingredientUnitPrice * quantity);
    
    if (ingredientId == 0 || quantity == '' || quantity <= 0) {
        return;
    }

    // check no row
    let mealPrice = parseFloat($('#price').val());
    let rowCount = parseInt($('#table-row-count').val());
    if (rowCount == 0) {
        $('#meals-table-no-row').remove();
    }
    $('#table-row-count').val(rowCount + 1);

    // increase price
    $('#price').val(mealPrice + rowPrice);

    let htmlRow = `<tr id="ingredient-row-${ingredientId}" data-row-id="${ingredientId}" data-row-quantity="${quantity}">`;
    htmlRow += `<td>${ingredientName}</td>`;
    htmlRow += `<td>${quantity}</td>`;
    htmlRow += `<td>${ingredientUnit}</td>`;
    htmlRow += `<td>${ingredientUnitPrice}</td>`;
    htmlRow += `<td>${rowPrice}</td>`;
    htmlRow += `<td><button type="button" class="btn btn-danger" title="Delete" onclick="removeIngredientFromTable('${ingredientId}', ${rowPrice})"><i class="fas fa-trash"></i></i></button></td>`;
    htmlRow += `</tr>`;

    // add to table
    $('#meals-table-body').append(htmlRow);

    // disable selected option of select
    $('#ingredient-option-' + ingredientId).attr('disabled', 'disabled');

    // reset select
    $("#select-ingredient").val("0").change();
    $('#quantity').val('');
}

function removeIngredientFromTable(id, removedPrice) {
    // remove row
    $('#ingredient-row-' + id).remove();

    // minus counter
    let rowCount = parseInt($('#table-row-count').val());
    $('#table-row-count').val(--rowCount);

    // increase price
    let mealPrice = parseFloat($('#price').val());
    $('#price').val(mealPrice - removedPrice);

    // check no row
    if (rowCount == 0) {
        $('#meals-table-body').html(`<tr id="meals-table-no-row"><td colspan="6"align="center">No ingredient</td></tr>`);
    }

    // enable selected option of select
    $('#ingredient-option-' + id).removeAttr('disabled');
}

function addMeal() {
    var ingredientArr = [];

    $('#meals-table-body tr').each(function (index, value) {
        ingredientArr.push({
            IngredientId: $(this).attr('data-row-id'),
            Quantity: parseInt($(this).attr('data-row-quantity'))
        });
    });
    console.log(ingredientArr);

    var dataObj = {
        Name: $('#name').val(),
        Ingredients: ingredientArr
    };

    $.ajax({
        url: "/Meal/AddMeal",
        data: JSON.stringify(dataObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                toastr.success(result.mess, "Success");
                $('#add-meal-modal').modal('hide');
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

function showDetailModal(id) {
    //$('#name').val('');
    //$('#unit').val('');
    //$('#price').val(0);
    //$("#select-ingredient").val("0").change();
    //$('#quantity').val('');
    //$('#table-row-count').val(0);
    //$('#meals-table-body').html(`<tr id="meals-table-no-row"><td colspan="6"align="center">No ingredient</td></tr>`);

    $.ajax({
        url: "/Meal/GetDetail/" + id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                $('#name-detail').val(result.data.name);
                $('#price-detail').val(result.data.price);

                let htmlRow = ``;
                let ingredients = result.data.ingredients;
                $.each(ingredients, function (index, item) {
                    htmlRow += `<tr>`;
                    htmlRow += `<td>${item.name}</td>`;
                    htmlRow += `<td>${item.quantity}</td>`;
                    htmlRow += `<td>${item.unit}</td>`;
                    htmlRow += `<td>${item.unitPrice}</td>`;
                    htmlRow += `<td>${item.totalPrice}</td>`;
                    htmlRow += `</tr>`;
                });

                $('#detail-meals-table-body').html(htmlRow);
                //var htmlSelect = '<option value="0" selected>Select Ingredient</option>';
                //$.each(result.data, function (key, item) {
                //    htmlSelect += `<option value="${item.id}" id="ingredient-option-${item.id}" data-unit="${item.unit}" data-unit-price="${item.unitPrice}">${item.name}</option>`;
                //});
                //$('#select-ingredient').html(htmlSelect);

                $('#detail-meal-modal').modal('show');
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
        url: "/Meal/GetMealDetail/" + id,
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
                $('#edit-Meal-modal').modal('show');
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function updateMeal() {
    var dataObj = {
        Id: $('#id-edit').val(),
        Name: $('#name-edit').val(),
    };

    $.ajax({
        url: "/Meal/UpdateMeal",
        data: JSON.stringify(dataObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                toastr.success(result.mess, "Success");
                $('#edit-Meal-modal').modal('hide');
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
        url: "/Meal/GetMealDetail/" + id,
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
                $('#delete-Meal-modal').modal('show');
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function deleteMeal() {
    $.ajax({
        url: "/Meal/DeleteMeal/" + $('#id-delete').val(),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                toastr.success(result.mess, "Success");
                $('#delete-Meal-modal').modal('hide');
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
        url: "/Meal/GetSimpleListMeals",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                var htmlSelect = '<option selected>Select Meal</option>';
                $.each(result.data, function (key, item) {
                    htmlSelect += `<option value="${item.id}">${item.name}</option>`;
                });
                $('#select-Meal').html(htmlSelect);
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

$('#select-Meal').on('change', function () {
    var id = this.value;
    $('#selected-Meal-import').val(id);
    calculateTotalPriceForImport(id, $('#quantity').val());
});

$('#quantity').on('change', function () {
    console.log($('#selected-Meal-import').val());
    calculateTotalPriceForImport($('#selected-Meal-import').val(), $('#quantity').val());
});

function changeQuantity() {
    calculateTotalPriceForImport($('#selected-Meal-import').val(), $('#quantity').val());
}

function calculateTotalPriceForImport(id, quantity) {
    if (id == '') {
        return;
    }

    $.ajax({
        url: "/Meal/GetTotalPriceForImport",
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
    if ($('#selected-Meal-import').val() == '') {
        return;
    }

    var dataObj = {
        MealId: $('#selected-Meal-import').val(),
        Quantity: parseInt($('#quantity').val()),
        TotalPrice: parseInt($('#total-price').val()),
    };

    $.ajax({
        url: "/Meal/ImportMealToStorage",
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