using System;

namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public short CabinetNumber { get; set; }

        public decimal Salary { get; set; }

        public char Category { get; set; }
    }
}
