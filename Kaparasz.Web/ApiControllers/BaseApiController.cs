using System;
using Microsoft.AspNetCore.Mvc;

namespace Kaparasz.Web.ApiControllers
{
    [Produces("application/json")]
    public abstract class BaseApiController: Controller
    {
        protected JsonResult JsonSuccess()
        {
            return Json(new { success = true });
        }

        protected JsonResult JsonSuccess<T>(T data)
        {
            return Json(new { success = true, data });
        }

        protected JsonResult JsonError(string error)
        {
            return Json(new { success = false, error });
        }
    }
}
