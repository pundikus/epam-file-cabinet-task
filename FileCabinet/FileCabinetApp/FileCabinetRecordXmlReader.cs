using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// This class implements read records from file in csv-format.
    /// </summary>
    public class FileCabinetRecordXmlReader
    {
        private StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="reader">Represents characters read.</param>
        public FileCabinetRecordXmlReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// This Method implements read records from file in csv-format.
        /// </summary>
        /// <returns>List records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            var list = new List<FileCabinetRecord>();

            using (XmlReader a = XmlReader.Create(this.reader.BaseStream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<FileCabinetRecord>));

                list = (List<FileCabinetRecord>)serializer.Deserialize(a);
            }

            return list;
        }
    }
}
