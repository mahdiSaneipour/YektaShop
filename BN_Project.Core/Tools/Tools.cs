using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
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

        public static int PercentagePrice(int price, decimal discount)
        {
            int result = (int) (price - ((price / 100) * discount));
            return result;
        }

        public static int DiscountPrice(int price, decimal discount)
        {
            int result = (int) ((price / 100) * discount);
            return result;
        }

        public static decimal HowManyPercent(int price, int finalPrice)
        {
            decimal result = price - finalPrice;
            result = (result / price) * 100;
            return result;
        }

        public static int GetMainPriceFromDiscount(int finalPrice, decimal discount)
        {
            int result = (int)((finalPrice * 100) / (100 - discount));

            return result;
        }

        public static string ConvertToShamsi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            string date = pc.GetYear(value) + "/" +
                pc.GetMonth(value) + "/" +
                pc.GetDayOfMonth(value);

            return date;
        }

        public static DateTime GetStartOfMonth()
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(DateTime.Now);
            int month = pc.GetMonth(DateTime.Now);

            return new DateTime(year, month, 1, pc);
        }

        public static DateTime GetEndOfMonth()
        {
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(DateTime.Now);
            int month = pc.GetMonth(DateTime.Now) + 1;

            return new DateTime(year, month, 1, pc);
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

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }

        public static decimal CalculateAverage(this List<int> values)
        {
            decimal count = values.Count;
            decimal total = values.Sum();
            decimal result = total / count;

            return Math.Round(result, 1);
        }
        public static decimal CalculateAveragePercent(this decimal value)
        {
            decimal result = (value * 100) / 5;
            return Math.Round(result, 1);
        }
    }
}
