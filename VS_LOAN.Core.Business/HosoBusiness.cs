﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.UploadModel;

namespace VS_LOAN.Core.Business
{
    public class HosoBusiness : BaseBusiness
    {
        public async Task<List<FileUploadModel>> GetTailieuByHosoId(int hosoId)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("hosoId", hosoId);

                var result = await con.QueryAsync<FileUploadModel>("getTailieuByHosoId", p,
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }
        public async Task<bool> RemoveTailieu(int hosoId, int tailieuId)
        {
            var p = new DynamicParameters();
            p.Add("hosoId", hosoId);
            p.Add("tailieuId", tailieuId);
            using (var con = GetConnection())
            {
                var result = await con.ExecuteAsync("removeTailieu", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }

        }
    }
}