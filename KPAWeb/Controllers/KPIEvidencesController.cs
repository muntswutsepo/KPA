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
using System.Reflection.Metadata;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace KPAWeb.Controllers
{
    public class KPIEvidencesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;
        List<KPIEvidence> KPIEVIDENCES = new();
        SqlCommand com = new();
        SqlDataReader dr;
        SqlConnection con = new SqlConnection();
        public KPIEvidencesController(ApplicationDbContext context, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            _context = context;
            _config = config;
            con.ConnectionString = _config.GetConnectionString("DefaultConnection");
        }

        // GET: KPIEvidences
        public async Task<IActionResult> Evidence_Index(int KPI_No)
        {
            TempData.Keep("Weighting");
            return _context.KPIEvidences != null ?
                        View(await _context.KPIEvidences.Where(k => k.KPI_Ref_No == KPI_No).ToListAsync()) :
                          //View(await _context.KPIs.Where(k => k.KPA_Ref_No == KPA_No).FirstOrDefaultAsync()):
                          //View(await _context.KPIs.ToListAsync()):
                          Problem("Entity set 'ApplicationDbContext.KPIEvidences'  is null.");
        }

    // GET: KPIEvidences/Details/5
    public async Task<IActionResult> Evidence_Details(int? id)
        {
            if (id == null || _context.KPIEvidences == null)
            {
                return NotFound();
            }

            var KPIEvidence = await _context.KPIEvidences
                .FirstOrDefaultAsync(m => m.Evidence_No == id);
            if (KPIEvidence == null)
            {
                return NotFound();
            }

            return View(KPIEvidence);
        }

        // GET: KPIEvidences/Create
        public IActionResult Evidence_Create( int? weighting)
        {
            TempData.Keep("Weighting");
            ViewData["KPI_Ref_No"] = new SelectList(_context.KPIs, "KPI_No", "KPI_No");
            TempData.Keep("KPI_Ref_No");
            return View();
        }

        // POST: KPIEvidences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Evidence_Create([Bind("Evidence_No,KPI_Ref_No,Months,Start_Date,End_Date,No_Of_Days,Own_Score,Line_Manager_Score,Weighting,Final_Score,File")] KPIEvidence KPIEvidences)
        {
            //int TotalCount = 0;
            //int CountKPI = 0;
            //int TotalKPA = 0;
            //if (KPIS.Count > 0)
            //{
            //    KPIS.Clear();
            //}
            //try
            //{
            //    con.Open();
            //    com.Connection = con;
            //    com.CommandText = "EXEC [KPA].[dbo].KPIWeightCheck @WeightKPI= '" + KPIs.Weighting + "',@KPA_NO= '" + KPIs.KPA_Ref_No + "'";
            //    dr = com.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        KPIS.Add(new KPI
            //        {
            //            KPA_Ref_No = (int)dr["CountKPIWeight"],
            //            Weighting = (int)dr["Total"],
            //            Line_Manager_Score = (int)dr["total1"],

            //        });
            //    }
            //    con.Close();
            //    ViewBag.Board1List = KPIS.ToList();

            //    if (ViewBag.Board1List != null)
            //    {
            //        foreach (var item in ViewBag.Board1List)

            //        {
            //            CountKPI = item.KPA_Ref_No;
            //            TotalCount = item.Weighting;
            //            TotalKPA = item.Line_Manager_Score;
            //        }


            //    }
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    throw;

            //}
            //TempData["total"] = TotalCount;
            //TempData["totalKPA"] = TotalKPA;
            //if (CountKPI > 0)
            //{
            //    return View("KPIexception");
            //}
            //else
            //
            int bob = KPIEvidences.KPI_Ref_No;
            var KPI = await _context.KPIs
                .FirstOrDefaultAsync(m => m.KPI_No == bob);
            //if (ModelState.IsValid)
            //{
                _context.Add(KPIEvidences);
                await _context.SaveChangesAsync();

                TempData["Success"] = "KPI Evidence Added Successfully";

                if (KPIEvidences.KPI_Ref_No == null || _context.KPIEvidences == null)
                {
                    return NotFound();
                }
               
                if (KPI == null)
                {
                    return NotFound();
                }            

                return RedirectToAction("Evidence_Index", "KPIEvidences", KPI);
            //}

            return RedirectToAction("Evidence_Index", "KPIEvidences", KPI);
        }

        // GET: KPIEvidences/Edit/5
        public async Task<IActionResult> Evidence_Edit(int? id)
        {
            if (id == null || _context.KPIEvidences == null)
            {
                return NotFound();
            }

            var KPIEvidence = await _context.KPIEvidences.FindAsync(id);

            if (KPIEvidence == null)
            {
                return NotFound();
            }
            return View(KPIEvidence);
        }

        // POST: KPIEvidences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Evidence_Edit(int id, [Bind("Evidence_No,KPI_Ref_No,Months,Start_Date,End_Date,No_Of_Days,Own_Score,Line_Manager_Score,Weighting,Final_Score")] KPIEvidence KPIEvidences)
        {
            //int TotalCount = 0;
            //int CountKPI = 0;
            //int TotalKPA = 0;

            //if (id != KPIs.KPI_No)
            //{
            //    return NotFound();
            //}

            //if (KPIS.Count > 0)
            //{
            //    KPIS.Clear();
            //}
            //try
            //{
            //    con.Open();
            //    com.Connection = con;
            //    com.CommandText = "EXEC [KPA].[dbo].[KPIWeightEditCheck] @weighteditKPI= '" + KPIs.Weighting + "',@KPA_NO= '" + KPIs.KPA_Ref_No + "',@kpi_no='" + KPIs.KPI_No + "'";
            //    dr = com.ExecuteReader();
            //    while (dr.Read())
            //    {
            //        KPIS.Add(new KPI
            //        {
            //            KPA_Ref_No = (int)dr["CountKPIWeight"],
            //            Weighting = (int)dr["Total_KPA"],
            //            Line_Manager_Score = (int)dr["totalEdit"],

            //        });
            //    }
            //    con.Close();
            //    ViewBag.Board1List = KPIS.ToList();

            //    if (ViewBag.Board1List != null)
            //    {
            //        foreach (var item in ViewBag.Board1List)

            //        {
            //            CountKPI = item.KPA_Ref_No;
            //            TotalCount = item.Weighting;
            //            TotalKPA = item.Line_Manager_Score;
            //        }


            //    }
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    throw;

            //}
            //TempData["total"] = TotalCount;
            //TempData["totalKPA"] = TotalKPA;
            //if (CountKPI > 0)
            //{
            //    return View("KPIexception");
            //}
            //else
            if (id != KPIEvidences.Evidence_No)
            {
                return NotFound();
            }
            try
            {
                    _context.Update(KPIEvidences);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "KPI Evidence Updated Successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KPIEvidenceExists(KPIEvidences.Evidence_No))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            if (KPIEvidences.KPI_Ref_No == null || _context.KPIEvidences == null)
            {
                return NotFound();
            }
            var KPI = await _context.KPIs
                .FirstOrDefaultAsync(m => m.KPI_No == KPIEvidences.KPI_Ref_No);
            if (KPI == null)
            {
                return NotFound();
            }

            //_context.Add(KPIEvidence);
            //        await _context.SaveChangesAsync();
            //return RedirectToAction("Index","KPIEvidences");   
            // }
            return RedirectToAction("Evidence_Index", "KPIEvidences", KPI);
            //return RedirectToAction(nameof(Index));
            //}
            return View(KPIEvidences);
        }

        // GET: KPIEvidences/Delete/5
        public async Task<IActionResult> Evidence_Delete(int? id)
        {
            if (id == null || _context.KPIs == null)
            {
                return NotFound();
            }

            var KPIEvidence = await _context.KPIEvidences
                .FirstOrDefaultAsync(m => m.Evidence_No == id);
            if (KPIEvidence == null)
            {
                return NotFound();
            }

            return View(KPIEvidence);
        }

        // POST: KPIEvidences/Delete/5
        [HttpPost, ActionName("Evidence_Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int Ref)
        {
            if (_context.KPIEvidences == null)
            {
                return Problem("Entity set 'ApplicationDbContext.KPIEvidences'  is null.");
            }
            var KPIEvidence = await _context.KPIs.FindAsync(id);
            if (KPIEvidence != null)
            {
                _context.KPIs.Remove(KPIEvidence);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "KPI Evidence Deleted Successfully";

            if (Ref == null || _context.KPIEvidences == null)
            {
                return NotFound();
            }
            var KPI = await _context.KPIs
                .FirstOrDefaultAsync(m => m.KPI_No == Ref);
            if (KPI == null)
            {
                return NotFound();
            }

            //_context.Add(KPIEvidence);
            //        await _context.SaveChangesAsync();
            //return RedirectToAction("Index","KPIEvidences");   
            // }
            return RedirectToAction("Evidence_Index", "KPIEvidences", KPI);

            return RedirectToAction(nameof(Evidence_Index));
        }

        private bool KPIEvidenceExists(int id)
        {
            return (_context.KPIEvidences?.Any(e => e.Evidence_No == id)).GetValueOrDefault();
        }
    }
}
