using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DoAnChuyenNganh
{
    class Library
    {
        public string changeDate(string name)
        {
            string day = "";
            switch (name)
            {
                case "Monday":
                    day = "Thứ hai";
                    break;
                case "Tuesday":
                    day = "Thứ ba";
                    break;
                case "Wednesday":
                    day = "Thứ tư";
                    break;
                case "Thursday":
                    day = "Thứ năm";
                    break;
                case "Friday":
                    day = "Thứ sáu";
                    break;
                case "Saturday":
                    day = "Thứ bảy";
                    break;
                default:
                    day = "Chủ nhật";
                    break;
            }
            return day;
        }

        public string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
