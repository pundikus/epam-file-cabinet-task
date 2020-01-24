using System;
using System.Globalization;
using System.Linq;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'remove' command.
    /// </summary>
    public class RemoveCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service to remove record.</param>
        public RemoveCommandHandler(IFileCabinetService fileCabinetService)
        {
            this.service = fileCabinetService;
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

            if (request.Command.ToUpperInvariant() == "REMOVE")
            {
                this.Remove(request.Parameters);
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void Remove(string parameters)
        {
            var parsedId = int.TryParse(parameters, NumberStyles.Integer, CultureInfo.InvariantCulture, out int id);
            if (!parsedId)
            {
                Console.WriteLine("Record #" + this.service.RemoveRecord(id) + " doesn't exists");

                return;
            }

            var listRecords = this.service.GetRecords();

            if (!listRecords.Any(x => x.Id == id))
            {
                Console.WriteLine($"Record #{id} doesn't exists.");
                return;
            }

            int removedId = this.service.RemoveRecord(id);

            Console.WriteLine("Record #" + removedId + " is removed.");
        }
    }
}
