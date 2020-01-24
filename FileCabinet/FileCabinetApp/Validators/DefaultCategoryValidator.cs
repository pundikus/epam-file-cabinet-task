using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Favourite character validator.
    /// </summary>
    public class DefaultCategoryValidator : ICategoryValidator
    {
        /// <summary>
        /// Validates category of input.
        /// </summary>
        /// <param name="category">category to validate.</param>
        /// <returns>Whether category is valid.</returns>
        public bool Validate(char category)
        {
            if (category < 65 || category > 67)
            {
                return false;
            }

            return true;
        }
    }
}
