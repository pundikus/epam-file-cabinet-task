using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler for the user's 'select' command.
    /// </summary>
    public class SelectCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service to create record in.</param>
        public SelectCommandHandler(IFileCabinetService fileCabinetService)
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

            if (request.Command.ToLower(CultureInfo.InvariantCulture) == "select")
            {
                this.Select(request.Parameters);
                return true;
            }
            else
            {
                return base.Handle(request);
            }
        }

        /// <summary>
        /// This Method check type.
        /// </summary>
        /// <param name="o">is object.</param>
        /// <returns>bool value.</returns>
        public bool IsNumericType(object o)
        {
            if (o == null)
            {
                throw new ArgumentNullException(nameof(o));
            }

            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        private void Select(string parameters)
        {
            int indexOfWhere = parameters.IndexOf("where", StringComparison.InvariantCulture);
            if (indexOfWhere == -1)
            {
                return;
            }

            List<PropertyInfo> propsToShow;

            if (parameters.StartsWith("where", StringComparison.InvariantCulture))
            {
                propsToShow = typeof(FileCabinetRecord).GetProperties().ToList();
            }
            else
            {
                propsToShow = this.ParseProperties(parameters.Substring(0, indexOfWhere - 1));
                if (propsToShow == null)
                {
                    return;
                }
            }

            ReadOnlyCollection<FileCabinetRecord> foundListRecords = this.WhereParser(parameters.Substring(indexOfWhere + 6).Split());
            ReadOnlyCollection<FileCabinetRecord> foundRecords;
            if (foundListRecords == null || foundListRecords.Count == 0)
            {
                foundRecords = this.Service.GetRecords();
            }
            else
            {
                foundRecords = new ReadOnlyCollection<FileCabinetRecord>(foundListRecords);
            }

            int[] maxLength = new int[propsToShow.Count];
            dynamic value;
            int valueLength;
            int propertyLength;
            int tempMaxLength;

            for (int i = 0; i < foundRecords.Count; i++)
            {
                for (int j = 0; j < propsToShow.Count; j++)
                {
                    if (foundRecords[i] == null)
                    {
                        break;
                    }

                    tempMaxLength = 0;
                    value = propsToShow[j].GetValue(foundRecords[i]);
                    if (propsToShow[j].PropertyType.Name == "DateTime")
                    {
                        valueLength = value.ToString("yyyy-MMM-d", CultureInfo.InvariantCulture).Length;
                    }
                    else
                    {
                        valueLength = value.ToString().Length;
                    }

                    propertyLength = propsToShow[j].Name.Length;
                    tempMaxLength = propertyLength > valueLength ? propertyLength : valueLength;
                    maxLength[j] = maxLength[j] > tempMaxLength ? maxLength[j] : tempMaxLength;
                    maxLength[j] = tempMaxLength;
                }
            }

            int padding = 1;

            Console.WriteLine(this.WriteWholeLine(maxLength.ToList(), padding));
            Console.WriteLine(this.WriteProperty(propsToShow, maxLength.ToList(), padding));
            Console.WriteLine(this.WriteWholeLine(maxLength.ToList(), padding));

            foreach (var record in foundRecords)
            {
                Console.WriteLine(this.WriteValue(record, propsToShow, maxLength.ToList(), padding));
                Console.WriteLine(this.WriteWholeLine(maxLength.ToList(), padding));
            }
        }

        private string WriteValue(FileCabinetRecord record, List<PropertyInfo> properties, List<int> maxLength, int padding)
        {
            string output = string.Empty;
            dynamic value;
            output += '|';
            for (int i = 0; i < properties.Count; i++)
            {
                if (record == null)
                {
                    break;
                }

                value = properties[i].GetValue(record);
                if (this.IsNumericType(value))
                {
                    output += this.WriteSpaces(maxLength[i] - (value.ToString().Length - 1));
                    output += value.ToString();
                    output += this.WriteSpaces(padding);
                    output += '|';
                }
                else
                {
                    output += this.WriteSpaces(padding);
                    int valueLength;
                    if (properties[i].PropertyType.Name == "DateTime")
                    {
                        output += value.ToString("yyyy-MMM-d", CultureInfo.InvariantCulture);
                        valueLength = value.ToString("yyyy-MMM-d", CultureInfo.InvariantCulture).Length;
                    }
                    else
                    {
                        output += value.ToString();
                        valueLength = value.ToString().Length;
                    }

                    output += this.WriteSpaces(maxLength[i] + padding - (padding + valueLength - 1));
                    output += '|';
                }
            }

            return output;
        }

        private string WriteProperty(List<PropertyInfo> properties, List<int> maxLength, int padding)
        {
            string output = string.Empty;
            output += '|';
            string tempOutput = string.Empty;
            for (int i = 0; i < properties.Count; i++)
            {
                tempOutput = this.WriteSpaces(padding);
                tempOutput += properties[i].Name.ToString(CultureInfo.InvariantCulture);
                output += tempOutput;
                output += this.WriteSpaces(maxLength[i] + padding - (tempOutput.Length - 1));
                output += '|';
            }

            return output;
        }

        private string WriteSpaces(int count)
        {
            string spaces = string.Empty;
            for (int i = 0; i < count; i++)
            {
                spaces += ' ';
            }

            return spaces;
        }

        private string WriteWholeLine(List<int> maxLength, int padding)
        {
            string output = string.Empty;
            int space;
            output += "+";

            for (int i = 0; i < maxLength.Count; i++)
            {
                space = maxLength[i] + (padding * 2);
                output += this.WriteSpecialLine(space + 1) + "+";
            }

            return output;
        }

        private string WriteSpecialLine(int length)
        {
            string line = string.Empty;
            for (int i = 1; i < length; i++)
            {
                if (i % 2 == 0)
                {
                    line += '-';
                }
                else
                {
                    line += ' ';
                }
            }

            return line;
        }
    }
}
