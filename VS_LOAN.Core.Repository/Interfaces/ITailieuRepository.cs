﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface ITailieuRepository
    {
        Task<bool> UpdateExistingFile(int fileId, string name, string url, int typeId = 1);
        Task<List<LoaiTaiLieuModel>> GetLoaiTailieuList(int profileType = 0);
        Task<bool> RemoveAllTailieu(int hosoId, int typeId);
        Task<bool> Add(TaiLieu model);
    }
}
