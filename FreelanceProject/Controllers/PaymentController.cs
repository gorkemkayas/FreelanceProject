using FreelanceProject.Data.Context;
using FreelanceProject.Data.Entities;
using FreelanceProject.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreelanceProject.Controllers
{
    public class PaymentController : Controller
    {
        private readonly FreelanceDbContext _context;

        public PaymentController(FreelanceDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Summary(Guid jobId, Guid userId, string receiverId)
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(x => x.Id == jobId);
            if (job == null)
                return NotFound("İş bulunamadı.");

            var commission = job.Budget * 0.10m;
            var total = job.Budget + commission;

            var viewModel = new PaymentSummaryViewModel
            {
                JobId = job.Id,
                UserId = userId,
                JobTitle = job.Title,
                Budget = (decimal)job.Budget,
                SiteCommission = (decimal)commission,
                TotalAmount = (decimal)total
            };

            return View(viewModel);
        }
        //[HttpPost]
        //public async Task<IActionResult> Complete(Guid jobId, Guid userId, decimal amount)
        //{
        //    // Ödeme işlemi burada yapılacak (örnek amaçlı direkt başarılı kabul ediyoruz)
        //    // Burada gerçek ödeme entegrasyonu (Stripe, PayPal vb.) olmalı

        //    // İş başvurusunu bul ve durumunu güncelle
        //    var jobApplication = await _context.JobApplications
        //        .FirstOrDefaultAsync(ja => ja.JobId == jobId && ja.ApplicantId == userId);

        //    if (jobApplication == null)
        //    {
        //        return NotFound("İş başvurusu bulunamadı.");
        //    }

        //    jobApplication.Status = JobApplicationStatus.Completed;

        //    // Değişiklikleri kaydet
        //    await _context.SaveChangesAsync();

        //    // Başarı mesajını TempData ile aktar
        //    TempData["SuccessMessage"] = $"Payment of ${amount.ToString("0.00")} was successful. İş durumu tamamlandı.";

        //    // İstersen bir sayfaya yönlendir (örneğin ödeme başarılı sayfası veya iş detayları)
        //    return RedirectToAction("Success");
        //}

        [HttpPost]
        public async Task<IActionResult> Complete(Guid jobId, Guid userId, decimal amount, bool approve, JobApplicationStatus status)
        {
            // İş durumunu güncelle
            //var job = _context.Jobs.FirstOrDefault(j => j.Id == jobId);
            var systemUser = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == "c583f39c-6e40-4e34-b852-08dd9633dfd1");
            if (systemUser == null)
            {
                TempData["UserNotFound"] = "Kullanıcı bulunamadı.";
                return View();
            }
            var receiverUser = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId.ToString());
            if (receiverUser == null)
            {
                TempData["ReceiverNotFound"] = "Alıcı bulunamadı.";
                return View();
            }
            systemUser.Wallet += (float)(amount / 10) ;
            receiverUser.Wallet += (float)((amount / 10) * 9);

            await _context.SaveChangesAsync();


            var application = await _context.JobApplications
                       .FirstOrDefaultAsync(a => a.ApplicantId == userId && a.JobId == jobId);

            if (application != null)
            {
                application.Status = status; // "Completed"
                _context.SaveChanges();
            }

            // Ana sayfaya yönlendir
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Success()
        {
            ViewBag.Message = TempData["SuccessMessage"];
            return View();
        }

    }
}
