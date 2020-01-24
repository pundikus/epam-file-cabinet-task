using System;
using System.Globalization;
using System.Linq;
using static FileCabinetApp.Program;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'remove' command.
    /// </summary>
    public class RemoveCommandHandler : CommandHandlerBase
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

            if (request.Command.ToUpperInvariant() == "REMOVE")
            {
                Remove(request.Parameters);
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private static void Remove(string parameters)
        {
            var parsedId = int.TryParse(parameters, NumberStyles.Integer, CultureInfo.InvariantCulture, out int id);
            if (!parsedId)
            {
                Console.WriteLine("Record #" + Program.FileCabinetService.RemoveRecord(id) + " doesn't exists");

                return;
            }

            var listRecords = Program.FileCabinetService.GetRecords();

            if (!listRecords.Any(x => x.Id == id))
            {
                Console.WriteLine($"Record #{id} doesn't exists.");
                return;
            }

            int removedId = Program.FileCabinetService.RemoveRecord(id);

            if (IsStorageFile)
            {
                Console.WriteLine("Record #" + removedId + " marked as deleted.");
            }
            else
            {
                Console.WriteLine("Record #" + removedId + " is removed.");
            }
        }
    }
}
