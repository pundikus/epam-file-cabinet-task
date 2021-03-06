﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using FileCabinetApp.Interfaces.Validators;

namespace FileCabinetApp.Helpers
{
    /// <summary>
    /// Class for working with list of users.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        /// <summary>
        /// The size of the record in bytes.
        /// </summary>
        public const int RecordSize = 280;

        private const int StringSize = 120;

        private readonly FileStream fileStream;

        private readonly IRecordValidator validator;

        private SortedList<int, int> recordIdOffset = new SortedList<int, int>();
        private SortedList<string, List<int>> recordFirstNameOffset = new SortedList<string, List<int>>();
        private SortedList<string, List<int>> recordLastNameOffset = new SortedList<string, List<int>>();
        private SortedList<DateTime, List<int>> recordDateOfBirthOffset = new SortedList<DateTime, List<int>>();
        private SortedList<short, List<int>> recordCabinetNumberOffset = new SortedList<short, List<int>>();
        private SortedList<char, List<int>> recordCategoryOffset = new SortedList<char, List<int>>();
        private SortedList<decimal, List<int>> recordSalaryOffset = new SortedList<decimal, List<int>>();

        private int count;
        private int deleted;

        private List<int> idlist = new List<int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">FileStream with opened binary file.</param>
        /// <param name="validator">Specific validator.</param>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream;
            this.validator = validator;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// Closes the stream.
        /// </summary>
        ~FileCabinetFilesystemService()
        {
            this.fileStream.Close();
        }

        /// <summary>
        /// Creates a new record.
        /// </summary>
        /// <param name="record">User's info.</param>
        /// <returns>User's id in the users' list.</returns>
        public int CreateRecord(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException($"Record object is invalid.");
            }

            this.validator.Validate(record);

            if (this.idlist.Count == 0)
            {
                record.Id = 1;
            }
            else
            {
                record.Id = this.idlist.Max() + 1;
            }

            this.count++;
            this.idlist.Add(record.Id);
            this.WriteRecord(record, (this.count - 1) * RecordSize);
            this.AddOffsets(record, (this.count - 1) * RecordSize);

