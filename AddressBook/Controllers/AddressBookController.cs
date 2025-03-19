

//using AutoMapper;
//using BusinessLayer.Interface;
//using BusinessLayer.Service;
//using FluentValidation;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using ModelLayer.DTO;
//using ModelLayer.Model;
//using RepositoryLayer.Entity;

//namespace AddressBook.Controllers
//{

//    [Route("api/[controller]")]
//    [ApiController]
//    public class AddressBookController : ControllerBase
//    {


//        private readonly IAddressBookBL _addressBookBL;
//        private readonly IMapper _mapper;
//        private readonly IValidator<AddressBookDTO> _validator;

//        public AddressBookController(IAddressBookBL addressBookBL, IMapper mapper, IValidator<AddressBookDTO> validator)
//        {
//            _addressBookBL = addressBookBL;
//            _mapper = mapper;
//            _validator = validator;
//        }
//        [HttpGet]
//        public IActionResult Get()
//        {
//            return Ok("AddressBook route is working.");
//        }

//        [HttpPost("AddContact/{userId}")]
//        public IActionResult AddContact([FromBody] AddressBookDTO addressBookDTO, int userId)
//        {
//            // Validate DTO
//            var validationResult = _validator.Validate(addressBookDTO);
//            if (!validationResult.IsValid)
//            {
//                return BadRequest(new ResponseModel<object>
//                {
//                    Success = false,
//                    Message = "Validation failed",
//                    Data = validationResult.Errors
//                });
//            }

//            var result = _addressBookBL.AddContact(addressBookDTO, userId);

//            return Ok(new ResponseModel<bool>
//            {
//                Success = result,
//                Message = result ? "Contact added successfully" : "Failed to add contact",
//                Data = result
//            });
//        }



//        [HttpGet("GetAllContacts")]
//        public IActionResult GetAllContacts()
//        {
//            var data = _addressBookBL.GetAllContacts();
//            return Ok(new ResponseModel<List<AddressBookDTO>>
//            {
//                Success = true,
//                Message = "Contacts fetched successfully",
//                Data = data
//            });
//        }

//        [HttpGet("GetContactById/{id}")]
//        public IActionResult GetContactById(int id)
//        {
//            var data = _addressBookBL.GetContactById(id);
//            return Ok(new ResponseModel<AddressBookDTO>
//            {
//                Success = data != null,
//                Message = data != null ? "Contact found" : "Contact not found",
//                Data = data
//            });
//        }

//        [HttpPut("UpdateContact/{userId}")]
//        public IActionResult UpdateContact(int id, [FromBody] AddressBookDTO addressBookDTO, int userId)
//        {
//            var validationResult = _validator.Validate(addressBookDTO);
//            if (!validationResult.IsValid)
//            {
//                return BadRequest(new ResponseModel<object>
//                {
//                    Success = false,
//                    Message = "Validation failed",
//                    Data = validationResult.Errors
//                });
//            }

//            var result = _addressBookBL.UpdateContact(id, addressBookDTO, userId);
//            return Ok(new ResponseModel<bool>
//            {
//                Success = result,
//                Message = result ? "Contact updated successfully" : "Failed to update contact"
//            });
//        }


//        [HttpDelete("DeleteContact/{id}")]
//        public IActionResult DeleteContact(int id)
//        {
//            var result = _addressBookBL.DeleteContact(id);
//            return Ok(new ResponseModel<bool>
//            {
//                Success = result,
//                Message = result ? "Contact deleted successfully" : "Failed to delete contact"
//            });
//        }
//    }

//}
//using AutoMapper;
//using BusinessLayer.Interface;
//using FluentValidation;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using ModelLayer.DTO;
//using ModelLayer.Model;
//using RepositoryLayer.Entity;
//using System.Collections.Generic;

//namespace AddressBook.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AddressBookController : ControllerBase
//    {
//        private readonly IAddressBookBL _addressBookBL;
//        private readonly IMapper _mapper;
//        private readonly IValidator<AddressBookDTO> _validator;

//        public AddressBookController(IAddressBookBL addressBookBL, IMapper mapper, IValidator<AddressBookDTO> validator)
//        {
//            _addressBookBL = addressBookBL;
//            _mapper = mapper;
//            _validator = validator;
//        }

//        [HttpGet]
//        public IActionResult Get()
//        {
//            return Ok("AddressBook route is working.");
//        }

//        // Add a contact to the address book for a specific user
//        [HttpPost("AddContact/{userId}")]
//        public IActionResult AddContact([FromBody] AddressBookDTO addressBookDTO, int userId)
//        {
//            // Validate DTO
//            var validationResult = _validator.Validate(addressBookDTO);
//            if (!validationResult.IsValid)
//            {
//                return BadRequest(new ResponseModel<object>
//                {
//                    Success = false,
//                    Message = "Validation failed",
//                    Data = validationResult.Errors
//                });
//            }

//            var result = _addressBookBL.AddContact(addressBookDTO, userId);

//            return Ok(new ResponseModel<bool>
//            {
//                Success = result,
//                Message = result ? "Contact added successfully" : "Failed to add contact",
//                Data = result
//            });
//        }

//        // Get all contacts for a specific user
//        [HttpGet("GetAllContacts/{userId}")]
//        public IActionResult GetAllContacts(int userId)
//        {
//            var data = _addressBookBL.GetAllContacts(userId);
//            return Ok(new ResponseModel<List<AddressBookDTO>>
//            {
//                Success = true,
//                Message = "Contacts fetched successfully",
//                Data = data
//            });
//        }

