using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This class initialize Handler.
    /// </summary>
    public class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler nextHandler;

        /// <summary>
        /// Handler the request.
        /// </summary>
        /// <param name="request">Request to handle.</param>
        /// <returns>The object back or null.</returns>
        public virtual object Handle(AppCommandRequest request)
        {
            if (this.nextHandler != null)
            {
                return this.nextHandler.Handle(request);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Set the next handler.
        /// </summary>
        /// <param name="commandHandler">Handler to set.</param>
        /// <returns>Handler back.</returns>
        public ICommandHandler SetNext(ICommandHandler commandHandler)
        {
            this.nextHandler = commandHandler;

            return commandHandler;
        }
    }
}
