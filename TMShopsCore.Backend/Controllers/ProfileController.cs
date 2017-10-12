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
    public class ProfileController : BaseController
    {
        public IActionResult Index()
        {
            return PartialView("PartialProfile");
        }
        public IActionResult ChangePassword()
        {
            return PartialView("PartialChangePassword");
        }
        public IActionResult UserSetting()
        {
            return PartialView("PartialUserSetting");
        }
    }
    //API
    [Produces("application/json")]
    [Route("api/ProfileAPI")]
    public class ProfileAPIController : Controller
    {
        private readonly TMShopsContext _context;
        private const string PathInUpload = @"Users\";
        public ProfileAPIController(TMShopsContext context) { _context = context; }

        [HttpGet("Profile")]
        public IActionResult Select()
        {
            try
            {
                //var users = await _context.Users.SingleOrDefaultAsync(m => m.Id == Common.Auth.AuthUser.Id);
                //if (users == null) return Json(new { danger = TM.Common.Error.Null });
                return Json(new { success = TM.Common.Success.Get, data = Common.Auth.AuthUser });
            }
            catch (Exception)
            {
                return Json(new { danger = TM.Common.Error.Exist });
            }
        }
        // PUT: api/UsersAPI/Profile
        [HttpPut("Profile"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Users users)//[FromBody]
        {
            if (!ModelState.IsValid)
                return Json(new { danger = TM.Common.Error.Model });
            if (users.Id != Common.Auth.AuthUser.Id && await _context.Users.SingleOrDefaultAsync(m => m.Id == Common.Auth.AuthUser.Id) == null)
                return Json(new { success = TM.Common.Success.Update });
            try
            {
                users.UpdatedBy = Common.Auth.getUserAction();
                users.UpdatedAt = DateTime.Now;
                _context.Users.Attach(users);
                var entry = _context.Entry(users);
                entry.Property(m => m.FullName).IsModified = true;
                entry.Property(m => m.Mobile).IsModified = true;
                entry.Property(m => m.Email).IsModified = true;
                entry.Property(m => m.Address).IsModified = true;
                entry.Property(m => m.Details).IsModified = true;
                entry.Property(m => m.Images).IsModified = true;
                entry.Property(m => m.UpdatedBy).IsModified = true;
                entry.Property(m => m.UpdatedAt).IsModified = true;
                //Create Directory Upload
                var upload = new TM.Helper.Upload(Request.Form.Files, TM.Common.Directories.Uploads + PathInUpload);
                var file = upload.ImagesName();
                if (file.Count > 0)
                {
                    users.Images = TM.Common.Directories.Uploads + PathInUpload + file[0];
                    entry.Property(m => m.Images).IsModified = true;
                }
                await _context.SaveChangesAsync();
                //Update Session User
                Common.Auth.SetAuth(await _context.Users.SingleOrDefaultAsync(m => m.Id == Common.Auth.AuthUser.Id));
            }
            catch (Exception)
            {
                return Json(new { danger = TM.Common.Error.DB });
            }
            return Json(new { success = TM.Common.Success.Update });
        }

        // PUT: api/UsersAPI/ChangePassword
        [HttpPut("ChangePassword"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(Users users)//[FromBody]
        {
            if (TM.Encrypt.MD5.CryptoMD5TM(users.Salt + Common.Auth.AuthUser.Salt) != Common.Auth.AuthUser.Password)
                return Json(new { success = "Users.msgErrorPassword" });
            try
            {
                users.Id = Common.Auth.AuthUser.Id;
                users.Password = TM.Encrypt.MD5.CryptoMD5TM(users.Password + Common.Auth.AuthUser.Salt);
                users.UpdatedBy = Common.Auth.getUserAction();
                users.UpdatedAt = DateTime.Now;
                _context.Users.Attach(users);
                var entry = _context.Entry(users);
                entry.Property(m => m.Password).IsModified = true;
                entry.Property(m => m.UpdatedBy).IsModified = true;
                entry.Property(m => m.UpdatedAt).IsModified = true;
                await _context.SaveChangesAsync();
                //Update Session User
                Common.Auth.SetAuth(await _context.Users.SingleOrDefaultAsync(m => m.Id == Common.Auth.AuthUser.Id));
            }
            catch (Exception)
            {
                return Json(new { danger = TM.Common.Error.DB });
            }
            return Json(new { success = TM.Common.Success.Update });
        }

        // PUT: api/UsersAPI/UserSetting
        [HttpPut("UserSetting"), ValidateAntiForgeryToken]
        public async Task<IActionResult> UserSetting(Users users)//[FromBody]
        {
            if (TM.Encrypt.MD5.CryptoMD5TM(users.Salt + Common.Auth.AuthUser.Salt) != Common.Auth.AuthUser.Password)
                return Json(new { success = "Users.msgErrorPassword" });
            try
            {
                users.Id = Common.Auth.AuthUser.Id;
                users.Password = TM.Encrypt.MD5.CryptoMD5TM(users.Password + Common.Auth.AuthUser.Salt);
                users.UpdatedBy = Common.Auth.getUserAction();
                users.UpdatedAt = DateTime.Now;
                _context.Users.Attach(users);
                var entry = _context.Entry(users);
                entry.Property(m => m.Password).IsModified = true;
                entry.Property(m => m.UpdatedBy).IsModified = true;
                entry.Property(m => m.UpdatedAt).IsModified = true;
                await _context.SaveChangesAsync();
                //Update Session User
                Common.Auth.SetAuth(await _context.Users.SingleOrDefaultAsync(m => m.Id == Common.Auth.AuthUser.Id));
            }
            catch (Exception)
            {
                return Json(new { danger = TM.Common.Error.DB });
            }
            return Json(new { success = TM.Common.Success.Update });
        }

    }
}
