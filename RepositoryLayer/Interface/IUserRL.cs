
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {

        //UsersEntity GetUserByEmail(string email);
        bool Register(UsersEntity user);
        UsersEntity Login(string email);

        //uc6 code
        void SavePasswordResetToken(int userId, string resetToken);

        // Validate Reset Token (Returns UserId if valid)
        int? ValidateResetToken(string token);

        // Remove Reset Token after successful reset
        void RemoveResetToken(int userId);

        // Update User Password
        void UpdatePassword(UsersEntity user);
        UsersEntity GetUserById(int userId);





    }
}

