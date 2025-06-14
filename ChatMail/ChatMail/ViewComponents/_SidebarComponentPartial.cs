using ChatMail.Context;
using ChatMail.Entities;
using ChatMail.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatMail.ViewComponents
{
    public class _SidebarComponentPartial : ViewComponent
    {
        private readonly MailContext _context;
        private readonly UserManager<AppUser> _userManager;

        public _SidebarComponentPartial(MailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var model = new MessageViewModel
            {
                ReceiveMessage = _context.Messages.Count(x => x.ReceiverEmail == user.Email && x.IsRead == true),
                SendMessage = _context.Messages.Count(x => x.SenderEmail == user.Email && x.IsRead == true),
                DeleteMessage = _context.Messages.Count(x => x.IsRead == false &&
                    (x.ReceiverEmail == user.Email || x.SenderEmail == user.Email))
            };

            return View(model);
        }
    }
}
