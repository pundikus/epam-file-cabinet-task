using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Favourite number validator.
    /// </summary>
    public interface ICabinetNumberValidator
    {
        /// <summary>
        /// Validates Cabinet Number input.
        /// </summary>
        /// <param name="cabinetNumber">Cabinet Number to validate.</param>
        /// <returns>Whether Cabine tNumber is valid.</returns>
        public bool Validate(short cabinetNumber);
    }
}
