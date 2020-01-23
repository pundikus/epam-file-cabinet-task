using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// This class storage parametrs.
    /// </summary>
    [Serializable]
    public class FileCabinetRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <param name="firstName">It is Firstname user.</param>
        /// <param name="lastName">It is Lastname user.</param>
        /// <param name="dateOfBirth">It is Date of Birth user.</param>
        /// <param name="cabinetNumber">It is Cabinet Number user.</param>
        /// <param name="salary">It is Salary user.</param>
        /// <param name="category">It is Category user.</param>
        public FileCabinetRecord(string firstName, string lastName, DateTime dateOfBirth, short cabinetNumber, decimal salary, char category)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.CabinetNumber = cabinetNumber;
            this.Salary = salary;
            this.Category = category;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        /// <param name="id">It is unique identificator user.</param>
        /// <param name="firstName">It is Firstname user.</param>
        /// <param name="lastName">It is Lastname user.</param>
        /// <param name="dateOfBirth">It is Date of Birth user.</param>
        /// <param name="cabinetNumber">It is Cabinet Number user.</param>
        /// <param name="salary">It is Salary user.</param>
        /// <param name="category">It is Category user.</param>
        public FileCabinetRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short cabinetNumber, decimal salary, char category)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.CabinetNumber = cabinetNumber;
            this.Salary = salary;
            this.Category = category;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class.
        /// </summary>
        public FileCabinetRecord()
        {

        }

        /// <summary>
        /// Gets or sets is a unique identifier for each user.
        /// </summary>
        /// <value>
        /// Is a unique identifier for each user.
        /// </value>
        [XmlAttribute]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets is a Firstname for each user.
        /// </summary>
        /// <value>
        /// Is a Firstname for each user.
        /// </value>
        [XmlElement]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets is a Lastname for each user.
        /// </summary>
        /// <value>
        /// Is a Lastname for each user.
        /// </value>
        [XmlElement]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets is a Date of Birth for each user.
        /// </summary>
        /// <value>
        /// Is a Date of Birth for each user.
        /// </value>
        [XmlElement]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets is a cabinet number for each user.
        /// </summary>
        /// <value>
        /// Is a cabinet number for each user.
        /// </value>
        [XmlElement]
        public short CabinetNumber { get; set; }

        /// <summary>
        /// Gets or sets is a Salary for each user.
        /// </summary>
        /// <value>
        /// Is a Salary for each user.
        /// </value>
        [XmlElement]
        public decimal Salary { get; set; }

        /// <summary>
        /// Gets or sets is a Category for each user.
        /// </summary>
        /// <value>
        /// Is a Category for each user.
        /// </value>
        [XmlElement]
        public char Category { get; set; }
    }
}
