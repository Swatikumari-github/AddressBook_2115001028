﻿
using BussinessLayer.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using ModelLayer.Model;

namespace AddressBook.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserBL _userBL;
        private readonly IValidator<RegisterDTO> _validator;

        public AuthController(IUserBL userBL, IValidator<RegisterDTO> validator)
        {
            _userBL = userBL;
            _validator = validator;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO registerDTO)
        {
            var validationResult = _validator.Validate(registerDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Data = validationResult.Errors
                });
            }

            var isRegistered = _userBL.Register(registerDTO);

            // Step 3: Return appropriate response based on result
            if (!isRegistered)
            {
                return Conflict(new ResponseModel<string>
                {
                    Success = false,
                    Message = "User already exists",
                    Data = null
                });
            }

            return Ok(new ResponseModel<bool>
            {
                Success = true,
                Message = "User registered successfully",
                Data = true
            });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO userLoginDTO)
        {
            var token = _userBL.Login(userLoginDTO);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid credentials",
                    Data = null
                });
            }

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Login successful",
                Data = token
            });
        }
        //uc6 ka kaam
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDTO)
        {
            var result = _userBL.SendPasswordResetEmail(forgotPasswordDTO.Email);
            if (!result)
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Email not found",
                    Data = null
                });
            }

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Password reset email sent successfully",
                Data = null
            });
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            var result = _userBL.ResetPassword(resetPasswordDTO);
            if (!result)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid token or expired",
                    Data = null
                });
            }

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Password reset successfully",
                Data = null
            });
        }

    }
}

