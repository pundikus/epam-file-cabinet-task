using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static FileCabinetApp.Program;

namespace FileCabinetApp
{
    /// <summary>
    /// This class initialize Handler.
    /// </summary>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        /// <summary>
        /// Index of command's name in HelpMessages table.
        /// </summary>
        protected const int CommandHelpIndex = 0;

        /// <summary>
        /// Index of command's description in HelpMessages table.
        /// </summary>
        protected const int DescriptionHelpIndex = 1;

        /// <summary>
        /// Index of command's explanation in HelpMessages table.
        /// </summary>
        protected const int ExplanationHelpIndex = 2;

        /// <summary>
        /// Prints the help messages to the user.
        /// </summary>
        protected static readonly string[][] HelpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints the number of records", "The 'stat' command prints the number of records." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new string[] { "edit", "edits the existing record", "The 'edit' command edits the existing record." },
            new string[] { "find", "finds records by given criteria", "The 'find' command finds records by given criteria" },
            new string[] { "list", "prints the records", "The 'list' prints the records" },
            new string[] { "export", "exports the records to the file in xml or csv format", "The 'export' command exports the records to the file in xml or csv format." },
            new string[] { "import", "imports the records from the xml or csv file", "The 'import' command imports the records from the xml or csv file." },
            new string[] { "remove", "removes the record from the list", "The 'remove' command removes the record from the list" },
            new string[] { "purge", "purges the deleted records from the list", "The 'purge' command purges the deleted records from the list." },
        };

        private ICommandHandler nextHandler;

        /// <summary>
        /// Handler the request.
        /// </summary>
        /// <param name="request">Request to handle.</param>
        /// <returns>The object back or null.</returns>
        public virtual object Handle(AppCommandRequest request)
        {
            if (this.nextHandler != null)
            {
                return this.nextHandler.Handle(request);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Set the next handler.
        /// </summary>
        /// <param name="commandHandler">Handler to set.</param>
        /// <returns>Handler back.</returns>
        public ICommandHandler SetNext(ICommandHandler commandHandler)
        {
            this.nextHandler = commandHandler;

            return commandHandler;
        }

        /// <summary>
        /// Parses and validates the user's input.
        /// </summary>
        /// <typeparam name="T">The type of input.</typeparam>
        /// <param name="converter">Converter from string to specific type.</param>
        /// <param name="validator">Validator for the converted input.</param>
        /// <returns>Converted and validated value.</returns>
        protected static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
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
        protected static Tuple<bool, string, string> StringConverter(string arg)
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
        protected static Tuple<bool, string, DateTime> DateTimeConverter(string arg)
        {
            bool parsedDateTime = DateTime.TryParseExact(arg, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime);

            return new Tuple<bool, string, DateTime>(parsedDateTime, arg, dateTime);
        }

        /// <summary>
        /// This Method convert value to Short.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>convert value.</returns>
        protected static Tuple<bool, string, short> ShortConverter(string arg)
        {
            var parsedCabinetNumber = short.TryParse(arg, out short cabinetNumber);

            return new Tuple<bool, string, short>(parsedCabinetNumber, arg, cabinetNumber);
        }

        /// <summary>
        /// This Method convert value to Decimal.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>convert value.</returns>
        protected static Tuple<bool, string, decimal> DecimalConverter(string arg)
        {
            var parsedSalary = decimal.TryParse(arg, out decimal salary);

            return new Tuple<bool, string, decimal>(parsedSalary, arg, salary);
        }

        /// <summary>
        /// This Method convert value to Char.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>convert value.</returns>
        protected static Tuple<bool, string, char> CharConverter(string arg)
        {
            var parsedCategory = char.TryParse(arg, out char category);

            return new Tuple<bool, string, char>(parsedCategory, arg, category);
        }

        /// <summary>
        /// This Method validate First Name.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>check valid.</returns>
        protected static Tuple<bool, string> FirstNameValidator(string arg)
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
        protected static Tuple<bool, string> LastNameValidator(string arg)
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
        protected static Tuple<bool, string> DateOfBirthValidator(DateTime arg)
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
        protected static Tuple<bool, string> CabinetNumberValidator(short arg)
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
        protected static Tuple<bool, string> SalaryValidator(decimal arg)
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
        protected static Tuple<bool, string> CategoryValidator(char arg)
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
    }
}
