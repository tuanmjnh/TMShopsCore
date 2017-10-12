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
    public class RolesController : BaseController
    {
        // GET: CMS/Data
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
    }
    //API
    [ServiceFilter(typeof(MiddlewareFilters.Auth))]
    [Produces("application/json")]
    [Route("api/RolesAPI")]
    public class RolesAPIController : Controller
    {
        private readonly TMShopsContext _context;
        private const string PathInUpload = @"Data\";
        public RolesAPIController(TMShopsContext context) { _context = context; }

        [HttpGet]
        public async Task<IActionResult> GetData(string sort, string order, string search, int offset = 0, int limit = 10, int flag = 1)
        {
            //Get data for Status
            var d = _context.Roles.Where(m => m.Flag == flag);
            //Get data for Search
            if (!string.IsNullOrEmpty(search))
                d = d.Where(m => m.Name.Contains(search));
            //Get total item
            var total = d.Count();
            //Sort And Orders
            if (!string.IsNullOrEmpty(sort)) { 
                if (sort.ToLower() == "name" && order.ToLower() == "asc")
                    d = d.OrderBy(m => m.Name);
                else if (sort.ToLower() == "name" && order.ToLower() == "desc")
                    d = d.OrderByDescending(m => m.Name);
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

        // GET: api/DataAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetData(long? id)
        {
            if (id == null)
                return Json(new { danger = TM.Common.Error.IDNull });
            if (!ModelState.IsValid)
                return Json(new { danger = TM.Common.Error.Model });

            var Data = await _context.Roles.SingleOrDefaultAsync(m => m.Id == id);
            //var Data = _context.Data.Where(m => m.Id == id).Select(m => new
            //{
            //    obj = m,
            //    cUser = _context.Data.SingleOrDefault(d => d.Id.ToString() == m.CreatedBy).FullName,
            //    uUser = _context.Data.SingleOrDefault(d => d.Id.ToString() == m.UpdatedBy).FullName,
            //}).SingleOrDefaultAsync();

            if (Data == null)
                return Json(new { danger = TM.Common.Error.Null });

            return Json(new { success = TM.Common.Success.Get, data = Data }); ;
        }

        // POST: api/DataAPI
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> PostData(Roles Data)
        {
            if (!ModelState.IsValid)
                return Json(new { danger = TM.Common.Error.Model });
            try
            {
                //Data.Id = Guid.NewGuid();
                Data.CreatedBy = Common.Auth.getUserAction();
                Data.CreatedAt = DateTime.Now;
                Data.UpdatedBy = Common.Auth.getUserAction();
                Data.UpdatedAt = DateTime.Now;

                _context.Roles.Add(Data);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DataExistsID(Data.Id))
                    return Json(new { danger = TM.Common.Error.IDExist });
                else
                    return Json(new { danger = TM.Common.Error.DB });
            }
            return Json(new { success = TM.Common.Success.Create });
        }

        // PUT: api/DataAPI/5
        [HttpPut("{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> PutData(long id, Roles Data)//[FromBody]
        {
            if (!ModelState.IsValid)
                return Json(new { danger = TM.Common.Error.Model });

            if (id != Data.Id)
                return Json(new { danger = TM.Common.Error.Update });

            try
            {
                Data.UpdatedBy = Common.Auth.getUserAction();
                Data.UpdatedAt = DateTime.Now;
                _context.Roles.Attach(Data);
                var entry = _context.Entry(Data);
                entry.Property(m => m.Name).IsModified = true;
                entry.Property(m => m.Icon).IsModified = true;
                entry.Property(m => m.Orders).IsModified = true;
                entry.Property(m => m.Flag).IsModified = true;
                entry.Property(m => m.Details).IsModified = true;
                entry.Property(m => m.Modules).IsModified = true;
                entry.Property(m => m.UpdatedBy).IsModified = true;
                entry.Property(m => m.UpdatedAt).IsModified = true;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { danger = TM.Common.Error.DB });
            }

            return Json(new { success = TM.Common.Success.Update });
        }

        // DELETE: api/DataAPI/5
        [HttpDelete, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteData(string id)
        {
            //if (!ModelState.IsValid)
            //    return Json(new { danger = TM.Common.Error.Model });
            var _id = id.Trim(',').Split(',');
            foreach (var item in _id)
            {
                var tmp = long.Parse(item);
                var Data = await _context.Roles.SingleOrDefaultAsync(m => m.Id == tmp);

                if (Data == null) return Json(new { danger = TM.Common.Error.Null });

                _context.Roles.Remove(Data);
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
                    var tmp = long.Parse(item);
                    var Data = await _context.Roles.SingleOrDefaultAsync(m => m.Id == tmp);

                    if (Data == null) return Json(new { danger = TM.Common.Error.Null });
                    if (Data.Flag == 0)
                    {
                        Data.Flag = 1;
                        flag = false;
                    }
                    else
                        Data.Flag = 0;
                    _context.Entry(Data);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Json(new { danger = flag ? TM.Common.Error.Delete : TM.Common.Error.Recover });
            }
            return Json(new { success = flag ? TM.Common.Success.Delete : TM.Common.Success.Recover });
        }
        private bool DataExistsID(long id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
        //Check Exists DataName Create
        [HttpGet("DataNameExistsCheck")]
        public JsonResult DataNameExistsCheck(string Title)
        {
            return Json(!DataNameExists(Title));
        }
        private bool DataNameExists(string Title)
        {
            return _context.Roles.Any(m => m.Name.ToLower() == Title.ToLower());
        }
    }
}
