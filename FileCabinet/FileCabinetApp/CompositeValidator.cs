using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces.Validators;

namespace FileCabinetApp
{
    /// <inheritdoc/>
    public class CompositeValidator : IRecordValidator
    {
        private readonly List<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="validators">validator input.</param>
        public CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            this.validators = (List<IRecordValidator>)validators;
        }

        /// <inheritdoc/>
        public bool Validate(FileCabinetRecord record)
        {
            bool isValid = false;

            foreach (var validator in this.validators)
            {
                isValid = validator.Validate(record);
            }

            return isValid;
        }
    }
}
