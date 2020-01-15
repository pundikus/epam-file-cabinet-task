using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This class storaged default validate parametrs.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// This Method create default Validator.
        /// </summary>
        /// <returns>new default validator.</returns>
        protected override IRecordValidator CreateValidator()
        {
            return new DefaultValidator();
        }
    }
}
