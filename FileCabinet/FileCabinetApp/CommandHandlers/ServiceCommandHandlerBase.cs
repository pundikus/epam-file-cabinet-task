using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using FileCabinetApp.Helpers;
using static FileCabinetApp.Program;

namespace FileCabinetApp
{
    /// <summary>
    /// Handler for commands which relate to the service.
    /// </summary>
    public abstract class ServiceCommandHandlerBase : CommandHandlerBase
    {
        /// <summary>
        /// Gets or sets the service to get information and modify.
        /// </summary>
        /// <value>
        /// The service to get information and modify.
        /// </value>
        protected IFileCabinetService Service { get; set; }

        /// <summary>
        /// This Method validate First Name.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>check valid.</returns>
        protected Tuple<bool, string> FirstNameValidator(string arg)
        {
            bool isValid = true;
            if (IsModeCustom)
            {
                foreach (var item in new string("!@#$%^&*():;{}=+1234567890/|\"'<>.,").ToCharArray())
                {
                    if (arg.Contains(item, StringComparison.InvariantCulture))
                    {
                        isValid = false;
                    }

                    if (arg.Contains(item, StringComparison.InvariantCulture))
                    {
                        isValid = false;
                    }
                }
            }
            else
            {
                if (arg == null || string.IsNullOrEmpty(arg.Trim()))
                {
                    isValid = false;
                }

                if (arg.Length < 2 || arg.Length > 60)
                {
                    isValid = false;
                }
            }

            return new Tuple<bool, string>(isValid, arg);
        }

        /// <summary>
        /// This Method validate Last Name.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>check valid.</returns>
        protected Tuple<bool, string> LastNameValidator(string arg)
        {
            bool isVaild = true;

            if (IsModeCustom)
            {
                foreach (var item in new string("!@#$%^&*():;{}=+1234567890/|\"'<>.,").ToCharArray())
                {
                    if (arg.Contains(item, StringComparison.InvariantCulture))
                    {
                        isVaild = false;
                    }

                    if (arg.Contains(item, StringComparison.InvariantCulture))
                    {
                        isVaild = false;
                    }
                }
            }
            else
            {
                if (arg == null || string.IsNullOrEmpty(arg.Trim()))
                {
                    isVaild = false;
                }

                if (arg.Length < 2 || arg.Length > 60)
                {
                    isVaild = false;
                }
            }

            return new Tuple<bool, string>(isVaild, arg);
        }

