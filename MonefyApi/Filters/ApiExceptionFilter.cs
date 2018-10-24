using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Monefy.Api.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MonefyApi.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if(context.Exception is NotFoundException)
            {
                //handle exception
                var ex = context.Exception as NotFoundException;
                context.Exception = null;

                context.Result = new JsonResult(ex.Message);
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.NotFound;
            }

            else if (context.Exception is Monefy.Api.Common.Exceptions.ForbiddenException)
            {
                context.Result = new JsonResult(context.Exception.Message);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }



            base.OnException(context);
        }
        
    }
}
