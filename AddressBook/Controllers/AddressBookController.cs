using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;

namespace AddressBook.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AddressBookController : ControllerBase
    {


        private readonly IAddressBookBL _addressBookBL;

        public AddressBookController(IAddressBookBL addressBookBL)
        {
            _addressBookBL = addressBookBL;
        }

        [HttpPost("AddContact")]
        public IActionResult AddContact([FromBody] AddressBookEntity user)
        {
            bool result = _addressBookBL.AddContact(user);
            return Ok(new { success = result });
        }

        [HttpGet("GetAllContacts")]
        public IActionResult GetAllContacts()
        {
            var result = _addressBookBL.GetAllContacts();
            return Ok(new { success = result });
        }

        [HttpGet("GetContactById/{id}")]
        public IActionResult GetContactById(int id)
        {
            var result = _addressBookBL.GetContactById(id);
            return Ok(new { success = result });
        }

        [HttpPut("UpdateContact/{id}")]
        public IActionResult UpdateContact(int id, [FromBody] AddressBookEntity updatedContact)
        {
            var result = _addressBookBL.UpdateContact(id, updatedContact);
            return Ok(new { success = result });
        }

        [HttpDelete("DeleteContact/{id}")]
        public IActionResult DeleteContact(int id)
        {
            var result = _addressBookBL.DeleteContact(id);
            return Ok(new { success = result });
        }
    }
}
