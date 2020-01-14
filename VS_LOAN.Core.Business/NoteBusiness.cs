using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.Model;

namespace VS_LOAN.Core.Business
{
    public class NoteBusiness:BaseBusiness
    {
        public NoteBusiness() : base(typeof(NoteBusiness)) { }
        public async Task AddNote(GhichuModel model)
        {
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("insert into Ghichu (UserId,Noidung,HosoId, CommentTime,TypeId) values(@userId,@noidung,@hosoId,@commentTime,@typeId)",
                    new
                    {
                        userId = model.UserId,
                        noidung = model.Noidung,
                        hosoId = model.HosoId,
                        commentTime = DateTime.Now,
                        typeId = model.TypeId
                    }, commandType: CommandType.Text);
            }
        }
    }
}
