using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Laboratory_02
{
    [Serializable]
    [WorkAgeConstraint]
    public class Student
    {
        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Длина имени может быть от 3 до 20 символов")]
        [RegularExpression(@"^[А-яа-яA-Za-z]+$", ErrorMessage = "Имя может соддержать только буквы")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Длина фамилии может быть от 3 до 20 символов")]
        [RegularExpression(@"^[А-Яа-яA-Za-z]+$", ErrorMessage = "Фамилия может содержать только буквы")]
        public string LastName { get; set; }

        [Required(ErrorMessage ="Это поле обязательно")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Длина отчества должна быть от 3 до 20 символов")]
        [RegularExpression(@"^[А-Яа-яA-Za-z]+$",ErrorMessage = "Отчество должно содержать только буквы")]
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Speciality { get; set; }
        public DateTime BirthDate { get; set; }
        public int Course { get; set; }
        public int Group { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [Range(0,10, ErrorMessage = "Средний балл должен быть в интервале от 0 до 10")]
        public double AverageGrade { get; set; }
        public bool Gender { get; set; }
        public Address HomeAddress { get; set; } = new Address();
        public Work CurrentJob { get; set; } = new Work();
    }
}
