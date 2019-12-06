using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;

namespace VS_LOAN.Core.Business
{
    public  class BusinessExtension
    {
        public static string GetFilesMissingV2(List<LoaiTaiLieuModel> loaiTailieus, List<int> uploadedKeyIds)
        {
            if (loaiTailieus == null || !loaiTailieus.Any())
                return string.Empty;
            var fileRequires = loaiTailieus.Where(p => p.BatBuoc == 1).ToList();
            if (fileRequires == null || !fileRequires.Any())
                return string.Empty;
            if (uploadedKeyIds == null || !uploadedKeyIds.Any())
            {
                return fileRequires.FirstOrDefault().Ten;
            }
            var missing = fileRequires.Select(p => p.ID).Except(uploadedKeyIds).ToList();
            if (missing == null || !missing.Any())
                return string.Empty;
            return fileRequires.Where(p => p.ID == missing[0]).FirstOrDefault().Ten;
        }
        public static void ProcessPaging(ref int page, ref int limit)
        {
            page = page <= 0 ? 1 : page;
            limit = (limit <= 0 || limit >= 150) ? 150 : limit;
        }
    }
}
