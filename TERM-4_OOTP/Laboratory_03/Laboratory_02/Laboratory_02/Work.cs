using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Laboratory_02
{
    [Serializable]
    public class Work
    {
        [Required(ErrorMessage ="Это поле обязательно")]
        [StringLength(30, MinimumLength =5, ErrorMessage ="Длина названия компании должна быть от 5 до 30 символов")]
        [RegularExpression(@"^[А-Яа-яA-Za-z]+$", ErrorMessage ="Название компании может включать только буквы")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Длина должности в компании должна быть от 5 до 30 символов")]
        [RegularExpression(@"^[А-Яа-яA-Za-z]+$", ErrorMessage = "Название должности может включать только буквы")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [Range(0,20, ErrorMessage ="Опыт должен лежать в интервале от 0 до 20")]
        public int Experience { get; set; }
    }
}
