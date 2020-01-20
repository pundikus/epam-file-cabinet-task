using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            this.fileStream.Seek(0, SeekOrigin.End);

            int id = 1;
            short status = 500;

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

            BinaryWriter binaryWriter = new BinaryWriter(this.fileStream);

            var sizerecord = this.GetSizeRecords(parametrs);

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

            binaryWriter.Flush(); 
            //binaryWriter.Dispose();

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
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            throw new NotImplementedException();
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

            record = AddArrayToResultArray(position, record, byteStatus);
            position += byteStatus.Length;

            record = AddArrayToResultArray(position, record, byteId);
            position += byteId.Length;

            record = AddArrayToResultArray(position, record, massFNamebyte);
            position += massFNamebyte.Length;

            record = AddArrayToResultArray(position, record, massLNamebyte);
            position += massLNamebyte.Length;

            record = AddArrayToResultArray(position, record, yearByte);
            position += yearByte.Length;

            record = AddArrayToResultArray(position, record, monthByte);
            position += monthByte.Length;

            record = AddArrayToResultArray(position, record, dayByte);
            position += dayByte.Length;

            record = AddArrayToResultArray(position, record, cabinetNumberByte);
            position += cabinetNumberByte.Length;

            record = AddArrayToResultArray(position, record, salaryByte);
            position += salaryByte.Length;

            record = AddArrayToResultArray(position, record, categoryByte);

            return record;
        }

        private static byte[] AddArrayToResultArray(int position, byte[] result, byte[] mass)
        {
            int j = 0;
            for (int i = position; i < mass.Length + position; i++)
            {
                result[i] = mass[j];
                j++;
            }

            return result;
        }
    }
}
