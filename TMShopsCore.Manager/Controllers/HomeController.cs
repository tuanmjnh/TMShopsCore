using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TM.Helper;

namespace TMShopsCore.Manager.Controllers
{
    [ServiceFilter(typeof(MiddlewareFilters.Auth))]
    public class HomeController : BaseController
    {
        private readonly Models.TMShopsContext _context;
        public HomeController(Models.TMShopsContext context) { _context = context; }
        public ActionResult Index(string id)
        {
            try
            {
                var setting = _context.Settings.FirstOrDefault();
                var a = TMAppContext.Http.Request;
                ViewBag.id = id;
            }
            catch (Exception)
            {
                this.danger(Language.Globals("msgConnectionError"));
            }
            return View();
        }
    }
}