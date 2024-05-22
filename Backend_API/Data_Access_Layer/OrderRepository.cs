using Business_Logic_Layer.Repo_Interfaces;
using Business_Logic_Layer.Services;
using Data_Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

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

    

        public async Task<IEnumerable<Customer>> Filter(FilterDto filter)
        {
            try 
            {
                var customersWithOrders = new List<Customer>();
                var customDictionary = new Dictionary<Guid, Customer>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    var sql = @"
                    SELECT  c.UserId, c.Username, c.Email, c.FirstName, c.LastName, c.CreatedOn AS CustomerCreatedDate, c.IsActive AS IsCustomerActive,
                            o.OrderId, o.ProductId, o.OrderStatus, o.OrderType, o.OrderedOn, o.ShippedOn, o.IsActive AS IsOrderActive
                    FROM [Order] o INNER JOIN Customer c ON c.UserId = o.OrderBy
                    WHERE (@CustomerId IS NULL OR c.UserId = @CustomerId)
                      AND (@CustomerName IS NULL OR c.Username LIKE '%' + @CustomerName + '%')
                      AND (@OrderId IS NULL OR o.OrderId = @OrderId)
                      AND (@OrderedOn IS NULL OR CONVERT(date,O.OrderedOn) = @OrderedOn)
                      AND (@Email IS NULL OR c.Email = @Email)";

                    connection.Open();
                    using (var command = new SqlCommand(sql,connection))
                    {
                        
                        command.Parameters.Add(new SqlParameter("@CustomerId", SqlDbType.UniqueIdentifier) { Value = (object)filter.CustomerId ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@CustomerName", SqlDbType.NVarChar, 255) { Value = (object)filter.CustomerName ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@OrderId", SqlDbType.UniqueIdentifier) { Value = (object)filter.OrderId ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@OrderedOn", SqlDbType.Date) { Value = (object)filter.OrderDate ?? DBNull.Value });
                        command.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar) { Value = (object)filter.Email ?? DBNull.Value });
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var customerId = reader.GetGuid(reader.GetOrdinal("UserId"));
                                if (!customDictionary.TryGetValue(customerId, out var customer))
                                {
                                    customer = new Customer()
                                    {
                                        UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                                        UserName = reader.GetString(reader.GetOrdinal("Username")),
                                        Email = reader.GetString(reader.GetOrdinal("Email")),
                                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                        CreatedOn = reader.GetDateTime(reader.GetOrdinal("CustomerCreatedDate")),
                                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsCustomerActive"))
                                    };
                                    customDictionary.Add(customerId, customer);
                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("OrderId")))
                                {
                                    var order = new Order()
                                    {
                                        OrderId = reader.GetGuid(reader.GetOrdinal("OrderId")),
                                        OrderedOn = reader.GetDateTime(reader.GetOrdinal("OrderedOn")),
                                        ProductId = reader.GetGuid(reader.GetOrdinal("ProductId")),
                                        OrderBy = reader.GetGuid(reader.GetOrdinal("UserId")),
                                        OrderStatus = reader.GetInt32(reader.GetOrdinal("OrderStatus")),
                                        OrderType = reader.GetInt32(reader.GetOrdinal("OrderType")),
                                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsOrderActive")),
                                        
                                    };
                                    if (!reader.IsDBNull(reader.GetOrdinal("ShippedOn")))
                                    {
                                        order.ShippedOn = reader.GetDateTime(reader.GetOrdinal("ShippedOn"));
                                    }
                                    customer.Orders.Add(order);
                                }
                                
                            }
                        }
                    }
                }
                customersWithOrders.AddRange(customDictionary.Values);
                _logger.LogInformation("Sussess");
                return customersWithOrders;
            }
            catch(SqlException ex)
            {
                _logger.LogError("SQL Exception : " + ex.Message);
                throw new Exception(ex.Message);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Exception: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }

}