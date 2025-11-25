using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Employee : UserApp
    {
        public string JobTitle { get; set; }          
        public DateTime HireDate { get; set; }        
        public decimal Salary { get; set; }      
        
        public int OfficeId { get; set; } // Foreign key property             
        public Office Office { get; set; } // Navigation property

        public bool IsActive { get; set; }            
        public string Notes { get; set; }          
        
    }

}
