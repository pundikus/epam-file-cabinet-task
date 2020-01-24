using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Custom favourite character validator.
    /// </summary>
    public class CustomCategoryValidator : ICategoryValidator
    {
        /// <summary>
        /// Validates favourite character of user's input.
        /// </summary>
        /// <param name="category">Favourite character to validate.</param>
        /// <returns>Whether favourite character is valid.</returns>
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
