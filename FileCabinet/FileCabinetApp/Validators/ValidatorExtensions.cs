using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces.Validators;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Extends ValidatorBuilder class.
    /// </summary>
    public static class ValidatorExtensions
    {
        /// <summary>
        /// Creates default validator.
        /// </summary>
        /// <param name="validatorBuilder">The instance of ValidatorBuilder class.</param>
        /// <returns>The instance of IRecordValidator class.</returns>
        public static IRecordValidator CreateDefault(this ValidatorBuilder validatorBuilder)
        {
            return new CompositeValidator(new List<IRecordValidator>
            {
                new FirstNameValidator(2, 60),
                new LastNameValidator(2, 60),
                new DateOfBirthValidator(0, 100),
                new CabinetNumberValidator(0, 1000),
                new CategoryValidator(65, 67),
                new SalaryValidator(0),
            });
        }

        /// <summary>
        /// Creates custom validator.
        /// </summary>
        /// <param name="validatorBuilder">The instance of ValidatorBuilder class.</param>
        /// <returns>The instance of IRecordValidator class.</returns>
        public static IRecordValidator CreateСustom(this ValidatorBuilder validatorBuilder)
        {
            return new CompositeValidator(new List<IRecordValidator>
            {
                new FirstNameValidator(2, 20),
                new LastNameValidator(2, 20),
                new DateOfBirthValidator(18, 65),
                new CabinetNumberValidator(500, 1000),
                new CategoryValidator(66, 67),
                new SalaryValidator(10000),
            });
        }
    }
}
