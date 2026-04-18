using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Laboratory_02
{
    [Serializable]
    public class Work
    {
        public string Company { get; set; }
        public string Position { get; set; }
        public int Experience { get; set; }
    }
}
