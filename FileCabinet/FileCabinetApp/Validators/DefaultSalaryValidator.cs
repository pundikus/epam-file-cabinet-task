using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Donations validator.
    /// </summary>
    public class DefaultSalaryValidator : ISalaryValidator
    {
        /// <summary>
        /// Validates salary of user's input.
        /// </summary>
        /// <param name="salary">Salary to validate.</param>
        /// <returns>Whether salary are valid.</returns>
        public bool Validate(decimal salary)
        {
            if (salary < 0)
            {
                return false;
            }

            return true;
        }
    }
}
