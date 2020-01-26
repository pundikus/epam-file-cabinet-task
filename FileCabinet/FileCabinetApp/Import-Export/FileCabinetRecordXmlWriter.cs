using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace FileCabinetApp
{
    /// <summary>
    ///  This class implements write records in file to xml-format.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        private StreamWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">Represents characters write.</param>
        public FileCabinetRecordXmlWriter(StreamWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// This Method implements write records in file to xml-format.
        /// </summary>
        /// <param name="records">It is records from our list.</param>
        public void Write(FileCabinetRecord[] records)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            XDocument document = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"));
            XElement root = new XElement("records");

            this.writer.WriteLine(document.Declaration);

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

            document.Add(root);

            this.writer.WriteLine(document);
        }
    }
}
