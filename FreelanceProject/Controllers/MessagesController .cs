using FreelanceProject.Data.Context;
using FreelanceProject.Data.Entities;
using FreelanceProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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



    //[Authorize]
    //[HttpGet]
    //public async Task<IActionResult> ViewChat(Guid receiverId, Guid jobId)
    //{
    //    var ownerId = Guid.Parse(_userManager.GetUserId(User)); // Giriş yapan kullanıcı

    //    var job = await _context.Jobs
    //        .Include(j => j.Owner)
    //        .FirstOrDefaultAsync(j => j.Id == jobId);

    //    if (job == null)
    //        return NotFound();

    //    if (job.OwnerId != ownerId)
    //        return Unauthorized();

    //    var messages = await _context.Messages
    //        .Where(m => m.JobId == jobId &&
    //               (
    //                   (m.SenderId == ownerId && m.ReceiverId == receiverId) ||
    //                   (m.SenderId == receiverId && m.ReceiverId == ownerId)
    //               ))
    //        .Include(m => m.Sender)
    //        .Include(m => m.Receiver)
    //        .OrderBy(m => m.SentDate)
    //        .ToListAsync();

    //    var model = new ChatViewModel
    //    {
    //        Job = job,
    //        Messages = messages,
    //        NewMessageContent = "",

    //        // ownerId'yi View'a taşıyoruz
    //        ReceiverId = receiverId,
    //        OwnerId = ownerId
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




    //////// Yeni mesaj gönderme işlemi
    //////[HttpPost]
    //////[Route("SendMessage/{jobId}")] // Özel route tanımlaması
    //////public async Task<IActionResult> SendMessage(Guid jobId, string newMessageContent)
    //////{
    //////    if (string.IsNullOrEmpty(newMessageContent))
    //////    {
    //////        // Eğer mesaj boş ise, hata mesajı gösterilebilir.
    //////        return RedirectToAction("ViewChat", new { jobId });
    //////    }

    //////    var senderId = _userManager.GetUserId(User);
    //////    var job = await _context.Jobs
    //////        .FirstOrDefaultAsync(j => j.Id == jobId);

    //////    if (job == null)
    //////    {
    //////        return NotFound();
    //////    }

    //////    var receiverId = job.OwnerId; // İşin sahibine mesaj gönderiliyor

    //////    var message = new MessageEntity
    //////    {
    //////        Content = newMessageContent,
    //////        SentDate = DateTime.UtcNow,
    //////        SenderId = Guid.Parse(senderId),
    //////        ReceiverId = receiverId,
    //////        JobId = jobId,
    //////        IsRead = false
    //////    };

    //////    _context.Messages.Add(message);
    //////    await _context.SaveChangesAsync();

    //////    // Mesaj gönderildikten sonra yeniden mesajlar sayfasına dön
    //////    return RedirectToAction("ViewChat", new { jobId, receiverId });
    //////}

    ////[HttpPost]
    ////[Route("SendMessage/{jobId}/{receiverId}")] // Özel route tanımlaması
    ////public async Task<IActionResult> SendMessage(Guid jobId, Guid receiverId, string newMessageContent)
    ////{
    ////    if (string.IsNullOrEmpty(newMessageContent))
    ////    {
    ////        // Eğer mesaj boş ise, hata mesajı gösterilebilir.
    ////        return RedirectToAction("ViewChat", new { jobId, receiverId });
    ////    }

    ////    var senderId = _userManager.GetUserId(User);
    ////    var job = await _context.Jobs
    ////        .FirstOrDefaultAsync(j => j.Id == jobId);

    ////    if (job == null)
    ////    {
    ////        return NotFound();
    ////    }

    ////    // Alıcıyı ve göndericiyi belirleyin
    ////    var message = new MessageEntity
    ////    {
    ////        Content = newMessageContent,
    ////        SentDate = DateTime.UtcNow,
    ////        SenderId = Guid.Parse(senderId),
    ////        ReceiverId = receiverId,  // Dinamik olarak alıcıyı burada belirliyoruz
    ////        JobId = jobId,
    ////        IsRead = false
    ////    };

    ////    _context.Messages.Add(message);
    ////    await _context.SaveChangesAsync();

    ////    // Mesaj gönderildikten sonra doğru sohbeti görüntüle
    ////    return RedirectToAction("ViewChat", new { jobId, receiverId });
    ////}


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


    [Authorize]
    [HttpGet("JobMessageThreads/{jobId}")]
    public async Task<IActionResult> JobMessageThreads(Guid jobId)
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

        var threads = messages
            .GroupBy(m => m.SenderId == currentUserId ? m.Receiver : m.Sender)
            .Select(g => new MessageThreadViewModel
            {
                UserId = g.Key.Id,
                UserName = g.Key.UserName,
                LastMessage = g.OrderByDescending(m => m.SentDate).FirstOrDefault()?.Content,
                LastMessageDate = g.Max(m => m.SentDate),
                JobId = jobId,
                ReceiverId = g.Key.Id,
                OwnerId = currentUserId
            })
            .ToList();

        var viewModel = new ChatViewModel
        {
            Job = job,
            OwnerId = currentUserId,
            JobId = jobId,
            Threads = threads
        };

        return View("JobMessageThreads", viewModel);
    }


}