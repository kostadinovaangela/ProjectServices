namespace InternetService.Core
{
    public static class Constants
    {
        public static int DefaultStockQuantity = 100;

        public static class ErrorMessages
        {
            public static string CategoryDoesNotExists = "Category with provided id does not exist";
            public static string ProductDoesNotExist = "Product with provided id does not exist";
            public static string NameRequired = "Name should not be null or empty";
            public static string PriceIsNegativeNumber = "Price must be positive number";
            public static string DescriptionRequired = "Description property is required";
            public static string OutOfStock = "We are sorry but we are out of";
        }
    }
}
