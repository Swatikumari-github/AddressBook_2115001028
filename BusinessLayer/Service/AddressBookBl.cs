

////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using AutoMapper;
////using BusinessLayer.Interface;
////using ModelLayer.DTO;
////using RepositoryLayer.Entity;
////using RepositoryLayer.Interface;

////namespace BusinessLayer.Service
////{
////    public class AddressBookBL : IAddressBookBL
////    {

////        private readonly IAddressBookRL _addressBookRL;
////        private readonly IMapper _mapper;
////        public AddressBookBL(IAddressBookRL addressBookRL, IMapper mapper)
////        {
////            _addressBookRL = addressBookRL;
////            _mapper = mapper;
////        }

////        public bool AddContact(AddressBookDTO addressBookDTO, int userId)
////        {
////            var entity = _mapper.Map<AddressBookEntity>(addressBookDTO);

////            return _addressBookRL.AddContact(entity, userId);
////        }

////        public List<AddressBookDTO> GetAllContacts()
////        {
////            var entityList = _addressBookRL.GetAllContacts();
////            return _mapper.Map<List<AddressBookDTO>>(entityList);
////        }


////        public AddressBookDTO GetContactById(int id)
////        {
////            var entity = _addressBookRL.GetContactById(id);
////            return _mapper.Map<AddressBookDTO>(entity);
////        }

////        public bool UpdateContact(int id, AddressBookDTO addressBookDTO, int userId)
////        {
////            var entity = _mapper.Map<AddressBookEntity>(addressBookDTO);
////            return _addressBookRL.UpdateContact(id, entity, userId);
////        }


////        public bool DeleteContact(int id)
////        {
////            return _addressBookRL.DeleteContact(id);
////        }
////    }
////}

//using System;
//using System.Collections.Generic;
//using AutoMapper;
//using BusinessLayer.Interface;
//using ModelLayer.DTO;
//using RepositoryLayer.Entity;
//using RepositoryLayer.Interface;

//namespace BusinessLayer.Service
//{
//    public class AddressBookBL : IAddressBookBL
//    {
//        private readonly IAddressBookRL _addressBookRL;
//        private readonly IMapper _mapper;

//        public AddressBookBL(IAddressBookRL addressBookRL, IMapper mapper)
//        {
//            _addressBookRL = addressBookRL;
//            _mapper = mapper;
//        }

//        // Add a new contact to the address book for the given user
//        public bool AddContact(AddressBookDTO addressBookDTO, int userId)
//        {
//            var entity = _mapper.Map<AddressBookEntity>(addressBookDTO);
//            return _addressBookRL.AddContact(entity, userId);
//        }

//        // Get all contacts for a specific user
//        public List<AddressBookDTO> GetAllContacts(int userId)
//        {
//            var entityList = _addressBookRL.GetAllContacts(userId);
//            return _mapper.Map<List<AddressBookDTO>>(entityList);
//        }

//        // Get a specific contact by its ID
//        public AddressBookDTO GetContactById(int id)
//        {
//            var entity = _addressBookRL.GetContactById(id);
//            return _mapper.Map<AddressBookDTO>(entity);
//        }

//        // Update an existing contact
//        public bool UpdateContact(int id, AddressBookDTO addressBookDTO, int userId)
//        {
//            var entity = _mapper.Map<AddressBookEntity>(addressBookDTO);
//            return _addressBookRL.UpdateContact(id, entity, userId);
//        }

//        // Delete a contact from the address book
//        public bool DeleteContact(int id, int userId)
//        {
//            return _addressBookRL.DeleteContact(id, userId);
//        }
//    }
//}



//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using AutoMapper;
//using BusinessLayer.Interface;
//using ModelLayer.DTO;
//using RepositoryLayer.Entity;
//using RepositoryLayer.Interface;

//namespace BusinessLayer.Service
//{
//    public class AddressBookBL : IAddressBookBL
//    {

//        private readonly IAddressBookRL _addressBookRL;
//        private readonly IMapper _mapper;
//        public AddressBookBL(IAddressBookRL addressBookRL, IMapper mapper)
//        {
//            _addressBookRL = addressBookRL;
//            _mapper = mapper;
//        }

