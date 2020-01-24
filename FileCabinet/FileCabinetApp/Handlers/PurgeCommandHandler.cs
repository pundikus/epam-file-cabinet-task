using System;
using static FileCabinetApp.Program;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'purge' command.
    /// </summary>
    public class PurgeCommandHandler : CommandHandlerBase
    {
        private readonly IFileCabinetService fileCabinetService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service to purge.</param>
        public PurgeCommandHandler(IFileCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService;
        }

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
                this.Purge();
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void Purge()
        {
            int countRecords = this.fileCabinetService.GetRecords().Count;
            int countRemovedRecords;

            if (IsStorageFile)
            {
                countRemovedRecords = this.fileCabinetService.PurgeRecords();

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
