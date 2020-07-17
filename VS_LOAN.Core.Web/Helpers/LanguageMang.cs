using VS_LOAN.Core.Repository;
using VS_LOAN.Core.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace VS_LOAN.Core.Web.Helpers
{
    public class LanguageMang
    {
        private static List<LanguageModel> _lstNgonNgu = new List<LanguageModel>();
        public static List<LanguageModel> LstNgonNgu
        {
            get
            {
                return _lstNgonNgu;
            }
            set
            {
                _lstNgonNgu = value;
            }
        }
        public static bool IsLanguageAvailable(string lang)
        {
            return false;
        }
        public static string GetDefaultLanguage()
        {
            return "vn";
        }
        public void SetLanguage(string lang)
        {
            try
            {
                if (!IsLanguageAvailable(lang)) lang = GetDefaultLanguage();
                var cultureInfo = new CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);
                HttpCookie langCookie = new HttpCookie("culture", lang);
                langCookie.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(langCookie);
                GlobalData.LagActive = lang;
            }
            catch (Exception ex) {
                throw ex;
            }
        }
    }
}