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
    }
}
