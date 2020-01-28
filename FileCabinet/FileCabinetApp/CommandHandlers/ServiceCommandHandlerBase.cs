using System;
using System.Globalization;
using static FileCabinetApp.Program;

namespace FileCabinetApp
{
    /// <summary>
    /// Handler for commands which relate to the service.
    /// </summary>
    public abstract class ServiceCommandHandlerBase : CommandHandlerBase
    {
        /// <summary>
        /// Gets or sets the service to get information and modify.
        /// </summary>
        /// <value>
        /// The service to get information and modify.
        /// </value>
        protected IFileCabinetService Service { get; set; }

        /// <summary>
        /// This Method validate First Name.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>check valid.</returns>
        protected Tuple<bool, string> FirstNameValidator(string arg)
        {
            bool isValid = true;
            if (IsModeCustom)
            {
                foreach (var item in new string("!@#$%^&*():;{}=+1234567890/|\"'<>.,").ToCharArray())
                {
                    if (arg.Contains(item, StringComparison.InvariantCulture))
                    {
                        isValid = false;
                    }

                    if (arg.Contains(item, StringComparison.InvariantCulture))
                    {
                        isValid = false;
                    }
                }
            }
            else
            {
                if (arg == null || string.IsNullOrEmpty(arg.Trim()))
                {
                    isValid = false;
                }

                if (arg.Length < 2 || arg.Length > 60)
                {
                    isValid = false;
                }
            }

            return new Tuple<bool, string>(isValid, arg);
        }

        /// <summary>
        /// This Method validate Last Name.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>check valid.</returns>
        protected Tuple<bool, string> LastNameValidator(string arg)
        {
            bool isVaild = true;

            if (IsModeCustom)
            {
                foreach (var item in new string("!@#$%^&*():;{}=+1234567890/|\"'<>.,").ToCharArray())
                {
                    if (arg.Contains(item, StringComparison.InvariantCulture))
                    {
                        isVaild = false;
                    }

                    if (arg.Contains(item, StringComparison.InvariantCulture))
                    {
                        isVaild = false;
                    }
                }
            }
            else
            {
                if (arg == null || string.IsNullOrEmpty(arg.Trim()))
                {
                    isVaild = false;
                }

                if (arg.Length < 2 || arg.Length > 60)
                {
                    isVaild = false;
                }
            }

            return new Tuple<bool, string>(isVaild, arg);
        }

        /// <summary>
        /// This Method validate Date Of Birth.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>check valid.</returns>
        protected Tuple<bool, string> DateOfBirthValidator(DateTime arg)
        {
            bool isValid = true;

            if (IsModeCustom)
            {
                DateTime validDate = new DateTime(2002, 1, 1);
                if (arg > validDate)
                {
                    isValid = false;
                }
            }
            else
            {
                DateTime minDate = new DateTime(1950, 1, 1);
                DateTime maxDate = DateTime.Now;

                if (arg < minDate || arg > maxDate)
                {
                    isValid = false;
                }
            }

            return new Tuple<bool, string>(isValid, arg.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// This Method validate Cabinet Number.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>check valid.</returns>
        protected Tuple<bool, string> CabinetNumberValidator(short arg)
        {
            bool flag = true;
            if (IsModeCustom)
            {
                short maxCabinetNumberCustom = 200;
                if (arg > maxCabinetNumberCustom)
                {
                    flag = false;
                }
            }
            else
            {
                if (arg < 1 || arg > 1500)
                {
                    flag = false;
                }
            }

            return new Tuple<bool, string>(flag, arg.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// This Method validate Salary.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>check valid.</returns>
        protected Tuple<bool, string> SalaryValidator(decimal arg)
        {
            bool flag = true;
            if (IsModeCustom)
            {
                decimal maxSalaryCategoryC = 10000;
                if (arg > maxSalaryCategoryC)
                {
                    flag = false;
                }
            }
            else
            {
                if (arg < 0 || arg > decimal.MaxValue)
                {
                    flag = false;
                }
            }

            return new Tuple<bool, string>(flag, arg.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// This Method validate Category.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>check valid.</returns>
        protected Tuple<bool, string> CategoryValidator(char arg)
        {
            bool flag = true;
            if (IsModeCustom)
            {
                if (arg < 65 || arg > 67)
                {
                    flag = false;
                }

                if (arg == 67)
                {
                    flag = false;
                }
            }
            else
            {
                if (arg < 65 || arg > 67)
                {
                    flag = false;
                }
            }

            return new Tuple<bool, string>(flag, arg.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Parses and validates the user's input.
        /// </summary>
        /// <typeparam name="T">The type of input.</typeparam>
        /// <param name="converter">Converter from string to specific type.</param>
        /// <param name="validator">Validator for the converted input.</param>
        /// <returns>Converted and validated value.</returns>
        protected T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        /// <summary>
        /// This Method convert value to String.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>convert value.</returns>
        protected Tuple<bool, string, string> StringConverter(string arg)
        {
            Tuple<bool, string, string> convertString = null;

            if (arg != null)
            {
                convertString = new Tuple<bool, string, string>(typeof(string).Equals(arg.GetType()), arg, arg.ToString(CultureInfo.InvariantCulture));
            }

            return convertString;
        }

        /// <summary>
        /// This Method convert value to DateTime.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>convert value.</returns>
        protected Tuple<bool, string, DateTime> DateTimeConverter(string arg)
        {
            bool parsedDateTime = DateTime.TryParseExact(arg, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime);

            return new Tuple<bool, string, DateTime>(parsedDateTime, arg, dateTime);
        }

        /// <summary>
        /// This Method convert value to Short.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>convert value.</returns>
        protected Tuple<bool, string, short> ShortConverter(string arg)
        {
            var parsedCabinetNumber = short.TryParse(arg, out short cabinetNumber);

            return new Tuple<bool, string, short>(parsedCabinetNumber, arg, cabinetNumber);
        }

        /// <summary>
        /// This Method convert value to Decimal.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>convert value.</returns>
        protected Tuple<bool, string, decimal> DecimalConverter(string arg)
        {
            var parsedSalary = decimal.TryParse(arg, out decimal salary);

            return new Tuple<bool, string, decimal>(parsedSalary, arg, salary);
        }

        /// <summary>
        /// This Method convert value to Char.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>convert value.</returns>
        protected Tuple<bool, string, char> CharConverter(string arg)
        {
            var parsedCategory = char.TryParse(arg, out char category);

            return new Tuple<bool, string, char>(parsedCategory, arg, category);
        }
    }
}
