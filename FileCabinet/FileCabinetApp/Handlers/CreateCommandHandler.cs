using System;
using static FileCabinetApp.Program;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'create' command.
    /// </summary>
    public class CreateCommandHandler : CommandHandlerBase
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

            if (request.Command.ToUpperInvariant() == "CREATE")
            {
                Create();
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private static void Create()
        {
            do
            {
                try
                {
                    Console.Write("First name: ");
                    string firstName = ReadInput(StringConverter, FirstNameValidator);

                    Console.Write("Last name: ");
                    string lastName = ReadInput(StringConverter, LastNameValidator);

                    Console.Write("Date of birth: ");
                    var dateofBirth = ReadInput(DateTimeConverter, DateOfBirthValidator);

                    Console.Write("Cabinet number: ");
                    var cabinetNumber = ReadInput(ShortConverter, CabinetNumberValidator);

                    Console.Write("Salary: ");
                    var salary = ReadInput(DecimalConverter, SalaryValidator);

                    Console.Write("Category(A, B, C): ");
                    var category = ReadInput(CharConverter, CategoryValidator);

                    FileCabinetRecord parametrs = new FileCabinetRecord(firstName, lastName, dateofBirth, cabinetNumber, salary, category);

                    int record = FileCabinetService.CreateRecord(parametrs);

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
