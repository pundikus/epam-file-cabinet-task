using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This class inizialize request.
    /// </summary>
    public class AppCommandRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommandRequest"/> class.
        /// </summary>
        /// <param name="command">The name of the command.</param>
        /// <param name="parameters">The command's parameters.</param>
        public AppCommandRequest(string command, string parameters)
        {
            this.Command = command;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets command for call.
        /// </summary>
        /// <value>
        /// Commands.
        /// </value>
        public string Command { get; }

        /// <summary>
        /// Gets Parameters commands.
        /// </summary>
        /// <value>
        /// Parameters of commands.
        /// </value>
        public string Parameters { get; }
    }
}
