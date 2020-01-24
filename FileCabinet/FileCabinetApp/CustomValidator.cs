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
        /// Validates the input.
        /// </summary>
        /// <param name="record">Record to validate.</param>
        /// <returns>Exception message.</returns>
        public string ValidateParameters(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException($"{record} object is invalid.");
            }

            if (!new CustomFirstNameValidator().Validate(record.FirstName))
            {
                return "First name is invalid.";
            }

            if (!new CustomLastNameValidator().Validate(record.LastName))
            {
                return "Last name is invalid.";
            }

            if (!new CustomDateOfBirthValidator().Validate(record.DateOfBirth))
            {
                return "Date of birth is invalid.";
            }

            if (!new CustomCabinetNumberValidator().Validate(record.CabinetNumber))
            {
                return "Cabinet Number is invalid.";
            }

            if (!new CustomCategoryValidator().Validate(record.Category))
            {
                return "Category is invalid.";
            }

            if (!new CustomSalaryValidator().Validate(record.Salary))
            {
                return "Salary invalid.";
            }

            return null;
        }
    }
}
