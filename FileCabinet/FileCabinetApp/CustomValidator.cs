using System;
using System.Collections.Generic;
using FileCabinetApp.Interfaces.Validators;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Custom validator input.
    /// </summary>
    public class CustomValidator : CompositeValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomValidator"/> class.
        /// Validates the user's input.
        /// </summary>
        public CustomValidator()
            : base(new List<IRecordValidator>
            {
                new FirstNameValidator(2, 15),
                new LastNameValidator(2, 15),
                new DateOfBirthValidator(18, 65),
                new CabinetNumberValidator(500, 1000),
                new CategoryValidator(66, 67),
                new SalaryValidator(10000),
            })
        {
        }
    }
}
