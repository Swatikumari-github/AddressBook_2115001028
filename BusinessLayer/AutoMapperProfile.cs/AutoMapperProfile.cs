using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ModelLayer.DTO;
using RepositoryLayer.Entity;

namespace BusinessLayer.AutoMapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddressBookEntity, AddressBookDTO>().ReverseMap();
            CreateMap<UsersEntity, RegisterDTO>().ReverseMap();
            CreateMap<LoginDTO, UsersEntity>();

        }
    }
}