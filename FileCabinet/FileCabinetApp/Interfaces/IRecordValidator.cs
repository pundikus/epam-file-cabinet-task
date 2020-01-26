namespace FileCabinetApp.Interfaces.Validators
{
    /// <summary>
    /// Validator for input.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <param name="record">Record to validate.</param>
        /// <returns>Result of validation.</returns>
        bool Validate(FileCabinetRecord record);
    }
}
