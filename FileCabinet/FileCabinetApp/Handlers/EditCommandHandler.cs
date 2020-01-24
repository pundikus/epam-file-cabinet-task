using System;
using System.Linq;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'edit' command.
    /// </summary>
    public class EditCommandHandler : CommandHandlerBase
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

            if (request.Command.ToUpperInvariant() == "EDIT")
            {
                Edit(request.Parameters);
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private static void Edit(string parameters)
        {
            var parsedId = int.TryParse(parameters, out int id);
            if (!parsedId)
            {
                Console.WriteLine("Invalid Id");
            }

            var listRecords = Program.FileCabinetService.GetRecords();

            if (!listRecords.Any(x => x.Id == id))
            {
                Console.WriteLine($"#{id} record is not found.");
                return;
            }

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

            try
            {
                FileCabinetRecord parametrs = new FileCabinetRecord(id, firstName, lastName, dateofBirth, cabinetNumber, salary, category);

                Program.FileCabinetService.EditRecord(parametrs);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine($"Record #{id} is updated.");
        }
    }
}
