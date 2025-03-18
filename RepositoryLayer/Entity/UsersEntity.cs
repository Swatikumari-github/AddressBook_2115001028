
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class UsersEntity
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //uc6 code
        public string ? ResetToken { get; set; }
        public DateTime? ResetTokenExpiration { get; set; }

        // Navigation Property
        public ICollection<AddressBookEntity> AddressBookEntries { get; set; } = new List<AddressBookEntity>();
    }
}

