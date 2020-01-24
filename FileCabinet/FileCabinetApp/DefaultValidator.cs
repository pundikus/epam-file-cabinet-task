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
        public string ValidateParameters(FileCabinetRecord record)
        {
            if (record == null)
            {
                return $"{record} object is invalid.";
            }

            if (!new DefaultFirstNameValidator().Validate(record.FirstName))
            {
                return "First name is invalid.";
            }

            if (!new DefaultLastNameValidator().Validate(record.LastName))
            {
                return "Last name is invalid.";
            }

            if (!new DefaultDateOfBirthValidator().Validate(record.DateOfBirth))
            {
                return "Date of birth is invalid.";
            }

            if (!new DefaultCabinetNumberValidator().Validate(record.CabinetNumber))
            {
                return "Cabinet Number is invalid.";
            }

            if (!new DefaultCategoryValidator().Validate(record.Category))
            {
                return "Category is invalid.";
            }

            if (!new DefaultSalaryValidator().Validate(record.Salary))
            {
                return "Salary are invalid.";
            }

            return null;
        }
    }
}
