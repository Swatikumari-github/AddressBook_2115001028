
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
        //uc6 code
        public void SavePasswordResetToken(int userId, string resetToken)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                user.ResetToken = resetToken;
                user.ResetTokenExpiration = DateTime.UtcNow.AddHours(1); // Token valid for 1 hour
                _context.SaveChanges();
            }
        }

        public int? ValidateResetToken(string token)
        {
            var user = _context.Users.FirstOrDefault(u => u.ResetToken == token && u.ResetTokenExpiration > DateTime.UtcNow);
            if (user != null)
            {
                return user.UserId;
            }
            return null; // Invalid or expired token
        }

        public void RemoveResetToken(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                user.ResetToken = null;
                user.ResetTokenExpiration = null;
                _context.SaveChanges();
            }
        }

        public void UpdatePassword(UsersEntity user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public UsersEntity GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId);
        }


    }
}

