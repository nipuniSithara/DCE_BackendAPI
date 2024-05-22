using Data_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Service_Interfaces
{
    public interface IOrderService
    {
        Task<BaseObject> DeleteOrder(Guid orderId);
        Task<IEnumerable<Customer>> Filter(FilterDto filter);
    }
}
