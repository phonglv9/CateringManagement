$(document).ready(function () {

    loadDataIngredients();

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
            { "data": "Id"},
            { "data": "Name" },
            { "data": "Quantity" },
            { "data": "Unit" },
            { "data": "Price" },
            { "data": "ImportDate" },
            { "data": "ExpiredDate" },
            { "data": "Status" },
            {
                "data": null, "render": function (data, type, row) {
                    return `<button type="button" class="btn btn-warning"><i class="fas fa-edit"></i></i></button>`;
                }
            },

        ]
    });
}