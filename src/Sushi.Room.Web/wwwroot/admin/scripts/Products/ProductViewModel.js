var ProductViewModel = (function (){
    function init(productsUrl, totalCount, currentPage) {
        initSearch(productsUrl);

        initPagination(productsUrl, totalCount, currentPage);

        $('.delete-btn').click( function (e) {
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
                                window.location = productsUrl;
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
    
    function initPagination(productsUrl, totalCount, currentPage) {
        $('#pagination-container').pagination({
            dataSource: function (done) {
                var result = [];
                for (var i =0; i < totalCount; i++) {
                    result.push(i);
                }
                
                done(result);
            },
            pageNumber: currentPage ? currentPage : 1,
            callback: function(data, pagination) {
                if(currentPage !== pagination.pageNumber){
                    window.location = productsUrl + '?pageNumber=' + pagination.pageNumber;
                }
            }
        });
    }
    
    function initSearch(productsUrl) {
        console.log('initSearch');
        $('#search-input').on('keypress',function(e) {
            if(e.which == 13) {
                var searchValue = $(this).val();
                window.location = productsUrl +'?searchValue=' + searchValue + '&pageNumber=' + 1;
            }
        });
    }
    
    return {
        init: init
    }
})();