            return record.Id;
        }

        /// <summary>
        /// Adds record to the list of records.
        /// </summary>
        /// <param name="record">Record to add.</param>
        /// <returns>Record's id.</returns>
        public int AddRecord(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException($"Record object is invalid.");
            }

            this.validator.Validate(record);

            if (this.idlist.Contains(record.Id))
            {
                int recordOffset = this.recordIdOffset[record.Id];
                FileCabinetRecord prevRecord = this.ReadRecord(recordOffset);
                this.DeleteOffsets(prevRecord, recordOffset);
                this.WriteRecord(record, recordOffset);
                this.AddOffsets(record, recordOffset);
            }
            else
            {
                this.idlist.Add(record.Id);
                this.count++;
                this.WriteRecord(record, (this.count - 1) * RecordSize);
                this.AddOffsets(record, (this.count - 1) * RecordSize);
            }

            return record.Id;
        }

        /// <summary>
        /// Edits the existing record.
        /// </summary>
        /// <param name="record">Record to edit.</param>
        /// <returns>Whether operation succeeded.</returns>
        public int EditRecord(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException($"Record object is invalid.");
            }

            if (record.Id < 0 || !this.idlist.Contains(record.Id))
            {
                return -1;
            }

            int recordOffset = this.recordIdOffset[record.Id];
            FileCabinetRecord prevRecord = this.ReadRecord(recordOffset);
            this.WriteRecord(record, recordOffset);
            this.DeleteOffsets(prevRecord, recordOffset);
            this.AddOffsets(record, recordOffset);

            return record.Id;
        }

        /// <summary>
        /// Removes a record from the list by given id.
        /// </summary>
        /// <param name="id">Id to remove record by.</param>
        /// <returns>Id of removed record if succeeded, -1 otherwise.
        /// </returns>
        public int RemoveRecord(int id)
        {
            int offset = this.recordIdOffset[id];
            if (offset == -1)
            {
                return -1;
            }

            FileCabinetRecord prevRecord = this.ReadRecord(offset);
            this.DeleteOffsets(prevRecord, offset);

            ushort numToAdd = 4;

            byte[] reservedBytes = new byte[2];
            this.fileStream.Seek(offset, SeekOrigin.Begin);
            this.fileStream.Read(reservedBytes, 0, 2);

            reservedBytes = BitConverter.GetBytes((ushort)(BitConverter.ToUInt16(reservedBytes) | numToAdd));

            this.fileStream.Seek(offset, SeekOrigin.Begin);

            this.fileStream.Write(reservedBytes, 0, 2);

            this.idlist.Remove(id);

            this.deleted++;

            return id;
        }

        /// <summary>
        /// Purges the list of records.
        /// </summary>
        /// <returns>The number of purged records,
        /// -1 if not all the records were purged.
        /// </returns>
        public int Purge()
        {
            this.PurgeIds();

            int recordsPurged = 0;
            List<int> indexes = new List<int>();

            for (int i = 0; i < this.count; i++)
            {
                if (this.IsDeleted(RecordSize * i))
                {
                    indexes.Add(i);
                }
                else
                {
                    continue;
                }
            }

            int currentIndex = 0;
            int numOfRecords = this.count;
            FileCabinetRecord currentRecord;

            for (int i = 0; i < numOfRecords; i++)
            {
                if (!indexes.Contains(i))
                {
                    if (i == currentIndex)
                    {
                        currentIndex++;
                        continue;
                    }

                    currentRecord = this.ReadRecord(i * RecordSize);
                    this.WriteRecord(currentRecord, currentIndex * RecordSize);
                    currentIndex++;
                }
                else
                {
                    recordsPurged++;
                    continue;
                }
            }

            this.count -= recordsPurged;

            if (this.deleted == recordsPurged)
            {
                this.deleted = 0;
                return recordsPurged;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Searches the records by id.
        /// </summary>
        /// <param name="id">Given id.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindById(string id)
        {
            if (id == null || !int.TryParse(id, out int parsedId))
            {
                return null;
            }

            return new FileSystemRecordEnumeratorCollection(this, new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>() { this.GetRecord(parsedId) }));
        }

        /// <summary>
        /// Searches the records by first name.
        /// </summary>
        /// <param name="firstName">Given first name.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (firstName == null)
            {
                return null;
            }

            return this.FindByKey(this.recordFirstNameOffset, firstName);
        }

        /// <summary>
        /// Searches the records by last name.
        /// </summary>
        /// <param name="lastName">Given last name.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (lastName == null)
            {
                return null;
            }

            return this.FindByKey(this.recordLastNameOffset, lastName);
        }

        /// <summary>
        /// Searches the records by date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Given date of birth.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            if (dateOfBirth == null)
            {
                return null;
            }

            if (!DateTime.TryParseExact(dateOfBirth, "yyyy-MMM-d", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateOfBirth))
            {
                return null;
            }

            return this.FindByKey(this.recordDateOfBirthOffset, parsedDateOfBirth);
        }

        /// <summary>
        /// Searches the records by cabinet number.
        /// </summary>
        /// <param name="cabinetNumber">Given cabinet number.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByCabinetNumber(string cabinetNumber)
        {
            if (cabinetNumber == null)
            {
                return null;
            }

            if (!short.TryParse(cabinetNumber, out short parsedFavNumver))
            {
                return null;
            }

            return this.FindByKey(this.recordCabinetNumberOffset, parsedFavNumver);
        }

        /// <summary>
        /// Searches the records by category.
        /// </summary>
        /// <param name="category">category.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByCategory(string category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            var categoryToChar = category.ToCharArray();

            if (category == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(category) || category.Length > 1)
            {
                return null;
            }

            return this.FindByKey<char>(this.recordCategoryOffset, categoryToChar.First());
        }

        /// <summary>
        /// Searches the records by salary.
        /// </summary>
        /// <param name="salary">Given salary.</param>
        /// <returns>The array of records.</returns>
        public IEnumerable<FileCabinetRecord> FindBySalary(string salary)
        {
            if (salary == null)
            {
                return null;
            }

            if (!decimal.TryParse(salary, out decimal parsedSalary))
            {
                return null;
            }

            return this.FindByKey(this.recordSalaryOffset, parsedSalary);
        }

        /// <summary>
        /// Gets all the records.
        /// </summary>
        /// <returns>The array of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            List<FileCabinetRecord> storedRecords = new List<FileCabinetRecord>();

            for (int i = 0; i < this.count; i++)
            {
                if (this.IsDeleted(RecordSize * i))
                {
                    continue;
                }

                storedRecords.Add(this.ReadRecord(i * RecordSize));
            }

            ReadOnlyCollection<FileCabinetRecord> records = new ReadOnlyCollection<FileCabinetRecord>(storedRecords);

            return records;
        }

        /// <summary>
        /// Returns the number of records in the list.
        /// </summary>
        /// <returns>The number of records.</returns>
        public int GetStat()
        {
            return this.count;
        }

        /// <summary>
        /// Returns the number of deleted records in the list.
        /// </summary>
        /// <returns>The number of deleted records.</returns>
        public int GetDeleted()
        {
            return this.deleted;
        }

        /// <summary>
        /// Returns list of ids in the list.
        /// </summary>
        /// <returns>List of ids in the list.</returns>
        public List<int> GetIds()
        {
            return this.idlist;
        }

        /// <summary>
        /// Gets the validator.
        /// </summary>
        /// <returns>Validator.</returns>
        public IRecordValidator GetValidator()
        {
            return this.validator;
        }

        /// <summary>
        /// Makes a snapshot of records in the concrete moment.
        /// </summary>
        /// <returns>An instance of the IFileCabinetServiceSnapshot class.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            ReadOnlyCollection<FileCabinetRecord> records = this.GetRecords();

            return new FileCabinetServiceSnapshot(records);
        }

        /// <summary>
        /// Restores the records from the snapshot.
        /// </summary>
        /// <param name="snapshot">A snapshot to restore.</param>
        /// <returns>Number of imported records.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException($"Snapshot is invalid.");
            }

            int recordsImported = 0;

            foreach (var record in snapshot.Records)
            {
                this.validator.Validate(record);

                if (this.idlist.Contains(record.Id))
                {
                    this.EditRecord(record);
                    recordsImported++;
                }
                else
                {
                    this.AddRecord(record);
                    recordsImported++;
                }
            }

            return recordsImported;
        }

        /// <summary>
        /// Reads the record from the file.
        /// </summary>
        /// <param name="offset">Offset to read from.</param>
        /// <returns>Record.</returns>
        public FileCabinetRecord ReadRecord(int offset)
        {
            this.fileStream.Seek(offset, SeekOrigin.Begin);
            byte[] reservedBytes = new byte[2];

            this.fileStream.Read(reservedBytes, 0, 2);
            byte[] bytes = new byte[sizeof(int)];

            this.fileStream.Read(bytes, 0, sizeof(int));
            int id = BitConverter.ToInt32(bytes);
            bytes = new byte[120];

            this.fileStream.Read(bytes, 0, 120);
            string firstName = BytesHelper.RemoveOffset(Encoding.ASCII.GetString(bytes));

            this.fileStream.Read(bytes, 0, 120);
            string lastName = BytesHelper.RemoveOffset(Encoding.ASCII.GetString(bytes));
            bytes = new byte[sizeof(int)];

            this.fileStream.Read(bytes, 0, sizeof(int));
            int year = BitConverter.ToInt32(bytes);

            this.fileStream.Read(bytes, 0, sizeof(int));
            int month = BitConverter.ToInt32(bytes);

            this.fileStream.Read(bytes, 0, sizeof(int));
            int day = BitConverter.ToInt32(bytes);
            bytes = new byte[sizeof(short)];

            this.fileStream.Read(bytes, 0, sizeof(short));
            short cabinetNumber = BitConverter.ToInt16(bytes);

            bytes = new byte[sizeof(decimal)];
            this.fileStream.Read(bytes, 0, sizeof(decimal));
            decimal salary = BytesHelper.BytesToDecimal(bytes);

            bytes = new byte[sizeof(char)];
            this.fileStream.Read(bytes, 0, sizeof(char));
            char category = BitConverter.ToChar(bytes);

            return new FileCabinetRecord(id, firstName, lastName, new DateTime(year, month, day), cabinetNumber, salary, category);
        }

        /// <summary>
        /// Writes a record to the file.
        /// </summary>
        /// <param name="record">A record to write.</param>
        /// <param name="offset">Offset to start from.</param>
        private void WriteRecord(FileCabinetRecord record, int offset)
        {
            this.fileStream.Seek(offset, SeekOrigin.Begin);
            this.fileStream.Write(new byte[2], 0, sizeof(short));
            this.fileStream.Write(BitConverter.GetBytes(record.Id), 0, sizeof(int));
            this.fileStream.Write(BytesHelper.MakeStringOffset(record.FirstName, StringSize), 0, StringSize);
            this.fileStream.Write(BytesHelper.MakeStringOffset(record.LastName, StringSize), 0, StringSize);
            this.fileStream.Write(BitConverter.GetBytes(record.DateOfBirth.Year), 0, sizeof(int));
            this.fileStream.Write(BitConverter.GetBytes(record.DateOfBirth.Month), 0, sizeof(int));
            this.fileStream.Write(BitConverter.GetBytes(record.DateOfBirth.Day), 0, sizeof(int));
            this.fileStream.Write(BitConverter.GetBytes(record.CabinetNumber), 0, sizeof(short));
            this.fileStream.Write(BytesHelper.DecimalToBytes(record.Salary), 0, sizeof(decimal));
            this.fileStream.Write(BitConverter.GetBytes(record.Category), 0, sizeof(char));
        }

        /// <summary>
        /// Removes duplicates from the ids list.
        /// </summary>
        private void PurgeIds()
        {
            this.idlist = this.idlist.Distinct().ToList();
        }

        private bool IsDeleted(int offset)
        {
            byte[] reservedBytes = new byte[2];
            this.fileStream.Seek(offset, SeekOrigin.Begin);
            this.fileStream.Read(reservedBytes, 0, 2);

            if ((ushort)((BitConverter.ToUInt16(reservedBytes) >> 2) & 1) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private FileSystemRecordEnumeratorCollection FindByKey<T>(SortedList<T, List<int>> offsetDictionary, T key)
        {
            List<FileCabinetRecord> storedRecords = new List<FileCabinetRecord>();

            if (offsetDictionary.ContainsKey(key))
            {
                List<int> offsets = offsetDictionary[key];

                foreach (int offset in offsets)
                {
                    if (this.IsDeleted(offset))
                    {
                        continue;
                    }

                    storedRecords.Add(this.ReadRecord(offset));
                }
            }

            if (storedRecords.Count == 0)
            {
                return null;
            }

            return new FileSystemRecordEnumeratorCollection(this, new ReadOnlyCollection<FileCabinetRecord>(storedRecords));
        }

        /// <summary>
        /// Gets a single method from the list of records.
        /// </summary>
        /// <param name="id">Id to get record by.</param>
        /// <returns>The record found, null otherwise.</returns>
        private FileCabinetRecord GetRecord(int id)
        {
            try
            {
                if (this.IsDeleted(this.recordIdOffset[id]))
                {
                    return null;
                }
                else
                {
                    return this.ReadRecord(this.recordIdOffset[id]);
                }
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Updates a single dictionary containing offset.
        /// </summary>
        /// <typeparam name="T">Specifies the type of property.</typeparam>
        /// <param name="dictionary">Dictionary to update.</param>
        /// <param name="value">Key to add or find by.</param>
        /// <param name="offset">Offset to add.</param>
        private void AddToDictionary<T>(SortedList<T, List<int>> dictionary, T value, int offset)
        {
            if (!dictionary.ContainsKey(value))
            {
                List<int> listOfOffsets = new List<int>
                {
                    offset,
                };

                dictionary.Add(value, listOfOffsets);
            }
            else
            {
                dictionary[value].Add(offset);
            }
        }

        /// <summary>
        /// Updates all the dictionaries containing offsets.
        /// </summary>
        /// <param name="record">Record to add properties from.</param>
        /// <param name="recordOffset">Record's offset in the file.</param>
        private void AddOffsets(FileCabinetRecord record, int recordOffset)
        {
            this.recordIdOffset.Add(record.Id, recordOffset);

            this.AddToDictionary(this.recordFirstNameOffset, record.FirstName, recordOffset);

            this.AddToDictionary(this.recordLastNameOffset, record.LastName, recordOffset);

            this.AddToDictionary(this.recordDateOfBirthOffset, record.DateOfBirth, recordOffset);

            this.AddToDictionary(this.recordCabinetNumberOffset, record.CabinetNumber, recordOffset);

            this.AddToDictionary(this.recordCategoryOffset, record.Category, recordOffset);

            this.AddToDictionary(this.recordSalaryOffset, record.Salary, recordOffset);
        }

        /// <summary>
        /// Deletes from all the dictionaries containing offsets.
        /// </summary>
        /// <param name="record">Record to delelte properties from.</param>
        /// <param name="recordOffset">Record's offset in the file.</param>
        private void DeleteOffsets(FileCabinetRecord record, int recordOffset)
        {
            this.recordIdOffset.Remove(record.Id);

            this.recordFirstNameOffset[record.FirstName].Remove(recordOffset);

            this.recordLastNameOffset[record.LastName].Remove(recordOffset);

            this.recordDateOfBirthOffset[record.DateOfBirth].Remove(recordOffset);

            this.recordCabinetNumberOffset[record.CabinetNumber].Remove(recordOffset);

            this.recordCategoryOffset[record.Category].Remove(recordOffset);

            this.recordSalaryOffset[record.Salary].Remove(recordOffset);
        }
    }
}