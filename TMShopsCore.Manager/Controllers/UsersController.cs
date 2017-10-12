using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMShopsCore.Models;

namespace TMShopsCore.Manager.Controllers
{
    [ServiceFilter(typeof(MiddlewareFilters.Auth))]
    public class UsersController : BaseController
    {
        // GET: CMS/Users
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PartialCreate()
        {
            return PartialView("PartialCreate");
        }
        public IActionResult PartialEdit()
        {
            return PartialView("PartialEdit");
        }
        public IActionResult PartialProfile()
        {
            return PartialView("PartialProfile");
        }
        public IActionResult PartialChangePassword()
        {
            return PartialView("PartialChangePassword");
        }
        public IActionResult PartialUserSetting()
        {
            return PartialView("PartialUserSetting");
        }
    }
    //API
    [ServiceFilter(typeof(MiddlewareFilters.Auth))]
    [Produces("application/json")]
    [Route("api/UsersAPI")]
    public class UsersAPIController : Controller
    {
        private readonly TMShopsContext _context;
        private const string PathInUpload = @"Users\";
        public UsersAPIController(TMShopsContext context) { _context = context; }

        // GET: api/UsersAPI
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync(string sort, string order, string search, int offset = 0, int limit = 10, int flag = 1)
        {
            //Get data for Status
            var d = _context.Users.Where(m => m.Flag == flag);
            //Get data for Search
            if (!string.IsNullOrEmpty(search))
                d = d.Where(m => m.Username.Contains(search) || m.FullName.Contains(search) || m.Address.Contains(search));
            //Get total item
            var total = d.Count();
            //Sort And Orders
            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.ToLower() == "fullname" && order.ToLower() == "asc")
                    d = d.OrderBy(m => m.FullName);
                else if (sort.ToLower() == "fullname" && order.ToLower() == "desc")
                    d = d.OrderByDescending(m => m.FullName);
                else if (sort.ToLower() == "username" && order.ToLower() == "asc")
                    d = d.OrderBy(m => m.Username);
                else if (sort.ToLower() == "username" && order.ToLower() == "desc")
                    d = d.OrderByDescending(m => m.Username);
                else
                    d = d.OrderBy(m => m.Orders).ThenByDescending(m => m.CreatedAt);
            }
            else
                d = d.OrderBy(m => m.Orders).ThenByDescending(m => m.CreatedAt);
            //Page Site
            var rs = await d.Skip(offset).Take(limit).ToListAsync();
            //Return Json Data
            return Json(new { total = total, rows = rs });
        }

        // GET: api/UsersAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers(Guid? id)
        {
            if (id == null)
                return Json(new { danger = TM.Common.Error.IDNull });
            if (!ModelState.IsValid)
                return Json(new { danger = TM.Common.Error.Model });

            var users = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            //var users = _context.Users.Where(m => m.Id == id).Select(m => new
            //{
            //    obj = m,
            //    cUser = _context.Users.SingleOrDefault(d => d.Id.ToString() == m.CreatedBy).FullName,
            //    uUser = _context.Users.SingleOrDefault(d => d.Id.ToString() == m.UpdatedBy).FullName,
            //}).SingleOrDefaultAsync();

            if (users == null)
                return Json(new { danger = TM.Common.Error.Null });

            return Json(new { success = TM.Common.Success.Get, data = users }); ;
        }

        // POST: api/UsersAPI
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> PostUsers(Users users)
        {
            if (!ModelState.IsValid)
                return Json(new { danger = TM.Common.Error.Model });
            if (UsersNameExists(users.Username))
                return Json(new { danger = TM.Common.Error.Exist });
            try
            {
                users.Id = Guid.NewGuid();
                users.Salt = Guid.NewGuid().ToString();
                users.Password = TM.Encrypt.MD5.CryptoMD5TM(users.Password + users.Salt);
                users.Roles = "";
                users.CreatedBy = Common.Auth.getUserAction();
                users.CreatedAt = DateTime.Now;
                users.UpdatedBy = Common.Auth.getUserAction();
                users.UpdatedAt = DateTime.Now;

                //Create Directory Upload
                var upload = new TM.Helper.Upload(Request.Form.Files, TM.Common.Directories.Uploads + PathInUpload);
                var file = upload.ImagesName();
                if (file.Count > 0)
                    users.Images = TM.Common.Directories.Uploads + PathInUpload + file[0];

                _context.Users.Add(users);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UsersExistsID(users.Id))
                    return Json(new { danger = TM.Common.Error.IDExist });
                else
                    return Json(new { danger = TM.Common.Error.DB });
            }
            return Json(new { success = TM.Common.Success.Create });
        }

        // PUT: api/UsersAPI/5
        [HttpPut("{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> PutUsers(Guid id, Users users)//[FromBody]
        {
            if (!ModelState.IsValid)
                return Json(new { danger = TM.Common.Error.Model });

            if (id != users.Id)
                return Json(new { danger = TM.Common.Error.Update });

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
                entry.Property(m => m.Orders).IsModified = true;
                entry.Property(m => m.Roles).IsModified = true;
                entry.Property(m => m.Flag).IsModified = true;
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
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { danger = TM.Common.Error.DB });
            }

            return Json(new { success = TM.Common.Success.Update });
        }

        //GET: api/UsersAPI/Profile
        [HttpGet("Profile")]
        public IActionResult GetUsersProfile()
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
        public async Task<IActionResult> PutUsersProfile(Users users)//[FromBody]
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
        public async Task<IActionResult> PutUsersChangePassword(Users users)//[FromBody]
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
        public async Task<IActionResult> PutUserSetting(Users users)//[FromBody]
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

        //Reset Password
        [HttpPost("resetPassword"), ValidateAntiForgeryToken]
        public async Task<IActionResult> resetPassword(Guid id)
        {
            if (!ModelState.IsValid)
                return Json(new { danger = TM.Common.Error.Model });
            var newPassword = Guid.NewGuid().ToString().Split('-')[0];
            try
            {
                var users = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
                users.Password = TM.Encrypt.MD5.CryptoMD5TM(newPassword + users.Salt);
                _context.Entry(users);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Json(new { danger = TM.Common.Error.DB });
            }
            return Json(new { success = "Users.msgNewPassword", data = newPassword });
        }

        // DELETE: api/UsersAPI/5
        [HttpDelete, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUsers(string id)
        {
            //if (!ModelState.IsValid)
            //    return Json(new { danger = TM.Common.Error.Model });
            var _id = id.Trim(',').Split(',');
            foreach (var item in _id)
            {
                var tmp = Guid.Parse(item);
                var users = await _context.Users.SingleOrDefaultAsync(m => m.Id == tmp);

                if (users == null) return Json(new { danger = TM.Common.Error.Null });

                _context.Users.Remove(users);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = TM.Common.Success.Delete });
        }
        //Update Flag
        [HttpPost("Remove"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(string id)
        {
            if (!ModelState.IsValid)
                return Json(new { danger = TM.Common.Error.Model });
            var _id = id.Trim(',').Split(',');
            var flag = true;
            try
            {
                foreach (var item in _id)
                {
                    var tmp = Guid.Parse(item);
                    var users = await _context.Users.SingleOrDefaultAsync(m => m.Id == tmp);

                    if (users == null) return Json(new { danger = TM.Common.Error.Null });
                    if (users.Flag == 0)
                    {
                        users.Flag = 1;
                        flag = false;
                    }
                    else
                        users.Flag = 0;
                    _context.Entry(users);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Json(new { danger = flag ? TM.Common.Error.Delete : TM.Common.Error.Recover });
            }
            return Json(new { success = flag ? TM.Common.Success.Delete : TM.Common.Success.Recover });
        }
        private bool UsersExistsID(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        //Check Exists UsersName Create
        [HttpGet("UsersNameExistsCheck")]
        public JsonResult UsersNameExistsCheck(string Username)
        {
            return Json(!UsersNameExists(Username));
        }
        private bool UsersNameExists(string username)
        {
            return _context.Users.Any(m => m.Username.ToLower() == username.ToLower());
        }
    }
    //public interface IRepository
    //{
    //    void Update<T>(T obj, params System.Linq.Expressions.Expression<Func<T, object>>[] propertiesToUpdate) where T : class;
    //}

    //public class Repository : DbContext, IRepository
    //{
    //    public void Update<T>(T obj, params System.Linq.Expressions.Expression<Func<T, object>>[] propertiesToUpdate) where T : class
    //    {
    //        Set<T>().Attach(obj);
    //        propertiesToUpdate.ToList().ForEach(p => Entry(obj).Property(p).IsModified = true);
    //        SaveChanges();
    //    }
    //}
}
