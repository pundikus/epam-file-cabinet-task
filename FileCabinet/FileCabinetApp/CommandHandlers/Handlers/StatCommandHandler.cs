using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'stat' command.
    /// </summary>
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        private static int countRemovedRecord = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service to get stats.</param>
        public StatCommandHandler(IFileCabinetService fileCabinetService)
        {
            this.Service = fileCabinetService;
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

            if (request.Command.ToUpperInvariant() == "STAT")
            {
                this.Stat();
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void Stat()
        {
            int recordsCount = this.Service.GetStat();

            Console.WriteLine($"{recordsCount} record(s).");
            Console.WriteLine($"{countRemovedRecord} records was deleted.");
        }
    }
}
