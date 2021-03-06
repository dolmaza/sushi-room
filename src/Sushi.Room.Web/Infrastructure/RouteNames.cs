namespace Sushi.Room.Web.Infrastructure
{
    public struct RouteNames
    {
        public struct Admin
        {
            public struct Account
            {
                public const string Login = "LoginPage";
                public const string Logout = "LogoutPage";
            }

            public struct User
            {
                public const string Users = "UsersPage";
                public const string Update = "UsersUpdate";
                public const string Create = "UsersCreate";
                public const string Delete = "UsersDelete";
            }

            public struct Dashboard
            {
                public const string DashboardPage = "DashboardPage";
            }

            public struct Category
            {
                public const string Categories = "CategoriesPage";
                public const string CategoryProducts = "CategoryProductsPage";
                public const string Update = "CategoriesUpdate";
                public const string Create = "CategoriesCreate";
                public const string Delete = "CategoriesDelete";
                public const string SyncSortIndexes = "CategoriesSyncSortIndexes";
            }

            public struct Product
            {
                public const string Products = "ProductsPage";
                public const string Update = "ProductsUpdate";
                public const string Create = "ProductsCreate";
                public const string Delete = "ProductsDelete";
                public const string SyncSortIndexes = "ProductsSyncSortIndexes";
            }
        }

        public struct WebCategories
        {
            public const string Categories = "WebCategoriesPage";
            public const string CategoriesData = "WebCategoriesData";
        }

        public struct WebProducts
        {
            public const string Products = "WebProductsPage";
            public const string ProductsData = "WebProductsData";
            public const string ProductDetails = "WebProductDetails";
            public const string ProductsByIds = "WebProductsByIds";
        }
    }
}
