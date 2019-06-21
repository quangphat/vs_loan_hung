using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VS_LOAN.Core.Utility
{
    public class RuleFunctionUtility
    {
        /// <summary>
        /// Kiểm tra quyền truy cập chức năng
        /// </summary>
        /// <param name="maQuyen">Chuỗi quyền hex</param>
        /// <param name="maChucNang">Quyền index</param>
        /// <returns></returns>
        public static bool CheckRule(string maQuyen, int maChucNang)
        {
            try
            {
                if (maChucNang == 0) return true;
                maQuyen = maQuyen.Trim();
                int kt = (maQuyen.Length - (maChucNang - 1) / 4) - 1;
                if (kt >= 0 && kt < maQuyen.Length)
                {
                    string ktMaQuyen = maQuyen[kt].ToString();
                    string gt = Convert.ToString(Convert.ToInt32(ktMaQuyen.ToString(), 16), 2).PadLeft(4, '0');

                    if (gt[gt.Length - (maChucNang - 1) % 4 - 1] == '1')
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Cập nhật chuỗi quyền Hex khi thêm vào quyền index mới
        /// </summary>
        /// <param name="maQuyen">Chuỗi quyền hex</param>
        /// <param name="index">Quyền index thêm</param>
        /// <returns>Chuỗi quyền hex</returns>
        public static string AddRule(string maQuyen, int index)
        {
            int block = (index - 1) / 4;
            int offset = (index - 1) % 4;
            if (maQuyen.Length <= block)
            {
                maQuyen = maQuyen.PadLeft(block + 1, '0');
            }
            int kt = maQuyen.Length - block - 1;
            if (kt >= 0 && kt < maQuyen.Length)
            {
                string ktMaQuyen = maQuyen[kt].ToString();
                int gt = Convert.ToInt32(ktMaQuyen, 16);
                gt = gt | (1 << offset);
                string gt1 = Convert.ToString(gt, 16);
                if (kt > 0)
                {
                    return maQuyen.Substring(0, kt) + gt1 + maQuyen.Substring(kt + 1, maQuyen.Length - kt - 1);
                }
                else
                {
                    return gt1 + maQuyen.Substring(kt + 1, maQuyen.Length - kt - 1);
                }

            }
            return "0";
        }

        /// <summary>
        /// Or hai chuỗi quyền hex
        /// </summary>
        /// <param name="quyen1">Chuỗi quyền hex 1</param>
        /// <param name="quyen2">Chuỗi quyền hex 2</param>
        /// <returns>Chuỗi quyền hex</returns>
        public static string OR(string quyen1, string quyen2)
        {
            string strResult = "";
            quyen1 = quyen1.Trim();
            quyen2 = quyen2.Trim();
            if (quyen2.Length < quyen1.Length)
            {
                quyen2 = quyen2.PadLeft(quyen1.Length, '0');
            }
            else
            {
                quyen1 = quyen1.PadLeft(quyen2.Length, '0');
            }
            for (int i = 0; i < quyen1.Length; i++)
            {
                int gt1 = Convert.ToInt32(quyen1[i].ToString(), 16);
                int gt2 = Convert.ToInt32(quyen2[i].ToString(), 16);
                int result = gt1 | gt2;
                strResult += result.ToString("X");
            }
            //strResult = strResult.TrimStart('0');
            return strResult;
        }

        /// <summary>
        /// And hai chuỗi quyền hex
        /// </summary>
        /// <param name="quyen1">Chuỗi quyền hex 1</param>
        /// <param name="quyen2">Chuỗi quyền hex 1</param>
        /// <returns>Chuỗi quyền hex</returns>
        public static string AND(string quyen1, string quyen2)
        {
            string strResult = "";
            quyen1 = quyen1.Trim();
            quyen2 = quyen2.Trim();
            if (quyen2.Length < quyen1.Length)
            {
                quyen2 = quyen2.PadLeft(quyen1.Length, '0');
            }
            else
            {
                quyen1 = quyen1.PadLeft(quyen2.Length, '0');
            }
            for (int i = 0; i < quyen1.Length; i++)
            {
                int gt1 = Convert.ToInt32(quyen1[i].ToString(), 16);
                int gt2 = Convert.ToInt32(quyen2[i].ToString(), 16);
                int result = gt1 & gt2;
                strResult += result.ToString("X");
            }
            return strResult;
        }
    }
}
