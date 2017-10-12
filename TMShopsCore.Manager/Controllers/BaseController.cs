using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMShopsCore.Models;

namespace TMShopsCore.Manager.Controllers
{
    public class BaseController : Controller
    {
        //public TMShopsContext db = new TMShopsContext();
        //public CMSBaseController(TMShopsContext context)
        //{
        //    db = context;
        //}
        //protected override void Dispose(bool disposing)
        //{
        //    //if (RoleManager != null) RoleManager.Dispose();
        //    //if (UserManager != null) UserManager.Dispose();
        //    if (db != null) db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}