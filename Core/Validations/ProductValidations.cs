using System.ComponentModel.DataAnnotations;
using InternetService.DAL.Models.Dtos;

namespace InternetService.Core
{
    public static class ProductValidations
    {
        public static void ValidateRequiredFields(this ProductDto product)
        {
            if (string.IsNullOrEmpty(product.Name))
            {
                throw new ValidationException(Constants.ErrorMessages.NameRequired);
            }

            if (string.IsNullOrEmpty(product.Description))
            {
                throw new ValidationException(Constants.ErrorMessages.DescriptionRequired);
            }

            if (product.Price < 0)
            {
                throw new ValidationException(Constants.ErrorMessages.PriceIsNegativeNumber);
            }

            if (product.Categories.Any(f => f < 0))
            {
                throw new ValidationException(Constants.ErrorMessages.CategoryDoesNotExists);
            }

        }

    }
}
