using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Laboratory_02
{
    public class Filters
    {
        [Required(ErrorMessage = "Поле \"Фильтр\" обязательно")]
        public string filter { get; set; }
        public string criterion { get; set; }
        public bool isRegex { get; set; }
    }
}
