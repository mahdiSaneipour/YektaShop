using System.Globalization;
using System.Security.Cryptography;
using System.Text;

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
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static string ConvertToShamsi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            string date = pc.GetYear(value) + "/" +
                pc.GetMonth(value) + "/" +
                pc.GetDayOfMonth(value);

            return date;
        }

        public static DateTime ToMiladi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return new DateTime(value.Year, value.Month, value.Day, pc);
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

        public static void RemoveFile(this string Path)
        {
            if (Directory.Exists(Path))
            {
                Directory.Delete(Path);
            }
        }
    }
}
