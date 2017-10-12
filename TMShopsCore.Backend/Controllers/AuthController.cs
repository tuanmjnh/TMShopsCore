using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TM.Helper;

namespace TMShopsCore.Manager.Controllers
{
    public class AuthController : BaseController
    {
        private readonly Models.TMShopsContext db;
        public AuthController(Models.TMShopsContext _db) { db = _db; }
        public IActionResult Index()
        {
            if (Common.Auth.isAuth)
                return Redirect(Url.Action("Index", "Home"));
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string logging)
        {
            try
            {
                //var collection = HttpContext.Request.ReadFormAsync();
                string username = Request.Form["username"];
                string password = Request.Form["password"];

                //AuthStatic
                var AuthStatic = Common.Auth.isAuthStatic(username, password);
                if (AuthStatic != null)
                {
                    Common.Auth.SetAuth(AuthStatic);
                    return Redirect(TM.Helper.Url.RedirectContinue);
                }
                //AuthDB
                var user = await db.Users.SingleOrDefaultAsync(u => u.Username == username);

                //Account not Exist
                if (user == null)
                {
                    this.danger(Language.Currents("msgAuthError"));
                    return RedirectToAction("Index");
                }

                //Password wrong
                password = TM.Encrypt.MD5.CryptoMD5TM(password + user.Salt);
                user = await db.Users.SingleOrDefaultAsync(u => u.Username == username && u.Password == password);
                if (user == null)
                {
                    this.danger(Language.Currents("msgAuthError"));
                    return RedirectToAction("Index");
                }

                //Account is locked
                if (user.Flag < 1)
                {
                    this.danger(Language.Currents("msgLocked"));
                    return RedirectToAction("Index");
                }

                //Set Auth Account
                Common.Auth.SetAuth(user, AuthRoles(user.Roles), AuthAllowRoles());
                return Redirect(TM.Helper.Url.RedirectContinue);
            }
            catch (Exception ex)
            {
                this.danger(Language.Currents("msgAuthError"));
            }
            return RedirectToAction("Index");
        }
        //[Filters.AuthVinaphone]
        [HttpGet]
        public IActionResult Logout()
        {
            //VinaphoneCommon.Auth.logout();
            //return Redirect(System.Web.HttpContext.Current.Request.UrlReferrer.ToString());//System.Web.HttpContext.Current.Request.UrlReferrer
            Common.Auth.Logout();
            return RedirectToAction("Index");
        }
        //[Filters.AuthVinaphone]
        public ActionResult ChangePassword(Guid id)
        {

            // return View(db.users.SingleOrDefault(u => u.id == id));
            return RedirectToAction("Index");
        }
        //[Filters.AuthVinaphone]
        [HttpPost]
        public IActionResult ChangePassword(Guid id, string password)
        {
            //try
            //{
            //    if (ModelState.IsValid)
            //    {
            //        var rs = db.users.Find(id);
            //        rs.password = TM.Encrypt.CryptoMD5TM(password + rs.salt);
            //        db.SaveChanges();
            //        ModelState.Clear();
            //        this.success("Cập nhật mật khẩu thành công");
            //        return RedirectToAction("Index");
            //    }
            //}
            //catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            //{
            //    Exception raise = dbEx;
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            string message = string.Format("{0}:{1}",
            //                validationErrors.Entry.Entity.ToString(),
            //                validationError.ErrorMessage);
            //            // raise a new exception nesting  
            //            // the current instance as InnerException  
            //            raise = new InvalidOperationException(message, raise);
            //        }
            //    }
            //    throw raise;
            //}
            return RedirectToAction("Index");
        }
        private List<TMShopsCore.Models.RolesAcess> AuthAllowRoles()
        {
            var rs = new List<TMShopsCore.Models.RolesAcess>(){
                new TMShopsCore.Models.RolesAcess { Controller = "Auth".ToUpper(), Action = "*" },
                new TMShopsCore.Models.RolesAcess { Controller =  "*", Action = "Index".ToUpper() },
                new TMShopsCore.Models.RolesAcess { Controller = "*", Action = "DataExistsCheck".ToUpper() },
                //new TMShopsCore.Models.RolesAcess { Controller = "*", Action = "PartialCreate".ToUpper() },
                //new TMShopsCore.Models.RolesAcess { Controller = "*", Action = "PartialEdit".ToUpper() }
            };
            return rs;
        }
        private List<TMShopsCore.Models.RolesAcess> AuthRoles(string userRoles)
        {
            var rs = new List<TMShopsCore.Models.RolesAcess>();
            var roles = db.Roles.Where(m => userRoles.Contains("," + m.Id.ToString() + ",")).ToList();
            foreach (var item in roles)
            {
                var tmp = item.Modules.Trim(',').Split(',');
                foreach (var i in tmp)
                {
                    var tmp2 = i.Split('.');
                    if (tmp2.Length > 1 && !rs.Any(m => m.Controller == tmp2[0] && m.Action == tmp2[1]))
                        rs.Add(new TMShopsCore.Models.RolesAcess() { Controller = tmp2[0], Action = tmp2[1] });
                }
            }
            return rs;
        }
    }
}