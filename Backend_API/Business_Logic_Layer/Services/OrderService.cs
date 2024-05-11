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
    public class OrderService : IOrderService
    {
        public readonly IOrderRepository _dal;

        public OrderService(IOrderRepository orderRepository)
        {
            _dal = orderRepository;
        }
        public async Task<BaseObject> DeleteOrder(Guid Id)
        {
            try
            {
                return await _dal.DeleteOrder(Id);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
