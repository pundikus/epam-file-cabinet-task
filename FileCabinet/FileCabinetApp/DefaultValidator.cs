using System.Collections.Generic;
using FileCabinetApp.Interfaces.Validators;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Default validator for user's input.
    /// </summary>
    public class DefaultValidator : CompositeValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValidator"/> class.
        /// Validates the user's input.
        /// </summary>
        public DefaultValidator()
            : base(new List<IRecordValidator>
            {
                new FirstNameValidator(2, 15),
                new LastNameValidator(2, 15),
                new DateOfBirthValidator(0, 100),
                new CabinetNumberValidator(0, 1000),
                new CategoryValidator(65, 67),
                new SalaryValidator(0),
            })
        {
        }
    }
}
