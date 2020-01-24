using System;
using static FileCabinetApp.Program;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'purge' command.
    /// </summary>
    public class PurgeCommandHandler : CommandHandlerBase
    {
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

            if (request.Command.ToUpperInvariant() == "PURGE")
            {
                Purge();
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private static void Purge()
        {
            int countRecords = FileCabinetService.GetRecords().Count;
            int countRemovedRecords;

            if (IsStorageFile)
            {
                countRemovedRecords = FileCabinetService.PurgeRecords();

                CountRemovedRecord += countRemovedRecords;

                Console.WriteLine($"Data file procesing is completed: {countRemovedRecords} of {countRecords} records were purged.");
            }
            else
            {
                Console.WriteLine("FileCabinetMemoryService does not support this command.");
                return;
            }
        }
    }
}
