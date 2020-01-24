using System;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'purge' command.
    /// </summary>
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service to purge.</param>
        public PurgeCommandHandler(IFileCabinetService fileCabinetService)
        {
            this.service = fileCabinetService;
        }

        /// <summary>
        /// Gets or sets it is count removed records.
        /// </summary>
        /// <value>
        /// It is count removed records.
        /// </value>
        public static int CountRemovedRecord { get; set; }

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
            int countRecords = this.service.GetRecords().Count;
            int countRemovedRecords;

            countRemovedRecords = this.service.PurgeRecords();

            CountRemovedRecord += countRemovedRecords;

            Console.WriteLine($"Data file procesing is completed: {countRemovedRecords} of {countRecords} records were purged.");
        }
    }
}
