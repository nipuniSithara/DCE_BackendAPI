using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Models
{
    public class BaseObject
    {
        public bool IsSuccess { get; set; }
        public object Data { get; set; } = new DataObjectDto();
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; } 
    }

    public class DataObjectDto
    { 
        public Customer[] Customer { get; set; } 
        public Product[] Product { get; set; } 
        public Supplier[] Supplier { get; set; }
    }
}
