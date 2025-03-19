
using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using RepositoryLayer.Entity;


//namespace RepositoryLayer.Interface
//{
//    public interface IAddressBookRL
//    {

//        bool AddContact(AddressBookEntity contact, int userId);
//        List<AddressBookEntity> GetAllContacts();
//        AddressBookEntity GetContactById(int id);
//        bool UpdateContact(int id, AddressBookEntity updatedContact, int userId);
//        bool DeleteContact(int id);
//    }
//}

using System.Collections.Generic;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IAddressBookRL
    {
        // Adds a new contact to the address book.
        bool AddContact(AddressBookEntity contact, int userId);

        // Retrieves all contacts for a specific user.
        List<AddressBookEntity> GetAllContacts(int userId);

        // Retrieves a contact by its ID.
        AddressBookEntity GetContactById(int id);

        // Updates an existing contact in the address book.
        bool UpdateContact(int id, AddressBookEntity updatedContact, int userId);

        // Deletes a contact from the address book.
        bool DeleteContact(int id, int userId);
    }
}
