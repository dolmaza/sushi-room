var CategoryViewModel = (function () {
    function init() {
        $('#categories-table-body').on('click', '.delete-btn', function (e) {
            e.preventDefault();
            var _this = $(this);

            var url = $(this).attr('data-url');

            bootbox.confirm({
                message: "ნამდვილად გსურთ კატეგორიის წაშლა?",
                buttons: {
                    confirm: {
                        label: 'კი',
                        className: 'btn-success'
                    },
                    cancel: {
                        label: 'არა',
                        className: 'btn-danger'
                    }
                },
                callback: function (result) {
                    if (result) {
                        $.ajax({
                            url: url,
                            method: 'POST',
                            success: function (response) {
                                _this.closest('tr').remove();
                                toastrNotification.init(response.message).showMessage();
                            },
                            error: function (response) {
                                toastrNotification.init(response.responseJSON.message, { notificationType: toastrNotification.types.error }).showMessage();
                            }
                        });
                    }
                }
            });

        });

        $("#categories-table-body").sortable({
            update: function (event, ui) {
                var sortIndexes = [];
                $('#categories-table-body tr').each(function(index, item) {
                    var id = $(item).attr('data-id');
                    sortIndexes.push({Key: id, Value: index });
                });

                var url = $('#categories-table').attr('data-sync-sort-indexes-url');

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
        $("#categories-table-body").disableSelection();
    }

    return {
        init: init
    }
})();