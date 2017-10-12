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
    public class SettingsController : BaseController
    {
        private readonly TMShopsContext _context;

        public SettingsController(TMShopsContext context)
        {
            _context = context;    
        }

        // GET: CMS/Settings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Settings.ToListAsync());
        }

        // GET: CMS/Settings/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settings = await _context.Settings
                .SingleOrDefaultAsync(m => m.Id == id);
            if (settings == null)
            {
                return NotFound();
            }

            return View(settings);
        }

        // GET: CMS/Settings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CMS/Settings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ModuleKey,SubKey,Value,SubValue,Desc,Orders,Flag,Extras")] Settings settings)
        {
            if (ModelState.IsValid)
            {
                //settings.Id = Guid.NewGuid();
                _context.Add(settings);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(settings);
        }

        // GET: CMS/Settings/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settings = await _context.Settings.SingleOrDefaultAsync(m => m.Id == id);
            if (settings == null)
            {
                return NotFound();
            }
            return View(settings);
        }

        // POST: CMS/Settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,ModuleKey,SubKey,Value,SubValue,Desc,Orders,Flag,Extras")] Settings settings)
        {
            if (id != settings.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(settings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettingsExists(settings.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(settings);
        }

        // GET: CMS/Settings/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settings = await _context.Settings
                .SingleOrDefaultAsync(m => m.Id == id);
            if (settings == null)
            {
                return NotFound();
            }

            return View(settings);
        }

        // POST: CMS/Settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var settings = await _context.Settings.SingleOrDefaultAsync(m => m.Id == id);
            _context.Settings.Remove(settings);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SettingsExists(long id)
        {
            return _context.Settings.Any(e => e.Id == id);
        }
    }
}
