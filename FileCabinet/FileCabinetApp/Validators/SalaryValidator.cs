using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces.Validators;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Salary validator.
    /// </summary>
    public class SalaryValidator : IRecordValidator
    {
        private readonly decimal minValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalaryValidator"/> class.
        /// </summary>
        /// <param name="minValue">Minimum possible value.</param>
        public SalaryValidator(decimal minValue)
        {
            this.minValue = minValue;
        }

        /// <summary>
        /// Validates Salary of input.
        /// </summary>
        /// <param name="record">Record to validate.</param>
        /// <returns>Salary are valid.</returns>
        public bool Validate(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.Salary < this.minValue)
            {
                return false;
            }

            return true;
        }
    }
}
