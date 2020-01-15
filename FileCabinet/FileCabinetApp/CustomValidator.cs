using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This class implements the custom method for checking parameters.
    /// </summary>
    public class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// This Method check parametrs.
        /// </summary>
        /// <param name="parametrs">It is user input parametrs.</param>
        public void Validate(ValueRange parametrs)
        {
            if (parametrs == null)
            {
                throw new ArgumentNullException(nameof(parametrs));
            }

            foreach (var item in new string("!@#$%^&*():;{}=+1234567890/|\"'<>.,").ToCharArray())
            {
                if (parametrs.FirstName.Contains(item, StringComparison.InvariantCulture))
                {
                    throw new ArgumentException($"{nameof(parametrs.FirstName)} is not correct.");
                }

                if (parametrs.LastName.Contains(item, StringComparison.InvariantCulture))
                {
                    throw new ArgumentException($"{nameof(parametrs.LastName)} is not correct.");
                }
            }

            DateTime validDate = new DateTime(2002, 1, 1);
            if (parametrs.DateOfBirth > validDate)
            {
                throw new ArgumentException($"{nameof(parametrs.DateOfBirth)} does not suit us.");
            }

            decimal maxSalaryCategoryA = 1000;
            if (parametrs.Category == 65 && parametrs.Salary > maxSalaryCategoryA)
            {
                throw new ArgumentException($"{nameof(parametrs.Category)} does not match wages without bonus.");
            }

            decimal maxSalaryCategpryB = 5000;
            if (parametrs.Category == 66 && parametrs.Salary > maxSalaryCategpryB)
            {
                throw new ArgumentException($"{nameof(parametrs.Category)} does not match wages without bonus.");
            }

            decimal maxSalaryCategoryC = 10000;
            if (parametrs.Category == 67 && parametrs.Salary > maxSalaryCategoryC)
            {
                throw new ArgumentException($"{nameof(parametrs.Category)} does not match wages without bonus.");
            }

            short maxCabinetNumberCategoryA = 500;
            if (parametrs.Category == 65 && parametrs.CabinetNumber > maxCabinetNumberCategoryA)
            {
                throw new ArgumentException($"{nameof(parametrs.CabinetNumber)} does not match this category.");
            }

            short maxCabinetNumberCategoryB = 1000;
            if (parametrs.Category == 66 && parametrs.CabinetNumber > maxCabinetNumberCategoryB)
            {
                throw new ArgumentException($"{nameof(parametrs.CabinetNumber)} does not match this category.");
            }

            short maxCabinetNumberCategoryC = 1500;
            if (parametrs.Category == 67 && parametrs.CabinetNumber > maxCabinetNumberCategoryC)
            {
                throw new ArgumentException($"{nameof(parametrs.CabinetNumber)} does not match this category.");
            }
        }
    }
}
