using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetGenerator
{
    public class RecordModel
    {
        /// <summary>
        /// Gets or sets is a unique identifier for each user.
        /// </summary>
        /// <value>
        /// Is a unique identifier for each user.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets is a Firstname for each user.
        /// </summary>
        /// <value>
        /// Is a Firstname for each user.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets is a Lastname for each user.
        /// </summary>
        /// <value>
        /// Is a Lastname for each user.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets is a Date of Birth for each user.
        /// </summary>
        /// <value>
        /// Is a Date of Birth for each user.
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets is a cabinet number for each user.
        /// </summary>
        /// <value>
        /// Is a cabinet number for each user.
        /// </value>
        public short CabinetNumber { get; set; }

        /// <summary>
        /// Gets or sets is a Salary for each user.
        /// </summary>
        /// <value>
        /// Is a Salary for each user.
        /// </value>
        public decimal Salary { get; set; }

        /// <summary>
        /// Gets or sets is a Category for each user.
        /// </summary>
        /// <value>
        /// Is a Category for each user.
        /// </value>
        public char Category { get; set; }
    }
}
