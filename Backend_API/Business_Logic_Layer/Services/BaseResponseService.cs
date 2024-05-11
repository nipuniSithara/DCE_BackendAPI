using Data_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.Services
{
    public class BaseResponseService
    {
        public BaseObject GetSuccessResponse(string message, object data)
        {
            return new BaseObject() { IsSuccess = true, Message = message, StatusCode = 200, Data = data };
        }

        public BaseObject GetSuccessResponse(object data)
        {
            return new BaseObject() { IsSuccess = true, Message = "Success", StatusCode = 200, Data = data };
        }

        public BaseObject GetErrorResponse(string message, int code)
        {
            return new BaseObject() { IsSuccess = false, Message = message, StatusCode = code };
        }
    }
}
