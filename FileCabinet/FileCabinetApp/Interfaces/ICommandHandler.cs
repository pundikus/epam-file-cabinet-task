namespace FileCabinetApp
{
    /// <summary>
    /// This interface sets behavor.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Set the next handler.
        /// </summary>
        /// <param name="commandHandler">Handler to set.</param>
        /// <returns>Handler back.</returns>
        public ICommandHandler SetNext(ICommandHandler commandHandler);

        /// <summary>
        /// Handler the request.
        /// </summary>
        /// <param name="request">Request to handle.</param>
        /// <returns>The object back or null.</returns>
        public object Handle(AppCommandRequest request);
    }
}
