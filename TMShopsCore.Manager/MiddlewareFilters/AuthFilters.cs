using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.Linq;

namespace TMShopsCore.MiddlewareFilters
{
    public class Auth : ActionFilterAttribute
    {
        public string Role { get; set; }
        public Auth() { }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // do something before the action executed
            var result = new RedirectToRouteResult(
                new Microsoft.AspNetCore.Routing.RouteValueDictionary {
                    { "controller", "Auth" },
                    { "action", "Index" },
                    { "continue", TM.Helper.Url.Continue } });
            if (Common.Auth.isAuth)
            {
                ////var roles = Role.Split(',');
                //if (Role != null && !Role.Split(',').Contains(Areas.CMS.Common.Auth.GetAuth().Roles))
                //{
                //    //context.Controller.TempData.Add("MsgDanger", "Bạn không có quyền truy cập. Vui lòng liên hệ với admin!");
                //    context.Result = result;
                //}
            }
            else
            {
                context.Result = result;
            }
            base.OnActionExecuting(context);
        }
    }
    public class AuthAsync : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            // do something before the action executes
            await next();
            // do something after the action executes
        }
    }
}