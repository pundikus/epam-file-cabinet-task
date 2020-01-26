using System;
using FileCabinetApp.Interfaces.Validators;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Custom validator input.
    /// </summary>
    public class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Validates the user's input.
        /// </summary>
        /// <param name="record">Record to validate.</param>
        /// <returns>Whether record is valid.</returns>
        public bool Validate(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (!new FirstNameValidator(2, 15).Validate(record))
            {
                // return "First name is invalid.";
                return false;
            }

            if (!new LastNameValidator(2, 15).Validate(record))
            {
                // return "Last name is invalid.";
                return false;
            }

            if (!new DateOfBirthValidator(15, 70).Validate(record))
            {
                // return "Date of birth is invalid.";
                return false;
            }

            if (!new CabinetNumberValidator(500, 1000).Validate(record))
            {
                // return "Cabinet Number is invalid.";
                return false;
            }

            if (!new CategoryValidator(66, 67).Validate(record))
            {
                // return "Category is invalid.";
                return false;
            }

            if (!new SalaryValidator(10000).Validate(record))
            {
                // return "Salary are invalid.";
                return false;
            }

            return true;
        }
    }
}
