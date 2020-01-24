using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Favourite number validator.
    /// </summary>
    public class DefaultCabinetNumberValidator : ICabinetNumberValidator
    {
        /// <summary>
        /// Validates Cabinet number input.
        /// </summary>
        /// <param name="cabinetNumber">Cabinet number to validate.</param>
        /// <returns>Whether Cabinet number is valid.</returns>
        public bool Validate(short cabinetNumber)
        {
            if (cabinetNumber < 0 || cabinetNumber > 1500)
            {
                return false;
            }

            return true;
        }
    }
}
