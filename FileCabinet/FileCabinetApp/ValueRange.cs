using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// This class storage parametrs.
    /// </summary>
    public class ValueRange
    {
        /// <summary>
        /// Gets is a unique identifier for each user.
        /// </summary>
        /// <value>
        /// Is a unique identifier for each user.
        /// </value>
        public int Id { get;  }

        /// <summary>
        /// Gets is a Firstname for each user.
        /// </summary>
        /// <value>
        /// Is a Firstname for each user.
        /// </value>
        public string FirstName { get;  }

        /// <summary>
        /// Gets is a Lastname for each user.
        /// </summary>
        /// <value>
        /// Is a Lastname for each user.
        /// </value>
        public string LastName { get; }

        /// <summary>
        /// Gets is a Date of Birth for each user.
        /// </summary>
        /// <value>
        /// Is a Date of Birth for each user.
        /// </value>
        public DateTime DateOfBirth { get;  }

        /// <summary>
        /// Gets is a cabinet number for each user.
        /// </summary>
        /// <value>
        /// Is a cabinet number for each user.
        /// </value>
        public short CabinetNumber { get; }

        /// <summary>
        /// Gets is a Salary for each user.
        /// </summary>
        /// <value>
        /// Is a Salary for each user.
        /// </value>
        public decimal Salary { get; }

        /// <summary>
        /// Gets is a Category for each user.
        /// </summary>
        /// <value>
        /// Is a Category for each user.
        /// </value>
        public char Category { get;  }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueRange"/> class.
        /// </summary>
        /// <param name="firstName">It is Firstname user.</param>
        /// <param name="lastName">It is Lastname user.</param>
        /// <param name="dateOfBirth">It is Date of Birth user.</param>
        /// <param name="cabinetNumber">It is Cabinet Number user.</param>
        /// <param name="salary">It is Salary user.</param>
        /// <param name="category">It is Category user.</param>
        public ValueRange(string firstName, string lastName, DateTime dateOfBirth, short cabinetNumber, decimal salary, char category)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.CabinetNumber = cabinetNumber;
            this.Salary = salary;
            this.Category = category;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueRange"/> class.
        /// </summary>
        /// <param name="id">It is unique identificator user.</param>
        /// <param name="firstName">It is Firstname user.</param>
        /// <param name="lastName">It is Lastname user.</param>
        /// <param name="dateOfBirth">It is Date of Birth user.</param>
        /// <param name="cabinetNumber">It is Cabinet Number user.</param>
        /// <param name="salary">It is Salary user.</param>
        /// <param name="category">It is Category user.</param>
        public ValueRange(int id, string firstName, string lastName, DateTime dateOfBirth, short cabinetNumber, decimal salary, char category)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.CabinetNumber = cabinetNumber;
            this.Salary = salary;
            this.Category = category;
        }
    }
}
