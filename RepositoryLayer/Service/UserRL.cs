
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {

        private readonly UserContext _context;

        public UserRL(UserContext context)
        {
            _context = context;
        }

        public bool Register(UsersEntity userEntity)
        {
            _context.Users.Add(userEntity);
            return _context.SaveChanges() > 0;
        }

        public UsersEntity Login(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}

