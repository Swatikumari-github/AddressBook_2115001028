using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IAddressBookBL
    {
        bool AddContact(AddressBookEntity contact);
        List<AddressBookEntity> GetAllContacts();
        AddressBookEntity GetContactById(int id);
        bool UpdateContact(int id, AddressBookEntity contact);
        bool DeleteContact(int id);
    }
}
