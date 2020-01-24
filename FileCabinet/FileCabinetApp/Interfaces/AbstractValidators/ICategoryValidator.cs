using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Category validator.
    /// </summary>
    public interface ICategoryValidator
    {
        /// <summary>
        /// Category input.
        /// </summary>
        /// <param name="category">Category to validate.</param>
        /// <returns>Whether Category is valid.</returns>
        public bool Validate(char category);
    }
}
