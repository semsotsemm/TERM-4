using System;
using System.Collections.Generic;
using System.Text;

namespace Laboratory_02
{
    [Serializable]
    public class Address
    {
        public string City { get; set; }
        public string Index { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Apartment { get; set; }
    }
}
