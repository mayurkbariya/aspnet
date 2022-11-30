using FBDropshipper.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace FBDropshipper.Infrastructure.Service
{
    public class ImageService : IImageService
    {
        private readonly ILogger<ImageService> _logger;
        private readonly IS3AmazonService _amazonService;
        private readonly HttpClient _httpClient;
        public ImageService(ILogger<ImageService> logger, IS3AmazonService amazonService, HttpClient httpClient)
        {
            _logger = logger;
            _amazonService = amazonService;
            _httpClient = httpClient;
        }

        public byte[] GetFile(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return new byte[0];
            }
            if (File.Exists("wwwroot" + url))
            {
                return File.ReadAllBytes("wwwroot" + url);
            }
            return new byte[0];
        }

        public async Task<string> DownloadAndSave(string url, string targetFolder = "img")
        {
            var bytes = await _httpClient.GetByteArrayAsync(url);
            var base64 = "image/jpeg;base64," + Convert.ToBase64String(bytes);
            return await _amazonService.SaveImage(base64);
        }

        public async Task<string> SaveImage(string base64, string targetFolder = "img", string extension = ".jpg")
        {
            return await _amazonService.SaveImage(base64);
            byte[] image = LoadImage(base64);
            string fileName = Guid.NewGuid() + extension;
            var seprator = Path.DirectorySeparatorChar;
            string directoryPath = Path.Combine(
                Directory.GetCurrentDirectory(), $"wwwroot{seprator}assets{seprator}{targetFolder}");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            var path = Path.Combine(directoryPath, fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                stream.Write(image, 0, image.Length);
                return $"/assets/{targetFolder}/" + fileName;
            }
        }

        public (string, string) SaveImageWithResize(string base64, string extension = ".jpg", string targetFolder = "img")
        {
            throw new NotImplementedException();
        }

        public async Task DeleteImage(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }

            await _amazonService.DeleteImage(url);
            // if (File.Exists("wwwroot" + url))
            // {
            //     File.Delete("wwwroot" + url);
            // }
        }

        byte[] LoadImage(string baseString)
        {
            baseString = baseString.Remove(0, baseString.IndexOf(',') + 1);
            return Convert.FromBase64String(baseString);
        }
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="sourcePath">The Path of image to resize.</param>
        /// <param name="pathToSave">The Path of image to save to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public string ResizeImage(string sourcePath,string pathToSave, int height)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="pathToSave">The Path of image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>Saved Path of Image</returns>
        /// <exception cref="Exception">Image doesnt exist or issue in stream</exception>
        public string ResizeImage(Stream stream, string pathToSave, int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}