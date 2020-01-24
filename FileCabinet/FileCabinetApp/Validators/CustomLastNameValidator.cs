using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces.Validators;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Custom last name validator.
    /// </summary>
    public class CustomLastNameValidator : ILastNameValidator
    {
        /// <summary>
        /// Validates last name of user's input.
        /// </summary>
        /// <param name="lastName">Last name to validate.</param>
        /// <returns>Whether last name is valid.</returns>
        public bool Validate(string lastName)
        {
            if (string.IsNullOrEmpty(lastName) || lastName.Length < 2 || lastName.Length > 60)
            {
                return false;
            }

            foreach (var item in new string("!@#$%^&*():;{}=+1234567890/|\"'<>.,").ToCharArray())
            {
                if (lastName.Contains(item, StringComparison.InvariantCulture))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
