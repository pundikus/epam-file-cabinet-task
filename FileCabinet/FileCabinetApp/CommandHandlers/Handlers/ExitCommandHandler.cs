using System;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'exit' command.
    /// </summary>
    public class ExitCommandHandler : CommandHandlerBase
    {
        private Action<bool> state;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommandHandler"/> class.
        /// </summary>
        /// <param name="state">Action to change the running state.</param>
        public ExitCommandHandler(Action<bool> state)
        {
            this.state = state;
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

            if (request.Command.ToUpperInvariant() == "EXIT")
            {
                this.Exit();
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void Exit()
        {
            Console.WriteLine("Exiting an application...");
            this.state.Invoke(false);
        }
    }
}
