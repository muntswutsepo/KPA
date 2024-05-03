using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KPAWeb.Data;
using KPAWeb.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Data.SqlClient;
using System.Xml.Schema;

namespace KPAWeb.Controllers
{
    public class KPIsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;
        List<KPI> KPIS = new();
        SqlCommand com = new();
        SqlDataReader dr;
        SqlConnection con = new SqlConnection();

        public static int KPANO = 0;

        public KPIsController(ApplicationDbContext context, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            _context = context;
            _config = config;
            con.ConnectionString = _config.GetConnectionString("DefaultConnection");
        }

        // GET: KPIs
        public async Task<IActionResult> KPI_Index(int KPA_No)
        {
            TempData.Keep("Weighting");
            KPANO = KPA_No;
            return _context.KPIs != null ?
                        View(await _context.KPIs.Where(k => k.KPA_Ref_No == KPA_No).ToListAsync()) :
                          //View(await _context.KPIs.Where(k => k.KPA_Ref_No == KPA_No).FirstOrDefaultAsync()):
                          //View(await _context.KPIs.ToListAsync()):
                          Problem("Entity set 'ApplicationDbContext.KPIs'  is null.");
        }

        // GET: KPIs/Details/5
        public async Task<IActionResult> KPI_Details(int? id)
        {
            if (id == null || _context.KPIs == null)
            {
                return NotFound();
            }

            var KPI = await _context.KPIs
                .FirstOrDefaultAsync(m => m.KPI_No == id);
            if (KPI == null)
            {
                return NotFound();
            }

            return View(KPI);
        }

        // GET: KPIs/Create
        public IActionResult KPI_Create()
        {
            TempData.Keep("Weighting");
            ViewData["KPA_Ref_No"] = new SelectList(_context.KPAs, "KPA_No", "KPA_No");
            TempData.Keep("KPA_Ref_No");
            return View();

        }

        // POST: KPIs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KPI_Create([Bind("KPI_No,KPA_Ref_No,KPI_Description,Weighting,Total_Weight,Target")] KPI KPIs)
        {
            int TotalCount = 0;
            int CountKPI = 0;
            int TotalKPA = 0;
            //if (ModelState.IsValid)
            //{
            if (KPIS.Count > 0)
            {
                KPIS.Clear();
            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "EXEC [KPA].[dbo].KPIWeightCheck @WeightKPI= '" + KPIs.Weighting + "',@KPA_NO= '" + KPIs.KPA_Ref_No + "'";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    KPIS.Add(new KPI
                    {
                        KPA_Ref_No = (int)dr["CountKPIWeight"],
                        Weighting = (int)dr["Total"],
                        Total_Weight = (int)dr["total1"],

                    });
                }
                con.Close();
                ViewBag.Board1List = KPIS.ToList();

                if (ViewBag.Board1List != null)
                {
                    foreach (var item in ViewBag.Board1List)

                    {
                        CountKPI = item.KPA_Ref_No;
                        TotalCount = item.Weighting;
                        TotalKPA = item.Total_Weight;
                    }


                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;

            }
            TempData["total"] = TotalCount;
            TempData["totalKPA"] = TotalKPA;

            if (CountKPI > 0)
            {
                return View("KPIexception");
            }
            else
            {
                _context.Add(KPIs);
                await _context.SaveChangesAsync();
                TempData["Success"] = "KPA Added Successfully";
                if (KPIs.KPA_Ref_No == null || _context.KPIs == null)
                {
                    return NotFound();
                }
                int bob = KPIs.KPA_Ref_No;
                var KPA = await _context.KPAs
                    .FirstOrDefaultAsync(m => m.KPA_No == bob);
                if (KPA == null)
                {
                    return NotFound();
                }


                return RedirectToAction("KPI_Index", "KPIs", KPA);
            }
                
            
        }

        // GET: KPIs/Edit/5
        public async Task<IActionResult> KPI_Edit(int? id)
        {
            if (id == null || _context.KPIs == null)
            {
                return NotFound();
            }

            var KPI = await _context.KPIs.FindAsync(id);

            if (KPI == null)
            {
                return NotFound();
            }
            return View(KPI);
        }

