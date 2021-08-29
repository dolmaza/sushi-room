var Basket = (function () {
    var basketKey = "__Basket__";
    var basketDefault = [];
    
    function init(basket) {
        if (basket) {
            localStorage.setItem(basketKey, JSON.stringify(basket));
        }
        else {
            localStorage.setItem(basketKey, JSON.stringify(basketDefault));
        }
        
        var productsCount = getBasketProductsCount();
        
        $('.basket-products-count').html(productsCount);
    }
    
    function initControls() {
        $(document).on('click','.add-product-in-basket-btn', function (){
            var productId = +$(this).attr('data-product-id');
            
            addProduct(productId);
        });

        $(document).on('click','.remove-product-from-basket-btn', function (){
            var productId = +$(this).attr('data-product-id');
            removeProduct(productId);
        });
    }
    
    function getBasket() {
        var basket = localStorage.getItem(basketKey);
        
        return basket ? JSON.parse(basket) : basketDefault;
    }
    
    function addProduct(productId, quantity) {
        var basket = getBasket();
        
        var filteredItems = basket.filter(function (item) {
            return item.productId === productId;
        });
        
        if (filteredItems.length) {
            var item = filteredItems[0];
            item.quantity = quantity ? quantity : item.quantity + 1;
        } else {
            quantity = quantity ? quantity : 1;
            basket.push({ productId, quantity});
        }
        
        init(basket);
    }
    
    function removeProduct(productId) {
        var basket = getBasket();
        
        for (var i = 0; i < basket.length; i++) {
            var item = basket[i];

            if (item.productId === +productId) {
                basket.splice(i, 1);
                init(basket);
                break;
            }
        }
    }
    
    function increaseQuantity(productId) {
        var basket = getBasket();

        var filteredItems = basket.filter(function (item) {
            return item.productId === +productId;
        });

        if (filteredItems.length) {
            var item = filteredItems[0];
            item.quantity = item.quantity + 1;
        }

        init(basket);
    }
    
    function decreaseQuantity(productId) {
        var basket = getBasket();

        var filteredItems = basket.filter(function (item) {
            return item.productId === +productId;
        });

        if (filteredItems.length) {
            var item = filteredItems[0];
            if (item.quantity > 1) {
                item.quantity = item.quantity - 1;
            }
        }

        init(basket);
    }
    
    function getBasketProductsCount() {
        var basket = getBasket();
        var count = 0;
        
        for (var i = 0; i < basket.length; i++) {
            count += basket[i].quantity;
        }
        
        return count;
    }
    
    function getProductsWithSummery() {
        return new Promise(function (resolve, reject) {
            $.ajax({
                url: Basket.getProductsUrl,
                method: 'POST',
                data: {
                    ProductIds: getProductIdsFromBasket()
                },
                success: function (response) {
                    var basket = getBasket();
                    var products = response.data;
                    var subTotalPrice = 0;
                    var commissionPrice = 0;
                    var totalPrice = 0;
                    
                    $.each(products, function (index, product) {
                        product.quantity = basket.filter(function (item) {
                            return item.productId === product.id;
                        })[0].quantity;
                        
                        subTotalPrice += (product.hasDiscount ? product.discountedPrice : product.price) * product.quantity;
                    });

                    subTotalPrice = Math.round(subTotalPrice * 100) / 100

                    commissionPrice =  subTotalPrice * 18 / 100;
                    
                    commissionPrice = Math.round(commissionPrice * 100) / 100
                    
                    totalPrice = subTotalPrice + commissionPrice

                    totalPrice = Math.round(totalPrice * 100) / 100


                    resolve({
                        products,
                        subTotalPrice,
                        commissionPrice,
                        totalPrice
                    });
                },
                error: function (response) {
                    reject(response);
                }
            });
        });
    }
    
    function getProductIdsFromBasket() {
        var basket = getBasket();
        var productIds = [];

        for (var i = 0; i < basket.length; i++) {
            var item = basket[i];

            productIds.push(item.productId);
        }
        
        return productIds;
    }
    
    function clearBasket() {
        init(basketDefault);
    }
    
    return {
        getProductsUrl: null,
        init: init,
        addProduct: addProduct,
        removeProduct: removeProduct,
        getBasket: getBasket,
        getBasketProductsCount: getBasketProductsCount,
        getProductsWithSummery: getProductsWithSummery,
        initControls: initControls,
        clearBasket: clearBasket,
        increaseQuantity: increaseQuantity,
        decreaseQuantity: decreaseQuantity
    };
})();

