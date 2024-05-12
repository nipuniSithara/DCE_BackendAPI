using Business_Logic_Layer.Service_Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.Extensions.Logging;

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
    }
}
