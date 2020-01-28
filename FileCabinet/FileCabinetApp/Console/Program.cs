using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FileCabinetApp.Handlers;
using FileCabinetApp.Helpers;
using FileCabinetApp.Interfaces.Validators;

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

        private static List<string> commandNames = new List<string>();

        private static IFileCabinetService fileCabinetService;
        private static IRecordValidator validator = ValidatorExtensions.CreateDefault(new ValidatorBuilder());

        private static bool isRunning = true;
        private static bool isStorageFile;
        private static bool stopwatchAdded;
        private static bool loggerAdded;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets.
        /// </summary>
        /// <value>
        /// A value indicating whether gets or sets.
        /// </value>
        public static bool IsModeCustom { get; set; }

        /// <summary>
        /// This is the main method in which all kinds of user interaction is performed.
        /// </summary>
        /// <param name="args">Parameters command-line.</param>
        public static void Main(string[] args)
        {
            var assemblyName = "FileCabinetApp";
            var nameSpace = "FileCabinetApp.CommandHandlers";
            var asm = Assembly.Load(assemblyName);
            var handlers = asm.GetTypes().Where(p => p.Namespace == nameSpace && p.Name.EndsWith("CommandHandler") && !p.IsInterface).ToList();
            foreach (var handler in handlers)
            {
                int indexOfCommandWord = handler.Name.IndexOf("CommandHandler");
                commandNames.Add(handler.Name.Substring(0, indexOfCommandWord).ToLower());
            }

            if (args != null && args.Length > 0)
            {
                CheckModeValidation(args, FullParameterValid, CustomParametrs, FullParameterStor, AbbreviatedParameterStor, FileParameters, AbbreviatedParameterValid);
            }

            StreamWriter logWriter = null;

            if (stopwatchAdded)
            {
                fileCabinetService = new ServiceMeter(fileCabinetService);
            }

            if (loggerAdded)
            {
                logWriter = new StreamWriter("logs.txt", true, System.Text.Encoding.UTF8);
                fileCabinetService = new ServiceLogger(fileCabinetService, logWriter);
            }

            var commandHandler = CreateCommandHandlers();

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
                // commandHandler.Handle(new AppCommandRequest(command, parameters));

                var wasSucceed = commandHandler.Handle(new AppCommandRequest(command, parameters));
                if (wasSucceed == null)
                {
                    Console.WriteLine("\'" + command + "\' is not a valid command. See 'help'.");
                    List<string> similarCommands = CommandHints(command);
                    if (similarCommands.Count == 1)
                    {
                        Console.WriteLine("The most similar command is ");
                        Console.WriteLine("\t\t" + similarCommands[0]);
                    }
                    else if (similarCommands.Count > 1)
                    {
                        Console.WriteLine("The most similar commands are ");
                        foreach (var com in similarCommands)
                        {
                            Console.WriteLine("\t\t" + com);
                        }
                    }
                }
            }
            while (isRunning);

            if (loggerAdded == true)
            {
                logWriter.Close();
            }
        }

        private static void ChangeState(bool value)
        {
            isRunning = value;
        }

        private static ICommandHandler CreateCommandHandlers()
        {
            var recordPrinter = new DefaultRecordPrinter();
            var createHandler = new CreateCommandHandler(fileCabinetService);
            var editHandler = new EditCommandHandler(fileCabinetService);
            var findHandler = new FindCommandHandler(fileCabinetService, recordPrinter);
            var listHandler = new ListCommandHandler(fileCabinetService, recordPrinter);
            var statHandler = new StatCommandHandler(fileCabinetService);
            var exportHandler = new ExportCommandHandler(fileCabinetService);
            var importHandler = new ImportCommandHandler(fileCabinetService);
            var insertHandler = new InsertCommandHandler(fileCabinetService);
            var deleteHandler = new DeleteCommandHandler(fileCabinetService);
            //var removeHandler = new RemoveCommandHandler(fileCabinetService);
            var purgeHandler = new PurgeCommandHandler(fileCabinetService);
            var helpHandler = new HelpCommandHandler();
            var exitHandler = new ExitCommandHandler(ChangeState);

            createHandler.SetNext(editHandler)
                         //.SetNext(removeHandler)
                         .SetNext(insertHandler)
                         .SetNext(deleteHandler)
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

        private static List<string> CommandHints(string command)
        {
            List<string> similarCommands = new List<string>();
            int averageLength = 5;

            int toMatch = command.Length <= averageLength ? command.Length : command.Length / 2;

            List<char> letters = new List<char>();
            char[] startsWith = new char[toMatch];
            for (int i = 0; i < toMatch; i++)
            {
                startsWith[i] = command[i];
            }

            for (int i = 0; i < commandNames.Count; i++)
            {
                int toCheck = toMatch < commandNames[i].Length ? toMatch : commandNames[i].Length;
                if (new string(startsWith) == commandNames[i].Substring(0, toCheck))
                {
                    if (!similarCommands.Contains(commandNames[i]))
                    {
                        similarCommands.Add(commandNames[i]);
                    }
                }
            }

            foreach (char c in command)
            {
                if (!letters.Contains(c))
                {
                    letters.Add(c);
                }
            }

            int matchedChars = 0;
            List<char> tempLetters = new List<char>();
            foreach (var com in commandNames)
            {
                foreach (char c in letters)
                {
                    tempLetters.Add(c);
                }

                foreach (char c in com)
                {
                    if (tempLetters.Contains(c))
                    {
                        matchedChars++;
                        if (tempLetters.Contains(c))
                        {
                            tempLetters.Remove(c);
                        }
                    }
                }

                if (matchedChars >= toMatch)
                {
                    if (!similarCommands.Contains(com))
                    {
                        similarCommands.Add(com);
                    }
                }

                matchedChars = 0;
                tempLetters.Clear();
            }

            return similarCommands;
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
                    validator = ValidatorExtensions.CreateСustom(new ValidatorBuilder());

                    CheckStorageInput(fullParameterStor, abbreviatedParameterStor, args, fileParameters);
                    CheckStopWatchAndLogger(args);
                }
                else
                {
                    CheckStorageInput(fullParameterStor, abbreviatedParameterStor, args, fileParameters);
                    CheckStopWatchAndLogger(args);
                }
            }
            else if (args[ModeParameterIndex].Contains(abbreviatedParameterValid, StringComparison.InvariantCulture))
            {
                if (args[ModeParameterValue].Equals(customParametrs, StringComparison.InvariantCultureIgnoreCase))
                {
                    IsModeCustom = true;
                    validator = ValidatorExtensions.CreateСustom(new ValidatorBuilder());

                    CheckStorageInput(fullParameterStor, abbreviatedParameterStor, args, fileParameters);
                    CheckStopWatchAndLogger(args);
                }
                else
                {
                    CheckStorageInput(fullParameterStor, abbreviatedParameterStor, args, fileParameters);
                    CheckStopWatchAndLogger(args);
                }
            }
            else
            {
                CheckStorageInput(fullParameterStor, abbreviatedParameterStor, args, fileParameters);
                CheckStopWatchAndLogger(args);
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

                        if (!File.Exists(FileName))
                        {
                            fileStream = File.Create(FileName);
                        }
                        else
                        {
                            fileStream = File.Open(FileName, FileMode.Open);
                        }

                        fileCabinetService = new FileCabinetFilesystemService(fileStream, validator);
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

                        fileCabinetService = new FileCabinetFilesystemService(fileStream, validator);
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

        private static void CheckStopWatchAndLogger(string[] args)
        {
            foreach (var item in args)
            {
                if (item == "--use-stopwatch")
                {
                    stopwatchAdded = true;
                }
                else if (item == "--use-logger")
                {
                    loggerAdded = true;
                }
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

            if (isStorageFile)
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