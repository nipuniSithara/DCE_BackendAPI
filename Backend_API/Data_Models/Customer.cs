using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Models
{
    public class Customer
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }

        /// <summary>
        /// Navigation 
        /// </summary>
        public virtual Product? Product { get; set; } 
        public virtual Supplier? Supplier { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
        
    }
}
