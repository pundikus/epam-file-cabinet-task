using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// This class is needed for user interaction.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Nikita Pundis";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const string DefaultMessage = "Using default validation rules.";
        private const string CustomMessage = "Using custom validation rules.";
        private const string MemoryMessage = "Using memory storage.";
        private const string FileMessage = "Using file storage.";
        private const string FileName = @"f:\cabinet-records.db";

        private const int ModeParameterIndex = 0;
        private const int ModeParameterValue = 1;
        private const int StorageParameterIndex = 2;

        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static IRecordValidator validator = new DefaultValidator();

        private static IFileCabinetService fileCabinetService;

        private static bool isRunning = true;
        private static bool isModeCustom = false;
        private static bool isStorageFile = false;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("import", Import),
            new Tuple<string, Action<string>>("remove", Remove),
            new Tuple<string, Action<string>>("purge", Purge),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "gets the number of records", "The 'stat' command returns the number of records." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new string[] { "list", "returns a list of records", "The 'list' command returns a list of records." },
            new string[] { "edit", "changes record", "The 'edit' command changes record." },
            new string[] { "find", "find records", "The 'find' command search records by input value." },
            new string[] { "export", "export all records to file", "The 'export' command export all records to file  various format." },
            new string[] { "import", "import all records from file", "The 'import' command import all records from file various format." },
            new string[] { "purge", "delete marks record from binary file", "The 'perge' command delete all records marked for deletion (only Filesystemservice)." },
        };

        /// <summary>
        /// This is the main method in which all kinds of user interaction is performed.
        /// </summary>
        /// <param name="args">Parameters command-line.</param>
        public static void Main(string[] args)
        {
            const string fullParameterStor = "--storage";
            const string abbreviatedParameterStor = "-s";
            const string fullParameterValid = "--validation-rules";
            const string abbreviatedParameterValid = "-v";
            const string fileParameters = "file";
            const string customParametrs = "custom";

            var inputsArrayParamsMode = Array.Empty<string>();

            if (args[ModeParameterIndex].Contains(fullParameterValid, StringComparison.InvariantCulture))
            {
                int modeIndex = 1;

                inputsArrayParamsMode = args[ModeParameterIndex].Split('=', 2);

                if (inputsArrayParamsMode[modeIndex].Equals(customParametrs, StringComparison.InvariantCultureIgnoreCase))
                {
                    isModeCustom = true;
                    validator = new CustomValidator();

                    CheckStorageInput(fullParameterStor, abbreviatedParameterStor, args, fileParameters);
                }
                else
                {
                    CheckStorageInput(fullParameterStor, abbreviatedParameterStor, args, fileParameters);
                }
            }
            else if (args[ModeParameterIndex].Contains(abbreviatedParameterValid, StringComparison.InvariantCulture))
            {
                if (args[ModeParameterValue].Equals(customParametrs, StringComparison.InvariantCultureIgnoreCase))
                {
                    isModeCustom = true;
                    validator = new CustomValidator();

                    CheckStorageInput(fullParameterStor, abbreviatedParameterStor, args, fileParameters);
                }
                else
                {
                    CheckStorageInput(fullParameterStor, abbreviatedParameterStor, args, fileParameters);
                }
            }
            else
            {
                CheckStorageInput(fullParameterStor, abbreviatedParameterStor, args, fileParameters);
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

            if (isStorageFile)
            {
                Console.WriteLine(FileMessage);
            }
            else
            {
                Console.WriteLine(MemoryMessage);
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

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
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
                    string firstName = ReadInput(StringConverter, FirstNameValidator);

                    Console.Write("Last name: ");
                    string lastName = ReadInput(StringConverter, LastNameValidator);

                    Console.Write("Date of birth: ");
                    var dateofBirth = ReadInput(DateTimeConverter, DateOfBirthValidator);

                    Console.Write("Cabinet number: ");
                    var cabinetNumber = ReadInput(ShortConverter, CabinetNumberValidator);

                    Console.Write("Salary: ");
                    var salary = ReadInput(DecimalConverter, SalaryValidator);

                    Console.Write("Category(A, B, C): ");
                    var category = ReadInput(CharConverter, CategoryValidator);

                    FileCabinetRecord parametrs = new FileCabinetRecord(firstName, lastName, dateofBirth, cabinetNumber, salary, category);

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

        private static Tuple<bool, string, string> StringConverter(string arg)
        {
            return new Tuple<bool, string, string>(typeof(string).Equals(arg.GetType()), arg, arg.ToString(CultureInfo.InvariantCulture));
        }

        private static Tuple<bool, string, DateTime> DateTimeConverter(string arg)
        {
            bool parsedDateTime = DateTime.TryParseExact(arg, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime);

            return new Tuple<bool, string, DateTime>(parsedDateTime, arg, dateTime);
        }

        private static Tuple<bool, string, short> ShortConverter(string arg)
        {
            var parsedCabinetNumber = short.TryParse(arg, out short cabinetNumber);

            return new Tuple<bool, string, short>(parsedCabinetNumber, arg, cabinetNumber);
        }

        private static Tuple<bool, string, decimal> DecimalConverter(string arg)
        {
            var parsedSalary = decimal.TryParse(arg, out decimal salary);

            return new Tuple<bool, string, decimal>(parsedSalary, arg, salary);
        }

        private static Tuple<bool, string, char> CharConverter(string arg)
        {
            var parsedCategory = char.TryParse(arg, out char category);

            return new Tuple<bool, string, char>(parsedCategory, arg, category);
        }

        private static Tuple<bool, string> FirstNameValidator(string arg)
        {
            bool isValid = true;
            if (isModeCustom)
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

        private static Tuple<bool, string> LastNameValidator(string arg)
        {
            bool isVaild = true;

            if (isModeCustom)
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

        private static Tuple<bool, string> DateOfBirthValidator(DateTime arg)
        {
            bool isValid = true;

            if (isModeCustom)
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

        private static Tuple<bool, string> CabinetNumberValidator(short arg)
        {
            bool flag = true;
            if (isModeCustom)
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

        private static Tuple<bool, string> SalaryValidator(decimal arg)
        {
            bool flag = true;
            if (isModeCustom)
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

        private static Tuple<bool, string> CategoryValidator(char arg)
        {
            bool flag = true;
            if (isModeCustom)
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

        private static void List(string parameters)
        {
            try
            {
                var listRecords = fileCabinetService.GetRecords();

                if (listRecords.Count == 0)
                {
                    Console.WriteLine("List records is Empty.");
                }

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
            catch (ArgumentNullException)
            {
                Console.WriteLine("List is empty!");
                return;
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
            string firstName = ReadInput(StringConverter, FirstNameValidator);

            Console.Write("Last name: ");
            string lastName = ReadInput(StringConverter, LastNameValidator);

            Console.Write("Date of birth: ");
            var dateofBirth = ReadInput(DateTimeConverter, DateOfBirthValidator);

            Console.Write("Cabinet number: ");
            var cabinetNumber = ReadInput(ShortConverter, CabinetNumberValidator);

            Console.Write("Salary: ");
            var salary = ReadInput(DecimalConverter, SalaryValidator);

            Console.Write("Category(A, B, C): ");
            var category = ReadInput(CharConverter, CategoryValidator);

            try
            {
                FileCabinetRecord parametrs = new FileCabinetRecord(id, firstName, lastName, dateofBirth, cabinetNumber, salary, category);

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

            ReadOnlyCollection<FileCabinetRecord> result = null;

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

        private static void Export(string parametrs)
        {
            const int formatIndex = 0;
            const int pathIndex = 1;

            const string formatCsv = "csv";
            const string formatXml = "xml";

            var parametersArrayForFromat = parametrs.Split(" ", 2);
            string format = parametersArrayForFromat[formatIndex];

            if (parametersArrayForFromat.Length == 1)
            {
                Console.WriteLine("Incorrect input");
                return;
            }

            string path = parametersArrayForFromat[pathIndex];

            if (!path.Contains(format, StringComparison.InvariantCulture))
            {
                Console.WriteLine("Incorrect format");
                return;
            }

            StreamWriter streamWriter;

            bool rewrite = false;

            try
            {
                if (File.Exists(path))
                {
                    Console.Write("File is exist - rewrite " + parametrs + "? [Y/n]");
                    string result = Console.ReadLine();

                    if (result == "Y")
                    {
                        streamWriter = new StreamWriter(path, rewrite);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                     streamWriter = new StreamWriter(path);
                }

                var snapshot = fileCabinetService.MakeSnapshot();

                if (format == formatCsv)
                {
                    snapshot.SaveToCsv(streamWriter);
                }

                if (format == formatXml)
                {
                    snapshot.SaveToXml(streamWriter);
                }

                streamWriter.Close();

                Console.WriteLine("All records are exported to file " + path);
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("Export failed: " + ex.Message);
                return;
            }
        }

        private static void CheckStorageInput(string fullParameterStor, string abbreviatedParameterStor, string[] inputsArrayParamsStorage, string fileParameters)
        {
            try
            {
                if (inputsArrayParamsStorage[StorageParameterIndex - 1].Contains(fullParameterStor, StringComparison.InvariantCulture))
                {
                    var inputsArray = inputsArrayParamsStorage[StorageParameterIndex - 1].Split('=', 2);
                    if (inputsArray[1].Equals(fileParameters, StringComparison.InvariantCultureIgnoreCase))
                    {
                        isStorageFile = true;
                        FileStream fileStream;

                        if (isStorageFile)
                        {
                            File.Delete(FileName);
                        }

                        if (!File.Exists(FileName))
                        {
                            fileStream = File.Create(FileName);
                        }
                        else
                        {
                            fileStream = File.Open(FileName, FileMode.Open);
                        }

                        fileCabinetService = new FileCabinetFilesystemService(fileStream);
                    }
                    else
                    {
                        fileCabinetService = new FileCabinetMemoryService(validator);
                    }
                }
                else if (inputsArrayParamsStorage[2].Equals(abbreviatedParameterStor, StringComparison.InvariantCulture))
                {
                    if (inputsArrayParamsStorage[3].Equals(fileParameters, StringComparison.InvariantCultureIgnoreCase))
                    {
                        isStorageFile = true;

                        if (isStorageFile)
                        {
                            File.Delete(FileName);
                        }

                        FileStream fileStream;

                        if (File.Exists(FileName))
                        {
                            fileStream = File.Open(FileName, FileMode.Open);
                        }
                        else
                        {
                            fileStream = File.Create(FileName);
                        }

                        fileCabinetService = new FileCabinetFilesystemService(fileStream);
                    }
                    else
                    {
                        fileCabinetService = new FileCabinetMemoryService(validator);
                    }
                }
                else
                {
                    fileCabinetService = new FileCabinetMemoryService(validator);
                }
            }
            catch (IndexOutOfRangeException)
            {
                fileCabinetService = new FileCabinetMemoryService(validator);
            }
        }

        private static void Import(string parametrs)
        {
            const string formatCsv = "csv";
            const string formatXml = "xml";

            string[] parameters = parametrs.Split(' ', 2);
            string path = parameters[1];

            var snapshot = new FileCabinetServiceSnapshot(fileCabinetService.GetRecords());

            if (File.Exists(path))
            {
                using (FileStream fileStream = File.OpenRead(path))
                {
                    StreamReader streamReader = new StreamReader(fileStream);

                    IList<FileCabinetRecord> records;

                    if (parameters[0].Equals(formatCsv, StringComparison.InvariantCultureIgnoreCase))
                    {
                        records = snapshot.LoadFromCsv(streamReader);

                        snapshot = new FileCabinetServiceSnapshot(records);

                        fileCabinetService.Restore(snapshot);
                    }
                    else if (parameters[0].Equals(formatXml, StringComparison.InvariantCultureIgnoreCase))
                    {
                        records = snapshot.LoadFromXml(streamReader);

                        snapshot = new FileCabinetServiceSnapshot(records);

                        fileCabinetService.Restore(snapshot);
                    }

                    Console.WriteLine(snapshot.Records.Count + " records were imported from " + path);

                    streamReader.Dispose();
                }
            }
            else
            {
                Console.WriteLine("Import error: file " + path + " is not exist.");
                return;
            }
        }

        private static void Remove(string parameters)
        {
            var parsedId = int.TryParse(parameters, NumberStyles.Integer, CultureInfo.InvariantCulture, out int id);
            if (!parsedId)
            {
                Console.WriteLine("Record #" + fileCabinetService.RemoveRecord(id) + " doesn't exists");

                return;
            }

            var listRecords = fileCabinetService.GetRecords();

            if (!listRecords.Any(x => x.Id == id))
            {
                Console.WriteLine($"Record #{id} doesn't exists.");
                return;
            }

            int removedId = fileCabinetService.RemoveRecord(id);

            if (isStorageFile)
            {
                Console.WriteLine("Record #" + removedId + " marked as deleted.");
            }
            else
            {
                Console.WriteLine("Record #" + removedId + " is removed.");
            }
        }

        private static void Purge(string parameters)
        {
            int countRecords = fileCabinetService.GetRecords().Count;
            int countRemovedRecords = 0;
            if (isStorageFile)
            {
                countRemovedRecords = fileCabinetService.PurgeRecords();

                Console.WriteLine($"Data file procesing is completed: {countRemovedRecords} of {countRecords} records were purged.");
            }
            else
            {
                Console.WriteLine("FileCabinetMemoryService does not support this command.");
                return;
            }
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }
    }
}