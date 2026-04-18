using System;
using System.ComponentModel.DataAnnotations;

namespace Laboratory_02
{
    // Атрибут накладывается на весь класс Student
    [AttributeUsage(AttributeTargets.Class)]
    public class WorkAgeConstraintAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var student = value as Student;
            if (student == null) return ValidationResult.Success;

            // Логика: если (Возраст - Опыт) < 14, значит начал работать слишком рано
            if (student.Age - student.CurrentJob.Experience < 14)
            {
                // Привязываем ошибку конкретно к полю Experience в объекте Job
                return new ValidationResult(
                    "Студент не мог начать работать до 14 лет (возраст минус стаж < 14)",
                    new[] { nameof(Student.Age) } // Указываем поле, чтобы ErrorProvider понял, где рисовать значок
                );
            }

            return ValidationResult.Success;
        }
    }
}