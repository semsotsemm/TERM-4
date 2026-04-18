using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Laboratory_02
{
    [Serializable]
    public class Address
    {
        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Длина названия города должна быть от 3 до 20 символов")]
        [RegularExpression(@"^[А-Яа-яA-Za-z]+$", ErrorMessage = "В названии города могут использоваться только буквы")]
        public string City { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [Range(100000, 999999, ErrorMessage = "Почтовый индекс должен быть в формате хххххх")]
        public string Index { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Длина названия улицы должна быть от 3 до 20 символов")]
        [RegularExpression(@"^[А-Яа-яA-Za-z]+$", ErrorMessage = "В названии улицы могут использоваться только буквы")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [Range(1, 1000, ErrorMessage = "Номер дома должен лежать в интервале от 1 до 1000")]
        public string House { get; set; }

        [Required(ErrorMessage = "Это поле обязательно")]
        [Range(1, 1000, ErrorMessage = "Номер кватиры должен лежать в интервале от 1 до 1000")]
        public string Apartment { get; set; }
    }
}
