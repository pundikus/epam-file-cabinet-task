using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces.Validators;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Custom date of birth validator.
    /// </summary>
    public class CustomDateOfBirthValidator : IDateOfBirthValidator
    {
        /// <summary>
        /// Validates date of birth input.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth to validate.</param>
        /// <returns>Whether date of birth is valid.</returns>
        public bool Validate(DateTime dateOfBirth)
        {
            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now)
            {
                return false;
            }

            DateTime validDate = new DateTime(2002, 1, 1);
            if (dateOfBirth > validDate)
            {
                return false;
            }

            return true;
        }
    }
}
