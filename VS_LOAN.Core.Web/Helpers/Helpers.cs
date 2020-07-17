using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Utility;

namespace VS_LOAN.Core.Web.Helpers
{
    public static class Helpers
    {
        //public static string GetLimitStatusString()
        //{
        //    return ((int)TrangThaiHoSo.TuChoi).ToString() + "," 
        //        + ((int)TrangThaiHoSo.NhapLieu).ToString() + "," 
        //        + ((int)TrangThaiHoSo.ThamDinh).ToString() + ","
        //        + ((int)TrangThaiHoSo.BoSungHoSo).ToString() + "," 
        //        + ((int)TrangThaiHoSo.GiaiNgan).ToString() + "," 
        //        + ((int)TrangThaiHoSo.Nhap).ToString();
        //}
        public static string GetAllStatusString()
        {
            return ((int)TrangThaiHoSo.TuChoi).ToString() + ","
                    + ((int)TrangThaiHoSo.NhapLieu).ToString() + ","
                    + ((int)TrangThaiHoSo.ThamDinh).ToString() + ","
                    + ((int)TrangThaiHoSo.BoSungHoSo).ToString() + ","
                    + ((int)TrangThaiHoSo.Cancel).ToString() + ","
                    + ((int)TrangThaiHoSo.DaDoiChieu).ToString() + ","
                    +  "19,"
                    + ((int)TrangThaiHoSo.PCB).ToString() + ","
                    + ((int)TrangThaiHoSo.GiaiNgan).ToString();
        }
    }
}