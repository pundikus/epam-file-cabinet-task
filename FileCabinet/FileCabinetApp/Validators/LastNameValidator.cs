﻿using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces.Validators;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Last name validator.
    /// </summary>
    public class LastNameValidator : IRecordValidator
    {
        private int minLength;
        private int maxLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameValidator"/> class.
        /// </summary>
        /// <param name="min">Minimum possible length of first name.</param>
        /// <param name="max">Maximum possible length of last name.</param>
        public LastNameValidator(int min, int max)
        {
            this.minLength = min;
            this.maxLength = max;
        }

        /// <summary>
        /// Validates last name of user's input.
        /// </summary>
        /// <param name="record">Record to validate.</param>
        /// <returns>Whether last name is valid.</returns>
        public bool Validate(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.LastName.Length < this.minLength || record.LastName.Length > this.maxLength)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
