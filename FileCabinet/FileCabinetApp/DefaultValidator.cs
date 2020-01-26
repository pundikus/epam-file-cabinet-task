using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces.Validators;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Default validator for user's input.
    /// </summary>
    public class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// Validates the user's input.
        /// </summary>
        /// <param name="record">Record to validate.</param>
        /// <returns>Exception message.</returns>
        public bool Validate(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (!new FirstNameValidator(2, 60).Validate(record))
            {
                // return "First name is invalid.";
                return false;
            }

            if (!new LastNameValidator(2, 60).Validate(record))
            {
                // return "Last name is invalid.";
                return false;
            }

            if (!new DateOfBirthValidator(0, 100).Validate(record))
            {
                // return "Date of birth is invalid.";
                return false;
            }

            if (!new CabinetNumberValidator(0, 1000).Validate(record))
            {
                // return "Cabinet Number is invalid.";
                return false;
            }

            if (!new CategoryValidator(65, 67).Validate(record))
            {
                // return "Category is invalid.";
                return false;
            }

            if (!new SalaryValidator(0).Validate(record))
            {
                // return "Salary are invalid.";
                return false;
            }

            return true;
        }
    }
}
