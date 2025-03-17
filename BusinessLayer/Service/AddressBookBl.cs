
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Interface;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class AddressBookBL : IAddressBookBL
    {

        private readonly IAddressBookRL _addressBookRL;
        private readonly IMapper _mapper;
        public AddressBookBL(IAddressBookRL addressBookRL, IMapper mapper)
        {
            _addressBookRL = addressBookRL;
            _mapper = mapper;
        }

        public bool AddContact(AddressBookDTO addressBookDTO, int userId)
        {
            var entity = _mapper.Map<AddressBookEntity>(addressBookDTO);

            return _addressBookRL.AddContact(entity, userId);
        }

        public List<AddressBookDTO> GetAllContacts()
        {
            var entityList = _addressBookRL.GetAllContacts();
            return _mapper.Map<List<AddressBookDTO>>(entityList);
        }


        public AddressBookDTO GetContactById(int id)
        {
            var entity = _addressBookRL.GetContactById(id);
            return _mapper.Map<AddressBookDTO>(entity);
        }

        public bool UpdateContact(int id, AddressBookDTO addressBookDTO, int userId)
        {
            var entity = _mapper.Map<AddressBookEntity>(addressBookDTO);
            return _addressBookRL.UpdateContact(id, entity, userId);
        }


        public bool DeleteContact(int id)
        {
            return _addressBookRL.DeleteContact(id);
        }
    }
}
