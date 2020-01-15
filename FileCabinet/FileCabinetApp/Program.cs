using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This class is needed for user interaction.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Nikita Pundis";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const string FirstMessage = "Specify usage mode (default or custom)";
        private const string DefaultMessage = "Using default validation rules.";
        private const string CustomMessage = "Using custom validation rules.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static IRecordValidator validator = new DefaultValidator();

        private static FileCabinetService fileCabinetService;

        private static bool isRunning = true;
        private static bool isModeCustom = false;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "gets the number of records", "The 'stat' command returns the number of records." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record" },
            new string[] { "list", "returns a list of records", "The 'list' command returns a list of records" },
            new string[] { "edit", "changes record", "The 'edit' command changes record" },
            new string[] { "find", "find records", "The 'find' command search records by input value" },
        };

        /// <summary>
        /// This is the main method in which all kinds of user interaction is performed.
        /// </summary>
        public static void Main()
        {
            Console.Write(FirstMessage);
            var inputMode = Console.ReadLine();

            const string fullParametr = "--validation-rules";
            const string abbreviatedParametr = "-v";
            const string customParametrs = "custom";
            var inputsArrayParams = Array.Empty<string>();

            try
            {
                if (inputMode.Contains(fullParametr, StringComparison.InvariantCulture))
                {
                    int modeIndex = 1;

                    inputsArrayParams = inputMode.Split('=', 2);
                    if (inputsArrayParams[modeIndex].Equals(customParametrs, StringComparison.InvariantCultureIgnoreCase))
                    {
                        isModeCustom = true;
                        validator = new CustomValidator();
                        fileCabinetService = new FileCabinetService(validator);
                    }
                }
                else if (inputMode.Contains(abbreviatedParametr, StringComparison.InvariantCulture))
                {
                    int modeIndex = 1;

                    inputsArrayParams = inputMode.Split(' ', 2);
                    if (inputsArrayParams[modeIndex].Equals(customParametrs, StringComparison.InvariantCultureIgnoreCase))
                    {
                        isModeCustom = true;
                        validator = new CustomValidator();
                        fileCabinetService = new FileCabinetService(validator);
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect input.");
                    fileCabinetService = new FileCabinetService(validator);
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Incorrect input.");
                fileCabinetService = new FileCabinetService(validator);
            }

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");

            if (isModeCustom)
            {
                Console.WriteLine(CustomMessage);
            }
            else
            {
                Console.WriteLine(DefaultMessage);
            }

            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Stat(string parameters)
        {
            int recordsCount = fileCabinetService.GetStat();

            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            do
            {
                try
                {
                    Console.Write("First name: ");
                    string firstName = Console.ReadLine();

                    Console.Write("Last name: ");
                    string lastName = Console.ReadLine();

                    Console.Write("Date of birth: ");
                    string dateofBirthString = Console.ReadLine();
                    var parseddateofBirth = DateTime.TryParseExact(dateofBirthString, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateofBirth);
                    if (!parseddateofBirth)
                    {
                        Console.WriteLine("Invalid Date.");
                        break;
                    }

                    Console.Write("Cabinet number: ");
                    string cabinetNumberString = Console.ReadLine();
                    var parsedCabinetNumber = short.TryParse(cabinetNumberString, out short cabinetNumber);
                    if (!parsedCabinetNumber)
                    {
                        Console.WriteLine("Invalid Cabinet number.");
                        break;
                    }

                    Console.Write("Salary: ");
                    string salaryString = Console.ReadLine();
                    var parsedSalary = decimal.TryParse(salaryString, out decimal salary);
                    if (!parsedSalary)
                    {
                        Console.WriteLine("Invalid Salary.");
                        break;
                    }

                    Console.Write("Category(A, B, C): ");
                    string categoryString = Console.ReadLine().ToUpperInvariant();
                    var parsedCategory = char.TryParse(categoryString, out char category);
                    if (!parsedCategory)
                    {
                        Console.WriteLine("Invalid Category.");
                        break;
                    }

                    ValueRange parametrs = new ValueRange(firstName, lastName, dateofBirth, cabinetNumber, salary, category);

                    int record = fileCabinetService.CreateRecord(parametrs);

                    Console.WriteLine("Record #" + record + " is created.");
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            while (true);
        }

        private static void List(string parameters)
        {
            var listRecords = fileCabinetService.GetRecords();

            foreach (var item in listRecords)
            {
                var recordString = new StringBuilder();

                recordString.Append($"#{item.Id}, ");
                recordString.Append($"{item.FirstName}, ");
                recordString.Append($"{item.LastName}, ");
                recordString.Append($"{item.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, ");
                recordString.Append($"{item.CabinetNumber}, ");
                recordString.Append($"{item.Salary}y.e, ");
                recordString.Append($"{item.Category}");

                Console.WriteLine(recordString);
            }
        }

        private static void Edit(string parameters)
        {
            var parsedId = int.TryParse(parameters, out int id);
            if (!parsedId)
            {
                Console.WriteLine("Invalid Id");
            }

            var listRecords = fileCabinetService.GetRecords();

            if (!listRecords.Any(x => x.Id == id))
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }

            Console.Write("First name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last name: ");
            string lastName = Console.ReadLine();

            Console.Write("Date of birth: ");
            string dateofBirthString = Console.ReadLine();
            var parseddateofBirth = DateTime.TryParseExact(dateofBirthString, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateofBirth);
            if (!parseddateofBirth)
            {
                Console.WriteLine("Invalid Date.");
            }

            Console.Write("Cabinet number: ");
            string cabinetNumberString = Console.ReadLine();
            var parsedCabinetNumber = short.TryParse(cabinetNumberString, out short cabinetNumber);
            if (!parsedCabinetNumber)
            {
                Console.WriteLine("Invalid Cabinet number.");
            }

            Console.Write("Salary: ");
            string salaryString = Console.ReadLine();
            var parsedSalary = decimal.TryParse(salaryString, out decimal salary);
            if (!parsedSalary)
            {
                Console.WriteLine("Invalid Salary.");
            }

            Console.Write("Category(A, B, C): ");
            string categoryString = Console.ReadLine().ToUpperInvariant();
            var parsedCategory = char.TryParse(categoryString, out char category);
            if (!parsedCategory)
            {
                Console.WriteLine("Invalid Category.");
            }

            try
            {
                ValueRange parametrs = new ValueRange(id, firstName, lastName, dateofBirth, cabinetNumber, salary, category);

                fileCabinetService.EditRecord(parametrs);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine($"Record #{id} is updated.");
        }

        private static void Find(string parametrs)
        {
            const int CriterionIndex = 0;
            const int InputValueIndex = 1;

            const string CriterionFirstName = "firstName";
            const string CriterionLastName = "lastName";
            const string CriterionDateOfBirth = "dateofBirth";

            FileCabinetRecord[] result = Array.Empty<FileCabinetRecord>();

            var paramArray = parametrs.Split(' ', 2);
            if (paramArray.Length != 2)
            {
                Console.WriteLine("Input is not correct.");
                return;
            }

            string criterion = paramArray[CriterionIndex];
            string inputValue = paramArray[InputValueIndex];
            if (inputValue.First() != '"' && inputValue.Last() != '"')
            {
                Console.WriteLine("second params must be \"\"");
                return;
            }

            if (criterion.Equals(CriterionFirstName, StringComparison.InvariantCultureIgnoreCase))
            {
                string firstName = inputValue.Trim('"').ToUpperInvariant();

                result = fileCabinetService.FindByFirstName(firstName);
            }

            if (criterion.Equals(CriterionLastName, StringComparison.InvariantCultureIgnoreCase))
            {
                string lastName = inputValue.Trim('"').ToUpperInvariant();

                result = fileCabinetService.FindByLastName(lastName);
            }

            if (criterion.Equals(CriterionDateOfBirth, StringComparison.InvariantCultureIgnoreCase))
            {
                string dateOfBirth = inputValue.Trim('"').ToUpperInvariant();
                var parseddateofBirth = DateTime.TryParseExact(dateOfBirth, "yyyy-MMM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateofBirth);
                if (!parseddateofBirth)
                {
                    Console.WriteLine("Invalid Date of birth.");
                }

                string dateOfBirthString = dateofBirth.ToString(CultureInfo.InvariantCulture);

                result = fileCabinetService.FindByDateOfBirth(dateOfBirthString);
            }

            foreach (var item in result)
            {
                var recordString = new StringBuilder();

                recordString.Append($"#{item.Id}, ");
                recordString.Append($"{item.FirstName}, ");
                recordString.Append($"{item.LastName}, ");
                recordString.Append($"{item.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, ");

                Console.WriteLine(recordString);
            }
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }
    }
}