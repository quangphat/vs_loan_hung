﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VietStar.Entities.Profile;

namespace VietStar.Entities.Infrastructures
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<ProfileAdd, ProfileAddSql>()
               .ForMember(a => a.ID, b => b.MapFrom(c => c.Id))
               .ForMember(a => a.Ma_Ho_So, b => b.MapFrom(c => c.Code))
               .ForMember(a => a.Ten_Khach_Hang, b => b.MapFrom(c => c.CustomerName))
               .ForMember(a => a.CMND, b => b.MapFrom(c => c.Cmnd))
               .ForMember(a => a.CMNDDay, b => b.MapFrom(c => c.CmndDay))
               .ForMember(a => a.BirthDay, b => b.MapFrom(c => c.BirthDay))
               .ForMember(a => a.Dia_Chi, b => b.MapFrom(c => c.Address))
               .ForMember(a => a.Ma_Khu_Vuc, b => b.MapFrom(c => c.DistrictId))
               .ForMember(a => a.SDT, b => b.MapFrom(c => c.Phone))
               .ForMember(a => a.Gioi_Tinh, b => b.MapFrom(c => c.Gender))
               .ForMember(a => a.SDT2, b => b.MapFrom(c => c.SalePhone))
               .ForMember(a => a.Ngay_Nhan_Don, b => b.MapFrom(c => c.ReceiveDate))
               .ForMember(a => a.San_Pham_Vay, b => b.MapFrom(c => c.ProductId))
               .ForMember(a => a.Co_Bao_Hiem, b => b.MapFrom(c => c.IsInsurrance))
               .ForMember(a => a.So_Tien_Vay, b => b.MapFrom(c => c.LoanAmount))
               .ForMember(a => a.Ma_Trang_Thai, b => b.MapFrom(c => c.Status))
               .ForMember(a => a.Han_Vay, b => b.MapFrom(c => c.Period))
               .ForMember(a => a.Ghi_Chu, b => b.MapFrom(c => c.Comment))
               .ForMember(a => a.Courier_Code, b => b.MapFrom(c => c.CourierId));

        }
    }
}
