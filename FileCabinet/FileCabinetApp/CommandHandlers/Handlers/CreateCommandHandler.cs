using System;
using System.Globalization;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'create' command.
    /// </summary>
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service to create record in.</param>
        public CreateCommandHandler(IFileCabinetService fileCabinetService)
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

            if (request.Command.ToLower(CultureInfo.InvariantCulture) == "create")
            {
                this.Create();
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void Create()
        {
            do
            {
                try
                {
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

                    FileCabinetRecord parametrs = new FileCabinetRecord(firstName, lastName, dateofBirth, cabinetNumber, salary, category);

                    int record = this.Service.CreateRecord(parametrs);

                    Console.WriteLine("Record #" + record + " is created.");
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            while (true);
        }
    }
}
