using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces.Validators;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Category Validator.
    /// </summary>
    public class CategoryValidator : IRecordValidator
    {
        private readonly int minValue;
        private readonly int maxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryValidator"/> class.
        /// </summary>
        /// <param name="minValue">Minimum possible category.</param>
        /// <param name="maxValue">Maximum possible category.</param>
        public CategoryValidator(int minValue, int maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        /// <summary>
        /// Validate Category input.
        /// </summary>
        /// <param name="record">Record to validate.</param>
        /// <returns>Category is valid.</returns>
        public bool Validate(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.Category < 65 || record.Category > 67)
            {
                return false;
            }

            if (record.Category < this.minValue || record.Category > this.maxValue)
            {
                return false;
            }

            return true;
        }
    }
}
