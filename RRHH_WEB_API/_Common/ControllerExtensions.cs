using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace RRHH_WEB_API._Common
{
    public static class ControllerExtensions
    {
        public static IActionResult ActionResultFrom<TData>(this ControllerBase controller, Response<TData> response)
        {
            if (response.AnswerType == AnswerTypeEnum.Warning)
                return controller.StatusCode(StatusCodes.Status400BadRequest, new { Message = response.Message });

            if (response.AnswerType == AnswerTypeEnum.Error)
                return controller.StatusCode(StatusCodes.Status500InternalServerError, new { Message = response.Message });

            return controller.StatusCode(StatusCodes.Status200OK, response.Data);
        }

        public static IActionResult GetClaim<T>(this ControllerBase controller, string claimType, out T responseValue)
        {
            Claim claim = controller.User.Claims.FirstOrDefault(x => x.Type == claimType);
            if (claim == null)
            {
                responseValue = (T)Convert.ChangeType(0, typeof(T));
                return controller.StatusCode(StatusCodes.Status500InternalServerError, new { Message = "ERROR GETUSER" });
            }

            responseValue = (T)Convert.ChangeType(claim.Value, typeof(T));
            return controller.StatusCode(StatusCodes.Status200OK, (T)Convert.ChangeType(1, typeof(T)));
        }

        public static T GetClaim<T>(this ControllerBase controller, string claimType)
        {
            T responseValue;
            Claim claim = controller.User.Claims.FirstOrDefault(x => x.Type == claimType);
            if (claim == null)
            {
                responseValue = (T)Convert.ChangeType(0, typeof(T));
                return responseValue;
            }

            responseValue = (T)Convert.ChangeType(claim.Value, typeof(T));
            return responseValue;
        }
    }
}
