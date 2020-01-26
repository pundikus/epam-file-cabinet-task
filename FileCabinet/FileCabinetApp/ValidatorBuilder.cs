using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces.Validators;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Build Validation.
    /// </summary>
    public class ValidatorBuilder
    {
        private readonly List<IRecordValidator> validators;

        /// <summary>
        /// FirstName validator.
        /// </summary>
        /// <param name="min">min size.</param>
        /// <param name="max">max size.</param>
        /// <returns>FirstName valid.</returns>
        public ValidatorBuilder ValidateFirstName(int min, int max)
        {
            this.validators.Add(new FirstNameValidator(min, max));

            return this;
        }

        /// <summary>
        /// LastName validator.
        /// </summary>
        /// <param name="min">min size.</param>
        /// <param name="max">max size Number.</param>
        /// <returns>LastName valid.</returns>
        public ValidatorBuilder ValidateLastName(int min, int max)
        {
            this.validators.Add(new LastNameValidator(min, max));

            return this;
        }

        /// <summary>
        /// Date of birth validator.
        /// </summary>
        /// <param name="from">min Date of birth.</param>
        /// <param name="to">max Date of birth.</param>
        /// <returns>Date of birth valid.</returns>
        public ValidatorBuilder ValidateDateOfBirth(int from, int to)
        {
            this.validators.Add(new FirstNameValidator(from, to));

            return this;
        }

        /// <summary>
        /// Cabinet number validator.
        /// </summary>
        /// <param name="minValue">min Cabinet Number.</param>
        /// <param name="maxValue">max Cabinet Number.</param>
        /// <returns>Cabinet number valid.</returns>
        public ValidatorBuilder ValidateCabinetNumber(short minValue, short maxValue)
        {
            this.validators.Add(new CabinetNumberValidator(minValue, maxValue));

            return this;
        }

        /// <summary>
        /// Category validator.
        /// </summary>
        /// <param name="minValue">min Category.</param>
        /// <param name="maxValue">max Category.</param>
        /// <returns>Category valid.</returns>
        public ValidatorBuilder ValidateCategory(int minValue, int maxValue)
        {
            this.validators.Add(new CategoryValidator(minValue, maxValue));

            return this;
        }

        /// <summary>
        /// Salary validator.
        /// </summary>
        /// <param name="minValue">min salary.</param>
        /// <returns>Salary valid.</returns>
        public ValidatorBuilder ValidateSalary(int minValue)
        {
            this.validators.Add(new SalaryValidator(minValue));

            return this;
        }

        /// <summary>
        /// Create validator.
        /// </summary>
        /// <returns>validator.</returns>
        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }
    }
}
