using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces.Validators;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Custom first name validator.
    /// </summary>
    public class CustomFirstNameValidator : IFirstNameValidator
    {
        /// <summary>
        /// Validates first name of user's input.
        /// </summary>
        /// <param name="firstName">First name to validate.</param>
        /// <returns>Whether first name is valid.</returns>
        public bool Validate(string firstName)
        {
            if (string.IsNullOrEmpty(firstName) || firstName.Length < 2 || firstName.Length > 60)
            {
                return false;
            }

            foreach (var item in new string("!@#$%^&*():;{}=+1234567890/|\"'<>.,").ToCharArray())
            {
                if (firstName.Contains(item, StringComparison.InvariantCulture))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
