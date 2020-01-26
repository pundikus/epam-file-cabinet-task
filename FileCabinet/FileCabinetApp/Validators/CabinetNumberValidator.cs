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
        private readonly int from;
        private readonly int to;

        /// <summary>
        /// Initializes a new instance of the <see cref="CabinetNumberValidator"/> class.
        /// </summary>
        /// <param name="from">Minimum possible CabinetNumber.</param>
        /// <param name="to">Maximum possible CabinetNumber.</param>
        public CabinetNumberValidator(int from, int to)
        {
            this.from = from;
            this.to = to;
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

            if (record.Category < this.from || record.Category > this.to)
            {
                return false;
            }

            return true;
        }
    }
}
