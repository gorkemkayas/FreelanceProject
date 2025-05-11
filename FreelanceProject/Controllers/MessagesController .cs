using FreelanceProject.Data.Context;
using FreelanceProject.Data.Entities;
using FreelanceProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;



[Authorize]
[Route("Messages")]  // Global route, tüm metodlar bu route ile başlar.
public class MessagesController : Controller
{
    private readonly FreelanceDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public MessagesController(FreelanceDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
       _userManager = userManager;
    }

    // Mesajlar sayfası

    //public async Task<IActionResult> ViewChat(Guid receiverId, Guid jobId)
    //{
    //    var job = await _context.Jobs
    //        .Include(j => j.Owner)
    //        .FirstOrDefaultAsync(j => j.Id == jobId);

    //    if (job == null)
    //    {
    //        return NotFound();
    //    }

    //    var currentUserId = Guid.Parse(_userManager.GetUserId(User));
    //    var ownerId = job.OwnerId;

    //    // Sadece: (current user ↔ iş sahibi) arasındaki mesajlar gelsin
    //    var messages = await _context.Messages
    //         .Where(m => m.JobId == jobId &&
    //               (
    //                   (m.SenderId == currentUserId && m.ReceiverId == job.OwnerId) ||
    //                   (m.SenderId == job.OwnerId && m.ReceiverId == currentUserId)
    //               ))
    //    .Include(m => m.Sender)
    //    .OrderBy(m => m.SentDate)
    //    .ToListAsync();

    //    //.Where(m => m.JobId == jobId)
    //    //.OrderBy(m => m.SentDate)
    //    //.Include(m => m.Sender) // Sender (Kullanıcı) bilgisini de dahil ediyoruz
    //    //.ToListAsync();


    //    var model = new ChatViewModel
    //    {
    //        Job = job,
    //        Messages = messages
    //    };

    //    return View(model);
    //}




    [Authorize]
    public async Task<IActionResult> ViewChat(Guid receiverId, Guid jobId)
    {
        var currentUserId = Guid.Parse(_userManager.GetUserId(User));

        var job = await _context.Jobs
            .FirstOrDefaultAsync(j => j.Id == jobId);

        if (job == null)
            return NotFound();

        var messages = await _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => m.JobId == jobId &&
                   ((m.SenderId == currentUserId && m.ReceiverId == receiverId) ||
                    (m.SenderId == receiverId && m.ReceiverId == currentUserId)))
            .OrderBy(m => m.SentDate)
            .ToListAsync();

        var viewModel = new ChatViewModel
        {
            Job = job,
            Messages = messages,
            ReceiverId = receiverId,
            OwnerId = currentUserId,
            JobId = jobId
        };

        return View("ViewChat", viewModel);
    }




    


    [HttpPost]
    [Route("SendMessage")]
    public async Task<IActionResult> SendMessage(Guid jobId, Guid receiverId, string newMessageContent)
    {
        if (string.IsNullOrWhiteSpace(newMessageContent))
        {
            return RedirectToAction("ViewChat", new { jobId, receiverId });
        }

        var senderId = Guid.Parse(_userManager.GetUserId(User));

        var job = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == jobId);
        if (job == null)
        {
            return NotFound();
        }

        var message = new MessageEntity
        {
            Content = newMessageContent,
            SentDate = DateTime.UtcNow,
            SenderId = senderId,
            ReceiverId = receiverId,
            JobId = jobId,
            IsRead = false
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return RedirectToAction("ViewChat", new { jobId, receiverId });
    }


    //[Authorize]
    //[HttpGet("JobMessageThreads/{jobId}")]
    //public async Task<IActionResult> JobMessageThreads(Guid jobId)
    //{
    //    var currentUserId = Guid.Parse(_userManager.GetUserId(User));

    //    var job = await _context.Jobs
    //        .Include(j => j.Owner)
    //        .FirstOrDefaultAsync(j => j.Id == jobId);

    //    if (job == null || job.OwnerId != currentUserId)
    //        return Unauthorized();

    //    var messages = await _context.Messages
    //        .Include(m => m.Sender)
    //        .Include(m => m.Receiver)
    //        .Where(m => m.JobId == jobId)
    //        .ToListAsync();

    //    var threads = messages
    //        .GroupBy(m => m.SenderId == currentUserId ? m.Receiver : m.Sender)
    //        .Select(g => new MessageThreadViewModel
    //        {
    //            UserId = g.Key.Id,
    //            UserName = g.Key.UserName,
    //            LastMessage = g.OrderByDescending(m => m.SentDate).FirstOrDefault()?.Content,
    //            LastMessageDate = g.Max(m => m.SentDate),
    //            JobId = jobId,
    //            ReceiverId = g.Key.Id,
    //            OwnerId = currentUserId
    //        })
    //        .ToList();

    //    var viewModel = new ChatViewModel
    //    {
    //        Job = job,
    //        OwnerId = currentUserId,
    //        JobId = jobId,
    //        Threads = threads
    //    };

    //    return View("JobMessageThreads", viewModel);
    //}



    [Authorize]
    [HttpGet("JobMessageThreads/{jobId}")]
    public async Task<IActionResult> JobMessageThreads(Guid jobId, string searchTerm)
    {
        var currentUserId = Guid.Parse(_userManager.GetUserId(User));

        var job = await _context.Jobs
            .Include(j => j.Owner)
            .FirstOrDefaultAsync(j => j.Id == jobId);

        if (job == null || job.OwnerId != currentUserId)
            return Unauthorized();

        var messages = await _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => m.JobId == jobId)
            .ToListAsync();

        // Kullanıcıya göre gruplama (sohbet eşleri)
        var threads = messages
            .GroupBy(m => m.SenderId == currentUserId ? m.Receiver : m.Sender)
            .Select(g =>
            {
                var lastMessage = g.OrderByDescending(m => m.SentDate).FirstOrDefault();
                var unreadCount = g.Count(m => m.ReceiverId == currentUserId && !m.IsRead);

                return new MessageThreadViewModel
                {
                    UserId = g.Key.Id,
                    UserName = g.Key.UserName,
                    LastMessage = lastMessage?.Content,
                    LastMessageDate = lastMessage?.SentDate ?? DateTime.MinValue,
                    JobId = jobId,
                    ReceiverId = g.Key.Id,
                    OwnerId = currentUserId,
                    
                };
            });

        // Arama varsa filtrele
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var lowered = searchTerm.ToLower();
            threads = threads
                .Where(t =>
                {
                    var allMessagesForUser = messages.Where(m =>
                        (m.SenderId == currentUserId && m.ReceiverId == t.UserId) ||
                        (m.ReceiverId == currentUserId && m.SenderId == t.UserId));

                    return (!string.IsNullOrEmpty(t.UserName) && t.UserName.ToLower().Contains(lowered)) ||
                           allMessagesForUser.Any(m => !string.IsNullOrEmpty(m.Content) && m.Content.ToLower().Contains(lowered));
                });
        }

        ViewBag.SearchTerm = searchTerm;

        var viewModel = new ChatViewModel
        {
            Job = job,
            OwnerId = currentUserId,
            JobId = jobId,
            Threads = threads.OrderByDescending(t => t.LastMessageDate).ToList()
        };

        return View("JobMessageThreads", viewModel);
    }


}