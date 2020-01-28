using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FileCabinetApp.Helpers
{

    /// <summary>
    /// Enumerator for iterating through records.
    /// </summary>
    public class FileSystemRecordEnumeratorCollection : IEnumerable<FileCabinetRecord>
    {
        private FileCabinetFilesystemService service;
        private ReadOnlyCollection<FileCabinetRecord> records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemRecordEnumeratorCollection"/> class.
        /// </summary>
        /// <param name="service">Service whose records to iterate through.</param>
        /// <param name="records">Records to iterate through.</param>
        public FileSystemRecordEnumeratorCollection(FileCabinetFilesystemService service, ReadOnlyCollection<FileCabinetRecord> records)
        {
            this.service = service;
            this.records = records;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            foreach (var record in this.records)
            {
                //yield return this.service.GetRecord(id);
                yield return record;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var id in this.service.GetIdList())
            {
                yield return this.service.GetRecord(id);
            }
        }
    }
}