        // POST: KPIs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KPI_Edit(int id, /*int KPANO, */[Bind("KPI_No,KPA_Ref_No,KPI_Description,Weighting,Total_Weight,Target")] KPI KPIs)
        {
            int TotalCount = 0;
            int CountKPI   = 0;
            int TotalKPA   = 0;
            //int KPANO      = KPIs.KPA_Ref_No;
            //int refNo = KPIs.KPA_Ref_No;
        
            if (id != KPIs.KPI_No)
            {
                return NotFound();
            }
 
            if (KPIS.Count > 0)
            {
                KPIS.Clear();
            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "EXEC [KPA].[dbo].[KPIWeightEditCheck] @weighteditKPI= '" + KPIs.Weighting + "',@KPA_NO= '" + KPANO + "',@kpi_no='"+KPIs.KPI_No+ "'";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    KPIS.Add(new KPI
                    {
                        KPI_No = (int)dr["CountKPIWeight"],
                        Weighting = (int)dr["Total_KPA"],
                        Total_Weight = (int)dr["totalEdit"],

                    });
                }
                con.Close();
                ViewBag.Board1List = KPIS.ToList();

                if (ViewBag.Board1List != null)
                {
                    foreach (var item in ViewBag.Board1List)

                    {
                        CountKPI = item.KPI_No;
                        TotalCount = item.Weighting;
                        TotalKPA = item.Total_Weight;
                    }


                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;

            }
            TempData["total"] = TotalCount;
            TempData["totalKPA"] = TotalKPA;
            if (CountKPI > 0)
            {
                return View("KPIexception");
            }
            else
            try
                {
                    _context.Update(KPIs);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "KPI Updated Successfully";
            }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KPIExists(KPIs.KPI_No))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            if (KPIs.KPA_Ref_No == null || _context.KPIs == null)
            {
                return NotFound();
            }
            var KPA = await _context.KPAs
                .FirstOrDefaultAsync(m => m.KPA_No == KPIs.KPA_Ref_No);
            if (KPA == null)
            {
                return NotFound();
            }

            //_context.Add(KPI);
            //        await _context.SaveChangesAsync();
            //return RedirectToAction("Index","KPIs");   
            // }
            return RedirectToAction("KPI_Index", "KPIs", KPA);
            //return RedirectToAction(nameof(Index));
            //}
            return View(KPIs);
        }

        // GET: KPIs/Delete/5
        public async Task<IActionResult> KPI_Delete(int? id)
        {
            if (id == null || _context.KPIs == null)
            {
                return NotFound();
            }

            var KPI = await _context.KPIs
                .FirstOrDefaultAsync(m => m.KPI_No == id);
            if (KPI == null)
            {
                return NotFound();
            }

            return View(KPI);
        }

        // POST: KPIs/Delete/5
        [HttpPost, ActionName("KPI_Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id,int Ref)
        {
            if (_context.KPIs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.KPIs'  is null.");
            }
            var KPI = await _context.KPIs.FindAsync(id);
            if (KPI != null)
            {
                _context.KPIs.Remove(KPI);
            }
            
            await _context.SaveChangesAsync();
            TempData["Success"] = "KPI Deleted Successfully";

            if (Ref == null || _context.KPIs == null)
            {
                return NotFound();
            }
            var KPA = await _context.KPAs
                .FirstOrDefaultAsync(m => m.KPA_No == Ref);
            if (KPA == null)
            {
                return NotFound();
            }

            //_context.Add(KPI);
            //        await _context.SaveChangesAsync();
            //return RedirectToAction("Index","KPIs");   
            // }
            return RedirectToAction("KPI_Index", "KPIs", KPA);

            return RedirectToAction(nameof(KPI_Index));
        }

        private bool KPIExists(int id)
        {
          return (_context.KPIs?.Any(e => e.KPI_No == id)).GetValueOrDefault();
        }
    }
}
