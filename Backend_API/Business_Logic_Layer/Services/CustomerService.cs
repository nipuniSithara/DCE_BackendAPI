using Business_Logic_Layer.Repo_Interfaces;
using Business_Logic_Layer.Service_Interfaces;
using Data_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Business_Logic_Layer.Services
{
    public class CustomerService : ICustomerService
    {
        public readonly ICustomerRepository _dal;
        private readonly ILogger<OrderService> _logger;

        public CustomerService(ICustomerRepository customerRepository, ILogger<OrderService> logger)
        {
            _dal = customerRepository;
            _logger = logger;
        }
        public async Task<BaseObject> GetAllCustomers()
        {
            try 
            {
                return await _dal.GetAllCustomers();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred. Please refer the error message");
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseObject> CreateCustomer(Customer customer)
        {
            try
            {
                if(customer == null || string.IsNullOrEmpty(customer.UserName) || string.IsNullOrEmpty(customer.FirstName) || string.IsNullOrEmpty(customer.Email))
                {
                    return new BaseResponseService().GetErrorResponse("Validation Error: Please fill mandotary fields",400);
                }
                return await _dal.AddCustomer(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred. Please refer the error message");
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseObject> UpdateCustomer(Customer customer)
        {
            try
            {
                if (customer == null || Guid.Empty == customer.UserId || string.IsNullOrEmpty(customer.UserName) || string.IsNullOrEmpty(customer.FirstName) || string.IsNullOrEmpty(customer.Email))
                {
                    return new BaseResponseService().GetErrorResponse("Validation Error: Please fill mandatary fields", 400);
                }
                return await _dal.UpdateCustomer(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred. Please refer the error message");
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseObject> DeleteCustomer(Guid Id)
        {
            try
            {
                if(Guid.Empty == Id)
                {
                    return new BaseResponseService().GetErrorResponse("Validation Error: Customer Id is mandatary", 400);
                }
                return await _dal.DeleteCustomer(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred. Please refer the error message");
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseObject> ActiveOrdersByCustomers(Guid customerId)
        {
            try
            {
                if (Guid.Empty == customerId)
                {
                    return new BaseResponseService().GetErrorResponse("Validation Error: Customer Id is mandatary", 400);
                }
                return await _dal.ActiveOrdersByCustomers(customerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred. Please refer the error message");
                throw new Exception(ex.Message);
            }
        }
    }
}
