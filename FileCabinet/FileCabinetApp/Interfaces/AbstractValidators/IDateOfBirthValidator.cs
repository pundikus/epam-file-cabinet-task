using System;

namespace FileCabinetApp.Interfaces.Validators
{
    /// <summary>
    /// Date of birth validator.
    /// </summary>
    public interface IDateOfBirthValidator
    {
        /// <summary>
        /// Validates date of birth of user's input.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth to validate.</param>
        /// <returns>Whether date of birth is valid.</returns>
        public bool Validate(DateTime dateOfBirth);
    }
}