        /// <summary>
        /// This Method validate Date Of Birth.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>check valid.</returns>
        protected Tuple<bool, string> DateOfBirthValidator(DateTime arg)
        {
            bool isValid = true;

            if (IsModeCustom)
            {
                DateTime validDate = new DateTime(2002, 1, 1);
                if (arg > validDate)
                {
                    isValid = false;
                }
            }
            else
            {
                DateTime minDate = new DateTime(1950, 1, 1);
                DateTime maxDate = DateTime.Now;

                if (arg < minDate || arg > maxDate)
                {
                    isValid = false;
                }
            }

            return new Tuple<bool, string>(isValid, arg.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// This Method validate Cabinet Number.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>check valid.</returns>
        protected Tuple<bool, string> CabinetNumberValidator(short arg)
        {
            bool flag = true;
            if (IsModeCustom)
            {
                short maxCabinetNumberCustom = 200;
                if (arg > maxCabinetNumberCustom)
                {
                    flag = false;
                }
            }
            else
            {
                if (arg < 1 || arg > 1500)
                {
                    flag = false;
                }
            }

            return new Tuple<bool, string>(flag, arg.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// This Method validate Salary.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>check valid.</returns>
        protected Tuple<bool, string> SalaryValidator(decimal arg)
        {
            bool flag = true;
            if (IsModeCustom)
            {
                decimal maxSalaryCategoryC = 10000;
                if (arg > maxSalaryCategoryC)
                {
                    flag = false;
                }
            }
            else
            {
                if (arg < 0 || arg > decimal.MaxValue)
                {
                    flag = false;
                }
            }

            return new Tuple<bool, string>(flag, arg.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// This Method validate Category.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>check valid.</returns>
        protected Tuple<bool, string> CategoryValidator(char arg)
        {
            bool flag = true;
            if (IsModeCustom)
            {
                if (arg < 65 || arg > 67)
                {
                    flag = false;
                }

                if (arg == 67)
                {
                    flag = false;
                }
            }
            else
            {
                if (arg < 65 || arg > 67)
                {
                    flag = false;
                }
            }

            return new Tuple<bool, string>(flag, arg.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Parses and validates the user's input.
        /// </summary>
        /// <typeparam name="T">The type of input.</typeparam>
        /// <param name="converter">Converter from string to specific type.</param>
        /// <param name="validator">Validator for the converted input.</param>
        /// <returns>Converted and validated value.</returns>
        protected T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        /// <summary>
        /// This Method convert value to String.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>convert value.</returns>
        protected Tuple<bool, string, string> StringConverter(string arg)
        {
            Tuple<bool, string, string> convertString = null;

            if (arg != null)
            {
                convertString = new Tuple<bool, string, string>(typeof(string).Equals(arg.GetType()), arg, arg.ToString(CultureInfo.InvariantCulture));
            }

            return convertString;
        }

        /// <summary>
        /// This Method convert value to DateTime.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>convert value.</returns>
        protected Tuple<bool, string, DateTime> DateTimeConverter(string arg)
        {
            bool parsedDateTime = DateTime.TryParseExact(arg, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime);

            return new Tuple<bool, string, DateTime>(parsedDateTime, arg, dateTime);
        }

        /// <summary>
        /// This Method convert value to Short.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>convert value.</returns>
        protected Tuple<bool, string, short> ShortConverter(string arg)
        {
            var parsedCabinetNumber = short.TryParse(arg, out short cabinetNumber);

            return new Tuple<bool, string, short>(parsedCabinetNumber, arg, cabinetNumber);
        }

        /// <summary>
        /// This Method convert value to Decimal.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>convert value.</returns>
        protected Tuple<bool, string, decimal> DecimalConverter(string arg)
        {
            var parsedSalary = decimal.TryParse(arg, out decimal salary);

            return new Tuple<bool, string, decimal>(parsedSalary, arg, salary);
        }

        /// <summary>
        /// This Method convert value to Char.
        /// </summary>
        /// <param name="arg">input value.</param>
        /// <returns>convert value.</returns>
        protected Tuple<bool, string, char> CharConverter(string arg)
        {
            var parsedCategory = char.TryParse(arg, out char category);

            return new Tuple<bool, string, char>(parsedCategory, arg, category);
        }

        /// <summary>
        /// Parser property.
        /// </summary>
        /// <param name="args">inputs.</param>
        /// <returns>new tuple.</returns>
        protected Tuple<string, string, int> ParsePropertyValuePair(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            string property;
            string value;
            int size;
            int equalsIndex = -1;

            int maxEqualIndex = args.Length < 3 ? args.Length : 3;

            for (int i = 0; i < maxEqualIndex; i++)
            {
                if (args[i].Contains('=', StringComparison.InvariantCulture))
                {
                    equalsIndex = i;
                    break;
                }
            }

            if (equalsIndex == -1 || equalsIndex >= 3)
            {
                return null;
            }

            if (equalsIndex == 1)
            {
                if (args[1] == "=")
                {
                    value = args[2];
                    size = 3;
                }
                else
                {
                    if (args[1].StartsWith('='))
                    {
                        value = args[1].Substring(0);
                        size = 2;
                    }
                    else
                    {
                        return null;
                    }
                }

                property = args[0];
            }
            else
            {
                if (args[0].EndsWith('='))
                {
                    property = args[1].Substring(0, args[1].Length - 1);
                    value = args[2];
                    size = 2;
                }
                else
                {
                    string[] subArgs = args[0].Split('=');
                    value = subArgs[1];
                    property = subArgs[0];
                    size = 1;
                }
            }

            if (!value.Contains('\'', StringComparison.InvariantCulture) || (value.IndexOf('\'', StringComparison.InvariantCulture) == value.LastIndexOf('\'')))
            {
                return null;
            }
            else
            {
                value = value[(value.IndexOf('\'', StringComparison.InvariantCulture) + 1) ..value.LastIndexOf('\'')];
            }

            return new Tuple<string, string, int>(property, value, size);
        }

        /// <summary>
        /// Analogy with SubString.
        /// </summary>
        /// <param name="array">array property.</param>
        /// <param name="from">position.</param>
        /// <returns>new array.</returns>
        protected string[] SubArrString(string[] array, int from)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (from == 0)
            {
                return array;
            }

            string[] newArr = new string[array.Length - from];
            for (int i = from; i < array.Length; i++)
            {
                newArr[i - from] = array[i];
            }

            return newArr;
        }

        /// <summary>
        /// Analogy with SubString.
        /// </summary>
        /// <param name="array">array property.</param>
        /// <param name="from">position.</param>
        /// <param name="to">position to.</param>
        /// <returns>new array.</returns>
        protected string[] SubArrString(string[] array, int from, int to)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            // if from == 0 return arr
            if (from == 0)
            {
                return array;
            }

            string[] newArr = new string[array.Length - from];
            for (int i = from; i < to; i++)
            {
                newArr[i - from] = array[i];
            }

            return newArr;
        }

        /// <summary>
        /// Parses the criterion after 'where'.
        /// </summary>
        /// <param name="args">Strings to parse.</param>
        /// <returns>Collection of records.</returns>
        protected ReadOnlyCollection<FileCabinetRecord> WhereParser(string[] args)
        {
            if (args == null)
            {
                return null;
            }

            ReadOnlyCollection<FileCabinetRecord> result;
            List<FileCabinetRecord> foundRecords = new List<FileCabinetRecord>();

            if (string.IsNullOrWhiteSpace(args[0]) || string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("No 'where' found. All the records:");
                result = new ReadOnlyCollection<FileCabinetRecord>(this.Service.GetRecords());
                return result;
            }

            int numOfOr = 0;

            foreach (string s in args)
            {
                if (s.ToLower(CultureInfo.InvariantCulture).Contains("or", StringComparison.InvariantCulture))
                {
                    numOfOr++;
                }
            }

            int numOfSets = numOfOr + 1;
            int numOfProps = typeof(FileCabinetRecord).GetProperties().Length;

            // contains values (ray, archie, gray)
            string[] sets = new string[numOfSets * numOfProps];

            // propertyName and its index in the dictionary
            Dictionary<string, int> propIndex = new Dictionary<string, int>();

            var properties = typeof(FileCabinetRecord).GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                // set the indexes
                propIndex.Add(properties[i].Name.ToLower(CultureInfo.InvariantCulture), i);
            }

            int currentSet = 0;
            int jump = 0;

            int argsLen = args.Length;

            for (int i = 0; i < argsLen;)
            {
                args = this.SubArrString(args, jump);
                Tuple<string, string, int> propValue = this.ParsePropertyValuePair(args);

                if (propValue == null)
                {
                    return null;
                }

                // we have string method string value
                if (!propIndex.ContainsKey(propValue.Item1.ToLower(CultureInfo.InvariantCulture)))
                {
                    Console.WriteLine("Incorrect name of property.");
                    return null;
                }
                else
                {
                    sets[propIndex[propValue.Item1.ToLower(CultureInfo.InvariantCulture)] + (numOfProps * currentSet)] = propValue.Item2;
                }

                // where all the conditions are read
                if (argsLen - i - propValue.Item3 == 0)
                {
                    // final logic and exit
                    for (int x = 0; x < numOfSets * numOfProps; x += numOfProps)
                    {
                        // contains a set of found records
                        List<FileCabinetRecord> tempRecords = new List<FileCabinetRecord>();
                        Dictionary<List<FileCabinetRecord>, int> conditions = new Dictionary<List<FileCabinetRecord>, int>();

                        for (int y = 0; y < numOfProps; y++)
                        {
                            if (sets[x + y] != null)
                            {
                                MethodInfo findMethod = this.Service.GetType().GetMethod("FindBy" + properties[y].Name);
                                if (findMethod == null)
                                {
                                    Console.WriteLine("Incorrect name of property.");
                                    return null;
                                }
                                else
                                {
                                    IEnumerable<FileCabinetRecord> records = (IEnumerable<FileCabinetRecord>)findMethod.Invoke(this.Service, new object[] { sets[x + y] });
                                    if (records is FileSystemRecordEnumeratorCollection)
                                    {
                                        tempRecords = FileSystemRecordEnumeratorCollection.ToList((FileSystemRecordEnumeratorCollection)records);
                                    }
                                    else
                                    {
                                        tempRecords = (List<FileCabinetRecord>)records;
                                    }

                                    if (tempRecords == null)
                                    {
                                        Console.WriteLine("No records found.");
                                        return null;
                                    }

                                    if (tempRecords.Count != 0)
                                    {
                                        conditions.Add(tempRecords, x + y);
                                    }
                                }
                            }
                        }

                        if (conditions.Count > 1)
                        {
                            // we have to sum the conditions and find records to delete
                            List<FileCabinetRecord> recordsToDelete = new List<FileCabinetRecord>();

                            // for each condition in dictionary
                            foreach (var pair in conditions)
                            {
                                // find property to check
                                PropertyInfo prop = properties[pair.Value - x];

                                foreach (var recordSet in conditions.Keys)
                                {
                                    foreach (var record in recordSet)
                                    {
                                        // find records which do not match
                                        if (prop.GetValue(record).ToString() != sets[pair.Value])
                                        {
                                            recordsToDelete.Add(record);
                                        }
                                    }
                                }
                            }

                            // delete dismatched records
                            for (int q = 0; q < recordsToDelete.Count; q++)
                            {
                                foreach (var pair in conditions)
                                {
                                    pair.Key.Remove(recordsToDelete[q]);
                                }
                            }

                            // add records to final list without duplicates
                            foreach (var recordSet in conditions.Keys)
                            {
                                foreach (var record in recordSet)
                                {
                                    if (!foundRecords.Contains(record))
                                    {
                                        foundRecords.Add(record);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (conditions.Count != 0)
                            {
                                foreach (var key in conditions.Keys)
                                {
                                    foreach (var record in key)
                                    {
                                        if (!foundRecords.Contains(record))
                                        {
                                            foundRecords.Add(record);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    return new ReadOnlyCollection<FileCabinetRecord>(foundRecords);
                }

                if (args[propValue.Item3].Contains("or", StringComparison.InvariantCulture))
                {
                    currentSet++;
                }
                else if (args[propValue.Item3].Contains("and", StringComparison.InvariantCulture))
                {
                }
                else
                {
                    return null;
                }

                jump = propValue.Item3 + 1;
                i += jump;
            }

            return new ReadOnlyCollection<FileCabinetRecord>(foundRecords);
        }

        /// <summary>
        /// Makes set property.
        /// </summary>
        /// <param name="parameters">inputs property.</param>
        /// <returns>new set prop.</returns>
        protected string[] MakeSet(string parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            int indexOfWhere = parameters.IndexOf("where", StringComparison.InvariantCulture);
            if (indexOfWhere == -1)
            {
                return null;
            }

            int numOfProps = typeof(FileCabinetRecord).GetProperties().Length;
            string[] sets = new string[numOfProps];

            string itemsToSet = parameters.Substring(0, indexOfWhere - 1);
            Tuple<string, string, int> propValue;

            // propertyName and its index in the dictionary
            Dictionary<string, int> propIndex = new Dictionary<string, int>();

            var properties = typeof(FileCabinetRecord).GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                // set the indexes
                propIndex.Add(properties[i].Name.ToLower(CultureInfo.InvariantCulture), i);
            }

            // if there is only one property
            if (!itemsToSet.Contains(',', StringComparison.InvariantCulture))
            {
                propValue = this.ParsePropertyValuePair(itemsToSet.Substring(1).Split());
                if (propValue == null)
                {
                    return null;
                }

                // check if such property exists
                if (!propIndex.ContainsKey(propValue.Item1.ToLower(CultureInfo.InvariantCulture)))
                {
                    // incorrect property
                    return null;
                }
                else
                {
                    sets[propIndex[propValue.Item1.ToLower(CultureInfo.InvariantCulture)]] = propValue.Item2;
                }

                return sets;
            }
            else
            {
                string[] args = itemsToSet.Split(',');
                int numOfConditions = args.Length;

                for (int i = 0; i < numOfConditions; i++)
                {
                    string[] tempArgs = args[i].Substring(1).Split();
                    propValue = this.ParsePropertyValuePair(tempArgs);

                    if (propValue == null)
                    {
                        return null;
                    }

                    // we have string method string value
                    if (!propIndex.ContainsKey(propValue.Item1.ToLower(CultureInfo.InvariantCulture)))
                    {
                        return null;
                    }
                    else
                    {
                        sets[propIndex[propValue.Item1.ToLower(CultureInfo.InvariantCulture)]] = propValue.Item2;
                    }
                }

                return sets;
            }
        }

        /// <summary>
        /// Parse property.
        /// </summary>
        /// <param name="parameters">inputs.</param>
        /// <returns>property.</returns>
        protected List<PropertyInfo> ParseProperties(string parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            List<PropertyInfo> propsToShow = new List<PropertyInfo>();

            // if there is only one property
            if (!parameters.Contains(',', StringComparison.InvariantCulture))
            {
                if (parameters.StartsWith(' '))
                {
                    parameters = parameters.Substring(1);
                }

                PropertyInfo property = typeof(FileCabinetRecord).GetProperty(parameters, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                // check if such property exists
                if (property == null)
                {
                    // incorrect property
                    return null;
                }
                else
                {
                    propsToShow.Add(property);
                }

                return propsToShow;
            }
            else
            {
                string[] args = parameters.Split(',');
                int numOfConditions = args.Length;

                for (int i = 0; i < numOfConditions; i++)
                {
                    if (args[i].StartsWith(' '))
                    {
                        args[i] = args[i].Substring(1);
                    }

                    // add case invariant
                    PropertyInfo property = typeof(FileCabinetRecord).GetProperty(args[i], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    // check if such property exists
                    if (property == null)
                    {
                        return null;
                    }
                    else
                    {
                        propsToShow.Add(property);
                    }
                }

                return propsToShow;
            }
        }
    }
}
