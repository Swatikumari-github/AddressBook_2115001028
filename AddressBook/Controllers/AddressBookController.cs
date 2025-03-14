using AutoMapper;
using BusinessLayer.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace AddressBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressBookController : ControllerBase
    {
        private readonly IAddressBookBL _addressBookBL;
        private readonly IMapper _mapper;
        private readonly IValidator<AddressBookDTO> _validator;

        public AddressBookController(IAddressBookBL addressBookBL, IMapper mapper, IValidator<AddressBookDTO> validator)
        {
            _addressBookBL = addressBookBL;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("AddressBook route is working.");
        }

        [HttpPost("AddContact")]
        public IActionResult AddContact([FromBody] AddressBookDTO addressBookDTO)
        {
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

            var entity = _mapper.Map<AddressBookEntity>(addressBookDTO);
            var result = _addressBookBL.AddContact(entity);
            return Ok(new ResponseModel<bool>
            {
                Success = result,
                Message = result ? "Contact added successfully" : "Failed to add contact",
                Data = result
            });
        }

        [HttpGet("GetAllContacts")]
        public IActionResult GetAllContacts()
        {
            var data = _addressBookBL.GetAllContacts();
            var result = _mapper.Map<List<AddressBookDTO>>(data);
            return Ok(new ResponseModel<List<AddressBookDTO>>
            {
                Success = true,
                Message = "Contacts fetched successfully",
                Data = result
            });
        }

        [HttpGet("GetContactById/{id}")]
        public IActionResult GetContactById(int id)
        {
            var data = _addressBookBL.GetContactById(id);
            var result = _mapper.Map<AddressBookDTO>(data);
            return Ok(new ResponseModel<AddressBookDTO>
            {
                Success = result != null,
                Message = result != null ? "Contact found" : "Contact not found",
                Data = result
            });
        }

        [HttpPut("UpdateContact/{id}")]
        public IActionResult UpdateContact(int id, [FromBody] AddressBookDTO addressBookDTO)
        {
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

            var entity = _mapper.Map<AddressBookEntity>(addressBookDTO);
            var result = _addressBookBL.UpdateContact(id, entity);
            return Ok(new ResponseModel<bool>
            {
                Success = result,
                Message = result ? "Contact updated successfully" : "Failed to update contact",
                Data = result
            });
        }

        [HttpDelete("DeleteContact/{id}")]
        public IActionResult DeleteContact(int id)
        {
            var result = _addressBookBL.DeleteContact(id);
            return Ok(new ResponseModel<bool>
            {
                Success = result,
                Message = result ? "Contact deleted successfully" : "Failed to delete contact",
                Data = result
            });
        }
    }
}
