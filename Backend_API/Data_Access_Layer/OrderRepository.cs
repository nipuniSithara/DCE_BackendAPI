using Business_Logic_Layer.Repo_Interfaces;
using Business_Logic_Layer.Services;
using Data_Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Data_Access_Layer
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(DatabaseConfiguration configuration, ILogger<OrderRepository> logger)
        {
            _connectionString = configuration.ConnectionString;
            _logger = logger;
        }

        public async Task<BaseObject> DeleteOrder(Guid orderId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string sql = @"
                            DELETE FROM [Order] 
                            WHERE OrderId = @OrderId";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderId);

                        int rowCount = 0;
                        rowCount = await command.ExecuteNonQueryAsync();
                        if (rowCount > 0)
                        {
                            return new BaseResponseService().GetSuccessResponse("Order deleted successfully!");
                        }
                        else
                        {
                            return new BaseResponseService().GetErrorResponse("Something went wrong. Please try again!", 500);
                        }
                    }
                    

                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An error occurred in the DeleteOrder method. : SQL Exception");
                return new BaseResponseService().GetErrorResponse(ex.Message, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the DeleteOrder method. : Exception");
                return new BaseResponseService().GetErrorResponse(ex.Message, 500);
            }
        }
    }
}