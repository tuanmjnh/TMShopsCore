using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TMShopsCore.Models;

namespace TMShopsCore.Manager.Controllers
{
    [ServiceFilter(typeof(MiddlewareFilters.Auth))]
    public class ProfileController : BaseController
    {
        // GET: CMS/Profile
        public IActionResult Index()
        {
            try
            {
                //await _context.Users.SingleOrDefaultAsync(m => m.Id == Common.Auth.AuthUser.Id)
                return View();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        public IActionResult ChangePassword()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
    //API
    [ServiceFilter(typeof(MiddlewareFilters.Auth))]
    [Produces("application/json")]
    [Route("api/ProfileAPI")]
    public class ProfileAPIController : Controller
    {
        private readonly TMShopsContext _context;
        public ProfileAPIController(TMShopsContext context) { _context = context; }

        // GET: api/ProfileAPI
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _context.Users.SingleOrDefaultAsync(m => m.Id == Common.Auth.AuthUser.Id);
                if (users == null) return Json(new { danger = TM.Common.Error.Null });
                return Json(new { success = TM.Common.Success.Get, data = users });
            }
            catch (Exception)
            {
                return Json(new { danger = TM.Common.Error.Exist });
            }
        }

        // PUT: api/ProfileAPI/5
        [HttpPut, ValidateAntiForgeryToken]
        public async Task<IActionResult> PutUsers(Users _users)//[FromBody]
        {
            if (!ModelState.IsValid)
                return Json(new { danger = TM.Common.Error.Model });

            try
            {
                _context.Users.Attach(_users);
                var entry = _context.Entry(_users);
                entry.Property(m => m.FullName).IsModified = true;
                entry.Property(m => m.Mobile).IsModified = true;
                entry.Property(m => m.Email).IsModified = true;
                entry.Property(m => m.Address).IsModified = true;
                entry.Property(m => m.Details).IsModified = true;
                entry.Property(m => m.Images).IsModified = true;
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Json(new { danger = TM.Common.Error.DB });
            }
            return Json(new { success = TM.Common.Success.Update });
        }
    }
}
