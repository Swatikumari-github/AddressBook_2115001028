



//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;
//using RepositoryLayer.Context;
//using RepositoryLayer.Entity;
//using RepositoryLayer;
//using RepositoryLayer.Interface;
//using RepositoryLayer.Service;

//namespace RepositoryLayer
//{
//    public class AddressBookRL : IAddressBookRL
//    {
//        private readonly UserContext _context;

//        public AddressBookRL(UserContext context)
//        {
//            _context = context;
//        }

//        public bool AddContact(AddressBookEntity contact, int userId)
//        {
//            contact.UserId = userId;
//            _context.AddressBookEntries.Add(contact);
//            _context.SaveChanges();
//            return true;
//        }

//        public List<AddressBookEntity> GetAllContacts()
//        {
//            return _context.AddressBookEntries.ToList();
//        }

//        public AddressBookEntity GetContactById(int id)
//        {
//            return _context.AddressBookEntries.FirstOrDefault(c => c.Id == id);
//        }

//        public bool UpdateContact(int id, AddressBookEntity contact, int userId)
//        {
//            var existingContact = _context.AddressBookEntries
//                                           .FirstOrDefault(c => c.Id == id && c.UserId == userId);

//            if (existingContact != null)
//            {
//                existingContact.Name = contact.Name;
//                existingContact.Email = contact.Email;
//                existingContact.Phone = contact.Phone;

//                _context.SaveChanges();
//                return true;
//            }
//            return false;
//        }


//        public bool DeleteContact(int id)
//        {
//            var contact = _context.AddressBookEntries.Find(id);
//            if (contact != null)
//            {
//                _context.AddressBookEntries.Remove(contact);
//                _context.SaveChanges();
//                return true;
//            }
//            return false;
//        }
//    }
//}






using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System.Text.Json;

namespace RepositoryLayer
{
    public class AddressBookRL : IAddressBookRL
    {
        private readonly UserContext _context;
        private readonly RedisCacheService _redisCache;
        private readonly ILogger<AddressBookRL> _logger;

        public AddressBookRL(UserContext context, RedisCacheService redisCache, ILogger<AddressBookRL> logger)
        {
            _context = context;
            _redisCache = redisCache;
            _logger = logger;
        }

        public bool AddContact(AddressBookEntity contact, int userId)
        {
            contact.UserId = userId;
            _context.AddressBookEntries.Add(contact);
            _context.SaveChanges();

            // Invalidate the cache for this user's contact list
            _redisCache.RemoveData($"contact_list_{contact.UserId}");
            _logger.LogInformation($"Cache invalidated for user {contact.UserId} contact list.");
            return true;
        }

        public List<AddressBookEntity> GetAllContacts(int userId)
        {
            // Try to get the contacts from Redis cache
            var cachedContacts = _redisCache.GetData($"contact_list_{userId}");
            if (!string.IsNullOrEmpty(cachedContacts))
            {
                _logger.LogInformation($"Cache hit for user {userId} contact list.");
                return JsonSerializer.Deserialize<List<AddressBookEntity>>(cachedContacts);
            }

            // If not found in cache, fetch from database
            var contacts = _context.AddressBookEntries.Where(c => c.UserId == userId).ToList();

            // Cache the contact list for the user for 10 minutes
            _redisCache.SetData($"contact_list_{userId}", JsonSerializer.Serialize(contacts), TimeSpan.FromMinutes(10));
            _logger.LogInformation($"Cache stored for user {userId} contact list.");
            return contacts;
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

                // Invalidate the cache for this user's contact list
                _redisCache.RemoveData($"contact_list_{userId}");
                _logger.LogInformation($"Cache invalidated for user {userId} contact list.");
                return true;
            }
            return false;
        }

        public bool DeleteContact(int id, int userId)
        {
            var contact = _context.AddressBookEntries.Find(id);
            if (contact != null)
            {
                _context.AddressBookEntries.Remove(contact);
                _context.SaveChanges();

                // Invalidate the cache for this user's contact list
                _redisCache.RemoveData($"contact_list_{userId}");
                _logger.LogInformation($"Cache invalidated for user {userId} contact list.");
                return true;
            }
            return false;
        }
    }
}
