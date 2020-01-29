using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Handler for the user's 'update' command.
    /// </summary>
    public class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service to create record in.</param>
        public UpdateCommandHandler(IFileCabinetService fileCabinetService)
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
                throw new ArgumentNullException($"Request is null.");
            }

            if (request.Command.ToLower(CultureInfo.InvariantCulture) == "update")
            {
                this.Update(request.Parameters);
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        private void Update(string parameters)
        {
            int indexOfSet = parameters.IndexOf("set", StringComparison.InvariantCulture);
            if (indexOfSet == -1)
            {
                return;
            }

            parameters = parameters.Substring(indexOfSet + 3);

            int numOfProps = typeof(FileCabinetRecord).GetProperties().Length;
            string[] sets = new string[numOfProps];

            Dictionary<string, int> propIndex = new Dictionary<string, int>();

            var properties = typeof(FileCabinetRecord).GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                propIndex.Add(properties[i].Name.ToLower(CultureInfo.InvariantCulture), i);
            }

            string[] set = this.MakeSet(parameters);
            if (set == null)
            {
                Console.WriteLine("Incorrect to-set conditions.");
                return;
            }

            ReadOnlyCollection<FileCabinetRecord> foundRecords = this.WhereParser(parameters.Substring(parameters.IndexOf("where", StringComparison.InvariantCulture) + 6).Split());
            List<FileCabinetRecord> copyOfRecords = new List<FileCabinetRecord>();

            if (foundRecords == null)
            {
                Console.WriteLine("No records found and changed.");
                return;
            }

            foreach (var record in foundRecords)
            {
                copyOfRecords.Add((FileCabinetRecord)record.Clone());
            }

            for (int i = 0; i < set.Length; i++)
            {
                if (set[i] != null)
                {
                    for (int j = 0; j < foundRecords.Count; j++)
                    {
                        PropertyInfo prop = typeof(FileCabinetRecord).GetProperty(propIndex.FirstOrDefault(x => x.Value == i).Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                        Type type = prop.PropertyType;

                        dynamic value = null;

                        if (type.Name == "DateTime")
                        {
                            try
                            {
                                value = DateTime.ParseExact(set[i], "yyyy-MMM-d", CultureInfo.InvariantCulture);
                            }
                            catch (FormatException)
                            {
                            }
                        }
                        else
                        {
                            if (type.Name == "String")
                            {
                                value = set[i];
                            }
                            else
                            {
                                value = Convert.ChangeType(set[i], type, CultureInfo.InvariantCulture);
                            }
                        }

                        if (ReferenceEquals(null, value))
                        {
                            Console.WriteLine("Convertion of " + prop.Name + " failed.");
                            return;
                        }

                        prop.SetValue(copyOfRecords[j], value);
                    }
                }
            }

            foreach (var record in copyOfRecords)
            {
                this.Service.AddRecord(record);
            }
        }
    }
}