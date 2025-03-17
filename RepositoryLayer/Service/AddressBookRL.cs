


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;

namespace RepositoryLayer
{
    public class AddressBookRL : IAddressBookRL
    {
        private readonly UserContext _context;

        public AddressBookRL(UserContext context)
        {
            _context = context;
        }

        public bool AddContact(AddressBookEntity contact, int userId)
        {
            contact.UserId = userId;
            _context.AddressBookEntries.Add(contact);
            _context.SaveChanges();
            return true;
        }

        public List<AddressBookEntity> GetAllContacts()
        {
            return _context.AddressBookEntries.ToList();
        }

        public AddressBookEntity GetContactById(int id)
        {
            return _context.AddressBookEntries.FirstOrDefault(c => c.Id == id);
        }

        public bool UpdateContact(int id, AddressBookEntity contact, int userId)
        {
            var existingContact = _context.AddressBookEntries
                                           .FirstOrDefault(c => c.Id == id && c.UserId == userId);

            if (existingContact != null)
            {
                existingContact.Name = contact.Name;
                existingContact.Email = contact.Email;
                existingContact.Phone = contact.Phone;

                _context.SaveChanges();
                return true;
            }
            return false;
        }


        public bool DeleteContact(int id)
        {
            var contact = _context.AddressBookEntries.Find(id);
            if (contact != null)
            {
                _context.AddressBookEntries.Remove(contact);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
