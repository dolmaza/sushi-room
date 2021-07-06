var UserViewModel = (function() {
    function init() {
        $('#users-table').on('click', '.delete-btn', function (e) {
            e.preventDefault();
            var _this = $(this);

            var url = $(this).attr('data-url');

            bootbox.confirm({
                message: "ნამდვილად გსურთ მომხმარებლის წაშლა?",
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
    }

    return {
        init: init
    }
})();