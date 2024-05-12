using Data_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Repo_Interfaces
{
    public interface ICustomerRepository
    {
        Task<BaseObject> GetAllCustomers();
        Task<BaseObject> AddCustomer(Customer customer);
        Task<BaseObject> UpdateCustomer(Customer customer);
        Task<BaseObject> DeleteCustomer(Guid Id);
        Task<BaseObject> ActiveOrdersByCustomers(Guid customerId);
    }
}
