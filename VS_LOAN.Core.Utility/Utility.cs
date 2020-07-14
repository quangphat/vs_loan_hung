using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Utility
{
    
    public static class FileUtils
    {
        public static string _profile_parent_folder = "/Upload/Profile";
        private static string _mc_profile_folder = "/MCredit";
        private static string _vietstar_profile_folder = "/VietStar";
        public static string GenerateProfileFolderForMc()
        {
            string folderByDate = DateTime.Now.ToString("yyyy/MM/dd").Replace('/','_');
            return $"{_mc_profile_folder}/{folderByDate}";
        }
        public static string GenerateProfileFolder()
        {
            string folderByDate = DateTime.Now.ToString("yyyy/MM/dd").Replace('/', '_');
            return $"{_mc_profile_folder}/{folderByDate}";
        }
    }
}
