using ChatMail.Context;
using ChatMail.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatMail.Controllers
{
    public class MessageController : Controller
    {
        private readonly MailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MessageController(MailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Gelen Kutusu
        public async Task<IActionResult> Inbox(string search)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            string userEmail = user?.Email ?? "";

            var messages = await _context.Messages
                .Where(m => m.ReceiverEmail == userEmail)
                .ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                messages = messages
                    .Where(m => m.Subject != null && m.Subject.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewBag.SearchTerm = search;
            return View(messages);
        }

        // Gönderilen Kutusu
    public async Task<IActionResult> Sendbox(string search)
{
    var user = await _userManager.FindByNameAsync(User.Identity.Name);
    if (user == null)
        return RedirectToAction("Login", "Account"); // Kullanıcı yoksa yönlendir

    string userEmail = user.Email;

    // Gönderilen mesajları çekiyoruz (IsRead filtresi burada ihtiyaca göre kaldırılabilir)
    var sendMessages = _context.GetSendboxMessages(user.UserName);

    if (!string.IsNullOrEmpty(search))
    {
        string term = search.ToLower();
        sendMessages = sendMessages
            .Where(m => (m.Subject != null && m.Subject.ToLower().Contains(term))
                     || (m.ReceiverEmail != null && m.ReceiverEmail.ToLower().Contains(term))
                     || (m.MessageDetail != null && m.MessageDetail.ToLower().Contains(term)))
            .ToList();
    }

    ViewBag.SearchTerm = search;

    return View(sendMessages);
}





        // Yeni Mesaj Oluşturma (GET)
        [HttpGet]
        public IActionResult CreateMessage()
        {
            return View();
        }

        // Yeni Mesaj Oluşturma (POST)
        [HttpPost]
        public async Task<IActionResult> CreateMessage(Message message)
        {
            if (!ModelState.IsValid)
            {
                return View(message);
            }

            var sender = await _userManager.FindByNameAsync(User.Identity.Name);
            message.SenderEmail = sender.Email;
            message.SendDate = DateTime.Now;
            message.IsRead = false;

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            ViewBag.Success = "Mesaj başarılı bir şekilde gönderildi.";
            ModelState.Clear();
            return View(new Message());
        }

        // Mesaj Detayları
        public async Task<IActionResult> MessageDetail(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            string userEmail = user?.Email ?? "";

            var message = await _context.Messages.FirstOrDefaultAsync(x => x.MessageId == id);

            if (message == null || (message.ReceiverEmail != userEmail && message.SenderEmail != userEmail))
            {
                return NotFound();
            }

            // Mesajı okunmuş yap
            if (message.ReceiverEmail == userEmail && !message.IsRead)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }

            ViewBag.ProfileImage = user.ProfileImageUrl;
            return View(message);
        }

        // Mesajları Okundu/Okunmadı olarak değiştir
        [HttpPost]
        public async Task<IActionResult> ChangeMessageStatus(List<int> messageIdList)
        {
            if (messageIdList != null && messageIdList.Count > 0)
            {
                foreach (var id in messageIdList)
                {
                    var message = await _context.Messages.FindAsync(id);
                    if (message != null)
                    {
                        message.IsRead = false; // Okunmamış yapıyor, istersen true da yapabilirsin
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("UnreadMessage");
        }

        // Okunmamış Mesajlar
        public async Task<IActionResult> UnreadMessage()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            string userEmail = user?.Email ?? "";

            var unreadMessages = await _context.Messages
                .Where(x => (x.SenderEmail == userEmail || x.ReceiverEmail == userEmail) && x.IsRead == false)
                .ToListAsync();

            return View(unreadMessages);
        }
    }
}
