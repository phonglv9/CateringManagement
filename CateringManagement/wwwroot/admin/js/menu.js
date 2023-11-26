$(document).ready(function () {
    loadDataMeals();
    loadDataCategories();
    loadCartInfo();
});

function loadDataMeals(categoryId = null) {
    $.ajax({
        url: "/Menu/GetListMeals",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: { categoryId: categoryId },
        success: function (result) {
            if (result.status == 1) {
                var html = '';
                $.each(result.data, function (key, item) {
                    html += `<div class="card-grid-style-3">`;
                    html += `    <div class="card-grid-inner">`;
                    html += `        <div class="image-box" data-bs-toggle="tooltip" data-bs-placement="right" title="${item.description}"><a href="javascript:void(0)"><img src="${item.image}" alt="meal"></a></div>`;
                    html += `        <div class="info-right">`;
                    html += `            <a class="font-xs color-gray-500" href="javascript:void(0)">${item.category}</a><br><a class="color-brand-3 font-sm-bold" href="javascript:void(0)">${item.name}</a>`;
                    html += `            <div class="price-info"><strong class="font-lg-bold color-brand-3 price-main">$${item.price}</strong></div>`;
                    html += `            <div class="mt-20 box-btn-cart"><a class="btn btn-cart" href="javascript:void(0)" onclick="addToCart('${item.id}', '${item.name}', '${item.category}', ${item.price}, '${item.image}')">Add To Cart</a></div>`;
                    html += `        </div>`;
                    html += `    </div>`;
                    html += `</div>`;
                });
                $('#meal-list').html(html);
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

function loadDataCategories(categoryId = null) {
    $.ajax({
        url: "/Menu/GetListCategories",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: { categoryId: categoryId },
        success: function (result) {
            if (result.status == 1) {
                var html = '<li><a href="javascript:void(0)" onclick="loadDataMeals()">All<span class="number" id="total-number">*</span></a></li>';
                var totalNum = 0;
                $.each(result.data, function (key, item) {
                    totalNum += parseInt(item.mealNumber);
                    html += `<li><a href="javascript:void(0)" onclick="loadDataMeals('${item.id}')">${item.name}<span class="number">${item.mealNumber}</span></a></li>`;
                });
                $('#category-list').html(html);
                $('#total-number').html(totalNum);
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}

var storedCartList = JSON.parse(localStorage.getItem('cartList')) || [];

function loadCartInfo() {
    var count = parseInt(0);
    $.each(storedCartList, function (key, item) {
        count += parseInt(item.Quantity);
    });
    $('#cart-info').html(`(${count})`);
}

function saveCartList() {
    localStorage.setItem('cartList', JSON.stringify(storedCartList));
}

function addToCart(id, name, category, price, image) {
    var existItem = storedCartList.find(function (item) {
        return item.Id === id
    });

    if (existItem) {
        existItem.Quantity = parseInt(existItem.Quantity) + 1;
    }
    else {
        var meal = {
            Id: id,
            Name: name,
            Category: category,
            Price: price,
            Image: image,
            Quantity: 1
        }
        storedCartList.push(meal);
    }
    saveCartList();
    loadCartInfo();
    toastr.success("Added " + name, "Add to cart");
}

function clearCart() {
    storedCartList = [];
    saveCartList();
    loadCartInfo();

    let htmlRow = `<tr id="orders-table-no-row"><td colspan="7" align="center">No meal</td></tr>`;
    $('#orders-table-body').html(htmlRow);
    $('#total-price').html(0);
}

function clearCustomerInfo() {
    $('#customer-name').val('');
    $('#customer-phone').val('');
    $('#pickup-date').val('');
}

function showCartMoal() {
    loadCartList();
    $('#cart-modal').modal('show');
}

function loadCartList() {
    let htmlRow = '';
    var totaCartlPrice = parseFloat(0);
    if (storedCartList.length > 0) {
        $.each(storedCartList, function (key, item) {
            let totalPrice = parseFloat(item.Price * item.Quantity).toFixed(2);
            htmlRow += `<tr id="meal-row-${item.Id}" data-row-id="${item.Id}" class="has-data" data-row-quantity="${item.Quantity}">`;
            htmlRow += `<td><img src="${item.Image}" style="max-width:100px; max-height:100px" alt="meal"></a></td>`;
            htmlRow += `<td>${item.Name}</td>`;
            htmlRow += `<td>${item.Category}</td>`;
            htmlRow += `<td>${item.Price}</td>`;
            htmlRow += `<td>${item.Quantity}</td>`;;
            htmlRow += `<td>${totalPrice}</td>`;
            htmlRow += `<td><button type="button" class="btn btn-danger" title="Delete" onclick="removeMealFromCart('${item.Id}')"><i class="fas fa-trash"></i></i></button></td>`;
            htmlRow += `</tr>`;

            totaCartlPrice += parseFloat(totalPrice);
        });
    }
    else {
        htmlRow += `<tr id="orders-table-no-row"><td colspan="7" align="center">No meal</td></tr>`;
    }

    $('#orders-table-body').html(htmlRow);
    $('#total-price').html(totaCartlPrice.toFixed(2));
}

function removeMealFromCart(id) {
    storedCartList = storedCartList.filter(function (item) {
        return item.Id !== id;
    });

    saveCartList();
    loadCartInfo();
    loadCartList();
}

function saveOrder() {
    var mealArr = [];

    if ($('#customer-name').val() == '') {
        toastr.error("Please enter customer name", "Error");
        return;
    }
    if ($('#customer-phone').val() == '') {
        toastr.error("Please enter customer phone", "Error");
        return;
    }
    if ($('#pickup-date').val() == '') {
        toastr.error("Please enter pickup date", "Error");
        return;
    }

    $('#orders-table-body tr.has-data').each(function (index, value) {
        mealArr.push({
            MealId: $(this).attr('data-row-id'),
            Quantity: parseInt($(this).attr('data-row-quantity'))
        });
    });

    if (mealArr.length == 0) {
        toastr.error("Please add the meal", "Error");
        return;
    }

    var dataObj = {
        CustomerName: $('#customer-name').val(),
        CustomerPhone: $('#customer-phone').val(),
        PickupDate: $('#pickup-date').val(),
        Meals: mealArr
    };

    $.ajax({
        url: "/Order/AddOrder",
        data: JSON.stringify(dataObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: true,
        processData: false,
        success: function (result) {
            if (result.status == 1) {
                toastr.success(result.mess, "Success");
                clearCustomerInfo();
                clearCart();
                $('#cart-modal').modal('hide');
            } else {
                toastr.error(result.mess, "Error");
            }
        },
        error: function (errormessage) {
            toastr.error(errormessage.responseText, "Error");
        }
    });
}