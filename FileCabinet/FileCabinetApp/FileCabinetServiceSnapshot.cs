using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
        public FileCabinetServiceSnapshot(List<FileCabinetRecord> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            this.records = list.ToArray();
        }

        /// <summary>
        /// This Methods save file in csv-format.
        /// </summary>
        /// <param name="textWriter">Represents characters write.</param>
        public void SaveToCsv(TextWriter textWriter)
        {
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            var csvWriter = new FileCabinetRecordCsvWriter(textWriter);

            string fieldsName = "Id,First Name,Last Name,Date of Birth,Cabinet Number,Salary,Category";
            textWriter.WriteLine(fieldsName);

            foreach (var item in this.records)
            {
                csvWriter.Write(item);
            }
        }
    }
}
