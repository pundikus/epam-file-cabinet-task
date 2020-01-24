using System;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'exit' command.
    /// </summary>
    public class ExitCommandHandler : CommandHandlerBase
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

            if (request.Command.ToUpperInvariant() == "EXIT")
            {
                Exit();
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private static void Exit()
        {
            Console.WriteLine("Exiting an application...");
            Program.IsRunning = false;
        }
    }
}
