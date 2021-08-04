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
        
        $('.basket-products-count').html(basket.length);
    }
    
    function initControls() {
        $(document).on('click','.add-product-in-basket-btn', function (){
            var productId = +$(this).attr('data-product-id');
            console.log(productId);
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

            if (item.productId === productId) {
                basket.splice(i, 1);
                init(basket);
                break;
            }
        }
    }
    
    function getBasketProductsCount() {
        var basket = getBasket();
        
        return basket.length;
    }
    
    function getProducts() {
        return new Promise(function (resolve, reject) {
            $.ajax({
                url: Basket.getProductsUrl,
                method: 'POST',
                data: {
                    ProductIds: getProductIdsFromBasket()
                },
                success: function (response) {
                    resolve(response.data);
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
    
    return {
        getProductsUrl: null,
        init: init,
        addProduct: addProduct,
        removeProduct: removeProduct,
        getBasket: getBasket,
        getBasketProductsCount: getBasketProductsCount,
        getProducts: getProducts,
        initControls: initControls
    };
})();

$(function (){
   Basket.init(Basket.getBasket()); 
   Basket.initControls();
});