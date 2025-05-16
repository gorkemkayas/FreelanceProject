using FreelanceProject.Data.Entities;    // Entity dosyasýný da ekle
using FreelanceProject.Extensions;
using FreelanceProject.Models.ViewModels;  // ViewModel dosyasýný eklemeyi unutma
using FreelanceProject.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using FreelanceProject.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;       // DbContext dosyasýný da ekle
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using FreelanceProject.Models;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Claims;

namespace FreelanceProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly FreelanceDbContext _context;

    public HomeController(ILogger<HomeController> logger, FreelanceDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index(string? query)
    {

        var excludedStatuses = new[] {
        JobApplicationStatus.Completed,
        JobApplicationStatus.Ongoing
    };

        var jobsQuery = _context.Jobs
            .Where(a => a.IsActive &&
                        !a.IsDeleted ||
                        a.JobApplications.Any(p => !excludedStatuses.Contains(p.Status))).Include(a => a.Owner)
            .AsQueryable();

        if (!string.IsNullOrEmpty(query))
        {
            jobsQuery = jobsQuery.Where(j =>
                j.Title.Contains(query) ||
                j.Description.Contains(query) ||
                j.Category.Contains(query));
        }

        var lastThreeJobs = jobsQuery
            .OrderByDescending(j => j.Id)
            .Take(3)
            .ToList();

        // Kategori bazlý iþ sayýsý
        var categoryCounts = _context.Jobs
            .Where(j => j.IsActive && !j.IsDeleted)
            .GroupBy(j => j.Category)
            .Select(g => new
            {
                Category = g.Key,
                JobCount = g.Count()
            })
            .ToList();

        // Dictionary olarak ViewBag'e aktar
        ViewBag.CategoryCounts = categoryCounts.ToDictionary(x => x.Category, x => x.JobCount);
        ViewBag.TotalCount = categoryCounts.Sum(x => x.JobCount);

        return View(lastThreeJobs);

    }

    public IActionResult Privacy()
    {
        return View();
    }
    public async Task<IActionResult> Index2(string? query, string? category)
    {
        //// Veritabaný sorgusuna baþla
        //var jobsQuery = _context.Jobs.AsQueryable();

        //// Eðer query parametresi varsa, onu kullanarak filtrele
        //if (!string.IsNullOrEmpty(query))
        //{
        //    jobsQuery = jobsQuery.Where(j =>
        //        j.JobTitle.Contains(query) ||
        //        j.JobDescription.Contains(query) ||
        //        j.Category.Contains(query));
        //}

        //// Eðer kategori parametresi varsa, onu kullanarak filtrele
        //if (!string.IsNullOrEmpty(category))
        //{
        //    jobsQuery = jobsQuery.Where(j => j.Category == category);
        //}

        //// Filtrelenmiþ iþ ilanlarýný liste olarak al
        //var jobs = jobsQuery.ToList();

        //// Filtrelenmiþ iþ ilanlarýný view'a gönder
        //return View(jobs);

        var excludedStatuses = new[] {
        JobApplicationStatus.Completed,
        JobApplicationStatus.Ongoing
    };

        var jobsQuery = _context.Jobs
            .Where(a => a.IsActive &&
                        !a.IsDeleted ||
                        a.JobApplications.Any(p => !excludedStatuses.Contains(p.Status)))
            .AsQueryable();

        // Arama query parametresine göre filtreleme
        if (!string.IsNullOrEmpty(query))
        {
            jobsQuery = jobsQuery.Where(j =>
                j.Title.Contains(query) ||
                j.Description.Contains(query) ||
                j.Category.Contains(query));
        }

        // Kategoriye göre filtreleme
        if (!string.IsNullOrEmpty(category))
        {
            jobsQuery = jobsQuery.Where(j => j.Category == category);
        }

        // Listeyi al ve view'a gönder
        var jobs = await jobsQuery.Select(a => new JobAndApplicationExistsViewModel()
        {
            Job = a,
            //Applied= _context.JobApplications.Any(ap => ap.ApplicantId.ToString() == User.FindFirst(ClaimTypes.NameIdentifier)!.Value) }).ToListAsync();
            Applied = _context.JobApplications.Any(ap => ap.Job == a && ap.ApplicantId.ToString() == User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
        }).ToListAsync();

        return View(jobs);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
