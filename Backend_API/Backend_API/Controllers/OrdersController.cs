using Business_Logic_Layer.Service_Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.Extensions.Logging;
using Data_Models;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _bll;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _bll = orderService;
            _logger = logger;
        }

        [HttpDelete]
        [Route("DeleteOrder")]
        public async Task<ActionResult> DeleteOrder(Guid Id)
        {
            try
            {
                var result = await _bll.DeleteOrder(Id);
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
        [Route("filter")]
        public async Task<IActionResult> FilterOrders([FromQuery] FilterDto criteria)
        {
            try
            {
                //check if all the input values are empty
                if (criteria.CustomerId == null && criteria.OrderId == null && string.IsNullOrWhiteSpace(criteria.CustomerName) && string.IsNullOrWhiteSpace(criteria.OrderDate) && string.IsNullOrWhiteSpace(criteria.Email))
                { 
                    return BadRequest("Enter a valid criteria");
                }

                if(!string.IsNullOrWhiteSpace(criteria.Email) && !IsValidEmail(criteria.Email)) //email validation
                {
                    return BadRequest("Enter valid email address");
                }
                
                if (!string.IsNullOrWhiteSpace(criteria.OrderDate)) //date validation
                {
                    DateTime orderDate;
                    string[] format = { "yyyy-MM-dd", "MM/dd/yyyy" };
                    if (!DateTime.TryParseExact((criteria.OrderDate),format, CultureInfo.InvariantCulture, DateTimeStyles.None, out orderDate))
                    {
                        return BadRequest("Please enter date in one of these formats : yyyy-MM-dd, MM/dd/yyyy");
                    }
                }
                
                var result = await _bll.Filter(criteria);
                if (result.Count() > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("No data found");
                }
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }


        static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
    }
}
