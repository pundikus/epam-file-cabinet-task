using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using FileCabinetApp.Helpers;

namespace FileCabinetApp.Helpers
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
        public int CreateRecord(FileCabinetRecord parametrs)
        {
            if (parametrs == null)
            {
                throw new ArgumentNullException(nameof(parametrs));
            }

            int id;
            BinaryWriter binaryWriter;

            if (parametrs.Id == 0)
            {
                id = this.GetRecords().Count + 1;
            }
            else
            {
                id = parametrs.Id;
            }

            if (!this.fileStream.CanWrite)
            {
                this.fileStream = File.OpenWrite(this.fileStream.Name);
                binaryWriter = new BinaryWriter(this.fileStream);
            }
            else
            {
                binaryWriter = new BinaryWriter(this.fileStream);
            }

            this.fileStream.Seek(0, SeekOrigin.End);

            var record = FileHelper.GetRecordInByte(parametrs, id);

            this.WriteRecord(binaryWriter, record);

            binaryWriter.Dispose();

            return id;
        }

        /// <inheritdoc/>
        public void EditRecord(FileCabinetRecord parametrs)
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

            int position = 0;

            while (this.fileStream.Position < this.fileStream.Length)
            {
                this.fileStream.Seek(position, SeekOrigin.Begin);

                var statusMass = binaryReader.ReadBytes(sizeof(short));
                var bitsStatus = new BitArray(statusMass);
                if (bitsStatus[2])
                {
                    Console.WriteLine("This records it's marks as deleted!");
                    break;
                }

                var idinByte = binaryReader.ReadBytes(sizeof(int));
                int id = BitConverter.ToInt32(idinByte);

                if (id.Equals(parametrs.Id))
                {
                    this.fileStream.Seek(position, SeekOrigin.Begin);

                    var record = FileHelper.GetRecordInByte(parametrs, id);

                    this.WriteRecord(binaryWriter, record);

                    break;
                }
                else
                {
                    position += FileHelper.GetSizeRecords();
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
            int positionDeleteRecords = 0;

            while (this.fileStream.Position < this.fileStream.Length)
            {
                this.fileStream.Seek(positionDeleteRecords, SeekOrigin.Begin);

                var status = binaryReader.ReadBytes(sizeof(short));
                var bitsStatus = new BitArray(status);
                if (bitsStatus[2])
                {
                    position += FileHelper.GetSizeRecords();
                }

                positionDeleteRecords += FileHelper.GetSizeRecords();

                this.fileStream.Seek(position, SeekOrigin.Begin);

                if (this.fileStream.Position > this.fileStream.Length)
                {
                    break;
                }

                DateTime dateofBirth = FileHelper.ReadDateofBirth(binaryReader);

                var date = DateTime.Parse(dateOfBirth, CultureInfo.InvariantCulture);

                if (date.Equals(dateofBirth))
                {
                    this.fileStream.Seek(position - 244, SeekOrigin.Begin);

                    var record = FileHelper.ReadRecords(binaryReader);

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
            int positionDeleteRecords = 0;

            while (this.fileStream.Position < this.fileStream.Length)
            {
                this.fileStream.Seek(positionDeleteRecords, SeekOrigin.Begin);

                var status = binaryReader.ReadBytes(sizeof(short));
                var bitsStatus = new BitArray(status);
                if (bitsStatus[2])
                {
                    position += FileHelper.GetSizeRecords();
                }

                positionDeleteRecords += FileHelper.GetSizeRecords();

                this.fileStream.Seek(position, SeekOrigin.Begin);

                var firstnameInFile = FileHelper.ReadFirstName(binaryReader);

                if (firstName.Equals(firstnameInFile, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.fileStream.Seek(position - 4, SeekOrigin.Begin);

                    var record = FileHelper.ReadRecords(binaryReader);

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
            int positionDeleteRecords = 0;

            while (this.fileStream.Position < this.fileStream.Length)
            {
                this.fileStream.Seek(positionDeleteRecords, SeekOrigin.Begin);

                var status = binaryReader.ReadBytes(sizeof(short));
                var bitsStatus = new BitArray(status);
                if (bitsStatus[2])
                {
                    position += FileHelper.GetSizeRecords();
                }

                positionDeleteRecords += FileHelper.GetSizeRecords();

                this.fileStream.Seek(position, SeekOrigin.Begin);

                var lastnameInFile = FileHelper.ReadFirstName(binaryReader);

                if (lastName.Equals(lastnameInFile, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.fileStream.Seek(position - 124, SeekOrigin.Begin);

                    var record = FileHelper.ReadRecords(binaryReader);
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

            this.fileStream.Seek(0, SeekOrigin.Begin);

            while (this.fileStream.Position < this.fileStream.Length)
            {
                this.fileStream.Seek(2, SeekOrigin.Current);

                var record = FileHelper.ReadRecords(binaryReader);

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

        /// <inheritdoc/>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            var records = snapshot.Records;

            if (this.GetRecords().Count == 0)
            {
                foreach (var recordImport in records)
                {
                    this.CreateRecord(recordImport);
                }
            }
            else
            {
                var recordList = this.GetRecords();

                var newList = new List<FileCabinetRecord>();
                foreach (var item in recordList)
                {
                    newList.Add(item);
                }

                foreach (var recordImport in records)
                {
                    var recordbyId = newList.Find(x => x.Id == recordImport.Id);

                    if (recordbyId != null)
                    {
                        this.EditRecord(recordImport);
                    }
                    else
                    {
                        this.CreateRecord(recordImport);
                    }
                }
            }
        }

        /// <summary>
        /// This Method marks the entry as deleted.
        /// </summary>
        /// <param name="removedId">It is Id removed record.</param>
        /// <returns>Id marks record.</returns>
        public int RemoveRecord(int removedId)
        {
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
            int id = 0;

            while (this.fileStream.Position < this.fileStream.Length)
            {
                this.fileStream.Seek(position, SeekOrigin.Begin);
                var idinByte = binaryReader.ReadBytes(sizeof(int));
                id = BitConverter.ToInt32(idinByte);

                if (id.Equals(removedId))
                {
                    this.fileStream.Seek(position - 2, SeekOrigin.Begin);

                    var status = binaryReader.ReadBytes(sizeof(short));

                    this.fileStream.Seek(position - 2, SeekOrigin.Begin);

                    var bitsStatus = new BitArray(status);
                    bitsStatus[2] = true;
                    bitsStatus.CopyTo(status, 0);

                    binaryWriter.Write(status);

                    break;
                }

                position += 278;
            }

            binaryReader.Dispose();
            binaryWriter.Dispose();

            return id;
        }

        /// <summary>
        /// This Method flush marks records.
        /// </summary>
        /// <returns>Count removed records.</returns>
        public int PurgeRecords()
        {
            BinaryReader binaryReader;
            BinaryWriter binaryWriter;

            int countRemovedRecords = 0;

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

            int positionDeleteRecords = 0;

            while (this.fileStream.Position < this.fileStream.Length)
            {
                this.fileStream.Seek(positionDeleteRecords, SeekOrigin.Begin);

                var status = binaryReader.ReadBytes(sizeof(short));
                if (status.Length == 0)
                {
                    break;
                }

                var bitsStatus = new BitArray(status);
                if (bitsStatus[2])
                {
                    countRemovedRecords++;

                    this.fileStream.Seek(positionDeleteRecords + FileHelper.GetSizeRecords(), SeekOrigin.Begin);
                    var buffer = binaryReader.ReadBytes((int)this.fileStream.Length - positionDeleteRecords);

                    this.fileStream.Seek(positionDeleteRecords, SeekOrigin.Begin);
                    binaryWriter.Write(buffer);

                    this.fileStream.SetLength(this.fileStream.Length - FileHelper.GetSizeRecords());
                    positionDeleteRecords += FileHelper.GetSizeRecords();
                }
                else
                {
                    positionDeleteRecords += FileHelper.GetSizeRecords();
                }

                if (this.fileStream.Position == this.fileStream.Length)
                {
                    positionDeleteRecords = 0;
                    this.fileStream.Seek(positionDeleteRecords, SeekOrigin.Begin);
                }
            }

            binaryReader.Dispose();
            binaryWriter.Dispose();

            return countRemovedRecords;
        }

        private void WriteRecord(BinaryWriter binaryWriter, byte[] record)
        {
            int offset = 0;

            binaryWriter.Write(record, offset, sizeof(short));
            offset += sizeof(short);

            binaryWriter.Write(record, offset, sizeof(int));
            offset += sizeof(int);

            binaryWriter.Write(record, offset, 120);
            offset += 120;

            binaryWriter.Write(record, offset, 120);
            offset += 120;

            binaryWriter.Write(record, offset, sizeof(int));
            offset += sizeof(int);

            binaryWriter.Write(record, offset, sizeof(int));
            offset += sizeof(int);

            binaryWriter.Write(record, offset, sizeof(int));
            offset += sizeof(int);

            binaryWriter.Write(record, offset, sizeof(short));
            offset += sizeof(short);

            binaryWriter.Write(record, offset, sizeof(decimal));
            offset += sizeof(decimal);

            binaryWriter.Write(record, offset, sizeof(char));
        }
    }
}
