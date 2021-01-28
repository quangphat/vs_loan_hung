using MCreditService;
using MCreditService.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VS_LOAN.Core.Business.Interfaces;
using VS_LOAN.Core.Entity;
using VS_LOAN.Core.Entity.MCreditModels;
using VS_LOAN.Core.Entity.MCreditModels.SqlModel;
using VS_LOAN.Core.Entity.Model;
using VS_LOAN.Core.Entity.UploadModel;
using VS_LOAN.Core.Repository;
using VS_LOAN.Core.Repository.Interfaces;
using VS_LOAN.Core.Utility;
using VS_LOAN.Core.Web.Helpers;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace VS_LOAN.Core.Web.Controllers
{
    public class MiraeController : BaseController
    {

        protected readonly IMiraeRepository _rpMCredit;
        protected readonly IMediaBusiness _bizMedia;
        protected readonly IMiraeService _odcService;
        public readonly IOcbBusiness _ocbBusiness;
        public readonly IMiraeMaratialRepository _rpTailieu;
        public static ProvinceResponseModel _provinceResponseModel;
        public MiraeController(IMiraeRepository rpMCredit,
              IMediaBusiness mediaBusiness,
            IMiraeService odcService, IOcbBusiness ocbBusiness, IMiraeMaratialRepository tailieuBusiness) : base()
        {
            _rpTailieu = tailieuBusiness;
            _rpMCredit = rpMCredit;
            _odcService = odcService;
            _bizMedia = mediaBusiness;
            _ocbBusiness = ocbBusiness;


        }

        public JsonResult LayDanhsachTinh()
        {
            return ToJsonResponse(true, "", MiraeService.AllProvince.ToList());

        }

        public JsonResult GetAllBank()
        {
            return ToJsonResponse(true, "", MiraeService.AllBank.ToList());

        }
        public JsonResult LayDanhSachThanhPho(string province)
        {
            return ToJsonResponse(true, "", MiraeService.AllDistrict.Where(x => x.Lmc_STATE_N == province).ToList());

        }
        public JsonResult LayDanhSachWard(string cityCode)
        {
            return ToJsonResponse(true, "", MiraeService.AllWard.Where(x => x.City == cityCode).ToList());

        }

        public JsonResult GetAllSelectUser(string cityCode)
        {
            return ToJsonResponse(true, "", MiraeService.AllSelectUser.ToList());

        }
        public JsonResult GetAllSaleOfficeUser(string cityCode)
        {
            return ToJsonResponse(true, "", MiraeService.AllOfficeUser.ToList());

        }

        public JsonResult GetAllProduct(string cityCode)
        {
            return ToJsonResponse(true, "", MiraeService.Allproduct.ToList());

        }
        public ActionResult CheckCIC()
        {
            return View();
        }

        public async Task<ActionResult> Tracking(int id)
        {
            var result = await _rpMCredit.GetDetail(id);
            ViewBag.isAdmin = GlobalData.User.RoleId == (int)UserTypeEnum.Admin ? 1 : 0;
            ViewBag.model = result;

            ViewBag.DobMonth = result.Dob.Month;
            ViewBag.DobYear = result.Dob.Year;
            ViewBag.DobDay = result.Dob.Day;

            return View();
        }
        public async Task<JsonResult> CheckCMND(StringModel2 model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Value))
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _odcService.CheckCustomer(model.Value, "SBK");
            var item = result.Data;
            var boolSuccess = false;



            if (item.StatusNumber == "0" || item.StatusNumber == "302")
            {
                boolSuccess = true;
            }
            return ToJsonResponse(boolSuccess, result.Data?.ToString(), result);
        }
        public async Task<JsonResult> SearchTemps(string freeText, string status, int page = 1, int limit = 10, string fromDate = null, string toDate = null, int loaiNgay = 0, int manhom = 0,

          int mathanhvien = 0)
        {
            page = page <= 0 ? 1 : page;

            DateTime dtFromDate = DateTime.MinValue, dtToDate = DateTime.Now.AddDays(3);
            if (fromDate != "")
                dtFromDate = DateTimeFormat.ConvertddMMyyyyToDateTimeNew(fromDate);
            if (toDate != "")
                dtToDate = DateTimeFormat.ConvertddMMyyyyToDateTimeNew(toDate);

            var profiles = await _rpMCredit.GetTempProfiles(page, limit, freeText, GlobalData.User.IDUser, status, dtFromDate, dtToDate, loaiNgay, manhom, mathanhvien);
            if (profiles == null || !profiles.Any())
            {
                return ToJsonResponse(true, "", DataPaging.Create(null as List<MiraeModelSearchModel>, 0));
            }
            var result = DataPaging.Create(profiles, profiles[0].TotalRecord);
            return ToJsonResponse(true, "", result);
        }
        public async Task<JsonResult> UpdateDDEAsync(MiraeDDEEditModel model)
        {
            if (model == null)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");

            var profilerequest = await _rpMCredit.GetTemProfileByMcId(model.Id);
            var request = new MiraeDDEEditModel();
            request.Id = profilerequest.Id;
            request.Maritalstatus = model.Maritalstatus;
            request.Qualifyingyear = model.Qualifyingyear;
            request.Qualifyingyear = "0";
            request.Eduqualify = model.Eduqualify;
            request.Noofdependentin = model.Noofdependentin;
            request.Paymentchannel = model.Paymentchannel;
            request.Nationalidissuedate = model.Nationalidissuedate;
            request.Familybooknumber = model.Familybooknumber;
            request.Idissuer = model.Idissuer;
            request.Spousename = model.Spousename;
            request.Spouse_id_c = model.Spouse_id_c;
            request.Categoryid = model.Categoryid;
            request.Categoryid = "SBK";
            request.Bankname = model.Bankname;
            request.Bankbranch = model.Bankbranch;
            request.Acctype = model.Acctype;
            request.Accno = model.Accno;
            request.Dueday = model.Dueday;
            request.Notecode = "DE_MOBILE";
            request.Notedetails = model.Notedetails;
            request.UpdatedBy = GlobalData.User.IDUser;
            request.Familybooknumber = model.Familybooknumber;
            request.Spousename = model.Spousename;
            request.Spouse_id_c = model.Spouse_id_c;
            request.Notedetails = model.Notedetails;


            var result = await _rpMCredit.UpdateDDE(request);

            if (!result)
            {
                return ToJsonResponse(result, "Lỗi cập nhật");
            }
            return ToJsonResponse(true);
        }
        public async Task<JsonResult> UpdateAsync(MiraeEditModel model)
        {
            if (model == null)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");

            var profilerequest = await _rpMCredit.GetTemProfileByMcId(model.Id);


            profilerequest.Channel = model.Channel;
            profilerequest.Schemeid = model.Schemeid;
            profilerequest.Downpayment = model.Downpayment != null ? model.Downpayment : 0;
            profilerequest.Totalloanamountreq = model.Totalloanamountreq;
            profilerequest.Tenure = model.Tenure;
            profilerequest.Sourcechannel = "ADVT";
            profilerequest.Salesofficer = model.Salesofficer;
            profilerequest.Loanpurpose = model.Loanpurpose;
            profilerequest.Creditofficercode = model.Creditofficercode;
            profilerequest.Bankbranchcode = model.Bankbranchcode;
            profilerequest.Laa_app_ins_applicable = model.Laa_app_ins_applicable;
            profilerequest.Possipbranch = model.Possipbranch;
            profilerequest.Priority_c = model.Priority_c;
            profilerequest.Userid = model.Userid;
            profilerequest.Fname = model.Fname;
            profilerequest.Mname = model.Mname;
            profilerequest.Lname = model.Lname;
            profilerequest.Nationalid = model.Nationalid;
            profilerequest.Title = model.Title;
            profilerequest.Gender = model.Gender;
            profilerequest.Dob = model.Dob;
            profilerequest.Constid = model.Constid;
            profilerequest.AddressPer_in_propertystatus = model.PropertystatusPer;
            profilerequest.AddressPer_address1stline = model.Address1stlinePer;
            profilerequest.AddressPer_Country = 189;
            profilerequest.AddressPer_District = model.DdlHuyenPer;
            profilerequest.AddressPer_City = model.DdlTinhPer;
            profilerequest.AddressPer_Ward = model.DdlRewardPer;
            profilerequest.AddressPer_roomno = model.RoomnoPer;
            profilerequest.AddressPer_stayduratPeradd_m = model.StayduratPeradd_mPer;
            profilerequest.AddressPer_stayduratPeradd_y = model.StayduratPeradd_yPer;
            profilerequest.AddressPer_mobile = model.MobilePer;
            profilerequest.AddressPer_landlord = model.LandlordPer;
            profilerequest.AddressPer_landmark = model.Landmarkper;
            profilerequest.AddressCur_in_propertystatus = model.PropertystatusCur;
            profilerequest.AddressCur_address1stline = model.Address1stlineCur;
            profilerequest.AddressCur_Country = model.DdlTinhCur;
            profilerequest.AddressCur_District = model.DdlHuyenCur;
            profilerequest.AddressCur_City = model.DdlTinhCur;
            profilerequest.AddressCur_Ward = model.DdlRewardCur;
            profilerequest.AddressCur_roomno = model.RoomnoCur;
            profilerequest.AddressCur_stayduratcuradd_m = model.Stayduratcuradd_mCur;
            profilerequest.AddressCur_stayduratcuradd_y = model.Stayduratcuradd_yCur;
            profilerequest.AddressCur_mobile = model.MobileCur;
            profilerequest.AddressCur_landlord = model.LandlordCur;
            profilerequest.AddressCur_landmark = model.LandmarkCur;
            profilerequest.Tax_code = model.Tax_code;
            profilerequest.Presentjobyear = model.Presentjobyear;
            profilerequest.Previousjobmth = model.Previousjobmth;
            profilerequest.Presentjobmth = model.Presentjobmth;
            profilerequest.Previousjobyear = model.Previousjobyear;
            profilerequest.Accountbank = model.Accountbank;
            profilerequest.Debit_credit = model.Debit_credit;
            profilerequest.In_per_cont = model.Per_cont;
            profilerequest.Amount = model.Amount;
            profilerequest.Head = model.Head;
            profilerequest.Frequency = model.Frequency;
            profilerequest.Others = model.Others;

            profilerequest.Refferee1_in_title = model.Refferee1_in_title;
            profilerequest.Refferee1_Refereename = model.Refferee1_Refereename;
            profilerequest.Refferee1_Refereerelation = model.Refferee1_Refereerelation;
            profilerequest.Refferee1_Phone2 = model.Refferee1_Phone1;
            profilerequest.Refferee1_Phone1 = model.Refferee1_Phone1;

            profilerequest.Refferee2_in_title = model.Refferee2_in_title;
            profilerequest.Refferee2_Refereename = model.Refferee2_Refereename;
            profilerequest.Refferee2_Refereerelation = model.Refferee2_Refereerelation;
            profilerequest.Refferee2_Phone2 = model.Refferee2_Phone1;
            profilerequest.Refferee2_Phone1 = model.Refferee2_Phone1;

            profilerequest.Refferee3_in_title = model.Refferee3_in_title;
            profilerequest.Refferee3_Refereename = model.Refferee3_Refereename;
            profilerequest.Refferee3_Refereerelation = model.Refferee3_Refereerelation;
            profilerequest.Refferee3_Phone2 = model.Refferee3_Phone2;
            profilerequest.Refferee3_Phone1 = model.Refferee3_Phone1;

            profilerequest.ContryCompany = model.ContryCompany;
            profilerequest.CityCompany = model.CityCompany;
            profilerequest.DistrictCompany = model.DistrictCompany;
            profilerequest.WardCompany = model.WardCompany;
            profilerequest.Addressline = model.Addressline;
            profilerequest.Phone = model.Phone;

            profilerequest.Natureofbuss = model.Natureofbuss;
            profilerequest.Addresstype = model.Addresstype;



            profilerequest.UpdatedBy = GlobalData.User.IDUser;
            profilerequest.Position = model.Position;
            profilerequest.IsDuplicateAdrees = model.IsDuplicateAdrees;

            profilerequest.Phone = model.Phone;
            profilerequest.Fixphone = model.Fixphone;
            profilerequest.Mobile = model.Mobile;
            profilerequest.IsDuplicateAdrees = model.IsDuplicateAdrees;



            var result = await _rpMCredit.UpdateDraftProfile(profilerequest);
            return ToJsonResponse(true);
        }
        public async Task<JsonResult> SumbitToDDE(int id)
        {
            var model = await _rpMCredit.GetTemProfileByMcId(id);


            if (model == null)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");

            if (model.Nationalidissuedate == null || model.Nationalidissuedate.HasValue == false)
            {
                return ToJsonResponse(false, "Vui lòng điền ngày cấp chứng minh nhân dân");

            }
            else if (string.IsNullOrEmpty(model.Idissuer))
            {
                return ToJsonResponse(false, "Chưa chọn địa chỉ cung cấp CMND");

            }
            else if (string.IsNullOrEmpty(model.Maritalstatus))
            {

                return ToJsonResponse(false, "Vui lòng chọn trạng thái hôn nhân");
            }
            else if (string.IsNullOrEmpty(model.Familybooknumber))
            {

                return ToJsonResponse(false, "Vui lòng  cung cấp thông tin sổ hộ khẩu");
            }
            else if (string.IsNullOrEmpty(model.Eduqualify))
            {

                return ToJsonResponse(false, "Vui lòng chọn trình độ học vấn");
            }

            else if (string.IsNullOrEmpty(model.Acctype))
            {

                return ToJsonResponse(false, "Vui lòng chọn loại tài khoản ");
            }




            var appId = 0;
            var bankName = model.Bankname;
            try
            {
                appId = int.Parse(model.AppId);
            }
            catch (Exception)
            {
            }
            try
            {
                if (bankName.Length < 8)
                {
                    bankName = "0" + bankName;
                }
            }
            catch (Exception)
            {


            }
            var request = new MiraeDDELeadReQuest()
            {
                in_appid = appId,
                in_maritalstatus = model.Maritalstatus,
                in_qualifyingyear = model.Qualifyingyear,
                in_eduqualify = model.Eduqualify,
                in_noofdependentin = model.Noofdependentin,
                in_paymentchannel = model.Paymentchannel,
                in_nationalidissuedate = model.Nationalidissuedate.Value.ToShortDateString(),
                in_familybooknumber = model.Familybooknumber,
                in_idissuer = model.Idissuer,
                in_spousename = model.Spousename,
                in_spouse_id_c = model.Spouse_id_c,
                in_categoryid = model.Categoryid,
                in_bankname = bankName,
                in_bankbranch = model.Bankbranch,
                in_acctype = model.Acctype,
                in_accno = model.Accno,
                in_dueday = model.Dueday,
                in_notecode = model.Notecode,
                in_notedetails = model.Notedetails
            };
            request.in_qualifyingyear = "0";
            request.in_notecode = "DE_MOBILE";
            request.in_channel = "SBK";
            request.in_userid = "EXT_SBK";
            request.in_categoryid = "SBK";
            var result = await _odcService.DDESubmit(request);

            if (result.Success)
            {
                await _rpMCredit.UpdateStatusMAFC(id, 2, int.Parse(model.AppId), GlobalData.User.IDUser);
            }

            return ToJsonResponse(result.Success, "", result);

        }
        public async Task<JsonResult> BDEToPor(int id)
        {
            var model = await _rpMCredit.GetTemProfileByMcId(id);
            if (model == null)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");

            if (string.IsNullOrEmpty(model.AppId))
            {
                return ToJsonResponse(false, "Chưa có thông appid");

            }
            var appId = 0;

            try
            {
                appId = int.Parse(model.AppId);
            }
            catch (Exception)
            {
            }
            var response = await _odcService.DDEToPoR(new DDEToPORReQuest()
            {
                in_channel = "SBK",
                in_userid = "EXT_SBK",
                msgName = "procDDEChangeState",
                p_appid = appId

            });

            if (response.Success)
            {
                await _rpMCredit.UpdateStatusMAFC(id, 3, int.Parse(model.AppId), GlobalData.User.IDUser);
            }
            return ToJsonResponse(response.Success, "", response);
        }

        private MiraeQDELeadReQuest MiramodelToQDERequest( MiraeModel model)
        {



            model.Sourcechannel = "ADVT";
            model.Userid = "EXT_SBK";

            var request = new MiraeQDELeadReQuest()
            {
                in_phone = model.Phone,

                in_fixphone = model.Fixphone != null ? model.Fixphone : "",
                in_channel = "SBK",
                in_schemeid = (model.Schemeid != null) ? int.Parse(model.Schemeid) : 0,
                in_downpayment = model.Downpayment != null ? model.Downpayment : 0,
                in_totalloanamountreq = model.Totalloanamountreq,
                in_tenure = model.Tenure,
                in_sourcechannel = "ADVT",
                in_salesofficer = (model.Salesofficer != null) ? int.Parse(model.Salesofficer) : 0,


                in_loanpurpose = model.Loanpurpose,
                in_creditofficercode = "EXT_SBK",
                in_bankbranchcode = model.Bankbranchcode,
                in_laa_app_ins_applicable = model.Laa_app_ins_applicable == "True" ? "Y" : "N",
                in_priority_c = model.Priority_c,

                in_userid = "EXT_SBK",
                in_fname = model.Fname
            };
            request.in_mname = model.Mname;
            request.in_lname = model.Lname;
            request.in_nationalid = model.Nationalid;
            request.in_title = model.Title + '.';
            request.in_gender = model.Gender == "0" ? "M" : "F";
            request.in_dob = model.Dob.ToString("dd/MM/yyyy");
            request.in_constid = 5;
            request.in_tax_code = model.Tax_code;
            request.in_presentjobmth = model.Presentjobmth;
            request.in_presentjobyear = model.Presentjobyear;
            request.in_previousjobmth = model.Previousjobmth;
            request.in_previousjobyear = model.Previousjobyear;
            request.in_natureofbuss = model.Natureofbuss;
            request.in_referalgroup = model.Referalgroup;
            request.in_addresstype = model.Addresstype;
            request.in_addressline = model.Addressline;
            request.in_country = 189;
            request.in_city = model.CityCompany;
            request.in_district = model.DistrictCompany;
            request.in_ward = model.WardCompany;
            request.in_phone = model.Phone;
            request.in_others = model.Others;
            request.in_position = model.Position;
            request.in_constid = 5;
            request.in_possipbranch = "14";
            request.in_debit_credit = "P";
            request.in_per_cont = "100";
            //request.in_others = "Công ty TNHH MTV ABC";
            request.in_title = model.Title + '.';
            //request.in_mobile = model.Mobile;
            //request.in_fixphone = model.Fixphone;
            request.in_others = model.Others;
            request.in_amount = model.Amount;



            if (model.IsDuplicateAdrees.HasValue)
            {

                if (model.IsDuplicateAdrees.Value)
                {
                    request.address = new List<AddressItem>()
                     {
                            new AddressItem()
                            {
                                in_mobile = model.Mobile,
                                in_address1stline = model.AddressCur_address1stline,
                                in_addresstype = "CURRES",
                                in_city = model.AddressCur_City,
                                in_country = 189,
                                in_district =model.AddressCur_District,
                                in_landlord = model.AddressCur_landlord,
                                in_landmark = model.AddressCur_landmark,
                                in_mailingaddress = "Y",
                                in_propertystatus = model.AddressCur_in_propertystatus,
                                in_roomno = model.AddressCur_roomno!=null?model.AddressCur_roomno:"",
                                in_stayduratcuradd_m = model.AddressCur_stayduratcuradd_m!=null ?  int.Parse( model.AddressCur_stayduratcuradd_m):0,
                                in_stayduratcuradd_y = model.AddressCur_stayduratcuradd_y!=null ?  int.Parse( model.AddressCur_stayduratcuradd_y):0,
                                in_ward = model.AddressCur_Ward,

                            }

                    };

                }
                else
                {
                    request.address = new List<AddressItem>()
                    {
                        new AddressItem()
                        {
                            in_mobile = model.Mobile,

                            in_address1stline = model.AddressCur_address1stline,
                            in_addresstype = "CURRES",
                            in_city = model.AddressCur_City,
                            in_country = 189,
                            in_district =model.AddressCur_District,
                            in_landlord = model.AddressCur_landlord,
                            in_landmark = model.AddressCur_landmark,
                            in_mailingaddress = "Y",
                            in_propertystatus = model.AddressCur_in_propertystatus,
                            in_roomno = model.AddressCur_roomno,
                            in_stayduratcuradd_m = model.AddressCur_stayduratcuradd_m!=null ?  int.Parse( model.AddressCur_stayduratcuradd_m):0,
                            in_stayduratcuradd_y = model.AddressCur_stayduratcuradd_y!=null ?  int.Parse( model.AddressCur_stayduratcuradd_y):0,
                            in_ward = model.AddressCur_Ward,

                        },
                         new AddressItem()
                        {
                            in_mobile = model.AddressPer_mobile,
                            in_address1stline = model.AddressPer_address1stline,
                            in_addresstype = "PERMNENT",
                            in_city = model.AddressPer_City,
                            in_country = 189,
                            in_district =model.AddressPer_District,
                            in_landlord = model.AddressPer_landlord,
                            in_landmark = model.AddressPer_landmark,
                            in_mailingaddress = "N",
                            in_propertystatus = model.AddressPer_in_propertystatus,
                            in_roomno = model.AddressPer_roomno,
                            in_stayduratcuradd_m = model.AddressPer_stayduratPeradd_m!=null ?  int.Parse( model.AddressPer_stayduratPeradd_m):0,
                            in_stayduratcuradd_y = model.AddressPer_stayduratPeradd_y!=null ?  int.Parse( model.AddressPer_stayduratPeradd_y):0,
                            in_ward = model.AddressPer_Ward,

                        },

                 };


                }

            }
            else
            {

                request.address = new List<AddressItem>()
                    {
                        new AddressItem()
                        {
                            in_mobile = model.Mobile,
                            in_fixphone ="",
                            in_address1stline = model.AddressCur_address1stline,
                            in_addresstype = "CURRES",
                            in_city = model.AddressCur_City,
                            in_country = 189,
                            in_district =model.AddressCur_District,
                            in_landlord = model.AddressCur_landlord,
                            in_landmark = model.AddressCur_landmark,
                            in_mailingaddress = "Y",
                            in_propertystatus = model.AddressCur_in_propertystatus,
                            in_roomno = model.AddressCur_roomno,
                            in_stayduratcuradd_m = model.AddressCur_stayduratcuradd_m!=null ?  int.Parse( model.AddressCur_stayduratcuradd_m):0,
                            in_stayduratcuradd_y = model.AddressCur_stayduratcuradd_y!=null ?  int.Parse( model.AddressCur_stayduratcuradd_y):0,
                            in_ward = model.AddressCur_Ward,

                        },
                         new AddressItem()
                        {
                            in_mobile ="",
                            in_fixphone ="",
                            in_address1stline = model.AddressPer_address1stline,
                            in_addresstype = "PERMNENT",
                            in_city = model.AddressPer_City,
                            in_country = 189,
                            in_district =model.AddressPer_District,
                            in_landlord = model.AddressPer_landlord,
                            in_landmark = model.AddressPer_landmark,
                            in_mailingaddress = "N",
                            in_propertystatus = model.AddressPer_in_propertystatus,
                            in_roomno = model.AddressPer_roomno,
                            in_stayduratcuradd_m = model.AddressPer_stayduratPeradd_m!=null ?  int.Parse( model.AddressPer_stayduratPeradd_m):0,
                            in_stayduratcuradd_y = model.AddressPer_stayduratPeradd_y!=null ?  int.Parse( model.AddressPer_stayduratPeradd_y):0,
                            in_ward = model.AddressPer_Ward,

                        },

                 };


            }

            request.reference = new List<ReferenceItem>();

            if (string.IsNullOrEmpty(model.Refferee1_Refereename) == false)
            {
                request.reference.Add(new ReferenceItem()
                {
                    in_phone_1 = model.Refferee1_Phone1,
                    in_phone_2 = "",
                    in_title = model.Refferee1_in_title + '.',
                    in_refereename = model.Refferee1_Refereename,
                    in_refereerelation = model.Refferee1_Refereerelation
                });

            }

            if (string.IsNullOrEmpty(model.Refferee2_Refereename) == false)
            {
                request.reference.Add(new ReferenceItem()
                {
                    in_title = model.Refferee2_in_title + '.',
                    in_phone_1 = model.Refferee2_Phone1 + '.',
                    in_phone_2 = "",
                    in_refereename = model.Refferee2_Refereename,
                    in_refereerelation = model.Refferee2_Refereerelation
                });

            }

            if (string.IsNullOrEmpty(model.Refferee3_Refereename) == false)
            {
                request.reference.Add(new ReferenceItem()
                {
                    in_title = model.Refferee3_in_title + '.',
                    in_phone_1 = model.Refferee3_Phone1 + '.',
                    in_phone_2 = "",
                    in_refereename = model.Refferee3_Refereename,
                    in_refereerelation = model.Refferee3_Refereerelation
                });

            }

            return request;

        }
        public async Task<JsonResult> SumbitToOcb (int id)
        {
            var model = await _rpMCredit.GetTemProfileByMcId(id);
            var request = MiramodelToQDERequest(model);

              var ressult =  await _odcService.QDESubmit(request);
                if(ressult.Success)
                {
                var appid = 0;
                try
                {
                    appid = int.Parse(ressult.Data);
                }
                catch (Exception)
                {

                    appid = 0;
                }

                await _rpMCredit.SetAppidProfile(model.Id, appid);

            
               
           }

            return ToJsonResponse(ressult.Success, "",ressult);
        }
        public async Task<JsonResult> QDEToDDE(int id)
        {
            var model = await _rpMCredit.GetTemProfileByMcId(id);
            if (model == null)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");

            if (string.IsNullOrEmpty(model.AppId))
            {
                return ToJsonResponse(false, "Chưa có thông appid");

            }
            var appId = 0;

            try
            {
                appId = int.Parse(model.AppId);
            }
            catch (Exception)
            {
            }
            var response = await _odcService.QDEToDDE(new QDEToDDEReQuest()
            {
               
                p_appid = appId

            });

            if(response.Success)
            {
                await _rpMCredit.UpdateStatusMAFC(id, 1, int.Parse(model.AppId), GlobalData.User.IDUser);
            }
            return ToJsonResponse(response.Success, "", response);
        }
        public async Task<JsonResult> UpdateMirae(MiraeEditModel model)
        {
                       
            if (model == null)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var profilerequest = await _rpMCredit.GetTemProfileByMcId(model.Id);
            profilerequest.Channel = model.Channel;
            profilerequest.Schemeid = model.Schemeid;
            profilerequest.Downpayment = model.Downpayment != null ? model.Downpayment : 0;
            profilerequest.Totalloanamountreq = model.Totalloanamountreq;
            profilerequest.Tenure = model.Tenure;
            profilerequest.Sourcechannel = "ADVT";
            profilerequest.Salesofficer = model.Salesofficer;
            profilerequest.Loanpurpose = model.Loanpurpose;
            profilerequest.Creditofficercode = model.Creditofficercode;
            profilerequest.Bankbranchcode = model.Bankbranchcode;
            profilerequest.Laa_app_ins_applicable = model.Laa_app_ins_applicable;
            profilerequest.Possipbranch = model.Possipbranch;
            profilerequest.Priority_c = model.Priority_c;
            profilerequest.Userid = model.Userid;
            profilerequest.Fname = model.Fname;
            profilerequest.Mname = model.Mname;
            profilerequest.Lname = model.Lname;
            profilerequest.Nationalid = model.Nationalid;
            profilerequest.Title = model.Title;
            profilerequest.Gender = model.Gender;
            profilerequest.Dob = model.Dob;

            profilerequest.Dob = string.IsNullOrWhiteSpace(model.DobStr) ? DateTime.Now : DateTimeFormat.ConvertddMMyyyyToDateTime(model.DobStr);


            profilerequest.Nationalidissuedate = model.Nationalidissuedate;
            profilerequest.Nationalidissuedate = string.IsNullOrWhiteSpace(model.NationalidissuedateStr) ? DateTime.Now : DateTimeFormat.ConvertddMMyyyyToDateTime(model.NationalidissuedateStr);


            profilerequest.Constid = model.Constid;
            profilerequest.AddressPer_in_propertystatus = model.PropertystatusPer;
            profilerequest.AddressPer_address1stline = model.Address1stlinePer;
            profilerequest.AddressPer_Country = 189;
            profilerequest.AddressPer_District = model.DdlHuyenPer;
            profilerequest.AddressPer_City = model.DdlTinhPer;
            profilerequest.AddressPer_Ward = model.DdlRewardPer;
            profilerequest.AddressPer_roomno = model.RoomnoPer;
            profilerequest.AddressPer_stayduratPeradd_m = model.StayduratPeradd_mPer;
            profilerequest.AddressPer_stayduratPeradd_y = model.StayduratPeradd_yPer;
            profilerequest.AddressPer_mobile = model.MobilePer;
            profilerequest.AddressPer_landlord = model.LandlordPer;
            profilerequest.AddressPer_landmark = model.Landmarkper;
            profilerequest.AddressCur_in_propertystatus = model.PropertystatusCur;
            profilerequest.AddressCur_address1stline = model.Address1stlineCur;
            profilerequest.AddressCur_Country = model.DdlTinhCur;
            profilerequest.AddressCur_District = model.DdlHuyenCur;
            profilerequest.AddressCur_City = model.DdlTinhCur;
            profilerequest.AddressCur_Ward = model.DdlRewardCur;
            profilerequest.AddressCur_roomno = model.RoomnoCur;
            profilerequest.AddressCur_stayduratcuradd_m = model.Stayduratcuradd_mCur;
            profilerequest.AddressCur_stayduratcuradd_y = model.Stayduratcuradd_yCur;
            profilerequest.AddressCur_mobile = model.MobileCur;
            profilerequest.AddressCur_landlord = model.LandlordCur;
            profilerequest.AddressCur_landmark = model.LandmarkCur;
            profilerequest.Tax_code = model.Tax_code;
            profilerequest.Presentjobyear = model.Presentjobyear;
            profilerequest.Previousjobmth = model.Previousjobmth;
            profilerequest.Presentjobmth = model.Presentjobmth;
            profilerequest.Previousjobyear = model.Previousjobyear;
            profilerequest.Accountbank = model.Accountbank;
            profilerequest.Debit_credit = model.Debit_credit;
            profilerequest.In_per_cont = model.Per_cont;
            profilerequest.Amount = model.Amount;
            profilerequest.Head = model.Head;
            profilerequest.Frequency = model.Frequency;
            profilerequest.Others = model.Others;
            profilerequest.Refferee1_in_title = model.Refferee1_in_title;
            profilerequest.Refferee1_Refereename = model.Refferee1_Refereename;
            profilerequest.Refferee1_Refereerelation = model.Refferee1_Refereerelation;
            profilerequest.Refferee1_Phone2 = model.Refferee1_Phone1;
            profilerequest.Refferee1_Phone1 = model.Refferee1_Phone1;
            profilerequest.Refferee2_in_title = model.Refferee2_in_title;
            profilerequest.Refferee2_Refereename = model.Refferee2_Refereename;
            profilerequest.Refferee2_Refereerelation = model.Refferee2_Refereerelation;
            profilerequest.Refferee2_Phone2 = model.Refferee2_Phone1;
            profilerequest.Refferee2_Phone1 = model.Refferee2_Phone1;
            profilerequest.Refferee3_in_title = model.Refferee3_in_title;
            profilerequest.Refferee3_Refereename = model.Refferee3_Refereename;
            profilerequest.Refferee3_Refereerelation = model.Refferee3_Refereerelation;
            profilerequest.Refferee3_Phone2 = model.Refferee3_Phone2;
            profilerequest.Refferee3_Phone1 = model.Refferee3_Phone1;
            profilerequest.ContryCompany = model.ContryCompany;
            profilerequest.CityCompany = model.CityCompany;
            profilerequest.DistrictCompany = model.DistrictCompany;
            profilerequest.WardCompany = model.WardCompany;
            profilerequest.Addressline = model.Addressline;
            profilerequest.Phone = model.Phone;

            profilerequest.Natureofbuss = model.Natureofbuss;
            profilerequest.Addresstype = model.Addresstype;



            profilerequest.UpdatedBy = GlobalData.User.IDUser;
            profilerequest.Position = model.Position;
            profilerequest.IsDuplicateAdrees = model.IsDuplicateAdrees;

            profilerequest.Phone = model.Phone;
            profilerequest.Fixphone = model.Fixphone;
            profilerequest.Mobile = model.Mobile;
            profilerequest.IsDuplicateAdrees = model.IsDuplicateAdrees;

           profilerequest.Maritalstatus = model.Maritalstatus;
            profilerequest.Qualifyingyear = model.Qualifyingyear;
            profilerequest.Qualifyingyear = "0";
            profilerequest.Eduqualify = model.Eduqualify;
            profilerequest.Noofdependentin = model.Noofdependentin;
            profilerequest.Paymentchannel = model.Paymentchannel;
       
            profilerequest.Familybooknumber = model.Familybooknumber;
            profilerequest.Idissuer = model.Idissuer;
            profilerequest.Spousename = model.Spousename;
            profilerequest.Spouse_id_c = model.Spouse_id_c;
            profilerequest.Categoryid = model.Categoryid;
            profilerequest.Categoryid = "SBK";
            profilerequest.Bankname = model.Bankname;
            profilerequest.Bankbranch = model.Bankbranch;
            profilerequest.Acctype = model.Acctype;
            profilerequest.Accno = model.Accno;
            profilerequest.Dueday = model.Dueday;
            profilerequest.Notecode = "DE_MOBILE";
            profilerequest.Notedetails = model.Notedetails;
            profilerequest.UpdatedBy = GlobalData.User.IDUser;
            profilerequest.Familybooknumber = model.Familybooknumber;
            profilerequest.Spousename = model.Spousename;
            profilerequest.Spouse_id_c = model.Spouse_id_c;
            profilerequest.Status = model.Status;
            profilerequest.Notedetails = model.Notedetails;
         
             if (GlobalData.User.UserType != (int)UserTypeEnum.Admin)
            {
                var checkError = ValidateProfile(profilerequest);

                if (!string.IsNullOrEmpty(checkError))
                {
                    return ToJsonResponse(false, checkError, checkError);
                }

            }

            var result = await _rpMCredit.UpdateDraftProfile(profilerequest);

            if (!string.IsNullOrEmpty(model.SellerNote))
            {
                await _rpMCredit.AddNoteAsync(model.Id, model.SellerNote, GlobalData.User.IDUser);

            }

            if (!result)
            {
                return ToJsonResponse(result, "Lỗi cập nhật");
            }


            return ToJsonResponse(true);


        }
        public async Task<JsonResult> CreateDraft(MiraeAddModel model)
        {
            


            if (model.Natureofbuss=="")
            {
                model.Natureofbuss = "hoat dong lam thue cac cong viec trong cac hgd,sx sp vat chat va dich vu tu tieu dung cua ho gia dinh";
            }
            var profile = new MiraeModel
            {
                Channel = "SBK",
                Schemeid = model.Schemeid,
                Downpayment = model.Downpayment != null ? model.Downpayment : 0,
                Totalloanamountreq = model.Totalloanamountreq,
                Tenure = model.Tenure,
                Sourcechannel = "ADVT",
                Salesofficer = model.Salesofficer,
                Loanpurpose = model.Loanpurpose,
                Creditofficercode = model.Creditofficercode,
                Bankbranchcode = model.Bankbranchcode,
                Laa_app_ins_applicable = model.Laa_app_ins_applicable,
                Possipbranch = model.Possipbranch,
                Priority_c = model.Priority_c,
                Userid = model.Userid,
                Fname = model.Fname,
                Mname = model.Mname,
                Lname = model.Lname,
                Nationalid = model.Nationalid,
                Title = model.Title,
                Gender = model.Gender,
                Dob = model.Dob,
                Constid = model.Constid,
                AddressPer_in_propertystatus = model.PropertystatusPer,
                AddressPer_address1stline = model.Address1stlinePer,
                AddressPer_Country = model.DdlTinhPer,
                AddressPer_District = model.DdlHuyenPer,
                AddressPer_City = model.DdlTinhPer,
                AddressPer_Ward = model.DdlRewardPer,
                AddressPer_roomno = model.RoomnoPer,
                AddressPer_stayduratPeradd_m = model.StayduratPeradd_mPer,
                AddressPer_stayduratPeradd_y = model.StayduratPeradd_yPer,
                AddressPer_mobile = model.MobilePer,
                AddressPer_landlord = model.LandlordPer,
                AddressPer_landmark = model.Landmarkper,

                AddressCur_in_propertystatus = model.PropertystatusCur,
                AddressCur_address1stline = model.Address1stlineCur,
                AddressCur_Country = model.DdlTinhCur,
                AddressCur_District = model.DdlHuyenCur,
                AddressCur_City = model.DdlTinhCur,
                AddressCur_Ward = model.DdlRewardCur,
                AddressCur_roomno = model.RoomnoCur,
                AddressCur_stayduratcuradd_m = model.Stayduratcuradd_mCur,
                AddressCur_stayduratcuradd_y = model.Stayduratcuradd_yCur,
                AddressCur_mobile = model.MobileCur,
                AddressCur_landlord = model.LandlordCur,
                AddressCur_landmark = model.LandmarkCur,
                Tax_code = model.Tax_code,
                Presentjobyear = model.Presentjobyear,
                Previousjobmth = model.Previousjobmth,
                Presentjobmth = model.Presentjobmth,
                Previousjobyear = model.Previousjobyear,
                Accountbank = model.Accountbank,
                Debit_credit = model.Debit_credit,
                In_per_cont = model.Per_cont,
                Amount = model.Amount,
                Head = model.Head,
                Addresstype = model.Addresstype,
                Frequency = model.Frequency,

                Refferee1_in_title = model.Refferee1_in_title,
                Refferee1_Refereename = model.Refferee1_Refereename,
                Refferee1_Refereerelation = model.Refferee1_Refereerelation,
                Refferee1_Phone2 = model.Refferee1_Phone1,
                Refferee1_Phone1 = model.Refferee1_Phone1,
                Refferee2_in_title = model.Refferee2_in_title,
                Refferee2_Refereename = model.Refferee2_Refereename,
                Refferee2_Refereerelation = model.Refferee2_Refereerelation,
                Refferee2_Phone2 = model.Refferee2_Phone2,
                Refferee2_Phone1 = model.Refferee2_Phone1,
                Refferee3_in_title = model.Refferee3_in_title,
                Refferee3_Refereename = model.Refferee3_Refereename,
                Refferee3_Refereerelation = model.Refferee3_Refereerelation,
                Refferee3_Phone2 = model.Refferee3_Phone2,
                Refferee3_Phone1 = model.Refferee3_Phone1,
                ContryCompany = model.ContryCompany,
                CityCompany = model.CityCompany,
                DistrictCompany = model.DistrictCompany,
                WardCompany = model.WardCompany,
                Addressline = model.Addressline,
               Natureofbuss = model.Natureofbuss,
                Position = model.Position,
                Phone = model.Phone,
                Fixphone = model.Fixphone,
                Mobile = model.Mobile,
                Bankbranch = model.Bankbranch,
                Bankname = model.Bankname,
                Categoryid = model.Categoryid,
                Eduqualify = model.Eduqualify,
                Dueday =model.Dueday,
                Familybooknumber = model.Familybooknumber,
                Idissuer = model.Idissuer,
                IsDuplicateAdrees = model.IsDuplicateAdrees,
                Maritalstatus =model.Maritalstatus,
                Accno = model.Accno,
                Nationalidissuedate =model.Nationalidissuedate,
                Noofdependentin = model.Noofdependentin,
                Notedetails =model.Notedetails,
                Notecode =model.Notecode,
                Acctype = model.Acctype,
                Paymentchannel =model.Paymentchannel,
                Others = model.Others,
                Referalgroup = "3",
                Qualifyingyear =model.Qualifyingyear,
                Spousename =model.Spousename,
                Spouse_id_c = model.Spouse_id_c,
                Status = 0
            };

            var checkMessage = ValidateProfile(profile);
            if(!string.IsNullOrEmpty(checkMessage))
            {
                return ToJsonResponse( false, checkMessage, checkMessage);
            }

            if(!string.IsNullOrEmpty(model.SellerNote))
            {
                await _rpMCredit.AddNoteAsync(model.Id, model.SellerNote, GlobalData.User.IDUser);
             
            }



            profile.Status = 0;
            profile.CreatedBy = GlobalData.User.IDUser;
            profile.Others = model.Others;

            profile.Dob = model.Dob;

            profile.Dob = string.IsNullOrWhiteSpace(model.DobStr) ? DateTime.Now : DateTimeFormat.ConvertddMMyyyyToDateTime(model.DobStr);

           
            profile.Nationalidissuedate = model.Nationalidissuedate;
            profile.Nationalidissuedate = string.IsNullOrWhiteSpace(model.NationalidissuedateStr) ? DateTime.Now : DateTimeFormat.ConvertddMMyyyyToDateTime(model.NationalidissuedateStr);
            profile.Amount = model.Amount;
            if(string.IsNullOrEmpty(model.Head))
            {
                profile.Head = "NETINCOM";
            }
          
            var id = await _rpMCredit.CreateDraftProfile(profile);
            if (id > 0)
            {
               
            }
            return ToJsonResponse(id>0, "", id);
        }
         public async Task<ActionResult> Mirae(int id)
        {
            var result = await _rpMCredit.GetTemProfileByMcId(id);
            ViewBag.isAdmin = GlobalData.User.TypeUser == (int)UserTypeEnum.Admin ? true : false;
            ViewBag.model = result;
            ViewBag.LstTaiLieu = new List<TaiLieuModel>();
            ViewBag.LstLoaiTaiLieu = await _rpTailieu.GetLoaiTailieuList(8);
            return View("viewQDE");


        }
        public async Task<JsonResult> SendFile(int id)
        {
            int profileId = id;

            var model = await _rpMCredit.GetTemProfileByMcId(profileId);
            var allTaiLieu = await _rpTailieu.GetTailieuMiraeHosoId(model.Id, 7);

            if (allTaiLieu.Count < 6)
            {
                return ToJsonResponse(false, "Ít nhất phải là 6 chứng từ bắt buộc", new PushToUNDReponse()
                {
                    Data = "Ít nhất phải là 6 chứng từ bắt buộc",
                    Message = "Ít nhất phải là 6 chứng từ bắt buộc",
                    Success = false
                });
            }
            var listRequireDocCode = new List<string>()
            {

                "DN","IDFRONT","IDBACK","HK","HINH","TTTB"

            };


            var multiForm = new MultipartFormDataContent();
            multiForm.Add(new StringContent("EXT_SBK"), "usersname");
            multiForm.Add(new StringContent("mafc123!"), "password");
            multiForm.Add(new StringContent(model.AppId), "appid");
            multiForm.Add(new StringContent("EXT_SBK"), "salecode");
            multiForm.Add(new StringContent(""), "warning");
            multiForm.Add(new StringContent(""), "warning_msg");

            foreach (var item in allTaiLieu)
            {
                if(listRequireDocCode.Contains(item.Keycode))
                {
                    listRequireDocCode.Remove(item.Keycode);
                }
                string filePath = Server.MapPath(item.FileUrl);
                multiForm.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(filePath)),item.Keycode , System.IO.Path.GetFileName(filePath));
            }

            if(listRequireDocCode.Count !=0)
            {
                return ToJsonResponse(false, "Chưa nhập đủ yêu cầu chứng từ bắt buộc", new PushToUNDReponse()
                {
                    Data = "Chưa nhập đủ yêu cầu chứng từ bắt buộc",
                    Message = "Chưa nhập đủ yêu cầu chứng từ bắt buộc",
                    Success = false
                });

            }

             var result =  await _odcService.PushToUND(multiForm);

            if (result.Success)
            {
                await _rpMCredit.UpdateStatusMAFC(id,4, int.Parse(model.AppId), GlobalData.User.IDUser);
            }

            return ToJsonResponse(result.Success,result.Data,result);

        }
        public async Task<ActionResult> Temp()
        {

          await _odcService.CheckAuthen();


            return View();
        }

        private string ValidateProfile<T>(T profile) where  T :MiraeModel

        {
                string message = "";
                if (string.IsNullOrEmpty(profile.Nationalid) || profile.Nationalid.Length < 9 || profile.Nationalid.Length > 12)

                {
                    message = "Số chứng mình nhân dân không hợp lệ hoặc chưa nhập";

                }

                else if (string.IsNullOrEmpty(profile.Idissuer))
                {
                    message = "Chưa cho biết thông tin địa chỉ nơi cấp CMND";
                }
                else if (profile.Dob == DateTime.MinValue)
                {

                    message = "Chưa có thông tin ngày sinh";
                }
                else if (!profile.Nationalidissuedate.HasValue)
                {
                    message = "Ngày cấp chứng minh nhân dân chưa nhập";

                }
                else if (string.IsNullOrEmpty(profile.Familybooknumber))
                {
                    message = "Sổ hộ khẩu chưa nhập";

                }

                else if (string.IsNullOrEmpty(profile.Schemeid))
                {
                    message = "Mã sản phẩm chưa nhập";
                }
                else if (string.IsNullOrEmpty(profile.Title))
                {
                    message = "Xưng hô  khách hàng chưa nhập";

                }
                else if (string.IsNullOrEmpty(profile.Salesofficer) || profile.Salesofficer == "0")
                {
                    message = "Mã số kinh doanh chưa nhập";

                }
                else if (string.IsNullOrEmpty(profile.Maritalstatus))
                {
                    message = "Tình trạng hôn nhân chưa nhập";
                }
                else if( string.IsNullOrEmpty(profile.Eduqualify))
                {
                message = "Bạn chưa nhập trình độ học vẫn";

                 }
                else if(string.IsNullOrEmpty(profile.Dueday))
                {
                message = "Bạn chưa điền thông tin ngày thanh toán hàng tháng";
                }

                else if (string.IsNullOrEmpty(profile.Loanpurpose))
                {
                    message = "Mục đích vay chưa nhập";
                }

                else if (profile.Tenure < 1)
                {
                    message = "Thời gian  vay chưa nhập";
                }
                else if(profile.Schemeid =="0" || string.IsNullOrEmpty(profile.Schemeid))
                {
                     message = "Bạn chưa chọn sản phẩm vay";
                }
                else if(string.IsNullOrEmpty(profile.Amount))
                {
                message = "Bạn chưa điền thông tin thu nhập";
                 }
                else if(string.IsNullOrEmpty(profile.Accountbank))
                {
                message = "Bạn chưa nhập thông tin hình thức thu nhập";
            }
                else if (!profile.Totalloanamountreq.HasValue)
                {
                    message = "Chưa nhập số tiền vay";
                }

                else if (string.IsNullOrEmpty(profile.Mobile))
                {
                    message = "Số điện thoại chưa nhập";
                }
                else if (profile.AddressCur_City < 1 || profile.AddressCur_Ward < 1 || profile.AddressCur_District < 1 || string.IsNullOrEmpty(profile.AddressCur_address1stline))
                {
                    message = "Nhập đủ thông tin địa chỉ hiện tại";
                }
                else if (profile.CityCompany < 1 || profile.WardCompany < 1 || profile.DistrictCompany < 1 || string.IsNullOrEmpty(profile.Addressline)
                || string.IsNullOrEmpty(profile.Position))
                
                {
                    message = "Nhập đủ thông tin nghề nghiệp ( Địa chỉ, thời gian làm việc, chức vụ";
                }
                else if(string.IsNullOrEmpty(profile.Others))
                {
                     message = "Chưa nhập tên công ty";
                }
              
                else if(string.IsNullOrEmpty(profile.Refferee1_Refereename) ||  string.IsNullOrEmpty(profile.Refferee1_in_title)

                || string.IsNullOrEmpty(profile.Refferee1_Phone1) || string.IsNullOrEmpty(profile.Refferee1_Refereerelation))
                 {
                    message = "Nhập đủ thông tin tham chiếu thứ nhất";
                }

            else if (!string.IsNullOrEmpty(profile.Refferee2_Refereename))
            {
               
                if( string.IsNullOrEmpty(profile.Refferee2_in_title)

            || string.IsNullOrEmpty(profile.Refferee2_Phone1) || string.IsNullOrEmpty(profile.Refferee2_Refereerelation))
                message = "Nhập đủ thông tin tham chiếu thứ hai";
            }

            else if (!string.IsNullOrEmpty(profile.Refferee3_Refereename))
            {

                if (string.IsNullOrEmpty(profile.Refferee3_in_title)

            || string.IsNullOrEmpty(profile.Refferee3_Phone1) || string.IsNullOrEmpty(profile.Refferee3_Refereerelation))
                    message = "Nhập đủ thông tin tham chiếu thứ ba";
            }

             else if(!profile.IsDuplicateAdrees.Value)
            {

                 if (profile.AddressPer_City < 1 || profile.AddressPer_Ward < 1 || profile.AddressPer_District < 1 || string.IsNullOrEmpty(profile.AddressPer_address1stline))
                {
                    message = "Nhập đủ thông tin địa chỉ thường trú";
                }

            }


            var itemProduct = MiraeService.Allproduct.Where(x => x.Schemeid == int.Parse(profile.Schemeid)).FirstOrDefault();

            if (itemProduct != null)
            {

                if (itemProduct.Mintenure > profile.Tenure)
                {
                    message = "Thời hạn vay không hợp lệ, Thời hạn vay tối thiêu là " + itemProduct.Mintenure;
                }
                else if (itemProduct.Maxtenure < profile.Tenure)
                {
                    message = "Thời hạn vay không hợp lệ, Thời hạn vay tối thiêu là " + itemProduct.Maxtenure;
                }

                else if (itemProduct.Minamtfin > (double) profile.Totalloanamountreq.Value)
                {
                    message = "Số tiền vay ở dưới mức tối thiểu, số tiền vay tối thiểu là " + itemProduct.Minamtfin;
                }

                else if (itemProduct.Maxamtfin < (double)profile.Totalloanamountreq.Value)
                {
                    message = "Số tiền vay trên mức tối đa, số tiền vay tối đa là " + itemProduct.Minamtfin;
                }


            }

            return message;
            }
        public async Task<JsonResult> PushToMafc(int id)
        {
            var profile = await _rpMCredit.GetDetail(id);
            var checkMessage = ValidateProfile(profile);
            if(string.IsNullOrWhiteSpace(checkMessage))
            {
                
            }

            else
            {
                return ToJsonResponse(false, checkMessage);
            }




            if (!string.IsNullOrWhiteSpace(profile.AppId))
            {


                return ToJsonResponse(false, "Hồ sơ đã đẩy rồi, vui lòng thực hiện thao tác khác");


            }
            else {
                var model = await _rpMCredit.GetTemProfileByMcId(id);
                var request = MiramodelToQDERequest(model);
                var ressultQDE = await _odcService.QDESubmit(request);

              
                if (ressultQDE.Success)
                {
                   


                    var appId = int.Parse(ressultQDE.Data);
                    await _rpMCredit.SetAppidProfile(model.Id, appId);
                    var response = await _odcService.QDEToDDE(new QDEToDDEReQuest()
                    {
                        p_appid = int.Parse(ressultQDE.Data)

                    });

              
                    var bankName = model.Bankname;
                    try
                    {
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        if (bankName.Length < 8)
                        {
                            bankName = "0" + bankName;
                        }
                    }
                    catch (Exception)
                    {


                    }
                    if (response.Success)
                    {

                       
                        await _rpMCredit.UpdateStatusMAFC(id, 1,appId, GlobalData.User.IDUser);
                        //DDE

                        var requestDDE = new MiraeDDELeadReQuest()
                        {
                            in_appid = appId,
                            in_maritalstatus = model.Maritalstatus,
                            in_qualifyingyear = model.Qualifyingyear,
                            in_eduqualify = model.Eduqualify,
                            in_noofdependentin = model.Noofdependentin,
                            in_paymentchannel = model.Paymentchannel,
                            in_nationalidissuedate = model.Nationalidissuedate.Value.ToShortDateString(),
                            in_familybooknumber = model.Familybooknumber,
                            in_idissuer = model.Idissuer,
                            
                            in_spousename = model.Spousename,
                            in_spouse_id_c = model.Spouse_id_c,
                            in_categoryid = model.Categoryid,
                            in_bankname = bankName,
                            in_bankbranch = model.Bankbranch,
                            in_acctype = model.Acctype,
                            in_accno = model.Accno,
                            in_dueday = model.Dueday,
                            in_notecode = model.Notecode,
                           
                            in_notedetails = model.Notedetails
                        };
                        requestDDE.in_qualifyingyear = "0";
                        requestDDE.in_notecode = "DE_MOBILE";
                        requestDDE.in_channel = "SBK";
                        requestDDE.in_userid = "EXT_SBK";
                        
                        requestDDE.in_categoryid = "SBK";
                        var resultDDE = await _odcService.DDESubmit(requestDDE);

                        if (resultDDE.Success)
                        {

                            await _rpMCredit.UpdateStatusMAFC(id, 2,appId, GlobalData.User.IDUser);
                            var responseToPOR = await _odcService.DDEToPoR(new DDEToPORReQuest()
                            {
                                in_channel = "SBK",
                                in_userid = "EXT_SBK",
                                msgName = "procDDEChangeState",
                                p_appid = appId

                            });

                            if (responseToPOR.Success)
                            {

                                await _rpMCredit.UpdateStatusMAFC(id, 3, appId, GlobalData.User.IDUser);
                            }
                            else
                            {

                                return ToJsonResponse(false, "DDE to POr thất bại", response);
                            }
                          
                        }
                        else
                        {

                            return ToJsonResponse(false, "Tạo DDE thất bại", response);
                        }
                    }
                    else

                    {

                        return ToJsonResponse(false, "chuyển trạng thái QDE to DDE thất bại", response);
                    }
                }

                else

                {
                    return ToJsonResponse(false,"Đẩy QDE thất bại", ressultQDE);
                }
            }

            return ToJsonResponse(true, "Đẩy hồ sơ qua thành công, Vui lòng đẩy file để hoàn thành");

        }

       

        public async Task<ActionResult> Index()
        {

            await _odcService.CheckAuthen();


            return View("Temp");
        }
        public async Task<ActionResult> AddNew()

        {
            await _odcService.CheckAuthen();
            return View();
        }
        public async Task<JsonResult> GetAllStatus()
        {
            var result = await _rpMCredit.GetAllStatus();
            return ToJsonResponse(true, "", data: result);
        }
        public async Task<JsonResult> GetLoanProduct(int MaDoiTac)
        {
            var result = await _rpMCredit.GetLoanProduct(MaDoiTac);
            return ToJsonResponse(true, "", data: result);
        }
        public async Task<JsonResult> Comments(int profileId)
        {
            var result = await _rpMCredit.GetCommentsAsync(profileId);
            return ToJsonResponse(true, null, result);
        }
        public async Task<JsonResult> AddNote(int profileId, StringModel model)
        {
            if (model == null)
                return ToJsonResponse(false, "Dữ liệu không hợp lệ");
            var result = await _rpMCredit.AddNoteAsync(profileId, model.Value, GlobalData.User.IDUser);
            return ToJsonResponse(result.IsSuccess, result.Message);
        }
        public async Task<JsonResult> UploadToHoso(int hosoId, bool isReset, List<FileUploadModelGroupByKey> filesGroup)
        {
            if (hosoId <= 0 || filesGroup == null)
                return ToJsonResponse(false);

            if (isReset)
            {
                var deleteAll = await _rpTailieu.RemoveAllTailieuMirae(hosoId, (int)HosoType.Ocb);
                if (!deleteAll)
                    return ToJsonResponse(false);
            }
            foreach (var item in filesGroup)
            {
                if (item.files.Any())
                {
                    foreach (var file in item.files)
                    {
                        var tailieu = new TaiLieu
                        {
                            FileName = file.FileName,
                            FilePath = file.FileUrl,
                            ProfileId = hosoId,
                            FileKey = Convert.ToInt32(file.Key),
                            ProfileTypeId = (int)HosoType.Ocb,
                            Folder = file.FileUrl
                        };
                        await _rpTailieu.AddMirae(tailieu);
                    }
                }
            }
            return ToJsonResponse(true);
        }
        public async Task<JsonResult> UploadFile(int key, int fileId, int type)
        {
            type = 7;
            string fileUrl = "";
            var _type = string.Empty;
            string deleteURL = string.Empty;
            var file = new FileModel();
            try
            {
                foreach (string f in Request.Files)
                {
                    var fileContent = Request.Files[f];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        Stream stream = fileContent.InputStream;
                        string root = Server.MapPath($"~{Utility.FileUtils._profile_parent_folder}");
                        stream.Position = 0;
                        file = _bizMedia.GetFileUploadUrl(fileContent.FileName, root, Utility.FileUtils.GenerateOcbProfile());
                        using (var fileStream = System.IO.File.Create(file.FullPath))
                        {
                            await stream.CopyToAsync(fileStream);
                            fileStream.Close();
                            fileUrl = file.FileUrl;
                        }
                        deleteURL = fileId <= 0 ? $"/Mirae/delete?key={key}" : $"/Mirae/delete/0/{fileId}";
                        if (fileId > 0)
                        {

                            await _rpTailieu.UpdateExistingFileMirae(new TaiLieu
                            {
                                FileName = file.Name,
                                Folder = file.Folder,
                                FilePath = file.FileUrl,
                                ProfileId = 0,
                                ProfileTypeId = type
                            }, fileId);
                        }
                        _type = System.IO.Path.GetExtension(fileContent.FileName);
                    }

                }
                if (_type.IndexOf("pdf") > 0)
                {
                    var config = new
                    {
                        initialPreview = fileUrl,
                        initialPreviewConfig = new[] {
                                            new {
                                                caption = file.Name,
                                                url = deleteURL,
                                                key =key,
                                                type="pdf",
                                                width ="120px"
                                                }
                                        },
                        append = false
                    };
                    return Json(config);
                }
                else
                {
                    var config = new
                    {
                        initialPreview = fileUrl,
                        initialPreviewConfig = new[] {
                                            new {
                                                caption = file.Name,
                                                url = deleteURL,
                                                key =key,
                                                width ="120px"
                                            }
                                        },
                        append = false
                    };
                    return Json(config);
                }
                //return Json(result);
            }
            catch (Exception)
            {
                Session["LstFileHoSo"] = null;
            }
            return Json(new { Result = fileUrl });
        }
        public async Task<JsonResult> RemoveTailieuByHoso(int hosoId, int fileId)
        {

            if (fileId <= 0)
            {
                return ToJsonResponse(false, null, "Dữ liệu không hợp lệ");
            }
            var result = await _rpTailieu.RemoveTailieuMirae(hosoId, fileId);
            return ToJsonResponse(true);
        }
        public string ConvertToBase64(Stream stream)
        {
            var bytes = new Byte[(int)stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, (int)stream.Length);

            return Convert.ToBase64String(bytes);
        }
        public JsonResult Delete(int key)
        {
            string fileUrl = "";

            return Json(new { Result = fileUrl });
        }
        public async Task<JsonResult> TailieuByHosoForEdit(int hosoId, int typeId = 7)
        {
            if (hosoId <= 0)
            {
                return ToJsonResponse(false, null, "Dữ liệu không hợp lệ");
            }
            var lstLoaiTailieu = await _rpTailieu.GetLoaiTailieuList(7);
            if (lstLoaiTailieu == null || !lstLoaiTailieu.Any())
                return ToJsonResponse(false);

            typeId = 7;

            var filesExist = await _rpTailieu.GetTailieuMiraeHosoId(hosoId, typeId);

            var result = new List<HosoTailieu>();

            foreach (var loai in lstLoaiTailieu)
            {
                var tailieus = filesExist.Where(p => p.Key == loai.ID);

                var item = new HosoTailieu
                {
                    ID = loai.ID,
                    Ten = loai.Ten,
                    BatBuoc = loai.BatBuoc,
                    Tailieus = tailieus != null ? tailieus.ToList() : new List<FileUploadModel>()
                };
                result.Add(item);

            }
            return ToJsonResponse(true, null, result);
        }
        public async Task<JsonResult> TailieuByHoso(int hosoId, int type = 1)
        {
            var result = await _rpTailieu.GetTailieuMiraeHosoId(hosoId, type);
            if (result == null)
                result = new List<FileUploadModel>();
            return ToJsonResponse(true, null, result);
        }




    }
}