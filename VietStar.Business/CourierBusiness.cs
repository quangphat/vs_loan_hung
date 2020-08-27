using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VietStar.Business.Interfaces;
using VietStar.Entities.Courier;
using VietStar.Entities.Infrastructures;
using VietStar.Entities.Messages;
using VietStar.Entities.Note;
using VietStar.Entities.ViewModels;
using VietStar.Repository.Interfaces;
using VietStar.Utility;
using static VietStar.Entities.Commons.Enums;

namespace VietStar.Business
{
    public class CourierBusiness : BaseBusiness, ICourierBusiness
    {
        protected readonly ICourierRepository _rpCourier;
        protected readonly IEmployeeRepository _rpEmployee;
        protected readonly INoteRepository _rpNote;
        public CourierBusiness(ICourierRepository courierRepository,
            IEmployeeRepository employeeRepository,
            INoteRepository noteRepository,
            IMapper mapper, CurrentProcess process) : base(mapper, process)
        {
            _rpCourier = courierRepository;
            _rpEmployee = employeeRepository;
            _rpNote = noteRepository;
        }

        public async Task<DataPaging<List<CourierIndexModel>>> GetsAsync(string freeText
            , int assigneeId
            , string status
            , int page
            , int limit
            , int groupId = 0
            , int provinceId = 0
            , string saleCode = null)
        {
            
            var response = await _rpCourier.GetsAsync(freeText, assigneeId, _process.User.Id, status, page, limit, groupId, provinceId, saleCode);
            if (response == null || !response.Any())
                return DataPaging.Create(response, 0);
            return DataPaging.Create(response, response.FirstOrDefault().TotalRecord);
        }
       
        public async Task<int> CreateAsync(CourierAddModel model)
        {
            var profile = _mapper.Map<CourierSql>(model);
            profile.CreatedBy = _process.User.Id;
            profile.Status = (int)ProfileStatus.New;
            var response = await _rpCourier.CreateAsync(profile);
            if (!response.success)
                return ToResponse(response);
            var sale = await _rpEmployee.GetEmployeeByCodeAsync(model.SaleCode.ToString().Trim(), _process.User.Id);
            if (sale == null)
            {
                return ToResponse(0, "Sale không tồn tại, vui lòng kiểm tra lại");
            }
            //var tasks = new List<Task>();
            //var ids = new List<int>() { model.AssignId, _process.User.Id, 1 };//1 is Thainm
            //if (!string.IsNullOrWhiteSpace(model.SaleCode))
            //{

            //    if (sale != null)
            //    {
            //        ids.Add(sale.Id);
            //    }
            //}
            //foreach (var assigneeId in ids)
            //{
            //    tasks.Add(_rpCourier.InsertCourierAssigneeAsync(response.data, assigneeId));
            //}
            //await Task.WhenAll(tasks);
            if (!string.IsNullOrWhiteSpace(model.LastNote))
            {

                var note = new NoteAddModel
                {
                    Content = model.LastNote,
                    ProfileId = response.data,
                    UserId = _process.User.Id,
                    ProfileTypeId = (int)NoteType.Courier
                };
                await _rpNote.AddNoteAsync(note);

            }
            return ToResponse(response);
        }

        public async Task<bool> UpdateAsync(CourierUpdateModel model)
        {
            if (model == null || model.Id <= 0)
            {
                return ToResponse(false, "Dữ liệu không hợp lệ");
            }
            if (string.IsNullOrWhiteSpace(model.CustomerName))
            {
                return ToResponse(false, "Tên khách hàng không được để trống");
            }
            if (model.AssignId <= 0)
            {
                return ToResponse(false, "Vui lòng chọn courier");
            }
            var existProfile = await _rpCourier.GetByIdAsync(model.Id);
            if (existProfile == null)
            {
                return ToResponse(false, "Hồ sơ không tồn tại");
            }
            bool isAdmin = _process.User.RoleCode =="admin" ? true :false;
            if (existProfile.Status == (int)ProfileStatus.Cancel)
            {
                if (!isAdmin)
                {
                    return ToResponse(false, "Bạn không có quyền, vui lòng liên hệ Admin");
                }
            }
            var profile = _mapper.Map<CourierSql>(model);
            profile.UpdatedBy = _process.User.Id;
            var response = await _rpCourier.UpdateAsync(profile);
            return ToResponse(response);
        }

        public async Task<CourierIndexModel> GetByIdAsync(int id)
        {
            var result = await _rpCourier.GetByIdAsync(id);
            return result;
        }
    }
}
