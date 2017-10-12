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
    public class RolesController : BaseController
    {
        // GET: CMS/Data
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Insert()
        {
            return PartialView("PartialCreate");
        }
        public IActionResult Update()
        {
            return PartialView("PartialEdit");
        }
    }
    //API
    [Produces("application/json")]
    [Route("api/RolesAPI")]
    public class RolesAPIController : Controller
    {
        private readonly TMShopsContext _context;
        private const string PathInUpload = @"Data\";
        public RolesAPIController(TMShopsContext context) { _context = context; }

        [HttpGet]
        public async Task<IActionResult> Select(string sort, string order, string search, int offset = 0, int limit = 10, int flag = 1)
        {
            //Get data for Status
            var d = _context.Roles.Where(m => m.Flag == flag);
            //Get data for Search
            if (!string.IsNullOrEmpty(search))
                d = d.Where(m => m.Name.Contains(search));
            //Get total item
            var total = d.Count();
            //Sort And Orders
            if (!string.IsNullOrEmpty(sort))
            {
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
            var rs = new List<Roles>();
            if (limit > 0)
                rs = await d.Skip(offset).Take(limit).ToListAsync();
            else
                rs = await d.ToListAsync();
            //Return Json Data
            return Json(new { total = total, rows = rs });
        }

        // GET: api/DataAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Select(Guid? id)
        {
            if (id == null)
                return Json(new { danger = TM.Common.Error.IDNull });
            if (!ModelState.IsValid)
                return Json(new { danger = TM.Common.Error.Model });

            var Data = await _context.Roles.SingleOrDefaultAsync(m => m.Id == id);
            if (Data == null)
                return Json(new { danger = TM.Common.Error.Null });

            return Json(new { success = TM.Common.Success.Get, data = Data, modules = GetModules() });
        }
        [HttpGet("INSERT")]
        public IActionResult INSERT()
        {
            return Json(new { success = TM.Common.Success.Get, modules = GetModules() });
        }
        // POST: api/DataAPI
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Insert(Roles Data)
        {
            if (!ModelState.IsValid)
                return Json(new { danger = TM.Common.Error.Model });
            try
            {
                Data.Id = Guid.NewGuid();
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
        public async Task<IActionResult> Update(Guid id, Roles Data)//[FromBody]
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
        //Update Flag
        [HttpPost("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
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
        // DELETE: api/DataAPI/5
        [HttpDelete, ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(string id)
        {
            //if (!ModelState.IsValid)
            //    return Json(new { danger = TM.Common.Error.Model });
            var _id = id.Trim(',').Split(',');
            foreach (var item in _id)
            {
                var tmp = Guid.Parse(item);
                var Data = await _context.Roles.SingleOrDefaultAsync(m => m.Id == tmp);

                if (Data == null) return Json(new { danger = TM.Common.Error.Null });

                _context.Roles.Remove(Data);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = TM.Common.Success.Delete });
        }

        //Check Exists DataName Create
        [HttpGet("DataExistsCheck")]
        public JsonResult DataExistsCheck(string AppKey)
        {
            return Json(!DataNameExists(AppKey));
        }
        private bool DataNameExists(string AppKey)
        {
            return _context.Roles.Any(m => m.Name.ToLower() == AppKey.ToLower());
        }
        private bool DataExistsID(Guid id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
        private dynamic GetModules()
        {
            var Modules = _context.Modules.Where(m => m.Flag > 0).Select(m => new
            {
                name = m.Name,
                appKey = m.AppKey
            }).ToList();
            return Modules;
        }
    }
}
