using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for FileCabinetFileSystemService.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Get's record in byte Array.
        /// </summary>
        /// <param name="parametrs">FileCabinetRecords params.</param>
        /// <param name="id">id user.</param>
        /// <returns>Record in byte Array.</returns>
        public static byte[] GetRecordInByte(FileCabinetRecord parametrs, int id)
        {
            if (parametrs == null)
            {
                throw new ArgumentNullException(nameof(parametrs));
            }

            short status = 0;

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

            var sizerecord = GetSizeRecords();

            byte[] record = new byte[sizerecord];

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

        /// <summary>
        /// Get's size one record.
        /// </summary>
        /// <returns>size record.</returns>
        public static int GetSizeRecords()
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

        /// <summary>
        /// Get's Date of Birth in file.
        /// </summary>
        /// <param name="binaryReader">inputs stream.</param>
        /// <returns>Read's Date of Birth.</returns>
        public static DateTime ReadDateofBirth(BinaryReader binaryReader)
        {
            if (binaryReader == null)
            {
                throw new ArgumentNullException(nameof(binaryReader));
            }

            var yearInByte = binaryReader.ReadBytes(sizeof(int));
            int yearIn = BitConverter.ToInt32(yearInByte);

            var monthInByte = binaryReader.ReadBytes(sizeof(int));
            int monthIn = BitConverter.ToInt32(monthInByte);

            var dayInByte = binaryReader.ReadBytes(sizeof(int));
            int dayIn = BitConverter.ToInt32(dayInByte);

            DateTime dateofBirth = new DateTime(yearIn, monthIn, dayIn);

            return dateofBirth;
        }

        /// <summary>
        /// Get's First Name in file.
        /// </summary>
        /// <param name="binaryReader">inputs stream.</param>
        /// <returns>First Name.</returns>
        public static string ReadFirstName(BinaryReader binaryReader)
        {
            if (binaryReader == null)
            {
                throw new ArgumentNullException(nameof(binaryReader));
            }

            var massChar = new char[60];
            byte[] masFNamebyte = Encoding.Unicode.GetBytes(massChar);

            var firstnameChar = binaryReader.ReadBytes(masFNamebyte.Length);
            var firstnameFull = Encoding.Unicode.GetString(firstnameChar);
            var firstnameInFile = RemoveSymbols(firstnameFull);

            return firstnameInFile;
        }

        /// <summary>
        /// Get's one record in file.
        /// </summary>
        /// <param name="binaryReader">inputs stream.</param>
        /// <returns>records in byte Array.</returns>
        public static FileCabinetRecord ReadRecords(BinaryReader binaryReader)
        {
            if (binaryReader == null)
            {
                throw new ArgumentNullException(nameof(binaryReader));
            }

            binaryReader.BaseStream.Seek(0, SeekOrigin.Current);

            if (binaryReader.BaseStream.Position > binaryReader.BaseStream.Length)
            {
                binaryReader.BaseStream.Seek(binaryReader.BaseStream.Position - 2, SeekOrigin.Begin);
            }

            if (binaryReader.BaseStream.Position == binaryReader.BaseStream.Length)
            {
                return null;
            }

            var idinByte = binaryReader.ReadBytes(sizeof(int));
            int id = BitConverter.ToInt32(idinByte);

            var massChar = new char[60];
            byte[] massFNamebyte = Encoding.Unicode.GetBytes(massChar);

            var firstNameChar = binaryReader.ReadBytes(massFNamebyte.Length);
            var firstNameFull = Encoding.Unicode.GetString(firstNameChar);
            var firstNameInFile = RemoveSymbols(firstNameFull);

            byte[] masLNamebyte = Encoding.Unicode.GetBytes(massChar);
            var lastNameChar = binaryReader.ReadBytes(masLNamebyte.Length);
            var lastNameFull = Encoding.Unicode.GetString(lastNameChar);
            var lastNameInFile = RemoveSymbols(lastNameFull);

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

        private static string RemoveSymbols(string str)
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
