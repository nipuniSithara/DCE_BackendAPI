using Business_Logic_Layer.Service_Interfaces;
using Data_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _bll;
        private readonly ILogger<OrdersController> _logger;
        public CustomersController(ICustomerService customerService, ILogger<OrdersController> logger)
        {
            _bll = customerService;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAllCustomers")]
        public async Task<ActionResult> GetCustomers()
        {
            try
            {
                var result = await _bll.GetAllCustomers();
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    int error = result.StatusCode;
                    switch (error)
                    {
                        case ((int)HttpStatusCode.NotFound):
                            return StatusCode((int)HttpStatusCode.NotFound, result);
                        case ((int)HttpStatusCode.Unauthorized):
                            return StatusCode((int)HttpStatusCode.Unauthorized, result);
                        case (int)HttpStatusCode.BadRequest:
                            return StatusCode((int)HttpStatusCode.BadRequest, result);
                        default:
                            return StatusCode(StatusCodes.Status500InternalServerError, result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred. Please refer the error message");
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetActiveOrdersByCustomer")]
        public async Task<ActionResult> GetActiveOrdersByCustomer(Guid customerId)
        {
            try
            {
                var result = await _bll.ActiveOrdersByCustomers(customerId);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    int error = result.StatusCode;
                    switch (error)
                    {
                        case ((int)HttpStatusCode.NotFound):
                            return StatusCode((int)HttpStatusCode.NotFound, result);
                        case ((int)HttpStatusCode.Unauthorized):
                            return StatusCode((int)HttpStatusCode.Unauthorized, result);
                        case (int)HttpStatusCode.BadRequest:
                            return StatusCode((int)HttpStatusCode.BadRequest, result);
                        default:
                            return StatusCode(StatusCodes.Status500InternalServerError, result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred. Please refer the error message");
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Route("CreateCustomer")]
        public async Task<ActionResult> CreateCustomer(Customer customer)
        {
            try
            {
                var result = await _bll.CreateCustomer(customer);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    int error = result.StatusCode;
                    switch (error)
                    {
                        case ((int)HttpStatusCode.NotFound):
                            return StatusCode((int)HttpStatusCode.NotFound, result);
                        case ((int)HttpStatusCode.Unauthorized):
                            return StatusCode((int)HttpStatusCode.Unauthorized, result);
                        case (int)HttpStatusCode.BadRequest:
                            return StatusCode((int)HttpStatusCode.BadRequest, result);
                        default:
                            return StatusCode(StatusCodes.Status500InternalServerError, result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred. Please refer the error message");
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateCustomer")]
        public async Task<ActionResult> UpdateCustomer(Customer customer)
        {
            try
            {
                var result = await _bll.UpdateCustomer(customer);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    int error = result.StatusCode;
                    switch (error)
                    {
                        case ((int)HttpStatusCode.NotFound):
                            return StatusCode((int)HttpStatusCode.NotFound, result);
                        case ((int)HttpStatusCode.Unauthorized):
                            return StatusCode((int)HttpStatusCode.Unauthorized, result);
                        case (int)HttpStatusCode.BadRequest:
                            return StatusCode((int)HttpStatusCode.BadRequest, result);
                        default:
                            return StatusCode(StatusCodes.Status500InternalServerError, result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred. Please refer the error message");
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteCustomer")]
        public async Task<ActionResult> DeleteCustomer(Guid Id)
        {
            try
            {
                var result = await _bll.DeleteCustomer(Id);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    int error = result.StatusCode;
                    switch (error)
                    {
                        case ((int)HttpStatusCode.NotFound):
                            return StatusCode((int)HttpStatusCode.NotFound, result);
                        case ((int)HttpStatusCode.Unauthorized):
                            return StatusCode((int)HttpStatusCode.Unauthorized, result);
                        case (int)HttpStatusCode.BadRequest:
                            return StatusCode((int)HttpStatusCode.BadRequest, result);
                        default:
                            return StatusCode(StatusCodes.Status500InternalServerError, result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred. Please refer the error message");
                throw new Exception(ex.Message);
            }
        }

    }
}
