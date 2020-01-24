﻿using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'find' command.
    /// </summary>
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private readonly IRecordPrinter printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service to find record.</param>
        /// /// <param name="printer">Printer for records.</param>
        public FindCommandHandler(IFileCabinetService fileCabinetService, IRecordPrinter printer)
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

            if (request.Command.ToUpperInvariant() == "FIND")
            {
                this.Find(request.Parameters);
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void Find(string parametrs)
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

                result = this.service.FindByFirstName(firstName);
            }

            if (criterion.Equals(CriterionLastName, StringComparison.InvariantCultureIgnoreCase))
            {
                string lastName = inputValue.Trim('"').ToUpperInvariant();

                result = this.service.FindByLastName(lastName);
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

                result = this.service.FindByDateOfBirth(dateOfBirthString);
            }

            this.printer.Print(result);
        }
    }
}