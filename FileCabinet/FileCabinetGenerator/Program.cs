using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace FileCabinetGenerator
{
    class Program
    {
        private const string OutputTypeCsv = "csv";
        private const string OutputTypeXml = "xml";
        private const int TypeParameterIndex = 0;
        private const int TypeParameterValue = 1;
        private const int OutputParameterIndex = 2;
        private const int OutputParameterValue = 3;
        private const int AmountParameterIndex = 4;
        private const int AmountParameterValue = 5;
        private const int StartIdParameterIndex = 6;
        private const int StartIdParameterValue = 7;

        private static int amountRecords;
        private static int startId;
        private static string path = null;
        private static bool isCsv = false;
        private static bool isXml = false;

        static void Main(string[] args)
        {
            const int parameterIndex = 0;
            const int parameterValue = 1;

            const string outputTypeName = "--output-type";
            const string outputPathName = "--output";
            const string recordsAmountName = "--records-amount";
            const string startIdName = "--start-id";

            const string outputTypeNameShort = "-t";
            const string outputPathNameShort = "-o";
            const string recordsAmountNameShort = "-a";
            const string startIdNameShort = "-i";

            if (args[0].Contains('='))
            {
                foreach (var item in args)
                {
                    string[] parameters = item.Split('=', 2);

                    if (parameters[parameterIndex].Equals(outputTypeName))
                    {
                        if (parameters[parameterValue].Equals(OutputTypeCsv))
                        {
                            isCsv = true;
                        }

                        if (parameters[parameterValue].Equals(OutputTypeXml))
                        {
                            isXml = true;
                        }
                    }

                    if (parameters[parameterIndex].Equals(outputPathName))
                    {
                        if (isCsv)
                        {
                            path = parameters[parameterValue];
                        }
                        else if (isXml)
                        {
                            path = parameters[parameterValue];
                        }
                        else
                        {
                            Console.WriteLine("Incorrect Format!");
                            return;
                        }
                    }

                    if (parameters[parameterIndex].Equals(recordsAmountName))
                    {
                        amountRecords = Convert.ToInt32(parameters[parameterValue]);
                    }

                    if (parameters[parameterIndex].Equals(startIdName))
                    {
                        startId = Convert.ToInt32(parameters[parameterValue]);
                    }
                }
            }
            else
            {
                foreach (var item in args)
                { 
                    if (args[TypeParameterIndex].Equals(outputTypeNameShort))
                    {
                        if (args[TypeParameterValue].Equals(OutputTypeCsv))
                        {
                            isCsv = true;
                        }

                        if (args[TypeParameterValue].Equals(OutputTypeXml))
                        {
                            isXml = true;
                        }
                    }

                    if (args[OutputParameterIndex].Equals(outputPathNameShort))
                    {
                        if (isCsv)
                        {
                            path = args[OutputParameterValue];
                        }
                        else if (isXml)
                        {
                            path = args[OutputParameterValue];
                        }
                        else
                        {
                            Console.WriteLine("Incorrect Format!");
                            return;
                        }
                    }

                    if (args[AmountParameterIndex].Equals(recordsAmountNameShort))
                    {
                        amountRecords = Convert.ToInt32(args[AmountParameterValue]);
                    }

                    if (args[StartIdParameterIndex].Equals(startIdNameShort))
                    {
                        startId = Convert.ToInt32(args[StartIdParameterValue]);
                    }
                }
            }

            var records = new FileCabinetApp.FileCabinetRecord[amountRecords];
            var list = new List<FileCabinetApp.FileCabinetRecord>(amountRecords);
            
            for (int i = 0; i < amountRecords; i++)
            {
                var record = new FileCabinetApp.FileCabinetRecord
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
                list.Add(record);
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
                SaveToXml(streamWriter, list);
            }

            streamWriter.Close();



            Console.WriteLine(amountRecords + " records were written to " + path);

            Console.ReadLine();
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

            short value = (short)rnd.Next(1, 1000);

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
            char category = 'A';

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

        private static void SaveToCsv(StreamWriter streamWriter, FileCabinetApp.FileCabinetRecord[] records)
        {
            if (streamWriter == null)
            {
                throw new ArgumentNullException(nameof(streamWriter));
            }

            WriteAllRecordsCsv(streamWriter, records);
        }

        private static void SaveToXml(StreamWriter streamWriter, List<FileCabinetApp.FileCabinetRecord> records)
        {
            if (streamWriter == null)
            {
                throw new ArgumentNullException(nameof(streamWriter));
            }

            WriteAllRecordsXml(streamWriter, records);
        }

        private static void WriteAllRecordsCsv(StreamWriter textWriter, FileCabinetApp.FileCabinetRecord[] records)
        {
            var csvWriter = new RecordCsvWriter(textWriter);

            string fieldsName = "Id,First Name,Last Name,Date of Birth,Cabinet Number,Salary,Category";
            textWriter.WriteLine(fieldsName);

            foreach (var item in records)
            {
                csvWriter.Write(item);
            }
        }

        private static void WriteAllRecordsXml(StreamWriter textWriter, List<FileCabinetApp.FileCabinetRecord> records)
        {
            var xmlWriter = new RecordXmlWriter(textWriter);

            xmlWriter.Write(records);
        }
    }
}
