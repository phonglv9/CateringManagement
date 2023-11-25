$(document).ready(function () {
    loadDataMeals();

    $('#add-btn').on('click', function () {
        showAddModal();
    });

    $('#save-btn').on('click', function () {
        addMeal();
    });

    $('#update-btn').on('click', function () {
        updateMeal();
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
            { "data": "Category" },
            { "data": "Description" },
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
    $("#category").val("0").change();
    $("#description").val('');
    $('#quantity').val('');
    $('#table-row-count').val(0);
    $('#meals-table-body').html(`<tr id="meals-table-no-row"><td colspan="6"align="center">No ingredient</td></tr>`);

    getCategorySelect();

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

function getCategorySelect(mode = '', selectedValue = '0') {
    $.ajax({
        url: "/Meal/GetListCategories",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                var htmlSelect = '<option value="0" selected>Select Category</option>';
                $.each(result.data, function (key, item) {
                    htmlSelect += `<option value="${item.id}" id="${item.id}">${item.name}</option>`;
                });
                $('#category' + mode).html(htmlSelect);
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function changeIngredient(mode = '') {
    let selectedIngredient = $('#select-ingredient' + mode).find(":selected");
    if (selectedIngredient.val() == 0) {
        $('#unit-group-label' + mode).html('---');
    }
    else {
        $('#unit-group-label' + mode).html(selectedIngredient.attr('data-unit'));
    }
}

function addIngredientToTable(mode = '') {
    let selectedIngredient = $('#select-ingredient' + mode).find(":selected");
    let ingredientId = selectedIngredient.val();
    let ingredientName = selectedIngredient.text();
    let ingredientUnit = selectedIngredient.attr("data-unit");
    let ingredientUnitPrice = parseFloat(selectedIngredient.attr("data-unit-price"));

    if (ingredientId == 0 || $('#quantity' + mode).val() == '' || $('#quantity' + mode).val() <= 0) {
        return;
    }

    let quantity = parseInt($('#quantity' + mode).val());
    let rowPrice = parseFloat(ingredientUnitPrice * quantity).toFixed(2);

    // check no row
    let mealPrice = parseFloat($('#price' + mode).val());
    let rowCount = parseInt($('#table-row-count' + mode).val());
    if (rowCount == 0) {
        $('#meals-table-no-row' + mode).remove();
    }
    $('#table-row-count' + mode).val(rowCount + 1);

    // increase price
    $('#price' + mode).val(mealPrice + rowPrice);

    let htmlRow = `<tr id="ingredient-row${mode}-${ingredientId}" data-row-id="${ingredientId}" class="has-data" data-row-quantity="${quantity}">`;
    htmlRow += `<td>${ingredientName}</td>`;
    htmlRow += `<td>${quantity}</td>`;
    htmlRow += `<td>${ingredientUnit}</td>`;
    htmlRow += `<td>${ingredientUnitPrice}</td>`;
    htmlRow += `<td>${rowPrice}</td>`;
    htmlRow += `<td><button type="button" class="btn btn-danger" title="Delete" onclick="removeIngredientFromTable('${ingredientId}', ${rowPrice}, '${mode}')"><i class="fas fa-trash"></i></i></button></td>`;
    htmlRow += `</tr>`;

    // add to table
    $('#meals-table-body' + mode).append(htmlRow);

    // disable selected option of select
    $(`#ingredient-option${mode}-` + ingredientId).attr('disabled', 'disabled');

    // reset select
    $("#select-ingredient" + mode).val("0").change();
    $('#quantity' + mode).val('');
}

function removeIngredientFromTable(id, removedPrice, mode = '') {
    // remove row
    $(`#ingredient-row${mode}-` + id).remove();

    // minus counter
    let rowCount = parseInt($('#table-row-count' + mode).val());
    $('#table-row-count' + mode).val(--rowCount);

    // decrease price
    let mealPrice = parseFloat($('#price' + mode).val());
    $('#price' + mode).val(mealPrice - removedPrice);

    // check no row
    if (rowCount == 0) {
        $('#meals-table-body' + mode).html(`<tr id="meals-table-no-row${mode}"><td colspan="6"align="center">No ingredient</td></tr>`);
    }

    // enable selected option of select
    $(`#ingredient-option${mode}-` + id).removeAttr('disabled');
}

function addMeal() {
    var ingredientArr = [];
    
    if ($('#name').val() == '') {
        toastr.error("Please enter name", "Error");
        return;
    }
    var categoryId = $('#category').find(":selected").val();
    if (categoryId == '0') {
        toastr.error("Please select category", "Error");
        return;
    }

    $('#meals-table-body tr.has-data').each(function (index, value) {
        ingredientArr.push({
            IngredientId: $(this).attr('data-row-id'),
            Quantity: parseInt($(this).attr('data-row-quantity'))
        });
    });

    if (ingredientArr.length == 0) {
        toastr.error("Please add the ingredient", "Error");
        return;
    }

    var dataObj = {
        Name: $('#name').val(),
        CategoryId: categoryId,
        Description: $('#description').val(),
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

function getDetailForEditModal(id) {
    //$('#name').val('');
    //$('#unit').val('');
    //$('#price').val(0);
    //$("#select-ingredient-edit").val("0").change();
    //$('#quantity-edit').val('');
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
                $('#name-edit').val(result.data.name);
                $('#price-edit').val(result.data.price);

                let htmlRow = ``;
                let ingredients = result.data.ingredients;
                $.each(ingredients, function (index, item) {
                    htmlRow += `<tr id="ingredient-row-edit-${item.ingredientId}" data-row-id="${item.ingredientId}" class="has-data" data-row-quantity="${item.quantity}">`;
                    htmlRow += `<td>${item.name}</td>`;
                    htmlRow += `<td>${item.quantity}</td>`;
                    htmlRow += `<td>${item.unit}</td>`;
                    htmlRow += `<td>${item.unitPrice}</td>`;
                    htmlRow += `<td>${item.totalPrice}</td>`;
                    htmlRow += `<td><button type="button" class="btn btn-danger" title="Delete" onclick="removeIngredientFromTable('${item.ingredientId}', ${item.totalPrice}, '-edit')"><i class="fas fa-trash"></i></i></button></td>`;
                    htmlRow += `</tr>`;

                    // disable selected option of select
                    $('#ingredient-option-edit-' + item.ingredientId).attr('disabled', 'disabled');
                });

                $('#table-row-count-edit').val(ingredients.length);
                $('#meals-table-body-edit').html(htmlRow);
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
    //$('#name-edit').val('');
    //$('#unit-edit').val('');
    //$('#price-edit').val(0);
    $("#select-ingredient-edit").val("0").change();
    $('#quantity-edit').val('');
    $('#id-edit').val(id);
    //$('#table-row-count-edit').val(0);
    //$('#meals-table-body-edit').html(`<tr id="meals-table-no-row-edit"><td colspan="6"align="center">No ingredient</td></tr>`);

    getDetailForEditModal(id);

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
                    htmlSelect += `<option value="${item.id}" id="ingredient-option-edit-${item.id}" data-unit="${item.unit}" data-unit-price="${item.unitPrice}">${item.name}</option>`;
                });
                $('#select-ingredient-edit').html(htmlSelect);

                $('#edit-meal-modal').modal('show');
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function updateMeal(id) {
    var ingredientArr = [];

    $('#meals-table-body-edit tr.has-data').each(function (index, value) {
        ingredientArr.push({
            IngredientId: $(this).attr('data-row-id'),
            Quantity: parseInt($(this).attr('data-row-quantity'))
        });
    });

    if (ingredientArr.length == 0) {
        toastr.error("Please add the ingredient", "Error");
        return;
    }

    if ($('#name-edit').val() == '') {
        toastr.error("Please enter name", "Error");
        return;
    }

    var dataObj = {
        Name: $('#name-edit').val(),
        Ingredients: ingredientArr
    };

    $.ajax({
        url: "/Meal/UpdateMeal/" + $('#id-edit').val(),
        data: JSON.stringify(dataObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                toastr.success(result.mess, "Success");
                $('#edit-meal-modal').modal('hide');
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
    $('#delete-meal-modal').modal('show');
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
                $('#delete-meal-modal').modal('hide');
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