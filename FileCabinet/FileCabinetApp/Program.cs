using System;
using System.IO;
using FileCabinetApp.Handlers;

namespace FileCabinetApp
{
    /// <summary>
    /// This class is needed for user interaction.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Nikita Pundis";
        private const string DefaultMessage = "Using default validation rules.";
        private const string CustomMessage = "Using custom validation rules.";
        private const string MemoryMessage = "Using memory storage.";
        private const string FileMessage = "Using file storage.";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const string FileName = @"f:\cabinet-records.db";

        private const string FullParameterStor = "--storage";
        private const string AbbreviatedParameterStor = "-s";
        private const string FullParameterValid = "--validation-rules";
        private const string AbbreviatedParameterValid = "-v";
        private const string FileParameters = "file";
        private const string CustomParametrs = "custom";

        private const int ModeParameterIndex = 0;
        private const int ModeParameterValue = 1;
        private const int StorageParameterIndex = 2;

        private static IRecordValidator validator = new DefaultValidator();

        /// <summary>
        /// Gets or sets it is count removed records.
        /// </summary>
        /// <value>
        /// It is count removed records.
        /// </value>
        public static int CountRemovedRecord { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets.
        /// </summary>
        /// <value>
        /// A value indicating whether gets or sets.
        /// </value>
        public static bool IsModeCustom { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets.
        /// </summary>
        /// <value>
        /// A value indicating whether gets or sets.
        /// </value>
        public static bool IsStorageFile { get; set; }

        /// <summary>
        /// Gets the instance of service.
        /// </summary>
        /// <value>The instance of service.</value>
        public static IFileCabinetService FileCabinetService { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the program is running or not.
        /// </summary>
        /// <value>Whether the program is running or not.</value>
        public static bool IsRunning { get; set; }

        /// <summary>
        /// This is the main method in which all kinds of user interaction is performed.
        /// </summary>
        /// <param name="args">Parameters command-line.</param>
        public static void Main(string[] args)
        {
            var commandHandler = CreateCommandHandlers();

            IsRunning = true;

            if (args != null && args.Length > 0)
            {
                CheckModeValidation(args, FullParameterValid, CustomParametrs, FullParameterStor, AbbreviatedParameterStor, FileParameters, AbbreviatedParameterValid);
            }

            WriteMessageSettings();

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

                const int parametersIndex = 1;
                var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                commandHandler.Handle(new AppCommandRequest(command, parameters));
            }
            while (IsRunning);
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var createHandler = new CreateCommandHandler();
            var editHandler = new EditCommandHandler();
            var findHandler = new FindCommandHandler();
            var listHandler = new ListCommandHandler();
            var statHandler = new StatCommandHandler();
            var exportHandler = new ExportCommandHandler();
            var importHandler = new ImportCommandHandler();
            var removeHandler = new RemoveCommandHandler();
            var purgeHandler = new PurgeCommandHandler();
            var helpHandler = new HelpCommandHandler();
            var exitHandler = new ExitCommandHandler();

            createHandler.SetNext(editHandler)
                         .SetNext(removeHandler)
                         .SetNext(findHandler)
                         .SetNext(listHandler)
                         .SetNext(statHandler)
                         .SetNext(exportHandler)
                         .SetNext(importHandler)
                         .SetNext(purgeHandler)
                         .SetNext(helpHandler)
                         .SetNext(exitHandler);

            return createHandler;
        }

        private static void CheckModeValidation(string[] args, string fullParameterValid, string customParametrs, string fullParameterStor, string abbreviatedParameterStor, string fileParameters, string abbreviatedParameterValid)
        {
            var inputsArrayParamsMode = Array.Empty<string>();

            if (args[ModeParameterIndex].Contains(fullParameterValid, StringComparison.InvariantCulture))
            {
                int modeIndex = 1;

                inputsArrayParamsMode = args[ModeParameterIndex].Split('=', 2);

                if (inputsArrayParamsMode[modeIndex].Equals(customParametrs, StringComparison.InvariantCultureIgnoreCase))
                {
                    IsModeCustom = true;
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
                    IsModeCustom = true;
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
                        IsStorageFile = true;
                        FileStream fileStream;

                        if (IsStorageFile)
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

                        Program.FileCabinetService = new FileCabinetFilesystemService(fileStream);
                    }
                    else
                    {
                        Program.FileCabinetService = new FileCabinetMemoryService(validator);
                    }
                }
                else if (inputsArrayParamsStorage[2].Equals(abbreviatedParameterStor, StringComparison.InvariantCulture))
                {
                    if (inputsArrayParamsStorage[3].Equals(fileParameters, StringComparison.InvariantCultureIgnoreCase))
                    {
                        IsStorageFile = true;

                        if (IsStorageFile)
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

                        Program.FileCabinetService = new FileCabinetFilesystemService(fileStream);
                    }
                    else
                    {
                        Program.FileCabinetService = new FileCabinetMemoryService(validator);
                    }
                }
                else
                {
                    Program.FileCabinetService = new FileCabinetMemoryService(validator);
                }
            }
            catch (IndexOutOfRangeException)
            {
                Program.FileCabinetService = new FileCabinetMemoryService(validator);
            }
        }

        private static void WriteMessageSettings()
        {
            Console.WriteLine($"File Cabinet Application, developed by {DeveloperName}");

            if (IsModeCustom)
            {
                Console.WriteLine(CustomMessage);
            }
            else
            {
                Console.WriteLine(DefaultMessage);
            }

            if (IsStorageFile)
            {
                Console.WriteLine(FileMessage);
            }
            else
            {
                Console.WriteLine(MemoryMessage);
            }

            Console.WriteLine(HintMessage);
            Console.WriteLine();
        }
    }
}