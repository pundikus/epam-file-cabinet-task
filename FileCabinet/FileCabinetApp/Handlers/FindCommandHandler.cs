using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'find' command.
    /// </summary>
    public class FindCommandHandler : CommandHandlerBase
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

            if (request.Command.ToUpperInvariant() == "FIND")
            {
                Find(request.Parameters);
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private static void Find(string parametrs)
        {
            const int CriterionIndex = 0;
            const int InputValueIndex = 1;

            const string CriterionFirstName = "firstName";
            const string CriterionLastName = "lastName";
            const string CriterionDateOfBirth = "dateofBirth";

            ReadOnlyCollection<FileCabinetRecord> result = null;

            var paramArray = parametrs.Split(' ', 2);
            if (paramArray.Length != 2)
            {
                Console.WriteLine("Input is not correct.");
                return;
            }

            string criterion = paramArray[CriterionIndex];
            string inputValue = paramArray[InputValueIndex];
            if (inputValue.First() != '"' && inputValue.Last() != '"')
            {
                Console.WriteLine("second params must be \"\"");
                return;
            }

            if (criterion.Equals(CriterionFirstName, StringComparison.InvariantCultureIgnoreCase))
            {
                string firstName = inputValue.Trim('"').ToUpperInvariant();

                result = Program.FileCabinetService.FindByFirstName(firstName);
            }

            if (criterion.Equals(CriterionLastName, StringComparison.InvariantCultureIgnoreCase))
            {
                string lastName = inputValue.Trim('"').ToUpperInvariant();

                result = Program.FileCabinetService.FindByLastName(lastName);
            }

            if (criterion.Equals(CriterionDateOfBirth, StringComparison.InvariantCultureIgnoreCase))
            {
                string dateOfBirth = inputValue.Trim('"').ToUpperInvariant();
                var parseddateofBirth = DateTime.TryParseExact(dateOfBirth, "yyyy-MMM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateofBirth);
                if (!parseddateofBirth)
                {
                    Console.WriteLine("Invalid Date of birth.");
                }

                string dateOfBirthString = dateofBirth.ToString(CultureInfo.InvariantCulture);

                result = Program.FileCabinetService.FindByDateOfBirth(dateOfBirthString);
            }

            foreach (var item in result)
            {
                var recordString = new StringBuilder();

                recordString.Append($"#{item.Id}, ");
                recordString.Append($"{item.FirstName}, ");
                recordString.Append($"{item.LastName}, ");
                recordString.Append($"{item.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture)}, ");

                Console.WriteLine(recordString);
            }
        }
    }
}
