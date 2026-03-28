using Core.Contracts;
using Infrastructure.Data;
using Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;

        public ContactService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveMessageAsync(string name, string email, string message)
        {
            var contactMessage = new ContactMessage
            {
                Name = name,
                Email = email,
                Message = message,
                CreatedOn = DateTime.UtcNow
            };

            _context.ContactMessages.Add(contactMessage);
            await _context.SaveChangesAsync();
        }
    }
}
