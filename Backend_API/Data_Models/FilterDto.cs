using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Models
{
    public class FilterDto
    {
        public Guid? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public Guid? OrderId { get; set; }
        public string? OrderDate { get; set; }
        public string? Email { get; set; }
    }
}
