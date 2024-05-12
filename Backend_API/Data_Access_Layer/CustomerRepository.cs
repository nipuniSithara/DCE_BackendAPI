using Business_Logic_Layer.Repo_Interfaces;
using Business_Logic_Layer.Services;
using Data_Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Data_Access_Layer
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(DatabaseConfiguration configuration, ILogger<CustomerRepository> logger)
        {
            _connectionString = configuration.ConnectionString;
            _logger = logger;
        }
  
        public async Task<BaseObject> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            try
            {
                customers = await GetCustomers();
                if (customers == null || customers.Count == 0)
                {
                    return new BaseResponseService().GetSuccessResponse("There are no customer data!");
                }
                return new BaseResponseService().GetSuccessResponse(customers);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An error occurred in the GetAllCustomers method. : SQL Exception");
                return new BaseResponseService().GetErrorResponse(ex.Message, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the GetAllCustomers method. : Exception");
                return new BaseResponseService().GetErrorResponse(ex.Message, 500);
            }
        }

        private async Task<List<Customer>> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string sql = @"SELECT c.UserId,c.Username,c.Email,c.FirstName,c.LastName, c.CreatedOn as CustomerCreatedOn, c.IsActive as IsCustomerActive, 
                                       s.SupplierId, s.SupplierName, s.CreatedOn as SupplierCreatedOn, s.IsActive as IsSupplierActive,
	                                   p.ProductId, p.ProductName, p.UnitPrice, p.SupplierId, p.CreatedOn as ProductCreatedOn, p.IsActive as IsProductActive
                                FROM Customer c
                                LEFT JOIN [Order] o ON c.UserId = o.OrderBy
                                LEFT JOIN Product p ON o.ProductId = p.ProductId
                                LEFT JOIN Supplier s ON s.SupplierId = p.SupplierId";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var customer = new Customer
                                {
                                    UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                                    UserName = reader.GetString(reader.GetOrdinal("Username")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    CreatedOn = reader.GetDateTime(reader.GetOrdinal("CustomerCreatedOn")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsCustomerActive"))
                                };

                                if (!reader.IsDBNull(reader.GetOrdinal("SupplierId")))
                                {
                                    var supplier = new Supplier
                                    {
                                        SupplierId = (reader.GetGuid(reader.GetOrdinal("SupplierId"))),
                                        SupplierName = reader.GetString(reader.GetOrdinal("SupplierName")),
                                        CreatedOn = reader.GetDateTime(reader.GetOrdinal("SupplierCreatedOn")),
                                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsSupplierActive"))
                                    };

                                    customer.Supplier = supplier;
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("ProductId")))
                                {  
                                    var product = new Product
                                    {
                                        ProductId = reader.GetGuid(reader.GetOrdinal("ProductId")),
                                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                        UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                                        SupplierId = reader.GetGuid(reader.GetOrdinal("SupplierId")),
                                        CreatedOn = reader.GetDateTime(reader.GetOrdinal("ProductCreatedOn")),
                                        IsActive = reader.GetBoolean(reader.GetOrdinal("IsProductActive"))
                                    };
                                    customer.Product = product;
                                }

                                //if (!reader.IsDBNull(reader.GetOrdinal("OrderId")))
                                //{
                                //    var order = new Order
                                //    {
                                //        OrderId = reader.GetGuid(reader.GetOrdinal("OrderId")),
                                //        OrderStatus = reader.GetInt32(reader.GetOrdinal("OrderStatus")),
                                //        OrderType = reader.GetInt32(reader.GetOrdinal("OrderType")),
                                //        OrderBy = reader.GetGuid(reader.GetOrdinal("OrderBy")),
                                //        OrderedOn = reader.GetDateTime(reader.GetOrdinal("OrderedOn")),
                                //        ShippedOn = reader.GetDateTime(reader.GetOrdinal("ShippedOn")),
                                //        IsActive = reader.GetBoolean(reader.GetOrdinal("OrderIsActive"))
                                //    };
                                //    customer.Order = order;
                                //}
                                
                                customers.Add(customer);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An error occurred in the GetCustomers method. : SQL Exception");
                throw; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the GetCustomers method. : Exception");
                throw; 
            }
            return customers;
        }

        public async Task<BaseObject> AddCustomer(Customer customer)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string sql = @"
                                INSERT INTO Customer (UserId, Username, Email, FirstName, LastName, CreatedOn, IsActive)
                                VALUES (@UserId, @Username, @Email, @FirstName, @LastName, @CreatedOn, @IsActive);
                                SELECT SCOPE_IDENTITY();";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        Guid newGuid = Guid.NewGuid();
                        command.Parameters.AddWithValue("@UserId", newGuid.ToString());
                        command.Parameters.AddWithValue("@Username", customer.UserName);
                        command.Parameters.AddWithValue("@Email", customer.Email);
                        command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                        command.Parameters.AddWithValue("@LastName", customer.LastName);
                        command.Parameters.AddWithValue("@CreatedOn", customer.CreatedOn);
                        command.Parameters.AddWithValue("@IsActive", customer.IsActive);

                        // Execute the command and retrieve the inserted UserId
                        var insertedUserId = await command.ExecuteScalarAsync();
                        if (insertedUserId != null)
                        {
                            List<Customer> customers = new List<Customer>();
                            customers = await GetCustomers();
                            return new BaseResponseService().GetSuccessResponse("Customer created successfully! Please refer the following list", customers);
                        }
                        else
                        {
                            return new BaseResponseService().GetErrorResponse("Something went wrong. Please try again!",500);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An error occurred in the AddCustomer method. : SQL Exception");
                return new BaseResponseService().GetErrorResponse(ex.Message, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the AddCustomer method. : Exception");
                return new BaseResponseService().GetErrorResponse(ex.Message, 500);
            }
        }

        public async Task<BaseObject> UpdateCustomer(Customer customer)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string sql = @"
                                UPDATE Customer 
                                SET  
                                    Username = @Username, 
                                    Email = @Email, 
                                    FirstName = @FirstName, 
                                    LastName = @LastName, 
                                    CreatedOn = @CreatedOn, 
                                    IsActive = @IsActive
                                WHERE UserId = @UserId";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Username", customer.UserName);
                        command.Parameters.AddWithValue("@Email", customer.Email);
                        command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                        command.Parameters.AddWithValue("@LastName", customer.LastName);
                        command.Parameters.AddWithValue("@CreatedOn", customer.CreatedOn);
                        command.Parameters.AddWithValue("@IsActive", customer.IsActive);
                        command.Parameters.AddWithValue("@UserId", customer.UserId);

                        // Execute the command and return the number of rows affected
                        int rowCount = 0;
                        rowCount = await command.ExecuteNonQueryAsync();
                        if (rowCount > 0)
                        {
                            List<Customer> customers = new List<Customer>();
                            customers = await GetCustomers();
                            return new BaseResponseService().GetSuccessResponse("Customer updated successfully! Please refer the following list", customers);
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
                _logger.LogError(ex, "An error occurred in the UpdateCustomer method. : SQL Exception");
                return new BaseResponseService().GetErrorResponse(ex.Message, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the UpdateCustomer method. : Exception");
                return new BaseResponseService().GetErrorResponse(ex.Message, 500);
            }
        }

        public async Task<BaseObject> DeleteCustomer(Guid id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    //check is customer exists
                    bool isCustomerExists = IsCustomerExists(id);
                    if (isCustomerExists)
                    {
                        return new BaseResponseService().GetErrorResponse("Customer not existing!", 404);
                    }
                    //Check if this customer id exist in Order table
                    bool isExisting = CheckOrderTable(id);
                    if (isExisting)
                    {
                        return new BaseResponseService().GetErrorResponse("Please remove the orders for this customer first!", 500);
                    }
                    else 
                    {
                        string sql = @"
                                DELETE FROM Customer 
                                WHERE UserId = @UserId";

                        using (var command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@UserId", id);

                            int rowCount = 0;
                            rowCount = await command.ExecuteNonQueryAsync();
                            if (rowCount > 0)
                            {
                                List<Customer> customers = new List<Customer>();
                                customers = await GetCustomers();
                                return new BaseResponseService().GetSuccessResponse("Customer deleted successfully! Please refer the following list", customers);
                            }
                            else
                            {
                                return new BaseResponseService().GetErrorResponse("Something went wrong. Please try again!", 500);
                            }
                        }
                    }
                    
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An error occurred in the DeleteCustomer method. : SQL Exception");
                return new BaseResponseService().GetErrorResponse(ex.Message, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the DeleteCustomer method. : Exception");
                return new BaseResponseService().GetErrorResponse(ex.Message, 500);
            }
        }

        public async Task<BaseObject> ActiveOrdersByCustomers(Guid customerId)
        {
            try
            {
                List<Customer> customers = new List<Customer>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("GetActiveOrdersByCustomer", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CustomerId", customerId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var customer = new Customer
                                {
                                    UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                                    UserName = reader.GetString(reader.GetOrdinal("Username")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    CreatedOn = reader.GetDateTime(reader.GetOrdinal("CustomerCreatedDate")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsCustomerActive"))
                                };

                                var order = new Order
                                {
                                    ProductId = reader.GetGuid(reader.GetOrdinal("ProductId")),
                                    OrderId = (reader.GetGuid(reader.GetOrdinal("OrderId"))),
                                    OrderStatus = reader.GetInt32(reader.GetOrdinal("OrderStatus")),
                                    OrderType = reader.GetInt32(reader.GetOrdinal("OrderType")),
                                    OrderBy = reader.GetGuid(reader.GetOrdinal("OrderBy")),
                                    OrderedOn = reader.GetDateTime(reader.GetOrdinal("OrderedOn")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsOrderActive"))
                                };
                                if (!reader.IsDBNull(reader.GetOrdinal("ShippedOn")))
                                {
                                    order.ShippedOn = reader.GetDateTime(reader.GetOrdinal("ShippedOn"));
                                }

                                var supplier = new Supplier
                                {
                                    SupplierId = (reader.GetGuid(reader.GetOrdinal("SupplierId"))),
                                    SupplierName = reader.GetString(reader.GetOrdinal("SupplierName")),
                                    CreatedOn = reader.GetDateTime(reader.GetOrdinal("SupplierCreatedDate")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsSupplierActive"))
                                };

                                var product = new Product
                                {
                                    ProductId = reader.GetGuid(reader.GetOrdinal("ProductId")),
                                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                    UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                                    SupplierId = reader.GetGuid(reader.GetOrdinal("SupplierId")),
                                    CreatedOn = reader.GetDateTime(reader.GetOrdinal("ProductCreatedDate")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsProductActive"))
                                };
                                customer.Order = order;
                                customer.Supplier = supplier;
                                customer.Product = product;
                                customers.Add(customer);
                            }
                            if(customers == null || customers.Count == 0)
                            {
                                return new BaseResponseService().GetSuccessResponse("This customer does not have active orders!");
                            }
                            return new BaseResponseService().GetSuccessResponse(customers);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An error occurred in the ActiveOrdersByCustomers method. : SQL Exception");
                return new BaseResponseService().GetErrorResponse(ex.Message, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the ActiveOrdersByCustomers method. : Exception");
                return new BaseResponseService().GetErrorResponse(ex.Message, 500);
            }
        }

        private bool CheckOrderTable(Guid id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.OpenAsync();
                    string sql = @"SELECT OrderId
                                FROM [Order] 
                                WHERE OrderBy = @UserId";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", id);

                        int rowCount = 0;
                        rowCount =  command.ExecuteNonQuery();
                        if (rowCount != 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An error occurred in the CheckOrderTable method. : SQL Exception");
                throw new Exception(ex.Message);
            }
        }

        private bool IsCustomerExists(Guid id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.OpenAsync();
                    string sql = @"SELECT UserId
                                FROM Customer 
                                WHERE UserId = @UserId";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", id);

                        int rowCount = 0;
                        rowCount = command.ExecuteNonQuery();
                        if (rowCount != 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An error occurred in the IsCustomerExists method. : SQL Exception");
                throw new Exception(ex.Message);
            }
        }

    }
}
