using System;
using System.IO;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'export' command.
    /// </summary>
    public class ExportCommandHandler : CommandHandlerBase
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

            if (request.Command.ToUpperInvariant() == "EXPORT")
            {
                Export(request.Parameters);
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private static void Export(string parametrs)
        {
            const int formatIndex = 0;
            const int pathIndex = 1;

            const string formatCsv = "csv";
            const string formatXml = "xml";

            var parametersArrayForFromat = parametrs.Split(" ", 2);
            string format = parametersArrayForFromat[formatIndex];

            if (parametersArrayForFromat.Length == 1)
            {
                Console.WriteLine("Incorrect input");
                return;
            }

            string path = parametersArrayForFromat[pathIndex];

            if (!path.Contains(format, StringComparison.InvariantCulture))
            {
                Console.WriteLine("Incorrect format");
                return;
            }

            StreamWriter streamWriter;

            bool rewrite = false;

            try
            {
                if (File.Exists(path))
                {
                    Console.Write("File is exist - rewrite " + parametrs + "? [Y/n]");
                    string result = Console.ReadLine();

                    if (result == "Y")
                    {
                        streamWriter = new StreamWriter(path, rewrite);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    streamWriter = new StreamWriter(path);
                }

                var snapshot = Program.FileCabinetService.MakeSnapshot();

                if (format == formatCsv)
                {
                    snapshot.SaveToCsv(streamWriter);
                }

                if (format == formatXml)
                {
                    snapshot.SaveToXml(streamWriter);
                }

                streamWriter.Close();

                Console.WriteLine("All records are exported to file " + path);
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("Export failed: " + ex.Message);
                return;
            }
        }
    }
}
