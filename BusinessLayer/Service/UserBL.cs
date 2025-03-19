
//using System;
//using BussinessLayer.Interface;
//using ModelLayer.DTO;
//using RepositoryLayer.Entity;
//using RepositoryLayer.Interface;
//using System.Security.Claims;
//using AutoMapper;
//using System.IdentityModel.Tokens.Jwt;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using BusinessLayer.Service;
//using RabitMQLayer;
//namespace BussinessLayer.Service
//{
//    public class UserBL : IUserBL
//    {
//        private readonly IUserRL _userRL;
//        private readonly IMapper _mapper;
//        private readonly IConfiguration _configuration;
//        private readonly EmailService _emailService;

//        public UserBL(IUserRL userRL, IMapper mapper, IConfiguration configuration, EmailService emailService)
//        {
//            _userRL = userRL;
//            _mapper = mapper;
//            _configuration = configuration;
//            _emailService = emailService;

//        }


//        public bool Register(RegisterDTO userDTO)
//        {

//            var existingUser = _userRL.Login(userDTO.Email);
//            if (existingUser != null)
//            {
//                return false;
//            }


//            var userEntity = _mapper.Map<UsersEntity>(userDTO);
//            userEntity.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
//            return _userRL.Register(userEntity);
//        }


//        public string Login(LoginDTO loginDTO)
//        {
//            var user = _userRL.Login(loginDTO.Email);
//            if (user != null && BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))

//            {
//                return GenerateJwtToken(user.Email, user.UserId);
//            }
//            return null;
//        }

//        private string GenerateJwtToken(string email, int userId)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(new[]
//                {
//                new Claim(ClaimTypes.Email, email),
//                new Claim("UserId", userId.ToString())
//            }),
//                Expires = DateTime.UtcNow.AddHours(2),
//                Issuer = _configuration["Jwt:Issuer"],
//                Audience = _configuration["Jwt:Audience"],
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//            };

//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            return tokenHandler.WriteToken(token);
//        }
//        //uc6 code
//        public bool SendPasswordResetEmail(string email)
//        {
//            var user = _userRL.Login(email);
//            if (user == null) return false;

//            // Generate Reset Token
//            var resetToken = GenerateResetToken(user.UserId);

//            // Save token in database (or cache it with an expiration time)
//            _userRL.SavePasswordResetToken(user.UserId, resetToken);

//            // Send email with the reset token (You will need to implement an Email Service)
//            _emailService.SendPasswordResetEmail(email, resetToken);

//            return true;
//        }

//        public bool ResetPassword(ResetPasswordDTO resetPasswordDTO)
//        {
//            // Verify the reset token
//            var userId = _userRL.ValidateResetToken(resetPasswordDTO.Token);
//            if (userId == null) return false;

//            // Reset the password
//            var user = _userRL.GetUserById(userId.Value);
//            if (user == null) return false;

//            user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDTO.NewPassword);
//            _userRL.UpdatePassword(user);

//            // Optionally, remove the reset token from the database or mark it as used
//            _userRL.RemoveResetToken(user.UserId);

//            return true;
//        }

//        private string GenerateResetToken(int userId)
//        {
//            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
//            return token; // You may want to include expiration and more logic
//        }



//    }
//}


using System;
using BussinessLayer.Interface;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System.Security.Claims;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BusinessLayer.Service;
using RabitMQLayer;
using RabbitMQ.Client;
namespace BussinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private readonly RabbitMQService _rabbitMQService;
        public UserBL(IUserRL userRL, IMapper mapper, IConfiguration configuration, EmailService emailService, RabbitMQService rabbitMQService)
        {
            _userRL = userRL;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;
            _rabbitMQService = rabbitMQService;
        }


        public bool Register(RegisterDTO userDTO)
        {

            var existingUser = _userRL.Login(userDTO.Email);
            if (existingUser != null)
            {
                return false;
            }


            var userEntity = _mapper.Map<UsersEntity>(userDTO);
            userEntity.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
            var registrationSuccess = _userRL.Register(userEntity);

            if (registrationSuccess)
            {
                // Publish a message to RabbitMQ about the new user registration
                var message = $"New user registered: {userDTO.Email}";
                PublishMessageToQueue("user-registered-queue", message); // Publish message to queue

                // Optionally, send a welcome email
                //_emailService.SendWelcomeEmail(userDTO.Email);
            }

            return registrationSuccess;
        }


        public string Login(LoginDTO loginDTO)
        {
            var user = _userRL.Login(loginDTO.Email);
            if (user != null && BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))

            {
                return GenerateJwtToken(user.Email, user.UserId);
            }
            return null;
        }

        private string GenerateJwtToken(string email, int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Email, email),
                new Claim("UserId", userId.ToString())
            }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        //uc6 code
        public bool SendPasswordResetEmail(string email)
        {
            var user = _userRL.Login(email);
            if (user == null) return false;

            // Generate Reset Token
            var resetToken = GenerateResetToken(user.UserId);

            // Save token in database (or cache it with an expiration time)
            _userRL.SavePasswordResetToken(user.UserId, resetToken);

            // Send email with the reset token (You will need to implement an Email Service)
            _emailService.SendPasswordResetEmail(email, resetToken);
            var message = $"Password reset requested for: {email}";
            PublishMessageToQueue("password-reset-queue", message);
            return true;
        }

        public bool ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            // Verify the reset token
            var userId = _userRL.ValidateResetToken(resetPasswordDTO.Token);
            if (userId == null) return false;

            // Reset the password
            var user = _userRL.GetUserById(userId.Value);
            if (user == null) return false;

            user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDTO.NewPassword);
            _userRL.UpdatePassword(user);

            // Optionally, remove the reset token from the database or mark it as used
            _userRL.RemoveResetToken(user.UserId);
            var message = $"Password reset successful for: {user.Email}";
            PublishMessageToQueue("password-reset-success-queue", message); // Publish message to queue

            return true;
        }

        private string GenerateResetToken(int userId)
        {
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return token; // You may want to include expiration and more logic
        }

        private void PublishMessageToQueue(string queueName, string message)
        {
            var channel = _rabbitMQService.GetChannel();
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body
            );

            Console.WriteLine($"Message sent to {queueName}: {message}");
        }

    }
}

