﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VietStar.Entities.CheckDup;
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
               .ForMember(a => a.Ho_So_Cua_Ai, b => b.MapFrom(c => c.SaleId))
               .ForMember(a => a.Courier_Code, b => b.MapFrom(c => c.CourierId));

            CreateMap<ProfileDetail, ProfileEditView>()
               .ForMember(a => a.Id, b => b.MapFrom(c => c.ID))
               .ForMember(a => a.Code, b => b.MapFrom(c => c.Ma_Ho_So))
               .ForMember(a => a.CustomerName, b => b.MapFrom(c => c.Ten_Khach_Hang))
               .ForMember(a => a.Cmnd, b => b.MapFrom(c => c.CMND))
               .ForMember(a => a.CmndDay, b => b.MapFrom(c => c.CMNDDay))
               .ForMember(a => a.BirthDay, b => b.MapFrom(c => c.BirthDay))
               .ForMember(a => a.Address, b => b.MapFrom(c => c.Dia_Chi))
               .ForMember(a => a.DistrictId, b => b.MapFrom(c => c.Ma_Khu_Vuc))
               .ForMember(a => a.ProvinceId, b => b.MapFrom(c => c.ProvinceId))
               .ForMember(a => a.Phone, b => b.MapFrom(c => c.SDT))
               .ForMember(a => a.Gender, b => b.MapFrom(c => c.Gioi_Tinh))
               .ForMember(a => a.SalePhone, b => b.MapFrom(c => c.SDT2))
               .ForMember(a => a.ReceiveDate, b => b.MapFrom(c => c.Ngay_Nhan_Don))
               .ForMember(a => a.ProductId, b => b.MapFrom(c => c.San_Pham_Vay))
               .ForMember(a => a.PartnerId, b => b.MapFrom(c => c.PartnerId))
               .ForMember(a => a.IsInsurrance, b => b.MapFrom(c => c.Courier_Code))
               .ForMember(a => a.LoanAmount, b => b.MapFrom(c => c.So_Tien_Vay))
               .ForMember(a => a.Status, b => b.MapFrom(c => c.Ma_Trang_Thai))
               .ForMember(a => a.Period, b => b.MapFrom(c => c.Han_Vay))
               .ForMember(a => a.Comment, b => b.MapFrom(c => c.Ghi_Chu))
               .ForMember(a => a.SaleId, b => b.MapFrom(c => c.Ho_So_Cua_Ai))
               .ForMember(a => a.CourierId, b => b.MapFrom(c => c.Courier_Code));
            CreateMap<CheckDupAddModel, CheckDupAddSql>();


        }
    }
}
