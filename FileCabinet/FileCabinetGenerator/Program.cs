using System;
using System.Globalization;
using System.IO;

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

            RecordModel[] records = new RecordModel[amountRecords];

            for (int i = 0; i < amountRecords; i++)
            {
                var record = new RecordModel
                {
                    Id = startId,
                    FirstName = GeneratorFirstName(),
                    LastName = GeneratorLastName(),
                    DateOfBirth = GeneratorDateOfBirth(),
                    CabinetNumber = GeneratorCabinetNumber(),
                    Salary = GeneratorSalary(),
                    Category = GeneratorCategory(),
                };

                startId++;

                records[i] = record;
            }

            StreamWriter streamWriter;

            bool rewrite = false;

            if (File.Exists(path))
            {
                Console.Write("File is exist - rewrite " + path.Remove(0, 3) + "? [Y/n]");
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

            if (isCsv)
            {
                SaveToCsv(streamWriter, records);
            }

            if (isXml)
            {
                SaveToXml(streamWriter, records);
            }

            streamWriter.Close();



            Console.WriteLine(amountRecords + " records were written to " + path);

            Console.ReadLine();
        }

        private static bool CheckInputFormat(string[] str, string type)
        {
            str[value] = str[value].Trim();

            if (str[value].Equals(type))
            {
                return true;
            }

            return false;
        }

        private static string GeneratorFirstName()
        {
            var firstNameBase = new string[] {"Alexander", 
                                                "Alexei","Anatoly","Andrew","Anton","Arkady",
                                                "Artyom","Borislav","Vadim","Valentine","Valery",
                                                "Vasiliy","Victor","Vitaliy","Vladimir","Vyacheslav",
                                                "Gennady","George","Gregory","Daniel",
                                                "Denis","Dmitriy","Evgeny","Egor", "Ivan","Igor",
                                                "Ilya","Kirill","Maksim","Michael", "Nikita",
                                                "Nikolay",  "Oleg", "Semen", "Sergei", "Stanislav",
                                                "Stepan","Fedor","Yuri"
                                                };
            Random rnd = new Random();

            int value = rnd.Next(0, firstNameBase.Length);

            return firstNameBase[value];
        }

        private static string GeneratorLastName()
        {
            var lastNameBase = new string[] {"Ivanov",
                                            "Vasiliev",
                                            "Petrov",
                                            "Smirnov",
                                            "Mikhailov",
                                            "Fedorov",
                                            "Sokolov",
                                            "Yakovlev",
                                            "Popov",
                                            "Andreev",
                                            "Alekseev",
                                            "Alexandrov",
                                            "Lebedev",
                                            "Grigoryev",
                                            "Stepanov",
                                            "Semenov",
                                            "Pavlov",
                                            "Bogdanov",
                                            "Nikolaev",
                                            "Dmitriev",
                                            "Egorov",
                                            "Wolves",
                                            "Kuznetsov",
                                            "Nikitin",
                                            "Solovyov",
                                            "Timofeev",
                                            "Orlov",
                                            "Afanasyev",
                                            "Filippov",
                                            "Sergeev",
                                            "Zakharov",
                                            "Matveev",
                                            "Vinogradov",
                                            "Kuzmin",
                                            "Maximov",
                                            "Kozlov",
                                            "Ilyin",
                                            "Gerasimov",
                                            "Markov",
                                            "Novikov",
                                            "Morozov",
                                            "Romanov",
                                            "Osipov",
                                            "Makarov",
                                            "Zaitsev",
                                            "Belyaev",
                                            "Gavrilov",
                                            "Antonov",
                                            "Efimov",
                                            "Leontiev",
                                            "Davydov",
                                            "Gusev",
                                            "Danilov",
                                            "Kiselev",
                                            "Sorokin",
                                            "Tikhomirov",
                                            "Krylov",
                                            "Nikiforov",
                                            "Kondratyev",
                                            "Kudryavtsev",
                                            "Borisov",
                                            "Zhukov",
                                            "Vorobyov",
                                            "Shcherbakov",
                                            "Polyakov",
                                            "Schmidt",
                                            "Trofimov",
                                            "Chistyakov",
                                            "Rams",
                                            "Sidorov",
                                            "Sobolev",
                                            "Karpov",
                                            "Belov",
                                            "Miller"
            };

            Random rnd = new Random();

            int value = rnd.Next(0, lastNameBase.Length);

            return lastNameBase[value];
        }

        private static DateTime GeneratorDateOfBirth()
        {
            var dateOfBirthBase = new string[] {"12/12/1960", "10/25/1960", "11/16/1960" ,"10/27/1960" ,"11/10/1960", "12/12/1978", "10/25/1978", "12/16/1980", "10/17/1991", "10/10/1985", "10/27/1978", "12/16/1983", "12/26/1996",
                                            "12/12/1968", "10/24/1975", "11/16/1972" ,"10/27/1976" ,"11/10/1984", "12/12/1988", "10/25/1977", "12/16/1989", "10/17/1995", "10/10/1974", "10/27/1968", "12/16/1973", "12/26/1989"};
            Random rnd = new Random();

            int value = rnd.Next(0, dateOfBirthBase.Length);

            bool parsedDateTime = DateTime.TryParseExact(dateOfBirthBase[value], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime);
            if (!parsedDateTime)
            {
                throw new ArgumentException("Not parsed date.");
            }

            return dateTime;
        }

        private static short GeneratorCabinetNumber()
        {
            Random rnd = new Random();

            short value = (short)rnd.Next(0, 800);

            return value;
        }

        private static decimal GeneratorSalary()
        {
            Random rnd = new Random();

            int value = rnd.Next(1000, 10000);
            decimal salary = Convert.ToDecimal(value);

            return salary;
        }

        private static char GeneratorCategory()
        {
            Random rnd = new Random();

            int value = rnd.Next(1, 4);
            char category = '\0';

            if(value == 1)
            {
                return 'A';
            }

            if (value == 2)
            {
                return 'B';
            }

            if (value == 3)
            {
                return 'C';
            }

            return category;
        }

        private static void SaveToCsv(StreamWriter streamWriter, RecordModel[] records)
        {
            if (streamWriter == null)
            {
                throw new ArgumentNullException(nameof(streamWriter));
            }

            WriteAllRecordsCsv(streamWriter, records);
        }

        private static void SaveToXml(StreamWriter streamWriter, RecordModel[] records)
        {
            if (streamWriter == null)
            {
                throw new ArgumentNullException(nameof(streamWriter));
            }

            WriteAllRecordsXml(streamWriter, records);
        }

        private static void WriteAllRecordsCsv(StreamWriter textWriter, RecordModel[] records)
        {
            var csvWriter = new RecordCsvWriter(textWriter);

            string fieldsName = "Id,First Name,Last Name,Date of Birth,Cabinet Number,Salary,Category";
            textWriter.WriteLine(fieldsName);

            foreach (var item in records)
            {
                csvWriter.Write(item);
            }
        }

        private static void WriteAllRecordsXml(StreamWriter textWriter, RecordModel[] records)
        {
            var xmlWriter = new RecordXmlWriter(textWriter);

            xmlWriter.Write(records);
        }
    }
}
