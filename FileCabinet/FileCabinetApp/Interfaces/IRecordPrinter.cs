using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This interface sets the behavior to the method 'Print'.
    /// </summary>
    public interface IRecordPrinter
    {
        /// <summary>
        /// Prints the list of records to the user.
        /// </summary>
        /// <param name="records">Records to print.</param>
        void Print(IEnumerable<FileCabinetRecord> records);
    }
}
