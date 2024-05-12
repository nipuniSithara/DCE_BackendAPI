using Data_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Service_Interfaces
{
    public interface ICustomerService
    {
        Task<BaseObject> GetAllCustomers();
        Task<BaseObject> CreateCustomer(Customer customer);
        Task<BaseObject> UpdateCustomer(Customer customer);
        Task<BaseObject> DeleteCustomer(Guid Id);
        Task<BaseObject> ActiveOrdersByCustomers(Guid customerId);
    }
}
