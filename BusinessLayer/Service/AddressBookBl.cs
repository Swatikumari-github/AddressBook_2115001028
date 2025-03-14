using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class AddressBookBL : IAddressBookBL
    {

        private readonly IAddressBookRL _addressBookRL;

        public AddressBookBL(IAddressBookRL addressBookRL)
        {
            _addressBookRL = addressBookRL;
        }

        public bool AddContact(AddressBookEntity contact)
        {
            return _addressBookRL.AddContact(contact);
        }

        public List<AddressBookEntity> GetAllContacts()
        {
            return _addressBookRL.GetAllContacts();
        }

        public AddressBookEntity GetContactById(int id)
        {
            return _addressBookRL.GetContactById(id);
        }

        public bool UpdateContact(int id, AddressBookEntity updatedContact)
        {
            return _addressBookRL.UpdateContact(id, updatedContact);
        }

        public bool DeleteContact(int id)
        {
            return _addressBookRL.DeleteContact(id);
        }
    }
}