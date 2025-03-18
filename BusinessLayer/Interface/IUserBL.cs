
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.DTO;
using RepositoryLayer.Entity;

namespace BussinessLayer.Interface
{
    public interface IUserBL
    {
        bool Register(RegisterDTO registerDTO);

        string Login(LoginDTO loginDTO);
        // Send Password Reset Email
        bool SendPasswordResetEmail(string email);

        // Reset Password using Token
        bool ResetPassword(ResetPasswordDTO resetPasswordDTO);


    }
}


