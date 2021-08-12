var CategoriesPage = (function () {
    function init() {
        initInfiniteScroll();
    }

    function initInfiniteScroll() {
        let $container = $( '.row').infiniteScroll({
            path: function() {
                return `${CategoriesPage.categoriesDataLoadUrl}?pageNumber=${this.pageIndex}`;
            },
            responseBody: 'json',
            status: '.scroll-status',
            history: false
        });

        $container.on( 'load.infiniteScroll', function( event, response ) {
            if (response.categories.length) {
                var template = $('#category-items-template').html();

                var compiledTemplate = Template7.compile(template);

                var html = compiledTemplate(response);
                let $items = $(html);

                $container.infiniteScroll( 'appendItems', $items );
            } else {
                $container.infiniteScroll('destroy');
            }

        });

        $container.infiniteScroll('loadNextPage');
    }
    
    return {
        categoriesDataLoadUrl: null,
        init: init
    }
})();