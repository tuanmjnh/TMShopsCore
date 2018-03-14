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
    public class ModulesController : BaseController
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
    [Route("api/ModulesAPI")]
    public class ModulesAPIController : Controller
    {
        private readonly TMShopsContext _context;
        private const string PathInUpload = @"Data\";
        public ModulesAPIController(TMShopsContext context) { _context = context; }

        [HttpGet]
        public async Task<IActionResult> Select(string sort, string order, string search, int offset = 0, int limit = 10, int flag = 1)
        {
            //Get data for Status
            var d = _context.Modules.Where(m => m.Flag == flag);
            //Get data for Search
            if (!string.IsNullOrEmpty(search))
                d = d.Where(m => m.Name.Contains(search) || m.Url.Contains(search));
            //Get total item
            var total = d.Count();
            //Sort And Orders
            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.ToLower() == "name" && order.ToLower() == "asc")
                    d = d.OrderBy(m => m.Name);
                else if (sort.ToLower() == "name" && order.ToLower() == "desc")
                    d = d.OrderByDescending(m => m.Name);
                else if (sort.ToLower() == "url" && order.ToLower() == "asc")
                    d = d.OrderBy(m => m.Url);
                else if (sort.ToLower() == "url" && order.ToLower() == "desc")
                    d = d.OrderByDescending(m => m.Url);
                else if (sort.ToLower() == "appkey" && order.ToLower() == "asc")
                    d = d.OrderBy(m => m.AppKey);
                else if (sort.ToLower() == "appkey" && order.ToLower() == "desc")
                    d = d.OrderByDescending(m => m.AppKey);
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
        public async Task<IActionResult> Select(long? id)
        {
            if (id == null)
                return Json(new { danger = TM.Common.Error.IDNull });
            if (!ModelState.IsValid)
                return Json(new { danger = TM.Common.Error.Model });

            var Data = await _context.Modules.SingleOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Insert(Modules Data)
        {
            if (!ModelState.IsValid)
                return Json(new { danger = TM.Common.Error.Model });
            if (DataNameExists(Data.AppKey))
                return Json(new { danger = TM.Common.Error.Exist });
            try
            {
                var defval = "";
                //Data.Id = Guid.NewGuid();
                Data.Action = Data.Action != null ? "," + Data.Action.Trim(',') + "," : "";
                Data.CreatedBy = Common.Auth.getUserAction();
                Data.CreatedAt = DateTime.Now;
                Data.UpdatedBy = Common.Auth.getUserAction();
                Data.UpdatedAt = DateTime.Now;

                _context.Modules.Add(Data);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
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
        public async Task<IActionResult> Update(long id, Modules Data)//[FromBody]
        {
            if (!ModelState.IsValid)
                return Json(new { danger = TM.Common.Error.Model });

            if (id != Data.Id)
                return Json(new { danger = TM.Common.Error.Update });

            try
            {
                Data.Action = Data.Action != null ? "," + Data.Action.Trim(',') + "," : null;
                Data.UpdatedBy = Common.Auth.getUserAction();
                Data.UpdatedAt = DateTime.Now;
                _context.Modules.Attach(Data);
                var entry = _context.Entry(Data);
                entry.Property(m => m.Name).IsModified = true;
                entry.Property(m => m.Url).IsModified = true;
                entry.Property(m => m.Action).IsModified = true;
                entry.Property(m => m.Icon).IsModified = true;
                entry.Property(m => m.Orders).IsModified = true;
                entry.Property(m => m.Flag).IsModified = true;
                entry.Property(m => m.UpdatedBy).IsModified = true;
                entry.Property(m => m.UpdatedAt).IsModified = true;

                await _context.SaveChangesAsync();
            }
            catch (Exception)
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
                    var tmp = long.Parse(item);
                    var Data = await _context.Modules.SingleOrDefaultAsync(m => m.Id == tmp);

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
                var tmp = long.Parse(item);
                var Data = await _context.Modules.SingleOrDefaultAsync(m => m.Id == tmp);

                if (Data == null) return Json(new { danger = TM.Common.Error.Null });

                _context.Modules.Remove(Data);
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
            return _context.Modules.Any(m => m.Name.ToLower() == AppKey.ToLower());
        }
        private bool DataExistsID(long id)
        {
            return _context.Modules.Any(e => e.Id == id);
        }
    }
}
