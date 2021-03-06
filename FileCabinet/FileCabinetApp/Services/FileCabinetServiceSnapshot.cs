﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace FileCabinetApp
{
    /// <summary>
    /// This class designed to save the file.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records = Array.Empty<FileCabinetRecord>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="list">List with records.</param>
        public FileCabinetServiceSnapshot(IList<FileCabinetRecord> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            this.Records = new ReadOnlyCollection<FileCabinetRecord>(list);
        }

        /// <summary>
        /// Gets collection Records.
        /// </summary>
        /// <value>
        /// Collection Records.
        /// </value>
        public ReadOnlyCollection<FileCabinetRecord> Records { get; }

        /// <summary>
        /// This Methods save file in csv-format.
        /// </summary>
        /// <param name="textWriter">Represents characters write.</param>
        public void SaveToCsv(StreamWriter textWriter)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            this.WriteAllRecordsCsv(textWriter, this.records);
        }

        /// <summary>
        /// This Methods save file in xml-format.
        /// </summary>
        /// <param name="textWriter">Represents characters write.</param>
        public void SaveToXml(StreamWriter textWriter)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            this.WriteAllRecordsXml(textWriter, this.records);
        }

        /// <summary>
        /// This Methods load from file in csv-format.
        /// </summary>
        /// <param name="streamReader">Represents characters read.</param>
        /// <returns>List records.</returns>
        public IList<FileCabinetRecord> LoadFromCsv(StreamReader streamReader)
        {
            FileCabinetRecordCsvReader csvReader = new FileCabinetRecordCsvReader(streamReader);

            return csvReader.ReadAll();
        }

        /// <summary>
        /// This Methods load from file in xml-format.
        /// </summary>
        /// <param name="streamReader">Represents characters read.</param>
        /// <returns>List records.</returns>
        public IList<FileCabinetRecord> LoadFromXml(StreamReader streamReader)
        {
            FileCabinetRecordXmlReader xmlReader = new FileCabinetRecordXmlReader(streamReader);

            return xmlReader.ReadAll();
        }

        private void WriteAllRecordsCsv(StreamWriter textWriter, FileCabinetRecord[] records)
        {
            var csvWriter = new FileCabinetRecordCsvWriter(textWriter);

            string fieldsName = "Id,First Name,Last Name,Date of Birth,Cabinet Number,Salary,Category";
            textWriter.WriteLine(fieldsName);

            foreach (var item in records)
            {
                csvWriter.Write(item);
            }
        }

        private void WriteAllRecordsXml(StreamWriter textWriter, FileCabinetRecord[] records)
        {
            var xmlWriter = new FileCabinetRecordXmlWriter(textWriter);

            xmlWriter.Write(records);
        }
    }
}