//        public bool AddContact(AddressBookDTO addressBookDTO, int userId)
//        {
//            var entity = _mapper.Map<AddressBookEntity>(addressBookDTO);

//            return _addressBookRL.AddContact(entity, userId);
//        }

//        public List<AddressBookDTO> GetAllContacts()
//        {
//            var entityList = _addressBookRL.GetAllContacts();
//            return _mapper.Map<List<AddressBookDTO>>(entityList);
//        }


//        public AddressBookDTO GetContactById(int id)
//        {
//            var entity = _addressBookRL.GetContactById(id);
//            return _mapper.Map<AddressBookDTO>(entity);
//        }

//        public bool UpdateContact(int id, AddressBookDTO addressBookDTO, int userId)
//        {
//            var entity = _mapper.Map<AddressBookEntity>(addressBookDTO);
//            return _addressBookRL.UpdateContact(id, entity, userId);
//        }


//        public bool DeleteContact(int id)
//        {
//            return _addressBookRL.DeleteContact(id);
//        }
//    }
//}

using System;
using System.Collections.Generic;
using AutoMapper;
using BusinessLayer.Interface;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RabitMQLayer;
using RabbitMQ.Client;

namespace BusinessLayer.Service
{
    public class AddressBookBL : IAddressBookBL
    {
        private readonly IAddressBookRL _addressBookRL;
        private readonly IMapper _mapper;
        private readonly RabbitMQService _rabbitMQService;
        public AddressBookBL(IAddressBookRL addressBookRL, IMapper mapper, RabbitMQService rabbitMQService)
        {
            _addressBookRL = addressBookRL;
            _mapper = mapper;
            _rabbitMQService = rabbitMQService;
        }

        // Add a new contact to the address book for the given user
        public bool AddContact(AddressBookDTO addressBookDTO, int userId)
        {
            var entity = _mapper.Map<AddressBookEntity>(addressBookDTO);
            var result = _addressBookRL.AddContact(entity, userId);
            if (result)
            {
                // Publish a message to RabbitMQ about the new contact being added
                var message = $"New contact added by user {userId}: {addressBookDTO.Name} ({addressBookDTO.Email})";
                PublishMessageToQueue("contact-added-queue", message);  // Publish message to RabbitMQ queue
            }

            return result;
        }

        // Get all contacts for a specific user
        public List<AddressBookDTO> GetAllContacts(int userId)
        {
            var entityList = _addressBookRL.GetAllContacts(userId);
            return _mapper.Map<List<AddressBookDTO>>(entityList);
        }

        // Get a specific contact by its ID
        public AddressBookDTO GetContactById(int id)
        {
            var entity = _addressBookRL.GetContactById(id);
            return _mapper.Map<AddressBookDTO>(entity);
        }

        // Update an existing contact
        public bool UpdateContact(int id, AddressBookDTO addressBookDTO, int userId)
        {
            var entity = _mapper.Map<AddressBookEntity>(addressBookDTO);
            var result = _addressBookRL.UpdateContact(id, entity, userId);

            if (result)
            {
                // Publish a message to RabbitMQ about the contact update
                var message = $"Contact updated by user {userId}: {addressBookDTO.Name} ({addressBookDTO.Email})";
                PublishMessageToQueue("contact-updated-queue", message);  // Publish message to RabbitMQ queue
            }

            return result;
        }

        // Delete a contact from the address book
        public bool DeleteContact(int id, int userId)
        {
            var result = _addressBookRL.DeleteContact(id, userId);

            if (result)
            {
                // Publish a message to RabbitMQ about the contact deletion
                var message = $"Contact deleted by user {userId}: {id}";
                PublishMessageToQueue("contact-deleted-queue", message);  // Publish message to RabbitMQ queue
            }

            return result;
        }
        private void PublishMessageToQueue(string queueName, string message)
        {
            var channel = _rabbitMQService.GetChannel();
            var body = System.Text.Encoding.UTF8.GetBytes(message);

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
