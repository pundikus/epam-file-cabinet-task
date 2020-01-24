using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Salary validator.
    /// </summary>
    public interface ISalaryValidator
    {
        /// <summary>
        /// Validates salary input.
        /// </summary>
        /// <param name="salary">Salary to validate.</param>
        /// <returns>Whether salary are valid.</returns>
        public bool Validate(decimal salary);
    }
}
