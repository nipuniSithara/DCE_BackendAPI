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
        public Product Product { get; set; } = null;
        public Supplier Supplier { get; set; } = null;
        public Order Order { get; set; } = null;
    }
}
