using System.Security.Cryptography;

namespace AspNetCoreEcommerce
{
    public static class GlobalConstants
    {
        public const string customerRole = "Customer";
        public const string vendorRole = "Vendor";
        public const string httpContentType = "application/json";
        public const string uploadPath = "Upload";
        public const string vendorSubPath = "Vendor";
        public const string productSubPath = "Products";

        public static async Task<string> SaveImageAsync(IFormFile imageFile, string subPath)
        {
            if (imageFile is null || imageFile.Length == 0)
                return "";

            List<string> allowedExtensions = [".jpg", ".jpeg", ".png", ".gif", "webp"];
            var extension = Path.GetExtension(imageFile.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid Image Format");

            //if (imageFile.Length / 1024 * 1024 > 10)
            //    throw new ArgumentException("Image File cannot be greater than 10 mb");

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), GlobalConstants.uploadPath, subPath);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}_{Path.GetExtension(imageFile.FileName)}".Replace(" ", "");
            var filePath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return $"{subPath}/{fileName}";
        }

        public static string GetImagetUrl(HttpRequest request, string? imgUrl)
        {
            if (string.IsNullOrWhiteSpace(imgUrl))
                return "";
            return $"{request.Scheme}://{request.Host}/{GlobalConstants.uploadPath}/{imgUrl}";
        }

        public static void DeletePreviuosImageOnUpdate(string? imageFilePath)
        {
            if (string.IsNullOrWhiteSpace(imageFilePath))
                return;
            try
            {
                File.Delete(imageFilePath);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Invalid Argument");
            }
            catch (IOException)
            {
                throw new IOException("File in use");
            }
        }
        public static string GenerateVerificationCode()
        {
            byte[] randomByte = new byte[4];
            RandomNumberGenerator.Fill(randomByte);
            int code = BitConverter.ToInt32(randomByte, 0) % 1000000;
            return Math.Abs(code).ToString("D6");
        }

    }
}
