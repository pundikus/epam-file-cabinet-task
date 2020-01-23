using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    public class RecordXmlWriter
    {
        private StreamWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">Represents characters write.</param>
        public RecordXmlWriter(StreamWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// This Method implements write records in file to xml-format.
        /// </summary>
        /// <param name="records">It is records from our list.</param>
        public void Write(List<FileCabinetApp.FileCabinetRecord> records)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<FileCabinetApp.FileCabinetRecord>));

            serializer.Serialize(this.writer, records);
        }
    }
}
