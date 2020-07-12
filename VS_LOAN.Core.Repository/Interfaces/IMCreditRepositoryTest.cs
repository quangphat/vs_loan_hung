using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Entity.MCreditModels;

namespace VS_LOAN.Core.Repository.Interfaces
{
    public interface IMCreditRepositoryTest
    {
        Task<GetFileUploadResponse> GetFilesNeedToUpload(GetFileUploadRequest model);
    }
}
