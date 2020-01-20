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

            int id = 10;
            short status = 500;

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

            var sizerecord = this.GetSizeRecords(parametrs);

            byte[] record = new byte[sizerecord];
            record = this.GetRecordArray(record, byteStatus, byteId, massFNamebyte, massLNamebyte, yearByte, monthByte, dayByte, cabinetNumberByte, salaryByte, categoryByte);

            BinaryWriter binaryWriter = new BinaryWriter(this.fileStream);

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

            binaryWriter.Flush();

            return id;
        }

        /// <inheritdoc/>
        public void EditRecord(ValueRange parametrs)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);

            BinaryReader binaryReader = new BinaryReader(this.fileStream);

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

            return records;
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            return this.GetRecords().Count;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }

        private int GetSizeRecords(ValueRange parametrs)
        {
            int id = 1;
            int sizerecord = 0;
            short status = 500;

            var byteStatus = BitConverter.GetBytes(status);
            sizerecord += byteStatus.Length;

            var byteId = BitConverter.GetBytes(id);
            sizerecord += byteId.Length;

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

            sizerecord += massFNamebyte.Length;

            byte[] massLNamebyte = Encoding.Unicode.GetBytes(massChar);

            var lastNameByte = Encoding.Unicode.GetBytes(parametrs.LastName);
            if (lastNameByte.Length < massLNamebyte.Length)
            {
                for (int i = 0; i < lastNameByte.Length; i++)
                {
                    massLNamebyte[i] = lastNameByte[i];
                }
            }

            sizerecord += massLNamebyte.Length;

            var yearByte = BitConverter.GetBytes(parametrs.DateOfBirth.Year);
            sizerecord += yearByte.Length;

            var monthByte = BitConverter.GetBytes(parametrs.DateOfBirth.Month);
            sizerecord += monthByte.Length;

            var dayByte = BitConverter.GetBytes(parametrs.DateOfBirth.Day);
            sizerecord += dayByte.Length;

            var cabinetNumberByte = BitConverter.GetBytes(parametrs.CabinetNumber);
            sizerecord += cabinetNumberByte.Length;

            int[] bits = decimal.GetBits(parametrs.Salary);
            List<byte> bytes = new List<byte>();
            foreach (int i in bits)
            {
                bytes.AddRange(BitConverter.GetBytes(i));
            }

            var salaryByte = bytes.ToArray();
            sizerecord += salaryByte.Length;

            var categoryByte = BitConverter.GetBytes(parametrs.Category);
            sizerecord += categoryByte.Length;

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
    }
}
