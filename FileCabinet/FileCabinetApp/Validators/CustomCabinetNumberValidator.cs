using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Custom favourite number validator.
    /// </summary>
    public class CustomCabinetNumberValidator : ICabinetNumberValidator
    {
        /// <summary>
        /// Validates favourite number of user's input.
        /// </summary>
        /// <param name="cabinetNumber">Favourite number to validate.</param>
        /// <returns>Whether favourite number is valid.</returns>
        public bool Validate(short cabinetNumber)
        {
            if (cabinetNumber < 1 || cabinetNumber > 1500)
            {
                return false;
            }

            return true;
        }
    }
}
