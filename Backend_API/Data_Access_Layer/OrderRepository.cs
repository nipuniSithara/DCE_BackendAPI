using Business_Logic_Layer.Repo_Interfaces;
using Business_Logic_Layer.Services;
using Data_Models;
using System.Data.SqlClient;

namespace Data_Access_Layer
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(DatabaseConfiguration configuration)
        {
            _connectionString = configuration.ConnectionString;
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
                Console.WriteLine($"SQL Exception: {ex.Message}");
                return new BaseResponseService().GetErrorResponse(ex.Message, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new BaseResponseService().GetErrorResponse(ex.Message, 500);
            }
        }
    }
}