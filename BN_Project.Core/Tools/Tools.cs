using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Core.Tools
{
    public static class Tools
    {
        public static string ToTrimAndLower(this string value)
        {
            return value.ToLower().Trim();
        }

        public static string GenerateUniqCode()
        {
            return Guid.NewGuid().ToString().Replace("-","");
        }

        public static string ConvertToShamsi(this DateTime value)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            string date = persianCalendar.GetYear(value) + "/" + persianCalendar.GetMonth(value) + "/" + persianCalendar.GetDayOfMonth(value);

            return date;
        }

        public static string EncodePasswordMd5(this string pass) //Encrypt using MD5   
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)   
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(pass);
            encodedBytes = md5.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string   
            return BitConverter.ToString(encodedBytes);
        }
    }
}
