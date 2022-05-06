using Application.DTO.User;
using AutoMapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles
{
    public class UserProfile :Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserAdd>().ReverseMap();
            CreateMap<User, UserView>().ReverseMap();
            CreateMap<User, UserLogin>().ReverseMap();
            CreateMap<User, UserFull>().ReverseMap();
            CreateMap<UserView, UserFull>().ReverseMap();

        }
    }
}
