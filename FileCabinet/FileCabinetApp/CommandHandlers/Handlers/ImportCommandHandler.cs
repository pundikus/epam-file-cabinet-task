using System;
using System.Collections.Generic;
using System.IO;

namespace FileCabinetApp.Handlers
{
    /// <summary>
    /// Handler for 'import' command.
    /// </summary>
    public class ImportCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service to import records.</param>
        public ImportCommandHandler(IFileCabinetService fileCabinetService)
        {
            this.Service = fileCabinetService;
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

            if (request.Command.ToUpperInvariant() == "IMPORT")
            {
                this.Import(request.Parameters);
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void Import(string parametrs)
        {
            const string formatCsv = "csv";
            const string formatXml = "xml";

            string[] parameters = parametrs.Split(' ', 2);
            string path = parameters[1];

            var snapshot = new FileCabinetServiceSnapshot(this.Service.GetRecords());

            if (File.Exists(path))
            {
                using (FileStream fileStream = File.OpenRead(path))
                {
                    StreamReader streamReader = new StreamReader(fileStream);

                    IList<FileCabinetRecord> records;

                    if (parameters[0].Equals(formatCsv, StringComparison.InvariantCultureIgnoreCase))
                    {
                        records = snapshot.LoadFromCsv(streamReader);

                        snapshot = new FileCabinetServiceSnapshot(records);

                        this.Service.Restore(snapshot);
                    }
                    else if (parameters[0].Equals(formatXml, StringComparison.InvariantCultureIgnoreCase))
                    {
                        records = snapshot.LoadFromXml(streamReader);

                        snapshot = new FileCabinetServiceSnapshot(records);

                        this.Service.Restore(snapshot);
                    }

                    Console.WriteLine(snapshot.Records.Count + " records were imported from " + path);

                    streamReader.Dispose();
                }
            }
            else
            {
                Console.WriteLine("Import error: file " + path + " is not exist.");
                return;
            }
        }
    }
}
