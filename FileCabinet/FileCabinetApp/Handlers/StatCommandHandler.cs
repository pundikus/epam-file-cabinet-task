using System;
using System.Collections.Generic;
using System.Text;
using static FileCabinetApp.Program;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'stat' command.
    /// </summary>
    public class StatCommandHandler : CommandHandlerBase
    {
        private static int countRemovedRecord = 0;

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="request">Request to handle.</param>
        /// <returns>The object back, or null otherwise.</returns>
        public override object Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Command.ToUpperInvariant() == "STAT")
            {
                Stat();
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private static void Stat()
        {
            int recordsCount = Program.FileCabinetService.GetStat();

            if (IsStorageFile)
            {
                Console.WriteLine($"{recordsCount} record(s).");
                Console.WriteLine($"{countRemovedRecord} records was deleted.");
            }
            else
            {
                Console.WriteLine($"{recordsCount} record(s).");
                Console.WriteLine("0 records was deleted.");
            }
        }
    }
}
