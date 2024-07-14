using SixLabors.ImageSharp.Formats.Webp;
using Swallow.Exceptions.CustomExceptions;
using Swallow.Models;

namespace Swallow.Utils.FileManagers
{
    public interface IUserFileManager
    {
        FileStream GetProfilePicture(Guid photoId);
        void DeleteProfilePicture(User user);
        Task<Guid> SaveProfilePicture(User user, IFormFile file);
    }

    public class UserFileManager(IConfiguration configuration) : IUserFileManager
    {
        private const int MAX_PROFILE_PICTURE_SIZE = 5 * 1024 * 1024; // 5 MB
        private readonly string _profilePicturePath = configuration["DataDirectory"]! + "profile-pictures/";
        private readonly string _attachmentPath = configuration["DataDirectory"]! + "attachments/";
        private readonly WebpEncoder _webpEncoder = new() { Quality = 75 };

        public FileStream GetProfilePicture(Guid photoId)
        {
            var filePath = _profilePicturePath + photoId + ".webp";

            if (!File.Exists(filePath))
            {
                throw new NotFoundException("Profile picture not found.");
            }

            return new FileStream(filePath, FileMode.Open);
        }

        public async Task<Guid> SaveProfilePicture(User user, IFormFile file)
        {
            VerifyProfilePicture(file);
            DeleteProfilePicture(user);

            var fileName = Guid.NewGuid();

            var outputStream = await ConvertToWebpAsync(file);
            var filePath = _profilePicturePath + fileName + ".webp";
            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await outputStream.CopyToAsync(fileStream);

            outputStream.Dispose();

            return fileName;
        }
        private static void VerifyProfilePicture(IFormFile file)
        {
            if (!file.ContentType.Contains("image")) throw new BadRequestException("Invalid file type.");
            if (file.Length > MAX_PROFILE_PICTURE_SIZE) throw new BadRequestException("File size is too large. Maximum file size is 5MB.");
        }
        private async Task<MemoryStream> ConvertToWebpAsync(IFormFile file)
        {
            using var inputStream = file.OpenReadStream();
            using var image = await Image.LoadAsync(inputStream);

            var outputStream = new MemoryStream();
            await image.SaveAsWebpAsync(outputStream, _webpEncoder);
            outputStream.Seek(0, SeekOrigin.Begin);

            return outputStream;
        }

        public void DeleteProfilePicture(User user)
        {
            var filePath = _profilePicturePath + user.ProfilePicturePath + ".webp";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
