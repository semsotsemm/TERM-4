using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory_02
{
    [Serializable]
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Speciality { get; set; }
        public DateTime BirthDate { get; set; }
        public int Course { get; set; }
        public int Group { get; set; }
        public double AverageGrade { get; set; }
        public bool Gender { get; set; }
        public Address HomeAddress { get; set; } = new Address();
        public Work CurrentJob { get; set; } = new Work();
    }
}
