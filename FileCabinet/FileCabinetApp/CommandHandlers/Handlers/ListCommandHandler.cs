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
        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service to show records.</param>
        public ListCommandHandler(IFileCabinetService fileCabinetService)
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

                foreach (var item in listRecords)
                {
                    var recordString = new StringBuilder();

                    recordString.Append($"#{item.Id}, ");
                    recordString.Append($"{item.FirstName}, ");
                    recordString.Append($"{item.LastName}, ");
                    recordString.Append($"{item.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, ");
                    recordString.Append($"{item.CabinetNumber}, ");
                    recordString.Append($"{item.Salary}y.e, ");
                    recordString.Append($"{item.Category}");

                    Console.WriteLine(recordString);
                }
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("List is empty!");
                return;
            }
        }
    }
}
