using System;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'list' command.
    /// </summary>
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private readonly IRecordPrinter printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service to show records.</param>
        /// <param name="printer">Printer for records.</param>
        public ListCommandHandler(IFileCabinetService fileCabinetService, IRecordPrinter printer)
        {
            this.service = fileCabinetService;
            this.printer = printer;
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

            if (request.Command.ToUpperInvariant() == "LIST")
            {
                this.List();
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void List()
        {
            try
            {
                var listRecords = this.service.GetRecords();

                if (listRecords.Count == 0)
                {
                    Console.WriteLine("List records is Empty.");
                }

                this.printer.Print(listRecords);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("List is empty!");
                return;
            }
        }
    }
}
