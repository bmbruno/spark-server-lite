using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkServerLite.Models
{
    public class Author
    {
        public int ID { get; set; }

        public Guid SSOID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool Active { get; set; }

        public DateTime? CreateDate { get; set; }

        public Author()
        {
            ID = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Active = true;
        }

    }
}
