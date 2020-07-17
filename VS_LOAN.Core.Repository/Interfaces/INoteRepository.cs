using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.Model;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface INoteRepository
    {
        Task AddNoteAsync(GhichuModel model);
        Task<List<GhichuViewModel>> GetNoteByTypeAsync(int id, int typeId);
    }
}
