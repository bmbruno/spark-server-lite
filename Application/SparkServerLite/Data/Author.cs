using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkServerLite.Data
{
    public class Author
    {
        public int ID { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }
        
        [DefaultValue(true)]
        public bool Active { get; set; }

        public DateTime CreateDate { get; set; }

        public Guid SSOID { get; set; }
    }
}
