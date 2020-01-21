using System;

namespace FileCabinetGenerator
{
    class Program
    {
        private const string FirstMessage = "Enter command line options:";
        private const string OptionsDescription = "Output format type (csv, xml). Output file name. Amount of generated records. ID value to start.";
        private const string OutputTypeCsv = "csv";
        private const string OutputTypeXml = "xml";
        private const int parametr = 0;
        private const int value = 1;

        private static int amountRecords;
        private static int startId;
        private static string path = null;
        private static bool isCsv = false;
        private static bool isXml = false;

        static void Main(string[] args)
        {
            Console.WriteLine(FirstMessage);
            Console.WriteLine(OptionsDescription);

            const string outputTypeName = "--output-type";
            const string outputPathName = "--output";
            const string recordsAmountName = "--records-amount";
            const string startIdName = "--start-id";

            const string outputTypeNameShort = "t";
            const string outputPathNameShort = "o";
            const string recordsAmountNameShort = "a";
            const string startIdNameShort = "i";

            var inputMode = Console.ReadLine();

            if (inputMode.Contains('='))
            {
                var parameters = inputMode.Split(' ', 4);

                foreach (var item in parameters)
                {
                    var interiorParameters = item.Split('=', 2);

                    if (interiorParameters[parametr].Equals(outputTypeName))
                    {
                        isCsv = CheckInputFormat(interiorParameters, OutputTypeCsv);
                        isXml = CheckInputFormat(interiorParameters, OutputTypeXml);
                    }

                    if (interiorParameters[parametr].Equals(outputPathName))
                    {
                        var pathFormat = interiorParameters[value].Split('.', 2);

                        if (pathFormat[value].Contains(OutputTypeCsv) || pathFormat[value].Contains(OutputTypeXml))
                        {
                            path = interiorParameters[value];
                        }
                    }

                    if (interiorParameters[parametr].Equals(recordsAmountName))
                    {
                        amountRecords = Convert.ToInt32(interiorParameters[value]);
                    }

                    if (interiorParameters[parametr].Equals(startIdName))
                    {
                        startId = Convert.ToInt32(interiorParameters[value]);
                    }
                }
            }
            else
            {
                var badParameters = inputMode.Split('-', 5);
                var parameters = new string[4];
                for (int i = 1; i < badParameters.Length; i++)
                {
                    parameters[i - 1] = badParameters[i];
                }

                foreach (var item in parameters)
                {
                    var interiorParameters = item.Split(' ', 2);

                    if (interiorParameters[parametr].Equals(outputTypeNameShort))
                    {
                        isCsv = CheckInputFormat(interiorParameters, OutputTypeCsv);
                        isXml = CheckInputFormat(interiorParameters, OutputTypeXml);
                    }

                    if (interiorParameters[parametr].Equals(outputPathNameShort))
                    {
                        var pathFormat = interiorParameters[value].Split('.', 2);

                        if (pathFormat[value].Contains(OutputTypeCsv) || pathFormat[value].Contains(OutputTypeXml))
                        {
                            path = interiorParameters[value];
                        }
                    }

                    if (interiorParameters[parametr].Equals(recordsAmountNameShort))
                    {
                        amountRecords = Convert.ToInt32(interiorParameters[value]);
                    }

                    if (interiorParameters[parametr].Equals(startIdNameShort))
                    {
                        startId = Convert.ToInt32(interiorParameters[value]);
                    }
                }
            }

            Console.WriteLine(amountRecords + " records were written to " + path);

            Console.ReadLine();
        }

        private static bool CheckInputFormat(string[] str, string type)
        {
            if (str[value].Equals(type))
            {
                return true;
            }

            return false;
        }
    }
}