//        // Get a contact by its ID
//        [HttpGet("GetContactById/{id}")]
//        public IActionResult GetContactById(int id)
//        {
//            var data = _addressBookBL.GetContactById(id);
//            return Ok(new ResponseModel<AddressBookDTO>
//            {
//                Success = data != null,
//                Message = data != null ? "Contact found" : "Contact not found",
//                Data = data
//            });
//        }

//        // Update an existing contact
//        [HttpPut("UpdateContact/{id}/{userId}")]
//        public IActionResult UpdateContact(int id, [FromBody] AddressBookDTO addressBookDTO, int userId)
//        {
//            // Validate DTO
//            var validationResult = _validator.Validate(addressBookDTO);
//            if (!validationResult.IsValid)
//            {
//                return BadRequest(new ResponseModel<object>
//                {
//                    Success = false,
//                    Message = "Validation failed",
//                    Data = validationResult.Errors
//                });
//            }

//            var result = _addressBookBL.UpdateContact(id, addressBookDTO, userId);
//            return Ok(new ResponseModel<bool>
//            {
//                Success = result,
//                Message = result ? "Contact updated successfully" : "Failed to update contact"
//            });
//        }

//        // Delete a contact from the address book for a specific user
//        [HttpDelete("DeleteContact/{id}/{userId}")]
//        public IActionResult DeleteContact(int id, int userId)
//        {
//            var result = _addressBookBL.DeleteContact(id, userId);
//            return Ok(new ResponseModel<bool>
//            {
//                Success = result,
//                Message = result ? "Contact deleted successfully" : "Failed to delete contact"
//            });
//        }
//    }
//}

using AutoMapper;
using BusinessLayer.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using System.Collections.Generic;
using RabitMQLayer.Producer;
namespace AddressBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressBookController : ControllerBase
    {
        private readonly IAddressBookBL _addressBookBL;
        private readonly IMapper _mapper;
        private readonly IValidator<AddressBookDTO> _validator;
        private readonly RabbitMQProducer _rabbitMQProducer;
        public AddressBookController(IAddressBookBL addressBookBL, IMapper mapper, IValidator<AddressBookDTO> validator, RabbitMQProducer rabbitMQProducer)
        {
            _addressBookBL = addressBookBL;
            _mapper = mapper;
            _validator = validator;
            _rabbitMQProducer = rabbitMQProducer;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("AddressBook route is working.");
        }

        // Add a contact to the address book for a specific user
        [HttpPost("AddContact/{userId}")]
        public IActionResult AddContact([FromBody] AddressBookDTO addressBookDTO, int userId)
        {
            // Validate DTO
            var validationResult = _validator.Validate(addressBookDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Data = validationResult.Errors
                });
            }

            var result = _addressBookBL.AddContact(addressBookDTO, userId);
            if (result)
            {
                var message = $"Contact added with name: {addressBookDTO.Name} for userId: {userId}";
                _rabbitMQProducer.Publish("addressBookQueue", message); // Publish to RabbitMQ
            }
            return Ok(new ResponseModel<bool>
            {
                Success = result,
                Message = result ? "Contact added successfully" : "Failed to add contact",
                Data = result
            });
        }

        // Get all contacts for a specific user
        [HttpGet("GetAllContacts/{userId}")]
        public IActionResult GetAllContacts(int userId)
        {
            var data = _addressBookBL.GetAllContacts(userId);
            return Ok(new ResponseModel<List<AddressBookDTO>>
            {
                Success = true,
                Message = "Contacts fetched successfully",
                Data = data
            });
        }

        // Get a contact by its ID
        [HttpGet("GetContactById/{id}")]
        public IActionResult GetContactById(int id)
        {
            var data = _addressBookBL.GetContactById(id);
            return Ok(new ResponseModel<AddressBookDTO>
            {
                Success = data != null,
                Message = data != null ? "Contact found" : "Contact not found",
                Data = data
            });
        }

        // Update an existing contact
        [HttpPut("UpdateContact/{id}/{userId}")]
        public IActionResult UpdateContact(int id, [FromBody] AddressBookDTO addressBookDTO, int userId)
        {
            // Validate DTO
            var validationResult = _validator.Validate(addressBookDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Data = validationResult.Errors
                });
            }

            var result = _addressBookBL.UpdateContact(id, addressBookDTO, userId);
            if (result)
            {
                var message = $"Contact updated with name: {addressBookDTO.Name} for userId: {userId}";
                _rabbitMQProducer.Publish("addressBookQueue", message); // Publish to RabbitMQ
            }
            return Ok(new ResponseModel<bool>
            {
                Success = result,
                Message = result ? "Contact updated successfully" : "Failed to update contact"
            });
        }

        // Delete a contact from the address book for a specific user
        [HttpDelete("DeleteContact/{id}/{userId}")]
        public IActionResult DeleteContact(int id, int userId)
        {
            var result = _addressBookBL.DeleteContact(id, userId);
            if (result)
            {
                var message = $"Contact deleted with id: {id} for userId: {userId}";
                _rabbitMQProducer.Publish("addressBookQueue", message); // Publish to RabbitMQ
            }
            return Ok(new ResponseModel<bool>
            {
                Success = result,
                Message = result ? "Contact deleted successfully" : "Failed to delete contact"
            });
        }
    }
}
