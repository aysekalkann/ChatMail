using ChatMail.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ChatMail.Context
{
    public class MailContext : IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=LAPTOP-UDEPILDO\\SQLEXPRESS;Initial Catalog=MailChatDb;Integrated Security=true;Trust Server Certificate=true");
        }

        public DbSet<Message> Messages { get; set; }

        public List<Message> GetInboxMessages(string userName)
        {
            var user = Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return new List<Message>();

            return Messages
                .Where(m => m.ReceiverEmail == user.Email && m.IsRead == true)
                .OrderByDescending(m => m.SendDate)
                .ToList();
        }

        public List<Message> GetSendboxMessages(string userName)
        {
            var user = Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) return new List<Message>();

            return Messages
                .Where(m => m.SenderEmail == user.Email)
                .OrderByDescending(m => m.SendDate)
                .ToList();
        }
    }
}
