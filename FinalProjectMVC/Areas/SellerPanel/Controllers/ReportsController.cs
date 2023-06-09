﻿using FinalProjectMVC.Areas.Identity.Data;
using FinalProjectMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.Areas.SellerPanel.Controllers
{
    [Area("SellerPanel")]
    [Authorize]
    public class ReportsController : Controller
    {
        readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context) => _context = context;

        // GET: SellerPanel/Reports/Create
        public IActionResult Create()
        {
            ViewData["ReviewId"] = new SelectList(_context.Reviews, "Id", "Id");
            return View();
        }

        // POST: SellerPanel/Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,IsSolved,ReviewId")] Report report)
        {
            if (ModelState.IsValid)
            {
                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ReviewId"] = new SelectList(_context.Reviews, "Id", "Id", report.ReviewId);
            return View(report);
        }

        // GET: SellerPanel/Reports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reports == null)
            {
                return NotFound();
            }

            var report = await _context.Reports.FindAsync(id);
            if (report == null) return NotFound();
            ViewData["ReviewId"] = new SelectList(_context.Reviews, "Id", "Id", report.ReviewId);
            return View(report);
        }

        // POST: SellerPanel/Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,IsSolved,ReviewId")] Report report)
        {
            if (id != report.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.Id)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ReviewId"] = new SelectList(_context.Reviews, "Id", "Id", report.ReviewId);
            return View(report);
        }

        bool ReportExists(int id) => (_context.Reports?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}