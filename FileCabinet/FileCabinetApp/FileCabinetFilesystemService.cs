using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This class intended for file system service.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">Providers for file operation.</param>
        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <inheritdoc/>
        public int CreateRecord(ValueRange parametrs)
        {
            if (parametrs == null)
            {
                throw new ArgumentNullException(nameof(parametrs));
            }

            BinaryWriter binaryWriter;

            if (!this.fileStream.CanWrite)
            {
                this.fileStream = File.OpenWrite(this.fileStream.Name);
                binaryWriter = new BinaryWriter(this.fileStream);
            }
            else
            {
                binaryWriter = new BinaryWriter(this.fileStream);
            }

            int id = new Random().Next(0, 1000);
            short status = 0;

            this.fileStream.Seek(0, SeekOrigin.End);

            var byteStatus = BitConverter.GetBytes(status);

            var byteId = BitConverter.GetBytes(id);

            var massChar = new char[60];
            for (int i = 0; i < massChar.Length; i++)
            {
                massChar[i] = '\0';
            }

            byte[] massFNamebyte = Encoding.Unicode.GetBytes(massChar);
            var firstNameByte = Encoding.Unicode.GetBytes(parametrs.FirstName);
            if (firstNameByte.Length < massFNamebyte.Length)
            {
                for (int i = 0; i < firstNameByte.Length; i++)
                {
                    massFNamebyte[i] = firstNameByte[i];
                }
            }

            byte[] massLNamebyte = Encoding.Unicode.GetBytes(massChar);
            var lastNameByte = Encoding.Unicode.GetBytes(parametrs.LastName);
            if (lastNameByte.Length < massLNamebyte.Length)
            {
                for (int i = 0; i < lastNameByte.Length; i++)
                {
                    massLNamebyte[i] = lastNameByte[i];
                }
            }

            var yearByte = BitConverter.GetBytes(parametrs.DateOfBirth.Year);

            var monthByte = BitConverter.GetBytes(parametrs.DateOfBirth.Month);

            var dayByte = BitConverter.GetBytes(parametrs.DateOfBirth.Day);

            var cabinetNumberByte = BitConverter.GetBytes(parametrs.CabinetNumber);

            int[] bits = decimal.GetBits(parametrs.Salary);
            List<byte> bytes = new List<byte>();
            foreach (int i in bits)
            {
                bytes.AddRange(BitConverter.GetBytes(i));
            }

            var salaryByte = bytes.ToArray();

            var categoryByte = BitConverter.GetBytes(parametrs.Category);

            var sizerecord = this.GetSizeRecords();

            byte[] record = new byte[sizerecord];
            record = this.GetRecordArray(record, byteStatus, byteId, massFNamebyte, massLNamebyte, yearByte, monthByte, dayByte, cabinetNumberByte, salaryByte, categoryByte);

            int offset = 0;

            binaryWriter.Write(record, offset, byteStatus.Length);
            offset += byteStatus.Length;

            binaryWriter.Write(record, offset, byteId.Length);
            offset += byteId.Length;

            binaryWriter.Write(record, offset, massFNamebyte.Length);
            offset += massFNamebyte.Length;

            binaryWriter.Write(record, offset, massLNamebyte.Length);
            offset += massLNamebyte.Length;

            binaryWriter.Write(record, offset, yearByte.Length);
            offset += yearByte.Length;

            binaryWriter.Write(record, offset, monthByte.Length);
            offset += monthByte.Length;

            binaryWriter.Write(record, offset, dayByte.Length);
            offset += dayByte.Length;

            binaryWriter.Write(record, offset, cabinetNumberByte.Length);
            offset += cabinetNumberByte.Length;

            binaryWriter.Write(record, offset, salaryByte.Length);
            offset += salaryByte.Length;

            binaryWriter.Write(record, offset, categoryByte.Length);

            binaryWriter.Dispose();

            return id;
        }

        /// <inheritdoc/>
        public void EditRecord(ValueRange parametrs)
        {
            if (parametrs == null)
            {
                throw new ArgumentNullException(nameof(parametrs));
            }

            BinaryReader binaryReader;
            BinaryWriter binaryWriter;

            if (!this.fileStream.CanRead || this.fileStream.CanWrite)
            {
                this.fileStream = File.Open(this.fileStream.Name, FileMode.Open);
                binaryReader = new BinaryReader(this.fileStream);
                binaryWriter = new BinaryWriter(this.fileStream);
            }
            else
            {
                binaryReader = new BinaryReader(this.fileStream);
                binaryWriter = new BinaryWriter(this.fileStream);
            }

            int position = 2;

            while (this.fileStream.Position < this.fileStream.Length)
            {
                this.fileStream.Seek(position, SeekOrigin.Begin);
                var idinByte = binaryReader.ReadBytes(sizeof(int));
                int id = BitConverter.ToInt32(idinByte);

                if (id.Equals(parametrs.Id))
                {
                    this.fileStream.Seek(position - 2, SeekOrigin.Begin);
                    short status = 0;
                    byte[] statusByte = BitConverter.GetBytes(status);

                    var massChar = new char[60];
                    for (int i = 0; i < massChar.Length; i++)
                    {
                        massChar[i] = '\0';
                    }

                    byte[] massFNamebyte = Encoding.Unicode.GetBytes(massChar);
                    var firstNameByte = Encoding.Unicode.GetBytes(parametrs.FirstName);
                    if (firstNameByte.Length < massFNamebyte.Length)
                    {
                        for (int i = 0; i < firstNameByte.Length; i++)
                        {
                            massFNamebyte[i] = firstNameByte[i];
                        }
                    }

                    byte[] massLNamebyte = Encoding.Unicode.GetBytes(massChar);
                    var lastNameByte = Encoding.Unicode.GetBytes(parametrs.LastName);
                    if (lastNameByte.Length < massLNamebyte.Length)
                    {
                        for (int i = 0; i < lastNameByte.Length; i++)
                        {
                            massLNamebyte[i] = lastNameByte[i];
                        }
                    }

                    var yearByte = BitConverter.GetBytes(parametrs.DateOfBirth.Year);

                    var monthByte = BitConverter.GetBytes(parametrs.DateOfBirth.Month);

                    var dayByte = BitConverter.GetBytes(parametrs.DateOfBirth.Day);

                    var cabinetNumberByte = BitConverter.GetBytes(parametrs.CabinetNumber);

                    int[] bits = decimal.GetBits(parametrs.Salary);
                    List<byte> bytes = new List<byte>();
                    foreach (int i in bits)
                    {
                        bytes.AddRange(BitConverter.GetBytes(i));
                    }

                    var salaryByte = bytes.ToArray();

                    var sizerecord = this.GetSizeRecords();
                    var categoryByte = BitConverter.GetBytes(parametrs.Category);

                    byte[] record = new byte[sizerecord];
                    record = this.GetRecordArray(record, statusByte, idinByte, massFNamebyte, massLNamebyte, yearByte, monthByte, dayByte, cabinetNumberByte, salaryByte, categoryByte);

                    int offset = 0;

                    binaryWriter.Write(record, offset, statusByte.Length);
                    offset += statusByte.Length;

                    binaryWriter.Write(record, offset, idinByte.Length);
                    offset += idinByte.Length;

                    binaryWriter.Write(record, offset, massFNamebyte.Length);
                    offset += massFNamebyte.Length;

                    binaryWriter.Write(record, offset, massLNamebyte.Length);
                    offset += massLNamebyte.Length;

                    binaryWriter.Write(record, offset, yearByte.Length);
                    offset += yearByte.Length;

                    binaryWriter.Write(record, offset, monthByte.Length);
                    offset += monthByte.Length;

                    binaryWriter.Write(record, offset, dayByte.Length);
                    offset += dayByte.Length;

                    binaryWriter.Write(record, offset, cabinetNumberByte.Length);
                    offset += cabinetNumberByte.Length;

                    binaryWriter.Write(record, offset, salaryByte.Length);
                    offset += salaryByte.Length;

                    binaryWriter.Write(record, offset, categoryByte.Length);

                    break;
                }
                else
                {
                    position += this.GetSizeRecords();
                }
            }

            binaryReader.Dispose();
            binaryWriter.Dispose();
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            var resultList = new List<FileCabinetRecord>();

            BinaryReader binaryReader;

            if (!this.fileStream.CanRead)
            {
                this.fileStream = File.OpenRead(this.fileStream.Name);
                binaryReader = new BinaryReader(this.fileStream);
            }
            else
            {
                binaryReader = new BinaryReader(this.fileStream);
            }

            int position = sizeof(short) + sizeof(int) + 240;

            while (this.fileStream.Position < this.fileStream.Length)
            {
                this.fileStream.Seek(position, SeekOrigin.Begin);

                if (this.fileStream.Position > this.fileStream.Length)
                {
                    break;
                }

                var yearInByte = binaryReader.ReadBytes(sizeof(int));
                int yearIn = BitConverter.ToInt32(yearInByte);

                var monthInByte = binaryReader.ReadBytes(sizeof(int));
                int monthIn = BitConverter.ToInt32(monthInByte);

                var dayInByte = binaryReader.ReadBytes(sizeof(int));
                int dayIn = BitConverter.ToInt32(dayInByte);

                DateTime dateofBirth = new DateTime(yearIn, monthIn, dayIn);

                var a = DateTime.Parse(dateOfBirth, CultureInfo.InvariantCulture);

              //  string dateToString = dateofBirth.ToString(CultureInfo.InvariantCulture);

                if (a.Equals(dateofBirth))
                {
                    this.fileStream.Seek(position - 244, SeekOrigin.Begin);
                    //var statusByte = binaryReader.ReadBytes(sizeof(short));
                    //short status = BitConverter.ToInt16(statusByte);
                    //size += sizeof(short);

                    var idinByte = binaryReader.ReadBytes(sizeof(int));
                    int id = BitConverter.ToInt32(idinByte);

                    var massChar = new char[60];
                    byte[] massFNamebyte = Encoding.Unicode.GetBytes(massChar);

                    var firstNameChar = binaryReader.ReadBytes(massFNamebyte.Length);
                    var firstNameFull = Encoding.Unicode.GetString(firstNameChar);
                    var firstNameInFile = this.RemoveSymbols(firstNameFull);

                    byte[] masLNamebyte = Encoding.Unicode.GetBytes(massChar);
                    var lastNameChar = binaryReader.ReadBytes(masLNamebyte.Length);
                    var lastNameFull = Encoding.Unicode.GetString(lastNameChar);
                    var lastNameInFile = this.RemoveSymbols(lastNameFull);

                    var yearByte = binaryReader.ReadBytes(sizeof(int));
                    int year = BitConverter.ToInt32(yearByte);

                    var monthByte = binaryReader.ReadBytes(sizeof(int));
                    int month = BitConverter.ToInt32(monthByte);

                    var dayByte = binaryReader.ReadBytes(sizeof(int));
                    int day = BitConverter.ToInt32(dayByte);

                    DateTime dateofBirthInFile = new DateTime(year, month, day);

                    var cabinetNumberByte = binaryReader.ReadBytes(sizeof(short));
                    short cabinetNumber = BitConverter.ToInt16(cabinetNumberByte);

                    decimal salary = binaryReader.ReadDecimal();

                    var categoryByte = binaryReader.ReadBytes(sizeof(char));
                    char category = BitConverter.ToChar(categoryByte);

                    var record = new FileCabinetRecord
                    {
                        Id = id,
                        FirstName = firstNameInFile,
                        LastName = lastNameInFile,
                        DateOfBirth = dateofBirthInFile,
                        CabinetNumber = cabinetNumber,
                        Salary = salary,
                        Category = category,
                    };

                    resultList.Add(record);
                }

                position += 278;
            }

            binaryReader.Dispose();

            var result = new ReadOnlyCollection<FileCabinetRecord>(resultList);

            return result;
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var resultList = new List<FileCabinetRecord>();

            BinaryReader binaryReader;

            if (!this.fileStream.CanRead)
            {
                this.fileStream = File.OpenRead(this.fileStream.Name);
                binaryReader = new BinaryReader(this.fileStream);
            }
            else
            {
                binaryReader = new BinaryReader(this.fileStream);
            }

            int position = sizeof(short) + sizeof(int);

            while (this.fileStream.Position < this.fileStream.Length)
            {
                this.fileStream.Seek(position, SeekOrigin.Begin);

                var massChar = new char[60];
                byte[] masFNamebyte = Encoding.Unicode.GetBytes(massChar);

                var firstnameChar = binaryReader.ReadBytes(masFNamebyte.Length);
                var firstnameFull = Encoding.Unicode.GetString(firstnameChar);
                var firstnameInFile = this.RemoveSymbols(firstnameFull);

                if (firstName.Equals(firstnameInFile, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.fileStream.Seek(position - 4, SeekOrigin.Begin);
                    //var statusByte = binaryReader.ReadBytes(sizeof(short));
                    //short status = BitConverter.ToInt16(statusByte);
                    //size += sizeof(short);

                    var idinByte = binaryReader.ReadBytes(sizeof(int));
                    int id = BitConverter.ToInt32(idinByte);

                    byte[] massFNamebyte = Encoding.Unicode.GetBytes(massChar);

                    var firstNameChar = binaryReader.ReadBytes(massFNamebyte.Length);
                    var firstNameFull = Encoding.Unicode.GetString(firstNameChar);
                    var firstNameInFile = this.RemoveSymbols(firstNameFull);

                    var lastNameChar = binaryReader.ReadBytes(massFNamebyte.Length);
                    var lastNameFull = Encoding.Unicode.GetString(lastNameChar);
                    var lastName = this.RemoveSymbols(lastNameFull);

                    var yearByte = binaryReader.ReadBytes(sizeof(int));
                    int year = BitConverter.ToInt32(yearByte);

                    var monthByte = binaryReader.ReadBytes(sizeof(int));
                    int month = BitConverter.ToInt32(monthByte);

                    var dayByte = binaryReader.ReadBytes(sizeof(int));
                    int day = BitConverter.ToInt32(dayByte);

                    DateTime dateofBirth = new DateTime(year, month, day);

                    var cabinetNumberByte = binaryReader.ReadBytes(sizeof(short));
                    short cabinetNumber = BitConverter.ToInt16(cabinetNumberByte);

                    decimal salary = binaryReader.ReadDecimal();

                    var categoryByte = binaryReader.ReadBytes(sizeof(char));
                    char category = BitConverter.ToChar(categoryByte);

                    var record = new FileCabinetRecord
                    {
                        Id = id,
                        FirstName = firstNameInFile,
                        LastName = lastName,
                        DateOfBirth = dateofBirth,
                        CabinetNumber = cabinetNumber,
                        Salary = salary,
                        Category = category,
                    };

                    resultList.Add(record);
                }

                position += 278;
            }

            binaryReader.Dispose();

            var result = new ReadOnlyCollection<FileCabinetRecord>(resultList);

            return result;
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            var resultList = new List<FileCabinetRecord>();

            BinaryReader binaryReader;

            if (!this.fileStream.CanRead)
            {
                this.fileStream = File.OpenRead(this.fileStream.Name);
                binaryReader = new BinaryReader(this.fileStream);
            }
            else
            {
                binaryReader = new BinaryReader(this.fileStream);
            }

            int position = sizeof(short) + sizeof(int) + 120;

            while (this.fileStream.Position < this.fileStream.Length)
            {
                this.fileStream.Seek(position, SeekOrigin.Begin);

                var massChar = new char[60];
                byte[] masLNamebyte = Encoding.Unicode.GetBytes(massChar);

                var lastnameChar = binaryReader.ReadBytes(masLNamebyte.Length);
                var lastnameFull = Encoding.Unicode.GetString(lastnameChar);
                var lastnameInFile = this.RemoveSymbols(lastnameFull);

                if (lastName.Equals(lastnameInFile, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.fileStream.Seek(position - 124, SeekOrigin.Begin);
                    //var statusByte = binaryReader.ReadBytes(sizeof(short));
                    //short status = BitConverter.ToInt16(statusByte);
                    //size += sizeof(short);

                    var idinByte = binaryReader.ReadBytes(sizeof(int));
                    int id = BitConverter.ToInt32(idinByte);

                    byte[] massFNamebyte = Encoding.Unicode.GetBytes(massChar);

                    var firstNameChar = binaryReader.ReadBytes(massFNamebyte.Length);
                    var firstNameFull = Encoding.Unicode.GetString(firstNameChar);
                    var firstNameInFile = this.RemoveSymbols(firstNameFull);

                    var lastNameChar = binaryReader.ReadBytes(masLNamebyte.Length);
                    var lastNameFull = Encoding.Unicode.GetString(lastNameChar);
                    var lastNameInFile = this.RemoveSymbols(lastNameFull);

                    var yearByte = binaryReader.ReadBytes(sizeof(int));
                    int year = BitConverter.ToInt32(yearByte);

                    var monthByte = binaryReader.ReadBytes(sizeof(int));
                    int month = BitConverter.ToInt32(monthByte);

                    var dayByte = binaryReader.ReadBytes(sizeof(int));
                    int day = BitConverter.ToInt32(dayByte);

                    DateTime dateofBirth = new DateTime(year, month, day);

                    var cabinetNumberByte = binaryReader.ReadBytes(sizeof(short));
                    short cabinetNumber = BitConverter.ToInt16(cabinetNumberByte);

                    decimal salary = binaryReader.ReadDecimal();

                    var categoryByte = binaryReader.ReadBytes(sizeof(char));
                    char category = BitConverter.ToChar(categoryByte);

                    var record = new FileCabinetRecord
                    {
                        Id = id,
                        FirstName = firstNameInFile,
                        LastName = lastNameInFile,
                        DateOfBirth = dateofBirth,
                        CabinetNumber = cabinetNumber,
                        Salary = salary,
                        Category = category,
                    };

                    resultList.Add(record);
                }

                position += 278;
            }

            binaryReader.Dispose();

            var result = new ReadOnlyCollection<FileCabinetRecord>(resultList);

            return result;
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var fi = new FileInfo(this.fileStream.Name);
            if (fi.Length == 0)
            {
                Console.WriteLine("File is empty!");
            }

            BinaryReader binaryReader;

            if (!this.fileStream.CanRead)
            {
                this.fileStream = File.OpenRead(this.fileStream.Name);
                binaryReader = new BinaryReader(this.fileStream);
            }
            else
            {
                binaryReader = new BinaryReader(this.fileStream);
            }

            var list = new List<FileCabinetRecord>();

            while (this.fileStream.Position < this.fileStream.Length)
            {
                this.fileStream.Seek(2, SeekOrigin.Current);
                //var statusByte = binaryReader.ReadBytes(sizeof(short));
                //short status = BitConverter.ToInt16(statusByte);
                //size += sizeof(short);

                var idinByte = binaryReader.ReadBytes(sizeof(int));
                int id = BitConverter.ToInt32(idinByte);

                var massChar = new char[60];
                byte[] massFNamebyte = Encoding.Unicode.GetBytes(massChar);

                var firstNameChar = binaryReader.ReadBytes(massFNamebyte.Length);
                var firstNameFull = Encoding.Unicode.GetString(firstNameChar);
                var firstName = this.RemoveSymbols(firstNameFull);

                var lastNameChar = binaryReader.ReadBytes(massFNamebyte.Length);
                var lastNameFull = Encoding.Unicode.GetString(lastNameChar);
                var lastName = this.RemoveSymbols(lastNameFull);

                var yearByte = binaryReader.ReadBytes(sizeof(int));
                int year = BitConverter.ToInt32(yearByte);

                var monthByte = binaryReader.ReadBytes(sizeof(int));
                int month = BitConverter.ToInt32(monthByte);

                var dayByte = binaryReader.ReadBytes(sizeof(int));
                int day = BitConverter.ToInt32(dayByte);

                DateTime dateofBirth = new DateTime(year, month, day);

                var cabinetNumberByte = binaryReader.ReadBytes(sizeof(short));
                short cabinetNumber = BitConverter.ToInt16(cabinetNumberByte);

                decimal salary = binaryReader.ReadDecimal();

                var categoryByte = binaryReader.ReadBytes(sizeof(char));
                char category = BitConverter.ToChar(categoryByte);

                var record = new FileCabinetRecord
                {
                    Id = id,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateofBirth,
                    CabinetNumber = cabinetNumber,
                    Salary = salary,
                    Category = category,
                };

                list.Add(record);
            }

            ReadOnlyCollection<FileCabinetRecord> records = new ReadOnlyCollection<FileCabinetRecord>(list);

            binaryReader.Dispose();
            return records;
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            return this.GetRecords().Count;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            var records = this.GetRecords();

            var listRecords = new List<FileCabinetRecord>();

            foreach (var item in records)
            {
                listRecords.Add(item);
            }

            var snapshot = new FileCabinetServiceSnapshot(this.GetRecords());

            return snapshot;
        }

        private int GetSizeRecords()
        {
            int sizerecord = 0;
            sizerecord += sizeof(short);
            sizerecord += sizeof(int);

            var massChar = new char[60];
            for (int i = 0; i < massChar.Length; i++)
            {
                massChar[i] = '\0';
            }

            byte[] massFNamebyte = Encoding.Unicode.GetBytes(massChar);
            sizerecord += massFNamebyte.Length;
            byte[] massLNamebyte = Encoding.Unicode.GetBytes(massChar);
            sizerecord += massLNamebyte.Length;

            sizerecord += sizeof(int);
            sizerecord += sizeof(int);
            sizerecord += sizeof(int);
            sizerecord += sizeof(short);
            sizerecord += sizeof(decimal);
            sizerecord += sizeof(char);

            return sizerecord;
        }

        private byte[] GetRecordArray(byte[] record, byte[] byteStatus, byte[] byteId, byte[] massFNamebyte, byte[] massLNamebyte, byte[] yearByte, byte[] monthByte, byte[] dayByte, byte[] cabinetNumberByte, byte[] salaryByte, byte[] categoryByte)
        {
            int position = 0;

            record = this.AddArrayToResultArray(position, record, byteStatus);
            position += byteStatus.Length;

            record = this.AddArrayToResultArray(position, record, byteId);
            position += byteId.Length;

            record = this.AddArrayToResultArray(position, record, massFNamebyte);
            position += massFNamebyte.Length;

            record = this.AddArrayToResultArray(position, record, massLNamebyte);
            position += massLNamebyte.Length;

            record = this.AddArrayToResultArray(position, record, yearByte);
            position += yearByte.Length;

            record = this.AddArrayToResultArray(position, record, monthByte);
            position += monthByte.Length;

            record = this.AddArrayToResultArray(position, record, dayByte);
            position += dayByte.Length;

            record = this.AddArrayToResultArray(position, record, cabinetNumberByte);
            position += cabinetNumberByte.Length;

            record = this.AddArrayToResultArray(position, record, salaryByte);
            position += salaryByte.Length;

            record = this.AddArrayToResultArray(position, record, categoryByte);

            return record;
        }

        private byte[] AddArrayToResultArray(int position, byte[] result, byte[] mass)
        {
            int j = 0;
            for (int i = position; i < mass.Length + position; i++)
            {
                result[i] = mass[j];
                j++;
            }

            return result;
        }

        private string RemoveSymbols(string str)
        {
            char removdSymbol = '\0';

            foreach (var item in str)
            {
                if (item.Equals(removdSymbol))
                {
                    str = str.TrimEnd(item);
                }
            }

            return str;
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            throw new NotImplementedException();
        }
    }
}
