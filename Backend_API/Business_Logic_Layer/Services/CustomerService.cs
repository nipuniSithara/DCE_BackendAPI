using Business_Logic_Layer.Repo_Interfaces;
using Business_Logic_Layer.Service_Interfaces;
using Data_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Services
{
    public class CustomerService : ICustomerService
    {
        public readonly ICustomerRepository _dal;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _dal = customerRepository;
        }
        public async Task<BaseObject> GetAllCustomers()
        {
            try 
            {
                return await _dal.GetAllCustomers();
            }
            catch 
            {
                throw new Exception();
            }
        }

        public async Task<BaseObject> CreateCustomer(Customer customer)
        {
            try
            {
                return await _dal.AddCustomer(customer);
            }
            catch
            {
                throw new Exception();
            }
        }

        public async Task<BaseObject> UpdateCustomer(Customer customer)
        {
            try
            {
                return await _dal.UpdateCustomer(customer);
            }
            catch
            {
                throw new Exception();
            }
        }

        public async Task<BaseObject> DeleteCustomer(Guid Id)
        {
            try
            {
                return await _dal.DeleteCustomer(Id);
            }
            catch
            {
                throw new Exception();
            }
        }

        public async Task<BaseObject> ActiveOrdersByCustomers(Guid customerId)
        {
            try
            {
                return await _dal.ActiveOrdersByCustomers(customerId);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
