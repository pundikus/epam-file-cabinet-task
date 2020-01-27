using System;
using System.Linq;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'edit' command.
    /// </summary>
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service to edit record.</param>
        public EditCommandHandler(IFileCabinetService fileCabinetService)
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

            if (request.Command.ToUpperInvariant() == "EDIT")
            {
                this.Edit(request.Parameters);
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private int Edit(string parameters)
        {
            var parsedId = int.TryParse(parameters, out int id);
            if (!parsedId)
            {
                Console.WriteLine("Invalid Id");
            }

            var listRecords = this.service.GetRecords();

            if (!listRecords.Any(x => x.Id == id))
            {
                Console.WriteLine($"#{id} record is not found.");
                return id;
            }

            Console.Write("First name: ");
            string firstName = this.ReadInput(this.StringConverter, this.FirstNameValidator);

            Console.Write("Last name: ");
            string lastName = this.ReadInput(this.StringConverter, this.LastNameValidator);

            Console.Write("Date of birth: ");
            var dateofBirth = this.ReadInput(this.DateTimeConverter, this.DateOfBirthValidator);

            Console.Write("Cabinet number: ");
            var cabinetNumber = this.ReadInput(this.ShortConverter, this.CabinetNumberValidator);

            Console.Write("Salary: ");
            var salary = this.ReadInput(this.DecimalConverter, this.SalaryValidator);

            Console.Write("Category(A, B, C): ");
            var category = this.ReadInput(this.CharConverter, this.CategoryValidator);

            try
            {
                FileCabinetRecord parametrs = new FileCabinetRecord(id, firstName, lastName, dateofBirth, cabinetNumber, salary, category);

                this.service.EditRecord(parametrs);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return id;
            }

            Console.WriteLine($"Record #{id} is updated.");

            return id;
        }
    }
}
