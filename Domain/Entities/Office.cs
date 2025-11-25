using Domain.Entities.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Office
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public ContactInfo ContactInfo { get; set; }


        public List<Lawyer> Lawyers { get; set; } // One-to-Many relationship with Lawyer
    }
}
