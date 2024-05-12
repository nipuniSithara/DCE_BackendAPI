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
    public class OrderService : IOrderService
    {
        public readonly IOrderRepository _dal;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
        {
            _dal = orderRepository;
            _logger = logger;
        }
        public async Task<BaseObject> DeleteOrder(Guid Id)
        {
            try
            {
                return await _dal.DeleteOrder(Id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred. Please refer the error message");
                throw new Exception(ex.Message);
            }
        }
    }
}
