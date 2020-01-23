using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This class implements the default method for checking parameters.
    /// </summary>
    public class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// This Method check parametrs.
        /// </summary>
        /// <param name="parametrs">It is user input parametrs.</param>
        public void ValidateParameters(FileCabinetRecord parametrs)
        {
            if (parametrs == null)
            {
                throw new ArgumentNullException(nameof(parametrs));
            }

            if (parametrs.FirstName == null || string.IsNullOrEmpty(parametrs.FirstName.Trim()))
            {
                throw new ArgumentNullException($"{nameof(parametrs.FirstName)} cannot be empty.");
            }

            if (parametrs.FirstName.Length < 2 || parametrs.FirstName.Length > 60)
            {
                throw new ArgumentException($"{nameof(parametrs.FirstName)} is not correct.");
            }

            if (parametrs.LastName == null || string.IsNullOrEmpty(parametrs.LastName.Trim()))
            {
                throw new ArgumentNullException($"{nameof(parametrs.LastName)} cannot be empty.");
            }

            if (parametrs.LastName.Length < 2 || parametrs.LastName.Length > 60)
            {
                throw new ArgumentException($"{nameof(parametrs.LastName)} is not correct.");
            }

            DateTime minDate = new DateTime(1950, 1, 1);
            DateTime maxDate = DateTime.Now;

            if (parametrs.DateOfBirth < minDate || parametrs.DateOfBirth > maxDate)
            {
                throw new ArgumentException($"{nameof(parametrs.DateOfBirth)} is not correct.");
            }

            if (parametrs.CabinetNumber < 1 || parametrs.CabinetNumber > 1500)
            {
                throw new ArgumentException($"{nameof(parametrs.CabinetNumber)} is not correct.");
            }

            if (parametrs.Salary < 0 || parametrs.Salary > decimal.MaxValue)
            {
                throw new ArgumentException($"{nameof(parametrs.Salary)} is not correct.");
            }

            if (parametrs.Category < 65 || parametrs.Category > 67)
            {
                throw new ArgumentException($"{nameof(parametrs.Category)} is not correct.");
            }
        }
    }
}
