var CategoryProductsViewModel = (function () {
    function init() {
        $("#products-table-body").sortable({
            update: function (event, ui) {
                var sortIndexes = [];
                $('#products-table-body tr').each(function(index, item) {
                    var id = $(item).attr('data-id');
                    sortIndexes.push({Key: id, Value: index });
                });

                var url = $('#products-table').attr('data-sync-sort-indexes-url');

                $.ajax({
                    url: url,
                    data: {
                        SortIndexes: sortIndexes
                    },
                    method: 'POST',
                    error: function (response) {
                        toastrNotification.init(response.responseJSON.message, { notificationType: toastrNotification.types.error }).showMessage();
                    }
                });
            }
        });
        $("#products-table-body").disableSelection();
    }
    return {
        init: init
    }
})()