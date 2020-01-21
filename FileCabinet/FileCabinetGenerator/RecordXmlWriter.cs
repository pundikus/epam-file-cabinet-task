using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
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
        public void Write(RecordModel[] records)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            XmlSerializer serializer = new XmlSerializer(typeof(XElement));

            XElement root = new XElement("records");

            foreach (var item in records)
            {
                root.Add(new XElement(
                    "record",
                    new XAttribute("id", item.Id),
                    new XElement("name", new XAttribute("first", item.FirstName), new XAttribute("last", item.LastName)),
                    new XElement("dateOfBirth", item.DateOfBirth.ToString(CultureInfo.InvariantCulture)),
                    new XElement("cabinetnumber", item.CabinetNumber),
                    new XElement("salary", item.Salary),
                    new XElement("category", item.Category)));
            }

            serializer.Serialize(this.writer, root);

            this.writer.WriteLine(serializer);
        }
    }
}
