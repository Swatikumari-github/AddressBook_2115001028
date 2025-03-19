

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ModelLayer.DTO;
//using RepositoryLayer.Entity;

//namespace BusinessLayer.Interface
//{
//    public interface IAddressBookBL
//    {
//        bool AddContact(AddressBookDTO addressBookDTO, int userId);
//        List<AddressBookDTO> GetAllContacts();
//        AddressBookDTO GetContactById(int id);
//        bool UpdateContact(int id, AddressBookDTO addressBookDTO, int userId);
//        bool DeleteContact(int id);

//    }
//}

using System;
using System.Collections.Generic;
using ModelLayer.DTO;

namespace BusinessLayer.Interface
{
    public interface IAddressBookBL
    {
        // Adds a new contact to the address book for the given user
        bool AddContact(AddressBookDTO addressBookDTO, int userId);

        // Retrieves all contacts for a specific user
        List<AddressBookDTO> GetAllContacts(int userId);

        // Retrieves a contact by its ID
        AddressBookDTO GetContactById(int id);

        // Updates an existing contact in the address book
        bool UpdateContact(int id, AddressBookDTO addressBookDTO, int userId);

        // Deletes a contact from the address book for the given user
        bool DeleteContact(int id, int userId);
    }
}


