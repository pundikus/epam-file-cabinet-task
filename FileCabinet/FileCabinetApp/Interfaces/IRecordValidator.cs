using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Interfaces.Validators
{
    /// <summary>
    /// Validator for input.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <param name="record">Record to validate.</param>
        /// <returns>Exception message.</returns>
        string ValidateParameters(FileCabinetRecord record);
    }
}
