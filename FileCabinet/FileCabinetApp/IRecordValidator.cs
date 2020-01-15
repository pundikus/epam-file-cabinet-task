using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This Interface sets a method for checking parameters.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// This Method check parametrs.
        /// </summary>
        /// <param name="parametrs">It is user input parametrs.</param>
        public void ValidateParameters(ValueRange parametrs);
    }
}
