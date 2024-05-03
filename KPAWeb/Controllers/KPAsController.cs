using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KPAWeb.Data;
using KPAWeb.Models;
using Microsoft.Data.SqlClient;

namespace KPAWeb.Controllers
{
    public class KPAsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;
        List<KPA> KPAs = new();
        SqlCommand com = new();
        SqlDataReader dr;
        SqlConnection con = new SqlConnection();


        public KPAsController(ApplicationDbContext context, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            _context = context;
            _config = config;
            con.ConnectionString = _config.GetConnectionString("DefaultConnection");
        }

        // GET: KPAs
        public async Task<IActionResult> KPA_Index()
        {
              return _context.KPAs != null ? 
                          View(await _context.KPAs.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.KPAs'  is null.");
        }

        // GET: KPAs/Details/5
        public async Task<IActionResult> KPA_Details(int? id)
        {
            if (id == null || _context.KPAs == null)
            {
                return NotFound();
            }

            var KPA = await _context.KPAs
                .FirstOrDefaultAsync(m => m.KPA_No == id);
            if (KPA == null)
            {
                return NotFound();
            }

            return View(KPA);
        }

        // GET: KPAs/Create
        public IActionResult KPA_Create()
        {
            return View();
        }

        // POST: KPAs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KPA_Create([Bind("KPA_No,KPA_Description,Weighting")] KPA KPA)  
        {
            int TotalCount = 0;
            int CountKPA = 0;
            //if (ModelState.IsValid)
            //{
            if (KPAs.Count > 0)
            {
                KPAs.Clear();
            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "EXEC [KPA].[dbo].WeightCheck @weightKPA= '" + KPA.Weighting + "'";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    KPAs.Add(new KPA
                    {
                        KPA_No = (int)dr["CountKPAWeight"],
                        Weighting = (int)dr["Total"],


                    });
                }
                con.Close();
                ViewBag.Board1List = KPAs.ToList();

                if (ViewBag.Board1List !=null)
                {foreach (var item in ViewBag.Board1List) 
                    
                    { 
                    CountKPA =item.KPA_No;
                        TotalCount = item.Weighting;
                    }


                }
            }
            catch (DbUpdateConcurrencyException)
            {
                    throw;
              
            }
            TempData["total"] = TotalCount;
            if (CountKPA>0)
            {
                return View("Exception");
            }
            else
            {
                _context.Add(KPA);
                await _context.SaveChangesAsync();
                TempData["Success"] = "KPA Added Successfully";
                return RedirectToAction(nameof(Index));


            }

            
           
            //}
            return View(KPA);
        }

        // GET: KPAs/Edit/5
        public async Task<IActionResult> KPA_Edit(int? id)
        {
            if (id == null || _context.KPAs == null)
            {
                return NotFound();
            }

            var KPA = await _context.KPAs.FindAsync(id);
            if (KPA == null)
            {
                return NotFound();
            }
            return View(KPA);
        }

        // POST: KPAs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KPA_Edit(int id, [Bind("KPA_No,KPA_Description,Weighting")] KPA KPA)
        {
            int TotalCount = 0;
            int CountKPA = 0;

            if (id != KPA.KPA_No)
            {
                return NotFound();
            }
   
            if (KPAs.Count > 0)
            {
                KPAs.Clear();
            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "EXEC [KPA].[dbo].WeightEditCheck @weightEditKPA= '" + KPA.Weighting + "',@kpa_no='"+KPA.KPA_No+ "'";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    KPAs.Add(new KPA
                    {
                        KPA_No = (int)dr["CountKPAEditWeight"],
                        Weighting = (int)dr["TotalEdit"],
                    });
                }
                con.Close();
                ViewBag.Board1List = KPAs.ToList();

                if (ViewBag.Board1List != null)
                {
                    foreach (var item in ViewBag.Board1List)

                    {
                        CountKPA = item.KPA_No;
                        TotalCount = item.Weighting;
                    }


                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;

            }
            TempData["total"] = TotalCount;
            if (CountKPA > 0)
            {
                return View("Exception");
            }
             else
                try
                {
                    _context.Update(KPA);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "KPA Updated Successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                if (!KPAExists(KPA.KPA_No))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(KPA_Index));
            //}
            return View(KPA);
        }

        // GET: KPAs/Delete/5
        public async Task<IActionResult> KPA_Delete(int? id)
        {
            if (id == null || _context.KPAs == null)
            {
                return NotFound();
            }

            var KPA = await _context.KPAs
                .FirstOrDefaultAsync(m => m.KPA_No == id);
            if (KPA == null)
            {
                return NotFound();
            }

            return View(KPA);
        }

        // POST: KPAs/Delete/5
        [HttpPost, ActionName("KPA_Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.KPAs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.KPAs'  is null.");
            }
            var KPA = await _context.KPAs.FindAsync(id);
            if (KPA != null)
            {
                _context.KPAs.Remove(KPA);
            }
            
            await _context.SaveChangesAsync();
            TempData["Success"] = "KPA Deleted Successfully";
            return RedirectToAction(nameof(KPA_Index));
        }

        private bool KPAExists(int id)
        {
          return (_context.KPAs?.Any(e => e.KPA_No == id)).GetValueOrDefault();
        }
    }
}
