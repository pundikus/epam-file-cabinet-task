using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FileCabinetApp.Interfaces.Validators;
using FileCabinetApp.Validators;
using Microsoft.Extensions.Configuration;

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
            var rulesets = GetRulesets();

            ValidationRuleset ruleset = rulesets.Default;

            return new CompositeValidator(new List<IRecordValidator>
            {
                new FirstNameValidator(ruleset.FirstName.MinLength, ruleset.FirstName.MaxLength),
                new LastNameValidator(ruleset.LastName.MinLength, ruleset.LastName.MaxLength),
                new DateOfBirthValidator(ruleset.DateOfBirth.From, ruleset.DateOfBirth.To),
                new CabinetNumberValidator(ruleset.CabinetNumber.MinValue, ruleset.CabinetNumber.MaxValue),
                new CategoryValidator(ruleset.Category.MinValue, ruleset.Category.MaxValue),
                new SalaryValidator(ruleset.Salary.MinValue),
            });
        }

        /// <summary>
        /// Creates custom validator.
        /// </summary>
        /// <param name="validatorBuilder">The instance of ValidatorBuilder class.</param>
        /// <returns>The instance of IRecordValidator class.</returns>
        public static IRecordValidator CreateСustom(this ValidatorBuilder validatorBuilder)
        {
            var rulesets = GetRulesets();

            ValidationRuleset ruleset = rulesets.Custom;

            return new CompositeValidator(new List<IRecordValidator>
            {
                new FirstNameValidator(ruleset.FirstName.MinLength, ruleset.FirstName.MaxLength),
                new LastNameValidator(ruleset.LastName.MinLength, ruleset.LastName.MaxLength),
                new DateOfBirthValidator(ruleset.DateOfBirth.From, ruleset.DateOfBirth.To),
                new CabinetNumberValidator(ruleset.CabinetNumber.MinValue, ruleset.CabinetNumber.MaxValue),
                new CategoryValidator(ruleset.Category.MinValue, ruleset.Category.MaxValue),
                new SalaryValidator(ruleset.Salary.MinValue),
            });
        }

        private static AppConfiguration GetRulesets()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("validation-rules.json");

            IConfiguration config = builder.Build();

            var rulesets = ConfigurationBinder.Get<AppConfiguration>(config);

            return rulesets;
        }
    }
}
