using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Entity.MCreditModels;

namespace VS_LOAN.Core.Repository
{
    public class MCreditRepositoryTest : BaseRepository, IMCreditRepositoryTest
    {

        public MCreditRepositoryTest() : base(typeof(MCreditRepositoryTest))
        {
        }

        public Task<GetFileUploadResponse> GetFilesNeedToUpload(GetFileUploadRequest model)
        {
            var result = new GetFileUploadResponse();
            result.Groups = new List<GetFileUploadGroup> {
                new GetFileUploadGroup{
                    GroupId = 19,
                    GroupName ="Hộ khẩu",
                    Mandatory = true,
                    HasAlternate = false,
                    Documents = new List<GetFileUploadDocument>{
                        new GetFileUploadDocument{
                            Id = 58,
                            DocumentCode="FamilyBook",
                            DocumentName ="Sổ hộ khẩu",
                            MapBpmVar = "DOC_FamilyBook"
                        }
                    }
                },
                new GetFileUploadGroup{
                    GroupId = 20,
                    GroupName ="Giấy xác nhận cư trú  của thủ trưởng đơn vị",
                    Mandatory = false,
                    HasAlternate = false,
                    Documents = new List<GetFileUploadDocument>{
                        new GetFileUploadDocument{
                            Id = 17,
                            DocumentCode="ResidenceConfirmationOfHeadUnit",
                            DocumentName ="Giấy xác nhận cư trú của thủ trưởng đơn vị",
                            MapBpmVar = "DOC_ResidenceConfirmationOfHeadUnit"
                        }
                    }
                },
                new GetFileUploadGroup{
                    GroupId = 23,
                    GroupName ="Sổ tạm trú/Thẻ tạm trú/Giấy xác nhận tạm trú",
                    Mandatory = false,
                    HasAlternate = false,
                    Documents = new List<GetFileUploadDocument>{
                        new GetFileUploadDocument{
                            Id = 18,
                            DocumentCode="TemporaryResidenceConfirmation",
                            DocumentName ="Giấy xác nhận tạm trú",
                            MapBpmVar = "DOC_TemporaryResidenceConfirmation"
                        },
                         new GetFileUploadDocument{
                            Id = 60,
                            DocumentCode="TemporaryResidenceBook",
                            DocumentName ="Sổ tạm trú",
                            MapBpmVar = "DOC_TemporaryResidenceBook"
                        },
                         new GetFileUploadDocument{
                            Id = 63,
                            DocumentCode="TemporaryResidenceCard",
                            DocumentName ="Thẻ tạm trú",
                            MapBpmVar = "DOC_TemporaryResidenceCard"
                        }
                    }
                }
            };
            return Task.FromResult(result);
        }
    }
}
