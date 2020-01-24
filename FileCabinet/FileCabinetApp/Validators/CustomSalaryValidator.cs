using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Custom Salary validator.
    /// </summary>
    public class CustomSalaryValidator : ISalaryValidator
    {
        /// <summary>
        /// Validates Salary of user's input.
        /// </summary>
        /// <param name="salary">Salary to validate.</param>
        /// <returns>Whether Salary are valid.</returns>
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
