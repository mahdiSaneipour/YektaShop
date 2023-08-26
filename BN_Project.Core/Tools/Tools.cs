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

        public static long PercentagePrice(long price, decimal discount)
        {
            long result = (long) (price - ((price / 100) * discount));
            return result;
        }

        public static long DiscountPrice(long price, decimal discount)
        {
            long result = (long) ((price / 100) * discount);
            return result;
        }

        public static decimal HowManyPercent(long price, long finalPrice)
        {
            decimal result = price - finalPrice;
            result = (result / price) * 100;
            return Math.Round(result, 1);
        }

        public static long GetMainPriceFromDiscount(long finalPrice, decimal discount)
        {
            long result = (long)((finalPrice * 100) / (100 - discount));

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

        public static decimal CalculateAvrage(this List<int> values)
        {
            decimal count = values.Count;
            decimal total = values.Sum();
            decimal result = total / count;

            return Math.Round(result, 1);
        }
        public static decimal CalculateAvragePercent(this decimal value)
        {
            decimal result = (value * 100) / 5;
            return Math.Round(result, 1);
        }
    }
}
