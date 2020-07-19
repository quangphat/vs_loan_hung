using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Infrastructures
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            //CreateMap<Nhanvien, Account>()
            //    .ForMember(a => a.Id, b => b.MapFrom(c => c.ID))
            //    .ForMember(a => a.UserName, b => b.MapFrom(c => c.Ten_Dang_Nhap))
            //    .ForMember(a => a.Code, b => b.MapFrom(c => c.Ma))
            //    .ForMember(a => a.Email, b => b.MapFrom(c => c.Email))
            //    .ForMember(a => a.FullName, b => b.MapFrom(c => c.Ho_Ten));
           
        }
    }
}
