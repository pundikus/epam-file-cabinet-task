using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Nikita Pundis";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static FileCabinetService fileCabinetService = new FileCabinetService();

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "gets the number of records", "The 'stat' command returns the number of records." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record" },
            new string[] { "list", "returns a list of records", "The 'list' command returns a list of records" },
            new string[] { "edit", "changes record", "The 'edit' changes record" },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
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
            var recordsCount = Program.fileCabinetService.GetStat();
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
                    var dateofBirth = DateTime.Parse(dateofBirthString, new CultureInfo("en-US", false));

                    Console.Write("Cabinet number: ");
                    string cabinetNumberString = Console.ReadLine();
                    var parsedCabinetNumber = short.TryParse(cabinetNumberString, out short cabinetNumber);

                    if (!parsedCabinetNumber)
                    {
                        Console.WriteLine("Invalid Cabinet number.");
                        Console.Clear();
                        break;
                    }

                    Console.Write("Salary: ");
                    string salaryString = Console.ReadLine();
                    var parsedSalary = decimal.TryParse(salaryString, out decimal salary);

                    if (!parsedSalary)
                    {
                        Console.WriteLine("Invalid Salary.");
                        Console.Clear();
                        break;
                    }

                    Console.Write("Category(A, B, C): ");
                    string categoryString = Console.ReadLine();
                    var parsedCategory = char.TryParse(categoryString, out char category);

                    if (!parsedCategory)
                    {
                        Console.WriteLine("Invalid Category.");
                        Console.Clear();
                        break;
                    }

                    var record = Program.fileCabinetService.CreateRecord(firstName, lastName, dateofBirth, cabinetNumber, salary, category);

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
            var listRecords = Program.fileCabinetService.GetRecords();

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
            if (Program.fileCabinetService.GetRecords() == null)
            {
                Console.WriteLine("List records is Empty!");
                return;
            }

            var parsedId = int.TryParse(parameters, out int id);

            if (!parsedId)
            {
                Console.WriteLine("Invalid Id");
            }

            var listRecords = Program.fileCabinetService.GetRecords();
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
            var dateofBirth = DateTime.Parse(dateofBirthString, new CultureInfo("en-US", false));

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
            string categoryString = Console.ReadLine();
            var parsedCategory = char.TryParse(categoryString, out char category);

            if (!parsedCategory)
            {
                Console.WriteLine("Invalid Category.");
            }

            Program.fileCabinetService.EditRecord(id, firstName, lastName, dateofBirth, cabinetNumber, salary, category);
            Console.WriteLine($"Record #{id} is updated.");
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }
    }
}