var BasketSidebar = (function () {
    function init() {
        Basket.getProductsWithSummery().then(function (productsWithSummeryObj){
            
            var template = $('#basket-sidebar-template').html();
            
            var compiledTemplate = Template7.compile(template);
            
            var html = compiledTemplate(productsWithSummeryObj);
            
            $('#bs-canvas-right .sidebar').remove();
            $('#bs-canvas-right').append(html);
        });
    }
    
    function initControls() {
        var w = window.innerWidth;
        var bsDefaults = {
                offset: false,
                overlay: true,
                width: w < 576 ?'330px' : "420px"
            },
            bsMain = $('.bs-offset-main'),
            bsOverlay = $('.bs-canvas-overlay');

        $('[data-toggle="canvas"][aria-expanded="false"]').on('click', function() {
            var canvas = $(this).data('target'),
                opts = $.extend({}, bsDefaults, $(canvas).data()),
                prop = $(canvas).hasClass('bs-canvas-right') ? 'margin-right' : 'margin-left';

            if (opts.width === '100%')
                opts.offset = false;

            $(canvas).css('width', opts.width);
            if (opts.offset && bsMain.length)
                bsMain.css(prop, opts.width);

            $(canvas + ' .bs-canvas-close').attr('aria-expanded', "true");
            $('[data-toggle="canvas"][data-target="' + canvas + '"]').attr('aria-expanded', "true");
            if (opts.overlay && bsOverlay.length)
                bsOverlay.addClass('show');
            
            BasketSidebar.init();
            return false;
        });

        $('.bs-canvas-close, .bs-canvas-overlay').on('click', function() {
            var canvas, aria;
            if ($(this).hasClass('bs-canvas-close')) {
                canvas = $(this).closest('.bs-canvas');
                aria = $(this).add($('[data-toggle="canvas"][data-target="#' + canvas.attr('id') + '"]'));
                if (bsMain.length)
                    bsMain.css(($(canvas).hasClass('bs-canvas-right') ? 'margin-right' : 'margin-left'), '');
            } else {
                canvas = $('.bs-canvas');
                aria = $('.bs-canvas-close, [data-toggle="canvas"]');
                if (bsMain.length)
                    bsMain.css({
                        'margin-left': '',
                        'margin-right': ''
                    });
            }
            canvas.css('width', '');
            aria.attr('aria-expanded', "false");
            if (bsOverlay.length)
                bsOverlay.removeClass('show');
            return false;
        });

        $(".basket-item-quantity-input").inputSpinner({
            buttonsWidth: "5px"
        });

        $('.count').prop('disabled', true);
        $(document).on('click','.plus',function(){
            var productId = $(this).attr('data-product-id');
            
            Basket.increaseQuantity(productId);
            BasketSidebar.init();
        });
        $(document).on('click','.minus',function(){
            var productId = $(this).attr('data-product-id');

            Basket.decreaseQuantity(productId);
            BasketSidebar.init();
        });
        
        $(document).on('click', '#clear-basket-btn', function (){
            var result = confirm(BasketSidebar.clearBasketConfirmText);
            
            if (result){
                Basket.clearBasket();
                BasketSidebar.init();
            }
        });
        
        $(document).on('click', '.remove-basket-item-btn', function (){
            var productId = $(this).attr('data-product-id');
            
            Basket.removeProduct(productId);
            init();
            
            return false;
        });
    }
    
    return {
        clearBasketConfirmText: null,
        init: init,
        initControls: initControls
    }
})();

$(function (){
   Basket.init(Basket.getBasket()); 
   Basket.initControls();
   BasketSidebar.initControls();
});