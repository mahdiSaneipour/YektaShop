using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Core.Tools.Image
{
    public static class UploadImage
    {
        public static string UploadFileImage(IFormFile file, string path, string? name = "")
        {
            string imageName = "";

            if(String.IsNullOrEmpty(name))
            {
                imageName = Tools.GenerateUniqCode() +
                        file.ContentType.Replace("image/", ".");
            } else
            {
                imageName = name;
            }

            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), path, imageName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return imageName;
        }

        public static void DeleteFile(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }
}