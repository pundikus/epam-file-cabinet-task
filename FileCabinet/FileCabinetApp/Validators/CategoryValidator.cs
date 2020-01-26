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
        private readonly int from;
        private readonly int to;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryValidator"/> class.
        /// </summary>
        /// <param name="from">Minimum possible category.</param>
        /// <param name="to">Maximum possible category.</param>
        public CategoryValidator(int from, int to)
        {
            this.from = from;
            this.to = to;
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

            if (record.Category < this.from || record.Category > this.to)
            {
                return false;
            }

            return true;
        }
    }
}
