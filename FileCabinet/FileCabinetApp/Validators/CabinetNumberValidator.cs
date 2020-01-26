using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces.Validators;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Cabinet Number validator.
    /// </summary>
    public class CabinetNumberValidator : IRecordValidator
    {
        private readonly short minValue;
        private readonly short maxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CabinetNumberValidator"/> class.
        /// </summary>
        /// <param name="minValue">Minimum possible CabinetNumber.</param>
        /// <param name="maxValue">Maximum possible CabinetNumber.</param>
        public CabinetNumberValidator(short minValue, short maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        /// <summary>
        /// Validate CabinetNumber input.
        /// </summary>
        /// <param name="record">Record to validate.</param>
        /// <returns>CabinetNumber is valid.</returns>
        public bool Validate(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.Category < 0 || record.Category > 1000)
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
