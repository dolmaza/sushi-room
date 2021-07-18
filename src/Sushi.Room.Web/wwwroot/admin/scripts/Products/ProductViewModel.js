var ProductViewModel = (function (){
    function init(productsUrl, totalCount) {
        initPagination(productsUrl, totalCount);

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
    
    function initPagination(productsUrl, totalCount) {
        var prevPageNumber = +getParameterByName('pageNumber');
        $('#pagination-container').pagination({
            dataSource: function (done) {
                var result = [];
                for (var i =0; i < totalCount; i++) {
                    result.push(i);
                }
                
                done(result);
            },
            pageNumber: prevPageNumber ? prevPageNumber : 1,
            callback: function(data, pagination) {
                if(prevPageNumber !== pagination.pageNumber){
                    window.location = productsUrl + '?pageNumber=' + pagination.pageNumber;
                }
            }
        });
    }

    function getParameterByName(name, url = window.location.href) {
        name = name.replace(/[\[\]]/g, '\\$&');
        var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    }
    
    return {
        init: init
    }
})();